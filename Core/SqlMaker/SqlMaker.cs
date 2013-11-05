using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace Core.SqlMaker
{
    public class SqlServerMaker:ISqlMaker
    {

        public string SelectMaker(Query query,string tableName)
        {
            StringBuilder sb=new StringBuilder();
            sb.Append("Select * from _TableName Where 1=1 And ");
            string queryStr = MakeWhereStr(query.WhereExpression);
            string orderBy = "";
            string ASC = query.ASC ? "ASC" : "DESC";
            sb.Append(queryStr);
            if (string.IsNullOrEmpty(query.Orderby))
            {
                orderBy = " Order By Id ASC";
            }
            else
            {
                orderBy = " Order By " + query.Orderby + ASC;
            }
            sb.Append(orderBy);
            sb.Replace("_TableName", tableName);
            return sb.ToString();
        }

        public string InsertMaker(Model.TableModel o)
        {
            StringBuilder sb=new StringBuilder();
            string columns = "";
            string values = "";
            sb.Append(@"Insert Into _TableName (");
            foreach (var row in o.rows)
            {
                if (row.Key == "Id"&&Convert.ToInt32(row.Value)==0)
                    continue;
                columns += row.Key+" ,";
                values += ToSqlString(row.Value) + ",";
            }
            columns = columns.TrimEnd(',');
            values = values.TrimEnd(',');
            sb.Append(columns).Append(") Values(").Append(values).Append(")");
            sb.Replace("_TableName",o.TableName );
            return sb.ToString();
        }

        public string UpdateMaker(Model.TableModel o)
        {
            StringBuilder sb=new StringBuilder();
            string sets = "";
            string IdInfo = "";
            sb.Append("Update _TableName Set ");
            foreach (var row in o.rows)
            {
                if (row.Key == "Id")
                    IdInfo = "Where Id =" + row.Value;
                else
                {
                    sets += row.Key + " = " + ToSqlString(row.Value) + ",";
                }    
            }
            sets = sets.TrimEnd(',');
            sb.Append(sets).Append(IdInfo);
            sb.Replace("_TableName", o.TableName);
            return sb.ToString();
        }

        public string DeleteMaker(Model.TableModel o)
        {
            StringBuilder sb=new StringBuilder();
            sb.Append("Delete _TableName ");
            string IdInfo = "";
            foreach (var row in o.rows)
            {
                if (row.Key == "Id")
                {
                    IdInfo = "Where Id =" + row.Value;
                    break;
                }
            }
            sb.Append(IdInfo);
            sb.Replace("_TableName", o.TableName);
            return sb.ToString();
        }

        private string ToSqlString(object o)
        {
            if (o is int)
                return o.ToString();
            else
            {
                return "'" + o.ToString().Replace("'","''") + "'";
            }
        }


        private string MakeWhereStr(OpExpression opExpression)
        {
            if (opExpression == null)
            {
                return "";
            }
            StringBuilder sb=new StringBuilder();

            switch ((int)opExpression.joinOp)
            {
                //叶子
                case 0:
                    sb.Append(opExpression.Condition.key)
  .Append(GetOpStr((int)opExpression.Condition.op))
  .Append(ToSqlString(opExpression.Condition.value));
                    break;
                case 1:
                    sb.Append(" ( ").Append(MakeWhereStr(opExpression.Left)).Append(" And ").Append(MakeWhereStr(opExpression.Right)).Append(" ) ");
                    break;
                case 2:
                    sb.Append(" ( ").Append(MakeWhereStr(opExpression.Left)).Append(" Or ").Append(MakeWhereStr(opExpression.Right)).Append(" ) ");
                    break;

            }
            return sb.ToString();
        }


        private string GetOpStr(int op)
        {
            string result = "";
            switch (op)
            {
                case 1:
                    result = " = ";
                    break;
                case 2:
                    result = "<>";
                    break;
            }
            return result;
        }
    }
}
