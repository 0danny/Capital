using ModernWpf.Controls;
using System;
using System.Windows;

namespace Capital
{
    public partial class CustomDialog
    {
        public CustomDialog(string title, string content)
        {
            InitializeComponent();
            Title = title;
            boxTextBlock.Text = content;
        }

        public CustomDialog(string title, string content, bool yesno)
        {
            InitializeComponent();
            Title = title;
            boxTextBlock.Text = content;
            PrimaryButtonText = "Yes";
            SecondaryButtonText = "No";
        }

        private void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }
}
