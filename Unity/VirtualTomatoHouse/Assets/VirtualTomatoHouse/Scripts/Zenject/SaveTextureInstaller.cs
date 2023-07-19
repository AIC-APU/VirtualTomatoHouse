using Zenject;
using Plusplus.VirtualTomatoHouse.Scripts.Model;
using Plusplus.VirtualTomatoHouse.Scripts.Data;

namespace Plusplus.VirtualTomatoHouse.Scripts.Zenject
{
    public class SaveTextureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
            .Bind<ISaveTextureModel>()
            .To<SaveTextureModel>()
            .AsSingle();

            Container
            .Bind<ISaveTextureData>()
            .To<SaveTextureToPNG>()
            .AsSingle();
        }
    }
}