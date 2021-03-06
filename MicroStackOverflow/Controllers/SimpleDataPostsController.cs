﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MicroStackOverflow.Helpers;
using MicroStackOverflow.Models;
using MicroStackOverflow.Services.Models;
using MicroStackOverflow.Services.SimpleData;
using PagedList;

namespace MicroStackOverflow.Controllers
{
    public class SimpleDataPostsController : Controller
    {
        private readonly ISimpleDataPostsServices _simpleDataPostsServices;

        public SimpleDataPostsController(ISimpleDataPostsServices simpleDataPostsServices)
        {
            _simpleDataPostsServices = simpleDataPostsServices;
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
          
            var postSearchModel = new SearchPostsBy
            {
               
                Body = searchModel.Body,
                PostTypeId = 1,
                Tags = searchModel.Tags
                ,PageNumberForSimpleData = pageNumber
            };

            //dynamic results = _simpleDataPostsServices.GetFewPosts();
            int total;
            dynamic results = _simpleDataPostsServices.Search(postSearchModel,out total);
            
            var posts = new List<PostModel>();
            
            foreach (dynamic result in results)
            {
                posts.Add(GetPostModel(result));
            }
            if (posts.Any())
            {
                
                var staticlist = new StaticPagedList<PostModel>(posts, pageNumber, pageSize, total);
                searchModel.Posts = staticlist;
            }
        }

        private PostModel GetPostModel(dynamic arg)
        {
            return TypeConverter.FromDynamicToStatic<PostModel>(arg);
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(PostModel postModel)
        {
            dynamic post = postModel;// needs more
            
            post.CreationDate = DateTime.Now;
            post.OwnerUserId = 1; // Atwood
            post.OwnerDisplayName = "Jeff Atwood";

            var id =  _simpleDataPostsServices.AddNewPost(post);
            return RedirectToActionPermanent("Edit", new {id });
        }

        public ActionResult Edit(int? id)
        {
            PostModel postModel = null;
            if (id.HasValue)
            {
                dynamic post = _simpleDataPostsServices.GetPostById(id.Value);
                postModel = TypeConverter.FromDynamicToStatic<PostModel>(post);
            }

            return View(postModel);
        }
        [HttpPost]
        public ActionResult Edit(PostModel postModel)
        {
            if (ModelState.IsValid)
            {
                dynamic post = postModel;
                _simpleDataPostsServices.UpdatePost(post);
                ViewBag.IsSuccessful = true;
            }
            
            return View(postModel);
        }
    }
}
