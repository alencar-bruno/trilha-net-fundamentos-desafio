using System.Globalization;
using System.Text.RegularExpressions;

namespace DesafioFundamentos.Models
{
    public class Estacionamento
    {
        private decimal precoInicial = 0;
        private decimal precoPorHora = 0;
        private List<Car> veiculos = new List<Car>();

        public Estacionamento(decimal precoInicial, decimal precoPorHora)
        {
            this.precoInicial = precoInicial;
            this.precoPorHora = precoPorHora;
        }

        public void AdicionarVeiculo(bool inEnglish=false)
        {
            string placa = string.Empty;
            int tentativas = 3;

            do
            {
                Console.Clear();
                if (placa == string.Empty && tentativas < 3) Console.WriteLine(inEnglish ? "<INVALID CAR PLATE>\n" : "<PLACA DE CARRO INVÁLIDA>\n");

                Console.WriteLine(inEnglish ? $"[{tentativas}] Enter the parking-intended vehicle's plate:" : $"[{tentativas}] Digite a placa do veículo para estacionar:");
                string entrada = Console.ReadLine();

                placa = Car.ValidateCarPlate(entrada) ? entrada : string.Empty;
                tentativas--;

            } while (!Car.ValidateCarPlate(placa) && tentativas > 0);

            Console.Clear();
            if (placa != string.Empty)
            {
                if (!ChecarEstacionamento(placa))
                {
                    veiculos.Add(new Car(plateId: placa));
                    Console.WriteLine(inEnglish ? $"VEHICLE <{placa}> GRANTED ACCESS" : $"VEÍCULO <{placa}> LIBERADO");
                }
                else
                {
                    Console.WriteLine(inEnglish ? $"VEHICLE <{placa}> ALREADY AT THE PARKING LOT!" : $"VEÍCULO <{placa}> JÁ ESTACIONADO!");
                }

            }
            else
            {
                Console.WriteLine(inEnglish ? "LIMIT OF BAD ENTRIES EXCEEDED" : "LIMITE DE ENTRADAS INVÁLIDAS EXCEDIDO");
            }
        }

        public void RemoverVeiculo(bool inEnglish=false)
        {
            Console.Clear();
            if (!ChecarEstacionamento())
            {
                Console.WriteLine(inEnglish ? "THERE'S NO VEHICLE PARKED BY NOW." : "NENHUM VEÍCULO ESTACIONADO ATÉ ENTÃO.");
            }
            else
            {
                Console.WriteLine(inEnglish ? "Enter the removing-intended vehicle's plate:" : "Digite a placa do veículo para remover:");

                string placa = string.Empty;
                placa = Console.ReadLine();

                // Verifica se o veículo existe
                if (ChecarEstacionamento(placa: placa))
                {
                    Car carroDeSaida = veiculos.Find(car => car.PlateId == placa.ToUpper());

                    carroDeSaida.LeaveParkingLot(carroDeSaida.GetRandomParkingDuration());

                    int duracao = carroDeSaida.InParkingLotTime.Hours;
                    decimal valorTotal = CalcularTarifa(duracao: duracao);

                    veiculos.Remove(carroDeSaida);
                    Console.Clear();
                    ImprimirTicket(valorTarifa: valorTotal, placa: placa, duracao: duracao, inEnglish: inEnglish);
                }
                else
                {
                    Console.WriteLine(inEnglish ? "Beg your pardon, there's no such vehicle parked here. Check the plate number again." : "Desculpe, esse veículo não está estacionado aqui. Confira se digitou a placa corretamente");
                }
            }
        }

        public void ListarVeiculos(bool inEnglish=false)
        {
            Console.Clear();
            // Verifica se há veículos no estacionamento
            if (veiculos.Any())
            {
                Console.WriteLine(inEnglish ? "Those are all the vehicles parked:" : "Os veículos estacionados são:");
                foreach (Car item in veiculos)
                {
                    Console.WriteLine($"*\t[{item.PlateId}]");
                }
            }
            else
            {
                Console.WriteLine(inEnglish ? "There's no vehicle parked here" : "Não há veículos estacionados.");
            }
        }

        private bool ChecarEstacionamento(string placa = "")
        {
            if (placa == "") return veiculos.Any();

            return veiculos.Any(item => item.PlateId == placa.ToUpper());
        }

        private decimal CalcularTarifa(int duracao)
        {
            return precoInicial + precoPorHora * duracao;
        }
        
        private void ImprimirTicket(decimal valorTarifa, string placa, int duracao, string moedaBase="pt-BR", bool inEnglish=false)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(moedaBase);

            string templatePortugues = $"""
            +--------------------------------------------------------------------------------------------------+
                                             ALENCAR'S ESTACIONAMENTO

                                                [     TICKET      ]

                * VEÍCULO <{placa}>
                |
                >   TEMPO EM ESTACIONAMENTO: <{duracao}h>    

                * A PAGAR <{valorTarifa:C}>

            +--------------------------------------------------------------------------------------------------+
            """;

            string templateIngles = $"""
            +--------------------------------------------------------------------------------------------------+
                                                ALENCAR'S PARKING LOT

                                                [     TICKET      ]

                * VEHICLE <{placa}>
                |
                >   IN-PARKING-LOT DURATION: <{duracao}h>

                * BILL <{valorTarifa:C}>

            +--------------------------------------------------------------------------------------------------+
            """;

            Console.WriteLine(inEnglish ? templateIngles : templatePortugues);
        }
    }
}
