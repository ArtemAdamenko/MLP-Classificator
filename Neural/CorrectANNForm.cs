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
using ZedGraph;


namespace Neural
{
    public partial class CorrectANNForm : Form
    {
        
        //App options
        Network network = null;
        Thread Worker;
        Subnet globalSubnet;
        Boolean SpreadTest = false;
        int numberSubNets = 0;
        String commonNetFileName = "";
        String dataFileName = "";

        private List<Record> MinMaxValues;
        private List<Record> relationsValues = new List<Record>();
        private List<Record> tempRelationsValues = new List<Record>();

        List<String> availableRelations = new List<String>(),
                    connectedInputNeurons = new List<String>(),
                    inputSubNet = new List<String>();

        List<String> outputSubnet = new List<String>(),
               connectedOutputNeurons = new List<String>();


        #region Neural Net options
            int input = 0;
            int[] hidden;
            int cntPopulation = 0;
            int cntOfPopulationsPerStep = 0;
            double alpha = 0.0;
            double validQuality = 0.0;
            double levelVariationsWeights = 0.0;
            Boolean bothEqualNets = false;

            int reconnectingCount = 0; 
            int population = 0;

            private double[,] data = null;
            private int[] classes;
            List<int> classesList = new List<int>();
            private bool needToStop = false;
            int rowCountData = 0;
            int colCountData = 0;
            DateTime t = DateTime.Now;
        #endregion

        #region draw options
                private SolidBrush _myBrush = new SolidBrush(Color.DarkSeaGreen);
                private SolidBrush _offBrush = new SolidBrush(Color.Gray);
                private Pen _myPen = new Pen(Color.Black);
        #endregion

        public CorrectANNForm()
        {
            InitializeComponent();
            //add stop event handler

            RollingPointPairList listErrorP = new RollingPointPairList(200);
            RollingPointPairList listError = new RollingPointPairList(200);
            GraphPane myPane = zedGraphControl1.GraphPane;
            LineItem curve = myPane.AddCurve("Изменение по модулю", listError, Color.Blue, SymbolType.None);
            LineItem curve2 = myPane.AddCurve("Изменение вероятности", listErrorP, Color.Goldenrod, SymbolType.XCross);
            myPane.Title.Text = "Корректирование нейронной сети";
            myPane.XAxis.Title.Text = "Корректирующие подсети";
            myPane.YAxis.Title.Text = "Процент правильных ответов";
            myPane.XAxis.Scale.MajorStep = 10;
            myPane.YAxis.Scale.MajorStep = 10;
            myPane.XAxis.MajorGrid.Color = Color.Black;
            myPane.YAxis.MajorGrid.Color = Color.Black;
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);
            myPane.YAxis.Scale.Max = 100;

            curve.Line.Width = 2.0F;
            curve2.Line.Width = 2.0F;
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.IsVisible = true;

            this.checkTopology();

        }

