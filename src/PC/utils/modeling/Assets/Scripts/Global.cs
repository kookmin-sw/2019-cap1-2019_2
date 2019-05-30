using System.IO;

public static class Global
{
    public static readonly string basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../../../"));
    public static readonly string meshPath = basePath + "data/mesh";
    public static readonly string texturePath = basePath + "data/texture";
    public static string targetName = "";
    public static string sourceName = "";
}
