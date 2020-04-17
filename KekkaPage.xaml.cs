using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;

namespace MASU25
{
    /// <summary>
    /// KekkaPage.xaml の相互作用ロジック
    /// </summary>
    public partial class KekkaPage : Page
    {
        public KekkaPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            kekkatext.Text = "";
            kekkatext.Inlines.Add("あなたが１分間に解いた、");
            kekkatext.Inlines.Add(new Run(Environment.NewLine));
            switch (mainWindow.course_check)
            {
                case 5: // 足し算
                    kekkatext.Inlines.Add("たしざんは");
                    break;
                case 6: // 引き算
                    kekkatext.Inlines.Add("ひきざんは");
                    break;
                case 7: // 掛け算
                    kekkatext.Inlines.Add("かけざんは");
                    break;
                case 8: // 割り算
                    kekkatext.Inlines.Add("わりざんは");
                    break;
            }
            kekkatext.Inlines.Add(new Run(mainWindow.OKCount.ToString("　0　")) { FontSize = 36, Foreground = Brushes.Red });
            kekkatext.Inlines.Add("問です");
        }

        // さいしょにもどるボタンをクリックしたとき
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                // おわりページ
                NavigationService.RemoveBackEntry();
                // 問題ページ
                NavigationService.RemoveBackEntry();
                // カウントダウンページ
                NavigationService.RemoveBackEntry();
                // 開始ページ
                NavigationService.RemoveBackEntry();
                NavigationService.GoBack();
            }
        }
    }
}
