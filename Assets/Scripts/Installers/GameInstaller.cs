using TimeTick;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo(typeof(TimeTickManager)).AsSingle();
        }
    }
}