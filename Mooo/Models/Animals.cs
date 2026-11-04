using Microsoft.EntityFrameworkCore;

namespace Mooo.Models
{
    public class Animals
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public int Price { get; set; }
        public string Description { get; set; } = "";
        public int Menge { get; set; }
        public string Experience { get; set; } = "";
        public string Provenience { get; set; } = "";
        public string Category { get; set; } = "";
    }
}
