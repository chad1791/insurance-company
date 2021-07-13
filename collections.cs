using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InsuranceCo
{
    public partial class collections : Form
    {
        dbManager dbHandler;
        private BindingList<Insurance> insList = new BindingList<Insurance>();

        public collections()
        {
            InitializeComponent();
        }

        private void PayInsurance_Load(object sender, EventArgs e)
        {
            dbHandler = new dbManager("localhost", "root", "", "insuranceCo", "3306", "Convert Zero Datetime=True");

            DataSet myClients = new DataSet();
            myClients = dbHandler.getAllClients();

            myClients.Tables[0].Columns.Add("FullName", typeof(string), "first + ' ' + middle + ' ' + last");

            comboBox1.DataSource = myClients.Tables[0];
            comboBox1.DisplayMember = "FullName";
            comboBox1.ValueMember = "id";

            comboBox1.BindingContext = this.BindingContext;

            if (comboBox1.Text != "")
            {
                string today = DateTime.Now.ToUniversalTime().AddMinutes(DateTime.Now.Subtract(DateTime.Now.ToUniversalTime()).TotalMinutes).ToString("yyyy-MM-dd");

                DataSet myInsuranceTypes = new DataSet();
                myInsuranceTypes = dbHandler.getInsTypesByClientId((int)comboBox1.SelectedValue, today);

                if (myInsuranceTypes.Tables[0].Rows.Count > 0)
                {
                    int insId = int.Parse(myInsuranceTypes.Tables[0].Rows[0].ItemArray.GetValue(2).ToString());
                    int counter = 0;

                    while (counter < myInsuranceTypes.Tables[0].Rows.Count)
                    {
                        DataSet insInfo = dbHandler.getInsInfoById(insId);

                        if (insInfo.Tables[0].Rows.Count > 0)
                        {
                            string insName = insInfo.Tables[0].Rows[0].ItemArray.GetValue(1).ToString();
                            //comboSource.Add(insId.ToString(), insName);

                            insList.Add(new Insurance(insName, insId));
                        }

                        counter++;
                    }

                }

                comboBox2.DataSource = insList;
                comboBox2.DisplayMember = "Name";
                comboBox2.ValueMember = "Id";
            }



        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void populateClientsInsurance()
        {
            //get today's date in yyyy-MM-dd format


        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            

        }

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            comboBox2.DataBindings.Clear();
            comboBox2.DataSource = null;
            comboBox2.Items.Clear();
            insList.Clear();

            if (comboBox1.Text != "")
            {
                string today = DateTime.Now.ToUniversalTime().AddMinutes(DateTime.Now.Subtract(DateTime.Now.ToUniversalTime()).TotalMinutes).ToString("yyyy-MM-dd");

                DataSet myInsuranceTypes = new DataSet();
                myInsuranceTypes = dbHandler.getInsTypesByClientId((int)comboBox1.SelectedValue, today);

                if (myInsuranceTypes.Tables[0].Rows.Count > 0)
                {
                    int insId = int.Parse(myInsuranceTypes.Tables[0].Rows[0].ItemArray.GetValue(2).ToString());
                    int counter = 0;

                    while (counter < myInsuranceTypes.Tables[0].Rows.Count)
                    {
                        DataSet insInfo = dbHandler.getInsInfoById(insId);

                        if (insInfo.Tables[0].Rows.Count > 0)
                        {
                            string insName = insInfo.Tables[0].Rows[0].ItemArray.GetValue(1).ToString();
                            //comboSource.Add(insId.ToString(), insName);

                            insList.Add(new Insurance(insName, insId));
                        }

                        counter++;
                    }

                }

                comboBox2.DataSource = insList;
                comboBox2.DisplayMember = "Name";
                comboBox2.ValueMember = "Id";
            }
        }
    }

    public class Insurance
    {
        public Insurance(string name, int id)
        {
            this.Name = name; this.Id = id;
        }
        public string Name
        { get; set; }
        public int Id
        { get; set; }
    }
}
