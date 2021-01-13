using LibGit2Sharp;
using System;

namespace Task1._0._1
{
    class NewBranch 
    {
        static string name;
        Repository repository;        

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

            }
        }
    }
}