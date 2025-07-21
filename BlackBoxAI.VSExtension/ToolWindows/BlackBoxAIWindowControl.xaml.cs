using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using BlackBoxAI.VSExtension.Services;

namespace BlackBoxAI.VSExtension.ToolWindows
{
    public partial class BlackBoxAIWindowControl : UserControl
    {
        private readonly AIService aiService;
        private readonly SettingsService settingsService;

        public BlackBoxAIWindowControl()
        {
            InitializeComponent();
            aiService = new AIService();
            settingsService = new SettingsService();
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await SendMessage();
        }

        private async void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                await SendMessage();
                e.Handled = true;
            }
        }

        private async Task SendMessage()
        {
            string message = InputTextBox.Text.Trim();
            if (string.IsNullOrEmpty(message))
                return;

            AddMessage(message, true);
            InputTextBox.Clear();

            try
            {
                string response = await aiService.SendMessageAsync(message);
                AddMessage(response, false);
            }
            catch (Exception ex)
            {
                AddMessage($"Error: {ex.Message}", false);
            }
        }

        private void AddMessage(string message, bool isUser)
        {
            var textBlock = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 4, 0, 4),
                Padding = new Thickness(8),
                Background = isUser ? 
                    SystemColors.HighlightBrush : 
                    new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                Foreground = isUser ? 
                    SystemColors.HighlightTextBrush : 
                    SystemColors.ControlTextBrush,
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                MaxWidth = 300
            };

            ChatPanel.Children.Add(textBlock);
            ChatScrollViewer.ScrollToEnd();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(settingsService);
            settingsWindow.ShowDialog();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Children.Clear();
            AddMessage("Chat cleared. How can I help you?", false);
        }
    }

    public class SettingsWindow : Window
    {
        private readonly SettingsService settingsService;

        public SettingsWindow(SettingsService settingsService)
        {
            this.settingsService = settingsService;
            Title = "BlackBox AI Settings";
            Width = 400;
            Height = 200;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            var stackPanel = new StackPanel { Margin = new Thickness(10) };
            
            var apiKeyLabel = new Label { Content = "API Key:" };
            var apiKeyTextBox = new TextBox 
            { 
                Text = settingsService.GetApiKey(),
                Margin = new Thickness(0, 0, 0, 10)
            };
            
            var saveButton = new Button 
            { 
                Content = "Save", 
                Width = 80,
                Margin = new Thickness(0, 10, 0, 0)
            };
            saveButton.Click += (s, e) =>
            {
                settingsService.SetApiKey(apiKeyTextBox.Text);
                Close();
            };

            stackPanel.Children.Add(apiKeyLabel);
            stackPanel.Children.Add(apiKeyTextBox);
            stackPanel.Children.Add(saveButton);

            Content = stackPanel;
        }
    }
}
