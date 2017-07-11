using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UniRx;

public enum StorageMethod
{
	Binary,
	JSON
}

public class SaveDataBase<TKey> : SingletonMonoBehaviour<TKey> where TKey : MonoBehaviour{


	public static void SafeCreateDirectory(string path){
		if(!Directory.Exists(path)){
			//SaveDataフォルダ作成
			Directory.CreateDirectory(path);
		}
	}

	/// <summary>
	/// Loads the data.
	/// </summary>
	/// <returns>The data.</returns>
	/// <param name="fileName">File name.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	protected static T LoadData<T>(string fileName, StorageMethod method){
		T data = JsonUtility.FromJson<T>(ReadJson(fileName, method));
		return data;
	}

	public static void CreateDir(){
		string dirPath = Application.persistentDataPath + "/SaveData/";
		SafeCreateDirectory (dirPath);
	}

	/// <summary>
	/// Reads the json.
	/// </summary>
	static string ReadJson(string fileName, StorageMethod method){
		string json = string.Empty;
		string filePath = GetFilePath(fileName);

		if (!Application.isEditor){
			CreateDir();
		}

//		if (Application.platform == RuntimePlatform.IPhonePlayer) {
//			// ファイルをコピー
//			CopyFile(fileName);
//		}

		try {
			switch (method) {
			case StorageMethod.Binary:
				BinaryFormatter bf = new BinaryFormatter ();
				using (FileStream file = File.Open (filePath, FileMode.Open)) {
					json = (string)bf.Deserialize (file);
					file.Close ();
				}
				break;

			case StorageMethod.JSON:
				json = File.ReadAllText(filePath);
				break;
			}
		} catch (Exception ex) {
			Debug.LogError (ex.Message);
			//Dialog.Instance.Show(ex.Message, ()=>{});
		}
		return json;

	}


	/// <summary>
	/// Saves the json.
	/// </summary>
	protected void SaveJson<T>(T data, string key, StorageMethod method)
	{
		string json = JsonUtility.ToJson (data);
		string[] pathArray = key.Split ('/');

		string path = GetFolderPath();
		if (pathArray.Length > 1) {
			for (int i=0; i < pathArray.Length-1; i++) {
				path += "/" + pathArray[i];
			}
			SafeCreateDirectory (path);
		}

		string filePath = GetFilePath(key);
		Debug.Log (filePath);
		try {
			switch(method){
			case StorageMethod.Binary:
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Create (filePath);
				bf.Serialize (file, json);
				file.Close ();
				break;
			case StorageMethod.JSON:
				File.WriteAllText(filePath, json);
				break;
			}
		} catch (Exception ex) {
			Debug.LogWarning ("エラー: " + ex.Message);
		}

		#if UNITY_EDITOR
		AssetDatabase.Refresh();
		#endif
	}

	static string GetFolderPath () {
		string folderPath = Application.persistentDataPath;
		#if UNITY_EDITOR
		folderPath = Application.dataPath;
		#endif
		folderPath += "/SaveData/";
		SafeCreateDirectory (folderPath);
		return folderPath;
	}

	static string GetFilePath(string key){
		return GetFolderPath () + key;
	}

	public static bool Exists (string key) {
		string path = GetFilePath(key);
		return File.Exists(path);
	}

	// ios用
	void CopyFile(string fileName){
		string baseFilePath = string.Format ("{0}/{1}.json", Application.streamingAssetsPath, fileName);
		string targetFilePath = string.Format ("{0}/{1}.json", Application.persistentDataPath, fileName);



		if (File.Exists (targetFilePath)) {
			var bTime = File.GetCreationTime (baseFilePath);
			var tTime = File.GetCreationTime (targetFilePath);
			if (bTime == tTime) {
				return;
			} else {
				File.Delete (targetFilePath);
			}
		}
		File.Copy (baseFilePath, targetFilePath);
	}

	//Android用
	public void CopyFile(IObserver<Unit> observer, string fileName){
		string baseFilePath = string.Format ("{0}/{1}.json", Application.streamingAssetsPath, fileName);
		string targetFilePath = string.Format ("{0}/{1}.json", Application.persistentDataPath, fileName);

		StartCoroutine(CopyFileCoroutine(observer,  baseFilePath, targetFilePath));
	}

	// Android用コピー
	IEnumerator CopyFileCoroutine(IObserver<Unit> observer,  string basePath, string targetPath){
		WWW www = new WWW(basePath);
		yield return www;

		File.WriteAllBytes(targetPath, www.bytes);
		observer.OnNext (Unit.Default);
		observer.OnCompleted ();
	}




}
