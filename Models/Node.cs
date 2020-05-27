using Foundation;
using System.Collections;
using System.Collections.Generic;

namespace Balsamic.Models
{
    class Node : NSObject, IEnumerator, IEnumerable
    {
        internal delegate void SelectionDelegate();

        private protected readonly List<Node> _nodes = new List<Node>();

        internal event SelectionDelegate Delegate;
        internal string Tag { get; set; } = string.Empty;

        #region Constructors

        public Node() {}

        public Node(string tag)
        {
            Tag = tag;
        }

        public Node(SelectionDelegate selectionDelegate) : this(string.Empty)
        {
            Delegate = selectionDelegate;
        }

        #endregion

        #region Indexing

        internal Node this[int index]
        {
            get => _nodes[index];
            set => _nodes[index] = value;
        }

        internal int Count => _nodes.Count;

        internal bool HasChildren => Count > 0;

        #endregion

        #region Enumerable

        private protected int _position = -1;

        public IEnumerator GetEnumerator()
        {
            _position = -1;
            return this;
        }

        public bool MoveNext()
        {
            _position++;
            return _position < Count;
        }

        public void Reset()
        {
            _position = -1;
        }

        public object Current => _position < 0 ? null : _nodes[_position];

        #endregion

        internal void Add(Node childNode)
        {
            _nodes.Add(childNode);
        }

        internal void Insert(int index, Node childNode)
        {
            _nodes.Insert(index, childNode);
        }

        internal void Remove(Node childNode)
        {
            _nodes.Remove(childNode);
        }

        internal void RemoveAt(int index)
        {
            _nodes.RemoveAt(index);
        }

        internal void Clear()
        {
            _nodes.Clear();
        }
    }
}
