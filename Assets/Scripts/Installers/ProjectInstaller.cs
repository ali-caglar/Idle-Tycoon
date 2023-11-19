using Save.DataServices;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller<ProjectInstaller>
    {
        private NewtonsoftSerializationService _serializationService = new NewtonsoftSerializationService();

        public override void InstallBindings()
        {
            Container.Bind<ISerializationService>().To<NewtonsoftSerializationService>().FromInstance(_serializationService).NonLazy();
        }
    }
}