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


namespace Neural
{
    public partial class Form1 : Form
    {
        private double[,] data = null;
        int rowCountData = 0;
        int colCountData = 0;

        private double learningRate = 0.1;
        private int[] classes;
        private int classesCount;
        private int[] samplesPerClass;

        private Thread workerThread = null;
        private bool needToStop = false;
        ActivationNetwork network;
        IActivationFunction activationFunc = new ThresholdFunction();
        PerceptronLearning teacherPerc = null;

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
            errorChart.AddDataSeries("error", Color.Red, Chart.SeriesType.ConnectedDots, 3);
            errorChart.AddDataSeries("validate", Color.Blue, Chart.SeriesType.ConnectedDots, 2);
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
            this.learningRateBox.Text = learningRate.ToString();
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
                
                // update list and chart
                UpdateDataListView();
                // enable "Start" button
                startButton.Enabled = true;
                chart.RangeX = new Range(minX, maxX);
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
                chart.AddDataSeries(className, dataSereisColors[i], Chart.SeriesType.Dots, 5);
                chart.UpdateDataSeries(className, dataSeries[i]);
                // add classifier
                chart.AddDataSeries(string.Format("classifier" + i), Color.Gray, Chart.SeriesType.Line, 1, false);
            }
        }

        // Update data in list view
        private void UpdateDataListView()
        {
            // remove all current records
            this.dataGridView1.Rows.Clear();
            int colCountGrid = this.dataGridView1.Columns.Count;
            for (int c = 0; c < colCountGrid; c++)
            {
                this.dataGridView1.Columns.Remove(c.ToString());
            }

            // add new records
            //add columns to grid
            int k = 0;
            for (k = 0; k < colCountData-1; k++)
            {
                this.dataGridView1.Columns.Add(k.ToString(), "Input: " + k.ToString());
            }
            this.dataGridView1.Columns.Add(k.ToString(), "Class");

            //add rows and values
            for (int i = 0; i < rowCountData; i++)
            {
                dataGridView1.Rows.Add();
                for (int j = 0; j < colCountData-1; j++)
                {   
                        this.dataGridView1.Rows[i].Cells[j].Value = data[i, j];
                }
                this.dataGridView1.Rows[i].Cells[colCountData-1].Value = classes[i];
            }
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            loadDataButton.Invoke(new Action(() => loadDataButton.Enabled = enable));
            
            learningRateBox.Invoke(new Action(() => learningRateBox.Enabled = enable));

            startButton.Invoke(new Action(() => startButton.Enabled = enable));

            stopButton.Invoke(new Action(() => stopButton.Enabled = !enable));

        }

        // On button "Start"
        private void startButton_Click(object sender, System.EventArgs e)
        {
            // get learning rate
            try
            {
                learningRate = Math.Max(0.00001, Math.Min(1, double.Parse(learningRateBox.Text)));
            }
            catch
            {
                learningRate = 0.1;
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
                    input[i] = new double[colCountData-1];

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

            network = new ActivationNetwork(activationFunc,
            colCountData-1, classesCount);
            ActivationLayer layer = network.Layers[0] as ActivationLayer;
            // create teacher
            teacherPerc = new PerceptronLearning(network);
            // set learning rate
            teacherPerc.LearningRate = learningRate;

            // iterations
            int iteration = 1;
            double error = 0.0;
            double[] validateError = new double[1]{0.0};
            //int j = 0;
            // erros list
            ArrayList errorsList = new ArrayList();
            ArrayList validateList = new ArrayList();
            // loop
            while (!needToStop)
            {

                    // run epoch of learning procedure
                        error = teacherPerc.RunEpoch(input, output);
                        if (errorsList.Count - 1 >= 1000)
                        {
                            errorsList.RemoveAt(0);
                            
                        }
                        errorsList.Add(error);
                        
                    validateError[0] = 0.0;
                    for (int count = 0; count < input.GetLength(0)-1; count++)
                    {
                        validateError[0] += Math.Abs(network.Compute(input[count])[classes[count]] - output[count][classes[count]]);
                    }
                    if (validateList.Count - 1 >= 1000)
                    {
                        validateList.RemoveAt(0);

                    }
                    validateList.Add(validateError[0]);

                        // set current iteration's info
                    currentIterationBox.Invoke(new Action<string>((s) => currentIterationBox.Text = s), iteration.ToString());
                    errorPercent.Invoke(new Action<string>((s) => errorPercent.Text = s), error.ToString("F14"));
                    validErrorBox.Invoke(new Action<string>((s) => validErrorBox.Text = s), (validateError[0]/samples).ToString("F14"));
                    // show error's dynamics
                    double[,] errors = new double[errorsList.Count, 2];
                    double[,] valid = new double[validateList.Count, 2];

                    for (int i = 0, n = errorsList.Count; i < n; i++)
                    {
                        errors[i, 0] = i;
                        errors[i, 1] = (double)errorsList[i];
                    }

                    for (int i = 0, n = validateList.Count; i < n; i++)
                    {
                        valid[i, 0] = i;
                        valid[i, 1] = (double)validateList[i];
                    }

                    errorChart.RangeX = new Range(1, errorsList.Count - 1);
                    errorChart.UpdateDataSeries("error", errors);
                    errorChart.UpdateDataSeries("validate", valid);
                    // show classifiers
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
    
                // increase current iteration
                    iteration++;
            }
            // enable settings controls
            EnableControls( true );
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
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\SpreadSheetTest.csv"))
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    res = network.Compute(new double[2] { data[i, 0], data[i, 1] });
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


    }
}
