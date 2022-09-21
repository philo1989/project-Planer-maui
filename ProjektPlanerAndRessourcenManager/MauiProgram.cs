namespace ProjektPlanerAndRessourcenManager;

public static class MauiProgram
{
	public const int buildStyle = 0; //1||0  1 = dotnet publish -f net6.0-windows10.0.19041.0 -c Release -p:WindowsPackageType=None
    public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		string dbPath = FileAccessHelper.GetLocalFilePath("PlanDataDB.db3",buildStyle);
		builder.Services.AddSingleton<DbHandler>(s => ActivatorUtilities.CreateInstance<DbHandler>(s, dbPath));
		builder.Services.AddSingleton<RandomColor>(s => ActivatorUtilities.CreateInstance<RandomColor>(s));

        return builder.Build();
	}

}
