using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UniRx;
using System.Linq;
using MasterData;

public static class FirebaseClient
{

    static DatabaseReference databaseReference;
    static DatabaseReference DataBaseRef
    {
        get
        {
            if (databaseReference == null)
            {
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(Urls.database);
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            }

            return databaseReference;
        }
    }

    public static IObservable<Unit> LoadMaster()
    {
        Debug.LogError("Loading..");
        var subject = new AsyncSubject<Unit>();
        // マスターから全てのシート名を取得，それぞれに対応したListに格納する．
        DataBaseRef.Child("Master").GetValueAsync().ContinueWith(x =>
        {
            if (x.IsFaulted)
            {
                subject.OnError(x.Exception);
            }
            else if (x.IsCompleted)
            {
                DataSnapshot ss = x.Result;
                var masterDataList = ss.Children;
                foreach (var sheet in masterDataList)
                {
                    SetMasterData(sheet);
                }
                subject.OnNext(Unit.Default);
                subject.OnCompleted();
            }
        });
        return subject;
    }

    static void SetMasterData(DataSnapshot ss)
	{
        switch (ss.Key)
        {
            case "food":
                MasterDataStore.foodList = ConvertSnapShotToDataList<Food>(ss);
                break;
        }

	}

    static List<T> ConvertSnapShotToDataList<T>(DataSnapshot ss)
	{
        List<T> dataList = new List<T>();
        foreach (var item in ss.Children)
		{
            dataList.Add(JsonUtility.FromJson<T>(item.GetRawJsonValue()));
		}
        return dataList;
	}

 //   static List<object> ConvertSnapShotToDataList(DataSnapshot ss, System.Type type)
	//{
 //       List<object> dataList = new List<object>();
	//	foreach (var item in ss.Children)
	//	{
	//		string json = item.GetRawJsonValue();
 //           dataList.Add(JsonUtility.FromJson(json, type));
	//	}
	//	return dataList;
	//}

    //static System.Type GetType(string typeName){
    //    return System.Reflection.Assembly.Load("UnityEngine.dll").GetType(typeName);
    //}


}
