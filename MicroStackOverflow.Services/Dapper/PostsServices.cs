using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.DAL.Infrastructure;
using Dapper.DAL.Models;

namespace MicroStackOverflow.Services.Dapper
{
    public class PostsServices
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

        public PostSearResults Search(PostSearchModel postSearchModel)
        {
            return new PostSearResults();
        }

        //insert 

        //update
    }

    public class PostSearResults
    {
        public IEnumerable<Post> Posts { get; set; }
        public int Total { get; set; }
    }

    public class PostSearchModel
    {
        public int PostTypeId { get; set; }
        public string Body { get; set; }
        public string Tags { get; set; }

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
            _queryBuilder.Append(@"AND Body like  @body ");
            return this;
        }

        public PostsQuery ByTags()
        {
            _queryBuilder.Append(@"AND Tags like  @Tags ");
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
