using LibGit2Sharp;
using System;

namespace Task1._0._0
{
    class Program
    {
        static void Main(string[] args)
        {

            Repository.Clone("https://github.com/Skaratzas/repository", @"C:\GitHub\repository");

            Repository.Init(@"C:\GitHub\git");


            using (var repository = new Repository(@"C:\GitHub\repository")) {
                var commits = repository.Commits;
                foreach (var commit in commits)
                {
                    Console.WriteLine($"{commit.Id.ToString().Substring(0, 6)} " +
                                      $"{commit.Author.When.ToLocalTime()} " +
                                      $"{commit.MessageShort} " +
                                      $"{commit.Author.Name}");
                }

            }
        }
    }
}