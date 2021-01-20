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
                Repository.Clone("https://github.com/Skaratzas/NewTask1", @"C:\GitHub\repository");
                Repository.Init(@"C:\GitHub\git");
            }
           

            Repository repository = new Repository(@"C:\GitHub\repository");

            NewCommit commits = new NewCommit(repository);
            commits.printCommitInfo();

            NewBranch branches = new NewBranch(repository);
            branches.printBranchInfo();
          
        }
    }
}