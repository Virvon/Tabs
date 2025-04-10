using Sources.Infrastructure.ServerRequests;
using Zenject;

namespace Sources.Installers
{
    public class ApplicationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ServerRequestQueue>().AsSingle();
        }
    }
}