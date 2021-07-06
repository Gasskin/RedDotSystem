using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RangeString : IEquatable<RangeString>
{
#region Field
    private string source;

    private int startIndex;

    private int endIndex;

    private int length;

    private bool isSourceNullOrEmpty;

    private int hashCode;
#endregion

#region Constructor
    public RangeString(string source, int startIndex, int endIndex)
    {
        this.source           = source;
        this.startIndex       = startIndex;
        this.endIndex         = endIndex;
        length              = endIndex - startIndex + 1;
        isSourceNullOrEmpty = string.IsNullOrEmpty(source);
        hashCode            = 0;
    }
#endregion


#region Override
    public override int GetHashCode()
    {
        if (hashCode == 0 && !isSourceNullOrEmpty)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                hashCode = 31 * hashCode + source[i];
            }
        }

        return hashCode;
    }

    public override string ToString()
    {
        RedDotManager.instance.cachedStringBuilder.Clear();
        for (int i = startIndex; i <= endIndex; i++)
        {
            RedDotManager.instance.cachedStringBuilder.Append(source[i]);
        }
        string str = RedDotManager.instance.cachedStringBuilder.ToString();

        return str;
    }
#endregion


#region Interface
    public bool Equals(RangeString other)
    {

        bool isOtherNullOrEmpty = string.IsNullOrEmpty(other.source);

        if (isSourceNullOrEmpty && isOtherNullOrEmpty)
        {
            return true;
        }

        if (isSourceNullOrEmpty || isOtherNullOrEmpty)
        {
            return false;
        }

        if (length != other.length)
        {
            return false;
        }

        for (int i = startIndex, j = other.startIndex; i <= endIndex; i++, j++)
        {
            if (source[i] != other.source[j])
            {
                return false;
            }
        }

        return true;
    }
#endregion

    
}
