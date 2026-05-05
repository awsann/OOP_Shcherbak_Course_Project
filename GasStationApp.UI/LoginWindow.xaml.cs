using GasStationApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GasStationApp.UI
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public List<FuelType> FuelTypes { get; } = new List<FuelType>();
        public List<Tank> Tanks { get; } = new List<Tank>();
        public List<Sale> Sales { get; } = new List<Sale>();
        public List<BonusCard> BonusCards { get; } = new List<BonusCard>();
        private List<User> _users;
        private bool _passwordVisible = false;

        public LoginWindow()
        {
            InitializeComponent();
            _users = new List<User>
            {
                new Administrator("admin", "Admin123"),
                new Manager("manager", "Manager123"),
                new Operator("operator", "Operator123")
            };
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtLogin.Text.Trim();
            string password = _passwordVisible ? TxtPasswordVisible.Text : TxtPassword.Password;
            var user = _users.FirstOrDefault(u => u.Login == login);
            if (user == null)
            {
                TxtError.Text = "Невірний логін або пароль";
                return;
            }
            bool success = user.LogIn(login, password);
            if (!success)
            {
                if (user.IsBlocked)
                    TxtError.Text = "Вхід заблоковано на 5 хвилин після 3 невдалих спроб";
                else
                    TxtError.Text = "Невірний логін або пароль";
                return;
            }
            TxtError.Text = "";
            TxtLogin.Clear();
            TxtPassword.Clear();
            if (user is Administrator admin)
            {
                var window = new AdminWindow(admin, FuelTypes, Tanks, Sales, BonusCards, this);
                this.Hide();
                window.Show();
            }
            else if (user is Manager manager)
            {
                var window = new ManagerWindow(manager, Tanks, Sales, BonusCards, this);
                this.Hide();
                window.Show();
            }
            else if (user is Operator op)
            {
                var window = new OperatorWindow(op, Tanks, Sales, BonusCards, this);
                this.Hide();
                window.Show();
            }
        }

        private void BtnTogglePassword_Click(object sender, RoutedEventArgs e)
        {
            _passwordVisible = !_passwordVisible;
            if (_passwordVisible)
            {
                TxtPasswordVisible.Text = TxtPassword.Password;
                TxtPassword.Visibility = Visibility.Collapsed;
                TxtPasswordVisible.Visibility = Visibility.Visible;
            }
            else
            {
                TxtPassword.Password = TxtPasswordVisible.Text;
                TxtPasswordVisible.Visibility = Visibility.Collapsed;
                TxtPassword.Visibility = Visibility.Visible;
            }
        }
    }
}