using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using ExcelLibrary.SpreadSheet;


namespace Neural
{
    public partial class FormDrawCorrectNeurons : Form
    {
        //Neural Net options
        private double[,] data = null;
        private double[][][][] tempWeights;
        int rowCountData = 0;
        int colCountData = 0;
        private int[] classes;
        List<int> classesList = new List<int>();
        private double validateError = 0.0;
        private int[] samplesPerClass;
        private String selectedType = "";
        private double testQuality = 0.0;
        

        //draw options
        private SolidBrush _myBrush = new SolidBrush(Color.DarkSeaGreen);
        private SolidBrush _offBrush = new SolidBrush(Color.Gray);
        private Pen _myPen = new Pen(Color.Black);

        //App options
        Network network = null;
        Thread Worker;
        Subnet subnet;


        public FormDrawCorrectNeurons()
        {
            InitializeComponent();
        }

        /**
         * получение всех доступных нейронов для связи(входы)
         **/
        private List<String> getAvailableWeights(Network net)
        {
            List<string> availableNeurons = new List<String>();
            //add inputs for available neurons, for connected subnet inputs
            for (int input = 0; input < net.InputsCount; input++)
            {
                availableNeurons.Add("-1:" + input.ToString());
            }

            //add neurons for available neurons, for connected subnet inputs
            for (int layer = 0; layer < net.Layers.Length - 1; layer++)
            {
                for (int neuron = 0; neuron < net.Layers[layer].Neurons.Length; neuron++)//-1 because last layer not to be connected inputs of subnet
                {
                    availableNeurons.Add(layer.ToString() + ":" + neuron.ToString());
                }
            }

            return availableNeurons;
        }

        /**
            * получение всех доступных нейронов для связи(выходы)
        **/
        private List<String> getAvailableOutputs(Network net)
        {
            List<string> availableNeurons = new List<String>();

            //add neurons for available neurons, for connected subnet outputs

            for (int neuron = 0; neuron < net.Layers[net.Layers.Length-1].Neurons.Length; neuron++)
            {
                availableNeurons.Add(net.Layers.Length - 1 + ":" + neuron.ToString());
            }
            

            return availableNeurons;
        }

