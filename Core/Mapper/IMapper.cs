using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace Core.Mapper
{
    public interface IMapper
    {
        TableModel MapToTable(Type type,object o=null);
        IList<T> MapToModel<T>(DataSet ds)where T:AbstractModel,new ();
    }
}
