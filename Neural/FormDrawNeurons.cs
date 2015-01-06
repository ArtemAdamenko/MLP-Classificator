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
using AForge.Controls;
using AForge.Neuro;
using AForge;


namespace Neural
{
    public partial class FormDrawNeurons : Form
    {
        //Neural Net options
        private double[,] data = null;
        private double[][][][] tempWeights;
        int rowCountData = 0;
        int colCountData = 0;
        private int[] classes;
        private int classesCount;
        private int[] samplesPerClass;

        //draw options
        private SolidBrush _myBrush = new SolidBrush(Color.Blue);
        private SolidBrush _offBrush = new SolidBrush(Color.Gray);
        private Pen _myPen = new Pen(Color.Black);

        //App options
        Network network = null;
        Thread Worker;

        public FormDrawNeurons()
        {
            InitializeComponent();
        }

        public void draw()
        {
            Bitmap bmp;
            Graphics formGraphics;

            // Create a bitmap the size of the form.
            //first layer does not contains in network.Layers, because +1
            bmp = new Bitmap(700, 530);

            formGraphics = Graphics.FromImage(bmp);
            int x = 0;

            //draw input layer
            int cntInput = network.InputsCount;
            _myPen.Width = 3;
            System.Drawing.Point[] fisrtPoints = new System.Drawing.Point[cntInput];
            //default color
            _myPen.Color = Color.Black;

            for (int k = 0; k < cntInput; k++)
            {
                Rectangle ellipse = new Rectangle(x, 50 * k, 100, 50);
                formGraphics.FillEllipse(this._myBrush, ellipse);
                formGraphics.DrawEllipse(this._myPen, ellipse);
                //draw string in ellipse
                formGraphics.DrawString("Input[" + k.ToString() + "]",
                                    new Font("Arial", 11, FontStyle.Bold),
                                    new SolidBrush(Color.Gray),
                                    new System.Drawing.Point(x + 20, 50 * k + 10));
                fisrtPoints[k].X = x + 100;
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
                    Rectangle ellipse = new Rectangle(x, 50 * j, 100, 50);
                    formGraphics.FillEllipse(this._myBrush, ellipse);
                    formGraphics.DrawEllipse(this._myPen, ellipse);
                    //draw string in ellipse
                    formGraphics.DrawString("L[" + i.ToString() + "]N[" + j.ToString() + "]",
                                    new Font("Arial", 11, FontStyle.Bold),
                                    new SolidBrush(Color.Black),
                                    new System.Drawing.Point(x + 20, 50 * j + 10));

                    hiddenLeftPoints[j].X = x;
                    hiddenLeftPoints[j].Y = 50 * j + (50 / 2);

                    hiddenRightPoints[j].X = x + 100;
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



        /*
         * Заполняет таблицу значениями 
         * слоев нейронов и весов соответственно
         */
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

        /**
         * Запуск потока загрузки нейронной сети
         * */
        private void LoadNetToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Worker = new Thread(LoadNet);
            Worker.SetApartmentState(ApartmentState.STA);
            Worker.Start();
            
        }

        /**
         * Загрузка нейронной сети
         * */
        private void LoadNet()
        {
            // Initialize the OpenFileDialog to look for text files.
            openFileDialog1.Filter = "Bin Files|*.bin";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    network = Network.Load(openFileDialog1.FileName);
                }
                catch (IOException)
                {
                    throw new IOException("Ошибка загрузки нейронной сети");
                }
                finally
                {
                    this.Invoke(new Action(InitWork));
                    this.Invoke(new Action(draw));
                    Worker.Abort();
                }
            }
        }


        /**
         * Инициализация компонентов для работы
         * */
        private void InitWork()
        {
            this.setNeuronsDataGrid();

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

        /**
         * Загрузка выборки
         * */
        private void loadTestData()
        {

            // show file selection dialog
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;
                // min and max X values
                float minX = float.MaxValue;
                float maxX = float.MinValue;

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
                    Array.Copy(tempClasses, 0, classes, 0, i);
                    this.testNetButton.Invoke(new Action(() => this.testNetButton.Enabled = true));

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
        }

        /**
         * Вызов потока загрузки выборки
         * */
        private void LoadDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Worker = new Thread(loadTestData);
            Worker.SetApartmentState(ApartmentState.STA);
            Worker.Start();
        }

        /**
         * Процент ошибки при тестировании на выбранной выборке
         * */
        private void testNetButton_Click(object sender, EventArgs e)
        {
            this.CheckNeurons();
            double[] validateError = new double[classesCount];
            double error = 0.0;
            try
            {
                double[] input = new double[colCountData - 1];

                for (int count = 0; count < data.GetLength(0) - 1; count++)
                {
                    //gather inputs for compute, n-1 inputs
                    for (int i = 0; i < colCountData - 1; i++)
                    {
                        input[i] = data[count, i];
                    }
                    validateError = network.Compute(input);
                    int j = 0;
                    for (j = 0; j < classesCount; j++)
                    {
                        if (validateError[j] == 1)
                            break;
                    }

                    error += Math.Abs(j - classes[count]);
                }
                this.errorTextBox.Text = (error / data.GetLength(0)).ToString("F10");
            }
            catch(Exception)
                {
                    MessageBox.Show("Ошибка тестирования сети.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
        }


    }
}
