using System;

namespace Sources.UI.Dogs
{
    public interface IDogCellPresenter
    {
        event Action LoadAnimationStarted;
        event Action LoadAnimationFinished;
        event Action Disposed; 
        
        string Name { get; }
        
        void OnClicked();
    }
}