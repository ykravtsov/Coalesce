﻿using IntelliTect.Coalesce.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IntelliTect.Coalesce.Helpers.IncludeTree
{
    /// <summary>
    ///    Represents a hierarchy of entity properties which should be included in results sent to clients.
    /// </summary>
    public class IncludeTree : IReadOnlyDictionary<string, IncludeTree>
    {
        private Dictionary<string, IncludeTree> _children = new Dictionary<string, IncludeTree>();

        public string PropertyName { get; set; }

        public void AddChild(IncludeTree tree)
        {
            if (_children.ContainsKey(tree.PropertyName))
            {
                // Recursively merge into the existing tree.
                foreach (IncludeTree child in tree.Values)
                {
                    _children[tree.PropertyName].AddChild(child);
                }
            }
            else
            {
                _children[tree.PropertyName] = tree;
            }
        }

        public static IncludeTree ParseMemberExpression(MemberExpression expr, out IncludeTree tail)
        {
            var newNode = tail = new IncludeTree();

            newNode.PropertyName = expr.Member.Name;
            var head = newNode;

            // If this lambda was a multi-level property specifier, walk up the chain and add each property as the parent of currentNode.
            // For example, .Include(e => e.Application.ApplicationType)
            while (expr.Expression.NodeType != ExpressionType.Parameter)
            {
                newNode = new IncludeTree();

                newNode.AddChild(head);

                expr = ((MemberExpression)expr.Expression);
                newNode.PropertyName = expr.Member.Name;
                head = newNode;
            }

            return head;
        }

        #region IReadOnlyDictionary

        public bool ContainsKey(string key)
        {
            return _children.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, IncludeTree>> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        public bool TryGetValue(string key, out IncludeTree value)
        {
            return _children.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        public IncludeTree this[string key]
        {
            get
            {
                if (!_children.ContainsKey(key)) return null;
                return _children[key];
            }
        }

        public int Count => _children.Count;
        public IEnumerable<string> Keys => _children.Keys;
        public IEnumerable<IncludeTree> Values => _children.Values;

        #endregion
    }
}