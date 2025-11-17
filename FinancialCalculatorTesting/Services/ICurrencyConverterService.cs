using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialCalculatorTesting.Services
{
    public interface ICurrencyConverterService
    {
        decimal Convert(decimal amount, string from, string to);
    }
}
