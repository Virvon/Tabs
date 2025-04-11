using System;
using System.Collections.Generic;
using Sources.Infrastructure.Dogs;
using Sources.UI.TabsMediator;
using UnityEngine.UI;
using Zenject;

namespace Sources.UI.Dogs
{
    public class DogsPresenter : TabPresenter, IInitializable, IDisposable
    {
        private readonly DogsModel _dogsModel;
        private readonly DogListView _dogListView;
        private readonly List<DogCellPresenter> _presenters;
        
        public DogsPresenter(Button button, DogsModel dogsModel, DogListView dogListView) : base(button)
        {
            _dogsModel = dogsModel;
            _dogListView = dogListView;

            _presenters = new();
        }

        public void Initialize()
        {
            _dogsModel.ListUpdated += OnListUpdated;
            _dogsModel.ListUpdateStarted += OnListUpdateStarted;
            Button.onClick.AddListener(OnButtonClicked);
        }

        public void Dispose()
        {
            _dogsModel.ListUpdated -= OnListUpdated;
            _dogsModel.ListUpdateStarted -= OnListUpdateStarted;
            Button.onClick.RemoveListener(OnButtonClicked);

            foreach (DogCellPresenter presenter in _presenters)
                presenter.Dispose();
        }

        public override void ShowTab()
        {
            Button.interactable = false;
            _dogListView.Show();
            _dogsModel.UpdateInfo().Forget();
        }

        public override void HideTab()
        {
            Button.interactable = true;
            _dogListView.Hide();
            _dogsModel.Stop();
            
            foreach (DogCellPresenter presenter in _presenters)
                presenter.Dispose();
            
            _presenters.Clear();
        }

        private void OnListUpdated(IReadOnlyList<DogCell> dogsCells)
        {
            _dogListView.StopLoading();
            
            for (int i = 0; i < dogsCells.Count; i++)
            {
                var dogCell = dogsCells[i];
                DogView view = _dogListView.SpawnItem();
                DogCellPresenter presenter = new(dogCell);
                view.Setup((i + 1).ToString(), presenter);
                
                _presenters.Add(presenter);
            }
        }

        private void OnButtonClicked() =>
            Mediator.Notify(this);

        private void OnListUpdateStarted() =>
            _dogListView.ShowLoading();
    }
}