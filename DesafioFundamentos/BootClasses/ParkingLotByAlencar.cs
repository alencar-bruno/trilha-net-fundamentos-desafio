using System.Globalization;
using DesafioFundamentos.Models;
using Newtonsoft.Json;

namespace DesafioFundamentos.BootClasses
{
    public class ParkingLotByAlencar : AbstractProgram
    {
        private decimal fixedTax = 0.00M;
        private decimal perHourTax = 0.00M;

        public override void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string filepath = "./Local/parkinglot-entries.js";
            var (succeeded, content) = ReadFile(filepath);

            if (succeeded)
            {
                ParkingLotSpecs specs = JsonConvert.DeserializeObject<ParkingLotSpecs>(content);
                fixedTax = specs.FixedTax;
                perHourTax = specs.PerHourTax;
            }
            else
            {
                Console.WriteLine($"FILE <{filepath}> NOT FOUND.");
                System.Environment.Exit(-1);
            }

            DisplayBanner();
            Console.WriteLine();
        }

        private void DisplayBanner(string culture="pt-BR")
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);

            Console.WriteLine($"""
            +--------------------------------------------------------------------------------------------------+
                                            WELCOME TO ALENCAR'S PARKING LOT

                    FIXED TAX: [{fixedTax:C}]                                TAX PER HOUR: [{perHourTax:C}]
            +--------------------------------------------------------------------------------------------------+
            """);
        }

        private (bool succeeded, string content) ReadFile(string path)
        {
            try
            {
                string localContent = File.ReadAllText(path: path);
                return (true, localContent);
            }
            catch
            {
                return (false, string.Empty);
            }
        } 
    }
}