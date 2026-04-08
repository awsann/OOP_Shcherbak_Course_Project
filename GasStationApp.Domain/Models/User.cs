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
        public static int BlockDurationMinutes { get; } = 5;

        private DateTime _blockedUntil;

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
            if (IsBlocked)
            {
                if (DateTime.Now < _blockedUntil)
                    return false;
                else
                    ResetFailedAttempts(); //блокування скінчилося
            }
            if (Login == login && Password == password)
            {
                ResetFailedAttempts();
                return true;
            }
            IncrementFailedAttempts();
            return false;
        }

        //Вийти з системи
        public virtual void LogOut() { }

        //Збільшити лічильник невдалих спроб
        public void IncrementFailedAttempts()
        {
            FailedAttempts++;
            if (FailedAttempts >= 3)
            {
                IsBlocked = true;
                _blockedUntil = DateTime.Now.AddMinutes(BlockDurationMinutes);
            }
        }

        //Скинути лічильник
        public void ResetFailedAttempts()
        {
            FailedAttempts = 0;
            IsBlocked = false;
        }
    }
}