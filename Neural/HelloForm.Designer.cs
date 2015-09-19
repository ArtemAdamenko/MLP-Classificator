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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelloForm));
            this.actionsBox = new System.Windows.Forms.GroupBox();
            this.evolutionLearnButton = new System.Windows.Forms.Button();
            this.GeneticButton = new System.Windows.Forms.Button();
            this.pullWeightsButton = new System.Windows.Forms.Button();
            this.correctButton = new System.Windows.Forms.Button();
            this.changeNetbutton = new System.Windows.Forms.Button();
            this.createButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.историяИзмененийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.авторыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actionsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // actionsBox
            // 
            this.actionsBox.Controls.Add(this.evolutionLearnButton);
            this.actionsBox.Controls.Add(this.GeneticButton);
            this.actionsBox.Controls.Add(this.pullWeightsButton);
            this.actionsBox.Controls.Add(this.correctButton);
            this.actionsBox.Controls.Add(this.changeNetbutton);
            this.actionsBox.Controls.Add(this.createButton);
            this.actionsBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.actionsBox.Location = new System.Drawing.Point(9, 216);
            this.actionsBox.Name = "actionsBox";
            this.actionsBox.Size = new System.Drawing.Size(531, 211);
            this.actionsBox.TabIndex = 1;
            this.actionsBox.TabStop = false;
            this.actionsBox.Text = "Операции";
            // 
            // evolutionLearnButton
            // 
            this.evolutionLearnButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.evolutionLearnButton.Location = new System.Drawing.Point(6, 153);
            this.evolutionLearnButton.Name = "evolutionLearnButton";
            this.evolutionLearnButton.Size = new System.Drawing.Size(273, 46);
            this.evolutionLearnButton.TabIndex = 4;
            this.evolutionLearnButton.Text = "Эволюционный алгоритм обучения";
            this.evolutionLearnButton.UseVisualStyleBackColor = true;
            this.evolutionLearnButton.Click += new System.EventHandler(this.evolutionLearnButton_Click);
            // 
            // GeneticButton
            // 
            this.GeneticButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GeneticButton.Location = new System.Drawing.Point(6, 109);
            this.GeneticButton.Name = "GeneticButton";
            this.GeneticButton.Size = new System.Drawing.Size(273, 38);
            this.GeneticButton.TabIndex = 6;
            this.GeneticButton.Text = "Генетический алгоритм";
            this.GeneticButton.UseVisualStyleBackColor = true;
            this.GeneticButton.Click += new System.EventHandler(this.GeneticButton_Click);
            // 
            // pullWeightsButton
            // 
            this.pullWeightsButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pullWeightsButton.Location = new System.Drawing.Point(6, 65);
            this.pullWeightsButton.Name = "pullWeightsButton";
            this.pullWeightsButton.Size = new System.Drawing.Size(273, 38);
            this.pullWeightsButton.TabIndex = 5;
            this.pullWeightsButton.Text = "Обучение методом перебора весов";
            this.pullWeightsButton.UseVisualStyleBackColor = true;
            this.pullWeightsButton.Click += new System.EventHandler(this.pullWeightsButton_Click);
            // 
            // correctButton
            // 
            this.correctButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.correctButton.Location = new System.Drawing.Point(285, 65);
            this.correctButton.Name = "correctButton";
            this.correctButton.Size = new System.Drawing.Size(240, 38);
            this.correctButton.TabIndex = 4;
            this.correctButton.Text = "Корректирование ИНС";
            this.correctButton.UseVisualStyleBackColor = true;
            this.correctButton.Click += new System.EventHandler(this.correctButton_Click);
            // 
            // changeNetbutton
            // 
            this.changeNetbutton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.changeNetbutton.Location = new System.Drawing.Point(285, 19);
            this.changeNetbutton.Name = "changeNetbutton";
            this.changeNetbutton.Size = new System.Drawing.Size(240, 40);
            this.changeNetbutton.TabIndex = 3;
            this.changeNetbutton.Text = "Повреждение ИНС";
            this.changeNetbutton.UseVisualStyleBackColor = true;
            this.changeNetbutton.Click += new System.EventHandler(this.button2_Click);
            // 
            // createButton
            // 
            this.createButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.createButton.Location = new System.Drawing.Point(6, 19);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(273, 38);
            this.createButton.TabIndex = 2;
            this.createButton.Text = "Алгоритм обратного распространения ошибки";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::ANNBuilder.Properties.Resources._1;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(15, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(528, 190);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.историяИзмененийToolStripMenuItem,
            this.авторыToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(555, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // историяИзмененийToolStripMenuItem
            // 
            this.историяИзмененийToolStripMenuItem.Name = "историяИзмененийToolStripMenuItem";
            this.историяИзмененийToolStripMenuItem.Size = new System.Drawing.Size(130, 20);
            this.историяИзмененийToolStripMenuItem.Text = "История изменений";
            // 
            // авторыToolStripMenuItem
            // 
            this.авторыToolStripMenuItem.Name = "авторыToolStripMenuItem";
            this.авторыToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.авторыToolStripMenuItem.Text = "Авторы";
            // 
            // HelloForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(555, 429);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.actionsBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "HelloForm";
            this.Text = "ANNBuilder 1.8.0.";
            this.actionsBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox actionsBox;
        private System.Windows.Forms.Button changeNetbutton;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button correctButton;
        private System.Windows.Forms.Button pullWeightsButton;
        private System.Windows.Forms.Button GeneticButton;
        private System.Windows.Forms.Button evolutionLearnButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem историяИзмененийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem авторыToolStripMenuItem;
    }
}