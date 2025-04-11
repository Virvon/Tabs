using Sources.Infrastructure.Dogs;
using Sources.Infrastructure.ServerRequests;
using Sources.Infrastructure.Weather;
using Sources.UI.Dogs;
using Sources.UI.Weather;
using UnityEngine;
using Zenject;

namespace Sources.Installers
{
    public class ApplicationInstaller : MonoInstaller
    {
        [SerializeField] private DogListView _dogListView;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ServerRequestQueue>().AsSingle();
            Container.BindInterfacesAndSelfTo<WeatherModel>().AsSingle();
            Container.BindInterfacesTo<WeatherPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<DogsModel>().AsSingle();
            Container.BindInterfacesTo<DogsPresenter>().AsSingle().WithArguments(_dogListView).NonLazy();
        }
    }
}