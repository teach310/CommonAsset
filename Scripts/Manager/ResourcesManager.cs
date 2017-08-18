using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class ResourcesManager : SingletonMonoBehaviour<ResourcesManager>
{

    // Resourcesフォルダから指定したPathのAssetをロード
    T LoadAsset<T>(string path) where T : Object
    {
        T asset = Resources.Load<T>(path);
        if (!asset)
        {
            Debug.LogError(path + "が取得できませんでした");
        }
        return asset;
    }

	static readonly string dialogPath = "Dialog/";
	public GameObject GetDialog(string name)
	{
		GameObject target;
		string path = dialogPath + name;
		target = LoadAsset<GameObject>(path);
		return target;
	}

    private readonly string windowPath = "Prefabs/Window/";
    public GameObject GetWindow(string name)
    {
        GameObject target;
        string path = windowPath + name;
        target = LoadAsset<GameObject>(path);
        return target;
    }

    private readonly string screenPath = "Prefabs/Screen/";
    public GameObject GetScreen(string name)
    {
        GameObject target;
        string path = screenPath + name;
        target = LoadAsset<GameObject>(path);
        return target;
    }

    public readonly string modelDir = "Prefabs/Model/";

    /// <summary>
    /// Resourcesからキャラクターのプレハブを取得
    /// </summary>
    /// <returns>The model.</returns>
    /// <param name="charaID">Chara I.</param>
    public GameObject GetModel(string charaID)
    {
        GameObject targetModel;
        string modelPath = modelDir + charaID;
        targetModel = LoadAsset<GameObject>(modelPath);
        return targetModel;
    }

    public AudioClip GetBGM(string path)
    {
        string audioPath = string.Format("Audio/BGM/{0}", path);
        return GetAudio(audioPath);
    }

    public AudioClip GetSE(string path)
    {
        string audioPath = string.Format("Audio/SE/{0}", path);
        return GetAudio(audioPath);
    }

    AudioClip GetAudio(string path)
    {
        return LoadAsset<AudioClip>(path);
    }

    // Resourcesフォルダからエフェクトを取得
    public GameObject GetEffect(string fileName)
    {
        GameObject go = LoadAsset<GameObject>("Prefabs/Effects/" + fileName);
        return go;
    }
}
