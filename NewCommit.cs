using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Task1._0._1
{
    class NewCommit
    {
        static String id;
        static DateTime dateTime;
        static string message;
        static string name;
        Repository repository;
       

        public NewCommit(String aId, DateTime aDateTime, string aMessage, string aName)
        {
            id = aId;
            dateTime = aDateTime;
            message = aMessage;
            name = aName;
        }

        public NewCommit(Repository repository)
        {
            this.repository = repository;
        }

        
        public void printCommitInfo() { 

            var commits = repository.Commits;
            Console.WriteLine("Commits: ");
            foreach (var commit in commits)
            {
                id = commit.Id.ToString().Substring(0, 6);
                dateTime = commit.Author.When.LocalDateTime;
                message = commit.Message;
                name = commit.Author.Name;

                Console.WriteLine(id + " " + dateTime + " " + message + " " + name);
                                  
            }          
        }
    }
}