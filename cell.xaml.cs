using System.Windows.Controls;
using System.Windows.Media;

namespace MASU25
{
    /// <summary>
    /// cell.xaml の相互作用ロジック
    /// </summary>
    public partial class cell : UserControl
    {
        public new Brush Background
        {
            set { border.Background = value; }
            get { return border.Background; }
        }
        public new Brush Foreground
        {
            set { label.Foreground = value; }
            get { return label.Foreground; }
        }
        public new object Content
        {
            set { label.Content = value; }
            get { return label.Content; }
        }
        public int anser { set; get; }
        public bool correct
        {
            get
            {
                int input;
                if (label.Content.ToString().Length == 0) return false;
                if (!int.TryParse(label.Content.ToString(), out input)) return false;
                return (input == anser);
            }
        }
        public cell()
        {
            InitializeComponent();
            Content = "";
        }
    }
}
