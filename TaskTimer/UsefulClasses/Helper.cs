using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace TaskTimer.UsefulClasses
{
    public class Helper
    {
        public static string GetUniqueFileName(string dirName, string fileName) 
        {
            DirectoryInfo dir = new DirectoryInfo(dirName);
            int counter = 1, i = 0;
            string newName = fileName.Replace(',',' ');
            var fileNameArr = newName.Split('.');
            string newFileName = "", newFileExt = "";
            for (; i < fileNameArr.Length - 1; i++)
                newFileName += fileNameArr[i];
            newFileExt = fileNameArr[i];
            IEnumerable<FileInfo> files;
            do
            {
                files = dir.EnumerateFiles(searchPattern: newName, searchOption: SearchOption.AllDirectories);
                if (files.Count() > 0)  //if file with same name already exist in directory
                {
                    newName = newFileName + "(" + counter + ")." + newFileExt;
                    counter++;
                }
            } while (files.Count() > 0);
            return newName;
        }
        public static void CheckQuestionImages(string dirName, string question, string[] fileNames)
        {
            //move files from tmp dir
            string tmpDirName = dirName + "Temporary\\";
            foreach (var fileName in fileNames)
                if (!question.Contains(fileName) && File.Exists(tmpDirName + fileName))
                    File.Delete(tmpDirName + fileName);
        }
        //check if directory exist during app start
        public static void CheckDirectorysOnServer(string[] dirNames)
        {
            foreach (var dirName in dirNames)
                if (!Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);
        }
        public static void MoveFiles(string dirName, string[] fileNames)
        {
            //move files from tmp dir
            string tmpDirName = dirName + "Temporary";
            foreach (var fileName in fileNames)
                if (File.Exists(tmpDirName + "\\" + fileName))
                    File.Move(tmpDirName + "\\" + fileName, dirName + "\\" + fileName);

            //delete old tmp files
            DirectoryInfo tmpDir = new DirectoryInfo(tmpDirName);
            var allFiles = tmpDir.EnumerateFiles();
            foreach (var file in allFiles)
                if ((DateTime.Now - file.CreationTime).Days > 0) 
                    file.Delete();
        }

        public static void RemoveFile(string dirName, string fileName)
        {
            if (File.Exists(dirName + fileName))
                File.Delete(dirName + fileName);
        }

        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;

            string newUrl = serverUrl;
            Uri originalUri = System.Web.HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                "://" + originalUri.Authority + newUrl;
            return newUrl;
        } 
    }
    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override Boolean IsValidForRequest(
                 ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request.IsAjaxRequest();
        }
    }
    
}