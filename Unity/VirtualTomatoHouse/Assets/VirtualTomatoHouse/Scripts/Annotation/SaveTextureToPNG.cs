using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Plusplus.VirtualTomatoHouse.Scripts.Annotation
{
    public class SaveTextureToPNG
    {
        readonly string DefaultDirectory = Path.GetFullPath(UnityEngine.Application.streamingAssetsPath + "/../../../AnnotationImages");

        public void Save(IEnumerable<AnnotationPair> pairs)
        {
            foreach (AnnotationPair pair in pairs)
            {
                //エンコード
                byte[] coloreBytes = pair.Color.texture2d.EncodeToPNG();
                byte[] tagBytes = pair.Tag.texture2d.EncodeToPNG();
                string id = pair.ID.ToString("D4");

                //パス作成
                var colorPath = DefaultDirectory + $"/color/color_{id}.png";
                var tagPath = DefaultDirectory + $"/tag/tag_{id}.png";

                //ディレクトリ作成
                var colorDir = Path.GetDirectoryName(colorPath);
                if (!Directory.Exists(colorDir)) Directory.CreateDirectory(colorDir);
                var tagDir = Path.GetDirectoryName(tagPath);
                if (!Directory.Exists(tagDir)) Directory.CreateDirectory(tagDir);

                //保存
                File.WriteAllBytes(colorPath, coloreBytes);
                File.WriteAllBytes(tagPath, tagBytes);

                //テクスチャの破棄（メモリリーク防止）
                UnityEngine.Object.Destroy(pair.Color.texture2d);
                UnityEngine.Object.Destroy(pair.Tag.texture2d);
            }
        }
    }
}