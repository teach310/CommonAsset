using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDataRepositoryBase<T>
{
    static List<T> dataList;
    public static List<T> DataList
    {
        get {
            if (dataList == null)
            {
                dataList = typeof(MasterDataStore)
                    .GetProperty(typeof(T).Name + "List")
                    .GetValue(null, null) as List<T>;
            }
            return dataList;
        }
    }

    public static List<T> FindAll(){
        return DataList;
    }
}
