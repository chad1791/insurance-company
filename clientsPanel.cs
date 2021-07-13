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
    public partial class clientsPanel : Form
    {
        dbManager dbHandler;
        DataSet clients;
        int clientId;

        public clientsPanel()
        {
            InitializeComponent();
        }

        public void populateGridWithClients()
        {
            ///clear previous dataset
            clients.Tables.Clear();

            ///clear previous rows from datagridview...
            dataGridView1.Rows.Clear();

            clients = dbHandler.getAllClients();

            ///rows for the datagridview...
            
            if (clients.Tables[0].Rows.Count > 0)
            {
                int counter = 0;

                while (counter < clients.Tables[0].Rows.Count)
                {
                    dataGridView1.Rows.Add();

                    int tag = int.Parse(clients.Tables[0].Rows[counter].ItemArray.GetValue(0).ToString());

                    var DGVComboBox = new DataGridViewComboBoxCell();
                    DGVComboBox.Tag = tag;
                    DGVComboBox.DataSource = new List<string> { "Options", "Edit", "Add Insurance", "Delete" };
                    DGVComboBox.Value = "Options";

                    dataGridView1.Rows[counter].Cells[0].Value = clients.Tables[0].Rows[counter].ItemArray.GetValue(1).ToString();
                    dataGridView1.Rows[counter].Cells[1].Value = clients.Tables[0].Rows[counter].ItemArray.GetValue(2).ToString();
                    dataGridView1.Rows[counter].Cells[2].Value = clients.Tables[0].Rows[counter].ItemArray.GetValue(4).ToString();
                    dataGridView1.Rows[counter].Cells[3].Value = clients.Tables[0].Rows[counter].ItemArray.GetValue(5).ToString();
                    dataGridView1.Rows[counter].Cells[4].Value = clients.Tables[0].Rows[counter].ItemArray.GetValue(6).ToString();
                    dataGridView1.Rows[counter].Cells[5].Value = clients.Tables[0].Rows[counter].ItemArray.GetValue(7).ToString();
                    dataGridView1.Rows[counter].Cells[6].Value = clients.Tables[0].Rows[counter].ItemArray.GetValue(8).ToString();
                    dataGridView1.Rows[counter].Cells[7].Value = clients.Tables[0].Rows[counter].ItemArray.GetValue(9).ToString();
                    dataGridView1.Rows[counter].Cells[8] = DGVComboBox;
                    dataGridView1.Rows[counter].Cells[8].Tag = tag;

                    counter++;
                }
            }
        }

        private void ManageClients_Load(object sender, EventArgs e)
        {
            dbHandler = new dbManager("localhost", "root", "", "insuranceCo", "3306");

            //load clients data into the datagridview...

            clients = new DataSet();
            
            //DataGridViewColumnCollection cellCollection = new DataGridViewColumnCollection();

            dataGridView1.Columns.Add("S.S", "S.S");
            dataGridView1.Columns.Add("Firstname", "Firstname");
            dataGridView1.Columns.Add("Lastname", "Lastname");
            dataGridView1.Columns.Add("Gender", "Gender");
            dataGridView1.Columns.Add("D.O.B", "D.O.B");
            dataGridView1.Columns.Add("Phone", "Phone");
            dataGridView1.Columns.Add("Email", "Email");
            dataGridView1.Columns.Add("Address", "Address");
            dataGridView1.Columns.Add("Options", "Options");

            populateGridWithClients();

        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 8 && e.Control is ComboBox)
            {
                ComboBox comboBox = e.Control as ComboBox;

                var currentcell = dataGridView1.CurrentCellAddress;
                comboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
                comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
                clientId = (int)dataGridView1.Rows[currentcell.Y].Cells[8].Tag;

                string value = (string)dataGridView1.Rows[currentcell.Y].Cells[8].Value;
                 
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sendingCB = sender as DataGridViewComboBoxEditingControl;
            string selected = sendingCB.EditingControlFormattedValue.ToString();

            if(selected != "Options")
            {
                switch (selected)
                {
                    case "Edit":
                        {
                            manageClientsInfo editClient = new manageClientsInfo();
                            editClient.Tag = clientId;

                            editClient.FormClosed -= EditClient_FormClosed;
                            editClient.FormClosed += EditClient_FormClosed;

                            editClient.ShowDialog();

                        }
                        break;
                    case "Add Insurance":
                        {
                           // MessageBox.Show(selected + ", " + clientId.ToString());
                            addInsurance addInsToClient = new addInsurance();
                            addInsToClient.Tag = clientId;
                            addInsToClient.ShowDialog();

                            //addInsToClient
                        }
                        break;
                    case "Delete":
                        {
                            DialogResult result = MessageBox.Show("Do you want to deleted this client from the database?", "Confirmation", MessageBoxButtons.YesNoCancel);
                            if (result == DialogResult.Yes)
                            {
                                bool deleted = dbHandler.delClient(clientId);

                                if (deleted)
                                {
                                    MessageBox.Show("Client successfully deleted... You might have to refresh window to view changes!");
                                    populateGridWithClients();
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void EditClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            populateGridWithClients();
        }
    }
}
