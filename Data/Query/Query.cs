using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new2me_api.Data.Query
{
    public partial class Query:IQuery
    {
        private readonly New2meDataContext datacontext;
        public Query(New2meDataContext datacontext)
        {
            this.datacontext = datacontext;

        }
    }
}