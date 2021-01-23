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
                //TODO ... pull
            }

            using (var repository = new Repository(root))
            {
                const string trackedBranchName = "origin/master";
                var branch = repository.Branches[trackedBranchName];

                Branch currentBranch = Commands.Checkout(repository, branch);


                Repository.Init(@"C:\GitHub\git");

                NewCommit commits = new NewCommit(repository);
                commits.printCommitInfo();
                //commits.commitChanges(repository);

                NewBranch branches = new NewBranch(repository);
                branches.printBranchInfo();
            }

        }
    }
}