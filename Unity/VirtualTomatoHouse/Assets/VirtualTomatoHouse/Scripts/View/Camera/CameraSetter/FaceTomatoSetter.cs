using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Plusplus.VirtualTomatoHouse.Scripts.View.Camera
{
    public class FaceTomatoSetter : BaseCameraSetter
    {
        #region Serialized Fields
        [Header("For Debug")]
        [SerializeField] private Transform _tomatoTest;
        #endregion

        #region Const
        private const float FLUCTUATION_FORWARD = 0.5f;
        private const float FLUCTUATION_RIGHT = 0.1f;
        private const float FLUCTUATION_UP = 0.1f;
        #endregion

        #region Monobehaviour Callbacks
        void Awake()
        {
            foreach (Transform rangeBoxObj in _rangeBoxObjects)
            {
                //rangeBox はゲーム中非表示にしたい。
                //rangeBoxObj.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        void Update()
        {
            
        }
        #endregion

        #region Public Methods
        [ContextMenu("SetCamera")]
        public override void SetCamera()
        {
            SetCameraPosAndAng(_camera);
            SetCameraFov(_camera);
        }
        #endregion

        #region Protected Methods
        protected override void SetCameraFov(UnityEngine.Camera camera)
        {
            //カメラのfovを指定
            camera.fieldOfView = Random.Range(minFov, maxFov);
        }

        protected override void SetCameraPosAndAng(UnityEngine.Camera camera)
        {
            //カメラの基準位置を計算
            var pos = CalcNearestPosInBox(_tomatoTest.position, _rangeBoxObjects);
            camera.transform.position = pos;

            //カメラの向きを設定
            camera.transform.LookAt(_tomatoTest.position);

            //位置に揺らぎを持たせる
            pos += camera.transform.forward * Random.Range(-FLUCTUATION_FORWARD, 0);
            pos += camera.transform.right * Random.Range(-FLUCTUATION_RIGHT, FLUCTUATION_RIGHT);
            pos += camera.transform.up * Random.Range(-FLUCTUATION_UP, FLUCTUATION_UP);
            
            //カメラの位置を設定
            camera.transform.position = pos;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// targetと最も近いrangeBox内の点の位置を計算する
        /// </summary>
        private Vector3 CalcNearestPosInBox(Vector3 target, IEnumerable<Transform> rangeBoxes)
        {
            //targetと最も近い位置にあるrangeBoxを取得
            var rangeBox = rangeBoxes
                .OrderBy(rangeBox => Vector3.Distance(target, rangeBox.position))
                .First();

            //targetとrangeBox間のベクトルを取得
            var vec = target - rangeBox.position;

            //rangeBoxの各軸の最大値と最小値の大きさを取得
            var maxForwad = (rangeBox.forward * rangeBox.localScale.z / 2f).magnitude;
            var minForwad = -maxForwad;
            var maxRight = (rangeBox.right * rangeBox.localScale.x / 2f).magnitude;
            var minRight = -maxRight;
            var maxUp = (rangeBox.up * rangeBox.localScale.y / 2f).magnitude;
            var minUp = -maxUp;

            //vecをrangeBoxの各軸の最大値と最小値の間に収める
            var clampedForward = ClampVector(vec, rangeBox.forward, minForwad, maxForwad);
            var clampedRight = ClampVector(vec, rangeBox.right, minRight, maxRight);
            var clampedUp = ClampVector(vec, rangeBox.up, minUp, maxUp);

            return rangeBox.position + clampedForward + clampedRight + clampedUp;
        }

        private Vector3 ClampVector(Vector3 vec, Vector3 axis, float min, float max)
        {
            //axisを正規化
            axis = axis.normalized;

            //vecのaxis軸の値をminとmaxの間に収める
            var clampedVec = axis * Mathf.Clamp(Vector3.Dot(vec, axis), min, max);
            return clampedVec;
        }
        #endregion
    }
}
