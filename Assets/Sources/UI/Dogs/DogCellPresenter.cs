using Sources.Infrastructure.Dogs;

namespace Sources.UI.Dogs
{
    public class DogCellPresenter : IDogCellPresenter
    {
        private readonly DogCell _dogCell;

        public DogCellPresenter(DogCell dogCell)
        {
            _dogCell = dogCell;
        }

        public string Name => _dogCell.Name;

        public void OnClicked() =>
            _dogCell.LoadDogInfo();
    }
}