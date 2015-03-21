namespace Neural
{
    partial class FormDrawNeurons
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
            this.NetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveNetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadNetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.neuronsCountBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.iterationsBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.startOffNeuronsButton = new System.Windows.Forms.Button();
            this.neuronsToOffBox = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.timeLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(338, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(706, 537);
            this.panel1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(700, 531);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.testNetButton);
            this.groupBox1.Controls.Add(this.errorTextBox);
            this.groupBox1.Controls.Add(this.testErrorLabel);
            this.groupBox1.Location = new System.Drawing.Point(10, 158);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 392);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Изменение сети";
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
            this.LoadDataToolStripMenuItem.Name = "LoadDataToolStripMenuItem";
            this.LoadDataToolStripMenuItem.Size = new System.Drawing.Size(124, 20);
            this.LoadDataToolStripMenuItem.Text = "Загрузить выборку";
            this.LoadDataToolStripMenuItem.Click += new System.EventHandler(this.LoadDataToolStripMenuItem_Click);
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
            this.saveNetToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.saveNetToolStripMenuItem.Text = "Сохранить сеть";
            this.saveNetToolStripMenuItem.Click += new System.EventHandler(this.saveNetToolStripMenuItem_Click);
            // 
            // LoadNetToolStripMenuItem
            // 
            this.LoadNetToolStripMenuItem.Name = "LoadNetToolStripMenuItem";
            this.LoadNetToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
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
            this.neuronsCountBox.Location = new System.Drawing.Point(227, 12);
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
            this.groupBox2.Controls.Add(this.timeLabel);
            this.groupBox2.Controls.Add(this.stopButton);
            this.groupBox2.Controls.Add(this.iterationsBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.startOffNeuronsButton);
            this.groupBox2.Controls.Add(this.neuronsToOffBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.neuronsCountBox);
            this.groupBox2.Location = new System.Drawing.Point(18, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(308, 122);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Автоматическое отключение нейронов";
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(226, 93);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 30;
            this.stopButton.Text = "Стоп";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // iterationsBox
            // 
            this.iterationsBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.iterationsBox.Location = new System.Drawing.Point(227, 66);
            this.iterationsBox.Name = "iterationsBox";
            this.iterationsBox.ReadOnly = true;
            this.iterationsBox.Size = new System.Drawing.Size(75, 21);
            this.iterationsBox.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Итерации";
            // 
            // startOffNeuronsButton
            // 
            this.startOffNeuronsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startOffNeuronsButton.Location = new System.Drawing.Point(145, 93);
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
            this.neuronsToOffBox.Location = new System.Drawing.Point(227, 38);
            this.neuronsToOffBox.Name = "neuronsToOffBox";
            this.neuronsToOffBox.Size = new System.Drawing.Size(74, 21);
            this.neuronsToOffBox.TabIndex = 26;
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(9, 102);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(0, 13);
            this.timeLabel.TabIndex = 31;
            // 
            // FormDrawNeurons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1062, 562);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormDrawNeurons";
            this.Text = "Изменение и проверка сети";
            this.Load += new System.EventHandler(this.FormDrawNeurons_Load);
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox iterationsBox;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label timeLabel;
    }
}