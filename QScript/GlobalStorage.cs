using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEditor.QScript
{
    static internal class GlobalStorage
    {
        private static Dictionary<string, object> storage = new Dictionary<string, object>();

        public static void Reset()
        {
            storage = new Dictionary<string, object>();
        }
        public static void Set(string name, object value)
        {
            storage.Add(name, value);
        }
        public static object Get(string name)
        {
            return storage[name];
        }
    }
}
