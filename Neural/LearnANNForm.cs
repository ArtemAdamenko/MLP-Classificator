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
    public partial class LearnANNForm : Form
    {
        #region ANN Options
        private double[][] validateInput;
        private int[] validClasses;
        private double[,] data = null;
        private int rowCountData = 0;
        private int colCountData = 0;

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
        private List<int> classesList = new List<int>();
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
            LineItem curve = myPane.AddCurve("Процент правильных ответов", listError, Color.Blue, SymbolType.Plus);
            LineItem curve2 = myPane.AddCurve("Кросс-валидация (вероятность)", listValidateP, Color.Green, SymbolType.Triangle);
            LineItem curve3 = myPane.AddCurve("Кросс-валидация (модуль)", listValidate, Color.Goldenrod, SymbolType.XCross);
            myPane.Title.Text = "Обучение ИНС";
            myPane.XAxis.Title.Text = "Итерации";
            myPane.YAxis.Title.Text = "Изменение показателей обучения";
            myPane.XAxis.Scale.MajorStep = 10;
            myPane.YAxis.Scale.MajorStep = 10;
            myPane.XAxis.MajorGrid.Color = Color.Black;
            myPane.YAxis.MajorGrid.Color = Color.Black;
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);
            myPane.YAxis.Scale.Max = 100;

            curve.Line.Width = 2.0F;
            curve2.Line.Width = 2.0F;
            curve3.Line.Width = 2.0F;
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

                    int row = 0;
                    int col = 0;

                    //get input and output count
                    line = reader.ReadLine();
                    row++;
                    //-1 last element empty
                    col = line.Trim().Split(';').Length;

                    //mass for new normalization cols input data
                    double[] minData = new double[col - 1];
                    double[] maxData = new double[col - 1];

                    //must be > 1 column in training data
                    if (col == 1)
                        throw new Exception();

                    while ((line = reader.ReadLine()) != null)
                    {
                        row++;
                    }
                    this.rowCountData = row;
                    this.colCountData = col;

                    double[,] tempData = new double[row, col - 1];
                    int[] tempClasses = new int[row];

                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    line = "";

                    // read the data
                    this.classesList.Clear();

                    while ((i < row) && ((line = reader.ReadLine()) != null))
                    {
                        string[] strs = line.Trim().Split(';');
                        List<String> inputVals = new List<String>(strs);

                        //del empty values in the end
                        inputVals.RemoveAll(str => String.IsNullOrEmpty(str));

                        // parse input and output values for learning
                        for (int j = 0; j < col - 1; j++)
                        {
                            tempData[i, j] = double.Parse(inputVals[j]);

                            //search min/max values for each column
                            if (tempData[i, j] < minData[j])
                                minData[j] = tempData[i, j];
                            if (tempData[i, j] > maxData[j])
                                maxData[j] = tempData[i, j];
                        }

                        tempClasses[i] = int.Parse(inputVals[col - 1]);

                        //insert class in list of classes, if not find
                        if (this.classesList.IndexOf(tempClasses[i]) == -1)
                        {
                            this.classesList.Add(tempClasses[i]);
                        }

                        i++;
                    }

                    tempData = ANNUtils.normalization(tempData, minData, maxData, row, col);

                    // allocate and set data
                    this.data = new double[i, col - 1];
                    Array.Copy(tempData, 0, this.data, 0, i * (col - 1));
                    this.classes = new int[i];

                    this.classesBox.Text = this.classesList.Count.ToString();
                    Array.Copy(tempClasses, 0, this.classes, 0, i);

                    inputCountBox.Invoke(new Action(() => inputCountBox.Text = (col - 1).ToString()));
                    fileTextBox.Invoke(new Action(() => fileTextBox.Text = openFileDialog.SafeFileName.ToString()));

                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "?????? ??  " + i.ToString() + " ??????", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                this.validLevel = Math.Max(0.0, Math.Min(100, double.Parse(validationLevelBox.Text)));
            }
            catch
            {
                this.validLevel = 95;
            }
            // get learning rate
            try
            {
                this.maxIterations = Math.Max(0, Math.Min(100000, int.Parse(maxIterationsBox.Text)));
            }
            catch
            {
                this.maxIterations = 1000;
            }
            // get learning rate
            try
            {
                this.maxNeuronsInLayer = Math.Max(1, Math.Min(1000, int.Parse(maxNeuronsInLayerBox.Text)));
            }
            catch
            {
                this.maxNeuronsInLayer = 5;
            }
            // get learning rate
            try
            {
                this.learningRate = Math.Max(0.00001, Math.Min(1, double.Parse(learningRateBox.Text)));
            }
            catch
            {
                this.learningRate = 0.5;
            }
            // get momentum
            try
            {
                this.moment = Math.Max(0, Math.Min(1.0, double.Parse(momentBox.Text)));
            }
            catch
            {
                this.moment = 0.9;
            }
            // get alpha
            try
            {
                this.alpha = Math.Max(0, Math.Min(2.0, double.Parse(alphaBox.Text)));
            }
            catch
            {
                this.alpha = 2.0;
            }
            // get neurons count in first layer
            try
            {
                String[] temp = neuronsBox.Text.Split(',');
                if (temp.Length < 1)
                    throw new Exception();
                this.neuronsAndLayers = new int[temp.Length + 1];
                for (int i = 0; i < temp.Length; i++)
                {
                    this.neuronsAndLayers[i] = Math.Max(1, Math.Min(1000, int.Parse(temp[i])));
                }

                this.neuronsAndLayers[temp.Length] = this.classesList.Count;

            }
            catch
            {
                this.neuronsAndLayers = new int[1];
                this.neuronsAndLayers[0] = 2;
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
            this.recordNeuroNet(this.network, this.moment, this.learningRate, this.iterations, this.error,
                            this.moduleValidateError, this.probabilisticValidateError, this.selectedAlgo, this.alpha);

        }

        private void recordNeuroNet(ActivationNetwork network, double moment, double learningRate, 
            int iterations, double error, double moduleValidateError, double probabilisticValidateError,
            String selectedAlgo, double alpha)
        {
            String topology = "";

            for (int i = 0; i < network.Layers.Length; i++)
            {
                topology += network.Layers[i].InputsCount + "-";
            }

            topology += network.Layers[network.Layers.Length-1].Neurons.Length;
            String momentStr = "";
            String learningRateStr = "";
            if (selectedAlgo != "RProp")
            {
                momentStr = moment.ToString();
                learningRateStr = learningRate.ToString();
            }

            this.lastRunsGridView.Invoke(
                            new Action(() =>
                                lastRunsGridView.Rows.Add(iterations.ToString(), error.ToString(), moduleValidateError.ToString(), probabilisticValidateError.ToString(),
                                selectedAlgo, topology, alpha.ToString(), learningRateStr, momentStr)
                                ));
        }

        private ActivationNetwork createNetwork(int[] topology, int colCountData)
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

        private ISupervisedLearning createTeacher(ActivationNetwork network, double learningRate, double moment, String selectedAlgo)
        {
            // create teacher
            ISupervisedLearning teacher = null;
            if (selectedAlgo == "RProp")
                teacher = new ParallelResilientBackpropagationLearning(network);
            else if (selectedAlgo == "BackProp")
            {
                BackPropagationLearning backProp = new BackPropagationLearning(network);
                backProp.LearningRate = learningRate;
                backProp.Momentum = moment;
                teacher = backProp;
            }
            return teacher;
        }

        private void dynamicChangeNet(ActivationNetwork network, int colCountData, double learningRate, double moment, String selectedAlgo)
        {
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
                this.network = createNetwork(tempNet, colCountData);
                this.teacher = createTeacher(this.network, learningRate, moment, selectedAlgo);
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
                this.network = createNetwork(tempNet, this.colCountData);
                this.teacher = createTeacher(this.network, this.learningRate, this.moment, this.selectedAlgo);
            }
        }

        // Worker thread
        void SearchSolution()
        {
            // number of learning samples
            int samples = data.GetLength(0);

            // prepare learning data
            //80% training, 20% for validate data
            double[][] input = new double[(samples * 4) / 5][];
            double[][] output = new double[(samples * 4) / 5][];
            int[] trainClasses = new int[(samples * 4) / 5];

            this.validateInput = new double[samples / 5][];
            this.validClasses = new int[samples / 5];

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
                        this.validateInput[K] = new double[this.colCountData - 1];

                        for (int c = 0; c < this.colCountData - 1; c++)
                        {
                            this.validateInput[K][c] = this.data[i, c];
                        }

                        this.validClasses[K] = this.classes[i];
                        K++;
                    }
                }
                else //forward input 80 %
                {
                    // input data
                    if (input.GetLength(0) > J)
                    {
                        input[J] = new double[colCountData - 1];

                        for (int c = 0; c < colCountData - 1; c++)
                        {
                            input[J][c] = data[i, c];
                        }

                        //output data
                        output[J] = new double[this.classesList.Count];
                        output[J][this.classesList.IndexOf(this.classes[i])] = 1;
                        trainClasses[J] = this.classes[i];
                        J++;
                    }
                }
            }

            this.network = createNetwork(this.neuronsAndLayers, this.colCountData);
            this.teacher = createTeacher(this.network, this.learningRate, this.moment, this.selectedAlgo);
            // iterations
            this.iterations = 1;
            this.error = 0.0;
            this.moduleValidateError = 0.0;
            this.probabilisticValidateError = 0.0;
            double[] results = new double[2];

            zedGraphControl1.GraphPane.CurveList[0].Clear();
            zedGraphControl1.GraphPane.CurveList[1].Clear();
            zedGraphControl1.GraphPane.CurveList[2].Clear();
            zedGraphControl1.AxisChange();

            // Get the first CurveItem in the graph
            CurveItem curve = zedGraphControl1.GraphPane.CurveList[0] as LineItem;
            CurveItem curve2 = zedGraphControl1.GraphPane.CurveList[1] as LineItem;
            CurveItem curve3 = zedGraphControl1.GraphPane.CurveList[2] as LineItem;

            // Get the PointPairList
            IPointListEdit listErrors = curve.Points as IPointListEdit;
            IPointListEdit listValidatesP = curve2.Points as IPointListEdit;
            IPointListEdit listValidates = curve3.Points as IPointListEdit;

            // loop
            while (!needToStop)
            {
                if (maxIterations != 0)
                {
                    if (iterations >= maxIterations)
                    {
                        this.recordNeuroNet(this.network, this.moment, this.learningRate, this.iterations, this.error,
                           this.moduleValidateError, this.probabilisticValidateError, this.selectedAlgo, this.alpha);

                        if (this.error < this.validLevel)
                        {
                            this.dynamicChangeNet(this.network, this.colCountData, this.learningRate, this.moduleValidateError, this.selectedAlgo);

                            zedGraphControl1.Invoke(new Action(() => zedGraphControl1.GraphPane.CurveList[0].Clear()));
                            zedGraphControl1.Invoke(new Action(() => zedGraphControl1.GraphPane.CurveList[1].Clear()));
                            zedGraphControl1.Invoke(new Action(() => zedGraphControl1.GraphPane.CurveList[2].Clear()));
                            this.iterations = 1;
                        }
                        else
                        {
                            needToStop = true;
                            break;
                        }
                    }
                }
                else if (maxIterations == 0 && this.error > this.validLevel)
                {
                    this.recordNeuroNet(this.network, this.moment, this.learningRate, this.iterations, this.error,
                            this.moduleValidateError, this.probabilisticValidateError, this.selectedAlgo, this.alpha);
                    needToStop = true;
                    break;
                }
                // run epoch of learning procedure
                this.error = (1 - (this.teacher.RunEpoch(input, output) / input.GetLength(0))) * 100;

                results = this.Validation(this.network, input, trainClasses, this.classesList);
                this.moduleValidateError = results[0];
                this.probabilisticValidateError = results[1];

                listErrors.Add(this.iterations, this.error);
                listValidates.Add(this.iterations, this.moduleValidateError);
                listValidatesP.Add(this.iterations, this.probabilisticValidateError);

                // Make sure the Y axis is rescaled to accommodate actual data
                zedGraphControl1.AxisChange();
                // Force a redraw
                zedGraphControl1.Invalidate();

                // set current iteration's info
                currentIterationBox.Invoke(new Action<string>((s) => currentIterationBox.Text = s), this.iterations.ToString());
                errorPercent.Invoke(new Action<string>((s) => errorPercent.Text = s), this.error.ToString("F14"));
                moduleValidBox.Invoke(new Action<string>((s) => moduleValidBox.Text = s), this.moduleValidateError.ToString("F14"));
                probabilisticValidBox.Invoke(new Action<string>((s) => probabilisticValidBox.Text = s), this.probabilisticValidateError.ToString("F14"));

                // increase current iteration
                this.iterations++;
            }
            // enable settings controls
            EnableControls(true);

        }

        private double[] Validation(ActivationNetwork network, double[][] input, int[] trainClasses, List<int> classesList)
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
            return new double[2] { ((1 - (moduleValue / input.Length)) * 100), ((1 - (probValue / input.Length)) * 100) };
        }

        private void SaveNetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "bin files (*.bin)|*.bin";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK
                    && saveFileDialog1.FileName.Length > 0)
            {

                network.Save(saveFileDialog1.FileName);
                MessageBox.Show("ИНС сохранена!");
            }
        }

        private void TestNetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.testNet(this.network, this.data, this.classesList, this.colCountData);
        }

        //incapsulate private method for crossValud TestNetToolStripMenuItem
        private void testNet(ActivationNetwork betterNet, double[,] data, List<int> classesList, int colCountData)
        {
            double[] res = new double[classesList.Count];
            double[] input = new double[colCountData - 1];

            FileInfo file = new FileInfo(LogHelper.getPath("Learn") + "\\Тестирование-" + LogHelper.getTime() + ".xlsx");
            ExcelPackage book = new ExcelPackage(file);
            ExcelWorksheet sheet = book.Workbook.Worksheets.Add("Тестирование на тестирующей (80%) подвыборке");

            for (int i = 0; i < data.GetLength(0); i++)
            {
                //gather inputs for compute, n-1 inputs
                for (int k = 0; k < colCountData - 1; k++)
                {
                    input[k] = data[i, k];
                }

                res = betterNet.Compute(input);

                int classificator = classesList[ANNUtils.max(res)];
                sheet.Cells[i + 1, 1].Value = classes[i];
                sheet.Cells[i + 1, 2].Value = classificator;

            }
            MessageBox.Show("Тестирование завершено!");
            book.Save();
            return;
        }

        //incapsulate private method for crossValidToolStripMenuItem Event
        private void crossValid(ActivationNetwork betterNet, double[][] validateInput, List<int> classesList, int colCountData, int[] validClasses)
        {
            double[] res = new double[classesList.Count];
            double[] input = new double[colCountData - 1];

            FileInfo file = new FileInfo(LogHelper.getPath("Learn") + "\\Валидация-" + LogHelper.getTime() + ".xlsx");
            ExcelPackage book = new ExcelPackage(file);
            ExcelWorksheet sheet = book.Workbook.Worksheets.Add("Валидация на валидационной (20%) подвыборке");

            for (int i = 0; i < validateInput.GetLength(0) - 1; i++)
            {
                //gather inputs for compute, n-1 inputs
                for (int k = 0; k < colCountData - 1; k++)
                {
                    input[k] = validateInput[i][k];
                }

                res = betterNet.Compute(input);

                int classificator = classesList[ANNUtils.max(res)];
                sheet.Cells[i + 1, 1].Value = validClasses[i];
                sheet.Cells[i + 1, 2].Value = classificator;
            }
            MessageBox.Show("Валидация завершена!");
            book.Save();
            return;
        }

        private void SaveWeightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveWeights(network);
        }

        //incapsulate private method for saveWeights Event
        private void saveWeights(ActivationNetwork betterNet)
        {
            FileInfo file = new FileInfo(LogHelper.getPath("Learn") + "\\Веса-" + LogHelper.getTime() + ".xlsx");
            ExcelPackage book = new ExcelPackage(file);
            ExcelWorksheet sheet = book.Workbook.Worksheets.Add("Весовые коэффициенты");

            sheet.Cells[1, 1].Value = "Индекс";
            sheet.Cells[1, 2].Value = "Слой";
            sheet.Cells[1, 3].Value = "Нейрон";
            sheet.Cells[1, 4].Value = "Вес";

            int row = 2;
            for (int i = 0; i < betterNet.Layers.Length; i++)
            {
                for (int j = 0; j < betterNet.Layers[i].Neurons.Length; j++)
                {
                    for (int k = 0; k < betterNet.Layers[i].Neurons[j].Weights.Length; k++)
                    {
                        sheet.Cells[row, 1].Value = i;
                        sheet.Cells[row, 2].Value = j;
                        sheet.Cells[row, 3].Value = k;
                        sheet.Cells[row, 4].Value = betterNet.Layers[i].Neurons[j].Weights[k];

                        row++;
                    }
                }
            }
            MessageBox.Show("Весовые коэффициенты сохранены!");
            book.Save();
            return;
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
        }

        private void crossValidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.crossValid(this.network, this.validateInput, this.classesList, this.colCountData, this.validClasses);
        }

        private void loadTrainDataButton_Click(object sender, EventArgs e)
        {
            this.getTrainDataForClass();
        }
    }
}
