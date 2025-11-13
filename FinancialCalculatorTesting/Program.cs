using System;
using System.Globalization;

namespace FinancialCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                ShowMainMenu();
                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        CreditCalculator();
                        break;
                    case "2":
                        CurrencyConverter();
                        break;
                    case "3":
                        DepositCalculator();
                        break;
                    case "4":
                        Console.WriteLine("До свидания!");
                        return;
                    default:
                        Console.WriteLine("Ошибка: выберите пункт от 1 до 4.");
                        break;
                }
                Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
                Console.ReadKey();
            }
        }

        static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ФИНАНСОВЫЙ КАЛЬКУЛЯТОР ===");
            Console.WriteLine("1. Расчет кредита");
            Console.WriteLine("2. Конвертер валют");
            Console.WriteLine("3. Калькулятор вкладов");
            Console.WriteLine("4. Выход");
            Console.Write("Выберите опцию: ");
        }

        #region === 1. Кредитный калькулятор ===
        static void CreditCalculator()
        {
            Console.Clear();
            Console.WriteLine("=== РАСЧЕТ КРЕДИТА ===");

            decimal amount = ReadDecimal("Сумма кредита (руб): ", 1000, 10000000);
            int months = ReadInt("Срок кредита (месяцев): ", 1, 360);
            decimal rate = ReadDecimal("Процентная ставка (% годовых): ", 0.1m, 99.9m);

            decimal monthlyRate = rate / 100 / 12;
            decimal payment = amount * monthlyRate * (decimal)Math.Pow(1 + (double)monthlyRate, months) /
                              ((decimal)Math.Pow(1 + (double)monthlyRate, months) - 1);

            decimal total = payment * months;
            decimal overpayment = total - amount;

            Console.WriteLine("\n--- РЕЗУЛЬТАТ ---");
            Console.WriteLine($"Ежемесячный платеж: {payment:F2} руб.");
            Console.WriteLine($"Общая сумма выплат: {total:F2} руб.");
            Console.WriteLine($"Переплата по кредиту: {overpayment:F2} руб.");
        }
        #endregion

        #region === 2. Конвертер валют ===
        static void CurrencyConverter()
        {
            Console.Clear();
            Console.WriteLine("=== КОНВЕРТЕР ВАЛЮТ ===");

            string[] currencies = { "RUB", "USD", "EUR" };
            Console.WriteLine("Доступные валюты: RUB, USD, EUR");

            string from = ReadCurrency("Исходная валюта: ");
            string to = ReadCurrency("Целевая валюта: ");
            decimal amount = ReadDecimal("Сумма для конвертации: ", 0.01m, 100000000);

            decimal result = ConvertCurrency(amount, from, to);

            Console.WriteLine($"\nРезультат: {amount:F2} {from} → {result:F2} {to}");
        }

        static decimal ConvertCurrency(decimal amount, string from, string to)
        {
            // Курсы относительно RUB
            const decimal usdToRub = 90.0m;
            const decimal eurToRub = 98.5m;

            decimal inRub;

            // Конвертация "от" → в RUB
            if (from == "RUB")
                inRub = amount;
            else if (from == "USD")
                inRub = amount * usdToRub;
            else if (from == "EUR")
                inRub = amount * eurToRub;
            else
                throw new InvalidOperationException("Неподдерживаемая исходная валюта: " + from);

            // Конвертация из RUB → "в"
            if (to == "RUB")
                return inRub;
            else if (to == "USD")
                return inRub / usdToRub;
            else if (to == "EUR")
                return inRub / eurToRub;
            else
                throw new InvalidOperationException("Неподдерживаемая целевая валюта: " + to);
        }
        #endregion

        #region === 3. Калькулятор вкладов ===
        static void DepositCalculator()
        {
            Console.Clear();
            Console.WriteLine("=== КАЛЬКУЛЯТОР ВКЛАДОВ ===");

            decimal amount = ReadDecimal("Сумма вклада (руб): ", 1000, 10000000);
            int months = ReadInt("Срок вклада (месяцев): ", 1, 360);
            decimal rate = ReadDecimal("Процентная ставка (% годовых): ", 0.1m, 99.9m);

            Console.Write("Тип вклада (1 - с капитализацией, 2 - без): ");
            string typeChoice = Console.ReadLine()?.Trim();
            bool withCapitalization = typeChoice == "1";

            decimal income, total;

            if (withCapitalization)
            {
                decimal monthlyRate = rate / 100 / 12;
                total = amount * (decimal)Math.Pow(1 + (double)monthlyRate, months);
                income = total - amount;
            }
            else
            {
                income = amount * rate * months / 12 / 100;
                total = amount + income;
            }

            Console.WriteLine("\n--- РЕЗУЛЬТАТ ---");
            Console.WriteLine($"Доход по вкладу: {income:F2} руб.");
            Console.WriteLine($"Итоговая сумма: {total:F2} руб.");
        }
        #endregion

        #region === Вспомогательные методы валидации ===
        static decimal ReadDecimal(string prompt, decimal min, decimal max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Ошибка: поле не может быть пустым.");
                    continue;
                }

                if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value))
                {
                    if (value >= min && value <= max)
                        return value;
                    else
                        Console.WriteLine($"Ошибка: значение должно быть от {min} до {max}.");
                }
                else
                {
                    Console.WriteLine("Ошибка: введите корректное число.");
                }
            }
        }

        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Ошибка: поле не может быть пустым.");
                    continue;
                }

                if (int.TryParse(input, out int value))
                {
                    if (value >= min && value <= max)
                        return value;
                    else
                        Console.WriteLine($"Ошибка: значение должно быть от {min} до {max}.");
                }
                else
                {
                    Console.WriteLine("Ошибка: введите целое число.");
                }
            }
        }

        static string ReadCurrency(string prompt)
        {
            string[] valid = { "RUB", "USD", "EUR" };
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim().ToUpper();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Ошибка: валюта не указана.");
                    continue;
                }

                if (Array.IndexOf(valid, input) >= 0)
                    return input;

                Console.WriteLine("Ошибка: поддерживаются только RUB, USD, EUR.");
            }
        }
        #endregion
    }
}