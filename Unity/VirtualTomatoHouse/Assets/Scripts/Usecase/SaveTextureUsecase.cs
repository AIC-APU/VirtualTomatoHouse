using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

namespace VirtualTomatoHouse.Scripts.Usecase
{
    public class SaveTextureUsecase : ISaveTextureUsecase
    {
        [Inject] readonly ISaveTextureRepository _saveTextureRepository;

        public async UniTask SaveTexture(int width, int height, Camera camera, TextureFormat format, string filePath)
        {
            await _saveTextureRepository.Save(width, height, camera, format, filePath);
        }
    }
}
