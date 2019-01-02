using System;
using ReactiveData.Sequence.IndexedList;

namespace ReactiveData.Sequence
{
    public class IndexedListOnSequence<T>
    {
        private readonly IIndexedList<T> _indexedList;
        private readonly ISequence<T> _rootSequence;
        private readonly Node _rootNode;


        public IndexedListOnSequence(IIndexedList<T> indexedList, ISequence<T> rootSequence)
        {
            _indexedList = indexedList;
            _rootSequence = rootSequence;

            _rootNode = Node.CreateNode(this, rootSequence, 0);
        }

        public IIndexedList<T> IndexedList => _indexedList;

        private int GetStartIndex(Node findNode)
        {
            int itemsBefore = 0;
            if (GetStartIndex(_rootNode, ref itemsBefore, findNode))
                return itemsBefore;

            throw new InvalidOperationException("Node unexpectedly not found");
        }

        private bool GetStartIndex(Node currNode, ref int itemsBefore, Node findNode)
        {
            if (ReferenceEquals(currNode, findNode))
                return true;
            else {
                Node[] childNodes = currNode.ChildNodes;

                if (childNodes == null)
                    itemsBefore += currNode.CurrDescendentItemCount;
                else {
                    for (var i = 0; i < childNodes.Length; i++) {
                        if (GetStartIndex(childNodes[i], ref itemsBefore, findNode))
                            return true;
                    }
                }

                return false;
            }
        }

        private class Node
        {
            private Node[] _childNodes;
            private int _currDescendentItemCount;

            internal static Node CreateNode(IndexedListOnSequence<T> indexedListOnSequence,
                ISequence<T> sequence, int currStartIndex)
            {
                if (sequence is INonreactiveSequence<T> nonreactiveSequence)
                    return new NonreactiveNode(indexedListOnSequence, nonreactiveSequence, currStartIndex);
                else if (sequence is IReactiveSequence<T> reactiveSequence)
                    return new ReactiveNode(indexedListOnSequence, reactiveSequence, currStartIndex);
                else throw new InvalidOperationException($"Unknown Sequence type: {sequence.GetType().FullName}");
            }

            protected Node()
            {
                _currDescendentItemCount = 0;
            }

            protected void UpdateFromSequence(IndexedListOnSequence<T> indexedListOnSequence, INonreactiveSequence<T> sequence, int currStartIndex)
            {
                IIndexedList<T> indexedList = indexedListOnSequence.IndexedList;

                if (sequence is ParentSequence<T> parentSequence)
                {
                    SequenceImmutableArray<ISequence<T>> childSequences = parentSequence.Children;

                    // Remove the old items, if there are any
                    if (_currDescendentItemCount > 0) {
                        indexedList.Update(currStartIndex, _currDescendentItemCount, null);
                        _currDescendentItemCount = 0;
                    }

                    int childSequencesLength = childSequences.Count;
                    _childNodes = new Node[childSequencesLength];
                    for (int i = 0; i < childSequencesLength; i++)
                    {
                        Node childNode = CreateNode(indexedListOnSequence, childSequences[i], currStartIndex);
                        int childNodeDescendentItemCount = childNode.CurrDescendentItemCount;

                        _childNodes[i] = childNode;
                        _currDescendentItemCount += childNodeDescendentItemCount;
                        currStartIndex += childNodeDescendentItemCount;
                    }
                }
                else if (sequence is IItemsSequence<T> itemsSequence)
                {
                    indexedList.Update(currStartIndex, _currDescendentItemCount, itemsSequence.Items.GetArray());

                    _childNodes = null;
                    _currDescendentItemCount = itemsSequence.ItemCount;
                } else throw new InvalidOperationException($"Unknown Sequence type: {sequence.GetType().FullName}");
            }

            public Node[] ChildNodes => _childNodes;

            public int CurrDescendentItemCount => _currDescendentItemCount;
        }

        private class NonreactiveNode : Node
        {
            public NonreactiveNode(IndexedListOnSequence<T> indexedListOnSequence, INonreactiveSequence<T> sequence, int currStartIndex)
            {
                UpdateFromSequence(indexedListOnSequence, sequence, currStartIndex);
            }
        }

        private class ReactiveNode : Node
        {
            private readonly IndexedListOnSequence<T> _indexedListOnSequence;
            private readonly IReactiveSequence<T> _reactiveSequence;

            public ReactiveNode(IndexedListOnSequence<T> indexedListOnSequence, IReactiveSequence<T> reactiveSequence, int currStartIndex)
            {
                _indexedListOnSequence = indexedListOnSequence;
                _reactiveSequence = reactiveSequence;

                UpdateFromSequence(indexedListOnSequence, reactiveSequence.Value, currStartIndex);
                reactiveSequence.Changed += OnSequenceChanged;
            }

            private void OnSequenceChanged()
            {
                int itemsBeforeMe = _indexedListOnSequence.GetStartIndex(this);
                UpdateFromSequence(_indexedListOnSequence, _reactiveSequence.Value, itemsBeforeMe);
            }
        }
    }
}