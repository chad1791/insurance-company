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
    public partial class insuranceTypes : Form
    {
        dbManager dbHandler;
        DataSet insurances;
        int insuranceId;

        public insuranceTypes()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            manageInsuranceType typesOfIns = new manageInsuranceType();
            typesOfIns.ShowDialog();
        }

        public void populateGridWithInsurances()
        {
            ///clear previous dataset
            insurances.Tables.Clear();

            ///clear previous rows from datagridview...
            dataGridView1.Rows.Clear();

            insurances = dbHandler.getAllInsurances();

            ///rows for the datagridview...

            if (insurances.Tables[0].Rows.Count > 0)
            {
                int counter = 0;

                while (counter < insurances.Tables[0].Rows.Count)
                {
                    dataGridView1.Rows.Add();

                    int tag = int.Parse(insurances.Tables[0].Rows[counter].ItemArray.GetValue(0).ToString());

                    var DGVComboBox = new DataGridViewComboBoxCell();
                    DGVComboBox.Tag = tag;
                    DGVComboBox.DataSource = new List<string> { "Options", "Edit", "Delete" };
                    DGVComboBox.Value = "Options";

                    dataGridView1.Rows[counter].Cells[0].Value = insurances.Tables[0].Rows[counter].ItemArray.GetValue(0).ToString();
                    dataGridView1.Rows[counter].Cells[1].Value = insurances.Tables[0].Rows[counter].ItemArray.GetValue(1).ToString();
                    dataGridView1.Rows[counter].Cells[2].Value = insurances.Tables[0].Rows[counter].ItemArray.GetValue(2).ToString();
                    dataGridView1.Rows[counter].Cells[3].Value = insurances.Tables[0].Rows[counter].ItemArray.GetValue(3).ToString();
                    dataGridView1.Rows[counter].Cells[4] = DGVComboBox;
                    dataGridView1.Rows[counter].Cells[4].Tag = tag;

                    counter++;
                }
            }
        }

        private void insuranceTypes_Load(object sender, EventArgs e)
        {
            dbHandler = new dbManager("localhost", "root", "", "insuranceCo", "3306");

            //load clients data into the datagridview...

            insurances = new DataSet();

            //DataGridViewColumnCollection cellCollection = new DataGridViewColumnCollection();
            dataGridView1.AutoResizeColumns();

            dataGridView1.Columns.Add("ID", "ID");
            dataGridView1.Columns.Add("Name", "Name");
            dataGridView1.Columns.Add("Description", "Description");
            dataGridView1.Columns.Add("Status", "Status");
            dataGridView1.Columns.Add("Options", "Options");

            populateGridWithInsurances();
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 4 && e.Control is ComboBox)
            {
                ComboBox comboBox = e.Control as ComboBox;

                var currentcell = dataGridView1.CurrentCellAddress;
                comboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
                comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
                insuranceId = (int)dataGridView1.Rows[currentcell.Y].Cells[4].Tag;

                string value = (string)dataGridView1.Rows[currentcell.Y].Cells[4].Value;

            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sendingCB = sender as DataGridViewComboBoxEditingControl;
            string selected = sendingCB.EditingControlFormattedValue.ToString();

            if (selected != "Options")
            {
                switch (selected)
                {
                    case "Edit":
                        {
                            manageInsuranceType editIns = new manageInsuranceType();
                            editIns.Tag = insuranceId;

                            editIns.FormClosed -= EditIns_FormClosed;
                            editIns.FormClosed += EditIns_FormClosed;

                            editIns.ShowDialog();

                        }
                        break;
                    case "Delete":
                        {
                            DialogResult result = MessageBox.Show("Do you want to deleted this Insurance type from the database?", "Confirmation", MessageBoxButtons.YesNoCancel);
                            if (result == DialogResult.Yes)
                            {
                                bool deleted = dbHandler.delInsurance(insuranceId);

                                if (deleted)
                                {
                                    MessageBox.Show("Insurance successfully deleted... You might have to refresh window to view changes!");
                                    populateGridWithInsurances();
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void EditIns_FormClosed(object sender, FormClosedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