        private void checkTopology()
        {
            //if string not empty and if string topology contain minimum 2 not empty values
            if ((this.subnetTopologyBox.Text != "") && (this.subnetTopologyBox.Text.Split(',').Where(n => !string.IsNullOrEmpty(n)).ToArray().Length > 1))
            {
                string[] topology = this.subnetTopologyBox.Text.Split(',').Where(n => !string.IsNullOrEmpty(n)).ToArray();
                int input = Int32.Parse(topology[0]);
                int output = Int32.Parse(topology[topology.Length - 1]);
                int[] hiddens = new int[topology.Length - 1];
                int hiddenLayer = 0;

                for (int i = 1; i <= topology.Length - 1; i++)
                {
                    hiddens[hiddenLayer] = Int32.Parse(topology[i]);
                    hiddenLayer++;
                }
                this.input = input;
                this.hidden = hiddens;

                this.relationsLabel.Text = "(" + (input + output).ToString() + ")";
            }
            else {
                this.relationsLabel.Text = "(0)";
            }
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
            openFileDialog1.Filter = "crash Bin Files|*.cbin";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    network = Network.Load(openFileDialog1.FileName);
                    commonNetFileName = openFileDialog1.SafeFileName;

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

            Worker = new Thread(this.getDataForClass);

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
        private double[] testing(Subnet thisSubnet = null)
        {
            double probabilisticTestQuality = 0.0;
            double probabilisticValidateError = 0.0;
            double moduleTestQuality = 0.0;
            double moduleValidateError = 0.0;
            double[] input = new double[this.network.InputsCount];
            double[] res;


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

                    double value = Math.Abs(classes[count] - classesList[this.max(res)]);
                    double valueP = 1 - res[classesList.IndexOf(classes[count])];


                    probabilisticValidateError += valueP;
                    moduleValidateError += value;

                }
                catch (Exception e)
                {
                    MessageBox.Show("Ошибка тестирования сети." + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }      
            }
            moduleTestQuality = (1 - (moduleValidateError / data.GetLength(0))) * 100;
            probabilisticTestQuality = (1 - (probabilisticValidateError / data.GetLength(0))) * 100;
            this.moduleErrorBox.Invoke(new Action(() => moduleErrorBox.Text = moduleTestQuality.ToString("F6")));
            this.probabilisticErrorTextBox.Invoke(new Action(() => probabilisticErrorTextBox.Text = probabilisticTestQuality.ToString("F6")));

            return new double[2]{moduleTestQuality, probabilisticTestQuality};
 
        }

        //Процент ошибки при тестировании на выбранной выборке
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
                    dataFileName = openFileDialog2.SafeFileName;
                    //get row count values
                    String line;
                    rowCountData = 0;
                    colCountData = 0;

                    //get input and output count
                    line = reader.ReadLine();
                    rowCountData++;
                    //-1 last element empty
                    colCountData = line.Trim().Split(';').Length;

                    //mass for new normalization cols input data
                    double[] minData = new double[colCountData - 1];
                    double[] maxData = new double[colCountData - 1];

                    //must be > 1 column in training data
                    if (colCountData == 1)
                        throw new Exception();

                    while ((line = reader.ReadLine()) != null)
                    {
                        rowCountData++;
                    }

                    double[,] tempData = new double[rowCountData, colCountData - 1];
                    int[] tempClasses = new int[rowCountData];

                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    line = "";

                    // read the data
                    classesList.Clear();
                    while ((i < rowCountData) && ((line = reader.ReadLine()) != null))
                    {
                        string[] strs = line.Trim().Split(';');
                        List<String> inputVals = new List<String>(strs);

                        //del empty values in the end
                        inputVals.RemoveAll(str => String.IsNullOrEmpty(str));

                        // parse input and output values for learning
                        for (int j = 0; j < colCountData - 1; j++)
                        {
                            tempData[i, j] = double.Parse(inputVals[j]);

                            //search min/max values for each column
                            if (tempData[i, j] < minData[j])
                                minData[j] = tempData[i, j];
                            if (tempData[i, j] > maxData[j])
                                maxData[j] = tempData[i, j];
                        }

                        tempClasses[i] = int.Parse(inputVals[colCountData - 1]);

                        //insert class in list of classes, if not find
                        if (classesList.IndexOf(tempClasses[i]) == -1)
                        {
                            classesList.Add(tempClasses[i]);
                        }

                        i++;
                    }

                    //normalization input values
                    tempData = ANNUtils.normalization( tempData, minData, maxData, rowCountData, colCountData );

                    // allocate and set data
                    data = new double[i, colCountData - 1];
                    Array.Copy(tempData, 0, data, 0, i * (colCountData - 1));
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

