using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {

    /// <summary>
    ///  名前空間こみでクラス名からタイプを取得
    /// </summary>
    /// <returns>The type.</returns>
    /// <param name="typeName">Type name.</param>
    public static System.Type GetType(string typeName){
        return System.Reflection.Assembly.Load("Assembly-CSharp").GetType(typeName);
    }
}
