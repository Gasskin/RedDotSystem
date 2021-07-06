using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RedDotManager 
{
#region Field
    private static RedDotManager _instance;

    private Dictionary<string, TreeNode> allNodes;
#endregion

#region Property
    public static RedDotManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RedDotManager ();
            }
            return _instance;
        }
    }
    
    public char splitChar { get; private set; }
    
    public StringBuilder cachedStringBuilder { get; private set; }
    
    public TreeNode root { get; private set; }
#endregion


#region Constructor
    public RedDotManager ()
    {
        splitChar           = '.';
        allNodes            = new Dictionary<string, TreeNode> ();
        root                = new TreeNode ("Root");
        cachedStringBuilder = new StringBuilder ();
    }
#endregion


#region Method
    public TreeNode GetTreeNode (string path)
    {
        if (string.IsNullOrEmpty(path))
            return null;
        
        if (allNodes.TryGetValue (path, out var node))
            return node;

        var cur    = root;
        int length = path.Length;

        int startIndex = 0;

        for (int i = 0; i < length; i++)
        {
            if (path[i].Equals (splitChar))
            {
                if (i == length - 1)
                    throw new Exception ("路径非法，不能以分隔符结尾：" + path);

                int endIndex = i - 1;
                if (endIndex < startIndex)
                    throw new Exception ("路径非法，不能存在连续分隔符或以分隔符开头：" + path);

                TreeNode child = cur.GetOrAddChild (new RangeString (path, startIndex, endIndex));

                startIndex = i + 1;

                cur = child;
            }
        }

        TreeNode target = cur.GetOrAddChild (new RangeString (path, startIndex, length - 1));
        
        allNodes.Add(path,target);

        return target;
    }

    public bool RemoveTreeNode (string path)
    {
        return false;
    }

    public void RemoveAllTreeNode ()
    {
        root.RemoveAllChild();
        allNodes.Clear();
    }
    
    public TreeNode AddListener (string path, Action<int> callBack)
    {
        if (callBack == null)
        {
            return null;
        }
        var node = GetTreeNode (path);
        node.AddListener(callBack);
        return node;
    }

    public void RemoveListener (string path, Action<int> callBack)
    {
        if (callBack == null) 
        {
            return;
        }
        GetTreeNode(path).RemoveListener(callBack);
    }

    public void RemoveAllListener (string path)
    {
        GetTreeNode(path).RemoveAllListener();
    }

    public void ChangeValue (string path, int value)
    {
        GetTreeNode(path).ChangeValue(value);
    }

    public int GetValue (string path)
    {
        var node = GetTreeNode (path);
        return node == null ? 0 : node.value;
    }
#endregion
}
