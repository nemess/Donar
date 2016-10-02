using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonarDatabase
{
    public class DatabaseMain
    {
        public static IDonarDb CreateDatabase()
        {
            return new DonarDbImp();
        }
    }
}
