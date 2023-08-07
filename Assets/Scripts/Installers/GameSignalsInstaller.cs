using Currency;
using Zenject;

namespace Installers
{
    public class GameSignalsInstaller : MonoInstaller<GameSignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<CurrencyChangedSignal>();
        }
    }
}