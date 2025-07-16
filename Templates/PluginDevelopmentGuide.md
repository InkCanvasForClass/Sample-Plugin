# InkCanvasForClass 插件开发指南

## 简介

InkCanvasForClass (ICC) 插件系统允许开发者扩展和增强ICC的功能。本文档将指导你如何创建一个ICC插件。

## 插件基础

### 插件文件格式

ICC插件使用`.iccpp`（InkCanvasForClass Plugin Package）文件扩展名。这实际上是一个包含插件程序集的.NET动态链接库（DLL）文件，只是扩展名不同。

### 插件存放位置

插件文件存放在ICC安装目录下的`Plugins`文件夹中。通常在`%APPDATA%\Ink Canvas\Plugins\`。

## 开发环境准备

### 所需工具

1. Visual Studio 2019或更高版本
2. .NET Framework 4.7.2 SDK
3. 对ICC主程序的引用（可选，用于获得更好的开发体验）

### 创建插件项目

1. 在Visual Studio中创建一个新的"类库(.NET Framework)"项目
2. 将目标框架设置为.NET Framework 4.7.2
3. 添加必要的引用：
   - PresentationCore
   - PresentationFramework
   - System.Windows.Forms
   - WindowsBase
   - System.Xaml

## 插件开发

### 实现插件接口

所有ICC插件都必须实现`IPlugin`接口。以下是接口定义：

```csharp
public interface IPlugin
{
    string Name { get; }
    string Description { get; }
    Version Version { get; }
    string Author { get; }
    bool IsBuiltIn { get; }
    
    void Initialize();
    void Enable();
    void Disable();
    UserControl GetSettingsView();
    void Cleanup();
}
```

### 插件生命周期

1. **Initialize**: 插件加载时调用，用于初始化工作
2. **Enable**: 插件启用时调用
3. **Disable**: 插件禁用时调用
4. **GetSettingsView**: 获取插件设置界面
5. **Cleanup**: 插件卸载时调用，用于清理资源

### 示例插件代码

```csharp
using System;
using System.Windows;
using System.Windows.Controls;

namespace YourCompany.ICCPlugin
{
    public class SamplePlugin : IPlugin
    {
        public string Id { get; private set; }
        public bool IsEnabled { get; private set; } = false;
        
        public string Name => "示例插件";
        public string Description => "这是一个示例插件。";
        public Version Version => new Version(1, 0, 0);
        public string Author => "Your Name";
        public bool IsBuiltIn => false;
        
        public void Initialize()
        {
            Id = GetType().FullName;
            // 初始化代码
        }
        
        public void Enable()
        {
            IsEnabled = true;
            // 启用代码
        }
        
        public void Disable()
        {
            IsEnabled = false;
            // 禁用代码
        }
        
        public UserControl GetSettingsView()
        {
            // 返回设置界面
            return new SampleSettingsControl();
        }
        
        public void Cleanup()
        {
            // 清理资源
        }
    }
}
```

## 插件功能开发

### 访问ICC功能

插件可以通过反射或直接引用ICC主程序集来访问ICC的功能。为了使插件更加稳定，建议使用反射方式访问ICC功能，以避免因ICC版本更新导致的兼容性问题。

### 存储配置

插件可以使用自己的配置文件来存储设置。建议将配置文件存储在`%APPDATA%\Ink Canvas\PluginConfigs\`目录下。

### 设置界面

插件可以提供自己的设置界面，这个界面应该是一个`UserControl`类型的控件。

```csharp
public class SampleSettingsControl : UserControl
{
    public SampleSettingsControl()
    {
        var panel = new StackPanel();
        
        // 添加设置控件
        panel.Children.Add(new TextBlock 
        { 
            Text = "插件设置", 
            FontSize = 16,
            FontWeight = FontWeights.Bold
        });
        
        // 设置界面内容
        this.Content = panel;
    }
}
```

## 打包和分发插件

1. 编译插件项目，生成DLL文件
2. 将DLL文件重命名为`.iccpp`扩展名
3. 将插件文件放入`%APPDATA%\Ink Canvas\Plugins\`目录

## 最佳实践

1. 确保插件在启用和禁用时不会影响ICC的稳定性
2. 在插件的`Cleanup()`方法中正确释放所有资源
3. 添加适当的错误处理，避免插件崩溃导致ICC崩溃
4. 为插件提供简洁明了的设置界面
5. 使用合适的命名空间，避免与其他插件或ICC本身冲突

## 示例项目

我们提供了一个完整的示例插件项目，你可以在`Templates/ICCPluginTemplate.cs`中找到它。这个示例展示了如何创建一个基本的ICC插件，包括：

1. 实现基本插件接口
2. 创建设置界面
3. 处理插件生命周期

## 常见问题解答

### Q: 我的插件在ICC中无法加载，可能是什么原因？
A: 请检查：
   - 插件文件是否使用了`.iccpp`扩展名
   - 插件是否实现了`IPlugin`接口
   - 插件是否存放在正确的目录中
   - 插件是否引用了与ICC兼容的.NET Framework版本

### Q: 我如何调试我的插件？
A: 在Visual Studio中，设置调试属性，将ICC的可执行文件路径设置为启动程序，这样可以在ICC中运行时调试插件。

### Q: 插件热重载是如何工作的？
A: ICC定期检查插件文件的变化，当检测到变化时，会自动重新加载插件。这使得你可以在不重启ICC的情况下更新插件。

## 联系支持

如果你在开发插件过程中遇到任何问题，可以通过以下方式获取帮助：

- GitHub Issue: [https://github.com/InkCanvasForClass/community/issues](https://github.com/InkCanvasForClass/community/issues)
- QQ群: 1054377349 