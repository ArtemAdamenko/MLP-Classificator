namespace Neural
{
    partial class CorrectANNForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.testNetButton = new System.Windows.Forms.Button();
            this.probabilisticErrorTextBox = new System.Windows.Forms.TextBox();
            this.testErrorLabel = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.LoadNetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.subNetButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.betterBox = new System.Windows.Forms.TextBox();
            this.PopulationBox = new System.Windows.Forms.TextBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.label5 = new System.Windows.Forms.Label();
            this.mediumErrorPopulationBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.betterPopulationValueBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.covariationPopulationBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.moduleErrorBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.subnetTopologyBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.relationsLabel = new System.Windows.Forms.Label();
            this.reconnectBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.validLevelBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.alphaBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.popultextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.populationtextBox = new System.Windows.Forms.TextBox();
            this.populationLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
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
            this.panel1.Location = new System.Drawing.Point(336, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(714, 342);
            this.panel1.TabIndex = 1;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(708, 336);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // testNetButton
            // 
            this.testNetButton.Enabled = false;
            this.testNetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.testNetButton.Location = new System.Drawing.Point(209, 174);
            this.testNetButton.Name = "testNetButton";
            this.testNetButton.Size = new System.Drawing.Size(100, 23);
            this.testNetButton.TabIndex = 6;
            this.testNetButton.Text = "Тестировать";
            this.testNetButton.UseVisualStyleBackColor = true;
            this.testNetButton.Click += new System.EventHandler(this.testNetButton_Click);
            // 
            // probabilisticErrorTextBox
            // 
            this.probabilisticErrorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.probabilisticErrorTextBox.Enabled = false;
            this.probabilisticErrorTextBox.Location = new System.Drawing.Point(209, 15);
            this.probabilisticErrorTextBox.Name = "probabilisticErrorTextBox";
            this.probabilisticErrorTextBox.ReadOnly = true;
            this.probabilisticErrorTextBox.Size = new System.Drawing.Size(100, 21);
            this.probabilisticErrorTextBox.TabIndex = 8;
            // 
            // testErrorLabel
            // 
            this.testErrorLabel.AutoSize = true;
            this.testErrorLabel.Location = new System.Drawing.Point(10, 23);
            this.testErrorLabel.Name = "testErrorLabel";
            this.testErrorLabel.Size = new System.Drawing.Size(93, 13);
            this.testErrorLabel.TabIndex = 7;
            this.testErrorLabel.Text = "% Вероятности: ";
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
            this.panel2.Location = new System.Drawing.Point(336, 399);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(714, 296);
            this.panel2.TabIndex = 28;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(0, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(705, 290);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // subNetButton
            // 
            this.subNetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.subNetButton.Location = new System.Drawing.Point(11, 174);
            this.subNetButton.Name = "subNetButton";
            this.subNetButton.Size = new System.Drawing.Size(110, 23);
            this.subNetButton.TabIndex = 29;
            this.subNetButton.Text = "Обучить подсеть";
            this.subNetButton.UseVisualStyleBackColor = true;
            this.subNetButton.Click += new System.EventHandler(this.subNetButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Отбор:";
            // 
            // betterBox
            // 
            this.betterBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.betterBox.Location = new System.Drawing.Point(260, 65);
            this.betterBox.Name = "betterBox";
            this.betterBox.ReadOnly = true;
            this.betterBox.Size = new System.Drawing.Size(49, 21);
            this.betterBox.TabIndex = 35;
            // 
            // PopulationBox
            // 
            this.PopulationBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PopulationBox.Location = new System.Drawing.Point(209, 65);
            this.PopulationBox.Name = "PopulationBox";
            this.PopulationBox.ReadOnly = true;
            this.PopulationBox.Size = new System.Drawing.Size(45, 21);
            this.PopulationBox.TabIndex = 36;
            // 
            // stopButton
            // 
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopButton.Location = new System.Drawing.Point(117, 174);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(73, 23);
            this.stopButton.TabIndex = 37;
            this.stopButton.Text = "Стоп";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(437, 373);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 39;
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.zedGraphControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.zedGraphControl1.Location = new System.Drawing.Point(12, 402);
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
            this.label5.Location = new System.Drawing.Point(8, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 13);
            this.label5.TabIndex = 42;
            this.label5.Text = "Средняя ошибка поколения:";
            // 
            // mediumErrorPopulationBox
            // 
            this.mediumErrorPopulationBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mediumErrorPopulationBox.Location = new System.Drawing.Point(209, 95);
            this.mediumErrorPopulationBox.Name = "mediumErrorPopulationBox";
            this.mediumErrorPopulationBox.ReadOnly = true;
            this.mediumErrorPopulationBox.Size = new System.Drawing.Size(100, 21);
            this.mediumErrorPopulationBox.TabIndex = 44;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 13);
            this.label4.TabIndex = 45;
            this.label4.Text = "Лучший результат поколения:";
            // 
            // betterPopulationValueBox
            // 
            this.betterPopulationValueBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.betterPopulationValueBox.Location = new System.Drawing.Point(209, 121);
            this.betterPopulationValueBox.Name = "betterPopulationValueBox";
            this.betterPopulationValueBox.ReadOnly = true;
            this.betterPopulationValueBox.Size = new System.Drawing.Size(100, 21);
            this.betterPopulationValueBox.TabIndex = 46;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(132, 13);
            this.label6.TabIndex = 47;
            this.label6.Text = "Коэффициент вариации:";
            // 
            // covariationPopulationBox
            // 
            this.covariationPopulationBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.covariationPopulationBox.Location = new System.Drawing.Point(209, 147);
            this.covariationPopulationBox.Name = "covariationPopulationBox";
            this.covariationPopulationBox.ReadOnly = true;
            this.covariationPopulationBox.Size = new System.Drawing.Size(100, 21);
            this.covariationPopulationBox.TabIndex = 48;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mediumErrorPopulationBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.covariationPopulationBox);
            this.groupBox1.Controls.Add(this.stopButton);
            this.groupBox1.Controls.Add(this.moduleErrorBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.betterPopulationValueBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.probabilisticErrorTextBox);
            this.groupBox1.Controls.Add(this.subNetButton);
            this.groupBox1.Controls.Add(this.PopulationBox);
            this.groupBox1.Controls.Add(this.testErrorLabel);
            this.groupBox1.Controls.Add(this.betterBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.testNetButton);
            this.groupBox1.Location = new System.Drawing.Point(9, 189);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(318, 207);
            this.groupBox1.TabIndex = 49;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Результаты";
            // 
            // moduleErrorBox
            // 
            this.moduleErrorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.moduleErrorBox.Enabled = false;
            this.moduleErrorBox.Location = new System.Drawing.Point(209, 40);
            this.moduleErrorBox.Name = "moduleErrorBox";
            this.moduleErrorBox.ReadOnly = true;
            this.moduleErrorBox.Size = new System.Drawing.Size(100, 21);
            this.moduleErrorBox.TabIndex = 42;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 41;
            this.label3.Text = "% По модулю: ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.subnetTopologyBox);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.relationsLabel);
            this.groupBox2.Controls.Add(this.reconnectBox);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.validLevelBox);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.alphaBox);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.popultextBox);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.populationtextBox);
            this.groupBox2.Controls.Add(this.populationLabel);
            this.groupBox2.Location = new System.Drawing.Point(12, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(318, 156);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Опции эволюционного обучения подсети";
            // 
            // subnetTopologyBox
            // 
            this.subnetTopologyBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.subnetTopologyBox.Location = new System.Drawing.Point(224, 122);
            this.subnetTopologyBox.Name = "subnetTopologyBox";
            this.subnetTopologyBox.Size = new System.Drawing.Size(82, 21);
            this.subnetTopologyBox.TabIndex = 56;
            this.subnetTopologyBox.Text = "20,30,20";
            this.subnetTopologyBox.TextChanged += new System.EventHandler(this.subnetTopologyBox_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(4, 125);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(112, 13);
            this.label13.TabIndex = 55;
            this.label13.Text = "Топология подсети :";
            // 
            // relationsLabel
            // 
            this.relationsLabel.AutoSize = true;
            this.relationsLabel.Location = new System.Drawing.Point(268, 98);
            this.relationsLabel.Name = "relationsLabel";
            this.relationsLabel.Size = new System.Drawing.Size(0, 13);
            this.relationsLabel.TabIndex = 54;
            // 
            // reconnectBox
            // 
            this.reconnectBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.reconnectBox.Location = new System.Drawing.Point(224, 95);
            this.reconnectBox.Name = "reconnectBox";
            this.reconnectBox.Size = new System.Drawing.Size(38, 21);
            this.reconnectBox.TabIndex = 53;
            this.reconnectBox.Text = "10";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 98);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(115, 13);
            this.label12.TabIndex = 52;
            this.label12.Text = "Переподсоединить : ";
            // 
            // validLevelBox
            // 
            this.validLevelBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.validLevelBox.Location = new System.Drawing.Point(224, 72);
            this.validLevelBox.Name = "validLevelBox";
            this.validLevelBox.Size = new System.Drawing.Size(37, 21);
            this.validLevelBox.TabIndex = 51;
            this.validLevelBox.Text = "75";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 74);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(160, 13);
            this.label11.TabIndex = 50;
            this.label11.Text = "Порог валидации по модулю :";
            // 
            // alphaBox
            // 
            this.alphaBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.alphaBox.Location = new System.Drawing.Point(224, 49);
            this.alphaBox.Name = "alphaBox";
            this.alphaBox.Size = new System.Drawing.Size(37, 21);
            this.alphaBox.TabIndex = 49;
            this.alphaBox.Text = "2";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 51);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 13);
            this.label10.TabIndex = 48;
            this.label10.Text = "Альфа :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(267, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 47;
            this.label9.Text = "cкрещ.";
            // 
            // popultextBox
            // 
            this.popultextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.popultextBox.Location = new System.Drawing.Point(224, 24);
            this.popultextBox.Name = "popultextBox";
            this.popultextBox.Size = new System.Drawing.Size(36, 21);
            this.popultextBox.TabIndex = 46;
            this.popultextBox.Text = "3";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(145, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 13);
            this.label8.TabIndex = 45;
            this.label8.Text = "в каждом по:";
            // 
            // populationtextBox
            // 
            this.populationtextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.populationtextBox.Location = new System.Drawing.Point(105, 24);
            this.populationtextBox.Name = "populationtextBox";
            this.populationtextBox.Size = new System.Drawing.Size(37, 21);
            this.populationtextBox.TabIndex = 44;
            this.populationtextBox.Text = "2";
            // 
            // populationLabel
            // 
            this.populationLabel.AutoSize = true;
            this.populationLabel.Location = new System.Drawing.Point(4, 26);
            this.populationLabel.Name = "populationLabel";
            this.populationLabel.Size = new System.Drawing.Size(103, 13);
            this.populationLabel.TabIndex = 43;
            this.populationLabel.Text = "Кол-во популяций:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(336, 373);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 40;
            this.label7.Text = "Текущий процесс";
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(569, 373);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(0, 13);
            this.timeLabel.TabIndex = 51;
            // 
            // FormDrawCorrectNeurons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1062, 707);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.zedGraphControl1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormDrawCorrectNeurons";
            this.Text = "Корректировка сети";
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
        private System.Windows.Forms.TextBox probabilisticErrorTextBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button subNetButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox betterBox;
        private System.Windows.Forms.TextBox PopulationBox;
        private System.Windows.Forms.Button stopButton;
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
        private System.Windows.Forms.TextBox moduleErrorBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label populationLabel;
        private System.Windows.Forms.TextBox popultextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox populationtextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox alphaBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox validLevelBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label relationsLabel;
        private System.Windows.Forms.TextBox reconnectBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox subnetTopologyBox;
        private System.Windows.Forms.Label label13;
    }
}