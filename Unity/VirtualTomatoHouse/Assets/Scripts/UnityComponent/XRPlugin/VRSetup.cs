using UnityEngine;

namespace VirtualTomatoHouse.Scripts.UnityComponet.XRPlugin
{
    public class VRSetup : MonoBehaviour
    {
        #region Private Fields
        private ManualXRControl _manualXRControl;
        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            //VRを起動
            _manualXRControl = new ManualXRControl();
            StartCoroutine(_manualXRControl.StartXRCoroutine());
        }

        private void OnDisable()
        {
            _manualXRControl = new ManualXRControl();
            _manualXRControl.StopXR();
        }
        #endregion
    }
}