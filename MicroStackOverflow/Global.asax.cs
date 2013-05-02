using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using Dapper.DAL.Models;
using MicroStackOverflow.Models;
using MicroStackOverflow.Services.Dapper;
using MicroStackOverflow.Services.Models;
using StackExchange.Profiling;

namespace MicroStackOverflow
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static void ConfigureMapsForAutoMapper()
        {
            Mapper.CreateMap<Post, PostModel>();
            Mapper.CreateMap<PostModel, Post>();
            Mapper.CreateMap<SearchPostsBy, PostsSearchModel>();
           
        }
        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }
        protected void Application_Start()
        {
          
            

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            ConfigureMapsForAutoMapper();
        }
    }
}