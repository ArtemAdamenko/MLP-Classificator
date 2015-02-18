namespace Neural
{
    partial class HelloForm
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
            this.typeLearningBox = new System.Windows.Forms.GroupBox();
            this.RegressionButton = new System.Windows.Forms.Button();
            this.classificationButton = new System.Windows.Forms.Button();
            this.actionsBox = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.createButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.typeLearningBox.SuspendLayout();
            this.actionsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // typeLearningBox
            // 
            this.typeLearningBox.Controls.Add(this.RegressionButton);
            this.typeLearningBox.Controls.Add(this.classificationButton);
            this.typeLearningBox.Location = new System.Drawing.Point(256, 187);
            this.typeLearningBox.Name = "typeLearningBox";
            this.typeLearningBox.Size = new System.Drawing.Size(200, 80);
            this.typeLearningBox.TabIndex = 0;
            this.typeLearningBox.TabStop = false;
            this.typeLearningBox.Text = "Задачи сети";
            // 
            // RegressionButton
            // 
            this.RegressionButton.Enabled = false;
            this.RegressionButton.Location = new System.Drawing.Point(6, 48);
            this.RegressionButton.Name = "RegressionButton";
            this.RegressionButton.Size = new System.Drawing.Size(188, 23);
            this.RegressionButton.TabIndex = 2;
            this.RegressionButton.Text = "Регрессия";
            this.RegressionButton.UseVisualStyleBackColor = true;
            this.RegressionButton.Click += new System.EventHandler(this.RegressionButton_Click);
            // 
            // classificationButton
            // 
            this.classificationButton.Enabled = false;
            this.classificationButton.Location = new System.Drawing.Point(6, 19);
            this.classificationButton.Name = "classificationButton";
            this.classificationButton.Size = new System.Drawing.Size(188, 23);
            this.classificationButton.TabIndex = 1;
            this.classificationButton.Text = "Классификация";
            this.classificationButton.UseVisualStyleBackColor = true;
            this.classificationButton.Click += new System.EventHandler(this.classificationButton_Click);
            // 
            // actionsBox
            // 
            this.actionsBox.Controls.Add(this.button2);
            this.actionsBox.Controls.Add(this.createButton);
            this.actionsBox.Location = new System.Drawing.Point(21, 187);
            this.actionsBox.Name = "actionsBox";
            this.actionsBox.Size = new System.Drawing.Size(200, 80);
            this.actionsBox.TabIndex = 1;
            this.actionsBox.TabStop = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 51);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(188, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Изменить сеть";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(6, 19);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(188, 23);
            this.createButton.TabIndex = 2;
            this.createButton.Text = "Создать сеть";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Enabled = false;
            this.nextButton.Location = new System.Drawing.Point(375, 283);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(75, 23);
            this.nextButton.TabIndex = 2;
            this.nextButton.Text = "Далее";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Neural.Properties.Resources.hello;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(459, 169);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // HelloForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 321);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.actionsBox);
            this.Controls.Add(this.typeLearningBox);
            this.Name = "HelloForm";
            this.Text = "Нейронные сети";
            this.typeLearningBox.ResumeLayout(false);
            this.actionsBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox typeLearningBox;
        private System.Windows.Forms.Button RegressionButton;
        private System.Windows.Forms.Button classificationButton;
        private System.Windows.Forms.GroupBox actionsBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}