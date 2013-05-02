using System.Collections.Generic;
using Dapper.DAL.Models;
using MicroStackOverflow.Services.Models;

namespace MicroStackOverflow.Services.Dapper
{
    public interface IDapperPostsServices
    {
        IEnumerable<Post> GetAllPosts();
        IEnumerable<PostSearResults> Search(SearchPostsBy searchPostsBy);
        int AddNewPost(Post post);
        Post GetPost(int id);
        int UpdatePost(Post post);
    }
}