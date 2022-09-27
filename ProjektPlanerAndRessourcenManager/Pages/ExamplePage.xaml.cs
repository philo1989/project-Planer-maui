using ProjektPlanerAndRessourcenManager.DbModels;

namespace ProjektPlanerAndRessourcenManager.Pages;

public partial class ExamplePage : ContentPage
{
	public ExamplePage()
	{
		InitializeComponent();

        
    }
    //void GotoMainPage(object sender, EventArgs e)
    //{
    //    Shell.Current.GoToAsync(nameof(MainPage));
    //}
    public async void OnAddNewProjectButtonClicked(object sender, EventArgs args)
    {
        statusMessage.Text = "";

        await App.DbHandle.AddNewProject(newProject.Text, "to be changed/changeable");
        statusMessage.Text = App.DbHandle.StatusMessage;
    }
    public void OnGibPfad(object sender, EventArgs args)
    {
        string path = FileAccessHelper.GetLocalFilePath("people.db",MauiProgram.buildStyle);
        DisplayAlert("DerPFAAD", path, "boaah ne ey");
    }
    public async void OnTime(object sender, EventArgs args)
    {
        statusMessage.Text = "";
        await App.DbHandle.TestManagmentProtocollDate();
        DateTime date = DateTime.Now;
        await DisplayAlert("DerZTeit", date.ToString(), "ok");
        statusMessage.Text = App.DbHandle.StatusMessage;
    }
    public async void OnGetButtonClicked(object sender, EventArgs args)
    {
        statusMessage.Text = "";
        List<Project> projects = await App.DbHandle.GetProjectTable();

        List.ItemsSource = projects;
    }
}