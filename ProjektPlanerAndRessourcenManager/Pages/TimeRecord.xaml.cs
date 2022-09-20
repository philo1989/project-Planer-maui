using ProjektPlanerAndRessourcenManager.DbModels;

namespace ProjektPlanerAndRessourcenManager.Pages;

public partial class TimeRecord : ContentPage
{

    private int ProjectId = 0;
    private string ProjectName;
    List<Project> projects = App.DbHandle.GetProjectTableSync();
    public TimeRecord()
    {
        InitializeComponent();

        OnInit();

    }


    protected void OnInit()
    {
        if (projects.Count == 0) Shell.Current.GoToAsync(nameof(ExamplePage));
        else
        {
            var projectNames = new List<string>();

            for (int i = 0; i <= projects.Count; i++)
            {

            }
            foreach (var project in projects)
            {
                projectNames.Add(project.Name);
            }
            picker.ItemsSource = projectNames;

            picker.SelectedItem = projectNames[0];
        }
    }
    public async void OnStartTaskClicked(object sender, EventArgs args)
    {


        List<Project> projects = await App.DbHandle.GetProjectTable();

        if (projects.Count() == 0) { await Shell.Current.GoToAsync(nameof(ExamplePage)); }

        foreach (var project in projects)
        {
            if (project.Name == picker.SelectedItem.ToString())
            {
                ProjectId = project.Id;
                ProjectName = project.Name;
            }

        }

        //string IDstring = "Id" + ProjectId.ToString() + ".";
        ////await DisplayAlert("holla", IDstring , "Cancel");

        await App.DbHandle.AddNewTask(TaskDescription.Text, ProjectId, ProjectName, "running", "[99999;]]");

        FillTaskView();
    }
    public async void FillTaskView()
    {
        List<Tasks> tasks = await App.DbHandle.GetTasksTable();

        ProjectView.ItemsSource = tasks.OrderByDescending(t => t.Id).ToList();
    }
    public void OnRestartTaskClicked(object sender, EventArgs args)
    { }
    protected override void OnAppearing()
    {
        OnInit();
        FillTaskView();

    }
}