using System;
using HarmonyLib;

namespace COM3D2.Example.Plugin;

// Demonstration Harmony patch collection
// check https://harmony.pardeike.net/articles/intro.html for more information
// Harmony 示例补丁集合
// 检查 https://harmony.pardeike.net/articles/intro.html 了解更多信息

public class Patch1
{
    /// <summary>
    ///     run before ImportCM.LoadTexture, record what file was loaded, don't block the original method
    ///     在 ImportCM.LoadTexture 前调用，记录加载了什么文件，不阻止原方法运行
    /// </summary>
    /// <param name="__result"></param>
    /// <param name="f_fileSystem"></param>
    /// <param name="f_strFileName"></param>
    /// <param name="usePoolBuffer"></param>
    [HarmonyPatch(typeof(ImportCM), nameof(ImportCM.LoadTexture))]
    [HarmonyPrefix]
    private static void ImportCM_LoadTexture_Prefix(ref TextureResource __result,
        AFileSystemBase f_fileSystem,
        string f_strFileName,
        bool usePoolBuffer)
    {
        try
        {
            ExamplePluginMain.MyLogger.LogInfo(
                $"ImportCM_LoadTexture_Prefix called: {f_strFileName}");
        }
        catch (Exception e)
        {
            ExamplePluginMain.MyLogger.LogError(
                $"ImportCM_LoadTexture_Prefix unknown error{e.Message}\n{e}");
        }
    }
}