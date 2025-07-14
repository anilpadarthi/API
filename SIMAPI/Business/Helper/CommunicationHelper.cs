﻿using SIMAPI.Business.Helper.PDF;
using SIMAPI.Data.Models.OrderListModels;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;

namespace SIMAPI.Business.Helper
{
    public static class CommunicationHelper
    {
        public static void SendRegistrationEmail(int shopId, string shopName, string shopEmail, string password, string ownerName)
        {
            string toMail = EmailSettings.toEmail;
            if (!string.IsNullOrEmpty(shopEmail))
                toMail += "," + shopEmail;
            if (string.IsNullOrEmpty(ownerName) || ownerName == "NULL")
                ownerName = shopName;

            string activationLink = "https://www.leap-tel.com/Retailer/login";
            StringBuilder strBody = new StringBuilder();
            strBody.Append("<p>Greetings, Mr " + ownerName + ", </p>");
            strBody.Append("<p>Thank you for signing up. Here is all the information you need for managing your leap-tel account online. </p><br />");
            strBody.Append("<p>Shop ID :" + shopId + "</p>");
            strBody.Append("<p>PASSWORD    :" + password + "</p>");
            strBody.Append("<p>To login to your leap-tel account, Please <a href='" + activationLink + "'>" + "Click" + " </a> here </p>");
            strBody.Append("<p>Have a great day!</p><br />");
            strBody.Append("<p>Customer Services Team</p>");
            strBody.Append("<p>leap-tel</p>");
            strBody.Append("<p>03330119880</p>");


            MailMessage objmail = new MailMessage();
            objmail.Subject = "LEAP – Your Online Account is Live Now!";
            objmail.Body = strBody.ToString();
            objmail.From = new MailAddress(EmailSettings.welcomeMail);

            foreach (string str in toMail.Split(','))
            {
                if (str.Contains("@"))
                    objmail.To.Add(new MailAddress(str));
            }

            objmail.IsBodyHtml = true;



            try
            {
                NetworkCredential credentioals = new NetworkCredential(EmailSettings.infoMail, EmailSettings.infoMailPwd);
                SendEmail(objmail, credentioals);
            }
            catch (Exception ex)
            {
                NetworkCredential credentioals = new NetworkCredential(EmailSettings.welcomeMail, EmailSettings.welcomeMailPwd);
                SendEmail(objmail, credentioals);
            }
        }

        public static void SendWelcomeEmail(int shopId, string shopName, string shopEmail, string password, string ownerName)
        {
            string toMail = EmailSettings.toEmail;
            if (!string.IsNullOrEmpty(shopEmail))
                toMail += "," + shopEmail;
            if (string.IsNullOrEmpty(ownerName) || ownerName == "NULL")
                ownerName = shopName;

            StringBuilder strBody = new StringBuilder();
            strBody.Append("<div style='width:100%;background-color:#F70000;color:white;text-align:center;'><h1>Welcome To Leap </h1></div>");
            strBody.Append("<p>Greetings, Mr " + ownerName + "!! </p>");
            strBody.Append("<p>Thank you for choosing leap-tel as your Pay As You Go SIM Cards and Mobile Accessories supplier. You have now joined over 20000 retailers just like you who trust us with managing their SIM Card and Mobile Accessories business segment. We constantly endeavour to build a great relationship with our network partners and retailers and strive towards providing the best service to our customers </p>");
            strBody.Append("<p>We encourage you to discover the great promotions and offers current available by contacting your Local leap-tel Executive today!</p>");
            strBody.Append("<p>We hope that you take this opportunity with leap-tel to reap great benefits, commissions and services to help your retail SIM business to great heights.</p>");
            strBody.Append("<p>Your online account details are sent on a separate email, please check your Junk or Spam mailboxes if you are unable to find the email in your inbox. </p><br />");
            strBody.Append("<p>Have a great day!</p><br />");
            strBody.Append("<p>Customer Services Team</p>");
            strBody.Append("<p>leap-tel</p>");
            strBody.Append("<p>03330119880</p>");
            MailMessage objmail = new MailMessage();
            objmail.Subject = "Welcome to LEAP! ";
            objmail.Body = strBody.ToString();
            objmail.From = new MailAddress(EmailSettings.welcomeMail);

            foreach (string str in toMail.Split(','))
            {
                if (str.Contains("@"))
                    objmail.To.Add(new MailAddress(str));
            }

            objmail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();

            NetworkCredential credentioals = new NetworkCredential(EmailSettings.welcomeMail, EmailSettings.welcomeMailPwd);
            SendEmail(objmail, credentioals);
        }

