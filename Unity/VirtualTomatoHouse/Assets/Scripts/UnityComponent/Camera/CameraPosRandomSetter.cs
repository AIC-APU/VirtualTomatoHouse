using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualTomatoHouse.Scripts.UnityComponet.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraPosRandomSetter : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Range Boxes")]
        [SerializeField] private List<Transform> _rangeBoxObjects;
        #endregion

        #region private struct
        private struct Range
        {
            public float maxX;
            public float minX;
            public float maxY;
            public float minY;
            public float maxZ;
            public float minZ;

            public Range(float maxX, float minX, float maxY, float minY, float maxZ, float minZ)
            {
                this.maxX = maxX;
                this.minX = minX;
                this.maxY = maxY;
                this.minY = minY;
                this.maxZ = maxZ;
                this.minZ = minZ;
            }
        }
        #endregion

        #region Private Fields
        private UnityEngine.Camera _camera;
        #endregion

        #region const
        const float MAX_ANGLE_X = 30f;
        const float MIN_ANGLE_X = -30f;
        const float MAX_ANGLE_Y = 359f;
        const float MIN_ANGLE_Y = 0f;
        #endregion

        #region  Monobehaviour Callbacks
        void Awake()
        {
            _camera = GetComponent<UnityEngine.Camera>();

            foreach(Transform rangeBoxObj in _rangeBoxObjects)
            {
                //rangeBox はゲーム中非表示にしたい。
                rangeBoxObj.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        #endregion

        #region Public method
        public void RandomSet()
        {
            //ランダムにカメラを配置
            var (pos, angle) = GetRandomPosAndAngle(_rangeBoxObjects);
            _camera.transform.SetPositionAndRotation(pos, Quaternion.Euler(angle));
        }
        #endregion

        #region Private method
        private (Vector3 pos, Vector3 angle) GetRandomPosAndAngle(List<Transform> rangeBoxes)
        {
            var randomIndex = UnityEngine.Random.Range(0, rangeBoxes.Count);
            return GetRandomPosAndAngle(rangeBoxes[randomIndex]);
        }

        private (Vector3 pos, Vector3 angle) GetRandomPosAndAngle(Transform rangeBox)
        {
            //範囲を決める
            var range = GetRnage(rangeBox);

            //カメラの位置を範囲内でランダムで決定する
            var posX = UnityEngine.Random.Range(range.minX, range.maxX);
            var posY = UnityEngine.Random.Range(range.minY, range.maxY);
            var posZ = UnityEngine.Random.Range(range.minZ, range.maxZ);
            var pos = new Vector3(posX, posY, posZ);

            //カメラの向きを条件内でランダムに決定する
            var angleX = UnityEngine.Random.Range(MIN_ANGLE_X, MAX_ANGLE_X);
            var angleY = UnityEngine.Random.Range(MIN_ANGLE_Y, MAX_ANGLE_Y);
            var angleZ = 0f;
            var angle = new Vector3(angleX, angleY, angleZ);

            return (pos, angle);
        }

        private Range GetRnage(Transform rangeBox)
        {
            var maxPos = rangeBox.position
                + rangeBox.right * rangeBox.localScale.x / 2f
                + rangeBox.up * rangeBox.localScale.y / 2f
                + rangeBox.forward * rangeBox.localScale.z / 2f;

            var minPos = rangeBox.position
                - rangeBox.right * rangeBox.localScale.x / 2f
                - rangeBox.up * rangeBox.localScale.y / 2f
                - rangeBox.forward * rangeBox.localScale.z / 2f;

            return new Range(maxPos.x, minPos.x, maxPos.y, minPos.y, maxPos.z, minPos.z);
        }
        #endregion
    }

}