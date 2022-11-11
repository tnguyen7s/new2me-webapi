using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using new2me_api.Data;
using new2me_api.Data.Query;

namespace new2me_api.UnitTesting.QueryTests
{
    public abstract class New2MeQueryTestBase:IDisposable
    {
        protected readonly New2meDbContext new2meDbContext;
        protected readonly IQuery query;
        public New2MeQueryTestBase(){
            // setup in-memory database
            var option = new DbContextOptionsBuilder<New2meDbContext>()
                        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                        .Options;

            new2meDbContext = new New2meDbContext(option);

            new2meDbContext.Database.EnsureCreated(); 

            // create the query layer
            query = new Query(new2meDbContext);
        }

        public void Dispose()
        {
            new2meDbContext.Database.EnsureDeleted();
            new2meDbContext.Dispose();
        }
    }
}