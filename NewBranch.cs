using LibGit2Sharp;
using System;
using System.Collections.Generic;

namespace Task1._0._1
{
    class NewBranch 
    {
        static string name;
        Repository repository;
        List<Commit> BranchCommit = new List<Commit>();

        static String id;
        static DateTime dateTime;
        static string message;
        static string author;
  

        public NewBranch(Repository repository)
        {
            this.repository = repository;
        }

        public NewBranch(Repository repository, string aName) 
        {
            this.repository = repository;           
            name = aName;
        }
   

        public void printBranchInfo()
        {            
            var branches = repository.Branches;
            Console.WriteLine("\nBranches: ");
            foreach (var branch in branches)
            {
                name = branch.FriendlyName;
                Console.WriteLine(name);
               var commits = branch.Commits;
               foreach(var commit in commits)
                {
                    BranchCommit.Add(commit);                                                     
                }
                foreach(var commit in BranchCommit)
                {
                    id = commit.Id.ToString().Substring(0, 6);
                    dateTime = commit.Author.When.LocalDateTime;
                    message = commit.Message;
                    author = commit.Author.Name;

                    Console.WriteLine(id + " " + dateTime + " " + message + " " + name);
                }
            



            }
        }

    }
}