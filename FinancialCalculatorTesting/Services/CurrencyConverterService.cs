using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialCalculatorTesting.Services
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private const decimal UsdToRub = 90.0m;
        private const decimal EurToRub = 98.5m;

        public decimal Convert(decimal amount, string from, string to)
        {
            decimal inRub;

            // Конвертация "от" → в RUB
            if (from == "RUB")
                inRub = amount;
            else if (from == "USD")
                inRub = amount * UsdToRub;
            else if (from == "EUR")
                inRub = amount * EurToRub;
            else
                throw new InvalidOperationException("Неподдерживаемая исходная валюта: " + from);

            // Конвертация из RUB → "в"
            if (to == "RUB")
                return inRub;
            else if (to == "USD")
                return inRub / UsdToRub;
            else if (to == "EUR")
                return inRub / EurToRub;
            else
                throw new InvalidOperationException("Неподдерживаемая целевая валюта: " + to);
        }
    }
}
