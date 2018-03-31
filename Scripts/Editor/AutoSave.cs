using System.Collections;

using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;
using UnityEditor;

[InitializeOnLoad]
public class AutoSave
{
	public static readonly string manualSaveKey = "autosave@manualSave";

	static double nextTime = 0;
	static bool isChangedHierarchy = false;

	static AutoSave()
	{
		IsManualSave = true;
		EditorApplication.playModeStateChanged += x =>
		{
			if (IsAutoSave && !EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
			{

				IsManualSave = false;

				if (IsSaveScene)
				{
					Debug.Log("save scene " + System.DateTime.Now);
					EditorSceneManager.SaveOpenScenes();
				}
				IsManualSave = true;
			}
			isChangedHierarchy = false;
		};

		nextTime = EditorApplication.timeSinceStartup + Interval;
		EditorApplication.update += () =>
		{
			if (isChangedHierarchy && nextTime < EditorApplication.timeSinceStartup)
			{
				nextTime = EditorApplication.timeSinceStartup + Interval;

				IsManualSave = false;

				if (IsSaveSceneTimer && IsAutoSave && !EditorApplication.isPlaying)
				{
					if (IsSaveScene)
					{
						Debug.Log("save scene " + System.DateTime.Now);
						EditorSceneManager.SaveOpenScenes();
					}
				}
				isChangedHierarchy = false;
				IsManualSave = true;
			}
		};

		EditorApplication.hierarchyWindowChanged += () =>
		{
			if (!EditorApplication.isPlaying)
				isChangedHierarchy = true;
		};
	}

	public static bool IsManualSave
	{
		get
		{
			return EditorPrefs.GetBool(manualSaveKey);
		}
		private set
		{
			EditorPrefs.SetBool(manualSaveKey, value);
		}
	}


	private static readonly string autoSave = "auto save";
	static bool IsAutoSave
	{
		get
		{
			string value = EditorUserSettings.GetConfigValue(autoSave);
			return !string.IsNullOrEmpty(value) && value.Equals("True");
		}
		set
		{
			EditorUserSettings.SetConfigValue(autoSave, value.ToString());
		}
	}

	private static readonly string autoSaveScene = "auto save scene";
	static bool IsSaveScene
	{
		get
		{
			string value = EditorUserSettings.GetConfigValue(autoSaveScene);
			return !string.IsNullOrEmpty(value) && value.Equals("True");
		}
		set
		{
			EditorUserSettings.SetConfigValue(autoSaveScene, value.ToString());
		}
	}

	private static readonly string autoSaveSceneTimer = "auto save scene timer";
	static bool IsSaveSceneTimer
	{
		get
		{
			string value = EditorUserSettings.GetConfigValue(autoSaveSceneTimer);
			return !string.IsNullOrEmpty(value) && value.Equals("True");
		}
		set
		{
			EditorUserSettings.SetConfigValue(autoSaveSceneTimer, value.ToString());
		}
	}

	private static readonly string autoSaveInterval = "save scene interval";
	static int Interval
	{
		get
		{

			string value = EditorUserSettings.GetConfigValue(autoSaveInterval);
			if (value == null)
			{
				value = "60";
			}
			return int.Parse(value);
		}
		set
		{
			if (value < 60)
				value = 60;
			EditorUserSettings.SetConfigValue(autoSaveInterval, value.ToString());
		}
	}


	[PreferenceItem("Auto Save")]
	static void ExampleOnGUI()
	{
		bool isAutoSave = EditorGUILayout.BeginToggleGroup("auto save", IsAutoSave);


		IsAutoSave = isAutoSave;
		EditorGUILayout.Space();

		IsSaveScene = EditorGUILayout.ToggleLeft("save scene", IsSaveScene);
		IsSaveSceneTimer = EditorGUILayout.BeginToggleGroup("save scene interval", IsSaveSceneTimer);
		Interval = EditorGUILayout.IntField("interval(sec)", Interval);
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.EndToggleGroup();
	}

	[MenuItem("File/Backup/Backup")]
	public static void Backup()
	{
		for (var i = 0; i < EditorSceneManager.sceneCount; ++i)
		{
			string sceneName = EditorSceneManager.GetSceneAt(i).path;
			string expoertPath = "Backup/" + sceneName;

			Directory.CreateDirectory(Path.GetDirectoryName(expoertPath));

			if (string.IsNullOrEmpty(sceneName))
				return;

			byte[] data = File.ReadAllBytes(sceneName);
			File.WriteAllBytes(expoertPath, data);
		}
	}

	[MenuItem("File/Backup/Rollback")]
	public static void RollBack()
	{
		for (var i = 0; i < EditorSceneManager.sceneCount; ++i)
		{
			string sceneName = EditorSceneManager.GetSceneAt(i).path;
			string expoertPath = "Backup/" + sceneName;

			byte[] data = File.ReadAllBytes(expoertPath);
			File.WriteAllBytes(sceneName, data);
			AssetDatabase.Refresh(ImportAssetOptions.Default);
		}
	}

}