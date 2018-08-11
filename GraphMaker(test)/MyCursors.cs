using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Windows.Resources;
using System.Windows;
namespace GraphMaker_test_
{
    public static class MyCursors
    {
        public static void DefaultCursor()
        {
            StreamResourceInfo stream = Application.GetResourceStream(new Uri("Default.cur", UriKind.Relative));
            Cursor cursor_ = new Cursor(stream.Stream);
            MainWindow MW = (MainWindow)Application.Current.MainWindow;
            MW.Cursor = cursor_;
            
        }
        public static void CursorAddEdge()
        {
            StreamResourceInfo stream = Application.GetResourceStream(new Uri("Edge.cur", UriKind.Relative));
            Cursor cursor_ = new Cursor(stream.Stream);
            MainWindow MW = (MainWindow)Application.Current.MainWindow;
            MW.Cursor = cursor_;
        }
        public static Cursor AddEdgeCursor
        {
            get
            {
                StreamResourceInfo stream = Application.GetResourceStream(new Uri("Edge.cur", UriKind.Relative));
                Cursor cursor_ = new Cursor(stream.Stream);
                return cursor_;
            }
        }
        public static void CursorDelete()
        {
            StreamResourceInfo stream = Application.GetResourceStream(new Uri("Delete.cur", UriKind.Relative));
            Cursor cursor_ = new Cursor(stream.Stream);
            MainWindow MW = (MainWindow)Application.Current.MainWindow;
            MW.Cursor = cursor_;
        }
        public static Cursor DeleteCursor
        {
            get
            {
                StreamResourceInfo stream = Application.GetResourceStream(new Uri("Delete.cur", UriKind.Relative));
                Cursor cursor_ = new Cursor(stream.Stream);
                return cursor_;
            }
        }
        public static void CursorDijkstra()
        {
            StreamResourceInfo stream = Application.GetResourceStream(new Uri("Dijkstra.cur", UriKind.Relative));
            Cursor cursor_ = new Cursor(stream.Stream);
            MainWindow MW = (MainWindow)Application.Current.MainWindow;
            MW.Cursor = cursor_;
        }
        public static Cursor DijkstraCursor
        {
            get
            {
                StreamResourceInfo stream = Application.GetResourceStream(new Uri("Dijkstra.cur", UriKind.Relative));
                Cursor cursor_ = new Cursor(stream.Stream);
                return cursor_;
            }
        }

    }
}
