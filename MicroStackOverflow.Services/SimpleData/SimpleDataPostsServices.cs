using MicroStackOverflow.Services.Models;
using Simple.Data;
using SimpleData.DAL.Infrastructure;

namespace MicroStackOverflow.Services.SimpleData
{
    public class SimpleDataPostsServices
    {
        private readonly IDatabaseContext _databaseContext;

        public SimpleDataPostsServices(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public dynamic Search(SearchPostsBy searchPostsBy)
        {
            //var sql = GenerateSql(searchPostsBy);
            var expression1 = _databaseContext.StackOverflowDb.Posts.PostTypeId == searchPostsBy.PostTypeId;
            var searchExpression = expression1;
            var results = _databaseContext.StackOverflowDb.Posts.All.Where(searchExpression);
            return results;
        }
        public void Update(dynamic post)
        {  
            using (var transaction = _databaseContext.StackOverflowDb.BeginTransaction)
            {
                _databaseContext.StackOverflowDb.Posts.Update(post); 
                transaction.Commit();
            }
        }

        public void Insert(dynamic post)
        {
            using (var transaction = _databaseContext.StackOverflowDb.BeginTransaction)
            {
                _databaseContext.StackOverflowDb.Posts.Insert(post);
                transaction.Commit();
            }
        }

        public dynamic GetFewPosts()
        {
            return _databaseContext.StackOverflowDb.Posts.FindAllById(1.to(10));
        }

        public dynamic GetPostById(int id)
        {
            return _databaseContext.StackOverflowDb.Posts.FindAllById(id);
        }


        public dynamic GetPostByTitle(string title)
        {
            return _databaseContext.StackOverflowDb.Posts.FindAllByTitle(title).FirstOrDefault();
        }
    }
}
