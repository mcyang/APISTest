using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APIS.Helpers
{
    public static class ControlsHelper
    {
        /// <summary>
        /// 輔助方法: 輸出字串-檔案大小
        /// </summary>
        /// <param name="FileSize">藉由HttpPostedFileBase上傳的檔案,其檔案大小的單位為bytes</param>
        /// <returns></returns>
        public static string GetFileSize(int FileSize)
        {
            string Size = "";

            if (FileSize > 0)
            {
                double temp = 0;
                temp = FileSize / 1024.0;

                if (temp > 1024)
                {
                    temp = temp / 1024.0;
                    Size = Math.Round(temp, 2) + " MB";
                }
                else
                {
                    Size = Math.Round(temp, 2) + " KB";
                }
            }

            return Size;
        }
    }
}