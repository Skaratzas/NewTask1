using LibGit2Sharp;
using System;

namespace Task1._0._1
{
    class Program
    {
        static void Main(string[] args)
        {

            Repository.Clone("https://github.com/Skaratzas/repository", @"C:\GitHub\repository");
            Repository.Init(@"C:\GitHub\git");

            Repository repository = new Repository(@"C:\GitHub\repository");

            NewCommit commits = new NewCommit(repository);
            commits.printCommitInfo();

            NewBranch branches = new NewBranch(repository);





           
        }
    }
}