using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace VirtualTomatoHouse.Scripts.Usecase
{
    public interface ISaveTextureUsecase
    {
        public void SaveTexture(IEnumerable<AnnotationPair> pairs);
    }
}