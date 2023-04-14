using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Plusplus.VirtualTomatoHouse.Scripts.Logic.Usecase
{
    public interface ISaveTextureUsecase
    {
        public void SaveTexture(IEnumerable<AnnotationPair> pairs);
    }
}