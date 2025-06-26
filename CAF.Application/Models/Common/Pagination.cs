using Microsoft.EntityFrameworkCore;

namespace CAF.Application.Models.Common;

public class Pagination<T> : List<T>
{
    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int TotalCount { get; private set; }
    public IEnumerable<int> AvailablePageSizes  { get; set; }
    public List<T> Datas { get; private set; }

    public Pagination(List<T> items, int count,int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        CurrentPage = pageIndex;
        TotalCount = count;
        AvailablePageSizes = new List<int> { 10, 20, 50, 100 }; // Example page sizes
        Datas = items ?? default!;
        AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<Pagination<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return new Pagination<T>(items, count, pageIndex, pageSize);
    }
}
