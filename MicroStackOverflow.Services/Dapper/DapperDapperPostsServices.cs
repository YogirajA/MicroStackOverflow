using System.Collections.Generic;
using System.Linq;
using Dapper;
using Dapper.DAL.Infrastructure;
using Dapper.DAL.Models;
using MicroStackOverflow.Services.Models;

namespace MicroStackOverflow.Services.Dapper
{
    public class DapperDapperPostsServices : IDapperPostsServices
    {
        private readonly IDatabaseContext _databaseContext;

        public DapperDapperPostsServices(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IEnumerable<Post> GetAllPosts()
        {
            using (var db = _databaseContext)
            {
               return db.Connection.GetList<Post>().ToList();
            }
        }

        public Post GetPost(int id)
        {
            using (var db = _databaseContext)
            {
                return db.Connection.Get<Post>(id);
            }
        }
        public int UpdatePost(Post post)
        {
            int returnVal;
            using (var db = _databaseContext)
            {
                var connection = db.Connection;
                var transaction = connection.BeginTransaction();
                returnVal = db.Connection.Update(post, transaction);
                transaction.Commit();
            }
            return returnVal;
        }
        public int AddNewPost(Post post)
        {
            int returnVal;
            using (var db = _databaseContext)
            {
                var connection = db.Connection;
                var transaction = connection.BeginTransaction();
                returnVal = db.Connection.Insert(post, transaction);
                transaction.Commit();
            }
            return returnVal;
        }

        public IEnumerable<PostSearResults> Search(SearchPostsBy searchPostsBy)
        {
            DynamicParameters param;
            var sql = GetSearchSQL(searchPostsBy,out param);
            using (var db = _databaseContext)
            {

                var posts = db.Connection.Query<PostSearResults>(sql, param);
                return posts.ToList();
            }
        }

        private static string GetSearchSQL(SearchPostsBy searchPostsBy, out DynamicParameters param)
        {
            var postQuery = new PostsQuery();
            param = new DynamicParameters();
            if (!string.IsNullOrEmpty(searchPostsBy.Body))
            {
                //param.AddDynamicParams(new {@body = "%" + SearchPostsBy.Body + "%"});
                param.AddDynamicParams(new {@body = searchPostsBy.Body});
                postQuery = postQuery.ByBody();
            }
            if (!string.IsNullOrEmpty(searchPostsBy.Tags))
            {
               // param.AddDynamicParams(new {@tags = "%" + SearchPostsBy.Tags + "%"});
                param.AddDynamicParams(new {@tags = searchPostsBy.Tags});
                postQuery = postQuery.ByTags();
            }
            

            // ReSharper disable RedundantAnonymousTypePropertyName
            param.AddDynamicParams(new {@startRow = searchPostsBy.StartRowNum});
            // ReSharper restore RedundantAnonymousTypePropertyName
            // ReSharper disable RedundantAnonymousTypePropertyName
            param.AddDynamicParams(new {@endRow = searchPostsBy.EndRowNum});
            // ReSharper restore RedundantAnonymousTypePropertyName
            return postQuery.Bind();
        }

        //insert 

        //update
    }

    public class PostSearResults : Post
    {
        public int Total { get; set; }
    }
}
