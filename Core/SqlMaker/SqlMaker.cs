using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SqlMaker
{
    public class SqlServerMaker:ISqlMaker
    {

        public string SelectMaker(Model.Condition condition,string tableName)
        {
            StringBuilder sb=new StringBuilder();
            sb.Append("Select * from _TableName Where 1=1 ");
            string equal = "";
            string orderBy = "";
            string ASC = condition.ASC ? "ASC" : "DESC";
            foreach (var equalCondition in condition.equalConditions)
            {
                equal += " And " + equalCondition.Key + " = " + ToSqlString(equalCondition.Value);
            }
            sb.Append(equal);
            if (string.IsNullOrEmpty(condition.OrderBy))
            {
                orderBy = " Order By Id ASC";
            }
            else
            {
                orderBy = " Order By " + condition.OrderBy + ASC;
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
    }
}
