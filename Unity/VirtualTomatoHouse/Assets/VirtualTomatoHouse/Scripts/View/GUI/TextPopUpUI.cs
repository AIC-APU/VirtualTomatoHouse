using System;
using System.Threading;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

namespace Plusplus.VirtualTomatoHouse.Scripts.View.GUI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TextPopUpUI : MonoBehaviour
    {
        //いずれPresenterのReactiveProperty<string>を監視してポップアップさせたい。

        #region Serilaized Private Fields
        [Header("Text")]
        [SerializeField] private TMP_Text _text;

        [Header("Main Settings")]
        [SerializeField, Range(1, 10)] private int _fadeInTime = 1;
        [SerializeField, Range(1, 10)] private int _popupTime = 3;
        [SerializeField, Range(1, 10)] private int _fadeOutTime = 1;

        [Header("Roop Settings")]
        [SerializeField] private bool _isRoop = false;
        [SerializeField, Range(1, 10)] private int _repeatInterval = 10;
        #endregion

        #region Private Fields
        private CanvasGroup _canvasGroup;
        private CancellationTokenSource _cancellationTokenSource;
        #endregion

        #region MonoBehaviour Callbacks
        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;

            _cancellationTokenSource = new CancellationTokenSource();
        }

        void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
        }
        #endregion

        #region Public method
        public void PopUpText(string text)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            _ = PopUpTask(text, _cancellationTokenSource.Token);
        }
        #endregion

        #region Private method
        private async UniTask PopUpTask(string text, CancellationToken ct = default)
        {
            while (true)
            {
                //最初は非表示
                _canvasGroup.alpha = 0;
                _text.text = text;

                //フェードで表示
                var time = 0f;
                while (_canvasGroup.alpha < 1)
                {
                    time += Time.deltaTime;
                    time = Mathf.Clamp(time, 0, _fadeInTime);
                    _canvasGroup.alpha = time / _fadeInTime;
                    await UniTask.Yield(PlayerLoopTiming.Update, ct);
                }

                //数秒待つ
                await UniTask.Delay(TimeSpan.FromSeconds(_popupTime), false, PlayerLoopTiming.Update, ct);

                //フェードで非表示
                time = _fadeOutTime;
                while (_canvasGroup.alpha > 0)
                {
                    time -= Time.deltaTime;
                    time = Mathf.Clamp(time, 0, _fadeOutTime);
                    _canvasGroup.alpha = time / _fadeOutTime;
                    await UniTask.Yield(PlayerLoopTiming.Update, ct);
                }

                //ループ分岐
                if (!_isRoop)
                {
                    break;
                }
                else
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(_repeatInterval), false, PlayerLoopTiming.Update, ct);
                }
            }
        }
        #endregion
    }
}