        /**
         * Отрисовка сети
         * */
        public void draw(Network network, PictureBox pictureBox1, List<String> connectedNeurons, List<String> connectOutputs)
        {
            Bitmap bmp;
            Graphics formGraphics;

            // Create a bitmap the size of the form.
            //first layer does not contains in network.Layers, because +1
            
            int maxNeurons = network.InputsCount;
            for (int layer = 0; layer < network.Layers.Length; layer++)
            {
                if (maxNeurons < network.Layers[layer].Neurons.Length)
                {
                    maxNeurons = network.Layers[layer].Neurons.Length;
                }
            }
            //except out of panel
            if (200 * (network.Layers.Length + 1) > this.pictureBox1.Width)
                bmp = new Bitmap(200 * (network.Layers.Length+1), 50 * maxNeurons);
            else
                bmp = new Bitmap(this.pictureBox1.Width, 50 * maxNeurons);
            formGraphics = Graphics.FromImage(bmp);
            int x = 0;

            //draw input layer
            int cntInput = network.InputsCount;
            _myPen.Width = 1;
            System.Drawing.Point[] fisrtPoints = new System.Drawing.Point[cntInput];
            //default color
            _myPen.Color = Color.Black;

            for (int k = 0; k < cntInput; k++)
            {
                _myPen.Color = Color.Black;
                _myBrush.Color = Color.DarkSeaGreen;
                if (connectedNeurons.Contains<String>("-1:" + k.ToString()))
                {
                    _myPen.Color = Color.Gray;
                    _myBrush.Color = Color.DarkGreen;
                }
                Rectangle ellipse = new Rectangle(x, 50 * k, 50, 50);
                formGraphics.FillEllipse(this._myBrush, ellipse);
                formGraphics.DrawEllipse(this._myPen, ellipse);
                //draw string in ellipse
                formGraphics.DrawString("Input(" + k.ToString() + ")",
                                    new Font("Arial", 9, FontStyle.Regular),
                                    new SolidBrush(Color.Black),
                                    new System.Drawing.Point(x + 3, 50 * k + 15));
                fisrtPoints[k].X = x + 50;
                fisrtPoints[k].Y = 50 * k + (50 / 2);
            }


            System.Drawing.Point[] tempRightPoints = new System.Drawing.Point[1];
            //draw other layers
            for (int i = 0; i < network.Layers.Length; i++)
            {
                x = 200 * (i + 1);

                System.Drawing.Point[] hiddenLeftPoints = new System.Drawing.Point[network.Layers[i].Neurons.Length];
                System.Drawing.Point[] hiddenRightPoints = new System.Drawing.Point[network.Layers[i].Neurons.Length];

                for (int j = 0; j < network.Layers[i].Neurons.Length; j++)
                {
                    //default color
                    _myPen.Color = Color.Black;
                    _myBrush.Color = Color.DarkSeaGreen;

                    //inputs connected color
                    if (connectedNeurons.Contains<String>(i.ToString() + ":" + j.ToString()))
                    {
                        _myPen.Color = Color.Gray;
                        _myBrush.Color = Color.DarkGreen;
                    }

                    //outputs connected color
                    if (connectOutputs.Contains<String>(i.ToString() + ":" + j.ToString()))
                    {
                        _myPen.Color = Color.Gray;
                        _myBrush.Color = Color.YellowGreen;
                    }
                    Rectangle ellipse = new Rectangle(x, 50 * j, 50, 50);
                    formGraphics.FillEllipse(this._myBrush, ellipse);
                    formGraphics.DrawEllipse(this._myPen, ellipse);
                    //draw string in ellipse
                    formGraphics.DrawString("L(" + i.ToString() + ")N(" + j.ToString() + ")",
                                    new Font("Arial", 9, FontStyle.Regular),
                                    new SolidBrush(Color.Black),
                                    new System.Drawing.Point(x, 50 * j + 15));

                    hiddenLeftPoints[j].X = x;
                    hiddenLeftPoints[j].Y = 50 * j + (50 / 2);

                    hiddenRightPoints[j].X = x + 50;
                    hiddenRightPoints[j].Y = 50 * j + (50 / 2);

                    //if this first hidden layer
                    if (i == 0)
                    {
                        //all neurons n-1 layer
                        for (int b = 0; b < fisrtPoints.Length; b++)
                        {
                            //if weight current neuron == 0.0
                            if (network.Layers[i].Neurons[j].Weights[b] == 0.0)
                                _myPen.Color = Color.Red;
                            else
                                _myPen.Color = Color.Black;
                            formGraphics.DrawLine(_myPen, hiddenLeftPoints[j], fisrtPoints[b]);
                        }
                    }
                    else if (i != 0)
                    {
                        for (int c = 0; c < network.Layers[i - 1].Neurons.Length; c++)
                        {
                            //if weight current neuron == 0.0
                            if (network.Layers[i].Neurons[j].Weights[c] == 0.0)
                                _myPen.Color = Color.Red;
                            else
                                _myPen.Color = Color.Black;
                            formGraphics.DrawLine(_myPen, hiddenLeftPoints[j], tempRightPoints[c]);
                        }
                    }
                }
                //temp mass of right points of current layer for next cycle
                tempRightPoints = new System.Drawing.Point[network.Layers[i].Neurons.Length];
                tempRightPoints = hiddenRightPoints;
            }

            pictureBox1.Image = bmp;
        }

        /**
         * Запуск потока загрузки нейронной сети
         * */
        private void LoadNetToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Worker = new Thread(LoadNet);
            Worker.SetApartmentState(ApartmentState.STA);
            Worker.Start();
            
        }

