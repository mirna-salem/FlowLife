using SQLite;

namespace FlowLife.Models
{
    public class WorkoutTracker
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CurrentStreak { get; set; }
        public int WeeklyGoal { get; set; }
        public int CompletedWorkouts { get; set; }
    }
} 