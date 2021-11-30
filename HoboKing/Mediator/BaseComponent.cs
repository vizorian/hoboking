namespace HoboKing.Mediator
{
    public class BaseComponent
    {
        protected IMediator mediator;

        public BaseComponent(IMediator mediator = null)
        {
            this.mediator = mediator;
        }

        public void SetMediator(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}