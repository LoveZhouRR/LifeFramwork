using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using Core.ORM;
using TestWork.Models;

namespace TestWork
{
    class Program
    {
        static void Main(string[] args)
        {
            Model model=new Model(){Id=10,Name = "life2",CreateDate = DateTime.Now};
            DbApi api=new DbApi();
            api.Update(model);
            //api.Insert(model);
            Condition condition=new Condition(){equalConditions = new Dictionary<string, object>(){{"Name","'life'"}}};
            var list=api.Select<Model>(condition);

            //api.Insert(model);
            api.Delete(model);
        }
    }
}
