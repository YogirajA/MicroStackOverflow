using MicroStackOverflow.Services.Models;
using Simple.Data;
using SimpleData.DAL.Infrastructure;

namespace MicroStackOverflow.Services.SimpleData
{
    public class SimpleDataPostsServices : ISimpleDataPostsServices
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
            const int pageSize = 10;
            var recordsToSkip = searchPostsBy.PageNumber > 0 ? pageSize*(searchPostsBy.PageNumber-1) :0;
            if (searchPostsBy.PostTypeId > 0)
            {
                expression1 = _databaseContext.StackOverflowDb.Posts.PostTypeId == searchPostsBy.PostTypeId;
               
            }
            if (!string.IsNullOrEmpty(searchPostsBy.Tags))
            {
                expression2 = _databaseContext.StackOverflowDb.Posts.Tags.Like(searchPostsBy.Tags);
                
            }
            if (!string.IsNullOrEmpty(searchPostsBy.Body))
            {
                expression3 = _databaseContext.StackOverflowDb.Posts.Body.Like(searchPostsBy.Body);
            }

            //dynamic searchExpression = expression1;// && expression2 && expression3;
            dynamic results = _databaseContext.StackOverflowDb
                                          .Posts.All.Where(expression1).ToList();
                           //.OrderByCreateDate()
                           //.Skip(recordsToSkip).Take(pageSize);
            return results;
        }
        public void UpdatePost(dynamic post)
        {  
            using (var transaction = _databaseContext.StackOverflowDb.BeginTransaction)
            {
                _databaseContext.StackOverflowDb.Posts.Update(post); 
                transaction.Commit();
            }
        }

        public void AddNewPost(dynamic post)
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
