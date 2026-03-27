using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Domain.Models
{
    public class Administrator : User
    {
        public Administrator(string login, string password)
            : base(login, password) { }

        public override string GetRole() => "Адміністратор";

        //Управління типами палива
        public bool AddFuelType(string name, double price, List<FuelType> fuelTypes)
        {
            throw new NotImplementedException();
        }

        public bool EditFuelType(int id, string newName, double newPrice, List<FuelType> fuelTypes)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFuelType(int id, List<FuelType> fuelTypes)
        {
            throw new NotImplementedException();
        }

        //Управління резервуарами
        public bool AddTank(int number, FuelType fuelType, double capacity, List<Tank> tanks)
        {
            throw new NotImplementedException();
        }

        public bool EditTank(int id, double newCapacity, List<Tank> tanks)
        {
            throw new NotImplementedException();
        }

        //Управління бонусними картками
        public BonusCard? CreateBonusCard(string fullName, string phone, List<BonusCard> bonusCards)
        {
            throw new NotImplementedException();
        }

        public bool EditBonusCard(string cardNumber, string newFullName, string newPhone, List<BonusCard> bonusCards)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBonusCard(string cardNumber, List<BonusCard> bonusCards)
        {
            throw new NotImplementedException();
        }

        //Звітність
        public Report GetFinancialReport(List<Sale> sales)
        {
            throw new NotImplementedException();
        }

        //Збереження/завантаження JSON
        public bool SaveDataToJson(string filePath, List<FuelType> fuelTypes, List<Tank> tanks, List<Sale> sales, List<BonusCard> bonusCards)
        {
            throw new NotImplementedException();
        }

        public bool LoadDataFromJson(string filePath, out List<FuelType> fuelTypes, out List<Tank> tanks, out List<Sale> sales, out List<BonusCard> bonusCards)
        {
            throw new NotImplementedException();
        }
    }
}