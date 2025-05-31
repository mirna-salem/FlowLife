using FlowLife.ViewModels;
using FlowLife.Models;

namespace FlowLife.Views;

public partial class BudgetEditPage : ContentPage
{
    private readonly BudgetEditViewModel _viewModel;

    public BudgetEditPage(BudgetSummary currentBudget)
    {
        InitializeComponent();
        _viewModel = new BudgetEditViewModel(currentBudget, Navigation);
        BindingContext = _viewModel;
    }

    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry)
        {
            // Only format if it's a number
            if (decimal.TryParse(entry.Text, out decimal value))
            {
                entry.Text = value.ToString("F2");
            }
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        
        // Ensure the dashboard updates when we return
        if (Application.Current?.MainPage is NavigationPage navigationPage)
        {
            var dashboard = navigationPage.RootPage as DashboardPage;
            _ = dashboard?.RefreshDataAsync();
        }
    }
} 