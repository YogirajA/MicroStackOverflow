using System.Collections.Generic;
using MicroStackOverflow.Services.Models;
using PetaPoco;
using PetaPoco.DAL.Models;

namespace MicroStackOverflow.Services.Petapoco
{
    public interface IPetaPocoPostsServices
    {
        IEnumerable<Post> GetAllPosts();
        Page<Post> Search(SearchPostsBy searchPostsBy);
        Post AddNewPost(Post post);
        Post GetPost(int id);
        int UpdatePost(Post post);
        Post AddNewPostIfNew(Post post);
    }
}