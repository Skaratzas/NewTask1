using LibGit2Sharp;
using System;
using System.Collections.Generic;
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


                        string newPattern = @"(?<=^\+)\s.*$";
                        foreach (Match match in Regex.Matches(input, newPattern, RegexOptions.Multiline))
                        {                      
                            addedLinesPerCommit.Add(match.Value);                                                      
                        }
                        
                    }

                    
                    

    /*                string text = System.IO.File.ReadAllText(@"C:\GitHub\repository\Program.cs");  
                    if (text.Contains(Convert.ToString(addedLinesPerCommit)))
                    {
                        foreach (Match match in Regex.Matches(text, klassPattern, RegexOptions.Multiline))
                        {
                            klasse.Add(match.Value);
                        }
                    }
                                                                                                 
    */

                    
                    

                    string[] Files = System.IO.Directory.GetFiles(@"C:\GitHub\repository", "*.cs");
                   
                    foreach (string file in Files)
                    {

                        var checkoutPaths = new[] { file };
                        CheckoutOptions options = new CheckoutOptions { CheckoutModifiers = CheckoutModifiers.Force };
                        repository.CheckoutPaths(previousCommit.Sha, checkoutPaths, options);





                        string[] text = System.IO.File.ReadAllLines(file);

                        foreach (var addedLine in addedLinesPerCommit)
                        {
                            var removedSpecialCharactersString = addedLine.Replace("\n+", "");
                            if (text.Contains(removedSpecialCharactersString))
                            {
                                continue;
                            }
                            else goto flag;
                           
                        }


                        string klassPattern = @"^.*((public|static|private)\s)?(class)\s.+";
                        string textKlass = System.IO.File.ReadAllText(file);

                        var regEx = Regex.Match(textKlass, klassPattern, RegexOptions.Multiline);
                        klasse.Add(Convert.ToString(regEx));                       

                        flag: continue;

                    }

                    Console.WriteLine("\nCommit: <" + message + "> make changes in classes: ");
                    foreach (var klas in klasse)
                    {
                        Console.WriteLine(klas);
                    }
                }

                


                id = commit.Id.ToString().Substring(0, 7);
                dateTime = commit.Author.When.LocalDateTime;
                message = commit.Message;
                name = commit.Author.Name;

                Console.WriteLine("\n" + id + " " + dateTime + " " + message + " " + name);


                addedLinesPerCommit.Clear();
                klasse.Clear();

                previousCommit = commit;

             }
           
        }

       

        public List<string> GetMethodsName(string FileName)
        {
            List<string> methods = new List<string>();
            var MethodLines = System.IO.File.ReadAllLines(FileName).Where((a => (a.Contains("protected") ||
                                            a.Contains("private") ||
                                            a.Contains("public")) &&
                                            !a.Contains("_")&&!a.Contains("class")));

            foreach (var item in MethodLines)
            {
                if (item.IndexOf("(") != -1)
                {
                    string temp = string.Join("", item.Substring(0, item.IndexOf("(")).Reverse());
                    methods.Add(string.Join("", temp.Substring(0, temp.IndexOf(" ")).Reverse()));
                }
            }
            return methods.Distinct().ToList();
        }    


    }
}