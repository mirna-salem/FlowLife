using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;

namespace FlowLife.Models
{
    public class BudgetSummary : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private decimal _totalBudget;
        private decimal _spentAmount;
        private decimal _remainingAmount;
        private ObservableCollection<CategoryExpense> _topCategories;

        public decimal TotalBudget 
        { 
            get => _totalBudget;
            set
            {
                if (_totalBudget != value)
                {
                    _totalBudget = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal SpentAmount 
        { 
            get => _spentAmount;
            set
            {
                if (_spentAmount != value)
                {
                    _spentAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal RemainingAmount 
        { 
            get => _remainingAmount;
            set
            {
                if (_remainingAmount != value)
                {
                    _remainingAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        [Ignore]
        public ObservableCollection<CategoryExpense> TopCategories 
        { 
            get => _topCategories;
            set
            {
                if (_topCategories != value)
                {
                    _topCategories = value;
                    OnPropertyChanged();
                }
            }
        }

        public BudgetSummary()
        {
            _topCategories = new ObservableCollection<CategoryExpense>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CategoryExpense : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public int BudgetId { get; set; }

        private string _category = string.Empty;
        private decimal _amount;
        private string _amountText = string.Empty;
        private bool _isEditing;

        public string Category 
        { 
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value ?? string.Empty;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    if (!_isEditing)
                    {
                        _amountText = value.ToString("F2");
                        OnPropertyChanged(nameof(AmountText));
                    }
                    OnPropertyChanged();
                }
            }
        }

        [Ignore]
        public string AmountText
        {
            get => _amountText;
            set
            {
                if (_amountText != value)
                {
                    _isEditing = true;
                    _amountText = value;
                    if (decimal.TryParse(value, out decimal result))
                    {
                        Amount = result;
                    }
                    OnPropertyChanged();
                    _isEditing = false;
                }
            }
        }

        public CategoryExpense()
        {
            Category = string.Empty;
            Amount = 0;
            AmountText = "0.00";
        }

        public CategoryExpense(string category, decimal amount = 0)
        {
            Category = category;
            Amount = amount;
            AmountText = amount.ToString("F2");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 