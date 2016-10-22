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
    [Export(typeof(IExport))]
    [ExportMetadata("Name", "Pillars of Eternity")]
    class Export : IExport
    {
        void IExport.Export(IDatabase database)
        {
            MessageBox.Show("Export!", "Pillars of Eternity");
        }
    }
}
