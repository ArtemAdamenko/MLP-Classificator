﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using System.IO;
using System.Threading;

using AForge;
using AForge.Neuro;
using AForge.Neuro.Learning;
using AForge.Controls;

namespace Neural
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public partial class Form1 : System.Windows.Forms.Form
    {
        private AForge.Controls.Chart chart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button loadDataButton;
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
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.chart = new AForge.Controls.Chart();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.inputCountBox = new System.Windows.Forms.TextBox();
            this.fileTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.classesBox = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.loadDataButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.learningRateBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.validErrorBox = new System.Windows.Forms.TextBox();
            this.errorPercent = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.currentIterationBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveNetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveWeightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TestNetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewTopologyNetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.dataGridViewWeights = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.graphInput = new ZedGraph.ZedGraphControl();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWeights)).BeginInit();
            this.SuspendLayout();
            // 
            // chart
            // 
            this.chart.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.chart.Location = new System.Drawing.Point(28, 222);
            this.chart.Name = "chart";
            this.chart.RangeX = ((AForge.Range)(resources.GetObject("chart.RangeX")));
            this.chart.RangeY = ((AForge.Range)(resources.GetObject("chart.RangeY")));
            this.chart.Size = new System.Drawing.Size(242, 231);
            this.chart.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.inputCountBox);
            this.groupBox1.Controls.Add(this.fileTextBox);
            this.groupBox1.Controls.Add(this.chart);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.classesBox);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.loadDataButton);
            this.groupBox1.Location = new System.Drawing.Point(3, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(276, 525);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Выборка";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(198, 502);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Вх.:";
            // 
            // inputCountBox
            // 
            this.inputCountBox.Enabled = false;
            this.inputCountBox.Location = new System.Drawing.Point(227, 499);
            this.inputCountBox.Name = "inputCountBox";
            this.inputCountBox.ReadOnly = true;
            this.inputCountBox.Size = new System.Drawing.Size(43, 20);
            this.inputCountBox.TabIndex = 6;
            // 
            // fileTextBox
            // 
            this.fileTextBox.Enabled = false;
            this.fileTextBox.Location = new System.Drawing.Point(136, 499);
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.ReadOnly = true;
            this.fileTextBox.Size = new System.Drawing.Size(56, 20);
            this.fileTextBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(295, 436);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Классы:";
            // 
            // classesBox
            // 
            this.classesBox.Enabled = false;
            this.classesBox.Location = new System.Drawing.Point(350, 433);
            this.classesBox.Name = "classesBox";
            this.classesBox.ReadOnly = true;
            this.classesBox.Size = new System.Drawing.Size(26, 20);
            this.classesBox.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(265, 471);
            this.dataGridView1.TabIndex = 2;
            // 
            // loadDataButton
            // 
            this.loadDataButton.Location = new System.Drawing.Point(6, 496);
            this.loadDataButton.Name = "loadDataButton";
            this.loadDataButton.Size = new System.Drawing.Size(124, 23);
            this.loadDataButton.TabIndex = 1;
            this.loadDataButton.Text = " Загрузить из файла";
            this.loadDataButton.Click += new System.EventHandler(this.loadDataButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            this.openFileDialog.Title = "Select data file";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.learningRateBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(834, 27);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(195, 49);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Настройки";
            // 
            // learningRateBox
            // 
            this.learningRateBox.Location = new System.Drawing.Point(124, 20);
            this.learningRateBox.Name = "learningRateBox";
            this.learningRateBox.Size = new System.Drawing.Size(60, 20);
            this.learningRateBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Коэф. обучения:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.validErrorBox);
            this.groupBox4.Controls.Add(this.errorPercent);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.currentIterationBox);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(834, 82);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(195, 100);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Текущая итерация";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 77);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Валидация";
            // 
            // validErrorBox
            // 
            this.validErrorBox.Location = new System.Drawing.Point(71, 74);
            this.validErrorBox.Name = "validErrorBox";
            this.validErrorBox.ReadOnly = true;
            this.validErrorBox.Size = new System.Drawing.Size(113, 20);
            this.validErrorBox.TabIndex = 5;
            // 
            // errorPercent
            // 
            this.errorPercent.Location = new System.Drawing.Point(58, 16);
            this.errorPercent.Name = "errorPercent";
            this.errorPercent.ReadOnly = true;
            this.errorPercent.Size = new System.Drawing.Size(126, 20);
            this.errorPercent.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Ошибка:";
            // 
            // currentIterationBox
            // 
            this.currentIterationBox.Location = new System.Drawing.Point(71, 48);
            this.currentIterationBox.Name = "currentIterationBox";
            this.currentIterationBox.ReadOnly = true;
            this.currentIterationBox.Size = new System.Drawing.Size(113, 20);
            this.currentIterationBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "Итерация:";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(108, 19);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(81, 23);
            this.stopButton.TabIndex = 8;
            this.stopButton.Text = "Остановить";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(6, 19);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(105, 23);
            this.startButton.TabIndex = 7;
            this.startButton.Text = "Начать обучение";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.startButton);
            this.groupBox5.Controls.Add(this.stopButton);
            this.groupBox5.Location = new System.Drawing.Point(834, 188);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(195, 53);
            this.groupBox5.TabIndex = 11;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = " Обучение";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveToolStripMenuItem,
            this.TestNetToolStripMenuItem,
            this.ViewTopologyNetToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1029, 24);
            this.menuStrip1.TabIndex = 19;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveNetToolStripMenuItem,
            this.SaveWeightsToolStripMenuItem});
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.SaveToolStripMenuItem.Text = "Сохранить";
            // 
            // SaveNetToolStripMenuItem
            // 
            this.SaveNetToolStripMenuItem.Name = "SaveNetToolStripMenuItem";
            this.SaveNetToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.SaveNetToolStripMenuItem.Text = "Сохранить сеть";
            // 
            // SaveWeightsToolStripMenuItem
            // 
            this.SaveWeightsToolStripMenuItem.Name = "SaveWeightsToolStripMenuItem";
            this.SaveWeightsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.SaveWeightsToolStripMenuItem.Text = "Сохранить веса";
            this.SaveWeightsToolStripMenuItem.Click += new System.EventHandler(this.SaveWeightsToolStripMenuItem_Click);
            // 
            // TestNetToolStripMenuItem
            // 
            this.TestNetToolStripMenuItem.Name = "TestNetToolStripMenuItem";
            this.TestNetToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.TestNetToolStripMenuItem.Text = "Тест";
            this.TestNetToolStripMenuItem.Click += new System.EventHandler(this.TestNetToolStripMenuItem_Click);
            // 
            // ViewTopologyNetToolStripMenuItem
            // 
            this.ViewTopologyNetToolStripMenuItem.Name = "ViewTopologyNetToolStripMenuItem";
            this.ViewTopologyNetToolStripMenuItem.Size = new System.Drawing.Size(104, 20);
            this.ViewTopologyNetToolStripMenuItem.Text = "Редактирование";
            this.ViewTopologyNetToolStripMenuItem.Click += new System.EventHandler(this.ViewTopologyNetToolStripMenuItem_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.graphInput);
            this.groupBox2.Controls.Add(this.zedGraphControl1);
            this.groupBox2.Controls.Add(this.dataGridViewWeights);
            this.groupBox2.Location = new System.Drawing.Point(285, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(543, 522);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Изменение ошибки обучения";
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.zedGraphControl1.Location = new System.Drawing.Point(6, 16);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(531, 264);
            this.zedGraphControl1.TabIndex = 1;
            // 
            // dataGridViewWeights
            // 
            this.dataGridViewWeights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWeights.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.dataGridViewWeights.Location = new System.Drawing.Point(254, 286);
            this.dataGridViewWeights.Name = "dataGridViewWeights";
            this.dataGridViewWeights.RowHeadersVisible = false;
            this.dataGridViewWeights.Size = new System.Drawing.Size(283, 227);
            this.dataGridViewWeights.TabIndex = 4;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Слой";
            this.Column1.Name = "Column1";
            this.Column1.Width = 60;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Нейрон";
            this.Column2.Name = "Column2";
            this.Column2.Width = 60;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Вес";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 60;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Значение";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // graphInput
            // 
            this.graphInput.Location = new System.Drawing.Point(6, 286);
            this.graphInput.Name = "graphInput";
            this.graphInput.ScrollGrace = 0D;
            this.graphInput.ScrollMaxX = 0D;
            this.graphInput.ScrollMaxY = 0D;
            this.graphInput.ScrollMaxY2 = 0D;
            this.graphInput.ScrollMinX = 0D;
            this.graphInput.ScrollMinY = 0D;
            this.graphInput.ScrollMinY2 = 0D;
            this.graphInput.Size = new System.Drawing.Size(242, 230);
            this.graphInput.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1029, 552);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Learning Neuro Classificator";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWeights)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private GroupBox groupBox5;
        private TextBox errorPercent;
        private Label label7;
        private TextBox validErrorBox;
        private SaveFileDialog saveFileDialog1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem SaveToolStripMenuItem;
        private ToolStripMenuItem TestNetToolStripMenuItem;
        private ToolStripMenuItem ViewTopologyNetToolStripMenuItem;
        private DataGridView dataGridView1;
        private Label label2;
        private TextBox classesBox;
        private GroupBox groupBox2;
        private Label label4;
        private TextBox inputCountBox;
        private DataGridView dataGridViewWeights;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private TextBox fileTextBox;
        private ToolStripMenuItem SaveNetToolStripMenuItem;
        private ToolStripMenuItem SaveWeightsToolStripMenuItem;
        private ZedGraph.ZedGraphControl graphInput;

    }
}
