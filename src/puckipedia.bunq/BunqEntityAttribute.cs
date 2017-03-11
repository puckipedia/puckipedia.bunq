using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace puckipedia.bunq
{
    public class BunqEntityAttribute : Attribute
    {
        public static Dictionary<string, Type> Types { get; private set; } = new Dictionary<string, Type>();
        public static Dictionary<Type, string> ReverseTypes { get; private set; } = new Dictionary<Type, string>();

        public string Name { get; private set; }

        public BunqEntityAttribute(string name)
        {
            Name = name;
        }

        static BunqEntityAttribute()
        {
            foreach (var type in typeof(BunqEntityAttribute).GetTypeInfo().Assembly.GetTypes())
            {
                var attr = type.GetTypeInfo().GetCustomAttribute<BunqEntityAttribute>();
                if (attr != null)
                {
                    Types[attr.Name] = type;
                    ReverseTypes[type] = attr.Name;
                }
            }
        }
    }
}
