using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    public enum PointerType
    {
        EMAIL, PHONE_NUMBER, IBAN
    }

    public struct Pointer
    {
        [JsonProperty("type"), JsonConverter(typeof(StringEnumConverter))]
        public PointerType Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public static Pointer Email(string address)
        {
            return new Pointer { Type = PointerType.EMAIL, Value = address };
        }

        public static Pointer Phone(string phone)
        {
            return new Pointer { Type = PointerType.PHONE_NUMBER, Value = phone };
        }

        public static Pointer IBAN(string iban, string name)
        {
            return new Pointer { Type = PointerType.IBAN, Value = iban, Name = name };
        }
    }
}
