namespace IntegratedBlazorProject.Client.Shared
{
    public class Pagination
    {
        public int Page { get; private set; }
        public int PageCount { get; private set; }
        public int TotalCount { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }

        public Pagination(int totalCount)
        {
            this.Take = 10;
            this.Skip = 0;
            this.Page = 0;
            this.TotalCount = totalCount;
            this.PageCount = ((int)Math.Ceiling((double)totalCount / this.Take));
        }

        public Pagination(int totalCount, int take)
        {
            if (take <= 0)
            {
                this.Take = 10;
            }
            else
            {
                this.Take = take;
            }

            this.Skip = 0;
            this.Page = 0;
            this.TotalCount = totalCount;
            this.PageCount = ((int)Math.Ceiling((double)totalCount / this.Take));
        }

        public Pagination(int totalCount, int skip, int take)
        {
            if (take <= 0)
            {
                this.Take = 10;
            } 
            else
            {
                this.Take= take;
            }
            if (skip < 0)
            {
                this.Skip = 0;
            }
            else
            {
                this.Skip = skip;
            }

            this.Page = ((int)Math.Floor((double)skip / take));
            this.TotalCount = totalCount;
            this.PageCount = ((int)Math.Ceiling((double)totalCount / this.Take));
        }

        public void NextPage()
        {
            if (Page + 1 < this.PageCount)
            {
                this.Page++;
                this.Skip = this.Page * this.Take;
            }
        }

        public void PreviousPage()
        {
            if (Page - 1 >= 0)
            {
                this.Page--;
                this.Skip = this.Page * this.Take;
            }
        }

        public void ChangePage(int page)
        {
            if ((page >= 0) && (page < this.PageCount))
            {
                this.Page = page;
                this.Skip = this.Page * this.Take;
            }
        }

        public void ChangeTake(int take)
        {
            if (take <= 0)
            {
                this.Take = 10;
            }
            else
            {
                this.Take = take;
            }

            this.Skip = 0;
            this.Page = 0;
            this.PageCount = ((int)Math.Ceiling((double)TotalCount / this.Take));
        }

        public void IncreaseTotalCount()
        {
            this.TotalCount++;
        }

        public void DecreaseTotalCount()
        {
            this.TotalCount--;
        }

        public int FCurrentPage()
        {
            return (Page + 1);
        }

        public int FNextPage()
        {
            return (Page + 2);
        }

        public int FPreviousPage()
        {
            return Page;
        }
    }
}
