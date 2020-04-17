using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace MASU25
{
    /// <summary>
    /// course_select.xaml の相互作用ロジック
    /// </summary>
    public partial class course_select : Page
    {
        private MainWindow mainWindow;
        public course_select()
        {
            InitializeComponent();

            mainWindow = (MainWindow)Application.Current.MainWindow;
        }

        private void exitbutton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomMsgBox.Show(Window.GetWindow(this), "２５マスけいさんをおわりにしますか？", "２５マス計算ソフト", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void beginbutton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SecondPage());
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            if (radio != null)
            {
                if (radio.Tag != null)
                {
                    // ラジオのtagを取得
                    int radio_num = int.Parse(radio.Tag.ToString());
                    mainWindow.course_check = radio_num;
                    var text = this.FindName("text" + radio_num) as TextBlock;
                    if (text != null)
                    {
                        text.Background = new SolidColorBrush(Color.FromRgb(255, 255, 128));
                    }
                }
            }
        }

        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            if (radio != null)
            {
                if (radio.Tag != null)
                {
                    // ラジオのtagを取得
                    int radio_num = int.Parse(radio.Tag.ToString());
                    var text = this.FindName("text" + radio_num) as TextBlock;
                    if (text != null)
                    {
                        text.Background = Brushes.White;
                    }
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var radio = this.FindName("radio" + mainWindow.course_check) as RadioButton;
            if (radio != null)
            {
                radio.IsChecked = true;
            }
        }
    }
}
