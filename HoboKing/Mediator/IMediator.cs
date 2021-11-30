using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Mediator
{
    public interface IMediator
    {
        void Notify(object sender, string ev);
    }
}
