using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Mapper;
using Core.Model;
using Core.SqlMaker;

namespace Core.ORM
{
    public class DbApi
    {
        private readonly IMapper _mapper=new Mapper.Mapper();
        private readonly ISqlMaker _sqlMaker=new SqlServerMaker();

        public int Insert<T>(T model) where T : AbstractModel
        {
            var tableModel = _mapper.MapToTable(typeof(T),model);
            string sql=_sqlMaker.InsertMaker(tableModel);
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int Update<T>(T model) where T : AbstractModel
        {
            var tabelModel = _mapper.MapToTable(typeof(T), model);
            string sql = _sqlMaker.UpdateMaker(tabelModel);
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public void Delete<T>(T model) where T : AbstractModel
        {
            var tableModel = _mapper.MapToTable(typeof(T), model);
            string sql = _sqlMaker.DeleteMaker(tableModel);
            SqlHelper.ExecuteNonQuery(sql);
        }

        public IList<T> Select<T>(Condition condition)where T : AbstractModel,new ()
        {
            var tableModel = _mapper.MapToTable(typeof (T));
            string sql =_sqlMaker.SelectMaker(condition,tableModel.TableName);
            var ds = SqlHelper.ExecuteDataSet(sql);
            return _mapper.MapToModel<T>(ds);
        }

    }
}
