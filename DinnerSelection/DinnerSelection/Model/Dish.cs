using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace DinnerSelection
{
    class Dish
    {
        [PrimaryKey, AutoIncrement]
        int Id { get; set; }
        string Name { get; set; }
        string Base { get; set; }
        string Type { get; set; }
        string Season { get; set; }
        double Score { get; set; }
    }
}
