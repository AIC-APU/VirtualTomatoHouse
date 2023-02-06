using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VirtualTomatoHouse.Scripts.UnityComponet.Camera;
using VirtualTomatoHouse.Scripts.UnityComponet.Light;
using VirtualTomatoHouse.Scripts.Controller;


namespace VirtualTomatoHouse.Scripts.UnityComponet.GUI
{
    public class RandomAnnotationUI : MonoBehaviour
    {
        #region Serialized Private Fields
        [SerializeField] private AnnotationTaker _annotationTaker;
        [SerializeField] private CameraPosRandomSetter _cameraPosSetter;
        [SerializeField] private CameraFovRandomSetter _cameraFovSetter;
        [SerializeField] private LightPosRandomSetter _lightPosSetter;
        [SerializeField] private TextPopUpUI _textUI;

        //あと、撮影用クラス

        [Header("Input Fields")]
        [SerializeField] private TMP_InputField _startNumField;
        [SerializeField] private TMP_InputField _endNumField;
        [SerializeField] private TMP_InputField _widthField;
        [SerializeField] private TMP_InputField _heightField;
        [SerializeField] private TMP_InputField _minFovField;
        [SerializeField] private TMP_InputField _maxFovField;

        [Header("Button")]
        [SerializeField] private Button _startButton;
        #endregion

        #region Private Fields
        private int _startNum = 0;
        private int _endNum = 1;
        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            //InputFieldらの初期設定
            _startNumField.text = _startNum.ToString();
            _endNumField.text = _endNum.ToString();
            _widthField.text = _annotationTaker.Width.ToString();
            _heightField.text = _annotationTaker.Height.ToString();
            _minFovField.text = _cameraFovSetter.MinFov.ToString();
            _maxFovField.text = _cameraFovSetter.MaxFov.ToString();

            //メソッドの登録
            _startNumField.onEndEdit.AddListener(OnEndEditStartNum);
            _endNumField.onEndEdit.AddListener(OnEndEditEndNum);
            _widthField.onEndEdit.AddListener(OnEndEditWidth);
            _heightField.onEndEdit.AddListener(OnEndEditHeight);
            _minFovField.onEndEdit.AddListener(OnEndEditMinFov);
            _maxFovField.onEndEdit.AddListener(OnEndEditMaxFov);

            _startButton.onClick.AddListener(OnClickStart);
        }
        #endregion

        #region Public method
        public async void OnClickStart()
        {
            var directory = Path.GetFullPath(UnityEngine.Application.streamingAssetsPath + "/../../../AnnotationImages");
            var now = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            for (int i = _startNum; i <= _endNum; i++)
            {
                _cameraPosSetter.RandomSet();
                _cameraFovSetter.RandomSet();
                _lightPosSetter.RandomSet();

                await _annotationTaker.TakeColorPhoto(directory + $"/{now}/images", i);
                await _annotationTaker.TakeTagPhoto(directory + $"/{now}/tags", i);
            }

            _textUI.PopUpText($"{directory}に画像が保存されました。");
        }

        public void OnEndEditStartNum(string text)
        {
            if (text == "" || text == "-")
            {
                _startNumField.text = _startNum.ToString();
                return;
            }

            //正の数に変換
            var value = Math.Abs(int.Parse(text));

            //endNumを超えない
            value = Math.Min(value, _endNum);

            //反映
            _startNumField.text = value.ToString();
            _startNum = value;
        }

        public void OnEndEditEndNum(string text)
        {
            if (text == "" || text == "-")
            {
                _endNumField.text = _endNum.ToString();
                return;
            }

            //正の数に変換
            var value = Math.Abs(int.Parse(text));

            //startNumを下回らない
            value = Math.Max(value, _startNum);

            //反映
            _endNumField.text = value.ToString();
            _endNum = value;
        }

        public void OnEndEditWidth(string text)
        {
            if (text == "" || text == "-")
            {
                _widthField.text = _annotationTaker.Width.ToString();
                return;
            }

            //正の数に変換
            var value = Math.Abs(int.Parse(text));

            //反映
            _annotationTaker.Width = value;
            _widthField.text = _annotationTaker.Width.ToString();
        }

        public void OnEndEditHeight(string text)
        {
            if (text == "" || text == "-")
            {
                _heightField.text = _annotationTaker.Height.ToString();
                return;
            }

            //正の数に変換
            var value = Math.Abs(int.Parse(text));

            //反映
            _annotationTaker.Height = value;
            _heightField.text = _annotationTaker.Height.ToString();
        }

        public void OnEndEditMinFov(string text)
        {
            if (text == "" || text == "-")
            {
                _minFovField.text = _cameraFovSetter.MinFov.ToString();
                return;
            }

            //正の数に変換
            var value = Mathf.Abs(float.Parse(text));

            //反映
            _cameraFovSetter.MinFov = value;
            _minFovField.text = ((int)_cameraFovSetter.MinFov).ToString();
        }

        public void OnEndEditMaxFov(string text)
        {
            if (text == "" || text == "-")
            {
                _maxFovField.text = _cameraFovSetter.MaxFov.ToString();
                return;
            }

            //正の数に変換
            var value = Mathf.Abs(float.Parse(text));

            //反映
            _cameraFovSetter.MaxFov = value;
            _maxFovField.text = ((int)_cameraFovSetter.MaxFov).ToString();
        }
        #endregion
    }
}