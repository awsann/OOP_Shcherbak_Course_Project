using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Domain.Models
{
    public abstract class User
    {
        //Характеристики
        public string Login { get; protected set; }
        protected string Password { get; set; }
        public int FailedAttempts { get; private set; }
        public bool IsBlocked { get; private set; }
        public static int BlockDurationMinutes { get; } = 5; //статична

        protected User(string login, string password)
        {
            Login = login;
            Password = password;
            FailedAttempts = 0;
            IsBlocked = false;
        }

        //Отримати роль — абстрактний (поліморфізм)
        public abstract string GetRole();

        //Увійти в систему
        public virtual bool LogIn(string login, string password)
        {
            throw new NotImplementedException();
        }

        //Вийти з системи
        public virtual void LogOut()
        {
            throw new NotImplementedException();
        }

        //Збільшити лічильник невдалих спроб
        public void IncrementFailedAttempts()
        {
            throw new NotImplementedException();
        }

        //Скинути лічильник
        public void ResetFailedAttempts()
        {
            throw new NotImplementedException();
        }
    }
}