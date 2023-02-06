using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VirtualTomatoHouse.Scripts.UnityComponet.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraMoverForDebug : MonoBehaviour
    {
        #region Readonly Fields
        readonly float _moveSpeed = 1f;
        readonly float _rotateSpeed = 0.5f;
        #endregion

        #region MonoBehaviour Callbacks
        void Update()
        {
            int forward = 0;
            int right = 0;

            if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
            {
                forward++;
            }
            if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
            {
                forward--;
            }
            if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            {
                right++;
            }
            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            {
                right--;
            }

            if (forward != 0)
            {
                transform.position += transform.forward * forward * _moveSpeed * Time.deltaTime;
            }

            if (right != 0)
            {
                transform.Rotate(0, right * _rotateSpeed, 0);
            }


        }
        #endregion
    }

}