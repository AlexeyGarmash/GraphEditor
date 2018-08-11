using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace GraphMaker_test_
{
    public static class HotKeys
    {
        private static RoutedCommand UndoCommand_ = new RoutedCommand();
        private static RoutedCommand RedoCommand_ = new RoutedCommand();
        private static RoutedCommand NewCommand_ = new RoutedCommand();
        private static RoutedCommand SaveCommand_ = new RoutedCommand();
        private static RoutedCommand SaveAsCommand_ = new RoutedCommand();
        private static RoutedCommand OpenCommand_ = new RoutedCommand();

        public static RoutedCommand UndoCommand
        {
            get
            {
                UndoCommand_.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
                return UndoCommand_;
            }
        }

        public static RoutedCommand RedoCommand
        {
            get
            {
                RedoCommand_.InputGestures.Add(new KeyGesture(Key.Y, ModifierKeys.Control));
                return RedoCommand_;
            }
        }

        public static RoutedCommand NewCommand
        {
            get
            {
                NewCommand_.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
                return NewCommand_;
            }
        }

        public static RoutedCommand SaveCommand
        {
            get
            {
                SaveCommand_.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
                return SaveCommand_;
            }
        }

        public static RoutedCommand SaveAsCommand
        {
            get
            {
                SaveAsCommand_.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));
                return SaveAsCommand_;
            }
        }

        public static RoutedCommand OpenCommand
        {
            get
            {
                OpenCommand_.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
                return OpenCommand_;
            }
        }

       
    }
}
