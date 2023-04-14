using UnityEngine;

namespace Plusplus.VirtualTomatoHouse.Scripts.Logic.Usecase
{
    public class TagTexture
    {
        private readonly Texture2D _value;

        public TagTexture(Texture2D value)
        {
            if (value == null) throw new System.ArgumentNullException(nameof(value));

            _value = value;
        }

        public Texture2D texture2d => _value;
    }
}