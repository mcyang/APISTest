using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIS.Models.MvcPaging
{
    /// <summary></summary>
    public interface IPagedList
    {
        /// <summary></summary>
        int PageCount { get; }
        /// <summary></summary>
        int TotalItemCount { get; }
        /// <summary></summary>
        int PageIndex { get; }
        /// <summary></summary>
        int PageNumber { get; }
        /// <summary></summary>
        int PageSize { get; }
        /// <summary></summary>
        bool HasPreviousPage { get; }
        /// <summary></summary>
        bool HasNextPage { get; }
        /// <summary></summary>
        bool IsFirstPage { get; }
        /// <summary></summary>
        bool IsLastPage { get; }
        /// <summary></summary>
        int CurrentPageItemCount { get; }
        /// <summary></summary>
        int CurrentPageStartIndex { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPagedList<T> : IPagedList, IList<T>, IEnumerable<T>
    {
    }
}