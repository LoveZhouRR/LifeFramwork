using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Condition
    {
        public Dictionary<string, object> equalConditions { get; set; }
        public string OrderBy { get; set; }
        public bool ASC { get; set; }
    }
}
