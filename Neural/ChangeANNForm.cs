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
using ExcelLibrary.SpreadSheet;


namespace Neural
{

    public partial class ChangeANNForm : Form
    {
        //App options
        Network network = null;
        Thread Worker;

        #region Neural Net options
                private Boolean needToStop = false;
                private double[,] data = null;
                private double[][][][] tempWeights;
                int rowCountData = 0;
                int colCountData = 0;
                private int[] classes;
                List<int> classesList = new List<int>();
                private double offWeightsSumInput = 0.0;
                private double offWeightsSumAbsoluteInput = 0.0;
                private double offWeightsSumOutput = 0.0;
                private double offWeightsSumAbsoluteOutput = 0.0;
        #endregion

        #region draw options
            private SolidBrush _myBrush = new SolidBrush(Color.DarkSeaGreen);
            private SolidBrush _offBrush = new SolidBrush(Color.Gray);
            private Pen _myPen = new Pen(Color.Black);
        #endregion

        #region QualityWorkBooks
            string fileXlsNeurons;
            Workbook QualityWorkBook;
            Worksheet QualityWorkSheet;

            string fileXlsWeightsRel;
            Workbook WeightsSumWorkBook;
            Worksheet WeightsSumWorkSheet;

            //store data for summ of absolutes values of weights
            string fileXlsWeightsAbs;
            Workbook AbsWeightsSumWorkBook;
            Worksheet AbsWeightsSumWorkSheet;
        #endregion


        public ChangeANNForm()
        {
            InitializeComponent();
        }

