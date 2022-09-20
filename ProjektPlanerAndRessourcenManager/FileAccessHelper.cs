using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektPlanerAndRessourcenManager
{
    public class FileAccessHelper
    {
        public static string GetLocalFilePath(string filename, int buildStyle)
        {
            if (buildStyle == 0)
            {
                string test = Environment.ProcessPath;
                /*Chaange back to FileSystem.AppDirectory when publishing "properly"*/
                return System.IO.Path.Combine(FileSystem.AppDataDirectory/*@"J:\NoNo\source\repos\ProjektPlanerAndRessourcenManager\bin\Release\net6.0-windows10.0.19041.0\win10-x64\publish"*/
                                        , filename);
                //Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else if (buildStyle == 1)
            {
                return System.IO.Path.Combine(Environment.CurrentDirectory, filename);
            }
            else return "error";
        }
    }
}
