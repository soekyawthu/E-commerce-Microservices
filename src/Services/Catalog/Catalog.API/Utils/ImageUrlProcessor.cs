namespace Catalog.API.Utils;

public class ImageUrlProcessor
{
    public static string? GetFileNameFromUrl(string imageUrl)
    {
        try
        {
            var uri = new Uri(imageUrl);
            var fileName = Path.GetFileName(uri.LocalPath);
            return fileName;
        }
        catch (UriFormatException ex)
        {
            Console.WriteLine($"Error parsing URL: {ex.Message}");
            return null;
        }
    }
}