using System;
using Sources.Infrastructure.Dogs;

namespace Sources.UI.Dogs
{
    public class DogCellPresenter : IDogCellPresenter, IDisposable
    {
        private readonly DogCell _dogCell;

        public DogCellPresenter(DogCell dogCell)
        {
            _dogCell = dogCell;

            _dogCell.LoadStarted += OnLoadStarted;
            _dogCell.LoadFinished += OnLoadFinished;
            _dogCell.LoadCanceled += OnLoadCanceled;
        }
        
        public event Action LoadAnimationStarted;
        public event Action LoadAnimationFinished;
        public event Action Disposed;
        public string Name => _dogCell.Name;

        public void Dispose()
        {
            _dogCell.LoadStarted -= OnLoadStarted;
            _dogCell.LoadFinished -= OnLoadFinished;
            _dogCell.LoadCanceled -= OnLoadCanceled;
            
            Disposed?.Invoke();
        }

        public void OnClicked() =>
            _dogCell.LoadDogInfo();

        private void OnLoadCanceled() =>
            LoadAnimationFinished?.Invoke();

        private void OnLoadFinished(DogCell dogCell, string description) =>
            LoadAnimationFinished?.Invoke();

        private void OnLoadStarted(DogCell dogCell) =>
            LoadAnimationStarted?.Invoke();
    }
}