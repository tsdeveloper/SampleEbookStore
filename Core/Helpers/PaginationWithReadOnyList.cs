using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Helpers;
public class PaginationWithReadOnyList<T> where T : class
{
    public PaginationWithReadOnyList(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = count;
        Data = data;
    }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
    public IReadOnlyList<T> Data { get; set; }
}

public class Pagination<T> where T : class
{
    public Pagination(int pageIndex, int pageSize, int count, IList<T> data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = count;
        Data = data;
    }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
    public IList<T> Data { get; set; }
}

public class PaginationModulesWithReadOnyList<T> where T : class
{
    public PaginationModulesWithReadOnyList(int pageIndex, int pageSize, int count, int countLessons,
        IReadOnlyList<T> data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = count;
        CountLessons = countLessons;
        Data = data;
    }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
    public int CountLessons { get; set; }
    public IReadOnlyList<T> Data { get; set; }
}