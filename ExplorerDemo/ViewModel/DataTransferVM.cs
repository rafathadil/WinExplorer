using ExplorerDemo.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerDemo.ViewModel
{
    public static class DataTransferVM
    {
        #region properties  
        public static string LastErrorMsg { get; set; }
        private static bool IsCopy = true;
        private static bool SetToCopyFiles = false;
        static FileTransferModel fileTransferModel { get; set; }
        #endregion

        #region Methods
        static string GetDirectory(string destFilePath)
        {
            string result = string.Empty;
            try
            {
                if (!IsDirectory(destFilePath) && File.Exists(destFilePath))
                {
                    destFilePath = Path.GetDirectoryName(destFilePath);

                    if (!IsDirectory(destFilePath))
                    {
                        throw new Exception("failed to find the destination directory");
                    }
                    else
                    {
                        result = destFilePath;
                    }
                }
                else
                {
                    result = destFilePath;
                }

            }
            catch (Exception EX)
            {
                LastErrorMsg = EX.Message;
            }
            return result;
        }
        static string GetDriveLetter(string path)
        {
            var LS = DriveInfo.GetDrives();

            var Label = "DriveLabel";
            foreach (var item in LS)
            {
                if (item.IsReady && item.Name.Contains(path))
                {
                    Label = item.VolumeLabel;
                }

            }

            return Label;

        }
        
        static void SetModeofCopying()
        {
            DirectoryInfo dir = new DirectoryInfo(fileTransferModel.SourceFile.Path);

            if (dir.Parent == null)
            {
                SetToCopyFiles = false;
            }
            else
                SetToCopyFiles = true;
        }
        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
        static bool IsDirectory(string Path)
        {
            FileAttributes attr = File.GetAttributes(Path);

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                return true;
            else
                return false;
        }
        



        public static bool Pastefiles(FileTransferModel fLTransModel)
        {
            LastErrorMsg = string.Empty;
            fileTransferModel = new FileTransferModel();
            fileTransferModel = fLTransModel;

            IsCopy = fileTransferModel.CurrentAction == FileTransferModel.EAction.copy ? true : false;



            fileTransferModel.DestinationAdress.Path = GetDirectory(fileTransferModel.DestinationAdress.Path);

            bool Result = false;

            if (!string.IsNullOrEmpty(fileTransferModel.DestinationAdress.Path))
            {
                if (IsDirectory(fileTransferModel.SourceFile.Path))
                {
                    SetModeofCopying();
                    Result = MoveDri(fileTransferModel.SourceFile.Path, fileTransferModel.DestinationAdress.Path);
                    if (!IsCopy && SetToCopyFiles)
                    {
                        try
                        {
                            //System.IO.Directory.Delete(fileTransferModel.SourceFile.Path,true);
                            DeleteDirectory(fileTransferModel.SourceFile.Path);
                        }
                        catch(Exception ex)
                        {
                            LastErrorMsg = "Files moved but Cannot source file";
                        }
                    }
                }
                else
                {
                    Result = MoveFiles(fileTransferModel.SourceFile.Path, fileTransferModel.DestinationAdress.Path);
                }
            }
            {
                return Result;
            }

        }



        private static bool MoveDri(string sourceDirName, string destDirName)
        {
            try
            {


                DirectoryInfo dir = new DirectoryInfo(sourceDirName);
                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException(
                        "Source directory does not exist or could not be found: "
                        + sourceDirName);
                }

                if (SetToCopyFiles)
                {
                    destDirName = destDirName +"\\"+ dir.Name;
                }
                else
                {
                    //for copying from drives two drives

                    if (dir.Parent == null)
                        destDirName = destDirName + GetDriveLetter(sourceDirName);
                    else if (string.IsNullOrEmpty(destDirName))
                    {
                        destDirName = destDirName + "\\" + dir.Name;
                    }
                }
                DirectoryInfo[] dirs = dir.GetDirectories();
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }
                FileInfo[] files = dir.GetFiles();

                foreach (FileInfo file in files)
                {
                    if (!file.Attributes.ToString().Contains("Hidden") && !file.FullName.ToString().Contains("System Volume Information") &&
                        !file.Directory.Attributes.ToString().Contains("Hidden"))
                    {
                        try
                        {
                            string temppath = Path.Combine(destDirName, file.Name);
                            if (IsCopy)
                                file.CopyTo(temppath, true);
                            else
                                file.MoveTo(temppath);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }
                foreach (DirectoryInfo subdir in dirs)
                {
                    if (!subdir.Attributes.ToString().Contains("Hidden") && !subdir.FullName.ToString().Contains("System Volume Information"))
                    {
                        try
                        {
                            string temppath = Path.Combine(destDirName, subdir.Name);
                            MoveDri(subdir.FullName, temppath);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }

                    }

                }
            }
            catch (Exception EX)
            {

                LastErrorMsg = EX.Message;
                return false;
            }
            return true;
        }

        static bool MoveFiles(string SoureDest, string DestDir)
        {
            try
            {
                if (IsCopy)
                {
                    File.Copy(SoureDest, Path.Combine(DestDir, Path.GetFileName(SoureDest)), true);
                }
                else
                {
                    File.Move(SoureDest, Path.Combine(DestDir, Path.GetFileName(SoureDest)));
                }
                return true;
            }
            catch (Exception EX)
            {

                LastErrorMsg = EX.Message;
                return false;
            }



        }
        #endregion

    }
}
