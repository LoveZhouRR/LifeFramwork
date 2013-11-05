using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class TableModel
    {
        public string TableName { get; set; }
        public int Id { get; set; }
        public Dictionary<string, object> rows { get; set; } 
    }
}
