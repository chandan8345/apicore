using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using RestSharp;

namespace APICORE.DAL
{
    public class mafia_access
    {
        private SqlTransaction Trans;
        private SqlConnection AppConn = new SqlConnection("Data Source=CHANDAN-PC;Database=TEST_DB;Trusted_Connection=True;MultipleActiveResultSets=true");
        private SqlCommand Cmnd;

        public DataTable ExecuteStoredProcedure(string storedProcedureName, Hashtable parameters)
        {
            OpenAppConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(storedProcedureName, AppConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;// 5 mins
                                         //if (Trans == null)
                                         //{
                                         //    Cmnd = new SqlCommand();
                                         //    Cmnd.Connection = AppConn;
                                         //}
                                         //Cmnd.CommandType = CommandType.StoredProcedure;
                                         //Cmnd.CommandText = storedProcedureName;

                if (parameters != null && parameters.Count > 0)
                {
                    foreach (string parametername in parameters.Keys)
                    {
                        SqlParameter param = new SqlParameter("@" + parametername, parameters[parametername]);
                        cmd.Parameters.Add(param);

                    }
                }
                //Cmnd.ExecuteNonQuery();
                //Cmnd.Parameters.Clear();
                //Cmnd.CommandType = CommandType.Text;

                DataSet ds = new DataSet();

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.SelectCommand = cmd;

                adp.Fill(ds);


                return ds.Tables[0];


            }
            catch (SqlException Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                    Trans = null;
                }
                throw Ex;
            }
            catch (Exception Ex)
            {
                //if (Trans != null)
                //{
                //    Trans.Rollback();
                //    Trans = null;
                //}

                throw Ex;
            }
            finally
            {
                //if (Trans == null)
                //{
                CloseAppConnection();
                // }

            }

        }

        private void OpenAppConnection()
        {
            //string ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;

            if (!AppConn.Equals(""))
            {
                if (AppConn.State != ConnectionState.Open)
                {
                    AppConn.Open();
                }
            }
        }

        private void CloseAppConnection()
        {
            if (AppConn.State == ConnectionState.Open)
            {
                AppConn.Close();
            }
        }

        public DataTable GetDataTableByCommand(string strSQL)
        {
            OpenAppConnection();

            try
            {
                SqlCommand cmd = new System.Data.SqlClient.SqlCommand(strSQL, AppConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 300;// 5 mins
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                adp.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                AppConn.Close();
            }
        }

        public object ExecuteScalar(string strSQL)
        {
            OpenAppConnection();

            try
            {
                if (Trans == null)
                {
                    Cmnd = new SqlCommand(strSQL, AppConn);
                }
                else
                {
                    Cmnd.CommandText = strSQL;
                }

                Cmnd.CommandType = CommandType.Text;

                return Cmnd.ExecuteScalar();
            }
            catch (SqlException Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                    Trans = null;
                }
                throw Ex;

            }
            catch (Exception Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                    Trans = null;
                }
                throw Ex;

            }
            finally
            {
                if (Trans == null)
                {
                    CloseAppConnection();
                }

            }
        }



        internal void SendSMS(string sms_to, string sms_body)
        {
            var masking = "NOMASK";
            var userName = "AsthaLifeInsurance";
            var password = "02e4fd431dd1399bc07006fb0c6ac25f";
            var MsgType = "TEXT";
            var receiver = sms_to;
            var message = sms_body;
            var client = new RestClient("http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=" + masking + "&userName=" + userName + "&password=" + password + "&MsgType=" + MsgType + "&receiver=" + receiver + "&message=" + message + "");
            var request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/json");
            IRestResponse response = client.Execute(request);
        }
        public static void SendMail(string mailTo, string mailSubject, string mailBody, List<string> mailAttachments)
        {
            //return mailAttachments.Count;
            string userName = "info@asthalife.com.bd";
            string password = "Success2019@";

            string mailFrom = "info@asthalife.com.bd";
            string mailHost = "mail.asthalife.com.bd";
            int mailPort = 587;

            //string userName = "asthalifezoom@gmail.com";
            //string password = "Success2019@";

            //string mailFrom = "asthalifezoom@gmail.com";
            //string mailHost = "smtp.gmail.com";
            //int mailPort = 587;

            //string userName = "56fe94ee37721f";
            //string password = "a9966f4f70b874";

            //string mailFrom = "saddamtest24@gmail.com";
            //string mailHost = "smtp.mailtrap.io";
            //int mailPort = 587;



            //string to = "receiver@domain.com";

            ////It seems, your mail server demands to use the same email-id in SENDER as with which you're authenticating. 
            ////string from = "sender@domain.com";
            //string from = "test@domain.com";

            //string subject = "Hello World!";
            //string body = "Hello Body!";
            //MailMessage message = new MailMessage(from, to, subject, body);
            //SmtpClient client = new SmtpClient("smtp.domain.com");
            //client.UseDefaultCredentials = false;
            //client.Credentials = new NetworkCredential("test@domain.com", "password");
            //client.Send(message);


            var mailMessage = new MailMessage();
            mailMessage.To.Add(new MailAddress(mailTo));
            mailMessage.From = new MailAddress(mailFrom);
            mailMessage.Subject = mailSubject;
            mailMessage.Body = mailBody;
            mailMessage.IsBodyHtml = false;

            if (mailAttachments.Count > 0)
            {
                foreach (var mailAttachment in mailAttachments)
                {
                    mailMessage.Attachments.Add(new Attachment(mailAttachment));
                }
            }

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = userName,
                    Password = password
                };
                //smtp.UseDefaultCredentials = false;
                smtp.Credentials = credential;
                smtp.Host = mailHost;
                smtp.Port = mailPort;
                smtp.EnableSsl = false;
                smtp.Send(mailMessage);
                mailMessage.Dispose();
            }
        }
    }
}
