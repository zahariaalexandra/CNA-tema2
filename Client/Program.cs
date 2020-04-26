using Grpc.Net.Client;
using Server;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            
            while(true)
            {
                DateTime date = new DateTime();
                bool valid = false;
                InsertDate(ref valid, ref date);
               
                if(valid)
                {
                    var input = new HelloRequest { Date = date.ToString() };
                    var reply = await client.SayHelloAsync(input);
                    Console.WriteLine("Your sign is: " + reply.Sign);
                }
                else
                {
                    Console.WriteLine("Invalid date format!");
                }
            }

        }

        private static void InsertDate(ref bool valid, ref DateTime date)
        {
            Console.WriteLine("Please insert your date of birth in MM/dd/yyyy format.");
            Console.Write("Date of birth: ");
            string input = Console.ReadLine();
            valid = ValidateDate(input, ref date);
        }

        private static bool ValidateDate(string date, ref DateTime result)
        {
            return DateTime.TryParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }
    }
}
