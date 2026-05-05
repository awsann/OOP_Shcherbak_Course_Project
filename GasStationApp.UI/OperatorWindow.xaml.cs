using GasStationApp.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
    /// Логика взаимодействия для OperatorWindow.xaml
    /// </summary>
    public partial class OperatorWindow : Window
    {
        private Operator _operator;
        private List<Tank> _tanks;
        private List<Sale> _sales;
        private List<BonusCard> _bonusCards;
        private LoginWindow _loginWindow;
        private BonusCard? _currentCard = null;

        public OperatorWindow(Operator op, List<Tank> tanks, List<Sale> sales, List<BonusCard> bonusCards, LoginWindow loginWindow)
        {
            InitializeComponent();
            _operator = op;
            _tanks = tanks;
            _sales = sales;
            _bonusCards = bonusCards;
            _loginWindow = loginWindow;
            RefreshAll();
            foreach (var tank in _tanks)
            {
                tank.LowFuelWarning += (t, level) =>
                {
                    MessageBox.Show($"Увага! Низький рівень палива в резервуарі №{t.Number}: {level:F0} л ({t.FillPercentage:F1}%)", "Низький рівень палива", MessageBoxButton.OK, MessageBoxImage.Warning);
                };
            }
        }

        private void RefreshAll()
        {
            DgTanksOperator.ItemsSource = null;
            DgTanksOperator.ItemsSource = _tanks;
            CmbFuelType.ItemsSource = null;
            CmbFuelType.ItemsSource = _tanks.Select(t => t.FuelType).Distinct().ToList();
            CmbFuelType.DisplayMemberPath = "Name";
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            _operator.LogOut();
            _loginWindow.Show();
            this.Close();
        }

        private void BtnMakeSale_Click(object sender, RoutedEventArgs e)
        {
            TxtSaleMessage.Text = "";
            //перевірка типу палива
            if (CmbFuelType.SelectedItem is not FuelType selectedFuel)
            {
                TxtSaleMessage.Text = "Помилка: оберіть тип палива зі списку";
                TxtSaleMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            //перевірка кількості літрів
            string litersText = TxtLiters.Text.Trim();
            if (string.IsNullOrEmpty(litersText))
            {
                TxtSaleMessage.Text = "Помилка: введіть кількість літрів";
                TxtSaleMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            if (!double.TryParse(litersText, out double liters))
            {
                TxtSaleMessage.Text = "Помилка: кількість літрів має бути числом";
                TxtSaleMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            if (liters <= 0)
            {
                TxtSaleMessage.Text = "Помилка: кількість літрів має бути більше нуля";
                TxtSaleMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            //перевірка бонусної картки якщо введено
            BonusCard? card = null;
            string cardNumber = TxtSaleCardNumber.Text.Trim();
            if (!string.IsNullOrEmpty(cardNumber))
            {
                card = _operator.FindClient(cardNumber, _bonusCards);
                if (card == null)
                {
                    TxtSaleMessage.Text = "Помилка: картку з таким номером не знайдено";
                    TxtSaleMessage.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }
            }
            //перевірка списання бонусів якщо введено
            double redeemAmount = 0;
            string redeemText = TxtRedeemAmount.Text.Trim();
            if (!string.IsNullOrEmpty(redeemText))
            {
                if (card == null)
                {
                    TxtSaleMessage.Text = "Помилка: для списання бонусів спочатку введіть номер картки";
                    TxtSaleMessage.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }
                if (!double.TryParse(redeemText, out redeemAmount))
                {
                    TxtSaleMessage.Text = "Помилка: кількість бонусів для списання має бути числом";
                    TxtSaleMessage.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }
                if (redeemAmount <= 0)
                {
                    TxtSaleMessage.Text = "Помилка: кількість бонусів для списання має бути більше нуля";
                    TxtSaleMessage.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }
                if (card.BonusBalance < redeemAmount)
                {
                    TxtSaleMessage.Text = $"Помилка: на рахунку лише {card.BonusBalance:F2} бонусів, недостатньо для списання {redeemAmount:F2}";
                    TxtSaleMessage.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }
            }
            //перевірка резервуару
            var tank = _tanks.FirstOrDefault(t => t.FuelType == selectedFuel);
            if (tank == null)
            {
                TxtSaleMessage.Text = "Помилка: резервуар для обраного типу палива не знайдено";
                TxtSaleMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            if (tank.IsLowLevel())
            {
                TxtSaleMessage.Text = $"Помилка: залишок палива в резервуарі нижче 10% ({tank.CurrentLevel:F0} л). Продаж заборонено";
                TxtSaleMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            if (tank.CurrentLevel < liters)
            {
                TxtSaleMessage.Text = $"Помилка: недостатньо палива. В резервуарі лише {tank.CurrentLevel:F0} л";
                TxtSaleMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            //проводимо продаж
            var sale = _operator.MakeSale(selectedFuel, liters, card, _tanks);
            if (sale != null)
            {
                _sales.Add(sale);
                if (redeemAmount > 0 && card != null)
                    _operator.RedeemBonuses(card, redeemAmount);
                double toPay = sale.TotalAmount - redeemAmount;
                TxtSaleMessage.Text = $"Продаж проведено!\n" +
                                      $"Сума: {sale.TotalAmount:F2} грн\n" +
                                      $"Списано бонусів: {redeemAmount:F2}\n" +
                                      $"До оплати: {toPay:F2} грн\n" +
                                      $"Нараховано бонусів: {sale.AccruedBonuses:F2}";
                TxtSaleMessage.Foreground = System.Windows.Media.Brushes.Green;
                TxtLiters.Clear();
                TxtSaleCardNumber.Clear();
                TxtRedeemAmount.Clear();
                RefreshAll();
            }
            else
            {
                TxtSaleMessage.Text = "Помилка: не вдалося провести продаж";
                TxtSaleMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void BtnFindClient_Click(object sender, RoutedEventArgs e)
        {
            string cardNumber = TxtFindCard.Text.Trim();
            if (string.IsNullOrEmpty(cardNumber))
            {
                TxtCardMessage.Text = "Помилка: введіть номер картки";
                TxtCardMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }
            _currentCard = _operator.FindClient(cardNumber, _bonusCards);
            if (_currentCard != null)
            {
                TxtClientInfo.Text = $"ПІБ: {_currentCard.FullName}\n" +
                                     $"Телефон: {_currentCard.Phone}\n" +
                                     $"Баланс бонусів: {_currentCard.BonusBalance:F2}\n" +
                                     $"Рівень лояльності: {_currentCard.LoyaltyLevel}";
                TxtCardMessage.Text = "";
            }
            else
            {
                TxtClientInfo.Text = "";
                TxtCardMessage.Text = "Помилка: картку з таким номером не знайдено";
                TxtCardMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void BtnShiftReport_Click(object sender, RoutedEventArgs e)
        {
            var report = _operator.GetShiftReport(_sales);
            TxtShiftReport.Text = report.GenerateReport();
        }
    }
}