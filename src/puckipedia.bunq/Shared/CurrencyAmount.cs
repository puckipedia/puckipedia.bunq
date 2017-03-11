using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    public struct CurrencyAmount
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonIgnore]
        public decimal DecimalValue { get { return decimal.Parse(Value); } set { Value = value.ToString(); } }

        [JsonIgnore]
        public bool IsNegative => DecimalValue < 0;
        [JsonIgnore]
        public bool IsPositive => DecimalValue > 0;
        [JsonIgnore]
        public bool IsZero => DecimalValue == 0;

        private static void _guaranteeEqualCurrency(CurrencyAmount a, CurrencyAmount b)
        {
            if (a.Currency != b.Currency) throw new Exception("Can't add two CurrencyAmounts with differing values!");
        }

        public static CurrencyAmount operator +(CurrencyAmount a, CurrencyAmount b)
        {
            _guaranteeEqualCurrency(a, b);
            return new CurrencyAmount { Currency = a.Currency, DecimalValue = a.DecimalValue + b.DecimalValue };
        }

        public static CurrencyAmount operator -(CurrencyAmount a, CurrencyAmount b)
        {
            _guaranteeEqualCurrency(a, b);
            return new CurrencyAmount { Currency = a.Currency, DecimalValue = a.DecimalValue - b.DecimalValue };
        }

        public static CurrencyAmount operator *(CurrencyAmount a, CurrencyAmount b)
        {
            _guaranteeEqualCurrency(a, b);
            return new CurrencyAmount { Currency = a.Currency, DecimalValue = a.DecimalValue * b.DecimalValue };
        }

        public static CurrencyAmount operator /(CurrencyAmount a, CurrencyAmount b)
        {
            _guaranteeEqualCurrency(a, b);
            return new CurrencyAmount { Currency = a.Currency, DecimalValue = a.DecimalValue / b.DecimalValue };
        }

        public static CurrencyAmount operator *(CurrencyAmount a, decimal b)
        {
            return new CurrencyAmount { Currency = a.Currency, DecimalValue = a.DecimalValue * b };
        }

        public static CurrencyAmount operator /(CurrencyAmount a, decimal b)
        {
            return new CurrencyAmount { Currency = a.Currency, DecimalValue = a.DecimalValue / b };
        }

        public static CurrencyAmount operator -(CurrencyAmount a)
        {
            return new CurrencyAmount { Currency = a.Currency, DecimalValue = -a.DecimalValue };
        }

        public static CurrencyAmount EUR(decimal value)
        {
            return new CurrencyAmount { Currency = "EUR", Value = value.ToString() };
        }
    }
}
