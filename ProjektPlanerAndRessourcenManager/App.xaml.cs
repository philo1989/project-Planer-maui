namespace ProjektPlanerAndRessourcenManager;

public partial class App : Application
{
	public static DbHandler DbHandle { get; set; }
	public static RandomColor RndColor { get; set; }

	public App(DbHandler db, RandomColor rc)
	{
		InitializeComponent();

		MainPage = new AppShell();
		DbHandle = db; //Handler singleton INstance
		RndColor = rc;
	}


}
