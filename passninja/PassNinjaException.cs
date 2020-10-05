using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace passninja
{
    public class PassNinjaException : Exception
    {
        public PassNinjaException()
        {

        }
        public PassNinjaException(string message)
            : base(message)
        { 
        }
    }
}
