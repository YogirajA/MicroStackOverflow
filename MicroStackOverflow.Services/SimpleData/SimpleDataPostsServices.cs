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
        public dynamic Search(SearchPostsBy searchPostsBy,out int totalRecords)
        {

            dynamic expression1 = _databaseContext.StackOverflowDb.Posts.Id == _databaseContext.StackOverflowDb.Posts.Id;
            dynamic expression2 = _databaseContext.StackOverflowDb.Posts.Id == _databaseContext.StackOverflowDb.Posts.Id;
            dynamic expression3 = _databaseContext.StackOverflowDb.Posts.Id == _databaseContext.StackOverflowDb.Posts.Id;
            const int pageSize = 10;
            if (searchPostsBy.PostTypeId > 0)
            {
                expression1 = _databaseContext.StackOverflowDb.Posts.PostTypeId == searchPostsBy.PostTypeId;
               
            }
            if (!string.IsNullOrEmpty(searchPostsBy.Tags))
            {
                //seems like full text search is not supported
                //http://stackoverflow.com/questions/16416934/fulltext-search-with-simple-data
                //expression2 = _databaseContext.StackOverflowDb.Posts.Tags.Freetext(searchPostsBy.Tags);
                expression2 = _databaseContext.StackOverflowDb.Posts.Tags.Like(searchPostsBy.Tags);
                
            }
            if (!string.IsNullOrEmpty(searchPostsBy.Body))
            {
                //seems like not supported
                //http://stackoverflow.com/questions/16416934/fulltext-search-with-simple-data
                //expression3 = _databaseContext.StackOverflowDb.Posts.Body.Freetext(searchPostsBy.Body);
                expression3 = _databaseContext.StackOverflowDb.Posts.Body.Like(searchPostsBy.Body);
            }

           // dynamic searchExpression = expression1;// && expression2 && expression3;
            var recordsToSkip = searchPostsBy.PageNumberForSimpleData > 0 ? pageSize * (searchPostsBy.PageNumberForSimpleData - 1) : 0;
            Future<int> Total;
            dynamic results = _databaseContext.StackOverflowDb
                            .Posts
                            .All()
                            .Where(expression1 && expression2 && expression3)
                            .OrderById()
                            .WithTotalCount(out Total)
                            .Skip(recordsToSkip).Take(pageSize).ToList();
            totalRecords = Total.HasValue ? Total.Value : 0;                   
            return results;
        }
        public void UpdatePost(dynamic post)
        {  
            using (var transaction = _databaseContext.StackOverflowDb.BeginTransaction())
            {
                _databaseContext.StackOverflowDb.Posts.Update(post); 
                transaction.Commit();
            }
        }

        public int AddNewPost(dynamic post)
        {
            using (var transaction = _databaseContext.StackOverflowDb.BeginTransaction())
            {
                dynamic data =  _databaseContext.StackOverflowDb.Posts.Insert(post);
                transaction.Commit();
                return data.Id;
            }
        }

        public dynamic GetFewPosts()
        {
            return _databaseContext.StackOverflowDb.Posts.FindAllById(1.to(10));
        }

        public dynamic GetPostById(int id)
        {
            return _databaseContext.StackOverflowDb.Posts.Get(id);
        }


        public dynamic GetPostByTitle(string title)
        {
            return _databaseContext.StackOverflowDb.Posts.FindAllByTitle(title).FirstOrDefault();
        }
    }
}
