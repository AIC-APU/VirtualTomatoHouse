using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

public static class MaterialSetupUtility
{
    private static List<MaterialData> _materialDataList = new List<MaterialData>();
    const string _materialDataFolderPath = "Assets/MaterialData";

    public static void MaterialSetup(string folderPath)
    {
        _materialDataList = SearchMaterialDatas().ToList();

        var materials = SearchMaterials(folderPath);
        var textures = SearchTextures(folderPath);

        //マテリアルの設定
        foreach (Material material in materials)
        {
            var materialData = _materialDataList.FirstOrDefault(data => material.name.StartsWith(data.Header));
            if (materialData == null) continue;

            material.shader = materialData.Shader;

            //他マテリアルの設定あればここに追加


            //テクスチャの設定
            foreach (var textureRule in materialData.TextureRules)
            {
                var texture = textures.FirstOrDefault(tex => tex.name == material.name + textureRule.footer);
                if (texture == null) continue;

                //ノーマルマップの場合はテクスチャタイプを変更
                if (textureRule.textureProperty == MaterialData.TextureProperty._BumpMap || textureRule.textureProperty == MaterialData.TextureProperty._NormalMap)
                {
                    ChangeTextureTypeToNormal(texture);
                }

                material.SetTexture(textureRule.textureProperty.ToString(), texture);
            }
        }
    }

    private static Material[] SearchMaterials(string folderPath)
    {
        Material[] materials = Directory
            .GetFiles(folderPath + "/Materials", "*.mat")
            .Select(filePath => PathConverter.ToAssetPath(filePath))
            .Select(assetPath => AssetDatabase.LoadAssetAtPath<Material>(assetPath))
            .ToArray();

        return materials;
    }

    private static Texture[] SearchTextures(string folderPath)
    {
        Texture[] textures = Directory
            .GetFiles(folderPath + "/Textures", "*.png")
            .Select(filePath => PathConverter.ToAssetPath(filePath))
            .Select(assetPath => AssetDatabase.LoadAssetAtPath<Texture>(assetPath))
            .ToArray();

        return textures;
    }

    private static MaterialData[] SearchMaterialDatas()
    {
        MaterialData[] materialDatas = Directory
            .GetFiles(_materialDataFolderPath, "*.asset")
            .Select(filePath => PathConverter.ToAssetPath(filePath))
            .Select(assetPath => AssetDatabase.LoadAssetAtPath<MaterialData>(assetPath))
            .ToArray();

        return materialDatas;
    }

    private static void ChangeTextureTypeToNormal(Texture texture)
    {
        var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(texture)) as TextureImporter;
        importer.textureType = TextureImporterType.NormalMap;
        importer.SaveAndReimport();
    }
}
