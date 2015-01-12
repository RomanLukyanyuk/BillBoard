using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillBoard.Models
{
    public class AdvertsListViewModel
    {
        public IEnumerable<Advert> Adverts { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}