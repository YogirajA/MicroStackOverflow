using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MicroStackOverflow.Models;
using MicroStackOverflow.Services.Models;
using MicroStackOverflow.Services.Petapoco;
using PagedList;
using PetaPoco.DAL.Models;

namespace MicroStackOverflow.Controllers
{
    public class PetaPocoPostsController : Controller
    {
        private readonly IPetaPocoPostsServices _petaPocoPostsServices;
        //
        // GET: /PetaPocoPosts/

        public PetaPocoPostsController(IPetaPocoPostsServices petaPocoPostsServices)
        {
            _petaPocoPostsServices = petaPocoPostsServices;
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
                Tags = searchModel.Tags,
                PageNumber = pageNumber
            };

            var result = _petaPocoPostsServices.Search(postSearchModel);
            var total = result.TotalItems;
            var posts = result.Items.Select(GetPostModel).ToList();
            var staticlist = new StaticPagedList<PostModel>(posts, pageNumber, pageSize, (int) total);// hopefully the number does not get too big
            searchModel.Posts = staticlist;
            
        }

        private PostModel GetPostModel(Post post)
        {
            return Mapper.Map<PostModel>(post);
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

             _petaPocoPostsServices.AddNewPost(post);

            return RedirectToActionPermanent("Edit", new { @id = post.Id });
        }

        public ActionResult Edit(int? id)
        {
            PostModel postModel = null;
            if (id.HasValue)
            {
                var post = _petaPocoPostsServices.GetPost(id.Value);
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
                _petaPocoPostsServices.UpdatePost(post);
            }
            ViewBag.IsSuccessful = false;
            return View(postModel);
        }


        public ActionResult AddIfNew()
        {
            return View();
        }

        public ActionResult FetchVsQuery()
        {
            return View();
        }

        public ActionResult Query()
        {
            return View();
        }
    }
}
