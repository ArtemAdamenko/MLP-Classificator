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
            this.correctButton = new System.Windows.Forms.Button();
            this.changeNetbutton = new System.Windows.Forms.Button();
            this.createButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.actionsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // actionsBox
            // 
            this.actionsBox.Controls.Add(this.correctButton);
            this.actionsBox.Controls.Add(this.changeNetbutton);
            this.actionsBox.Controls.Add(this.createButton);
            this.actionsBox.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.actionsBox.Location = new System.Drawing.Point(92, 187);
            this.actionsBox.Name = "actionsBox";
            this.actionsBox.Size = new System.Drawing.Size(285, 143);
            this.actionsBox.TabIndex = 1;
            this.actionsBox.TabStop = false;
            this.actionsBox.Text = "Операции";
            // 
            // correctButton
            // 
            this.correctButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.correctButton.Location = new System.Drawing.Point(6, 104);
            this.correctButton.Name = "correctButton";
            this.correctButton.Size = new System.Drawing.Size(273, 33);
            this.correctButton.TabIndex = 4;
            this.correctButton.Text = "Корректировать сеть";
            this.correctButton.UseVisualStyleBackColor = true;
            this.correctButton.Click += new System.EventHandler(this.correctButton_Click);
            // 
            // changeNetbutton
            // 
            this.changeNetbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.changeNetbutton.Location = new System.Drawing.Point(6, 63);
            this.changeNetbutton.Name = "changeNetbutton";
            this.changeNetbutton.Size = new System.Drawing.Size(273, 35);
            this.changeNetbutton.TabIndex = 3;
            this.changeNetbutton.Text = "Изменить сеть";
            this.changeNetbutton.UseVisualStyleBackColor = true;
            this.changeNetbutton.Click += new System.EventHandler(this.button2_Click);
            // 
            // createButton
            // 
            this.createButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createButton.Location = new System.Drawing.Point(6, 19);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(273, 38);
            this.createButton.TabIndex = 2;
            this.createButton.Text = "Создать сеть";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Neural.Properties.Resources._1;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(459, 169);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // HelloForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(483, 342);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.actionsBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HelloForm";
            this.Text = "Нейронные сети v1.4.5";
            this.actionsBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox actionsBox;
        private System.Windows.Forms.Button changeNetbutton;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button correctButton;
    }
}