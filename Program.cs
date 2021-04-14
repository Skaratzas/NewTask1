using LibGit2Sharp;
using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

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
                gitPull(root);
            }

        
            using (var repository = new Repository(root))
            {

                ChooseBranch(repository);                            

                Repository.Init(@"C:\GitHub\git");

                NewCommit commits = new NewCommit(repository);
                commits.printCommitInfo();          

                NewBranch branches = new NewBranch(repository);
                branches.printBranchInfo();

                Serialize(commits, branches);                
            }                    
        }



        static void gitPull(string root)
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                FileName = "git.exe",
                Arguments = "pull",
                WorkingDirectory = root,
            };

            var process = System.Diagnostics.Process.Start(startInfo);
            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            process.WaitForExit();
        }

        static void ChooseBranch(Repository repository)
        {
            Branch branch = null;
            do
            {
                try
                {
                    Console.WriteLine("Choose a branch: ");
                    String chosenBranch = Console.ReadLine();

                    branch = repository.Branches[chosenBranch];
                    Branch currentBranch = Commands.Checkout(repository, branch);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Please try again \n");
                }
            } while (branch == null);
        }

        static void Serialize(NewCommit commits, NewBranch branches)
        {
            XmlSerializer serializerCommit = new XmlSerializer(typeof(NewCommit));
            XmlSerializer serializerBranch = new XmlSerializer(typeof(NewBranch));

            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var relativePath = Path.Combine(outPutDirectory, "data.xml");
            string relative_path = new Uri(relativePath).LocalPath;

            using (TextWriter writer = new StreamWriter(relative_path))
            {
                serializerCommit.Serialize(writer, commits);
                serializerBranch.Serialize(writer, branches);
            }
        }

    }
}