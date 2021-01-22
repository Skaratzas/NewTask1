using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace Task1._0._1
{
    class NewBranch 
    {
        static string branchName;
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

        public NewBranch(Repository repository, string aBranchName) 
        {
            this.repository = repository;           
            branchName = aBranchName;
        }

        public NewBranch(String aId, DateTime aDateTime, string aMessage, string anAuthor)
        {
            id = aId;
            dateTime = aDateTime;
            message = aMessage;
            author = anAuthor;
        }


        public void printBranchInfo()
        {            
            var branches = repository.Branches;
            Console.WriteLine("\nBranches: ");
            foreach (var branch in branches)
            {
               branchName = branch.FriendlyName;
               Console.WriteLine(branchName);

               var commits = branch.Commits;
               int count = 0;
               foreach(var commit in commits)
                {
                    BranchCommit.Add(commit);
                    count++;
                }
                Console.WriteLine("BRANCH NAME: " + branchName + "\nCOMMIT NUMBER: " + count);

                foreach(var commit in BranchCommit)
                {
                    id = commit.Id.ToString().Substring(0, 7);
                    dateTime = commit.Author.When.LocalDateTime;
                    message = commit.Message;
                    author = commit.Author.Name;

                    Console.WriteLine(id + " " + dateTime + " " + message + " " + author);
                }
                BranchCommit.Clear();
            
                
            }
        }                    
    }
}