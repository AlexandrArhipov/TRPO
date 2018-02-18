using System;

namespace DateRegExp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            TestDateConveter();
            Console.WriteLine();
            TestRegExChecker();
        }

        public static void TestDateConveter()
        {
            string dateString = "2018-02-14T18:00:00+03:00";

            try
            {
                DateConverter dateConverter = new DateConverter(dateString);
                dateConverter.PrintTask();
            }
            catch (FormatException)
            {
                Console.WriteLine(dateString + " is not represent string in date format.");
            }
        }

        public static void TestRegExChecker()
        {
            const string correctDate = "26.05.2018";
            const string incorrectDate = "26.50.2018";

            Console.WriteLine(correctDate + " is " + (RegExChecker.CheckForDate(correctDate) ? "date." : "not date."));
            Console.WriteLine(incorrectDate + " is " + (RegExChecker.CheckForDate(incorrectDate) ? "date." : "not date."));
            
            const string correctDateAndTime = "26.05.2018 15:04";
            const string incorrectDateAndTime = "26.05.2018 24:01";

            Console.WriteLine(correctDateAndTime + " is " + (RegExChecker.CheckForDateAndTime(correctDateAndTime)
                                  ? "date and time."
                                  : "not date and time."));
            Console.WriteLine(incorrectDateAndTime + " is " + (RegExChecker.CheckForDateAndTime(incorrectDateAndTime)
                                  ? "date and time."
                                  : "not date and time."));
            
            const string correctEmail = "arhip.sasha.26@yandex.ru";
            const string incorrectEmail = ".arhip.sasha.26@yandex.ru";
            
            Console.WriteLine(correctEmail + " is " + (RegExChecker.CheckForEmail(correctEmail) ? "email." : "not email."));
            Console.WriteLine(incorrectEmail + " is " + (RegExChecker.CheckForEmail(incorrectEmail) ? "email." : "not email."));
        }
    }
}