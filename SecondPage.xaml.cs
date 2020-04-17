using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MASU25
{
    /// <summary>
    /// SecondPage.xaml の相互作用ロジック
    /// </summary>
    public partial class SecondPage : Page
    {
        private MainWindow mainWindow;
        public SecondPage()
        {
            InitializeComponent();

            mainWindow = (MainWindow)Application.Current.MainWindow;
        }

        private void startbuttton_Click(object sender, RoutedEventArgs e)
        {
            // カウントダウンページに遷移
            NavigationService.Navigate(new countdown());
        }

        private void backbutton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            switch (mainWindow.course_check)
            {
                case 5: // 足し算
                    label.Content = "たしざん";
                    break;
                case 6: // 引き算
                    label.Content = "ひきざん";
                    break;
                case 7: // 掛け算
                    label.Content = "かけざん";
                    break;
                case 8: // 割り算
                    label.Content = "わりざん";
                    break;
            }
            
        }
    }
}
