/*
 * Created by SharpDevelop.
 * User: a
 * Date: 08/04/2020
 * Time: 11:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace InsuranceCo
{
	/// <summary>
	/// Description of Registration.
	/// </summary>
	public partial class addInsurance : Form
	{
        dbManager dbHandler;
        int clientId;

        public addInsurance()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void GroupBox1Enter(object sender, EventArgs e)
		{
			
		}
		
		void RegistrationLoad(object sender, EventArgs e)
		{
            clientId = (int)this.Tag;

            //establish connection with the database....

            dbHandler = new dbManager("localhost", "root", "", "insuranceCo", "3306");

            //populate the insurance combobox with the data from the insurance table...

            DataSet myInsuranceTypes = new DataSet();
            myInsuranceTypes = dbHandler.getInsTypes();

            comboBox1.DataSource = myInsuranceTypes.Tables[0];
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";

            comboBox1.BindingContext = this.BindingContext;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //collect insurance data for the client

            int insTypeId = int.Parse(comboBox1.SelectedValue.ToString());  ///get the id of the selected value...
            //string businessAddress = textBox1.Text;
            string paymentMethod = comboBox2.Text;
            string packagePayment = comboBox3.Text;
            string expirty = dateTimePicker1.Value.ToString("yyyy-MM-dd");

            int queryStatus = dbHandler.addClientInsurance(clientId, insTypeId, paymentMethod, packagePayment, expirty);

            if (queryStatus > 0)
            {
                MessageBox.Show("Client Insurance was successfully added!");

                //clear textboxes

                //textBox1.Text = "";
            }
            else
            {
                MessageBox.Show("Oh no, Client Insurance could not be added to the system!");
            }
        }
    }
}
