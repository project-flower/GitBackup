using System;
using System.Diagnostics;
using System.IO;

namespace GitBackup
{
    public static class MainEngine
    {
        #region Public Fields

        public static string Command;
        public static string GitPath;
        public static string NewLine;

        #endregion

        #region Public Methods

        public static void Export(string branch, string destination)
        {
            string diff;

            try
            {
                diff = GetDiff(branch);
            }
            catch (Exception exception)
            {
                throw new Exception("Git diff failed.", exception);
            }

            // git.exe が出力する改行は Environment.NewLine ではない
            string[] files = diff.Replace('/', Path.DirectorySeparatorChar).Split(new string[] { NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if (files.Length < 1)
            {
                throw new Exception("No files were found to be backed up.");
            }

            try
            {
                DoExport(files, branch, destination);
            }
            catch (Exception exception)
            {
                throw new Exception("The files could not be exported.", exception);
            }
        }

        #endregion

        #region Private Methods

        private static void DoExport(string[] files, string branch, string destination)
        {
            foreach (string fileName in files)
            {
                try
                {
                    string sourceFileName = Path.Combine(branch, fileName);
                    string destFileName = Path.Combine(destination, fileName);
                    string destDirectoryName = Path.GetDirectoryName(destFileName);

                    if (!Directory.Exists(destDirectoryName))
                    {
                        Directory.CreateDirectory(destDirectoryName);
                    }

                    File.Copy(sourceFileName, destFileName);
                }
                catch
                {
                    throw;
                }
            }
        }

        private static string GetDiff(string branch)
        {
            var process = new Process();

            var startInfo = new ProcessStartInfo()
            {
                Arguments = Command,
                CreateNoWindow = true,
                FileName = GitPath,
                RedirectStandardInput = false,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = branch
            };

            process.StartInfo = startInfo;
            string output;

            try
            {
                process.Start();
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                process.Close();
            }
            catch
            {
                throw;
            }

            return output;
        }

        #endregion
    }
}
