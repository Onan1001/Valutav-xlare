using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valutaväxlare
{
    class Program
    {
        static void Main(string[] args)
        {
            string hej = "nikke";
            string valuta1 = null;
            string valuta2 = null;
            double tal = 0;


            if (args.Length == 3)
            {
                //0 från
                //1 till
                //2 summan
                tal = Convert.ToDouble(args[2]);
                valuta1 = args[0].ToLower();
                valuta2 = args[1].ToLower();

            }


            var validBills = new Dictionary<string, float[]>();
            var validCoins = new Dictionary<string, float[]>();
            var currencyRates = new Dictionary<string, Dictionary<string, float>>();

            string usd = "usd";
            string sek = "sek";
            string eur = "eur";

            var currencyNames = new Dictionary<string, string[]>
            {
                [usd] = new string[2] { "$", "Cent" },
                [sek] = new string[2] { "kr", "kr" },
                [eur] = new string[2] { "Eur", "Cent" }
            };
            validBills[sek] = new float[4] { 500, 100, 50, 20 };
            validCoins[sek] = new float[2] { 10, 1 };
            validBills[usd] = new float[7] { 100, 50, 20, 10, 5, 2, 1 };
            validCoins[usd] = new float[5] { 0.5F, 0.25F, 0.1F, 0.05F, 0.01F };
            validBills[eur] = new float[9] { 500, 200, 100, 50, 20, 10, 5, 2, 1 };
            validCoins[eur] = new float[6] { 0.5F, 0.20F, 0.10F, 0.05F, 0.02F, 0.01F };

            currencyRates[sek] = new Dictionary<string, float>();
            currencyRates[eur] = new Dictionary<string, float>();
            currencyRates[usd] = new Dictionary<string, float>();
            currencyRates[eur][sek] = 9.48F;
            currencyRates[sek][eur] = 1 / currencyRates[eur][sek];
            currencyRates[usd][sek] = 8.08F;
            currencyRates[sek][usd] = 1 / currencyRates[usd][sek];
            currencyRates[usd][eur] = 0.85F;
            currencyRates[eur][usd] = 1 / currencyRates[usd][eur];

            if (String.IsNullOrWhiteSpace(valuta1))
            {
                Console.Write("Dom här valutorna kan vi växla : Eur, Sek, Usd !");
                Console.WriteLine();

                Console.Write("mata in valuta 1:");
                valuta1 = Convert.ToString(Console.ReadLine());
                Console.Write("mata in valuta 2:");
                valuta2 = Convert.ToString(Console.ReadLine());
                Console.Write("Hur mycket vill du växla? :");

                tal = Convert.ToDouble(Console.ReadLine());


            }

            var result = Math.Round(tal * currencyRates[valuta1][valuta2], 2);
            Console.WriteLine(Math.Round(result, 2));


            var remainingExchange = result;

            var resultStrings = new List<string>();



            //här omvandlar jag totala summan till valörer via loops och statements
            foreach (var bill in validBills[valuta2])
            {
                var amountOfBills = 0;
                while (bill <= remainingExchange)
                {
                    amountOfBills++;
                    remainingExchange -= bill;
                }
                if (amountOfBills > 0)
                {
                    resultStrings.Add($"{amountOfBills}x{bill} {currencyNames[valuta2][0]}");
                }
            }

            float loopRemainingExchange = (float)Math.Round(remainingExchange, 2);

            foreach (var coin in validCoins[valuta2])
            {

                //deleteGhost är variabeln som avrundar "spököret´" rätt
                float deletedGhost = (float)Math.Round(loopRemainingExchange * 100);
                loopRemainingExchange = deletedGhost / 100;

                if(valuta2 == "sek")
                {
                    loopRemainingExchange = (float)Math.Round(loopRemainingExchange);
                }
               

                var amountOfCoins = 0;
                while (coin <= loopRemainingExchange)
                {
                    amountOfCoins++;
                    loopRemainingExchange -= coin;
                }
                if (amountOfCoins > 0)
                {
                    if(valuta2 == "sek" && coin == 1)
                    {
                        resultStrings.Add($"{amountOfCoins}x{coin}{currencyNames[valuta2][1]}");
                    }
                    else
                    {
                        resultStrings.Add($"{amountOfCoins}x{coin * 100}{currencyNames[valuta2][1]}");
                    }
                    
                }
            }

            Console.WriteLine(String.Join(", ", resultStrings));

            Console.ReadKey();
        }
    }
}
