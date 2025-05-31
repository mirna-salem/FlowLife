using FlowLife.Views;

namespace FlowLife;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		var navigationPage = new NavigationPage(new DashboardPage());
		MainPage = navigationPage;
	}
}
