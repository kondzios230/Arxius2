
using System.Collections.Generic;

namespace Arxius.CrossLayer.PCL
{
    public class GenericGroupedCollection<T1,T2> : List<T2>
    {
        public T1 Key { get; set; }
        public GenericGroupedCollection(T1 value)
        {
            Key = value;
        }
        public static IList<T2> All { private set; get; }
    }
}