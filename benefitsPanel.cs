using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InsuranceCo
{
    public partial class benefitsPanel : Form
    {
        dbManager dbHandler;
        List<int> benefits = new List<int>();

        public benefitsPanel()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            manageBenefits addBenefit = new manageBenefits();
            addBenefit.ShowDialog();
        }

        private void benefitsPanel_Load(object sender, EventArgs e)
        {
            dbHandler = new dbManager("localhost", "root", "", "insuranceCo", "3306");

            // get benefits for insurance.. if any in the benefits table...

            DataSet InsuranceBenefits = new DataSet();

            InsuranceBenefits = dbHandler.getInsBenefits();

            //while loop to draw insurance benefits on the panel... (checkboxes)

            if (InsuranceBenefits.Tables[0].Rows.Count > 0)
            {
                int count = 0;

                int x = 10;
                int y = 10;

                while (InsuranceBenefits.Tables[0].Rows.Count > count)
                {
                    string name = InsuranceBenefits.Tables[0].Rows[count].ItemArray.GetValue(1).ToString();
                    int tag = int.Parse(InsuranceBenefits.Tables[0].Rows[count].ItemArray.GetValue(0).ToString());

                    CheckBox dynChBox = new CheckBox();
                    dynChBox.Text = name;
                    dynChBox.Tag = tag;
                    dynChBox.Size = new Size(250, 30);
                    dynChBox.Location = new Point(x, y);

                    dynChBox.CheckStateChanged += DynChBox_CheckStateChanged;

                    panel1.Controls.Add(dynChBox);

                    count++;
                    y += 25;
                }

                foreach (var control in this.panel1.Controls)
                {
                    CheckBox myCheckBox = (CheckBox)control;
                    myCheckBox.Checked = true;
                    
                }
            }
        }

        private void DynChBox_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox myCheckBox = (CheckBox)sender;
            int benefitId = (int)myCheckBox.Tag;

            if (myCheckBox.CheckState != CheckState.Checked)
            {
                DialogResult result = MessageBox.Show("Do you want to deleted this benefit entry from the database?", "Confirmation", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    bool deleted = dbHandler.delBenefit(benefitId);

                    if (deleted)
                    {
                        MessageBox.Show("Benefit record was successfully deleted... You might have to refresh the window to view changes!");
                        this.Controls.Remove(myCheckBox);
                    }
                }
            }
            
        }
    }
}
