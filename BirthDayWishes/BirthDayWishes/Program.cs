using System;


namespace BirthDayWishes
{
    class Program
    {
        static void Main(string[] args)
        {
            var sendMail = new SendEmail();
            sendMail.GetEmployee();
        }
    }
}

