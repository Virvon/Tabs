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
        
        public DogsPresenter(Button button, DogsModel dogsModel, DogListView dogListView) : base(button)
        {
            _dogsModel = dogsModel;
            _dogListView = dogListView;
        }

        public void Initialize()
        {
            _dogsModel.Setuped += OnSetupded;
            Button.onClick.AddListener(OnButtonClicked);
        }

        public void Dispose()
        {
            _dogsModel.Setuped -= OnSetupded;
            Button.onClick.RemoveListener(OnButtonClicked);
        }

        public override void ShowTab()
        {
            Button.interactable = false;
            _dogListView.Show();
        }

        public override void HideTab()
        {
            Button.interactable = true;
            _dogListView.Hide();
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

        private void OnButtonClicked() =>
            Mediator.Notify(this);
    }
}