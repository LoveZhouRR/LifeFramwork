using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace Core.SqlMaker
{
    public interface ISqlMaker
    {
        string InsertMaker(TableModel o);
        string UpdateMaker(TableModel o);
        string DeleteMaker(TableModel o);
        string SelectMaker(Condition condition,string tableName);
    }
}
