using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitor.Data
{
    public class ModelRunning
    {
        public int RunId { get; set; }
        public bool isRunning { get; set; }
        public Modelo model { get; set; }
        public string PID { get; set; }
    }
}
