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
        CalculateAndWriteTimeEffort(8);
        List.ItemsSource = projects;
    }
    private void CalculateAndWriteTimeEffort(int taskId)
    {
        int result = 0;
        List<Tasks> tasks = App.DbHandle.GetTasksTableSync();
        foreach (Tasks task in tasks)
        {
            if (task.Id == taskId)
            {
                
                int year = int.Parse(task.EndDateTime.Substring(6, 4));//11,12
                int month = int.Parse(task.EndDateTime.Substring(3, 2));//11,12
                int day = int.Parse(task.EndDateTime.Substring(0, 2));//11,12

                int endTimeHours = int.Parse(task.EndDateTime.Substring(11, 2));//11,12
                int endTimeMinutes = int.Parse(task.EndDateTime.Substring(14, 2));//11,12
                int endTimeSeconds = int.Parse(task.EndDateTime.Substring(17, 2));//11,12
                int startTimeHours = int.Parse(task.StartDateTime.Substring(11, 2));//11,12
                int startTimeMinutes = int.Parse(task.StartDateTime.Substring(14, 2));//11,12
                int startTimeSeconds = int.Parse(task.StartDateTime.Substring(17, 2));//11,12

                int _totlH = endTimeHours - startTimeHours;
                int _totlM = endTimeMinutes - startTimeMinutes;
                int _totlS = endTimeSeconds - startTimeSeconds;

                _totlH *= 60;
                _totlS /= 60;

                ////int tmphours = ((endTimeHours - startTimeHours)*60)+();      
                //int startseconds = ((startTimeHours*60) * 60) + (startTimeMinutes*60)+startTimeSeconds;
                //int endseconds = ((endTimeHours * 60) * 60) + (endTimeMinutes * 60) + endTimeSeconds;

                float totalTime = (_totlH+_totlM+_totlS);
                totalTime /= 60;

                DateTime dateTimeA = DateTime.Parse("28.09.2022 00:16:48");
                DateTime dateTimeB = DateTime.Parse("28.09.2022 15:00:46");
                TimeSpan deltaTime = dateTimeB.Subtract(dateTimeA);

                DisplayAlert("",
                    $"total Minutes: {deltaTime.TotalMinutes} Hours:{deltaTime.Hours}, total: {deltaTime.Minutes}",//month  day endTimeHours endTimeMinutes startTimeHours startTimeMinutes
                    "ok");
            }
        }

    }
}