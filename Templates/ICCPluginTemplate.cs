using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq; // Added for .OfType()

namespace Ink_Canvas.Helpers.Plugins
{
    /// <summary>
    /// 插件模板 V2，展示如何使用优化后的接口结构
    /// 注意：实际开发时，请将此类移到单独的程序集中
    /// </summary>
    public class PluginTemplateV2 : EnhancedPluginBaseV2
    {
        #region 插件基本信息

        /// <summary>
        /// 插件名称
        /// </summary>
        public override string Name => "插件模板 V2";

        /// <summary>
        /// 插件描述
        /// </summary>
        public override string Description => "这是一个使用优化接口结构的插件开发模板，展示如何分类使用不同的服务接口。";

        /// <summary>
        /// 插件版本
        /// </summary>
        public override Version Version => new Version(2, 0, 0);

        /// <summary>
        /// 插件作者
        /// </summary>
        public override string Author => "ICC CE 团队";

        /// <summary>
        /// 是否为内置插件（外部插件请返回false）
        /// </summary>
        public override bool IsBuiltIn => false;

        #endregion

        #region 插件生命周期

        /// <summary>
        /// 插件初始化
        /// 在这里进行插件的初始化工作，如加载配置、注册事件等
        /// </summary>
        public override void Initialize()
        {
            // 先调用基类方法，这样会设置插件ID和记录日志
            base.Initialize();

            // 示例：记录初始化信息
            LogHelper.WriteLogToFile($"插件 {Name} 开始初始化");

            // 示例：加载配置
            LoadConfig();

            // 示例：注册自定义事件
            RegisterEventHandler("CanvasCleared", OnCanvasCleared);
            RegisterEventHandler("DrawingModeChanged", OnDrawingModeChanged);

            LogHelper.WriteLogToFile($"插件 {Name} 初始化完成");
        }

        /// <summary>
        /// 启用插件
        /// 在这里激活插件的功能
        /// </summary>
        public override void Enable()
        {
            if (IsEnabled) return; // 防止重复启用

            base.Enable();

            // 示例：显示启用通知
            ShowNotification($"插件 {Name} 已启用", NotificationType.Success);

            // 示例：获取当前状态
            var currentMode = CurrentDrawingMode;
            LogHelper.WriteLogToFile($"当前绘制模式: {currentMode}");

            // 示例：设置一些默认配置
            SetSetting("PluginTemplateV2.Enabled", true);
            SaveSettings();
        }

        /// <summary>
        /// 禁用插件
        /// 在这里停用插件的功能
        /// </summary>
        public override void Disable()
        {
            if (!IsEnabled) return; // 防止重复禁用

            base.Disable();

            // 示例：显示禁用通知
            ShowNotification($"插件 {Name} 已禁用", NotificationType.Warning);

            // 示例：保存禁用状态
            SetSetting("PluginTemplateV2.Enabled", false);
            SaveSettings();
        }

        /// <summary>
        /// 插件卸载时的清理工作
        /// </summary>
        public override void Cleanup()
        {
            // 示例：注销事件处理器
            UnregisterEventHandler("CanvasCleared", OnCanvasCleared);
            UnregisterEventHandler("DrawingModeChanged", OnDrawingModeChanged);

            base.Cleanup();
        }

        #endregion

        #region 插件设置界面

        /// <summary>
        /// 获取插件设置界面
        /// </summary>
        /// <returns>插件设置界面</returns>
        public override UserControl GetSettingsView()
        {
            // 创建插件设置界面
            return new PluginTemplateV2SettingsControl();
        }

        #endregion

        #region 插件功能方法

