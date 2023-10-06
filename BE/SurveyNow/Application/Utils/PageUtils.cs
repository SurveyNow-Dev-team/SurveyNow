using Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Runtime.InteropServices;

namespace Application.Utils
{
    public static class PageUtils
    {

        public static PagingResponse<T> Paginate<T>(this List<T> list, int? page, int? recordsPerPage)
        {
            if (page == null && recordsPerPage == null)
            {
                recordsPerPage = 20;
                page = 1;
            }
            else if (page < 1 || recordsPerPage < 1)
            {
                return null;
            }
            int? numberOfSkipRecord = recordsPerPage * (page - 1);
            var results = list.Skip(numberOfSkipRecord.Value).Take(recordsPerPage.Value).ToList();
            return new PagingResponse<T>
            {
                TotalPages = (int)Math.Ceiling((double)list.Count / recordsPerPage.Value),
                TotalRecords = list.Count,
                CurrentPage = page.Value,
                RecordsPerPage = recordsPerPage.Value,
                Results = results
            };
        }
    }
}
