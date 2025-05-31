using System.Collections.ObjectModel;

namespace FlowLife.Models
{
    public class HabitChecklist
    {
        public ObservableCollection<DailyHabit> Habits { get; set; }

        public HabitChecklist()
        {
            Habits = new ObservableCollection<DailyHabit>();
        }
    }

    public class DailyHabit
    {
        private string _name = string.Empty;

        public string Name 
        { 
            get => _name;
            set => _name = value ?? string.Empty;
        }

        public bool IsCompleted { get; set; }
        public int CurrentStreak { get; set; }

        public DailyHabit(string name)
        {
            Name = name;
        }
    }
} 