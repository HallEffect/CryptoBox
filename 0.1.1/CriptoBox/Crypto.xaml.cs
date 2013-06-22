using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ApplicationSettings;


// Документацию по шаблону элемента "Основная страница" см. по адресу http://go.microsoft.com/fwlink/?LinkId=234237

namespace CriptoBox
{
    /// <summary>
    /// Основная страница, которая обеспечивает характеристики, являющимися общими для большинства приложений.
    /// </summary>
    public sealed partial class BasicPage1 : CriptoBox.Common.LayoutAwarePage
    {
        public BasicPage1()
        {
            this.InitializeComponent();

            if (Window.Current.Bounds.Width == 1024)
            {
                InputTextTextbox.Width = 350;
                IVInputTextTextbox.Width = 350;
                KeyInputTextTextbox.Width = 350;
                OutputTextTextbox.Width = 350;

                InputTextTextbox.Height = Window.Current.Bounds.Height / 10 + 23;           
                OutputTextTextbox.Height = Window.Current.Bounds.Height / 10 + 23;
            }
            else
            {
                InputTextTextbox.Width = Window.Current.Bounds.Width / 2 + 38;
                IVInputTextTextbox.Width = Window.Current.Bounds.Width / 2 + 38;
                KeyInputTextTextbox.Width = Window.Current.Bounds.Width / 2 + 38;
                OutputTextTextbox.Width = Window.Current.Bounds.Width / 2 + 38;

                InputTextTextbox.Height = Window.Current.Bounds.Height / 10 + 23;              
                OutputTextTextbox.Height = Window.Current.Bounds.Height / 10 + 23;
            }
        }

       

        /// <summary>
        /// Шифрование/дешифрование
        /// </summary>
        private void EncryptDecryptButton_Click(object sender, RoutedEventArgs e)
        {
        string AlgName = null;

        CopyEncText.Visibility = Visibility.Visible;
        CopyKeyText.Visibility = Visibility.Visible;


    // ################################# Шифрование ################################
        if (EncryptionModeRadioButton.IsChecked==true)
        {
            if (CBCRadioButton.IsChecked == true)
            {
                AlgName = AlgNameCombobox.SelectionBoxItem.ToString() + "_CBC_PKCS7";
                CopyIVText.Visibility = Visibility.Visible;
            }
            else
            {
                AlgName = AlgNameCombobox.SelectionBoxItem.ToString() + "_ECB_PKCS7";
            }

            try
            {
                Cryptobox EncObject = new Cryptobox();
                if (AlgName.Contains("RC4"))
                {
                    AlgName = "RC4";
                }
                OutputTextTextbox.Text = EncObject.EncryptMode(InputTextTextbox.Text, KeyInputTextTextbox.Text, AlgName, IVInputTextTextbox.Text, "256");//KeySizeCombobox.SelectionBoxItem.ToString());
                AlgName = null;
             }
             catch (Exception ex)
             {
               string ErrorMessage = ex.Message;

              var DialogMessage = new MessageDialog("Ой");

               if (ErrorMessage.Contains("Value does not fall"))
                {
                 DialogMessage = new MessageDialog("Указанная длина ключа не подходит для алгоритма", "Ой");

                }
               DialogMessage.ShowAsync();
                
             }
        }

    // ################################# Дешифрование ################################
        else
        {
            if (CBCRadioButton.IsChecked == true)
            {
                AlgName = AlgNameCombobox.SelectionBoxItem.ToString() + "_CBC_PKCS7";
                CopyIVText.Visibility = Visibility.Visible;
            }
            else
            {
                AlgName = AlgNameCombobox.SelectionBoxItem.ToString() + "_ECB_PKCS7";
            }

            try
            {
                Cryptobox DecObject = new Cryptobox();
                if (AlgName.Contains("RC4"))
                {
                    AlgName = "RC4";
                }

                OutputTextTextbox.Text = DecObject.DecrypMode(InputTextTextbox.Text, KeyInputTextTextbox.Text, AlgName, IVInputTextTextbox.Text, "256");//;KeySizeCombobox.SelectionBoxItem.ToString());
                AlgName = null;
                GC.Collect();
        
            }
            catch (Exception exception)
            {
                string ErrorMessage=exception.Message;
                // ################################# Обработчики ошибок дешифрования ################################
                var DialogMessage=new MessageDialog("Ой");

                if (ErrorMessage.Contains("0x80070017"))
                {
                    DialogMessage = new MessageDialog("Неправильный ключ", "Ой");
            
                }
                else if (ErrorMessage.Contains("Плохие данные"))
                {
                    DialogMessage = new MessageDialog("Вы пытаетесь расшифровать не зашифрованный текст", "Ой");
            
                }
                else if (ErrorMessage.Contains("буфер не подходит"))
                {
                    DialogMessage = new MessageDialog("Введите вектор инициализации IV", "Ой");
            
                }
                else if (ErrorMessage.Contains("Value does not fall"))
                {
                    DialogMessage = new MessageDialog("Указанная длина ключа не подходит для алгоритма", "Ой");

                }

                else
                {
                    DialogMessage = new MessageDialog(ErrorMessage, "Ой");
         
                }
                DialogMessage.ShowAsync();
        
                ////////////////////////////////////////////////////////////////////
            }
        }
            
            

            
        }
        // ################################# Генерирование ключа ################################
        private void KeyCheckButton_Checked(object sender, RoutedEventArgs e)
        {
            Cryptobox KeyRandObject = new Cryptobox();
            KeyInputTextTextbox.Text = KeyRandObject.Random(UInt32.Parse("128"));//KeySizeCombobox.SelectionBoxItem.ToString()));
        }


