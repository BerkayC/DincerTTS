    using System;
    using System.Globalization;
    using System.IO;
    using EAGetMail; //add EAGetMail namespace

namespace TicketDesk.Web.Client.mailttoticket
{
    


    public class CreateTicketFromMail
    {
            // Generate an unqiue email file name based on date time
            static string _generateFileName(int sequence)
            {
                DateTime currentDateTime = DateTime.Now;
                return string.Format("{0}-{1:000}-{2:000}.eml",
                    currentDateTime.ToString("yyyyMMddHHmmss", new CultureInfo("tr-TR")),
                    currentDateTime.Millisecond,
                    sequence);
            }
        //    public void searchForValidTicket() 
        //{
        //    //OpenPoP---

        //    //var client = new POPClient();
        //    //client.Connect("pop.gmail.com", 995, true);
        //    //client.Authenticate("admin@bendytree.com", "YourPasswordHere");
        //    //var count = client.GetMessageCount();
        //    //Message message = client.GetMessage(count);
        //    //Console.WriteLine(message.Headers.Subject);
           
        //}
            public static void getMail()
            {
                try
                {
                    // Create a folder named "inbox" under current directory
                    // to save the email retrieved.
                    string localInbox = string.Format("{0}\\inbox", Directory.GetCurrentDirectory());
                    // If the folder is not existed, create it.
                    if (!Directory.Exists(localInbox))
                    {
                        Directory.CreateDirectory(localInbox);
                    }

                    MailServer oServer = new MailServer("pop3.emailarchitect.net",
                            "test@emailarchitect.net",
                            "testpassword",
                            ServerProtocol.Pop3);

                    // Enable SSL/TLS connection, most modern email server require SSL/TLS by default
                    oServer.SSLConnection = true;
                    oServer.Port = 995;

                    // if your server doesn't support SSL/TLS, please use the following codes
                    // oServer.SSLConnection = false;
                    // oServer.Port = 110;

                    MailClient oClient = new MailClient("TryIt");
                    oClient.Connect(oServer);
                   

                    MailInfo[] infos = oClient.GetMailInfos();
                    Console.WriteLine("Total {0} email(s)\r\n", infos.Length);
                    for (int i = 0; i < infos.Length; i++)
                    {
                        MailInfo info = infos[i];
                        Console.WriteLine("Index: {0}; Size: {1}; UIDL: {2}",
                        info.Index, info.Size, info.UIDL);

                        // Receive email from POP3 server
                        Mail oMail = oClient.GetMail(info);


                    

                        Console.WriteLine("From: {0}", oMail.From.ToString());
                        Console.WriteLine("Subject: {0}\r\n", oMail.Subject);

                        // Generate an unqiue email file name based on date time.
                        string fileName = _generateFileName(i + 1);
                        string fullPath = string.Format("{0}\\{1}", localInbox, fileName);
                        //oMail.

                        // Save email to local disk
                        oMail.SaveAs(fullPath, true);

                        // Mark email as deleted from POP3 server.
                        oClient.Delete(info);
                    }

                    // Quit and expunge emails marked as deleted from POP3 server.
                    oClient.Quit();
                    Console.WriteLine("Completed!");
                }
                catch (Exception ep)
                {
                    Console.WriteLine(ep.Message);
                }
            }
        }
    
}