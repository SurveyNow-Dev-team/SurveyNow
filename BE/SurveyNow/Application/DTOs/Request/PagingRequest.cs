using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class PagingRequest
    {
        public int Page { get; set; }
        public int RecordsPerPage { get; set; }
    }
}
