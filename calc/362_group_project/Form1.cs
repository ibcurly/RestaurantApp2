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
    public partial class FormResult1 : Form
    {
        Double resultValue = 0;
        String operationPerformed = "";
        String currentEquation = "";
        String equationAfterBackspace;
        Double currentValue = 0;
        bool isOperationPerformed = false;
        bool isOperatorChanged = false;
        String textBoxUnformattedText = "";
        String textboxValue = "";

        public FormResult1()
        {
            InitializeComponent();
        }

       
        private void setAndFormatText(string text)
        {
            textBoxUnformattedText = text;
        }

        private void number_Click(object sender, EventArgs e)
        {
            // receive the clicked number and show it on the display
            Button number = (Button)sender;

            // clear the display for the first 0 (only 1 zero) or when the user enters the next number
            if ((textBoxInput.Text == "0") || (isOperationPerformed))
            {
                if (number.Text == ".")
                {
                    textBoxInput.Text = "0";            // accept both .x and 0.x
                }
                else
                {
                    textBoxInput.Clear();
                }
            }

            // starting the new number after the operator is clicked
            isOperationPerformed = false;

            // only allow one decimal
            if (number.Text == ".")
            {
                if (!textBoxInput.Text.Contains("."))
                {
                    textBoxInput.Text = textBoxInput.Text + number.Text;
                }
            }
            else
            {
                textBoxInput.Text = textBoxInput.Text + number.Text;
            }
        }

        // clicking any operator (+, -, *, /)
        private void operator_Click(object sender, EventArgs e)
        {
            Button operation = (Button)sender;
            currentValue = Double.Parse(textBoxInput.Text);

            if (resultValue != 0)
            {
                // check that the operator can be changed
                if (!isOperatorChanged)
                {
                    buttonEqual.PerformClick();
                    operationPerformed = operation.Text;
                    resultValue = Double.Parse(textBoxInput.Text);
                    currentEquation += currentValue + " " + operationPerformed + " ";
                    currentOperation.Text = currentEquation;
                    isOperationPerformed = true;
                }
            }
            else
            {
                operationPerformed = operation.Text;
                resultValue = Double.Parse(textBoxInput.Text);
                currentEquation += currentValue + " " + operationPerformed + " ";
                currentOperation.Text = currentEquation;
                isOperationPerformed = true;
            }
            textboxValue = resultValue.ToString();       // set textboxValue to the current result
            textBoxInput.Text = "";                 // show blank textBoxInput
        }

        // clicking all_clear button
        private void allclear_Click(object sender, EventArgs e)
        {
            textBoxInput.Text = "0";
            textboxValue = "";
            resultValue = 0;
            currentOperation.Text = "";
            currentEquation = "";
        }

        // clicking clear button
        private void clear_Click(object sender, EventArgs e)
        {
            textBoxInput.Text = "0";
        }

        // clicking = button
        private void equal_Click(object sender, EventArgs e)
        {
           switch (operationPerformed)
            {
                case "+":
                    textBoxInput.Text = (resultValue + Double.Parse(textBoxInput.Text)).ToString();
                    break;
                case "-":
                    textBoxInput.Text = (resultValue - Double.Parse(textBoxInput.Text)).ToString();
                    break;
                case "*":
                    textBoxInput.Text = (resultValue * Double.Parse(textBoxInput.Text)).ToString();
                    break;
                case "/":
                    textBoxInput.Text = (resultValue / Double.Parse(textBoxInput.Text)).ToString();
                    break;
                default:
                    break;
            }
            if (textBoxInput.Text.Contains("."))        // for every non-integer number, round the number to 1-2 decimal points
            {
                resultValue = Math.Round(Double.Parse(textBoxInput.Text), 2, MidpointRounding.AwayFromZero);        // for resultValue -- type double
                textBoxInput.Text = Double.Parse(textBoxInput.Text).ToString("0.##");                               // for display the number - type text
            }
            else
            {
                resultValue = Double.Parse(textBoxInput.Text);
            }
            currentOperation.Text = "";
        }
        
        private void menuToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult0 formresult0 = new FormResult0();
            formresult0.Show();
        }

        private void standardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void tipsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult2 formresult2 = new FormResult2();
            formresult2.Show();
        }

        private void shoppingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult3 formresult3 = new FormResult3();
            formresult3.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult4 formresult4 = new FormResult4();
            formresult4.Show();
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

        // clicking backspace button
        private void backspace_Click(object sender, EventArgs e)
        {
            equationAfterBackspace = textBoxInput.Text;

            if (equationAfterBackspace.Length > 1)
            {
                equationAfterBackspace = equationAfterBackspace.Substring(0, equationAfterBackspace.Length - 1);
            }
            else
            {
                equationAfterBackspace = "0";
            }

            textBoxInput.Text = equationAfterBackspace;
        }

        // clicking negatation button
        private void negate_Click(object sender, EventArgs e)
        {
            if (textBoxInput.Text.StartsWith("-"))
            {
                // It's negative, remove '-' sign to make it positive
                textBoxInput.Text = textBoxInput.Text.Substring(1);
            }
            else if (!string.IsNullOrEmpty(textBoxInput.Text) && decimal.Parse(textBoxInput.Text) != 0)
            {
                // It's positive, add '-' sign to make it negative
                textBoxInput.Text = "-" + textBoxInput.Text;
            }
        }

        private void textBoxInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46) // if the char is not number, backspace, .
            {
                e.Handled = true;
            }

            switch (ch)
            {
                case '+': buttonAdd.PerformClick();
                    break;
                case '-': buttonSubtract.PerformClick();
                    break;
                case '*': buttonMultiply.PerformClick();
                    break;
                case '/': buttonDivide.PerformClick();
                    break;
            }
            if (ch == 13)
            {
                buttonEqual.PerformClick();
            }
        }

        private void textbox_Click(object sender, EventArgs e)
        {
            textBoxInput.Text = "";
        }
    }
}
