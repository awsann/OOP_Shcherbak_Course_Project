using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GasStationApp.Domain.Models
{
    //Делегат і подія для низького рівня палива
    public delegate void LowFuelEventHandler(Tank tank, double currentLevel);

    public class Tank : ISaveable
    {
        private static int _nextId = 1;

        public event LowFuelEventHandler? LowFuelWarning;

        public int Id { get; private set; }
        public int Number { get; private set; }
        public FuelType FuelType { get; private set; }
        public double Capacity { get; internal set; }

        private double _currentLevel;
        public double CurrentLevel
        {
            get => _currentLevel;
            private set => _currentLevel = value;
        }

        public Tank(int number, FuelType fuelType, double capacity)
        {
            Id = _nextId++;
            Number = number;
            FuelType = fuelType;
            Capacity = capacity;
            _currentLevel = 0;
        }

        public static void ResetIdCounter(int startFrom)
        {
            _nextId = startFrom;
        }

        //Поповнити резервуар
        public bool Refill(double liters)
        {
            if (liters <= 0)
                return false;
            if (_currentLevel + liters > Capacity)
                return false;
            _currentLevel += liters;
            if (IsLowLevel())
                LowFuelWarning?.Invoke(this, _currentLevel);
            return true;
        }

        //Зменшити залишок
        public bool Decrease(double liters)
        {
            if (liters <= 0)
                return false;
            if (_currentLevel < liters)
                return false;
            _currentLevel -= liters;
            if (IsLowLevel())
                LowFuelWarning?.Invoke(this, _currentLevel);
            return true;
        }

        //Перевірити низький рівень
        public bool IsLowLevel()
        {
            return _currentLevel <= Capacity * 0.10;
        }

        //Відсоток заповнення
        public double GetFillPercentage()
        {
            return (_currentLevel / Capacity) * 100.0;
        }

        //ISaveable
        public void SaveToJson(string filePath)
        {
            var json = JsonSerializer.Serialize(new { Id, Number, Capacity, CurrentLevel });
            File.WriteAllText(filePath, json);
        }

        public void LoadFromJson(string filePath)
        {
            //реалізується через Administrator.LoadDataFromJson
        }
    }
}