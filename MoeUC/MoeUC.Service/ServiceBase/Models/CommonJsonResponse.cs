namespace MoeUC.Service.ServiceBase.Models;

public class CommonJsonResponse<T>
{
    public bool Success { get; set; }

    public string? ErrorCode { get; set; } = "200";

    public string? Message { get; set; }
    
    public T? Data { get; set; }

    public RequestStatisticModel? Statistics { get; set; }
}

public class CommonJsonResponse : CommonJsonResponse<object>
{
    
}

