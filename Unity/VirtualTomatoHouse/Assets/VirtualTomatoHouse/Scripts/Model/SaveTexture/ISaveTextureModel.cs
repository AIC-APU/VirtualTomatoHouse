using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Plusplus.VirtualTomatoHouse.Scripts.Model
{
    public interface ISaveTextureModel
    {
        public void SaveTexture(IEnumerable<AnnotationPair> pairs);
    }
}