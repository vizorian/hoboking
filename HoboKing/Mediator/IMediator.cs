namespace HoboKing.Mediator
{
    public interface IMediator
    {
        void Notify(object sender, string ev);
    }
}
