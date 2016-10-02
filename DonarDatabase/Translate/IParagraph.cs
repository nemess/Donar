using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonarDatabase.Translate
{
    /// <summary>
    /// One Paragraph (Line) from the translation text
    /// </summary>
    public interface IParagraph
    {
        /// <summary>
        /// Reference to the container Text object
        /// </summary>
        IText Text { get; }

        /// <summary>
        /// Zero based index of the paragraph in the text.
        /// </summary>
        int ParagraphIndex { get; }

        /// <summary>
        /// If it true, the Paragraph cannot be written (historical data)
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Paragrpah text. It can be empty, but it cannot be null.
        /// It cannot contains end of line charaters (\n or \r).
        /// </summary>
        string Paragraph { get; set; }

        bool IsModified { get; }
    }
}
