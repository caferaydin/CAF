namespace CAF.Application.Models.Common;

public class ResultModel<T>
{
    public string ResultMessage { get; set; }
    public int ResultCode { get; set; }
    public T Data { get; set; }
}