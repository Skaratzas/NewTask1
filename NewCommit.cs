using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Task1._0._1
{
    public class NewCommit
    {
        public string id { get; set; }
        public DateTime dateTime { get; set; }
        public string message { get; set; }
        public string name { get; set; }
        Repository repository { get; set; }
        List<string> classes = new List<string>();
        List<string> addedLinesPerCommit = new List<string>();
        List<string> methods = new List<string>();
        
        
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

        public NewCommit() : this (null) { }


        public void printCommitInfo()
        {
            
            var commits = repository.Commits;
            
            Commit previousCommit = null;

            foreach (var commit in commits)
            {
               
                if (previousCommit != null)
                {
                    var patch = repository.Diff.Compare<Patch>(commit.Tree, previousCommit.Tree);                                     

                    foreach (var pec in patch)
                    {
                        
                        string input = pec.Patch;                                                                                        
                                           
                        string newPattern = @"(?<=^\+)\s.*$";
                        foreach (Match match in Regex.Matches(input, newPattern, RegexOptions.Multiline))
                        {                      
                            addedLinesPerCommit.Add(match.Value);                                                      
                        }

                        if (addedLinesPerCommit.Count!=0)
                        {

                            string[] Files = System.IO.Directory.GetFiles(@"C:\GitHub\repository", "*.cs");

                            foreach (string file in Files)
                            {
                                // checks in which folders each commit makes changes to
                                check(file, previousCommit);     
                                
                            }

                        }
                        addedLinesPerCommit.Clear();

                    }
                    printClassAndMethods(classes, methods);
                    
                }
                
                id = commit.Id.ToString().Substring(0, 7);
                dateTime = commit.Author.When.LocalDateTime;
                message = commit.Message;
                name = commit.Author.Name;

              
                classes.Clear();
                methods.Clear();

                previousCommit = commit;             

             }
            
            repository.Reset(ResetMode.Hard, repository.Head.Tip);         
        }
        


        private void check(string file, Commit previousCommit)
        {
            var checkoutPaths = new[] { file };
            CheckoutOptions options = new CheckoutOptions { CheckoutModifiers = CheckoutModifiers.Force };
            repository.CheckoutPaths(previousCommit.Sha, checkoutPaths, options);

            if (!File.Exists(file))
            {
                return;
            }

            string[] text = System.IO.File.ReadAllLines(Convert.ToString(file));

            foreach (var addedLine in addedLinesPerCommit)
            {

                string removedSpecialCharactersString = addedLine;
                string[] specialCharacters = { "\n ", "\n+" };

                foreach (string sc in specialCharacters)
                {
                    removedSpecialCharactersString = removedSpecialCharactersString.Replace(sc, null);
                }

                if (text.Contains(removedSpecialCharactersString))
                {
                    continue;
                }
                else return;

            }

            string input = System.IO.File.ReadAllText(file);

            CalculateClassAndMethods(input);
        }

        private void CalculateClassAndMethods(string input)
        {
            string klassPattern = @"\b((public|static|private)\s)?(class)\s.+";

            foreach (Match match in Regex.Matches(input, klassPattern, RegexOptions.Multiline))
            {
                classes.Add(match.Value);
            }


            string methodPattern = @"\b(public|private|protected|static)\s*" + @"\b(static|virtual|abstract|void)\s*[a-zA-Z]*\s[a-zA-Z]+\s*";
            foreach (Match match in Regex.Matches(input, methodPattern, RegexOptions.Multiline))
            {
                methods.Add(match.Value);
            }
        }

        private void printClassAndMethods(List<string> classes, List<string> methods)
        {
            Console.WriteLine("\nCommit: " + message + ", make changes in classes: ");
            foreach (var aClass in classes)
            {
                Console.WriteLine(aClass);
            }

            Console.WriteLine(" and in methods: ");
            foreach (var method in methods)
            {
                Console.WriteLine(method);
            }
        }
    }   
}