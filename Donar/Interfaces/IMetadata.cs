using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donar.Interfaces
{
    public interface IMetadata
    {
        /// <summary>
        /// Name of the implementation
        /// </summary>
        string Name { get; }
    }
}
