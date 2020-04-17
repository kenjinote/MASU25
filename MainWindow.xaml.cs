using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace MASU25
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        // 計算のコース
        public int course_check = 5;
        // 正解数
        public int OKCount = 0;
        public MainWindow()
        {
            InitializeComponent();

            // デバッグの利便性のためデバッグ実行時は最大表示と最前面表示を解除する
#if DEBUG
            WindowState = System.Windows.WindowState.Normal;
            WindowStyle = System.Windows.WindowStyle.ThreeDBorderWindow;
            Topmost = false;
#endif

            // 上部のナビゲーションバーを非表示にする
            ShowsNavigationUI = false;

            // 画面遷移ショートカットを無効にする
            NavigationCommands.Refresh.InputGestures.Clear();
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();

            // コース選択ページに遷移
            NavigationService.Navigate(new course_select());
        }

        // アプリケーションを閉じようとしたときの処理
        private void NavigationWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CustomMsgBox.Show(Window.GetWindow(this), "２５マスけいさんをおわりにしますか？", "２５マス計算ソフト", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
