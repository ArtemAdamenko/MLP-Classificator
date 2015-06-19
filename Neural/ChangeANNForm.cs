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
using OfficeOpenXml;


namespace Neural
{

    public partial class ChangeANNForm : Form
    {
        //App options
        Network network = null;
        Thread Worker;
        int globalOffNeuronsCount = 0;

        #region Neural Net options
                private Boolean needToStop = false;
                private Boolean needToSpotWeightsOff = false;
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
            private static FileInfo fileXlsNeurons;
            private static ExcelPackage QualityWorkBook;
            private static ExcelWorksheet QualityWorkSheet;

            private static FileInfo fileXlsWeightsRel;
            private static ExcelPackage WeightsSumWorkBook;
            private static ExcelWorksheet WeightsSumWorkSheet;

            //store data for summ of absolutes values of weights
            private static FileInfo fileXlsWeightsAbs;
            private static ExcelPackage AbsWeightsSumWorkBook;
            private static ExcelWorksheet AbsWeightsSumWorkSheet;
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
                bmp = new Bitmap(700, 50 * maxNeurons); //700 picturebox.width

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
                            {
                                _myPen.Color = Color.Red;
                                _myPen.Width = 3;
                            } 
                            
                            formGraphics.DrawLine(_myPen, hiddenLeftPoints[j], fisrtPoints[b]);
                            _myPen.Color = Color.Black;
                            _myPen.Width = 1;
                        }
                    }
                    else if (i != 0)
                    {
                        for (int c = 0; c < network.Layers[i - 1].Neurons.Length; c++)
                        {
                            //if weight current neuron == 0.0
                            if (network.Layers[i].Neurons[j].Weights[c] == 0.0)
                            {
                                _myPen.Color = Color.Red;
                                _myPen.Width = 3;
                            }
                            
                            formGraphics.DrawLine(_myPen, hiddenLeftPoints[j], tempRightPoints[c]);
                            _myPen.Color = Color.Black;
                            _myPen.Width = 1;
                        }
                    }
                }
                //temp mass of right points of current layer for next cycle
                tempRightPoints = new System.Drawing.Point[network.Layers[i].Neurons.Length];
                tempRightPoints = hiddenRightPoints;
            }
            pictureBox1.Invoke(new Action(() => pictureBox1.Image = bmp));
          //  pictureBox1.Image = bmp;
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

        //проверка отключения/включения весов
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
                    int neuronsCount = ANNUtils.getNeuronsCount(network);

                    this.neuronsCountBox.Invoke(new Action(() => neuronsCountBox.Text = neuronsCount.ToString())); 
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
            tempWeights = ANNUtils.emptyWeightsArray(network);
        }

        //Запуск тестирования сети, с желаемым классом
        private void testNetButton_Click(object sender, EventArgs e)
        {
            //проверка откл/вкл нейронов
            this.CheckNeurons();
            //тестирование
            double res = ANNUtils.testing(network, data, classes, classesList);
            this.errorTextBox.Invoke(new Action(() => this.errorTextBox.Text = res.ToString("F10")));
                
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
                    tempData = ANNUtils.normalization(tempData, minData, maxData, rowCountData, colCountData);

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
        

       //Запуск автоматическго отключения нейронов
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

        }

        //отключение нейронов и запись данных в файл
        private void offNeurons(int beginLayer, int endLayer, int beginNeuron, int endNeuron, int acount)
        {
            String time = LogHelper.getTime();

            fileXlsNeurons = new FileInfo(LogHelper.getPath("CrashNeurons") + "\\Результаты-" + time + ".xlsx");
            QualityWorkBook = new ExcelPackage(fileXlsNeurons);
            QualityWorkSheet = QualityWorkBook.Workbook.Worksheets.Add("Результаты отключения нейронов");

            fileXlsWeightsRel = new FileInfo(LogHelper.getPath("CrashNeurons") + "\\СуммаЗначОтклВесов-" + time + ".xlsx");
            WeightsSumWorkBook = new ExcelPackage(fileXlsWeightsRel);
            WeightsSumWorkSheet = WeightsSumWorkBook.Workbook.Worksheets.Add("Относительные суммы отключенных весов");

            fileXlsWeightsAbs = new FileInfo(LogHelper.getPath("CrashNeurons") + "\\АбсСуммаЗначОтклВесов-" + time + ".xlsx");
            AbsWeightsSumWorkBook = new ExcelPackage(fileXlsWeightsAbs);
            AbsWeightsSumWorkSheet = AbsWeightsSumWorkBook.Workbook.Worksheets.Add("Абсолютные суммы отключенных весов");

            //глобальный счетчик выделенных в диапазон нейронов
            int count = 0;
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
            QualityWorkSheet = LogHelper.setNeuronsWorkBookHeaders(QualityWorkSheet, rangeNeurons);
            WeightsSumWorkSheet = LogHelper.setWeightsWorkBookHeaders(WeightsSumWorkSheet, rangeNeurons);
            AbsWeightsSumWorkSheet = LogHelper.setWeightsWorkBookHeaders(AbsWeightsSumWorkSheet, rangeNeurons);

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
            int allCombinations = 0;

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

                    QualityWorkSheet.Cells[combinations + 1, 1].Value = verticalGroup;
                    WeightsSumWorkSheet.Cells[combinations + 1, 1].Value = verticalGroup;
                    AbsWeightsSumWorkSheet.Cells[combinations + 1, 1].Value = verticalGroup;

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

                        QualityWorkSheet.Cells[combinations + 1, j + 2].Value = ANNUtils.testing( network, data, classes, classesList );
                        DateTime t2 = DateTime.Now;
                        System.TimeSpan diffTime = t2.Subtract(t);
                        allCombinations++;
                        CombinationsLabel.Invoke(new Action(() => CombinationsLabel.Text = allCombinations.ToString()));
                        timeLabel.Invoke(new Action(() => timeLabel.Text = diffTime.ToString()));

                        WeightsSumWorkSheet.Cells[combinations + 1, step + 1].Value = offWeightsSumInput;
                        WeightsSumWorkSheet.Cells[combinations + 1, step + 2].Value = offWeightsSumOutput;
                        AbsWeightsSumWorkSheet.Cells[combinations + 1, step + 1].Value = offWeightsSumAbsoluteInput;
                        AbsWeightsSumWorkSheet.Cells[combinations + 1, step + 2].Value = offWeightsSumAbsoluteOutput;

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

                    QualityWorkSheet.Cells[combinations + 1, rangeNeurons.Length + 2].Value = fixedOffNeurons + 1;
                    QualityWorkSheet.Cells[combinations + 1, rangeNeurons.Length + 3].Value = ((double)(fixedOffNeurons + 1) / (double)ANNUtils.getNeuronsCount(network));
                    QualityWorkSheet.Cells[combinations + 1, rangeNeurons.Length + 4].Value = (fixedOffNeurons + 1).ToString() + "/" + ANNUtils.getNeuronsCount(network);

                    combinations++;
                    verticalGroup = "";
                    fixedOffNeurons = 0;

                }
            }
            QualityWorkBook.Save();

            WeightsSumWorkBook.Save();

            AbsWeightsSumWorkBook.Save();
            MessageBox.Show("Перебор окончен.");


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
            saveFileDialog1.Filter = "crash bin files (*.cbin)|*.cbin";
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

                    tempData = ANNUtils.normalization(tempData, minData, maxData, rowCountData, colCountData);

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

            FileInfo file = new FileInfo(LogHelper.getPath("Test") + "\\ВыходнойВектор-" + LogHelper.getTime() + ".xlsx");
            ExcelPackage book = new ExcelPackage(file);
            ExcelWorksheet worksheet = book.Workbook.Worksheets.Add("Выходной вектор");

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

                    worksheet.Cells[count + 1, 1].Value = classesList[ANNUtils.max(res)];
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ошибка тестирования сети." + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }

            }

            MessageBox.Show("Выходной вектор сформирован.");

            book.Save();
            return;
        }


        private int[] getDataOfANNProtocol(String input)
        {
            String[] parts = input.Split(':');
            int[] resParts = new int[3];

            resParts[0] = Int32.Parse(parts[0]);
            resParts[1] = Int32.Parse(parts[1]);
            resParts[2] = Int32.Parse(parts[2]);

            return resParts;
        }

        private void offWeights()
        {
            List<String> availableWeights = ANNUtils.getAvailableWeights(this.network);
            List<String> tempWeightCollection = new List<String>();
            double level = 0.0;
            double radius = 0.0;
            this.globalOffNeuronsCount = 0;
            try
            {
                level = Math.Max(0.0, Math.Min(100, double.Parse(this.offWeightsLevelBox.Text)));
            }
            catch
            {
                level = 80;
            }
            try
            {
                radius = Math.Max(0.0, Math.Min(level, double.Parse(this.radiusBox.Text)));
            }
            catch
            {
                radius = 15;
            }
            this.offWeightsLevelBox.Invoke(new Action(() => this.offWeightsLevelBox.Text = level.ToString()));
            this.radiusBox.Invoke(new Action(() => this.radiusBox.Text = radius.ToString()));
            
            int combinations = 0;
            DateTime t = DateTime.Now;

            while (!this.needToSpotWeightsOff)
            {
                for (int k = 0; k < availableWeights.Count - 2; k++)
                {
                    for (int i = 0; i < availableWeights.Count; i++)
                    {

                        int Layer = 0;
                        int Neuron = 0;
                        int Weight = 0;
                        String currentWeight = "";

                        for (int off = i; off <= i + k; off++)
                        {
                            //repeat around
                            if (off > availableWeights.Count - 1)
                            {
                                currentWeight = availableWeights[off - availableWeights.Count];

                            }
                            else
                            {
                                currentWeight = availableWeights[off];
                            }

                            int[] parts = getDataOfANNProtocol(currentWeight);
                            Layer = parts[0];
                            Neuron = parts[1];
                            Weight = parts[2];

                            //off current weights
                            tempWeights[Layer][Neuron][Weight][0] = network.Layers[Layer].Neurons[Neuron].Weights[Weight];
                            network.Layers[Layer].Neurons[Neuron].Weights[Weight] = 0.0;
                            draw();
                            this.globalOffNeuronsCount++;
                            tempWeightCollection.Add(currentWeight);

                            
                        }

                        //del currents weights 
                        for (int n = 0; n < tempWeightCollection.Count; n++)
                        {
                            availableWeights.Remove(tempWeightCollection[n]);
                        }


                        for (int j = 0; j < availableWeights.Count; j++)
                        {
                            String otherWeight = availableWeights[j];

                            int[] OtherParts = getDataOfANNProtocol(otherWeight);
                            int OtherLayer = OtherParts[0];
                            int OtherNeuron = OtherParts[1];
                            int OtherWeight = OtherParts[2];

                            //off other weight
                            tempWeights[OtherLayer][OtherNeuron][OtherWeight][0] = network.Layers[OtherLayer].Neurons[OtherNeuron].Weights[OtherWeight];
                            network.Layers[OtherLayer].Neurons[OtherNeuron].Weights[OtherWeight] = 0.0;
                        //    this.dynamicDrawWeights(OtherLayer, OtherNeuron, Color.Red);
                            draw();
                            this.globalOffNeuronsCount++;

                            double res = ANNUtils.testing(network, data, classes, classesList);
                            this.errorTextBox.Invoke(new Action(() => this.errorTextBox.Text = res.ToString("F10")));
                            combinations++;
                            this.CombinationsLabel.Invoke(new Action(() => this.CombinationsLabel.Text = combinations.ToString()));
                            this.offCountLabel.Invoke(new Action(() => this.offCountLabel.Text = this.globalOffNeuronsCount.ToString()));
                            DateTime t2 = DateTime.Now;
                            System.TimeSpan diffTime = t2.Subtract(t);
                            this.timeLabel.Invoke(new Action(() => this.timeLabel.Text = diffTime.ToString()));

                            if ((res >= radius) && (res <= level ))
                            {
                                //update grid
                                network.Layers[OtherLayer].Neurons[OtherNeuron].Weights[OtherWeight] = tempWeights[OtherLayer][OtherNeuron][OtherWeight][0];
                                //on current weight
                                for (int on = 0; on < tempWeightCollection.Count; on++)
                                {


                                    int[] parts = getDataOfANNProtocol(tempWeightCollection[on]);
                                    Layer = parts[0];
                                    Neuron = parts[1];
                                    Weight = parts[2];

                                    //on current weights    
                                    network.Layers[Layer].Neurons[Neuron].Weights[Weight] = tempWeights[Layer][Neuron][Weight][0];
                                  //  this.dynamicDrawWeights(Layer, Neuron, Color.Black);
                                    draw();
                                    availableWeights.Add(tempWeightCollection[on]);

                                }
                                tempWeightCollection.Clear();

                                updateGrid();
                                return;
                            }
                            //on other weight
                            network.Layers[OtherLayer].Neurons[OtherNeuron].Weights[OtherWeight] = tempWeights[OtherLayer][OtherNeuron][OtherWeight][0];
                            tempWeights[OtherLayer][OtherNeuron][OtherWeight][0] = 0.0;
                          //  this.dynamicDrawWeights(OtherLayer, OtherNeuron, Color.Black);
                            draw();
                            this.globalOffNeuronsCount--;

                            if (needToSpotWeightsOff)
                                break;
                        }

                        //on current weight
                        for (int on = 0; on < tempWeightCollection.Count; on++)
                        {


                            int[] parts = getDataOfANNProtocol(tempWeightCollection[on]);
                            Layer = parts[0];
                            Neuron = parts[1];
                            Weight = parts[2];

                            //on current weights    
                            network.Layers[Layer].Neurons[Neuron].Weights[Weight] = tempWeights[Layer][Neuron][Weight][0];
                            tempWeights[Layer][Neuron][Weight][0] = 0.0;
                            draw();
                            this.globalOffNeuronsCount--;

                            availableWeights.Add(tempWeightCollection[on]);

                        }
                        tempWeightCollection.Clear();

                        if (needToSpotWeightsOff)
                            break;

                        availableWeights.Sort();
                    }
                    if (needToSpotWeightsOff)
                        break;
                }
            }
            needToSpotWeightsOff = false;
            return;
        }

        private void dynamicDrawWeights(int Layer, int Neuron, Color color)
        {
            int x1 = 50 + (Layer * 200);
            int y1 = 50 * Neuron + (50 / 2);

            int x2 = 200 + (Layer * 200);
            int y2 = 50 * Neuron + (50 / 2);

            Pen pen = new Pen(color);
            Bitmap bmp;
            Graphics formGraphics;

            bmp = new Bitmap(this.pictureBox1.Image);
            formGraphics = Graphics.FromImage(bmp);
            formGraphics.DrawLine(pen, new Point(x2, y2), new Point(x1, y1));
            this.pictureBox1.Invoke(new Action(() => this.pictureBox1.Image = bmp));
            return;
        }
        private void startOffWeightsButton_Click(object sender, EventArgs e)
        {
            Worker = new Thread(() => offWeights());
            Worker.Start();

        }

        private void updateGrid()
        {
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                int layer = Int32.Parse(this.dataGridView1.Rows[i].Cells[0].Value.ToString());
                int neuron = Int32.Parse(this.dataGridView1.Rows[i].Cells[1].Value.ToString());
                int weight = Int32.Parse(this.dataGridView1.Rows[i].Cells[2].Value.ToString());

                if (tempWeights[layer][neuron][weight][0] != 0.0)
                {
                    this.dataGridView1.Rows[i].Cells[4].Value = "T";
                }
            }
        }

        private void stopWeightsButton_Click(object sender, EventArgs e)
        {
            this.needToSpotWeightsOff = true;
        }



    }

}
