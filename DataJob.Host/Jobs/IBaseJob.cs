using Quartz;

namespace DataJob.Server.Jobs
{
    public interface IBaseJob : IJob
    {
        public void Start();
    }
}