        /**
         * Загрузка нейронной сети
         * */
        private void LoadNet()
        {
            // Initialize the OpenFileDialog to look for text files.
            openFileDialog1.Filter = "Bin Files|*.bin";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    network = Network.Load(openFileDialog1.FileName);

                }
                catch (IOException)
                {
                    throw new IOException("Ошибка загрузки нейронной сети");
                }
                finally
                {
                    this.Invoke(new Action(InitWork));
                    this.Invoke(new Action<Network, PictureBox, List<String>, List<String>>(draw), this.network, this.pictureBox1, new List<String>(), new List<String>());
                    Worker.Abort();
                }
            }
        }


        /**
         * Инициализация компонентов для работы
         * */
        private void InitWork()
        {

            tempWeights = new double[network.Layers.Length][][][];
            for (int i = 0; i < network.Layers.Length; i++)
            {
                tempWeights[i] = new double[network.Layers[i].Neurons.Length][][];
                for (int j = 0; j < network.Layers[i].Neurons.Length; j++)
                {
                    tempWeights[i][j] = new double[network.Layers[i].Neurons[j].Weights.Length][];
                    for (int k = 0; k < network.Layers[i].Neurons[j].Weights.Length; k++)
                    {
                        tempWeights[i][j][k] = new double[1];
                        tempWeights[i][j][k][0] = network.Layers[i].Neurons[j].Weights[k];
                    }
                }
            }
        }

        /**
         * Загрузка выборки
         * */
        private void loadTestData()
        {

            if (this.selectedType== "classification")
            {
                this.getDataForClass();
            }
            else if (this.selectedType== "regression")
            {
                this.getDataForRegression();
            }
        }

        /**
         * Вызов потока загрузки выборки
         * */
        private void LoadDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Worker = new Thread(loadTestData);
            Worker.SetApartmentState(ApartmentState.STA);
            Worker.Start();
            this.testNetButton.Enabled = true;
        }

        /** запуск тестирования сети
         * */
        private void testing()
        {
  
            if (this.selectedType == "classification")
            {
                double[] res;
                try
                {
                    double[] input = new double[colCountData - 1];
                    validateError = 0.0;
                    for (int count = 0; count < data.GetLength(0) - 1; count++)
                    {
                        //gather inputs for compute, n-1 inputs
                        for (int i = 0; i < colCountData - 1; i++)
                        {
                            input[i] = data[count, i];
                        }

                        if (this.subnet != null)
                            res = network.Compute(input, this.subnet);
                        else
                            res = network.Compute(input);
                       // res = network.Compute(input);
                        double value = Math.Abs(1 - res[classesList.IndexOf(classes[count])]);
                        if (value > 0.0001)
                        {
                            validateError += value;
                        }

                    }
                    this.testQuality = (1 - (validateError / data.GetLength(0))) * 100;
                    this.errorTextBox.Text = this.testQuality.ToString("F10");
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка тестирования сети.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            else if (this.selectedType == "regression")
            {
                double testError = 0.0;
                double[] input = new double[colCountData - 1];

                for (int i = 0; i < data.GetLength(0); i++)
                {
                    //gather inputs for compute, n-1 inputs
                    for (int j = 0; j < colCountData - 1; j++)
                    {
                        input[j] = data[i, j];
                    }
                    testError += Math.Abs(network.Compute(input)[0] - data[i, colCountData - 1]);
                }
                this.errorTextBox.Text = (testError / data.GetLength(0)).ToString("F10");


            }
 
        }

        /**
         * Процент ошибки при тестировании на выбранной выборке
         * */
        private void testNetButton_Click(object sender, EventArgs e)
        {
            this.testing();    
        }


        //load data for classification
        private void getDataForClass()
        {
            // show file selection dialog
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;
                int i = 0;
                try
                {
                    // open selected file
                    reader = File.OpenText(openFileDialog2.FileName);

                    //get row count values
                    String line;
                    rowCountData = 0;
                    colCountData = 0;

                    //get input and output count
                    line = reader.ReadLine();
                    rowCountData++;
                    colCountData = line.Trim().Split(' ').Length;

                    //mass for new normalization data
                    double[] minData = new double[colCountData];
                    double[] maxData = new double[colCountData];

                    //must be > 1 column in training data
                    if (colCountData == 1)
                        throw new Exception();

                    while ((line = reader.ReadLine()) != null)
                    {
                        rowCountData++;
                    }

                    double[,] tempData = new double[rowCountData, colCountData];
                    int[] tempClasses = new int[rowCountData];

                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    line = "";

                    samplesPerClass = new int[2000];

                    // read the data
                    classesList.Clear();
                    while ((i < rowCountData) && ((line = reader.ReadLine()) != null))
                    {
                        string[] strs = line.Trim().Split(' ');
                        // parse input and output values for learning
                        //gather all input by cols
                        for (int j = 0; j < colCountData - 1; j++)
                        {
                            tempData[i, j] = double.Parse(strs[j]);

                            //search min/max values for each columnt
                            if (tempData[i, j] < minData[j])
                                minData[j] = tempData[i, j];
                            if (tempData[i, j] > maxData[j])
                                maxData[j] = tempData[i, j];
                        }

                        if (strs.Length - 1 < colCountData - 1)
                            continue;
                        tempClasses[i] = int.Parse(strs[colCountData - 1]);

                        //insert class in list of classes, if not find
                        if (classesList.IndexOf(tempClasses[i]) == -1)
                        {
                            classesList.Add(tempClasses[i]);
                        }

                        samplesPerClass[tempClasses[i]]++;

                        i++;
                    }

                    for (int row = 0; row < rowCountData; row++)
                    {
                        for (int column = 0; column < colCountData; column++)
                        {
                            tempData[row, column] = (((tempData[row, column] - minData[column]) * 1 / (maxData[column] - minData[column])));

                        }
                    }

                    // allocate and set data
                    data = new double[i, colCountData];
                    Array.Copy(tempData, 0, data, 0, i * colCountData);
                    classes = new int[i];
                    Array.Copy(tempClasses, 0, classes, 0, i);

                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Ошибка на  " + i.ToString() + " строке", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    // close file
                    if (reader != null)
                        reader.Close();
                }

            }
        }

        //load data for regression
        private void getDataForRegression()
        {
            // show file selection dialog
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;

                try
                {
                    // open selected file
                    reader = File.OpenText(openFileDialog2.FileName);

                    //get row count values
                    String line;
                    rowCountData = 0;
                    colCountData = 0;

                    //get input and output count
                    line = reader.ReadLine();
                    rowCountData++;
                    colCountData = line.Split(';').Length;

                    //must be > 1 column in training data
                    if (colCountData == 1)
                        throw new Exception();

                    while ((line = reader.ReadLine()) != null)
                    {
                        rowCountData++;
                    }

                    double[,] tempData = new double[rowCountData, colCountData];

                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    line = "";
                    int i = 0;

                    // read the data
                    while ((i < rowCountData) && ((line = reader.ReadLine()) != null))
                    {
                        string[] strs = line.Split(';');
                        // parse input and output values for learning
                        //gather all values by cols
                        for (int j = 0; j < colCountData; j++)
                        {
                            tempData[i, j] = double.Parse(strs[j]);
                        }

                        i++;
                    }

                    // allocate and set data
                    data = new double[i, colCountData];
                    Array.Copy(tempData, 0, data, 0, i * colCountData);

                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка чтения файла", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    // close file
                    if (reader != null)
                        reader.Close();
                }

            }
            this.testNetButton.Invoke(new Action(() => testNetButton.Enabled = true));

        }

        /**
         * Кол-во нейронов основной сети
         * */
        private int getCountNeuronsOfNet()
        {
            int count = 0;
            for (int i = 0; i < network.Layers.Length; i++)
            {
                for (int j = 0; j < network.Layers[i].Neurons.Length; j++)
                {
                    count++;
                }
            }
            return count;
        }

        private void FormDrawCorrectNeurons_Load(object sender, EventArgs e)
        {
            HelloForm form = this.Owner as HelloForm;
            this.selectedType = form.typeLearn;
        }


        /**
         * генерация подсети
         * */
        private void subNetButton_Click(object sender, EventArgs e)
        {
            //create subnet  
            Random rnd = new Random();
            int input = rnd.Next(1, this.getCountNeuronsOfNet());
            int hidden = rnd.Next(1, 20);
            int output = rnd.Next(1, this.network.Layers[this.network.Layers.Length - 1].Neurons.Length);

            ActivationNetwork network = new ActivationNetwork(new SigmoidFunction(), input, hidden, output);
            subnet = new Subnet(network);
            
            //generate connections between common net and subnet
            List<String> availableNeurons = this.getAvailableNeurons(this.network), 
                    connectedInputNeurons = new List<String>(), 
                    inputSubNet = new List<String>();

            int rndLayer = 0;
            int rndNeuron = 0;

            for (int i = 0; i < input;)
            {
                rndLayer = rnd.Next(-1, this.network.Layers.Length-1);
                if (rndLayer == -1)
                    rndNeuron = rnd.Next(0, this.network.InputsCount);
                else
                    rndNeuron = rnd.Next(0, this.network.Layers[rndLayer].Neurons.Length);

                if (availableNeurons.Contains<String>( rndLayer.ToString() + ":" + rndNeuron.ToString() ))
                {
                    availableNeurons.Remove(rndLayer.ToString() + ":" + rndNeuron.ToString());
                    connectedInputNeurons.Add( rndLayer.ToString() + ":" + rndNeuron.ToString() );
                    inputSubNet.Add( "-1:" + i.ToString() );
                    i++;
                }

            }

            Hashtable inputRelations = new Hashtable();
            int neuron = 0;
            foreach (string connected in connectedInputNeurons)
            {
                inputRelations.Add(neuron, connected);
                neuron++;
            }
            subnet.setInputAssosiated(inputRelations);

            //connected last layer of subnet to last layer of common net
            List<String> availableOutputs = this.getAvailableOutputs(this.network), 
                outputSubnet = new List<String>(), 
                connectedOutputNeurons = new List<String>();

            rndLayer = this.network.Layers.Length - 1;
            for (int outputNeuron = 0; outputNeuron < output;)
            {

                rndNeuron = rnd.Next(0, this.network.Layers[rndLayer].Neurons.Length);

                if (availableOutputs.Contains<String>(rndLayer.ToString() + ":" + rndNeuron.ToString()))
                {
                    availableOutputs.Remove(rndLayer.ToString() + ":" + rndNeuron.ToString());
                    connectedOutputNeurons.Add(this.network.Layers.Length - 1 + ":" + outputNeuron.ToString());
                    outputSubnet.Add(subnet.Network.Layers.Length - 1 + ":" + outputNeuron.ToString());
                    outputNeuron++;
                }
            }

            Hashtable outputRelations = new Hashtable();
            neuron = 0;
            foreach (string connected in connectedOutputNeurons)
            {
                outputRelations.Add(neuron, connected);
                neuron++;
            }
            subnet.setOutputAssosiated(outputRelations);

            this.draw(subnet.Network, pictureBox2, inputSubNet, outputSubnet);
            this.draw(this.network, pictureBox1, connectedInputNeurons, connectedOutputNeurons);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String[] lines = new String[1];
            double[] res = new double[classesList.Count];
            double[] input = new double[colCountData - 1];

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\SpreadSheetTest.csv"))
                //file.WriteLine("Желаемый ответ; Результат");

                for (int i = 0; i < data.GetLength(0); i++)
                {
                    //gather inputs for compute, n-1 inputs
                    for (int k = 0; k < colCountData - 1; k++)
                    {
                        input[k] = data[i, k];
                    }

                    res = network.Compute(input, this.subnet);
                    if (this.selectedType == "classification")
                    {
                        int classificator = this.max(res);
                        lines[0] = classes[i].ToString() + ";" + classificator.ToString("F8");

                    }
                    else if (this.selectedType == "regression")
                        lines[0] = data[i, colCountData - 1].ToString() + ";" + res.ToString();

                    file.WriteLine(lines[0].ToString());
                }
            MessageBox.Show("Тестирование пройдено");
        }

        private int max(double[] mass)
        {
            int classificator = 0;
            double tempValue = 0.0;
            for (int i = 0; i < mass.Length; i++)
            {
                if (tempValue < mass[i])
                {
                    tempValue = mass[i];
                    classificator = i;
                }
            }
            return classificator;
        }

    }


}
