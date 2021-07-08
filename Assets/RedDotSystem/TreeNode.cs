using System;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
#region Field
    /// <summary>
    /// 子节点
    /// </summary>
    private Dictionary<RangeString, TreeNode> children;

    private string _fullPath;
#endregion

#region Event
    /// <summary>
    /// 节点改变时的回调函数
    /// </summary>
    private Action<int> onNodeChanged;
#endregion

#region Property
    /// <summary>
    /// 节点名称
    /// </summary>
    public string name { get; private set; }
    
    /// <summary>
    /// 节点值
    /// </summary>
    public int value { get;       private set; }

    /// <summary>
    /// 父节点
    /// </summary>
    public TreeNode parent { get; private set; }

    public string fullPath
    {
        get
        {
            if (string.IsNullOrEmpty(_fullPath))
            {
                if (parent == null || parent == RedDotManager.instance.root)
                {
                    _fullPath = name;
                }
                else
                {
                    _fullPath = parent.fullPath + RedDotManager.instance.splitChar + name;
                }
            }
            return _fullPath;
        }
    }
#endregion


#region Constructor
    public TreeNode (string name)
    {
        this.name     = name;
        value         = 0;
        onNodeChanged = null;
    }

    public TreeNode (string name, TreeNode parent) : this (name)
    {
        this.parent = parent;
    }
#endregion


#region Method
    public void AddListener (Action<int> callBack)
    {
        onNodeChanged += callBack;
    }

    public void RemoveListener (Action<int> callBack)
    {
        onNodeChanged -= callBack;
    }

    public void RemoveAllListener ()
    {
        onNodeChanged = null;
    }

    public void ChangeValue (int value)
    {
        if (children != null && children.Count != 0) 
            return;
        InternalChangeValue(value);
    }

    public void ChangeValue ()
    {
        int sum = 0;
        if (children != null && children.Count != 0) 
        {
            foreach (var child in children)
            {
                sum += child.Value.value;
            }
        }
        InternalChangeValue (sum);
    }

    public TreeNode GetChild (RangeString key)
    {
        if (children == null)
        {
            return null;
        }
        children.TryGetValue (key, out var node);
        return node;
    }

    public TreeNode AddChild (RangeString key)
    {
        if (children == null)
        {
            children = new Dictionary<RangeString, TreeNode> ();
        }
        else if (children.ContainsKey (key))
        {
            return null;
        }
        var child = new TreeNode (key.ToString(), this);
        children.Add(key,child);
        return child;
    }
    
    public TreeNode GetOrAddChild (RangeString key)
    {
        var child = GetChild (key);
        if (child == null)
        {
            child = AddChild (key);
        }
        return child;
    }

    public bool RemoveChild (RangeString key)
    {
        if (children == null || children.Count == 0)
        {
            return false;
        }
        var child = GetChild (key);

        if (child != null) 
        {
            RedDotManager.instance.MarkDirtyNode (this);
            children.Remove (key);
            RedDotManager.instance.onNodeNumChanged?.Invoke ();
            return true;
        }
        
        return false;
    }

    public void RemoveAllChild ()
    {
        if (children == null || children.Count == 0) 
        {
            return;
        }
        children.Clear();
        RedDotManager.instance.MarkDirtyNode (this);
        RedDotManager.instance.onNodeNumChanged?.Invoke();
    }
#endregion


#region ToolMethod
    private void InternalChangeValue (int value)
    {
        if (this.value==value)
            return;
        this.value = value;
        onNodeChanged?.Invoke(value);
        RedDotManager.instance.MarkDirtyNode (parent);
    }
#endregion
}



















































































