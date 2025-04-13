using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy
{
    class Query
    {
        public string Column { get; set; }
        public string Table { get; set; }
        public string Condition { get; set; }
        public string GroupBy { get; set; }
        public Query(string columns, string tables, string condition ="",string group_by="")
        {
            Column = columns;
            Table = tables;
            Condition = condition;
            GroupBy = group_by;
        }
        public Query(Query other)
        {
            this.Column = other.Column;
            this.Table = other.Table;
            this.Condition = other.Condition;
            this.GroupBy = other.GroupBy;
        }
    }
}
