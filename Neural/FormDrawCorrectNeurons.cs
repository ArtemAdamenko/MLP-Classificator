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
using ZedGraph;


namespace Neural
{
    public partial class FormDrawCorrectNeurons : Form
    {
        //App options
        Network network = null;
        Thread Worker;
        Subnet globalSubnet;

        private List<Record> MinMaxValues;
        private List<Record> relationsValues = new List<Record>();
        private List<Record> tempRelationsValues = new List<Record>();

        #region Neural Net options
            private double[,] data = null;
            private int[] classes;
            List<int> classesList = new List<int>();
            private String selectedType = "";
            private bool needToStop = false;
        #endregion

        #region draw options
                private SolidBrush _myBrush = new SolidBrush(Color.DarkSeaGreen);
                private SolidBrush _offBrush = new SolidBrush(Color.Gray);
                private Pen _myPen = new Pen(Color.Black);
        #endregion

        public FormDrawCorrectNeurons()
        {
            InitializeComponent();

            RollingPointPairList listError = new RollingPointPairList(200);
            GraphPane myPane = zedGraphControl1.GraphPane;
            LineItem curve = myPane.AddCurve("Изменение качества", listError, Color.Blue, SymbolType.None);
            myPane.Title.Text = "Корректирование нейронной сети";
            myPane.XAxis.Title.Text = "Корректирующие подсети";
            myPane.YAxis.Title.Text = "Процент правильных ответов";
            myPane.XAxis.Scale.MajorStep = 10;

        }

        //получение всех доступных связей для подсоединения(входы)   
        private List<String> getAvailableWeights(Network net)
        {
            List<string> availableNeurons = new List<String>();

            //add neurons for available neurons, for connected subnet inputs
            for (int layer = 0; layer < net.Layers.Length; layer++)
            {
                for (int neuron = 0; neuron < net.Layers[layer].Neurons.Length; neuron++)
                {
                    for (int weight = 0; weight < net.Layers[layer].Neurons[neuron].Weights.Length; weight++)
                    {
                        availableNeurons.Add(layer.ToString() + ":" + neuron.ToString() + ":" + weight.ToString());
                    }
                }
            }

            return availableNeurons;
        }

        //Отрисовка сети     
        public void draw(Network network, PictureBox pictureBox1, List<String> connectedInputs, List<String> connectOutputs)
        {
            Bitmap bmp;
            Graphics formGraphics;
            
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
            System.Drawing.Point[] fisrtPoints = new System.Drawing.Point[cntInput];
            
            //default color, width
            _myPen.Color = Color.Black;
            _myPen.Width = 1;

            for (int k = 0; k < cntInput; k++)
            {
                _myPen.Color = Color.Black;
                _myBrush.Color = Color.DarkSeaGreen;
                _myPen.Width = 1;

                //inputs connected color
                if (connectedInputs.Contains<String>("-1:" + k.ToString()))
                {
                    _myBrush.Color = Color.Coral;
                    _myPen.Width = 3;
                    _myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
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
                    _myPen.Width = 1;
                    _myPen.Color = Color.Black;
                    _myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    _myBrush.Color = Color.DarkSeaGreen;

                    //outputs subnet connected color
                    if (connectOutputs.Contains<String>(i.ToString() + ":" + j.ToString()))
                    {
                        _myBrush.Color = Color.DarkCyan;
                        _myPen.Width = 3;
                        _myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
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
                        //all neurons 1 layer
                        for (int b = 0; b < fisrtPoints.Length; b++)
                        {
                            //if weight current neuron == 0.0
                            if (network.Layers[i].Neurons[j].Weights[b] == 0.0)
                                _myPen.Color = Color.Red;
                            else
                                _myPen.Color = Color.Black;

                            //if weight connect to subnet
                            _myPen.Width = 1;
                            _myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                            if (connectedInputs.Contains<String>(i.ToString() + ":" + j.ToString() + ":" + b.ToString()))
                            {
                                _myPen.Color = Color.Coral;
                                _myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                                _myPen.Width = 3;
                            }
                            if (connectOutputs.Contains<String>(i.ToString() + ":" + j.ToString() + ":" + b.ToString()))
                            {
                                _myPen.Color = Color.DarkCyan;
                                _myPen.Width = 3;
                                _myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                            }
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
                            //if weight connect to subnet
                            _myPen.Width = 1;
                            _myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                            if (connectedInputs.Contains<String>(i.ToString() + ":" + j.ToString() + ":" + c.ToString()))
                            {
                                _myPen.Color = Color.Coral;
                                _myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                                _myPen.Width = 3;
                            }
                            if (connectOutputs.Contains<String>(i.ToString() + ":" + j.ToString() + ":" + c.ToString()))
                            {
                                _myPen.Color = Color.DarkCyan;
                                _myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                                _myPen.Width = 3;

                            }
                            formGraphics.DrawLine(_myPen, hiddenLeftPoints[j], tempRightPoints[c]);
                        }
                    }
                }
                //temp mass of right points of current layer for next cycle
                tempRightPoints = new System.Drawing.Point[network.Layers[i].Neurons.Length];
                tempRightPoints = hiddenRightPoints;
            }

            //pictureBox1.Image = bmp;
            pictureBox1.Invoke(new Action(() => pictureBox1.Image = bmp));
        }

