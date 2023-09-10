using Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

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

        public static IQueryable<T> Filter<T>(this IQueryable<T> source, T filter)
        {
            if (filter != null)
            {
                var properties = filter.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var data = filter.GetType().GetProperty(property.Name)?.GetValue(filter);
                    if (data != null)
                    {
                        Type type = data.GetType();
                        if (type == typeof(string))
                        {
                            // IQueryable: NuGet System.Linq.Dynamic.Core
                            source = source.Where<T>(property.Name + ".ToLower().Contains(@0)", (data as string).ToLower());
                            // IEnumarable
                            //source = source.Where(delegate(T x)
                            //{
                            //    var sourceData = typeof(T).GetProperty(property.Name)?.GetValue(x) as string;
                            //    return sourceData.ToLower().Contains((data as string).ToLower());
                            //}).AsQueryable();
                        } else if (type == typeof(int))
                        {
                            source = source.Where<T>(property.Name + " == @0", data);
                        }
                    }
                }
            }
            return source;
        }
    }
}
