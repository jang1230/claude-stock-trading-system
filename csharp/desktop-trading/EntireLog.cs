using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockAutoTrade2
{
    public partial class EntireLog : Form
    {
        public MainForm gMainForm = MainForm.GetInstance();


        public EntireLog()
        {
            InitializeComponent();

        }
        public void AppendLogTextToTextBox(string log)
        {
            string curTime = DateTime.Now.ToString("HH:mm:ss.ff");
            string newLogEntry = "[" + curTime + "] " + log + Environment.NewLine;

            if (logTextBox.InvokeRequired)
            {
                logTextBox.Invoke(new Action(() => {
                    logTextBox.AppendText(newLogEntry);
                }));
            }
            else
            {
                logTextBox.AppendText(newLogEntry);
            }
        }
    }
}
