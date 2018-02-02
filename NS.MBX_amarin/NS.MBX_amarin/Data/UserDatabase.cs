using NS.MBX_amarin.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NS.MBX_amarin.Data
{
    public class UserDatabase
    {
        readonly SQLiteAsyncConnection bd;

        public UserDatabase(string dbPath)
        {
            bd = new SQLiteAsyncConnection(dbPath);
            bd.CreateTableAsync<User>().Wait();
        }

        public async Task<List<User>> GetItemsAsync()
        {
            return await bd.Table<User>().ToListAsync();
        }

        public Task<User> GetItemAsync(int id)
        {
            return bd.Table<User>()
                .Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(User item)
        {
            if (item.ID != 0)
            {
                return bd.UpdateAsync(item);
            }
            else
            {
                return bd.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(User item)
        {
            return bd.DeleteAsync(item);
        }
    }
}
