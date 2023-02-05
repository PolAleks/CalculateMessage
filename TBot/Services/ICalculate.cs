using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBot.Services
{
    public interface ICalculate
    {
        string GetCount(string message, string method);
    }
}
