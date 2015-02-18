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
using Accord.Neuro;
using Accord.Math;
using Accord.Neuro.Learning;
using AForge.Neuro.Learning;
using Accord;
using ZedGraph;


using Accord.IO;

using Accord.Statistics.Analysis;


namespace Neural
{
    public partial class LearnNetForm : Form
    {
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
        List<int> classesList = new List<int>();
        private int[] samplesPerClass;
        private double validLevel = 0.2;
        private int maxIterations = 500;
        private int maxNeuronsInLayer = 10;

        private Thread workerThread = null;
        private bool needToStop = false;
        ActivationNetwork network;
        IActivationFunction activationFunc = null;
        ISupervisedLearning teacher = null;

        // Constructor
        public LearnNetForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            RollingPointPairList listError = new RollingPointPairList(300);
            RollingPointPairList listValidate = new RollingPointPairList(300);
            GraphPane myPane = zedGraphControl1.GraphPane;
            LineItem curve = myPane.AddCurve("Качество обучения", listError, Color.Blue, SymbolType.None);
            LineItem curve2 = myPane.AddCurve("Кросс-валидация", listValidate, Color.Green, SymbolType.None);
            myPane.Title.Text = "Обучение нейронной сети";
            myPane.XAxis.Title.Text = "Итерации";
            myPane.YAxis.Title.Text = "Изменение ошибки обучения";
            myPane.XAxis.Scale.MajorStep = 10;
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

        //load data for classification
        private void getDataForClass()
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

                        if (strs.Length-1 < colCountData - 1)
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

        //load data for regression
        private void getDataForregression()
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
                    colCountData = line.Split(' ').Length;

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
                        string[] strs = line.Split(' ');
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
            if (this.selectedTypeLearn == "classification")
            {
                this.getDataForClass();
            }
            else if (this.selectedTypeLearn == "regression")
            {
                this.getDataForregression();
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
                maxNeuronsInLayer = Math.Max(3, Math.Min(30, int.Parse(maxNeuronsInLayerBox.Text)));
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
                moment = Math.Max(0, Math.Min(1.0, double.Parse(momentBox.Text)));
            }
            catch
            {
                moment = 0.9;
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
                neuronsAndLayers = new int[temp.Length+1];
                for (int i = 0; i < temp.Length; i++)
                {
                    neuronsAndLayers[i] = Math.Max(1, Math.Min(200, int.Parse(temp[i])));
                }
                if (this.selectedTypeLearn == "classification")
                    neuronsAndLayers[temp.Length] = classesList.Count;
                else if (this.selectedTypeLearn == "regression")
                    neuronsAndLayers[temp.Length] = 1;
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
            String topology = "";

            for (int i = 0; i < network.Layers.Length; i++)
            {
                topology += network.Layers[i].InputsCount + "-";
            }

            topology += network.Layers[network.Layers.Length-1].Neurons.Length;
            String moment = this.moment.ToString();
            String learningRate = this.learningRate.ToString();

            this.lastRunsGridView.Invoke(
                            new Action(() =>
                                lastRunsGridView.Rows.Add(this.iterations.ToString(), this.error.ToString(), (this.validateError).ToString(),
                                this.selectedAlgo, topology, this.alpha.ToString(), learningRate, moment)
                                ));
        }

        private void createLearn(int[] topology)
        {
            if (this.alphaBox.Text != "")
                activationFunc = new SigmoidFunction(Double.Parse(this.alphaBox.Text));
            else
                activationFunc = new SigmoidFunction();

            network = new ActivationNetwork(activationFunc,
            colCountData - 1, topology);
            //ActivationLayer layer = network.Layers[0] as ActivationLayer;

            NguyenWidrow initializer = new NguyenWidrow(network);
            initializer.Randomize();
            // create teacher
            if (this.selectedAlgo == "RProp")
                teacher = new ParallelResilientBackpropagationLearning(network);
            else if (this.selectedAlgo == "BackProp")
            {
                BackPropagationLearning backProp = new BackPropagationLearning(network);
                backProp.LearningRate = this.learningRate;
                backProp.Momentum = this.moment;
                teacher = backProp;
            }
        }

