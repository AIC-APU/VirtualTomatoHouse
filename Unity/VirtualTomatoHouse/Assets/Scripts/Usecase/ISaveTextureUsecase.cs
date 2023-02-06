using UnityEngine;
using Cysharp.Threading.Tasks;

namespace VirtualTomatoHouse.Scripts.Usecase
{
    public interface ISaveTextureUsecase
    {
        public UniTask SaveTexture(int width, int height, Camera camera, TextureFormat format, string filePath);
    }
}