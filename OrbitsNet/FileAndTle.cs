using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeptomoby.OrbitTools;

namespace OrbitsNet
{
    public class FileAndTle
    {
        public string FileName { get; set; }
        public Tle TwoLineElements { get; set; }

        public FileAndTle(string file, string id, string line1, string line2)
        {
            this.FileName = file;
            this.TwoLineElements = new Tle(id, line1, line2);
        }
    }
}
