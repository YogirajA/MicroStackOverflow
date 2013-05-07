using MicroStackOverflow.Services.Models;

namespace MicroStackOverflow.Services.SimpleData
{
    public interface ISimpleDataPostsServices
    {
        dynamic Search(SearchPostsBy searchPostsBy);
        void UpdatePost(dynamic post);
        void AddNewPost(dynamic post);
        dynamic GetFewPosts();
        dynamic GetPostById(int id);
        dynamic GetPostByTitle(string title);
    }
}