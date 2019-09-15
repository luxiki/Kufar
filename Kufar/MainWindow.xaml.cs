using System.Reflection;
using System.Windows;

namespace Kufar
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dynamic activeX = this.WB.GetType().InvokeMember("ActiveXInstance",
            BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, this.WB, new object[] { });

            activeX.Silent = true;
        }
    }
}
