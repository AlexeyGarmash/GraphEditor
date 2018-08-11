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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Resources;
using Petzold;
using Petzold.Media2D;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Windows.Markup;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
namespace GraphMaker_test_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int ImageSource_ = 0;
        private string pathName = "";
        public Graph graph;
        private bool IsDirected;
        private bool PlainIsCreated;
        public MainWindow()
        {
            InitializeComponent();
            InfoCurr.Visibility = System.Windows.Visibility.Hidden;
            this.KeyDown += new KeyEventHandler(KeyF12Click);
            WorkSpace.Background = Brushes.Gray;
            PlainIsCreated = false;
            IsDirected = false;
            CommandBindings.Add(new CommandBinding(HotKeys.UndoCommand, Undo_Click));
            CommandBindings.Add(new CommandBinding(HotKeys.RedoCommand, Redo_Click));
            CommandBindings.Add(new CommandBinding(HotKeys.NewCommand, NewGraph_Click));
            CommandBindings.Add(new CommandBinding(HotKeys.SaveCommand, SaveButton_Click));
            CommandBindings.Add(new CommandBinding(HotKeys.SaveAsCommand, SaveAs_Click));
            CommandBindings.Add(new CommandBinding(HotKeys.OpenCommand, OpenFile_Click));
            MyCursors.DefaultCursor();

        }

        private void KeyF12Click(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                Info inf = new Info();
                inf.Show();
            }
            if (e.Key == Key.F1)
            {
                HelpWindow help = new HelpWindow();
                help.Show();
            }
            if (e.Key == Key.Escape)
                graph.DeactivateAll();
        }
        private bool MessageBoxMessage()
        {
            if (!PlainIsCreated)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Создайте новый граф для начала", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            return false;
        }
        private void RunUndo()
        {
            string x = XamlWriter.Save(WorkSpace);
            UndoRedo.UndoActionsStack.Push(x);
        }
        private void RunRedo()
        {
            string x = XamlWriter.Save(WorkSpace);
            UndoRedo.RedoActionsStack.Push(x);
        }
        private void AddVertex_Click(object sender, RoutedEventArgs e)
        {
            
            if (MessageBoxMessage()) return;
            
            RunUndo();
            graph.AddVertexOnPlain(new Ellipse(), new TextBox(), WorkSpace.ActualWidth / 2, WorkSpace.ActualHeight / 2);
        }

        private void AddEdge_Click(object sender, RoutedEventArgs e)
        {
            
            if (MessageBoxMessage()) return;
            if (graph.VertexesList.Count == 0 || graph.VertexesList.Count == 1)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Малое количество вершин для добавления ребра", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            graph.DeactivateAll();
            InfoCurr.Text = "Добавление ребра. Выберете 1-ую вершину.ESC - отмена";
            MyCursors.CursorAddEdge();
            RunUndo();
            graph.AddEdgeEnabled = true;
        }

        private void DeleteVertex_Click(object sender, RoutedEventArgs e)
        {
            
            if (MessageBoxMessage()) return;
            if (graph.VertexesList.Count == 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Малое количество вершин для удаления вершин", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            graph.DeactivateAll();
            InfoCurr.Text = "Удаление вершины. Выберете вершину.ESC - отмена";
            MyCursors.CursorDelete();
            RunUndo();
            graph.DeletingVertexEnable = true;
        }

        private void DeleteEdge_Click(object sender, RoutedEventArgs e)
        {
            
            if (MessageBoxMessage()) return;
            if (graph.EdgesList.Count == 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Малое количество ребер для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            graph.DeactivateAll();
            InfoCurr.Text = "Удаление ребра. Выберете ребро.ESC - отмена";
            MyCursors.CursorDelete();
            RunUndo();
            graph.DeleteingEdgeEnabled = true;
        }

        private void ASTAR_Click(object sender, RoutedEventArgs e)
        {
            
            if (MessageBoxMessage()) return;
            if (!graph.IsOriented)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Этот алгоритм не может применится к Неориентированному графу. Сделайте граф направленным!!!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            graph.DeactivateAll();
            InfoCurr.Text = "A*. Выберете первую вершину.ESC - отмена";
            graph.AStarEnabled = true;
        }

        private void Dijkstra_Click(object sender, RoutedEventArgs e)
        {
            
            if (MessageBoxMessage()) return;
            if (graph.EdgesList.Count == 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Нет ребер. Добавьте ребра для выполнения алгоритма Деикстры!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            graph.DeactivateAll();
            InfoCurr.Text = "Деикстра. Выберете вершину.ESC - отмена";
            MyCursors.CursorDijkstra();
            graph.DijkstraEnabled = true;
        }

        private void Prima_Click(object sender, RoutedEventArgs e)
        {
            
            //graph.IsPrimRun = true;
            if (MessageBoxMessage()) return;
            if (graph.EdgesList.Count == 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Нет ребер. Добавьте ребра для выполнения алгоритма Примы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            graph.DeactivateAll();
            graph.MinimumSpaningTreePrim();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxMessage()) return;
            graph.DeactivateAll();
            SaveFileAs();
        }
        private void SaveFileAs()
        {
            if (pathName == "")
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.Filter = "Text graph (.txt)|*.txt";
                dlg.FileName = "graph";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    pathName = dlg.FileName;
                    SimpleSave(pathName);
                }
            }
            else
            {
                SimpleSave(pathName);
            }
        }
        private void SimpleSave(string FileName)
        {
            string x = XamlWriter.Save(WorkSpace);
            StreamWriter str = new StreamWriter(FileName);
            str.Write(x);
            str.Close();
        }

        private void AnalyzeAction(string command, bool undo)
        {
            try
            {
                StringReader stringReader = new StringReader(command);
                XmlReader xml = XmlReader.Create(stringReader);
                WorkSpace.Children.Clear();
                Canvas tempcanv = ((Canvas)XamlReader.Load(xml));
                List<object> listS = new List<object>();
                foreach (var t in tempcanv.Children)
                {
                    listS.Add(t);
                }
                tempcanv.Children.Clear();
                graph = new Graph(WorkSpace, false);
                PlainIsCreated = true;
                foreach (var t in listS)
                {
                    if (t is Ellipse)
                    {
                        graph.ReadVertex((Ellipse)t, graph.SearchVertexTextBox(listS, (Ellipse)t));
                    }
                }
                foreach (var t in listS)
                {
                    if (t is ArrowLine)
                    {
                        if (undo)
                            graph.ReadEdge((ArrowLine)t, graph.SearchEdgeTextBox(listS, (ArrowLine)t), graph.SearchVertexByEllipse(graph.FileFindVertexByEllipse((ArrowLine)t, 1)), graph.SearchVertexByEllipse(graph.FileFindVertexByEllipse((ArrowLine)t, 2)));
                        else
                        {
                            ArrowLine line = (ArrowLine)t;
                            line.ArrowEnds = ArrowEnds.None;
                            graph.ReadEdge(line, graph.SearchEdgeTextBox(listS, line), graph.SearchVertexByEllipse(graph.FileFindVertexByEllipse(line, 1)), graph.SearchVertexByEllipse(graph.FileFindVertexByEllipse(line, 2)));
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            BuildUndo();
        }
        private void BuildUndo()
        {
            if (!PlainIsCreated) return;
            if (UndoRedo.UndoActionsStack.Count == 0) return;
            graph.DeactivateAll();
            string command = UndoRedo.UndoActionsStack.Pop();
            RunRedo();
            AnalyzeAction(command, true);
            graph.OnDefaultColor();
        }
        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            
            BuildRedo();
        }
        private void BuildRedo()
        {
            if (!PlainIsCreated) return;
            if (UndoRedo.RedoActionsStack.Count == 0) return;
            graph.DeactivateAll();
            string command = UndoRedo.RedoActionsStack.Pop();
            RunUndo();
            AnalyzeAction(command, true);
            graph.OnDefaultColor();
        }
        private void Zoom_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!PlainIsCreated) return;
            if (e.Delta > 0)
            {
                Zoom.Zoom(0.2, new Point(Mouse.GetPosition(e.Device.Target).X, Mouse.GetPosition(e.Device.Target).Y));
            }
            if (e.Delta < 0)
            {
                Zoom.Zoom(-0.2, new Point(Mouse.GetPosition(e.Device.Target).X, Mouse.GetPosition(e.Device.Target).Y));
            }
        }

        private void MakeOriented_Click(object sender, RoutedEventArgs e)
        {
            
            if (MessageBoxMessage()) return;
            graph.DeactivateAll();
            if (graph.EdgesList.Count == 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Нет ребер. Добавьте ребра для изменения напрвленности!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (!IsDirected)
            {
                graph.IsOriented = false;
                IsDirected = true;
                graph.ChangeDirection();
                MakeOriented.Content = "Make noOriented";
                graph.IsOriented = true;
            }
            else
            {
                graph.IsOriented = true;
                IsDirected = false;
                graph.ChangeDirection();
                MakeOriented.Content = "Make Oriented";
                graph.IsOriented = false;
            }
        }

        
        private void ASTAR_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = new Button();
            button = (Button)sender;
            button.Cursor = Cursors.Hand;
            button.Foreground = Brushes.White;
        }

        private void AddVertex_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = new Button();
            button = (Button)sender;
            button.Foreground = Brushes.Black;
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxMessage()) return;
            graph.DeactivateAll();
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Png image (.png)|*.png|Text graph (.txt)|*.txt";
            dlg.FileName = "graph";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                SaveToImage(dlg.FileName);
            }
        }
        private void NewWorkPlain()
        {
            InfoCurr.Clear();
            InfoCurr.Visibility = System.Windows.Visibility.Visible;
            WorkSpace.Background = null;
            PlainIsCreated = false;
            WorkSpace.Children.Clear();
            graph = new Graph(WorkSpace, false);
            graph.zoom = Zoom;
            UndoRedo.UndoActionsStack.Clear();
            PlainIsCreated = true;
            pathName = "";
        }
        private void NewGraph_Click(object sender, RoutedEventArgs e)
        {
            
            if (!PlainIsCreated || WorkSpace.Children.Count == 0)
            {
                NewWorkPlain();
                graph.DeactivateAll();
            }
            else
            {
                IfCurrentNotSave();
                graph.DeactivateAll();
            }
        }
        private void IfCurrentNotSave()
        {
            MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Сохранить изменения?", "Предупреждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (dialog == MessageBoxResult.Yes)
            {
                SaveFileAs();
                NewWorkPlain();
            }
            else
                if (dialog == MessageBoxResult.No)
                {
                    NewWorkPlain();
                    return;
                }
        }
        private void SaveToImage(string pathName_)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)WorkSpace.RenderSize.Width,(int)WorkSpace.RenderSize.Height,96d,96d, PixelFormats.Default);
            rtb.Render(WorkSpace);
            var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, (int)WorkSpace.RenderSize.Width, (int)WorkSpace.RenderSize.Height));
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(crop));
            using (var fs = System.IO.File.OpenWrite(pathName_))
            {
                pngEncoder.Save(fs);
            }
        }

        private void WorkSpace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            /*if (!IsCreated) return;
            if (!graph.IsColored)
            graph.ReturnColor();*/
        }

        private void Zoom_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            /*if (!IsCreated) return;
            if (!graph.IsColored)
            graph.ReturnColor();*/
            //graph.TabStop();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            open.Filter = "Text graph (.txt)|*.txt";
            Nullable<bool> result = open.ShowDialog();
            if (result == true)
            {
                try
                {
                    if (PlainIsCreated || WorkSpace.Children.Count != 0)
                        IfCurrentNotSave();
                    pathName = open.FileName;
                    StreamReader r = new StreamReader(pathName);
                    string s = r.ReadToEnd();
                    r.Close();
                    PlainIsCreated = false;
                    AnalyzeAction(s, false);
                    graph.DeactivateAll();
                    graph.EnumerableVertexes = graph.VertexesList.Count;
                    WorkSpace.Background = null;
                }
                catch(Exception ex)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Файл поврежден или содержимое отсутствует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        

        private void WND_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            graph.DeactivateAll();
            if (!PlainIsCreated || WorkSpace.Children.Count == 0)
            {
                return;
            }
            MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Сохранить изменения?", "Предупреждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (dialog == MessageBoxResult.Yes)
            {
                SaveFileAs();
            }
            else
            if (dialog == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow help = new HelpWindow();
            help.Show();
        }

        private void WND_Loaded(object sender, RoutedEventArgs e)
        {
            this.ResizeMode = System.Windows.ResizeMode.CanMinimize;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void ProgramSpace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ImageSource_ == 0)
            {
                ProgramSpace.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Icons/MatD.jpg")));
                GraphWork.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Icons/MatD.jpg")));
                ImageSource_++;
            }
            if (ImageSource_ != 0)
            {
                ProgramSpace.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Icons/IMG_138460.png")));
                GraphWork.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Icons/IMG_138460.png")));
                ImageSource_ = 0;
            }
            
        }

        private void Zoom_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!PlainIsCreated) return;
            graph.OnDefaultColor();
            graph.DisableTextBoxFocus();
        }

        
    }
}
