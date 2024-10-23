namespace DataJob.Server.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class JobConfigItem
    {
        public string Url { get; set; }

        /// <summary>
        /// 脚本定时周期 - 以秒为单位
        /// </summary>
        public int IntervalSecond { get; set; }

        /// <summary>
        /// 接口超时时间(秒)
        /// </summary>
        public int TimeOutSecond { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// 参数 - 参数需要转义
        /// </summary>
        public string Parameter { get; set; }
    }
}
