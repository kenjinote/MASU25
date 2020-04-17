using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace MASU25
{
    /// <summary>
    /// countdown.xaml の相互作用ロジック
    /// </summary>
    public partial class countdown : Page
    {
        private int count_down;
        private Brush brush3;
        private Brush brush2;
        private Brush brush1;
        // タイマのインスタンス
        private DispatcherTimer _timer1;
        private DispatcherTimer _timer2;
        public countdown()
        {
            InitializeComponent();

            brush3 = label3.Background;
            brush2 = label2.Background;
            brush1 = label1.Background;

            label3.Background = 
            label2.Background =
            label1.Background = Brushes.Black;

            count_down = 0;

            // ページ破棄のイベントを設定
            this.Unloaded += Page1_Unloaded;

            // タイマのインスタンスを生成
            _timer1 = new DispatcherTimer();
            // インターバルを設定
            _timer1.Interval = new TimeSpan(0, 0, 1);
            // タイマメソッドを設定
            _timer1.Tick += new EventHandler(Timer1_Tick);
            // タイマを開始
            _timer1.Start();
        }

        // タイマメソッド
        private void Timer1_Tick(object sender, EventArgs e)
        {
            switch (count_down)
            {
                case 0:
                    new SoundPlayer(Properties.Resources.CBC1).Play();
                    label3.Background = brush3;
                    label2.Background = Brushes.Black;
                    label1.Background = Brushes.Black;
                    break;
                case 1:
                    new SoundPlayer(Properties.Resources.CBC1).Play();
                    label3.Background = Brushes.Black;
                    label2.Background = brush2;
                    label1.Background = Brushes.Black;
                    break;
                case 2:
                    new SoundPlayer(Properties.Resources.CBC1).Play();
                    label3.Background = Brushes.Black;
                    label2.Background = Brushes.Black;
                    label1.Background = brush1;
                    break;
                case 3:
                    // タイマをストップ
                    _timer1.Stop();
                    // タイマのインスタンスを生成
                    _timer2 = new DispatcherTimer();
                    // インターバルを設定
                    _timer2.Interval = TimeSpan.FromMilliseconds(500);
                    // タイマメソッドを設定
                    _timer2.Tick += new EventHandler(Timer2_Tick);
                    // タイマを開始
                    _timer2.Start();
                    break;
            }

            count_down++;

        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            _timer2.Stop();

            // 問題ページに遷移
            NavigationService.Navigate(new mondai_time());
        }

        void Page1_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_timer1 != null)
            {
                _timer1.Stop();
            }
            if (_timer2 != null)
            {
                _timer2.Stop();
            }
            Console.WriteLine("{0} is unloaded", this.Title);
        }

    }

}
