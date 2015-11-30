using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APISTest
{
    /// <summary>
    /// 系統相關設定初始化
    /// </summary>
    public class AppConfig
    {
        public static ILoginUser LoginUser { get; private set; }

        public static void Init()
        {
            LoginUser = new DefLoginUser();
        }
    }
}