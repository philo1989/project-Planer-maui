
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;
using System.Threading.Tasks;

/*MAYBE IMPORTANT for Compiling to different platforms ?*/
//using static Android.Media.Audiofx.DynamicsProcessing;
//using System.ComponentModel.DataAnnotations;

namespace ProjektPlanerAndRessourcenManager.DbModels
{
    //PROJECTCOLORS
    [Table("project")]
    public class Project
    {
        [PrimaryKey, AutoIncrement,/*, Column("_id")*/]
        public int Id { get; set; }

        [MaxLength(250), Unique]
        public string Name { get; set; }

        [MaxLength(10)] /*Probably replace this with a custom DataType//Class ala public class Version {public Enum phase = [Alpha=a,Beta=b,Final = NUll,Preproduction = pre]; public float version number; public char VersionModifier*/
        public string Version { get; set; }/* = string.Format("{0}{1}{2}");*/
        [MaxLength(1000)]
        public string ProjectDescription { get; set; }
        public string Customer { get; set; }
        public int OpenTasks { get; set; }
        public int ClosedTasks { get; set; }
        public int AllTasks { get; set; }
        public string StartDateTime { get; set; } ///DD.MM.YYYY HH:MM:SS
        public string EndDateTime { get; set; } ///DD.MM.YYYY HH:MM:SS
        public string Color { get; set; }
        public int FontColor { get; set; }
        //public DateTime StartDate { get; set; }
        public float SpendTime { get; set; }
        public enum ProjectType { Private, Comission, Intern}

        //public string Type { get; set; } //replace with Enum when you looked up how to use them; Possible Values = PrivateProject, Art, Misc,   
    }
    /*Project is 1 to n Tasks */ //not possible becouse sqlite--net-pcl doesn't support foreignKEys instead use Indexed and some custom code ;-| 
    [Table("tasks")]
    public class Tasks
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Project))] //SQLite NET EXTENSIONS SEI DANK !!!!! ---> Extensions? what are they again ?
        public int ProjectID  { get; set; } //ToDo: INDXED + Setter Code somwhow link to Project table <--- BÖSE !!!!
        public string ProjectName  { get; set; }
        public string Status { get; set; }
        [Unique]
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public string Color { get; set; }
        public int FontColor { get; set; }
        public string StartDateTime { get; set; } ///DD.MM.YYYY HH:MM:SS
        public string EndDateTime { get; set; } ///DD.MM.YYYY HH:MM:SS
        public int TimesOfBeeingStarted { get; set; } = 0;
        public string EditingHistory { get; set; } 
        //ls json reinnschreiben? oder selbst parsen so ala
        //- > [inhalt]
        /* for history.lenght i++
         * histor[i] ist fgdfg //      
         * */
        public int TotalHours { get; set; } //ls json reinnschreiben?
        public int TotalMinutes { get; set; } //ls json reinnschreiben?
        public float TotalTimeInMinutes { get; set; } //ls json reinnschreiben?
        public string TagIDs { get; set; } //1:n relation//speichert alle tags durch ihre Ids in der jeweiligen AUfgaabe, muss durch Programm geparst werden da Sqlite keine Array/Listen Datentypen zur verfügung stellt
    }
    /*has subTasks as a bool flag in tthe tasks tttable aand also a int numberOFSubttasks,
    then in an seperate subtask table store the tasks and give them the main task id as a foreign key,....*/
    public class SubTasks
    {

    }
    public class DeletedTasks
    {

    }
    [Table("starts")]
    public class Starts
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TasksID { get; set; }
        public bool IsSubTask { get; set; } = false;
        public string StartDateTime { get; set; } ///DD.MM.YYYY HH:MM:SS
        public string EndDateTime { get; set; } ///DD.MM.YYYY HH:MM:SS
        public int TotalHours { get; set; } //ls json reinnschreiben?
        public int TotalMinutes { get; set; } //ls json reinnschreiben?
        public float TotalTimeInMinutes { get; set; } //ls json reinnschreiben?

    }
    public class Owner { }
    public class Customer { }
    public class OpenTasks { }
    public class ClosedTasks { }
    public class ReopendTasks { }
    public class TimeManagmentProtocol
    { 
        public int Id { get; set; }
        public string DateTime { get; set; } //DD.MM.YYYY HH:MM:SS
        //public string TaskName { get; set; } 

    }
    public class Tags
    {
        public string tagName { get; set; }
        public int tagID  { get; set; }

    }
    public class Sprints
    {
        public string StartDate;
        public string EndDate;
        public int[] TaskIds;
        // necessary ???
    }
    public class RoadMap { }
    public class Collaborator { }
}
