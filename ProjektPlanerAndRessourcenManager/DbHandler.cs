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
            await SQLiteAsyncConnection.CreateTablesAsync<Project, Tasks, TimeManagmentProtocol, Starts, DoneTasks>();

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
        public List<DoneTasks> GetDoneTasksTableSync()
        {
            try
            {
                SyncInit();
                return SQLiteConnection.Table<DoneTasks>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<DoneTasks>();
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
        public async void MoveTaskToDoneTasksTable(int taskId)
        {
            int result = 0;
            List<Tasks> tasks = GetTasksTableSync();
            foreach (Tasks task in tasks)
            {
                if (taskId == task.Id)
                {
                    try
                    {
                        await Init();
                        //if (string.IsNullOrEmpty(projectName)) throw new Exception("Valid Name required... ToDo: Add Allowed Charaacters");
                        //string sqlCommandEnd = "UPDATE tasks SET EndDateTime=\"" + DateTime.Now + "\" WHERE Id=\"" + taskId + "\"";
                        string sqlCommnd = "DELETE FROM tasks WHERE Id=\"" + taskId + "\"";

                        result = await SQLiteAsyncConnection.InsertAsync(new DoneTasks
                        {
                            Id = task.Id,
                            ProjectID = task.ProjectID,
                            ProjectName = task.ProjectName,
                            Status = "IsDone :)",
                            Description = task.Description,
                            IsDone = true,
                            Color = task.Color,
                            TimesOfBeeingStarted = task.TimesOfBeeingStarted,
                            EditingHistory = task.EditingHistory,
                            TotalHours = task.TotalHours,
                            TotalMinutes = task.TotalMinutes,
                            TotalTimeInMinutes = task.TotalTimeInMinutes,
                            TagIDs = task.TagIDs
                        });
                        result = await SQLiteAsyncConnection.ExecuteAsync(sqlCommnd);

                        StatusMessage = string.Format("{0} new Time() added (Name: {1})", result, sqlCommnd);
                    }
                    catch (Exception ex)
                    {
                        StatusMessage = string.Format("Failed to add {0}. Error: {1}", taskId, ex.Message);
                    }
                }
            }

        }
        public async Task AddNewTask(string TaskDescription, int ProjectID, string ProjectName, string status, string tagIDs)
        {
            int result = 0;
            string ProjectColor = "";
            DateTime date = DateTime.Now;
            try
            {
                await Init();
                if (string.IsNullOrEmpty(TaskDescription)) throw new Exception("Valid Name required... ToDo: Add Allowed Characters");

                List<Project> projects = await SQLiteAsyncConnection.Table<Project>().ToListAsync();
                foreach (Project project in projects)
                {
                    if (project.Id == ProjectID)
                    {
                        ProjectColor = project.Color;//*"ff00ff"*/;
                    }
                }

                result = await SQLiteAsyncConnection.InsertAsync(new Tasks { ProjectID = ProjectID, ProjectName = ProjectName, Description = TaskDescription, Status = status, TagIDs = tagIDs + ",", Color = ProjectColor, TimesOfBeeingStarted = 0 });
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
        public async Task EditTime(int taskId, string kind)
        {
            int result = 0;


            if (kind == "end")
            {
                string sqlCommandEnd = "UPDATE tasks SET EndDateTime=\"" + DateTime.Now + "\" WHERE Id=\"" + taskId + "\"";
                try
                {

                    await Init();
                    result = await SQLiteAsyncConnection.ExecuteAsync(sqlCommandEnd);
                    CalculateAndWriteTimeEffort(taskId);
                }
                catch (Exception ex) { StatusMessage = string.Format("Failed to Set EndDateTime, sql->{0}, Exception {1} ", sqlCommandEnd, ex.Message); }
            }

            if (kind == "start")
            {
                string sqlCommandStart = "UPDATE tasks SET StartDateTime=\"" + DateTime.Now + "\" WHERE Id=\"" + taskId + "\"";
                try
                {

                    await Init();
                    result = await SQLiteAsyncConnection.ExecuteAsync(sqlCommandStart);
                }
                catch (Exception ex) { StatusMessage = string.Format("Failed to StartDaateTime, sql->{0}, Exception {1} ", sqlCommandStart, ex.Message); }
            }
        }
        //Check if there is already a time entry in for the task in db
        //TODO: Check if sttart&endDateTime is on the same daay,year
        private void CalculateAndWriteTimeEffort(int taskId)
        {
            int result = 0;
            List<Tasks> tasks = App.DbHandle.GetTasksTableSync();
            foreach (Tasks task in tasks)
            {
                if (task.Id == taskId)
                {
                    DateTime startTime = DateTime.Parse(task.StartDateTime);
                    DateTime endTime = DateTime.Parse(task.EndDateTime);

                    TimeSpan deltatime = endTime.Subtract(startTime);

                    int newTotalHours = deltatime.Hours + task.TotalHours;
                    int newTotalMinutes = deltatime.Minutes + task.TotalMinutes;

                    float newTotalTimeInMinutes = (float)(deltatime.TotalMinutes) + task.TotalTimeInMinutes;
                    int startCount = task.TimesOfBeeingStarted++;
                    //Get possible existing values from tasktablelist and add them to the total before writing to db
                    string sqlCommandHours = "UPDATE tasks SET TotalHours=\"" + newTotalHours + "\" WHERE Id=\"" + taskId + "\"";
                    string sqlCommandMinutes = "UPDATE tasks SET TotalMinutes=\"" + newTotalMinutes + "\" WHERE Id=\"" + taskId + "\"";
                    string sqlCommandTotalMinutes = "UPDATE tasks SET TotalTimeInMinutes=\"" + newTotalTimeInMinutes + "\" WHERE Id=\"" + taskId + "\"";
                    string sqlCommandTimesStarted = "UPDATE tasks SET TimesOfBeeingStarted=\"" + startCount + "\" WHERE Id=\"" + taskId + "\"";

                    //string sqlCommandClearStart = "DELETE StartDateTime FROM tasks WHERE Id=\"" + taskId + "\"";
                    string sqlCommandClearStart = $"UPDATE tasks SET StartDateTime=\"" + "" + "\" WHERE Id=\"" + taskId + "\"";
                    string sqlCommandClearEnd = "UPDATE tasks SET EndDateTime=\"" + "" + "\" WHERE Id=\"" + taskId + "\"";


                    try
                    {
                        SyncInit();

                        //write new totals to tasks table
                        result = SQLiteConnection.Execute(sqlCommandHours);
                        result = SQLiteConnection.Execute(sqlCommandMinutes);
                        result = SQLiteConnection.Execute(sqlCommandTotalMinutes);
                        result = SQLiteConnection.Execute(sqlCommandTimesStarted);
                        //Add Times to starts table
                        result = SQLiteConnection.Insert(new Starts { TasksID = task.Id, StartDateTime = task.StartDateTime, EndDateTime = task.EndDateTime, TotalHours = deltatime.Hours, TotalMinutes = deltatime.Minutes, TotalTimeInMinutes = (float)(deltatime.TotalMinutes)/*Convert.ToInt32(*//*)*/ });
                        //Clear Start&&EndTime in tasks Taable

                        result = SQLiteConnection.Execute(sqlCommandClearStart);
                        result = SQLiteConnection.Execute(sqlCommandClearEnd);


                        //await Init();
                        //result = await SQLiteAsyncConnection.ExecuteAsync(sqlCommand);
                    }
                    catch (Exception ex) { StatusMessage = string.Format("Failed to do allota thhings, sql->{0}||{1}||{2}, Exception {3} ", sqlCommandHours, sqlCommandMinutes, sqlCommandTotalMinutes, ex.Message); }
                }
            }


        }

        public async Task<bool> CheckStartTime(int taskId)
        {
            List<Tasks> tasks = await App.DbHandle.GetTasksTable();

            return false;
        }
        public void ChangeTaskStatus(int taskId, string newStatus)
        {
            int result = 0;
            List<Tasks> tasks = GetTasksTableSync();
            string TaskDescription = "NONE";
            string oldStatus = "";
            foreach (Tasks task in tasks)
            {
                if (task.Id == taskId) { TaskDescription = task.Description; oldStatus = task.Status; }
            }
            //string task;
            //string sqlCmdOldStatus = $"SELECT Description FROM Tasks WHERE Id=\"" + taskId + "\"";
            string sqlCommand = $"UPDATE tasks SET Status=\"" + newStatus + "\" WHERE Id=\"" + taskId + "\"";

            try
            {
                SyncInit();
                //await connection.
                //task = SQLiteConnection.Execute(sqlCmdOldStatus);
                result = SQLiteConnection.Execute(sqlCommand);
                //result = await SQLiteAsyncConnection.InsertAsync(new Tasks { StartDateTime = DateTime.Now.ToString() });

                StatusMessage = string.Format("Task: {0}`s, Status changed from: {1} to: {2}  ", TaskDescription, oldStatus, newStatus);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to change Status (SQL COmmand->){0}, Exception {1} ", sqlCommand, ex.Message);
            }
        }
        //public async Task ChangeTaskStatus(int taskId, string newStatus)
        //{
        //    int result = 0;

        //    string sqlCommand = $"UPDATE tasks SET Status=\"" + newStatus + "\" WHERE Id=\"" + taskId + "\"";

        //    try
        //    {
        //        await Init();
        //        //await connection.

        //        result = await SQLiteAsyncConnection.ExecuteAsync(sqlCommand);
        //        //result = await SQLiteAsyncConnection.InsertAsync(new Tasks { StartDateTime = DateTime.Now.ToString() });
        //        StatusMessage = string.Format("{0} new Project() added (Name: {1})", result, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage = string.Format("Failed to add Time {0}, Exception {1} ", sqlCommand, ex.Message);
        //    }
        //}
        //private void CalculateAndWriteTimeEffort(int taskId)
        //{
        //    int result = 0;
        //    List<Tasks> tasks = App.DbHandle.GetTasksTableSync();
        //    foreach (Tasks task in tasks)
        //    {
        //        if (task.Id == taskId)
        //        {

        //            int year = int.Parse(task.EndDateTime.Substring(6, 4));//11,12
        //            int month = int.Parse(task.EndDateTime.Substring(3, 2));//11,12
        //            int day = int.Parse(task.EndDateTime.Substring(0, 2));//11,12

        //            int endTimeHours = int.Parse(task.EndDateTime.Substring(11, 2));//11,12
        //            int endTimeMinutes = int.Parse(task.EndDateTime.Substring(14, 2));//11,12
        //            int endTimeSeconds = int.Parse(task.EndDateTime.Substring(17, 2));//11,12
        //            int startTimeHours = int.Parse(task.StartDateTime.Substring(11, 2));//11,12
        //            int startTimeMinutes = int.Parse(task.StartDateTime.Substring(14, 2));//11,12
        //            int startTimeSeconds = int.Parse(task.StartDateTime.Substring(17, 2));//11,12

        //            int _totlH = endTimeHours - startTimeHours;
        //            int _totlM = endTimeMinutes - startTimeMinutes;
        //            int _totlS = endTimeSeconds - startTimeSeconds;

        //            int totlH = _totlH;
        //            //alles auf minutten als baasis bringen
        //            _totlH *= 60; //in m
        //            _totlS /= 60; // in m

        //            float totalTimeMinuten = (_totlH + _totlM + _totlS);
        //            float totalTimeStunden = totalTimeMinuten / 60; // wieder zurück zu stunden


        //            //Getting possible existing values and dd them to tthe ttotalk before writing to db
        //            string sqlCommandHours = $"UPDATE tasks SET TotalHours=\"" + totlH + task.TotalHours + "\" WHERE Id=\"" + taskId + "\"";
        //            string sqlCommandMinutes = $"UPDATE tasks SET TotalMinutes=\"" + _totlM + task.TotalMinutes + "\" WHERE Id=\"" + taskId + "\"";
        //            string sqlCommandTotalMinutes = $"UPDATE tasks SET TotalTimeInMinutes=\"" + totalTimeMinuten + task.TotalTimeInMinutes + "\" WHERE Id=\"" + taskId + "\"";
        //            string sqlCommandTimesStarted = $"UPDATE tasks SET TimesOfBeeingStarted=\"" + task.TimesOfBeeingStarted++ + "\" WHERE Id=\"" + taskId + "\"";

        //            //string sqlCommandClearStart = $"UPDATE tasks SET StartDateTime=\"" + "" + "\" WHERE Id=\"" + taskId + "\"";
        //            //string sqlCommandClearEnd = $"UPDATE tasks SET EndDateTime=\"" + "" + "\" WHERE Id=\"" + taskId + "\"";


        //            try
        //            {
        //                SyncInit();

        //                //write new totals to tasks table
        //                result = SQLiteConnection.Execute(sqlCommandHours);
        //                result = SQLiteConnection.Execute(sqlCommandMinutes);
        //                result = SQLiteConnection.Execute(sqlCommandTotalMinutes);
        //                result = SQLiteConnection.Execute(sqlCommandTimesStarted);
        //                //Add Times to starts table
        //                result = SQLiteConnection.Insert(new Starts { TasksID = task.Id, StartDateTime = task.StartDateTime, EndDateTime = task.EndDateTime, TotalHours = totlH, TotalMinutes = _totlM, TotalTimeInMinutes = totalTimeMinuten });
        //                //Clear Start&&EndTime in tasks Taable

        //                //result = SQLiteConnection.Execute(sqlCommandClearStart);
        //                //result = SQLiteConnection.Execute(sqlCommandClearEnd);


        //                //await Init();
        //                //result = await SQLiteAsyncConnection.ExecuteAsync(sqlCommand);
        //            }
        //            catch (Exception ex) { StatusMessage = string.Format("Failed to Add TotalTime Edit ttime, sql->{0}||{1}||{2}, Exception {3} ", sqlCommandHours, sqlCommandMinutes, sqlCommandTotalMinutes, ex.Message); }


        //            //_totlH *= 60;
        //            //_totlS /= 60;
        //            ////int tmphours = ((endTimeHours - startTimeHours)*60)+();      
        //            //int startseconds = ((startTimeHours*60) * 60) + (startTimeMinutes*60)+startTimeSeconds;
        //            //int endseconds = ((endTimeHours * 60) * 60) + (endTimeMinutes * 60) + endTimeSeconds;
        //            //float totalTime = (_totlH + _totlM + _totlS);
        //            //totalTime /= 60;
        //        }
        //    }


        //}

    }
}
