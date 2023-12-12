using System;
using System.Linq;
using UnityEngine;

namespace Plusplus.VirtualTomatoHouse.Scripts.View.Tomato
{
    public class RandomTomatoSetter : BaseTomatoSetter
    {
        #region Serialized Private Fields
        [SerializeField] private bool _ajustScale = false;
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

            var tomatoNum = random.Next(0, _locators.Count + 1);;

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