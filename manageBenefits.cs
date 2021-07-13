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
    public partial class manageBenefits : Form
    {
        dbManager dbHandler;

        public manageBenefits()
        {
            InitializeComponent();
        }

        private void Benefits_Load(object sender, EventArgs e)
        {
            dbHandler = new dbManager("localhost", "root", "", "insuranceCo", "3306");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox3.Text;
            string description = richTextBox1.Text;

            int queryStatus = dbHandler.addBenefit(name, description);

            if (Convert.ToBoolean(queryStatus))
            {
                MessageBox.Show("Benefit successfully added to the system!");
                textBox3.Text = "";
                richTextBox1.Text = "";
            }
            else
            {
                MessageBox.Show("Oh no, Client could not be added to the system!");
            }
        }
    }
}
