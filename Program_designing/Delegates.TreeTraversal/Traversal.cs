using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.TreeTraversal
{
    /// * Алгоритм:
    ///     1. Заходим в узел
    ///     2. Получаем список ветвей
    ///     3. Возвращаем значение
    ///     4. Обходим все ветви
    /// 
    /// * В стеке лежит TNode (Stack<TNode>)
    /// * Для Job: если SubJob = null, до вывести значение
    /// * Для Products: выводить начинаем с последнего узла
    /// * For BinaryTree: if Left and Right = null => output

    public class TreeTraversal<TNode, TReturnValue>
    {
        public Stack<TNode> Stack = new Stack<TNode>();

        public TreeTraversal(Func<TNode, IEnumerable<TReturnValue>> func)
        {
            Func = func;
        }

        Func<TNode, IEnumerable<TReturnValue>> Func;

        public IEnumerable<TReturnValue> Tranvers(TNode current)
        {
            if (current == null) yield break;

            var r = new List<int>() { 1, 2 };
            var l = new List<int>() { 3, 4 }.Concat(r);
        }
    }

    public static class Traversal<Tv>
    {
        public static Tv Value;

        

        public static IEnumerable<Product> GetProducts(ProductCategory root)
        {
            var treeTraversal = new TreeTraversal<ProductCategory, Product>((node) => node);

            return new List<Product>();
        }

        public static IEnumerable<Job> GetEndJobs(Job root)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> root)
        {
            //var stack = new TreeTraversal<BinaryTree<T>>();

            throw new NotImplementedException();
        }
    }
}
