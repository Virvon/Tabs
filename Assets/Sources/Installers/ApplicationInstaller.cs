using Sources.Infrastructure.Dogs;
using Sources.Infrastructure.ServerRequests;
using Sources.Infrastructure.Weather;
using Sources.UI.Dogs;
using Sources.UI.TabsMediator;
using Sources.UI.Weather;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sources.Installers
{
    public class ApplicationInstaller : MonoInstaller
    {
        [SerializeField] private DogListView _dogListView;
        [SerializeField] private Button _weatherButton;
        [SerializeField] private Button _dogsButton;
        [SerializeField] private DogPopup _dogPopup;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ServerRequestQueue>().AsSingle();
            Container.BindInterfacesAndSelfTo<WeatherModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<WeatherPresenter>().AsSingle().WithArguments(_weatherButton);
            Container.BindInterfacesAndSelfTo<DogsModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<DogsPresenter>().AsSingle().WithArguments(_dogListView, _dogsButton);
            Container.BindInterfacesTo<Mediator>().AsSingle().NonLazy();
            Container.BindInterfacesTo<DogPopupPresenter>().AsSingle().WithArguments(_dogPopup).NonLazy();
        }
    }
}