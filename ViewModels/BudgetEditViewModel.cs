using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FlowLife.Models;
using FlowLife.Services;

namespace FlowLife.ViewModels
{
    public class BudgetEditViewModel : INotifyPropertyChanged
    {
        private decimal _totalBudget;
        private ObservableCollection<CategoryExpense> _categories = new();
        private readonly BudgetSummary _originalBudget;
        private string _totalBudgetText = string.Empty;
        private bool _isEditing;
        private readonly INavigation _navigation;
        private readonly DatabaseService _databaseService;

        public string TotalBudgetText
        {
            get => _totalBudgetText;
            set
            {
                if (_totalBudgetText != value)
                {
                    _isEditing = true;
                    _totalBudgetText = value ?? string.Empty;
                    if (decimal.TryParse(value, out decimal result))
                    {
                        TotalBudget = result;
                    }
                    OnPropertyChanged();
                    _isEditing = false;
                }
            }
        }

        public decimal TotalBudget
        {
            get => _totalBudget;
            set
            {
                if (_totalBudget != value)
                {
                    _totalBudget = value;
                    if (!_isEditing)
                    {
                        _totalBudgetText = value.ToString("F2");
                        OnPropertyChanged(nameof(TotalBudgetText));
                    }
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<CategoryExpense> Categories
        {
            get => _categories;
            set
            {
                if (_categories != value)
                {
                    _categories = value ?? new ObservableCollection<CategoryExpense>();
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }
        public ICommand SaveChangesCommand { get; }

        public BudgetEditViewModel(BudgetSummary budget, INavigation navigation)
        {
            _originalBudget = budget ?? throw new ArgumentNullException(nameof(budget));
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _databaseService = new DatabaseService();
            
            // Initialize with default values
            _categories = new ObservableCollection<CategoryExpense>();
            TotalBudget = budget.TotalBudget;
            TotalBudgetText = TotalBudget.ToString("F2");
            
            // Copy categories from the original budget
            if (budget.TopCategories != null)
            {
                foreach (var category in budget.TopCategories)
                {
                    if (category != null)
                    {
                        Categories.Add(new CategoryExpense(category.Category, category.Amount));
                    }
                }
            }

            AddCategoryCommand = new Command(AddCategory);
            DeleteCategoryCommand = new Command<CategoryExpense>(DeleteCategory);
            SaveChangesCommand = new Command(async () => await SaveChangesAsync());
        }

        private void AddCategory()
        {
            Categories?.Add(new CategoryExpense("New Category"));
        }

        private void DeleteCategory(CategoryExpense category)
        {
            if (category != null && Categories != null)
            {
                Categories.Remove(category);
            }
        }

        private async Task SaveChangesAsync()
        {
            try
            {
                if (_originalBudget == null)
                {
                    throw new InvalidOperationException("Budget data is not initialized.");
                }

                if (_originalBudget.TopCategories == null)
                {
                    _originalBudget.TopCategories = new ObservableCollection<CategoryExpense>();
                }

                if (Categories == null)
                {
                    throw new InvalidOperationException("Categories collection is not initialized.");
                }

                // Update the original budget with new values
                _originalBudget.TotalBudget = TotalBudget;
                _originalBudget.TopCategories.Clear();

                // Add categories and calculate total spent
                decimal totalSpent = 0;
                foreach (var category in Categories.Where(c => c != null))
                {
                    var newCategory = new CategoryExpense(category.Category, category.Amount);
                    _originalBudget.TopCategories.Add(newCategory);
                    totalSpent += category.Amount;
                }

                // Update spent and remaining amounts
                _originalBudget.SpentAmount = totalSpent;
                _originalBudget.RemainingAmount = TotalBudget - totalSpent;

                // Save to database
                await _databaseService.SaveBudgetSummaryAsync(_originalBudget);

                // Show success message
                if (Application.Current != null && Application.Current.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Budget changes saved successfully!", "OK");
                }

                // Navigate back
                if (_navigation != null && _navigation.NavigationStack.Count > 1)
                {
                    await _navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving budget: {ex}");
                
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", 
                        $"Failed to save changes: {ex.Message}", "OK");
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RefreshData()
        {
            OnPropertyChanged(nameof(BudgetSummary));
            OnPropertyChanged(nameof(Categories));
            OnPropertyChanged(nameof(TotalBudget));
            OnPropertyChanged(nameof(TotalBudgetText));
        }
    }
} 