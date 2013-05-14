using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MicroStackOverflow.Helpers;
using MicroStackOverflow.Models;
using MicroStackOverflow.Services.Massive;
using MicroStackOverflow.Services.Models;
using PagedList;

namespace MicroStackOverflow.Controllers
{
    public class MassivePostsController : Controller
    {
        private readonly IMassivePostsServices _massivePostsServices;
        //
        // GET: /MassivePosts/

        public MassivePostsController(IMassivePostsServices massivePostsServices)
        {
            _massivePostsServices = massivePostsServices;
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
            dynamic results = _massivePostsServices.Search(postSearchModel);
            int total = results.TotalRecords;
            var posts = new List<PostModel>();

            foreach (dynamic result in results.Items)
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
            postModel.CreationDate = DateTime.Now;
            postModel.OwnerUserId = 1; // Atwood
            postModel.OwnerDisplayName = "Jeff Atwood";
            var postToInsert = (IDictionary<string, object>) TypeConverter.FromStaticToDynamic(postModel) ;
            
            if (postToInsert != null)
                postToInsert.Remove("Id");

            var id = _massivePostsServices.AddNewPost(postToInsert);
            return RedirectToActionPermanent("Edit", new { id });
        }

        public ActionResult Edit(int? id)
        {
            PostModel postModel = null;
            if (id.HasValue)
            {
                dynamic post = _massivePostsServices.GetPost(id.Value);
                postModel = TypeConverter.FromDynamicToStatic<PostModel>(post);
            }

            return View(postModel);
        }
        [HttpPost]
        public ActionResult Edit(PostModel postModel)
        {
            if (ModelState.IsValid)
            {
                dynamic post = TypeConverter.FromStaticToDynamic(postModel) ;
                _massivePostsServices.UpdatePost(post);
                ViewBag.IsSuccessful = true;
            }
            return View(postModel);
        }

    }
}
