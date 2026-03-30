using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Domain.Models
{
    //Делегат і подія для низького рівня палива
    public delegate void LowFuelEventHandler(Tank tank, double currentLevel);

    public class Tank : ISaveable
    {
        public event LowFuelEventHandler? LowFuelWarning;

        public int Id { get; private set; }
        public int Number { get; private set; }
        public FuelType FuelType { get; private set; }
        public double Capacity { get; private set; }

        private double _currentLevel;
        public double CurrentLevel
        {
            get => _currentLevel;
            private set => _currentLevel = value;
        }

        public Tank(int number, FuelType fuelType, double capacity)
        {
            Number = number;
            FuelType = fuelType;
            Capacity = capacity;
            _currentLevel = 0;
        }

        //Поповнити резервуар
        public bool Refill(double liters)
        {
            throw new NotImplementedException();
            //Генерувати LowFuelWarning якщо залишок < 10%
        }

        //Зменшити залишок
        public bool Decrease(double liters)
        {
            throw new NotImplementedException();
        }

        //Перевірити низький рівень
        public bool IsLowLevel()
        {
            throw new NotImplementedException();
        }

        //Відсоток заповнення
        public double GetFillPercentage()
        {
            throw new NotImplementedException();
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

        public FuelType FuelType1
        {
            get => default;
            set
            {
            }
        }
    }
}