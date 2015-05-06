using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using Accord.Neuro;
using Accord.Math;
using Accord.Neuro.Learning;
using ZedGraph;


namespace Neural
{
    public partial class LearnANNForm : Form
    {
        #region ANN Options
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
        private double moduleValidateError = 0.0;
        private double probabilisticValidateError = 0.0;

        private String selectedAlgo = "";
        private int[] neuronsAndLayers;
        private int[] classes;
        List<int> classesList = new List<int>();
        private double validLevel = 95;
        private int maxIterations = 500;
        private int maxNeuronsInLayer = 10;

        private Thread workerThread = null;
        private bool needToStop = false;
        
        ActivationNetwork network;
        IActivationFunction activationFunc = null;
        ISupervisedLearning teacher = null;
        #endregion

        // Constructor
        public LearnANNForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            RollingPointPairList listError = new RollingPointPairList(300);
            RollingPointPairList listValidate = new RollingPointPairList(300);
            RollingPointPairList listValidateP = new RollingPointPairList(300);
            GraphPane myPane = zedGraphControl1.GraphPane;
            LineItem curve = myPane.AddCurve("Качество обучения", listError, Color.Blue, SymbolType.Plus);
            LineItem curve2 = myPane.AddCurve("Кросс-валидация (вероятность)", listValidateP, Color.Green, SymbolType.Triangle);
            LineItem curve3 = myPane.AddCurve("Кросс-валидация (модуль)", listValidate, Color.Goldenrod, SymbolType.XCross);
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

                        //if (strs.Length-1 < colCountData - 1)
                          //  continue;
                        tempClasses[i] = int.Parse(inputVals[colCountData - 1]);

                        //insert class in list of classes, if not find
                        if (classesList.IndexOf(tempClasses[i]) == -1)
                        {
                            classesList.Add(tempClasses[i]);
                        }

                        //samplesPerClass[tempClasses[i]]++;

                        i++;
                    }

