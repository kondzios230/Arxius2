using Arxius.DataAccess.PCL;
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
        public async static Task<T> Get<T>(object key, Func<Task<T>> delegat, bool clean = false)
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
                try
                {
                    dictionary[key] = await delegat();
                    return (T)dictionary[key];
                }
                catch (ArxiusDataException e)
                {
                    throw new ArxiusException(e);
                }
            }
        }
        public static void Clear(object key)
        {
            if (dictionary != null)
            {
                object ret;
                if (dictionary.TryGetValue(key, out ret))
                    dictionary.Remove(key);
            }

        }
    }
}
