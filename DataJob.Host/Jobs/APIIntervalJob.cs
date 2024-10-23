using DataJob.Server.Config;
using DataJob.Server.Helper;
using DataJob.Server.Util;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataJob.Server.Jobs
{
    /// <summary>
    /// 周期调用接口
    /// </summary>
    public class APIIntervalJob : IBaseJob
    {
        public virtual Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.MergedJobDataMap;
            if (dataMap == null)
            {
                return Task.FromResult(0);
            }

            //获取配置
            var config = GetJobConfig(context);
            var url = HttpUtil.BindUrl(config.BaseUrl, config.Job.Url);

            try
            {

                Dictionary<string, object> parameter = new();
                if (!string.IsNullOrEmpty(config.Job.Parameter))
                {
                    parameter = JsonConvert.DeserializeObject<Dictionary<string, object>>(config.Job.Parameter);
                }

                int timeoutSecond = 10;
                if (config.Job.TimeOutSecond > 10)
                {
                    timeoutSecond = config.Job.TimeOutSecond;
                }

                HttpUtil.APIPost<object>(url, parameter, timeoutSecond);

                Console.WriteLine(url);
                LogHelper.Info($"执行job:{url},parameter:{JsonConvert.SerializeObject(parameter)}");
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                LogHelper.Error($"执行job:{url},error:{e.Message}");

            }
            return Console.Out.WriteLineAsync($"{context.JobDetail.Key.Name}job工作了 在{DateTime.Now}");
        }

        private APIIntervalJobConfig GetJobConfig(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.MergedJobDataMap;
            if (dataMap == null)
            {
                return default;
            }


            dataMap.TryGetValue(context.JobDetail.Key.Name, out object jobObj);
            if (jobObj is APIIntervalJobConfig jobConfig)
            {
                return jobConfig;
            }

            return default;
        }

        public async void Start()
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
                //.WithSimpleSchedule(x => x.WithIntervalInSeconds(MessageConfig.messageConfig.IntervalSecond).RepeatForever())
                .Build();

            // 5.使用trigger规划执行任务job
            await sched.ScheduleJob(job, trigger);
        }
    }
}
