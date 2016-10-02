using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonarDatabase.Translate
{
    public interface IUnit
    {
        /// <summary>
        /// Parent translate database
        /// </summary>
        ITranslate Translate { get; }

        /// <summary>
        /// Identify the translation unit.
        /// It should not be empty or null.
        /// </summary>
        string UnitID { get; }

        /// <summary>
        /// It gives the Text based on the type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IText this[TextType type] { get; }

        /// <summary>
        /// True when any text is changed in this unit
        /// </summary>
        bool IsModified { get; }

        /// <summary>
        /// Save all modifications in this unit
        /// </summary>
        void Save();
    }
}
