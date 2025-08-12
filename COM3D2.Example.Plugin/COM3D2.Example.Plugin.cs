/*
  Example plugin demonstrating:
 - BepInEx config (Config.Bind / KeyboardShortcut / AcceptableValueRange / hot reload)
 - Conditional compilation for COM3D2 (2.0) and COM3D2.5 (Unity 2022.3)
 - A simple hotkey and IMGUI UI
 - Using HarmonyLib to intercept methods
 How to use:
 1) Choose a build configuration:
    - COM3D2-Debug / COM3D2-Release -> 2.0 (Unity 5.6.4)
    - COM3D25_UNITY_2022-Debug / COM3D25_UNITY_2022-Release -> 2.5 (Unity 2022.3)
 2) After first run, edit the generated config at BepInEx/config/Github.yourName.COM3D2.Example.Plugin.cfg
 3) Default hotkey: Ctrl + F6 to toggle the demo UI

 示例插件：展示
 - BepInEx 配置（Config.Bind / KeyboardShortcut / AcceptableValueRange / 热重载）
 - 条件编译兼容 COM3D2 (2.0) 与 COM3D2.5 (Unity 2022.3)
 - 简单快捷键与 IMGUI UI 示例
 - 使用 HarmonyLib 来拦截方法
 使用方法：
 1) 选择构建配置：
    - COM3D2-Debug / COM3D2-Release -> 2.0（Unity 5.6.4）
    - COM3D25_UNITY_2022-Debug / COM3D25_UNITY_2022-Release -> 2.5（Unity 2022.3）
 2) 首次运行后在 BepInEx/config/Github.yourName.COM3D2.Example.Plugin.cfg 中修改配置
 3) 默认快捷键：Ctrl + F6 切换示例 UI
*/

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace COM3D2.Example.Plugin;

[BepInPlugin("Github.yourName.COM3D2.Example.Plugin",
    "COM3D2.Example.Plugin", "0.0.1")]
public class ExamplePluginMain : BaseUnityPlugin
{
    // use a universal logger
    // 使用通用日志
    public static ManualLogSource MyLogger;

    private int _counter;

    // ---------- Config entries ----------
    // ---------- 配置项示例 ----------
    private ConfigEntry<bool> _enabled;
    private ConfigEntry<string> _greeting;

    // Harmony instance for patching
    // 注册 Harmony 实例以应用补丁
    private Harmony _harmonyPatch1;
    private ConfigEntry<int> _maxCount;

    // ---------- Runtime state ---------
    // ---------- 运行时状态 ----------
    private bool _showUI;
    private ConfigEntry<KeyboardShortcut> _toggleShortcut;

    /// <summary>
    ///     Awake: called when the game is loaded. Good place for initialization and binding config
    ///     entries.
    ///     Awake：游戏加载时调用。适合做初始化与配置绑定。
    ///     https://docs.unity3d.com/2022.3/Documentation/ScriptReference/MonoBehaviour.Awake.html
    /// </summary>
    private void Awake()
    {
        // Initialize logger
        // 初始化日志
        MyLogger = Logger;

        // Bind BepInEx config entries. The config file is generated under BepInEx/config after first run.
        // 绑定 BepInEx 配置。首次运行后会在 BepInEx/config/Github.yourName.COM3D2.Example.Plugin.cfg 生成配置文件
        _enabled = Config.Bind(
            "General", "Enabled", true,
            "Enable features in this sample plugin (hot reload)");

        _maxCount = Config.Bind(
            "General", "MaxCount", 5,
            new ConfigDescription(
                "Demo counter upper bound (1-10)",
                new AcceptableValueRange<int>(1, 10)));

        _greeting = Config.Bind(
            "General", "Greeting", "Hello Maid!",
            "Greeting printed in Start");

        _toggleShortcut = Config.Bind(
            "Shortcut", "ToggleUI",
            new KeyboardShortcut(KeyCode.F6, KeyCode.LeftControl),
            "Hotkey to toggle the demo UI (default: Ctrl+F6)");

        // Subscribe to config hot-reload event: triggered when cfg is changed and saved
        // 监听配置变化：修改 cfg 后保存，游戏中会回调此事件
        Config.SettingChanged += OnSettingChanged;

        // Subscribe to shortcut hot-reload event - 监听快捷键变化
        // _toggleShortcut.SettingChanged += OnToggleShortcutChanged;

        // Register Harmony patches, which that patches will be applied
        // 注册补丁，此时会应用补丁
        _harmonyPatch1 = Harmony.CreateAndPatchAll(typeof(Patch1),
            "Github.yourName.COM3D2.Example.Plugin.Patch1");

        // Conditional compilation example for different game versions
        // 根据不同的构建配置输出差异化信息（条件编译示例）
#if COM3D25_UNITY_2022
        MyLogger.LogInfo("running on COM3D2.5 / Unity 2022.3");
#else
        MyLogger.LogInfo("running on COM3D2 / Unity 5.6.4");
#endif

        // Print Unity and game information
        // 打印 Unity 及游戏信息
        MyLogger.LogInfo($"Unity: {Application.unityVersion}  Game: {Application.productName}");

        MyLogger.LogInfo("COM3D2.Example.Plugin is loaded!");
    }

    /// <summary>
    ///     Start: called before the first frame update.
    ///     Start：在游戏开始运行前调用。
    ///     https://docs.unity3d.com/2022.3/Documentation/ScriptReference/MonoBehaviour.Start.html
    /// </summary>
    private void Start()
    {
        if (_enabled.Value)
            MyLogger.LogInfo($"COM3D2.Example.Plugin is started! Greeting: {_greeting.Value}");
        else
            MyLogger.LogInfo("Plugin is disabled (Enabled=false).");
    }

