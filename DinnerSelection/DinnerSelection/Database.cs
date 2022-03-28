using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DinnerSelection
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Dish>();
        }

        public Task<List<Dish>> GetDishesAsync()
        {
            return _database.Table<Dish>().ToListAsync();
        }

        public Task<int> SaveDishAsync(Dish dish)
        {
            return _database.InsertAsync(dish);
        }

        public async Task DeleteDishAsync(int id)
        {
            var collectionItem = await _database.Table<Dish>().Where(x => x.Id == id).ToListAsync();
            foreach (Dish item in collectionItem)
            {
                await _database.DeleteAsync(item);
            }
        }
    }
}
