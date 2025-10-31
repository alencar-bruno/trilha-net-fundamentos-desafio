using System.Globalization;
using System.Text.RegularExpressions;

namespace DesafioFundamentos.Models
{
    public class Estacionamento
    {
        private decimal precoInicial = 0;
        private decimal precoPorHora = 0;
        // TODO: CHANGE LIST <veiculos> INTO A LIST OF OBJECTS <Car>
        private List<string> veiculos = new List<string>();

        public Estacionamento(decimal precoInicial, decimal precoPorHora)
        {
            this.precoInicial = precoInicial;
            this.precoPorHora = precoPorHora;
        }

        public void AdicionarVeiculo(bool inEnglish=false)
        {
            string placa = string.Empty;

            do
            {
                Console.Clear();
                Console.WriteLine(inEnglish ? "Enter the parking-intended vehicle's plate:" : "Digite a placa do veículo para estacionar:");
                string entrada = Console.ReadLine();

                placa = ValidarPlaca(entrada) ? entrada : string.Empty;

            } while (!ValidarPlaca(placa));

            veiculos.Add(placa);
        }

        public void RemoverVeiculo(bool inEnglish=false)
        {
            if (!ChecarEstacionamento())
            {
                Console.Clear();
                Console.WriteLine(inEnglish ? "THERE'S NO VEHICLE PARKED BY NOW." : "NENHUM VEÍCULO ESTACIONADO ATÉ ENTÃO.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine(inEnglish ? "Enter the removing-intended vehicle's plate:" : "Digite a placa do veículo para remover:");

                string placa = string.Empty;
                placa = Console.ReadLine();

                // Verifica se o veículo existe
                if (ChecarEstacionamento(placa: placa))
                {
                    Console.WriteLine(inEnglish ? "Enter how long the vehicle's been parked:" : "Digite a quantidade de horas que o veículo permaneceu estacionado:");

                    int horas = Convert.ToInt32(Console.ReadLine());
                    decimal valorTotal = CalcularTarifa(duracao: horas);

                    veiculos.Remove(placa);
                    Console.Clear();
                    ImprimirTicket(valorTarifa: valorTotal, placa: placa, inEnglish: inEnglish);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine(inEnglish ? "Beg your pardon, there's no such vehicle parked here. Check the plate number again." : "Desculpe, esse veículo não está estacionado aqui. Confira se digitou a placa corretamente");
                }
            }
        }

        public void ListarVeiculos(bool inEnglish=false)
        {
            // Verifica se há veículos no estacionamento
            if (veiculos.Any())
            {
                Console.WriteLine(inEnglish ? "Those are all the vehicles parked:" : "Os veículos estacionados são:");
                foreach (string item in veiculos)
                {
                    Console.WriteLine($"*\t[{item}]");
                }
            }
            else
            {
                Console.WriteLine(inEnglish ? "There's no vehicle parked here" : "Não há veículos estacionados.");
            }
        }

        private bool ValidarPlaca(string placa)
        {
            // Regex = "Regular Expression"
            Regex formato = new Regex("^([A-Z]{3}-)+(\\d{1}[A-Z]{1}\\d{2})$");

            return formato.IsMatch(placa);
        }

        private bool ChecarEstacionamento(string placa = "")
        {
            if (placa == "") return veiculos.Any();

            return veiculos.Any(item => item.ToUpper() == placa.ToUpper());
        }

        private decimal CalcularTarifa(int duracao)
        {
            return precoInicial + precoPorHora * duracao;
        }
        
        private void ImprimirTicket(decimal valorTarifa, string placa, string moedaBase="pt-BR", bool inEnglish=false)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(moedaBase);

            string templatePortugues = $"""
            +--------------------------------------------------------------------------------------------------+
                                             ALENCAR'S ESTACIONAMENTO

                                                [     TICKET      ]

                * VEÍCULO <{placa}> <----------------------------------------------> A PAGAR <{valorTarifa:C}>

            +--------------------------------------------------------------------------------------------------+
            """;

            string templateIngles = $"""
            +--------------------------------------------------------------------------------------------------+
                                                ALENCAR'S PARKING LOT

                                                [     TICKET      ]

                * VEHICLE <{placa}> <------------------------------------------------> BILL <{valorTarifa:C}>

            +--------------------------------------------------------------------------------------------------+
            """;

            Console.WriteLine(inEnglish ? templateIngles : templatePortugues);
        }
    }
}
