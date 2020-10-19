using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Net.Mail;
using System.ComponentModel;

namespace BirthDayWishes
{
    class SendEmail
    {
        static bool mailSent = false;
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }
        public void GetEmployee()
        {
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("https://eohmc-acme-api.azurewebsites.net/api/Employees"));

            WebReq.Method = "GET";

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            Console.WriteLine(WebResp.StatusCode);
            Console.WriteLine(WebResp.Server);

            string jsonString;
            using (Stream stream = WebResp.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
                Console.WriteLine(jsonString);
            }

            List<Employees> items = JsonConvert.DeserializeObject<List<Employees>>(jsonString);
            foreach (var item in items)
            {
                if (item.DOB == DateTime.Today)
                {

                    SmtpClient client = new SmtpClient();
                    MailAddress from = new MailAddress("thatomashego01@gmail.com", "Thato " + (char)0xD8 + " Mashego", System.Text.Encoding.UTF8);
                    MailAddress to = new MailAddress("thato.mashego@24.com");
                    MailMessage message = new MailMessage(from, to);
                    message.Body = "Happy Birthday " + item.Name + item.Lastname + ", Wishing you a Fabulous day filled with joy";

                    // Include some non-ASCII characters in body and subject.
                    string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
                    message.Body += Environment.NewLine + someArrows;
                    message.BodyEncoding = System.Text.Encoding.UTF8;
                    message.Subject = "Happy Birthday to you" + someArrows;
                    message.SubjectEncoding = System.Text.Encoding.UTF8;

                    // Set the method that is called back when the send operation ends.
                    client.SendCompleted += new
                    SendCompletedEventHandler(SendCompletedCallback);

                }

                if (item.EmploymentStartDate == DateTime.Today)
                {

                }
            }
        }
    }
}