        // Worker thread
        void SearchSolution()
        {
            if (this.selectedTypeLearn == "classification")
            {
                // number of learning samples
                samples = data.GetLength(0);

                // prepare learning data
                //80% training, 20% for validate data
                this.input = new double[(samples * 4) / 5][];
                this.output = new double[(samples  * 4) /5][];
                trainClasses = new int[(samples * 4) / 5];

                validateInput = new double[samples / 5][];
                validateOutput = new double[samples / 5][];
                validClasses = new int[samples / 5];

                // create multi-layer neural network

                int K = 0;
                int J = 0;

                for (int i = 1; i < samples; i++)
                {
                    //80% training, 20% for validate data
                    if ((i % 5) == 0) // validate input 20 %
                    {                               
                        validateInput[K] = new double[colCountData-1];

                        for (int c = 0; c < colCountData - 1; c++)
                        {
                            validateInput[K][c] = data[i, c];
                        }

                        validateOutput[K] = new double[classesList.Count];
                        validateOutput[K][classesList.IndexOf(classes[i])] = 1;
                        validClasses[K] = classes[i];
                        K++;
                    }
                    else //forward input 80 %
                    {
                        // input data
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
            else if (this.selectedTypeLearn == "regression")
            {
                // number of learning samples
                samples = data.GetLength(0);

                // prepare learning data
                //80% training, 20% for validate data
                this.input = new double[samples * 4 / 5][];
                this.output = new double[samples * 4 / 5][];
                this.validateInput = new double[samples / 5][];
                this.validateOutput = new double[samples / 5][];

                // create multi-layer neural network

                int k = 0;
                int j = 0;

                for (int i = 1; i < samples; i++)
                {
                    //80% training, 20% for validate data
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
            createLearn(neuronsAndLayers);
            // iterations
            this.iterations = 1;
            this.error = 0.0;
            this.validateError = 0.0;

            zedGraphControl1.GraphPane.CurveList[0].Clear();
            zedGraphControl1.GraphPane.CurveList[1].Clear();
            zedGraphControl1.AxisChange();

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
                        createLearn(tempNet);
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
                        createLearn(tempNet);
                    }


                    zedGraphControl1.Invoke(new Action(() => zedGraphControl1.GraphPane.CurveList[0].Clear()));
                    zedGraphControl1.Invoke(new Action(() => zedGraphControl1.GraphPane.CurveList[1].Clear()));
                    iterations = 1;
                }
                // run epoch of learning procedure
                error = 1 - ( teacher.RunEpoch( this.input, this.output )/this.input.GetLength(0) );
                validateError = 0.0;

                if (this.selectedTypeLearn == "classification")
                {
                    this.validation();
                }
                else if (this.selectedTypeLearn == "regression")
                {
                    for (int count = 0; count < validateInput.GetLength(0) - 1; count++)
                    {
                        validateError += Math.Abs(network.Compute(validateInput[count])[0] - validateOutput[count][0]);
                    }
                    validateError = validateError / validateInput.GetLength(0);
                }

                // Make sure that the curvelist has at least one curve
                if (zedGraphControl1.GraphPane.CurveList.Count <= 0)
                    return;

                // Get the first CurveItem in the graph
                CurveItem curve = zedGraphControl1.GraphPane.CurveList[0] as LineItem;
                CurveItem curve2 = zedGraphControl1.GraphPane.CurveList[1] as LineItem;
                if (curve == null)
                    return;
                if (curve2 == null)
                    return;

                // Get the PointPairList
                IPointListEdit listErrors = curve.Points as IPointListEdit;
                IPointListEdit listValidates = curve2.Points as IPointListEdit;

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
                validErrorBox.Invoke(new Action<string>((s) => validErrorBox.Text = s), validateError.ToString("F14"));


                // increase current iteration
                this.iterations++;
            }
            // enable settings controls
            EnableControls(true);

        }

        private void validation()
        {
            int problems = 0;
            double[] res;
            for (int count = 0; count < validateInput.Length - 1; count++)
            {
                res = network.Compute(validateInput[count]);
                double value = Math.Abs( 1 - res[classesList.IndexOf(validClasses[count])] );
                if (value > 0.0001)
                {
                    validateError += value;
                    problems++;
                }
            }
            validateError = 1 - ( validateError / validateInput.Length );
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
            double[] res = new double[classesList.Count];
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
                    if (this.selectedTypeLearn == "classification")
                    {
                        int classificator = this.max(res);
                        lines[0] = classes[i].ToString() + ";" + classificator.ToString("F8");
                        
                    }
                    else if (this.selectedTypeLearn == "regression")
                        lines[0] = data[i,colCountData-1].ToString() + ";" + res.ToString("F8");

                    file.WriteLine(lines[0].ToString());
                }
                MessageBox.Show("Тестирование пройдено");

        }

        private void crossValid()
        {
            String[] lines = new String[1];
            double[] res = new double[classesList.Count];
            double[] input = new double[colCountData - 1];

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\SpreadSheetValid.csv"))

                for (int i = 0; i < validateInput.GetLength(0)-1; i++)
                {
                    //gather inputs for compute, n-1 inputs
                    for (int k = 0; k < colCountData - 1; k++)
                    {
                        input[k] = validateInput[i][k];
                    }

                    res = network.Compute(input);

                    int classificator = this.max(res);
                    lines[0] = validClasses[i].ToString() + ";" + classificator.ToString("F8");

                    file.WriteLine(lines[0].ToString());
                }
            MessageBox.Show("Кросс-валидация пройдена.");
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
            if (this.selectedAlgo == "RProp")
            {
                this.momentBox.Enabled = false;
                this.momentBox.ReadOnly = true;
                this.learningRateBox.Enabled = false;
                this.learningRateBox.ReadOnly = true;
            }
            else if (this.selectedAlgo == "BackProp")
            {
                this.momentBox.Enabled = true;
                this.momentBox.ReadOnly = false;
                this.learningRateBox.Enabled = true;
                this.learningRateBox.ReadOnly = false;
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HelloForm form = this.Owner as HelloForm;
            this.selectedTypeLearn = form.typeLearn;
        }

        private void crossValidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.crossValid();
        }
    }
}
