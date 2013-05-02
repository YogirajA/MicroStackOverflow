using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Dapper.DAL.Models;
using MicroStackOverflow.Models;
using MicroStackOverflow.Services.Dapper;
using MicroStackOverflow.Services.Models;
using PagedList;

namespace MicroStackOverflow.Controllers
{
    public class DapperPostsController : Controller
    {
        private readonly IDapperPostsServices _dapperPostsServices;

        public DapperPostsController(IDapperPostsServices dapperPostsServices)
        {
            _dapperPostsServices = dapperPostsServices;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(int? page, PostsSearchModel postSearchModel)
        {
            PostSearch(page, postSearchModel);
            
            return View(postSearchModel);
        }


        private void PostSearch(int? page, PostsSearchModel searchModel)
        {
            var pageNumber = (page ?? 1);
            const int pageSize = 10;
            var startRowNum = (pageNumber - 1) * pageSize;
            var endRowNum = startRowNum + pageSize;
            var postSearchModel = new SearchPostsBy
                {
                    StartRowNum = startRowNum,
                    EndRowNum = endRowNum,
                    Body = searchModel.Body,
                    PostTypeId = 1,
                    Tags= searchModel.Tags
                };
            
            var results = _dapperPostsServices.Search(postSearchModel).ToList();
            var firstResult = results.Any() ? results.FirstOrDefault() : null;
            if (firstResult != null)
            {
                var total = firstResult.Total;
                var posts = results.Select(GetPostModel).ToList();
                var staticlist = new StaticPagedList<PostModel>(posts, pageNumber, pageSize, total);
                searchModel.Posts = staticlist;
            }
        }

        private PostModel GetPostModel(PostSearResults arg)
        {
            return Mapper.Map<PostModel>(arg);
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(PostModel postModel)
        {
            var post = Mapper.Map<Post>(postModel);
            post.CreationDate = DateTime.Now;

            post.OwnerUserId = 1; // Atwood
            post.OwnerDisplayName = "Jeff Atwood";

            var id = _dapperPostsServices.AddNewPost(post);

            return RedirectToActionPermanent("Edit",new{@id =id});
        }

        public ActionResult Edit(int? id)
        {
            PostModel postModel=null;
            if (id.HasValue)
            {
                var post = _dapperPostsServices.GetPost(id.Value);
                postModel = Mapper.Map<PostModel>(post);
            }
            
            return View(postModel);
        }
        [HttpPost]
        public ActionResult Edit(PostModel postModel)
        {
            if (ModelState.IsValid)
            {
                var post = Mapper.Map<Post>(postModel);
                _dapperPostsServices.UpdatePost(post);
            }
            ViewBag.IsSuccessful = false;
            return View(postModel);
        }
    }
}
