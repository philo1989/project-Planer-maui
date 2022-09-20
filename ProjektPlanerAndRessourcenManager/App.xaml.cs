namespace ProjektPlanerAndRessourcenManager;

public partial class App : Application
{
	public static DbHandler DbHandle { get; set; }

	public App(DbHandler db)
	{
		InitializeComponent();

		MainPage = new AppShell();
		DbHandle = db; //Handler singleton INstance
	}
}
