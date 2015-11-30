using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APIS.Models.MvcPaging;

namespace APIS.ViewModels
{
    public class fvmRDRManage
    {
        public readonly IPagedList<RDRMainIndexViewModel> PagedList;

        public fvmRDRManage(IPagedList<RDRMainIndexViewModel> list)
        {
            this.PagedList = list;
        }

        /// <summary>
        /// 搜尋及排序值
        /// </summary>
        public readonly SearchModel RouteValue = new SearchModel();

        [Serializable]
        public class SearchModel : SortModel
        {
            public string SearchBytitleCh { get; set; }
            public string SearchBytitleEng { get; set; }
            public string SearchByteacherName { get; set; }
            public string SearchBySkill { get; set; }

            public void SetSearch(SearchModel searchRouter)
            {
                base.SetSearch(searchRouter);
                this.SearchBytitleCh = searchRouter.SearchBytitleCh;
                this.SearchBytitleEng = searchRouter.SearchBytitleEng;
                this.SearchByteacherName = searchRouter.SearchByteacherName;
                this.SearchBySkill = searchRouter.SearchBySkill;
            }
        }
    }
}