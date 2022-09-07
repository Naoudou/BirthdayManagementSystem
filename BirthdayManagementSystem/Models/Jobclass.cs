using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace BirthdayManagementSystem.Models
{
    public class Jobclass : IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            throw new NotImplementedException();

            /* using (var message = new MailMessage("testuser@gmail.com", "testdestinationmail@gmail.com"))
             {
                 message.Subject = "Message Subject test";
                 message.Body = "Message body test at " + DateTime.Now;
                 using (SmtpClient client = new SmtpClient
                 {
                     EnableSsl = true,
                     Host = "smtp.gmail.com",
                     Port = 587,
                     Credentials = new NetworkCredential("testuser@gmail.com", "123546")
                 })
                 {
                     client.Send(message);
                 }
             }*/
        }

        async Task IJob.Execute(IJobExecutionContext context)
        {
            // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
             HttpClient client = new HttpClient();
            HttpResponseMessage response = await  client.GetAsync("http://localhost:60276/Sender/Send");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);

        }






    }

}