using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Linq;
using ZedGraph;
using OfficeOpenXml;

namespace Neural
{
    public partial class LearnPullANNForm : Form
    {
        #region ANN Options
        private double[][] validateInput;
        private int[] trainClasses;
        private int[] validClasses;
        private double[,] data = null;
        private int rowCountData = 0;
        private int colCountData = 0;

        private double alpha = 2.0;
        private int iterations = 0;
        private int loops = 1;
        private int randomWeightsCount = 0;
        private double maxWeightValue = 0;
        private double minWeightValue = 0;

        private double moduleValidateError = 0.0;
        private double probabilisticValidateError = 0.0;

        private int[] neuronsAndLayers;
        private int[] classes;
        private List<int> classesList = new List<int>();

        private Thread workerThread = null;
        private bool needToStop = false;

        ActivationNetwork network;
        IActivationFunction activationFunc = null;
        #endregion

        // Constructor
        public LearnPullANNForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            RollingPointPairList listValidate = new RollingPointPairList(300);
            RollingPointPairList listValidateP = new RollingPointPairList(300);
            GraphPane myPane = zedGraphControl1.GraphPane;
         
            LineItem curve2 = myPane.AddCurve("Кросс-валидация (вероятность)", listValidateP, Color.Green, SymbolType.Triangle);
            LineItem curve3 = myPane.AddCurve("Кросс-валидация (модуль)", listValidate, Color.Goldenrod, SymbolType.XCross);
            myPane.Title.Text = "Обучение нейронной сети";
            myPane.XAxis.Title.Text = "Итерации";
            myPane.YAxis.Title.Text = "Изменение ошибки обучения";
            myPane.XAxis.Scale.MajorStep = 10;
            myPane.YAxis.Scale.MajorStep = 10;
            myPane.XAxis.MajorGrid.Color = Color.Black;
            myPane.YAxis.MajorGrid.Color = Color.Black;
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);
            myPane.YAxis.Scale.Max = 100;

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
            this.randomCountWeightsBox.Text = randomWeightsCount.ToString();
            this.maxWeightValueBox.Text = maxWeightValue.ToString();
            this.minWeightValueBox.Text = minWeightValue.ToString();
            this.limitRepeatBox.Text = loops.ToString();

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
            minWeightValueBox.Invoke(new Action(() => minWeightValueBox.Enabled = enable));
            maxWeightValueBox.Invoke(new Action(() => maxWeightValueBox.Enabled = enable));
            weightsChangeBox.Invoke(new Action(() => weightsChangeBox.Enabled = enable));
            randomCountWeightsBox.Invoke(new Action(() => randomCountWeightsBox.Enabled = enable));
            neuronsBox.Invoke(new Action(() => neuronsBox.Enabled = enable));
            limitRepeatBox.Invoke(new Action(() => limitRepeatBox.Enabled = enable));

        }

        // On button "Start"
        private void startButton_Click(object sender, System.EventArgs e)
        {
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

            this.randomWeightsCount = Int32.Parse(randomCountWeightsBox.Text);
            this.maxWeightValue = Double.Parse(maxWeightValueBox.Text);
            this.minWeightValue = Double.Parse(minWeightValueBox.Text);
            this.loops = Int32.Parse(limitRepeatBox.Text);
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
            this.recordNeuroNet(this.network, this.iterations, this.moduleValidateError, this.probabilisticValidateError, this.alpha);

        }

        private void recordNeuroNet(ActivationNetwork network,
            int iterations, double moduleValidateError, double probabilisticValidateError, double alpha)
        {
            String topology = "";

            for (int i = 0; i < network.Layers.Length; i++)
            {
                topology += network.Layers[i].InputsCount + "-";
            }

            topology += network.Layers[network.Layers.Length - 1].Neurons.Length;

            this.lastRunsGridView.Invoke(
                            new Action(() => lastRunsGridView.Rows.Add(iterations.ToString(), moduleValidateError.ToString()
                            , probabilisticValidateError.ToString()
                            , topology, alpha.ToString())));
        }

        //создание сети для обучения и учителя по топологии
        private ActivationNetwork createLearn(int[] topology, int colCountData)
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

        private List<String> checkWeights(List<String> trainingWeights)
        {
            //SORT BY INDEX
            if (sortByNumBox.Checked == true)
            {
                trainingWeights.Sort();
                if (sortSignNumSortBox.Text == "-1")
                {
                    trainingWeights.Reverse();
                }

            }
            //SORT BY ABS VALUE
            else if (sortByModuleAscBox.Checked == true)
            {
                trainingWeights = sortByModuleAsc(trainingWeights, network);
                if (sortSignAbsSortBox.Text == "-1")
                {
                    trainingWeights.Reverse();
                }
            }
            //SORT BY LAYERS
            else if (sortByLayersBox.Checked == true && allWeightsBox.Checked == true)
            {
                trainingWeights = sortByLayers(trainingWeights, network);
            }

            return trainingWeights;
        }
        // Worker thread
        void SearchSolution()
        {

            // number of learning samples
           int samples = data.GetLength(0);

            // prepare learning data
            //80% training, 20% for validate data
            double[][] input = new double[(samples * 4) / 5][];
            this.trainClasses = new int[(samples * 4) / 5];

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
                        input[J] = new double[this.colCountData - 1];

                        for (int c = 0; c < this.colCountData - 1; c++)
                        {
                            input[J][c] = this.data[i, c];
                        }

                        this.trainClasses[J] = this.classes[i];
                        J++;
                    }
                }
            }

            zedGraphControl1.GraphPane.CurveList[0].Clear();
            zedGraphControl1.GraphPane.CurveList[1].Clear();
            zedGraphControl1.AxisChange();

            // Get the first CurveItem in the graph
            CurveItem curve2 = zedGraphControl1.GraphPane.CurveList[0] as LineItem;
            CurveItem curve3 = zedGraphControl1.GraphPane.CurveList[1] as LineItem;
            // Get the PointPairList
            IPointListEdit listValidatesP = curve2.Points as IPointListEdit;
            IPointListEdit listValidates = curve3.Points as IPointListEdit;

            Random rnd = new Random();

            this.network = this.createLearn(this.neuronsAndLayers, this.colCountData);
            List<String> trainingWeights = this.createListWeights(this.network);
            trainingWeights = this.checkWeights(trainingWeights);

            // iterations
            this.moduleValidateError = 0.0;
            this.probabilisticValidateError = 0.0;
            double weightChange = 0;
            int currentWeight = 0;
            String[] signature = new String[3];
            int layer = 0;
            int neuron = 0;
            int weight = 0;
            int order = 1;
            int check = 0;
            int currentIteration = 1;
            double lastValidation = -1000;
            double lastValue = 0;
            double value = 0;
            double initialValue = 0;
            double[] results = new double[2];
            if (weightsChangeBox.Text.Length != 0)
            {
                weightChange = Math.Abs(Double.Parse(weightsChangeBox.Text));
            }
            // loop
            while (!needToStop)
            {
                //adds new row for gridView
                if (weightsView.Rows.Count < currentWeight + 2)
                {
                    weightsView.Invoke(new Action(() => weightsView.Rows.Add()));
                }
                //run new weight for changes
                if (signature[0] == null)
                {
                    signature = trainingWeights[currentWeight].Split(':');
                    layer = Int32.Parse(signature[0]);
                    neuron = Int32.Parse(signature[1]);
                    weight = Int32.Parse(signature[2]);
                    value = this.network.Layers[layer].Neurons[neuron].Weights[weight];
                    initialValue = this.network.Layers[layer].Neurons[neuron].Weights[weight];
                    weightsView.Invoke(new Action(() => weightsView.Rows[currentWeight].Cells[3].Value = initialValue));

                    if (percentChangeBox.Text.Length != 0)
                    {
                        Double percent = Double.Parse(percentChangeBox.Text);
                        weightChange = Math.Abs((value / 100) * percent);
                    }
                }
                //new value better the old
                if (lastValidation <= this.moduleValidateError)
                {
                    lastValidation = this.moduleValidateError;
                    lastValue = value;
                }

                //current weights != old weight
                if (weightsView.Rows[currentWeight].Cells[0].Value != trainingWeights[currentWeight])
                {
                    weightsView.Invoke(new Action(() => weightsView.Rows[currentWeight].Cells[0].Value = trainingWeights[currentWeight]));
                    weightsView.Invoke(new Action(() => weightsView.Rows[currentWeight].Cells[1].Value = value));
                }

                //sum to greater order
                if (value <= maxWeightValue && order == 1)
                {
                    value += weightChange;
                    weightsView.Invoke(new Action(() => weightsView.Rows[currentWeight].Cells[1].Value = value));

                }
                //limit sum
                else if (value >= maxWeightValue)
                {
                    value = initialValue;
                    order = -1;
                    ++check;
                }
                //sum to less order
                if (value >= minWeightValue && order == -1)
                {
                    value -= weightChange;
                    weightsView.Invoke(new Action(() => weightsView.Rows[currentWeight].Cells[1].Value = value));
                }
                //limit sum
                else if(value <= minWeightValue)
                {
                    order = 1;
                    ++check;
                }
                //if current weight changes ends
                if (check == 2)
                {
                    if (lastValidation == 0)
                    {
                        lastValue = initialValue;

                    }
                    this.network.Layers[layer].Neurons[neuron].Weights[weight] = lastValue;
                    weightsView.Invoke(new Action(() => weightsView.Rows[currentWeight].Cells[1].Value = lastValue));
                    weightsView.Invoke(new Action(() => weightsView.Rows[currentWeight].Cells[2].Value = lastValidation));

                    signature[0] = null;
                    
                    lastValidation = 0;
                    lastValue = 0;
                    check = 0;
                    if (currentWeight == trainingWeights.Count - 1)
                    {
                        if (currentIteration < this.loops)
                        {
                            trainingWeights = this.createListWeights(this.network, trainingWeights);
                            trainingWeights = this.checkWeights(trainingWeights);

                            currentWeight = 0;
                            currentIteration++;
                        }
                        else
                        {
                            needToStop = true;
                        }

                    }
                    else
                    {
                        currentWeight++;
                    }
                    
                }
                //changes weights value
                else
                {
                    this.network.Layers[layer].Neurons[neuron].Weights[weight] = value;
                }

                results = this.Validation(this.network, input, trainClasses, this.classesList);
                this.moduleValidateError = results[0];
                this.probabilisticValidateError = results[1];

                listValidates.Add(this.iterations, moduleValidateError);
                listValidatesP.Add(this.iterations, probabilisticValidateError);

                // Make sure the Y axis is rescaled to accommodate actual data
                zedGraphControl1.AxisChange();
                // Force a redraw
                zedGraphControl1.Invalidate();


                // set current iteration's info
                currentIterationBox.Invoke(new Action<string>((s) => currentIterationBox.Text = s), this.iterations.ToString());
               
                moduleValidBox.Invoke(new Action<string>((s) => moduleValidBox.Text = s), this.moduleValidateError.ToString("F14"));
                probabilisticValidBox.Invoke(new Action<string>((s) => probabilisticValidBox.Text = s), this.probabilisticValidateError.ToString("F14"));


                // increase current iteration
                this.iterations++;
            }
            // enable settings controls
            EnableControls(true);

        }

        private List<String> sortByLayers(List<String> weights, Network network)
        {
            List<String> returnList = new List<String>();
            Dictionary<int, double> totalSum = new Dictionary<int, double>();
            if (fromInputToOutputBox.Checked == true)
            {
                returnList = weights;
                returnList.Sort();

                if (sortSignFromToSortBox.Text == "-1")
                {
                    returnList.Reverse();
                }

            }else if(bySumWeightsBox.Checked == true)
            {
                
                for (int i = 0; i < network.Layers.Length; i++)
                {
                    double absSum = 0;
                    for (int j = 0; j < network.Layers[i].Neurons.Length; j++)
                    {
                        for (int k = 0; k < network.Layers[i].Neurons[j].Weights.Length; k++)
                        {
                            absSum += Math.Abs(network.Layers[i].Neurons[j].Weights[k]);
                        }
                    }
                    totalSum.Add(i, absSum);
                }
                if (sortSignAbsSumSortBox.Text == "1")
                {
                    totalSum = totalSum.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
                }

                if (sortSignAbsSumSortBox.Text == "-1")
                {
                    totalSum = totalSum.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
                }

                foreach(KeyValuePair<int, double> pair in totalSum)
                {
                    foreach(String weight in weights)
                    {
                        int layer = Int32.Parse(weight.Split(':')[0]);
                        if (layer == pair.Key)
                        {
                            returnList.Add(weight);
                        }
                    }
                }
            }


            return returnList;
        }

        private List<string> sortByModuleAsc(List<String> weights, Network network)
        {
            List<String> returnList = new List<string>();
            Dictionary<string, double> topology = new Dictionary<string, double>();
            int layer, neuron, weight = 0;

            String[] split = new String[3];

            foreach(String key in weights)
            {
                split = key.Split(':');
                layer = Int32.Parse(split[0]);
                neuron = Int32.Parse(split[1]);
                weight = Int32.Parse(split[2]);

                topology.Add(key, Math.Abs(network.Layers[layer].Neurons[neuron].Weights[weight]));

            }

            topology = topology.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach(KeyValuePair<string, double> pair in topology)
            {
                returnList.Add(pair.Key);
            }
            return returnList;
        }

        //create list of weights for changes
        private List<String> createListWeights(ActivationNetwork network, List<String> weights = null)
        {
            List<String> trainingWeights = new List<String>();
            List<String> list = ANNUtils.getAvailableWeights(network);

            if (this.allWeightsBox.Checked == true)
            {
                trainingWeights = list;
                return trainingWeights;
            }
            else
            {
                randomWeightsCount = Int32.Parse(randomCountWeightsBox.Text);
            }

            if (this.repeatPullBox.Checked != true || weights == null)
            {
                Random rnd = new Random();


                for (int i = 0; i < randomWeightsCount; i++)
                {
                    int rndWeight = rnd.Next(0, list.Count - 1);
                    trainingWeights.Add(list[rndWeight]);
                    list.RemoveAt(rndWeight);
                }
                return trainingWeights;
            }
            else
            {
                return weights;
            }

            
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

        //тестирование сети с записью результата в файл
        private void TestNetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.testNet(this.network, this.data, this.classesList, this.colCountData);
        }

        private void testNet(ActivationNetwork betterNet, double[,] data, List<int> classesList, int colCountData)
        {
            double[] res = new double[classesList.Count];
            double[] input = new double[colCountData - 1];

            FileInfo file = new FileInfo(LogHelper.getPath("Learn") + "\\РўРµСЃС‚РёСЂРѕРІР°РЅРёРµ-" + LogHelper.getTime() + ".xlsx");
            ExcelPackage book = new ExcelPackage(file);
            ExcelWorksheet sheet = book.Workbook.Worksheets.Add("РўРµСЃС‚РёСЂРѕРІР°РЅРёРµ РЅР° С‚РµСЃС‚РёСЂСѓСЋС‰РµР№ (80%) РїРѕРґРІС‹Р±РѕСЂРєРµ");

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
            MessageBox.Show("РўРµСЃС‚РёСЂРѕРІР°РЅРёРµ Р·Р°РІРµСЂС€РµРЅРѕ!");
            book.Save();
            return;
        }

        //incapsulate private method for crossValidToolStripMenuItem Event
        private void crossValid(ActivationNetwork betterNet, double[][] validateInput, List<int> classesList, int colCountData, int[] validClasses)
        {
            double[] res = new double[classesList.Count];
            double[] input = new double[colCountData - 1];

            FileInfo file = new FileInfo(LogHelper.getPath("Learn") + "\\Р’Р°Р»РёРґР°С†РёСЏ-" + LogHelper.getTime() + ".xlsx");
            ExcelPackage book = new ExcelPackage(file);
            ExcelWorksheet sheet = book.Workbook.Worksheets.Add("Р’Р°Р»РёРґР°С†РёСЏ РЅР° РІР°Р»РёРґР°С†РёРѕРЅРЅРѕР№ (20%) РїРѕРґРІС‹Р±РѕСЂРєРµ");

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
            MessageBox.Show("Р’Р°Р»РёРґР°С†РёСЏ Р·Р°РІРµСЂС€РµРЅР°!");
            book.Save();
            return;
        }

        //сохранение весов текущей ИНС в файл
        private void SaveWeightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveWeights(this.network);
        }

        //incapsulate private method for saveWeights Event
        private void saveWeights(ActivationNetwork betterNet)
        {
            FileInfo file = new FileInfo(LogHelper.getPath("Learn") + "\\Р’РµСЃР°-" + LogHelper.getTime() + ".xlsx");
            ExcelPackage book = new ExcelPackage(file);
            ExcelWorksheet sheet = book.Workbook.Worksheets.Add("Р’РµСЃРѕРІС‹Рµ РєРѕСЌС„С„РёС†РёРµРЅС‚С‹");

            sheet.Cells[1, 1].Value = "РРЅРґРµРєСЃ";
            sheet.Cells[1, 2].Value = "РЎР»РѕР№";
            sheet.Cells[1, 3].Value = "РќРµР№СЂРѕРЅ";
            sheet.Cells[1, 4].Value = "Р’РµСЃ";

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
            MessageBox.Show("Р’РµСЃРѕРІС‹Рµ РєРѕСЌС„С„РёС†РёРµРЅС‚С‹ СЃРѕС…СЂР°РЅРµРЅС‹!");
            book.Save();
            return;
        }

        //запуск кросс-валидации
        private void crossValidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.crossValid(this.network, this.validateInput, this.classesList, this.colCountData, this.validClasses);
        }

        //загрузка обучающей выборки
        private void loadTrainDataButton_Click(object sender, EventArgs e)
        {
            this.getTrainDataForClass();
        }

        private void allWeightsBox_CheckedChanged(object sender, EventArgs e)
        {
            randomCountWeightsBox.Enabled = !randomCountWeightsBox.Enabled;
            repeatPullBox.Enabled = !repeatPullBox.Enabled;
            sortByLayersBox.Enabled = !sortByLayersBox.Enabled;
        }

        private void repeatPullBox_CheckedChanged(object sender, EventArgs e)
        {
            allWeightsBox.Enabled = !allWeightsBox.Enabled;
        }

        private void sortByModuleAscBox_CheckedChanged(object sender, EventArgs e)
        {
            
            sortByNumBox.Enabled = !sortByNumBox.Enabled;
            sortSignNumSortBox.Enabled = !sortSignNumSortBox.Enabled;
            if (allWeightsBox.Checked == true)
            {
                sortByLayersBox.Enabled = !sortByLayersBox.Enabled;
            }
        }

        private void sortByNumBox_CheckedChanged(object sender, EventArgs e)
        {
            sortByModuleAscBox.Enabled = !sortByModuleAscBox.Enabled;
            
            sortSignAbsSortBox.Enabled = !sortSignAbsSortBox.Enabled;
            if (allWeightsBox.Checked ==  true)
            {
                sortByLayersBox.Enabled = !sortByLayersBox.Enabled;
            }
        }

        private void sortByLayersBox_CheckedChanged(object sender, EventArgs e)
        {
            sortByModuleAscBox.Enabled = !sortByModuleAscBox.Enabled;
            sortByNumBox.Enabled = !sortByNumBox.Enabled;
            fromInputToOutputBox.Enabled = !fromInputToOutputBox.Enabled;
            bySumWeightsBox.Enabled = !bySumWeightsBox.Enabled;
            sortSignFromToSortBox.Enabled = !sortSignFromToSortBox.Enabled;
            sortSignAbsSumSortBox.Enabled = !sortSignAbsSumSortBox.Enabled;
            sortSignNumSortBox.Enabled = !sortSignNumSortBox.Enabled;
            sortSignAbsSortBox.Enabled = !sortSignAbsSortBox.Enabled;
        }

        private void fromInputToOutputBox_CheckedChanged(object sender, EventArgs e)
        {
            bySumWeightsBox.Enabled = !bySumWeightsBox.Enabled;
            sortSignAbsSumSortBox.Enabled = !sortSignAbsSumSortBox.Enabled;
        }

        private void bySumWeightsBox_CheckedChanged(object sender, EventArgs e)
        {
            fromInputToOutputBox.Enabled = !fromInputToOutputBox.Enabled;
            sortSignFromToSortBox.Enabled = !sortSignFromToSortBox.Enabled;
        }

        private void weightsChangeBox_TextChanged(object sender, EventArgs e)
        {
            if (weightsChangeBox.Text.Length != 0 && percentChangeBox.Enabled == true)
            {
                percentChangeBox.Enabled = false;
            }
            else if(weightsChangeBox.Text.Length == 0 && percentChangeBox.Enabled == false)
            {
                percentChangeBox.Enabled = true;
            }
        }

        private void percentChangeBox_TextChanged(object sender, EventArgs e)
        {
            if (percentChangeBox.Text.Length != 0 && weightsChangeBox.Enabled == true)
            {
                weightsChangeBox.Enabled = !weightsChangeBox.Enabled;
            } else if (percentChangeBox.Text.Length == 0 && weightsChangeBox.Enabled == false)
            {
                weightsChangeBox.Enabled = true;
            }
        }
    }
}