                    //normalization input values
                    for (int row = 0; row < rowCountData; row++)
                    {
                        for (int column = 0; column < colCountData - 1; column++)
                        {
                            tempData[row, column] = (((tempData[row, column] - minData[column]) * 1 / (maxData[column] - minData[column])));
                            
                        }
                    }

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
                validLevel = Math.Max(0.0, Math.Min(100, double.Parse(validationLevelBox.Text)));
            }
            catch
            {
                validLevel = 95;
            }
            // get learning rate
            try
            {
                maxIterations = Math.Max(0, Math.Min(100000, int.Parse(maxIterationsBox.Text)));
            }
            catch
            {
                maxIterations = 1000;
            }
            // get learning rate
            try
            {
                maxNeuronsInLayer = Math.Max(1, Math.Min(1000, int.Parse(maxNeuronsInLayerBox.Text)));
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

            topology += network.Layers[network.Layers.Length-1].Neurons.Length;
            String moment = "";
            String learningRate = "";
            if (this.selectedAlgo != "RProp")
            {
                moment = this.moment.ToString();
                learningRate = this.learningRate.ToString();
            }

            this.lastRunsGridView.Invoke(
                            new Action(() =>
                                lastRunsGridView.Rows.Add(this.iterations.ToString(), this.error.ToString(), (this.moduleValidateError).ToString(), (this.probabilisticValidateError).ToString(),
                                this.selectedAlgo, topology, this.alpha.ToString(), learningRate, moment)
                                ));
        }

        //создание сети для обучения и учителя по топологии
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


            createLearn(neuronsAndLayers);
            // iterations
            this.iterations = 1;
            this.error = 0.0;
            this.moduleValidateError = 0.0;
            this.probabilisticValidateError = 0.0;

            zedGraphControl1.GraphPane.CurveList[0].Clear();
            zedGraphControl1.GraphPane.CurveList[1].Clear();
            zedGraphControl1.GraphPane.CurveList[2].Clear();
            zedGraphControl1.AxisChange();

            // loop
            while (!needToStop)
            {
                if (maxIterations != 0)
                {
                    if ((iterations >= maxIterations) && (this.error < validLevel))
                    {
                        recordNeuroNet();
                        int potentialLayer = -1;

                        //search free layer for add neuron
                        for (int hiddenLayer = 0; hiddenLayer < network.Layers.Length - 1; hiddenLayer++)
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
                            int[] tempNet = new int[network.Layers.Length + 1];
                            int i = 0;
                            for (i = 0; i < network.Layers.Length - 1; i++)
                            {
                                tempNet[i] = network.Layers[i].Neurons.Length;
                            }
                            tempNet[i] = 1; // new layer before last layer
                            tempNet[i + 1] = network.Output.Length; // last layer in end
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
                        zedGraphControl1.Invoke(new Action(() => zedGraphControl1.GraphPane.CurveList[2].Clear()));
                        iterations = 1;
                    }
                    else if (iterations >= maxIterations && this.error > validLevel)
                    {
                        recordNeuroNet();
                        needToStop = true;
                        break;
                    }
                }
                else if (maxIterations == 0 && this.error > validLevel)
                {
                    recordNeuroNet();
                    needToStop = true;
                    break;
                }
                // run epoch of learning procedure
                error = (1 - (teacher.RunEpoch(this.input, this.output) / this.input.GetLength(0))) * 100;


                moduleValidateError = this.moduleValidation();
                probabilisticValidateError = this.probabilisticValidation();



                // Make sure that the curvelist has at least one curve
                if (zedGraphControl1.GraphPane.CurveList.Count <= 0)
                    return;

                // Get the first CurveItem in the graph
                CurveItem curve = zedGraphControl1.GraphPane.CurveList[0] as LineItem;
                CurveItem curve2 = zedGraphControl1.GraphPane.CurveList[1] as LineItem;
                CurveItem curve3 = zedGraphControl1.GraphPane.CurveList[2] as LineItem;
                if (curve == null)
                    return;
                if (curve2 == null)
                    return;
                if (curve3 == null)
                    return;
                // Get the PointPairList
                IPointListEdit listErrors = curve.Points as IPointListEdit;
                IPointListEdit listValidatesP = curve2.Points as IPointListEdit;
                IPointListEdit listValidates = curve3.Points as IPointListEdit;

                if (listErrors == null)
                    return;
                if (listValidates == null)
                    return;
                if (listValidatesP == null)
                    return;

                listErrors.Add(this.iterations, error);
                listValidates.Add(this.iterations, moduleValidateError);
                listValidatesP.Add(this.iterations, probabilisticValidateError);

                // Make sure the Y axis is rescaled to accommodate actual data
                zedGraphControl1.AxisChange();
                // Force a redraw
                zedGraphControl1.Invalidate();


                // set current iteration's info
                currentIterationBox.Invoke(new Action<string>((s) => currentIterationBox.Text = s), this.iterations.ToString());
                errorPercent.Invoke(new Action<string>((s) => errorPercent.Text = s), error.ToString("F14"));
                moduleValidBox.Invoke(new Action<string>((s) => moduleValidBox.Text = s), moduleValidateError.ToString("F14"));
                probabilisticValidBox.Invoke(new Action<string>((s) => probabilisticValidBox.Text = s), probabilisticValidateError.ToString("F14"));


                // increase current iteration
                this.iterations++;
            }
            // enable settings controls
            EnableControls(true);

        }

        //валидация по модулю
        private double moduleValidation()
        {
            double testQuality = 0.0;
            double validate = 0.0;
            double[] res;
            for (int count = 0; count < validateInput.Length; count++)
            {
                res = network.Compute(validateInput[count]);
                double value = Math.Abs(validClasses[count] - this.max(res));

                validate += value;
       
            }
            testQuality = (1 - (validate / validateInput.Length)) * 100;
            return testQuality;
        }

        //валидация по вероятности
        private double probabilisticValidation()
        {
            double testQuality = 0.0;
            double validate = 0.0;
            double[] res;
            for (int count = 0; count < validateInput.Length; count++)
            {
                res = network.Compute(validateInput[count]);
                double value = 1 - res[validClasses[count]];

                validate += value;

            }
            testQuality = (1 - (validate / validateInput.Length)) * 100;
            return testQuality;
        }

        //Сохранение нейронной сети по указанному пути
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

        //тестирование сети(80%) с записью результата в файл
        private void TestNetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String[] lines = new String[1];
            double[] res = new double[classesList.Count];
		    double[] input = new double[colCountData - 1];

            using(System.IO.StreamWriter file = new System.IO.StreamWriter(LogHelper.getPath("Learn") + "\\Test_" + LogHelper.getTime() + ".csv"))
            //file.WriteLine("Желаемый ответ; Результат");

                for (int i = 0; i < data.GetLength(0); i++)
                {
                    //gather inputs for compute, n-1 inputs
                    for (int k = 0; k < colCountData - 1; k++)
                    {
                        input[k] = data[i, k];
                    }

                    res = network.Compute(input);

                    int classificator = this.max(res);
                    lines[0] = classes[i].ToString() + ";" + classificator.ToString("F8");

                    file.WriteLine(lines[0].ToString());
                }
                MessageBox.Show("Тестирование пройдено");

        }


        //кросс-валидация(20%) сети с записью результата в файл
        private void crossValid()
        {
            String[] lines = new String[1];
            double[] res = new double[classesList.Count];
            double[] input = new double[colCountData - 1];

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogHelper.getPath("Learn") + "\\SpreadSHeetValid" + LogHelper.getTime() + ".csv"))

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

        //поиcк наиболее вероятного класса
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

        //сохранение весов текущей ИНС в файл
        private void SaveWeightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogHelper.getPath("Learn") + "\\Weights_" + LogHelper.getTime() + ".csv"))
            for (int i = 0; i < network.Layers.Length; i++)
            {
                for (int j = 0; j < network.Layers[i].Neurons.Length; j++)
                {
                    for (int k = 0; k < network.Layers[i].Neurons[j].Weights.Length; k++)
                    {
                        
                        file.WriteLine("L[" + i.ToString() + "]N[" + j.ToString() + "]W[" + k.ToString() + "];" + network.Layers[i].Neurons[j].Weights[k] + ";");
                        
                    }
                }
            }
            MessageBox.Show("Веса сохранены");
        }

        //выбор алгоритма обучения
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
    }
}
