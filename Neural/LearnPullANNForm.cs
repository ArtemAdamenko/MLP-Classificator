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
        private int loops = 1;
        private double weightChange = 0;
        private int randomWeightsCount = 0;
        private double maxWeightValue = 0;
        private double minWeightValue = 0;

        private double error = 0.0;
        private double moduleValidateError = 0.0;
        private double probabilisticValidateError = 0.0;

        private int[] neuronsAndLayers;
        private int[] classes;
        List<int> classesList = new List<int>();

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
            this.randomCountWeightsBox.Text = randomWeightsCount.ToString();
            //this.weightsChangeBox.Text = weightChange.ToString();
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

            randomWeightsCount = Int32.Parse(randomCountWeightsBox.Text);
            maxWeightValue = Double.Parse(maxWeightValueBox.Text);
            minWeightValue = Double.Parse(minWeightValueBox.Text);
            loops = Int32.Parse(limitRepeatBox.Text);
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


            this.lastRunsGridView.Invoke(
                            new Action(() =>
                                lastRunsGridView.Rows.Add(this.iterations.ToString(), (this.moduleValidateError).ToString(), (this.probabilisticValidateError).ToString(),
                                topology, this.alpha.ToString())
                                ));
        }

        //создание сети для обучения и учителя по топологии
        private void createLearn(int[] topology)
        {
            if (this.alphaBox.Text != "")
                activationFunc = new BipolarSigmoidFunction(Double.Parse(this.alphaBox.Text));
            else
                activationFunc = new BipolarSigmoidFunction();

            network = new ActivationNetwork(activationFunc,
            colCountData - 1, topology);
            //ActivationLayer layer = network.Layers[0] as ActivationLayer;

            NguyenWidrow initializer = new NguyenWidrow(network);
            initializer.Randomize();
            // create teacher
            /*GeneticLearning genetic = new GeneticLearning(network, chromosomes);
            teacher = genetic;*/

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
            this.error = 0.0;
            this.moduleValidateError = 0.0;
            this.probabilisticValidateError = 0.0;

            zedGraphControl1.GraphPane.CurveList[0].Clear();
            zedGraphControl1.GraphPane.CurveList[1].Clear();
            zedGraphControl1.GraphPane.CurveList[2].Clear();
            zedGraphControl1.AxisChange();

            Random rnd = new Random();

            List<String> trainingWeights = createListWeights();
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
            else if(sortByModuleAscBox.Checked == true)
            {
                trainingWeights = sortByModuleAsc(trainingWeights, network);
                if (sortSignAbsSortBox.Text == "-1")
                {
                    trainingWeights.Reverse();
                }
            }
            //SORT BY LAYERS
            else if(sortByLayersBox.Checked == true && allWeightsBox.Checked == true)
            {
                trainingWeights = sortByLayers(trainingWeights, network);
            }

            if (weightsChangeBox.Text.Length != 0)
            {
                weightChange = Math.Abs(Double.Parse(weightsChangeBox.Text));
            }
            int currentWeight = 0;
            String[] signature = new String[3];
            int layer = 0;
            int neuron = 0;
            int weight = 0;
            int order = 1;
            int check = 0;
            int currentIteration = 1;
            double lastValidation = 0;
            double lastValue = 0;
            double value = 0;
            double initialValue = 0;
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
                    value = network.Layers[layer].Neurons[neuron].Weights[weight];
                    initialValue = network.Layers[layer].Neurons[neuron].Weights[weight];
                    weightsView.Invoke(new Action(() => weightsView.Rows[currentWeight].Cells[3].Value = initialValue));

                    if (percentChangeBox.Text.Length != 0)
                    {
                        Double percent = Double.Parse(percentChangeBox.Text);
                        weightChange = Math.Abs((value / 100) * percent);
                    }
                }
                //new value better the old
                if (lastValidation <= moduleValidateError)
                {
                    lastValidation = moduleValidateError;
                    lastValue = value;
                }



