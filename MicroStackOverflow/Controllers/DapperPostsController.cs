using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Dapper.DAL.Models;
using MicroStackOverflow.Models;
using MicroStackOverflow.Services.Dapper;
using PagedList;

namespace MicroStackOverflow.Controllers
{
    public class DapperPostsController : Controller
    {
        private readonly IPostsServices _postsServices;

        public DapperPostsController(IPostsServices postsServices)
        {
            _postsServices = postsServices;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(int? page, Models.PostsSearchModel postSearchModel)
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
            var postSearchModel = new PostSearchModel
                {
                    StartRowNum = startRowNum,
                    EndRowNum = endRowNum,
                    Body = searchModel.Body,
                    PostTypeId = 1,
                    Tags= searchModel.Tags
                };
            
            var results = _postsServices.Search(postSearchModel).ToList();
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

        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}
