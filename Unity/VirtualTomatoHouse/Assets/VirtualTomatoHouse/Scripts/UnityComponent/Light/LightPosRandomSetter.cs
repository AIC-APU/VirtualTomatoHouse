using UnityEngine;

namespace Plusplus.VirtualTomatoHouse.Scripts.UnityComponet.Light
{
    [RequireComponent(typeof(UnityEngine.Light))]
    public class LightPosRandomSetter : MonoBehaviour
    {
        #region Serialized Private Fields
        [SerializeField] private float _lightRangeRadius = 10f;
        #endregion

        #region Private Fields
        private UnityEngine.Light _light;
        #endregion

        #region MonoBehaviour Callbacks
        void Awake()
        {
            _light = GetComponent<UnityEngine.Light>();
        }
        #endregion

        #region Public method
        public void RandomSet()
        {
            _light.transform.position = GetRandomLightPos(_lightRangeRadius);
        }
        #endregion

        #region Private method
        private Vector3 GetRandomLightPos(float radius)
        {
            //原点を中心とする半径radiusの天球上の座標を返す
            var posX = UnityEngine.Random.Range(-radius, radius);

            var rangeZ = Mathf.Sqrt(-posX * posX + radius * radius);
            var posZ = UnityEngine.Random.Range(-rangeZ, rangeZ);

            var posY = Mathf.Sqrt(-posX * posX - posZ * posZ + radius * radius);

            return new Vector3(posX, posY, posZ);
        }
        #endregion
    }

}