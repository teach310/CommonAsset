using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GenericExt
{

    /// <summary>
    /// オブジェクトが指定されたいずれかのオブジェクトと等しいかどうかを返します
    /// </summary>
    public static bool ContainsAny<T>(this T self, params T[] values)
    {
        return values.Any(c => c.Equals(self));
    }
}
