using FlowLife.Models;
using FlowLife.ViewModels;
using System.Runtime.Versioning;

namespace FlowLife.Views;

[SupportedOSPlatform("android")]
[SupportedOSPlatform("ios")]
[SupportedOSPlatform("maccatalyst")]
[SupportedOSPlatform("windows")]
public partial class DashboardPage : ContentPage
{
    private readonly DashboardViewModel _viewModel;

    public DashboardPage()
    {
        InitializeComponent();
        _viewModel = new DashboardViewModel();
        BindingContext = _viewModel;
    }

    private async void OnEditBudgetClicked(object sender, EventArgs e)
    {
        try
        {
            var budgetEditPage = new BudgetEditPage(_viewModel.BudgetSummary);
            await Navigation.PushAsync(budgetEditPage);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Navigation error: {ex}");
            await DisplayAlert("Error", "Could not open budget edit page.", "OK");
        }
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button button)
            {
                // Disable button and show loading state
                button.IsEnabled = false;
                var originalText = button.Text;
                button.Text = "Refreshing...";

                await RefreshDataAsync();

                // Restore button state
                button.Text = originalText;
                button.IsEnabled = true;

                await DisplayAlert("Success", "Data refreshed successfully!", "OK");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Refresh error: {ex}");
            await DisplayAlert("Error", "Could not refresh data.", "OK");
        }
    }

    public async Task RefreshDataAsync()
    {
        await _viewModel.RefreshDataAsync();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshDataAsync();
    }
} 