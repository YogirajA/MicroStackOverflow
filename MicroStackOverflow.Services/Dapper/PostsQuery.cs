using System;
using System.Text;

namespace MicroStackOverflow.Services.Dapper
{
    public class PostsQuery
    {

        private readonly StringBuilder _queryBuilder;
        public PostsQuery()
        {
            _queryBuilder = new StringBuilder();
            _queryBuilder.Append(@"With seq as
                                   (SELECT tbl.*,ROW_NUMBER() OVER (ORDER BY ID ASC) rownum
	                                ,Count(*) OVER () Total
 	                                 FROM Posts as tbl
	                                 where 1=1 ");
        }


        public PostsQuery ByPostType()
        {
            _queryBuilder.Append(@" AND PostTypeId = @postTypeId");
            return this;
        }

        public PostsQuery ByBody()
        {
            // _queryBuilder.Append(@"AND Body like  @body ");
            _queryBuilder.Append(@" AND FREETEXT(Body,@body) ");
            return this;
        }

        public PostsQuery ByTags()
        {
            //_queryBuilder.Append(@"AND Tags like  @tags ");
            _queryBuilder.Append(@" AND FREETEXT(Tags,@tags) ");
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