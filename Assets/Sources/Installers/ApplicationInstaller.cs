using Sources.Infrastructure.ServerRequests;
using Sources.Infrastructure.Weather;
using Sources.UI.Weather;
using Zenject;

namespace Sources.Installers
{
    public class ApplicationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ServerRequestQueue>().AsSingle();
            Container.BindInterfacesAndSelfTo<WeatherModel>().AsSingle();
            Container.BindInterfacesTo<WeatherPresenter>().AsSingle();
        }
    }
}