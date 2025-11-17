using System;
using Xunit;
using Moq;
using FinancialCalculatorTesting.Services;

namespace FinancialCalculator.Tests
{
    public class FinancialCalculatorTests
    {
        private static decimal AnnuityPayment(decimal amount, int months, decimal rate)
        {
            decimal r = rate / 1200m;
            if (r == 0) return amount / months;
            decimal pow = (decimal)Math.Pow(1 + (double)r, months);
            return amount * r * pow / (pow - 1);
        }

        private static decimal DepositWithCap(decimal amount, int months, decimal rate)
        {
            decimal r = rate / 1200m;
            return amount * (decimal)Math.Pow(1 + (double)r, months);
        }

        [Theory]
        [InlineData(100000, 12, 10, 8791.59)]
        [InlineData(500000, 60, 15, 11894.97)]
        [InlineData(200000, 24, 8, 9045.46)]
        public void CreditCalculator_VariousInputs_ReturnsCorrectPayment(
            decimal amount, int months, decimal rate, decimal expected)
        {
            var actual = AnnuityPayment(amount, months, rate);
            Assert.Equal(expected, actual, 2);
        }

        [Fact]
        public void DepositCalculator_WithCapitalization_ReturnsCorrectTotal()
        {
            var total = DepositWithCap(100000m, 12, 10m);
            Assert.Equal(110471.31m, total, 2);
        }

        [Theory]
        [InlineData(500000, 36, 12, true, 715384.39)]
        [InlineData(200000, 6, 5, false, 205000.00)]
        public void DepositCalculator_VariousScenarios_ReturnsCorrectTotal(
            decimal amount, int months, decimal rate, bool withCap, decimal expected)
        {
            decimal total = withCap
                ? DepositWithCap(amount, months, rate)
                : amount + (amount * rate * months / 1200m);

            Assert.Equal(expected, total, 2);
        }

        [Theory]
        [InlineData(100, "RUB", "USD", 1.1111111111)]
        [InlineData(100, "USD", "RUB", 9000)]
        [InlineData(500, "EUR", "USD", 547.22222222)]
        public void CurrencyConverter_WorksCorrectly(decimal amount, string from, string to, decimal expected)
        {
            var result = Program.ConvertCurrency(amount, from, to);
            Assert.Equal(expected, result, 8);
        }

        //  Moq + Verify
        [Fact]
        public void CurrencyConverter_CallsService_ExactlyOnce()
        {
            var mock = new Mock<ICurrencyConverterService>();
            mock.Setup(m => m.Convert(999m, "RUB", "USD"))
                .Returns(11.1m)
                .Verifiable();

            // Внедряем мок через рефлексию
            var field = typeof(Program).GetField("_converter",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var original = field.GetValue(null);
            field.SetValue(null, mock.Object);

            try
            {
                Program.ConvertCurrency(999m, "RUB", "USD");
            }
            finally
            {
                field.SetValue(null, original); // возвращаем оригинал
            }

            mock.Verify(m => m.Convert(999m, "RUB", "USD"), Times.Once);
        }
    }
}