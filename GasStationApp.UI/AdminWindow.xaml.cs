using GasStationApp.Domain.Models;
using Microsoft.Win32;
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
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private Administrator _admin;
        private List<FuelType> _fuelTypes;
        private List<Tank> _tanks;
        private List<Sale> _sales;
        private List<BonusCard> _bonusCards;
        private LoginWindow _loginWindow;

        public AdminWindow(Administrator admin, List<FuelType> fuelTypes, List<Tank> tanks, List<Sale> sales, List<BonusCard> bonusCards, LoginWindow loginWindow)
        {
            InitializeComponent();
            _admin = admin;
            _fuelTypes = fuelTypes;
            _tanks = tanks;
            _sales = sales;
            _bonusCards = bonusCards;
            _loginWindow = loginWindow;
            RefreshAll();
        }

        private void RefreshAll()
        {
            DgFuelTypes.ItemsSource = null;
            DgFuelTypes.ItemsSource = _fuelTypes;
            DgTanks.ItemsSource = null;
            DgTanks.ItemsSource = _tanks;
            DgBonusCards.ItemsSource = null;
            DgBonusCards.ItemsSource = _bonusCards;
            CmbTankFuel.ItemsSource = null;
            CmbTankFuel.ItemsSource = _fuelTypes;
            CmbTankFuel.DisplayMemberPath = "Name";
        }

        //ВИХІД
        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            _admin.LogOut();
            _loginWindow.Show();
            this.Close();
        }

        //ТИПИ ПАЛИВА
        private void BtnAddFuel_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtFuelName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                TxtFuelMessage.Text = "Помилка: назва палива є обов'язковою";
                TxtFuelMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            if (!double.TryParse(TxtFuelPrice.Text.Trim(), out double price) || price <= 0)
            {
                TxtFuelMessage.Text = "Помилка: введіть коректну ціну більше нуля";
                TxtFuelMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            bool result = _admin.AddFuelType(name, price, _fuelTypes);
            if (result)
            {
                TxtFuelMessage.Text = "Тип палива успішно додано";
                TxtFuelMessage.Foreground = System.Windows.Media.Brushes.Green;
                TxtFuelName.Clear();
                TxtFuelPrice.Clear();
                RefreshAll();
            }
            else
            {
                TxtFuelMessage.Text = "Помилка: тип палива з такою назвою вже існує";
                TxtFuelMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void BtnEditFuel_Click(object sender, RoutedEventArgs e)
        {
            if (DgFuelTypes.SelectedItem is not FuelType selected)
            {
                TxtFuelMessage.Text = "Помилка: оберіть тип палива у таблиці";
                TxtFuelMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            string name = TxtFuelName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                TxtFuelMessage.Text = "Помилка: назва палива є обов'язковою";
                TxtFuelMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            if (!double.TryParse(TxtFuelPrice.Text.Trim(), out double price) || price <= 0)
            {
                TxtFuelMessage.Text = "Помилка: введіть коректну ціну більше нуля";
                TxtFuelMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            bool result = _admin.EditFuelType(selected.Id, name, price, _fuelTypes);
            if (result)
            {
                TxtFuelMessage.Text = "Тип палива успішно оновлено";
                TxtFuelMessage.Foreground = System.Windows.Media.Brushes.Green;
                TxtFuelName.Clear();
                TxtFuelPrice.Clear();
                RefreshAll();
            }
            else
            {
                TxtFuelMessage.Text = "Помилка: тип не знайдено або ціна некоректна";
                TxtFuelMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void BtnDeleteFuel_Click(object sender, RoutedEventArgs e)
        {
            if (DgFuelTypes.SelectedItem is not FuelType selected)
            {
                TxtFuelMessage.Text = "Помилка: оберіть тип палива у таблиці";
                TxtFuelMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            bool result = _admin.DeleteFuelType(selected.Id, _fuelTypes);
            if (result)
            {
                TxtFuelMessage.Text = "Тип палива успішно видалено";
                TxtFuelMessage.Foreground = System.Windows.Media.Brushes.Green;
                RefreshAll();
            }
            else
            {
                TxtFuelMessage.Text = "Помилка: тип не знайдено";
                TxtFuelMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        //РЕЗЕРВУАРИ
        private void BtnAddTank_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TxtTankNumber.Text.Trim(), out int number) || number <= 0)
            {
                TxtTankMessage.Text = "Помилка: введіть коректний номер резервуару більше нуля";
                TxtTankMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            if (CmbTankFuel.SelectedItem is not FuelType fuelType)
            {
                TxtTankMessage.Text = "Помилка: оберіть тип палива";
                TxtTankMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            if (!double.TryParse(TxtTankCapacity.Text.Trim(), out double capacity) || capacity <= 0)
            {
                TxtTankMessage.Text = "Помилка: введіть коректну місткість більше нуля";
                TxtTankMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            bool result = _admin.AddTank(number, fuelType, capacity, _tanks);
            if (result)
            {
                TxtTankMessage.Text = "Резервуар успішно додано";
                TxtTankMessage.Foreground = System.Windows.Media.Brushes.Green;
                TxtTankNumber.Clear();
                TxtTankCapacity.Clear();
                RefreshAll();
            }
            else
            {
                TxtTankMessage.Text = "Помилка: резервуар з таким номером вже існує";
                TxtTankMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void BtnEditTank_Click(object sender, RoutedEventArgs e)
        {
            if (DgTanks.SelectedItem is not Tank selected)
            {
                TxtTankMessage.Text = "Помилка: оберіть резервуар у таблиці";
                TxtTankMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            if (!double.TryParse(TxtTankCapacity.Text.Trim(), out double capacity) || capacity <= 0)
            {
                TxtTankMessage.Text = "Помилка: введіть коректну місткість більше нуля";
                TxtTankMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            bool result = _admin.EditTank(selected.Id, capacity, _tanks);
            if (result)
            {
                TxtTankMessage.Text = "Резервуар успішно оновлено";
                TxtTankMessage.Foreground = System.Windows.Media.Brushes.Green;
                TxtTankCapacity.Clear();
                RefreshAll();
            }
            else
            {
                TxtTankMessage.Text = "Помилка: залишок перевищує нову місткість";
                TxtTankMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void BtnRefillTank_Click(object sender, RoutedEventArgs e)
        {
            if (DgTanks.SelectedItem is not Tank selected)
            {
                TxtTankMessage.Text = "Помилка: оберіть резервуар у таблиці";
                TxtTankMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            if (!double.TryParse(TxtRefillAmount.Text.Trim(), out double liters))
            {
                TxtTankMessage.Text = "Помилка: введіть коректну кількість літрів";
                TxtTankMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            if (liters <= 0)
            {
                TxtTankMessage.Text = "Помилка: кількість літрів не може бути нульовою або від'ємною";
                TxtTankMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            bool result = selected.Refill(liters);
            if (result)
            {
                TxtTankMessage.Text = $"Резервуар №{selected.Number} поповнено. Залишок: {selected.CurrentLevel:F0} л";
                TxtTankMessage.Foreground = System.Windows.Media.Brushes.Green;
                TxtRefillAmount.Clear();
                RefreshAll();
            }
            else
            {
                TxtTankMessage.Text = "Помилка: кількість перевищує вільний об'єм резервуару";
                TxtTankMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        //БОНУСНІ КАРТКИ
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
            var card = _admin.CreateBonusCard(fullName, phone, _bonusCards);
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
            bool result = _admin.EditBonusCard(selected.CardNumber, fullName, phone, _bonusCards);
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

        private void BtnDeleteCard_Click(object sender, RoutedEventArgs e)
        {
            if (DgBonusCards.SelectedItem is not BonusCard selected)
            {
                TxtCardMessage.Text = "Помилка: оберіть картку у таблиці";
                TxtCardMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            bool result = _admin.DeleteBonusCard(selected.CardNumber, _bonusCards);
            if (result)
            {
                TxtCardMessage.Text = "Картку успішно видалено";
                TxtCardMessage.Foreground = System.Windows.Media.Brushes.Green;
                RefreshAll();
            }
            else
            {
                TxtCardMessage.Text = "Помилка: картку не знайдено";
                TxtCardMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        //ЗВІТ
        private void BtnFinancialReport_Click(object sender, RoutedEventArgs e)
        {
            var report = _admin.GetFinancialReport(_sales);
            TxtFinancialReport.Text = report.GenerateReport();
        }

        private void BtnOperatorStats_Click(object sender, RoutedEventArgs e)
        {
            if (!_sales.Any())
            {
                TxtOperatorStats.Text = "Немає даних про продажі";
                return;
            }
            var operators = _sales.Select(s => s.PerformedBy).Distinct().ToList();
            string result = "";
            foreach (var user in operators)
            {
                if (user is Operator op)
                {
                    var opSales = _sales.Where(s => s.PerformedBy == op).ToList();
                    double total = opSales.Sum(s => s.TotalAmount);
                    result += $"Оператор: {op.Login}\n";
                    result += $"  Час входу: {op.ShiftStartTime:dd.MM.yyyy HH:mm}\n";
                    result += $"  Час виходу: {(op.ShiftEndTime == default ? "ще не вийшов" : op.ShiftEndTime.ToString("dd.MM.yyyy HH:mm"))}\n";
                    result += $"  Кількість продажів: {opSales.Count}\n";
                    result += $"  Загальна виручка: {total:F2} грн\n\n";
                }
            }
            TxtOperatorStats.Text = result;
        }

        //JSON
        private void BtnSaveJson_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                FileName = "gasstation_data.json"
            };
            if (dialog.ShowDialog() == true)
            {
                bool result = _admin.SaveDataToJson(dialog.FileName, _fuelTypes, _tanks, _sales, _bonusCards);
                if (result)
                {
                    TxtJsonMessage.Text = "Дані успішно збережено";
                    TxtJsonMessage.Foreground = System.Windows.Media.Brushes.Green;
                }
                else
                {
                    TxtJsonMessage.Text = "Помилка: немає даних для збереження";
                    TxtJsonMessage.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
        }

        private void BtnLoadJson_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json"
            };
            if (dialog.ShowDialog() == true)
            {
                bool result = _admin.LoadDataFromJson(dialog.FileName,
                    out List<FuelType> fuelTypes,
                    out List<Tank> tanks,
                    out List<Sale> sales,
                    out List<BonusCard> bonusCards);
                if (result)
                {
                    _fuelTypes.Clear();
                    _fuelTypes.AddRange(fuelTypes);
                    _tanks.Clear();
                    _tanks.AddRange(tanks);
                    _sales.Clear();
                    _sales.AddRange(sales);
                    _bonusCards.Clear();
                    _bonusCards.AddRange(bonusCards);
                    TxtJsonMessage.Text = "Дані успішно завантажено";
                    TxtJsonMessage.Foreground = System.Windows.Media.Brushes.Green;
                    RefreshAll();
                }
                else
                {
                    TxtJsonMessage.Text = "Помилка: некоректний формат файлу";
                    TxtJsonMessage.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
        }
    }
}