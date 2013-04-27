using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Dapper.DAL.Infrastructure;
using Dapper.DAL.Models;

namespace MicroStackOverflow.Services.Dapper
{
    public interface IPostsServices
    {
        IEnumerable<Post> GetAllPosts();
        IEnumerable<PostSearResults> Search(PostSearchModel postSearchModel);
    }

    public class PostsServices : IPostsServices
    {
        private readonly IDatabaseContext _databaseContext;

        public PostsServices(IDatabaseContext databaseContext)
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

        public IEnumerable<PostSearResults> Search(PostSearchModel postSearchModel)
        {
            DynamicParameters param;
            var sql = GetSearchSQL(postSearchModel,out param);
            using (var db = _databaseContext)
            {

                var posts = db.Connection.Query<PostSearResults>(sql, param);
                return posts.ToList();
            }
        }

        private static string GetSearchSQL(PostSearchModel postSearchModel, out DynamicParameters param)
        {
            var postQuery = new PostsQuery();
            param = new DynamicParameters();
            if (!string.IsNullOrEmpty(postSearchModel.Body))
            {
                param.AddDynamicParams(new {@body = "%" + postSearchModel.Body + "%"});
                postQuery = postQuery.ByBody();
            }
            if (!string.IsNullOrEmpty(postSearchModel.Tags))
            {
                param.AddDynamicParams(new {@tags = "%" + postSearchModel.Tags + "%"});
                postQuery = postQuery.ByTags();
            }
            

            // ReSharper disable RedundantAnonymousTypePropertyName
            param.AddDynamicParams(new {@startRow = postSearchModel.StartRowNum});
            // ReSharper restore RedundantAnonymousTypePropertyName
            // ReSharper disable RedundantAnonymousTypePropertyName
            param.AddDynamicParams(new {@endRow = postSearchModel.EndRowNum});
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

    public class PostSearchModel
    {
        public int PostTypeId { get; set; }
        public string Body { get; set; }
        public string Tags { get; set; }
        public int StartRowNum { get; set; }
        public int EndRowNum { get; set; }
    }

    public class PostsQuery
    {

        private readonly StringBuilder _queryBuilder;
        public PostsQuery()
        {
            _queryBuilder = new StringBuilder();
            _queryBuilder.Append(
                                  @"With seq as
                                   (
                                    SELECT tbl.*,ROW_NUMBER() OVER (ORDER BY ID ASC) rownum
	                                ,Count(*) OVER () Total
 	                                 FROM Posts as tbl
	                                 where 1=1 ");
        }


        public PostsQuery ByPostType()
        {
            _queryBuilder.Append(@"AND PostTypeId = @postTypeId");
            return this;
        }

        public PostsQuery ByBody()
        {
           // _queryBuilder.Append(@"AND Body like  @body ");
            _queryBuilder.Append(@"AND CONTAINS(Body,@body) ");
            return this;
        }

        public PostsQuery ByTags()
        {
            //_queryBuilder.Append(@"AND Tags like  @tags ");
            _queryBuilder.Append(@"AND CONTAINS(Tags,@tags) ");
            return this;
        }

        public string Bind()
        {
            _queryBuilder.Append(@")");
            _queryBuilder.Append(Environment.NewLine);
            _queryBuilder.Append(@"Select * from seq 
                                 WHERE seq.rownum BETWEEN @startRow AND @endRow
                                 ORDER BY seq.rownum");
            return _queryBuilder.ToString();
        }


    }
}
