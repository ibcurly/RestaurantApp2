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
    public partial class FormResult2 : Form
    {
        Double resultTipsAmount = 0;
        Double resultTotalAmount = 0;
        Double resultTipsAmountForPerPerson = 0;
        Double resultTotalAmountforPerPerson = 0;
        Double valueBillAmount = 0;
        Double valueTipsRate = 0;
        int valueNumberPeople = 0;
        string tipsMessage = "Total Tip: ";
        string billMessage = "Total Bill + Tip: ";
        string tipsMessagePerson = "Tip Each: ";
        string billMessagePerson = "Total Each: ";  

        TextBox focusedText;

        public FormResult2()
        {
            InitializeComponent();
            tbPriceInput.Text = "$";
            tbTipsInput.Text = "%";
            tbNumPeopleInput.Text = "1";  
        }
        
        //clicks number 0 ~ 9 buttons
        private void number_Click(object sender, EventArgs e)
        {
            if ((tbPriceInput.Text == "$") || (tbTipsInput.Text == "%"))
            {
                tbPriceInput.Clear();
                tbTipsInput.Clear();
                tbNumPeopleInput.Clear();
            }

            Button number = (Button)sender;
            updateText(number);
        }

        private void updateText(Button number)
        {
            if (focusedText == null)
                return;

            if (number.Text == "." && focusedText.Text.Contains("."))
                return;

            String newText = focusedText.Text + number.Text;         
            focusedText.Text = newText;
        }

        private void updateTextboxFocus(Object sender, EventArgs e)
        {
            focusedText = (TextBox)sender;
        }
        
        private double safeDouble(String s)
        {
            String cleaned = Regex.Replace(s, "[^0-9.]", "");
            double result = 0;

            Double.TryParse(cleaned, out result);

            return result;
        }
        
        private void tbPriceInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }

            if (ch == 13)
            {
                btEqualSign.PerformClick();
            }
        }

        private void tbPriceInput_TextChanged(object sender, EventArgs e)
        {
            valueBillAmount = safeDouble(tbPriceInput.Text);
            
            tbPriceInput.Select(tbPriceInput.Text.Length, 0);
        }   
        private void tbTipsInput_TextChanged(object sender, EventArgs e)
        {
            valueTipsRate = safeDouble(tbTipsInput.Text);
        }

        private void tbNumPeopleInput_TextChanged(object sender, EventArgs e)
        {
            valueNumberPeople = (int)safeDouble(tbNumPeopleInput.Text);
        }

        // clicks = button
        private void equal_Click(object sender, EventArgs e)
        {
            if (tbPriceInput.Text == "" || tbNumPeopleInput.Text == "" || tbTipsInput.Text == "" || tbTipsInput.Text == "%" || tbPriceInput.Text == "$" || tbNumPeopleInput.Text == "0")
            {
                requiredFieldLabel1.Hide();
                requiredFieldLabel2.Hide();
                requiredFieldLabel3.Hide();
                if (tbPriceInput.Text == "" || tbPriceInput.Text == "$")
                {
                    requiredFieldLabel1.Show();
                }
                if (tbTipsInput.Text == "" || tbTipsInput.Text == "%")
                {
                    requiredFieldLabel2.Show();
                }
                if (tbNumPeopleInput.Text == "" || tbNumPeopleInput.Text == "0")
                {
                    requiredFieldLabel3.Show();
                }
                MessageBox.Show("The required field(s) cannot be empty!", "Error: Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } else
            {
                requiredFieldLabel1.Hide();
                requiredFieldLabel2.Hide();
                requiredFieldLabel3.Hide();

                //tips amount 
                resultTipsAmount = valueBillAmount * valueTipsRate / 100;
                //total amount
                resultTotalAmount = valueBillAmount + resultTipsAmount;
                //tips per person
                resultTipsAmountForPerPerson = resultTipsAmount / valueNumberPeople;
                //total per person
                resultTotalAmountforPerPerson = resultTotalAmount / valueNumberPeople;

                //convert double to string value
                //appear the value in the label of tips amount
                lbTipsAmount.Text = tipsMessage + convertDobuletoString(resultTipsAmount);
                //convert double to string value
                //appear the value in the label of bill amount
                lbTotalAmount.Text = billMessage + convertDobuletoString(resultTotalAmount);
                //convert double to string value
                //appear the value in the label of per person tips amount
                lbTipsAmountPerson.Text = tipsMessagePerson + convertDobuletoString(resultTipsAmountForPerPerson);
                //convert double to string value
                //appear the value in the label of per person total amount
                lbTotalAmountPerson.Text = billMessagePerson + convertDobuletoString(resultTotalAmountforPerPerson);

                groupBox1.Visible = true;
            }
            

        }

        private string convertDobuletoString(double doubleValue)
        {
             string stringValue = string.Format("{0:F}", doubleValue);

            return stringValue;
        }

        private void tipsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult0 formResult0 = new FormResult0();
            formResult0.Show();
        }

        // clicks clear button
        private void clear_Click(object sender, EventArgs e)
        {
            tbPriceInput.Text = "$";
            tbTipsInput.Text = "%";
            tbNumPeopleInput.Text = "1";

            lbTipsAmount.Text = "";
            lbTotalAmount.Text = "";
            lbTipsAmountPerson.Text = "";
            lbTotalAmountPerson.Text = "";

            groupBox1.Visible = false;

            requiredFieldLabel1.Hide();
            requiredFieldLabel2.Hide();
            requiredFieldLabel3.Hide();
        }

        private void btback_Click(object sender, EventArgs e)
        {
            if (focusedText.Text.Length > 0)
                focusedText.Text = focusedText.Text.Substring(0, focusedText.Text.Length - 1);
        }

        private void standardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult1 formResult1 = new FormResult1();
            formResult1.Show();
        }

        private void tipsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Show();           
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

        // clear textbox when click
        private void tbPriceInput_Click(object sender, EventArgs e)
        {
            tbPriceInput.Text = "";
        }

        private void tbTipsInput_Click(object sender, EventArgs e)
        {
            tbTipsInput.Text = "";
        }

        private void tbNumPeopleInput_Click(object sender, EventArgs e)
        {
            tbNumPeopleInput.Text = "";
        }

        private void Form_Load(object sender, EventArgs e)
        {
            requiredFieldLabel1.Hide();
            requiredFieldLabel2.Hide();
            requiredFieldLabel3.Hide();
        }
    }
}


