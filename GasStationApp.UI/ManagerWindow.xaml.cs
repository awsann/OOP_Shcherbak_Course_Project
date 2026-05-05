using GasStationApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        private Manager _manager;
        private List<Tank> _tanks;
        private List<Sale> _sales;
        private List<BonusCard> _bonusCards;
        private LoginWindow _loginWindow;

        public ManagerWindow(Manager manager, List<Tank> tanks, List<Sale> sales, List<BonusCard> bonusCards, LoginWindow loginWindow)
        {
            InitializeComponent();
            _manager = manager;
            _tanks = tanks;
            _sales = sales;
            _bonusCards = bonusCards;
            _loginWindow = loginWindow;
            RefreshAll();
        }

        private void RefreshAll()
        {
            DgTanks.ItemsSource = null;
            DgTanks.ItemsSource = _tanks;
            DgBonusCards.ItemsSource = null;
            DgBonusCards.ItemsSource = _bonusCards;
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            _manager.LogOut();
            _loginWindow.Show();
            this.Close();
        }

        private void BtnRefill_Click(object sender, RoutedEventArgs e)
        {
            if (DgTanks.SelectedItem is not Tank selected)
            {
                TxtRefillMessage.Text = "Помилка: оберіть резервуар у таблиці";
                TxtRefillMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            if (!double.TryParse(TxtRefillAmount.Text.Trim(), out double liters))
            {
                TxtRefillMessage.Text = "Помилка: введіть коректну кількість літрів";
                TxtRefillMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            if (liters <= 0)
            {
                TxtRefillMessage.Text = "Помилка: кількість літрів не може бути нульовою або від'ємною";
                TxtRefillMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            bool result = _manager.RefillTank(selected, liters);
            if (result)
            {
                TxtRefillMessage.Text = $"Резервуар №{selected.Number} поповнено. Залишок: {selected.CurrentLevel:F0} л";
                TxtRefillMessage.Foreground = System.Windows.Media.Brushes.Green;
                TxtRefillAmount.Clear();
                RefreshAll();
            }
            else
            {
                TxtRefillMessage.Text = "Помилка: кількість перевищує вільний об'єм резервуару";
                TxtRefillMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void BtnCreateCard_Click(object sender, RoutedEventArgs e)
        {
            string fullName = TxtCardFullName.Text.Trim();
            string phone = TxtCardPhone.Text.Trim();
            if (string.IsNullOrWhiteSpace(fullName))
            {
                TxtCardMessage.Text = "Помилка: некоректний ПІБ";
                TxtCardMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            if (!BonusCard.IsValidPhone(phone))
            {
                TxtCardMessage.Text = "Помилка: некоректний формат телефону. Використовуйте +38(0XX)-XXX-XX-XX";
                TxtCardMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            if (_bonusCards.Any(c => c.Phone == phone))
            {
                TxtCardMessage.Text = "Помилка: картка з таким номером телефону вже існує";
                TxtCardMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            var card = _manager.CreateBonusCard(fullName, phone, _bonusCards);
            if (card != null)
            {
                TxtCardMessage.Text = $"Картку успішно створено. Номер: {card.CardNumber}";
                TxtCardMessage.Foreground = System.Windows.Media.Brushes.Green;
                TxtCardFullName.Clear();
                TxtCardPhone.Clear();
                RefreshAll();
            }
            else
            {
                TxtCardMessage.Text = "Помилка: не вдалося створити картку";
                TxtCardMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void BtnEditCard_Click(object sender, RoutedEventArgs e)
        {
            if (DgBonusCards.SelectedItem is not BonusCard selected)
            {
                TxtCardMessage.Text = "Помилка: оберіть картку у таблиці";
                TxtCardMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            string fullName = TxtCardFullName.Text.Trim();
            string phone = TxtCardPhone.Text.Trim();
            if (string.IsNullOrWhiteSpace(fullName))
            {
                TxtCardMessage.Text = "Помилка: некоректний ПІБ";
                TxtCardMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            if (!BonusCard.IsValidPhone(phone))
            {
                TxtCardMessage.Text = "Помилка: некоректний формат телефону. Використовуйте +38(0XX)-XXX-XX-XX";
                TxtCardMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            bool result = _manager.EditBonusCard(selected.CardNumber, fullName, phone, _bonusCards);
            if (result)
            {
                TxtCardMessage.Text = "Картку успішно оновлено";
                TxtCardMessage.Foreground = System.Windows.Media.Brushes.Green;
                TxtCardFullName.Clear();
                TxtCardPhone.Clear();
                RefreshAll();
            }
            else
            {
                TxtCardMessage.Text = "Помилка: картку не знайдено";
                TxtCardMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
            var report = _manager.GetGeneralStatistics(_sales);
            TxtStatistics.Text = report.GenerateReport();
        }
    }
}