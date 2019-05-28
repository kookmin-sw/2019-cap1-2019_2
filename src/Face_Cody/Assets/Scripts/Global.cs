using UnityEngine;

public static class Global
{
    public static readonly string imagePath = Application.persistentDataPath + "/Pictures";
    public static readonly string logPath = Application.persistentDataPath + "/Logs";
    public static readonly string ipAddress = "192.168.0.5";
    public static int selectMode = 0; /* 0: not select
                                         1: select target image
                                         2: select source image */
    public static string targetImageName = "";
    public static string sourceImageName = "";
}