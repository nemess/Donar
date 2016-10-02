using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonarDatabase.Translate
{
    public interface ITranslate : IDictionary<string, IUnit>
    {
        /// <summary>
        /// Parent Donar database object
        /// </summary>
        IDonarDb Donar { get; }

        /// <summary>
        /// True when any Unit is modified in this Database
        /// </summary>
        bool IsModified { get; }

        /// <summary>
        /// Save all modified units
        /// </summary>
        void Save();

        /// <summary>
        /// Add a new empty Unit with the given ID.
        /// </summary>
        /// <param name="unitID"></param>
        void Add(string unitID);
    }
}
