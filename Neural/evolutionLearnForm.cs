using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using ZedGraph;
using OfficeOpenXml;

namespace Neural
{
    public partial class EvolutionLearnForm : Form
    {
        #region ANN Options
        ActivationNetwork betterNet;
        int samples = 0;
        double[][] input;
        double[][] output;
        double[][] validateInput;
        double[][] validateOutput;
        int[] trainClasses;
        int[] validClasses;
        private double[,] data = null;
        int rowCountData = 0;
        int colCountData = 0;

        private double alpha = 2.0;
        private int iterations = 0;

        private double error = 0.0;
        private double moduleValidateError = 0.0;
        private double probabilisticValidateError = 0.0;

        private int[] neuronsAndLayers;
        private int[] classes;
        List<int> classesList = new List<int>();
        private int maxIterations = 500;
        private int numberNets = 50;
        private int cntPopulation = 2;
        private int cntOfPopulationsPerStep = 2;

        private Thread workerThread = null;
        private bool needToStop = false;

        ActivationNetwork network;
        IActivationFunction activationFunc = null;
        ISupervisedLearning teacher = null;
        #endregion

        // Constructor
        public EvolutionLearnForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            RollingPointPairList listValidate = new RollingPointPairList(300);
            GraphPane myPane = zedGraphControl1.GraphPane;

            LineItem curve = myPane.AddCurve("Кросс-валидация (модуль)", listValidate, Color.Goldenrod, SymbolType.XCross);
            myPane.Title.Text = "Обучение нейронной сети";
            myPane.XAxis.Title.Text = "Итерации";
           // myPane.YAxis.Title.Text = "Изменение ошибки обучения";
            myPane.XAxis.Scale.MajorStep = 10;
            myPane.YAxis.Scale.MajorStep = 10;
            myPane.XAxis.MajorGrid.Color = Color.Black;
            myPane.YAxis.MajorGrid.Color = Color.Black;
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);
            myPane.YAxis.Scale.Max = 100;

            //curve.Line.Width = 2.0F;
            curve.Line.Width = 2.0F;
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.IsVisible = true;

