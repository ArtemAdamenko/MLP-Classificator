using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using System.IO;
using System.Threading;


namespace Neural
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public partial class LearnANNForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox learningRateBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox currentIterationBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LearnANNForm));
            this.label4 = new System.Windows.Forms.Label();
            this.inputCountBox = new System.Windows.Forms.TextBox();
            this.fileTextBox = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.neuronsBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.momentBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.algoritmBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.alphaBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.classesBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.learningRateBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pLabel = new System.Windows.Forms.Label();
            this.probabilisticValidBox = new System.Windows.Forms.TextBox();
            this.mLabel = new System.Windows.Forms.Label();
            this.moduleValidBox = new System.Windows.Forms.TextBox();
            this.errorPercent = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.currentIterationBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.нейроннаяСетьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveNetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveWeightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TestNetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crossValidToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выборкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTrainDataButton = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lastRunsGridView = new System.Windows.Forms.DataGridView();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.probabilisticCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.maxIterationsBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.maxNeuronsInLayerBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.validationLevelBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lastRunsGridView)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Кол-во входов:";
            // 
            // inputCountBox
            // 
            this.inputCountBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputCountBox.Location = new System.Drawing.Point(100, 42);
            this.inputCountBox.Name = "inputCountBox";
            this.inputCountBox.ReadOnly = true;
            this.inputCountBox.Size = new System.Drawing.Size(86, 21);
            this.inputCountBox.TabIndex = 6;
            // 
            // fileTextBox
            // 
            this.fileTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileTextBox.Location = new System.Drawing.Point(100, 14);
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.ReadOnly = true;
            this.fileTextBox.Size = new System.Drawing.Size(86, 21);
            this.fileTextBox.TabIndex = 5;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            this.openFileDialog.Title = "Select data file";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.neuronsBox);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.momentBox);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.algoritmBox);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.alphaBox);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.classesBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.inputCountBox);
            this.groupBox3.Controls.Add(this.fileTextBox);
            this.groupBox3.Controls.Add(this.learningRateBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox3.Location = new System.Drawing.Point(707, 29);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(195, 253);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Настройки";
            // 
            // neuronsBox
            // 
            this.neuronsBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.neuronsBox.Location = new System.Drawing.Point(99, 125);
            this.neuronsBox.Name = "neuronsBox";
            this.neuronsBox.Size = new System.Drawing.Size(87, 21);
            this.neuronsBox.TabIndex = 18;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 128);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(91, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "Нейроны и слои:";
            // 
            // momentBox
            // 
            this.momentBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.momentBox.Location = new System.Drawing.Point(99, 224);
            this.momentBox.Name = "momentBox";
            this.momentBox.ReadOnly = true;
            this.momentBox.Size = new System.Drawing.Size(85, 21);
            this.momentBox.TabIndex = 16;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 227);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Момент:";
            // 
            // algoritmBox
            // 
            this.algoritmBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.algoritmBox.FormattingEnabled = true;
            this.algoritmBox.Items.AddRange(new object[] {
            "BackProp",
            "RProp"});
            this.algoritmBox.Location = new System.Drawing.Point(7, 166);
            this.algoritmBox.Name = "algoritmBox";
            this.algoritmBox.Size = new System.Drawing.Size(183, 21);
            this.algoritmBox.TabIndex = 14;
            this.algoritmBox.SelectedValueChanged += new System.EventHandler(this.algoritmBox_SelectedValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 150);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(110, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Алгоритм обучения:";
            // 
            // alphaBox
            // 
            this.alphaBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.alphaBox.Location = new System.Drawing.Point(100, 98);
            this.alphaBox.Name = "alphaBox";
            this.alphaBox.Size = new System.Drawing.Size(86, 21);
            this.alphaBox.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 101);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Альфа:";
            // 
            // classesBox
            // 
            this.classesBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.classesBox.Location = new System.Drawing.Point(100, 70);
            this.classesBox.Name = "classesBox";
            this.classesBox.ReadOnly = true;
            this.classesBox.Size = new System.Drawing.Size(86, 21);
            this.classesBox.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Кол-во классов:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Имя файла:";
            // 
            // learningRateBox
            // 
            this.learningRateBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.learningRateBox.Location = new System.Drawing.Point(98, 196);
            this.learningRateBox.Name = "learningRateBox";
            this.learningRateBox.ReadOnly = true;
            this.learningRateBox.Size = new System.Drawing.Size(86, 21);
            this.learningRateBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 199);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Коэф. обучения:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.pLabel);
            this.groupBox4.Controls.Add(this.probabilisticValidBox);
            this.groupBox4.Controls.Add(this.mLabel);
            this.groupBox4.Controls.Add(this.moduleValidBox);
            this.groupBox4.Controls.Add(this.errorPercent);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.currentIterationBox);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(707, 405);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(195, 149);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Текущая итерация";
            // 
            // pLabel
            // 
            this.pLabel.AutoSize = true;
            this.pLabel.Location = new System.Drawing.Point(4, 89);
            this.pLabel.Name = "pLabel";
            this.pLabel.Size = new System.Drawing.Size(86, 13);
            this.pLabel.TabIndex = 8;
            this.pLabel.Text = "% Вероятности";
            // 
            // probabilisticValidBox
            // 
            this.probabilisticValidBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.probabilisticValidBox.Location = new System.Drawing.Point(119, 86);
            this.probabilisticValidBox.Name = "probabilisticValidBox";
            this.probabilisticValidBox.Size = new System.Drawing.Size(66, 21);
            this.probabilisticValidBox.TabIndex = 7;
            // 
            // mLabel
            // 
            this.mLabel.AutoSize = true;
            this.mLabel.Location = new System.Drawing.Point(3, 122);
            this.mLabel.Name = "mLabel";
            this.mLabel.Size = new System.Drawing.Size(77, 13);
            this.mLabel.TabIndex = 6;
            this.mLabel.Text = "% По модулю";
            // 
            // moduleValidBox
            // 
            this.moduleValidBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.moduleValidBox.Location = new System.Drawing.Point(118, 119);
            this.moduleValidBox.Name = "moduleValidBox";
            this.moduleValidBox.Size = new System.Drawing.Size(66, 21);
            this.moduleValidBox.TabIndex = 5;
            // 
            // errorPercent
            // 
            this.errorPercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorPercent.Location = new System.Drawing.Point(118, 17);
            this.errorPercent.Name = "errorPercent";
            this.errorPercent.Size = new System.Drawing.Size(66, 21);
            this.errorPercent.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 21);
            this.label3.TabIndex = 2;
            this.label3.Text = "% Обучения:";
            // 
            // currentIterationBox
            // 
            this.currentIterationBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentIterationBox.Location = new System.Drawing.Point(118, 52);
            this.currentIterationBox.Name = "currentIterationBox";
            this.currentIterationBox.Size = new System.Drawing.Size(66, 21);
            this.currentIterationBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(18, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "Итерация:";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopButton.Location = new System.Drawing.Point(108, 20);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(81, 25);
            this.stopButton.TabIndex = 8;
            this.stopButton.Text = "Остановить";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startButton.Location = new System.Drawing.Point(6, 20);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(105, 25);
            this.startButton.TabIndex = 7;
            this.startButton.Text = "Начать обучение";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.startButton);
            this.groupBox5.Controls.Add(this.stopButton);
            this.groupBox5.Location = new System.Drawing.Point(707, 560);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(195, 57);
            this.groupBox5.TabIndex = 11;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = " Обучение";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.нейроннаяСетьToolStripMenuItem,
            this.выборкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(906, 24);
            this.menuStrip1.TabIndex = 19;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // нейроннаяСетьToolStripMenuItem
            // 
            this.нейроннаяСетьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveToolStripMenuItem,
            this.TestNetToolStripMenuItem,
            this.crossValidToolStripMenuItem});
            this.нейроннаяСетьToolStripMenuItem.Name = "нейроннаяСетьToolStripMenuItem";
            this.нейроннаяСетьToolStripMenuItem.Size = new System.Drawing.Size(100, 20);
            this.нейроннаяСетьToolStripMenuItem.Text = "Нейронная сеть";
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveNetToolStripMenuItem,
            this.SaveWeightsToolStripMenuItem});
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.SaveToolStripMenuItem.Text = "Сохранить";
            // 
            // SaveNetToolStripMenuItem
            // 
            this.SaveNetToolStripMenuItem.Name = "SaveNetToolStripMenuItem";
            this.SaveNetToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.SaveNetToolStripMenuItem.Text = "Сеть";
            this.SaveNetToolStripMenuItem.Click += new System.EventHandler(this.SaveNetToolStripMenuItem_Click);
            // 
            // SaveWeightsToolStripMenuItem
            // 
            this.SaveWeightsToolStripMenuItem.Name = "SaveWeightsToolStripMenuItem";
            this.SaveWeightsToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.SaveWeightsToolStripMenuItem.Text = "Веса";
            this.SaveWeightsToolStripMenuItem.Click += new System.EventHandler(this.SaveWeightsToolStripMenuItem_Click);
            // 
            // TestNetToolStripMenuItem
            // 
            this.TestNetToolStripMenuItem.Name = "TestNetToolStripMenuItem";
            this.TestNetToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.TestNetToolStripMenuItem.Text = "Тестировать";
            this.TestNetToolStripMenuItem.Click += new System.EventHandler(this.TestNetToolStripMenuItem_Click);
            // 
            // crossValidToolStripMenuItem
            // 
            this.crossValidToolStripMenuItem.Name = "crossValidToolStripMenuItem";
            this.crossValidToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.crossValidToolStripMenuItem.Text = "Кросс-валидация";
            this.crossValidToolStripMenuItem.Click += new System.EventHandler(this.crossValidToolStripMenuItem_Click);
            // 
            // выборкаToolStripMenuItem
            // 
            this.выборкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadTrainDataButton});
            this.выборкаToolStripMenuItem.Name = "выборкаToolStripMenuItem";
            this.выборкаToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.выборкаToolStripMenuItem.Text = "Выборка";
            // 
            // loadTrainDataButton
            // 
            this.loadTrainDataButton.Name = "loadTrainDataButton";
            this.loadTrainDataButton.Size = new System.Drawing.Size(124, 22);
            this.loadTrainDataButton.Text = "Обучения";
            this.loadTrainDataButton.Click += new System.EventHandler(this.loadTrainDataButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.zedGraphControl1);
            this.groupBox2.Location = new System.Drawing.Point(12, 29);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(689, 595);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Изменение ошибки обучения";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lastRunsGridView);
            this.groupBox1.Location = new System.Drawing.Point(6, 358);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(677, 230);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Последние запуски";
            // 
            // lastRunsGridView
            // 
            this.lastRunsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.lastRunsGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.lastRunsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lastRunsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.Column6,
            this.Column7,
            this.probabilisticCol,
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column12});
            this.lastRunsGridView.Location = new System.Drawing.Point(6, 17);
            this.lastRunsGridView.Name = "lastRunsGridView";
            this.lastRunsGridView.RowHeadersVisible = false;
            this.lastRunsGridView.Size = new System.Drawing.Size(658, 207);
            this.lastRunsGridView.TabIndex = 0;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Итерация";
            this.Column5.Name = "Column5";
            this.Column5.Width = 81;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Ошибка";
            this.Column6.Name = "Column6";
            this.Column6.Width = 72;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Валидация(модуль)";
            this.Column7.Name = "Column7";
            this.Column7.Width = 132;
            // 
            // probabilisticCol
            // 
            this.probabilisticCol.HeaderText = "Валидация(вер.)";
            this.probabilisticCol.Name = "probabilisticCol";
            this.probabilisticCol.Width = 117;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Алгоритм";
            this.Column8.Name = "Column8";
            this.Column8.Width = 80;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Топология";
            this.Column9.Name = "Column9";
            this.Column9.Width = 85;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "Альфа";
            this.Column10.Name = "Column10";
            this.Column10.Width = 65;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Коэф.обучения";
            this.Column11.Name = "Column11";
            this.Column11.Width = 110;
            // 
            // Column12
            // 
            this.Column12.HeaderText = "Момент";
            this.Column12.Name = "Column12";
            this.Column12.Width = 70;
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.zedGraphControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.zedGraphControl1.Location = new System.Drawing.Point(6, 13);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(677, 334);
            this.zedGraphControl1.TabIndex = 1;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.maxIterationsBox);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this.maxNeuronsInLayerBox);
            this.groupBox6.Controls.Add(this.label13);
            this.groupBox6.Controls.Add(this.validationLevelBox);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Location = new System.Drawing.Point(707, 289);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(195, 109);
            this.groupBox6.TabIndex = 21;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Добавление нейронов";
            // 
            // maxIterationsBox
            // 
            this.maxIterationsBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxIterationsBox.Location = new System.Drawing.Point(127, 75);
            this.maxIterationsBox.Name = "maxIterationsBox";
            this.maxIterationsBox.Size = new System.Drawing.Size(61, 21);
            this.maxIterationsBox.TabIndex = 5;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 82);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(111, 13);
            this.label14.TabIndex = 4;
            this.label14.Text = "Максимум итераций:";
            // 
            // maxNeuronsInLayerBox
            // 
            this.maxNeuronsInLayerBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxNeuronsInLayerBox.Location = new System.Drawing.Point(127, 47);
            this.maxNeuronsInLayerBox.Name = "maxNeuronsInLayerBox";
            this.maxNeuronsInLayerBox.Size = new System.Drawing.Size(61, 21);
            this.maxNeuronsInLayerBox.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(5, 51);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(95, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "Нейронов в слое:";
            // 
            // validationLevelBox
            // 
            this.validationLevelBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.validationLevelBox.Location = new System.Drawing.Point(127, 19);
            this.validationLevelBox.Name = "validationLevelBox";
            this.validationLevelBox.Size = new System.Drawing.Size(61, 21);
            this.validationLevelBox.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(124, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Уровень обучения(%):";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // LearnANNForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(906, 621);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "LearnANNForm";
            this.Text = "ANNBuilder: Обучение многослойного персептрона";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lastRunsGridView)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private GroupBox groupBox5;
        private TextBox errorPercent;
        private Label mLabel;
        private TextBox moduleValidBox;
        private SaveFileDialog saveFileDialog1;
        private MenuStrip menuStrip1;
        private GroupBox groupBox2;
        private Label label4;
        private TextBox inputCountBox;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private TextBox fileTextBox;
        private ToolStripMenuItem нейроннаяСетьToolStripMenuItem;
        private ToolStripMenuItem SaveToolStripMenuItem;
        private ToolStripMenuItem TestNetToolStripMenuItem;
        private ToolStripMenuItem SaveNetToolStripMenuItem;
        private ToolStripMenuItem SaveWeightsToolStripMenuItem;
        private ToolStripMenuItem выборкаToolStripMenuItem;
        private ToolStripMenuItem loadTrainDataButton;
        private TextBox classesBox;
        private Label label6;
        private Label label2;
        private TextBox momentBox;
        private Label label10;
        private ComboBox algoritmBox;
        private Label label9;
        private TextBox alphaBox;
        private Label label8;
        private GroupBox groupBox1;
        private TextBox neuronsBox;
        private Label label11;
        private DataGridView lastRunsGridView;
        private GroupBox groupBox6;
        private TextBox maxIterationsBox;
        private Label label14;
        private TextBox maxNeuronsInLayerBox;
        private Label label13;
        private TextBox validationLevelBox;
        private Label label12;
        private ToolStripMenuItem crossValidToolStripMenuItem;
        private IContainer components;
        private OpenFileDialog openFileDialog1;
        private Label pLabel;
        private TextBox probabilisticValidBox;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn probabilisticCol;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column10;
        private DataGridViewTextBoxColumn Column11;
        private DataGridViewTextBoxColumn Column12;

    }
}
