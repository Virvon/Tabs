using System;
using System.Collections.Generic;
using Sources.Infrastructure.Dogs;
using Zenject;

namespace Sources.UI.Dogs
{
    public class DogsPresenter : IInitializable, IDisposable
    {
        private readonly DogsModel _dogsModel;
        private readonly DogListView _dogListView;

        public DogsPresenter(DogsModel dogsModel, DogListView dogListView)
        {
            _dogsModel = dogsModel;
            _dogListView = dogListView;
        }

        public void Initialize()
        {
            _dogsModel.Setuped += OnSetupded;
        }

        public void Dispose()
        {
            _dogsModel.Setuped -= OnSetupded;
        }

        private void OnSetupded(IReadOnlyList<DogCell> dogsCells)
        {
            for (int i = 0; i < dogsCells.Count; i++)
            {
                var dogCell = dogsCells[i];
                DogView view = _dogListView.SpawnItem();
                DogCellPresenter presenter = new(dogCell);
                view.Setup((i + 1).ToString(), presenter);
            }
        }
    }
}