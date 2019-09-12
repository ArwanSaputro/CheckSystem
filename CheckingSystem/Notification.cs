using AegisImplicitMail;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckingSystem
{
    class Notification
    {
        public static int? SendNotificationGlobal(string EmailTo, string Subject, string MessageContent, string ParamConfig)
        {
            int? status = 1;

            try
            {
                //var dbMAILCONFIG = Common.getSysParam(ParamConfig);
                string[] config = ParamConfig.Split(new char[] { '|' });
                int port = Convert.ToInt16(config[1]);
                string Security = config[5];
                if (!string.IsNullOrEmpty(EmailTo))
                {
                    var mymessage = new MimeMailMessage();
                    mymessage.From = new MimeMailAddress(config[2]);
                    mymessage.To.Add(EmailTo);
                    mymessage.Subject = Subject;
                    mymessage.Body = MessageContent;
                    mymessage.Priority = System.Net.Mail.MailPriority.High;
                    var mailer = new MimeMailer(config[0], port);
                    mailer.User = config[3];
                    mailer.Password = GeneralCommon.Decrypt(config[4]); 
                    if (Security == "TLS")
                        mailer.SslType = SslMode.Tls;
                    else if (Security == "SSL")
                        mailer.SslType = SslMode.Ssl;
                    else if (Security == "NONE")
                        mailer.SslType = SslMode.Ssl;
                    else
                        mailer.SslType = SslMode.Auto;

                    if (mailer.SslType == SslMode.None)
                        mailer.AuthenticationMode = AuthenticationType.PlainText;
                    else
                        mailer.AuthenticationMode = AuthenticationType.Base64;
                    mailer.SendCompleted += compEvent;
                    mailer.SendMail(mymessage);
                    status = 0;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                //UIException.LogException(ex, "public static long SendNotification(string EmailTo,string Subject,string MessageContent)");
            }
            return status;
        }

        private static void compEvent(object sender, AsyncCompletedEventArgs e)
        {
            if (e.UserState != null)
                Console.Out.WriteLine(e.UserState.ToString());
            if (e.Error != null)
            {
                // MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //UIException.LogException(e.Error.Message, "public static long SendNotification(string EmailTo,string Subject,string MessageContent)");

                throw new Exception(e.Error.Message);
            }
            else if (!e.Cancelled)
            {
                //MessageBox.Show("Send successfull!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
