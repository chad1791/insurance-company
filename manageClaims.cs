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
    public partial class manageClaims : Form
    {
        dbManager dbHandler;
        

        public manageClaims()
        {
            InitializeComponent();
        }

        private void FileInsClaim_Load(object sender, EventArgs e)
        {
            dbHandler = new dbManager("localhost", "root", "", "insuranceCo", "3306");

            DataSet myClients = new DataSet();
            myClients = dbHandler.getAllClients();

            myClients.Tables[0].Columns.Add("FullName", typeof(string), "first + ' ' + middle + ' ' + last");

            comboBox1.DataSource = myClients.Tables[0];
            comboBox1.DisplayMember = "FullName";
            comboBox1.ValueMember = "id";

            comboBox1.BindingContext = this.BindingContext;


            DataSet myInsuranceTypes = new DataSet();
            myInsuranceTypes = dbHandler.getInsTypes();

            comboBox2.DataSource = myInsuranceTypes.Tables[0];
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "id";

            comboBox2.BindingContext = this.BindingContext;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int clientId = int.Parse(comboBox1.SelectedValue.ToString());
            int insuranceId = int.Parse(comboBox2.SelectedValue.ToString());
            string description = richTextBox1.Text;

            int claimStatus = dbHandler.addInsClaim(clientId, insuranceId, description);

            if (Convert.ToBoolean(claimStatus))
            {
                MessageBox.Show("Client claim was successfully filed!");
                richTextBox1.Text = "";
            }
            else
            {
                MessageBox.Show("There was an error filing client's claim! Please contact your developer!");
            }
        }
    }
}
