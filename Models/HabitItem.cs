using SQLite;

namespace FlowLife.Models
{
    public class HabitItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        private string _name = string.Empty;

        public string Name 
        { 
            get => _name;
            set => _name = value ?? string.Empty;
        }

        public bool IsCompleted { get; set; }

        public HabitItem(string name)
        {
            Name = name;
        }

        public HabitItem() { } // Required for SQLite
    }
} 