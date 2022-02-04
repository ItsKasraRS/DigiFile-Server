namespace BackEnd.Core.DTOs
{
    public class BasePaging
    {
        public BasePaging()
        {
            PageId = 1;
            TakeEntity = 9;
        }

        public int PageId { get; set; }

        public int PageCount { get; set; }

        public int ActivePage { get; set; }

        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int TakeEntity { get; set; }
        public int SkipEntity { get; set; }
    }

    public class Pager
    {
        public static BasePaging Build(int pageCount, int pageNumber, int take)
        {
            if (pageNumber <= 1) pageNumber = 1;
            return new BasePaging()
            {
                ActivePage = pageNumber,
                PageCount = pageCount,
                PageId = pageNumber,
                TakeEntity = take,
                SkipEntity = (pageNumber - 1) * take,
                StartPage = pageNumber - 3 <= 0 ? 1 : pageNumber - 3,
                EndPage = pageNumber + 3 > pageCount ? pageCount : pageNumber + 3
            };
        }
    }
}
