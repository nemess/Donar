using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Donar.Interfaces;
using System.ComponentModel.Composition;

namespace BaldursGate
{
    [Export(typeof(IImport))]
    [ExportMetadata("Name", "Baldur's Gate")]
    class Import : IImport
    {
        bool IImport.Import(IDatabase database)
        {
            ImportDialog dlg = new ImportDialog();
            dlg.ShowDialog();
            return false;
        }
    }
}
