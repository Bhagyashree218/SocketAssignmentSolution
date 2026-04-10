using System.Collections.Generic;

namespace ServerApp.Services
{
    public class DataService : IDataService
    {
        private readonly Dictionary<string, Dictionary<string, int>> data;

        public DataService()
        {
            data = new Dictionary<string, Dictionary<string, int>>
            {
                { "SetA", new Dictionary<string, int> { { "One", 1 }, { "Two", 2 } } },
                { "SetB", new Dictionary<string, int> { { "Three", 3 }, { "Four", 4 } } },
                { "SetC", new Dictionary<string, int> { { "Five", 5 }, { "Six", 6 } } },
                { "SetD", new Dictionary<string, int> { { "Seven", 7 }, { "Eight", 8 } } },
                { "SetE", new Dictionary<string, int> { { "Nine", 9 }, { "Ten", 10 } } }
            };
        }

        public int? GetValue(string setName, string keyName)
        {
            if (!data.ContainsKey(setName)) return null;
            if (!data[setName].ContainsKey(keyName)) return null;

            return data[setName][keyName];
        }
    }
}