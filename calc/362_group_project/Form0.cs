using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Calculator
{
    public partial class FormResult0 : Form
    {

        public FormResult0()
        {
            InitializeComponent();
        }

        private void mainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void standardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult1 formResult1 = new FormResult1();
            formResult1.Show();
        }

        private void tipsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult2 formResult2 = new FormResult2();
            formResult2.Show();
        }

        private void shoppingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult3 formResult3 = new FormResult3();
            formResult3.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult4 formResult4 = new FormResult4();
            formResult4.Show();
        }

        private void incomeExpenseTrackerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult5 formResult5 = new FormResult5();
            formResult5.Show();
        }

        private void isItWorthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult6 formResult6 = new FormResult6();
            formResult6.Show();
        }

        private void About_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult4 formResult4 = new FormResult4();
            formResult4.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult1 formResult1 = new FormResult1();
            formResult1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult2 formResult2 = new FormResult2();
            formResult2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult3 formResult3 = new FormResult3();
            formResult3.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult5 formResult5 = new FormResult5();
            formResult5.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult6 formResult6 = new FormResult6();
            formResult6.Show();
        }
    }
}


