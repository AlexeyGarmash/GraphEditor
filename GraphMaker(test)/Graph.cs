using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Text.RegularExpressions;
using QuickGraph.Algorithms;
using QuickGraph;
using QuickGraph.Algorithms.ShortestPath;
using QuickGraph.Algorithms.Observers;
using System.Xml.Serialization;
using System.Windows.Markup;
namespace GraphMaker_test_
{
    [Serializable]
    public class Graph
    {
        public Xceed.Wpf.Toolkit.Zoombox.Zoombox zoom;
        public List<GraphVertex> VertexesList { get; set; }
        public List<GraphEdge> EdgesList { get; set; }
        [XmlIgnore]
        public Canvas GraphPlain;
        public bool IsOriented { get; set; }
        private bool IsHolded;
        public bool AddEdgeEnabled { get; set; }
        [XmlIgnore]
        private Ellipse FirstVertex;
        private int VertexesInEdge;
        [XmlIgnore]
        private Brush BufferColor;
        [XmlIgnore]
        private Brush BufferStrokeColor;
        public string InfoAboutAction;
        public int EnumerableVertexes;
        public bool DijkstraEnabled { get; set; }
        public bool AStarEnabled { get; set; }
        private int AStarVertexesEnumerator = 0;
        public bool IsHighLighted = false;
        public Graph() { }
        MainWindow MainWindow_;
        public Graph(Canvas canvas, bool oriented)
        {
            VertexesList = new List<GraphVertex>();
            EdgesList = new List<GraphEdge>();
            GraphPlain = canvas;
            IsOriented = oriented;
            IsHolded = false;
            AddEdgeEnabled = false;
            VertexesInEdge = 0;
            DeletingVertexEnable = false;
            DeleteingEdgeEnabled = false;
            EnumerableVertexes = 0;
            DijkstraEnabled = false;
            AStarEnabled = false;
            //GraphPlain.MouseMove += new MouseEventHandler(DrawHelpLineMouseMove);
            MainWindow_ = (MainWindow)Application.Current.MainWindow;
            
        }
        //private Line HelpLine = new Line();
        //private void DrawHelpLineMouseMove(object sender, MouseEventArgs e)
        //{
        //    if (AddEdgeEnabled && VertexesInEdge == 1)
        //    {
        //        HelpLine.X2 = e.GetPosition(zoom).X;
        //        HelpLine.Y2 = e.GetPosition(zoom).Y;
        //    }
        //}
        public void DeactivateAll()
        {
            IsHolded = false;
            AddEdgeEnabled = false;
            VertexesInEdge = 0;
            DeletingVertexEnable = false;
            DeleteingEdgeEnabled = false;
            AStarVertexesEnumerator = 0;
            DijkstraEnabled = false;
            AStarEnabled = false;
            MainWindow_.InfoCurr.Clear();
            MyCursors.DefaultCursor();
        }
        private void RunUndo()
        {
            string x = XamlWriter.Save(GraphPlain);
            UndoRedo.UndoActionsStack.Push(x);
        }
        private void RunRedo()
        {
            string x = XamlWriter.Save(GraphPlain);
            UndoRedo.RedoActionsStack.Push(x);
        }
        public void AddVertexOnPlain(Ellipse ell, TextBox textBox,  double x, double y)
        {
            EnumerableVertexes++;
            Random randColor = new System.Random();
            textBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(ChangeNameVertex);
            textBox.MouseDoubleClick += new MouseButtonEventHandler(TextBoxVertexDoubleClick);
            textBox.KeyDown += new KeyEventHandler(BreakFocus);
            textBox.MouseEnter += new MouseEventHandler(IsMouseOnTextBox);
            textBox.MouseMove += new MouseEventHandler(MouseMoveOnTextBox);
            textBox.CaretBrush = Brushes.Black;
            ell.MouseLeftButtonDown += new MouseButtonEventHandler(EllipseLeftButtonDown);
            ell.MouseLeftButtonUp += new MouseButtonEventHandler(VertexLeftButtonUp);
            ell.MouseMove += new MouseEventHandler(DragAndDropVertex);
            ell.MouseEnter += new MouseEventHandler(IsMouseEnterVertex);
            ell.MouseLeave += new MouseEventHandler(IsMouseLeaveVertex);
            ell.Height = 40;
           
            ell.Width = 40;
            textBox.Width = 40;
            textBox.Height = 25;
            textBox.FontSize = 16;
            textBox.FontWeight = FontWeights.Bold;
            textBox.BorderThickness = new Thickness(0, 0, 0, 0);
            textBox.TextAlignment = TextAlignment.Center;
            var color = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            textBox.Background = color;
            textBox.Text = EnumerableVertexes.ToString();
            textBox.Tag = EnumerableVertexes.ToString();
            ell.Tag = textBox.Text;
            ell.Fill = new SolidColorBrush(Color.FromArgb((byte)randColor.Next(0, 255), (byte)randColor.Next(0, 255), (byte)randColor.Next(0, 255), (byte)randColor.Next(0, 255)));
            ell.Stroke = Brushes.Blue;
            ell.StrokeThickness = 3;
            Canvas.SetLeft(ell, x - 15);
            Canvas.SetTop(ell, y - 15);
            Canvas.SetZIndex(ell, 1);
            Canvas.SetLeft(textBox, Canvas.GetLeft(ell));
            Canvas.SetTop(textBox, Canvas.GetTop(ell) + ell.Height + 1);
            Canvas.SetZIndex(textBox, 1);
            GraphPlain.Children.Add(ell);
            GraphPlain.Children.Add(textBox);
            VertexesList.Add(new GraphVertex(ell, new List<GraphEdge>(), textBox, textBox.Text));
        }

