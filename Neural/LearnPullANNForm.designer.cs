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
    public partial class LearnPullANNForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox currentIterationBox;
        private System.Windows.Forms.Label label5;

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
            this.label4 = new System.Windows.Forms.Label();
            this.inputCountBox = new System.Windows.Forms.TextBox();
            this.fileTextBox = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.percentChangeBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.weightsChangeBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.minWeightValueBox = new System.Windows.Forms.TextBox();
            this.maxWeightValueBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.randomCountWeightsBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.neuronsBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.alphaBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.classesBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pLabel = new System.Windows.Forms.Label();
            this.probabilisticValidBox = new System.Windows.Forms.TextBox();
            this.mLabel = new System.Windows.Forms.Label();
            this.moduleValidBox = new System.Windows.Forms.TextBox();
            this.currentIterationBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
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
            this.weightsView = new System.Windows.Forms.DataGridView();
            this.index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Valid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.initWeightValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lastRunsGridView = new System.Windows.Forms.DataGridView();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.probabilisticCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.sortSignAbsSortBox = new System.Windows.Forms.TextBox();
            this.sortSignNumSortBox = new System.Windows.Forms.TextBox();
            this.sortByLayersBox = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.sortSignFromToSortBox = new System.Windows.Forms.TextBox();
            this.sortSignAbsSumSortBox = new System.Windows.Forms.TextBox();
            this.bySumWeightsBox = new System.Windows.Forms.CheckBox();
            this.fromInputToOutputBox = new System.Windows.Forms.CheckBox();
            this.sortByModuleAscBox = new System.Windows.Forms.CheckBox();
            this.sortByNumBox = new System.Windows.Forms.CheckBox();
            this.limitRepeatBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.repeatPullBox = new System.Windows.Forms.CheckBox();
            this.allWeightsBox = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.weightsView)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lastRunsGridView)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox5.SuspendLayout();
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
            this.inputCountBox.Location = new System.Drawing.Point(182, 41);
            this.inputCountBox.Name = "inputCountBox";
            this.inputCountBox.ReadOnly = true;
            this.inputCountBox.Size = new System.Drawing.Size(86, 21);
            this.inputCountBox.TabIndex = 6;
            // 
            // fileTextBox
            // 
            this.fileTextBox.Location = new System.Drawing.Point(182, 13);
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
            this.groupBox3.Controls.Add(this.groupBox9);
            this.groupBox3.Controls.Add(this.minWeightValueBox);
            this.groupBox3.Controls.Add(this.maxWeightValueBox);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.randomCountWeightsBox);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.neuronsBox);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.alphaBox);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.classesBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.inputCountBox);
            this.groupBox3.Controls.Add(this.fileTextBox);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox3.Location = new System.Drawing.Point(742, 29);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(274, 305);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Настройки";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.percentChangeBox);
            this.groupBox9.Controls.Add(this.label13);
            this.groupBox9.Controls.Add(this.weightsChangeBox);
            this.groupBox9.Controls.Add(this.label12);
            this.groupBox9.Location = new System.Drawing.Point(6, 177);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(260, 72);
            this.groupBox9.TabIndex = 30;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Изменение веса";
            // 
            // percentChangeBox
            // 
            this.percentChangeBox.Location = new System.Drawing.Point(167, 44);
            this.percentChangeBox.Name = "percentChangeBox";
            this.percentChangeBox.Size = new System.Drawing.Size(87, 21);
            this.percentChangeBox.TabIndex = 29;
            this.percentChangeBox.TextChanged += new System.EventHandler(this.percentChangeBox_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 46);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 13);
            this.label13.TabIndex = 28;
            this.label13.Text = "Относительн.";
            // 
            // weightsChangeBox
            // 
            this.weightsChangeBox.Location = new System.Drawing.Point(167, 17);
            this.weightsChangeBox.Name = "weightsChangeBox";
            this.weightsChangeBox.Size = new System.Drawing.Size(86, 21);
            this.weightsChangeBox.TabIndex = 23;
            this.weightsChangeBox.TextChanged += new System.EventHandler(this.weightsChangeBox_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 25);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "Абсолютн.";
            // 
            // minWeightValueBox
            // 
            this.minWeightValueBox.Location = new System.Drawing.Point(181, 278);
            this.minWeightValueBox.Name = "minWeightValueBox";
            this.minWeightValueBox.Size = new System.Drawing.Size(87, 21);
            this.minWeightValueBox.TabIndex = 26;
            // 
            // maxWeightValueBox
            // 
            this.maxWeightValueBox.Location = new System.Drawing.Point(181, 255);
            this.maxWeightValueBox.Name = "maxWeightValueBox";
            this.maxWeightValueBox.Size = new System.Drawing.Size(87, 21);
            this.maxWeightValueBox.TabIndex = 25;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 280);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "Нижняя граница:";
            // 
            // randomCountWeightsBox
            // 
            this.randomCountWeightsBox.Location = new System.Drawing.Point(223, 153);
            this.randomCountWeightsBox.Name = "randomCountWeightsBox";
            this.randomCountWeightsBox.Size = new System.Drawing.Size(45, 21);
            this.randomCountWeightsBox.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 257);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Верхняя граница:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Кол-во случайных весов:";
            // 
            // neuronsBox
            // 
            this.neuronsBox.Location = new System.Drawing.Point(181, 124);
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
            // alphaBox
            // 
            this.alphaBox.Location = new System.Drawing.Point(182, 97);
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
            this.classesBox.Location = new System.Drawing.Point(182, 69);
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
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.pLabel);
            this.groupBox4.Controls.Add(this.probabilisticValidBox);
            this.groupBox4.Controls.Add(this.mLabel);
            this.groupBox4.Controls.Add(this.moduleValidBox);
            this.groupBox4.Controls.Add(this.currentIterationBox);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(742, 602);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(274, 107);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Текущая итерация";
            // 
            // pLabel
            // 
            this.pLabel.AutoSize = true;
            this.pLabel.Location = new System.Drawing.Point(6, 48);
            this.pLabel.Name = "pLabel";
            this.pLabel.Size = new System.Drawing.Size(86, 13);
            this.pLabel.TabIndex = 8;
            this.pLabel.Text = "% Вероятности";
            // 
            // probabilisticValidBox
            // 
            this.probabilisticValidBox.Location = new System.Drawing.Point(193, 48);
            this.probabilisticValidBox.Name = "probabilisticValidBox";
            this.probabilisticValidBox.Size = new System.Drawing.Size(66, 21);
            this.probabilisticValidBox.TabIndex = 7;
            // 
            // mLabel
            // 
            this.mLabel.AutoSize = true;
            this.mLabel.Location = new System.Drawing.Point(9, 80);
            this.mLabel.Name = "mLabel";
            this.mLabel.Size = new System.Drawing.Size(77, 13);
            this.mLabel.TabIndex = 6;
            this.mLabel.Text = "% По модулю";
            // 
            // moduleValidBox
            // 
            this.moduleValidBox.Location = new System.Drawing.Point(193, 80);
            this.moduleValidBox.Name = "moduleValidBox";
            this.moduleValidBox.Size = new System.Drawing.Size(66, 21);
            this.moduleValidBox.TabIndex = 5;
            // 
            // currentIterationBox
            // 
            this.currentIterationBox.Location = new System.Drawing.Point(193, 13);
            this.currentIterationBox.Name = "currentIterationBox";
            this.currentIterationBox.Size = new System.Drawing.Size(66, 21);
            this.currentIterationBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "Итерация:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.нейроннаяСетьToolStripMenuItem,
            this.выборкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1028, 24);
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
            this.нейроннаяСетьToolStripMenuItem.Size = new System.Drawing.Size(107, 20);
            this.нейроннаяСетьToolStripMenuItem.Text = "Нейронная сеть";
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveNetToolStripMenuItem,
            this.SaveWeightsToolStripMenuItem});
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
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
            this.TestNetToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.TestNetToolStripMenuItem.Text = "Тестировать";
            this.TestNetToolStripMenuItem.Click += new System.EventHandler(this.TestNetToolStripMenuItem_Click);
            // 
            // crossValidToolStripMenuItem
            // 
            this.crossValidToolStripMenuItem.Name = "crossValidToolStripMenuItem";
            this.crossValidToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.crossValidToolStripMenuItem.Text = "Кросс-валидация";
            this.crossValidToolStripMenuItem.Click += new System.EventHandler(this.crossValidToolStripMenuItem_Click);
            // 
            // выборкаToolStripMenuItem
            // 
            this.выборкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadTrainDataButton});
            this.выборкаToolStripMenuItem.Name = "выборкаToolStripMenuItem";
            this.выборкаToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.выборкаToolStripMenuItem.Text = "Выборка";
            // 
            // loadTrainDataButton
            // 
            this.loadTrainDataButton.Name = "loadTrainDataButton";
            this.loadTrainDataButton.Size = new System.Drawing.Size(129, 22);
            this.loadTrainDataButton.Text = "Обучения";
            this.loadTrainDataButton.Click += new System.EventHandler(this.loadTrainDataButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.weightsView);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.zedGraphControl1);
            this.groupBox2.Location = new System.Drawing.Point(12, 29);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(724, 743);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Изменение ошибки обучения";
            // 
            // weightsView
            // 
            this.weightsView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.weightsView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.weightsView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.weightsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.weightsView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.index,
            this.value,
            this.Valid,
            this.initWeightValue});
            this.weightsView.Location = new System.Drawing.Point(480, 13);
            this.weightsView.Name = "weightsView";
            this.weightsView.RowHeadersVisible = false;
            this.weightsView.RowTemplate.Height = 23;
            this.weightsView.Size = new System.Drawing.Size(238, 723);
            this.weightsView.TabIndex = 6;
            // 
            // index
            // 
            this.index.HeaderText = "Индекс";
            this.index.Name = "index";
            this.index.Width = 69;
            // 
            // value
            // 
            this.value.HeaderText = "Вес";
            this.value.Name = "value";
            this.value.Width = 49;
            // 
            // Valid
            // 
            this.Valid.HeaderText = "Ошибка";
            this.Valid.Name = "Valid";
            this.Valid.Width = 72;
            // 
            // initWeightValue
            // 
            this.initWeightValue.HeaderText = "Нач.значение";
            this.initWeightValue.Name = "initWeightValue";
            this.initWeightValue.Width = 102;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lastRunsGridView);
            this.groupBox1.Location = new System.Drawing.Point(6, 465);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(468, 272);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Последние запуски";
            // 
            // lastRunsGridView
            // 
            this.lastRunsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.lastRunsGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.lastRunsGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lastRunsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lastRunsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.Column7,
            this.probabilisticCol,
            this.Column9,
            this.Column10});
            this.lastRunsGridView.Location = new System.Drawing.Point(6, 17);
            this.lastRunsGridView.Name = "lastRunsGridView";
            this.lastRunsGridView.RowHeadersVisible = false;
            this.lastRunsGridView.Size = new System.Drawing.Size(462, 249);
            this.lastRunsGridView.TabIndex = 0;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Итерация";
            this.Column5.Name = "Column5";
            this.Column5.Width = 81;
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
            // zedGraphControl1
            // 
            this.zedGraphControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.zedGraphControl1.Location = new System.Drawing.Point(12, 13);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(462, 454);
            this.zedGraphControl1.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox8);
            this.groupBox6.Controls.Add(this.limitRepeatBox);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.repeatPullBox);
            this.groupBox6.Controls.Add(this.allWeightsBox);
            this.groupBox6.Location = new System.Drawing.Point(742, 340);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(274, 256);
            this.groupBox6.TabIndex = 21;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Опции";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.sortSignAbsSortBox);
            this.groupBox8.Controls.Add(this.sortSignNumSortBox);
            this.groupBox8.Controls.Add(this.sortByLayersBox);
            this.groupBox8.Controls.Add(this.groupBox7);
            this.groupBox8.Controls.Add(this.sortByModuleAscBox);
            this.groupBox8.Controls.Add(this.sortByNumBox);
            this.groupBox8.Location = new System.Drawing.Point(6, 88);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(259, 162);
            this.groupBox8.TabIndex = 12;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Сортировка весов";
            // 
            // sortSignAbsSortBox
            // 
            this.sortSignAbsSortBox.Location = new System.Drawing.Point(208, 20);
            this.sortSignAbsSortBox.Name = "sortSignAbsSortBox";
            this.sortSignAbsSortBox.Size = new System.Drawing.Size(45, 21);
            this.sortSignAbsSortBox.TabIndex = 13;
            this.sortSignAbsSortBox.Text = "1";
            this.sortSignAbsSortBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // sortSignNumSortBox
            // 
            this.sortSignNumSortBox.Location = new System.Drawing.Point(208, 47);
            this.sortSignNumSortBox.Name = "sortSignNumSortBox";
            this.sortSignNumSortBox.Size = new System.Drawing.Size(45, 21);
            this.sortSignNumSortBox.TabIndex = 12;
            this.sortSignNumSortBox.Text = "1";
            this.sortSignNumSortBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // sortByLayersBox
            // 
            this.sortByLayersBox.AutoSize = true;
            this.sortByLayersBox.Enabled = false;
            this.sortByLayersBox.Location = new System.Drawing.Point(6, 66);
            this.sortByLayersBox.Name = "sortByLayersBox";
            this.sortByLayersBox.Size = new System.Drawing.Size(115, 17);
            this.sortByLayersBox.TabIndex = 8;
            this.sortByLayersBox.Text = "Упоряд. по слоям";
            this.sortByLayersBox.UseVisualStyleBackColor = true;
            this.sortByLayersBox.CheckedChanged += new System.EventHandler(this.sortByLayersBox_CheckedChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.sortSignFromToSortBox);
            this.groupBox7.Controls.Add(this.sortSignAbsSumSortBox);
            this.groupBox7.Controls.Add(this.bySumWeightsBox);
            this.groupBox7.Controls.Add(this.fromInputToOutputBox);
            this.groupBox7.Location = new System.Drawing.Point(13, 89);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(240, 69);
            this.groupBox7.TabIndex = 11;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Сортировка по слоям";
            // 
            // sortSignFromToSortBox
            // 
            this.sortSignFromToSortBox.Enabled = false;
            this.sortSignFromToSortBox.Location = new System.Drawing.Point(189, 16);
            this.sortSignFromToSortBox.Name = "sortSignFromToSortBox";
            this.sortSignFromToSortBox.Size = new System.Drawing.Size(45, 21);
            this.sortSignFromToSortBox.TabIndex = 12;
            this.sortSignFromToSortBox.Text = "1";
            this.sortSignFromToSortBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // sortSignAbsSumSortBox
            // 
            this.sortSignAbsSumSortBox.Enabled = false;
            this.sortSignAbsSumSortBox.Location = new System.Drawing.Point(189, 42);
            this.sortSignAbsSumSortBox.Name = "sortSignAbsSumSortBox";
            this.sortSignAbsSumSortBox.Size = new System.Drawing.Size(45, 21);
            this.sortSignAbsSumSortBox.TabIndex = 11;
            this.sortSignAbsSumSortBox.Text = "1";
            this.sortSignAbsSumSortBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // bySumWeightsBox
            // 
            this.bySumWeightsBox.AutoSize = true;
            this.bySumWeightsBox.Enabled = false;
            this.bySumWeightsBox.Location = new System.Drawing.Point(6, 46);
            this.bySumWeightsBox.Name = "bySumWeightsBox";
            this.bySumWeightsBox.Size = new System.Drawing.Size(180, 17);
            this.bySumWeightsBox.TabIndex = 10;
            this.bySumWeightsBox.Text = "По суммарному значению слоя";
            this.bySumWeightsBox.UseVisualStyleBackColor = true;
            this.bySumWeightsBox.CheckedChanged += new System.EventHandler(this.bySumWeightsBox_CheckedChanged);
            // 
            // fromInputToOutputBox
            // 
            this.fromInputToOutputBox.AutoSize = true;
            this.fromInputToOutputBox.Enabled = false;
            this.fromInputToOutputBox.Location = new System.Drawing.Point(6, 20);
            this.fromInputToOutputBox.Name = "fromInputToOutputBox";
            this.fromInputToOutputBox.Size = new System.Drawing.Size(125, 17);
            this.fromInputToOutputBox.TabIndex = 9;
            this.fromInputToOutputBox.Text = "От входа к выходу";
            this.fromInputToOutputBox.UseVisualStyleBackColor = true;
            this.fromInputToOutputBox.CheckedChanged += new System.EventHandler(this.fromInputToOutputBox_CheckedChanged);
            // 
            // sortByModuleAscBox
            // 
            this.sortByModuleAscBox.AutoSize = true;
            this.sortByModuleAscBox.Location = new System.Drawing.Point(6, 20);
            this.sortByModuleAscBox.Name = "sortByModuleAscBox";
            this.sortByModuleAscBox.Size = new System.Drawing.Size(176, 17);
            this.sortByModuleAscBox.TabIndex = 5;
            this.sortByModuleAscBox.Text = "Упоряд. по значению модуля";
            this.sortByModuleAscBox.UseVisualStyleBackColor = true;
            this.sortByModuleAscBox.CheckedChanged += new System.EventHandler(this.sortByModuleAscBox_CheckedChanged);
            // 
            // sortByNumBox
            // 
            this.sortByNumBox.AutoSize = true;
            this.sortByNumBox.Location = new System.Drawing.Point(6, 43);
            this.sortByNumBox.Name = "sortByNumBox";
            this.sortByNumBox.Size = new System.Drawing.Size(122, 17);
            this.sortByNumBox.TabIndex = 4;
            this.sortByNumBox.Text = "Упоряд. по номеру";
            this.sortByNumBox.UseVisualStyleBackColor = true;
            this.sortByNumBox.CheckedChanged += new System.EventHandler(this.sortByNumBox_CheckedChanged);
            // 
            // limitRepeatBox
            // 
            this.limitRepeatBox.Location = new System.Drawing.Point(193, 17);
            this.limitRepeatBox.Name = "limitRepeatBox";
            this.limitRepeatBox.Size = new System.Drawing.Size(72, 21);
            this.limitRepeatBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Кол-во повторений: ";
            // 
            // repeatPullBox
            // 
            this.repeatPullBox.AutoSize = true;
            this.repeatPullBox.Location = new System.Drawing.Point(6, 65);
            this.repeatPullBox.Name = "repeatPullBox";
            this.repeatPullBox.Size = new System.Drawing.Size(187, 17);
            this.repeatPullBox.TabIndex = 1;
            this.repeatPullBox.Text = "Повторное измен. тех же весов";
            this.repeatPullBox.UseVisualStyleBackColor = true;
            this.repeatPullBox.CheckedChanged += new System.EventHandler(this.repeatPullBox_CheckedChanged);
            // 
            // allWeightsBox
            // 
            this.allWeightsBox.AutoSize = true;
            this.allWeightsBox.Location = new System.Drawing.Point(6, 42);
            this.allWeightsBox.Name = "allWeightsBox";
            this.allWeightsBox.Size = new System.Drawing.Size(194, 17);
            this.allWeightsBox.TabIndex = 0;
            this.allWeightsBox.Text = "Изменение всех весов топологии";
            this.allWeightsBox.UseVisualStyleBackColor = true;
            this.allWeightsBox.CheckedChanged += new System.EventHandler(this.allWeightsBox_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.startButton);
            this.groupBox5.Controls.Add(this.stopButton);
            this.groupBox5.Location = new System.Drawing.Point(742, 715);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(195, 57);
            this.groupBox5.TabIndex = 22;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = " Обучение";
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(6, 20);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(105, 25);
            this.startButton.TabIndex = 7;
            this.startButton.Text = "Начать обучение";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(108, 20);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(81, 25);
            this.stopButton.TabIndex = 8;
            this.stopButton.Text = "Остановить";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // LearnPullANNForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(1028, 777);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "LearnPullANNForm";
            this.Text = "ANNBuilder: перебор весовых коэффициентов";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.weightsView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lastRunsGridView)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
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
        private TextBox alphaBox;
        private Label label8;
        private GroupBox groupBox1;
        private TextBox neuronsBox;
        private Label label11;
        private DataGridView lastRunsGridView;
        private ToolStripMenuItem crossValidToolStripMenuItem;
        private IContainer components;
        private OpenFileDialog openFileDialog1;
        private Label pLabel;
        private TextBox probabilisticValidBox;
        private DataGridView weightsView;
        private Label label9;
        private Label label1;
        private TextBox randomCountWeightsBox;
        private TextBox weightsChangeBox;
        private TextBox minWeightValueBox;
        private TextBox maxWeightValueBox;
        private Label label10;
        private GroupBox groupBox6;
        private CheckBox allWeightsBox;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn probabilisticCol;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column10;
        private CheckBox repeatPullBox;
        private TextBox limitRepeatBox;
        private Label label3;
        private CheckBox sortByNumBox;
        private CheckBox sortByModuleAscBox;
        private DataGridViewTextBoxColumn index;
        private DataGridViewTextBoxColumn value;
        private DataGridViewTextBoxColumn Valid;
        private DataGridViewTextBoxColumn initWeightValue;
        private CheckBox sortByLayersBox;
        private CheckBox bySumWeightsBox;
        private CheckBox fromInputToOutputBox;
        private GroupBox groupBox8;
        private GroupBox groupBox7;
        private TextBox sortSignAbsSortBox;
        private TextBox sortSignNumSortBox;
        private TextBox sortSignFromToSortBox;
        private TextBox sortSignAbsSumSortBox;
        private TextBox percentChangeBox;
        private Label label13;
        private Label label12;
        private GroupBox groupBox9;
        private GroupBox groupBox5;
        private Button startButton;
        private Button stopButton;
    }
}
