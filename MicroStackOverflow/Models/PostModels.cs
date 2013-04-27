using System;
using System.ComponentModel;
using Dapper.DAL.Models;
using PagedList;

namespace MicroStackOverflow.Models
{
     public class PostsSearchModel
    {
        public PostsSearchModel()
        {
            Posts = new PagedList<PostModel>(null, 1, 1);
        }
        //public int PostTypeId { get; set; }
        [DisplayName("Search Content")]
        public string Body { get; set; }
        [DisplayName("Search Tags")]
        public string Tags { get; set; }
        //public int StartRowNum { get; set; }
        //public int EndRowNum { get; set; }

        public IPagedList<PostModel> Posts { get; set; }
    }
    public class PostModel
    {
        public int Id { get; set; }
        public int? PostTypeId { get; set; }
        public int? AcceptedAnswerId { get; set; }
        [DisplayName("Creation Date")]
        public DateTime? CreationDate { get; set; }
        [DisplayName("Score")]
        public int? Score { get; set; }
        [DisplayName("View Count")]
        public int? ViewCount { get; set; }
        [DisplayName("Body")]
        public string Body { get; set; }
        [DisplayName("OwnerUserId")]
        public int? OwnerUserId { get; set; }
        [DisplayName("OwerDisplayName")]
        public string OwnerDisplayName { get; set; }
        public int? LastEditorUserId { get; set; }
        public DateTime? LastEditDate { get; set; }
        public DateTime? LastActivityDate { get; set; }
        [DisplayName("Title")]
        public string Title { get; set; }
        [DisplayName("Tags")]
        public string Tags { get; set; }
        public int? AnswerCount { get; set; }
        public int? CommentCount { get; set; }
        public int? FavoriteCount { get; set; }
        public DateTime? ClosedDate { get; set; }
        public int? ParentId { get; set; }
        public DateTime? CommunityOwnedDate { get; set; }
    }
}