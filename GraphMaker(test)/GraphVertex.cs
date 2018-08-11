using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Xml.Serialization;
namespace GraphMaker_test_
{
    [Serializable]
    
    public class GraphVertex
    {
        public List<GraphEdge> lines { get; set; }
        public Ellipse ellipse;
        public TextBox textBox;
        public string Name { get; set; }
        public GraphVertex(Ellipse e, List<GraphEdge> l, TextBox tb, string name)
        {
            this.ellipse = e;
            this.lines = l;
            this.textBox = tb;
            this.Name = name;
        }
        public GraphVertex()
        {

        }
    }
}
