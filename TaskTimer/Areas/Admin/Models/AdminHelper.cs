using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TaskTimer.Areas.Admin.Models
{
    public class AdminHelper
    {
        public static string GetUniqueFileName(string dirPath, string fileName)
        {
            var uniqueFileName = fileName;
            var allSitePageImages = Directory.GetFiles(dirPath);
            var isFileAlreadyExist = false;
            foreach (var filePath in allSitePageImages)
            {
                var fileNameFromDir = Path.GetFileName(filePath);
                if (fileNameFromDir.Equals(uniqueFileName)) //search for new file name
                {
                    isFileAlreadyExist = true;
                    break;
                }
            }
            if (isFileAlreadyExist) //get new file name
            {
                var saveFileCounter = 1;
                var uniqueFileNameArr = uniqueFileName.Split('.');
                do //check if file with same name already exist
                {
                    var newUniqueFileName = uniqueFileNameArr[0] + "(" + saveFileCounter + ")." + uniqueFileNameArr[1];
                    isFileAlreadyExist = false;
                    foreach (var filePath in allSitePageImages)
                    {
                        var fileNameFromDir = Path.GetFileName(filePath);
                        if (fileNameFromDir.Equals(newUniqueFileName)) //search for new file name
                        {
                            isFileAlreadyExist = true;
                            saveFileCounter++;
                            break;
                        }
                        uniqueFileName = newUniqueFileName;
                    }
                } while (isFileAlreadyExist);
            }
            return uniqueFileName;
        }
        public static string ClearStrForDBSave(string str)
        {
            return str.Replace("%",string.Empty).Replace("'",string.Empty);
        }
    }
}