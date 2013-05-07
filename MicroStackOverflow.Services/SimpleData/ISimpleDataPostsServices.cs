using MicroStackOverflow.Services.Models;

namespace MicroStackOverflow.Services.SimpleData
{
    public interface ISimpleDataPostsServices
    {
        dynamic Search(SearchPostsBy searchPostsBy);
        void Update(dynamic post);
        void Insert(dynamic post);
        dynamic GetFewPosts();
        dynamic GetPostById(int id);
        dynamic GetPostByTitle(string title);
    }
}