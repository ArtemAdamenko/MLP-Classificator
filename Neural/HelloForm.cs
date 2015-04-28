﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Neural
{
    public partial class HelloForm : Form
    {
        public String option = "";
        public String typeLearn = "";

        public HelloForm()
        {
            InitializeComponent();
        }

        private void start()
        {
            if (typeLearn != "")
            {
                if (option == "create")
                {
                    LearnNetForm form = new LearnNetForm();
                    form.Show(this);
                }
                if (option == "edit")
                {
                    FormDrawNeurons form = new FormDrawNeurons();
                    form.Show(this);
                }
                if (option == "correct")
                {
                    FormDrawCorrectNeurons form = new FormDrawCorrectNeurons();
                    form.Show(this);
                }
                this.classificationButton.Enabled = false;
                this.RegressionButton.Enabled = false;
            }
            
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            option = "create";
            this.classificationButton.Enabled = true;
            this.RegressionButton.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            option = "edit";
            this.classificationButton.Enabled = true;
            this.RegressionButton.Enabled = true;
        }

        private void classificationButton_Click(object sender, EventArgs e)
        {
            typeLearn = "classification";
            start();
        }

        private void RegressionButton_Click(object sender, EventArgs e)
        {
            typeLearn = "regression";
            start();
        }

        private void correctButton_Click(object sender, EventArgs e)
        {
            option = "correct";
            this.classificationButton.Enabled = true;
            this.RegressionButton.Enabled = true;
        }
    }
}
