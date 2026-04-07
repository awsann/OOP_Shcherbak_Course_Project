using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Domain.Models
{
    public class FuelType : ISaveable
    {
        private static int _nextId = 1;

        public int Id { get; private set; }
        public string Name { get; private set; }
        private double _pricePerLiter;

        public bool Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) return false;
            Name = newName;
            return true;
        }
        public double PricePerLiter
        {
            get => _pricePerLiter;
            private set => _pricePerLiter = value;
        }

        public FuelType(string name, double price)
        {
            Id = _nextId++;
            Name = name;
            _pricePerLiter = price;
        }

        //Встановити ціну
        public bool SetPrice(double newPrice)
        {
            throw new NotImplementedException();
        }

        //Рядкове представлення
        public override string ToString()
        {
            throw new NotImplementedException();
            //Формат: "А-95 — 56.50 грн/л"
        }

        //ISaveable
        public void SaveToJson(string filePath)
        {
            throw new NotImplementedException();
        }

        public void LoadFromJson(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}