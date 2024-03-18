using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

public class FBXSetupUtility
{
    public static void SetupFBX(string folderPath)
    {
        //例外処理
        if (string.IsNullOrEmpty(folderPath))
        {
            throw new System.ArgumentException("folderPath is null or empty");
        }

        //fbxファイルのパスを取得
        string[] fbxPaths = SearchFBXAssetPath(folderPath);

        //全てのfbxファイルのテクスチャとマテリアルを抽出
        foreach (string fbxPath in fbxPaths)
        {
            ExtractMatAndTexFromFBX(fbxPath);
        }
    }


    /// <summary>
    /// フォルダ内全てのfbxファイルのパスを取得する
    /// </summary>
    private static string[] SearchFBXAssetPath(string folderPath)
    {
        string[] fbxPaths = Directory
                        .GetFiles(folderPath, "*.fbx", SearchOption.TopDirectoryOnly)
                        .Select(path => PathConverter.ToAssetPath(path))
                        .ToArray();

        return fbxPaths;
    }

    /// <summary>
    /// fbxファイルのテクスチャとマテリアルを抽出する
    /// </summary>
    private static void ExtractMatAndTexFromFBX(string fbxPath)
    {
        //Importerの取得
        ModelImporter importer = AssetImporter.GetAtPath(fbxPath) as ModelImporter;

        //MaterialLocationをInPrefabに設定
        //これによりマテリアルとテクスチャの抽出ができるようになる
        importer.materialLocation = ModelImporterMaterialLocation.InPrefab;

        //保存先のパスを生成
        string texSaveDir = fbxPath + $"/../Textures";
        string matSaveDir = fbxPath + $"/../Materials";

        //テクスチャとマテリアルの抽出
        importer.ExtractTextures(texSaveDir);
        ExtractMaterials(importer, fbxPath, matSaveDir);

        //fbxファイルのインポート
        importer.SaveAndReimport();

        //後処理
        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(fbxPath, ImportAssetOptions.ForceSynchronousImport);
    }

    /// <summary>
    /// マテリアルを抽出する
    /// </summary>
    private static void ExtractMaterials(ModelImporter importer, string fbxPath, string saveDirectory)
    {
        // 移動先のフォルダが存在しない場合は作成
        if (!Directory.Exists(saveDirectory)) Directory.CreateDirectory(saveDirectory);

        // fbxファイル内の全てのマテリアルを取得
        Material[] materials = AssetDatabase
            .LoadAllAssetsAtPath(fbxPath)
            .OfType<Material>()
            .ToArray();

        //マテリアルの抽出フロー
        foreach (Material material in materials)
        {
            //移動先のパスを生成
            string materialPath = saveDirectory + "/" + material.name + ".mat";

            // 移動先に同名のマテリアルが存在するか確認
            if (File.Exists(materialPath))
            {
                //マテリアルの削除
                AssetDatabase.RemoveObjectFromAsset(material);

                //fbxファイルに既存のマテリアルを設定
                importer.SearchAndRemapMaterials(ModelImporterMaterialName.BasedOnMaterialName, ModelImporterMaterialSearch.RecursiveUp);
            }
            else
            {
                // マテリアルを抽出
                AssetDatabase.ExtractAsset(material, materialPath);

                // マテリアルを指定フォルダに移動
                AssetDatabase.WriteImportSettingsIfDirty(materialPath);
                AssetDatabase.ImportAsset(materialPath, ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }
}
