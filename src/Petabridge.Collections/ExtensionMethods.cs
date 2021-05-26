using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Petabridge.Collections
{
    public static class ExtensionMethods
    {

        public static CircularBuffer<T> ToCircularBuffer<T>(this IEnumerable<T> list)
        {
            var eToList = list.ToList();
            var temp = new CircularBuffer<T>(eToList.Count());
            foreach (var variable in eToList)
                temp.Add(variable);
            return temp;
        }
        
    }
}
