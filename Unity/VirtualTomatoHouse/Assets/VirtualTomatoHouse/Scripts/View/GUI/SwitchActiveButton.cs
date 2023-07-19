using UnityEngine;
using UnityEngine.UI;

namespace Plusplus.VirtualTomatoHouse.Scripts.View.GUI
{
    [RequireComponent(typeof(Button))]
    public class SwitchActiveButton : MonoBehaviour
    {
        #region  Serialized Private Fields
        [Tooltip("ボタンを押すことでtargetのActiveを切り替えます")]
        [SerializeField] private GameObject _target;
        #endregion

        #region MonoBehaviour Callbacks
        void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(() => SwhitchActive(_target));
        }
        #endregion

        #region private Method
        private void SwhitchActive(GameObject obj)
        {
            try
            {
                obj.SetActive(!obj.activeSelf);
            }
            catch (System.NullReferenceException ex)
            {
                Debug.LogError(ex);
                return;
            }
        }
        #endregion
    }
}