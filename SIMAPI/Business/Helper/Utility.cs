using SIMAPI.Data.Models;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SIMAPI.Business.Helper
{
    public static class Utility
    {
        
        public class SendMail
        {

            public static string sendMail(String toAddress, string body, string subject)
            {
                return sendMail(toAddress, body, subject, null);
            }
            public static string sendMail(String toAddress, string body, string subject, string bcc)
            {
                return sendMail(toAddress, body, subject, bcc, null);
            }
            public static string sendMail(String toAddress, string body, string subject, string bcc, string cc)
            {
                return sendMail(toAddress, body, subject, bcc, cc, null);
            }


            public static string sendMail(String toAddress, string body, string subject, string bcc, string cc, List<Attachment> attachments)
            {
                String fromAddress = ApplicationSettings.MailSettings.fromMail;
                String fromPassword = ApplicationSettings.MailSettings.fromPassword;
                String smtpHost = ApplicationSettings.MailSettings.smtpHost;
                String smtpPort = ApplicationSettings.MailSettings.smtpPort;
                bool useSSL = ApplicationSettings.MailSettings.useSSL == "True";
                bool isTest = ApplicationSettings.MailSettings.IsTest == "True";

                if (isTest)
                {
                    toAddress = ApplicationSettings.MailSettings.testMail;
                    bcc = "";
                    cc = "";
                }
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Port = Convert.ToInt32(smtpPort);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = smtpHost;
                if (useSSL)
                {
                    client.EnableSsl = true;
                }

                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(fromAddress, fromPassword);

                using (var mail = new MailMessage(fromAddress, toAddress))
                {
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.IsBodyHtml = true;

                    if (attachments != null && attachments.Any())
                    {
                        foreach (var attach in attachments)
                        {
                            mail.Attachments.Add(attach);
                        }
                    }

                    if (!string.IsNullOrEmpty(bcc))
                        mail.Bcc.Add(bcc);
                    if (!string.IsNullOrEmpty(cc))
                        mail.CC.Add(cc);
                    try
                    {
                        client.Send(mail);
                    }
                    catch (SmtpException exception)
                    {
                        return "Mail Sending Failed" + exception.Message;
                    }
                }
                return "Success";
            }

        }

        public static CommonResponse CreateResponse(object result, HttpStatusCode statusCode)
        {
            List<HttpStatusCode> successStatus = new List<HttpStatusCode> { HttpStatusCode.OK, HttpStatusCode.Created };
            CommonResponse response = new CommonResponse();
            response.data = result;
            response.statusCode = statusCode;
            response.status = successStatus.Any(s => s.Equals(statusCode));
            return response;
        }

        public static string NumberToText(int number, bool isUK)
        {
            if (number == 0) return "Zero";
            string and = isUK ? "and " : ""; // deals with UK or US numbering
            if (number == -2147483648) return "Minus Two Billion One Hundred " + and +
            "Forty Seven Million Four Hundred " + and + "Eighty Three Thousand " +
            "Six Hundred " + and + "Forty Eight";
            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }
            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
            string[] words3 = { "Thousand ", "Million ", "Billion " };
            num[0] = number % 1000;           // units
            num[1] = number / 1000;
            num[2] = number / 1000000;
            num[1] = num[1] - 1000 * num[2];  // thousands
            num[3] = number / 1000000000;     // billions
            num[2] = num[2] - 1000 * num[3];  // millions
            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;
                u = num[i] % 10;              // ones
                t = num[i] / 10;
                h = num[i] / 100;             // hundreds
                t = t - 10 * h;               // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i < first) sb.Append(and);
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }
    }
    public static class ApplicationSettings
    {
        public const string dbConnection = "ConnectionStrings";
        public const string mailSettings = "MailSettings";

        public static ConnectionStrings ConnectionString { get; set; } = new ConnectionStrings();
        public static MailSettings MailSettings { get; set; } = new MailSettings();

        // other options here...
    }

    public static class FolderUtility
    {
        public const string category = "Category";
        public const string subCategory = "SubCategory";
        public const string product = "Product";
        public const string shop = "Shop";
        public const string user = "User";
        public const string userDocument = "UserDocuments";
    }
}
