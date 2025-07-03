using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAutoTrade2
{
    public static class Extension
    {
        public static void DoubleBuffered(this System.Windows.Forms.Control control, bool enabled)
        {
            var prop = control.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            prop.SetValue(control, enabled, null);
        }
    }
}