    /// <summary>
    ///     Update：每帧调用一次。适合处理输入或轮询逻辑。
    ///     Update: called once per frame. Good for handling inputs and polling logic.
    /// </summary>
    private void Update()
    {
        // Toggle the demo UI
        // 快捷键：切换示例 UI
        if (_toggleShortcut.Value.IsDown())
        {
            _showUI = !_showUI;
            MyLogger.LogInfo($"Toggle demo UI: {(_showUI ? "Show" : "Hide")}");
        }

        // Simple counter example: usage of AcceptableValueRange
        // 简单计数器：演示 AcceptableValueRange 的使用
        if (_enabled.Value && _counter < _maxCount.Value) _counter++;
    }

    /// <summary>
    ///     OnDestroy: called when the plugin is unloaded or the game exits. Good for cleanup.
    ///     OnDestroy：游戏退出或插件卸载时调用。适合做清理工作。
    /// </summary>
    private void OnDestroy()
    {
        MyLogger.LogInfo("COM3D2.Example.Plugin is destroyed!");
        Config.SettingChanged -= OnSettingChanged;

        // Cleanup();

        // Unpatch Harmony methods
        // 清理 Harmony 方法
        _harmonyPatch1?.UnpatchSelf();
        _harmonyPatch1 = null;
    }

    /// <summary>
    ///     Simple IMGUI demo: show config values, toggle features, and reset the counter.
    ///     简单 IMGUI 示例界面，用于展示配置值、切换功能与重置计数器。
    /// </summary>
    private void OnGUI()
    {
        if (!_showUI) return;

        // HiDPI scaling: scale all subsequent IMGUI drawing from top-left
        // HiDPI 缩放：从左上角开始缩放所有 IMGUI 绘制
        var prevMatrix = GUI.matrix;
        var scale = GetHiDpiScale();
        GUI.matrix =
            Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(scale, scale, 1f)) *
            prevMatrix;

        // Logical (unscaled) screen size; use these instead of Screen.width/height when anchoring to borders
        // 逻辑 (未缩放) 屏幕尺寸
        var logicalWidth = Screen.width / scale;
        var logicalHeight = Screen.height / scale;

        // For demo purpose; use your own UI solution for production
        // 仅为示例，真实项目可使用自定义 UI 或 Unity UI 系统
        GUILayout.BeginArea(new Rect(20, 20, 420, 220));
        GUILayout.Label("COM3D2.Example.Plugin - Demo UI");
        GUILayout.Space(8);

        GUILayout.Label($"Enabled: {_enabled.Value}");
        GUILayout.Label($"MaxCount: {_maxCount.Value}");
        GUILayout.Label($"Greeting: {_greeting.Value}");
        GUILayout.Space(8);

        GUILayout.Label(GetVersionSpecificMessage());

        if (GUILayout.Button("Reset counter"))
        {
            _counter = 0;
            MyLogger.LogInfo("Counter reset.");
        }

        if (GUILayout.Button(_enabled.Value ? "Disable plugin features" : "Enable plugin features"))
        {
            _enabled.Value = !_enabled.Value;
            Config.Save(); // Persist changes to config file immediately
        }

        GUILayout.EndArea();

        // Restore previous GUI.matrix to avoid affecting other IMGUI drawing
        GUI.matrix = prevMatrix;
    }

    /// <summary>
    ///     Example: implement different logic per version.
    ///     Real API differences can be placed in these branches.
    ///     示例：根据版本差异实现不同逻辑。
    ///     你可以把真正有差异的 API 调用放进条件编译分支中。
    /// </summary>
#if COM3D25_UNITY_2022
    private string GetVersionSpecificMessage()
    {
        return "This is the implementation for 2.5 (adjust real API calls here).";
    }
#else
    private string GetVersionSpecificMessage()
    {
        return "This is the implementation for 2.0 (adjust real API calls here).";
    }
#endif

    // HiDPI helpers
    // HiDPI 帮助
    private static float GetHiDpiScale()
    {
        // Calculate scale based on a 1920x1080 design resolution
        // 根据 1920x1080 设计分辨率计算比例
        return Mathf.Max(Screen.height / 1080.0f, Screen.width / 1920.0f);
    }

    // Coordinate conversion utilities that ignore GUI.matrix scaling
    // 坐标转换工具，忽略 GUI.matrix 的缩放
    private static Vector2 ScreenToGUIPointUnscaled(Vector2 screenPoint)
    {
        var saved = GUI.matrix;
        GUI.matrix = Matrix4x4.identity;
        var result = GUIUtility.ScreenToGUIPoint(screenPoint);
        GUI.matrix = saved;
        return result;
    }

    private static Vector2 GUIToScreenPointUnscaled(Vector2 guiPoint)
    {
        var saved = GUI.matrix;
        GUI.matrix = Matrix4x4.identity;
        var result = GUIUtility.GUIToScreenPoint(guiPoint);
        GUI.matrix = saved;
        return result;
    }

    /// <summary>
    ///     Config hot-reload callback: triggered when a cfg entry is changed and saved.
    ///     配置热重载回调：当 cfg 中的某项被修改并保存后触发。
    /// </summary>
    private void OnSettingChanged(object sender, SettingChangedEventArgs e)
    {
        MyLogger.LogInfo(
            $"Config changed: [{e.ChangedSetting.Definition.Section}] {e.ChangedSetting.Definition.Key} = {e.ChangedSetting.BoxedValue}");
    }
}