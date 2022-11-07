using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryVHGP.Core.Models
{
    public class PagingRequest
    {
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 8;
        public string keySearch { get; set; } = "";
        public string searchBy { get; set; } = "";
        //public SortOrder SortType { get; set; } = SortOrder.Descending;
        //public string ColName { get; set; } = "Id";
    }
}
