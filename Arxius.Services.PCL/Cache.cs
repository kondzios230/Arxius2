using Arxius.Services.PCL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arxius.Services.PCL
{
    public static class Cache
    {
        static Dictionary<object, object> dictionary;
        public async static Task<T> Get<T>(object key,Func<Task<T>>delegat, bool clean = false)
        {
            if (dictionary == null) dictionary = new Dictionary<object, object>();
            if (clean)
                dictionary[key] = await delegat();
            try
            {
                return (T)dictionary[key];
            }
            catch
            {
                dictionary[key] = await delegat();
                return (T)dictionary[key];
            }
        }
    }
}
