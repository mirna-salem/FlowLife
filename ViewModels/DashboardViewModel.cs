using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FlowLife.Models;

namespace FlowLife.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private BudgetSummary _budgetSummary = new();
        private WorkoutTracker _workoutTracker = new();
        private ObservableCollection<HabitItem> _habits = new();

        public BudgetSummary BudgetSummary
        {
            get => _budgetSummary;
            set
            {
                _budgetSummary = value;
                OnPropertyChanged();
            }
        }

        public WorkoutTracker WorkoutTracker
        {
            get => _workoutTracker;
            set
            {
                _workoutTracker = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<HabitItem> Habits
        {
            get => _habits;
            set
            {
                _habits = value;
                OnPropertyChanged();
            }
        }

        public DashboardViewModel()
        {
            // Initialize with sample data
            BudgetSummary.TotalBudget = 2000;
            BudgetSummary.SpentAmount = 1200;
            BudgetSummary.RemainingAmount = 800;

            // Add sample budget categories
            BudgetSummary.TopCategories.Add(new CategoryExpense("Groceries", 400));
            BudgetSummary.TopCategories.Add(new CategoryExpense("Utilities", 300));
            BudgetSummary.TopCategories.Add(new CategoryExpense("Entertainment", 200));
            BudgetSummary.TopCategories.Add(new CategoryExpense("Transportation", 300));

            WorkoutTracker.CurrentStreak = 5;
            WorkoutTracker.WeeklyGoal = 4;
            WorkoutTracker.CompletedWorkouts = 3;

            var meditationHabit = new HabitItem("Morning Meditation") { IsCompleted = true };
            var readingHabit = new HabitItem("Read 30 Minutes") { IsCompleted = false };
            var waterHabit = new HabitItem("Drink Water") { IsCompleted = true };

            Habits.Add(meditationHabit);
            Habits.Add(readingHabit);
            Habits.Add(waterHabit);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RefreshData()
        {
            // Trigger property changed notifications to refresh the UI
            OnPropertyChanged(nameof(BudgetSummary));
            OnPropertyChanged(nameof(WorkoutTracker));
            OnPropertyChanged(nameof(Habits));
        }
    }
} 