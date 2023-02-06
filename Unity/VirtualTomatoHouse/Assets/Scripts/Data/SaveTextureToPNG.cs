using System.IO;
using UnityEngine;
using VirtualTomatoHouse.Scripts.Usecase;
using Cysharp.Threading.Tasks;

namespace VirtualTomatoHouse.Scripts.Data
{
    public class SaveTextureToPNG : ISaveTextureRepository
    {
        public async UniTask Save(int width, int height, Camera camera, TextureFormat format, string filePath)
        {
            var texture = new Texture2D(width, height, format, false);
            var render = new RenderTexture(width, height, 24);

            //cameraの映像を反映するRenderTextureを設定
            if (camera.targetTexture != null)
            {
                camera.targetTexture.Release();
            }
            camera.targetTexture = render;

            //1フレーム待つ
            await UniTask.Yield();

            //RenderTextureと同じ画像をTextureにコピー
            var cache = RenderTexture.active;
            RenderTexture.active = render;
            texture.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
            texture.Apply();

            //エンコード
            byte[] PNGBytes = texture.EncodeToPNG();

            //拡張子のつけ忘れ防止
            if (!filePath.EndsWith(".png")) filePath += ".png";

            //ディレクトリ作成
            var directryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directryName))
            {
                //savePathのディレクトリがなければ作る
                Directory.CreateDirectory(directryName);
            }

            //保存
            File.WriteAllBytes(filePath, PNGBytes);

            //テクスチャの破棄（メモリリーク防止）
            Object.Destroy(texture);
            Object.Destroy(render);

            //後処理
            RenderTexture.active = cache;
            if (camera.targetTexture != null)
            {
                camera.targetTexture.Release();
            }
            camera.targetTexture = null;
        }
    }
}