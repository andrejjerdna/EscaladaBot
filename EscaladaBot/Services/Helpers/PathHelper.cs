namespace EscaladaBot.Services.Helpers;

public static class PathHelper
{
    public static string GetFolderPath(string folderId)
    {
        return GetFolder(folderId);
    }
    
    public static string GetFilePath(string folderPath, string name, string ext)
    {
        return Path.Combine(folderPath, name + ext);
    }

    private static string GetFolder(string folderId)
    {
        return Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, 
            "problems", 
            folderId);
    }
}