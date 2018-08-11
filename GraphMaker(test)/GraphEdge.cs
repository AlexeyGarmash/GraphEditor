using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.Windows.Controls;
using Petzold;
using Petzold.Media2D;
namespace GraphMaker_test_
{
    [Serializable]
    public class GraphEdge
    {
        public GraphVertex StartVertex;
        public GraphVertex EndVertex;
        public ArrowLine line;
        public double WeightEdge { get; set; }
        private System.Windows.Media.Brush tempStroke;
        public TextBox textBox;
        public GraphEdge(GraphVertex startV, GraphVertex endV, bool oriented, TextBox text, double w)
        {
            line = new ArrowLine();
            this.StartVertex = startV;
            this.EndVertex = endV;
            line.Stroke = System.Windows.Media.Brushes.Black;
            this.line.X1 = Canvas.GetLeft(startV.ellipse) + startV.ellipse.ActualWidth / 2;
            this.line.Y1 = Canvas.GetTop(startV.ellipse) + startV.ellipse.ActualHeight / 2;
            this.line.X2 = Canvas.GetLeft(endV.ellipse) + endV.ellipse.ActualWidth / 2;
            this.line.Y2 = Canvas.GetTop(endV.ellipse) + endV.ellipse.ActualHeight / 2;
            double u_l = Math.Atan2(line.X1 - line.X2, line.Y1 - line.Y2);
            double u = Math.PI / 33;
            line.X1 = line.X1 + (-20) * Math.Sin(u_l);
            line.Y1 = line.Y1 + (-20) * Math.Cos(u_l);
            line.X2 = line.X2 + (20) * Math.Sin(u_l);
            line.Y2 = line.Y2 + (20) * Math.Cos(u_l);
            if (oriented)
                this.line.ArrowEnds = ArrowEnds.End;
            else
                this.line.ArrowEnds = ArrowEnds.None;
            line.StrokeThickness = 3;
            line.MouseEnter += new System.Windows.Input.MouseEventHandler(EdgeLineMouseEnter);
            line.MouseLeave += new System.Windows.Input.MouseEventHandler(EdgeLineMouseLeave);
            line.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(EdgeClick_empty_);
            WeightEdge = w;
            textBox = text;
            line.Tag = startV.ellipse.Tag.ToString() + ";" + endV.ellipse.Tag.ToString();
        }
        public GraphEdge() { }
        public GraphEdge(ArrowLine line_, TextBox text, double w, GraphVertex v1, GraphVertex v2)
        {
            line = line_;
            this.StartVertex = v1;
            this.EndVertex = v2;
            line.MouseEnter += new System.Windows.Input.MouseEventHandler(EdgeLineMouseEnter);
            line.MouseLeave += new System.Windows.Input.MouseEventHandler(EdgeLineMouseLeave);
            WeightEdge = w;
            textBox = text;
        }
        private void EdgeClick_empty_(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }

        private void EdgeLineMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ArrowLine templine = new ArrowLine();
            templine = (ArrowLine)sender;
            templine.Stroke = tempStroke;
            
        }

        private void EdgeLineMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ArrowLine templine = new ArrowLine();
            templine = (ArrowLine)sender;
            tempStroke = templine.Stroke;
            templine.Stroke = System.Windows.Media.Brushes.Green;
            templine.Cursor = System.Windows.Input.Cursors.Hand;
        }
    }
}
