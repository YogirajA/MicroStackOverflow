using MicroStackOverflow.Services.Models;

namespace MicroStackOverflow.Services.Massive
{
    public interface IMassivePostsServices
    {
        dynamic GetAllPosts();
        dynamic Search(SearchPostsBy searchPostsBy);
        decimal AddNewPost(dynamic post);
        dynamic GetPost(int id);
        void UpdatePost(dynamic post);
    }
}