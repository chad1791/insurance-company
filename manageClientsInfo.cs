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
    public partial class manageClientsInfo : Form
    {

        dbManager dbHandler;
        int clientId;

        public manageClientsInfo()
        {
            InitializeComponent();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //collect clients data to send to the database as an edit.

            string ss = textBox5.Text;
            string first = textBox3.Text;
            string middle = textBox1.Text;
            string last = textBox2.Text;
            string gender = comboBox1.Text;
            string dob = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string phone = textBox4.Text;
            string email = textBox7.Text;
            string address = richTextBox1.Text;
            string workplace = textBox6.Text;

            int queryStatus = dbHandler.editClientInfo(clientId, ss, first, middle, last, gender, dob, phone, email, address, workplace);

            if (queryStatus > 0)
            {
                clientId = queryStatus;

                MessageBox.Show("Client information was successfully edited!");

                //clear textboxes

                //textBox1.Text = "";
                //textBox2.Text = "";
                //textBox3.Text = "";
                //textBox4.Text = "";
                //textBox5.Text = "";
                //textBox7.Text = "";
                //textBox6.Text = "";
                //richTextBox1.Text = "";
            }
            else
            {
                MessageBox.Show("Oh no, Client could not be added to the system!");
            }
        }

        private void EditClientInfo_Load(object sender, EventArgs e)
        {
            clientId = (int)this.Tag;
            dbHandler = new dbManager("localhost", "root", "", "insuranceCo", "3306");

            //get current client info using client id.

            DataSet singleClient = new DataSet();
            singleClient = dbHandler.getClientInfoById(clientId);

            if(singleClient.Tables[0].Rows.Count > 0)
            {
                int dbClientId   = Convert.ToInt32(singleClient.Tables[0].Rows[0].ItemArray.GetValue(0).ToString());
                string social    = singleClient.Tables[0].Rows[0].ItemArray.GetValue(1).ToString();
                string first     = singleClient.Tables[0].Rows[0].ItemArray.GetValue(2).ToString();
                string middle    = singleClient.Tables[0].Rows[0].ItemArray.GetValue(3).ToString();
                string last      = singleClient.Tables[0].Rows[0].ItemArray.GetValue(4).ToString();
                string gender    = singleClient.Tables[0].Rows[0].ItemArray.GetValue(5).ToString();
                string dob       = singleClient.Tables[0].Rows[0].ItemArray.GetValue(6).ToString();
                string phone     = singleClient.Tables[0].Rows[0].ItemArray.GetValue(7).ToString();
                string email     = singleClient.Tables[0].Rows[0].ItemArray.GetValue(8).ToString();
                string address   = singleClient.Tables[0].Rows[0].ItemArray.GetValue(9).ToString();
                string workplace = singleClient.Tables[0].Rows[0].ItemArray.GetValue(10).ToString();

                var culture = new CultureInfo("en-us");
                dob = DateTime.Parse(dob, culture).ToString("MM/dd/yyyy");

                textBox3.Text = first;
                textBox1.Text = middle;
                textBox2.Text = last;
                textBox4.Text = phone;
                comboBox1.SelectedItem = gender;
                dateTimePicker1.Text = dob;
                richTextBox1.Text = address;
                textBox7.Text = email;
                textBox6.Text = workplace;
                textBox5.Text = social;
            }

        }
    }
}
