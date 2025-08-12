[English](#english) | [简体中文](#%E7%AE%80%E4%BD%93%E4%B8%AD%E6%96%87)

[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/MeidoPromotionAssociation/COM3D2.PluginTemplate)

# English
## COM3D2.PluginTemplate

A ready-to-use template for creating BepInEx plugins for COM3D2 (2.0, Unity 5.6.4) and COM3D2.5 (Unity 2022.3). It includes config binding, a sample IMGUI UI, a demo hotkey, Harmony patches, and conditional compilation.

Readme written by GPT-5 high

## Features

- BepInEx Config usages: Config.Bind, KeyboardShortcut, AcceptableValueRange, hot reload via SettingChanged
- Simple IMGUI demo UI, toggled by Ctrl + F6
- HarmonyLib patch example (prefix patch on ImportCM.LoadTexture)
- Conditional compilation for 2.0 vs 2.5 (Unity 2022.3) with dedicated build configs
- Unified logging via ManualLogSource
- Demonstrates how to use Github Action to automatically build plugins



## Project layout

- `COM3D2.Example.Plugin/COM3D2.Example.Plugin.cs` — main plugin entry (`ExamplePluginMain`)
- `COM3D2.Example.Plugin/Patch1.cs` — Harmony patch samples
- `COM3D2.Example.Plugin/COM3D2.Example.Plugin.csproj` — build matrix and NuGet deps
- `COM3D2.Example.Plugin.sln` — solution file



## Requirements

- Game: COM3D2 or COM3D2.5 (Unity 2022.3)
- BepInEx 5.x installed in the game directory
- JetBrains Rider (recommended) or Visual Studio
- .NET SDK 9 (recommended)
- Internet access to restore NuGet packages



## Build

1) Open the solution: `COM3D2.Example.Plugin.sln`
2) Choose one configuration:
   - `COM3D2-Debug` / `COM3D2-Release` → COM3D2 (Unity 5.6.4, `net35`)
   - `COM3D25_UNITY_2022-Debug` / `COM3D25_UNITY_2022-Release` → COM3D2.5 (Unity 2022.3, `net48`)
3) Build the project. Artifacts will be placed under:
   - `bin/Debug/COM3D2/` or `bin/Release/COM3D2/`
   - `bin/Debug/COM3D25/` or `bin/Release/COM3D25/`

NuGet feeds used are already declared in the csproj for Unity and COM3D2 game libs.



## Install

- Copy the built plugin dll into your game directory under:
  - `BepInEx/plugins/`
- Launch the game once; the config will be generated at:
  - `BepInEx/config/Github.yourName.COM3D2.Example.Plugin.cfg`



## Usage

- Default hotkey: press `Ctrl + F6` to toggle the demo IMGUI panel
- Watch the console or `BepInEx/LogOutput.log` for messages



## Config entries

Section `[General]`:
- `Enabled` (bool, default: `true`) — enable/disable demo features
- `MaxCount` (int, default: `5`, range `1~10`) — counter upper bound
- `Greeting` (string, default: `Hello Maid!`) — greeting printed on Start

Section `[Shortcut]`:
- `ToggleUI` (KeyboardShortcut, default: `LeftControl + F6`) — toggle the demo UI

After editing and saving the cfg, hot reload is triggered via `Config.SettingChanged`.



## Harmony patch

- File: `Patch1.cs`
- Patch: prefix on `ImportCM.LoadTexture(...)`
- Behavior: logs the texture file being loaded; it does not block the original method



## Conditional compilation

- `#if COM3D25_UNITY_2022` branches implement version-specific logic
- Build configs define symbols automatically based on selection



## Development notes

- Main class: `ExamplePluginMain` (Awake/Start/Update/OnGUI/OnDestroy)
- Logger: `ExamplePluginMain.MyLogger`
- Patch registration: `Harmony.CreateAndPatchAll(typeof(Patch1), "Github.yourName.COM3D2.Example.Plugin.Patch1")`
- UI: simple IMGUI with HiDPI scaling helper



## Troubleshooting

- Plugin not loading? Ensure you built the correct configuration for your game version and copied the dll to `BepInEx/plugins/`.
- Check the log for: `COM3D2.Example.Plugin is loaded!` and Unity/game version prints.
- NuGet restore failed? Ensure network access; csproj already lists the required feeds.



## Plugin identity

- BepInEx ID: `Github.yourName.COM3D2.Example.Plugin`
- Name: `COM3D2.Example.Plugin`
- Version: `0.0.1`



## License

The Unlicense


<br>
<br>
<br>
<br>
<br>
<br>

---

<br>
<br>
<br>
<br>
<br>
<br>

# 简体中文

