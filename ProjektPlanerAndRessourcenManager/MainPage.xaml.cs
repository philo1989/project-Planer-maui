using ProjektPlanerAndRessourcenManager.DbModels;
using System.Collections.Generic;
using System;
using System.Globalization;
using ProjektPlanerAndRessourcenManager.Pages;
//using Android.App;

namespace ProjektPlanerAndRessourcenManager;

public partial class MainPage : ContentPage
{
    private int ProjectId;
    private string ProjectName;
    private string TagIDs;


    public MainPage()
    {
        InitializeComponent();
        if (!CheckDBContent())
        {
            ProjectLabel.Text = "click project in the Left  Navigation Panel to add your first project";
            /*Shell.Current.GoToAsync(nameof(ExamplePage)); */
        }
        OnInit();

        ////if (CheckDBContent().Result) { DisplayAlert("CheckDbCOntent=true", CheckDBContent().Result.ToString(), "Okn"); }
        ////else { DisplayAlert("CheckDbCOntent=false", CheckDBContent().Result.ToString(), "Okn"); }
    }
    //private async Task<bool> CheckDBContent()
    //{
    //    List<Project> projects = await App.DbHandle.GetProjectTable();
    //    if (projects == null) { return false; }
    //    else { return true; }
    //}
    protected void OnInit()
    {
        List<Project> projects = App.DbHandle.GetProjectTableSync();
        //picker.ItemsSource = projects;

        //if (projects.Count == 0) Shell.Current.GoToAsync(nameof(ExamplePage));
        //else
        //{
        var projectNames = new List<string>();

        //for (int i = 0; i <= projects.Count; i++)
        //{

        //}
        foreach (var project in projects)
        {
            projectNames.Add(project.Name);
            projectNames.Add(project.ProjectDescription);
        }
        projectPicker.ItemsSource = projectNames;

        projectPicker.SelectedItem = projectNames[0];
        tagPicker.SelectedIndex = 0;
        toggleRecurringTask.SelectedIndex = 0;
        //}
        FillTaskView();
    }
    public async void FillTaskView()
    {
        List<Tasks> tasks = await App.DbHandle.GetTasksTable();

        ProjectView.ItemsSource = tasks.OrderByDescending(t => t.Id).ToList();
    }
    public void OnRestartTaskClicked(object sender, EventArgs args)
    { }
    private bool CheckDBContent()//ChheckDBContentProjectTable
    {
        List<Project> projects = GetProjectsTable();
        if (projects.Count == 0) { return false; }
        else { return true; }
    }
    private List<Project> GetProjectsTable()
    {
        return App.DbHandle.GetProjectTableSync();
    }
    protected override void OnAppearing()
    {
        if (CheckDBContent()) {
            ProjectOverview.ItemsSource = GetProjectsTable();
            ProjectLabel.Text = "";
            //foreach (var project in projects)
            //{
            //    //ProjectLabel.Text += project.Name;

            //}
        }
        OnInit();
        FillTaskView();

        //    CheckDBContent();
        //Bad  Solution becouse of bck button problem, works for now
        //if (!CheckDBContent()) { await Shell.Current.GoToAsync(nameof(ExamplePage)); }
        //bool test =  CheckDBContent().Result;
        //await DisplayAlert("CheckDbCOntent=?", /*CheckDBContent().ToString()*/"hmm", "Okn");
        //if (CheckDBContent()) { DisplayAlert("CheckDbCOntent=true", CheckDBContent().ToString(), "Okn"); }
        //else { DisplayAlert("CheckDbCOntent=false", CheckDBContent().ToString(), "Okn"); }
        //Check for Owner not existend at the moment //Implementation with txt file ?
        //Check for projects
        //Check for Sprint
        //Check for Tasks
    }
    async void GotoExPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ExamplePage));
    }
    public async void AddNewTask(object sender, EventArgs args)
    {
        List<Project> projects = await App.DbHandle.GetProjectTable();

        //if (projects.Count() == 0) { await Shell.Current.GoToAsync(nameof(ExamplePage)); }
        if (TaskDescription.Text != null)
        {
            foreach (var project in projects)
            {
                if (project.Name == projectPicker.SelectedItem.ToString())
                {
                    ProjectId = project.Id;
                    ProjectName = project.Name;
                    TagIDs = tagPicker.SelectedIndex.ToString();
                }

            }
            await App.DbHandle.AddNewTask(TaskDescription.Text, ProjectId, ProjectName, "running", TagIDs);

            FillTaskView();

            await DisplayAlert(">+<", "erfolgreich", "ok");

        } else { await DisplayAlert("Fehler", "TaskDescritpion can't be null", "ok"); }


        //string IDstring = "Id" + ProjectId.ToString() + ".";
        ////await DisplayAlert("holla", IDstring , "Cancel");


        //!!!!Wichtig!!!! Fehlt noch
        startNewTask();
    }
    public void startNewTask()
    {}

}

/*ToDO*/
/*		Create Extra Side for Documentation of:	1. This Project
 *												2. C# learned Stuff&Concepts
 * 
 * 
 *      FeatureIdeas:   AAutoComplete Function
 *                      Only Show what is recorded ? no tasks done on a Day no representantion of this day ???
 * 
 *           2.3.5-0041
            │ │ │  └────── Buildnummer              <- lass ick weg, jedenfalls solange kein AAutoBuildSystemEingesetzt wird 
            │ │ └───────── Revisionsnummer          <- für Bugs
            │ └─────────── Nebenversionsnummer      <- für Features
            └───────────── Hauptversionsnummer      <- Like da Biggie ya know
 */


//NET-MAui Default starter solution
//int count = 0;

//public MainPage()
//{
//	InitializeComponent();
//}

//private void OnCounterClicked(object sender, EventArgs e)
//{
//	count++;

//	if (count == 1)
//		CounterBtn.Text = $"Clicked {count} time";
//	else
//		CounterBtn.Text = $"Clicked {count} times";

//	SemanticScreenReader.Announce(CounterBtn.Text);
//}