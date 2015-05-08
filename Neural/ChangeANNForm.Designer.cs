namespace Neural
{
    partial class ChangeANNForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.outputVectorButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Layer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Neuron = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weightValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.testNetButton = new System.Windows.Forms.Button();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.testErrorLabel = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.LoadDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.тестToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.входнойВекторToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveNetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadNetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.neuronsCountBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.timeLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.startOffNeuronsButton = new System.Windows.Forms.Button();
            this.neuronsToOffBox = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.stopWeightsButton = new System.Windows.Forms.Button();
            this.startOffWeightsButton = new System.Windows.Forms.Button();
            this.radiusBox = new System.Windows.Forms.TextBox();
            this.offWeightsLevelBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CombinationsLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(338, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(706, 646);
            this.panel1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(3, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(700, 635);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.outputVectorButton);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.testNetButton);
            this.groupBox1.Controls.Add(this.errorTextBox);
            this.groupBox1.Controls.Add(this.testErrorLabel);
            this.groupBox1.Location = new System.Drawing.Point(0, 257);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 422);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Изменение сети";
            // 
            // outputVectorButton
            // 
            this.outputVectorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.outputVectorButton.Location = new System.Drawing.Point(8, 391);
            this.outputVectorButton.Name = "outputVectorButton";
            this.outputVectorButton.Size = new System.Drawing.Size(112, 23);
            this.outputVectorButton.TabIndex = 9;
            this.outputVectorButton.Text = "Выходной вектор";
            this.outputVectorButton.UseVisualStyleBackColor = true;
            this.outputVectorButton.Click += new System.EventHandler(this.outputVectorButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Layer,
            this.Neuron,
            this.Weight,
            this.weightValue,
            this.Check});
            this.dataGridView1.Location = new System.Drawing.Point(6, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(308, 339);
            this.dataGridView1.TabIndex = 8;
            // 
            // Layer
            // 
            this.Layer.HeaderText = "Слой";
            this.Layer.Name = "Layer";
            this.Layer.ReadOnly = true;
            this.Layer.Width = 40;
            // 
            // Neuron
            // 
            this.Neuron.HeaderText = "Нейрон";
            this.Neuron.Name = "Neuron";
            this.Neuron.ReadOnly = true;
            this.Neuron.Width = 60;
            // 
            // Weight
            // 
            this.Weight.HeaderText = "Вес";
            this.Weight.Name = "Weight";
            this.Weight.ReadOnly = true;
            this.Weight.Width = 30;
            // 
            // weightValue
            // 
            this.weightValue.HeaderText = "Значение";
            this.weightValue.Name = "weightValue";
            this.weightValue.ReadOnly = true;
            this.weightValue.Width = 110;
            // 
            // Check
            // 
            this.Check.FalseValue = "F";
            this.Check.HeaderText = "Вкл/Выкл";
            this.Check.Name = "Check";
            this.Check.TrueValue = "T";
            this.Check.Width = 70;
            // 
            // testNetButton
            // 
            this.testNetButton.Enabled = false;
            this.testNetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.testNetButton.Location = new System.Drawing.Point(7, 362);
            this.testNetButton.Name = "testNetButton";
            this.testNetButton.Size = new System.Drawing.Size(83, 23);
            this.testNetButton.TabIndex = 6;
            this.testNetButton.Text = "Тестировать";
            this.testNetButton.UseVisualStyleBackColor = true;
            this.testNetButton.Click += new System.EventHandler(this.testNetButton_Click);
            // 
            // errorTextBox
            // 
            this.errorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorTextBox.Enabled = false;
            this.errorTextBox.Location = new System.Drawing.Point(214, 364);
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.Size = new System.Drawing.Size(100, 21);
            this.errorTextBox.TabIndex = 8;
            // 
            // testErrorLabel
            // 
            this.testErrorLabel.AutoSize = true;
            this.testErrorLabel.Location = new System.Drawing.Point(121, 367);
            this.testErrorLabel.Name = "testErrorLabel";
            this.testErrorLabel.Size = new System.Drawing.Size(93, 13);
            this.testErrorLabel.TabIndex = 7;
            this.testErrorLabel.Text = "% обученности: ";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadDataToolStripMenuItem,
            this.NetToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1062, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // LoadDataToolStripMenuItem
            // 
            this.LoadDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.тестToolStripMenuItem,
            this.входнойВекторToolStripMenuItem});
            this.LoadDataToolStripMenuItem.Name = "LoadDataToolStripMenuItem";
            this.LoadDataToolStripMenuItem.Size = new System.Drawing.Size(118, 20);
            this.LoadDataToolStripMenuItem.Text = "Загрузить выборку";
            // 
            // тестToolStripMenuItem
            // 
            this.тестToolStripMenuItem.Name = "тестToolStripMenuItem";
            this.тестToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.тестToolStripMenuItem.Text = "Тест";
            this.тестToolStripMenuItem.Click += new System.EventHandler(this.тестToolStripMenuItem_Click);
            // 
            // входнойВекторToolStripMenuItem
            // 
            this.входнойВекторToolStripMenuItem.Name = "входнойВекторToolStripMenuItem";
            this.входнойВекторToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.входнойВекторToolStripMenuItem.Text = "Входной вектор";
            this.входнойВекторToolStripMenuItem.Click += new System.EventHandler(this.входнойВекторToolStripMenuItem_Click);
            // 
            // NetToolStripMenuItem
            // 
            this.NetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveNetToolStripMenuItem,
            this.LoadNetToolStripMenuItem});
            this.NetToolStripMenuItem.Name = "NetToolStripMenuItem";
            this.NetToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.NetToolStripMenuItem.Text = "Сеть";
            // 
            // saveNetToolStripMenuItem
            // 
            this.saveNetToolStripMenuItem.Name = "saveNetToolStripMenuItem";
            this.saveNetToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.saveNetToolStripMenuItem.Text = "Сохранить сеть";
            this.saveNetToolStripMenuItem.Click += new System.EventHandler(this.saveNetToolStripMenuItem_Click);
            // 
            // LoadNetToolStripMenuItem
            // 
            this.LoadNetToolStripMenuItem.Name = "LoadNetToolStripMenuItem";
            this.LoadNetToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.LoadNetToolStripMenuItem.Text = "Загрузить сеть";
            this.LoadNetToolStripMenuItem.Click += new System.EventHandler(this.LoadNetToolStripMenuItem_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Кол-во нейронов в сети: ";
            // 
            // neuronsCountBox
            // 
            this.neuronsCountBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.neuronsCountBox.Location = new System.Drawing.Point(240, 12);
            this.neuronsCountBox.Name = "neuronsCountBox";
            this.neuronsCountBox.ReadOnly = true;
            this.neuronsCountBox.Size = new System.Drawing.Size(74, 21);
            this.neuronsCountBox.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Отключать нейроны:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.stopButton);
            this.groupBox2.Controls.Add(this.startOffNeuronsButton);
            this.groupBox2.Controls.Add(this.neuronsToOffBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.neuronsCountBox);
            this.groupBox2.Location = new System.Drawing.Point(0, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(320, 97);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Автоматический перебор нейронов";
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(46, 241);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(0, 13);
            this.timeLabel.TabIndex = 31;
            // 
            // stopButton
            // 
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopButton.Location = new System.Drawing.Point(239, 65);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 30;
            this.stopButton.Text = "Стоп";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startOffNeuronsButton
            // 
            this.startOffNeuronsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startOffNeuronsButton.Location = new System.Drawing.Point(158, 65);
            this.startOffNeuronsButton.Name = "startOffNeuronsButton";
            this.startOffNeuronsButton.Size = new System.Drawing.Size(75, 23);
            this.startOffNeuronsButton.TabIndex = 27;
            this.startOffNeuronsButton.Text = "Запуск";
            this.startOffNeuronsButton.UseVisualStyleBackColor = true;
            this.startOffNeuronsButton.Click += new System.EventHandler(this.startOffNeuronsButton_Click);
            // 
            // neuronsToOffBox
            // 
            this.neuronsToOffBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.neuronsToOffBox.Location = new System.Drawing.Point(240, 38);
            this.neuronsToOffBox.Name = "neuronsToOffBox";
            this.neuronsToOffBox.Size = new System.Drawing.Size(74, 21);
            this.neuronsToOffBox.TabIndex = 26;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.stopWeightsButton);
            this.groupBox3.Controls.Add(this.startOffWeightsButton);
            this.groupBox3.Controls.Add(this.radiusBox);
            this.groupBox3.Controls.Add(this.offWeightsLevelBox);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(0, 133);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(320, 94);
            this.groupBox3.TabIndex = 27;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Автоматическое отключение связей";
            // 
            // stopWeightsButton
            // 
            this.stopWeightsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopWeightsButton.Location = new System.Drawing.Point(239, 65);
            this.stopWeightsButton.Name = "stopWeightsButton";
            this.stopWeightsButton.Size = new System.Drawing.Size(75, 23);
            this.stopWeightsButton.TabIndex = 5;
            this.stopWeightsButton.Text = "Стоп";
            this.stopWeightsButton.UseVisualStyleBackColor = true;
            this.stopWeightsButton.Click += new System.EventHandler(this.stopWeightsButton_Click);
            // 
            // startOffWeightsButton
            // 
            this.startOffWeightsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startOffWeightsButton.Location = new System.Drawing.Point(158, 65);
            this.startOffWeightsButton.Name = "startOffWeightsButton";
            this.startOffWeightsButton.Size = new System.Drawing.Size(75, 23);
            this.startOffWeightsButton.TabIndex = 4;
            this.startOffWeightsButton.Text = "Запуск";
            this.startOffWeightsButton.UseVisualStyleBackColor = true;
            this.startOffWeightsButton.Click += new System.EventHandler(this.startOffWeightsButton_Click);
            // 
            // radiusBox
            // 
            this.radiusBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.radiusBox.Location = new System.Drawing.Point(240, 37);
            this.radiusBox.Name = "radiusBox";
            this.radiusBox.Size = new System.Drawing.Size(74, 21);
            this.radiusBox.TabIndex = 3;
            this.radiusBox.Text = "15";
            // 
            // offWeightsLevelBox
            // 
            this.offWeightsLevelBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.offWeightsLevelBox.Location = new System.Drawing.Point(240, 10);
            this.offWeightsLevelBox.Name = "offWeightsLevelBox";
            this.offWeightsLevelBox.Size = new System.Drawing.Size(74, 21);
            this.offWeightsLevelBox.TabIndex = 2;
            this.offWeightsLevelBox.Text = "80";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Разброс :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Желаемый порог :";
            // 
            // CombinationsLabel
            // 
            this.CombinationsLabel.AutoSize = true;
            this.CombinationsLabel.Location = new System.Drawing.Point(229, 241);
            this.CombinationsLabel.Name = "CombinationsLabel";
            this.CombinationsLabel.Size = new System.Drawing.Size(0, 13);
            this.CombinationsLabel.TabIndex = 32;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 241);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Время";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(155, 241);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 34;
            this.label6.Text = "Комбинации";
            // 
            // ChangeANNForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1062, 680);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CombinationsLabel);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ChangeANNForm";
            this.Text = "Изменение и проверка сети";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem LoadDataToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Button testNetButton;
        private System.Windows.Forms.Label testErrorLabel;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn Layer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Neuron;
        private System.Windows.Forms.DataGridViewTextBoxColumn Weight;
        private System.Windows.Forms.DataGridViewTextBoxColumn weightValue;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox neuronsCountBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox neuronsToOffBox;
        private System.Windows.Forms.Button startOffNeuronsButton;
        private System.Windows.Forms.ToolStripMenuItem NetToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem saveNetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadNetToolStripMenuItem;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.ToolStripMenuItem тестToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem входнойВекторToolStripMenuItem;
        private System.Windows.Forms.Button outputVectorButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button stopWeightsButton;
        private System.Windows.Forms.Button startOffWeightsButton;
        private System.Windows.Forms.TextBox radiusBox;
        private System.Windows.Forms.TextBox offWeightsLevelBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label CombinationsLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
    }
}