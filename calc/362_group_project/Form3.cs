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
    public partial class FormResult3 : Form
    {

        double valueBillAmount = 0;
      
        double percentTaxRate = 0;
        double valueFirstDiscount = 0;
        double valueSecondDiscount = 0;
        double valueThirdDiscount = 0;
        double valueSubTotal = 0;
        double valueTotal = 0;

        double valueDiscountRateOne = 0;
        double valueDiscountRateTwo = 0;
        double valueDiscountRateThree = 0;

        double valueDiscountOne = 0;
        double valueDiscountTwo = 0;
        double valueDiscountThree = 0;

        string msgSubTotal = "Original Price: $";
        string msgRebate = "You Saved: $";
        string msgTotal = "After Discount: $";

        string unitTypeOne = "";
        string unitTypeTwo = "";
        string unitTypeThree = "";

        String currentEquation2 = "";
        TextBox focusedText;

        public FormResult3()
        {
            InitializeComponent();
            
            tbPriceInput.Text = "$";
          
            cmBoxUnit1.Items.Add("");
            cmBoxUnit2.Items.Add("");
            cmBoxUnit3.Items.Add("");            
        }
               
        //clicks number 0 ~ 9 buttons
        private void number_Click(object sender, EventArgs e)
        {
            if (tbPriceInput.Text == "$")
            {
                tbPriceInput.Clear();
                
                tbDiscountInput1.Clear();
                tbDiscountInput2.Clear();
                tbDiscountInput3.Clear();

                cmBoxUnit1.Items.Clear();
                cmBoxUnit1.Items.Add("%");
                cmBoxUnit1.Items.Add("Dollar");
                cmBoxUnit1.SelectedIndex = cmBoxUnit1.FindStringExact("%");

                cmBoxUnit2.Items.Clear();
                cmBoxUnit2.Items.Add("%");
                cmBoxUnit2.Items.Add("Dollar");
                cmBoxUnit2.SelectedIndex = cmBoxUnit2.FindStringExact("%");

                cmBoxUnit3.Items.Clear();
                cmBoxUnit3.Items.Add("%");
                cmBoxUnit3.Items.Add("Dollar");
                cmBoxUnit3.SelectedIndex = cmBoxUnit3.FindStringExact("%");
            }

            Button number = (Button)sender;
            updateText(number);
        }

        private void updateText(Button number)
        {
            if (focusedText == null)
                return;

            String newText = focusedText.Text + number.Text;

            focusedText.Text = newText;
        }

        private void updateTextboxFocus(Object sender, EventArgs e)
        {
            focusedText = (TextBox)sender;
        }

        private void tbPriceInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch !=8 && ch !=46)
            {
                e.Handled = true;
            }

            if (ch == 13)
            {
                btEqualSign.PerformClick();
            }
        }
        
        private Boolean isOperationChosen()
        {
            return currentEquation2 != "";
        }

        private double safeDouble(String s)
        {
            String cleaned = Regex.Replace(s, "[^0-9.]", "");
            double result = 0;

            Double.TryParse(cleaned, out result);

            return result;
        }

        private double computeDiscount(double currentTotal, double discountValue, Boolean isRate)
        {
            if (isRate)             // if it's %
                return currentTotal * discountValue / 100;

            return discountValue;
        }
        
        private void tbPriceInput_TextChanged(object sender, EventArgs e)
        {
            valueBillAmount = safeDouble(tbPriceInput.Text);                   
        }
       
        private void tbDiscountInput1_TextChanged(object sender, EventArgs e)
        {
            valueFirstDiscount = safeDouble(tbDiscountInput1.Text);
        }

        private void tbDiscountInput2_TextChanged(object sender, EventArgs e)
        {
            valueSecondDiscount = safeDouble(tbDiscountInput2.Text);
        }

        private void tbDiscountInput3_TextChanged(object sender, EventArgs e)
        {
            valueThirdDiscount = safeDouble(tbDiscountInput3.Text);
        }
        
        private void equal_Click(object sender, EventArgs e)
        {
            if (tbPriceInput.Text == "" || tbPriceInput.Text == "$")
            {
                requiredFieldLabel.Show();
                MessageBox.Show("The required field cannot be empty!", "Error: Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } else
            {
                requiredFieldLabel.Hide();

                double savingOne = 0;
                double savingTwo = 0;
                double savingThree = 0;


                valueSubTotal = valueBillAmount;

                if (gboxItem1.Visible)
                {
                    savingOne = computeDiscount(valueSubTotal, safeDouble(tbDiscountInput1.Text), cmBoxUnit1.Text == "%");
                    valueSubTotal -= savingOne;
                }

                if (gboxItem2.Visible)
                {
                    savingTwo = computeDiscount(valueSubTotal, safeDouble(tbDiscountInput2.Text), cmBoxUnit2.Text == "%");
                    valueSubTotal -= savingTwo;
                }

                if (gboxItem3.Visible)
                {
                    savingThree = computeDiscount(valueSubTotal, safeDouble(tbDiscountInput3.Text), cmBoxUnit3.Text == "%");
                    valueSubTotal -= savingThree;
                }

                if (tbDiscountInput1.Text == "")
                {
                    pnlDiscoutOutput1.Visible = false;
                }
                else
                {
                    pnlDiscoutOutput1.Visible = true;
                    lbName1.Text = lbTitileName1.Text;
                    lbAmount1.Text = tbDiscountInput1.Text;
                    lbUniteRate1.Text = unitTypeOne;
                }

                if (tbDiscountInput2.Text == "")
                {
                    pnlDiscoutOutput2.Visible = false;
                }
                else
                {
                    pnlDiscoutOutput2.Visible = true;
                    lbName2.Text = lbTitileName2.Text;
                    lbAmount2.Text = tbDiscountInput2.Text;
                    lbUniteRate2.Text = unitTypeTwo;
                }

                if (tbDiscountInput3.Text == "")
                {
                    pnlDiscoutOutput3.Visible = false;

                }
                else
                {
                    pnlDiscoutOutput3.Visible = true;
                    lbName3.Text = lbTitileName3.Text;
                    lbAmount3.Text = tbDiscountInput3.Text;
                    lbUniteRate3.Text = unitTypeThree;
                }

                lbSubTotal.Text = msgSubTotal + tbPriceInput.Text;
                lbRebate.Text = msgRebate + (savingOne + savingTwo + savingThree);
                valueTotal = valueSubTotal - valueSubTotal * percentTaxRate;
                lbTotal.Text = msgTotal + valueTotal;

                pnlListItem.Visible = false;
                pnlOuput.Visible = true;
            }
            

        }        
       
        private string convertDobuletoString(double doubleValue)
        {
           
            string stringValue = string.Format("{0:F}", doubleValue);
           
            return stringValue;
        }
        
        // clicks clear button
        private void clear_Click(object sender, EventArgs e)
        {
            tbPriceInput.Text = "$";
            
            tbDiscountInput1.Clear();
            tbDiscountInput2.Clear();
            tbDiscountInput3.Clear();

            cmBoxUnit1.Items.Clear();
            cmBoxUnit1.Items.Add("%");
            cmBoxUnit1.Items.Add("Dollar");
            cmBoxUnit1.SelectedIndex = cmBoxUnit1.FindStringExact("%");

            cmBoxUnit2.Items.Clear();
            cmBoxUnit2.Items.Add("%");
            cmBoxUnit2.Items.Add("Dollar");
            cmBoxUnit2.SelectedIndex = cmBoxUnit2.FindStringExact("%");

            cmBoxUnit3.Items.Clear();
            cmBoxUnit3.Items.Add("%");
            cmBoxUnit3.Items.Add("Dollar");
            cmBoxUnit3.SelectedIndex = cmBoxUnit3.FindStringExact("%");

            pnlListItem.Visible = true;
            pnlOuput.Visible = false;

            gboxItem1.Visible = true;
            gboxItem2.Visible = false;
            gboxItem3.Visible = false;

            requiredFieldLabel.Hide();
        }

        private void btback_Click(object sender, EventArgs e)
        {
            if (focusedText.Text.Length > 0)
                focusedText.Text = focusedText.Text.Substring(0, focusedText.Text.Length - 1);
        }
        
        private void btAddItem_Click(object sender, EventArgs e)
        {
            if (gboxItem2.Visible == false)
            {
                gboxItem2.Visible = true;
            }
            else
            {
                gboxItem3.Visible = true;
            }
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
            this.Show();
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

        private void cmBoxUnit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmBoxUnit1.Text == "%")
            {
                valueDiscountRateOne = valueFirstDiscount / 100;
                unitTypeOne = "%";
            }
            else
            {
                valueDiscountOne = valueFirstDiscount;
                unitTypeOne = "Dollar";
            }
        }

        private void cmBoxUnit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmBoxUnit2.Text == "%")
            {
                valueDiscountRateTwo = valueSecondDiscount / 100;
                unitTypeTwo = "%";
            }
            else
            {
                valueDiscountTwo = valueSecondDiscount;
                unitTypeTwo = "Dollar";
            }
        }

        private void cmBoxUnit3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmBoxUnit3.Text == "%")
            {
                valueDiscountRateThree = valueThirdDiscount / 100;
                unitTypeThree = "%";
            }
            else
            {
                valueDiscountThree = valueThirdDiscount;
                unitTypeThree = "dollar";
            }
        }

        private void FormResult3_Load(object sender, EventArgs e)
        {
            requiredFieldLabel.Hide();
            pnlListItem.Visible = true;
            pnlOuput.Visible = false;

            cmBoxUnit1.Items.Clear();
            cmBoxUnit1.Items.Add("%");
            cmBoxUnit1.Items.Add("Dollar");
            cmBoxUnit1.SelectedIndex = cmBoxUnit1.FindStringExact("%");

            cmBoxUnit2.Items.Clear();
            cmBoxUnit2.Items.Add("%");
            cmBoxUnit2.Items.Add("Dollar");
            cmBoxUnit2.SelectedIndex = cmBoxUnit2.FindStringExact("%");

            cmBoxUnit3.Items.Clear();
            cmBoxUnit3.Items.Add("%");
            cmBoxUnit3.Items.Add("Dollar");
            cmBoxUnit3.SelectedIndex = cmBoxUnit3.FindStringExact("%");
            
            tbDiscountInput1.Clear();
            tbDiscountInput2.Clear();
            tbDiscountInput3.Clear();

            gboxItem1.Visible = true;
            gboxItem2.Visible = false;
            gboxItem3.Visible = false;
        }
    }
}