#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
                              //current weights != old weight
                if (weightsView.Rows[currentWeight].Cells[0].Value != trainingWeights[currentWeight])
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
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
                    network.Layers[layer].Neurons[neuron].Weights[weight] = lastValue;
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
                            trainingWeights = createListWeights(trainingWeights);
                            if (sortByNumBox.Checked == true && repeatPullBox.Checked != true)
                            {
                                trainingWeights.Sort();
                                if (sortSignNumSortBox.Text == "-1")
                                {
                                    trainingWeights.Reverse();
                                }
                            }else if(sortByModuleAscBox.Checked == true)
                            {
                                trainingWeights = sortByModuleAsc(trainingWeights, network);
                                if (sortSignAbsSortBox.Text == "-1")
                                {
                                    trainingWeights.Reverse();
                                }
                            }
                            else if (sortByLayersBox.Checked == true && allWeightsBox.Checked == true)
                            {
                                trainingWeights = sortByLayers(trainingWeights, network);
                            }
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
                    network.Layers[layer].Neurons[neuron].Weights[weight] = value;
                }

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
               
                moduleValidBox.Invoke(new Action<string>((s) => moduleValidBox.Text = s), moduleValidateError.ToString("F14"));
                probabilisticValidBox.Invoke(new Action<string>((s) => probabilisticValidBox.Text = s), probabilisticValidateError.ToString("F14"));


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
        private List<String> createListWeights(List<String> weights = null)
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

        //валидация по модулю
        private double moduleValidation()
        {
            double testQuality = 0.0;
            double validate = 0.0;
            double[] res;
            for (int count = 0; count < input.Length; count++)
            {
                res = network.Compute(input[count]);
                double value = Math.Abs(trainClasses[count] - classesList[ANNUtils.max(res)]);

                validate += value;

            }
            testQuality = (1 - (validate / input.Length)) * 100;
            return testQuality;
        }

        //валидация по вероятности
        private double probabilisticValidation()
        {
            double testQuality = 0.0;
            double validate = 0.0;
            double[] res;
            for (int count = 0; count < input.Length; count++)
            {
                res = network.Compute(input[count]);
                double value = 1 - res[classesList.IndexOf(trainClasses[count])];

                validate += value;

            }
            testQuality = (1 - (validate / input.Length)) * 100;
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

        //тестирование сети с записью результата в файл
        private void TestNetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[] res = new double[classesList.Count];
            double[] input = new double[colCountData - 1];

            FileInfo file = new FileInfo(LogHelper.getPath("Learn") + "\\Тестирование-" + LogHelper.getTime() + ".xlsx");
            ExcelPackage book = new ExcelPackage(file);
            ExcelWorksheet sheet = book.Workbook.Worksheets.Add("Тестирование всей обучающей выборки");

            for (int i = 0; i < data.GetLength(0); i++)
            {
                //gather inputs for compute, n-1 inputs
                for (int k = 0; k < colCountData - 1; k++)
                {
                    input[k] = data[i, k];
                }

                res = network.Compute(input);

                int classificator = classesList[ANNUtils.max(res)];
                sheet.Cells[i + 1, 1].Value = classes[i];
                sheet.Cells[i + 1, 2].Value = classificator;

            }
            MessageBox.Show("Тестирование пройдено");
            book.Save();
            return;
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

        //сохранение весов текущей ИНС в файл
        private void SaveWeightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(LogHelper.getPath("Learn") + "\\Веса-" + LogHelper.getTime() + ".xlsx");
            ExcelPackage book = new ExcelPackage(file);
            ExcelWorksheet sheet = book.Workbook.Worksheets.Add("Значения весов всей обученной сети");

            sheet.Cells[1, 1].Value = "Слой";
            sheet.Cells[1, 2].Value = "Нейрон";
            sheet.Cells[1, 3].Value = "Вес";
            sheet.Cells[1, 4].Value = "Значение";

            int row = 2;
            for (int i = 0; i < network.Layers.Length; i++)
            {
                for (int j = 0; j < network.Layers[i].Neurons.Length; j++)
                {
                    for (int k = 0; k < network.Layers[i].Neurons[j].Weights.Length; k++)
                    {
                        sheet.Cells[row, 1].Value = i;
                        sheet.Cells[row, 2].Value = j;
                        sheet.Cells[row, 3].Value = k;
                        sheet.Cells[row, 4].Value = network.Layers[i].Neurons[j].Weights[k];

                        row++;
                    }
                }
            }
            MessageBox.Show("Веса сохранены");
            book.Save();
            return;
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
