using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;

public class LocalDataRepositoryBase<T>{
    
    public static T Load(){
         return FileService.Load<T>();
    }

    public static void Save(T data){
        FileService.SaveJson(data, typeof(T).Name);
    }
}
