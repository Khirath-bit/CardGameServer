using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardGame.Views
{
    /// <summary>
    /// Interaction logic for ChatMessageView.xaml
    /// </summary>
    public partial class ChatMessageView : UserControl
    {
        public ChatMessageView()
        {
            InitializeComponent();

            DateTime.Text = System.DateTime.Now.ToShortTimeString();
        }

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        //  Using a DependencyProperty as the backing store for Percentage.  This 
        //  enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string),
                typeof(ChatMessageView), new PropertyMetadata(""));
    }
}
