using Zenject;
using VirtualTomatoHouse.Scripts.Usecase;
using VirtualTomatoHouse.Scripts.Data;

namespace VirtualTomatoHouse.Scripts.Zenject
{
    public class SaveTextureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
            .Bind<ISaveTextureUsecase>()
            .To<SaveTextureUsecase>()
            .AsSingle();

            Container
            .Bind<ISaveTextureRepository>()
            .To<SaveTextureToPNG>()
            .AsSingle();
        }
    }
}