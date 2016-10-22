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
    [Export(typeof(IExport))]
    [ExportMetadata("Name", "Baldur's Gate")]
    class Export : IExport
    {
        void IExport.Export(IDatabase database)
        {
            MessageBox.Show("Export!", "Baldur's Gate");
        }
    }
}
