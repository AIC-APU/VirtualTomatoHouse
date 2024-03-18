using System;
using System.Linq;
using UnityEngine;

namespace Plusplus.VirtualTomatoHouse.Scripts.Tomato
{
    public class RandomTomatoSetter : BaseTomatoSetter
    {
        #region Serialized Private Fields
        [SerializeField] private bool _ajustScale = false;
        [SerializeField, Tooltip("0の時はlocatorの数がトマトの最大数となる")] private int _maxTomatoNum = 0;
        #endregion

        #region MonoBehaviour Callbacks
        void Awake()
        {
            SetTomato();
        }
        #endregion

        #region Protected methods
        protected override void SetTomato()
        {
            var random = new System.Random();

            var tomatoNum = random.Next(0, _locators.Count + 1);
            if (_maxTomatoNum > 0)
            {
                tomatoNum = Math.Min(tomatoNum, _maxTomatoNum);
            }

            var randomLocList = _locators
                                .OrderBy(x => Guid.NewGuid())
                                .Take(tomatoNum)
                                .ToList();

            foreach (Transform locator in randomLocList)
            {
                var tomatoIndex = random.Next(0, _tomatoPrefabs.Count);
                var tomato = Instantiate(_tomatoPrefabs[tomatoIndex], locator);

                if (_ajustScale)
                {
                    tomato.transform.localScale = CalcTomatoScale(locator.position.y);
                }
            }

            randomLocList.Clear();
        }
        #endregion

        #region Private methods
        private Vector3 CalcTomatoScale(float tomatoHeight)
        {
            var scale = 1f;
            if (tomatoHeight > 1)
            {
                scale = -0.5f * tomatoHeight + 1.5f;
            }

            return new Vector3(scale, scale, scale);
        }

        [ContextMenu("Search Locators")]  
        private void SearchLocators()
        {
            _locators.Clear();
            foreach (Transform child in transform)
            {
                if (child.name.Contains("Locator"))
                {
                    _locators.Add(child);
                }
            }
        }
        #endregion
    }

}