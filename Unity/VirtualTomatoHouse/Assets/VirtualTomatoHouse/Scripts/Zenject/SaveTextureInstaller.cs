using Zenject;
using Plusplus.VirtualTomatoHouse.Scripts.Logic.Usecase;
using Plusplus.VirtualTomatoHouse.Scripts.Logic.Data;

namespace Plusplus.VirtualTomatoHouse.Scripts.Zenject
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