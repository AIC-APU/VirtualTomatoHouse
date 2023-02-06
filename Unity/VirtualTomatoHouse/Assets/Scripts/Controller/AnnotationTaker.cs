using System.Text.RegularExpressions;
using System.IO;
using System;
using UnityEngine;
using Zenject;
using VirtualTomatoHouse.Scripts.Usecase;
using Cysharp.Threading.Tasks;

namespace VirtualTomatoHouse.Scripts.Controller
{
    public class AnnotationTaker : MonoBehaviour
    {
        #region  Public Fileds
        public int Width
        {
            get { return _width; }
            set
            {
                _width = Mathf.Clamp(value, MIN_WIDTH, MAX_WIDTH);
            }
        }
        public int Height
        {
            get { return _height; }
            set
            {
                _height = Mathf.Clamp(value, MIN_HEIGHT, MAX_HEIGHT);
            }
        }
        #endregion

        #region Serialized Private Fields
        [SerializeField] private Camera _colorCamera;
        [SerializeField] private Camera _tagCamera;
        [SerializeField] private Camera _targetCamera;
        #endregion

        #region Private Fields
        private int _width = 224;
        private int _height = 224;
        #endregion

        #region Readonly Fields
        [Inject] readonly ISaveTextureUsecase _saveTextureUsecase;
        #endregion

        #region Const
        const int MAX_WIDTH = 1024;
        const int MIN_WIDTH = 32;
        const int MAX_HEIGHT = 1024;
        const int MIN_HEIGHT = 32;
        #endregion

        #region Public method
        public async void TakeOnePairForDebug()
        {
            var directory = Path.GetFullPath(UnityEngine.Application.streamingAssetsPath + "/../../../AnnotationImages/Test");
            
            await TakeColorPhoto(directory, 0);
            await TakeTagPhoto(directory, 0);

            Debug.Log($"{directory}に画像が保存されました。");
        }

        public async UniTask TakeColorPhoto(string directory, int fileNum = 0)
        {
            var fileNumString = Math.Abs(fileNum).ToString("D4");

            var filePath = Path.GetFullPath($"{directory}/color_{fileNumString}.png");
            filePath = NumberingFilePath(filePath);

            //指定されたカメラと同じ位置に撮影用カメラを移動させる
            _colorCamera.transform.position = _targetCamera.transform.position;
            _colorCamera.transform.rotation = _targetCamera.transform.rotation;

            //テクスチャの保存
            await _saveTextureUsecase.SaveTexture(_width, _height, _colorCamera, TextureFormat.RGB24, filePath);
        }

        public async UniTask TakeTagPhoto(string directory = "", int fileNum = 0)
        {
            var fileNumString = Math.Abs(fileNum).ToString("D4");

            var filePath = Path.GetFullPath($"{directory}/tag_{fileNumString}.png");
            filePath = NumberingFilePath(filePath);

            //指定されたカメラと同じ位置に撮影用カメラを移動させる
            _tagCamera.transform.position = _targetCamera.transform.position;
            _tagCamera.transform.rotation = _targetCamera.transform.rotation;

            //テクスチャの保存
            await _saveTextureUsecase.SaveTexture(_width, _height, _tagCamera, TextureFormat.R8, filePath);
        }
        #endregion

        #region Private method
        /// <summary>
        /// ディレクトリに同名のファイルがあれば、ファイル名に番号を付けたパスを返す。
        /// </summary>
        private string NumberingFilePath(string filePath)
        {
            //ディレクトリに同名のパスがあれば、それに番号を付けた物を返す。
            var directry = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);

            while (File.Exists($"{directry}/{fileName}{extension}"))
            {
                //同名のファイルが存在した場合、ファイル名の末尾の数字を足す
                fileName = IncrementStringEnd(fileName);
            }

            return $"{directry}/{fileName}{extension}";
        }

        private string IncrementStringEnd(string str)
        {
            //ファイルネームの最後に数字がついているかを判定
            if (EndWithNumber(str))
            {
                var end = str.Substring(str.Length - 1);
                var others = str.Substring(0, str.Length - 1);

                if (Regex.IsMatch(end, "[0-8]"))
                {
                    //末尾が0-8であればその数に1を足す
                    end = (int.Parse(end) + 1).ToString();
                }
                else if (end == "9")
                {
                    //末尾が9であれば上の位の数を増やして、9を0にする
                    others = IncrementStringEnd(others);
                    end = "0";
                }
                str = others + end;
            }
            else
            {
                //ついていないなら1を付ける
                str += "1";
            }

            return str;
        }

        private bool EndWithNumber(string str)
        {
            var pattern = "([0-9]+$)";
            return Regex.IsMatch(str, pattern);
        }
        #endregion
    }

}