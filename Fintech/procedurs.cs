using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace Fintech
{
    public class procedurs
    {
        private connString constring = new connString();
        private MailMgr mailMessenger = new MailMgr();
        private SH1Encryption sh1 = new SH1Encryption();
        private DataTable dt = new DataTable();

        public int existUser(string uname, string pwd, string connLocation, out string expMessage)
        {
            string str = uname;
            int num = 0;
            try
            {

                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = this.constring.connstring(connLocation);
                connection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(new SqlCommand("select username,password from [users] where username='" + str + "'and password='" + pwd + "'and status='1'", connection));
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = dataTable1;
                sqlDataAdapter.Fill(dataTable2);
                num = dataTable1.Rows.Count <= 0 ? 0 : dataTable1.Rows.Count;
                expMessage = "";
                connection.Close();
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public int existUserPin(string uname, string pin, string connLocation, out string expMessage)
        {
            string str = uname;
            int num = 0;
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = this.constring.connstring(connLocation);
                connection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(new SqlCommand("select username,transPIN from [users] where username='" + str + "'and transPIN='" + pin + "'and status='1'", connection));
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = dataTable1;
                sqlDataAdapter.Fill(dataTable2);
                num = dataTable1.Rows.Count <= 0 ? 0 : dataTable1.Rows.Count;
                expMessage = "";
                connection.Close();
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public int existphone(string uname, string connLocation, out string expMessage)
        {
            string str = uname;
            int num = 0;
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = this.constring.connstring(connLocation);
                connection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(new SqlCommand("select * from [users] where phone='" + str + "' and status='1'", connection));
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = dataTable1;
                sqlDataAdapter.Fill(dataTable2);
                num = dataTable1.Rows.Count <= 0 ? 0 : dataTable1.Rows.Count;
                expMessage = "";
                connection.Close();
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public string getPawword(
          string uname,
          string pwd,
          out string mail,
          string connLocation,
          out string expMessage)
        {
            string str1 = uname;
            string str2 = "";
            mail = "";
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = this.constring.connstring(connLocation);
                connection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(new SqlCommand("select username,password,email from [users] where username='" + str1 + "'and password='" + pwd + "'", connection));
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = dataTable1;
                sqlDataAdapter.Fill(dataTable2);
                foreach (DataRow row in (InternalDataCollectionBase)dataTable1.Rows)
                {
                    str2 = row["password"].ToString();
                    mail = row["email"].ToString();
                }
                expMessage = "";
                connection.Close();
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return str2;
        }

        public int existUser2(string uname, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "existuser";
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)uname);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = int.Parse(dataTable.Rows[0]["counts"].ToString());
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public int existUser1(string uname, string pwd, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = string.IsNullOrEmpty(pwd) ? "existuser" : "existuserpwd";
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)uname);
                        if (cmdText.Equals("existuserpwd"))
                            selectCommand.Parameters.AddWithValue("@in_pwd", (object)pwd);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = int.Parse(dataTable.Rows[0]["counts"].ToString());
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public int existuserpwdz(string uname, string pwd, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(existuserpwdz);
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)uname);
                        selectCommand.Parameters.AddWithValue("@in_pwd", (object)pwd);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                if (dataTable.Rows.Count > 0)
                    num = 1;
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public DataTable getUserDetails(
          string in_uname,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "gettuser";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)in_uname);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public DataTable getResetDetails(
          string in_email,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getEmailInfo";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_email", (object)in_email);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public int exisToken1(string token, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "exisToken";
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_token", (object)token);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = int.Parse(dataTable.Rows[0]["counts"].ToString());
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public int exisbatchNo(string in_batch, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "exisBatch";
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_batch", (object)in_batch);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = int.Parse(dataTable.Rows[0]["counts"].ToString());
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public DataTable getUserbyToken(
          string token,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getUserByToken";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_token", (object)token);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public DataTable getMenuGroup(string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getMenuGroup);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                int.Parse(dataTable.Rows[0]["counts"].ToString());
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public DataTable getMenuDetails(
          string inParentID,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getMenuDetails);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@MENUID", (object)inParentID);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public int accessControl(
          string uname,
          out string displayName,
          string connLocation,
          out string strEmail,
          out string strPhone,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(accessControl);
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)uname);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = int.Parse(dataTable.Rows[0][nameof(accessControl)].ToString());
                displayName = dataTable.Rows[0]["fName"].ToString() + "  " + dataTable.Rows[0]["lName"].ToString();
                strEmail = dataTable.Rows[0]["email"].ToString();
                strPhone = dataTable.Rows[0]["phone"].ToString();
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                strEmail = "";
                strPhone = "";
                displayName = (string)null;
            }
            return num;
        }

        public DataTable getLeftNavigation(
          int accesscontrol,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getLeftNavigation);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_accessControl", (object)accesscontrol);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public string Updatepassword(string uname, string password, string connLocation)
        {
            string str;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(connLocation));
                SqlCommand sqlCommand = new SqlCommand("updatePassword", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_uname", (object)uname);
                sqlCommand.Parameters.AddWithValue("@in_password", (object)password);
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                str = "Sussessful";
                connection.Close();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public string Updatepin(string uname, string pin, string connLocation)
        {
            string str;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(connLocation));
                SqlCommand sqlCommand = new SqlCommand("updatePin", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_uname", (object)uname);
                sqlCommand.Parameters.AddWithValue("@in_password", (object)pin);
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                str = "Sussessful";
                connection.Close();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public string UpdateToken(string uname, string token, string connLocation)
        {
            string str;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(connLocation));
                SqlCommand sqlCommand = new SqlCommand("updateToken", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_uname", (object)uname);
                sqlCommand.Parameters.AddWithValue("@in_token", (object)token);
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                str = "Sussessful";
                connection.Close();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public int UpdatevendorLevel(string saveaseId, string connLocation, out int expMessage)
        {
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(connLocation));
                SqlCommand sqlCommand = new SqlCommand("updateVendorLevel", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@insaveaseId", (object)saveaseId);
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                expMessage = 1;
                connection.Close();
            }
            catch (Exception ex)
            {
                expMessage = 0;
            }
            return expMessage;
        }

        public int UpdateLevel(string saveaseId, string connLocation, out int expMessage)
        {
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(connLocation));
                SqlCommand sqlCommand = new SqlCommand("updateLevel", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@insaveaseId", (object)saveaseId);
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                expMessage = 1;
                connection.Close();
            }
            catch (Exception ex)
            {
                expMessage = 0;
            }
            return expMessage;
        }

        public string RegisterUser(
          int accesscontrol,
          string fname,
          string lname,
          string phone,
          string email,
          string username,
          string password,
          string createdBy,
          string connLocation,
          string token,
          string status,
          string TransPin)
        {
            string shA1HashData = this.sh1.GetSHA1HashData(username);
            string randomAcctOfficer = this.getRandomAcctOfficer(connLocation);
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(connLocation));
                SqlCommand sqlCommand = new SqlCommand(nameof(RegisterUser), connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_fname", (object)fname);
                sqlCommand.Parameters.AddWithValue("@in_lname", (object)lname);
                sqlCommand.Parameters.AddWithValue("@in_dname", (object)(lname + " " + fname));
                sqlCommand.Parameters.AddWithValue("@in_phone", (object)phone);
                sqlCommand.Parameters.AddWithValue("@in_username", (object)username);
                sqlCommand.Parameters.AddWithValue("@in_password", (object)password);
                sqlCommand.Parameters.AddWithValue("@in_email", (object)email);
                sqlCommand.Parameters.AddWithValue("@in_saveaseid", (object)phone.Substring(phone.Length - Math.Min(10, phone.Length)));
                sqlCommand.Parameters.AddWithValue("@in_createdBy", (object)createdBy);
                sqlCommand.Parameters.AddWithValue("@in_userlvl", (object)accesscontrol);
                sqlCommand.Parameters.AddWithValue("@in_token", (object)shA1HashData);
                sqlCommand.Parameters.AddWithValue("@in_status", (object)status);
                sqlCommand.Parameters.AddWithValue("@in_transPIN", (object)TransPin);
                sqlCommand.Parameters.AddWithValue("@in_accountOfficer", (object)randomAcctOfficer);
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                string expMessage = "Successful";
                connection.Close();
                this.sendMailNotificationToUser(this.getSaveaseID_By_Name(username, connLocation, out expMessage), username);
                string url = "http://savease.ng/activate.aspx?ud=" + username + "&token=" + shA1HashData;
                string body = this.PopulateBody2(fname + " " + lname, "Customer registration", url, "Savease New Customer Registration", "mailer/index.html", "Reg");
               // this.mailMessenger.SendHtmlFormattedEmailAsync(email, "Complete Your Registration", body).ConfigureAwait(true);
                expMessage = "Successful";
                return expMessage;
            }
            catch (Exception ex)
            {
                return "Fail";
            }
        }

        private string PopulateBody(
          string userName,
          string title,
          string url,
          string description,
          string filepath,
          string mailtype)
        {
            return string.Empty + " <div style = 'width: 500px; background: whitesmoke; margin-right: auto; margin-left: auto;' > <div  style = 'margin-left: auto;margin-right: auto;width: 350px;background: white;' ><div  style='height: 70px; background:whitesmoke;'></div><div style = 'width: 195px;margin-left: auto;margin-right: auto;'> <img src = 'http://www.savease.ng/logo/logo.png' style = 'margin-top: 20px' ></div><br><br><div><h3 style = 'text-align: center;font-family:calibri;font-size: 18px;' > Complete your Registration</h3><br><br><p style = 'margin-left: 15px; font-family:calibri;font-size: 11px' > Please click the  button below to complete your registration</p></div><br><br> <div style = ' width: 250px;height: 70px;margin-right: auto;margin-left: auto;' > <a href='" + url + "'><input type= 'submit' name= '' value= 'Complete Registration' style= 'background: #fa9928;color: white;margin-left: 30px;margin-right: 30px;margin-top: 20px;border-radius: 4px;height:35px;width: 200px;' ></ a > </div ><br ><p style= 'color: #212435; text-align: center;font-family:calibri;font-size: 10px;'> or Copy Link</p>" + url + " <br><br><hr><br><br><br>  <div><h4 style = 'color: #fa9928;text-align: center;font-family:calibri; font-size: 12px;' > Questions ? Get your answer here:&nbsp;<a href = 'faq.aspx' style='color: blue; font-size: smaller;font-family:calibri; font-size: 9px;'>Help Center</a></h4></div><br><br><br><div><h5 style = 'color: #212435; text-align: center;font-family:calibri;font-size: 10px;' > You may contact us through any of the option:</h5></div><div> <p style = 'color: #212435; text-align: center;font-family:calibri;font-size: 10px;' > Phone: 0700SAVEASE, 07032599897,08056161069</p></div> <div style = 'text-align: center;font-size: 10px;' ><p style= 'color: #212435; text-align: center;font-family:calibri;' > care@savease.ng, enquiry @savease.ng</div>  <div style = 'text-align: center;color: #212435;font-family:calibri;font-size: 10px;font-weight: bold;' > &copy; 2018 Savease.All Rights Reserved.</div><div style = 'text-align: center; font-family: calibri;color:#212435;font-size: 10px;font-weight: bolder; '>|<a href='doc/TnC.pdf' target='_blank'>Privacy Policy</a> | <a href = 'doc/TnC.pdf' target= '_blank' > General Terms and Condition</a> </div> </div></div>";
        }

        private string PopulateBody2(
          string userName,
          string title,
          string url,
          string description,
          string filepath,
          string mailtype)
        {
            return string.Empty + "<head><body style='padding: 0 !important;margin: 0 !important;display: block !important;min-width: 100% !important;width: 100% !important;background: #ffffff;'><div class='body' style='width: 100% !important;background-image: url('http://www.savease.ng/dashboard/assets/images/dashboard-bg.png');background-repeat: no-repeat;background-size: cover; background-position: right center;display: flex;justify-content: center;align-items: center; padding: 35px 0;'><div class='wrapper' style='background-color: #fff; width: 650px;min-width: 650px;font-size: 0pt;line-height: 0pt;margin: 0;font-weight: normal;padding: 25px 0px;border-radius: 26px;'><div class='header' style='padding: 40px 60px;padding-bottom: 0px;border-radius: 26px 26px 0px 0px;'><div style='padding-bottom: 10px;'><img src='http://www.savease.ng/assets/images/banner-1.png' style=' width: 100%;'/></div><div style='width: 100%;padding-bottom: 10px;text-align: right;'> <img src='http://www.savease.ng/assets/images/logo.png' style=' width: 20%;'/></div><div class='header-title' style='color: #12325c;font-family: sans-serif;font-size: 16px;line-height: 20px;text-align:center;padding-bottom: 15px;' >Complete Your Registration</div></div><div class='main' style=' padding: 0px 60px; padding-bottom: 0px;'><div class='text2' style='color: #12325c;font-family:sans-serif;font-size: 12px;line-height: 26px;text-align: center;padding-bottom: 20px;'>Please click on the button below to complete your registration.</div><div class='text3' style='width:fit-content;margin:0 auto;color: #c1cddc;font-family:sans-serif; font-size: 12px;padding: 15px 30px;text-align: center;'> <a href='" + url + "'><input type= 'submit' name= '' value= 'Complete Registration' style= 'background: #fa9928;color: white;font-size:12px;margin-left: 30px;margin-right: 30px;margin-top: 20px;border-radius: 0px 22px 22px 22px;border: none;height:35px;width: 200px;' ></ a ></div><div class='text2' style='color: #12325c;font-family:sans-serif;font-size: 12px;line-height: 26px;text-align: center;padding-bottom: 20px;'>Button not working? Paste the following link into your browser:</div><div class='text2' style='color: #12325c;font-family:sans-serif;font-size: 12px;line-height: 26px;text-align: center;padding-bottom: 20px;'><a href='' target='_blank' class='link-txt' style='color: #FA9928;font-weight: bold;line-height: 26px;text-decoration: none;'>'" + url + "'</a></div><div class='text2' style='color: #12325c;font-family: sans-serif;font-size: 12px;line-height: 26px;text-align: center;padding-bottom: 20px;'>You're receiving this message because you signed up to savease gateway.</div><div class='text2 style='color: #12325c;font-family: sans-serif;font-size: 12px;line-height: 26px;text-align: center;padding-bottom: 20px;'>Questions? Get your answers here: <a href='#' target='_blank' class='reg-link' style='color: #FA9928;text-decoration: none;'>Help Center</a>.</div><div class='img' width='55' style='font-size: 0pt;line-height: 0pt; text-align: center;'><a href='#' target='_blank'><img src='http://www.savease.ng/assets/images/google.png' border='0' alt=''  style=' margin-right: 10px;'/></a> <a href='#' target='_blank'><img src='http://www.Savease.ng/assets/images/apple.png' border='0' alt='' /></a></div></div><div class='footer' style='display:block;padding: 50px 30px;border-radius: 0px 0px 26px 26px;'><div class='social' style='padding-bottom: 10px;text-align: center;'><ul style='list-style:none;margin:0 auto;'><li style='font-size: 0pt;line-height: 0pt;text-align: center;width: fit-content;display:inline-block;'><a href='#' target='_blank'><img src='http://www.savease.ng/assets/images/face.png' width='38' height='38' border='0' alt='' /></a></li><li style='font-size: 0pt;line-height: 0pt;text-align: center;width: fit-content;display:inline-block;'><a href='#' target='_blank'><img src='http://www.savease.ng/assets/images/tweet.png' width='38' height='38' border='0' alt='' /></li><li style='font-size: 0pt;line-height: 0pt;text-align: center;width: fit-content;display:inline-block;'><a href='#' target='_blank'><img src='http://www.savease.ng/assets/images/insta.png' width='38' height='38' border='0' alt='' /></li><li style='font-size: 0pt;line-height: 0pt;text-align: center;width: fit-content;display:inline-block;'><a href='#' target='_blank'><img src='http://www.savease.ng/assets/images/link.png' width='38' height='38' border='0' alt='' /></li></ul><div class='text-footer1' style='color: #12325c;font-family:sans-serif;font-size: 10px;line-height: 10px;text-align: center;padding-bottom: 6px;'><a href='http://www.savease.ng' target='_blank' class='link2-u' style='color: #12325c;text-decoration: none;'><span class='link2-u' style='color: #475c77;text-decoration: none !important;'>www.savease.ng</span></a></div><div class='text-footer1' style='color: #12325c;font-family:sans-serif;font-size: 10px;line-height: 10px;text-align: center;padding-bottom: 6px;'><a href='mailto:info@savease.ng' class='link2-u' style=' color: #12325c;text-decoration: none;margin-right: 7px;'><span class='link2-u'style='color: #475c77;text-decoration: none !important;'>info@savease.ng</span></a>,<a href='mailto:enquiry@savease.ng' class='link2-u'style='color: #475c77;text-decoration: none !important;'><span class='link2-u'>enquiry@savease.ng</span></a></div><div class='text-footer1' style='color: #12325c;font-family:sans-serif;font-size: 10px;line-height: 10px;text-align: center;padding-bottom: 6px;'><a href='tell:2348027226627' class='link2-u' style=' color: #12325c;text-decoration: none;margin-right: 7px;'><span class='link2-u'style='color: #475c77;text-decoration: none !important;'>+234 8027226627</span></a>, <a href='tell:+2349090692222' class='link2-u'><span class='link2-u'>+2349090692222</span></a></div><div class='text-footer1' style='color:#12325c; font-family:sans-serif; font-size:10px; line-height:10px; text-align:center;'>© 2018. Savease. All Rights Reserved. | <a href='#' class='link2-u'style=' color: #12325c;text-decoration: none;margin-right: 7px;'><span class='link2-u'style='color: #475c77;text-decoration: none !important;'>Privacy Policy</span></a> |<a href = '#' class='link2-u' style='color:#12325c; font-family:sans-serif; font-size:10px; line-height:10px; text-align:center;'><span class='link2-u'style='color: #475c77;text-decoration: none !important;'><span class='link2-u'style='color: #475c77;text-decoration: none !important;'>General Terms and Conditions</span></a> </div> </div></div></div></body>";
        }

        public int sendMailNotificationToUser(string saveaseID, string username)
        {
            SH1Encryption sh1Encryption = new SH1Encryption();
            MailMgr mailMgr = new MailMgr();
            try
            {
                sh1Encryption.GetSHA1HashData(username);
                foreach (DataRow row in (InternalDataCollectionBase)this.getAcctOfficer(saveaseID, "", out string _).Rows)
                    row["emailID"].ToString();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private string PopulateBody(string regfname, string reglname, string email, string phone) => string.Empty + " <div style = 'width: 500px; background: whitesmoke; margin-right: auto; margin-left: auto;' > <div  style = 'margin-left: auto;margin-right: auto;width: 350px;background: white;' ><div  style='height: 70px; background:whitesmoke;'></div><div style = 'width: 195px;margin-left: auto;margin-right: auto;'> <img src = 'http://www.savease.ng/logo/logo.png' style = 'margin-top: 20px' ></div><br><br><div><h3 style = 'text-align: center;font-family:calibri;font-size: 18px;' > New Customer created and assigned to you</h3><br><br><table><tr><td>First Name</td><td>Sur Name</td><td>Phone Number</td><td>Email Address</td></tr><tr><td>" + regfname + "</td><td>" + reglname + "</td><td>" + phone + "</td><td>" + email + "</td></tr></table><p style = 'margin-left: 15px; font-family:calibri;font-size: 11px' > Please follow up on him/her</p> <br><br><hr><br><br><br>  <div><h4 style = 'color: #fa9928;text-align: center;font-family:calibri; font-size: 12px;' > Questions ? Get your answer here:&nbsp;<a href = 'faq.aspx' style='color: blue; font-size: smaller;font-family:calibri; font-size: 9px;'>Help Center</a></h4></div><br><br><br><div><h5 style = 'color: #212435; text-align: center;font-family:calibri;font-size: 10px;' > You may contact us through any of the option:</h5></div><div> <p style = 'color: #212435; text-align: center;font-family:calibri;font-size: 10px;' > Phone: 0700SAVEASE, 07032599897,08056161069</p></div> <div style = 'text-align: center;font-size: 10px;' ><p style= 'color: #212435; text-align: center;font-family:calibri;' > care@savease.ng, enquiry @savease.ng</div>  <div style = 'text-align: center;color: #212435;font-family:calibri;font-size: 10px;font-weight: bold;' > &copy; 2018 Savease.All Rights Reserved.</div><div style = 'text-align: center; font-family: calibri;color:#212435;font-size: 10px;font-weight: bolder; '>|<a href='doc/TnC.pdf' target='_blank'>Privacy Policy</a> | <a href = 'doc/TnC.pdf' target= '_blank' > General Terms and Condition</a> </div> </div></div>";

        public string registerVendor1(
          string fname,
          string lname,
          string phone,
          string email,
          string username,
          string password,
          string createdBy,
          string companyname,
          string passport,
          string caccert,
          string cacdoc,
          string identitfication,
          string vatreg,
          string taxid,
          string address,
          string connLocation,
          string token,
          string status,
          string saveaseid)
        {
            int num = 2;
            string str;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(connLocation));
                SqlCommand sqlCommand = new SqlCommand("RegisterVendor", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_fname", (object)fname);
                sqlCommand.Parameters.AddWithValue("@in_lname", (object)lname);
                sqlCommand.Parameters.AddWithValue("@in_dname", (object)(lname + " " + fname));
                sqlCommand.Parameters.AddWithValue("@in_phone", (object)phone);
                sqlCommand.Parameters.AddWithValue("@in_username", (object)username);
                sqlCommand.Parameters.AddWithValue("@in_password", (object)password);
                sqlCommand.Parameters.AddWithValue("@in_email", (object)email);
                sqlCommand.Parameters.AddWithValue("@in_saveaseid", (object)saveaseid);
                sqlCommand.Parameters.AddWithValue("@in_createdBy", (object)createdBy);
                sqlCommand.Parameters.AddWithValue("@in_userlvl", (object)num);
                sqlCommand.Parameters.AddWithValue("@in_token", (object)token);
                sqlCommand.Parameters.AddWithValue("@in_status", (object)status);
                sqlCommand.Parameters.AddWithValue("@in_identificationType", (object)identitfication);
                sqlCommand.Parameters.AddWithValue("@in_passport", (object)passport);
                sqlCommand.Parameters.AddWithValue("@in_cacDoc", (object)caccert);
                sqlCommand.Parameters.AddWithValue("@in_allCAC", (object)cacdoc);
                sqlCommand.Parameters.AddWithValue("@in_VATReg", (object)vatreg);
                sqlCommand.Parameters.AddWithValue("@in_taxCert", (object)taxid);
                sqlCommand.Parameters.AddWithValue("@in_company", (object)companyname);
                sqlCommand.Parameters.AddWithValue("@in_CompanyAddress", (object)address);
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                str = "Sussessful";
                connection.Close();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public string registerVendor2(
          string fname,
          string lname,
          string phone,
          string email,
          string username,
          string password,
          string createdBy,
          string companyname,
          string address,
          string connLocation,
          string token,
          string status,
          string saveaseid,
          string in_bvn,
          string in_cacRegNo)
        {
            int num = 2;
            string str;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(connLocation));
                SqlCommand sqlCommand = new SqlCommand("RegisterVendor3", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_fname", (object)fname);
                sqlCommand.Parameters.AddWithValue("@in_lname", (object)lname);
                sqlCommand.Parameters.AddWithValue("@in_dname", (object)(lname + " " + fname));
                sqlCommand.Parameters.AddWithValue("@in_phone", (object)phone);
                sqlCommand.Parameters.AddWithValue("@in_username", (object)username);
                sqlCommand.Parameters.AddWithValue("@in_password", (object)password);
                sqlCommand.Parameters.AddWithValue("@in_email", (object)email);
                sqlCommand.Parameters.AddWithValue("@in_saveaseid", (object)saveaseid);
                sqlCommand.Parameters.AddWithValue("@in_createdBy", (object)createdBy);
                sqlCommand.Parameters.AddWithValue("@in_userlvl", (object)num);
                sqlCommand.Parameters.AddWithValue("@in_token", (object)token);
                sqlCommand.Parameters.AddWithValue("@in_status", (object)status);
                sqlCommand.Parameters.AddWithValue("@in_company", (object)companyname);
                sqlCommand.Parameters.AddWithValue("@in_CompanyAddress", (object)address);
                sqlCommand.Parameters.AddWithValue("@in_bvn", (object)in_bvn);
                sqlCommand.Parameters.AddWithValue("@in_cacRegNo", (object)in_cacRegNo);
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                str = "Sussessful";
                connection.Close();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public string registerVendor(
          string fname,
          string lname,
          string phone,
          string email,
          string username,
          string password,
          string createdBy,
          string companyname,
          string passport,
          byte[] passportB,
          string caccert,
          byte[] caccertB,
          string cacdoc,
          byte[] cacdocB,
          string identitfication,
          byte[] identitficationB,
          string vatreg,
          byte[] vatregB,
          string taxid,
          byte[] taxidB,
          string address,
          string connLocation,
          string token,
          string status,
          string saveaseid)
        {
            int num = 2;
            string str;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(connLocation));
                SqlCommand sqlCommand = new SqlCommand("RegisterVendor", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_fname", (object)fname);
                sqlCommand.Parameters.AddWithValue("@in_lname", (object)lname);
                sqlCommand.Parameters.AddWithValue("@in_dname", (object)(lname + " " + fname));
                sqlCommand.Parameters.AddWithValue("@in_phone", (object)phone);
                sqlCommand.Parameters.AddWithValue("@in_username", (object)username);
                sqlCommand.Parameters.AddWithValue("@in_password", (object)password);
                sqlCommand.Parameters.AddWithValue("@in_email", (object)email);
                sqlCommand.Parameters.AddWithValue("@in_saveaseid", (object)saveaseid);
                sqlCommand.Parameters.AddWithValue("@in_createdBy", (object)createdBy);
                sqlCommand.Parameters.AddWithValue("@in_userlvl", (object)num);
                sqlCommand.Parameters.AddWithValue("@in_token", (object)token);
                sqlCommand.Parameters.AddWithValue("@in_status", (object)status);
                sqlCommand.Parameters.AddWithValue("@in_identificationType", (object)identitfication);
                sqlCommand.Parameters.AddWithValue("@in_identificationTypeB", (object)identitficationB);
                sqlCommand.Parameters.AddWithValue("@in_passport", (object)passport);
                sqlCommand.Parameters.AddWithValue("@in_passportB", (object)passportB);
                sqlCommand.Parameters.AddWithValue("@in_cacDoc", (object)caccert);
                sqlCommand.Parameters.AddWithValue("@in_cacDocB", (object)caccertB);
                sqlCommand.Parameters.AddWithValue("@in_allCAC", (object)cacdoc);
                sqlCommand.Parameters.AddWithValue("@in_allCACB", (object)cacdocB);
                sqlCommand.Parameters.AddWithValue("@in_VATReg", (object)vatreg);
                sqlCommand.Parameters.AddWithValue("@in_VATRegB", (object)vatregB);
                sqlCommand.Parameters.AddWithValue("@in_taxCert", (object)taxid);
                sqlCommand.Parameters.AddWithValue("@in_taxCertB", (object)taxidB);
                sqlCommand.Parameters.AddWithValue("@in_company", (object)companyname);
                sqlCommand.Parameters.AddWithValue("@in_CompanyAddress", (object)address);
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                str = "Sussessful";
                connection.Close();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public DataTable getProfiles(
          int accesscontrol,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getLeftNavigation";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_accessControl", (object)accesscontrol);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public DataTable getVendors(
          int accesscontrol,
          string connLocation,
          string procc,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = procc;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public DataTable loadBuyPinAll(string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getAllOrderByAdmin";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public int completedorders(
          string sveIDs,
          string ordernumber,
          string totalh,
          string connLocation)
        {
            string str1 = "<strong>Order Number: " + ordernumber;
            string str2 = "";
            string str3 = str1 + "<table><tr><td>";
            string expMessage = "";
            DataTable ordersOnSave = this.getOrdersOnSave(ordernumber, connLocation);
            double num1 = 0.0;
            string str4 = "";
            DataTable dataTable = new DataTable();
            try
            {
                foreach (DataRow row1 in (InternalDataCollectionBase)ordersOnSave.Rows)
                {
                    string str5 = row1["card_pin"].ToString();
                    string str6 = row1["cardpin_sn"].ToString();
                    double num2 = double.Parse(row1["cardType"].ToString());
                    num1 = double.Parse(row1["chargeOnCard"].ToString());
                    double.Parse(row1["percentage"].ToString());
                    str4 = row1["orderby"].ToString();
                    str2 = str2 + "<table border='1'>< tr><td> PIN: '" + str5 + "'</td></tr>< tr><td> S/N: '" + str6 + "'</td></tr>< tr><td> Amount: '" + (object)num2 + "'</td></tr></table>";
                    this.getEmail(str4, "", out expMessage);
                    foreach (DataRow row2 in (InternalDataCollectionBase)dataTable.Rows)
                        row2["Email"].ToString();
                }
                this.getDName(str4, "", out expMessage);
                this.getSaveaseID_By_Name(str4, "", out expMessage);
                this.getref();
                mailHelper mailHelper = new mailHelper();
                DateTime today = DateTime.Today;
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string getref()
        {
            Random random = new Random();
            string str = "";
            for (int index = 0; index < 2; ++index)
            {
                int num = random.Next(1000, 9999);
                str += (string)(object)num;
            }
            return str;
        }

        public DataTable getOrdersOnSave(string ordernumber, string connLocation)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getOrdersOnSave);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_orderNumber", (object)ordernumber);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return dataTable;
        }

        public int saveOrder(
          string inorderstatus,
          string in_card_pin,
          string in_cardpin_sn,
          string in_orderNumber,
          string in_cardType,
          double in_cardAmount,
          double in_chargeOnCard,
          double in_percentageCharg,
          string in_orderby,
          string in_computerName,
          string in_ipaddress,
          out string expObj)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("insertOrderDetails", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_card_pin), (object)in_card_pin);
                sqlCommand.Parameters.AddWithValue(nameof(in_cardpin_sn), (object)in_cardpin_sn);
                sqlCommand.Parameters.AddWithValue(nameof(in_orderNumber), (object)in_orderNumber);
                sqlCommand.Parameters.AddWithValue(nameof(in_cardType), (object)in_cardType);
                sqlCommand.Parameters.AddWithValue(nameof(in_cardAmount), (object)in_cardAmount);
                sqlCommand.Parameters.AddWithValue(nameof(in_chargeOnCard), (object)in_chargeOnCard);
                sqlCommand.Parameters.AddWithValue("@in_percentageCharg", (object)in_percentageCharg);
                sqlCommand.Parameters.AddWithValue(nameof(in_orderby), (object)in_orderby);
                sqlCommand.Parameters.AddWithValue(nameof(in_computerName), (object)in_computerName);
                sqlCommand.Parameters.AddWithValue(nameof(in_ipaddress), (object)in_ipaddress);
                sqlCommand.Parameters.AddWithValue("in_orderStatus", (object)inorderstatus);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
                num2 = 1;
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            string dname = this.getDName(in_orderby, "", out expObj);
            string saveaseIdByName = this.getSaveaseID_By_Name(in_orderby, "", out expObj);
            this.getref();
            double in_debit = in_chargeOnCard;
            this.saveTransactiondetails(in_orderNumber, "Expenses(Buy Voucher)", in_orderby, dname, saveaseIdByName, dname, in_chargeOnCard, in_debit, 0.0, in_orderby, out expObj);
            double in_bal = this.getBal(in_orderby) - in_chargeOnCard;
            this.updateBalance(in_orderby, in_bal, out expObj);
            mailHelper mailHelper = new mailHelper();
            DateTime today = DateTime.Today;
            this.composeMail("SV000333", "Purchase Notification ", " Order Number: " + in_orderNumber, dname, saveaseIdByName, dname, in_orderNumber, out expObj);
            return num2;
        }

        public int saveOrder2(
          string saveaseIDz,
          string inorderstatus,
          string in_cardType,
          double in_cardAmount,
          double in_chargeOnCard,
          double in_percentageCharg,
          string in_orderby,
          string in_computerName,
          string in_ipaddress,
          out string expObj,
          int qty,
          string lblBalhz)
        {
            int num1 = 0;
            int num2;
            try
            {
                string orderNumber = this.getOrderNumber();
                double num3 = 0.0;
                string str1 = "<strong>Order Number: " + orderNumber;
                string str2 = "";
                string str3 = str1 + "<table><tr><td>";
                string str4 = "";
                for (int index = 1; index <= qty; ++index)
                {
                    SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                    SqlCommand sqlCommand = new SqlCommand("insertOrderDetails", connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("in_card_pin", (object)this.genPin());
                    sqlCommand.Parameters.AddWithValue("in_cardpin_sn", (object)this.genSN());
                    sqlCommand.Parameters.AddWithValue("in_orderNumber", (object)orderNumber);
                    sqlCommand.Parameters.AddWithValue(nameof(in_cardType), (object)in_cardType);
                    sqlCommand.Parameters.AddWithValue(nameof(in_cardAmount), (object)in_cardAmount);
                    sqlCommand.Parameters.AddWithValue(nameof(in_chargeOnCard), (object)in_chargeOnCard);
                    sqlCommand.Parameters.AddWithValue("@in_percentageCharg", (object)in_percentageCharg);
                    sqlCommand.Parameters.AddWithValue(nameof(in_orderby), (object)in_orderby);
                    sqlCommand.Parameters.AddWithValue(nameof(in_computerName), (object)in_computerName);
                    sqlCommand.Parameters.AddWithValue(nameof(in_ipaddress), (object)in_ipaddress);
                    sqlCommand.Parameters.AddWithValue("in_orderStatus", (object)inorderstatus);
                    connection.Open();
                    num1 = sqlCommand.ExecuteNonQuery();
                    connection.Close();
                    num3 += in_cardAmount;
                }
                double in_amount = in_chargeOnCard;
                string dname = this.getDName(in_orderby, "", out expObj);
                string saveaseIdByName = this.getSaveaseID_By_Name(in_orderby, "", out expObj);
                string str5 = this.getref();
                double num4 = in_amount;
                double num5 = double.Parse(lblBalhz);
                if (!(saveaseIDz == saveaseIdByName))
                    return 0;
                this.saveTransactiondetails(orderNumber, "Expenses(Buy Voucher)", saveaseIdByName, dname, saveaseIdByName, dname, in_amount, num4 + 0.0, 0.0, in_orderby, out expObj);
                double num6 = num5;
                double num7 = num3;
                string str6 = str3 + str4 + str2 + "</td></tr></table>";
                double num8 = num7;
                double in_bal = double.Parse((num6 - num8).ToString());
                this.updateBalance(in_orderby, in_bal, out expObj);
                mailHelper mailHelper = new mailHelper();
                DateTime today = DateTime.Today;
                string balz = this.getBalz(in_orderby);
                string strTransDate = today.ToString();
                string transref = str5;
                string tablez = str6;
               // string msgBody = mailHelper.populatBuyPin(strTransDate, transref, tablez);
                this.composeMail("SV000333", "Purchase Notification ", " Order Number: " + orderNumber, dname, saveaseIdByName, dname, orderNumber, out expObj);
                double num9 = num3 - num7;
                string msgSubj = "Voucher purchase of N" + (object)num9 + "<br/>";
               // this.mailMessenger.sendMail2Usr(balz, msgBody, msgSubj).ConfigureAwait(true);
                expObj = "";
                this.completedorders(saveaseIDz, orderNumber, num9.ToString(), "");
                num2 = 1;
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            return num2;
        }

        public int saveOrder3(
          string saveaseIDz,
          string inorderstatus,
          string in_cardType,
          double in_cardAmount,
          double in_chargeOnCard,
          double in_percentageCharg,
          string in_orderby,
          string in_computerName,
          string in_ipaddress,
          out string expObj,
          int qty,
          string lblBalhz)
        {
            int num1 = 0;
            int num2;
            try
            {
                string orderNumber = this.getOrderNumber();
                double num3 = 0.0;
                string str1 = "<strong>Order Number: " + orderNumber;
                string str2 = "";
                string str3 = str1 + "<table><tr><td>";
                string str4 = "";
                for (int index = 1; index <= qty; ++index)
                {
                    SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                    SqlCommand sqlCommand = new SqlCommand("insertOrderDetails", connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("in_card_pin", (object)this.genPin());
                    sqlCommand.Parameters.AddWithValue("in_cardpin_sn", (object)this.genSN());
                    sqlCommand.Parameters.AddWithValue("in_orderNumber", (object)orderNumber);
                    sqlCommand.Parameters.AddWithValue(nameof(in_cardType), (object)in_cardType);
                    sqlCommand.Parameters.AddWithValue(nameof(in_cardAmount), (object)in_cardAmount);
                    sqlCommand.Parameters.AddWithValue(nameof(in_chargeOnCard), (object)in_chargeOnCard);
                    sqlCommand.Parameters.AddWithValue("@in_percentageCharg", (object)in_percentageCharg);
                    sqlCommand.Parameters.AddWithValue(nameof(in_orderby), (object)in_orderby);
                    sqlCommand.Parameters.AddWithValue(nameof(in_computerName), (object)in_computerName);
                    sqlCommand.Parameters.AddWithValue(nameof(in_ipaddress), (object)in_ipaddress);
                    sqlCommand.Parameters.AddWithValue("in_orderStatus", (object)inorderstatus);
                    connection.Open();
                    num1 = sqlCommand.ExecuteNonQuery();
                    connection.Close();
                    num3 += num3;
                }
                double in_amount = in_chargeOnCard;
                string dname = this.getDName(in_orderby, "", out expObj);
                string saveaseIdByName = this.getSaveaseID_By_Name(in_orderby, "", out expObj);
                string str5 = this.getref();
                double in_debit = in_amount;
                double num4 = double.Parse(lblBalhz);
                if (!(saveaseIDz == saveaseIdByName))
                    return 0;
                this.saveTransactiondetails(orderNumber, "Expenses(Buy Voucher)", saveaseIdByName, dname, saveaseIdByName, dname, in_amount, in_debit, 0.0, saveaseIdByName, out expObj);
                double num5 = num4;
                double num6 = num3;
                string str6 = str3 + str4 + str2 + "</td></tr></table>";
                double num7 = num6;
                double in_bal = num5 - num7;
                this.updateBalance(in_orderby, in_bal, out expObj);
                mailHelper mailHelper = new mailHelper();
                DateTime today = DateTime.Today;
                string balz = this.getBalz(in_orderby);
                string strTransDate = today.ToString();
                string transref = str5;
                string tablez = str6;
               // string msgBody = mailHelper.populatBuyPin(strTransDate, transref, tablez);
                this.composeMail("SV000333", "Purchase Notification ", " Order Number: " + orderNumber, dname, saveaseIdByName, dname, orderNumber, out expObj);
                string msgSubj = "Voucher purchase of N" + (object)num3 + "<br/>";
               // this.mailMessenger.sendMail2Usr(balz, msgBody, msgSubj);
                expObj = "";
                num2 = 1;
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            return num2;
        }

        public string getBalz(string usname)
        {
            foreach (DataRow row in (InternalDataCollectionBase)this.getBalance(usname, "", out string _).Rows)
            {
                try
                {
                    return row["saveaseID"].ToString();
                }
                catch (Exception ex)
                {
                }
            }
            return this.ToString();
        }

        public string getOrderNumber() => new Random().Next(100000, 999999).ToString() + DateTime.Now.ToString("d/M/yyyy").Replace("/", "").Trim();

        public DataTable getBalance(
          string inputParam,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            int num = inputParam.All<char>(new Func<char, bool>(char.IsDigit)) ? 1 : 0;
            if (num != 0 && inputParam.Length > 10)
                inputParam = inputParam.Substring(inputParam.Length - Math.Min(10, inputParam.Length));
            string cmdText = num != 0 ? "getBalance2" : nameof(getBalance);
            string parameterName = num != 0 ? "@in_account" : "@in_uname";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue(parameterName, (object)inputParam);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public string getBalz(string inputParam, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            int num = inputParam.All<char>(new Func<char, bool>(char.IsDigit)) ? 1 : 0;
            if (num != 0 && inputParam.Length > 10)
                inputParam = inputParam.Substring(inputParam.Length - Math.Min(10, inputParam.Length));
            string cmdText = num != 0 ? "getBalance2" : "getBalance";
            string parameterName = num != 0 ? "@in_account" : "@in_uname";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue(parameterName, (object)inputParam);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? "0" : dataTable.Rows[0]["balance"].ToString();
        }

        public DataTable getBVN(string saveaseid, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getBVN);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)saveaseid);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public DataTable getBalance2(
          string inputParam,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getBalance2);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_account", (object)inputParam);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public int insertRebatch(
          string in_newOrderNumber,
          string in_newCardPin,
          string in_newSerialNumber,
          string in_oldOrderNumber,
          string in_oldCardPin,
          string in_oldSerialNumber,
          double in_amount,
          string in_orderby,
          string in_computerName,
          string in_ipaddress,
          out string expObj)
        {
            int num;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand(nameof(insertRebatch), connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_newOrderNumber), (object)in_newOrderNumber);
                sqlCommand.Parameters.AddWithValue(nameof(in_newCardPin), (object)in_newCardPin);
                sqlCommand.Parameters.AddWithValue(nameof(in_newSerialNumber), (object)in_newSerialNumber);
                sqlCommand.Parameters.AddWithValue(nameof(in_oldOrderNumber), (object)in_oldOrderNumber);
                sqlCommand.Parameters.AddWithValue(nameof(in_oldCardPin), (object)in_oldCardPin);
                sqlCommand.Parameters.AddWithValue(nameof(in_oldSerialNumber), (object)in_oldSerialNumber);
                sqlCommand.Parameters.AddWithValue(nameof(in_amount), (object)in_amount);
                sqlCommand.Parameters.AddWithValue("in_rebatchBy", (object)in_orderby);
                sqlCommand.Parameters.AddWithValue(nameof(in_computerName), (object)in_computerName);
                sqlCommand.Parameters.AddWithValue(nameof(in_ipaddress), (object)in_ipaddress);
                connection.Open();
                num = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
            }
            catch (Exception ex)
            {
                num = 0;
                expObj = ex.Message;
            }
            return num;
        }

        public string genSN()
        {
            Random random = new Random();
            string str = "";
            for (int index = 0; index < 2; ++index)
            {
                int num = random.Next(10000000, 99999999);
                str += (string)(object)num;
            }
            return str;
        }

        public string genPin()
        {
            Random random = new Random();
            string str1 = "";
            for (int index = 0; index < 2; ++index)
            {
                int num = random.Next(100000, 999999);
                string str2;
                str1 = (str2 = str1 + (object)num) + (object)num;
            }
            return str1;
        }

        public string genSaveaseID()
        {
            Random random = new Random();
            string str1 = "";
            for (int index = 0; index < 2; ++index)
            {
                int num = random.Next(1000, 9999);
                string str2;
                str1 = str2 = str1 + (object)num;
            }
            return str1;
        }

        public DataTable getAcct(
          string inputParam,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getAcct);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)inputParam);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public DataTable getEmail(
          string inputParam,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getEmail);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)inputParam);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public DataTable getBeneficiary(
          string inputParam,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getbeneficiary";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)inputParam);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public DataTable getBeneficiaryBank(
          string inputParam,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getbeneficiarybank";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)inputParam);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public int existBeneficiary(
          string in_saveaseID,
          string in_savedFor,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "existBenificiary";
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_saveaseID", (object)in_saveaseID);
                        selectCommand.Parameters.AddWithValue("@in_savedFor", (object)in_savedFor);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = int.Parse(dataTable.Rows[0]["counts"].ToString());
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public int existSaveaseId(string uname, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "existAccountNo2";
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_accountNo", (object)uname);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = int.Parse(dataTable.Rows[0]["counts"].ToString());
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public int saveBeneficiary(
          string in_saveaseID,
          string savedFor,
          string bankname,
          out string expObj)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("insertSaveaseBeneficiary", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_saveaseID", (object)in_saveaseID);
                sqlCommand.Parameters.AddWithValue("@savedFor", (object)savedFor);
                sqlCommand.Parameters.AddWithValue("@BankName", (object)bankname);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                num2 = 1;
                expObj = "";
                connection.Close();
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            return num2;
        }

        public int saveBenficiary2(
          string in_uname,
          string savedFor,
          string bankname,
          out string expObj)
        {
            int num;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("insertSaveaseBeneficiary2", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_uname), (object)in_uname);
                sqlCommand.Parameters.AddWithValue(nameof(savedFor), (object)savedFor);
                connection.Open();
                num = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
            }
            catch (Exception ex)
            {
                num = 0;
                expObj = ex.Message;
            }
            return num;
        }

        public int transferFund(
          float in_balB4Transfer,
          double amountTransfered,
          double balance,
          string beneficiaryAccount,
          string beneficaryName,
          string accountNo,
          string transferedBy,
          string in_computerName,
          string in_transferStatus,
          out string expObj)
        {
            int num;
            try
            {
                string str = "";
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand(nameof(transferFund), connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("in_transferCode", (object)str);
                sqlCommand.Parameters.AddWithValue(nameof(amountTransfered), (object)amountTransfered);
                sqlCommand.Parameters.AddWithValue(nameof(balance), (object)balance);
                sqlCommand.Parameters.AddWithValue(nameof(beneficiaryAccount), (object)beneficiaryAccount);
                sqlCommand.Parameters.AddWithValue(nameof(beneficaryName), (object)beneficaryName);
                sqlCommand.Parameters.AddWithValue(nameof(accountNo), (object)accountNo);
                sqlCommand.Parameters.AddWithValue(nameof(transferedBy), (object)transferedBy);
                sqlCommand.Parameters.AddWithValue(nameof(in_computerName), (object)in_computerName);
                sqlCommand.Parameters.AddWithValue(nameof(in_transferStatus), (object)in_transferStatus);
                connection.Open();
                num = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
            }
            catch (Exception ex)
            {
                num = 0;
                expObj = ex.Message;
            }
            double in_bal1 = (double)in_balB4Transfer - amountTransfered;
            if (num > 0)
            {
                this.updateBalance(transferedBy, in_bal1, out expObj);
                string userNam = this.getUserNam(beneficiaryAccount, "", out expObj);
                double in_bal2 = this.getBal4Update(userNam, out string _) + amountTransfered;
                this.updateBalance(userNam, in_bal2, out expObj);
            }
            return num;
        }

        public DataTable getsearCardPin(
          string pin,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getSearchPinDetails";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_pin", (object)pin);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public DataTable getCardPin(string pin, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getPinDetails";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_pin", (object)pin);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public int usedPin(string pin, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "UsedPin";
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_pin", (object)pin);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = int.Parse(dataTable.Rows[0]["counts"].ToString());
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public DataTable getAcct2(
          string saveaseid,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getAcct2);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_saveaseID", (object)saveaseid);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public DataTable getName(string saveaseid, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getName_By_SaveaseID";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_saveaseID", (object)saveaseid);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public string getUserNam(string saveaseid, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getUserName";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_saveaseID", (object)saveaseid);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? "" : dataTable.Rows[0]["username"].ToString();
        }

        public DataTable getAllBankNames(string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getAllBanks";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public int existBeneficiary(
          string in_acctNo,
          string in_acctOwner,
          string in_bankName,
          string in_sender,
          string connLocation,
          out string expMessage)
        {
            int num = 0;
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = this.constring.connstring(connLocation);
                connection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(new SqlCommand("select SELECT count_big(*) AS counts FROM beneficiaryBank where acctNo ='" + in_acctNo + "'and acctOwner='" + in_acctOwner + "' and bankName='" + in_bankName + "' and sender='" + in_sender + "'", connection));
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = dataTable1;
                sqlDataAdapter.Fill(dataTable2);
                num = dataTable1.Rows.Count <= 0 ? 0 : dataTable1.Rows.Count;
                expMessage = "";
                connection.Close();
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public DataTable getBenBySenders(
          string in_uname,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getbeneficiary2";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)in_uname);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public DataTable getBenBySenders1(
          string in_uname,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getbeneficiary4";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)in_uname);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public DataTable getTransMessage(
          string in_uname,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getTransMessage);
            string saveaseIdByName = this.getSaveaseID_By_Name(in_uname, connLocation, out expMessage);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)saveaseIdByName);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public DataTable getBankdetails(
          string in_uname,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getBankd";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)in_uname);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public DataTable getSavedBenDeatials(
          string in_saveaseID,
          string in_sender,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getBenByAcctNo1";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_acctNo", (object)in_saveaseID);
                        selectCommand.Parameters.AddWithValue("@in_sender", (object)in_sender);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public string getCommision(string in_chargeFor, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string str = "";
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getCommision);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_chargeFor", (object)in_chargeFor);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            if (dataTable == null)
            {
                str = "";
            }
            else
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                    str = row["charge"].ToString();
            }
            return str;
        }

        public int saveCommision(
          string in_chargeType,
          double in_charge,
          string in_chargeFor,
          out string expObj)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("insertCommision", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_chargeType), (object)in_chargeType);
                sqlCommand.Parameters.AddWithValue(nameof(in_charge), (object)in_charge);
                sqlCommand.Parameters.AddWithValue(nameof(in_chargeFor), (object)in_chargeFor);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
                num2 = 1;
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            return num2;
        }

        public int updateCommision(double in_charge, string in_chargefor, out string expObj)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("Updpercentage", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_charge), (object)in_charge);
                sqlCommand.Parameters.AddWithValue(nameof(in_chargefor), (object)in_chargefor);
                connection.Open();
                num2 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
            }
            catch (Exception ex)
            {
                num1 = 0;
                expObj = ex.Message;
                num2 = 1;
            }
            return num2;
        }

        public int saveTransactiondetails(
          string in_tranRef,
          string in_transacType,
          string in_acctNo,
          string in_acctName,
          string in_depNum,
          string in_depositor,
          double in_amount,
          double in_debit,
          double in_credit,
          string in_uname,
          out string expObj)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("insertTransactiondetails", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_acctNo", (object)in_acctNo);
                sqlCommand.Parameters.AddWithValue("@in_acctName", (object)in_acctName);
                sqlCommand.Parameters.AddWithValue("@in_amount", (object)in_amount);
                sqlCommand.Parameters.AddWithValue("@in_depositor", (object)in_depositor);
                sqlCommand.Parameters.AddWithValue("@in_depNum", (object)in_depNum);
                sqlCommand.Parameters.AddWithValue("@in_tranRef", (object)in_tranRef);
                sqlCommand.Parameters.AddWithValue("@in_transacType", (object)in_transacType);
                sqlCommand.Parameters.AddWithValue("@in_credit", (object)in_credit);
                sqlCommand.Parameters.AddWithValue("@in_debit", (object)in_debit);
                sqlCommand.Parameters.AddWithValue("@in_uname", (object)in_uname);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
                num2 = 1;
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            return num2;
        }

        public double getBal4Update(string userName, out string saveid)
        {
            DataTable balance = this.getBalance(userName, "", out string _);
            string s = "0";
            saveid = "";
            foreach (DataRow row in (InternalDataCollectionBase)balance.Rows)
            {
                saveid = row["saveaseID"].ToString();
                row["fname"].ToString();
                row["lname"].ToString();
                s = row["balance"].ToString();
            }
            return double.Parse(s);
        }

        public DataTable SearchTransaction(
          string saveaseid,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(SearchTransaction);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_transref", (object)saveaseid);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public DataTable getAlluserbyid(
          string saveaseid,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "searchusers";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_accountno", (object)saveaseid);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public DataTable searchAdmin(string dname, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "searchAllAdm";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_admin", (object)dname);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public DataTable getAllAdmin(string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getAllAdm";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public DataTable getAlluser(string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getAllusers";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable.Rows.Count < 1 ? (DataTable)null : dataTable;
        }

        public static DateTime GetDateTime()
        {
            DateTime dateTime = DateTime.Now;
            //HttpWebRequest httpWebRequest = (httpWebRequest)WebRequest.Create("http://www.google.com.ng");
            //httpWebRequest.Method = "GET";
            //httpWebRequest.Accept = "text/html, application/xhtml+xml, */*";
            //httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
            //httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            //httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            //HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            //if (response.StatusCode == HttpStatusCode.OK)
            //    dateTime = DateTime.ParseExact(response.Headers["date"], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", (IFormatProvider)CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
            return dateTime;
        }

        public string[] saveDeposit(
          string in_acctNo,
          string in_cardpin,
          string in_cardsn,
          string in_bankName,
          string in_acctName,
          double in_amount,
          string in_depositor,
          string in_computerName,
          string in_ipaddress,
          string in_tranRef,
          out string expObj,
          string in_naration)
        {
            string str1 = Convert.ToString(in_tranRef);
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("insertDeposit", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_acctNo), (object)in_acctNo);
                sqlCommand.Parameters.AddWithValue(nameof(in_cardpin), (object)in_cardpin);
                sqlCommand.Parameters.AddWithValue(nameof(in_cardsn), (object)in_cardsn);
                sqlCommand.Parameters.AddWithValue(nameof(in_bankName), (object)in_bankName);
                sqlCommand.Parameters.AddWithValue(nameof(in_acctName), (object)in_acctName);
                sqlCommand.Parameters.AddWithValue(nameof(in_amount), (object)in_amount);
                sqlCommand.Parameters.AddWithValue(nameof(in_depositor), (object)in_depositor);
                sqlCommand.Parameters.AddWithValue(nameof(in_computerName), (object)in_computerName);
                sqlCommand.Parameters.AddWithValue(nameof(in_ipaddress), (object)in_ipaddress);
                sqlCommand.Parameters.AddWithValue(nameof(in_tranRef), (object)in_tranRef);
                sqlCommand.Parameters.AddWithValue(nameof(in_naration), (object)in_naration);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
                num2 = 1;
                this.transMessage(in_tranRef, "Credit", "Hi " + in_depositor + ", Your deposit with ref: " + in_tranRef + " of " + in_amount.ToString() + " was successful. (" + (object)DateTime.Today + "). Thank you for using savease", in_acctNo, in_acctNo, float.Parse(in_amount.ToString()), out expObj);
                string str2 = "";
                string connLocation = "";
                double in_credit = in_amount;
                double in_debit = 0.0;
                string userNam1 = this.getUserNam(in_acctNo, connLocation, out str2);
                string saveaseIdByName = this.getSaveaseID_By_Name(userNam1, connLocation, out str2);
                if (in_acctNo == saveaseIdByName)
                    this.saveTransactiondetails(in_tranRef, "Income(Self Deposit)", in_acctNo, in_acctName, saveaseIdByName, in_depositor, in_amount, in_debit, in_credit, userNam1, out expObj);
                else if (in_acctNo != saveaseIdByName)
                {
                    string userNam2 = this.getUserNam(in_acctNo, connLocation, out str2);
                    this.saveTransactiondetails(in_tranRef, "Income(ThirdParty Deposit)", in_acctNo, in_acctName, saveaseIdByName, in_depositor, in_amount, in_debit, in_credit, userNam2, out expObj);
                }
                string str3 = procedurs.GetDateTime().ToString();
                string expMessage = "";
                string userNam3 = this.getUserNam(in_acctNo, connLocation, out expMessage);
                double in_bal = this.getBal(userNam3) + in_amount;
                this.updateBalance(userNam3, in_bal, out expObj);
                double bal = this.getBal(userNam3);
                this.updateCardpin(in_cardpin, in_acctName, str3, out expObj);
                this.SystemLog(saveaseIdByName, "Deposit", in_computerName, in_ipaddress, "Deposit.aspx", out str2);
                string strNarative = "";
                string msgBody = this.formatedSMS(in_acctName, in_bankName, in_depositor, in_acctNo, in_amount, str3, strNarative, bal.ToString());
                string msgTo = "";
                foreach (DataRow row in (InternalDataCollectionBase)this.getEmail(userNam3, connLocation, out str2).Rows)
                    msgTo = row["Email"].ToString();
                string msgSubj1 = "Deposit of N" + (object)in_amount + " to " + in_acctName + "<br/>";
                string msgSubj2 = "Deposit of N" + (object)in_amount + " to " + in_acctName + "<br/>";
               // this.mailMessenger.sendMail2Usr(msgTo, msgBody, msgSubj1).ConfigureAwait(true);
                //this.mailMessenger.sendMail2Usr(msgTo, msgBody, msgSubj2).ConfigureAwait(true);
                this.composeMail("SV000111", "Deposit Notificatio", " ", in_depositor, in_acctNo, in_depositor, "", out str2);
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
                this.transMessage(in_tranRef, "Faild Credit", "Hi " + in_depositor + ", Your deposit with ref: " + in_tranRef + " of " + in_amount.ToString() + " has failed. (" + (object)DateTime.Today + "). Thank you for using savease", in_acctNo, in_acctNo, float.Parse(in_amount.ToString()), out expObj);
            }
            return new string[2] { num2.ToString(), str1 };
        }

        public string formatedSMS(
          string strName,
          string strBank,
          string strAcctOwner,
          string strAcctNum,
          double transAmount,
          string tranDate,
          string strNarative,
          string strBalance)
        {
            return "<img ImageUrl='~/images/logo/logo.png' runat='server' Height='18px' Width='152px' /> < br />  < br /> &nbsp; Hello " + strName + "<br /> &nbsp;< br /> &nbsp;< table cellpadding = '5' class='auto-style1'> <tr> <td class='auto-style2'>Beneficiary Bank:</td> <td>&nbsp;</td> </tr><tr> <td class='auto-style2'>Beneficiary Name:</td> <td>&nbsp;" + strAcctOwner + "</td> </tr> <tr> <td class='auto-style2'>Beneficiary Bank:</td> <td>&nbsp;" + strBank + "</td> </tr> <tr> <td class='auto-style2'>Beneficiary Account:</td> <td>&nbsp;" + strAcctNum + " </td> </tr>  <tr> <td class='auto-style2'>Transaction Amount:</td><td>&nbsp;" + (object)transAmount + " </td> </tr> <tr><td class='auto-style2'>Transaction Date:</td><td>&nbsp;" + tranDate + "</td>  </tr> <tr> <td class='auto-style2'>Narative:</td> <td>&nbsp;" + strNarative + "</td> </tr> <tr> <td class='auto-style3'>Balance:</td> <td><b>" + strBalance + "</b></td></tr> </table><b><i><u><span style = 'font-size: 10.0pt; line-height: 115%; font-family: &quot;Arial&quot;,sans-serif; mso-fareast-font-family: &quot;Times New Roman&quot;; color: #003399; background: white; mso-ansi-language: EN-US; mso-fareast-language: EN-US; mso-bidi-language: AR-SA' > Please click here to provide feedback on your experience<br /> </span></u></i></b> <br />  <br /> <span style = 'font-size:10.0pt;line-height:115%; font-family:&quot; Arial&quot;,sans-serif;mso-fareast-font-family:&quot;Times New Roman&quot;; color:#003399;mso-ansi-language:EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'>Thank you for using Savease Nigeria</span></div>";
        }

        public double getBal(string userName)
        {
            string s = "";
            foreach (DataRow row in (InternalDataCollectionBase)this.getBalance(userName, "", out string _).Rows)
            {
                try
                {
                    row["saveaseID"].ToString();
                    row["fname"].ToString();
                    row["lname"].ToString();
                    s = row["balance"].ToString();
                    row["email"].ToString();
                    row["phone"].ToString();
                }
                catch (Exception ex)
                {
                    s = "-1";
                }
            }
            return double.Parse(s);
        }

        public int saveSelfDeposit(
          string in_acctNo,
          string in_cardpin,
          string in_cardsn,
          string in_bankName,
          string in_acctName,
          double in_amount,
          string in_depositor,
          string in_computerName,
          string in_ipaddress,
          string in_tranRef,
          out string expObj,
          string in_naration)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("insertDeposit", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_acctNo), (object)in_acctNo);
                sqlCommand.Parameters.AddWithValue(nameof(in_cardpin), (object)in_cardpin);
                sqlCommand.Parameters.AddWithValue(nameof(in_cardsn), (object)in_cardsn);
                sqlCommand.Parameters.AddWithValue(nameof(in_bankName), (object)in_bankName);
                sqlCommand.Parameters.AddWithValue(nameof(in_acctName), (object)in_acctName);
                sqlCommand.Parameters.AddWithValue(nameof(in_amount), (object)in_amount);
                sqlCommand.Parameters.AddWithValue(nameof(in_depositor), (object)in_depositor);
                sqlCommand.Parameters.AddWithValue(nameof(in_computerName), (object)in_computerName);
                sqlCommand.Parameters.AddWithValue(nameof(in_ipaddress), (object)in_ipaddress);
                sqlCommand.Parameters.AddWithValue(nameof(in_tranRef), (object)in_tranRef);
                sqlCommand.Parameters.AddWithValue(nameof(in_naration), (object)in_naration);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
                num2 = 1;
                this.transMessage(in_tranRef, "Credit", "Hi " + in_depositor + ", Your deposit with ref: " + in_tranRef + " of " + in_amount.ToString() + " was successful. (" + (object)DateTime.Today + "). Thank you for using savease", in_acctNo, in_acctNo, float.Parse(in_amount.ToString()), out expObj);
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
                this.transMessage(in_tranRef, "Faild Credit", "Hi " + in_depositor + ", Your deposit with ref: " + in_tranRef + " of " + in_amount.ToString() + " has failed. (" + (object)DateTime.Today + "). Thank you for using savease", in_acctNo, in_acctNo, float.Parse(in_amount.ToString()), out expObj);
            }
            return num2;
        }

        public string[] transferFund(
          string in_transferCode,
          double in_balB4Transfer,
          double amountTransfered,
          double balance,
          string beneficiaryAccount,
          string beneficaryName,
          string accountNo,
          string transferedBy,
          string in_computerName,
          out string expObj,
          string in_naration,
          string username)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand(nameof(transferFund), connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_transferCode), (object)in_transferCode);
                sqlCommand.Parameters.AddWithValue(nameof(in_balB4Transfer), (object)in_balB4Transfer);
                sqlCommand.Parameters.AddWithValue(nameof(amountTransfered), (object)amountTransfered);
                sqlCommand.Parameters.AddWithValue(nameof(balance), (object)balance);
                sqlCommand.Parameters.AddWithValue(nameof(beneficiaryAccount), (object)beneficiaryAccount);
                sqlCommand.Parameters.AddWithValue(nameof(beneficaryName), (object)beneficaryName);
                sqlCommand.Parameters.AddWithValue(nameof(accountNo), (object)accountNo);
                sqlCommand.Parameters.AddWithValue(nameof(transferedBy), (object)transferedBy);
                sqlCommand.Parameters.AddWithValue(nameof(in_computerName), (object)in_computerName);
                sqlCommand.Parameters.AddWithValue(nameof(in_naration), (object)in_naration);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
                num2 = 1;
                string str1;
                this.getUserNam(accountNo, "", out str1);
                string str2 = "I, " + transferedBy + " initiate the following transaction of " + (object)float.Parse(amountTransfered.ToString()) + " to be debited from account: " + accountNo + " and credited to account number: " + beneficiaryAccount + " on " + (object)DateTime.Today;
                this.transMessage(in_transferCode, "Debit", "Hi " + transferedBy + ", Your transfer with ref: " + in_transferCode + " of " + amountTransfered.ToString() + " to " + beneficaryName + " was successful. (" + (object)DateTime.Today + "). Thank you for using savease", accountNo, beneficiaryAccount, float.Parse(amountTransfered.ToString()), out expObj);
                this.transMessage(in_transferCode, "Credit", "Hi " + beneficaryName + ", transfer with ref: " + in_transferCode + " of " + amountTransfered.ToString() + " to " + beneficaryName + " was successful. (" + (object)DateTime.Today + "). Thank you for using savease", beneficiaryAccount, beneficiaryAccount, float.Parse(amountTransfered.ToString()), out expObj);
                string saveaseIdByName = this.getSaveaseID_By_Name(username, "", out str1);
                double num3 = (double)float.Parse(amountTransfered.ToString());
                double in_debit = (double)float.Parse(amountTransfered.ToString());
                if (beneficiaryAccount == saveaseIdByName)
                {
                    this.saveTransactiondetails(in_transferCode, "Income(Self Deposit)", beneficiaryAccount, beneficaryName, saveaseIdByName, beneficaryName, num3, in_debit, num3, username, out expObj);
                    this.composeMail("SV000222", "Transfer Notification", str2 + " " + in_transferCode, beneficaryName, beneficiaryAccount, beneficaryName, "", out str1);
                }
                else if (beneficiaryAccount != saveaseIdByName)
                {
                    string connLocation = "";
                    this.saveTransactiondetails(in_transferCode, "Transfer", beneficiaryAccount, beneficaryName, accountNo, transferedBy, num3, in_debit, 0.0, username, out expObj);
                    this.composeMail("SV000222", "Transfer Notification", str2 + " " + in_transferCode, "Savease", beneficiaryAccount, beneficaryName, "", out str1);
                    string saveid = "";
                    double in_bal1 = this.getBal4Update(accountNo, out saveid) - (double)float.Parse(amountTransfered.ToString());
                    this.updateBalance(username, in_bal1, out expObj);
                    string userNam = this.getUserNam(beneficiaryAccount, "", out str1);
                    double in_bal2 = this.getBal4Update(userNam, out saveid) + (double)float.Parse(amountTransfered.ToString());
                    this.updateBalance(userNam, in_bal2, out expObj);
                    this.saveTransactiondetails(in_transferCode, "Transfer(ThirdParty Transfer)", beneficiaryAccount, beneficaryName, saveaseIdByName, transferedBy, num3, 0.0, num3, this.getUserNam(beneficiaryAccount, connLocation, out expObj), out expObj);
                    this.composeMail("SV000222", "Credit Alert", str2 + " " + in_transferCode, transferedBy, beneficiaryAccount, beneficaryName, "", out str1);
                }
            }
            catch (Exception ex)
            {
                num2 = 0;
                this.transMessage(in_transferCode, "Failed Debit", "Hi " + transferedBy + ", Your transfer with ref: " + in_transferCode + " of " + amountTransfered.ToString() + " to " + beneficaryName + " failed. (" + (object)DateTime.Today + "). Thank you for using savease", accountNo, beneficiaryAccount, float.Parse(amountTransfered.ToString()), out expObj);
                expObj = ex.Message;
            }
            return new string[2]
            {
        num2.ToString(),
        in_transferCode
            };
        }

        public int inserttransactionLog(
          string in_referenceCode,
          string in_TransactionType,
          string in_TransactionDescription,
          string in_recipientID,
          string in_recipientName,
          string in_senderID,
          string in_senderName,
          float in_amt,
          out string expObj)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand(nameof(inserttransactionLog), connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_referenceCode), (object)in_referenceCode);
                sqlCommand.Parameters.AddWithValue(nameof(in_TransactionType), (object)in_TransactionType);
                sqlCommand.Parameters.AddWithValue(nameof(in_TransactionDescription), (object)in_TransactionDescription);
                sqlCommand.Parameters.AddWithValue(nameof(in_recipientID), (object)in_recipientID);
                sqlCommand.Parameters.AddWithValue(nameof(in_recipientName), (object)in_recipientName);
                sqlCommand.Parameters.AddWithValue(nameof(in_senderID), (object)in_senderID);
                sqlCommand.Parameters.AddWithValue(nameof(in_senderName), (object)in_senderName);
                sqlCommand.Parameters.AddWithValue("in_amount", (object)in_amt);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
                num2 = 1;
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            return num2;
        }

        public int transMessage(
          string in_referenceCode,
          string in_TransactionType,
          string in_TransactionMessage,
          string in_senderID,
          string in_recipientID,
          float in_amount,
          out string expObj)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("inserttransMessage", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_referenceCode), (object)in_referenceCode);
                sqlCommand.Parameters.AddWithValue(nameof(in_TransactionType), (object)in_TransactionType);
                sqlCommand.Parameters.AddWithValue(nameof(in_TransactionMessage), (object)in_TransactionMessage);
                sqlCommand.Parameters.AddWithValue(nameof(in_senderID), (object)in_senderID);
                sqlCommand.Parameters.AddWithValue(nameof(in_recipientID), (object)in_recipientID);
                sqlCommand.Parameters.AddWithValue(nameof(in_amount), (object)in_amount);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
                num2 = 1;
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            return num2;
        }

        public int composeMail(
          string in_sender,
          string in_subject,
          string in_content,
          string in_senderName,
          string in_reciverId,
          string in_reciverName,
          string in_orderNumber,
          out string expObj)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("insertMailbox", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_senderid", (object)in_sender);
                sqlCommand.Parameters.AddWithValue("@in_subject ", (object)in_subject);
                sqlCommand.Parameters.AddWithValue("@in_content", (object)in_content);
                sqlCommand.Parameters.AddWithValue("@in_senderName", (object)in_senderName);
                sqlCommand.Parameters.AddWithValue("@in_reciverId", (object)in_reciverId);
                sqlCommand.Parameters.AddWithValue("@in_reciverName", (object)in_reciverName);
                sqlCommand.Parameters.AddWithValue("@in_orderNumber", (object)in_orderNumber);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
                num2 = 1;
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            return num2;
        }

        public int InsertBankdetails(
          string in_acctName,
          string in_acctNo,
          string in_bankName,
          string saveaseId,
          string bankcode,
          out string expObj)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("insertBankdetails", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_accntName", (object)in_acctName);
                sqlCommand.Parameters.AddWithValue("@in_accntnum", (object)in_acctNo);
                sqlCommand.Parameters.AddWithValue("@in_bankname", (object)in_bankName);
                sqlCommand.Parameters.AddWithValue("@in_uname", (object)saveaseId);
                sqlCommand.Parameters.AddWithValue("@in_bankcode", (object)bankcode);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
                num2 = 1;
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            return num2;
        }

        public int DeleteUser(string in_saveaseid, string uname, out string expObj)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("DeleteUserDetsails", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_uname", (object)uname);
                sqlCommand.Parameters.AddWithValue("@in_saveaseid", (object)in_saveaseid);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
                num2 = 1;
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            return num2;
        }

        public int DeleteAdmin(string in_acctNo, out string expObj)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("DleteAdm", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_admin", (object)in_acctNo);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
                num2 = 1;
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            return num2;
        }

        public int DeleteBankdetails(string in_acctNo, out string expObj)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("DeleteBankDetails", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_uname", (object)in_acctNo);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
                num2 = 1;
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            return num2;
        }

        public int saveBenDetails(
          string in_acctNo,
          string in_acctName,
          string in_bankName,
          string in_depositor,
          string in_cardpin,
          string in_phoneNumber,
          string in_emailID,
          out string expObj)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("insertBankBeneficiary", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_acctNo), (object)in_acctNo);
                sqlCommand.Parameters.AddWithValue(nameof(in_acctName), (object)in_acctName);
                sqlCommand.Parameters.AddWithValue(nameof(in_bankName), (object)in_bankName);
                sqlCommand.Parameters.AddWithValue(nameof(in_depositor), (object)in_depositor);
                sqlCommand.Parameters.AddWithValue(nameof(in_cardpin), (object)in_cardpin);
                sqlCommand.Parameters.AddWithValue(nameof(in_phoneNumber), (object)in_phoneNumber);
                sqlCommand.Parameters.AddWithValue(nameof(in_emailID), (object)in_emailID);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
                num2 = 1;
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            return num2;
        }

        public int existObjectsy(
          string tblName,
          string searchObj,
          string searchObjVal,
          string lastSerch,
          string connLocation,
          out string expMessage)
        {
            int num = 0;
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = this.constring.connstring(connLocation);
                connection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(new SqlCommand(" SELECT count_big(*) AS counts FROM " + tblName + " where " + searchObj + " ='" + searchObjVal + "' " + lastSerch, connection));
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = dataTable1;
                sqlDataAdapter.Fill(dataTable2);
                num = dataTable1.Rows.Count <= 0 ? 0 : dataTable1.Rows.Count;
                expMessage = "";
                connection.Close();
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public string getCardAmountByPin(string in_cardpin, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string str = "";
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getCardSerial";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_cardpin", (object)in_cardpin);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                str = row["cardAmount"].ToString();
            return str;
        }

        public string getCardSerial(string in_cardpin, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string str = "";
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getCardSerial);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_cardpin", (object)in_cardpin);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                str = row["cardpin_sn"].ToString();
            return str;
        }

        public string getCardSerialOnly(string in_cardpin, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string str = "";
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getCardSerialOnly);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_cardpin", (object)in_cardpin);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                str = row["cardpin_sn"].ToString();
            return str;
        }

        public int getCardSerialOnly1(string in_cardpin, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            int num = 0;
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getCardSerialOnly";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_cardpin", (object)in_cardpin);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                row["cardpin_sn"].ToString();
                num = int.Parse(row["cardpin_sn"].ToString());
            }
            return num;
        }

        public DataTable getCardSerial2(
          string in_cardpin,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getCardSerial";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_cardpin", (object)in_cardpin);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public int saveBank(string in_bankName, string connLocation, out string expObj)
        {
            int num;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(connLocation));
                SqlCommand sqlCommand = new SqlCommand("insertBank", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_bankName), (object)in_bankName);
                connection.Open();
                num = sqlCommand.ExecuteNonQuery();
                expObj = "Record Inserted Succesfully into the Database";
                connection.Close();
            }
            catch (Exception ex)
            {
                num = 0;
                expObj = "Failed to insert record: " + ex.Message;
            }
            return num;
        }

        public int existBank(string in_bankName, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(existBank);
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_bankName", (object)in_bankName);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = int.Parse(dataTable.Rows[0]["counts"].ToString());
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public int changepass(
          string in_uname,
          string in_newpwd,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(changepass);
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("in_username", (object)in_uname);
                        selectCommand.Parameters.AddWithValue("@in_newpwd", (object)in_newpwd);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = 1;
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public int existEmail(string in_email, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(existEmail);
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_email", (object)in_email);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = int.Parse(dataTable.Rows[0]["counts"].ToString());
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public int existTransPIN(
          string in_username,
          string pin,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(existTransPIN);
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_username", (object)in_username);
                        selectCommand.Parameters.AddWithValue("@in_pin", (object)this.sh1.GetSHA1HashData(pin));
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = int.Parse(dataTable.Rows[0]["counts"].ToString());
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public int gettMailunread(string in_SaveaseID, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(gettMailunread);
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_SaveaseID", (object)in_SaveaseID);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = int.Parse(dataTable.Rows[0]["counts"].ToString());
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public int feedback(
          string in_username,
          string in_refnum,
          string in_descrip,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getfeedback";
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_Username", (object)in_username);
                        selectCommand.Parameters.AddWithValue("@in_refnum", (object)in_refnum);
                        selectCommand.Parameters.AddWithValue("@in_descrip ", (object)in_descrip);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = int.Parse(dataTable.Rows[0]["counts"].ToString());
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public int complains(
          string in_name,
          string in_username,
          string in_comtype,
          string in_complain,
          string in_image,
          string in_refnum,
          string in_status,
          string in_date,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getcomplains";
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_name", (object)in_name);
                        selectCommand.Parameters.AddWithValue("@in_username", (object)in_username);
                        selectCommand.Parameters.AddWithValue("@in_comtype", (object)in_comtype);
                        selectCommand.Parameters.AddWithValue("@in_complain", (object)in_complain);
                        selectCommand.Parameters.AddWithValue("@in_image", (object)in_image);
                        selectCommand.Parameters.AddWithValue("@in_refnum", (object)in_refnum);
                        selectCommand.Parameters.AddWithValue("@in_status ", (object)in_status);
                        selectCommand.Parameters.AddWithValue("@in_date", (object)in_date);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = int.Parse(dataTable.Rows[0]["counts"].ToString());
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        private DataTable GetData(string filterString, string connLocation)
        {
            SqlConnection sqlConnection = new SqlConnection(this.constring.connstring(connLocation));
            SqlCommand selectCommand = new SqlCommand("SELECT [ProductId], [ProductName] FROM [Products] WHERE [ProductName] LIKE '%' + @filterString + '%' ORDER BY [ProductId]");
            selectCommand.Parameters.AddWithValue("@filterString", (object)filterString.Replace("%", "[%]").Replace("_", "[_]"));
            selectCommand.Connection = sqlConnection;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = dataTable1;
            sqlDataAdapter.Fill(dataTable2);
            return dataTable1;
        }

        public DataTable getPinBybatch(
          string in_uname,
          string in_pin,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getPinwithAmount";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)in_uname);
                        selectCommand.Parameters.AddWithValue("@in_pin", (object)in_pin);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public DataTable getPinBybatch1(
          string in_uname,
          string in_pin,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getPinwithAmount1";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)in_uname);
                        selectCommand.Parameters.AddWithValue("@in_pin", (object)in_pin);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public string getPinAmount(
          string in_uname,
          string in_ostatus,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string str = "";
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getOrderAmount";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)in_uname);
                        selectCommand.Parameters.AddWithValue("@in_ostatus", (object)in_ostatus);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                str = dataTable.Rows[0]["Total"].ToString();
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return str;
        }

        public string getTransPin(string in_saveaseID, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string str = "";
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getTransPin);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_saneaseID", (object)in_saveaseID);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                str = dataTable.Rows[0]["transPIN"].ToString();
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return str;
        }

        public DataTable SortPinby(
          string in_uname,
          string in_pin,
          string in_searchby,
          string dateFrom,
          string dateTo,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getPinwithAmount3";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)in_uname);
                        selectCommand.Parameters.AddWithValue("@in_pin", (object)in_pin);
                        selectCommand.Parameters.AddWithValue("@in_searchby", (object)in_searchby);
                        selectCommand.Parameters.AddWithValue("@in_datefrom", (object)dateFrom);
                        selectCommand.Parameters.AddWithValue("@in_dateTo", (object)dateTo);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public string getcardImageName(string connLocation, out string expMessage)
        {
            string str = "";
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getCardImage";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                str = dataTable.Rows[0]["imageName"].ToString();
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return str;
        }

        public DataTable getallVoucher(string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getAllPinDetails";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public DataTable aLLgetTranssaction(string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "adminGetTransaction";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public DataTable getTranssactionDetails(
          string in_bankname,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getTransactionDetails1";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)in_bankname);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public DataTable getTransfers(
          string in_uname,
          string in_transferRef,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getTransactionDetails2";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)in_uname);
                        selectCommand.Parameters.AddWithValue("@in_RefNumber", (object)in_transferRef);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public DataTable getBankDeposit(
          string in_bankname,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getBankDeposit);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_bankname", (object)in_bankname);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public DataTable getBankDeposit4user(
          string in_bankname,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getBankDeposit4user);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_bankname", (object)in_bankname);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public DataTable getBankDepositSelf(
          string in_depositor,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getBankDepositSelf);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_depositor", (object)in_depositor);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public DataTable getOrder(
          string in_depositor,
          string in_orderstatus,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getAllOrderByUser";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)in_depositor);
                        selectCommand.Parameters.AddWithValue("@in_orderstatus", (object)in_orderstatus);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public DataTable getOrderdetails(
          string in_depositor,
          string in_orderstatus,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getAllOrderByUser1";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)in_depositor);
                        selectCommand.Parameters.AddWithValue("@in_orderstatus", (object)in_orderstatus);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public DataTable getTransfer(
          string in_depositor,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getTransferByUser";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)in_depositor);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public string getDName(string in_username, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string str = "";
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getDiplayName";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_username", (object)in_username);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                str = dataTable.Rows[0]["dName"].ToString();
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return str;
        }

        public string getSaveaseID_By_Name(
          string in_username,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string str = "";
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getSaveaseID_By_Name);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_username", (object)in_username);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                str = dataTable.Rows[0]["saveaseID"].ToString();
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return str;
        }

        public string getSaveaseID_By_phone(string in_phone)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring("");
            string cmdText = nameof(getSaveaseID_By_phone);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_username", (object)in_phone);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                return dataTable.Rows[0]["saveaseID"].ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string getUserLevel(string in_username, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string str = "";
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getUserLevel);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_username", (object)in_username);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                str = dataTable.Rows[0]["userlevel"].ToString();
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return str;
        }

        public int updateInboxStatus(string in_notID, char in_statusType, out string expObj)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("updMailbox2", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_notID), (object)in_notID);
                sqlCommand.Parameters.AddWithValue(nameof(in_statusType), (object)in_statusType);
                connection.Open();
                num2 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
            }
            catch (Exception ex)
            {
                num1 = 0;
                expObj = ex.Message;
                num2 = 1;
            }
            return num2;
        }

        public int updateBalance(string inuser, double in_bal, out string expObj)
        {
            string str = in_bal.ToString();
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand(nameof(updateBalance), connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("in_user", (object)inuser);
                sqlCommand.Parameters.AddWithValue(nameof(in_bal), (object)str);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
                num2 = 1;
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            return num2;
        }

        public int updateBVNStatus(string inuser, out string expObj)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand(nameof(updateBVNStatus), connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("in_user", (object)inuser);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
                num2 = 1;
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            return num2;
        }

        public string getRandomAcctOfficer(string connLocation)
        {
            DataTable dataTable = new DataTable();
            string str = "";
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getRandomAcctOfficer);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (dataTable == null)
            {
                str = "0";
            }
            else
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                    str = row["sn"].ToString();
            }
            return str;
        }

        public string getAcctOfficerBySVID(string svid, string connLocation)
        {
            DataTable dataTable = new DataTable();
            string str = "";
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getAcctOfficerBySVID);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_svid", (object)svid);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (dataTable == null)
            {
                str = "0";
            }
            else
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                    str = row["accountofficer"].ToString();
            }
            return str;
        }

        public DataTable getAcctOfficer(
          string svid,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string acctOfficerBySvid = this.getAcctOfficerBySVID(svid, connLocation);
            string cmdText = nameof(getAcctOfficer);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_sn", (object)acctOfficerBySvid);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public DataTable getAllAcctOfficerByID(
          string sn,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getAcctOfficer";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_sn", (object)sn);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public int updateStatus(string in_token, out string expObj)
        {
            int num;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand(nameof(updateStatus), connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_token), (object)in_token);
                connection.Open();
                num = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
            }
            catch (Exception ex)
            {
                num = 0;
                expObj = ex.Message;
            }
            return num;
        }

        public int updateCardpin(string in_cardpin, string usedby, string date, out string expObj)
        {
            int num;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("updateCardpinstatus", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_cardpin), (object)in_cardpin);
                sqlCommand.Parameters.AddWithValue("@in_usedby", (object)usedby);
                sqlCommand.Parameters.AddWithValue("@in_date", (object)date);
                connection.Open();
                num = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
            }
            catch (Exception ex)
            {
                num = 0;
                expObj = ex.Message;
            }
            return num;
        }

        public DataTable getMailInbox(
          string in_saveaseID,
          char statustype,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getMailBoxInbox";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_saveaseID", (object)in_saveaseID);
                        selectCommand.Parameters.AddWithValue("@in_statusType", (object)statustype);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public DataTable getMailSent(
          string in_saveaseID,
          char statustype,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getMailBoxsent";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_saveaseID", (object)in_saveaseID);
                        selectCommand.Parameters.AddWithValue("@in_statusType", (object)statustype);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public DataTable getReadMailInbox1(
          string in_saveaseID,
          char statustype,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getReadMailBox";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_saveaseID", (object)in_saveaseID);
                        selectCommand.Parameters.AddWithValue("@in_statusType", (object)statustype);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public void getMailCounters(
          string in_saveaseID,
          out string ctrInbox,
          out string ctrUnread,
          out string ctrRead,
          out string ctrSent,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "CtrMailboxByUser";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_saveaseID", (object)in_saveaseID);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                ctrInbox = dataTable.Rows[0]["countInbox"].ToString();
                ctrUnread = dataTable.Rows[0]["countUnRead"].ToString();
                ctrRead = dataTable.Rows[0]["countRead"].ToString();
                ctrSent = dataTable.Rows[0]["countSent"].ToString();
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                ctrInbox = "0";
                ctrUnread = "0";
                ctrRead = "0";
                ctrSent = "0";
            }
        }

        public DataTable getMailByID4user(
          string notID,
          string in_saveaseID,
          char statustype,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getMailByID4user);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_notID", (object)notID);
                        selectCommand.Parameters.AddWithValue("@in_saveaseID", (object)in_saveaseID);
                        selectCommand.Parameters.AddWithValue("@in_statusType", (object)statustype);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public int LoginLog(
          string in_saveaseID,
          string in_loggedin,
          string in_loggedout,
          string in_status,
          string in_computerName,
          string in_ipaddress,
          string in_createupdate,
          out string expObj)
        {
            int num;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("insertLoggedIn", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_saveaseID), (object)in_saveaseID);
                sqlCommand.Parameters.AddWithValue(nameof(in_loggedin), (object)in_loggedin);
                sqlCommand.Parameters.AddWithValue(nameof(in_loggedout), (object)in_loggedout);
                sqlCommand.Parameters.AddWithValue(nameof(in_status), (object)in_status);
                sqlCommand.Parameters.AddWithValue(nameof(in_computerName), (object)in_computerName);
                sqlCommand.Parameters.AddWithValue(nameof(in_ipaddress), (object)in_ipaddress);
                sqlCommand.Parameters.AddWithValue(nameof(in_createupdate), (object)in_createupdate);
                connection.Open();
                num = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
            }
            catch (Exception ex)
            {
                num = 0;
                expObj = ex.Message;
            }
            return num;
        }

        public int SystemLog(
          string in_saveaseID,
          string in_actionPerformed,
          string in_computerName,
          string in_ipaddress,
          string in_actionPage,
          out string expObj)
        {
            int num;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("insertLoggedIn", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(nameof(in_saveaseID), (object)in_saveaseID);
                sqlCommand.Parameters.AddWithValue(nameof(in_actionPerformed), (object)in_actionPerformed);
                sqlCommand.Parameters.AddWithValue(nameof(in_computerName), (object)in_computerName);
                sqlCommand.Parameters.AddWithValue(nameof(in_ipaddress), (object)in_ipaddress);
                sqlCommand.Parameters.AddWithValue(nameof(in_actionPage), (object)in_actionPage);
                connection.Open();
                num = sqlCommand.ExecuteNonQuery();
                expObj = "";
                connection.Close();
            }
            catch (Exception ex)
            {
                num = 0;
                expObj = ex.Message;
            }
            return num;
        }

        public int existAdminEmail(string email, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "adminEmailExist";
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)email);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = int.Parse(dataTable.Rows[0]["counts"].ToString());
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public int existAdmin(string uname, string pwd, string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "Adminexistuserpwd";
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_uname", (object)uname);
                        selectCommand.Parameters.AddWithValue("@in_pwd", (object)pwd);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
                num = int.Parse(dataTable.Rows[0]["counts"].ToString());
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return num;
        }

        public DataTable getAdminUname(
          string dname,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getAdUserName1";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_saveaseID", (object)dname);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public DataTable getBlockpinrequest(string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "AdmingetBlockpinreq";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public int AdminBlockpin(string in_pinType, string in_value, out string expObj)
        {
            int num1 = 0;
            int num2;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(""));
                SqlCommand sqlCommand = new SqlCommand("AdminblockPin", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_pinType", (object)in_pinType);
                sqlCommand.Parameters.AddWithValue("@in_value", (object)in_value);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
                expObj = "";
                num2 = 1;
                connection.Close();
            }
            catch (Exception ex)
            {
                num2 = 0;
                expObj = ex.Message;
            }
            return num2;
        }

        public DataTable getVendorCount(string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "countVendors";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public DataTable getUserCount(string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "countUsers";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public DataTable getCardCount(string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "countCardSgen";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public DataTable getTotalAmount(string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "TotalAmount";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public DataTable getOnlineUsers(string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getlogedinUsers";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public string RegisterAdmin(
          string fname,
          string lname,
          string phone,
          string email,
          string password,
          string admintype,
          string adminlevel,
          string connLocation)
        {
            string str;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(connLocation));
                SqlCommand sqlCommand = new SqlCommand(nameof(RegisterAdmin), connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_fname", (object)fname);
                sqlCommand.Parameters.AddWithValue("@in_lname", (object)lname);
                sqlCommand.Parameters.AddWithValue("@in_dname", (object)(lname + " " + fname));
                sqlCommand.Parameters.AddWithValue("@in_phone", (object)phone);
                sqlCommand.Parameters.AddWithValue("@in_password", (object)password);
                sqlCommand.Parameters.AddWithValue("@in_email", (object)email);
                sqlCommand.Parameters.AddWithValue("@in_adminid", (object)this.genSaveaseID());
                sqlCommand.Parameters.AddWithValue("@in_adminlvl", (object)adminlevel);
                sqlCommand.Parameters.AddWithValue("@in_userlvl", (object)admintype);
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                str = "Sussessful";
                connection.Close();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public DataTable getUserByIDAdm(
          string in_ID,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getUserByUID_adm";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_ID", (object)in_ID);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
            }
            return dataTable;
        }

        public string UpdateUsers(
          string id,
          string fname,
          string lname,
          string dname,
          string phone,
          string email,
          string address,
          string connLocation)
        {
            string str;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(connLocation));
                SqlCommand sqlCommand = new SqlCommand("updateUsersbyAdmin", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_id", (object)id);
                sqlCommand.Parameters.AddWithValue("@in_fname", (object)fname);
                sqlCommand.Parameters.AddWithValue("@in_lname", (object)lname);
                sqlCommand.Parameters.AddWithValue("@in_dname", (object)dname);
                sqlCommand.Parameters.AddWithValue("@in_phone", (object)phone);
                sqlCommand.Parameters.AddWithValue("@in_email", (object)email);
                sqlCommand.Parameters.AddWithValue("@in_address", (object)address);
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                str = "Sussessful";
                connection.Close();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public int UpdateSaveaseBusiness(
          string saveaseId,
          string BusinessAddress,
          string BusinessAddress_Town,
          string BusinessAddress_State,
          string HomeAddress,
          string HomeAddress_town,
          string HomeAddress_State,
          string NextOfKin,
          string NextOfkin_Phone,
          string Relationship_With_Kin,
          string connLocation)
        {
            int num;
            try
            {
                SqlConnection connection = new SqlConnection(this.constring.connstring(connLocation));
                SqlCommand sqlCommand = new SqlCommand("updateSaveaseBusiness", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@in_saveaseid", (object)saveaseId);
                sqlCommand.Parameters.AddWithValue("@in_BusinessAddress", (object)BusinessAddress);
                sqlCommand.Parameters.AddWithValue("@in_BusinessAddress_Town", (object)BusinessAddress_Town);
                sqlCommand.Parameters.AddWithValue("@in_BusinessAddress_State", (object)BusinessAddress_State);
                sqlCommand.Parameters.AddWithValue("@in_HomeAddress", (object)HomeAddress);
                sqlCommand.Parameters.AddWithValue("@in_HomeAddress_town", (object)HomeAddress_town);
                sqlCommand.Parameters.AddWithValue("@in_HomeAddress_State", (object)HomeAddress_State);
                sqlCommand.Parameters.AddWithValue("@in_NextOfKin", (object)NextOfKin);
                sqlCommand.Parameters.AddWithValue("@in_NextOfkin_Phone", (object)NextOfkin_Phone);
                sqlCommand.Parameters.AddWithValue("@in_Relationship_With_Kin", (object)Relationship_With_Kin);
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                num = 1;
                connection.Close();
            }
            catch (Exception ex)
            {
                num = 0;
            }
            return num;
        }

        public DataTable getusersOnline(string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getonlineinusers";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public DataTable getOnlineVendors(string connLocation, out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "getonlineVendors";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public DataTable rptTransferAdm(
          string str_dtFrom,
          string str_dtTo,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(rptTransferAdm);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_startDate", (object)str_dtFrom);
                        selectCommand.Parameters.AddWithValue("@in_endDate", (object)str_dtTo);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public DataTable rptDepositAdm(
          string str_dtFrom,
          string str_dtTo,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(rptDepositAdm);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_startDate", (object)str_dtFrom);
                        selectCommand.Parameters.AddWithValue("@in_endDate", (object)str_dtTo);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public DataTable rptvoucherAdm(
          string str_dtFrom,
          string str_dtTo,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = "rptVoucherAdm";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_startDate", (object)str_dtFrom);
                        selectCommand.Parameters.AddWithValue("@in_endDate", (object)str_dtTo);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }

        public DataTable getUserByBVN(
          string in_bvn,
          string connLocation,
          out string expMessage)
        {
            DataTable dataTable = new DataTable();
            string connectionString = this.constring.connstring(connLocation);
            string cmdText = nameof(getUserByBVN);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@in_bvn", (object)in_bvn);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                            sqlDataAdapter.Fill(dataTable);
                    }
                }
                expMessage = "";
            }
            catch (Exception ex)
            {
                expMessage = ex.Message;
                dataTable = (DataTable)null;
            }
            return dataTable;
        }
    }
}