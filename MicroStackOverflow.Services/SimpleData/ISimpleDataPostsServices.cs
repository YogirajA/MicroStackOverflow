using MicroStackOverflow.Services.Models;

namespace MicroStackOverflow.Services.SimpleData
{
    public interface ISimpleDataPostsServices
    {
        dynamic Search(SearchPostsBy searchPostsBy, out int totalRecords);
        void UpdatePost(dynamic post);
        int AddNewPost(dynamic post);
        dynamic GetFewPosts();
        dynamic GetPostById(int id);
        dynamic GetPostByTitle(string title);
    }
}