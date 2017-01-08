using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arxius.Services.PCL.Interfaces
{
    public interface ISaveAndLoad
    {
        Task SaveTextAsync(string filename, string text);
        Task<string> LoadTextAsync(string filename);
        bool FileExists(string filename);
    }
}
