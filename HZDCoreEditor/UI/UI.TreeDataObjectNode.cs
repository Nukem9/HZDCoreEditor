﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Utility;

namespace HZDCoreEditor.UI
{
    public class TreeDataObjectNode : TreeDataNode
    {
        public override object Value { get { return ObjectInstance.ToString(); } }

        public override bool HasChildren => Children.Count > 0;
        public override List<TreeDataNode> Children { get; }

        private readonly object ObjectInstance;

        public TreeDataObjectNode(object instance, string name, NodeAttributes attributes)
        {
            if (instance == null)
                throw new ArgumentNullException("Not supported. Use TreeDataClassMemberHolder instead if objects are potentially null.", nameof(instance));

            Name = name;
            TypeName = instance.GetType().Name;

            ObjectInstance = instance;

            if (!attributes.HasFlag(NodeAttributes.HideChildren))
            {
                Children = new List<TreeDataNode>();
                AddObjectChildren();
            }
        }

        private void AddObjectChildren()
        {
            var objectType = ObjectInstance.GetType();

            if (Type.GetTypeCode(objectType) != TypeCode.Object)
                return;

            // Class member variables act as children
            var fields = objectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
                Children.Add(CreateNode(ObjectInstance, new FieldOrProperty(field)));
        }
    }
}
