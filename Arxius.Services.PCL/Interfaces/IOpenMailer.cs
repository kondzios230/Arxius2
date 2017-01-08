using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arxius.Services.PCL.Interfaces
{
    public interface IOpenMailer
    {
       void  SendMail(string adress);
    }
}
