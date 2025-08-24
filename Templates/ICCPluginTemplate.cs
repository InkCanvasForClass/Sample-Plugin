using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Ink_Canvas.Helpers.Plugins
{
    /// <summary>
    /// 示例插件
    /// </summary>
    public class ExamplePlugin : EnhancedPluginBase
    {
        private Button _toolbarButton;
        private MenuItem _menuItem;
        private bool _isDrawingMode = false;

        public override string Name => "示例插件";

        public override string Description => "这是一个示例插件";

        public override Version Version => new Version(1, 0, 0);

        public override string Author => "ICC开发团队";

        public override bool IsBuiltIn => true;

        public override void OnStartup()
        {
            base.OnStartup();

            // 加载插件配置
            _isDrawingMode = PluginConfigurationManager.GetConfiguration(Name, "IsDrawingMode", false);

            // 注册事件处理器
            PluginService.RegisterEventHandler("CanvasCleared", OnCanvasCleared);
            PluginService.RegisterEventHandler("PageChanged", OnPageChanged);

            LogHelper.WriteLogToFile($"示例插件已启动，绘制模式: {_isDrawingMode}");
        }

        public override void OnShutdown()
        {
            // 注销事件处理器
            PluginService.UnregisterEventHandler("CanvasCleared", OnCanvasCleared);
            PluginService.UnregisterEventHandler("PageChanged", OnPageChanged);

            base.OnShutdown();
        }

        public override MenuItem[] GetMenuItems()
        {
            if (_menuItem == null)
            {
                _menuItem = new MenuItem
                {
                    Header = "示例插件",
                    Icon = new TextBlock { Text = "🎨", FontSize = 16 }
                };

                var toggleDrawingModeItem = new MenuItem
                {
                    Header = "切换绘制模式",
                    Icon = new TextBlock { Text = "✏️", FontSize = 16 }
                };
                toggleDrawingModeItem.Click += (s, e) => ToggleDrawingMode();

                var clearCanvasItem = new MenuItem
                {
                    Header = "清除画布",
                    Icon = new TextBlock { Text = "🗑️", FontSize = 16 }
                };
                clearCanvasItem.Click += (s, e) => ClearCanvas();

                var addShapeItem = new MenuItem
                {
                    Header = "添加形状",
                    Icon = new TextBlock { Text = "🔷", FontSize = 16 }
                };
                addShapeItem.Click += (s, e) => AddShape();

                var showInfoItem = new MenuItem
                {
                    Header = "显示信息",
                    Icon = new TextBlock { Text = "ℹ️", FontSize = 16 }
                };
                showInfoItem.Click += (s, e) => ShowInfo();

                _menuItem.Items.Add(toggleDrawingModeItem);
                _menuItem.Items.Add(clearCanvasItem);
                _menuItem.Items.Add(addShapeItem);
                _menuItem.Items.Add(new Separator());
                _menuItem.Items.Add(showInfoItem);
            }

            return new[] { _menuItem };
        }

        public override Button[] GetToolbarButtons()
        {
            if (_toolbarButton == null)
            {
                _toolbarButton = new Button
                {
                    Content = "🎨",
                    ToolTip = "示例插件 - 点击切换绘制模式",
                    Width = 32,
                    Height = 32,
                    Margin = new Thickness(2),
                    Background = new SolidColorBrush(Colors.Transparent),
                    BorderBrush = new SolidColorBrush(Colors.Gray),
                    BorderThickness = new Thickness(1)
                };

                _toolbarButton.Click += (s, e) => ToggleDrawingMode();
            }

            return new[] { _toolbarButton };
        }

        public override string GetStatusBarInfo()
        {
            return $"示例插件 - 绘制模式: {(_isDrawingMode ? "开启" : "关闭")} | 当前页面: {PluginService.CurrentPageIndex + 1}/{PluginService.TotalPageCount}";
        }

        public override void OnConfigurationChanged()
        {
            base.OnConfigurationChanged();

            // 更新UI状态
            UpdateUIState();
        }

        private void ToggleDrawingMode()
        {
            _isDrawingMode = !_isDrawingMode;

            // 保存配置
            PluginConfigurationManager.SetConfiguration(Name, "IsDrawingMode", _isDrawingMode);

            // 更新UI状态
            UpdateUIState();

            // 显示通知
            PluginService.ShowNotification($"绘制模式已{( _isDrawingMode ? "开启" : "关闭")}", NotificationType.Info);

            LogHelper.WriteLogToFile($"示例插件绘制模式已切换为: {(_isDrawingMode ? "开启" : "关闭")}");
        }

        private void ClearCanvas()
        {
            if (PluginService.ShowConfirmDialog("确定要清除当前画布吗？", "确认清除"))
            {
                PluginService.ClearCanvas();
                PluginService.ShowNotification("画布已清除", NotificationType.Success);
                LogHelper.WriteLogToFile("示例插件已清除画布");
            }
        }

        private void AddShape()
        {
            try
            {
                var canvas = PluginService.CurrentCanvas;
                if (canvas != null)
                {
                    // 创建一个简单的矩形
                    var rectangle = new Rectangle
                    {
                        Width = 100,
                        Height = 60,
                        Fill = new SolidColorBrush(Colors.LightBlue),
                        Stroke = new SolidColorBrush(Colors.Blue),
                        StrokeThickness = 2
                    };

                    // 设置位置（居中）
                    Canvas.SetLeft(rectangle, (canvas.ActualWidth - rectangle.Width) / 2);
                    Canvas.SetTop(rectangle, (canvas.ActualHeight - rectangle.Height) / 2);

                    // 添加到画布
                    canvas.Children.Add(rectangle);

                    PluginService.ShowNotification("已添加形状", NotificationType.Success);
                    LogHelper.WriteLogToFile("示例插件已添加形状到画布");
                }
            }
            catch (Exception ex)
            {
                PluginService.ShowNotification($"添加形状失败: {ex.Message}", NotificationType.Error);
                LogHelper.WriteLogToFile($"示例插件添加形状时出错: {ex.Message}", LogHelper.LogType.Error);
            }
        }

        private void ShowInfo()
        {
            var info = $"示例插件信息:\n" +
                      $"名称: {Name}\n" +
                      $"版本: {Version}\n" +
                      $"作者: {Author}\n" +
                      $"状态: {(IsEnabled ? "已启用" : "已禁用")}\n" +
                      $"绘制模式: {(_isDrawingMode ? "开启" : "关闭")}\n" +
                      $"当前页面: {PluginService.CurrentPageIndex + 1}/{PluginService.TotalPageCount}\n" +
                      $"画布数量: {PluginService.TotalPageCount}\n" +
                      $"主题: {(PluginService.IsDarkTheme ? "深色" : "浅色")}\n" +
                      $"模式: {(PluginService.IsWhiteboardMode ? "白板" : "画板")}";

            PluginService.ShowNotification(info, NotificationType.Info);
        }

        private void UpdateUIState()
        {
            try
            {
                if (_toolbarButton != null)
                {
                    _toolbarButton.Background = _isDrawingMode 
                        ? new SolidColorBrush(Colors.LightGreen) 
                        : new SolidColorBrush(Colors.Transparent);
                }

                if (_menuItem != null)
                {
                    var toggleItem = _menuItem.Items[0] as MenuItem;
                    if (toggleItem != null)
                    {
                        toggleItem.Header = $"绘制模式: {(_isDrawingMode ? "开启" : "关闭")}";
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogToFile($"示例插件更新UI状态时出错: {ex.Message}", LogHelper.LogType.Error);
            }
        }

        private void OnCanvasCleared(object sender, EventArgs e)
        {
            LogHelper.WriteLogToFile("示例插件收到画布清除事件");
            
            // 可以在这里执行一些清理工作
            if (_isDrawingMode)
            {
                PluginService.ShowNotification("画布已清除，绘制模式已自动关闭", NotificationType.Warning);
                _isDrawingMode = false;
                PluginConfigurationManager.SetConfiguration(Name, "IsDrawingMode", false);
                UpdateUIState();
            }
        }

        private void OnPageChanged(object sender, EventArgs e)
        {
            LogHelper.WriteLogToFile($"示例插件收到页面变更事件，当前页面: {PluginService.CurrentPageIndex + 1}");
            
            // 可以在这里执行一些页面相关的操作
            if (_isDrawingMode)
            {
                PluginService.ShowNotification($"已切换到第 {PluginService.CurrentPageIndex + 1} 页", NotificationType.Info);
            }
        }
    }
} 
