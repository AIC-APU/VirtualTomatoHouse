using UnityEngine;
using UnityEditor;

public class FBXImporter : EditorWindow
{
    private const string _infoText = "以下のボタンを押してフォルダを選択すれば、指定フォルダ内のfbxファイルのマテリアルとテクスチャを抽出します。";

    [MenuItem("Tools/FBXImporter")]
    public static void ShowWindow()
    {
        GetWindow<FBXImporter>("FBXImporter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select FBX Folder", EditorStyles.boldLabel);

        GUILayout.Space(10);

        GUILayout.Label(_infoText, EditorStyles.wordWrappedLabel);

        GUILayout.Space(10);

        //フォルダ選択ボタン
        if (GUILayout.Button("Import FBX", GUILayout.Height(50)))
        {
            var folderPath = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, "");

            //例外処理
            if (string.IsNullOrEmpty(folderPath))
            {
                throw new System.ArgumentException("folderPath is null or empty");
            }

            FBXSetupUtility.SetupFBX(folderPath);
            MaterialSetupUtility.MaterialSetup(folderPath);

            //後処理
            AssetDatabase.Refresh();
        }
    }
}
