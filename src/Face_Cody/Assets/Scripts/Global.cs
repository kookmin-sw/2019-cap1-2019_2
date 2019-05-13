using UnityEngine;

public static class Global
{
    public static readonly string imagePath = Application.persistentDataPath + "/Pictures/";
    public static readonly string logPath = Application.persistentDataPath + "/Logs/";
    public static readonly string ipAddress = "192.168.0.101";
    public static bool selectMode = false;
    public static string targetImageName = "";
    public static string sourceImageName = "";
}