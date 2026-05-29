using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace cybersecurity_chatbot_p2
{//start of namespace
    public class message_displayer
    {//start of class
        public void AddMessage(ListBox chats, string sender, string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Border messageBorder = new Border
                {
                    Margin = new Thickness(5, 3, 5, 3),
                    Padding = new Thickness(10, 8, 10, 8),
                    CornerRadius = new CornerRadius(10)
                };
                
                bool isBot = sender.Contains("CyberGuard");
                
                if (isBot)
                {
                    messageBorder.Background = new SolidColorBrush(Color.FromRgb(52, 152, 219));
                    messageBorder.HorizontalAlignment = HorizontalAlignment.Left;
                    messageBorder.MaxWidth = 450;
                }
                else
                {
                    messageBorder.Background = new SolidColorBrush(Color.FromRgb(46, 204, 113));
                    messageBorder.HorizontalAlignment = HorizontalAlignment.Right;
                    messageBorder.MaxWidth = 450;
                }
                
                TextBlock messageText = new TextBlock
                {
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 14,
                    Foreground = Brushes.White
                };
                
                messageText.Text = $"{sender}: {message}";
                messageBorder.Child = messageText;
                
                chats.Items.Add(messageBorder);
                chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
            });
        }//end of method
    }//end of class
}//end of namespace