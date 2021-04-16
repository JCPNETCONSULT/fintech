using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using System.Xml;
using System.Net;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Web.Script.Services;

namespace Fintech
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://eseokpako-001-site1.itempurl.com")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : WebService
    {
        private MailMgr mailMessenger = new MailMgr();
        private procedurs dtproc = new procedurs();
        private string hosts = HttpContext.Current.Request.Url.Host;
        private SH1Encryption sh1 = new SH1Encryption();
        private RandomPassword temPassword = new RandomPassword();

        [WebMethod]
        public string getVendorCount()
        {
            string expMessage = "";
            return JsonConvert.SerializeObject((object)this.dtproc.getVendorCount(this.hosts, out expMessage), Newtonsoft.Json.Formatting.Indented);
        }

        [WebMethod]
        public string getAcctOfficer(string saveaseID)
        {
            string expMessage = "";
            DataTable acctOfficer = this.dtproc.getAcctOfficer(saveaseID, this.hosts, out expMessage);
            string empty = string.Empty;
            return JsonConvert.SerializeObject((object)acctOfficer);
        }

        [WebMethod]
        public int getUserAccessControl(string uname)
        {
            string expMessage = "";
            string dname;
            return this.dtproc.accessControl(uname, out string displayName, hosts,out string strEmail, out string strPhone, out expMessage);
        }

        [WebMethod]
        public string existUser(string email)
        {
            string expMessage = "";
            string str = "";
            this.dtproc.existEmail(email, this.hosts, out expMessage);
            return str;
        }

        [WebMethod]
        public int existTransPIN(string in_username, string transPIN)
        {
            string expMessage = "";
            return this.dtproc.existTransPIN(in_username, transPIN, this.hosts, out expMessage);
        }

        [WebMethod]
        public int updateBVNStatus(string in_username)
        {
            string expObj = "";
            return this.dtproc.updateBVNStatus(in_username, out expObj);
        }

        [WebMethod]
        public int changepass(string in_username, string in_oldpwd, string in_newpwd)
        {
            string expMessage = "";
            int num = 0;
            if (this.dtproc.existuserpwdz(in_username, this.sh1.GetSHA1HashData(in_oldpwd), "", out expMessage) > 0 && this.dtproc.changepass(in_username, this.sh1.GetSHA1HashData(in_newpwd), "", out expMessage) > 0)
                num = 1;
            return num;
        }

        [WebMethod]
        public int ExistEmail(string email)
        {
            string expMessage = "";
            return this.dtproc.existEmail(email, this.hosts, out expMessage) >= 1 ? 0 : 1;
        }

        [WebMethod]
        public int Existphone(string phone)
        {
            string expMessage = "";
            return this.dtproc.existphone(phone, this.hosts, out expMessage) <= 0 ? 1 : 0;
        }

        [WebMethod]
        public int ExistUname(string username)
        {
            string expMessage = "";
            return this.dtproc.existUser2(username, this.hosts, out expMessage) <= 0 ? 1 : 0;
        }

        [WebMethod]
        public int RegisterUser(
          string fname,
          string lname,
          string phone,
          string email,
          string username,
          string password,
          string transPIN)
        {
            return !this.dtproc.RegisterUser(1, fname, lname, phone, email, username, this.sh1.GetSHA1HashData(password), "Guest", this.hosts, "", "1", this.sh1.GetSHA1HashData(transPIN)).Equals("Successful") ? 0 : 1;
        }

        public int sendMailNotificationToUser(
          string regfname,
          string reglname,
          string email,
          string username)
        {
            try
            {
                string shA1HashData = this.sh1.GetSHA1HashData(username);
                string url = "http://savease.ng/activate.aspx?ud=" + username + "&token=" + shA1HashData;
                string body = this.PopulateBody(regfname + " " + reglname, "Customer registration", url, "Savease New Customer Registration", "mailer/index.html", "Reg");
                //this.mailMessenger.SendHtmlFormattedEmailAsync(email, "Complete Your Registration", body).ConfigureAwait(true);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [WebMethod]
        public int updateUlevel(string saveaseid) => this.dtproc.UpdateLevel(saveaseid, this.hosts, out int _);

        [WebMethod]
        public int InsertBankDetails(
          string accountname,
          string accountno,
          string bankname,
          string saveaseid,
          string bankcode)
        {
            string expObj = "";
            return this.dtproc.InsertBankdetails(accountname, accountno, bankname, saveaseid, bankcode, out expObj);
        }

        [WebMethod]
        public void RegisterVendor(
          string fname,
          string lname,
          string phone,
          string email,
          string username,
          string password,
          string businessType,
          string companyname,
          string address,
          string cacregNo,
          string Bvn)
        {
            JsonConvert.SerializeObject((object)this.dtproc.registerVendor2(fname, lname, phone, email, username, this.sh1.GetSHA1HashData(password), "guest", companyname, address, this.hosts, this.sh1.GetSHA1HashData(username), "0", this.genSaveaseID(), Bvn, cacregNo));
        }

        [WebMethod]
        public string getProfiles(int accesscontrol)
        {
            string expMessage = "";
            return JsonConvert.SerializeObject((object)this.dtproc.getProfiles(accesscontrol, this.hosts, out expMessage));
        }

        [WebMethod]
        public string getUserByBVN(string in_bvn)
        {
            string expMessage = "";
            return JsonConvert.SerializeObject((object)this.dtproc.getUserByBVN(in_bvn, this.hosts, out expMessage));
        }

        [WebMethod]
        public string getVendors(int accesscontrol)
        {
            string expMessage = "";
            return JsonConvert.SerializeObject((object)this.dtproc.getVendors(accesscontrol, "Procedure", this.hosts, out expMessage));
        }

        [WebMethod]
        public string getBalance(string straccountNo)
        {
            string expMessage = "";
            DataTable balance = this.dtproc.getBalance(straccountNo, this.hosts, out expMessage);
            string empty = string.Empty;
            return JsonConvert.SerializeObject((object)balance);
        }

        [WebMethod]
        public string getBalz(string saveaseID)
        {
            XmlDocument xmlDocument = new XmlDocument();
            string expMessage = "";
            string balz = this.dtproc.getBalz(saveaseID, this.hosts, out expMessage);
            return string.IsNullOrEmpty(balz) ? "0" : balz;
        }

        [WebMethod]
        public string getBVN(string straccountNo)
        {
            string expMessage = "";
            string str = this.dtproc.getBVN(straccountNo, this.hosts, out expMessage).Rows[0]["bvn"].ToString();
            string empty = string.Empty;
            return JsonConvert.SerializeObject((object)str);
        }

        public float getBalance2(string straccountNo)
        {
            string expMessage = "";
            DataTable balance = this.dtproc.getBalance(straccountNo, this.hosts, out expMessage);
            string str = string.Empty;
            str = JsonConvert.SerializeObject((object)balance);
            string s = "";
            foreach (DataRow row in (InternalDataCollectionBase)balance.Rows)
                s = row["balance"].ToString();
            return float.Parse(s);
        }

        [WebMethod]
        public float getBalanceTest(string straccountNo)
        {
            string expMessage = "";
            DataTable balance = this.dtproc.getBalance(straccountNo, this.hosts, out expMessage);
            string str = string.Empty;
            str = JsonConvert.SerializeObject((object)balance);
            string s = "";
            foreach (DataRow row in (InternalDataCollectionBase)balance.Rows)
                s = row["balance"].ToString();
            return float.Parse(s);
        }

        [WebMethod]
        public string getBeneficiary(string inputParame)
        {
            string expMessage = "";
            return JsonConvert.SerializeObject((object)this.dtproc.getBeneficiary(inputParame, this.hosts, out expMessage));
        }

        [WebMethod]
        public string saveDeposit(
          string in_acctNo,
          string in_cardpin,
          string in_cardsn,
          string in_bankName,
          string in_acctName,
          string in_amount,
          string in_depositor,
          string in_naration)
        {
            string expObj = "";
            string[] strArray = this.dtproc.saveDeposit(in_acctNo, in_cardpin, in_cardsn, in_bankName, in_acctName, double.Parse(in_amount), in_depositor, this.computerName(), this.ipaddress1(), this.references(), out expObj, in_naration);
            return JsonConvert.SerializeObject((object)new DataTable()
            {
                Columns = {
          "TransStatus",
          "TansRef"
        },
                Rows = {
          (object[]) strArray
        }
            });
        }

        [WebMethod]
        public string getBankDetails(string saveaseid)
        {
            string expMessage = "";
            return JsonConvert.SerializeObject((object)this.dtproc.getBankdetails(saveaseid, this.hosts, out expMessage));
        }

        [WebMethod]
        public int deletebankdetials(string accountNumber) => this.dtproc.DeleteBankdetails(accountNumber, out string _);

        [WebMethod]
        public string saveDepositUSSD(string in_acctNo, string in_cardpin, string in_naration)
        {
            string str = "";
            string cardSerial = this.dtproc.getCardSerial(in_cardpin, "", out str);
            string in_bankName = "Savease";
            string cardAmountByPin = this.dtproc.getCardAmountByPin(in_cardpin, "", out str);
            string dname = this.dtproc.getDName(in_acctNo, "", out str);
            string userNam = this.dtproc.getUserNam(in_acctNo, "", out str);
            return JsonConvert.SerializeObject((object)this.dtproc.saveDeposit(in_acctNo, in_cardpin, cardSerial, in_bankName, dname, double.Parse(cardAmountByPin), userNam, this.computerName(), this.ipaddress1(), this.references(), out str, in_naration), Newtonsoft.Json.Formatting.Indented);
        }

        [WebMethod]
        public void updateBalance(string inuser, string App_amt)
        {
            string str = "";
            string s = "";
            foreach (DataRow row in (InternalDataCollectionBase)this.dtproc.getBalance(inuser, this.hosts, out str).Rows)
                s = row["balance"].ToString();
            double in_bal = double.Parse(s) - double.Parse(App_amt);
            JsonConvert.SerializeObject((object)this.dtproc.updateBalance(inuser, in_bal, out str));
        }

        [WebMethod]
        public int FundAcct(string inuser, string App_amt)
        {
            string str = "";
            string s = "";
            foreach (DataRow row in (InternalDataCollectionBase)this.dtproc.getBalance(inuser, this.hosts, out str).Rows)
                s = row["balance"].ToString();
            double in_bal = double.Parse(s) + double.Parse(App_amt);
            return this.dtproc.updateBalance(inuser, in_bal, out str);
        }

        [WebMethod]
        public string transferFund(
          string amountTransfered,
          string balance,
          string beneficiaryAccount,
          string saveaseid,
          string transferedBy,
          string in_naration,
          string username)
        {
            string str = "";
            string beneficaryName = "";
            foreach (DataRow row in (InternalDataCollectionBase)this.dtproc.getName(beneficiaryAccount, this.hosts, out str).Rows)
                beneficaryName = row["dname"].ToString();
            string[] strArray = this.dtproc.transferFund(this.genPin(), (double)this.getBalance2(saveaseid), double.Parse(amountTransfered), double.Parse(balance), beneficiaryAccount, beneficaryName, saveaseid, transferedBy, this.computerName(), out str, in_naration, username);
            return JsonConvert.SerializeObject((object)new DataTable()
            {
                Columns = {
          "TransStatus",
          "TransRef"
        },
                Rows = {
          (object[]) strArray
        }
            });
        }

        [WebMethod]
        public string transferFundUSSD(
          string amountTransfered,
          string beneficiaryAccount,
          string saveaseid,
          string in_naration,
          string userpin)
        {
            string str1 = "";
            DataTable name = this.dtproc.getName(beneficiaryAccount, this.hosts, out str1);
            string transferedBy = this.dtproc.getName(saveaseid, this.hosts, out str1).Rows[0]["dname"].ToString();
            string balz = this.getBalz(saveaseid);
            string userNam = this.dtproc.getUserNam(saveaseid, this.hosts, out str1);
            string beneficaryName = name.Rows[0]["dname"].ToString();
            string transPin = this.dtproc.getTransPin(saveaseid, this.hosts, out str1);
            string str2;
            if (double.Parse(balz) <= double.Parse(balz))
                str2 = 3.ToString();
            else if (!transPin.Equals(this.sh1.GetSHA1HashData(userpin)))
            {
                str2 = 2.ToString();
            }
            else
            {
                string[] strArray = this.dtproc.transferFund(this.genPin(), (double)this.getBalance2(saveaseid), double.Parse(amountTransfered), double.Parse(balz), beneficiaryAccount, beneficaryName, saveaseid, transferedBy, this.computerName(), out str1, in_naration, userNam);
                str2 = new DataTable()
                {
                    Columns = {
            "TransStatus",
            "TransRef"
          },
                    Rows = {
            (object[]) strArray
          }
                }.ToString();
            }
            return JsonConvert.SerializeObject((object)str2, Newtonsoft.Json.Formatting.Indented);
        }

        [WebMethod]
        public int addBeneficiary(string accountNumber, string savedFor, string bankname)
        {
            string expObj = "";
            return this.dtproc.saveBeneficiary(accountNumber, savedFor, bankname, out expObj);
        }

        public string genPin()
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

        public string references()
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

        [WebMethod]
        public int saveBenDetails(
          string in_acctNo,
          string in_acctName,
          string in_bankName,
          string in_depositor,
          string in_cardpin,
          string in_phoneNumber,
          string in_emailID)
        {
            string expObj = "";
            return this.dtproc.saveBenDetails(in_acctNo, in_acctName, in_bankName, in_depositor, in_cardpin, in_phoneNumber, in_emailID, out expObj);
        }

        [WebMethod]
        public string VerifyPin(string inputParame)
        {
            XmlDocument xmlDocument = new XmlDocument();
            string expMessage = "";
            int num = this.dtproc.usedPin(inputParame, this.hosts, out expMessage);
            string str = "";
            if (num < 1)
            {
                DataTable cardPin = this.dtproc.getCardPin(inputParame, this.hosts, out expMessage);
                if (cardPin == null)
                {
                    str = "Invalid Voucher details supplied!!!";
                }
                else
                {
                    foreach (DataRow row in (InternalDataCollectionBase)cardPin.Rows)
                        str = "Card Valid. Amount: " + row["cardAmount"].ToString();
                }
            }
            else
                str = "Voucher details has been used!!!";
            return str;
        }

        [WebMethod]
        public int getlogin(string uname, string pword)
        {
            string str = "";
            string displayName = "";
            string strEmail = "";
            string strPhone = "";
            if (this.dtproc.existUser(uname, this.sh1.GetSHA1HashData(pword), this.hosts, out str) <= 0)
                return int.Parse("100");
            this.dtproc.accessControl(uname, out displayName, this.hosts, out strEmail, out strPhone, out str);
            Dns.GetHostName();
            Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
            string saveaseIdByName = this.dtproc.getSaveaseID_By_Name(uname, this.hosts, out str);
            string userLevel = this.dtproc.getUserLevel(uname, this.hosts, out str);
            this.dtproc.LoginLog(saveaseIdByName, "Logged In", "", "Online", this.computerName(), this.ipaddress1(), "c", out str);
            this.dtproc.SystemLog(saveaseIdByName, "Logged In", this.computerName(), this.ipaddress1(), "Login.aspx", out str);
            return int.Parse(userLevel);
        }

        [WebMethod]
        public string getlogin2(string uname, string pword)
        {
            string str = "";
            string displayName = "";
            string strEmail = "";
            string strPhone = "";
            if (this.dtproc.existUser(uname, this.sh1.GetSHA1HashData(pword), this.hosts, out str) <= 0)
                return "100";
            this.dtproc.accessControl(uname, out displayName, this.hosts, out strEmail, out strPhone, out str);
            Dns.GetHostName();
            Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
            string saveaseIdByName = this.dtproc.getSaveaseID_By_Name(uname, this.hosts, out str);
            string userLevel = this.dtproc.getUserLevel(uname, this.hosts, out str);
            this.dtproc.LoginLog(saveaseIdByName, "Logged In", "", "Online", this.computerName(), this.ipaddress1(), "c", out str);
            this.dtproc.SystemLog(saveaseIdByName, "Logged In", this.computerName(), this.ipaddress1(), "Login.aspx", out str);
            return JsonConvert.SerializeObject((object)userLevel, Newtonsoft.Json.Formatting.Indented);
        }

        [WebMethod]
        public string saveOrder(
          string in_card_pin,
          string in_cardpin_sn,
          string in_orderNumber,
          string in_cardType,
          double in_cardAmount,
          string in_orderby)
        {
            string expObj = "";
            return JsonConvert.SerializeObject((object)this.dtproc.saveOrder("Complete", in_card_pin, in_cardpin_sn, in_orderNumber, in_cardType, in_cardAmount, 0.0, 0.25, in_orderby, this.computerName(), this.ipaddress1(), out expObj).ToString());
        }

        [WebMethod]
        public string saveToTble(string orderby, string in_orderNumber)
        {
            string str = "";
            try
            {
                string saveaseIdByName = this.dtproc.getSaveaseID_By_Name(orderby, this.hosts, out str);
                string dname = this.dtproc.getDName(orderby, this.hosts, out str);
                this.dtproc.composeMail("SV000333", "Purchase Notification ", " Order Number: " + in_orderNumber, dname, saveaseIdByName, dname, in_orderNumber, out str);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error." + ex.Message;
            }
        }

        [WebMethod]
        public int saveOrder1(
          string in_card_pin,
          string in_cardpin_sn,
          string in_orderNumber,
          string in_cardType,
          double in_cardAmount,
          string in_orderby)
        {
            string expObj = "";
            return this.dtproc.saveOrder("Complete", in_card_pin, in_cardpin_sn, in_orderNumber, in_cardType, in_cardAmount, 0.0, 0.25, in_orderby, this.computerName(), this.ipaddress1(), out expObj);
        }

        [WebMethod]
        public int saveOrder2(
          string saveaseIDz,
          string in_cardType,
          string in_cardAmount,
          string in_orderby,
          string percentage,
          int qty,
          string lblBa)
        {
            string expObj = "";
            double in_percentageCharg = double.Parse(percentage);
            return this.dtproc.saveOrder2(saveaseIDz, "Completed", in_cardType, double.Parse(in_cardAmount), double.Parse(in_cardAmount), in_percentageCharg, in_orderby, this.computerName(), this.ipaddress1(), out expObj, qty, lblBa);
        }

        [WebMethod]
        public string getCardAmount(string in_card_pin)
        {
            string expMessage = "";
            DataTable cardPin = this.dtproc.getCardPin(in_card_pin, this.hosts, out expMessage);
            string s = "";
            foreach (DataRow row in (InternalDataCollectionBase)cardPin.Rows)
                s = row["cardAmount"].ToString();
            return JsonConvert.SerializeObject((object)int.Parse(s));
        }

        [WebMethod]
        public string resetPassword(string in_email)
        {
            string expMessage = "";
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string token = "";
            foreach (DataRow row in (InternalDataCollectionBase)this.dtproc.getResetDetails(in_email, this.hosts, out expMessage).Rows)
            {
                str1 = row["username"].ToString();
                str2 = row["firstname"].ToString();
                str3 = row["lastname"].ToString();
                token = this.sh1.GetSHA1HashData(str1);
            }
            string url = "http://savease.ng/activate.aspx?ud=" + str1 + "&token=" + token;
            string body = this.PopulateBody(str2 + " " + str3, "Password Reset", url, "Savease Password Reset", "mailer/index.html", "Res");
            this.dtproc.UpdateToken(str1, token, this.hosts);
            //this.mailMessenger.SendHtmlFormattedEmailAsync(in_email, "Reset Your Password", body);
            return JsonConvert.SerializeObject((object)"Check mail for confirmation");
        }

        private string PopulateBody(
          string userName,
          string title,
          string url,
          string description,
          string filepath,
          string mailtype)
        {
            return string.Empty + " <div style = 'width: 500px; background: whitesmoke; margin-right: auto; margin-left: auto;' >" + " <div  style = 'margin-left: auto;margin-right: auto;width: 350px;background: white;' >" + "<div  style='height: 70px; background:whitesmoke;'>" + "</div>" + "<div style = 'width: 195px;margin-left: auto;margin-right: auto;'>" + " <img src = 'http://www.savease.ng/logo/logo.png' style = 'margin-top: 20px' >" + "</div><br><br>" + "<div>" + "<h3 style = 'text-align: center;font-family:calibri;font-size: 18px;' > Complete your Registration</h3><br><br>" + "<p style = 'margin-left: 15px; font-family:calibri;font-size: 11px' > Please click the  button below to reset password</p>" + "</div><br><br>" + " <div style = ' width: 250px;height: 70px;margin-right: auto;margin-left: auto;' >" + " <a href='" + url + "'><input type= 'submit' name= '' value= 'Reset Password' style= 'background: #fa9928;color: white;margin-left: 30px;margin-right: 30px;margin-top: 20px;border-radius: 4px;height:35px;width: 200px;' ></ a >" + " </div ><br ><p style= 'color: #212435; text-align: center;font-family:calibri;font-size: 10px;'> or Copy Link</p>" + url + " <br><br><hr><br><br><br>" + "  <div>" + "<h4 style = 'color: #fa9928;text-align: center;font-family:calibri; font-size: 12px;' > Questions ? Get your answer here:&nbsp;<a href = 'faq.aspx' style='color: blue; font-size: smaller;font-family:calibri; font-size: 9px;'>Help Center</a></h4>" + "</div><br><br><br>" + "<div>" + "<h5 style = 'color: #212435; text-align: center;font-family:calibri;font-size: 10px;' > You may contact us through any of the option:</h5>" + "</div>" + "<div>" + " <p style = 'color: #212435; text-align: center;font-family:calibri;font-size: 10px;' > Phone: 0700SAVEASE, 07032599897,08056161069</p>" + "</div>" + " <div style = 'text-align: center;font-size: 10px;' >" + "<p style= 'color: #212435; text-align: center;font-family:calibri;' > care@savease.ng, enquiry @savease.ng" + "</div>" + "  <div style = 'text-align: center;color: #212435;font-family:calibri;font-size: 10px;font-weight: bold;' >" + " &copy; 2018 Savease.All Rights Reserved." + "</div>" + "<div style = 'text-align: center; font-family: calibri;color:#212435;font-size: 10px;font-weight: bolder; '>" + "|<a href='doc/TnC.pdf' target='_blank'>Privacy Policy</a> | <a href = 'doc/TnC.pdf' target= '_blank' > General Terms and Condition</a>" + " </div>" + " </div>" + "</div>";
        }

        [WebMethod]
        public string getCardSerial(string in_card_pin)
        {
            string expMessage = "";
            return JsonConvert.SerializeObject((object)this.dtproc.getCardSerialOnly(in_card_pin, this.hosts, out expMessage));
        }

        [WebMethod]
        public string getNameOnDeposit(string in_saveaseid)
        {
            string expMessage = "";
            return JsonConvert.SerializeObject((object)this.dtproc.getName(in_saveaseid, this.hosts, out expMessage));
        }

        public string genSaveaseID()
        {
            Random random = new Random();
            string str = "";
            for (int index = 0; index < 2; ++index)
            {
                int num = random.Next(10000, 99999);
                str += (string)(object)num;
            }
            return str;
        }

        [WebMethod]
        public string getTransactionUsers(string uname)
        {
            string expMessage = "";
            DataTable transsactionDetails = this.dtproc.getTranssactionDetails(uname, this.hosts, out expMessage);
            StringBuilder stringBuilder = new StringBuilder();
            if (transsactionDetails.Rows.Count > 0)
            {
                stringBuilder.Append("[");
                for (int index1 = 0; index1 < transsactionDetails.Rows.Count; ++index1)
                {
                    stringBuilder.Append("{");
                    for (int index2 = 0; index2 < transsactionDetails.Columns.Count; ++index2)
                    {
                        if (index2 < transsactionDetails.Columns.Count - 1)
                            stringBuilder.Append("\"" + transsactionDetails.Columns[index2].ColumnName.ToString() + "\":\"" + transsactionDetails.Rows[index1][index2].ToString() + "\",");
                        else if (index2 == transsactionDetails.Columns.Count - 1)
                            stringBuilder.Append("\"" + transsactionDetails.Columns[index2].ColumnName.ToString() + "\":\"" + transsactionDetails.Rows[index1][index2].ToString() + "\"");
                    }
                    if (index1 == transsactionDetails.Rows.Count - 1)
                        stringBuilder.Append("}");
                    else
                        stringBuilder.Append("},");
                }
                stringBuilder.Append("]");
            }
            return stringBuilder.ToString();
        }

        [WebMethod]
        public string DataTableToJSONWithJSONNet(string uname)
        {
            string expMessage = "";
            DataTable transsactionDetails = this.dtproc.getTranssactionDetails(uname, this.hosts, out expMessage);
            string empty = string.Empty;
            return JsonConvert.SerializeObject((object)transsactionDetails);
        }

        [WebMethod]
        public string getTransMessageNotification(string uname)
        {
            string expMessage = "";
            DataTable transMessage = this.dtproc.getTransMessage(uname, this.hosts, out expMessage);
            string empty = string.Empty;
            return JsonConvert.SerializeObject((object)transMessage);
        }

        [WebMethod]
        public string displayTblUsed(string uname)
        {
            string expMessage = "";
            string empty = string.Empty;
            return JsonConvert.SerializeObject((object)this.dtproc.SortPinby(uname, "", "1", "", "", this.hosts, out expMessage));
        }

        [WebMethod]
        public string displayTblUnused(string uname)
        {
            string expMessage = "";
            string empty = string.Empty;
            return JsonConvert.SerializeObject((object)this.dtproc.SortPinby(uname, "", "2", "", "", this.hosts, out expMessage));
        }

        [WebMethod]
        public string displayTblDaterange(string uname, string startDate, string endDate)
        {
            string expMessage = "";
            DateTime dateTime1 = Convert.ToDateTime(startDate);
            DateTime dateTime2 = Convert.ToDateTime(endDate);
            string empty = string.Empty;
            string dateFrom = string.Format("{0:yyyy/MM/dd}", (object)dateTime1);
            string dateTo = string.Format("{0:yyyy/MM/dd}", (object)dateTime2);
            return JsonConvert.SerializeObject((object)this.dtproc.SortPinby(uname, "", "5", dateFrom, dateTo, this.hosts, out expMessage));
        }

        public string computerName() => Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();

        public string ipaddress1() => HttpContext.Current.Request.UserHostAddress;

        [WebMethod]
        public string USSDInput(string uname)
        {
            string expMessage = "";
            DataTable transsactionDetails = this.dtproc.getTranssactionDetails(uname, this.hosts, out expMessage);
            string empty = string.Empty;
            return JsonConvert.SerializeObject((object)transsactionDetails);
        }

        [WebMethod]
        public int updatesaveaseBusiness(
          string saveaseId,
          string BusinessAddress,
          string BusinessAddress_Town,
          string BusinessAddress_State,
          string HomeAddress,
          string HomeAddress_town,
          string HomeAddress_State,
          string NextOfKin,
          string NextOfkin_Phone,
          string Relationship_With_Kin)
        {
            return this.dtproc.UpdateSaveaseBusiness(saveaseId, BusinessAddress, BusinessAddress_Town, BusinessAddress_State, HomeAddress, HomeAddress_town, HomeAddress_State, NextOfKin, NextOfkin_Phone, Relationship_With_Kin, this.hosts);
        }
    }
}
