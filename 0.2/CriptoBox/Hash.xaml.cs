using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Navigation;


// Документацию по шаблону элемента "Основная страница" см. по адресу http://go.microsoft.com/fwlink/?LinkId=234237

namespace CriptoBox
{
    /// <summary>
    /// Основная страница, которая обеспечивает характеристики, являющимися общими для большинства приложений.
    /// </summary>
    public sealed partial class BasicPage2 : CriptoBox.Common.LayoutAwarePage
    {
        public BasicPage2()
        {
            this.InitializeComponent();

            if (Window.Current.Bounds.Width == 1024)
            {
                InputTextTextbox.Width = 300;              
                OutputTextTextbox.Width = 300;

                InputTextTextbox.Height = Window.Current.Bounds.Height / 10 + 23;
                
            }
            else
            {
                InputTextTextbox.Width = Window.Current.Bounds.Width / 2-70;                
                OutputTextTextbox.Width = Window.Current.Bounds.Width / 2-70;

                InputTextTextbox.Height = Window.Current.Bounds.Height / 10 + 23;               
            }

        }

        private void HashButton_Click(object sender, RoutedEventArgs e)
        {
            CopyHashButton.Visibility = Visibility.Visible;
            
            Cryptobox HashObject=new Cryptobox();
            OutputTextTextbox.Text = HashObject.HashMode(InputTextTextbox.Text, AlgNameCombobox.SelectionBoxItem.ToString());
        }

        private void NavigateToHashButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BasicPage2));
        }

        private void NavigateToEncryptButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BasicPage1));
        }

        private void NavigateToGeneratorButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BasicPage3));
        }

        private void CopyHashButton_Click(object sender, RoutedEventArgs e)
        {
            DataPackage DataToClipboarbObject = new DataPackage();
            DataToClipboarbObject.SetText(OutputTextTextbox.Text);
            Clipboard.SetContent(DataToClipboarbObject);
        }
    }
}
