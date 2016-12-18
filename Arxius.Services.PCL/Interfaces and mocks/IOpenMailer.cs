using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arxius.Services.PCL.Interfaces_and_mocks
{
    public interface IOpenMailer
    {
       void  SendMail(string adress);
    }
}
