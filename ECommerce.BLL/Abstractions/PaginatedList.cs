using Microsoft.EntityFrameworkCore;

namespace ECommerce.BLL.Abstractions
{
    public class PaginatedList<T>
    {

        public PaginatedList(List<T> items,int pageNumber , int pageSize,int count)
        {
            Items = items;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
        public List<T> Items { get; private set; } = [];

        public int PageNumber { get; private set; }

        public int TotalPages { get; private set; }

        public bool IsPrevious => PageNumber > 1;

        public bool IsNext => PageNumber<TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, pageNumber, pageSize,count);
           
        }


    }
}

