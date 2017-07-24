using UnityEngine;
using System.IO;
using System.Text;
using UnityEditor;

public static class ScriptCreater{

	//テンプレートがあるディレクトリへのパス
    public static readonly string TEMPLATE_SCRIPT_DIRECTORY_PATH = "/Common/Scripts/ScriptCreater/Templates/";

    public static void Create(string templateName, string filePath)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);

        if (File.Exists(filePath)) {
            return;
        }

        string scriptText = LoadTemplate(templateName);

        // 各項目を置換
        scriptText = scriptText.Replace("#SCRIPTNAME#", fileName);

        ExportScript(scriptText, filePath);
    }

    // テンプレート読み込み
    static string LoadTemplate(string templateName){
        var templatePath = string.Format("{0}{1}{2}.txt",Application.dataPath, TEMPLATE_SCRIPT_DIRECTORY_PATH, templateName);
		StreamReader streamReader = new StreamReader(templatePath, Encoding.GetEncoding("Shift_JIS"));
		if (streamReader == null)
		{
			Debug.LogError("テンプレートがありません");
            return string.Empty;
		}
        return streamReader.ReadToEnd();
    }

    // 文字列をスクリプトとして書き出し
    static void ExportScript(string text, string path){
        DirectoryUtils.SafeCreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, text, Encoding.UTF8);
        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
    }
}
