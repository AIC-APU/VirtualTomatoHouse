using UnityEngine;
using Cysharp.Threading.Tasks;

namespace VirtualTomatoHouse.Scripts.Usecase
{
    public interface ISaveTextureRepository
    {
        public UniTask Save(int width, int height, Camera camera, TextureFormat format, string filePath);
    }
}