        private void MouseMoveOnTextBox(object sender, MouseEventArgs e)
        {
            TextBox t = new TextBox();
            t = (TextBox)sender;
            t.Focusable = true;
        }

        private void IsMouseOnTextBox(object sender, MouseEventArgs e)
        {
            TextBox t = new TextBox();
            t = (TextBox)sender;
            t.Focusable = true;
        }

        private void TextBoxVertexDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RunUndo();
            TextBox tempTextBox = new TextBox();
            tempTextBox = (TextBox)sender;
            tempTextBox.Focusable = true;
            tempTextBox.BorderThickness = new Thickness(1);
            tempTextBox.BorderBrush = Brushes.Green;
        }

        private void BreakFocus(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                VertexesList[0].ellipse.Focus();
            }
        }
        public void ReadVertex(Ellipse ell,TextBox textBox)
        {
            ell.MouseLeftButtonDown += new MouseButtonEventHandler(EllipseLeftButtonDown);
            ell.MouseLeftButtonUp += new MouseButtonEventHandler(VertexLeftButtonUp);
            ell.MouseMove += new MouseEventHandler(DragAndDropVertex);
            ell.MouseEnter += new MouseEventHandler(IsMouseEnterVertex);
            ell.MouseLeave += new MouseEventHandler(IsMouseLeaveVertex);
            textBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(ChangeNameVertex);
            textBox.MouseDoubleClick += new MouseButtonEventHandler(TextBoxVertexDoubleClick);
            textBox.KeyDown += new KeyEventHandler(BreakFocus);
            textBox.MouseEnter += new MouseEventHandler(IsMouseOnTextBox);
            textBox.MouseMove += new MouseEventHandler(MouseMoveOnTextBox);
            GraphPlain.Children.Add(ell);
            GraphPlain.Children.Add(textBox);
            VertexesList.Add(new GraphVertex(ell, new List<GraphEdge>(), textBox, textBox.Text));
        }
        public void ReadEdge(Petzold.Media2D.ArrowLine line, TextBox textBox, GraphVertex v1, GraphVertex v2)
        {
            textBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(ChangeEdgeWeight);
            textBox.KeyUp += new KeyEventHandler(KeyEnterClickTextBox);
            textBox.MouseEnter += new MouseEventHandler(IsMouseOnTextBox);
            textBox.MouseDoubleClick += new MouseButtonEventHandler(TextBoxVertexDoubleClick);
            textBox.MouseMove += new MouseEventHandler(MouseMoveOnTextBox);
            GraphEdge tempEdge = new GraphEdge(line, textBox, Convert.ToDouble(textBox.Text), v1, v2);
            foreach (var t in VertexesList)
            {
                if (t.ellipse == v1.ellipse)
                {
                    t.lines.Add(tempEdge);
                }
                if (t.ellipse == v2.ellipse)
                {
                    t.lines.Add(tempEdge);
                }
            }
            tempEdge.line.MouseLeftButtonUp += new MouseButtonEventHandler(LineEdgeMouseClick);
            GraphPlain.Children.Add(tempEdge.line);
            GraphPlain.Children.Add(textBox);
            EdgesList.Add(tempEdge);
        }

        

        public void ChangeNameVertex(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox txt = new System.Windows.Controls.TextBox();
            txt = (TextBox)sender;
            foreach (var vert in VertexesList)
            {
                if (vert.textBox == txt)
                {
                    vert.Name = txt.Text;
                }
            }
        }

        
        
        
        public void IsMouseLeaveVertex(object sender, MouseEventArgs e)
        {
            if (AStarEnabled) return;
            Ellipse ellipse = new System.Windows.Shapes.Ellipse();
            ellipse = (Ellipse)sender;
            ellipse.Fill = BufferColor;

            if (!AddEdgeEnabled && !AStarEnabled && !IsHighLighted)
            {
                ellipse.Stroke = BufferStrokeColor;
                RecolorAdjacentEdges(false, ellipse);
            }
        }

        public void IsMouseEnterVertex(object sender, MouseEventArgs e)
        {
            if (AStarEnabled) return;
            Ellipse ellipse = new System.Windows.Shapes.Ellipse();
            ellipse = (Ellipse)sender;
            ellipse.ToolTip = SearchVertexByEllipse(ellipse).Name;
            BufferColor = ellipse.Fill;
            BufferStrokeColor = ellipse.Stroke;
            //ellipse.Fill = Brushes.LightSkyBlue;
            if (AddEdgeEnabled)
            {
                ellipse.Cursor = MyCursors.AddEdgeCursor;
            }
            if (DeletingVertexEnable)
                ellipse.Cursor = MyCursors.DeleteCursor;
            if (!AddEdgeEnabled && !AStarEnabled && !IsHighLighted)
            {
                ellipse.Stroke = Brushes.Red;
                RecolorAdjacentEdges(true, ellipse);
            }
        }
        public void RecolorAdjacentEdges(bool IsEntered, Ellipse ellipse)
        {
            if (IsEntered)
            {
                foreach (GraphVertex vert in VertexesList)
                {
                    if (vert.ellipse == ellipse)
                    {
                        foreach (var edge in vert.lines)
                        {
                            if (IsOriented)
                            {
                                if (ellipse == edge.StartVertex.ellipse)
                                edge.line.Stroke = Brushes.Red;
                            }
                            else
                            {
                                edge.line.Stroke = Brushes.Red;
                            }
                        }
                    }
                }
            }
            if (!IsEntered)
            {
                foreach (GraphVertex vert in VertexesList)
                {
                    if (vert.ellipse == ellipse)
                    {
                        foreach (var edge in vert.lines)
                        {
                            edge.line.Stroke = Brushes.Black;
                        }
                    }
                }
            }
        }
        private TextBox DragTextBox(Ellipse ellipse)
        {
            foreach (var vert in VertexesList)
            {
                if (vert.ellipse == ellipse)
                    return vert.textBox;
            }
            return null;
        }
        public void DragAndDropVertex(object sender, MouseEventArgs e)
        {
            Ellipse ellipse = new Ellipse();
            ellipse = (Ellipse)sender;
            //if (AddEdgeEnabled)
            //{
            //    HelpLine.X2 = Canvas.GetLeft(ellipse) + ellipse.Width / 2;
            //    HelpLine.Y2 = Canvas.GetTop(ellipse) + ellipse.Height / 2;
            //}
            if (IsHolded)
            {
                    var mousePos = e.GetPosition(GraphPlain);
                    double left = mousePos.X - 20;
                    double top = mousePos.Y - 20;
                    Canvas.SetLeft(ellipse, left);
                    Canvas.SetTop(ellipse, top);
                    Canvas.SetLeft(DragTextBox(ellipse), Canvas.GetLeft(ellipse));
                    Canvas.SetTop(DragTextBox(ellipse), Canvas.GetTop(ellipse) + ellipse.Height + 1);
                    DragAndDropEdges(ellipse);
            }
            
        }

        private void RewriteTextBoxEdge(GraphEdge edge, TextBox TB)
        {
            if (edge.line.X1 >= edge.line.X2)
            {
                Canvas.SetLeft(edge.textBox, Math.Abs(edge.line.X1 - edge.line.X2) / 2 + edge.line.X2);
                if (edge.line.Y1 >= edge.line.Y2)
                {
                    Canvas.SetTop(edge.textBox, Math.Abs(edge.line.Y1 - edge.line.Y2) / 2 + edge.line.Y2);
                }
                if (edge.line.Y1 <= edge.line.Y2)
                {
                    Canvas.SetTop(edge.textBox, Math.Abs(edge.line.Y1 - edge.line.Y2) / 2 + edge.line.Y1);
                }
            }
            if (edge.line.X1 <= edge.line.X2)
            {
                Canvas.SetLeft(edge.textBox, Math.Abs(edge.line.X1 - edge.line.X2) / 2 + edge.line.X1);
                if (edge.line.Y1 >= edge.line.Y2)
                {
                    Canvas.SetTop(edge.textBox, Math.Abs(edge.line.Y1 - edge.line.Y2) / 2 + edge.line.Y2);
                }
                if (edge.line.Y1 <= edge.line.Y2)
                {
                    Canvas.SetTop(edge.textBox, Math.Abs(edge.line.Y1 - edge.line.Y2) / 2 + edge.line.Y1);
                }
            }

        }

        public void DragAndDropEdges(Ellipse e1)
        {
            foreach (var vert in VertexesList)
            {
                if (vert.ellipse == e1)
                {
                    foreach (var edges in vert.lines)
                    {
                        if (e1 == edges.StartVertex.ellipse)
                        {
                            edges.line.X1 = Canvas.GetLeft(e1) + e1.ActualWidth / 2;
                            edges.line.Y1 = Canvas.GetTop(e1) + e1.ActualHeight / 2;
                            edges.line.X2 = Canvas.GetLeft(edges.EndVertex.ellipse) + e1.ActualWidth / 2;
                            edges.line.Y2 = Canvas.GetTop(edges.EndVertex.ellipse) + e1.ActualHeight / 2;
                            double u_l = Math.Atan2(edges.line.X1 - edges.line.X2, edges.line.Y1 - edges.line.Y2);
                            edges.line.X1 = edges.line.X1 + (-20) * Math.Sin(u_l);
                            edges.line.Y1 = edges.line.Y1 + (-20) * Math.Cos(u_l);
                            edges.line.X2 = edges.line.X2 + 20 * Math.Sin(u_l);
                            edges.line.Y2 = edges.line.Y2 + 20 * Math.Cos(u_l);
                            RewriteTextBoxEdge(edges, null);
                        }
                        if (e1 == edges.EndVertex.ellipse)
                        {
                            edges.line.X1 = Canvas.GetLeft(edges.StartVertex.ellipse) + e1.ActualWidth / 2;
                            edges.line.Y1 = Canvas.GetTop(edges.StartVertex.ellipse) + e1.ActualHeight / 2;
                            edges.line.X2 = Canvas.GetLeft(e1) + e1.ActualWidth / 2;
                            edges.line.Y2 = Canvas.GetTop(e1) + e1.ActualHeight / 2;
                            double u_l = Math.Atan2(edges.line.X1 - edges.line.X2, edges.line.Y1 - edges.line.Y2);
                            edges.line.X1 = edges.line.X1 + (-20) * Math.Sin(u_l);
                            edges.line.Y1 = edges.line.Y1 + (-20) * Math.Cos(u_l);
                            edges.line.X2 = edges.line.X2 + 20 * Math.Sin(u_l);
                            edges.line.Y2 = edges.line.Y2 + 20 * Math.Cos(u_l);
                            RewriteTextBoxEdge(edges, null);
                        }
                    }
                }
            }

        }
        private bool EdgeAlreadyExist(GraphVertex v1, GraphVertex v2)
        {
            if (AddEdgeEnabled)
            foreach (var edge in EdgesList)
            {
                if ((edge.StartVertex == v1 && edge.EndVertex == v2) || (edge.StartVertex == v2 && edge.EndVertex == v1))
                {
                    return true;
                }
            }
            return false;
        }
        public void DisableTextBoxFocus()
        {
            foreach (var obj in GraphPlain.Children)
            {
                if (obj is TextBox)
                {
                    ((TextBox)obj).CaretBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    ((TextBox)obj).Focusable = false;
                    ((TextBox)obj).BorderThickness = new Thickness(0);
                }
            }
        }
        public void VertexLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            Ellipse ell = new Ellipse();
            ell = (Ellipse)sender;
            IsHolded = false;
            ell.Cursor = Cursors.Arrow;
            if (DijkstraEnabled)
            {
                ShortestPathDijkstra(SearchVertexByEllipse(ell));
                MyCursors.DefaultCursor();
            }
            
            ell.ReleaseMouseCapture();
        }
        Label vertAddedinEdge = new Label();
        public void EllipseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DisableTextBoxFocus();
            Ellipse ell = new Ellipse();

            ell = (Ellipse)sender;
            
            if (DeletingVertexEnable)
            {
                MainWindow_.InfoCurr.Clear();
                DeleteVertex(ell);
                DeletingVertexEnable = false;
                MyCursors.DefaultCursor();
                return;
            }
            if (AddEdgeEnabled != true)
            {
                IsHolded = true;
                RunUndo();
                ell.CaptureMouse();
                ell.Cursor = Cursors.Cross;
            }
            if (AddEdgeEnabled)
            {
                VertexesInEdge++;
                if (VertexesInEdge == 1)
                {
                    FirstVertex = ell;
                    FirstVertex.StrokeThickness = 5;
                    FirstVertex.Stroke = Brushes.Red;
                    MainWindow_.InfoCurr.Text = "Добавление ребра. Выберете 2-ую вершину.ESC - отмена";
                    //
                    //HelpLine.StrokeThickness = 2;
                    //HelpLine.Stroke = Brushes.Gray;
                    //HelpLine.X1 = Canvas.GetLeft(FirstVertex) + FirstVertex.Width / 2;
                    //HelpLine.Y1 = Canvas.GetTop(FirstVertex) + FirstVertex.Height / 2;
                    //HelpLine.X2 = Canvas.GetLeft(FirstVertex) + FirstVertex.Width / 2;
                    //HelpLine.Y2 = Canvas.GetTop(FirstVertex) + FirstVertex.Height / 2;
                    //GraphPlain.Children.Add(HelpLine);
                }
                if (VertexesInEdge == 2)
                {
                    MainWindow_.InfoCurr.Clear();
                    OnDefaultColor();
                    VertexesInEdge = 0;
                    ConnectVertex(SearchVertexByEllipse(FirstVertex), SearchVertexByEllipse(ell));
                    AddEdgeEnabled = false;
                    MyCursors.DefaultCursor();
                }
            }
            if (AStarEnabled)
            {
                IsHolded = false;
                AStarVertexesEnumerator++;
                if (AStarVertexesEnumerator == 1)
                {
                    FirstVertex = ell;
                    MainWindow_.InfoCurr.Text = "A*. Выберете вторую вершину.ESC - отмена";
                }
                if (AStarVertexesEnumerator == 2)
                {
                    MainWindow_.InfoCurr.Clear();
                    AStarVertexesEnumerator = 0;
                    SearchShortestPathAStar(SearchVertexByEllipse(FirstVertex), SearchVertexByEllipse(ell));
                    MyCursors.DefaultCursor();
                    AStarEnabled = false;
                }
            }
        }
        public GraphVertex SearchVertexByEllipse(Ellipse ellipse)
        {
            foreach (var vert in VertexesList)
            {
                if (vert.ellipse == ellipse)
                    return vert;
            }
            return null;
        }
        public void ConnectVertex(GraphVertex e1, GraphVertex e2)
        {
            if (e1 == e2)
                return;
            if (EdgeAlreadyExist(e1, e2))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Ребро уже было добавлено!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
                TextBox textBox = new System.Windows.Controls.TextBox();
            
            GraphEdge tempEdge_ = new GraphEdge(e1, e2, IsOriented, textBox, 0);
            RewriteTextBoxEdge(tempEdge_, textBox);
            textBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(ChangeEdgeWeight);
            textBox.KeyUp += new KeyEventHandler(KeyEnterClickTextBox);
            textBox.CaretBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            textBox.Width = 30;
            textBox.Height = 25;
            textBox.FontSize = 16;
            textBox.FontWeight = FontWeights.Bold;
            var color = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            textBox.Background = Brushes.White;
            textBox.BorderThickness = new Thickness(0, 0, 0, 0);
            textBox.TextAlignment = TextAlignment.Center;
            textBox.Text = "1";
            textBox.Tag = tempEdge_.line.Tag.ToString();
            textBox.MouseEnter += new MouseEventHandler(IsMouseOnTextBox);
            textBox.MouseDoubleClick += new MouseButtonEventHandler(TextBoxVertexDoubleClick);
            textBox.MouseMove += new MouseEventHandler(MouseMoveOnTextBox);
            //textBox.MouseLeftButtonUp += new MouseButtonEventHandler(MouseLeftDown);
            GraphEdge tempEdge = new GraphEdge(e1, e2, IsOriented, textBox, Convert.ToDouble(textBox.Text));
            foreach (var t in VertexesList)
            {
                if (t.ellipse == e1.ellipse)
                {
                    t.lines.Add(tempEdge);
                }
                if (t.ellipse == e2.ellipse)
                {
                    t.lines.Add(tempEdge);
                }
            }
            tempEdge.line.MouseLeftButtonUp += new MouseButtonEventHandler(LineEdgeMouseClick);
            GraphPlain.Children.Add(tempEdge.line);
            GraphPlain.Children.Add(textBox);
            EdgesList.Add(tempEdge);
        }

        private void BuildEdgeWeight(object sender)
        {
            TextBox txt = new System.Windows.Controls.TextBox();
            txt = (TextBox)sender;
            foreach (var edge in EdgesList)
            {
                if (edge.textBox == txt)
                {
                    if (IsWeightTrue(txt.Text) && txt.Text.Length != 0)
                        edge.WeightEdge = Convert.ToDouble(txt.Text);
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Ввод только цифр! Скидываюсь на 1", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        edge.WeightEdge = 1;
                        txt.Text = "1";
                    }
                }
            }
            VertexesList[0].ellipse.Focus();
        }
        private void KeyEnterClickTextBox(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            BuildEdgeWeight(sender);
            
        }
        private bool IsWeightTrue(string weight)
        {
            string trueSymb = "-0123456789";
            for (int i = 0; i < weight.Length; i++)
            {
                if (!trueSymb.Contains(weight[i]))
                {
                    return false;
                }
                if (weight == "")
                    return false;
            }
            return true;
        }
        public void ChangeEdgeWeight(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox txt = new System.Windows.Controls.TextBox();
            txt = (TextBox)sender;
            foreach (var edge in EdgesList)
            {
                if (edge.textBox == txt)
                {
                    if (txt.Text == "-" || txt.Text == "") return;
                    else
                        if (IsWeightTrue(txt.Text))
                            edge.WeightEdge = Convert.ToDouble(txt.Text);
                        else
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Ввод только цифр! Скидываюсь на 1", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            edge.WeightEdge = 1;
                            txt.Text = "1";
                        }
                }
            }
        }

        public void LineEdgeMouseClick(object sender, MouseButtonEventArgs e)
        {
            //StringActionUndo();
            Petzold.Media2D.ArrowLine line = new Petzold.Media2D.ArrowLine();
            line = (Petzold.Media2D.ArrowLine)sender;
            if (DeleteingEdgeEnabled)
            {
                MainWindow_.InfoCurr.Clear();
                RemoveEdge(line);
                DeleteingEdgeEnabled = false;
                MyCursors.DefaultCursor();
            }
            if (IsOriented)
            {
                foreach (var edge in EdgesList)
                {
                    if (edge.line == line)
                    {
                        GraphVertex StartV = edge.StartVertex;
                        GraphVertex EndV = edge.EndVertex;
                        ConnectVertex(EndV, StartV);
                        RemoveEdge(line);
                        break;
                    }
                }
            }
        }
        public void ChangeDirection()
        {
            
            if (!IsOriented)
            {
                foreach (GraphEdge edge in EdgesList)
                {
                    edge.line.ArrowEnds = Petzold.Media2D.ArrowEnds.End;
                }
            }
            if (IsOriented)
            {
                foreach (GraphEdge edge in EdgesList)
                {
                    edge.line.ArrowEnds = Petzold.Media2D.ArrowEnds.None;
                }
            }
        }

        public bool DeletingVertexEnable { get; set; }

        public void DeleteVertex(Ellipse ellipse)
        {
            foreach (var vert in VertexesList)
            {
                if (vert.ellipse == ellipse)
                {
                    foreach (var edge in vert.lines)
                    {
                        if (vert.ellipse == edge.StartVertex.ellipse)
                        {
                            helpdeleteEdge(edge.EndVertex.ellipse, edge);
                        }
                        if (vert.ellipse == edge.EndVertex.ellipse)
                        {
                            helpdeleteEdge(edge.StartVertex.ellipse, edge);
                        }
                        GraphPlain.Children.Remove(edge.line);
                        GraphPlain.Children.Remove(edge.textBox);
                        EdgesList.Remove(edge);
                    }
                    vert.lines.Clear();
                    VertexesList.Remove(vert);
                    GraphPlain.Children.Remove(vert.ellipse);
                    GraphPlain.Children.Remove(vert.textBox);
                    break;
                }
            }
        }
        public void helpdeleteEdge(Ellipse ellipse, GraphEdge edge)
        {
            foreach (var vert in VertexesList)
            {
                if (vert.ellipse == ellipse)
                    vert.lines.Remove(edge);
            }
        }

        public bool DeleteingEdgeEnabled { get; set; }

        private  void RemoveEdge(Petzold.Media2D.ArrowLine line)
        {
            foreach (var edge in EdgesList)
            {
                if (edge.line == line)
                {
                    GraphPlain.Children.Remove(line);
                    GraphPlain.Children.Remove(edge.textBox);
                    EdgesList.Remove(edge);
                    foreach (var vert in VertexesList)
                    {
                        foreach (var edges in vert.lines)
                        {
                            if (edges.line == line)
                            {
                                vert.lines.Remove(edges);
                                break;
                            }
                        }
                    }
                        break; 
                }
            }
        }

        

        public void SearchShortestPathAStar(GraphVertex startVertex, GraphVertex endVertex)
        {
                var ASTAR = Algorithms.ShortestWayAstarAlgorithm(VertexesList, EdgesList, startVertex, endVertex);
                if (ASTAR != null)
                {
                    RecolorByAstar(ASTAR);
                    IsHighLighted = true;
                }
                else Xceed.Wpf.Toolkit.MessageBox.Show("Пути не нашёл((", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //IsEulirianRun = false;            
        }

        public void RecolorByAstar(IEnumerable<Edge<GraphVertex>> circle)
        {
            foreach (var kvp in circle)
            {
                foreach (var edge in EdgesList)
                {
                    if (edge.StartVertex == kvp.Source && edge.EndVertex == kvp.Target)
                    {
                        edge.StartVertex.ellipse.StrokeThickness = 5;
                        edge.EndVertex.ellipse.StrokeThickness = 5;
                        edge.StartVertex.ellipse.Stroke = Brushes.SpringGreen;
                        edge.line.Stroke = Brushes.SpringGreen;
                        edge.line.StrokeThickness = 5;
                        edge.EndVertex.ellipse.Stroke = Brushes.SpringGreen;
                    }
                }
            }
            circle.ElementAt(0).Source.ellipse.StrokeThickness = 7;
            circle.ElementAt(0).Source.ellipse.Stroke = Brushes.Red;
            //circle.Last().Source.ellipse.StrokeThickness = 7;
            //circle.Last().Source.ellipse.Stroke = Brushes.Red;
            /*foreach (KeyValuePair<Vertex, double> kvp in circle)
            {
                foreach (var t in listVertex)
                {
                    if (kvp.Key == t)
                    {
                        t.ellipse.StrokeThickness = 7;
                        t.ellipse.Stroke = Brushes.SpringGreen;
                    }
                }
            }*/
        }
        public void MinimumSpaningTreePrim()
        {
            if (!IsOriented)
            {
                var primTree = Algorithms.MinimumTreePrima(VertexesList, EdgesList);
                RecolorByPrima(primTree);
                IsHighLighted = true;
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Этот алгоритм не может применится к Ориентированному графу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        public void RecolorByPrima(IEnumerable<UndirectedEdge<GraphVertex>> primtree)
        {
            foreach (var kvp in primtree)
            {
                foreach (var edge in EdgesList)
                {
                    if (edge.StartVertex == kvp.Source && edge.EndVertex == kvp.Target)
                    {
                        edge.StartVertex.ellipse.StrokeThickness = 5;
                        edge.EndVertex.ellipse.StrokeThickness = 5;
                        edge.StartVertex.ellipse.Stroke = Brushes.SpringGreen;
                        edge.line.Stroke = Brushes.SpringGreen;
                        edge.line.StrokeThickness = 5;
                        edge.EndVertex.ellipse.Stroke = Brushes.SpringGreen;
                    }
                }
            }
            primtree.ElementAt(0).Source.ellipse.StrokeThickness = 7;
            primtree.ElementAt(0).Source.ellipse.Stroke = Brushes.Red;
        }
        public void ShortestPathDijkstra(GraphVertex vertexD)
        {
            if (!IsOriented)
            {
                string result = Algorithms.ShortestWayDijsktraAlgorithmUnDirected(vertexD, EdgesList, VertexesList);
                Xceed.Wpf.Toolkit.MessageBox.Show(result, "Результат Деисктры", MessageBoxButton.OK, MessageBoxImage.Information);
                DijkstraEnabled = false;
                MainWindow_.InfoCurr.Clear();
            }
            else
            {
                string result = Algorithms.ShortestWayDijsktraAlgorithmDirected(vertexD, EdgesList, VertexesList);
                Xceed.Wpf.Toolkit.MessageBox.Show(result, "Результат Деисктры", MessageBoxButton.OK, MessageBoxImage.Information);
                DijkstraEnabled = false;
                MainWindow_.InfoCurr.Clear();
            }
        }

        public void OnDefaultColor()
        {
            foreach (var vert in VertexesList)
            {
                vert.ellipse.Stroke = Brushes.Blue;
                vert.ellipse.StrokeThickness = 3;
                foreach (var edge in vert.lines)
                {
                    edge.line.Stroke = Brushes.Black;
                    edge.line.StrokeThickness = 3;
                }
            }
            foreach (var edge in EdgesList)
            {
                edge.line.Stroke = Brushes.Black;
                edge.line.StrokeThickness = 3;
            }
            IsHighLighted = false;
        }
        public Ellipse FileFindVertexByEllipse(Petzold.Media2D.ArrowLine line, int vertex)
        {
            Ellipse ellipse = new Ellipse();
            foreach (var t in VertexesList)
            {

                string[] strs = line.Tag.ToString().Split(';');
                if (vertex == 1)
                {
                    if (strs[0] == t.ellipse.Tag.ToString())
                    {
                        ellipse = t.ellipse;
                        return ellipse;
                    }
                }
                if (vertex == 2)
                {
                    if (strs[1] == t.ellipse.Tag.ToString())
                    {
                        ellipse = t.ellipse;
                        return ellipse;
                    }
                }

            }
            return ellipse;
        }

        public TextBox SearchEdgeTextBox(List<object> file, Petzold.Media2D.ArrowLine line)
        {
            TextBox TB = new TextBox();
            foreach (var t in file)
            {
                if (t is TextBox)
                {
                    if (line.Tag.ToString() == ((TextBox)t).Tag.ToString())
                    {
                        TB = (TextBox)t;
                        return TB;
                    }
                }
            }
            return null;
        }

        public TextBox SearchVertexTextBox(List<object> file, Ellipse ellipse)
        {
            TextBox TB = new TextBox();
            foreach (var t in file)
            {
                if (t is TextBox)
                {
                    if (ellipse.Tag.ToString() == ((TextBox)t).Tag.ToString())
                    {
                        TB = (TextBox)t;
                        return TB;
                    }
                }
            }
            return null;
        }
    }
}
