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
using System.IO;
using System.Windows;


namespace Calculator
{
    public partial class FormResult4 : Form
    {
        public FormResult4()
        {
            InitializeComponent();
        }

        private void mainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult0 formResult0 = new FormResult0();
            formResult0.Show();
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

        private void discountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult3 formResult3 = new FormResult3();
            formResult3.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
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

        //blinding crate
        private void FormResult4_Load(object sender, EventArgs e)
        {
            
            richTextBox1.Text = _362_group_project.Properties.Resources.aboutMe;

            richTextBox1.Find("About the Everyday Calculator", RichTextBoxFinds.MatchCase);
            richTextBox1.SelectionFont = new Font("Arial", 12, FontStyle.Bold);

            richTextBox1.Find("Basic Calculator", RichTextBoxFinds.MatchCase);
            richTextBox1.SelectionFont = new Font("Arial", 12, FontStyle.Bold);

            richTextBox1.Find("Bill Splitting Calculator", RichTextBoxFinds.MatchCase);
            richTextBox1.SelectionFont = new Font("Arial", 12, FontStyle.Bold);
            
            richTextBox1.Find("Discount Calculator", RichTextBoxFinds.MatchCase);
            richTextBox1.SelectionFont = new Font("Arial", 12, FontStyle.Bold);
            richTextBox1.DeselectAll();
            richTextBox1.ReadOnly = true;
        }
    }
}


