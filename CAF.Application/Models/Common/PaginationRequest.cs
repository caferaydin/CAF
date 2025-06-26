namespace CAF.Application.Models.Common;

public class PaginationRequest
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
}