        //проверка входящих параметров
        private void checkOptions()
        {
            try
            {
                if (this.levelVariationscheckBox.Checked)
                {
                    this.levelVariationsWeights = Math.Max(0, Math.Min(1, double.Parse(levelVariationstextBox.Text)));
                }
            }
            catch
            {
                this.levelVariationsWeights = 0.5;
            }

            try
            {
                this.bothEqualNets = this.BothEqualcheckBox.Checked;
            }
            catch
            {
                this.bothEqualNets = true;
            }

            try
            {
                this.cntPopulation  = Math.Max(1, Math.Min(100, Int32.Parse(populationtextBox.Text)));
            }
            catch
            {
                this.cntPopulation = 2;
            }

            try
            {
                this.cntOfPopulationsPerStep = Math.Max(1, Math.Min(100, Int32.Parse(popultextBox.Text)));
            }
            catch
            {
                this.cntOfPopulationsPerStep = 2;
            }
            try
            {
                this.alpha = Math.Max(0, Math.Min(2.0, double.Parse(alphaBox.Text)));
            }
            catch
            {
                this.alpha = 2.0;
            }
            try
            {
                this.validQuality = Math.Max(0, Math.Min(100, double.Parse(validLevelBox.Text)));
            }
            catch
            {
                this.validQuality = 75;
            }
            try
            {
                this.numberSubNets = Math.Max(0, Math.Min(1000, Int32.Parse(numberSubnetsBox.Text)));
                if (this.numberSubNets % 2 == 1)
                    throw new Exception("Введите четное число для кол-ва подсетей в поколении!");
            }
            catch
            {
                this.numberSubNets = 100;
            }

            this.BothEqualcheckBox.Invoke(new Action(() => this.BothEqualcheckBox.Checked = this.bothEqualNets));
            this.levelVariationstextBox.Invoke(new Action(() => this.levelVariationstextBox.Text = this.levelVariationsWeights.ToString()));
            this.numberSubnetsBox.Invoke(new Action(() => this.numberSubnetsBox.Text = this.numberSubNets.ToString()));
            this.populationtextBox.Invoke(new Action(() => this.populationtextBox.Text =  this.cntPopulation.ToString()));
            this.popultextBox.Invoke(new Action(() => this.popultextBox.Text = this.cntOfPopulationsPerStep.ToString()));
            this.alphaBox.Invoke(new Action(() => this.alphaBox.Text = this.alpha.ToString()));
            this.validLevelBox.Invoke(new Action(() => this.validLevelBox.Text = this.validQuality.ToString()));
        }

        private void sortConnects(Subnet subnet1)
        {
            //Random rnd = new Random();
            //generate connections between common net and subnet
            this.availableRelations = new List<String>(ANNUtils.getAvailableWeights(this.network));
            this.connectedInputNeurons = new List<String>();
            this.inputSubNet = new List<String>();

            for (int i = 0; i < input; )
            {

                //int rndRelation = rnd.Next(0, availableRelations.Count);
                String connect = this.availableRelations[i];
                //availableRelations.Remove(connect);
                this.connectedInputNeurons.Add(connect);
                this.inputSubNet.Add("-1:" + i.ToString());
                i++;

            }

            //connected last layer of subnet to last layer of common net
            this.outputSubnet = new List<String>();
            this.connectedOutputNeurons = new List<String>();

            for (int outputNeuron = 0; outputNeuron < this.hidden[this.hidden.Length - 1]; )
            {

                //int rndRelation = rnd.Next(0, availableRelations.Count);
                String connect = this.availableRelations[(this.availableRelations.Count - 1) - outputNeuron];
                //availableRelations.Remove(connect);
                this.connectedOutputNeurons.Add(connect);
                this.outputSubnet.Add(subnet1.Network.Layers.Length - 1 + ":" + outputNeuron.ToString());
                outputNeuron++;
            }
        }

