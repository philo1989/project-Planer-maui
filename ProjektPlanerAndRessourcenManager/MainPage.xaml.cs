using ProjektPlanerAndRessourcenManager.DbModels;
using System.Collections.Generic;
using System;
using System.Globalization;
using ProjektPlanerAndRessourcenManager.Pages;
using System.Threading.Tasks;
using System.Linq;
//using Android.App;

namespace ProjektPlanerAndRessourcenManager;

public partial class MainPage : ContentPage
{
    private int ProjectId;
    private string ProjectName;
    private string TagIDs;
    int FillTaskViewCounter = 0;

    Label label1;
    Label label;

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

            projectPicker.SelectedItem = projectNames[0];
            tagPicker.SelectedIndex = 0;
            toggleRecurringTask.SelectedIndex = 0;
        }
        FillTaskView();
    }
    public void FillTaskView(/*bool isLocked = false*/)
    {
        testhd.Children.Clear();
        //if (isLocked)
        //{ return true; }

        List<Tasks> tasks = App.DbHandle.GetTasksTableSync();
        List<Project> projects = App.DbHandle.GetProjectTableSync();

        label = new Label()
        {
            Text = ""
        };

        //ProjectTasksView.ItemsSource = tasks.OrderByDescending(t => t.Id).ToList();
        //ProjectView.ItemsSource = projects./*OrderByDescending(t => t.Id).*/ToList();

        //if (testhd.Count() == tasks.Count)
        //{ 

        //if (FillTaskViewCounter != 0) { return; }
        //else if (FillTaskViewCounter == 0)
        //{

        foreach (var task in tasks.OrderByDescending(t => t.Id).ToList())
        {
            FillTaskViewCounter++;//Debug Variable 

            //Label tesLaabel = new Label();

            //if (label.Text != $"{task.Id}")
            //{

           
            //for (int i = 0; i < testhd.Children.Count; i++)
            //{
            //    var tmplabel = new Label();
            //    tmplabel = (Label)testhd.Children[i];
            //    if (tmplabel.Text == task.Id.ToString()) { return; }
            //    else {
            //        //DisplayAlert("To muttch", "added new task", "hrmpf");
            //        //test8 += testhd.Children[i].ToString();
                    
            //    }
            //}
            
            label1 = new Label
            {
                BackgroundColor = App.RndColor.TranslateDbColor(task.Color),
                Text = $"[{task.ProjectName}][{task.Description}]{task.Id}",/*[{testhd.Count()}][{tasks.Count}][{FillTaskViewCounter}]*/

                VerticalOptions = LayoutOptions.Center
            };
            label = new Label()
            {
                Text = $"{task.Id}",
                IsVisible = false,
            };
            //new HorizontalStackLayout container;
            testhd.Children.Add(label1);
            testhd.Children.Add(label);
        }
        //}
        //return false;
        //}
        //else if (FillTaskViewCounter == tasks.Count()) { FillTaskViewCounter = 0; }
        //}
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
            List<Tasks> tasks = App.DbHandle.GetTasksTableSync();
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

        ProjectTasksView.BackgroundColor = App.RndColor.RndRGBValue();
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

            //await DisplayAlert(">+<", "erfolgreich", "ok");

        }
        else { /*await DisplayAlert("Fehler", "TaskDescritpion can't be null", "ok");*/ }


        //string IDstring = "Id" + ProjectId.ToString() + ".";
        ////await DisplayAlert("holla", IDstring , "Cancel");


        //!!!!Wichtig!!!! Fehlt noch
        startNewTask();
    }
    public void startNewTask()
    { }

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