        //Запуск потока загрузки нейронной сети
        private void LoadNetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Worker = new Thread(LoadNet);
            Worker.SetApartmentState(ApartmentState.STA);
            Worker.Start();       
        }

        //Загрузка нейронной сети
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
                    this.Invoke(new Action<Network, PictureBox, List<String>, List<String>>(draw), this.network, this.pictureBox1, new List<String>(), new List<String>());
                    Worker.Abort();
                }
            }
        }

        //Вызов потока загрузки выборки
        private void LoadDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.selectedType == "classification")
            {
                Worker = new Thread(this.getDataForClass);
            }
            else if (this.selectedType == "regression")
            {
                Worker = new Thread(this.getDataForRegression);
            }
            Worker.SetApartmentState(ApartmentState.STA);
            Worker.Start();
            this.testNetButton.Enabled = true;
        }

        //минимальные и максимальные сигналы проходящие по каждой связи
        private void MinMaxRelationsValues(List<Record> relationsValues)
        {
            if (MinMaxValues == null)
            {
                MinMaxValues = new List<Record>(relationsValues);
                foreach (Record record in MinMaxValues)
                {
                    record.min = record.value;
                    record.max = record.value;
                }
            }
            else
            {
                for (int i = 0; i < relationsValues.Count; i++)
                {
                    if (relationsValues[i].value < MinMaxValues[i].min)
                    {
                        MinMaxValues[i].min = relationsValues[i].value;
                    }
                    else if (relationsValues[i].value > MinMaxValues[i].max)
                    {
                        MinMaxValues[i].max = relationsValues[i].value;
                    }
                }
            }

        }

        // запуск тестирования сети
        private double testing(Subnet thisSubnet = null)
        {
            double testQuality = 0.0;
            double validateError = 0.0;
            double[] input = new double[this.network.InputsCount];
            double[] res;

            if (this.selectedType == "classification")
            {
                for (int count = 0; count < data.GetLength(0); count++)
                {
                    try
                    {
                        //gather inputs for compute, n-1 inputs
                        for (int i = 0; i < this.network.InputsCount; i++)
                        {
                            input[i] = data[count, i];
                        }

                        if (thisSubnet != null)
                            res = this.network.Compute(input, thisSubnet, MinMaxValues);
                        else
                        {
                            res = this.network.Compute(input);
                            tempRelationsValues.AddRange(network.RelationsValues.ToArray());
                        }

                        this.MinMaxRelationsValues(tempRelationsValues);
                        tempRelationsValues.Clear();
                        //double value = Math.Abs(1 - res[classesList.IndexOf(classes[count])]);
                        double value = Math.Abs(classes[count] - this.max(res));

                        

                        validateError += value;

                        /*else
                        {
                            int dec = 1;
                            for (int output = 0; output < res.Length; output++)
                            {
                                if (res[output] == 1.0)
                                    dec++;
                            }
                            if (dec > 0)
                                validateError += 1 / dec;
                        }*/
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Ошибка тестирования сети." + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }

                }
                testQuality = (1-(validateError / data.GetLength(0))) * 100;
                this.errorTextBox.Invoke(new Action(() => this.errorTextBox.Text = testQuality.ToString("F6")));

            }

            else if (this.selectedType == "regression")
            {
                /*double testError = 0.0;

                for (int i = 0; i < data.GetLength(0); i++)
                {
                    //gather inputs for compute, n-1 inputs
                    for (int j = 0; j < colCountData - 1; j++)
                    {
                        input[j] = data[i, j];
                    }
                    testError += Math.Abs(network.Compute(input)[0] - data[i, colCountData - 1]);
                }
                this.errorTextBox.Text = (testError / data.GetLength(0)).ToString("F10");*/
            }
            return testQuality;
 
        }

        //Процент ошибки при тестировании на выбранной выборке
        private void testNetButton_Click(object sender, EventArgs e)
        {
            this.test();
           //this.errorTextBox.Text = this.testing().ToString("F6");    
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

                    
                    int rowCountData = 0;
                    String line = reader.ReadLine();
                    rowCountData++;
                    int colCountData = line.Trim().Split(' ').Length;

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

                        tempClasses[i] = int.Parse(strs[colCountData - 1]);

                        //insert class in list of classes, if not find
                        if (classesList.IndexOf(tempClasses[i]) == -1)
                        {
                            classesList.Add(tempClasses[i]);
                        }
                        i++;
                    }

                    tempData = normalization(tempData, minData.ToArray(), maxData.ToArray(), rowCountData, colCountData);
                  
                    // allocate and set data
                    data = new double[i, colCountData];
                    Array.Copy(tempData, 0, data, 0, i * colCountData);
                    classes = new int[i];
                    Array.Copy(tempClasses.ToArray(), 0, classes, 0, i);

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

        //normalization data for learning
        private double[,] normalization(double[,] data, double[] min, double[] max, int rowCountData, int colCountData)
        {
            for (int row = 0; row < rowCountData; row++)
            {
                for (int column = 0; column < colCountData; column++)
                {
                    data[row, column] = (((data[row, column] - min[column]) * 1 / (max[column] - min[column])));

                }
            }
            return data;
        }

        //load data for regression
        private void getDataForRegression()
        {
            // show file selection dialog
          /*  if (openFileDialog2.ShowDialog() == DialogResult.OK)
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
            this.testNetButton.Invoke(new Action(() => testNetButton.Enabled = true));*/

        }

        //Кол-во связей в сети
        private int getCountOfWeights(Network network)
        {
            int i = 0; 
            for (int layer = 0; layer < network.Layers.Length; layer++)
            {
                for (int neuron = 0; neuron < network.Layers[layer].Neurons.Length; neuron++)
                {
                    for (int weight = 0; weight < network.Layers[layer].Neurons[neuron].Weights.Length; weight++)
                    {
                        i++;
                    }
                }
            }
            return i;
        }

        private void FormDrawCorrectNeurons_Load(object sender, EventArgs e)
        {
            HelloForm form = this.Owner as HelloForm;
            this.selectedType = form.typeLearn;
        }

        //генерация подсетей
        private void GenerateSubNets()
        {
            double commonNetQuality = this.testing();
            zedGraphControl1.GraphPane.CurveList[0].Clear();
            //create subnet  
            Random rnd = new Random();
            int input = 10;
           // int hidden = rnd.Next(1, 20);
            int hidden = 20;
            int output = 20;

            ActivationNetwork network1 = new ActivationNetwork(new SigmoidFunction(2.0), input, hidden, output);
            Subnet subnet1 = new Subnet(network1);
            List<Subnet> subnets = new List<Subnet>();
            List<String> availableRelations = this.getAvailableWeights(this.network);
  
            //generate connections between common net and subnet
            List<String> availableRelationsTemp = new List<String>(availableRelations),
                    connectedInputNeurons = new List<String>(),
                    inputSubNet = new List<String>();

            for (int i = 0; i < input; )
            {

                int rndRelation = rnd.Next(0, availableRelationsTemp.Count);
                String connect = availableRelationsTemp[rndRelation];
                availableRelationsTemp.Remove(connect);
                connectedInputNeurons.Add(connect);
                inputSubNet.Add("-1:" + i.ToString());
                i++;

            }

            //connected last layer of subnet to last layer of common net
            List<String> outputSubnet = new List<String>(),
                connectedOutputNeurons = new List<String>();

            for (int outputNeuron = 0; outputNeuron < output; )
            {

                int rndRelation = rnd.Next(0, availableRelationsTemp.Count);
                String connect = availableRelationsTemp[rndRelation];
                availableRelationsTemp.Remove(connect);
                connectedOutputNeurons.Add(connect);
                outputSubnet.Add(subnet1.Network.Layers.Length-1 + ":" + outputNeuron.ToString());
                outputNeuron++;
            }

            this.draw(subnet1.Network, pictureBox2, inputSubNet, outputSubnet);
            this.draw(this.network, pictureBox1, connectedInputNeurons, connectedOutputNeurons);

            this.label2.Invoke(new Action(() => this.label2.Text = "Отбор..."));
            LogHelper.Write("Отбор...", "CorrectLog");

            double tempQuality = 0.0;
            double betterQuality = 0.0;
            while (!needToStop)
            {
                ActivationNetwork network = new ActivationNetwork(new BipolarSigmoidFunction(2.0), input, hidden, output);

                NguyenWidrow initializer = new NguyenWidrow(network);
                initializer.Randomize(0);

                Subnet subnet = new Subnet(network);
                subnet.inputAssosiated = connectedInputNeurons;
                subnet.outputAssosiated = connectedOutputNeurons;
                subnet.quality = this.testing(subnet);
                subnets.Add(subnet);

                LogHelper.Write(subnets.Count.ToString() + ";" + subnet.quality.ToString(), "CorrectLog");
                this.updateBarCHart(subnet.quality, subnets.Count - 1);

                //window in 100 elements
                if (subnets.Count == 100)
                {
                    LogHelper.Write("== 100", "CorrectLog");
                    tempQuality = this.evolution(subnets, connectedInputNeurons, connectedOutputNeurons);
                    this.test();
                    if (tempQuality > betterQuality)
                        betterQuality = tempQuality;
                    subnets.Clear();
                    zedGraphControl1.GraphPane.CurveList[0].Clear();
                    

                    //get one connection and set one random new connection for find optimization connecteds
                    for (int k = 0; k < 5; k++)
                    {
                        int oldOutput = rnd.Next(0, connectedOutputNeurons.Count);
                        String oldValue = connectedOutputNeurons[oldOutput];
                        connectedOutputNeurons.RemoveAt(oldOutput);

                        //new connection output
                        int newOutput = rnd.Next(0, availableRelationsTemp.Count);
                        String newValue = availableRelationsTemp[newOutput];
                        availableRelationsTemp.RemoveAt(newOutput);

                        connectedOutputNeurons.Add(newValue);
                        availableRelationsTemp.Add(oldValue);
                    }

                    this.draw(subnet1.Network, pictureBox2, inputSubNet, outputSubnet);
                    this.draw(this.network, pictureBox1, connectedInputNeurons, connectedOutputNeurons);
                }
                else
                    this.betterBox.Invoke(new Action(() => this.betterBox.Text = subnets.Count.ToString()));
            }

        }

        //запуск потока генерации подсети
        private void subNetButton_Click(object sender, EventArgs e)
        {
            needToStop = false;
            Worker = new Thread(GenerateSubNets);

            Worker.SetApartmentState(ApartmentState.STA);
            Worker.Start();

           
            
        }

        //удаление заданное кол-во сетей по возрастанию
        private List<Subnet> dropBadSubnets(List<Subnet> subnets, int count/*, double quality*/)
        {
            this.label2.Invoke(new Action(() => this.label2.Text = "Отбор лучших..."));
            LogHelper.Write("Отбор лучших...", "CorrectLog");

            zedGraphControl1.GraphPane.CurveList[0].Clear();

            //сортировка по убыванию
            subnets = subnets.OrderByDescending(net => net.quality).ToList();

            for (int i = 0; i < count; i++)
            {
                //удаление худших с конца
                subnets.RemoveAt(subnets.Count - 1);
                this.betterBox.Invoke(new Action(() => this.betterBox.Text = subnets.Count.ToString()));
            }

            return subnets;
        }

        //эволюция подсетей
        private double evolution(List<Subnet> subnets, List<String> connectedInputNeurons, List<String> connectedOutputNeurons)
        {
            List<Subnet> localSubNets = new List<Subnet>(subnets);
            List<Subnet> startSubnets = new List<Subnet>(subnets);
            Random rnd = new Random();
            double[] coefficients;
            double quality = 0.0;

            int population = 0;
            int cntOfPopulationsPerStep = 2;

            LogHelper.Write("Скрещивание...", "CorrectLog");
            while (localSubNets.Count > 0 && !needToStop)
            {
                coefficients = this.coefficientVariations(subnets);
                this.label2.Invoke(new Action(() => this.label2.Text = "Скрещивание..."));
                

                this.betterBox.Invoke(new Action(() => this.betterBox.Text = subnets.Count.ToString()));
                //get random pair of nets, delete after from list
                int net = rnd.Next(0, localSubNets.Count);
                Subnet tempSub1 = localSubNets[net];
                localSubNets.RemoveAt(net);

                net = rnd.Next(0, localSubNets.Count);
                Subnet tempSub2 = localSubNets[net];
                localSubNets.RemoveAt(net);

                //testing and adding
                Subnet sub = new Subnet(this.summarazing(tempSub1, tempSub2, coefficients));
                sub.inputAssosiated = connectedInputNeurons;
                sub.outputAssosiated = connectedOutputNeurons;
                sub.quality = this.testing(sub);

                subnets.Add(sub);
                LogHelper.Write(subnets.Count.ToString() + ";" + sub.quality.ToString(), "CorrectLog");
                this.updateBarCHart(sub.quality, subnets.Count);

                //скрещивания окончилось
                if (localSubNets.Count == 0)
                {
                    --cntOfPopulationsPerStep;
                    if (cntOfPopulationsPerStep == 0)
                    {
                        population++;

                        this.PopulationBox.Invoke(new Action(() => this.PopulationBox.Text = population.ToString()));
                        LogHelper.Write("Population " + population.ToString(), "CorrectLog");

                        //удаляем 100 самых худших
                        localSubNets = this.dropBadSubnets(subnets, 100);

                        //начинаем заново
                        subnets = new List<Subnet>(localSubNets);

                        double mediumErrorForPopulation = 0.0;

                        //отображаем лучших оставшихся на графике
                        for (int i = 0; i < subnets.Count; i++)
                        {
                            this.updateBarCHart(subnets[i].quality, i);
                            mediumErrorForPopulation += subnets[i].quality;
                        }

                        mediumErrorForPopulation = mediumErrorForPopulation / subnets.Count;

                        double summ = 0.0;
                        double coefficient = 0.0;

                        for (int i = 0; i < subnets.Count; i++)
                        {
                            summ += Math.Pow((subnets[i].quality - mediumErrorForPopulation), 2);
                        }
                        coefficient = summ / subnets.Count;
                        coefficient = coefficient / mediumErrorForPopulation;
                        coefficient = coefficient * 100;

                        this.mediumErrorPopulationBox.Invoke(new Action(() => this.mediumErrorPopulationBox.Text = mediumErrorForPopulation.ToString("F6")));
                        this.betterPopulationValueBox.Invoke(new Action(() => this.betterPopulationValueBox.Text = subnets[0].quality.ToString("F6")));
                        this.covariationPopulationBox.Invoke(new Action(() => this.covariationPopulationBox.Text = coefficient.ToString("F6")));

                        LogHelper.Write("Скрещивание...", "CorrectLog");
                        startSubnets = new List<Subnet>(subnets);
                        cntOfPopulationsPerStep = 2;
                        if (population == 3)
                        {
                            /*for (int i = 0; i < subnets.Count; i++)
                            {
                                this.testing(subnets[i]);
                                LogHelper.Write(i.ToString() + ";" + subnets[i].quality, "CorrectLog");
                            }*/
                            quality = mediumErrorForPopulation;
                            this.globalSubnet = subnets[0];
                            return quality;
                            //needToStop = true;
                        }
                    }
                    else
                    {
                        localSubNets = new List<Subnet>(startSubnets);
                    }
                }
            }
            return quality;
        }

        //коэффициент вариации для каждой позиции веса
        private double[] coefficientVariations(List<Subnet> subnets)
        {
            double[] mediumWeights = new double[this.getCountOfWeights(subnets[0].Network)];
            double[] coefficients = new double[this.getCountOfWeights(subnets[0].Network)];


            //calculate mediums
            int iWeight = 0;
            for (int layer = 0; layer < subnets[0].Network.Layers.Length; layer++)
            {
                for (int neuron = 0; neuron < subnets[0].Network.Layers[layer].Neurons.Length; neuron++)
                {
                    for (int weight = 0; weight < subnets[0].Network.Layers[layer].Neurons[neuron].Weights.Length; weight++)
                    {
                        double summ = 0.0;
                        for (int network = 0; network < subnets.Count; network++)
                        {
                            summ += Math.Abs(subnets[network].Network.Layers[layer].Neurons[neuron].Weights[weight]);
                        }
                        mediumWeights[iWeight] = summ / subnets.Count;
                        iWeight++;
                    }
                }

            }

            //calculate standart deviation
            iWeight = 0;
            for (int layer = 0; layer < subnets[0].Network.Layers.Length; layer++)
            {
                for (int neuron = 0; neuron < subnets[0].Network.Layers[layer].Neurons.Length; neuron++)
                {
                    for (int weight = 0; weight < subnets[0].Network.Layers[layer].Neurons[neuron].Weights.Length; weight++)
                    {
                        double summ = 0.0;
                        for (int network = 0; network < subnets.Count; network++)
                        {
                            summ += Math.Pow((subnets[network].Network.Layers[layer].Neurons[neuron].Weights[weight] - mediumWeights[weight]), 2);
                        }
                        coefficients[iWeight] = summ / subnets.Count;
                        coefficients[iWeight] = coefficients[iWeight] / mediumWeights[iWeight];
                        coefficients[iWeight] = coefficients[iWeight] * 100;
                        iWeight++;
                    }
                }

            }
            return coefficients;
        }

        //изменение весов в зависимости от результатов тестирования
        private void trololo(Subnet subnet)
        {
            Network network = subnet.Network;
            double quality = 0.0;
            double tempQuality = 0.0;
            double commonQuality = this.testing(subnet);

            for (int i = 0; i < network.Layers.Length; i++)
            {
                for (int j = 0; j < network.Layers[i].Neurons.Length; j++)
                {
                    for (int k = 0; k < network.Layers[i].Neurons[j].Weights.Length;)
                    {
                        network.Layers[i].Neurons[j].Weights[k] +=1;
                        quality = this.testing(subnet);

                        if (quality > commonQuality)
                        {
                            tempQuality = quality;
                            continue;
                        }
                        else if (quality < commonQuality)
                        {
                            network.Layers[i].Neurons[j].Weights[k] -= 1;
                            this.betterPopulationValueBox.Invoke(new Action(() => this.betterPopulationValueBox.Text = tempQuality.ToString("F6")));
                            this.updateBarCHart(quality, k);
                            k++;

                        }
                    }
                }
            }
        }

        //суммирование весов двух подсетей
        private Network summarazing(Subnet net1, Subnet net2, double[] coefficients)
        {
            int iWeight = 0;
            int availableMutations = 0;
            Random rnd = new Random();

            ActivationNetwork evoSubNet = new ActivationNetwork(new BipolarSigmoidFunction(2.0), net1.Network.InputsCount, net1.topology);
            NguyenWidrow ng = new NguyenWidrow(evoSubNet);
            ng.Randomize(0);

            for (int layer = 0; layer < net1.Network.Layers.Length; layer++)
            {
                for (int neuron = 0; neuron < net1.Network.Layers[layer].Neurons.Length; neuron++)
                {
                    for (int weight = 0; weight < net1.Network.Layers[layer].Neurons[neuron].Weights.Length; weight++)
                    {
                        if (coefficients[iWeight] < 20.0 && availableMutations < 25)
                        {
                            evoSubNet.Layers[layer].Neurons[neuron].Weights[weight] =
                                (net1.Network.Layers[layer].Neurons[neuron].Weights[weight] + net2.Network.Layers[layer].Neurons[neuron].Weights[weight])/2  + rnd.Next(0, 1000);
                            availableMutations++;
                        }
                        else {
                            evoSubNet.Layers[layer].Neurons[neuron].Weights[weight] =
                                (net1.Network.Layers[layer].Neurons[neuron].Weights[weight] + net2.Network.Layers[layer].Neurons[neuron].Weights[weight])/2;
                        }
                        iWeight++;
                    }
                }
            }
            return evoSubNet;
        }

        private void test()
        {
            double testQuality = 0.0;
            double validateError = 0.0;

            String[] lines = new String[1];
            double[] res = new double[classesList.Count];
            double[] input = new double[this.network.InputsCount];

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\SpreadSheetTest.csv"))

                for (int i = 0; i < data.GetLength(0); i++)
                {
                    //gather inputs for compute, n-1 inputs
                    for (int k = 0; k < this.network.InputsCount - 1; k++)
                    {
                        input[k] = data[i, k];
                    }

                    if (this.globalSubnet != null)
                    {
                        res = network.Compute(input.ToArray(), this.globalSubnet, MinMaxValues);
                    }
                    else
                    {
                        res = network.Compute(input.ToArray());
                    }

                    double value = Math.Abs(classes[i] - this.max(res));

                    validateError += value;
       

                    if (this.selectedType == "classification")
                    {
                        lines[0] = classes[i].ToString() + ";" + this.max(res).ToString("F8");

                    }
                    //else if (this.selectedType == "regression")
                        //lines[0] = data[i, this.network.InputsCount - 1].ToString() + ";" + res.ToString();

                    file.WriteLine(lines[0].ToString());
                }

            testQuality = (1-(validateError / data.GetLength(0))) * 100;
            this.errorTextBox.Invoke(new Action(() => this.errorTextBox.Text = testQuality.ToString("F6")));
            MessageBox.Show("Тестирование пройдено");
        }
        
        //обновление графика
        private void updateBarCHart(double y,  int x)
        {
            CurveItem curve = zedGraphControl1.GraphPane.CurveList[0] as LineItem;
            if (curve == null)
                return;

            // Get the PointPairList
            IPointListEdit qualities = curve.Points as IPointListEdit;

            if (qualities == null)
                return;

            qualities.Add(x, y);
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        //индекс максимального значения в массиве
        private double max(double[] mass)
        {
            double classificator = 0;
            double tempValue = 0.0;
            int ones = 0;
            for (int i = 0; i < mass.Length; i++)
            {
                if (mass[i] == 1.0)
                {
                    ones++;
                    tempValue += i;
                }
            }
            if (ones <= 1)
            {              
                for (int i = 0; i < mass.Length; i++)
                {
                    if (tempValue < mass[i])
                    {
                        tempValue = mass[i];
                        classificator = i;
                    }
                }
            }
            else
            {
                classificator = tempValue / ones;
            }
            return classificator;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            needToStop = true;
        }

    }


}
