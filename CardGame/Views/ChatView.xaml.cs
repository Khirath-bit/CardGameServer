﻿using System;
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
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : UserControl
    {
        public ChatView()
        {
            InitializeComponent();
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && DataContext is ChatViewViewModel vm)
            {
                vm.SendMessage();
                messageinput.Text = "";
            }

        }
    }
}