            // init controls
            UpdateSettings();


        }

        // On main form closing
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // check if worker thread is running
            if ((workerThread != null) && (workerThread.IsAlive))
            {
                needToStop = true;
                workerThread.Join();
            }
        }

        // Update settings controls
        private void UpdateSettings()
        {
            this.alphaBox.Text = alpha.ToString();

        }

        //load data for classification
        private void getTrainDataForClass()
        {
            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;
                int i = 0;
                try
                {
                    // open selected file
                    reader = File.OpenText(openFileDialog.FileName);

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

                    tempData = ANNUtils.normalization(tempData, minData, maxData, rowCountData, colCountData);

                    // allocate and set data
                    data = new double[i, colCountData - 1];
                    Array.Copy(tempData, 0, data, 0, i * (colCountData - 1));
                    classes = new int[i];



                    this.classesBox.Text = classesList.Count.ToString();
                    Array.Copy(tempClasses, 0, classes, 0, i);

                    inputCountBox.Invoke(new Action(() => inputCountBox.Text = (colCountData - 1).ToString()));
                    fileTextBox.Invoke(new Action(() => fileTextBox.Text = openFileDialog.SafeFileName.ToString()));

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

                // enable "Start" button
                startButton.Enabled = true;
            }
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            alphaBox.Invoke(new Action(() => alphaBox.Enabled = enable));
            startButton.Invoke(new Action(() => startButton.Enabled = enable));
            stopButton.Invoke(new Action(() => stopButton.Enabled = !enable));

        }

        // On button "Start"
        private void startButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.numberNets = Math.Max(0, Math.Min(1000, Int32.Parse(numberNetsBox.Text)));
                if (this.numberNets % 2 == 1)
                    throw new Exception("Введите четное число для кол-ва подсетей в поколении!");
            }
            catch
            {
                this.numberNets = 100;
            }
            try
            {
                this.cntPopulation = Math.Max(1, Math.Min(100, Int32.Parse(populationtextBox.Text)));
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
            // get alpha
            try
            {
                alpha = Math.Max(0, Math.Min(2.0, double.Parse(alphaBox.Text)));
            }
            catch
            {
                alpha = 2.0;
            }
            // get neurons count in first layer
            try
            {
                String[] temp = neuronsBox.Text.Split(',');
                if (temp.Length < 1)
                    throw new Exception();
                neuronsAndLayers = new int[temp.Length + 1];
                for (int i = 0; i < temp.Length; i++)
                {
                    neuronsAndLayers[i] = Math.Max(1, Math.Min(1000, int.Parse(temp[i])));
                }

                neuronsAndLayers[temp.Length] = classesList.Count;

            }
            catch
            {
                neuronsAndLayers = new int[1];
                neuronsAndLayers[0] = 2;
            }
            // update settings controls
            UpdateSettings();

            // disable all settings controls except "Stop" button
            EnableControls(false);

            // run worker thread
            needToStop = false;
            workerThread = new Thread(new ThreadStart(SearchSolution));
            workerThread.Start();
        }

        // On button "Stop"
        private void stopButton_Click(object sender, System.EventArgs e)
        {
            // stop worker thread
            needToStop = true;
            recordNeuroNet();

        }

        //Запись результата очередного обучения в таблицу
        private void recordNeuroNet()
        {
            String topology = "";

            for (int i = 0; i < network.Layers.Length; i++)
            {
                topology += network.Layers[i].InputsCount + "-";
            }

            topology += network.Layers[network.Layers.Length - 1].Neurons.Length;

            /*this.lastRunsGridView.Invoke(
                            new Action(() =>
                                lastRunsGridView.Rows.Add(this.iterations.ToString(), this.error.ToString(), (this.moduleValidateError).ToString(), (this.probabilisticValidateError).ToString(),
                                this.selectedAlgo, topology, this.alpha.ToString(), learningRate, moment)
                                ));*/
        }

        //создание сети для обучения и учителя по топологии
        private ActivationNetwork createNetwork(int[] topology)
        {
            if (this.alphaBox.Text != "")
                activationFunc = new SigmoidFunction(Double.Parse(this.alphaBox.Text));
            else
                activationFunc = new SigmoidFunction();

            ActivationNetwork network = new ActivationNetwork(activationFunc,
            colCountData - 1, topology);

            NguyenWidrow initializer = new NguyenWidrow(network);
            initializer.Randomize();

            return network;
        }

        // Worker thread
        void SearchSolution()
        {

            // number of learning samples
            samples = data.GetLength(0);

            // prepare learning data
            //80% training, 20% for validate data
            this.input = new double[(samples * 4) / 5][];
            this.output = new double[(samples * 4) / 5][];
            trainClasses = new int[(samples * 4) / 5];

            validateInput = new double[samples / 5][];
            validateOutput = new double[samples / 5][];
            validClasses = new int[samples / 5];

            // create multi-layer neural network

            int K = 0;
            int J = 0;

            for (int i = 0; i < samples; i++)
            {
                //80% training, 20% for validate data
                if ((i % 5) == 0) // validate input 20 %
                {
                    if (this.validateInput.GetLength(0) > K)
                    {
                        validateInput[K] = new double[colCountData - 1];

                        for (int c = 0; c < colCountData - 1; c++)
                        {
                            validateInput[K][c] = data[i, c];
                        }

                        validateOutput[K] = new double[classesList.Count];
                        validateOutput[K][classesList.IndexOf(classes[i])] = 1;
                        validClasses[K] = classes[i];
                        K++;
                    }
                }
                else //forward input 80 %
                {
                    // input data
                    if (this.input.GetLength(0) > J)
                    {
                        this.input[J] = new double[colCountData - 1];

                        for (int c = 0; c < colCountData - 1; c++)
                        {
                            this.input[J][c] = data[i, c];
                        }

                        //output data
                        this.output[J] = new double[classesList.Count];
                        this.output[J][classesList.IndexOf(classes[i])] = 1;
                        trainClasses[J] = classes[i];
                        J++;
                    }
                }
            }


            // iterations
            this.iterations = 1;
            this.error = 0.0;
            this.moduleValidateError = 0.0;
            this.probabilisticValidateError = 0.0;
            double currentQuality = 0;
            double[] results = new double[2];
            zedGraphControl1.GraphPane.CurveList[0].Clear();
           // zedGraphControl1.GraphPane.CurveList[2].Clear();
            zedGraphControl1.AxisChange();
            List<ActivationNetwork> networks = new List<ActivationNetwork>();

            // loop
            while (!needToStop)
            {



                ActivationNetwork network = createNetwork(neuronsAndLayers);

                results = this.Validation(network);
                moduleValidateError = results[0];
                //probabilisticValidateError = results[1];

                networks.Add(network);
                    // Make sure that the curvelist has at least one curve
                    if (zedGraphControl1.GraphPane.CurveList.Count <= 0)
                    return;

                // Get the first CurveItem in the graph
                CurveItem curve = zedGraphControl1.GraphPane.CurveList[0] as LineItem;
                if (curve == null)
                    return;
                // Get the PointPairList
                IPointListEdit listValidates = curve.Points as IPointListEdit;

                if (listValidates == null)
                    return;

                listValidates.Add(this.iterations, moduleValidateError);

                // Make sure the Y axis is rescaled to accommodate actual data
                zedGraphControl1.AxisChange();
                // Force a redraw
                zedGraphControl1.Invalidate();


                // set current iteration's info
                currentIterationBox.Invoke(new Action<string>((s) => currentIterationBox.Text = s), this.iterations.ToString());
                moduleValidBox.Invoke(new Action<string>((s) => moduleValidBox.Text = s), moduleValidateError.ToString("F14"));

                // increase current iteration
                this.iterations++;
                //когда отбор окончился
                if (networks.Count == this.numberNets)
                {
                    currentQuality = this.evolution(networks);
                }
            }
            // enable settings controls
            EnableControls(true);

        }

        //суммирование весов двух подсетей
        private ActivationNetwork summarazing(ActivationNetwork net1, ActivationNetwork net2)
        {
            Random rnd = new Random();

            ActivationNetwork evoNet = createNetwork(neuronsAndLayers);

            for (int layer = 0; layer < evoNet.Layers.Length; layer++)
            {
                for (int neuron = 0; neuron < evoNet.Layers[layer].Neurons.Length; neuron++)
                {
                    for (int weight = 0; weight < evoNet.Layers[layer].Neurons[neuron].Weights.Length; weight++)
                    {
                            evoNet.Layers[layer].Neurons[neuron].Weights[weight] =
                                   (net1.Layers[layer].Neurons[neuron].Weights[weight] + net2.Layers[layer].Neurons[neuron].Weights[weight]) / 2;
                    }
                }
            }
            return evoNet;
        }

        //эволюция сетей
        private double evolution(List<ActivationNetwork> networks)
        {
            List<ActivationNetwork> localNets = new List<ActivationNetwork>(networks);
            List<ActivationNetwork> startNets = new List<ActivationNetwork>(networks);
            List<String> selectionResult = new List<String>();

            Random rnd = new Random();

            double quality = 0.0;
            double[] results = new double[2];
            int population = 1;

            selectionResult.Add("Скрещивание " + population);

            while (localNets.Count > 0 && !needToStop)
            {
                PopulationBox.Invoke(new Action(() => PopulationBox.Text = population.ToString()));
                label2.Invoke(new Action(() => label2.Text = "Скрещивание..."));
                betterBox.Invoke(new Action(() => betterBox.Text = networks.Count.ToString()));

                //get random pair of nets, delete after from list
                int net = rnd.Next(0, localNets.Count);
                ActivationNetwork parentSub1 = localNets[net];
                localNets.RemoveAt(net);

                net = rnd.Next(0, localNets.Count);
                ActivationNetwork parentSub2 = localNets[net];
                localNets.RemoveAt(net);

                //testing and adding
                ActivationNetwork newNet = summarazing(parentSub1, parentSub2);

                results = this.Validation(newNet);
                newNet.moduleResult = results[0];

                networks.Add(newNet);
                CurveItem curve = zedGraphControl1.GraphPane.CurveList[0] as LineItem;

                // Get the PointPairList
                IPointListEdit listValidates = curve.Points as IPointListEdit;

                listValidates.Add(this.iterations, newNet.moduleResult);
                // Make sure the Y axis is rescaled to accommodate actual data
                zedGraphControl1.AxisChange();
                // Force a redraw
                zedGraphControl1.Invalidate();
                this.iterations++;
                //скрещивания окончилось
                if (localNets.Count == 0)
                {
                    --cntOfPopulationsPerStep;

                    if (cntOfPopulationsPerStep == 0)
                    {

                        population++;
                        double mediumErrorForPopulation = 0.0;
                        double summ = 0.0;

                        //удаляем  самых худших
                        zedGraphControl1.GraphPane.CurveList[0].Clear();
                        label2.Invoke(new Action(() => label2.Text = "Отбор лучших..."));
                        localNets = ANNUtils.dropBadSubnets(networks, (numberNets / 2) * Int32.Parse(popultextBox.Text));
                        networks = new List<ActivationNetwork>(localNets);
                        betterBox.Invoke(new Action(() => betterBox.Text = localNets.Count.ToString()));

                        //отображаем лучших оставшихся на графике
                        int i = 0;
                        foreach (ActivationNetwork network in localNets)
                        {
                            mediumErrorForPopulation += network.moduleResult;
                            summ += Math.Pow((network.moduleResult - mediumErrorForPopulation), 2);
                        }

                        mediumErrorForPopulation /= networks.Count;

                        cntOfPopulationsPerStep = Int32.Parse(popultextBox.Text); ;
                        startNets = new List<ActivationNetwork>(localNets);
                        mediumErrorPopulationBox.Invoke(new Action(() => mediumErrorPopulationBox.Text = mediumErrorForPopulation.ToString("F6")));
                        betterPopulationValueBox.Invoke(new Action(() => betterPopulationValueBox.Text = localNets[0].moduleResult.ToString("F6")));

                        if (population > cntPopulation)
                        {
                            betterNet = networks[0];
                            PopulationBox.Invoke(new Action(() => PopulationBox.Text = "0"));
                            return networks[0].moduleResult;
                        }
                    }
                    else
                    {
                        localNets = new List<ActivationNetwork>(startNets);
                    }
                }
            }
            return quality;
        }

        //валидация
        private double[] Validation(ActivationNetwork network)
        {

            double[] res;
            int count = 0;
            int len = 0;
            double moduleValue = 0;
            double probValue = 0;

            for (count = 0, len = input.Length; count < len; count++)
            {
                res = network.Compute(input[count]);
                moduleValue += Math.Abs(trainClasses[count] - classesList[ANNUtils.max(res)]);
                probValue += 1 - res[classesList.IndexOf(trainClasses[count])];

            }
            return new double[2] {((1 - (moduleValue / input.Length)) * 100), ((1 - (probValue / input.Length)) * 100)};
        }


        //Сохранение нейронной сети по указанному пути
        private void SaveNetToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //тестирование сети с записью результата в файл
        private void TestNetToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //кросс-валидация(20%) сети с записью результата в файл
        private void crossValid()
        {
            double[] res = new double[classesList.Count];
            double[] input = new double[colCountData - 1];

            FileInfo file = new FileInfo(LogHelper.getPath("Learn") + "\\Валидация-" + LogHelper.getTime() + ".xlsx");
            ExcelPackage book = new ExcelPackage(file);
            ExcelWorksheet sheet = book.Workbook.Worksheets.Add("Тестирование 20% валидационной выборки");

            for (int i = 0; i < validateInput.GetLength(0) - 1; i++)
            {
                //gather inputs for compute, n-1 inputs
                for (int k = 0; k < colCountData - 1; k++)
                {
                    input[k] = validateInput[i][k];
                }

                res = network.Compute(input);

                int classificator = classesList[ANNUtils.max(res)];
                sheet.Cells[i + 1, 1].Value = validClasses[i];
                sheet.Cells[i + 1, 2].Value = classificator;
            }
            MessageBox.Show("Кросс-валидация пройдена.");
            book.Save();
            return;
        }


        //загрузка формы
        private void Form1_Load(object sender, EventArgs e)
        {
            HelloForm form = this.Owner as HelloForm;
        }

        //запуск кросс-валидации
        private void crossValidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.crossValid();
        }

        //загрузка обучающей выборки
        private void loadTrainDataButton_Click(object sender, EventArgs e)
        {

            this.getTrainDataForClass();

        }

        private void SaveWeightsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
