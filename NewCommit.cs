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
        public List<string> addedLines = new List<string>();
        public List<string> removedLines = new List<string>();
        List<string> klasse = new List<string>();
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
            Console.WriteLine("Commits: ");
            Commit previousCommit = null;

            foreach (var commit in commits)
            {
                
                if (previousCommit != null)
                {
                    var patch = repository.Diff.Compare<Patch>(commit.Tree, previousCommit.Tree);
                    var patch2 = patch.Content;
                                      

                    foreach (var pec in patch)
                    {                    
                                             
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


                        string newPattern = @"(?<=^\+)\s.*$";
                        foreach (Match match in Regex.Matches(input, newPattern, RegexOptions.Multiline))
                        {                      
                            addedLinesPerCommit.Add(match.Value);                                                      
                        }
                        
                    }

                    
                    flag2: ;
                    string[] Files = System.IO.Directory.GetFiles(@"C:\GitHub\repository", "*.cs");
                   
                    foreach (string file in Files)
                    {
                        var checkoutPaths = new[] { file };
                        CheckoutOptions options = new CheckoutOptions { CheckoutModifiers = CheckoutModifiers.Force };   
                        repository.CheckoutPaths(previousCommit.Sha, checkoutPaths, options);
                                           

                        if(!File.Exists(file))
                        {
                            goto flag2;
                        }


                        string[] text = System.IO.File.ReadAllLines(Convert.ToString(file));

                        foreach (var addedLine in addedLinesPerCommit)
                        {     
                            
                            string removedSpecialCharactersString = addedLine;
                            string[] specialCharacters = { "\n ", "\n+" };

                            foreach(string sc in specialCharacters)
                            {
                                removedSpecialCharactersString = removedSpecialCharactersString.Replace(sc, null);
                            }
                            
                            if (text.Contains(removedSpecialCharactersString))
                            {
                                continue;
                            }
                            else goto flag;
                           
                        }


                        string input = System.IO.File.ReadAllText(file);

                        string klassPattern = @"\b((public|static|private)\s)?(class)\s.+";
                        
                        foreach(Match match in Regex.Matches(input, klassPattern, RegexOptions.Multiline))
                        {
                            klasse.Add(match.Value);
                        }
                        

                        string methodPattern = @"\b(public|private|protected|static)\s*" + @"\b(static|virtual|abstract|void)\s*[a-zA-Z]*\s[a-zA-Z]+\s*";
                        foreach(Match match in Regex.Matches(input, methodPattern, RegexOptions.Multiline))
                         {
                             methods.Add(match.Value);
                         }
                       

                        flag: continue;

                    }

                    Console.WriteLine("\nCommit: <" + message + "> make changes in classes: ");
                    foreach (var aKlasse in klasse)
                    {
                        Console.WriteLine(aKlasse);
                    }

                    Console.WriteLine(" and in methods: ");
                    foreach(var method in methods)
                    {
                        Console.WriteLine(method);
                    }
                }
                

                id = commit.Id.ToString().Substring(0, 7);
                dateTime = commit.Author.When.LocalDateTime;
                message = commit.Message;
                name = commit.Author.Name;


                addedLinesPerCommit.Clear();
                klasse.Clear();
                methods.Clear();

                previousCommit = commit;             

             }
            
            repository.Reset(ResetMode.Hard, repository.Head.Tip);
         
        }        
    }   
}