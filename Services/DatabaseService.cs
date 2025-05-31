using SQLite;
using FlowLife.Models;
using System.Collections.ObjectModel;

namespace FlowLife.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "flowlife.db");
            _database = new SQLiteAsyncConnection(dbPath);
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await _database.CreateTablesAsync<BudgetSummary, CategoryExpense, WorkoutTracker, HabitItem>();
        }

        // Budget Methods
        public async Task SaveBudgetSummaryAsync(BudgetSummary budget)
        {
            if (budget == null) return;

            var existingBudget = await _database.Table<BudgetSummary>().FirstOrDefaultAsync();
            if (existingBudget != null)
            {
                budget.Id = existingBudget.Id;
                await _database.UpdateAsync(budget);
            }
            else
            {
                await _database.InsertAsync(budget);
            }

            // Save categories
            if (budget.TopCategories != null)
            {
                // Delete existing categories
                await _database.DeleteAllAsync<CategoryExpense>();
                
                // Insert new categories
                foreach (var category in budget.TopCategories)
                {
                    category.BudgetId = budget.Id;
                    await _database.InsertAsync(category);
                }
            }
        }

        public async Task<BudgetSummary> GetBudgetSummaryAsync()
        {
            var budget = await _database.Table<BudgetSummary>().FirstOrDefaultAsync() ?? new BudgetSummary();
            var categories = await _database.Table<CategoryExpense>().Where(c => c.BudgetId == budget.Id).ToListAsync();
            budget.TopCategories = new ObservableCollection<CategoryExpense>(categories);
            return budget;
        }

        // Workout Methods
        public async Task SaveWorkoutTrackerAsync(WorkoutTracker tracker)
        {
            if (tracker == null) return;

            var existingTracker = await _database.Table<WorkoutTracker>().FirstOrDefaultAsync();
            if (existingTracker != null)
            {
                tracker.Id = existingTracker.Id;
                await _database.UpdateAsync(tracker);
            }
            else
            {
                await _database.InsertAsync(tracker);
            }
        }

        public async Task<WorkoutTracker> GetWorkoutTrackerAsync()
        {
            return await _database.Table<WorkoutTracker>().FirstOrDefaultAsync() ?? new WorkoutTracker();
        }

        // Habit Methods
        public async Task SaveHabitsAsync(ObservableCollection<HabitItem> habits)
        {
            if (habits == null) return;

            await _database.DeleteAllAsync<HabitItem>();
            foreach (var habit in habits)
            {
                await _database.InsertAsync(habit);
            }
        }

        public async Task<ObservableCollection<HabitItem>> GetHabitsAsync()
        {
            var habits = await _database.Table<HabitItem>().ToListAsync();
            return new ObservableCollection<HabitItem>(habits);
        }
    }
} 