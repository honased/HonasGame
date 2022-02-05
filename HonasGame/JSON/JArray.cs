using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.JSON
{
    public class JArray : IEnumerable<object>
    {
        public List<object> Array { get; private set; }

        public JArray()
        {
            Array = new List<object>();
        }

        public int Count => Array.Count;

        public object this[int index]
        {
            get
            {
                return Array[index];
            }
        }

        public T GetValue<T>(int index)
        {
            object o = Array[index];

            if (o is T retObj)
            {
                return retObj;
            }

            throw new Exception($"Object {o} is not of given type.");
        }

        public IEnumerator<object> GetEnumerator()
        {
            foreach(object o in Array)
            {
                yield return o;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
