using ProjektPlanerAndRessourcenManager.DbModels;
namespace ProjektPlanerAndRessourcenManager.Pages;

public partial class ProjectPage : ContentPage
{
	public ProjectPage()
	{
		InitializeComponent();
		//CheckForExistingProjects();
	}
    protected override async void OnAppearing()
    {
        await CheckForExistingProjects();
    }

    private async Task CheckForExistingProjects()//And Build List Dough
	{
        List<Project> projects = await App.DbHandle.GetProjectTable();
		if (projects.Count() == 0 )
		{
            //await Shell.Current.GoToAsync(nameof(ExamplePage));
			
        }
		ProjectView.ItemsSource = projects;
		
    }
	private async void OnAddNewProjectButtonClicked(object sender, EventArgs args)
	{
        statusMessage.Text = "";

        await App.DbHandle.AddNewProject(newProjectName.Text, newProjectDescription.Text); /*+newProjectDescription*/
        statusMessage.Text = App.DbHandle.StatusMessage;
        await CheckForExistingProjects();
    }
    
        
   
}