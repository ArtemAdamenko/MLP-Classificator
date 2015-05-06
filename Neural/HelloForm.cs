using System;
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

        public HelloForm()
        {
            InitializeComponent();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            LearnANNForm form = new LearnANNForm();
            form.Show(this);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChangeANNForm form = new ChangeANNForm();
            form.Show(this);

        }

        private void correctButton_Click(object sender, EventArgs e)
        {
            CorrectANNForm form = new CorrectANNForm();
            form.Show(this);

        }
    }
}
