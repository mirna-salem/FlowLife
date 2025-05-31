namespace FlowLife.Models
{
    public class HabitItem
    {
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
    }
} 