using System;
using Sources.Infrastructure.Dogs;
using Zenject;

namespace Sources.UI.Dogs
{
    public class DogPopupPresenter : IInitializable, IDisposable
    {
        private readonly DogsModel _dogsModel;
        private readonly DogPopup _dogPopup;

        public DogPopupPresenter(DogsModel dogsModel, DogPopup dogPopup)
        {
            _dogsModel = dogsModel;
            _dogPopup = dogPopup;
        }

        public void Initialize() =>
            _dogsModel.InfoLoaded += OnInfoLoaded;

        public void Dispose() =>
            _dogsModel.InfoLoaded -= OnInfoLoaded;

        private void OnInfoLoaded(string name, string description)
        {
            _dogPopup.Show();
            _dogPopup.SetInfo(name, description);
        }
    }
}