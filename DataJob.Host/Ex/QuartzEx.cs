using Quartz;

namespace DataJob.Server.Ex
{
    public static class QuartzEx
    {
        public static void addSchedule<T>(this IServiceCollectionQuartzConfigurator quartz, string id, string groupName, string desc, int intervalSecond, string cronStr = null, JobDataMap newJobDataMap = null) where T : IJob
        {
            // 基本Quartz调度器、作业和触发器配置
            JobKey jobKey = new JobKey(id, groupName);
            quartz.AddJob<T>(jobKey, j =>
            {
                j.WithDescription(desc);
                j.SetJobData(newJobDataMap);
                j.StoreDurably(true);
            }
            );

            if (cronStr != null)
            {
                quartz.AddTrigger(t => t
                      .WithIdentity(id)
                      .ForJob(jobKey)
                      .StartNow()
                      .WithCronSchedule(cronStr)//持续工作
                      .WithDescription(desc)
                  );
            }
            else
            {
                quartz.AddTrigger(t => t
                .WithIdentity(id)
                .ForJob(jobKey)
                .StartNow()
                .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromSeconds(intervalSecond))
                .RepeatForever())//持续工作
                .WithDescription(desc)
                );
            }
        }
    }
}