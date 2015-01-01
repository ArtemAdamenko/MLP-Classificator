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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.LoadNetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.testNetButton = new System.Windows.Forms.Button();
            this.testErrorLabel = new System.Windows.Forms.Label();
            this.Layer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Neuron = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weightValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(338, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(719, 548);
            this.panel1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
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
            this.groupBox1.Location = new System.Drawing.Point(12, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 497);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Изменение сети";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Layer,
            this.Neuron,
            this.Weight,
            this.weightValue,
            this.Check});
            this.dataGridView1.Location = new System.Drawing.Point(6, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(308, 472);
            this.dataGridView1.TabIndex = 8;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadNetToolStripMenuItem,
            this.LoadDataToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1062, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // LoadNetToolStripMenuItem
            // 
            this.LoadNetToolStripMenuItem.Name = "LoadNetToolStripMenuItem";
            this.LoadNetToolStripMenuItem.Size = new System.Drawing.Size(97, 20);
            this.LoadNetToolStripMenuItem.Text = "Загрузить сеть";
            this.LoadNetToolStripMenuItem.Click += new System.EventHandler(this.LoadNetToolStripMenuItem_Click);
            // 
            // LoadDataToolStripMenuItem
            // 
            this.LoadDataToolStripMenuItem.Name = "LoadDataToolStripMenuItem";
            this.LoadDataToolStripMenuItem.Size = new System.Drawing.Size(118, 20);
            this.LoadDataToolStripMenuItem.Text = "Загрузить выборку";
            this.LoadDataToolStripMenuItem.Click += new System.EventHandler(this.LoadDataToolStripMenuItem_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // testNetButton
            // 
            this.testNetButton.Location = new System.Drawing.Point(12, 527);
            this.testNetButton.Name = "testNetButton";
            this.testNetButton.Size = new System.Drawing.Size(83, 23);
            this.testNetButton.TabIndex = 6;
            this.testNetButton.Text = "Тестировать";
            this.testNetButton.UseVisualStyleBackColor = true;
            this.testNetButton.Click += new System.EventHandler(this.testNetButton_Click);
            // 
            // testErrorLabel
            // 
            this.testErrorLabel.AutoSize = true;
            this.testErrorLabel.Location = new System.Drawing.Point(197, 532);
            this.testErrorLabel.Name = "testErrorLabel";
            this.testErrorLabel.Size = new System.Drawing.Size(62, 13);
            this.testErrorLabel.TabIndex = 7;
            this.testErrorLabel.Text = "% ошибки: ";
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
            this.Neuron.Width = 40;
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
            this.weightValue.Width = 90;
            // 
            // Check
            // 
            this.Check.FalseValue = "F";
            this.Check.HeaderText = "Вкл/Выкл";
            this.Check.Name = "Check";
            this.Check.TrueValue = "T";
            this.Check.Width = 70;
            // 
            // FormDrawNeurons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1062, 562);
            this.Controls.Add(this.testErrorLabel);
            this.Controls.Add(this.testNetButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormDrawNeurons";
            this.Text = "Изменение и проверка сети";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem LoadNetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadDataToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Button testNetButton;
        private System.Windows.Forms.Label testErrorLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Layer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Neuron;
        private System.Windows.Forms.DataGridViewTextBoxColumn Weight;
        private System.Windows.Forms.DataGridViewTextBoxColumn weightValue;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check;
    }
}