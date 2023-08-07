using Currency;
using TimeTick;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(CurrencyManager)).AsSingle();
            Container.BindInterfacesAndSelfTo(typeof(TimeTickManager)).AsSingle();
        }
    }
}