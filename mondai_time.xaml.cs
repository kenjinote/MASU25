using System;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace MASU25
{
    /// <summary>
    /// mondai_time.xaml の相互作用ロジック
    /// </summary>
    public partial class mondai_time : Page
    {
        private DispatcherTimer _timer1; // タイマー変数
        private Window window; // キーイベント登録・解除用
        private int TimeCount; // 現在の秒数
        private const int LimitSec = 60; // 問題時間は1分間（60秒）
        private cell ActiveCell; // 現在の入力セル
        private short[] yoko = new short[25]; // 割り算モード時に参照されるためメンバ変数に設定
        private MainWindow mainWindow; // メインウィンドウ（正解数の更新や計算モードを取得するため）

        public mondai_time()
        {
            InitializeComponent();

            mainWindow = (MainWindow)Application.Current.MainWindow;
        }

        // --------------------------＜ランダムで問題を生成する＞------------------
        private void Question()
        {
            Random rand = new System.Random();
            // ＜問題生成＞

            // 縦(列)問題作成のための初期化        
            short[] tate = new short[5];
            short tmp_tate = 0;
            bool flag = true;
            short jj = 0;
            short tt = 0;

            // 縦(列)問題作成
            while (tt < 5)
            {
                tmp_tate = (short)((rand.NextDouble() * 8.5) + 1);
                for (jj = 0; jj <= tt; jj++)
                {
                    if (tmp_tate == tate[jj])
                        flag = false;
                    if ((mainWindow.course_check == 7 || mainWindow.course_check == 8) && tmp_tate == 1)
                        flag = false;
                }
                if (tt != 0)
                {
                    if ((tmp_tate == tate[tt - 1] - 1 || tmp_tate == tate[tt - 1] + 1))
                        flag = false;
                }
                if (flag == true)
                {
                    tate[tt] = tmp_tate;
                    tt++;
                }
                flag = true;
            }

            // 横(行)問題作成のための初期化
            short tmp_yoko = 0;
            short yy;
            jj = 0;
            tmp_yoko = 0;
            flag = true;

            if (mainWindow.course_check == 5 || mainWindow.course_check == 7) // 足し算、掛け算
            {
                // 横(行)問題作成
                yy = 0;
                while (yy < 5)
                {
                    tmp_yoko = (short)((rand.NextDouble() * 8.5) + 1);
                    for (jj = 0; jj <= yy; jj++)
                    {
                        if (tmp_yoko == yoko[jj])
                            flag = false;
                        if (mainWindow.course_check == 7 && tmp_yoko == 1)
                            flag = false;
                    }
                    if (yy != 0)
                    {
                        if (tmp_yoko == yoko[yy - 1] - 1 || tmp_yoko == yoko[yy - 1] + 1)
                            flag = false;
                    }

                    if (flag == true)
                    {
                        yoko[yy] = tmp_yoko;
                        yy++;
                    }
                    flag = true;
                }
            }
            else if (mainWindow.course_check == 6) // 引き算
            {
                // ＜引き算のときは、１０～１９までの数を生成＞
                yy = 0;
                while (yy < 5)
                {
                    tmp_yoko = (short)(rand.NextDouble() * 10 + 10);
                    for (jj = 0; jj <= yy; jj++)
                    {
                        if (tmp_yoko == yoko[jj])
                            flag = false;
                    }
                    if (yy != 0)
                    {
                        if (tmp_yoko == yoko[yy - 1] - 1 || tmp_yoko == yoko[yy - 1] + 1)
                            flag = false;
                    }
                    if (flag == true)
                    {
                        yoko[yy] = tmp_yoko;
                        yy++;
                    }
                    flag = true;
                }
            }
            else if (mainWindow.course_check == 8) // 割り算
            {
                // 横(行)問題作成
                for (int ii = 0; ii < 5; ii++)
                {
                    yy = 0;
                    while (yy < 5)
                    {
                        tmp_yoko = (short)((rand.NextDouble() * 8.5) + 1);
                        for (jj = 0; jj <= yy; jj++)
                        {
                            if (tmp_yoko == yoko[ii * 5 + jj])
                                flag = false;
                        }

                        if (yy != 0)
                        {
                            if ((tmp_yoko == yoko[ii * 5 + yy - 1] - 1 | tmp_yoko == yoko[ii * 5 + yy - 1] + 1))
                                flag = false;
                        }

                        if (flag == true)
                        {
                            yoko[ii * 5 + yy] = tmp_yoko;
                            yy++;
                        }

                        flag = true;
                    }
                }

                // <行問題生成>
                for (short f = 0; f < 5; f++)
                {
                    yoko[0 + f] = (short)(yoko[0 + f] * tate[0]);
                    yoko[5 + f] = (short)(yoko[5 + f] * tate[1]);
                    yoko[10 + f] = (short)(yoko[10 + f] * tate[2]);
                    yoko[15 + f] = (short)(yoko[15 + f] * tate[3]);
                    yoko[20 + f] = (short)(yoko[20 + f] * tate[4]);
                }
            }

            // 問題の表示
            yoko1.Content = yoko[0];
            yoko2.Content = yoko[1];
            yoko3.Content = yoko[2];
            yoko4.Content = yoko[3];
            yoko5.Content = yoko[4];
            tate1.Content = tate[0];
            tate2.Content = tate[1];
            tate3.Content = tate[2];
            tate4.Content = tate[3];
            tate5.Content = tate[4];

            // セルに解答の設定する
            for (int ii = 0; ii < 25; ii++)
            {
                cell c = FindName("cell" + (ii + 1)) as cell;
                if (c != null)
                {
                    switch (mainWindow.course_check)
                    {
                        case 5: // 足し算
                            c.anser = yoko[ii % 5] + tate[ii / 5];
                            break;
                        case 6: // 引き算
                            c.anser = yoko[ii % 5] - tate[ii / 5];
                            break;
                        case 7: // 掛け算
                            c.anser = yoko[ii % 5] * tate[ii / 5];
                            break;
                        case 8: // 割り算
                            c.anser = yoko[ii] / tate[ii / 5];
                            break;
                    }
                }
            }
        }

        // タイマメソッド
        private void Timer1_Tick(object sender, EventArgs e)
        {
            TimeCount++;

            timelabel.Content = (LimitSec - TimeCount).ToString() + "秒";

            //＜１分経過したら終了する＞
            if (TimeCount == LimitSec)
            {
                _timer1.Stop();

                // おわりページに遷移する
                NavigationService.Navigate(new FinishPage());               
            }
        }

        private void backbutton_Click(object sender, RoutedEventArgs e)
        {
            if (_timer1 != null && _timer1.IsEnabled)
            {
                _timer1.Stop();
            }
            if (NavigationService.CanGoBack)
            {
                // カウントダウンページ
                NavigationService.RemoveBackEntry();
                // 開始ページ
                NavigationService.RemoveBackEntry();
                NavigationService.GoBack();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow.OKCount = 0;

            ActiveCell = cell1;
            ActiveCell.Background = Brushes.White;

            switch (mainWindow.course_check)
            {
                case 5: // 足し算
                    enzanlabel.Content = "＋";
                    break;
                case 6: // 引き算
                    enzanlabel.Content = "－";
                    break;
                case 7: // 掛け算
                    enzanlabel.Content = "×";
                    break;
                case 8: // 割り算
                    enzanlabel.Content = "÷";
                    yokoborder1.Background = 
                    yokoborder2.Background = 
                    yokoborder3.Background = 
                    yokoborder4.Background = 
                    yokoborder5.Background = new SolidColorBrush(Color.FromRgb(255, 255, 192));
                    break;
            }

            // 問題作成
            Question();

            window = Window.GetWindow(this);
            window.KeyDown += HandleKeyDown;

            TimeCount = 0;
            // タイマのインスタンスを生成
            _timer1 = new DispatcherTimer();
            // インターバルを設定
            _timer1.Interval = new TimeSpan(0, 0, 1);
            // タイマメソッドを設定
            _timer1.Tick += new EventHandler(Timer1_Tick);
            // タイマを開始
            _timer1.Start();
        }

        // キーボード入力のイベント
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (　// 数字を入力した場合
                e.Key == Key.D0 || e.Key == Key.D1 || e.Key == Key.D2 || e.Key == Key.D3 || e.Key == Key.D4 ||
                e.Key == Key.D5 || e.Key == Key.D6 || e.Key == Key.D7 || e.Key == Key.D8 || e.Key == Key.D9 || 
                e.Key == Key.NumPad0 || e.Key == Key.NumPad1 || e.Key == Key.NumPad2 || e.Key == Key.NumPad3 || e.Key == Key.NumPad4 ||
                e.Key == Key.NumPad5 || e.Key == Key.NumPad6 || e.Key == Key.NumPad7 || e.Key == Key.NumPad8 || e.Key == Key.NumPad9
                )
            {
                // 未入力のセルに0を入力しようとしたときは入力を無視する
                if ((e.Key == Key.D0 || e.Key == Key.NumPad0) && ActiveCell.Content.ToString().Length == 0)
                {
                    return;
                }
                // 2桁未満のときのみ数値入力を許容する
                if (ActiveCell.Content.ToString().Length < 2)
                {
                    string keyCodeString = e.Key.ToString();
                    ActiveCell.Content += keyCodeString[keyCodeString.Length - 1].ToString();
                }
            }
            // スペースキー、マイナスキー、バックスペースキー、デリートキーを入力した場合は、セルの入力をクリアする
            else if (e.Key == Key.Space || e.Key == Key.OemMinus || e.Key == Key.Subtract || e.Key == Key.Back || e.Key == Key.Delete)
            {
                ActiveCell.Content = "";
            }
            // エンターキーを入力した場合は
            else if (e.Key == Key.Enter)
            {
                // 数字未入力の場合は何もしない
                if (ActiveCell.Content.ToString() == "") return;

                // 入力値が正解の場合
                if (ActiveCell.correct)
                {
                    // 正解の音を鳴らす
                    new SoundPlayer(Properties.Resources.CBC).Play();

                    // 正解数をカウントアップ
                    mainWindow.OKCount++;

                    // 回答済みのセルは灰色に色を変える
                    ActiveCell.Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                    ActiveCell.Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));

                    // 現在のセルの番号を取得
                    int cell_num = int.Parse(Regex.Replace(ActiveCell.Name, @"[^0-9]", ""));

                    // 割り算モードで次の行に移動する場合は、上部の数字5つを更新する
                    if (mainWindow.course_check == 8)
                    {
                        if (cell_num % 5 == 0 && cell_num != 25)
                        {
                            yoko1.Content = yoko[cell_num];
                            yoko2.Content = yoko[cell_num + 1];
                            yoko4.Content = yoko[cell_num + 3];
                            yoko5.Content = yoko[cell_num + 4];
                            yoko3.Content = yoko[cell_num + 2];
                        }
                    }

                    // 次のセルを探す
                    ActiveCell = this.FindName("cell" + (cell_num + 1)) as cell;

                    // 次のセルが存在する場合は、背景を白色にする
                    if (ActiveCell != null)
                    {
                        ActiveCell.Background = Brushes.White;
                    }
                    // 次のセルが存在しない場合は、次の25問の問題を出題する
                    else
                    {
                        for (int ii = 0; ii < 25; ii++)
                        {
                            cell c = FindName("cell" + (ii + 1)) as cell;
                            if (c != null)
                            {
                                if (ii == 0)
                                {
                                    ActiveCell = c;
                                    cell1.Background = Brushes.White;
                                }
                                else
                                {
                                    c.Background = Brushes.Black;
                                }
                                c.Foreground = Brushes.Black;
                                c.Content = "";
                            }
                        }
                        Question();　// 次の問題を出題する
                    }
                }
                // 不正解の場合は、回答をクリアする
                else
                {
                    new SoundPlayer(Properties.Resources.BUBU).Play();
                    ActiveCell.Content = "";
                }
            }
        }

        // ページのアンロード時にイベントハンドラを解除しておく
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            window.KeyDown -= HandleKeyDown;
        }
    }
}
