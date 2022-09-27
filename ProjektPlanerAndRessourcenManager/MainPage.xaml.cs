using ProjektPlanerAndRessourcenManager.DbModels;
using System.Collections.Generic;
using System;
using System.Globalization;
using ProjektPlanerAndRessourcenManager.Pages;
using System.Threading.Tasks;
using System.Linq;

namespace ProjektPlanerAndRessourcenManager;

public partial class MainPage : ContentPage
{
    private int ProjectId;
    private string ProjectName;
    private string TagIDs;
    int FillTaskViewCounter = 0;
    int activeTaskId;
    List<Tasks> tasks = App.DbHandle.GetTasksTableSync();
    List<Project> projects = App.DbHandle.GetProjectTableSync();

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
        projects = App.DbHandle.GetProjectTableSync();
        //picker.ItemsSource = projects;

        if (projects.Count == 0) { /*Shell.Current.GoToAsync(nameof(ExamplePage))*/; }
        else
        {
            var projectNames = new List<string>();

            //for (int i = 0; i <= projects.Count; i++)
            //{

            //}
            foreach (var project in projects)
            {
                projectNames.Add(project.Name);


            }
            projectPicker.ItemsSource = projectNames;

            projectPicker.SelectedItem = 0; /*projectNames[0]*/
            tagPicker.SelectedIndex = 0;
            toggleRecurringTask.SelectedIndex = 0;
        }
        FillTaskView();
    }
    public void FillTaskView(/*add debug string param*/)
    {
        testhd.Children.Clear();

        tasks = App.DbHandle.GetTasksTableSync();
        projects = App.DbHandle.GetProjectTableSync();

        Label label1;
        Label label;
        HorizontalStackLayout horizontalTaskEntry;
        ImageButton startTaskImageButton;


        foreach (var task in tasks.OrderByDescending(t => t.Id).ToList())
        {
            FillTaskViewCounter++;//Debug Variable 

            horizontalTaskEntry = new HorizontalStackLayout
            {
                Padding = new Thickness(5, 0, 5, 0),
                BackgroundColor = App.RndColor.TranslateDbColor(task.Color),
            };

            startTaskImageButton = new ImageButton
            {
                //BackgroundColor = App.RndColor.TranslateDbColor(task.Color),
                Source = "not_started.png",
            };
            //startTaskImageButton.Focused += (sender, e) => startTaskImageButton.BackgroundColor = App.RndColor.TranslateDbColor(task.Color);
            startTaskImageButton.Clicked += async (s, e) => await StartTask(task.Id);

            label1 = new Label
            {
                TextColor = Color.FromArgb("FF69B4")/*App.RndColor.TranslateDbColor(task.Color)*/,
                Text = $"[{task.ProjectName}] [{task.Description}] [{task.Status}]",/*[{testhd.Count()}][{tasks.Count}][{FillTaskViewCounter}]*/

                VerticalOptions = LayoutOptions.Center
            };
           
            label = new Label()
            {
                Text = $"{task.Id}",
                IsVisible = true,
            };

            horizontalTaskEntry.Children.Add(startTaskImageButton);
            horizontalTaskEntry.Children.Add(label1);
            horizontalTaskEntry.Children.Add(label);
            testhd.Children.Add(horizontalTaskEntry);
        }
    }
    public async Task StartTask(int taskId)
    {
        statusMessage.Text = "";
        tasks = await App.DbHandle.GetTasksTable();
        int _activeID;
        foreach (var task in tasks)
        {
            if (task.Status == "running" && task.Id != taskId)
            {
                AAAAHHHH();
                App.DbHandle.ChangeTaskStatus(task.Id, "done");
                //task.Status = "done"; // würde nur die lokale kopie der Daten aber nicht die Db Einträge ändern
                System.Diagnostics.Debug.WriteLine($"if: task.Id = {task.Id}, taskId = {taskId}");
                await App.DbHandle.EditTime(taskId, "end");
                statusMessage.Text = App.DbHandle.StatusMessage;
            }
            else if (task.Status == "running" && task.Id == taskId) { 
                 App.DbHandle.ChangeTaskStatus(task.Id, "done");
                System.Diagnostics.Debug.WriteLine($"else if: task.Id = {task.Id}, taskId = {taskId}");
                await App.DbHandle.EditTime(taskId, "end");
                statusMessage.Text = App.DbHandle.StatusMessage;
                break;
            }
            else {
                activeTaskId = task.Id;
                App.DbHandle.ChangeTaskStatus(taskId, "running");
                System.Diagnostics.Debug.WriteLine($"else: task.Id = {task.Id}, taskId = {taskId}");
                statusMessage.Text = App.DbHandle.StatusMessage;
                if (task.StartDateTime == null) {await App.DbHandle.EditTime(taskId, "start");
                    System.Diagnostics.Debug.WriteLine($"else: task.StartDateTime = {task.StartDateTime}, taskId = {taskId},{task}");
                    statusMessage.Text = App.DbHandle.StatusMessage;
                }
            }


        }
        

       
        

        FillTaskView();


        ////JUST SOME TESTS
        projects = App.DbHandle.GetProjectTableSync();
        ProjectLabel.Text = taskId.ToString()+"|||||";
        ProjectLabel.Text += projects[0].StartDateTime + "|||||";
        ProjectLabel.Text += App.DbHandle.ToUtcDateTime() + "|||||";
        ProjectLabel.Text += App.DbHandle.ToLocalDateTime() + "|||||";
    }
    public void OnRestartTaskClicked(object sender, EventArgs args)
    {
        if (App.RndColor.Hello())
        {
            string test8 = "";
            for (int i = 0; i < testhd.Children.Count; i++)
            {
                var tmplabel = new Label();
                tmplabel = (Label)testhd.Children[i];

                test8 += tmplabel.Text;
                //test8 += testhd.Children[i].ToString();    
            }
            testhd.Children.Clear();    
            string test = App.RndColor.HexString(6);
            tasks = App.DbHandle.GetTasksTableSync();
            string test2 = tasks[0].Color.Substring(1, 2);
            string test4 = tasks[0].Color.Substring(3, 2);
            string test5 = tasks[0].Color.Substring(5, 2);

            int test3 = int.Parse(test2, NumberStyles.HexNumber);
            test3.ToString();
            int test6 = int.Parse(test4, NumberStyles.HexNumber);
            test6.ToString();
            int test7 = int.Parse(test5, NumberStyles.HexNumber);
            test7.ToString();
            DisplayAlert("RndColorClaass", "says TTrue == Ello" + " >>>" + test2 + ":::" + test3 + "<<<<"
                + " >>>" + test4 + ":::" + test6 + "<<<<"
                + " >>>" + test5 + ":::" + test7 + "<<<<" + test8
                , "XD");
        }
        //Convert.FromHexString

    }
    private bool CheckDBContent()//ChheckDBContentProjectTable
    {
        projects = GetProjectsTable();
        tasks = App.DbHandle.GetTasksTableSync();
        if (projects.Count == 0 || tasks.Count == 0) { return false; }
        else { return true; }
    }
    private List<Project> GetProjectsTable()
    {
        return App.DbHandle.GetProjectTableSync();
    }
    protected override void OnAppearing()
    {
        System.Diagnostics.Debug.WriteLine("Appearing");
        if (CheckDBContent())
        {
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
        statusMessage.Text = "";
        
        ProjectTasksView.BackgroundColor = App.RndColor.RndRGBValue();
        projects = await App.DbHandle.GetProjectTable();
        statusMessage.Text = App.DbHandle.StatusMessage;
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
            await App.DbHandle.AddNewTask(TaskDescription.Text, ProjectId, ProjectName, "open", TagIDs);
            statusMessage.Text = App.DbHandle.StatusMessage;
            FillTaskView();

            //await DisplayAlert(">+<", "erfolgreich", "ok");

        }
        else { /*await DisplayAlert("Fehler", "TaskDescritpion can't be null", "ok");*/ }


        //string IDstring = "Id" + ProjectId.ToString() + ".";
        ////await DisplayAlert("holla", IDstring , "Cancel");


        //!!!!Wichtig!!!! Fehlt noch
        // erstml weglassen, später dedicaated button for direct start
        //StartNewTask();
    }
    public void StartNewTask()
    { }
    protected override void OnDisappearing()
    {

        //tasks = App.DbHandle.GetTasksTableSync();
        //foreach (var task in tasks)
        //{
        //    if (task.Status == "running") { App.DbHandle.ChangeTaskStatus(task.Id, "done"); }
        //}
        if (activeTaskId != 0) { App.DbHandle.ChangeTaskStatus(activeTaskId, "done"); }
        
        System.Diagnostics.Debug.WriteLine("Disappearing");
    }
    public void AAAAHHHH()
    {
        DisplayAlert("", "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", "ok");
    }
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