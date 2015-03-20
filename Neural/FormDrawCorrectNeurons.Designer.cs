namespace Neural
{
    partial class FormDrawCorrectNeurons
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
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.testNetButton = new System.Windows.Forms.Button();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.testErrorLabel = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.LoadNetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.subNetButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.betterBox = new System.Windows.Forms.TextBox();
            this.PopulationBox = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.label5 = new System.Windows.Forms.Label();
            this.mediumErrorPopulationBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.betterPopulationValueBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.covariationPopulationBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Location = new System.Drawing.Point(336, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(714, 218);
            this.panel1.TabIndex = 1;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(708, 212);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // testNetButton
            // 
            this.testNetButton.Enabled = false;
            this.testNetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.testNetButton.Location = new System.Drawing.Point(218, 16);
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
            this.errorTextBox.Location = new System.Drawing.Point(103, 19);
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.Size = new System.Drawing.Size(100, 20);
            this.errorTextBox.TabIndex = 8;
            // 
            // testErrorLabel
            // 
            this.testErrorLabel.AutoSize = true;
            this.testErrorLabel.Location = new System.Drawing.Point(4, 21);
            this.testErrorLabel.Name = "testErrorLabel";
            this.testErrorLabel.Size = new System.Drawing.Size(83, 13);
            this.testErrorLabel.TabIndex = 7;
            this.testErrorLabel.Text = "% валидности: ";
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
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(336, 254);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(714, 296);
            this.panel2.TabIndex = 28;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(705, 290);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // subNetButton
            // 
            this.subNetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.subNetButton.Location = new System.Drawing.Point(42, 89);
            this.subNetButton.Name = "subNetButton";
            this.subNetButton.Size = new System.Drawing.Size(120, 23);
            this.subNetButton.TabIndex = 29;
            this.subNetButton.Text = "Обучить подсеть";
            this.subNetButton.UseVisualStyleBackColor = true;
            this.subNetButton.Click += new System.EventHandler(this.subNetButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Популяций:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Отбор:";
            // 
            // betterBox
            // 
            this.betterBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.betterBox.Location = new System.Drawing.Point(103, 71);
            this.betterBox.Name = "betterBox";
            this.betterBox.ReadOnly = true;
            this.betterBox.Size = new System.Drawing.Size(100, 20);
            this.betterBox.TabIndex = 35;
            // 
            // PopulationBox
            // 
            this.PopulationBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PopulationBox.Location = new System.Drawing.Point(103, 45);
            this.PopulationBox.Name = "PopulationBox";
            this.PopulationBox.ReadOnly = true;
            this.PopulationBox.Size = new System.Drawing.Size(100, 20);
            this.PopulationBox.TabIndex = 36;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(168, 89);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(117, 23);
            this.button2.TabIndex = 37;
            this.button2.Text = "Стоп";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(239, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 39;
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.zedGraphControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.zedGraphControl1.Location = new System.Drawing.Point(12, 257);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(318, 296);
            this.zedGraphControl1.TabIndex = 40;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(151, 13);
            this.label5.TabIndex = 42;
            this.label5.Text = "Средняя ошибка поколения:";
            // 
            // mediumErrorPopulationBox
            // 
            this.mediumErrorPopulationBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mediumErrorPopulationBox.Location = new System.Drawing.Point(198, 8);
            this.mediumErrorPopulationBox.Name = "mediumErrorPopulationBox";
            this.mediumErrorPopulationBox.ReadOnly = true;
            this.mediumErrorPopulationBox.Size = new System.Drawing.Size(100, 20);
            this.mediumErrorPopulationBox.TabIndex = 44;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(159, 13);
            this.label4.TabIndex = 45;
            this.label4.Text = "Лучший результат поколения:";
            // 
            // betterPopulationValueBox
            // 
            this.betterPopulationValueBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.betterPopulationValueBox.Location = new System.Drawing.Point(198, 34);
            this.betterPopulationValueBox.Name = "betterPopulationValueBox";
            this.betterPopulationValueBox.ReadOnly = true;
            this.betterPopulationValueBox.Size = new System.Drawing.Size(100, 20);
            this.betterPopulationValueBox.TabIndex = 46;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 13);
            this.label6.TabIndex = 47;
            this.label6.Text = "Коэффициент вариации:";
            // 
            // covariationPopulationBox
            // 
            this.covariationPopulationBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.covariationPopulationBox.Location = new System.Drawing.Point(198, 60);
            this.covariationPopulationBox.Name = "covariationPopulationBox";
            this.covariationPopulationBox.ReadOnly = true;
            this.covariationPopulationBox.Size = new System.Drawing.Size(100, 20);
            this.covariationPopulationBox.TabIndex = 48;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mediumErrorPopulationBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.covariationPopulationBox);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.betterPopulationValueBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.subNetButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 130);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(318, 121);
            this.groupBox1.TabIndex = 49;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Результаты";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.errorTextBox);
            this.groupBox2.Controls.Add(this.PopulationBox);
            this.groupBox2.Controls.Add(this.betterBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.testNetButton);
            this.groupBox2.Controls.Add(this.testErrorLabel);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(318, 97);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Показания";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(209, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 13);
            this.label7.TabIndex = 40;
            this.label7.Text = "Текущий процесс";
            // 
            // FormDrawCorrectNeurons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1062, 562);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.zedGraphControl1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormDrawCorrectNeurons";
            this.Text = "Корректировка сети";
            this.Load += new System.EventHandler(this.FormDrawCorrectNeurons_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem LoadNetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadDataToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Button testNetButton;
        private System.Windows.Forms.Label testErrorLabel;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button subNetButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox betterBox;
        private System.Windows.Forms.TextBox PopulationBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox mediumErrorPopulationBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox betterPopulationValueBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox covariationPopulationBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
    }
}