        private void KeyCheckButton_UnChecked(object sender, RoutedEventArgs e)
        {
            KeyInputTextTextbox.Text = "Key";
        }

        // ################################# Отображение IV ################################
        private void CBCRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            IVCheckbox.Visibility = Visibility.Visible;
            IVInputTextTextbox.Visibility = Visibility.Visible;
        }


        private void CBCRadioButton_UnChecked(object sender, RoutedEventArgs e)
        {
            IVCheckbox.Visibility = Visibility.Collapsed;
            IVInputTextTextbox.Visibility = Visibility.Collapsed;
        }


        // ################################# Генерирование IV ################################
        private void IVCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Cryptobox IVRandObject = new Cryptobox();
            IVInputTextTextbox.Text = IVRandObject.Random(128);
        }

        private void IVCheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            IVInputTextTextbox.Text = "IV";
        }

        // ################################# Обозначение ################################
        private void DecryptMode_Checked(object sender, RoutedEventArgs e)
        {
            EncryptDecryptButton.Content = "Дешифровать";
            InputTextBlock.Text = "Зашифрованный текст";
            OutputTextBlock.Text = "Исходный текст";
            pageTitle.Text = "Дешифрование";
            CopyEncText.Visibility = Visibility.Collapsed;
            CopyKeyText.Visibility = Visibility.Collapsed;
            CopyIVText.Visibility = Visibility.Collapsed;
            InputTextTextbox.Text = "Encrypted text";
            OutputTextTextbox.Text = "Source text";
        }

        private void DecryptMode_UnChecked(object sender, RoutedEventArgs e)
        {
            EncryptDecryptButton.Content = "Зашифровать";
            InputTextBlock.Text = "Исходный текст";
            OutputTextBlock.Text = "Зашифрованный текст";
            pageTitle.Text = "Шифрование";
            InputTextTextbox.Text = "Source text";
            OutputTextTextbox.Text = "Encrypted text";
        }


        // ################################# Навигация ################################
        private void NavigateToEncryptButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BasicPage1));
        }


        private void NavigateToHashButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BasicPage2));
        }


        private void NavigateToGeneratorButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BasicPage3));
        }

        private void size(object sender, SizeChangedEventArgs e)
        {
           // InputTextBlock.Text = "Изменился";
        }

        private void CoptTextToClipboardButton_Click(object sender, RoutedEventArgs e)
        {
            DataPackage DataToClipboarbObject=new DataPackage();
            DataToClipboarbObject.SetText(OutputTextTextbox.Text);
            Clipboard.SetContent(DataToClipboarbObject);
        }

        private void CoptKeyToClipboardButton_Click(object sender, RoutedEventArgs e)
        {
            DataPackage DataToClipboarbObject = new DataPackage();
            DataToClipboarbObject.SetText(IVInputTextTextbox.Text);
            Clipboard.SetContent(DataToClipboarbObject);
        }

        private void CoptIVToClipboardButton_Click(object sender, RoutedEventArgs e)
        {
            DataPackage DataToClipboarbObject = new DataPackage();
            DataToClipboarbObject.SetText(KeyInputTextTextbox.Text);
            Clipboard.SetContent(DataToClipboarbObject);
        }

        /*
        private void AlgorithmSelected2(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            string AlgNameSelected = AlgNameCombobox.SelectionBoxItem.ToString();
            if (AlgNameSelected.Contains("AES"))
            {
                S56.Visibility = Visibility.Collapsed;
                S64.Visibility = Visibility.Collapsed;
                S168.Visibility = Visibility.Collapsed;
                S512.Visibility = Visibility.Collapsed;
                S128.Visibility = Visibility.Visible;
                S128.IsSelected = true;
                S192.Visibility = Visibility.Visible;
                S256.Visibility = Visibility.Visible;

            }
            else if (AlgNameSelected.Contains("DES"))
            {
                S56.Visibility = Visibility.Visible;
                S56.IsSelected = true;
                S64.Visibility = Visibility.Collapsed;
                S168.Visibility = Visibility.Collapsed;
                S512.Visibility = Visibility.Collapsed;

                S128.Visibility = Visibility.Collapsed;
                S192.Visibility = Visibility.Collapsed;
                S256.Visibility = Visibility.Collapsed;
            }
            else if (AlgNameSelected.Contains("3DES"))
            {
                S56.Visibility = Visibility.Visible;
                S56.IsSelected = true;
                S64.Visibility = Visibility.Collapsed;
                S168.Visibility = Visibility.Visible;
                S512.Visibility = Visibility.Collapsed;

                S128.Visibility = Visibility.Collapsed;
                S192.Visibility = Visibility.Collapsed;
                S256.Visibility = Visibility.Collapsed;
            }

            else if ((AlgNameSelected.Contains("RC4")) || (AlgNameSelected.Contains("RC2")) )
            {
                S56.Visibility = Visibility.Collapsed;
                //S56.IsSelected = true;
                S64.Visibility = Visibility.Collapsed;
                S168.Visibility = Visibility.Collapsed;
                S512.Visibility = Visibility.Collapsed;

                S128.Visibility = Visibility.Collapsed;
                S192.Visibility = Visibility.Collapsed;
                S256.Visibility = Visibility.Collapsed;
            }

        } */       
    }
}