        private void randomConnects(Subnet subnet1)
        {
            Random rnd = new Random();
            this.availableRelations = new List<String>(ANNUtils.getAvailableWeights(this.network));
            this.connectedInputNeurons = new List<String>();
            this.inputSubNet = new List<String>();

            for (int i = 0; i < input; )
            {

                int rndRelation = rnd.Next(0, this.availableRelations.Count);
                String connect = this.availableRelations[rndRelation];
                this.availableRelations.Remove(connect);
                this.connectedInputNeurons.Add(connect);
                this.inputSubNet.Add("-1:" + i.ToString());
                i++;

            }

            //connected last layer of subnet to last layer of common net
            outputSubnet = new List<String>();
            connectedOutputNeurons = new List<String>();

            for (int outputNeuron = 0; outputNeuron < this.hidden[this.hidden.Length - 1]; )
            {

                int rndRelation = rnd.Next(0, this.availableRelations.Count);
                String connect = this.availableRelations[rndRelation];
                this.availableRelations.Remove(connect);
                this.connectedOutputNeurons.Add(connect);
                this.outputSubnet.Add(subnet1.Network.Layers.Length - 1 + ":" + outputNeuron.ToString());
                outputNeuron++;
            }
        }
        //генерация подсетей
        private void GenerateSubNets()
        {
            this.checkOptions();
            List<String> selectionResult = new List<String>();
            //обнуление параметров
            Subnet._ID = 0;
            this.reconnectingCount = 0;
            zedGraphControl1.GraphPane.CurveList[0].Clear();
            zedGraphControl1.GraphPane.CurveList[1].Clear();

            double[] commonNetQuality = this.testing();
            //create subnet  
            Random rnd = new Random();

            Subnet subnet1 = new Subnet(new ActivationNetwork(new SigmoidFunction(this.alpha), this.input, this.hidden));
            LogHelper.NewSessionFolder(commonNetFileName, dataFileName);
            LogHelper.InitReporting( subnet1 );

            List<Subnet> subnets = new List<Subnet>();

            if (this.sortConnectBox.Checked)
            {
                this.sortConnects(subnet1); 
            }else
            {
                this.randomConnects(subnet1);
            }       


            LogHelper.SubConnectReport(this.connectedInputNeurons, this.connectedOutputNeurons, this.reconnectingCount);
            this.test();

            this.draw(subnet1.Network, pictureBox2, this.inputSubNet, this.outputSubnet);
            this.draw(this.network, pictureBox1, this.connectedInputNeurons, this.connectedOutputNeurons);

            this.label2.Invoke(new Action(() => this.label2.Text = "Отбор..."));
            
            double currentQuality = 0.0;
            DateTime t = DateTime.Now;

            --Subnet._ID;

            selectionResult.Add("Отбор");

            while (!needToStop)
            {
                ActivationNetwork network = new ActivationNetwork(new SigmoidFunction(this.alpha), input, hidden);

                NguyenWidrow initializer = new NguyenWidrow(network);
                initializer.Randomize(0);

                Subnet subnet = new Subnet(network);
                subnet.inputAssosiated = this.connectedInputNeurons;
                subnet.outputAssosiated = this.connectedOutputNeurons;

                commonNetQuality = this.testing(subnet);
                subnet.quality = commonNetQuality[0];

                subnets.Add(subnet);

                LogHelper.SubnetToFile(subnet, reconnectingCount, false);

                selectionResult.Add("ID_" + subnet.ID.ToString() + "-" + subnet.quality.ToString() + "%");

                DateTime t2 = DateTime.Now;
                System.TimeSpan diffTime = t2.Subtract(t);
                this.timeLabel.Invoke(new Action(() => this.timeLabel.Text = diffTime.ToString()));

                this.updateBarCHart(subnet.quality, subnets.Count - 1, commonNetQuality[1]);

                //когда отбор окончился
                if (subnets.Count == this.numberSubNets)
                {
                    LogHelper.saveMSGToSheet( selectionResult, 0 );

                    currentQuality = this.evolution(subnets, this.connectedInputNeurons, this.connectedOutputNeurons);

                    subnets.Clear();
                    zedGraphControl1.GraphPane.CurveList[0].Clear();
                    zedGraphControl1.GraphPane.CurveList[1].Clear();

                    if ((this.validQuality > currentQuality) && !needToStop)
                    {
                        //if random option check
                        if (!this.sortConnectBox.Checked)
                        {
                            //get one connection and set one random new connection for find optimization connecteds
                            int reconnected = Int32.Parse(reconnectBox.Text);
                            Random random = new Random();

                            List<String> tempNewInputs = new List<String>();
                            List<String> tempNewOutputs = new List<String>();
                            List<String> tempOlds = new List<String>();

                            for (int k = 0; k < reconnected; k++)
                            {
                                int temp = random.Next(0, 2);
                                //or input conn reconn, or output conn reconn  - random
                                if (temp == 0 && (this.connectedInputNeurons.Count >= 1))
                                {
                                    int oldOutput = rnd.Next(0, this.connectedInputNeurons.Count);
                                    String oldValue = this.connectedInputNeurons[oldOutput];
                                    this.connectedInputNeurons.RemoveAt(oldOutput);
                                    tempOlds.Add(oldValue);

                                    //new connection output
                                    int newOutput = rnd.Next(0, this.availableRelations.Count);
                                    String newValue = this.availableRelations[newOutput];
                                    this.availableRelations.RemoveAt(newOutput);
                                    tempNewInputs.Add(newValue);

                                }
                                else if (temp == 1 && (this.connectedOutputNeurons.Count >= 1))
                                {
                                    int oldOutput = rnd.Next(0, this.connectedOutputNeurons.Count);
                                    String oldValue = this.connectedOutputNeurons[oldOutput];
                                    this.connectedOutputNeurons.RemoveAt(oldOutput);
                                    tempOlds.Add(oldValue);

                                    //new connection output
                                    int newOutput = rnd.Next(0, this.availableRelations.Count);
                                    String newValue = this.availableRelations[newOutput];
                                    this.availableRelations.RemoveAt(newOutput);
                                    tempNewOutputs.Add(newValue);

                                }

                            }
                            connectedInputNeurons.AddRange(tempNewInputs);
                            connectedOutputNeurons.AddRange(tempNewOutputs);
                            availableRelations.AddRange(tempOlds);

                            //увеличиваем счетчик переключений
                            ++this.reconnectingCount;
                            LogHelper.SubConnectReport(this.connectedInputNeurons, this.connectedOutputNeurons, this.reconnectingCount);
                            LogHelper.Commit();
                            LogHelper.InitReporting(subnet1, this.reconnectingCount);

                            this.draw(subnet1.Network, pictureBox2, this.inputSubNet, this.outputSubnet);
                            this.draw(this.network, pictureBox1, this.connectedInputNeurons, this.connectedOutputNeurons);
                        }
                        //if sort connectn option check
                        else
                        {
                            //LogHelper.Commit();
                            needToStop = true;
                            this.test();
                            SpreadTest = false;
                            break;
                        }
                    }
                    else if(!needToStop && (this.validQuality < currentQuality)) {

                        needToStop = true;
                        this.test();
                        SpreadTest = false;
                        break;
                    }            
                }
                else
                    this.betterBox.Invoke(new Action(() => this.betterBox.Text = subnets.Count.ToString()));
            }
            LogHelper.Commit();
            return;

        }

