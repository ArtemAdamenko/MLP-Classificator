using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using AForge;
using System.Collections;
using AForge.Controls;
using AForge.Neuro;
using AForge.Neuro.Learning;
using Accord.Neuro;
using Accord.Math;
using Accord.Neuro.Learning;
using Accord;
using ZedGraph;


namespace Neural
{
    public partial class Form1 : Form
    {
        private double[,] data = null;
        int rowCountData = 0;
        int colCountData = 0;

        string[] sourceColumns;
        double[,] sourceMatrix;

        private double learningRate = 0.1;
        private double alpha = 0.1;
        private double moment = 0.3;
        private int iterations = 0;
        private double error = 0.0;
        private double validateError = 0.0;
        private String selectedAlgo = "";
        private int[] neuronsAndLayers;
        private int[] classes;
        private int classesCount;
        private int[] samplesPerClass;
        private ActivationNetwork[] allNetworks = null;
        private double validLevel = 0.2;
        private int maxIterations = 1000;
        private int maxNeuronsInLayer = 10;

        private Thread workerThread = null;
        private bool needToStop = false;
        ActivationNetwork network;
        IActivationFunction activationFunc = null;
        ISupervisedLearning teacher = null;

        // color for data series
        private static Color[] dataSereisColors = new Color[10] {
																	 Color.Red,		Color.Blue,
																	 Color.Green,	Color.DarkOrange,
																	 Color.Violet,	Color.Brown,
																	 Color.Black,	Color.Pink,
																	 Color.Olive,	Color.Navy };


        // Constructor
        public Form1()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

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
            this.momentBox.Text = moment.ToString();
            this.learningRateBox.Text = learningRate.ToString();
            this.validationLevelBox.Text = validLevel.ToString();
            this.maxIterationsBox.Text = maxIterations.ToString();
            this.maxNeuronsInLayerBox.Text = maxNeuronsInLayer.ToString();
        }

