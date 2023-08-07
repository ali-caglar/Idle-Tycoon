using Currency;
using TimeTick;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "Settings Installer", menuName = "Data/Installers/Settings Installer", order = 0)]
    public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
    {
        [SerializeField] private DefaultTimeTickControllersData defaultTimeTickControllersData;
        [SerializeField] private DefaultCurrenciesData defaultCurrenciesData;

        public override void InstallBindings()
        {
            Container.BindInstance(defaultTimeTickControllersData.DefaultControllers);
            Container.BindInstance(defaultCurrenciesData.DefaultCurrencies);
        }
    }
}