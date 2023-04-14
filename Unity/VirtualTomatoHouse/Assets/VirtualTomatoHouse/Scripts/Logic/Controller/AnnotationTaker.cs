using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Plusplus.VirtualTomatoHouse.Scripts.Logic.Usecase;
using Cysharp.Threading.Tasks;

namespace Plusplus.VirtualTomatoHouse.Scripts.Logic.Controller
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
        private List<AnnotationPair> _pairList = new List<AnnotationPair>();
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

        #region  Monobehaviour Callbacks
        void Awake()
        {
            _tagCamera.SetReplacementShader(Shader.Find("IndexTexture"), null);
        }

        void OnDestroy()
        {
            _tagCamera.ResetReplacementShader();
        }
        #endregion

        #region Public method
        public async UniTask StoreAnnotationPair(int id)
        {
            //color,tagカメラの位置を合わせる
            _colorCamera.transform.position = _targetCamera.transform.position;
            _colorCamera.transform.rotation = _targetCamera.transform.rotation;
            _tagCamera.transform.position = _targetCamera.transform.position;
            _tagCamera.transform.rotation = _targetCamera.transform.rotation;

            //テクスチャの取得
            var colorTex2d = await CreateTexture(_width, _height, _colorCamera, TextureFormat.RGB24);
            var tagTex2d = await CreateTexture(_width, _height, _tagCamera, TextureFormat.R8);

            //AnnotationPairに格納
            var pair = new AnnotationPair(new ColorTexture(colorTex2d), new TagTexture(tagTex2d), id);

            _pairList.Add(pair);
        }

        public void Save()
        {
            _saveTextureUsecase.SaveTexture(_pairList);

            _pairList.Clear();
        }
        #endregion

        #region Private method
        private async UniTask<Texture2D> CreateTexture(int width, int height, Camera camera, TextureFormat format)
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

            UnityEngine.Object.Destroy(render);

            //後処理
            RenderTexture.active = cache;
            if (camera.targetTexture != null)
            {
                camera.targetTexture.Release();
            }
            camera.targetTexture = null;

            return texture;
        }
        #endregion
    }

}