using System.IO;

public static class Global
{
    public static readonly string basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../../../"));
    public static readonly string meshPath = basePath + "data/mesh";
    public static readonly string texturePath = basePath + "data/texture";
    public static readonly int imageWidth = (int)(1080 / 2.5f);
    public static readonly int imageHeight = (int)(2220 / 2.5f);
    public static string targetName = "";
    public static string sourceName = "";
}
