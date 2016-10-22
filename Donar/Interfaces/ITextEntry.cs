using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donar.Interfaces
{
    /// <summary>
    /// Reprecent on text, which can be 
    /// </summary>
    public interface ITextEntry : IList<IParagraph>
    {
        /// <summary>
        /// Reference to the container History object
        /// </summary>
        IUnit Unit { get; }

        /// <summary>
        /// Type of the text inside.
        /// Source or target language text, or other metadata
        /// </summary>
        TextType Type { get; }

        /// <summary>
        /// True when this string is modified
        /// </summary>
        bool IsModified { get; }

        /// <summary>
        /// It overwrite the whole Text object with the given string.
        /// It will parser to paragraphs based on the end of line charaters in the string.
        /// </summary>
        /// <param name="str">input string</param>
        void FromString(string str);

        /// <summary>
        /// Convert this object into string
        /// </summary>
        /// <param name="newLine"></param>
        /// <returns>Generated string from this Text object</returns>
        string ToString(string newLine);

        /// <summary>
        /// Insert an empty Paragraph to the given index.
        /// </summary>
        /// <param name="index">new empty paragraph will be on this index</param>
        /// <returns>Created new paragraph</returns>
        IParagraph Insert(int index);
    }
}
