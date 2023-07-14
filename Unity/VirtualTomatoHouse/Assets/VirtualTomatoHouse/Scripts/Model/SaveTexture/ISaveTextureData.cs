using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Plusplus.VirtualTomatoHouse.Scripts.Model
{
    public interface ISaveTextureData
    {
        public void Save(IEnumerable<AnnotationPair> pairs);
    }
}