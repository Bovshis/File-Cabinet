using System.Collections.Generic;

namespace FileCabinetApp.Services
{
    public class Storage
    {
        private readonly Dictionary<string, IList<int>> dictionary = new ();

        public bool Contains(string parameters)
        {
            return this.dictionary.ContainsKey(parameters);
        }

        public void Add(string parameters, IList<int> result)
        {
            this.dictionary.Add(parameters, result);
        }

        public IList<int> GetResult(string parameters)
        {
            return this.dictionary[parameters];
        }

        public void Clear()
        {
            this.dictionary.Clear();
        }
    }
}
