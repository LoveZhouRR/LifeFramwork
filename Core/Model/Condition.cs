using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Condition
    {
        public string key { get; set; }
        public object value { get; set; }
        public Op op { get; set; }

        public Condition(string key, object value, Op op)
        {
            this.key = key;
            this.value = value;
            this.op = op;
        }
    }

    public class OpExpression
    {
        public OpExpression Left { get; set; }
        public OpExpression Right { get; set; }
        public JoinOP joinOp { get; set; }
        public Condition Condition { get; set; }

        public OpExpression(Condition condition)
        {
            Condition = condition;
            Left = null;
            Right = null;
            joinOp=JoinOP.Along;
        }

        public OpExpression(OpExpression left,OpExpression right,JoinOP joinOp)
        {
            this.Left = left;
            this.Right = right;
            this.joinOp = joinOp;
            this.Condition = null;
        }
    }

    public class Query
    {
        public OpExpression WhereExpression { get; set; }
        public string Orderby { get; set; }
        public bool ASC { get; set; }

    }

    public enum JoinOP
    {
        Along = 0,
        And=1,
        Or=2,
    }
    public enum Op
    {
        Equal=1,
        NotEqual=2,
    }
}
