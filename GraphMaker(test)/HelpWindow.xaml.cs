using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GraphMaker_test_
{
    /// <summary>
    /// Логика взаимодействия для HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        Info inf;
        public HelpWindow()
        {
            InitializeComponent();
             inf = new Info();
            this.KeyDown += new KeyEventHandler(InfoF12);
        }
        private void InfoF12(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                inf.Show();
            }
        }
    }
}
