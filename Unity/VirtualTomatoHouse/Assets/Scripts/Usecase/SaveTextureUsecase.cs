using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace VirtualTomatoHouse.Scripts.Usecase
{
    public class SaveTextureUsecase : ISaveTextureUsecase
    {
        [Inject] readonly ISaveTextureRepository _saveTextureRepository;

        public void SaveTexture(IEnumerable<AnnotationPair> pairs)
        {
            _saveTextureRepository.Save(pairs);
        }
    }
}
