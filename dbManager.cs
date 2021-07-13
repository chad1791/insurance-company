using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace InsuranceCo
{
    class dbManager
    {
        private string conString;
        private string sqlQuery;

        private MySqlConnection myCon;
        private MySqlDataAdapter myAdapter;
        private MySqlCommand com;
        private DataSet myDataSet;

        //Database settings...
        private string server;
        private string username;
        private string password;
        private string database;
        private string port;


        public dbManager(string server, string username, string password, string database, string port)
        {
            this.server = server;
            this.username = username;
            this.password = password;
            this.database = database;
            this.port = port;

            this.conString = "datasource = " + this.server + "; port= " + this.port + "; username = " + this.username + "; password = " + this.password + "; database = " + this.database + ";";
            this.myCon = new MySqlConnection(this.conString);
        }

        public dbManager(string server, string username, string password, string database, string port, string dateTimeOptions)
        {
            this.server = server;
            this.username = username;
            this.password = password;
            this.database = database;
            this.port = port;

            this.conString = "datasource = " + this.server + "; port= " + this.port + "; username = " + this.username + "; password = " + this.password + "; database = " + this.database + ";" + dateTimeOptions;
            this.myCon = new MySqlConnection(this.conString);
        }

        public bool testConnection()
        {
            try
            {
                myCon.Open();
                return myCon.State == ConnectionState.Open ? true : false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        public int addClient(string socialSecurity, string firstname, string middlename, string lastname, string gender, string dateofbirth, string phone, string email, string address, string workplace)
        {
            int status = 1;
            int deleted = 0;

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            com = new MySqlCommand("INSERT INTO clients(ss, first, middle, last, gender, dob, phone, email, address, workplace, status, deleted) VALUES(@ss, @first, @middle, @last, @gender, @dob, @phone, @email, @address, @workplace, @status, @deleted); select last_insert_id();", myCon);
            
            try
            {
            
                com.Parameters.AddWithValue("@ss", socialSecurity);
                com.Parameters.AddWithValue("@first", firstname);
                com.Parameters.AddWithValue("@middle", middlename);
                com.Parameters.AddWithValue("@last", lastname);
                com.Parameters.AddWithValue("@gender", gender);
                com.Parameters.AddWithValue("@dob", dateofbirth);
                com.Parameters.AddWithValue("@phone", phone);
                com.Parameters.AddWithValue("@email", email);
                com.Parameters.AddWithValue("@address", address);
                com.Parameters.AddWithValue("@workplace", workplace);
                com.Parameters.AddWithValue("@status", status);
                com.Parameters.AddWithValue("@deleted", deleted);

                int id = Convert.ToInt32(com.ExecuteScalar());
                myCon.Close();

                // return last inserted id.

                return id;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }            
        }

        public int editClientInfo(int clientId, string socialSecurity, string firstname, string middlename, string lastname, string gender, string dateofbirth, string phone, string email, string address, string workplace)
        {

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            this.sqlQuery = "UPDATE clients SET ss=@ss, first=@first, middle=@middle, last=@last, gender=@gender, dob=@dob, phone=@phone, email=@email, address=@address, workplace=@workplace WHERE id =@clientId";
            com = new MySqlCommand(this.sqlQuery, myCon);            

            try
            {
                com.Parameters.AddWithValue("@clientId", clientId);
                com.Parameters.AddWithValue("@ss", socialSecurity);
                com.Parameters.AddWithValue("@first", firstname);
                com.Parameters.AddWithValue("@middle", middlename);
                com.Parameters.AddWithValue("@last", lastname);
                com.Parameters.AddWithValue("@gender", gender);
                com.Parameters.AddWithValue("@dob", dateofbirth);
                com.Parameters.AddWithValue("@phone", phone);
                com.Parameters.AddWithValue("@email", email);
                com.Parameters.AddWithValue("@address", address);
                com.Parameters.AddWithValue("@workplace", workplace);
                com.ExecuteNonQuery();
                
                myCon.Close();

                // return last inserted id.

                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        public DataSet getAllClients() //getAllInsurances
        {
            myDataSet = new DataSet();

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            this.sqlQuery = "Select * FROM `clients` WHERE deleted=0";

            try
            {
                myAdapter = new MySqlDataAdapter(this.sqlQuery, myCon);
                myAdapter.Fill(myDataSet);
                myCon.Close();

                return myDataSet;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return myDataSet;
            }

        }

        public DataSet getAllInsurances()
        {
            myDataSet = new DataSet();

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            this.sqlQuery = "Select * FROM `insurance_types` WHERE deleted=0";

            try
            {
                myAdapter = new MySqlDataAdapter(this.sqlQuery, myCon);
                myAdapter.Fill(myDataSet);
                myCon.Close();

                return myDataSet;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return myDataSet;
            }

        }

        public DataSet getClientInfoById(int clientId)
        {
            myDataSet = new DataSet();

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            this.sqlQuery = "Select * FROM `clients` WHERE id = '" + clientId + "'";

            try
            {
                myAdapter = new MySqlDataAdapter(this.sqlQuery, myCon);
                myAdapter.Fill(myDataSet);
                myCon.Close();

                return myDataSet;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return myDataSet;
            }
        }

        public bool delClient(int clientId)
        {
            int deleted = 1;

            myDataSet = new DataSet();

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            this.sqlQuery = "UPDATE clients SET deleted=@deleted WHERE id =@clientId";

            com = new MySqlCommand(this.sqlQuery, myCon);           

            try
            {
                com.Parameters.AddWithValue("@deleted", deleted);
                com.Parameters.AddWithValue("@clientId", clientId);
                com.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

        }

        public bool updateIns(string name, string description, int insId, List<int> benefits)
        {
            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            ///delete all benefits...
            this.sqlQuery = "delete from ins_benefits WHERE insurance_id =@insId";
            com = new MySqlCommand(this.sqlQuery, myCon);
            com.Parameters.AddWithValue("@insId", insId);
            com.ExecuteNonQuery();

            //update insurance record with new values..

            this.sqlQuery = "UPDATE insurance_types SET name=@name, des=@des WHERE id =@insuranceId";
            com = new MySqlCommand(this.sqlQuery, myCon);

            try
            {
                com.Parameters.AddWithValue("@name", name);
                com.Parameters.AddWithValue("@des", description);
                com.Parameters.AddWithValue("@insuranceId", insId);
                com.ExecuteNonQuery();

                //readd benefits into the ins_benefits table

                foreach (var item in benefits)
                {
                    int benefitId = (int)item;

                    com = new MySqlCommand("INSERT INTO ins_benefits(insurance_id, benefit_id, status, deleted) VALUES(@insurance_id, @benefit_id, @status, @deleted)", myCon);
                    com.Parameters.AddWithValue("@insurance_id", insId);
                    com.Parameters.AddWithValue("@benefit_id", benefitId);
                    com.Parameters.AddWithValue("@status", 1);
                    com.Parameters.AddWithValue("@deleted", 0);

                    com.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

        }

        public bool delInsurance(int insuranceId)
        {
            int deleted = 1;

            myDataSet = new DataSet();

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            this.sqlQuery = "UPDATE insurance_types SET deleted=@deleted WHERE id =@insuranceId";

            com = new MySqlCommand(this.sqlQuery, myCon);

            try
            {
                com.Parameters.AddWithValue("@deleted", deleted);
                com.Parameters.AddWithValue("@insuranceId", insuranceId);
                com.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

        }

        public int addClientInsurance(int clientId, int insuranceId, string paymentMethod, string packagePayment, string expiryDate)
        {
            int status = 1;
            int deleted = 0;

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            com = new MySqlCommand("INSERT INTO clients_insurance(client_id, insurance_id, payment_method, package_payments, expiry_date, status, deleted) VALUES(@client_id, @insurance_id, @payment_method, @package_payments, @expiry_date, @status, @deleted)", myCon);

            try
            {
                com.Parameters.AddWithValue("@client_id", clientId);
                com.Parameters.AddWithValue("@insurance_id", insuranceId);
                com.Parameters.AddWithValue("@payment_method", paymentMethod);
                com.Parameters.AddWithValue("@package_payments", packagePayment);
                com.Parameters.AddWithValue("@expiry_date", expiryDate);
                com.Parameters.AddWithValue("@status", status);
                com.Parameters.AddWithValue("@deleted", deleted);

                com.ExecuteNonQuery();
                myCon.Close();

                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        public int addBenefit(string name, string description)
        {
            int status = 1;
            int deleted = 0;

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            com = new MySqlCommand("INSERT INTO benefits(name, des, status, deleted) VALUES(@name, @des, @status, @deleted)", myCon);

            try
            {
                com.Parameters.AddWithValue("@name", name);
                com.Parameters.AddWithValue("@des", description);     
                com.Parameters.AddWithValue("@status", status);
                com.Parameters.AddWithValue("@deleted", deleted);

                com.ExecuteNonQuery();
                myCon.Close();

                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        public bool delBenefit(int benefitId)
        {
            int deleted = 1;

            myDataSet = new DataSet();

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            this.sqlQuery = "UPDATE benefits SET deleted=@deleted WHERE id =@benefitId";

            com = new MySqlCommand(this.sqlQuery, myCon);

            try
            {
                com.Parameters.AddWithValue("@deleted", deleted);
                com.Parameters.AddWithValue("@benefitId", benefitId);
                com.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

        }

        public DataSet getInsBenefits()
        {
            myDataSet = new DataSet();

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            this.sqlQuery = "Select * FROM `benefits` WHERE deleted=0";

            try
            {
                myAdapter = new MySqlDataAdapter(this.sqlQuery, myCon);
                myAdapter.Fill(myDataSet);
                myCon.Close();

                return myDataSet;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return myDataSet;
            }
        }

        public int addInsType(string name, string description, List<int> benefits)
        {
            int status = 1;
            int deleted = 0;

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            com = new MySqlCommand("INSERT INTO insurance_types(name, des, status, deleted) VALUES(@name, @des, @status, @deleted); select last_insert_id();", myCon);

            try
            {

                com.Parameters.AddWithValue("@name", name);
                com.Parameters.AddWithValue("@des", description);
                com.Parameters.AddWithValue("@status", status);
                com.Parameters.AddWithValue("@deleted", deleted);

                int id = Convert.ToInt32(com.ExecuteScalar());                                

                //add insurance benefits in the ins_benefits tables

                try
                {
                    foreach (var item in benefits)
                    {
                        int benefitId = (int)item;

                        com = new MySqlCommand("INSERT INTO ins_benefits(insurance_id, benefit_id, status, deleted) VALUES(@insurance_id, @benefit_id, @status, @deleted)", myCon);
                        com.Parameters.AddWithValue("@insurance_id", id);
                        com.Parameters.AddWithValue("@benefit_id", benefitId);
                        com.Parameters.AddWithValue("@status", status);
                        com.Parameters.AddWithValue("@deleted", deleted);

                        com.ExecuteNonQuery();
                    }
                }
                catch (Exception ex2)
                {
                    
                }

                myCon.Close();

                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        public DataSet getInsTypes()
        {
            myDataSet = new DataSet();

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            this.sqlQuery = "Select * FROM `insurance_types`";

            try
            {
                myAdapter = new MySqlDataAdapter(this.sqlQuery, myCon);
                myAdapter.Fill(myDataSet);
                myCon.Close();

                return myDataSet;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return myDataSet;
            }
        }

        public DataSet getInsInfoById(int insId)
        {
            myDataSet = new DataSet();

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            this.sqlQuery = "Select * FROM `insurance_types` WHERE id = '" + insId + "'";

            try
            {
                myAdapter = new MySqlDataAdapter(this.sqlQuery, myCon);
                myAdapter.Fill(myDataSet);
                myCon.Close();

                return myDataSet;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return myDataSet;
            }
        }

        public DataSet getInsTypesByClientId(int clientId, string today) 
        {
            myDataSet = new DataSet();

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            //this.sqlQuery = "Select * FROM `clients_insurance` WHERE client_id = '" + clientId + "' AND expiry_date >" + today;
            this.sqlQuery = "Select * FROM `clients_insurance` WHERE client_id = '" + clientId + "'";

            try
            {
                myAdapter = new MySqlDataAdapter(this.sqlQuery, myCon);
                myAdapter.Fill(myDataSet);
                myCon.Close();

                return myDataSet;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return myDataSet;
            }
        }

        public int addInsClaim(int clientId, int insId, string description)
        {
            int status = 1;
            int deleted = 0;

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            com = new MySqlCommand("INSERT INTO claims(client_id, insurance_id, des, status, deleted) VALUES(@client_id, @insurance_id, @des, @status, @deleted)", myCon);

            try
            {
                com.Parameters.AddWithValue("@client_id", clientId);
                com.Parameters.AddWithValue("@insurance_id", insId);
                com.Parameters.AddWithValue("@des", description);
                com.Parameters.AddWithValue("@status", status);
                com.Parameters.AddWithValue("@deleted", deleted);

                com.ExecuteNonQuery();
                myCon.Close();

                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        public DataSet getAllClaims()
        {
            myDataSet = new DataSet();

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            this.sqlQuery = "Select * FROM `claims`";

            try
            {
                myAdapter = new MySqlDataAdapter(this.sqlQuery, myCon);
                myAdapter.Fill(myDataSet);
                myCon.Close();

                return myDataSet;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return myDataSet;
            }
        }

        public DataSet getBenefitsByInsId(int insId)
        {
            myDataSet = new DataSet();

            if (myCon.State == ConnectionState.Closed)
                myCon.Open();

            this.sqlQuery = "Select * FROM `ins_benefits` WHERE insurance_id = '" + insId + "'";

            try
            {
                myAdapter = new MySqlDataAdapter(this.sqlQuery, myCon);
                myAdapter.Fill(myDataSet);
                myCon.Close();

                return myDataSet;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return myDataSet;
            }

        }

    }
}
