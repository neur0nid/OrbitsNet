using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitsNet
{
    public class ButtonEventArgs : EventArgs
    {
        public string LabelText { get; set; }
        public int Level { get; set; }

        public ButtonEventArgs(string labelText, int level)
        {
            this.LabelText = labelText;
            this.Level = level;
        }
    }
}