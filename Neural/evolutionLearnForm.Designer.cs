namespace Neural
{
    partial class EvolutionLearnForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lastRunsGridView = new System.Windows.Forms.DataGridView();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.population = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mutation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.netsInPopulation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.meanError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.betterResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.labelProcess = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.mLabel = new System.Windows.Forms.Label();
            this.moduleValidBox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.neuronsBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.alphaBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.classesBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.inputCountBox = new System.Windows.Forms.TextBox();
            this.fileTextBox = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.нейроннаяСетьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveNetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveWeightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TestNetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crossValidToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выборкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTrainDataButton = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.numberNetsBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.popultextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.populationtextBox = new System.Windows.Forms.TextBox();
            this.populationLabel = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.mediumErrorPopulationBox = new System.Windows.Forms.TextBox();
            this.betterPopulationValueBox = new System.Windows.Forms.TextBox();
            this.PopulationBox = new System.Windows.Forms.TextBox();
            this.betterBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lastRunsGridView)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.zedGraphControl1);
            this.groupBox2.Location = new System.Drawing.Point(0, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(689, 595);
            this.groupBox2.TabIndex = 25;
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
            this.lastRunsGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lastRunsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lastRunsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column7,
            this.Column9,
            this.Column10,
            this.population,
            this.mutation,
            this.netsInPopulation,
            this.meanError,
            this.betterResult});
            this.lastRunsGridView.Location = new System.Drawing.Point(6, 17);
            this.lastRunsGridView.Name = "lastRunsGridView";
            this.lastRunsGridView.RowHeadersVisible = false;
            this.lastRunsGridView.Size = new System.Drawing.Size(658, 207);
            this.lastRunsGridView.TabIndex = 0;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Валидация(модуль)";
            this.Column7.Name = "Column7";
            this.Column7.Width = 132;
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
            // population
            // 
            this.population.HeaderText = "Популяций";
            this.population.Name = "population";
            this.population.Width = 87;
            // 
            // mutation
            // 
            this.mutation.HeaderText = "Скрещиваний в каждом";
            this.mutation.Name = "mutation";
            this.mutation.Width = 105;
            // 
            // netsInPopulation
            // 
            this.netsInPopulation.HeaderText = "Сетей в поколении";
            this.netsInPopulation.Name = "netsInPopulation";
            this.netsInPopulation.Width = 118;
            // 
            // meanError
            // 
            this.meanError.HeaderText = "Средняя ошибка";
            this.meanError.Name = "meanError";
            this.meanError.Width = 107;
            // 
            // betterResult
            // 
            this.betterResult.HeaderText = "Лучший результат";
            this.betterResult.Name = "betterResult";
            this.betterResult.Width = 116;
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.zedGraphControl1.Location = new System.Drawing.Point(6, 14);
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
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.startButton);
            this.groupBox5.Controls.Add(this.stopButton);
            this.groupBox5.Location = new System.Drawing.Point(695, 469);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(195, 57);
            this.groupBox5.TabIndex = 24;
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
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.labelProcess);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.mLabel);
            this.groupBox4.Controls.Add(this.moduleValidBox);
            this.groupBox4.Location = new System.Drawing.Point(695, 391);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(195, 72);
            this.groupBox4.TabIndex = 23;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Текущая итерация";
            // 
            // labelProcess
            // 
            this.labelProcess.AutoSize = true;
            this.labelProcess.Location = new System.Drawing.Point(120, 45);
            this.labelProcess.Name = "labelProcess";
            this.labelProcess.Size = new System.Drawing.Size(0, 13);
            this.labelProcess.TabIndex = 52;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 41;
            this.label7.Text = "Текущий процесс";
            // 
            // mLabel
            // 
            this.mLabel.AutoSize = true;
            this.mLabel.Location = new System.Drawing.Point(6, 17);
            this.mLabel.Name = "mLabel";
            this.mLabel.Size = new System.Drawing.Size(77, 13);
            this.mLabel.TabIndex = 6;
            this.mLabel.Text = "% По модулю";
            // 
            // moduleValidBox
            // 
            this.moduleValidBox.Location = new System.Drawing.Point(123, 15);
            this.moduleValidBox.Name = "moduleValidBox";
            this.moduleValidBox.Size = new System.Drawing.Size(66, 21);
            this.moduleValidBox.TabIndex = 5;
            // 
            // groupBox3
            // 
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
            this.groupBox3.Location = new System.Drawing.Point(695, 27);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(195, 153);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Настройки";
            // 
            // neuronsBox
            // 
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
            // alphaBox
            // 
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
            this.inputCountBox.Location = new System.Drawing.Point(100, 42);
            this.inputCountBox.Name = "inputCountBox";
            this.inputCountBox.ReadOnly = true;
            this.inputCountBox.Size = new System.Drawing.Size(86, 21);
            this.inputCountBox.TabIndex = 6;
            // 
            // fileTextBox
            // 
            this.fileTextBox.Location = new System.Drawing.Point(100, 14);
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.ReadOnly = true;
            this.fileTextBox.Size = new System.Drawing.Size(86, 21);
            this.fileTextBox.TabIndex = 5;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.нейроннаяСетьToolStripMenuItem,
            this.выборкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(898, 24);
            this.menuStrip1.TabIndex = 27;
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
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.numberNetsBox);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this.popultextBox);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.populationtextBox);
            this.groupBox6.Controls.Add(this.populationLabel);
            this.groupBox6.Location = new System.Drawing.Point(695, 186);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(195, 99);
            this.groupBox6.TabIndex = 28;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Опции эволюции сетей";
            // 
            // numberNetsBox
            // 
            this.numberNetsBox.Location = new System.Drawing.Point(147, 67);
            this.numberNetsBox.Name = "numberNetsBox";
            this.numberNetsBox.Size = new System.Drawing.Size(37, 21);
            this.numberNetsBox.TabIndex = 59;
            this.numberNetsBox.Text = "50";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 69);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(147, 13);
            this.label14.TabIndex = 58;
            this.label14.Text = "Кол-во сетей в поколении: ";
            // 
            // popultextBox
            // 
            this.popultextBox.Location = new System.Drawing.Point(147, 40);
            this.popultextBox.Name = "popultextBox";
            this.popultextBox.Size = new System.Drawing.Size(37, 21);
            this.popultextBox.TabIndex = 49;
            this.popultextBox.Text = "2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 48;
            this.label1.Text = "Кол-во скрещ. в каждом:";
            // 
            // populationtextBox
            // 
            this.populationtextBox.Location = new System.Drawing.Point(147, 15);
            this.populationtextBox.Name = "populationtextBox";
            this.populationtextBox.Size = new System.Drawing.Size(37, 21);
            this.populationtextBox.TabIndex = 46;
            this.populationtextBox.Text = "2";
            // 
            // populationLabel
            // 
            this.populationLabel.AutoSize = true;
            this.populationLabel.Location = new System.Drawing.Point(3, 17);
            this.populationLabel.Name = "populationLabel";
            this.populationLabel.Size = new System.Drawing.Size(103, 13);
            this.populationLabel.TabIndex = 45;
            this.populationLabel.Text = "Кол-во поколений:";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.mediumErrorPopulationBox);
            this.groupBox7.Controls.Add(this.betterPopulationValueBox);
            this.groupBox7.Controls.Add(this.PopulationBox);
            this.groupBox7.Controls.Add(this.betterBox);
            this.groupBox7.Controls.Add(this.label9);
            this.groupBox7.Controls.Add(this.label10);
            this.groupBox7.Controls.Add(this.label12);
            this.groupBox7.Location = new System.Drawing.Point(695, 291);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(195, 94);
            this.groupBox7.TabIndex = 29;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Эволюция";
            // 
            // mediumErrorPopulationBox
            // 
            this.mediumErrorPopulationBox.Location = new System.Drawing.Point(147, 41);
            this.mediumErrorPopulationBox.Name = "mediumErrorPopulationBox";
            this.mediumErrorPopulationBox.ReadOnly = true;
            this.mediumErrorPopulationBox.Size = new System.Drawing.Size(42, 21);
            this.mediumErrorPopulationBox.TabIndex = 54;
            // 
            // betterPopulationValueBox
            // 
            this.betterPopulationValueBox.Location = new System.Drawing.Point(147, 67);
            this.betterPopulationValueBox.Name = "betterPopulationValueBox";
            this.betterPopulationValueBox.ReadOnly = true;
            this.betterPopulationValueBox.Size = new System.Drawing.Size(42, 21);
            this.betterPopulationValueBox.TabIndex = 55;
            // 
            // PopulationBox
            // 
            this.PopulationBox.Location = new System.Drawing.Point(99, 11);
            this.PopulationBox.Name = "PopulationBox";
            this.PopulationBox.ReadOnly = true;
            this.PopulationBox.Size = new System.Drawing.Size(35, 21);
            this.PopulationBox.TabIndex = 53;
            // 
            // betterBox
            // 
            this.betterBox.Location = new System.Drawing.Point(147, 11);
            this.betterBox.Name = "betterBox";
            this.betterBox.ReadOnly = true;
            this.betterBox.Size = new System.Drawing.Size(42, 21);
            this.betterBox.TabIndex = 52;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(2, 69);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 13);
            this.label9.TabIndex = 50;
            this.label9.Text = "Лучший результат:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(2, 41);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 13);
            this.label10.TabIndex = 49;
            this.label10.Text = "Средняя ошибка:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(2, 19);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 13);
            this.label12.TabIndex = 48;
            this.label12.Text = "Отбор:";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            this.openFileDialog.Title = "Select data file";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // EvolutionLearnForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 629);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Name = "EvolutionLearnForm";
            this.Text = "ANNBuilder: эволюционный алгоритм";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lastRunsGridView)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView lastRunsGridView;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label mLabel;
        private System.Windows.Forms.TextBox moduleValidBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox neuronsBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox classesBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox inputCountBox;
        private System.Windows.Forms.TextBox fileTextBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem нейроннаяСетьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveNetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveWeightsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TestNetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem crossValidToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выборкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTrainDataButton;
        private System.Windows.Forms.TextBox alphaBox;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox populationtextBox;
        private System.Windows.Forms.Label populationLabel;
        private System.Windows.Forms.TextBox popultextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox numberNetsBox;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox mediumErrorPopulationBox;
        private System.Windows.Forms.TextBox betterPopulationValueBox;
        private System.Windows.Forms.TextBox PopulationBox;
        private System.Windows.Forms.TextBox betterBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelProcess;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn population;
        private System.Windows.Forms.DataGridViewTextBoxColumn mutation;
        private System.Windows.Forms.DataGridViewTextBoxColumn netsInPopulation;
        private System.Windows.Forms.DataGridViewTextBoxColumn meanError;
        private System.Windows.Forms.DataGridViewTextBoxColumn betterResult;
    }
}