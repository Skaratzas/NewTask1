using LibGit2Sharp;
using System;
using System.IO;


namespace Task1._0._1
{
    class Program
    {     
        static void Main(string[] args)
        {

            string root = @"C:\GitHub\repository";

            if (!Directory.Exists(root))
            {
                Repository.Clone("https://github.com/Skaratzas/NewTask1", root);
            }
            else
            {
                gitPull(root);
            }

        
            using (var repository = new Repository(root))
            {

                Console.WriteLine("Choose a branch: ");
                string chosenBranch = Console.ReadLine();
             
                var branch = repository.Branches[chosenBranch];
                Branch currentBranch = Commands.Checkout(repository, branch);


                Repository.Init(@"C:\GitHub\git");

                NewCommit commits = new NewCommit(repository);
                commits.printCommitInfo();
                

                NewBranch branches = new NewBranch(repository);
                branches.printBranchInfo();
            }
        }


        public static void gitPull(string root)
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                FileName = "git.exe",
                Arguments = "pull",
                WorkingDirectory = root,

            };

            var process = System.Diagnostics.Process.Start(startInfo);
            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            process.WaitForExit();
        }

    }
}