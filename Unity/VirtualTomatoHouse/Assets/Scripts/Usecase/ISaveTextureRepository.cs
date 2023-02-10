using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace VirtualTomatoHouse.Scripts.Usecase
{
    public interface ISaveTextureRepository
    {
        public void Save(IEnumerable<AnnotationPair> pairs);
    }
}