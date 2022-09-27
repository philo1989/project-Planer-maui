using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using ProjektPlanerAndRessourcenManager.DbModels;
using ProjektPlanerAndRessourcenManager.Pages;
using Microsoft.Maui.Controls;

namespace ProjektPlanerAndRessourcenManager
{
    public class DbHandler
    {
        string _dbPath;

        public string StatusMessage { get; set; }

        private SQLiteAsyncConnection SQLiteAsyncConnection;
        private SQLiteConnection SQLiteConnection;
        private async Task Init()
        {
            if (SQLiteAsyncConnection != null) return;

            SQLiteAsyncConnection = new SQLiteAsyncConnection(_dbPath);
            await SQLiteAsyncConnection.CreateTablesAsync<Project, Tasks, TimeManagmentProtocol>();

        }
        private void SyncInit()
        {
            if (SQLiteConnection != null) return;

            SQLiteConnection = new SQLiteConnection(_dbPath);
            SQLiteConnection.CreateTables<Project, Tasks, TimeManagmentProtocol>();
        }
        public DbHandler(string dbPath)
        {
            _dbPath = dbPath;

        }

        public async Task AddNewProject(string projectName, string projectDescription)
        {
            int result = 0;
            DateTime date = DateTime.Now;
            try
            {
                await Init();
                if (string.IsNullOrEmpty(projectName)) throw new Exception("Valid Name required... ToDo: Add Allowed Charaacters");
                //string NewRandomColor = App.RndColor.RndRGBValue().ToString();
                result = await SQLiteAsyncConnection.InsertAsync(new Project { Name = projectName, ProjectDescription = projectDescription, Version = "test", StartDateTime = date.ToString(), Color = App.RndColor.GetColor().ToArgbHex(), });
                StatusMessage = string.Format("{0} new Project() added (Name: {1})", result, projectName);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", projectName, ex.Message);
            }


            /*.Insert returns the number of added rows*/

            //functionality
        }
        public async Task TestManagmentProtocollDate()
        {
            int result = 0;
            DateTime now = DateTime.Now;
            try
            {
                await Init();
                //if (string.IsNullOrEmpty(projectName)) throw new Exception("Valid Name required... ToDo: Add Allowed Charaacters");

                result = await SQLiteAsyncConnection.InsertAsync(new TimeManagmentProtocol { DateTime = now.ToString() });
                StatusMessage = string.Format("{0} new Time() added (Name: {1})", result, now);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", now, ex.Message);
            }
        }
        public async Task<List<Project>> GetProjectTable()
        {
            try
            {
                await Init();
                return await SQLiteAsyncConnection.Table<Project>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<Project>();
        }
        //public async Task<int> GetProjectId(string ProjectName)
        //{
        //    try
        //    {
        //        await Init();
        //        await SQLiteAsyncConnection.Table<Project>().Where()
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        //    }

        //    return new List<Project>();
        //}
        public List<Project> GetProjectTableSync()
        {
            try
            {
                SyncInit();
                return SQLiteConnection.Table<Project>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<Project>();
        }
        public List<Tasks> GetTasksTableSync()
        {
            try
            {
                SyncInit();
                return SQLiteConnection.Table<Tasks>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<Tasks>();
        }

        //public async Task<bool> CheckForExistingProjects()//And Build List Dough WAYYYYYNNNEEE
        //{
        //    List<Project> projects = await App.DbHandle.GetProjectTable();
        //    if (projects.Count() == 0)
        //    {
        //        await Shell.Current.GoToAsync(nameof(ExamplePage));
        //        return false;
        //    }
        //    return true;

        //}

        public async Task AddNewTask(string TaskDescription, int ProjectID, string ProjectName, string status, string tagIDs)
        {
            int result = 0;
            string ProjectColor = "";
            DateTime date = DateTime.Now;
            try
            {
                await Init();
                if (string.IsNullOrEmpty(TaskDescription)) throw new Exception("Valid Name required... ToDo: Add Allowed Charaacters");

                List<Project> projects = await SQLiteAsyncConnection.Table<Project>().ToListAsync();
                foreach (Project project in projects)
                {
                    if (project.Id == ProjectID)
                    {
                        ProjectColor = project.Color;//*"ff00ff"*/;
                    }
                }

                result = await SQLiteAsyncConnection.InsertAsync(new Tasks { ProjectID = ProjectID, ProjectName = ProjectName, Description = TaskDescription, Status = status, TagIDs = tagIDs + ",", Color = ProjectColor });
                StatusMessage = string.Format("{0} new Project() added (Name: {1})", result, TaskDescription);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", TaskDescription, ex.Message);
            }
        }
        public async Task EditTask(string name)
        {
            //functionality
        }
        public async Task<List<Tasks>> GetTasksTable()
        {
            try
            {
                await Init();
                return await SQLiteAsyncConnection.Table<Tasks>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<Tasks>();
        }
        public DateTime ToUtcDateTime()
        {
            return DateTime.UtcNow;

        }
        public DateTime ToLocalDateTime()
        {
            return DateTime.UtcNow;

        }

        public async Task AddStartTimeToTask(int taskId)
        {
           
            int result = 0;
            object _result;
            string sqlcmd = ("UPDATE Employee SET Skill=\"" + taskId + "\" WHERE BadgeId=\"" + taskId + "\"");

            string sqlCommand = $"UPDATE tasks SET StartDateTime=\"" + DateTime.Now + "\" WHERE Id=\"" + taskId + "\"";
            //string sqlCommand = $"SELECT StartDateTime FROM tasks WHERE Id {taskId} ";
            //_result = await SQLiteAsyncConnection.FindAsync<Tasks>(c => c.Id == taskId); --> thorws exception 
            //await SQLiteAsyncConnection.InsertAsync(sqlCommand);
            try
            {
                await Init();
                //await connection.
                
                result = await SQLiteAsyncConnection.ExecuteAsync(sqlCommand);
                //result = await SQLiteAsyncConnection.InsertAsync(new Tasks { StartDateTime = DateTime.Now.ToString() });
                StatusMessage = string.Format("{0} new Project() added (Name: {1})", result, result);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add Time {0}, Exception {1} ", sqlCommand, ex.Message);
            }
        }

    }
}