        // Load data
        private void loadDataButton_Click(object sender, System.EventArgs e)
        {
            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;
                // min and max X values
                float minX = float.MaxValue;
                float maxX = float.MinValue;

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
                    colCountData = line.Split(';').Length;

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
                    int i = 0;
                    // classes count
                    classesCount = 0;
                    samplesPerClass = new int[10];

                    // read the data
                    while ((i < rowCountData) && ((line = reader.ReadLine()) != null))
                    {
                        string[] strs = line.Split(';');
                        // parse input and output values for learning
                        //gather all input by cols
                        for (int j = 0; j < colCountData - 1; j++)
                        {
                            tempData[i, j] = double.Parse(strs[j]);
                        }

                        tempClasses[i] = int.Parse(strs[colCountData - 1]);

                        // skip classes over 10, except only first 10 classes
                        if (tempClasses[i] >= 10)
                            continue;

                        // count the amount of different classes
                        if (tempClasses[i] >= classesCount)
                            classesCount = tempClasses[i] + 1;
                        // count samples per class
                        samplesPerClass[tempClasses[i]]++;

                        // search for min value
                        if (tempData[i, 0] < minX)
                            minX = (float)tempData[i, 0];
                        // search for max value
                        if (tempData[i, 0] > maxX)
                            maxX = (float)tempData[i, 0];

                        i++;
                    }

                    // allocate and set data
                    data = new double[i, colCountData];
                    Array.Copy(tempData, 0, data, 0, i * colCountData);
                    classes = new int[i];
                    this.classesBox.Text = classesCount.ToString();
                    Array.Copy(tempClasses, 0, classes, 0, i);

                    inputCountBox.Invoke(new Action(() => inputCountBox.Text = (colCountData - 1).ToString()));
                    fileTextBox.Invoke(new Action(() => fileTextBox.Text = openFileDialog.SafeFileName.ToString()));

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
                
                // enable "Start" button
                startButton.Enabled = true;
                chart.RangeX = new Range(minX, maxX);
                
                // remove all previous data series from chart control
                chart.RemoveAllDataSeries();
                //input > 3 = 3d graph
                if (colCountData-1 <= 2)
                    ShowTrainingData();
            }
        }

        // Show training data on chart
        private void ShowTrainingData()
        {
            double[][,] dataSeries = new double[classesCount][,];
            int[] indexes = new int[classesCount];

            // allocate data arrays
            for (int i = 0; i < classesCount; i++)
            {
                dataSeries[i] = new double[samplesPerClass[i], 2];
            }

            // fill data arrays
            for (int i = 0; i < rowCountData; i++)
            {
                // get sample's class
                int dataClass = classes[i];
                // copy data into appropriate array
                dataSeries[dataClass][indexes[dataClass], 0] = data[i, 0];
                dataSeries[dataClass][indexes[dataClass], 1] = data[i, 1];
                indexes[dataClass]++;
            }

            // remove all previous data series from chart control
            chart.RemoveAllDataSeries();

            // add new data series
            for (int i = 0; i < classesCount; i++)
            {
                string className = string.Format("class" + i);

                // add data series
                chart.AddDataSeries(className, dataSereisColors[i], AForge.Controls.Chart.SeriesType.Dots, 5);
                chart.UpdateDataSeries(className, dataSeries[i]);
                // add classifier
                chart.AddDataSeries(string.Format("classifier" + i), Color.Gray, AForge.Controls.Chart.SeriesType.Line, 1, false);
            }
        }

        // Update data in list view
        /*private void UpdateDataListView()
        {
        }*/

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            alphaBox.Invoke(new Action(() => alphaBox.Enabled = enable));

            startButton.Invoke(new Action(() => startButton.Enabled = enable));

            stopButton.Invoke(new Action(() => stopButton.Enabled = !enable));

            validationLevelBox.Invoke(new Action(() => validationLevelBox.Enabled = enable));

            maxIterationsBox.Invoke(new Action(() => maxIterationsBox.Enabled = enable));

            maxNeuronsInLayerBox.Invoke(new Action(() => maxNeuronsInLayerBox.Enabled = enable));

        }

        // On button "Start"
        private void startButton_Click(object sender, System.EventArgs e)
        {
            // get learning rate
            try
            {
                validLevel = Math.Max(0.00001, Math.Min(1, double.Parse(validationLevelBox.Text)));
            }
            catch
            {
                validLevel = 0.2;
            }
            // get learning rate
            try
            {
                maxIterations = Math.Max(500, Math.Min(10000, int.Parse(maxIterationsBox.Text)));
            }
            catch
            {
                maxIterations = 1000;
            }
            // get learning rate
            try
            {
                maxNeuronsInLayer = Math.Max(3, Math.Min(15, int.Parse(maxNeuronsInLayerBox.Text)));
            }
            catch
            {
                maxNeuronsInLayer = 5;
            }
            // get learning rate
            try
            {
                learningRate = Math.Max(0.00001, Math.Min(1, double.Parse(learningRateBox.Text)));
            }
            catch
            {
                learningRate = 0.1;
            }
            // get momentum
            try
            {
                moment = Math.Max(0, Math.Min(0.5, double.Parse(momentBox.Text)));
            }
            catch
            {
                moment = 0;
            }
            // get alpha
            try
            {
                alpha = Math.Max(0, Math.Min(2.0, double.Parse(alphaBox.Text)));
            }
            catch
            {
                alpha = 1.0;
            }
            // get neurons count in first layer
            try
            {
                String[] temp = neuronsBox.Text.Split(',');
                if (temp.Length < 1)
                    throw new Exception();
                neuronsAndLayers = new int[temp.Length+1];
                for (int i = 0; i < temp.Length; i++)
                {
                    neuronsAndLayers[i] = Math.Max(1, Math.Min(50, int.Parse(temp[i])));
                }

                neuronsAndLayers[temp.Length] = classesCount;
            }
            catch
            {
                neuronsAndLayers = new int[1];
                neuronsAndLayers[0] = classesCount;
               // neuronsAndLayers[1] = classesCount;
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

        private void recordNeuroNet()
        {
            String topology = network.InputsCount.ToString();

            for (int i = 0; i < network.Layers.Length; i++)
            {
                topology += "-" + network.Layers[i].Neurons.Length.ToString();
            }
            //String topology = network.InputsCount.ToString() + "-" + neuronsBox.Text + "-" + classesCount.ToString();
            String moment = "-";
            String learningRate = "-";

            if (this.selectedAlgo == "Backpropagation")
            {
                moment = this.moment.ToString();
                learningRate = this.learningRate.ToString();
            }

            this.lastRunsGridView.Invoke(
                            new Action<String, String, String, String, String, String, String, String>((iter, error, validError, algoritm, topologyNet, alpha, learnRate, momentum) =>
                                lastRunsGridView.Rows.Add(iter, error, validError, algoritm, topologyNet, alpha, learnRate, momentum)),
                                this.iterations.ToString(), this.error.ToString(), (this.validateError / rowCountData).ToString(),
                                this.selectedAlgo, topology, this.alpha.ToString(), learningRate, moment);

           /* this.lastRunsGridView.Rows.Add(this.iterations.ToString(), this.error.ToString(), (this.validateError / rowCountData).ToString(),
                algoritmBox.SelectedItem.ToString(), topology, this.alpha.ToString(), learningRate, moment
                    );*/
        }

        // Worker thread
        void SearchSolution()
        {
            // number of learning samples
            int samples = data.GetLength(0);

            // prepare learning data
            //80% training, 20% for validate data(to do 70% lear., 20% validate, 10% test)
            double[][] input = new double[samples][];
            double[][] output = new double[samples][];
            //double[][] validateInput = new double[samples / 5][];
            //double[][] validateOutput = new double[samples / 5][];

            // create multi-layer neural network

            //int K = 0;
            //int J = 0;

            for (int i = 0; i < samples; i++)
            {
                //80% training, 20% for validate data(to do 70% lear., 20% validate, 10% test)
                /*if ((i % 5) == 0) // validate input 20 %
                {                               
                    validateInput[K] = new double[colCountData-1];

                    for (int c = 0; c < colCountData - 1; c++)
                    {
                        validateInput[K][c] = data[i, c];
                    }

                    validateOutput[K] = new double[classesCount];
                    validateOutput[K][classes[K]] = 1;
                    K++;
                }
                else //forward input 80 %
                {*/
                // input data
                input[i] = new double[colCountData - 1];

                for (int c = 0; c < colCountData - 1; c++)
                {
                    input[i][c] = data[i, c];
                }

                //output data
                output[i] = new double[classesCount];
                output[i][classes[i]] = 1;

                // J++;
                //}
            }

            if (this.alphaBox.Text != "")
                activationFunc = new SigmoidFunction(Double.Parse(this.alphaBox.Text));
            else
                activationFunc = new SigmoidFunction();

            network = new ActivationNetwork(activationFunc,
            colCountData - 1, neuronsAndLayers);
            ActivationLayer layer = network.Layers[0] as ActivationLayer;

            NguyenWidrow initializer = new NguyenWidrow(network);
            initializer.Randomize();
            // create teacher
            if (this.selectedAlgo == "Parallel Resilient Backpropagation")
                teacher = new ParallelResilientBackpropagationLearning(network);
            else if (this.selectedAlgo == "Backpropagation")
            {
                BackPropagationLearning backProp = new BackPropagationLearning(network);
                backProp.LearningRate = this.learningRate;
                backProp.Momentum = this.moment;
                teacher = backProp;
            }

            this.updateWeightsGrid();

            // iterations
            this.iterations = 1;
            this.error = 0.0;
            this.validateError = 0.0;
            // erros list
            RollingPointPairList listError = new RollingPointPairList(250);
            RollingPointPairList listValidate = new RollingPointPairList(250);

            zedGraphControl1.GraphPane.CurveList.Clear();
            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.Title.Text = "Обучение нейронной сети";
            myPane.XAxis.Title.Text = "Итерации";
            myPane.YAxis.Title.Text = "Изменение ошибки обучения";
            // Initially, a curve is added with no data points (list is empty)
            // Color is blue, and there will be no symbols
            LineItem curve = myPane.AddCurve("Сумма квадратов ошибок", listError, Color.Blue, SymbolType.None);
            LineItem curve2 = myPane.AddCurve("Валидация", listValidate, Color.Red, SymbolType.None);

            myPane.XAxis.Scale.MajorStep = 10;

            // Scale the axes
            zedGraphControl1.AxisChange();

            // loop
            while (!needToStop)
            {
                if ((iterations >= maxIterations) && (validateError > validLevel))
                {
                    int potentialLayer = -1;

                    //search free layer for add neuron
                    for (int hiddenLayer = 0; hiddenLayer < network.Layers.Length-1; hiddenLayer++)
                    {
                        if (network.Layers[hiddenLayer].Neurons.Length < maxNeuronsInLayer)
                        {
                            potentialLayer = hiddenLayer;
                            break;
                        }
                    }
                    //layers full, add new free layer
                    if (potentialLayer == -1)
                    {
                        int[] tempNet = new int[network.Layers.Length+1];
                        int i = 0;
                        for (i = 0; i < network.Layers.Length-1; i++)
                        {
                            tempNet[i] = network.Layers[i].Neurons.Length;
                        }
                        tempNet[i] = 1; // new layer before last layer
                        tempNet[i+1] = network.Output.Length; // last layer in end

                        network = new ActivationNetwork(activationFunc, colCountData - 1, tempNet);
                        initializer = new NguyenWidrow(network);
                        initializer.Randomize();
                        if (this.selectedAlgo == "Parallel Resilient Backpropagation")
                            teacher = new ParallelResilientBackpropagationLearning(network);
                        else if (this.selectedAlgo == "Backpropagation")
                        {
                            BackPropagationLearning backProp = new BackPropagationLearning(network);
                            backProp.LearningRate = this.learningRate;
                            backProp.Momentum = this.moment;
                            teacher = backProp;
                        }
                        updateWeightsGrid();

                    }
                    else if (potentialLayer != -1)
                    {
                        int[] tempNet = new int[network.Layers.Length];
                        int i = 0;
                        int newCntNeurons = 0;
                        for (i = 0; i < network.Layers.Length; i++)
                        {
                            if (i == potentialLayer)
                            {
                                newCntNeurons = network.Layers[i].Neurons.Length + 1;
                                tempNet[i] = newCntNeurons;
                                //break;
                            }
                            else 
                            {
                                tempNet[i] = network.Layers[i].Neurons.Length;
                            }
                            
                        }
                        //tempNet[i] = network.Output.Length; // last layer in end
                        network = new ActivationNetwork(activationFunc, colCountData - 1, tempNet);
                        initializer = new NguyenWidrow(network);
                        initializer.Randomize();
                        if (this.selectedAlgo == "Parallel Resilient Backpropagation")
                            teacher = new ParallelResilientBackpropagationLearning(network);
                        else if (this.selectedAlgo == "Backpropagation")
                        {
                            BackPropagationLearning backProp = new BackPropagationLearning(network);
                            backProp.LearningRate = this.learningRate;
                            backProp.Momentum = this.moment;
                            teacher = backProp;
                        }
                        updateWeightsGrid();
                    }
                    recordNeuroNet();
                    iterations = 1;
                }
                // run epoch of learning procedure
                error = teacher.RunEpoch(input, output);
                validateError = 0.0;
                for (int count = 0; count < input.GetLength(0) - 1; count++)
                {
                    validateError += Math.Abs(network.Compute(input[count])[classes[count]] - output[count][classes[count]]);
                }

                int Rows = 0;
                for (int i = 0; i < network.Layers.Length; i++)
                {
                    for (int neurons = 0; neurons < network.Layers[i].Neurons.Length; neurons++)
                    {
                        for (int weights = 0; weights < network.Layers[i].Neurons[neurons].Weights.Length; weights++)
                        {

                            /* if (Math.Abs(network.Layers[i].Neurons[neurons].Weights[weights]) >= 50.0 * learningRate)
                             {
                                 network.Layers[i].Neurons[neurons].Weights[weights] = network.Layers[i].Neurons[neurons].Weights[weights] / network.Layers[i].Neurons.Length;
                                 // RowsRisk[Rows]++;

                                 this.dataGridViewWeights.Invoke(
                                 new Action<int>((row) =>
                                     dataGridViewWeights.Rows[row].Cells[3].Style.BackColor = Color.Red), Rows);
                             }*/

                            this.dataGridViewWeights.Invoke(
                                new Action<double>((weight) =>
                                    dataGridViewWeights.Rows[Rows].Cells[3].Value = weight), network.Layers[i].Neurons[neurons].Weights[weights]);



                            Rows++;
                        }
                    }
                }

                // Make sure that the curvelist has at least one curve
                if (zedGraphControl1.GraphPane.CurveList.Count <= 0)
                    return;

                // Get the first CurveItem in the graph
                curve = zedGraphControl1.GraphPane.CurveList[0] as LineItem;
                curve2 = zedGraphControl1.GraphPane.CurveList[1] as LineItem;
                if (curve == null)
                    return;
                if (curve2 == null)
                    return;

                // Get the PointPairList
                IPointListEdit listErrors = curve.Points as IPointListEdit;
                IPointListEdit listValidates = curve2.Points as IPointListEdit;
                // If this is null, it means the reference at curve.Points does not
                // support IPointListEdit, so we won't be able to modify it
                if (listErrors == null)
                    return;
                if (listValidates == null)
                    return;
                listErrors.Add(this.iterations, error);
                listValidates.Add(this.iterations, validateError / samples);

                // Make sure the Y axis is rescaled to accommodate actual data
                zedGraphControl1.AxisChange();
                // Force a redraw
                zedGraphControl1.Invalidate();


                // set current iteration's info
                currentIterationBox.Invoke(new Action<string>((s) => currentIterationBox.Text = s), this.iterations.ToString());
                errorPercent.Invoke(new Action<string>((s) => errorPercent.Text = s), error.ToString("F14"));
                validErrorBox.Invoke(new Action<string>((s) => validErrorBox.Text = s), (validateError / samples).ToString("F14"));

                // show classifiers
                /*if (colCountData - 1 <= 2)
                {
                    for (int j = 0; j < classesCount; j++)
                    {
                        double k = (layer.Neurons[j].Weights[1] != 0) ? (-layer.Neurons[j].Weights[0] / layer.Neurons[j].Weights[1]) : 0;
                        double b = (layer.Neurons[j].Weights[1] != 0) ? (-((ActivationNeuron)layer.Neurons[j]).Threshold / layer.Neurons[j].Weights[1]) : 0;

                        double[,] classifier = new double[2, 2] {
							{ chart.RangeX.Min, chart.RangeX.Min * k + b },
							{ chart.RangeX.Max, chart.RangeX.Max * k + b }
																};

                        // update chart
                        chart.UpdateDataSeries(string.Format("classifier" + j), classifier);
                    }
                }*/

                // increase current iteration
                this.iterations++;
            }
            // enable settings controls
            EnableControls(true);

        }

        /**
         * init grid of weights
         * */
        private void updateWeightsGrid()
        {
            //set weights grid
            this.dataGridViewWeights.Invoke(new Action(() => dataGridViewWeights.Rows.Clear()));
            //TO DO incapsulate to function
            for (int i = 0; i < network.Layers.Length; i++)
            {
                for (int neurons = 0; neurons < network.Layers[i].Neurons.Length; neurons++)
                {
                    for (int weights = 0; weights < network.Layers[i].Neurons[neurons].Weights.Length; weights++)
                    {
                        this.dataGridViewWeights.Invoke(
                            new Action<int, int, int>((i1, i2, i3) =>
                                dataGridViewWeights.Rows.Add(i1.ToString(), i2.ToString(), i3.ToString(), network.Layers[i1].Neurons[i2].Weights[i3].ToString())), i, neurons, weights);
                    }
                }
            }
        }

        /**
        * Сохранение нейронной сети по указанному пути
        * */
        private void SaveNetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "bin files (*.bin)|*.bin";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK
                    && saveFileDialog1.FileName.Length > 0)
            {

                network.Save(saveFileDialog1.FileName);
                MessageBox.Show("Сеть сохранена");
            }
        }

        private void TestNetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String[] lines = new String[1];
            double[] res = new double[classesCount];
		    double[] input = new double[colCountData - 1];

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\SpreadSheetTest.csv"))
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    //gather inputs for compute, n-1 inputs
                    for (int k = 0; k < colCountData - 1; k++)
                    {
                        input[k] = data[i, k];
                    }

                    res = network.Compute(input);
                    int j = 0;
                    for (j = 0; j < classesCount; j++)
                    {
                        if (res[j] == 1)
                            break;
                    }
                    lines[0] = classes[i].ToString() + ";" + j.ToString("F8");
                    file.WriteLine(lines[0].ToString());
                }
            MessageBox.Show("Тестирование пройдено");
        }

        private void ViewTopologyNetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Thread(() => Application.Run(new FormDrawNeurons())).Start();
        }

        private void SaveWeightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Weights.csv"))
            for (int i = 0; i < network.Layers.Length; i++)
            {
                for (int j = 0; j < network.Layers[i].Neurons.Length; j++)
                {
                    for (int k = 0; k < network.Layers[i].Neurons[j].Weights.Length; k++)
                    {
                        
                        file.WriteLine("L[" + i.ToString() + "]N[" + j.ToString() + "]W[" + k.ToString() + "] = " + network.Layers[i].Neurons[j].Weights[k]);
                        
                    }
                }
            }
            MessageBox.Show("Веса сохранены");
        }

        private void algoritmBox_SelectedValueChanged(object sender, EventArgs e)
        {
            selectedAlgo = algoritmBox.SelectedItem.ToString();
            if (this.selectedAlgo == "Parallel Resilient Backpropagation")
            {
                this.momentBox.Enabled = false;
                this.momentBox.ReadOnly = true;
                this.learningRateBox.Enabled = false;
                this.learningRateBox.ReadOnly = true;
            }
            else if (this.selectedAlgo == "Backpropagation")
            {
                this.momentBox.Enabled = true;
                this.momentBox.ReadOnly = false;
                this.learningRateBox.Enabled = true;
                this.learningRateBox.ReadOnly = false;
            }
            
        }


    }
}
