using System.Globalization;
using DesafioFundamentos.Models;
using Newtonsoft.Json;

namespace DesafioFundamentos.BootClasses
{
    public class ParkingLotByAlencar : AbstractProgram
    {
        private decimal fixedTax = 0.00M;
        private decimal perHourTax = 0.00M;
        private Estacionamento parkingLot;

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

                parkingLot = new Estacionamento(precoInicial: fixedTax, precoPorHora: perHourTax);
            }
            else
            {
                Console.WriteLine($"FILE <{filepath}> NOT FOUND.");
                System.Environment.Exit(-1);
            }

            DisplayMenu();
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

        private void DisplayMenu()
        {
            bool menuOn = true;

            // Realiza o loop do menu
            while (menuOn)
            {
                Console.Clear();
                DisplayBanner();
                Console.WriteLine("SELECT AN OPTION: ");
                Console.WriteLine("1 - REGISTER a Vehicle");
                Console.WriteLine("2 - REMOVE a Vehicle");
                Console.WriteLine("3 - SHOW ALL Vehicles");
                Console.WriteLine("4 - EXIT");

                switch (Console.ReadLine())
                {
                    case "1":
                        parkingLot.AdicionarVeiculo(inEnglish: true);
                        break;

                    case "2":
                        parkingLot.RemoverVeiculo(inEnglish: true);
                        break;

                    case "3":
                        parkingLot.ListarVeiculos(inEnglish: true);
                        break;

                    case "4":
                        menuOn = false;
                        break;

                    default:
                        Console.WriteLine("NOT AN OPTION");
                        break;
                }

                Console.WriteLine("PRESS ANY KEY TO CONTINUE...");
                Console.ReadLine();
            }

            Console.WriteLine("END OF PROGRAM");
        }
    }
}