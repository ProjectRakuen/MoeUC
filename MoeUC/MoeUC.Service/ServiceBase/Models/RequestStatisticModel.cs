namespace MoeUC.Service.ServiceBase.Models;

public class RequestStatisticModel
{
    public int DbRead { get; set; }
    
    public int DbWrite { get; set; }
    
    public int CacheRead { get; set; }
    
    public int CacheWrite { get; set; }
}