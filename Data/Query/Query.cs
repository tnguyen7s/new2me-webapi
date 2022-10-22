using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new2me_api.Data.Query
{
    public partial class Query:IQuery
    {
        private readonly New2meDataContext new2meDb;
        public Query(New2meDataContext new2meDb)
        {
            this.new2meDb = new2meDb;

        }
    }
}