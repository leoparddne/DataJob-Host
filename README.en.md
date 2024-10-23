# DataJob. Host

####Introduction
Timed service, timed calling of configured interfaces

####Software Architecture
Based on. net5


####Instructions for use

1. Configure the interfaces that need to be called periodically in appsettings. json
2. Interfaces with the same IP address can maintain a common BaseURL in the same configuration
3. Supports cron expressions. If cron expressions are maintained, they will be used first. Otherwise, the timed interval configured by IntervalSecond will be used, with a default time interval of 10 seconds


####Configuration file reference
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


####Participate and contribute

1. Fork's own warehouse
2. Create a new Feat_xxx branch
3. Submit code
4. Create a new Pull Request
