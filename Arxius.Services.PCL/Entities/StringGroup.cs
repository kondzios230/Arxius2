using System;
using System.Collections.Generic;

namespace Arxius.Services.PCL.Entities
{
    public class StringGroup : List<string>
    {
        public string Key { get; set; }
        public StringGroup(string key)
        {
            Key = key;
        }
        public static IList<string> All { private set; get; }
    }
}
