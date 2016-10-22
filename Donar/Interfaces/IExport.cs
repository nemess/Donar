using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donar.Interfaces
{
    public interface IExport
    {
        /// <summary>
        /// It exports data from this database.
        /// It should not modify the database.
        /// </summary>
        /// <param name="database">Source database.</param>
        void Export(IDatabase database);
    }
}
