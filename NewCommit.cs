using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Task1._0._1
{
    class NewCommit
    {
        static string id;
        static DateTime dateTime;
        static string message;
        static string name;
        Repository repository;        
        List<string> addedLines = new List<string>();
        List<string> removedLines = new List<string>();

        public NewCommit(string aId, DateTime aDateTime, string aMessage, string aName)
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


        public void printCommitInfo()
        {

            var commits = repository.Commits;
            Console.WriteLine("Commits: ");
            Commit previousCommit = null;
            foreach (var commit in commits)
            {
                if (previousCommit != null)
                {
                    var patch = repository.Diff.Compare<Patch>(commit.Tree, previousCommit.Tree);

                    foreach (var pec in patch)
                    {                     
                        Console.WriteLine("{0} Lines changed: {1} = ({2}+ and {3}-)",
                            pec.Patch,
                            pec.LinesAdded + pec.LinesDeleted,
                            pec.LinesAdded,
                            pec.LinesDeleted);


                        string input = pec.Patch;

                        string addedPattern = @"^\+\s.*$";
                        foreach (Match match in Regex.Matches(input, addedPattern, RegexOptions.Multiline))
                        {
                            addedLines.Add(match.Value);
                        }

                        string removedPattern = @"^\-\s.*$";
                        foreach (Match match in Regex.Matches(input, removedPattern, RegexOptions.Multiline))
                        {
                            removedLines.Add(match.Value);
                        }
                      
                    }                   
                }

                id = commit.Id.ToString().Substring(0, 7);
                dateTime = commit.Author.When.LocalDateTime;
                message = commit.Message;
                name = commit.Author.Name;

                Console.WriteLine("\n" + id + " " + dateTime + " " + message + " " + name);


                previousCommit = commit;
            }         

        }
    }
}
