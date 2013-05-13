using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Massive.DAL;
using Massive.DAL.Models;
using MicroStackOverflow.Services.Models;

namespace MicroStackOverflow.Services.Massive
{
        //IEnumerable<Post> GetAllPosts();
        //IEnumerable<PostSearResults> Search(SearchPostsBy searchPostsBy);
        //int AddNewPost(Post post);
        //Post GetPost(int id);
        //int UpdatePost(Post post);
    public interface IMassivePostsServices
    {
        dynamic GetAllPosts();
        dynamic Search(SearchPostsBy searchPostsBy);
        decimal AddNewPost(dynamic post);
        dynamic GetPost(int id);
        void UpdatePost(dynamic post);
    }

    public class MassivePostsServices : IMassivePostsServices
    {
        private readonly string _connectionStringName;

        public MassivePostsServices(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
            DynamicModel.Open(_connectionStringName);
     
        }

        public dynamic GetAllPosts()
        {
            
            dynamic posts = new Posts(_connectionStringName);

            return posts.All();
        }

        public dynamic Search(SearchPostsBy searchPostsBy)
        {
            
            dynamic posts = new Posts(_connectionStringName);

            var generatedSql = GenerateSql(searchPostsBy);
            var objects = GetArgs(searchPostsBy);
            var result = posts.Paged( where: generatedSql,currentPage: searchPostsBy.PageNumberForSimpleData, pageSize: 10,args: objects);
           
            return result;
        }

        private static string[] GetArgs(SearchPostsBy searchPostsBy)
        {
            var args = new List<string>();
           
            if (searchPostsBy.PostTypeId > 0)
                args.Add(searchPostsBy.PostTypeId.ToString());//, new { @postTypeId = searchPostsBy.PostTypeId });

            if (!string.IsNullOrEmpty(searchPostsBy.Body))
                args.Add(searchPostsBy.Body);

            if (!string.IsNullOrEmpty(searchPostsBy.Tags))
                args.Add(searchPostsBy.Tags);

            return args.ToArray();
            //return string.Join(",", args.ToArray());
        }
        private static string GenerateSql(SearchPostsBy searchPostsBy)
        {

            var sql = new StringBuilder();
            //sql.Append(@" Where 1=1");
//            sql.Append(@"SELECT tbl.* 
// 	                    FROM Posts as tbl
//	                    where 1=1 ");
            if (searchPostsBy.PostTypeId > 0)
                sql.Append(@" PostTypeId = @0");//, new { @postTypeId = searchPostsBy.PostTypeId });

            if (!string.IsNullOrEmpty(searchPostsBy.Body))
                sql.Append(@" AND FREETEXT(Body,@1)");//, new { @body = searchPostsBy.Body });

            if (!string.IsNullOrEmpty(searchPostsBy.Tags))
                sql.Append(@" AND FREETEXT(Tags,@2)"); //, new { @tags = searchPostsBy.Tags });

            return sql.ToString();

        }
        public decimal AddNewPost(dynamic post)
        {
            dynamic posts = new Posts(_connectionStringName);
       
            dynamic returnObject = posts.Insert(post);

            if(returnObject.ID is decimal)
                return returnObject.ID;

            return 0.0M;

        }
        public dynamic GetPost(int id)
        {
            
            dynamic posts = new Posts(_connectionStringName);

            dynamic post = posts.First(Id: id);

            return post;
        }
        public void UpdatePost(dynamic post)
        {
            
            dynamic posts = new Posts(_connectionStringName);

            posts.Update(post,post.Id);
        }
 
    }
}
