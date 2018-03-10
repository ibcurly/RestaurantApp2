using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel; // must add reference file 'Microsoft.Office.Interop.Excel'!
using System.IO;    // added
using System.Configuration; // added


namespace Calculator
{
    public partial class FormResult5 : Form
    {
        Double income = 0;
        Double expense = 0;
        Double total = 0;
        Double incomeTotal = 0;
        Double expenseTotal = 0;
        //String valueAfterBackspace;

        public FormResult5()
        {
            InitializeComponent();
            datePicker.Text = DateTime.Now.Date.ToString();

        }


        private void mainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult0 formresult0 = new FormResult0();
            formresult0.Show();
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
        private void incomeExpenseTrackerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult4 formResult4 = new FormResult4();
            formResult4.Show();
        }

        private void isItWorthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormResult6 formResult6 = new FormResult6();
            formResult6.Show();
        }




        private void addIncome_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(comboType.Text) || String.IsNullOrEmpty(valueTextbox.Text))
            {
                if (String.IsNullOrEmpty(comboType.Text) && String.IsNullOrEmpty(valueTextbox.Text))    // change border/background to red
                {
                    requiredAmountBorder.Show();
                    requiredComboBorder.Show();
                }
                else if (String.IsNullOrEmpty(valueTextbox.Text) && !String.IsNullOrEmpty(comboType.Text))
                {
                    requiredComboBorder.Hide();
                    requiredAmountBorder.Show();
                }
                else if (!String.IsNullOrEmpty(valueTextbox.Text) && String.IsNullOrEmpty(comboType.Text))
                {
                    requiredAmountBorder.Hide();
                    requiredComboBorder.Show();
                }
                MessageBox.Show("The required field(s) cannot be empty!", "Error: Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               
            }
            else
            {
                ListViewItem item = new ListViewItem(datePicker.Text); // add item to 1st column
                if (requiredComboBorder.Visible || requiredAmountBorder.Visible)    // change error field border/background to default
                {
                    requiredComboBorder.Hide();
                    requiredAmountBorder.Hide();
                }

                item.SubItems.Add(comboType.Text);      // add type into 2nd column
                item.SubItems.Add(nameTextbox.Text);    // add name into 3rd column
                item.SubItems.Add(valueTextbox.Text);   // add income into 4th column
                income = Double.Parse(valueTextbox.Text);

                // check in case of previous values
                if (balanceShowLabel.Text != "0")
                {
                    total = Double.Parse(balanceShowLabel.Text);
                }
                if (incomeShowLabel.Text != "0")
                {
                    incomeTotal = Double.Parse(incomeShowLabel.Text);
                }

                total += income;            // for calculate
                incomeTotal += income;      // for display
                valueTextbox.Clear();       // clear textbox
                nameTextbox.Clear();       // clear textbox
                comboType.SelectedIndex = -1; // clear combobox
                datePicker.ResetText();     // reset date

                item.SubItems.Add("");                 // ignore expense in 5th column
                item.SubItems.Add(total.ToString());    // add balance into 6th column
                displayListView.Items.Add(item);    // display on listview

                if (total < 0)      // check if total is < 0 then change font color to red, else, green for balance
                {
                    item.SubItems[5].ForeColor = System.Drawing.Color.Red; ;
                    item.UseItemStyleForSubItems = false;
                }
                else if (total > 0)
                {
                    item.SubItems[5].ForeColor = System.Drawing.Color.Green;
                    item.UseItemStyleForSubItems = false;
                }
                else
                {
                    item.SubItems[5].ForeColor = System.Drawing.Color.Black;
                    item.UseItemStyleForSubItems = false;
                }
                incomeShowLabel.Text = incomeTotal.ToString();  // display on result section
                balanceShowLabel.Text = total.ToString();
            }
        }

        private void addExpense_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(comboType.Text) || String.IsNullOrEmpty(valueTextbox.Text))
            {
                if (String.IsNullOrEmpty(comboType.Text) && String.IsNullOrEmpty(valueTextbox.Text))    // change border/background to red
                {
                    requiredAmountBorder.Show();
                    requiredComboBorder.Show();
                }
                else if (String.IsNullOrEmpty(valueTextbox.Text) && !String.IsNullOrEmpty(comboType.Text))
                {
                    requiredComboBorder.Hide();
                    requiredAmountBorder.Show();
                }
                else if (!String.IsNullOrEmpty(valueTextbox.Text) && String.IsNullOrEmpty(comboType.Text))
                {
                    requiredAmountBorder.Hide();
                    requiredComboBorder.Show();
                }
                MessageBox.Show("The required field(s) cannot be empty!", "Error: Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (requiredComboBorder.Visible || requiredAmountBorder.Visible)    // change error field border/background to default
                {
                    requiredComboBorder.Hide();
                    requiredAmountBorder.Hide();
                }

                ListViewItem item = new ListViewItem(datePicker.Text); // add item to 1st column
                item.SubItems.Add(comboType.Text);      // add type into 2nd column
                item.SubItems.Add(nameTextbox.Text);      // add name into 3rd column
                item.SubItems.Add("");                  // ignore income in 4th column
                item.SubItems.Add(valueTextbox.Text);   // add expense into 5th column
                displayListView.Items.Add(item);
                // check in case of previous values
                if (balanceShowLabel.Text != "0")
                {
                    total = Double.Parse(balanceShowLabel.Text);
                }
                if (expenseShowLabel.Text != "0")
                {
                    expenseTotal = Double.Parse(expenseShowLabel.Text);
                }
                expense = Double.Parse(valueTextbox.Text);
                total -= expense;           // for calculate
                expenseTotal += expense;    // for display
                valueTextbox.Clear();       // clear textbox
                nameTextbox.Clear();       // clear textbox
                comboType.SelectedIndex = -1; // clear combobox
                datePicker.ResetText();     // reset date

                item.SubItems.Add(total.ToString());    // add item to 6th column
                if (total < 0)      // check if total is < 0 then change font color to red, else, green
                {
                    item.SubItems[5].ForeColor = System.Drawing.Color.Red; ;
                    item.UseItemStyleForSubItems = false;
                }
                else if (total > 0)
                {
                    item.SubItems[5].ForeColor = System.Drawing.Color.Green;
                    item.UseItemStyleForSubItems = false;
                }
                else
                {
                    item.SubItems[5].ForeColor = System.Drawing.Color.Black;
                    item.UseItemStyleForSubItems = false;
                }
                expenseShowLabel.Text = expenseTotal.ToString();  // display on result section
                balanceShowLabel.Text = total.ToString();


                if (total < 0) // friendly reminder!
                {
                    MessageBox.Show("Warning: Your balance is below 0, please spend money wisely!", "Friendly Reminder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /*private void number_Click(object sender, EventArgs e)
        {
            // receive the clicked number and show it on the display
            System.Windows.Forms.Button number = (System.Windows.Forms.Button)sender;


            // only allow one decimal
            if (number.Text == ".")
            {
                if (valueTextbox.Text.Equals("")) // allow to have 0.x when type only .x
                {
                    valueTextbox.Text = "0" + number.Text;
                }
                if (!valueTextbox.Text.Contains("."))
                {
                    valueTextbox.Text = valueTextbox.Text + number.Text;
                }
            }
            else
            {
                valueTextbox.Text = valueTextbox.Text + number.Text;
            }
        }*/

        // clicking backspace button
        /*private void backspace_Click(object sender, EventArgs e)
        {
            valueAfterBackspace = valueTextbox.Text;

            if (valueAfterBackspace.Length > 1)
            {
                valueAfterBackspace = valueAfterBackspace.Substring(0, valueAfterBackspace.Length - 1);
            }
            else
            {
                valueAfterBackspace = "";
            }

            valueTextbox.Text = valueAfterBackspace;
        }*/

        private void save_Click(object sender, EventArgs e) // Project --> name of project's properties --> Settings
        {
            //_362_group_project.Properties.Settings.Default.label1.
            //Properties.Settings.Default.label1 = incomeShowLabel.Text;
            //Properties.Settings.Default.label2 = expenseShowLabel.Text;
            //Properties.Settings.Default.label3 = balanceShowLabel.Text;

            _362_group_project.Properties.Settings.Default.Save();
            _362_group_project.Properties.Settings.Default.Upgrade();

            /* save data listview in application */
            // code to write to text file using StreamWriter
            StreamWriter writer = new StreamWriter("data.txt", true);
            foreach (ListViewItem item in displayListView.Items)
            {
                writer.WriteLine(item.SubItems[0].Text);
                writer.WriteLine(item.SubItems[1].Text);
                writer.WriteLine(item.SubItems[2].Text);
                
                writer.WriteLine(item.SubItems[3].Text);
                writer.WriteLine(item.SubItems[4].Text);
                writer.WriteLine(item.SubItems[5].Text);
            }
            writer.Close();
            MessageBox.Show("Saved!", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void Form5_Load(object sender, EventArgs e)
        {
            requiredAmountBorder.Hide();    // initialize borders for required field
            requiredComboBorder.Hide();     // initialize borders for required field

            expenseChart.Hide();            // hide chart
            incomeChart.Hide();             // hide chart

            saveExpenseChartButton.Hide();      // hide chart export button
            saveIncomeChartButton.Hide();       // hide chart export button

            comparisonChart.Hide();         // hide chart
            saveChartButton.Hide();         // hide chart export button
            squareDivider.Hide();

            displayListView.View = View.Details;
            displayListView.FullRowSelect = true; // allow to select the full row

            incomeShowLabel.Text = _362_group_project.Properties.Settings.Default.label1;
            expenseShowLabel.Text = _362_group_project.Properties.Settings.Default.label2;
            balanceShowLabel.Text = _362_group_project.Properties.Settings.Default.label3;
            

            /* retrieve data listview from application */
            // code to write to text file in \bin\Debug using StreamWriter

            StreamReader reader = new StreamReader("data.txt");
            // read all the line in text file
            while (!reader.EndOfStream)
            {
                ListViewItem item = new ListViewItem(reader.ReadLine()); // add item to 1st column
                item.SubItems.Add(reader.ReadLine());   // add item into 2nd column
                item.SubItems.Add(reader.ReadLine());   // add item into 3rd column
                item.SubItems.Add(reader.ReadLine());   // add item into 4th column
                item.SubItems.Add(reader.ReadLine());   // add item into 5th column
                item.SubItems.Add(reader.ReadLine());   // add item into 6th column
                if (item.SubItems[5].Text.Contains("-"))      // check if total is < 0 then change font color to red, else, green
                {
                    item.SubItems[5].ForeColor = System.Drawing.Color.Red; ;
                    item.UseItemStyleForSubItems = false;
                }
                else if (!item.SubItems[5].Text.Equals("0"))
                {
                    item.SubItems[5].ForeColor = System.Drawing.Color.Green;
                    item.UseItemStyleForSubItems = false;
                }
                displayListView.Items.Add(item);    // display on listview
            }            
            reader.Close();
        }

        private void reset_Click(object sender, EventArgs e)
        {

            _362_group_project.Properties.Settings.Default.Reset(); // this is for total income/expense/balance
            // need to perform resetting now!
            incomeShowLabel.Text = "0";
            expenseShowLabel.Text = "0";
            balanceShowLabel.Text = "0";
            displayListView.Items.Clear();
            total = 0;
            incomeTotal = 0;
            expenseTotal = 0;
            income = 0;
            expense = 0;
            valueTextbox.Clear();       // clear textbox
            nameTextbox.Clear();       // clear textbox
            comboType.SelectedIndex = -1; // clear combobox
            datePicker.ResetText();     // reset date
            incomeChart.Hide();         // hide chart
            expenseChart.Hide();        // hide chart
            saveExpenseChartButton.Hide();      // hide chart export button
            saveIncomeChartButton.Hide();       // hide chart export button
            comparisonChart.Hide();             // hide chart
            saveChartButton.Hide();             // hide chart export button
            squareDivider.Hide();

            // reset the data.txt file
            File.WriteAllText("data.txt", "");

            MessageBox.Show("All data has been reset!", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void export_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook (*.xlsx)|*.xlsx", ValidateNames = true })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    Workbook wb = app.Workbooks.Add(XlSheetType.xlWorksheet);
                    Worksheet ws = (Worksheet)app.ActiveSheet;
                    app.Visible = false;
                    ws.Cells[1, 1] = "Date";
                    ws.Cells[1, 2] = "Type";
                    ws.Cells[1, 3] = "Name";
                    ws.Cells[1, 4] = "Income";
                    ws.Cells[1, 5] = "Expense";
                    ws.Cells[1, 6] = "Balance";
                    int i = 2;
                    foreach (ListViewItem item in displayListView.Items)
                    {
                        ws.Cells[i, 1] = item.SubItems[0].Text;
                        ws.Cells[i, 2] = item.SubItems[1].Text;
                        ws.Cells[i, 3] = item.SubItems[2].Text;
                        ws.Cells[i, 4] = item.SubItems[3].Text;
                        ws.Cells[i, 5] = item.SubItems[4].Text;
                        ws.Cells[i, 6] = item.SubItems[5].Text;
                        i++;
                    }
                    ws.Columns.AutoFit();
                    wb.SaveAs(sfd.FileName, XlFileFormat.xlExcel7, Type.Missing, Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
                    app.Quit();
                    MessageBox.Show("Your data has been successfully exported!", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // allow the textbox to accept only number, backspace and .
        private void textBoxInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46) // if the char is not number, backspace, .
            {
                e.Handled = true;
            }
            if (valueTextbox.Text.Contains(".") && ch == '.')       // to allow only 1 decimal point
            {
                e.Handled = true;
            } 
        }

        private void clear_Click(object sender, EventArgs e)
        {
            datePicker.ResetText();     // reset date
            valueTextbox.Clear();       // clear textbox
            nameTextbox.Clear();       // clear textbox
            comboType.SelectedIndex = -1; // clear combobox
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete the selected row(s)?", "Warning!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                ListViewItem selectedRow = displayListView.SelectedItems[0];
                if (selectedRow.SubItems[3].Text == "")    // expense row
                {
                    total += Double.Parse(selectedRow.SubItems[4].Text);
                    expenseTotal -= Double.Parse(selectedRow.SubItems[4].Text);

                    expenseShowLabel.Text = expenseTotal.ToString();  // display on result section
                    balanceShowLabel.Text = total.ToString();

                } else if (selectedRow.SubItems[4].Text == "") // income row
                {
                    total -= Double.Parse(selectedRow.SubItems[3].Text);
                    incomeTotal -= Double.Parse(selectedRow.SubItems[3].Text);

                    incomeShowLabel.Text = incomeTotal.ToString();  // display on result section
                    balanceShowLabel.Text = total.ToString();
                }
                displayListView.Items.RemoveAt(displayListView.SelectedIndices[0]);
                
            }
        }

        private void chart_Click(object sender, EventArgs e)
        {
            String[] expenseTypeColumn = new String[displayListView.Items.Count];          // array for expense type
            String[] incomeTypeColumn = new String[displayListView.Items.Count];            // array for income type
            String[] dateColumn = new string[displayListView.Items.Count];                  // array for date
            Double[] expenseCount = new Double[displayListView.Items.Count];       // array for expense count that corresponding with typeColumn array
            Double[] incomeCount = new Double[displayListView.Items.Count];         // array for income count that corresponding with typeColumn array
            Double[] expenseDate = new Double[displayListView.Items.Count];         // array for accumulative expense based on date
            Double[] incomeDate = new Double[displayListView.Items.Count];          // array for accumulative income based on date

            int i = 0;
            int j = 0;

            int pos;

            foreach (ListViewItem item in displayListView.Items)
            {
                if (!dateColumn.Contains(item.SubItems[0].Text))
                {
                    dateColumn[j] = item.SubItems[0].Text;      // collect date and put into the dateColumn array
                    j++;
                }

                // sort income/expense by date
                pos = Array.IndexOf(dateColumn, item.SubItems[0].Text);
                if (pos > -1)
                {
                    if (item.SubItems[3].Text != "")            // sort income
                    {
                        incomeDate[pos] += Double.Parse(item.SubItems[3].Text);
                    }
                    else if (item.SubItems[4].Text != "")       // sort expense
                    {
                        expenseDate[pos] += Double.Parse(item.SubItems[4].Text);
                    }
                }
                


                if (item.SubItems[4].Text != "")                            // sort only expense
                {
                    if (!expenseTypeColumn.Contains(item.SubItems[1].Text))        // if there is none of that type in typeColumn
                    {
                        expenseTypeColumn[i] = item.SubItems[1].Text;              // add into the typeColumn
                        expenseCount[i] += Double.Parse(item.SubItems[4].Text);
                    }
                    else                                                   // there is that type in typeColumn
                    {
                        pos = Array.IndexOf(expenseTypeColumn, item.SubItems[1].Text);
                        if (pos > -1)
                        {
                            expenseCount[pos] += Double.Parse(item.SubItems[4].Text);         // count ++ at that position
                        }
                    }
                    i++;                                    // go to next row
                }

                else if (item.SubItems[3].Text != "")                 // sort only income
                {
                    if (!incomeTypeColumn.Contains(item.SubItems[1].Text))        // if there is none of that type in typeColumn
                    {
                        incomeTypeColumn[i] = item.SubItems[1].Text;              // add into the typeColumn
                        incomeCount[i] += Double.Parse(item.SubItems[3].Text);
                    }
                    else                                                   // there is that type in typeColumn
                    {
                        pos = Array.IndexOf(incomeTypeColumn, item.SubItems[1].Text);
                        if (pos > -1)
                        {
                            incomeCount[pos] += Double.Parse(item.SubItems[3].Text);         // count ++ at that position
                        }
                    }
                    i++;
                }

            }

            // before construct, make sure to have empty chart
            // ref: https://stackoverflow.com/questions/34310065/how-to-clear-plotted-values-in-points-chart
            foreach (var series in expenseChart.Series)
            {
                series.Points.Clear();
            }

            foreach (var series in incomeChart.Series)
            {
                series.Points.Clear();
            }

            foreach (var series in comparisonChart.Series)
            {
                series.Points.Clear();
            }

            // clear title of the chart
            expenseChart.Titles.Clear();
            incomeChart.Titles.Clear();
            comparisonChart.Titles.Clear();

            if (comparisonChart.Series.IsUniqueName("Expense"))
            {
                comparisonChart.Series.Add("Expense");         // create 2nd series (expense)
            }
            
            
            


            // construct the expense chart --> ignore the index that has null value
            i = 0;
            // pieChart.Series[0].Points.DataBindXY(typeColumn, countColumn);
            while (i < expenseTypeColumn.Length)
            {
                if (expenseTypeColumn[i] != null)          // ignore income --> display chart only for expense
                {

                    expenseChart.Series[0].Points.AddXY(expenseTypeColumn[i], expenseCount[i]);
                }
                i++;
            }

            // construct the income chart --> ignore the index that has null value
            i = 0;
            while (i < incomeTypeColumn.Length)
            {
                if (incomeTypeColumn[i] != null)          // ignore expense --> display chart only for expense
                {

                    incomeChart.Series[0].Points.AddXY(incomeTypeColumn[i], incomeCount[i]);
                }
                i++;
            }

            // construct the comparison chart
            i = 0;
            while (i < j) 
            {
                comparisonChart.Series[0].Points.AddXY(dateColumn[i], incomeDate[i]);  // add date into the x axis + income
                comparisonChart.Series[1].Points.AddXY(dateColumn[i], expenseDate[i]);  // add date into the x axis + expense
                i++;
            }

            // ref: https://msdn.microsoft.com/en-us/library/dd456687.aspx

            expenseChart.Titles.Add("Expense Chart");
            expenseChart.Series[0].Label = "#PERCENT{0.00 %}";
            expenseChart.Series[0].LegendText = "#AXISLABEL";
            expenseChart.ChartAreas[0].BackColor = Color.Transparent;
            expenseChart.Legends[0].BackColor = Color.Transparent;

            incomeChart.Titles.Add("Income Chart");
            incomeChart.Series[0].Label = "#PERCENT{0.00 %}";
            incomeChart.Series[0].LegendText = "#AXISLABEL";
            incomeChart.ChartAreas[0].BackColor = Color.Transparent;
            incomeChart.Legends[0].BackColor = Color.Transparent;


            comparisonChart.Titles.Add("Comparison Chart");
            comparisonChart.Series[0].LegendText = "Income";
            comparisonChart.ChartAreas[0].BackColor = Color.Transparent;
            comparisonChart.Legends[0].BackColor = Color.Transparent;
            comparisonChart.Series[0].Label = "#VAL";
            comparisonChart.Series[1].Label = "#VAL";


            // format the font style for chart

            incomeChart.Series[0].Font = new System.Drawing.Font("Arial", 9F);
            expenseChart.Series[0].Font = new System.Drawing.Font("Arial", 9F);
            comparisonChart.Series[0].Font = new System.Drawing.Font("Arial", 9F);

            incomeChart.Titles[0].Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            expenseChart.Titles[0].Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            comparisonChart.Titles[0].Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);

            incomeChart.Legends[0].Font = new System.Drawing.Font("Arial", 9F);
            expenseChart.Legends[0].Font = new System.Drawing.Font("Arial", 9F);
            comparisonChart.Legends[0].Font = new System.Drawing.Font("Arial", 9F);

            expenseChart.Show();
            incomeChart.Show();
            comparisonChart.Show();
            

            if (expenseChart.Series[0].Points.Count == 0 && incomeChart.Series[0].Points.Count != 0)
            {                
                saveIncomeChartButton.Show();       // show chart export button
                saveExpenseChartButton.Hide();
                saveChartButton.Show();
            }
            else if (incomeChart.Series[0].Points.Count == 0 && expenseChart.Series[0].Points.Count != 0)
            {
                saveExpenseChartButton.Show();      // show chart export button   
                saveIncomeChartButton.Hide();
                saveChartButton.Show();
            }
            else if (incomeChart.Series[0].Points.Count != 0 && expenseChart.Series[0].Points.Count != 0)
            {
                saveExpenseChartButton.Show();      // show chart export button
                saveIncomeChartButton.Show();       // show chart export button
                saveChartButton.Show();
            }
            else
            {
                saveExpenseChartButton.Hide();      // show chart export button
                saveIncomeChartButton.Hide();       // show chart export button
                saveChartButton.Show();
            }
            squareDivider.Show();
        }


        private void exportExpenseChart_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*JPEG files (*.jpeg)|*.jpeg";
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                expenseChart.SaveImage(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                MessageBox.Show("Image saved successfully!", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void exportIncomeChart_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*JPEG files (*.jpeg)|*.jpeg";
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                incomeChart.SaveImage(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                MessageBox.Show("Image saved successfully!", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void exportComparisonChart_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*JPEG files (*.jpeg)|*.jpeg";
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                comparisonChart.SaveImage(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                MessageBox.Show("Image saved successfully!", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
