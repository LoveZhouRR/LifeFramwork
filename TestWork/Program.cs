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
            //Model model=new Model(){Id=10,Name = "life2",CreateDate = DateTime.Now};
            DbApi api=new DbApi();
            //api.Update(model);
            //api.Insert(model);
            Condition A1=new Condition("Id",1,Op.Equal);
            Condition A2=new Condition("Id",3,Op.Equal);
            Condition A3=new Condition("Name","life",Op.NotEqual);
            Query query = new Query()
                {
                    WhereExpression = new OpExpression(new OpExpression(new OpExpression(A1),new OpExpression(A2),JoinOP.Or ), new OpExpression(A3), JoinOP.And)
                };
            var list=api.Select<Model>(query);

            //api.Insert(model);
            //api.Delete(model);
        }
    }
}
