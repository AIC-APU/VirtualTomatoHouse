using UnityEngine;
using UnityEngine.InputSystem;

namespace Plusplus.VirtualTomatoHouse.Scripts.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraMoverForDebug : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private float _moveSpeed = 1f;
        [SerializeField] private float _rotateSpeed = 0.5f;
        [SerializeField] private float _upSpeed = 1f;
        #endregion

        #region MonoBehaviour Callbacks
        void Update()
        {
            int forward = 0;
            int right = 0;
            int up = 0;

            if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
            {
                if(Keyboard.current.shiftKey.isPressed) up++;
                else forward++;
            }
            if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
            {
                if (Keyboard.current.shiftKey.isPressed) up--;
                else forward--;
            }
            if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            {
                right++;
            }
            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            {
                right--;
            }

            if (forward != 0 || up != 0)
            {
                transform.position += transform.forward * forward * _moveSpeed * Time.deltaTime
                                    + transform.up * up * _upSpeed * Time.deltaTime;
            }

            if (right != 0)
            {
                transform.Rotate(0, right * _rotateSpeed, 0);
            }
        }
        #endregion
    }
}