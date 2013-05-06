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
           
            dynamic expression1 = true;
            dynamic expression2 = true;
            dynamic expression3 = true;
            if (searchPostsBy.PostTypeId > 0)
            {
                expression1 = _databaseContext.StackOverflowDb.Posts.PostTypeId == searchPostsBy.PostTypeId;
               
            }
            if (string.IsNullOrEmpty(searchPostsBy.Tags))
            {
                expression2 = _databaseContext.StackOverflowDb.Posts.Tags == searchPostsBy.Tags;
                
            }
            if (string.IsNullOrEmpty(searchPostsBy.Body))
            {
                expression3 = _databaseContext.StackOverflowDb.Posts.Body == searchPostsBy.Body;
            }

            dynamic searchExpression = expression1 && expression2 && expression3;
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
