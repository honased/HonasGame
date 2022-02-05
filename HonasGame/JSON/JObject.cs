using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.JSON
{
    public class JObject
    {
        public Dictionary<string, object> Fields { get; private set; }

        public JObject()
        {
            Fields = new Dictionary<string, object>();
        }

        public object this[string key]
        {
            get
            {
                return Fields[key];
            }
        }

        public T GetValue<T>(string key)
        {
            object o = Fields[key];

            if(o is T retObj)
            {
                return retObj;
            }

            throw new Exception($"Object {o} is not of given type.");
        }
    }
}
