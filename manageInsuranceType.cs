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
    public partial class manageInsuranceType : Form
    {
        dbManager dbHandler;
        List<int> benefits = new List<int>();
        int insuranceId;

        public manageInsuranceType()
        {
            InitializeComponent();
        }

        private void InsuranceType_Load(object sender, EventArgs e)
        {
            //establish connection with the database....

            dbHandler = new dbManager("localhost", "root", "", "insuranceCo", "3306");

            // get benefits for insurance.. if any in the benefits table...

            DataSet InsuranceBenefits = new DataSet();
            
            InsuranceBenefits = dbHandler.getInsBenefits();

            //while loop to draw insurance benefits on the panel... (checkboxes)

            if(InsuranceBenefits.Tables[0].Rows.Count > 0)
            {
                int count = 0;

                int x = 10;
                int y = 10;

                while(InsuranceBenefits.Tables[0].Rows.Count > count)
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
            }

            //check if you're in edit mode...

            if(this.Tag != null)
            {

                label1.Text = "Edit Insurance Record";
                this.Text = "Edit Insurance";
                button1.Text = "Update";

                insuranceId = (int)this.Tag;

                //get insurance from the database...

                DataSet insurance = new DataSet();
                insurance = dbHandler.getInsInfoById(insuranceId);

                string name = insurance.Tables[0].Rows[0].ItemArray.GetValue(1).ToString();
                string desc = insurance.Tables[0].Rows[0].ItemArray.GetValue(2).ToString();

                textBox3.Text = name;
                richTextBox1.Text = desc;

                //loop through ins benefits and check the benefits for that insurance...

                List<int> insDbBenefits = new List<int>();

                DataSet insBenefits = new DataSet();
                insBenefits = dbHandler.getBenefitsByInsId(insuranceId);

                foreach(var row in insBenefits.Tables[0].Rows)
                {
                    DataRow myRow = (DataRow)row;
                    string benId = myRow.ItemArray.GetValue(2).ToString();
                    insDbBenefits.Add(int.Parse(benId));

                    ///if(this.panel1.Controls.)
                    foreach (var control in this.panel1.Controls)
                    {
                        CheckBox myCheckBox = (CheckBox)control;
                        if(myCheckBox.Tag.ToString() == benId)
                        {
                            myCheckBox.Checked = true;
                        }
                    }
                }
            }
  
        }

        private void DynChBox_CheckStateChanged(object sender, EventArgs e)
        { 
            CheckBox myCheckBox = (CheckBox)sender;

            int tag = (int)myCheckBox.Tag;

            if (myCheckBox.CheckState == CheckState.Checked) {
                benefits.Add(tag);
            }
            else {
                benefits.Remove(tag);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ////add or update insurance record

            string name = textBox3.Text;
            string description = richTextBox1.Text;

            if (button1.Text == "Add")
            {
                if (name != "" && benefits.Count > 0)
                {
                    int queryStatus = dbHandler.addInsType(name, description, benefits);

                    if (Convert.ToBoolean(queryStatus))
                    {
                        MessageBox.Show("Insurance was successfully added!");

                        //clear all the fields... 
                        textBox3.Text = "";
                        richTextBox1.Text = "";

                        foreach (var item in panel1.Controls)
                        {
                            CheckBox myCheckBox = (CheckBox)item;
                            if (myCheckBox.CheckState == CheckState.Checked)
                                myCheckBox.Checked = false;
                        }

                        if (benefits.Count > 0)
                        {
                            benefits.Clear();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error! Insurance record could not be added! Contact your developer");
                    }
                }
                else
                {
                    MessageBox.Show("Please fill in all fields!");
                }
            }
            else 
            if(button1.Text == "Update")
            {
                // delete all benefits for selected insurance...

                if (name != "" && benefits.Count > 0)
                {
                    bool queryStatus = dbHandler.updateIns(name, description, insuranceId, benefits);

                    if (queryStatus)
                    {
                        MessageBox.Show("Insurance was successfully updated!");
                    }
                    else
                    {
                        MessageBox.Show("Error! Insurance record could not be updated! Contact your developer");
                    }
                }
                else
                {
                    MessageBox.Show("Please fill in all fields!");
                }

            }

        }
    }
}
