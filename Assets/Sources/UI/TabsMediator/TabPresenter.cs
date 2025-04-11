using UnityEngine.UI;

namespace Sources.UI.TabsMediator
{
    public abstract class TabPresenter
    {
        protected readonly Button Button;
        
        protected IMediator Mediator;

        protected TabPresenter(Button button) =>
            Button = button;

        public void SetMediator(IMediator mediator) =>
            Mediator = mediator;

        public abstract void ShowTab();
        public abstract void HideTab();
    }
}