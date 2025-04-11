namespace Sources.UI.Dogs
{
    public interface IDogCellPresenter
    {
        string Name { get; }
        
        void OnClicked();
    }
}