using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace passninja
{
    public class PassNinjaInvalidArgumentsException : PassNinjaException
    {
        public PassNinjaInvalidArgumentsException()
        {

        }
        public PassNinjaInvalidArgumentsException(string message)
            : base(message)
        {
             
        }
    }
}
