using ProductDirectory_v4.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductDirectory_v4.DataBase
{
    public class ProductDB
    {
        readonly SQLiteAsyncConnection db;

        public ProductDB(string conectionString)
        {
            db = new SQLiteAsyncConnection(conectionString);

            db.CreateTableAsync<Product>().Wait();
        }

        #region Data {this region worked whith sqlite}

        public Task<List<Product>> GetProductsAsync()
        {
            return db.Table<Product>().ToListAsync();
        }

        public Task<Product> GetProductAsync(int id)
        {
            return db.Table<Product>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<int> SaveProductAsync(Product product)
        {
            product.DateChanged = DateTime.Now;
            if (product.Id == 0)
            {
                return db.InsertAsync(product);
            }
            else
            {
                return db.UpdateAsync(product);
            }
        }

        public Task<int> DeleateProductAsync(Product product)
        {
            return db.DeleteAsync(product);
        }

        #endregion
    }
}
