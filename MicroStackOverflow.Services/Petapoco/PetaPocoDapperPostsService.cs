using System.Collections.Generic;
using System.Linq;
using MicroStackOverflow.Services.Models;
using PetaPoco;
using PetaPoco.DAL.Infrastructure;
using PetaPoco.DAL.Models;


namespace MicroStackOverflow.Services.Petapoco
{
    public class PetaPocoDapperPostsService 
    {
         private readonly IDatabaseContext _databaseContext;

         public PetaPocoDapperPostsService(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public IEnumerable<Post> GetAllPosts()
        {
            using (var db = _databaseContext.StackOverflowDB)
            {
                var posts = db.Query<Post>("Select * from Posts");
                return posts.ToList();
            }
        }

        public Page<Post> Search(SearchPostsBy searchPostsBy)
        {
            using (var db = _databaseContext.StackOverflowDB)
            {
                var sql = GenerateSql();
                const int itemsPerPage = 10;
                return db.Page<Post>(searchPostsBy.PageNumber, itemsPerPage, sql);
            }
        }

        private string GenerateSql()
        {
            var sql = Sql.Builder.Append("");
            sql.Append("");
            //sql.InnerJoin()
            sql.Append("");
        }

        public Post AddNewPost(Post post)
        {
            using (var db = _databaseContext.StackOverflowDB)
            using (var transaction = db.GetTransaction())
            {
                var returnObject = db.Insert(post);
                transaction.Complete();
                return returnObject as Post;

            }
        }

        public Post GetPost(int id)
        {
            using (var db = _databaseContext.StackOverflowDB)
            {
                return db.First<Post>("Select * from Post where id= @id",new{@id=id});
            }
        }

        public int UpdatePost(Post post)
        {
             using (var db = _databaseContext.StackOverflowDB)
             using (var transaction = db.GetTransaction())
             {
                 var returnVal = db.Update(post);
                 transaction.Complete();
                 return returnVal;
             }
        }
    }
}
