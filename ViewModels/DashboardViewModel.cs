using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FlowLife.Models;
using FlowLife.Services;

namespace FlowLife.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;
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
            _databaseService = new DatabaseService();
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                BudgetSummary = await _databaseService.GetBudgetSummaryAsync();
                WorkoutTracker = await _databaseService.GetWorkoutTrackerAsync();
                Habits = await _databaseService.GetHabitsAsync();

                // If no data exists, initialize with sample data
                if (BudgetSummary.TopCategories.Count == 0)
                {
                    InitializeSampleData();
                    await SaveDataAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading data: {ex}");
                // Initialize with sample data if loading fails
                InitializeSampleData();
            }
        }

        private void InitializeSampleData()
        {
            BudgetSummary.TotalBudget = 2000;
            BudgetSummary.SpentAmount = 1200;
            BudgetSummary.RemainingAmount = 800;

            BudgetSummary.TopCategories.Clear();
            BudgetSummary.TopCategories.Add(new CategoryExpense("Groceries", 400));
            BudgetSummary.TopCategories.Add(new CategoryExpense("Utilities", 300));
            BudgetSummary.TopCategories.Add(new CategoryExpense("Entertainment", 200));
            BudgetSummary.TopCategories.Add(new CategoryExpense("Transportation", 300));

            WorkoutTracker.CurrentStreak = 5;
            WorkoutTracker.WeeklyGoal = 4;
            WorkoutTracker.CompletedWorkouts = 3;

            Habits.Clear();
            Habits.Add(new HabitItem("Morning Meditation") { IsCompleted = true });
            Habits.Add(new HabitItem("Read 30 Minutes") { IsCompleted = false });
            Habits.Add(new HabitItem("Drink Water") { IsCompleted = true });
        }

        public async Task SaveDataAsync()
        {
            try
            {
                await _databaseService.SaveBudgetSummaryAsync(BudgetSummary);
                await _databaseService.SaveWorkoutTrackerAsync(WorkoutTracker);
                await _databaseService.SaveHabitsAsync(Habits);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving data: {ex}");
                // Handle error (e.g., show alert to user)
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task RefreshDataAsync()
        {
            await LoadDataAsync();
            OnPropertyChanged(nameof(BudgetSummary));
            OnPropertyChanged(nameof(WorkoutTracker));
            OnPropertyChanged(nameof(Habits));
        }
    }
} 