        //запуск потока генерации подсети
        private void subNetButton_Click(object sender, EventArgs e)
        {
            needToStop = false;
            Worker = new Thread(GenerateSubNets);

            Worker.SetApartmentState(ApartmentState.STA);
            Worker.Start();     
        }

        //эволюция подсетей
        private double evolution(List<Subnet> subnets, List<String> connectedInputNeurons, List<String> connectedOutputNeurons)
        {
            List<Subnet> localSubNets = new List<Subnet>(subnets);
            List<Subnet> startSubnets = new List<Subnet>(subnets);
            List<String> selectionResult = new List<String>();

            Random rnd = new Random();
            double[] coefficients;
            double[] mediums;
            double[] deviations;
            double quality = 0.0;
            double[] commonNetQuality = new double[2];
            population = 1;

            //standart deviation
            mediums = ANNUtils.mediums(subnets);
            deviations = ANNUtils.standartDeviation(mediums, subnets);
            

            coefficients = ANNUtils.coefficientVariations( mediums, deviations );
            LogHelper.StandartDeviationToFile(deviations, population, "После отбора");
            LogHelper.coefVariationsToFile( coefficients, population, "После отбора" );
            LogHelper.saveMediumsValuesOfWeightsIndexes(subnets, population, "После отбора");

            selectionResult.Add("Скрещивание " + population);

            while (localSubNets.Count > 0 && !needToStop)
            {
                PopulationBox.Invoke(new Action(() => PopulationBox.Text = population.ToString()));
                label2.Invoke(new Action(() => label2.Text = "Скрещивание..."));
                betterBox.Invoke(new Action(() => betterBox.Text = subnets.Count.ToString()));

                //get random pair of nets, delete after from list
                int net = rnd.Next(0, localSubNets.Count);
                Subnet parentSub1 = localSubNets[net];
                localSubNets.RemoveAt(net);

                net = rnd.Next(0, localSubNets.Count);
                Subnet parentSub2 = localSubNets[net];
                localSubNets.RemoveAt(net);

                //testing and adding
                Subnet sub = new Subnet( summarazing( parentSub1, parentSub2, coefficients ) );

                sub.inputAssosiated = connectedInputNeurons;
                sub.outputAssosiated = connectedOutputNeurons;

                commonNetQuality = testing(sub);
                sub.quality = commonNetQuality[0];
                sub.probabilistic = commonNetQuality[1];

                selectionResult.Add("ID_" + sub.ID.ToString() + "-P1_" + parentSub1.ID.ToString() + "-P2_" + parentSub2.ID.ToString()
                                    + "-" + sub.quality.ToString() + "%");

                LogHelper.SubnetToFile(sub, reconnectingCount, true, parentSub1.ID + "-" + parentSub2.ID, population);
                subnets.Add(sub);
                updateBarCHart(sub.quality, subnets.Count, sub.probabilistic);

                timeLabel.Invoke(new Action(() => timeLabel.Text = DateTime.Now.Subtract(t).ToString()));

                //скрещивания окончилось
                if (localSubNets.Count == 0)
                {
                    --cntOfPopulationsPerStep;

                    if (cntOfPopulationsPerStep == 0)
                    {
                        LogHelper.saveMSGToSheet(selectionResult, population);
                        selectionResult.Clear();

                        population++;
                        double mediumErrorForPopulation = 0.0;
                        double summ = 0.0;
                        double coefficient = 0.0;
                        selectionResult.Add("Скрещивание " + population);

                        //удаляем  самых худших
                        zedGraphControl1.GraphPane.CurveList[0].Clear();
                        zedGraphControl1.GraphPane.CurveList[1].Clear();
                        label2.Invoke(new Action(() => label2.Text = "Отбор лучших..."));        
                        localSubNets = ANNUtils.dropBadSubnets( subnets, (numberSubNets / 2) * Int32.Parse(popultextBox.Text) );
                        subnets = new List<Subnet>(localSubNets);
                        betterBox.Invoke(new Action(() => betterBox.Text = localSubNets.Count.ToString()));

                        //отображаем лучших оставшихся на графике
                        int i = 0;
                        foreach (Subnet subnet in localSubNets)
                        {
                            updateBarCHart( subnet.quality, i++, subnet.probabilistic );
                            mediumErrorForPopulation += subnet.quality;
                        }

                        mediumErrorForPopulation /= subnets.Count;

                        foreach (Subnet subnet in localSubNets)
                        {
                            summ += Math.Pow( (subnet.quality - mediumErrorForPopulation), 2 );
                        }

                        coefficient = Math.Sqrt( summ / subnets.Count );
                        coefficient = coefficient / mediumErrorForPopulation;

                        //standart deviation
                        mediums = ANNUtils.mediums(localSubNets);
                        deviations = ANNUtils.standartDeviation(mediums, localSubNets);

                        coefficients = ANNUtils.coefficientVariations(mediums, deviations);
                        LogHelper.StandartDeviationToFile( deviations, population, "После скрещивания " + (population - 1) );
                        LogHelper.coefVariationsToFile( coefficients, population, "После скрещивания " + (population - 1) );
                        LogHelper.saveMediumsValuesOfWeightsIndexes( localSubNets, population, "После скрещивания " + (population - 1) );
                        LogHelper.commonCorrectResults( mediumErrorForPopulation, localSubNets[0].quality, coefficient, (population - 1));

                        cntOfPopulationsPerStep = Int32.Parse(popultextBox.Text); ;
                        startSubnets = new List<Subnet>(localSubNets);
                        mediumErrorPopulationBox.Invoke(new Action(() => mediumErrorPopulationBox.Text = mediumErrorForPopulation.ToString("F6")));
                        betterPopulationValueBox.Invoke(new Action(() => betterPopulationValueBox.Text = localSubNets[0].quality.ToString("F6")));
                        covariationPopulationBox.Invoke(new Action(() => covariationPopulationBox.Text = coefficient.ToString("F6")));

                        if (population > cntPopulation)
                        {
                            globalSubnet = subnets[0];
                            PopulationBox.Invoke(new Action(() => PopulationBox.Text = "0"));
                            return subnets[0].quality;
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

        //суммирование весов двух подсетей
        private Network summarazing(Subnet net1, Subnet net2, double[] coefficients)
        {
            int iWeight = 0;
            int availableMutations = 0;
            Random rnd = new Random();

            ActivationNetwork evoSubNet = new ActivationNetwork(new SigmoidFunction(this.alpha), net1.Network.InputsCount, net1.topology);
            NguyenWidrow ng = new NguyenWidrow(evoSubNet);
            ng.Randomize(0);

            for (int layer = 0; layer < net1.Network.Layers.Length; layer++)
            {
                for (int neuron = 0; neuron < net1.Network.Layers[layer].Neurons.Length; neuron++)
                {
                    for (int weight = 0; weight < net1.Network.Layers[layer].Neurons[neuron].Weights.Length; weight++)
                    {

                        if (this.levelVariationscheckBox.Checked)
                        {
                            //если коэф вариации позиции веса ниже порога - встряхивание
                            if (Math.Abs(coefficients[iWeight]) < this.levelVariationsWeights)
                            {
                                double currWeight = evoSubNet.Layers[layer].Neurons[neuron].Weights[weight];

                                currWeight =
                                    (net1.Network.Layers[layer].Neurons[neuron].Weights[weight] + net2.Network.Layers[layer].Neurons[neuron].Weights[weight]) / 2;


                                currWeight *= rnd.NextDouble() * currWeight;
                                availableMutations++;
                            }
                            else if ((Math.Abs(coefficients[iWeight]) > this.levelVariationsWeights))
                            {
                                evoSubNet.Layers[layer].Neurons[neuron].Weights[weight] =
                                    (net1.Network.Layers[layer].Neurons[neuron].Weights[weight] + net2.Network.Layers[layer].Neurons[neuron].Weights[weight]) / 2;
                            }
                        }

                        //если стоит галочка на одинаковые сети и результаты данных сетей одинаковые - встряхивание
                        if (this.BothEqualcheckBox.Checked)
                        {
                            if (net1.quality == net2.quality)
                            {
                                double currWeight = evoSubNet.Layers[layer].Neurons[neuron].Weights[weight];
                                currWeight =
                                   (net1.Network.Layers[layer].Neurons[neuron].Weights[weight] + net2.Network.Layers[layer].Neurons[neuron].Weights[weight]) / 2;
                                currWeight *= rnd.NextDouble() * currWeight;

                                availableMutations++;
                            }
                            else
                            {
                                evoSubNet.Layers[layer].Neurons[neuron].Weights[weight] =
                                    (net1.Network.Layers[layer].Neurons[neuron].Weights[weight] + net2.Network.Layers[layer].Neurons[neuron].Weights[weight]) / 2;
                            }

                        }
                        if (!this.levelVariationscheckBox.Checked && !this.BothEqualcheckBox.Checked)
                        {
                            evoSubNet.Layers[layer].Neurons[neuron].Weights[weight] =
                                   (net1.Network.Layers[layer].Neurons[neuron].Weights[weight] + net2.Network.Layers[layer].Neurons[neuron].Weights[weight]) / 2;
                        }
                        //если ни то ни то - не встряхиваем
                        
                        iWeight++;
                    }
                }
            }
            return evoSubNet;
        }

        private void test()
        {
            double moduleTestQuality = 0.0;
            double probabilisticTestQuality = 0.0;
            double moduleValidateError = 0.0;
            double probabilisticValidateError = 0.0;

            double[] res = new double[classesList.Count];
            double[] input = new double[this.network.InputsCount];

            double[] desireClass = new double[classes.Length];
            double[] outputClass = new double[classes.Length];
            String filename = "";

            if (!this.SpreadTest)
                filename = "ДоКорректировки";
            else
                filename = "ПослеКорректировки";

            for (int i = 0; i < data.GetLength(0); i++)
            {
                //gather inputs for compute, n-1 inputs
                for (int k = 0; k < this.network.InputsCount; k++)
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

                double resClass = classesList[this.max(res)];
                desireClass[i] = classes[i];
                outputClass[i] = resClass;


                double value = Math.Abs(classes[i] - resClass);
                double valueP = 1 - res[classesList.IndexOf(classes[i])];

                moduleValidateError += value;
                probabilisticValidateError += valueP;


            }
            LogHelper.InputVectorResults(filename, desireClass, outputClass);

            this.SpreadTest = true;

            moduleTestQuality = (1 - (moduleValidateError / data.GetLength(0))) * 100;
            probabilisticTestQuality = (1 - (probabilisticValidateError / data.GetLength(0))) * 100;
            this.moduleErrorBox.Invoke(new Action(() => this.moduleErrorBox.Text = moduleTestQuality.ToString("F6")));
            this.probabilisticErrorTextBox.Invoke(new Action(() => this.probabilisticErrorTextBox.Text = probabilisticTestQuality.ToString("F6")));
            return;
        }
        
        //обновление графика
        private void updateBarCHart(double y,  int x, double p)
        {
            CurveItem curve = zedGraphControl1.GraphPane.CurveList[0] as LineItem;
            if (curve == null)
                return;
            CurveItem curve2 = zedGraphControl1.GraphPane.CurveList[1] as LineItem;
            if (curve2 == null)
                return;

            // Get the PointPairList
            IPointListEdit qualities = curve.Points as IPointListEdit;
            IPointListEdit qualitiesP = curve2.Points as IPointListEdit;

            if (qualities == null)
                return;
            if (qualitiesP == null)
                return;

            qualities.Add(x, y);
            qualitiesP.Add(x, p);
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        //индекс максимального значения в массиве
        private int max(double[] mass)
        {
            int classificator = 0;
            double tempValue = 0.0;
            /*int ones = 0;
            for (int i = 0; i < mass.Length; i++)
            {
                if (mass[i] == 1.0)
                {
                    ones++;
                    tempValue += i;
                }
            }
            if (ones <= 1)
            {*/
                tempValue = 0.0;
                for (int i = 0; i < mass.Length; i++)
                {
                    if (tempValue < mass[i])
                    {
                        tempValue = mass[i];
                        classificator = i;
                    }
                }
          /*  }
            else
            {
                classificator = tempValue / ones;
            }*/
            return classificator;
        }

        private void subnetTopologyBox_TextChanged(object sender, EventArgs e)
        {
            this.checkTopology();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            needToStop = true;
            this.test();
            SpreadTest = false;
        }

        private void sortConnectBox_CheckedChanged(object sender, EventArgs e)
        {
            if (sortConnectBox.Checked)
                this.reconnectBox.Enabled = false;
            else
                this.reconnectBox.Enabled = true;
        }
    }


}