        /// <summary>
        /// 示例方法：执行一些功能
        /// </summary>
        public void DoSomething()
        {
            if (!IsEnabled) return;

            try
            {
                // 示例：使用获取服务
                var currentMode = GetService.CurrentDrawingMode;
                var canUndo = GetService.CanUndo;

                // 示例：使用窗口服务
                ShowNotification($"当前模式: {currentMode}, 可撤销: {canUndo}", NotificationType.Info);

                // 示例：使用操作服务
                if (ShowConfirmDialog("是否要清除画布？", "确认操作"))
                {
                    ActionService.ClearCanvas();
                    ShowNotification("画布已清除", NotificationType.Success);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogToFile($"执行插件功能时出错: {ex.Message}", LogHelper.LogType.Error);
                ShowNotification($"执行失败: {ex.Message}", NotificationType.Error);
            }
        }

        /// <summary>
        /// 示例方法：演示分类使用服务接口
        /// </summary>
        public void DemonstrateServiceUsage()
        {
            // 1. 使用获取服务 - 获取各种状态和信息
            var mainWindow = GetService.MainWindow;
            var currentCanvas = GetService.CurrentCanvas;
            var isDarkTheme = GetService.IsDarkTheme;
            var currentInkColor = GetService.CurrentInkColor;

            // 2. 使用窗口服务 - 处理窗口和用户交互
            ShowNotification("演示服务使用", NotificationType.Info);
            var userInput = ShowInputDialog("请输入一些文本:", "输入测试", "默认值");
            
            if (ShowConfirmDialog($"您输入的是: {userInput}\n是否继续？", "确认"))
            {
                // 3. 使用操作服务 - 执行各种操作
                ActionService.SetInkColor(System.Windows.Media.Colors.Red);
                ActionService.SetDrawingMode(1);
                
                ShowNotification("设置已应用", NotificationType.Success);
            }
        }

        #endregion

        #region 事件处理器

        /// <summary>
        /// 画布清除事件处理器
        /// </summary>
        private void OnCanvasCleared(object sender, EventArgs e)
        {
            if (IsEnabled)
            {
                ShowNotification("画布已被清除", NotificationType.Info);
            }
        }

        /// <summary>
        /// 绘制模式变更事件处理器
        /// </summary>
        private void OnDrawingModeChanged(object sender, EventArgs e)
        {
            if (IsEnabled)
            {
                var newMode = GetService.CurrentDrawingMode;
                LogHelper.WriteLogToFile($"绘制模式已变更为: {newMode}");
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 加载配置
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                // 示例：从设置中加载配置
                var isEnabled = GetSetting("PluginTemplateV2.Enabled", false);
                var customSetting = GetSetting("PluginTemplateV2.CustomSetting", "默认值");

                LogHelper.WriteLogToFile($"加载配置: Enabled={isEnabled}, CustomSetting={customSetting}");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogToFile($"加载配置时出错: {ex.Message}", LogHelper.LogType.Error);
            }
        }

        #endregion
    }

    /// <summary>
    /// 插件设置控件 V2
    /// </summary>
    public class PluginTemplateV2SettingsControl : UserControl
    {
        public PluginTemplateV2SettingsControl()
        {
            // 创建设置界面布局
            var panel = new StackPanel
            {
                Margin = new Thickness(10)
            };

            // 添加标题
            panel.Children.Add(new TextBlock
            {
                Text = "插件模板 V2 设置",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            // 添加说明文字
            panel.Children.Add(new TextBlock
            {
                Text = "这是一个使用优化接口结构的插件模板。\n展示了如何分类使用不同的服务接口：\n• IGetService - 获取状态和信息\n• IWindowService - 窗口和用户交互\n• IActionService - 执行操作",
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 10)
            });

            // 添加功能按钮
            var demoButton = new Button
            {
                Content = "演示服务使用",
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(10, 5, 10, 5)
            };

            demoButton.Click += (s, e) =>
            {
                var plugin = PluginManager.Instance.Plugins.OfType<PluginTemplateV2>().FirstOrDefault();
                plugin?.DemonstrateServiceUsage();
            };

            panel.Children.Add(demoButton);

            // 设置控件内容
            Content = panel;
        }
    }
} 
