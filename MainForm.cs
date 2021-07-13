/*
 * Created by SharpDevelop.
 * User: a
 * Date: 07/04/2020
 * Time: 19:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace InsuranceCo
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
        dbManager dbHandler;
        int clientId;

        PaperSize paperSize = new PaperSize("papersize", 595, 842);//set the paper size
        int totalnumber = 0;//this is for total number of items of the list or array
        int itemperpage = 0;//this is for no of item per page

        public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void PictureBox1Click(object sender, EventArgs e)
		{
			
		}

        void addClient()
        {
            //collect clients data.... validate for empty values sent to the database....

            string ss = textBox5.Text;
            string first = textBox1.Text;
            string middle = textBox2.Text;
            string last = textBox7.Text;
            string gender = comboBox1.Text;
            string dob = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string phone = textBox8.Text;
            string email = textBox3.Text;
            string address = richTextBox1.Text;
            string workplace = textBox4.Text;

            int queryStatus = dbHandler.addClient(ss, first, middle, last, gender, dob, phone, email, address, workplace);

            if (queryStatus > 0)
            {
                clientId = queryStatus;

                MessageBox.Show("Client successfully added to the system!");

                //clear textboxes

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                richTextBox1.Text = "";
            }
            else
            {
                MessageBox.Show("Oh no, Client could not be added to the system!");
            }
        }
		
		void Button1Click(object sender, EventArgs e)
		{
            
		}

        private void clientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //collections payIns = new collections();
            //payIns.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //manageBenefits insBenefits = new manageBenefits();
            //insBenefits.ShowDialog();

            ///test database connection...
            ///

            dbHandler = new dbManager("localhost", "root", "", "insuranceCo", "3306");
            bool connectionStatus = dbHandler.testConnection();

            if (connectionStatus)
            {
                label13.Text = "Connection to database was successful!";
            }
            else
            {
                label13.Text = "Error connecting to the database... :'v";
            }
        }

        private void manageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            insuranceTypes insurance = new insuranceTypes();
            insurance.ShowDialog();
        }

        private void manageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clientsPanel clientsWindow = new clientsPanel();
            clientsWindow.ShowDialog();
        }

        private void printPreviewDialog1_Click(object sender, EventArgs e)
        {
            
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            //get database info for clients here...
            DataSet clientsList = new DataSet();
            clientsList = dbHandler.getAllClients();

            DateTime now = DateTime.Now;
            //Console.WriteLine(now.ToString("F"));

            if (clientsList.Tables[0].Rows.Count > 0)
            {
                int totalCleints = clientsList.Tables[0].Rows.Count;

                Font headerFont = new Font("Arial", 16, FontStyle.Bold);
                e.Graphics.DrawString("Insurance Co. Ltd", headerFont, Brushes.Black, 20, 20);
                Font headerDateFont = new Font("Arial", 10, FontStyle.Bold);
                e.Graphics.DrawString(now.ToString("F"), headerDateFont, Brushes.Black, 330, 26);

                //line breaker for report header.
                Pen blackPen = new Pen(Color.Black, 3);

                PointF point1 = new PointF(20.0F, 45.0F);
                PointF point2 = new PointF(570.0F, 45.0F);

                e.Graphics.DrawLine(blackPen, point1, point2);

                //line under the table headers.
                Pen blackPen2 = new Pen(Color.Black, 1);

                PointF point3 = new PointF(20.0F, 95.0F);
                PointF point4 = new PointF(570.0F, 95.0F);

                e.Graphics.DrawLine(blackPen2, point3, point4);

                //define font size and style...

                Font normalFont = new Font("Arial", 7);
                Font addressFont = new Font("Arial", 7);
                Font titleFont = new Font("Arial", 12, FontStyle.Bold);
                Font boldFont = new Font("Arial", 8, FontStyle.Bold);

                float currentY = 55;// declare  one variable for height measurement
                e.Graphics.DrawString("Clients List", titleFont, Brushes.Black, 20, currentY);//this will print one heading/title in every page of the document
                currentY += 25;

                // draw the table headers here....
                e.Graphics.DrawString("ID", boldFont, Brushes.Black, 20, currentY);
                e.Graphics.DrawString("S.S #", boldFont, Brushes.Black, 45, currentY);
                e.Graphics.DrawString("Name", boldFont, Brushes.Black, 110, currentY);
                e.Graphics.DrawString("Phone", boldFont, Brushes.Black, 205, currentY);
                e.Graphics.DrawString("Email", boldFont, Brushes.Black, 268, currentY);
                e.Graphics.DrawString("Home Address", boldFont, Brushes.Black, 390, currentY);

                currentY += 22;

                while (totalnumber < totalCleints) // check the number of items
                {
                    string ss = clientsList.Tables[0].Rows[totalnumber].ItemArray.GetValue(1).ToString();
                    string name = clientsList.Tables[0].Rows[totalnumber].ItemArray.GetValue(2).ToString() + " " + clientsList.Tables[0].Rows[totalnumber].ItemArray.GetValue(4).ToString();
                    string cell = clientsList.Tables[0].Rows[totalnumber].ItemArray.GetValue(7).ToString();
                    string email = clientsList.Tables[0].Rows[totalnumber].ItemArray.GetValue(8).ToString();
                    string address = clientsList.Tables[0].Rows[totalnumber].ItemArray.GetValue(9).ToString();

                    e.Graphics.DrawString((totalnumber + 1).ToString(), normalFont, Brushes.Black, 20, currentY);
                    e.Graphics.DrawString(ss, normalFont, Brushes.Black, 45, currentY);
                    e.Graphics.DrawString(name, normalFont, Brushes.Black, 110, currentY);
                    e.Graphics.DrawString(cell, normalFont, Brushes.Black, 205, currentY);
                    e.Graphics.DrawString(email, normalFont, Brushes.Black, 268, currentY);
                    e.Graphics.DrawString(address, addressFont, Brushes.Black, 390, currentY);

                    currentY += 20; // set a gap between every item
                    totalnumber += 1; //increment count by 1
                    if (itemperpage < 20) // check whether  the number of item(per page) is more than 20 or not
                    {
                        itemperpage += 1; // increment itemperpage by 1
                        e.HasMorePages = false; // set the HasMorePages property to false , so that no other page will not be added

                    }

                    else // if the number of item(per page) is more than 20 then add one page
                    {
                        itemperpage = 0; //initiate itemperpage to 0 .
                        e.HasMorePages = true; //e.HasMorePages raised the PrintPage event once per page .
                        return;//It will call PrintPage event again

                    }
                }
            }
        }

        private void printDocument2_PrintPage(object sender, PrintPageEventArgs e)
        {
            //get all insurance types...
            DataSet insuranceList = new DataSet();
            insuranceList = dbHandler.getInsTypes();

            DateTime now = DateTime.Now;

            if (insuranceList.Tables[0].Rows.Count > 0)
            {
                int totalInsurance = insuranceList.Tables[0].Rows.Count;

                Font headerFont = new Font("Arial", 16, FontStyle.Bold);
                e.Graphics.DrawString("Insurance Co. Ltd", headerFont, Brushes.Black, 20, 20);
                Font headerDateFont = new Font("Arial", 10, FontStyle.Bold);
                e.Graphics.DrawString(now.ToString("F"), headerDateFont, Brushes.Black, 330, 26);

                //line breaker for report header.
                Pen blackPen = new Pen(Color.Black, 3);

                PointF point1 = new PointF(20.0F, 45.0F);
                PointF point2 = new PointF(570.0F, 45.0F);

                e.Graphics.DrawLine(blackPen, point1, point2);

                //line under the table headers.
                Pen blackPen2 = new Pen(Color.Black, 1);

                PointF point3 = new PointF(20.0F, 95.0F);
                PointF point4 = new PointF(570.0F, 95.0F);

                e.Graphics.DrawLine(blackPen2, point3, point4);

                //define font size and style...

                Font normalFont = new Font("Arial", 7);
                Font addressFont = new Font("Arial", 7);
                Font titleFont = new Font("Arial", 12, FontStyle.Bold);
                Font boldFont = new Font("Arial", 8, FontStyle.Bold);

                float currentY = 55;// declare  one variable for height measurement
                e.Graphics.DrawString("Insurance Types Report", titleFont, Brushes.Black, 20, currentY);//this will print one heading/title in every page of the document
                currentY += 25;

                // draw the table headers here....
                e.Graphics.DrawString("ID", boldFont, Brushes.Black, 20, currentY);
                e.Graphics.DrawString("Name", boldFont, Brushes.Black, 50, currentY);
                e.Graphics.DrawString("Description", boldFont, Brushes.Black, 152, currentY);
                e.Graphics.DrawString("Status", boldFont, Brushes.Black, 525, currentY);

                currentY += 22;

                while (totalnumber < totalInsurance) // check the number of items
                {
                    string name = insuranceList.Tables[0].Rows[totalnumber].ItemArray.GetValue(1).ToString();                    
                    string desc = insuranceList.Tables[0].Rows[totalnumber].ItemArray.GetValue(2).ToString();
                    string status = insuranceList.Tables[0].Rows[totalnumber].ItemArray.GetValue(3).ToString() == "1" ? "Active" : "Inactive";

                    SizeF sf = e.Graphics.MeasureString(desc, normalFont, 365);
                    SizeF iSize = e.Graphics.MeasureString(name, normalFont, 120);

                    e.Graphics.DrawString((totalnumber + 1).ToString(), normalFont, Brushes.Black, 20, currentY);
                    e.Graphics.DrawString(name, normalFont, Brushes.Black, new RectangleF(new PointF(50.0F, currentY), iSize), StringFormat.GenericTypographic);
                    e.Graphics.DrawString(desc, normalFont, Brushes.Black, new RectangleF(new PointF(152.0F, currentY), sf), StringFormat.GenericTypographic);                  
                    e.Graphics.DrawString(status, normalFont, Brushes.Black, 525, currentY);
                    

                    currentY += sf.Height; // set a gap between every item
                    currentY += 10;
                    totalnumber += 1; //increment count by 1
                    if (itemperpage < 20) // check whether  the number of item(per page) is more than 20 or not
                    {
                        itemperpage += 1; // increment itemperpage by 1
                        e.HasMorePages = false; // set the HasMorePages property to false , so that no other page will not be added

                    }

                    else // if the number of item(per page) is more than 20 then add one page
                    {
                        itemperpage = 0; //initiate itemperpage to 0 .
                        e.HasMorePages = true; //e.HasMorePages raised the PrintPage event once per page .
                        return;//It will call PrintPage event again

                    }
                }
            }
        }

        private void printDocument3_PrintPage(object sender, PrintPageEventArgs e)
        {
            //get all insurance types...
            DataSet claimsList = new DataSet();
            claimsList = dbHandler.getAllClaims();

            DateTime now = DateTime.Now;

            if (claimsList.Tables[0].Rows.Count > 0)
            {
                Font headerFont = new Font("Arial", 16, FontStyle.Bold);
                e.Graphics.DrawString("Insurance Co. Ltd", headerFont, Brushes.Black, 20, 20);
                Font headerDateFont = new Font("Arial", 10, FontStyle.Bold);
                e.Graphics.DrawString(now.ToString("F"), headerDateFont, Brushes.Black, 330, 26);

                int totalInsurance = claimsList.Tables[0].Rows.Count;

                //line breaker for report header.
                Pen blackPen = new Pen(Color.Black, 3);

                PointF point1 = new PointF(20.0F, 45.0F);
                PointF point2 = new PointF(570.0F, 45.0F);

                e.Graphics.DrawLine(blackPen, point1, point2);

                //line under the table headers.
                Pen blackPen2 = new Pen(Color.Black, 1);

                PointF point3 = new PointF(20.0F, 95.0F);
                PointF point4 = new PointF(570.0F, 95.0F);

                e.Graphics.DrawLine(blackPen2, point3, point4);

                //define font size and style...

                Font normalFont = new Font("Arial", 7);
                Font addressFont = new Font("Arial", 7);
                Font titleFont = new Font("Arial", 12, FontStyle.Bold);
                Font boldFont = new Font("Arial", 8, FontStyle.Bold);

                float currentY = 55;// declare  one variable for height measurement
                e.Graphics.DrawString("Insurance Claims Report", titleFont, Brushes.Black, 20, currentY);//this will print one heading/title in every page of the document
                currentY += 25;

                // draw the table headers here....
                e.Graphics.DrawString("ID", boldFont, Brushes.Black, 20, currentY);
                e.Graphics.DrawString("Name", boldFont, Brushes.Black, 50, currentY);
                e.Graphics.DrawString("Insurance Type", boldFont, Brushes.Black, 126, currentY);
                e.Graphics.DrawString("Description", boldFont, Brushes.Black, 225, currentY);
                e.Graphics.DrawString("Status", boldFont, Brushes.Black, 525, currentY);

                currentY += 22;

                while (totalnumber < totalInsurance) // check the number of items
                {
                    int c_name_id = int.Parse(claimsList.Tables[0].Rows[totalnumber].ItemArray.GetValue(1).ToString());
                    int ins_id = int.Parse(claimsList.Tables[0].Rows[totalnumber].ItemArray.GetValue(2).ToString());
                    string desc = claimsList.Tables[0].Rows[totalnumber].ItemArray.GetValue(3).ToString();
                    string status = claimsList.Tables[0].Rows[totalnumber].ItemArray.GetValue(4).ToString() == "1" ? "Paid" : "Pending";

                    DataSet clientRow = dbHandler.getClientInfoById(c_name_id);
                    DataSet insRow = dbHandler.getInsInfoById(ins_id);

                    string clientName = "";
                    string insName = "";

                    if (clientRow.Tables[0].Rows.Count > 0)
                        clientName = clientRow.Tables[0].Rows[0].ItemArray.GetValue(2).ToString() + " " + clientRow.Tables[0].Rows[0].ItemArray.GetValue(4).ToString();

                    if (insRow.Tables[0].Rows.Count > 0)
                        insName = insRow.Tables[0].Rows[0].ItemArray.GetValue(1).ToString();

                    SizeF sf = e.Graphics.MeasureString(desc, normalFont, 300);
                    SizeF iSize = e.Graphics.MeasureString(insName, normalFont, 120);

                    e.Graphics.DrawString((totalnumber + 1).ToString(), normalFont, Brushes.Black, 20, currentY);
                    e.Graphics.DrawString(clientName, normalFont, Brushes.Black, 50, currentY);
                    e.Graphics.DrawString(insName, normalFont, Brushes.Black, new RectangleF(new PointF(126.0F, currentY), iSize), StringFormat.GenericTypographic);
                    e.Graphics.DrawString(desc, normalFont, Brushes.Black, new RectangleF(new PointF(225.0F, currentY), sf), StringFormat.GenericTypographic);
                    e.Graphics.DrawString(status, normalFont, Brushes.Black, 525, currentY);

                    currentY += sf.Height; // set a gap between every item
                    currentY += 10;
                    totalnumber += 1; //increment count by 1
                    if (itemperpage < 20) // check whether  the number of item(per page) is more than 20 or not
                    {
                        itemperpage += 1; // increment itemperpage by 1
                        e.HasMorePages = false; // set the HasMorePages property to false , so that no other page will not be added

                    }

                    else // if the number of item(per page) is more than 20 then add one page
                    {
                        itemperpage = 0; //initiate itemperpage to 0 .
                        e.HasMorePages = true; //e.HasMorePages raised the PrintPage event once per page .
                        return;//It will call PrintPage event again

                    }
                }
            }
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void clientsDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            itemperpage = totalnumber = 0;
            printPreviewDialog1.Document = printDocument1;

            printDocument1.DefaultPageSettings.PaperSize = paperSize;
            printPreviewDialog1.ShowDialog();
        }

        private void insuranceListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            itemperpage = totalnumber = 0;
            printPreviewDialog1.Document = printDocument2;

            printDocument2.DefaultPageSettings.PaperSize = paperSize;
            printPreviewDialog1.ShowDialog();
        }

        private void claimsHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            itemperpage = totalnumber = 0;
            printPreviewDialog1.Document = printDocument3;

            printDocument3.DefaultPageSettings.PaperSize = paperSize;
            printPreviewDialog1.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            addClient();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            addClient();

            addInsurance addInsToClient = new addInsurance();
            addInsToClient.Tag = clientId;
            addInsToClient.ShowDialog();
        }

        private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manageClaims fileClaim = new manageClaims();
            fileClaim.ShowDialog();
        }

        private void manageToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            benefitsPanel sysBenefits = new benefitsPanel();
            sysBenefits.ShowDialog();
        }

        private void clientsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            clientsPanel clientsWindow = new clientsPanel();
            clientsWindow.ShowDialog();
        }

        private void insurranceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            insuranceTypes insurance = new insuranceTypes();
            insurance.ShowDialog();
        }

        private void benefitsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            benefitsPanel sysBenefits = new benefitsPanel();
            sysBenefits.ShowDialog();
        }
    }
}
