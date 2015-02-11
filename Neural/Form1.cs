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


using Accord.IO;

using Accord.Statistics.Analysis;


namespace Neural
{
    public partial class Form1 : Form
    {
        int samples = 0;
        double[][] input;
        double[][] output;
        double[][] validateInput;
        double[][] validateOutput;
        private double[,] data = null;
        int rowCountData = 0;
        int colCountData = 0;

        private double learningRate = 0.5;
        private double alpha = 2.0;
        private double moment = 0.3;
        private int iterations = 0;
        private double error = 0.0;
        private double validateError = 0.0;
        private String selectedAlgo = "";
        private String selectedTypeLearn = "";
        private int[] neuronsAndLayers;
        private int[] classes;
        private int classesCount;
        private int[] samplesPerClass;
        private double validLevel = 0.2;
        private int maxIterations = 500;
        private int maxNeuronsInLayer = 10;
        private double minWeight = 50;

        private Thread workerThread = null;
        private bool needToStop = false;
        ActivationNetwork network;
        IActivationFunction activationFunc = null;
        ISupervisedLearning teacher = null;

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
            this.minWeightBox.Text = minWeight.ToString();
        }

        //load data for classification
        private void getDataForClass()
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
            }
        }

        //load data for regression
        private void getDataForRegression()
        {
            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;

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
            }

        }

        // Load data
        private void loadDataButton_Click(object sender, System.EventArgs e)
        {
            if (this.selectedTypeLearn == "Классификация")
            {
                this.getDataForClass();
            }
            else if (this.selectedTypeLearn == "Регрессия")
            {
                this.getDataForRegression();
            }
            
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            alphaBox.Invoke(new Action(() => alphaBox.Enabled = enable));

            startButton.Invoke(new Action(() => startButton.Enabled = enable));

            stopButton.Invoke(new Action(() => stopButton.Enabled = !enable));

            validationLevelBox.Invoke(new Action(() => validationLevelBox.Enabled = enable));

            maxIterationsBox.Invoke(new Action(() => maxIterationsBox.Enabled = enable));

            maxNeuronsInLayerBox.Invoke(new Action(() => maxNeuronsInLayerBox.Enabled = enable));

            minWeightBox.Invoke(new Action(() => minWeightBox.Enabled = enable));

        }

        // On button "Start"
        private void startButton_Click(object sender, System.EventArgs e)
        {

            // get learning rate
            try
            {
                minWeight = Math.Max(50.0, Math.Min(10000, double.Parse(minWeightBox.Text)));
            }
            catch
            {
                minWeight = 50.0;
            }
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
                learningRate = 0.5;
            }
            // get momentum
            try
            {
                moment = Math.Max(0, Math.Min(0.5, double.Parse(momentBox.Text)));
            }
            catch
            {
                moment = 0.3;
            }
            // get alpha
            try
            {
                alpha = Math.Max(0, Math.Min(10.0, double.Parse(alphaBox.Text)));
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
                neuronsAndLayers = new int[temp.Length+1];
                for (int i = 0; i < temp.Length; i++)
                {
                    neuronsAndLayers[i] = Math.Max(1, Math.Min(50, int.Parse(temp[i])));
                }
                if (this.selectedTypeLearn == "Классификация")
                    neuronsAndLayers[temp.Length] = classesCount;
                else if (this.selectedTypeLearn == "Регрессия")
                    neuronsAndLayers[temp.Length] = 1;
            }
            catch
            {
                neuronsAndLayers = new int[1];
                neuronsAndLayers[0] = classesCount;
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

        private int getCountOfRelations()
        {
            int count = 0;
            count = network.InputsCount * network.Layers[0].Neurons.Length;
            for (int i = 1; i < network.Layers.Length; i++)
            {
                count += network.Layers[i].Neurons.Length * network.Layers[i - 1].Neurons.Length;
            }
            return count;
        }

        /**
         * Запись результата очередного обучения в таблицу
         * */
        private void recordNeuroNet()
        {
            String topology = network.InputsCount.ToString();

            for (int i = 0; i < network.Layers.Length; i++)
            {
                topology += "-" + network.Layers[i].Neurons.Length.ToString();
            }
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
        }

        private void WeightsChart(double[] y)
        {
            zedGraphControl2.GraphPane.CurveList.Clear();
            // get a reference to the GraphPane
            GraphPane myPane = zedGraphControl2.GraphPane;

            // Generate a red bar with "Curve 1" in the legend
            // Get the first CurveItem in the graph
            CurveItem curve3 = myPane.AddCurve("Curve 1", null, y, Color.Blue);

            // Draw the X tics between the labels instead of at the labels
            myPane.XAxis.MajorTic.IsBetweenLabels = true;

            // Set the XAxis to the ordinal type
            myPane.XAxis.Type = AxisType.Ordinal;

            // Indicate that the bars are overlay type, which are drawn on top of eachother
            myPane.BarSettings.Type = BarType.Overlay;

            // Fill the axis background with a color gradient
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45.0F);

            // Calculate the Axis Scale Ranges
            this.zedGraphControl2.AxisChange();
            // Force a redraw
            this.zedGraphControl2.Invalidate();
        }

        // Worker thread
        void SearchSolution()
        {
            if (this.selectedTypeLearn == "Классификация")
            {
                // number of learning samples
                samples = data.GetLength(0);

                // prepare learning data
                //80% training, 20% for validate data(to do 70% lear., 20% validate, 10% test)
                this.input = new double[samples][];
                this.output = new double[samples][];
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
                    this.input[i] = new double[colCountData - 1];

                    for (int c = 0; c < colCountData - 1; c++)
                    {
                        this.input[i][c] = data[i, c];
                    }

                    //output data
                    this.output[i] = new double[classesCount];
                    this.output[i][classes[i]] = 1;

                    // J++;
                    //}
                }
            }
            else if (this.selectedTypeLearn == "Регрессия")
            {
                // number of learning samples
                samples = data.GetLength(0);

                // prepare learning data
                //80% training, 20% for validate data(to do 70% lear., 20% validate, 10% test)
                this.input = new double[samples * 4 / 5][];
                this.output = new double[samples * 4 / 5][];
                this.validateInput = new double[samples / 5][];
                this.validateOutput = new double[samples / 5][];

                // create multi-layer neural network

                int k = 0;
                int j = 0;

                for (int i = 1; i < samples; i++)
                {
                    //80% training, 20% for validate data(to do 70% lear., 20% validate, 10% test)
                    if ((i % 5) == 0) // validate input 20 %
                    {
                        this.validateInput[k] = new double[colCountData - 1];
                        this.validateOutput[k] = new double[1];

                        for (int c = 0; c < colCountData - 1; c++)
                        {
                            this.validateInput[k][c] = data[i, c];
                        }

                        this.validateOutput[k][0] = data[i, colCountData - 1];
                        k++;
                    }
                    else //forward input 80 %
                    {
                        // input data
                        this.input[j] = new double[colCountData - 1];

                        for (int c = 0; c < colCountData - 1; c++)
                        {
                            this.input[j][c] = data[i, c];
                        }

                        //output data

                        this.output[j] = new double[1];
                        this.output[j][0] = data[i, colCountData - 1];


                        j++;
                    }
                }
            }

            if (this.alphaBox.Text != "")
                activationFunc = new SigmoidFunction(Double.Parse(this.alphaBox.Text));
            else
                activationFunc = new SigmoidFunction();

            network = new ActivationNetwork(activationFunc,
            colCountData - 1, neuronsAndLayers);
            //ActivationLayer layer = network.Layers[0] as ActivationLayer;

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

            // erros list
            double[] weights1 = new double[this.getCountOfRelations()];

            zedGraphControl2.GraphPane.CurveList.Clear();
            GraphPane myPane2 = zedGraphControl2.GraphPane;
            myPane2.Title.Text = "Динамика весов при обучении";
            myPane2.XAxis.Title.Text = "Порядковый номер";
            myPane2.YAxis.Title.Text = "Показатель";
            // Initially, a curve is added with no data points (list is empty)
            // Color is blue, and there will be no symbols
            CurveItem curve3 = myPane2.AddCurve("Curve 1", null,  weights1, Color.Blue);
            myPane2.XAxis.Scale.MajorStep = 2;
            zedGraphControl2.AxisChange();

            // loop
            while (!needToStop)
            {
                if ((iterations >= maxIterations) && (validateError > validLevel))
                {
                    recordNeuroNet();
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
                            }
                            else 
                            {
                                tempNet[i] = network.Layers[i].Neurons.Length;
                            }
                            
                        }
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


                    zedGraphControl1.Invoke(new Action(() => zedGraphControl1.GraphPane.CurveList[0].Clear()));
                    zedGraphControl1.Invoke(new Action(() => zedGraphControl1.GraphPane.CurveList[1].Clear()));
                    iterations = 1;
                }
                // run epoch of learning procedure
                error = teacher.RunEpoch(this.input, this.output);
                validateError = 0.0;
                if (this.selectedTypeLearn == "Классификация")
                {
                    for (int count = 0; count < input.GetLength(0) - 1; count++)
                    {
                        validateError += Math.Abs(network.Compute(input[count])[classes[count]] - output[count][classes[count]]);
                    }
                    validateError = validateError / samples;
                }
                else if (this.selectedTypeLearn == "Регрессия")
                {
                    for (int count = 0; count < validateInput.GetLength(0) - 1; count++)
                    {
                        validateError += Math.Abs(network.Compute(validateInput[count])[0] - validateOutput[count][0]);
                    }
                    validateError = validateError / samples;
                }
                

                int Rows = 0;
                double[] weightsBar = new double[this.getCountOfRelations()];
                for (int i = 0; i < network.Layers.Length; i++)
                {
                    for (int neurons = 0; neurons < network.Layers[i].Neurons.Length; neurons++)
                    {
                        for (int weights = 0; weights < network.Layers[i].Neurons[neurons].Weights.Length; weights++)
                        {
                            weightsBar[Rows] = network.Layers[i].Neurons[neurons].Weights[weights];
                             if (Math.Abs(network.Layers[i].Neurons[neurons].Weights[weights]) >= this.minWeight)
                             {
                                 network.Layers[i].Neurons[neurons].Weights[weights] = network.Layers[i].Neurons[neurons].Weights[weights] / network.Layers[i].Neurons.Length;

                                 this.dataGridViewWeights.Invoke(
                                 new Action<int>((row) =>
                                     dataGridViewWeights.Rows[row].Cells[3].Style.BackColor = Color.Red), Rows);
                             }

                            this.dataGridViewWeights.Invoke(
                                new Action<double>((weight) =>
                                    dataGridViewWeights.Rows[Rows].Cells[3].Value = weight), network.Layers[i].Neurons[neurons].Weights[weights]);



                            Rows++;
                        }
                    }
                }
                this.WeightsChart(weightsBar);

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
                listValidates.Add(this.iterations, validateError);

                // Make sure the Y axis is rescaled to accommodate actual data
                zedGraphControl1.AxisChange();
                // Force a redraw
                zedGraphControl1.Invalidate();


                // set current iteration's info
                currentIterationBox.Invoke(new Action<string>((s) => currentIterationBox.Text = s), this.iterations.ToString());
                errorPercent.Invoke(new Action<string>((s) => errorPercent.Text = s), error.ToString("F14"));
                validErrorBox.Invoke(new Action<string>((s) => validErrorBox.Text = s), (validateError).ToString("F14"));


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

            using(System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\SpreadSheetTest.csv"))
            //file.WriteLine("Желаемый ответ; Результат");
                
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    //gather inputs for compute, n-1 inputs
                    for (int k = 0; k < colCountData - 1; k++)
                    {
                        input[k] = data[i, k];
                    } 

                    res = network.Compute(input);
                    if (this.selectedTypeLearn == "Классификация")
                    {
                        int j = 0;
                        for (j = 0; j < classesCount; j++)
                        {
                            if (Math.Abs(res[j] - 1) < 0.00001)
                                break;
                        }
                        lines[0] = classes[i].ToString() + ";" + j.ToString("F8");
                    }
                    else if (this.selectedTypeLearn == "Регрессия")
                        lines[0] = data[i,colCountData-1].ToString() + ";" + res.ToString("F8");

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

        private void typeLearnBox_SelectedValueChanged(object sender, EventArgs e)
        {
            this.loadDataButton.Enabled = true;
            this.selectedTypeLearn = typeLearnBox.SelectedItem.ToString();
        }
    }
}
