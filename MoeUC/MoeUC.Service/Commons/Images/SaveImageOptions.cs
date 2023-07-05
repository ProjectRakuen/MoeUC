namespace MoeUC.Service.Commons.Images;

public class SaveImageOptions
{
    public string Name { get; set; } = Guid.NewGuid().ToString("N");

    public string? SubDir { get; set; } = null;

    public int? Width { get; set; }

    public int? Height { get; set; }
}