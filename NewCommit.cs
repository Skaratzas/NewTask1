using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

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
                id = commit.Id.ToString().Substring(0, 7);
                dateTime = commit.Author.When.LocalDateTime;
                message = commit.Message;
                name = commit.Author.Name;

                Console.WriteLine(id + " " + dateTime + " " + message + " " + name);                                  
            }          
        }


        public Task<TreeChanges> Compare(IRepository repository, string sha1, string sha2, bool detectRenames)
        {

            Guard.ArgumentNotNull(repository, nameof(repository)); 
            Guard.ArgumentNotEmptyString(sha1, nameof(sha1)); 
            Guard.ArgumentNotEmptyString(sha2, nameof(sha2)); 

            return Task.Factory.StartNew(() => 
            {
            var options = new CompareOptions { Similarity = detectRenames ? SimilarityOptions.Renames : SimilarityOptions.None };
            var commit1 = repository.Lookup<Commit>(sha1); 
            var commit2 = repository.Lookup<Commit>(sha2);
            if (commit1 != null && commit2 != null)
                {
                    return repository.Diff.Compare<TreeChanges>(commit1.Tree, commit2.Tree, options);
               }
            else
                {
                    return null;
                }

});
        }








    }


}
}