        //Отрисовка нейросети на pictureBox
        public void draw()
        {
            Bitmap bmp;
            Graphics formGraphics;
            
            int maxNeurons = 0;
            for (int layer = 0; layer < network.Layers.Length; layer++)
            {
                if (maxNeurons < network.Layers[layer].Neurons.Length)
                {
                    maxNeurons = network.Layers[layer].Neurons.Length;
                }
            }
            //except out of panel
            if (200 * (network.Layers.Length + 1) > this.pictureBox1.Width)
                bmp = new Bitmap(200 * (network.Layers.Length + 1), 50 * maxNeurons);
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
                    _myPen.Color = Color.Black;
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
                        //all neurons n-1 layer
                        for (int b = 0; b < fisrtPoints.Length; b++)
                        {
                            //if weight current neuron == 0.0
                            if (network.Layers[i].Neurons[j].Weights[b] == 0.0)
                                _myPen.Color = Color.Red;
                            else
                                _myPen.Color = Color.Black;
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
                            formGraphics.DrawLine(_myPen, hiddenLeftPoints[j], tempRightPoints[c]);
                        }
                    }
                }
                //temp mass of right points of current layer for next cycle
                tempRightPoints = new System.Drawing.Point[network.Layers[i].Neurons.Length];
                tempRightPoints = hiddenRightPoints;
            }

            pictureBox1.Image = bmp;
        }

        //Заполняет таблицу значениями  слоев, нейронов и весов соответственно
        private void setNeuronsDataGrid() 
        {
            // remove all current records
            this.dataGridView1.Rows.Clear();
            
            for (int i = 0; i < network.Layers.Length; i++)
            {
                for (int j = 0; j < network.Layers[i].Neurons.Length; j++)
                {
                    for (int k = 0; k < network.Layers[i].Neurons[j].Weights.Length; k++ )

                        this.dataGridView1.Rows.Add(i.ToString(), j.ToString(), k.ToString(), network.Layers[i].Neurons[j].Weights[k].ToString(), "F");
                }
            }

        }

        //Запуск сети для проверки
        private void CheckNeurons()
        {
            for (int i = 0; i < this.dataGridView1.RowCount; i++)
            {
                int layer = Int32.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString());
                int neuron = Int32.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                int weight = Int32.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());

                //если стоит галочка и до этого момента нейрон не отключался, значит отключаем нейрон и записываем его вес во временный массив,
                //чтобы потом можно было обратно включить нейрон
                if ((this.dataGridView1.Rows[i].Cells[4].Value.ToString() == "T".ToString()) &&
                    network.Layers[layer].Neurons[neuron].Weights[weight] != 0.0)
                {
                    tempWeights[layer][neuron][weight][0] = network.Layers[layer].Neurons[neuron].Weights[weight];
                    network.Layers[layer].Neurons[neuron].Weights[weight] = 0.0;
                    this.dataGridView1.Rows[i].Cells[3].Value = 0.0.ToString();
                }
                else {
                    //если галочка не стоит, и вес этого нейрона записан в временном массиве,
                    //значит он был отключен, а сейчас его нужно включить
                    if ((this.dataGridView1.Rows[i].Cells[4].Value.ToString() == "F".ToString()) && tempWeights[layer][neuron][weight][0] != 0.0)
                    {
                        network.Layers[layer].Neurons[neuron].Weights[weight] = tempWeights[layer][neuron][weight][0];
                        tempWeights[layer][neuron][weight][0] = 0.0;
                        this.dataGridView1.Rows[i].Cells[3].Value = network.Layers[layer].Neurons[neuron].Weights[weight].ToString();
                    }
                }
            }

            draw();
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
            openFileDialog2.Filter = "Bin Files|*.bin";

            if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    network = Network.Load(openFileDialog2.FileName);

                    //подсчет нейронов в сети
                    int neuronsCount = 0;
                    for (int layer = 0; layer < network.Layers.Length; layer++)
                    {
                        for (int neuron = 0; neuron < network.Layers[layer].Neurons.Length; neuron++)
                        {
                            neuronsCount++;
                        }
                    }

                    this.neuronsCountBox.Invoke(new Action<String>((nCount) => neuronsCountBox.Text = nCount), neuronsCount.ToString()); 
                }
                catch (IOException)
                {
                    throw new IOException("Ошибка загрузки нейронной сети");
                }
                finally
                {
                    this.Invoke(new Action(InitWorkComponents));
                    this.Invoke(new Action(draw));
                    Worker.Abort();
                }
            }
        }

        //Инициализация компонентов для работы
        private void InitWorkComponents()
        {
            //заполнение таблицы значений
            this.setNeuronsDataGrid();

            //инициализация временного массива для поддержки отключения и включения нейронов и связей
            tempWeights = new double[network.Layers.Length][][][];
            for (int i = 0; i < network.Layers.Length; i++)
            {
                tempWeights[i] = new double[network.Layers[i].Neurons.Length][][];
                for (int j = 0; j < network.Layers[i].Neurons.Length; j++)
                {
                    tempWeights[i][j] = new double[network.Layers[i].Neurons[j].Weights.Length][];
                    for (int k = 0; k < network.Layers[i].Neurons[j].Weights.Length; k++)
                    {
                        tempWeights[i][j][k] = new double[1];
                        tempWeights[i][j][k][0] = network.Layers[i].Neurons[j].Weights[k];
                    }
                }
            }
        }

        //поиск наиболее вероятного класса
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

        //запуск тестирования сети
        private double testing()
        {
            double[] res;
            double[] input = new double[colCountData - 1];
            double validate = 0.0;
            double testQuality = 0.0;


            for (int count = 0; count < data.GetLength(0) - 1; count++)
            {
                try
                {
                    //gather inputs for compute, n-1 inputs
                    for (int i = 0; i < colCountData - 1; i++)
                    {
                        input[i] = data[count, i];
                    }
                    res = network.Compute(input);
                    double output = this.max(res);
                    double value = Math.Abs(classes[count] - output);

                    validate += value;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ошибка тестирования сети." + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }

            }
            testQuality = (1 - (validate / data.GetLength(0))) * 100;
            this.errorTextBox.Invoke(new Action(() => this.errorTextBox.Text = testQuality.ToString("F10")));



            return testQuality;

        }

        //Запуск тестирования сети, с желаемым классом
        private void testNetButton_Click(object sender, EventArgs e)
        {
            //проверка откл/вкл нейронов
            this.CheckNeurons();
            //тестирование
            this.testing();
                
        }

        //load data for classification
        private void getDataForClass()
        {
            // show file selection dialog
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;
                int i = 0;
                try
                {
                    // open selected file
                    reader = File.OpenText(openFileDialog1.FileName);

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

        //load data for regression !!OFF
        private void getDataForRegression()
        {
            // show file selection dialog
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;

                try
                {
                    // open selected file
                    reader = File.OpenText(openFileDialog2.FileName);

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

            }
            this.testNetButton.Invoke(new Action(() => testNetButton.Enabled = true));

        }

       //Запуск автоматическго отключения нейронов !!!!FIX
        private void startOffNeuronsButton_Click(object sender, EventArgs e)
        {
            int begin = 0;
            int end = 0;
            try
            {
                string[] range = this.neuronsToOffBox.Text.Split('-');
                begin = int.Parse(range[0]);
                end = int.Parse(range[1]);
            }
            catch(Exception exc) 
            {
                throw new Exception("Неверно введен диапазон.", exc);
            }

            //перебор сети и отключение нейронов
            int neuronsCount = 0;

            int beginLayer = 0;
            int beginNeuron = 0;

            //поиск начала диапазона в нейронной сети
            for (int layer = 0; layer < network.Layers.Length; layer++)
            {
                for (int neuron = 0; neuron < network.Layers[layer].Neurons.Length; neuron++)
                {
                    if (neuronsCount == begin)
                    {
                        beginLayer = layer;
                        beginNeuron = neuron;
                    }
                    neuronsCount++;
                }
            }

            int endLayer = 0;
            int endNeuron = 0;
            neuronsCount = 0;
            //поиск конца диапазона в нейронной сети
            for (int layer = 0; layer < network.Layers.Length; layer++)
            {
                for (int neuron = 0; neuron < network.Layers[layer].Neurons.Length; neuron++)
                {
                    if (neuronsCount == end)
                    {
                        endLayer = layer;
                        endNeuron = neuron;
                    }
                    neuronsCount++;
                }
            }

            try
            {
                if (beginLayer <= endLayer)
                {
                    if (beginLayer == endLayer)
                    {
                        if (beginNeuron >= endNeuron)
                        {
                            throw new Exception();
                        }
                    }
                }
                else
                {
                    throw new Exception();
                }
                
            }
            catch
            {
                MessageBox.Show("Начало диапазона не может быть больше конца диапазона.", "Ошибка!");
                return;
            }
            Worker = new Thread(() => offNeurons(beginLayer, endLayer, beginNeuron, endNeuron, (end - begin) + 1));
            Worker.Start();
             //   offNeurons(beginLayer, endLayer, beginNeuron, endNeuron, (end - begin) + 1);

        }

        //отключение нейронов и запись данных в файл
        private void offNeurons(int beginLayer, int endLayer, int beginNeuron, int endNeuron, int acount)
        {
            String time = LogHelper.getTime();

            fileXlsNeurons = LogHelper.getPath("CrashNeurons") + "\\Neurons_" + time + ".xls";
            QualityWorkBook = new Workbook();
            QualityWorkSheet = new Worksheet("First Sheet");

            fileXlsWeightsRel = LogHelper.getPath("CrashNeurons") + "\\Weights_" + time + ".xls";
            WeightsSumWorkBook = new Workbook();
            WeightsSumWorkSheet = new Worksheet("First Sheet");

            fileXlsWeightsAbs = LogHelper.getPath("CrashNeurons") + "\\WeightsAbs_" + time + ".xls";
            AbsWeightsSumWorkBook = new Workbook();
            AbsWeightsSumWorkSheet = new Worksheet("First Sheet");

            //глобальный счетчик выделенных в диапазон нейронов
            int count = 0;
            int testIterations = 0;
            //массив выделенных нейронов
            Record[] rangeNeurons = new Record[acount];

            //проход по вычисленным из диапазона слоям
            for (int layer = beginLayer; layer <= endLayer; layer++)
            {
                // если текущий слой первый и не последний из выделенного диапазона
                if ((layer == beginLayer) && (layer != endLayer))
                {
                    //собираем нейроны от первого нейрона, до конца слоя
                    for (int neurons = beginNeuron; neurons < network.Layers[layer].Neurons.Length; neurons++)
                    {
                        rangeNeurons[count] = new Record(layer, neurons);
                        count++;
                    }
                }

                //если слой первый и последний
                if ((layer == beginLayer) && (layer == endLayer))
                {
                    //собираем нейроны из заданного диапазона
                    for (int neurons = beginNeuron; neurons <= endNeuron; neurons++)
                    {
                        rangeNeurons[count] = new Record(layer, neurons);
                        count++;
                    }
                }

                //если слой последний и не первый
                if ((layer == endLayer) && (layer != beginLayer))
                {
                    //собираем нейроны от начала слоя и до выделенного из диапазона нейрона
                    for (int neurons = 0; neurons <= endNeuron; neurons++)
                    {
                        rangeNeurons[count] = new Record(layer, neurons);
                        count++;
                    }
                }

                //если слой не первый и не последний
                if ((layer != beginLayer) && (layer != endLayer))
                {
                    //собираем тупо нейроны всего слоя
                    for (int neurons = 0; neurons < network.Layers[layer].Neurons.Length; neurons++)
                    {
                        rangeNeurons[count] = new Record(layer, neurons);
                        count++;
                    }
                }
            }


            //write headers to xls
            QualityWorkSheet = this.setNeuronsWorkBookHeaders(QualityWorkSheet, rangeNeurons);
            WeightsSumWorkSheet = this.setWeightsWorkBookHeaders(WeightsSumWorkSheet, rangeNeurons);
            AbsWeightsSumWorkSheet = this.setWeightsWorkBookHeaders(AbsWeightsSumWorkSheet, rangeNeurons);

            int combinations = 1;
            String verticalGroup = "";
            int fixedOffNeurons = 0;
            double allWeightsOfNet = 0;
            //TO Function
            for (int layer = 0; layer < network.Layers.Length; layer++)
            {
                for (int neuro = 0; neuro < network.Layers[layer].Neurons.Length; neuro++)
                {
                    for (int wei = 0; wei < network.Layers[layer].Neurons[neuro].Weights.Length; wei++)
                    {
                        allWeightsOfNet += network.Layers[layer].Neurons[neuro].Weights[wei];
                    }
                }
            }
            //start clock
            DateTime t = DateTime.Now;

            for (int k = 0; k < rangeNeurons.Length - 2; k++)
            {
                //if (needToStop)
                // break;
                for (int currentNeuron = 0; currentNeuron < rangeNeurons.Length; currentNeuron++)
                {
                    if (needToStop)
                        break;
                    //batch neurons off
                    for (int off = currentNeuron; off <= currentNeuron + k; off++)
                    {
                        if (off > rangeNeurons.Length - 1)
                        {
                            offWeights(network, rangeNeurons[off - rangeNeurons.Length]);
                            verticalGroup += rangeNeurons[off - rangeNeurons.Length].numberLayer.ToString() + ":" + rangeNeurons[off - rangeNeurons.Length].numberNeuron.ToString();
                        }
                        else
                        {
                            offWeights(network, rangeNeurons[off]);
                            verticalGroup += rangeNeurons[off].numberLayer.ToString() + ":" + rangeNeurons[off].numberNeuron.ToString();
                        }
                        verticalGroup += "|";
                        fixedOffNeurons++;

                    }

                    QualityWorkSheet.Cells[combinations, 0] = new Cell(verticalGroup);
                    WeightsSumWorkSheet.Cells[combinations, 0] = new Cell(verticalGroup);
                    AbsWeightsSumWorkSheet.Cells[combinations, 0] = new Cell(verticalGroup);

                    int step = 1;
                    for (int j = 0; j < rangeNeurons.Length; j++, step += 2)
                    {
                        if (needToStop)
                            break;
                        if ((j >= currentNeuron) && (j <= currentNeuron + k))
                            continue;
                        if ((j <= ((currentNeuron + k) - rangeNeurons.Length)) && (j >= (currentNeuron - rangeNeurons.Length)))
                            continue;

                        offWeights(network, rangeNeurons[j]);

                        QualityWorkSheet.Cells[combinations, j + 1] = new Cell(this.testing());
                        testIterations++;
                        DateTime t2 = DateTime.Now;
                        System.TimeSpan diffTime = t2.Subtract(t);
                        this.timeLabel.Invoke(new Action(() => this.timeLabel.Text = diffTime.ToString()));
                        this.iterationsBox.Invoke(new Action(() => this.iterationsBox.Text = testIterations.ToString()));
                        WeightsSumWorkSheet.Cells[combinations, step] = new Cell(this.offWeightsSumInput.ToString("F2"));
                        WeightsSumWorkSheet.Cells[combinations, step + 1] = new Cell(this.offWeightsSumOutput.ToString("F2"));
                        AbsWeightsSumWorkSheet.Cells[combinations, step] = new Cell(this.offWeightsSumAbsoluteInput.ToString("F2"));
                        AbsWeightsSumWorkSheet.Cells[combinations, step + 1] = new Cell(this.offWeightsSumAbsoluteOutput.ToString("F2"));

                        onWeights(network, rangeNeurons[j]);
                    }
                    //batch neurons to On
                    for (int off = currentNeuron; off <= currentNeuron + k; off++)
                    {
                        if (off > rangeNeurons.Length - 1)
                        {
                            onWeights(network, rangeNeurons[off - rangeNeurons.Length]);

                        }
                        else
                        {
                            onWeights(network, rangeNeurons[off]);
                        }
                    }

                    QualityWorkSheet.Cells[combinations, rangeNeurons.Length + 1] = new Cell(fixedOffNeurons + 1);
                    QualityWorkSheet.Cells[combinations, rangeNeurons.Length + 2] = new Cell(((double)(fixedOffNeurons + 1) / (double)getCountNeuronsOfNet(network)).ToString());
                    QualityWorkSheet.Cells[combinations, rangeNeurons.Length + 3] = new Cell((fixedOffNeurons + 1).ToString() + "/" + getCountNeuronsOfNet(network));

                    combinations++;
                    verticalGroup = "";
                    fixedOffNeurons = 0;

                }
            }
            QualityWorkBook.Worksheets.Add(QualityWorkSheet);
            QualityWorkBook.Save(fileXlsNeurons);

            WeightsSumWorkBook.Worksheets.Add(WeightsSumWorkSheet);
            WeightsSumWorkBook.Save(fileXlsWeightsRel);

            AbsWeightsSumWorkBook.Worksheets.Add(AbsWeightsSumWorkSheet);
            AbsWeightsSumWorkBook.Save(fileXlsWeightsAbs);
            MessageBox.Show("Перебор окончен.");


        }

        //установление заголовков для отчета по отключения нейронов в xls файл
        private Worksheet setNeuronsWorkBookHeaders(Worksheet QualityWorkSheet, Record[] rangeNeurons)
        {
            for (int header = 0; header < rangeNeurons.Length; header++)
            {
                QualityWorkSheet.Cells[0, header + 1] = new Cell(rangeNeurons[header].numberLayer.ToString() + ":" + rangeNeurons[header].numberNeuron.ToString());
            }
            QualityWorkSheet.Cells[0, rangeNeurons.Length + 1] = new Cell("Число отключенных нейронов");
            QualityWorkSheet.Cells[0, rangeNeurons.Length + 2] = new Cell("ЧОН/Нейронов всего");
            QualityWorkSheet.Cells[0, rangeNeurons.Length + 3] = new Cell("Соотношение нейронов");
            return QualityWorkSheet;
        }

        //установление заголовков для отчета по связям нейронов в xls файл
        private Worksheet setWeightsWorkBookHeaders(Worksheet QualityWorkSheet, Record[] rangeNeurons)
        {
            int step = 1;
            for (int header = 0; header < rangeNeurons.Length; header++)
            {
                
                QualityWorkSheet.Cells[0, step] = new Cell(rangeNeurons[header].numberLayer.ToString() + ":" + rangeNeurons[header].numberNeuron.ToString() + "|Вх.");
                QualityWorkSheet.Cells[0, step + 1] = new Cell(rangeNeurons[header].numberLayer.ToString() + ":" + rangeNeurons[header].numberNeuron.ToString() + "|Исх.");
                step += 2;
            }
            return QualityWorkSheet;
 
        }

        //получение количества нейронов в сети
        private int getCountNeuronsOfNet(Network network)
        {
            int count = 0;
            for (int i = 0; i < network.Layers.Length; i++)
            {
                for (int j = 0; j < network.Layers[i].Neurons.Length; j++)
                {
                    count++;
                }
            }
            return count;
        }

        //включение весов
        private void onWeights(Network network, Record currentNeuron)
        {
            for (int weight = 0; weight < network.Layers[currentNeuron.numberLayer].Neurons[currentNeuron.numberNeuron].Weights.Length; weight++)
            {
                network.Layers[currentNeuron.numberLayer].Neurons[currentNeuron.numberNeuron].Weights[weight] = tempWeights[currentNeuron.numberLayer][currentNeuron.numberNeuron][weight][0];
                tempWeights[currentNeuron.numberLayer][currentNeuron.numberNeuron][weight][0] = 0.0;

                this.offWeightsSumInput -= network.Layers[currentNeuron.numberLayer].Neurons[currentNeuron.numberNeuron].Weights[weight];
                this.offWeightsSumAbsoluteInput -= Math.Abs(network.Layers[currentNeuron.numberLayer].Neurons[currentNeuron.numberNeuron].Weights[weight]);
            }

            //if next layer is
            if ((network.Layers.Length - 1) >= (currentNeuron.numberLayer + 1))
            {
                for (int i = 0; i < network.Layers[currentNeuron.numberLayer + 1].Neurons.Length; i++)
                {
                    this.offWeightsSumOutput -= network.Layers[currentNeuron.numberLayer + 1].Neurons[i].Weights[currentNeuron.numberNeuron];
                    this.offWeightsSumAbsoluteOutput -= Math.Abs(network.Layers[currentNeuron.numberLayer + 1].Neurons[i].Weights[currentNeuron.numberNeuron]);
                }
            }

        }

        //отключение весов
        private void offWeights(Network network, Record currentNeuron)
        {
            for (int weights = 0; weights < network.Layers[currentNeuron.numberLayer].Neurons[currentNeuron.numberNeuron].Weights.Length; weights++)
            {
                //save to temp list of weights
                tempWeights[currentNeuron.numberLayer][currentNeuron.numberNeuron][weights][0] = network.Layers[currentNeuron.numberLayer].Neurons[currentNeuron.numberNeuron].Weights[weights];
                network.Layers[currentNeuron.numberLayer].Neurons[currentNeuron.numberNeuron].Weights[weights] = 0.0;
                
                //summ with other weights of input side of neuron, for report
                this.offWeightsSumInput += tempWeights[currentNeuron.numberLayer][currentNeuron.numberNeuron][weights][0];
                this.offWeightsSumAbsoluteInput += Math.Abs(tempWeights[currentNeuron.numberLayer][currentNeuron.numberNeuron][weights][0]);
            }
            //if next layer is, summ output weights of neuron
            if ((network.Layers.Length - 1) >= (currentNeuron.numberLayer + 1))
            {
                for (int i = 0; i < network.Layers[currentNeuron.numberLayer + 1].Neurons.Length; i++ )
                {
                    this.offWeightsSumOutput += network.Layers[currentNeuron.numberLayer + 1].Neurons[i].Weights[currentNeuron.numberNeuron];
                    this.offWeightsSumAbsoluteOutput += Math.Abs(network.Layers[currentNeuron.numberLayer + 1].Neurons[i].Weights[currentNeuron.numberNeuron]);
                }
            }
        }


        //save neural net
        private void saveNetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "bin files (*.bin)|*.bin";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK
                    && saveFileDialog1.FileName.Length > 0)
            {

                network.Save(saveFileDialog1.FileName);
                MessageBox.Show("Сеть сохранена");
            }
        }

        //остановка отлючения нейронов
        private void stopButton_Click(object sender, EventArgs e)
        {
            needToStop = true;
        }

        //выбор опции Тестирующего вектора
        private void тестToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Worker = new Thread(getDataForClass);

            Worker.SetApartmentState(ApartmentState.STA);
            Worker.Start();
            this.testNetButton.Enabled = true;
            this.outputVectorButton.Enabled = false;
        }

        //выбор опции Входной вектор
        private void входнойВекторToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Worker = new Thread(getDataForClassCompute);

            Worker.SetApartmentState(ApartmentState.STA);
            Worker.Start();
            this.testNetButton.Enabled = false;
            this.outputVectorButton.Enabled = true;

        }

        //загрузка входного вектора без желаемого класса
        private void getDataForClassCompute()
        {
            // show file selection dialog
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;
                int i = 0;
                try
                {
                    // open selected file
                    reader = File.OpenText(openFileDialog1.FileName);

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

                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    line = "";

                    while ((i < rowCountData) && ((line = reader.ReadLine()) != null))
                    {
                        string[] strs = line.Trim().Split(';');
                        List<String> inputVals = new List<String>(strs);

                        //del empty values in the end
                        inputVals.RemoveAll(str => String.IsNullOrEmpty(str));

                        // parse input and output values for learning
                        for (int j = 0; j < colCountData; j++)
                        {
                            tempData[i, j] = double.Parse(inputVals[j]);

                            //search min/max values for each column
                            if (tempData[i, j] < minData[j])
                                minData[j] = tempData[i, j];
                            if (tempData[i, j] > maxData[j])
                                maxData[j] = tempData[i, j];
                        }


                        i++;
                    }

                    //normalization input values
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

        //запуск просчет выходного вектора
        private void outputVectorButton_Click(object sender, EventArgs e)
        {
            //проверка откл/вкл нейронов
            this.CheckNeurons();
            //тестирование
            this.getOutputVector();
        }

        //просчет выходного вектора на основе входного, не используется желаемый класс
        private void getOutputVector()
        {
            double[] res;
            double[] input = new double[colCountData];

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogHelper.getPath("Test") + "\\OutVector_" + LogHelper.getTime() + ".csv"))

                for (int count = 0; count < data.GetLength(0) - 1; count++)
                {
                    try
                    {
                        //gather inputs for compute, n-1 inputs
                        for (int i = 0; i < colCountData; i++)
                        {
                            input[i] = data[count, i];
                        }
                        res = network.Compute(input);
                        double output = this.max(res);
                        file.WriteLine(output.ToString() + ";");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Ошибка тестирования сети." + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }

                }



            MessageBox.Show("Выходной вектор сформирован.");

        }

    }

}
