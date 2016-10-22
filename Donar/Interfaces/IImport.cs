using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donar.Interfaces
{
    public interface IImport
    {
        /// <summary>
        /// Import data to this libaray.
        /// This function could show dialog box if it needs.
        /// </summary>
        /// <param name="database">Target database</param>
        /// <returns>it should be true when the database is modified.</returns>
        bool Import(IDatabase database);
    }
}
