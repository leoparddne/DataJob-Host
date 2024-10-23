# DataJob.Host

#### 介绍
定时服务，定时调用配置的接口

#### 软件架构
基于.net5


#### 使用说明

1.  在appsettings.json中配置需要定时调用的接口
2.  接口ip相同的可以维护在同一个配置中公用一个BaseUrl
3.  支持cron表达式，如果维护了cron表达式则优先使用cron表达式，否则使用IntervalSecond配置的定时间隔，默认时间间隔为10秒


#### 配置文件参考
```
{
  "Jobs": [
    //{
    //  //"BaseUrl": "http://192.168.2.49:7000",
    //  "BaseUrl": "http://192.168.2.84:7024",
    //  "Jobs": [
    //    {
    //      "Url": "api/Andon/BaseJob/AutoSendMessageJob",
    //      "IntervalSecond": 10,
    //      "Parameter": "{\"Count\":5}"
    //    },
    //    {
    //      "Url": "api/Andon/BaseJob/CheckExceptionStatusHandleJob",
    //      "IntervalSecond": 60
    //    },
    //    {
    //      "Url": "api/Andon/BaseJob/AutoSendSubscribeMsg",
    //      "IntervalSecond": 10,
    //      "Parameter": "{\"Count\":5}"
    //    }
    //  ]
    //},
    //{
    //  //"BaseUrl": "http://192.168.2.49:7000",
    //  "BaseUrl": "http://192.168.2.84:7024",
    //  "Jobs": [
    //    {
    //      "Url": "api/Andon/BaseJob/AutoSendMessageJob",
    //      "IntervalSecond": 10,
    //      "Cron": "0 * * * * ?"
    //    }
    //  ]
    //},
    //{
    //  "BaseUrl": "http://192.168.2.84:7000",
    //  "Jobs": [
    //    {
    //      "Url": "api/Platform/Sync/SyncUser",
    //      "IntervalSecond": 10,
    //      "Cron": "0 * * * * ?"
    //    },
    //    {
    //      "Url": "api/Platform/Sync/SyncVendor",
    //      "IntervalSecond": 10,
    //      "Cron": "0 * * * * ?"
    //    }
    //  ]
    //},
    {
      //"BaseUrl": "http://127.0.0.1:7013",
      "BaseUrl": "http://192.168.2.171:7014",
      "Jobs": [
        {
          "Url": "api/SRM/Sync/SyncPO",
          "IntervalSecond": 180,
          "TimeOutSecond": 60,
          "Parameter": "{ }"
        }
        //,
        //{
        //  "Url": "api/SRM/Sync/SyncVendorUser",
        //  "IntervalSecond": 180,
        //  "Parameter": "{ }"
        //},
        //{
        //  "Url": "api/Platform/Sync/SyncUser",
        //  "IntervalSecond": 180,
        //  "TimeOutSecond": 120
        //"Parameter": "{ }"
        //}
        //,
        //{
        //  "Url": "api/Platform/Sync/SyncVendor",
        //  "IntervalSecond": 180,
        //  "Parameter": "{ }"
        //}
      ]
    }
  ]
}
```

#### 参与贡献

1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request
