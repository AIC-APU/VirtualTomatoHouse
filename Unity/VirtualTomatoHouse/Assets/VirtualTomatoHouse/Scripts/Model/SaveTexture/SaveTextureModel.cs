using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Plusplus.VirtualTomatoHouse.Scripts.Model
{
    public class SaveTextureModel : ISaveTextureModel
    {
        [Inject] readonly ISaveTextureData _saveTextureRepository;

        public void SaveTexture(IEnumerable<AnnotationPair> pairs)
        {
            _saveTextureRepository.Save(pairs);
        }
    }
}
