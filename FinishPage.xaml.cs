using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace MASU25
{
    /// <summary>
    /// FinishPage.xaml の相互作用ロジック
    /// </summary>
    public partial class FinishPage : Page
    {
        // タイマのインスタンス
        private DispatcherTimer _timer1;

        public FinishPage()
        {
            InitializeComponent();

            // タイマのインスタンスを生成
            _timer1 = new DispatcherTimer();
            // インターバルを設定
            _timer1.Interval = new TimeSpan(0, 0, 2);
            // タイマメソッドを設定
            _timer1.Tick += new EventHandler(Timer1_Tick);
            // タイマを開始
            _timer1.Start();
        }

        // タイマメソッド
        private void Timer1_Tick(object sender, EventArgs e)
        {
            _timer1.Stop();

            // 結果ページに遷移する
            NavigationService.Navigate(new KekkaPage());
        }
    }
}
