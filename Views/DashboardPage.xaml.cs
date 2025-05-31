using FlowLife.Models;
using System.Runtime.Versioning;
using System.ComponentModel;

namespace FlowLife.Views;

[SupportedOSPlatform("android")]
[SupportedOSPlatform("ios")]
[SupportedOSPlatform("maccatalyst")]
[SupportedOSPlatform("windows")]
public partial class DashboardPage : ContentPage
{
    private BudgetSummary _budget;

    public DashboardPage()
    {
        InitializeComponent();
        _budget = new BudgetSummary();
        BindingContext = _budget;
    }

    private async void OnEditBudgetClicked(object sender, EventArgs e)
    {
        try
        {
            var budgetEditPage = new BudgetEditPage(_budget);
            await Navigation.PushAsync(budgetEditPage);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Navigation error: {ex}");
            await DisplayAlert("Error", "Could not open budget edit page.", "OK");
        }
    }

    public void RefreshData()
    {
        // Re-set the binding context to force a UI refresh
        var currentBudget = BindingContext as BudgetSummary;
        if (currentBudget != null)
        {
            BindingContext = null;
            BindingContext = currentBudget;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshData();
    }
} 