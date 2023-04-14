using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Plusplus.VirtualTomatoHouse.Scripts.Logic.Usecase
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