        public static void SendForgotPasswordEmail(int shopId, string shopName, string shopEmail, string password, string ownerName, string shopGuid)
        {
            string toMail = EmailSettings.toEmail;
            if (!string.IsNullOrEmpty(shopEmail))
                toMail += "," + shopEmail;
            if (string.IsNullOrEmpty(ownerName) || ownerName == "NULL")
                ownerName = shopName;

            string activationLink = "https://www.leap-tel.com/Retailer/login?EmailId=" + shopGuid;

            StringBuilder strBody = new StringBuilder();
            strBody.Append("<p>Greetings, Mr " + ownerName + ", </p>");
            strBody.Append("<p>Thank you for signing up. Here is all the information you need for managing your leap-tel account online. </p><br />");
            strBody.Append("<p>Shop ID :" + shopId + "</p>");
            strBody.Append("<p>PASSWORD    :" + password + "</p>");
            strBody.Append("<p>Click on the below link to login to your Leap Tel account? </p>");
            strBody.Append("<p><a href='" + activationLink + "'>" + activationLink + "</a></p>");
            strBody.Append("<p>To order Mobile and Travel Accessories, click here " + activationLink + "</p><br />");
            strBody.Append("<p>For other services such as Mobile unlocking, Air ticketing  and Online mobile top-up services, please click here " + activationLink + "</p><br />");
            strBody.Append("<p>Have a great day!</p><br />");
            strBody.Append("<p>Customer Services Team</p>");
            strBody.Append("<p>leap-tel</p>");
            strBody.Append("<p>03330119880</p>");


            MailMessage objmail = new MailMessage();
            objmail.Subject = "leap-tel : Your Online Account is Live Now!";
            objmail.Body = strBody.ToString();
            objmail.From = new MailAddress(EmailSettings.infoMail);

            foreach (string str in toMail.Split(','))
            {
                if (str.Contains("@"))
                    objmail.To.Add(new MailAddress(str));
            }

            objmail.IsBodyHtml = true;
            NetworkCredential credentioals = new NetworkCredential(EmailSettings.infoMail, EmailSettings.infoMailPwd);
            SendEmail(objmail, credentioals);
        }

        public static void UserPasswordResetEmail(int userId, string userName, string email)
        {
            string toMail = EmailSettings.toEmail;
            if (!string.IsNullOrEmpty(email))
                toMail += "," + email;


            //string activationLink = "https://www.leap-tel.com/Retailer/login?EmailId=" + shopGuid;

            //StringBuilder strBody = new StringBuilder();
            //strBody.Append("<p>Greetings, Mr " + ownerName + ", </p>");
            //strBody.Append("<p>Thank you for signing up. Here is all the information you need for managing your leap-tel account online. </p><br />");
            //strBody.Append("<p>Shop ID :" + shopId + "</p>");
            //strBody.Append("<p>PASSWORD    :" + password + "</p>");
            //strBody.Append("<p>Click on the below link to login to your Leap Tel account? </p>");
            //strBody.Append("<p><a href='" + activationLink + "'>" + activationLink + "</a></p>");
            //strBody.Append("<p>To order Mobile and Travel Accessories, click here " + activationLink + "</p><br />");
            //strBody.Append("<p>For other services such as Mobile unlocking, Air ticketing  and Online mobile top-up services, please click here " + activationLink + "</p><br />");
            //strBody.Append("<p>Have a great day!</p><br />");
            //strBody.Append("<p>Customer Services Team</p>");
            //strBody.Append("<p>leap-tel</p>");
            //strBody.Append("<p>03330119880</p>");


            //MailMessage objmail = new MailMessage();
            //objmail.Subject = "leap-tel : Your Online Account is Live Now!";
            //objmail.Body = strBody.ToString();
            //objmail.From = new MailAddress(EmailSettings.infoMail);

            //foreach (string str in toMail.Split(','))
            //{
            //    if (str.Contains("@"))
            //        objmail.To.Add(new MailAddress(str));
            //}

            //objmail.IsBodyHtml = true;
            //NetworkCredential credentioals = new NetworkCredential(EmailSettings.infoMail, EmailSettings.infoMailPwd);
            //SendEmail(objmail, credentioals);
        }


        public static void SendVATInvoiceEmail(InvoiceDetailModel invoiceDetailModel)
        {

            string toMail = EmailSettings.toEmail;
            if (!string.IsNullOrEmpty(invoiceDetailModel.ShopEmail))
                toMail += "," + invoiceDetailModel.ShopEmail;

            MailMessage objmail = new MailMessage();
            objmail.Subject = "Leap_Invoice_" + invoiceDetailModel.OrderId;
            objmail.Body = "Your order has been successfully placed.";
            objmail.From = new MailAddress(EmailSettings.invoiceMail);


            foreach (string str in toMail.Split(','))
            {
                if (str.Contains("@"))
                    objmail.To.Add(new MailAddress(str));
            }
            var invoice = new PDFInvoice().GenerateInvoice(invoiceDetailModel, true);
            MemoryStream file = new MemoryStream(invoice);

            file.Seek(0, SeekOrigin.Begin);
            Attachment data = new Attachment(file, "Invoice_" + invoiceDetailModel.OrderId + ".pdf", "application/pdf");
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.DateTime.Now;
            disposition.ModificationDate = System.DateTime.Now;
            disposition.DispositionType = DispositionTypeNames.Attachment;
            objmail.Attachments.Add(data);//Attach the file  

            objmail.IsBodyHtml = true;
            NetworkCredential credentioals = new NetworkCredential(EmailSettings.invoiceMail, EmailSettings.invoiceMailPwd);
            SendEmail(objmail, credentioals);
        }

        private static void SendEmail(MailMessage objmail, NetworkCredential credentioals)
        {
            //smtp.Host = "relay-hosting.secureserver.net";
            SmtpClient smtp = new SmtpClient();
            smtp.Host = EmailSettings.host;
            smtp.EnableSsl = EmailSettings.enableSSL;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credentioals;
            //smtp.Port =25;
            smtp.Port = EmailSettings.port;
            smtp.Send(objmail);
        }

        public static string GeneratePassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            StringBuilder result = new StringBuilder();
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[1];
                for (int i = 0; i < length; i++)
                {
                    rng.GetBytes(buffer);
                    result.Append(chars[buffer[0] % chars.Length]);
                }
            }
            return result.ToString();
        }

    }
}
