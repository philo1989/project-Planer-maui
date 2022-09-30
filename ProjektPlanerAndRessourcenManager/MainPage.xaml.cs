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
    int activeTaskId = -10;
    List<Tasks> tasks = App.DbHandle.GetTasksTableSync();
    List<DoneTasks> doneTasks = App.DbHandle.GetDoneTasksTableSync();
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

            //projectPicker.SelectedItem = projectNames[0]/*0*/; 
            projectPicker.SelectedIndex = 0; 
            tagPicker.SelectedIndex = 0;
            toggleRecurringTask.SelectedIndex = 0;
        }
        FillSprintViewWithTasks();
        FillDoneTasks();
    }
    public void FillDoneTasks()
    {
        if (DoneTasks.Children.Count != 0) DoneTasks.Children.Clear();

        doneTasks = App.DbHandle.GetDoneTasksTableSync();
        projects = App.DbHandle.GetProjectTableSync();

        Label label1;
        Entry entry1;
        Label label2;
        Label label;
        HorizontalStackLayout horizontalTaskEntry;
        ImageButton startTaskImageButton;
        ImageButton editTaskImageButton;
        ImageButton cancelTaskImageButton;
        ImageButton doneTaskImageButton;
        // change status done to paused and add the done button if task is set to paused/stoped

        foreach (var task in doneTasks.OrderByDescending(t => t.Id).ToList())
        {
            FillTaskViewCounter++;//Debug Variable 

            horizontalTaskEntry = new HorizontalStackLayout
            {
                BackgroundColor = App.RndColor.TranslateDbColor(task.Color),
            };

            startTaskImageButton = new ImageButton
            {
                //BackgroundColor = App.RndColor.TranslateDbColor(task.Color),
                Source = "play_circle.png",
            };
            startTaskImageButton.Pressed += (s, e) => OnButtonPressed(startTaskImageButton, task.Color);
            //startTaskImageButton.Focused += (sender, e) => startTaskImageButton.BackgroundColor = App.RndColor.TranslateDbColor(task.Color);
            startTaskImageButton.Clicked += async (s, e) => await StartTask(task.Id);

            editTaskImageButton = new ImageButton
            {
                //BackgroundColor = App.RndColor.TranslateDbColor(task.Color),
                Source = "edit.png",
            };
            //startTaskImageButton.Focused += (sender, e) => startTaskImageButton.BackgroundColor = App.RndColor.TranslateDbColor(task.Color);
            editTaskImageButton.Clicked += async (s, e) => await EditTask(task.Id);

            doneTaskImageButton = new ImageButton
            {
                Source = "task_check.png",
            };
            doneTaskImageButton.Clicked += async (s, e) => await SetTaskToDone(task.Id);
            entry1 = new Entry
            {
                MinimumWidthRequest = 150,
                MaximumWidthRequest = 150,
                /*  TextColor = Color.FromArgb("FF69B4")*//*App.RndColor.TranslateDbColor(task.Color),*/
                TextColor = /*Color.FromArgb("111111")*/App.RndColor.TranslateDbColor(task.Color).AddLuminosity(4.35f).GetComplementary()/*GetComplementary().AddLuminosity(0.4f)*/,
                Placeholder = $"[{task.Status}];[{task.TotalHours}]h;[{task.TotalMinutes}]m;",/*[{testhd.Count()}][{tasks.Count}][{FillTaskViewCounter}]*/

                VerticalOptions = LayoutOptions.Center
            };
            //label1 = new Label
            //{
            //    MinimumWidthRequest = 150,
            //    MaximumWidthRequest = 150,
            //    /*  TextColor = Color.FromArgb("FF69B4")*//*App.RndColor.TranslateDbColor(task.Color),*/
            //    TextColor = /*Color.FromArgb("111111")*/App.RndColor.TranslateDbColor(task.Color).AddLuminosity(4.35f).GetComplementary()/*GetComplementary().AddLuminosity(0.4f)*/,
            //    Text = $"[{task.Status}];[{task.TotalHours}]h;[{task.TotalMinutes}]m;",/*[{testhd.Count()}][{tasks.Count}][{FillTaskViewCounter}]*/

            //    VerticalOptions = LayoutOptions.Center
            //};
            label2 = new Label
            {
                MinimumWidthRequest = 300,
                MaximumWidthRequest = 300,
                /*  TextColor = Color.FromArgb("FF69B4")*//*App.RndColor.TranslateDbColor(task.Color),*/
                TextColor = App.RndColor.TranslateDbColor(task.Color).GetComplementary(),/*App.RndColor.TranslateDbColor(task.Color)*/
                Text = $"[{task.ProjectName}];[{task.Description}];",/*[{testhd.Count()}][{tasks.Count}][{FillTaskViewCounter}]*/

                VerticalOptions = LayoutOptions.End
            };

            label = new Label()
            {
                MinimumWidthRequest = 20,
                MaximumWidthRequest = 20,
                Text = $"{task.Id}",
                IsVisible = true,
            };
            if (task.Status == "running") { startTaskImageButton.Source = "pause_circl.png"; }
            if (task.Status == "paused") { startTaskImageButton.Source = "replaya.png"; }

            horizontalTaskEntry.Children.Add(startTaskImageButton);
            horizontalTaskEntry.Children.Add(entry1);
            horizontalTaskEntry.Children.Add(label2);
            horizontalTaskEntry.Children.Add(label);
            horizontalTaskEntry.Children.Add(editTaskImageButton);
            /*if (task.Status == "done" || task.Status == "running") {*/
            horizontalTaskEntry.Children.Add(doneTaskImageButton);/* }
*/
            DoneTasks.Children.Add(horizontalTaskEntry);
        }
    }
    public void OnButtonPressed(ImageButton startTaskImageButton,string taskColor)
    {
        startTaskImageButton.BackgroundColor = App.RndColor.TranslateDbColor(taskColor);
    }
    public void FillSprintViewWithTasks(/*add debug string param*/)
    {
       if (SprintTasks.Children.Count != 0) SprintTasks.Children.Clear();

        tasks = App.DbHandle.GetTasksTableSync();
        projects = App.DbHandle.GetProjectTableSync();

        Label label1;
        Label label2;
        Label label;
        HorizontalStackLayout horizontalTaskEntry;
        ImageButton startTaskImageButton;
        ImageButton editTaskImageButton;
        ImageButton cancelTaskImageButton;
        ImageButton doneTaskImageButton;
        // change status done to paused and add the done button if task is set to paused/stoped
        
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
                Source = "play_circle.png",
            };
            startTaskImageButton.Pressed += (s, e) => OnButtonPressed(startTaskImageButton, task.Color); 
                //startTaskImageButton.Focused += (sender, e) => startTaskImageButton.BackgroundColor = App.RndColor.TranslateDbColor(task.Color);
            startTaskImageButton.Clicked += async (s, e) => await StartTask(task.Id);

            editTaskImageButton = new ImageButton
            {
                //BackgroundColor = App.RndColor.TranslateDbColor(task.Color),
                Source = "edit.png",
            };
            //startTaskImageButton.Focused += (sender, e) => startTaskImageButton.BackgroundColor = App.RndColor.TranslateDbColor(task.Color);
            editTaskImageButton.Clicked += async (s, e) => await EditTask(task.Id);

            doneTaskImageButton = new ImageButton 
            { 
                Source = "task_check.png",
            };
            doneTaskImageButton.Clicked += async (s, e) => await SetTaskToDone(task.Id);
            label1 = new Label
            {
                MinimumWidthRequest = 150,
                MaximumWidthRequest = 150,
              /*  TextColor = Color.FromArgb("FF69B4")*//*App.RndColor.TranslateDbColor(task.Color),*/
                TextColor = /*Color.FromArgb("111111")*/App.RndColor.TranslateDbColor(task.Color).AddLuminosity(4.35f).GetComplementary()/*GetComplementary().AddLuminosity(0.4f)*/,
                Text = $"[{task.Status}];[{task.TotalHours}]h;[{task.TotalMinutes}]m;",/*[{testhd.Count()}][{tasks.Count}][{FillTaskViewCounter}]*/

                VerticalOptions = LayoutOptions.Center
            };
            label2 = new Label
            {
                MinimumWidthRequest = 300,
                MaximumWidthRequest = 300,
                /*  TextColor = Color.FromArgb("FF69B4")*//*App.RndColor.TranslateDbColor(task.Color),*/
                TextColor = App.RndColor.TranslateDbColor(task.Color).GetComplementary(),/*App.RndColor.TranslateDbColor(task.Color)*/
                Text = $"[{task.ProjectName}];[{task.Description}];",/*[{testhd.Count()}][{tasks.Count}][{FillTaskViewCounter}]*/

                VerticalOptions = LayoutOptions.End
            };

            label = new Label()
            {
                MinimumWidthRequest = 20,
                MaximumWidthRequest = 20,
                Text = $"{task.Id}",
                IsVisible = true,
            };
            if (task.Status == "running") { startTaskImageButton.Source = "pause_circl.png"; }
            if (task.Status == "paused") { startTaskImageButton.Source = "replaya.png"; }
            
            horizontalTaskEntry.Children.Add(startTaskImageButton);
            horizontalTaskEntry.Children.Add(label1);
            horizontalTaskEntry.Children.Add(label2);
            horizontalTaskEntry.Children.Add(label);
            horizontalTaskEntry.Children.Add(editTaskImageButton);
            /*if (task.Status == "done" || task.Status == "running") {*/ horizontalTaskEntry.Children.Add(doneTaskImageButton);/* }
*/
            SprintTasks.Children.Add(horizontalTaskEntry);
        }
    }
    public async Task EditTask(int taskId)   { }
    public async Task SetTaskToDone(int taskId)   {
        statusMessage.Text = "";
           App.DbHandle.MoveTaskToDoneTasksTable(taskId);
        statusMessage.Text = App.DbHandle.StatusMessage;
        FillSprintViewWithTasks();
        FillDoneTasks();
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
                App.DbHandle.ChangeTaskStatus(task.Id, "paused");
                App.DbHandle.ChangeTaskStatus(taskId, "running");
                //task.Status = "done"; // würde nur die lokale kopie der Daten aber nicht die Db Einträge ändern
                System.Diagnostics.Debug.WriteLine($"if: task.Id = {task.Id}, taskId = {taskId}");
                await App.DbHandle.EditTime(task.Id, "end");
                await App.DbHandle.EditTime(taskId, "start");
                statusMessage.Text = App.DbHandle.StatusMessage;
            }
            else if (task.Status == "running" && task.Id == taskId) { 
                 App.DbHandle.ChangeTaskStatus(task.Id, "paused");
                System.Diagnostics.Debug.WriteLine($"else if: task.Id = {task.Id}, taskId = {taskId}");
                await App.DbHandle.EditTime(taskId, "end");
                statusMessage.Text = App.DbHandle.StatusMessage;
                break;
            }
            else if (task.Id == taskId){
                activeTaskId = task.Id;
                App.DbHandle.ChangeTaskStatus(taskId, "running");
                System.Diagnostics.Debug.WriteLine($"else: task.Id = {task.Id}, taskId = {taskId}");
                statusMessage.Text = App.DbHandle.StatusMessage;
                //if (string.IsNullOrEmpty(task.StartDateTime)&&task.Id == taskId) {
                    await App.DbHandle.EditTime(taskId, "start");
                    System.Diagnostics.Debug.WriteLine($"else: task.StartDateTime = {task.StartDateTime}, taskId = {taskId},{task}");
                    statusMessage.Text = App.DbHandle.StatusMessage;
                //}
            }


        }
        FillSprintViewWithTasks();

        ////JUST SOME TESTS
        projects = App.DbHandle.GetProjectTableSync();
        ProjectLabel.Text = taskId.ToString()+"|||||";
        ProjectLabel.Text += projects[0].StartDateTime + "|||||";
        ProjectLabel.Text += App.DbHandle.ToUtcDateTime() + "|||||";
        ProjectLabel.Text += App.DbHandle.ToLocalDateTime() + "|||||";
    }
    //Obsolete
    public void OnRestartTaskClicked(object sender, EventArgs args)
    {
        if (App.RndColor.Hello())
        {
            string test8 = "";
            for (int i = 0; i < SprintTasks.Children.Count; i++)
            {
                var tmplabel = new Label();
                tmplabel = (Label)SprintTasks.Children[i];

                test8 += tmplabel.Text;
                //test8 += testhd.Children[i].ToString();    
            }
            SprintTasks.Children.Clear();    
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
        FillSprintViewWithTasks();
        FillDoneTasks();

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
    public async void AddNewProject(object sender, EventArgs args)
    {
        statusMessage.Text = "";
        List<string> projectNames = new List<string>(); //var projectNames = new List<string>();

        await App.DbHandle.AddNewProject(NewProjectName.Text, NewProjectDescription.Text); /*+newProjectDescription*/
        statusMessage.Text = App.DbHandle.StatusMessage;
        ProjectOverview.ItemsSource = "";
        ProjectOverview.ItemsSource = GetProjectsTable();
        foreach (var project in GetProjectsTable()) { projectNames.Add(project.Name); }
        projectPicker.ItemsSource = projectNames;

    }
    public async void AddNewTask(object sender, EventArgs args)
    {
        statusMessage.Text = "";
        
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
                    await App.DbHandle.AddNewTask(TaskDescription.Text, ProjectId, ProjectName, "open", TagIDs);
                    statusMessage.Text = App.DbHandle.StatusMessage;
                }

            }
            
            
            FillSprintViewWithTasks();

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
        //if (activeTaskId != -10) { App.DbHandle.ChangeTaskStatus(activeTaskId, "done"); }
        
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