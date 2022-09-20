using ProjektPlanerAndRessourcenManager.Pages;

namespace ProjektPlanerAndRessourcenManager;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(TimeRecord), typeof(TimeRecord));
		Routing.RegisterRoute(nameof(ExamplePage), typeof(ExamplePage));
		Routing.RegisterRoute(nameof(ProjectPage), typeof(ProjectPage));
		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
	}
}
