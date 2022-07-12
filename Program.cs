using System;
using System.IO;
using System.Diagnostics;

namespace commandline_practice
{
    class Program
    {
        //Static Variables
        static string killCommand = "/C taskkill /PID 2856 /F";
        static string writeTasksCommand = @"/C tasklist > ";
        static string workingDir = @"c:\users\derek\desktop\";
        static string targetFile = "writeMe.txt";
        static string targetProcess = "lghub.exe";

        public void WriteCommandToFile()
        {
            //Open a CMD, get a list of tasks, write it to a text file
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.WorkingDirectory = workingDir;
            process.StartInfo.Arguments = writeTasksCommand + targetFile;
            process.Start();
            process.WaitForExit();
        }

        public void HandleDuplicateProcesses()
        {
            //Process Duplicate Hunting Strings
            string[] lines = File.ReadAllLines(workingDir + targetFile);
            string[] tempLines = lines;
            string processA, processB;
            

            //Loop through 2 copies of the Tasks file
            Console.WriteLine("Contents of WriteMe:");
            for (int i = 3; i < lines.Length - 1; i++)
            {
                Console.WriteLine("\t" + lines[i]);

                for (int j = i + 1; j < tempLines.Length; j++)
                {
                    //Get just the process name from the current lines
                    processA = lines[i].Split(' ')[0];
                    processB = tempLines[j].Split(' ')[0];

                    //Compare both process names, if they match and match the target, DO SOMETHING
                    if (processA == processB && processA == targetProcess)
                    {
                        Console.WriteLine("\n WHOA WHOA WHOA\n");

                        /*
                         TO DO
                         */

                    }
                }

            }

        }

        static void Main(string[] args)
        {
            Program myProgram = new Program();

            //STEP 1
            myProgram.WriteCommandToFile();
            //STEP 2
            myProgram.HandleDuplicateProcesses();

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }
    }
}
