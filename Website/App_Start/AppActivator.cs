﻿using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DynamicDataEFCodeFirst;
using Elmah;
using Elmah.Contrib.Mvc;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Mvc;
using NuGetGallery;
using NuGetGallery.Infrastructure;
using NuGetGallery.Jobs;
using NuGetGallery.Migrations;
using StackExchange.Profiling;
using StackExchange.Profiling.MVCHelpers;
using WebActivator;
using WebBackgrounder;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AppActivator), "PreStart")]
[assembly: PostApplicationStartMethod(typeof(AppActivator), "PostStart")]
[assembly: ApplicationShutdownMethod(typeof(AppActivator), "Stop")]

namespace NuGetGallery
{
    public static class AppActivator
    {
        private static JobManager _jobManager;
        private static readonly Bootstrapper NinjectBootstrapper = new Bootstrapper();

        public static void PreStart()
        {
            NinjectPreStart();
            MiniProfilerPreStart();
            ElmahPreStart();
        }

        public static void PostStart()
        {
            // Get configuration from the kernel
            var config = Container.Kernel.Get<IConfiguration>();

            MiniProfilerPostStart();
            DbMigratorPostStart();
            BackgroundJobsPostStart(config);
            AppPostStart();
            DynamicDataPostStart(config);
        }

        public static void Stop()
        {
            BackgroundJobsStop();
            NinjectStop();
        }

        private static void ElmahPreStart()
        {
            ServiceCenter.Current = _ => Container.Kernel;
        }

        private static void AppPostStart()
        {
            Routes.RegisterRoutes(RouteTable.Routes);
            GlobalFilters.Filters.Add(new ElmahHandleErrorAttribute());
            GlobalFilters.Filters.Add(new ReadOnlyModeErrorFilter());
            ValueProviderFactories.Factories.Add(new HttpHeaderValueProviderFactory());
        }

        private static void BackgroundJobsPostStart(IConfiguration configuration)
        {
            // readonly: false background jobs and the coordinator should always be able to write to DB just so the job doesn't fail. And we don't care if those updates it writes get lost anyway.
            var jobs = new IJob[]
                {
                    new UpdateStatisticsJob(TimeSpan.FromMinutes(5), 
                        () => new EntitiesContext(configuration.SqlConnectionString, readOnly: false), 
                        timeout: TimeSpan.FromMinutes(5)),
                    new WorkItemCleanupJob(TimeSpan.FromDays(1), () => new EntitiesContext(configuration.SqlConnectionString, readOnly: false), timeout: TimeSpan.FromDays(4)),
                    new LuceneIndexingJob(TimeSpan.FromMinutes(10), () => new EntitiesContext(configuration.SqlConnectionString, readOnly: true), timeout: TimeSpan.FromMinutes(2))
                };
            var jobCoordinator = new WebFarmJobCoordinator(new EntityWorkItemRepository(() => new EntitiesContext(configuration.SqlConnectionString, readOnly: false)));
            _jobManager = new JobManager(jobs, jobCoordinator)
                {
                    RestartSchedulerOnFailure = true
                };
            _jobManager.Fail(e => ErrorLog.GetDefault(null).Log(new Error(e)));
            _jobManager.Start();
        }

        private static void BackgroundJobsStop()
        {
            _jobManager.Dispose();
        }

        private static void DbMigratorPostStart()
        {
            var dbMigrator = new DbMigrator(new MigrationsConfiguration());
            // After upgrading to EF 4.3 and MiniProfile 1.9, there is a bug that causes several 
            // 'Invalid object name 'dbo.__MigrationHistory' to be thrown when the database is first created; 
            // it seems these can safely be ignored, and the database will still be created.
            dbMigrator.Update();
        }

        private static void DynamicDataPostStart(IConfiguration configuration)
        {
            Registration.Register(RouteTable.Routes, configuration);
        }

        private static void MiniProfilerPreStart()
        {
            MiniProfilerEF.Initialize();
            DynamicModuleUtility.RegisterModule(typeof(MiniProfilerStartupModule));
            GlobalFilters.Filters.Add(new ProfilingActionFilter());
        }

        private static void MiniProfilerPostStart()
        {
            var copy = ViewEngines.Engines.ToList();
            ViewEngines.Engines.Clear();
            foreach (var item in copy)
            {
                ViewEngines.Engines.Add(new ProfilingViewEngine(item));
            }
        }

        private static void NinjectPreStart()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestModule));
            DynamicModuleUtility.RegisterModule(typeof(HttpApplicationInitializationModule));
            NinjectBootstrapper.Initialize(() => Container.Kernel);
        }

        private static void NinjectStop()
        {
            NinjectBootstrapper.ShutDown();
        }

        private class MiniProfilerStartupModule : IHttpModule
        {
            public void Init(HttpApplication context)
            {
                context.BeginRequest += (sender, e) => MiniProfiler.Start();

                context.AuthorizeRequest += (sender, e) =>
                                                {
                                                    bool stopProfiling;
                                                    var httpContext = HttpContext.Current;

                                                    if (httpContext == null)
                                                    {
                                                        stopProfiling = true;
                                                    }
                                                    else
                                                    {
                                                        // Temporarily removing until we figure out the hammering of request we saw.
                                                        //var userCanProfile = httpContext.User != null && HttpContext.Current.User.IsInRole(Const.AdminRoleName);
                                                        var requestIsLocal = httpContext.Request.IsLocal;

                                                        //stopProfiling = !userCanProfile && !requestIsLocal
                                                        stopProfiling = !requestIsLocal;
                                                    }

                                                    if (stopProfiling)
                                                    {
                                                        MiniProfiler.Stop(true);
                                                    }
                                                };

                context.EndRequest += (sender, e) => MiniProfiler.Stop();
            }

            public void Dispose()
            {
            }
        }
    }
}
