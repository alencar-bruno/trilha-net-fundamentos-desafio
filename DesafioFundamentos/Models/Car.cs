using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DesafioFundamentos.Models
{
    public class Car
    {
        public string PlateId
        {
            get => _plateId;
            set
            {
                if (ValidateCarPlate(value)) _plateId = value;
            }
        }
        public DateTime ArrivalTime { get; set; }
        public DateTime ExitTime { get; set; }

        private string _plateId;

        public Car(string plateId)
        {
            PlateId = plateId;
            ArrivalTime = DateTime.Now;
        }

        public bool LeaveParkingLot(DateTime? exitTime = null)
        {
            if (exitTime != null && exitTime < DateTime.Now) return false;

            ExitTime = (exitTime == null) ? DateTime.Now: (DateTime) exitTime;
            return true;
        }

        public static bool ValidateCarPlate(string plateId)
        {
            // Regex = "Regular Expression"
            Regex format = new Regex("^([A-Z]{3}-)+(\\d{1}[A-Z]{1}\\d{2})$");

            return format.IsMatch(plateId);
        }

        public DateTime GetRandomParkingDuration()
        {
            Random rand = new Random();
            int hoursPassed = rand.Next(1, 49);

            return ArrivalTime.AddHours(Convert.ToDouble(hoursPassed));
        }
    }
}