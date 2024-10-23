using DataJob.Server.Config;
using DataJob.Server.Ex;
using DataJob.Server.Helper;
using DataJob.Server.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using System.Collections.Generic;

namespace DataJob.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args);

            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                AddQuartzService(services);
            });
            if (System.OperatingSystem.IsWindows())
            {
                hostBuilder.UseWindowsService();
            }
            if (System.OperatingSystem.IsLinux())
            {
                hostBuilder.UseSystemd();
            }

            return hostBuilder;
        }


        private static void AddQuartzService(IServiceCollection services)
        {
            //services.AddTransient<ITestService, TestService>();
            //Start();
            //services.AddHostedService<Worker>();
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                var configs = AppSettingsHelper.GetObject<List<JobConfig>>("Jobs");
                if (configs == null || configs.Count == 0)
                {
                    return;
                }

                foreach (var config in configs)
                {
                    foreach (var job in config.Jobs)
                    {
                        string id = (config.BaseUrl ?? "") + (job.Url ?? "") + GUIDHelper.NewGuid;
                        var obj = new APIIntervalJobConfig
                        {
                            BaseUrl = config.BaseUrl,
                            Job = job
                        };
                        var para = new JobDataMap
                        {
                            { id, obj },
                        };


                        q.addSchedule<APIIntervalJob>(id, nameof(APIIntervalJob), nameof(APIIntervalJob), job.IntervalSecond, job.Cron, para);
                    }
                }
            });

            // ASP.NET核心托管-添加Quartz服务器
            services.AddQuartzServer(options =>
            {
                // 关闭时，我们希望作业正常完成
                options.WaitForJobsToComplete = false;
            });
        }


        /// <summary>
        /// 预留
        /// </summary>
        public static async void Start()
        {
            // 1.创建scheduler的引用
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            IScheduler sched = await schedFact.GetScheduler();

            //2.启动 scheduler
            await sched.Start();

            // 3.创建 job
            IJobDetail job = JobBuilder.Create<APIIntervalJob>()
                    .WithIdentity("job1", "group1")
                    .Build();

            // 4.创建 trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                //.WithSimpleSchedule(x => x.WithIntervalInSeconds(1).RepeatForever())
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever())
                .Build();

            // 5.使用trigger规划执行任务job
            await sched.ScheduleJob(job, trigger);
        }
    }
}
