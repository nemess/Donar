using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Donar.Interfaces;
using System.ComponentModel.Composition;

namespace PillarsOfEternity
{
    [Export(typeof(IImport))]
    [ExportMetadata("Name", "Pillars of Eternity")]
    class Import : IImport
    {
        bool IImport.Import(IDatabase database)
        {
            ITranslate trans = database.Translate;
            trans.Add("Test ID 1");
            trans.Add("Test ID 2");
            trans.Add("Test ID 3");
            IUnit unit = trans["Test ID 1"];
            unit[TextType.Source].FromString("One line");
            unit[TextType.Target].FromString("Egy sor");
            unit = trans["Test ID 2"];
            unit[TextType.Source].FromString("Two\nlines");
            unit[TextType.Target].FromString("Két\nsor");
            unit = trans["Test ID 3"];
            unit[TextType.Source].FromString("Multi\nlines\nexample\nThis asdlsd");
            unit[TextType.Target].FromString("Több\nsoros\npélsa\nEz slfkmnsfsden");
            return true;
        }
    }
}
