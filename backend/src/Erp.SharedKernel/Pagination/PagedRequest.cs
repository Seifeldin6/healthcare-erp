namespace Erp.SharedKernel.Pagination;

public class PagedRequest
{
    private int _pageNumber = 1;
    private int _pageSize = 10;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 1 : value > 50 ? 50 : value;
    }

    public int NormalizedPageNumber => _pageNumber;
    public int NormalizedPageSize => _pageSize;
}