## COM3D2 插件模板

一个可直接上手的 BepInEx 插件模板，兼容 COM3D2（2.0，Unity 5.6.4）与 COM3D2.5（Unity 2022.3）。内置配置绑定示例、IMGUI 示例界面、演示用快捷键、Harmony 补丁以及条件编译分支。

Readme 由 GPT-5 high 编写

## 功能特性

- BepInEx 配置示例：Config.Bind、KeyboardShortcut、AcceptableValueRange、SettingChanged 热重载
- 简单 IMGUI 示例界面，通过 Ctrl + F6 切换
- HarmonyLib 补丁示例（对 ImportCM.LoadTexture 的前缀补丁日志）
- 面向 2.0 与 2.5（Unity 2022.3）的条件编译与独立构建配置
- 使用 ManualLogSource 统一日志输出
- 演示如何使用 Github Action 自动构建插件



## 工程结构

- `COM3D2.Example.Plugin/COM3D2.Example.Plugin.cs` — 主插件入口 (`ExamplePluginMain`)
- `COM3D2.Example.Plugin/Patch1.cs` — Harmony 补丁示例
- `COM3D2.Example.Plugin/COM3D2.Example.Plugin.csproj` — 构建矩阵和 NuGet 依赖项
- `COM3D2.Example.Plugin.sln` — 解决方案文件



## 环境要求

- 游戏：COM3D2 或 COM3D2.5（Unity 2022.3）
- 游戏目录已安装 BepInEx 5.x
- JetBrains Rider (推荐) 或 Visual Studio
- .NET SDK 9 (推荐)
- 可联网以恢复 NuGet 包



## 构建

1）打开解决方案：`COM3D2.Example.Plugin.sln`
2）选择构建配置：
- `COM3D2-Debug` / `COM3D2-Release` → COM3D2（Unity 5.6.4，`net35`）
- `COM3D25_UNITY_2022-Debug` / `COM3D25_UNITY_2022-Release` → COM3D2.5（Unity 2022.3，`net48`）
  3）构建项目，产物输出到：
- `bin/Debug/COM3D2/` 或 `bin/Release/COM3D2/`
- `bin/Debug/COM3D25/` 或 `bin/Release/COM3D25/`

csproj 已声明 Unity 与 COM3D2 相关 NuGet 源，构建时会自动还原依赖。



## 安装

- 将生成的 dll 复制到游戏目录下：
    - `BepInEx/plugins/`
- 启动游戏一次，配置文件将生成于：
    - `BepInEx/config/Github.yourName.COM3D2.Example.Plugin.cfg`



## 使用方法

- 默认快捷键：按下 `Ctrl + F6` 切换示例 IMGUI 面板
- 可在控制台或 `BepInEx/LogOutput.log` 查看日志输出



## 配置项

`[General]` 部分：
- `Enabled`（布尔值，默认值：`true`）— 启用/禁用演示功能
- `MaxCount`（整数，默认值：`5`，范围：`1~10`）— 计数器上限
- `Greeting`（字符串，默认值：`Hello Maid!`）— 启动时打印的问候语

`[Shortcut]` 部分：
- `ToggleUI`（键盘快捷键，默认值：`LeftControl + F6`）— 切换演示 UI

编辑并保存配置文件后，将通过 `Config.SettingChanged` 触发热重载。



## Harmony 补丁

- 文件：`Patch1.cs`
- 补丁：对 `ImportCM.LoadTexture(...)` 的前缀补丁
- 行为：记录被加载的纹理文件，不阻止原方法执行



## 条件编译

- 通过 `#if COM3D25_UNITY_2022` 分支实现版本差异逻辑
- 根据所选构建配置自动定义编译常量



## Development notes | 开发提示

- 主类：`ExamplePluginMain`（Awake/Start/Update/OnGUI/OnDestroy）
- 日志：`ExamplePluginMain.MyLogger`
- 补丁注册：`Harmony.CreateAndPatchAll(typeof(Patch1), "Github.yourName.COM3D2.Example.Plugin.Patch1")`
- UI：简易 IMGUI，提供 HiDPI 缩放辅助



## Troubleshooting | 常见问题

- 插件未加载？请确认根据游戏版本选择了正确的构建配置，并将 dll 放入 `BepInEx/plugins/`。
- 检查日志中是否出现：`COM3D2.Example.Plugin is loaded!` 以及 Unity/游戏版本信息。
- NuGet 还原失败？检查网络访问；csproj 已包含需要的源。

---

## Plugin identity | 插件标识

- 插件 ID：`Github.yourName.COM3D2.Example.Plugin`
- 名称：`COM3D2.Example.Plugin`
- 版本：`0.0.1`

