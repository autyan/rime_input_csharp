using System.Windows;
using RimeInputCSharp;

namespace RimeInputCSharpDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RimeInput _rimeInput;

        public MainWindow()
        {
            InitializeComponent();
            _rimeInput = RimeInput.CreateOrGetInstance(new RimeTraits
                                                       {
                                                           AppName = "FortrunInput"
                                                       });
        }
    }
}
