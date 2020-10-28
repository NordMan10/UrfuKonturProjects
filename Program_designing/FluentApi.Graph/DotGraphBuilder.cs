using System;
using System.Globalization;
using System.Linq;
using System.Text;
using FluentAssertions;

namespace FluentApi.Graph
{
    /// * For GraphNode and GraphEdge to do the extension methods AddNode() and AddEdge() that return Graph
    /// * To do static class NodeShape with corresponding properties
    /// * With(), Build() are also extension methods
    /// * Build() starts at Graph class

    public class NodeAttributes
    {
        public string Color_ { get; private set; }
        public int FontSize_ { get; private set; }
        public string Label_ { get; private set; }
        public string Shape_ { get; private set; }
        

        public NodeAttributes Color(string color)
        {
            Color_ = color;
            return this;
        }

        public NodeAttributes Shape(string shape)
        {
            Shape_ = shape;
            return this;
        }

        public NodeAttributes FontSize(int fontSize)
        {
            FontSize_ = fontSize;
            return this;
        }

        public NodeAttributes Label(string label)
        {
            Label_ = label;
            return this;
        }
    }

    public static class NodeShape
    {
        public static string Box = "box";
        public static string Ellipse = "ellipse";
    }

    public class EdgeAttributes
    {
        public string Color_ { get; private set; }
        public int FontSize_ { get; private set; }
        public string Label_ { get; private set; }
        public string Weight_ { get; private set; }

        public EdgeAttributes Color(string color)
        {
            Color_ = color;
            return this;
        }

        public EdgeAttributes Weight(string weight)
        {
            Weight_ = weight;
            return this;
        }

        public EdgeAttributes FontSize(int fontSize)
        {
            FontSize_ = fontSize;
            return this;
        }

        public EdgeAttributes Label(string label)
        {
            Label_ = label;
            return this;
        }
    }

    public class DotGraphBuilder
    {
        public static Graph DirectedGraph(string graphName)
        {
            return new Graph(graphName, true, true);
		}

        public static Graph NondirectedGraph(string graphName)
        {
            return new Graph(graphName, false, true);
        }
    }

    public static class GraphNodeExtensions
    {
        public static GraphNode With(this GraphNode graphNode, Func<NodeAttributes, NodeAttributes> addAttributes)
        {
            var nodeAttributes = addAttributes(new NodeAttributes());

            foreach (var property in nodeAttributes.GetType().GetProperties())
            {
                graphNode.Attributes.Add(property.Name.ToLower().
                    Substring(0, property.Name.Length - 2), property.GetValue(nodeAttributes).ToString());
            }

            return graphNode;   
        }

        public static string Build(this Graph graph)
        {
            return graph.ToDotFormat();
        }
    }

    public static class GraphEdgeExtensions
    {
        public static GraphEdge With(this GraphEdge graphEdge, Func<EdgeAttributes, EdgeAttributes> addAttributes)
        {
            var edgeAttributes = addAttributes(new EdgeAttributes());

            foreach (var property in edgeAttributes.GetType().GetProperties())
            {
                graphEdge.Attributes.Add(property.Name.ToLower().
                    Substring(0, property.Name.Length - 2), property.GetValue(edgeAttributes).ToString());
            }

            return graphEdge;
        }
    }
}