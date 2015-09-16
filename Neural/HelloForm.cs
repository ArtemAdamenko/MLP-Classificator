using System;
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

        private void pullWeightsButton_Click(object sender, EventArgs e)
        {
            LearnPullANNForm form = new LearnPullANNForm();
            form.Show(this);
        }

        private void GeneticButton_Click(object sender, EventArgs e)
        {
            LearnGeneticANNForm form = new LearnGeneticANNForm();
            form.Show(this);
        }

        private void evolutionLearnButton_Click(object sender, EventArgs e)
        {
            EvolutionLearnForm form = new EvolutionLearnForm();
            form.Show(this);
        }
    }
}
