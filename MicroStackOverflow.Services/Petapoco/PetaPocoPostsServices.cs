using System.Collections.Generic;
using MicroStackOverflow.Services.Models;
using PetaPoco;
using PetaPoco.DAL.Infrastructure;
using PetaPoco.DAL.Models;


namespace MicroStackOverflow.Services.Petapoco
{
    public class PetaPocoPostsServices : IPetaPocoPostsServices
    {
        private readonly IDatabaseContext _databaseContext;

        public PetaPocoPostsServices(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public IEnumerable<Post> GetAllPosts()
        {
            using (var db = _databaseContext.StackOverflowDB)
            {
                var posts = db.Query<Post>("Select * from Posts"); //queyr uses yield return .. fetch gets everything
                foreach (var post in posts)
                {
                    yield return post;
                }
            }
        }

        public Page<Post> Search(SearchPostsBy searchPostsBy)
        {
            
            var sql = GenerateSql(searchPostsBy);
            const int itemsPerPage = 10;
            using (var db = _databaseContext.StackOverflowDB)
            {   
                return db.Page<Post>(searchPostsBy.PageNumberForPetaPoco, itemsPerPage, sql);
            }
        }

        private static Sql GenerateSql(SearchPostsBy searchPostsBy)
        {
            var sql = Sql.Builder.Append(@" 
                                     SELECT tbl.*,ROW_NUMBER() OVER (ORDER BY ID ASC) rownum
	                                ,Count(*) OVER () Total
 	                                 FROM Posts as tbl
	                                 where 1=1 ");
            if(searchPostsBy.PostTypeId > 0)
                sql.Append(@"AND PostTypeId = @postTypeId",new {@postTypeId = searchPostsBy.PostTypeId});

            if (!string.IsNullOrEmpty(searchPostsBy.Body))
                sql.Append(@"AND FREETEXT(Body,@body)",new{@body = searchPostsBy.Body});

            if (!string.IsNullOrEmpty(searchPostsBy.Tags))
                sql.Append(@"AND FREETEXT(Tags,@tags)",new{@tags = searchPostsBy.Tags});
            
            return sql;

        }

        public Post AddNewPost(Post post)
        {
            using (var db = _databaseContext.StackOverflowDB)
            using (var transaction = db.GetTransaction())
            {
                if (db.IsNew(post))   // helps to avoid multiple inserts
                {
                    var returnObject = db.Insert(post);
                    transaction.Complete();
                    return returnObject as Post;
                }
                return null;

            }
        }

        public Post GetPost(int id)
        {
            using (var db = _databaseContext.StackOverflowDB)
            {
                //return db.First<Post>("Select * from Post where id= @id",new{@id=id});
                return db.SingleOrDefault<Post>("WHERE id=@0", id);
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
