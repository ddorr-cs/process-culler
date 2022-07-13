using System;
using System.IO;
using System.Diagnostics;

namespace commandline_practice
{
    class Program
    {
        //Static Variables
        static string killCommand = "/C taskkill /PID ";
        static string killCommandOption = " /F";
        static string writeTasksCommand = @"/C tasklist > ";
        static string workingDir = @"C:\Users\maria\Desktop\";
        static string targetFile = "writeMe.txt";
        static string targetProcess = "mspaint.exe";

        public class numDups
        {
            public int number { get; set; }
        }

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

        public void FindDuplicateProcesses(numDups myNumDups)
        {
            //Process Duplicate Hunting Strings
            string[] lines = File.ReadAllLines(workingDir + targetFile);
            string[] tempLines = lines;
            string[] processA, processB;

            //Loop through 2 copies of the Tasks file, 2nd loop ahead of the 1st to find matches
            Console.WriteLine("Contents of WriteMe:");
            for (int i = 3; i < lines.Length - 1; i++)
            {
                Console.WriteLine("\t" + lines[i]);

                for (int j = i + 1; j < tempLines.Length; j++)
                {
                    //Get just the process name from the current lines
                    processA = lines[i].Split(' ');
                    processB = tempLines[j].Split(' ');              

                    //Compare both process names, if they match and match the target, DO SOMETHING
                    if (processA[0] == processB[0] && processA[0] == targetProcess)
                    {
                        //Console.WriteLine("\n WHOA WHOA WHOA DUPLICATE FOUND\n");
                        myNumDups.number++;
                        DuplicateProcessFound(processA, processB);

                    }
                }
            }
        }

        public void DuplicateProcessFound(string[] processA, string[] processB)
        {
            //TESTING SPLIT
            string[] copyProcA = new string[10];
            string[] copyProcB = new string[10];
            int counterA = 0;
            int counterB = 0;

            //Parse thru ProcessA and ProcessB to remove white space, making each column in tasklist its own element in the arr.
            for (int k = 0; k < processA.Length; k++)
            {
                if (!processA[k].All(char.IsWhiteSpace))
                {
                    copyProcA[counterA] = processA[k];

                    counterA++;
                }
            }

            for (int l = 0; l < processB.Length; l++)
            {
                if (!processB[l].All(char.IsWhiteSpace))
                {
                    copyProcB[counterB] = processB[l];

                    counterB++;
                }
            }

            //Determine which of the duplicate processes to kill by PID
            if (Int32.Parse(copyProcA[1]) >= Int32.Parse(copyProcB[1]))
            {
                Console.WriteLine("\nKILLING no1:" + copyProcA[0] + " " + copyProcA[1] + " " + copyProcA[2] + "\n");
                KillProcessHandler(Int32.Parse(copyProcA[1]));
            }
            else if (Int32.Parse(copyProcA[1]) < Int32.Parse(copyProcB[1]))
            {
                Console.WriteLine("\nKILLING no2:" + copyProcB[0] + " " + copyProcB[1] + " " + copyProcB[2] + "\n");
                KillProcessHandler(Int32.Parse(copyProcB[1]));
            }
        }

        public void KillProcessHandler(int targetPID)
        {
            Process killProcess = new Process();
            killProcess.StartInfo.FileName = "cmd.exe";
            //killProcess.StartInfo.WorkingDirectory = workingDir;
            killProcess.StartInfo.Arguments = killCommand + targetPID.ToString() + killCommandOption;
            killProcess.Start();
            killProcess.WaitForExit();
        }

        static void Main(string[] args)
        {
            Program myProgram = new Program();
            numDups myNumDups = new numDups();

            //STEP 1
            myProgram.WriteCommandToFile();
            //STEP 2
            myProgram.FindDuplicateProcesses(myNumDups);
            //STEP 3 finish line
            Console.WriteLine("\nNumber of Duplicates Found: " + myNumDups.number);

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }
    }
}
