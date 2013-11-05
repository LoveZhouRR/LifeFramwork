using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Core.Model;

namespace Core.Mapper
{
    public class Mapper:IMapper
    {

        public Model.TableModel MapToTable(Type type, object o = null)
        {
            TableModel tableModel=new TableModel();
            Dictionary<string,object> rows=new Dictionary<string, object>();
                //get Xml
            var xml = ReadXml(type);
            bool isUseConfig = xml != null;
            if (isUseConfig)
            {
                XmlNode root = xml.SelectSingleNode("Mapping");
                string tableName = root.SelectSingleNode("TableName").InnerText.Trim();
                tableModel.TableName = tableName;
            }
            else
            {
                tableModel.TableName = type.Name;
            }
            if (o != null)
            {
                var properties = o.GetType().GetProperties();
                foreach (var propertyInfo in properties)
                {
                    if (isUseConfig)
                    {
                        XmlNode root = xml.SelectSingleNode("Mapping");
                        XmlNode node = root.SelectSingleNode(propertyInfo.Name);
                        if (node != null)
                        {
                            rows.Add(node.InnerText.Trim(), propertyInfo.GetValue(o));
                        }
                        else
                        {
                            rows.Add(propertyInfo.Name, propertyInfo.GetValue(o));
                        }
                    }
                    else
                    {
                        rows.Add(propertyInfo.Name, propertyInfo.GetValue(o));
                    }
                }
                tableModel.rows = rows;
            }
            return tableModel;
        }


        public XmlDocument ReadXml(Type type)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@"../../Mappings/" + type.Name + ".xml");
                return doc;
            }
            catch (Exception)
            {
                return null;
            } 
        }


        public IList<T> MapToModel<T>(DataSet ds) where T : AbstractModel,new ()
        {
            List<T> response=new List<T>();
            var xml = ReadXml(typeof(T));
            bool isUseConfig = xml != null;
            if (HasMoreRow(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    T item=new T();
                    var properties = typeof (T).GetProperties();
                    foreach (var propertyInfo in properties)
                    {
                        string rowName = propertyInfo.Name;
                        if (isUseConfig)
                        {
                            XmlNode root = xml.SelectSingleNode("Mapping");
                            XmlNode node = root.SelectSingleNode(propertyInfo.Name);
                            if (node != null)
                            {
                                rowName = node.InnerText.Trim();
                            }
                        }
                        Type rowType = propertyInfo.GetType();
                        var value = row[rowName];
                        if(value!=DBNull.Value)
                            propertyInfo.SetValue(item,value);
                    }
                    response.Add(item);
                }
            }
            return response;
        }

        public bool HasMoreRow(DataSet ds)
        {
            if (ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                return true;
            else
            {
                return false;
            }
        }
    }
}
