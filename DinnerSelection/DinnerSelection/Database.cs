using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace DinnerSelection.Database
{
    class Database
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

        public Task DeleteDishAsync(int id)
        {
            return _database.DeleteAsync(id);
        }
    }
}
