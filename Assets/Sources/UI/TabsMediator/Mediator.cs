using Sources.UI.Dogs;
using Sources.UI.Weather;
using Zenject;

namespace Sources.UI.TabsMediator
{
    public class Mediator : IMediator, IInitializable
    {
        private readonly WeatherPresenter _weatherPresenter;
        private readonly DogsPresenter _dogsPresenter;

        public Mediator(WeatherPresenter weatherPresenter, DogsPresenter dogsPresenter)
        {
            _weatherPresenter = weatherPresenter;
            _dogsPresenter = dogsPresenter;
        }

        public void Initialize()
        {
            _weatherPresenter.SetMediator(this);
            _dogsPresenter.SetMediator(this);
            
            Notify(_weatherPresenter);
        }

        public void Notify(TabPresenter tabPresenter)
        {
            tabPresenter.ShowTab();
            
            if (tabPresenter is WeatherPresenter)
                _dogsPresenter.HideTab();
            else
                _weatherPresenter.HideTab();
        }
    }
}