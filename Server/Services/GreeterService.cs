using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace Server
{
    public class GreeterService : Greeter.GreeterBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            DateTime birthDate = Convert.ToDateTime(request.Date);
            string sign = "";

            GetZodiacSign(birthDate, ref sign);

            return Task.FromResult(new HelloReply
            {
                Sign = sign
            }); ;
        }

        public void GetZodiacSign(DateTime birthDate, ref string sign)
        {
            string signs = File.ReadAllText("Signs.txt");
            int row = 0;
            int column;
            string[,] signIntervals = new string[12, 3];

            foreach (var line in signs.Split("\n"))
            {
                column = 0;

                foreach(var word in line.Trim().Split(" "))
                {
                    signIntervals[row, column] = word.Trim().ToString();
                    column++;
                }

                row++;
            }

            string year = "/" + birthDate.Year.ToString();

            for (int index = 0; index < signIntervals.GetLength(0); index++) 
            {
                string start = signIntervals[index, 0] + year;
                string end = signIntervals[index, 1] + year;
                
                if(Convert.ToDateTime(start) <= birthDate && birthDate <= Convert.ToDateTime(end))
                {
                    sign = signIntervals[index, 2];
                }
            }
        }
    }
}
