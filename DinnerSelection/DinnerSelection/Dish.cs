using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace DinnerSelection
{
    public class Dish
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Base { get; set; }
        public string Type { get; set; }
        public string Season { get; set; }
        public double Score { get; set; }
    }
}
