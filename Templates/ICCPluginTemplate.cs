using System;
using System.Windows;
using System.Windows.Controls;

// 外部插件命名空间
namespace YourCompany.ICCPlugin
{
    /// <summary>
    /// InkCanvasForClass插件示例
    /// </summary>
    public class SamplePlugin : IPlugin
    {
        #region 插件基本信息

        /// <summary>
        /// 插件ID
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// 插件是否启用
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name => "示例插件";

        /// <summary>
        /// 插件描述
        /// </summary>
        public string Description => "这是一个示例插件，用于演示如何创建InkCanvasForClass插件。";

        /// <summary>
        /// 插件版本
        /// </summary>
        public Version Version => new Version(1, 0, 0);

        /// <summary>
        /// 插件作者
        /// </summary>
        public string Author => "Your Name";

        /// <summary>
        /// 是否为内置插件
        /// </summary>
        public bool IsBuiltIn => false;

        #endregion

        #region 插件生命周期

        /// <summary>
        /// 初始化插件
        /// </summary>
        public void Initialize()
        {
            // 设置插件ID
            Id = GetType().FullName;
            
            // 初始化工作
            Console.WriteLine($"插件 {Name} 已初始化");
        }

        /// <summary>
        /// 启用插件
        /// </summary>
        public void Enable()
        {
            IsEnabled = true;
            Console.WriteLine($"插件 {Name} 已启用");
        }

        /// <summary>
        /// 禁用插件
        /// </summary>
        public void Disable()
        {
            IsEnabled = false;
            Console.WriteLine($"插件 {Name} 已禁用");
        }

        /// <summary>
        /// 获取插件设置界面
        /// </summary>
        /// <returns>插件设置界面</returns>
        public UserControl GetSettingsView()
        {
            // 创建设置界面
            return new SamplePluginSettingsControl();
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        public void Cleanup()
        {
            // 清理资源
            Console.WriteLine($"插件 {Name} 已卸载");
        }

        #endregion
    }

    /// <summary>
    /// 插件设置控件
    /// </summary>
    public class SamplePluginSettingsControl : UserControl
    {
        public SamplePluginSettingsControl()
        {
            // 创建设置界面布局
            var panel = new StackPanel
            {
                Margin = new Thickness(10)
            };

            // 添加标题
            panel.Children.Add(new TextBlock
            {
                Text = "示例插件设置",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            // 添加说明文字
            panel.Children.Add(new TextBlock
            {
                Text = "这是一个示例设置界面，你可以在这里添加自己的设置控件。",
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 15)
            });

            // 添加按钮
            var button = new Button
            {
                Content = "测试功能",
                Padding = new Thickness(10, 5, 10, 5),
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            
            button.Click += (sender, e) => 
            {
                MessageBox.Show("插件功能测试成功！", "示例插件", MessageBoxButton.OK, MessageBoxImage.Information);
            };
            
            panel.Children.Add(button);

            // 设置控件内容
            this.Content = panel;
        }
    }
}

// 插件接口定义（可以在单独的文件中）
public interface IPlugin
{
    /// <summary>
    /// 插件名称
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// 插件描述
    /// </summary>
    string Description { get; }
    
    /// <summary>
    /// 插件版本
    /// </summary>
    Version Version { get; }
    
    /// <summary>
    /// 插件作者
    /// </summary>
    string Author { get; }
    
    /// <summary>
    /// 是否为内置插件
    /// </summary>
    bool IsBuiltIn { get; }
    
    /// <summary>
    /// 初始化插件
    /// </summary>
    void Initialize();
    
    /// <summary>
    /// 启用插件
    /// </summary>
    void Enable();
    
    /// <summary>
    /// 禁用插件
    /// </summary>
    void Disable();
    
    /// <summary>
    /// 获取插件设置界面
    /// </summary>
    /// <returns>插件设置界面</returns>
    UserControl GetSettingsView();
    
    /// <summary>
    /// 插件卸载时的清理工作
    /// </summary>
    void Cleanup();
} 