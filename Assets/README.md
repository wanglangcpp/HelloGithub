# 文件夹说明

## Behavior Designer

来自 [Opsive](http://www.opsive.com/assets/BehaviorDesigner/) 的行为树插件。

## Bugly

来自 [Tencent](https://bugly.qq.com/v2/sdkDownload) 的错误/异常报告插件。版本：Unity Plugin 1.5.0，其中 iOS Plugin 单独升级到 2.4.0。

## EasyTouch

旧版本触控插件，有改动。

## GameFramework

游戏框架插件。

## Gizmos

__仅用于编辑器。__在 Unity 编辑器层级视图中对象旁边显示图标用的文件夹。任何插件都可能把图标放在此处。

## JPush

本地通知插件。

## Main

项目主目录。

## NGUI

图形界面插件，版本 3.10.2，来自 [Tasharen](http://www.tasharen.com/?page_id=140)。

- 移除了示例。
- 修改 UIDrawCall.cs 以防止特殊情况的报错。

## NSubstitute

__仅用于编辑器。__单元测试辅助库，用于伪造 C# 类以方便测试。

## SharpSSH

__仅用于编辑器。__一个 SSH/SFTP 库，版本：1.1.1.13。

## StreamingAssets

Unity 文件夹，用于放置打进程序包的 AssetBundle。

## ToLua

来自 [github](https://github.com/topameng/tolua) 的 Lua 插件。在提交 84cac4881c94a39376ba6c441d2ce8e9cbe0cdc5 (master) 上做了如下修改：

- 修改文件夹结构以适应 Unity 5.x。
- 修改 CustomSettings.cs 中的一些关于路径的常量。
- 移除示例。
- 暂时不导入 LuaJit 工具。
