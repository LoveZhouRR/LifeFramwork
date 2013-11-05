using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace TestWork.Models
{
    public class Model:AbstractModel
    {
        //public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
