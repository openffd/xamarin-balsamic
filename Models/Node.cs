using AppKit;
using Foundation;
using System.Collections;
using System.Collections.Generic;

namespace Balsamic.Models
{
    class Node : NSObject, IEnumerator, IEnumerable
    {
        private protected readonly List<Node> _nodes = new List<Node>();

        internal string Title { get; set; } = string.Empty;
        internal new string Description { get; set; } = string.Empty;
        internal NSImage Image { get; set; }
        internal string Tag { get; set; } = string.Empty;

        public Node() {}

        public Node(string title, string description, NSImage image, string tag, SelectionDelegate selectionDelegate)
        {
            Title = title;
            Description = description;
            Image = image;
            Tag = tag;
            Delegate = selectionDelegate;
        }

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

        private int _position = -1;

        public IEnumerator GetEnumerator()
        {
            _position = -1;
            return this as IEnumerator;
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

        internal delegate void SelectionDelegate();
        internal event SelectionDelegate Delegate;

        internal void RaiseSelectedEvent()
        {
            Delegate?.Invoke();
        }
    }

    class OutlineViewNode : Node
    {
        internal bool IsHello()
        {
            
        }
    }
}
