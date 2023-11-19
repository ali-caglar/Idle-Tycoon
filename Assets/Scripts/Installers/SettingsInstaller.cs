using Datas.ScriptableDatas.Currencies;
using TimeTick;
using UnityEngine;
using Workers;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "Settings Installer", menuName = "Data/Installers/Settings Installer", order = 0)]
    public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
    {
        [SerializeField] private DefaultTimeTickControllersData defaultTimeTickControllersData;
        [SerializeField] private DefaultCurrenciesData defaultCurrenciesData;
        [SerializeField] private WorkersListData workersListData;

        public override void InstallBindings()
        {
            Container.BindInstance(defaultTimeTickControllersData.DefaultControllers);
            Container.BindInstance(defaultCurrenciesData.DefaultCurrencies);
            Container.BindInstance(workersListData.WorkersList);
        }
    }
}