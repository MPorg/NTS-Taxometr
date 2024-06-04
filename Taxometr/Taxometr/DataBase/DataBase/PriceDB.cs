using ProductDirectory_v4.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductDirectory_v4.DataBase
{

    public class PriceDB
    {
        readonly SQLiteAsyncConnection db;

        public PriceDB(string conectionString)
        {
            db = new SQLiteAsyncConnection(conectionString);

            db.CreateTableAsync<Price>().Wait();
        }

        #region Data {this region worked whis sqlite}

        public Task<List<Price>> GetPricesAsync()
        {
            return db.Table<Price>().ToListAsync();
        }

        public Task<Price> GetPriceAsync(int id)
        {
            return db.Table<Price>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<int> SavePriceAsync(Price price)
        {
            if (price.Amount != 0)
            {
                return db.InsertAsync(price);
            }
            return null;
        }

        public Task<int> DeleatePriceAsync(Price price)
        {
            return db.DeleteAsync(price);
        }

        #endregion
    }
}
