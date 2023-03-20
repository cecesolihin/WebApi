using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderApp.Models
{
    public class Paging
    {
        int mxpg = 3;
        public int CountData { get; set; }
        public int StartData { get; set; }
        public int EndData { get; set; }
        public int PositionPage { get; set; }
        public int DataPerPage { get; set; }
        public double CountPage { get; set; }
        public int First { get; set; }
        public int Last { get; set; }
        public int Next { get; set; }
        public int Prev { get; set; }
        public int MaxPage { get; set; }
        public int MinPage { get; set; }
        public Paging(int countdata, int positionpage, int dataperpage)
        {
            EndData = positionpage * dataperpage;
            CountData = countdata;
            PositionPage = positionpage;
            DataPerPage = dataperpage;
            StartData = (positionpage - 1) * dataperpage + 1;
            CountPage = Math.Ceiling((double)countdata / dataperpage);
            First = 1;
            Last = (int)CountPage;
            Next = positionpage < (int)CountPage ? positionpage + 1 : (int)CountPage;
            Prev = positionpage == 1 ? 1 : positionpage - 1;
            EndData = EndData > CountData ? CountData : EndData;
            StartData = CountData > 0 ? StartData : CountData;
            MaxPage = positionpage + mxpg;
            MinPage = positionpage - mxpg;
        }
    }
}