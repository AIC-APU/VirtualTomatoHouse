using UnityEngine;

namespace Plusplus.VirtualTomatoHouse.Scripts.Model
{
    public class ColorTexture
    {
        private readonly Texture2D _value;

        public ColorTexture(Texture2D value)
        {
            if (value == null) throw new System.ArgumentNullException(nameof(value));

            _value = value;
        }

        public Texture2D texture2d => _value;
    }
}