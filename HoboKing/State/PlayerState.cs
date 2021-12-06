using System;
using System.Collections.Generic;
using System.Text;
using HoboKing.Control.Strategy;
using HoboKing.Entities;
using Microsoft.Xna.Framework;

namespace HoboKing.State
{
    internal abstract class PlayerState
    {
        protected Player player;
        protected Movement movement;
        protected PlayerState(Player p)
        {
            player = p;
        }

        public virtual void HandleInputs(GameTime gameTime)
        {
            movement.AcceptInputs(gameTime);
        }

        public virtual void NextState()
        {

        }
    }

    // Player can move normally
    internal class NormalState : PlayerState
    {
        public NormalState(Player p) : base(p)
        {
            Console.WriteLine("Player state: Normal");
            movement = new PlayerMovement(p);
        }
    }

    //Player cannot jump while reloading
    internal class Reloading : PlayerState
    {
        private int time;
        public Reloading(Player p) : base(p)
        {
            Console.WriteLine("Player state: Reloading");
            time = 0;
        }

        public override void HandleInputs(GameTime gameTime)
        {
            var elapsedTime = gameTime.ElapsedGameTime.Milliseconds;
            time += elapsedTime;
            if (time > 300)
            {
                player.SetState(new NormalState(player));
            }
        }
    }

    internal class OnIce : PlayerState
    {
        public OnIce(Player p) : base(p)
        {
            Console.WriteLine("Player state: OnIce");
            movement = new IceMovement(p);
        }
    }

    internal class InAir : PlayerState
    {
        public InAir(Player p) : base(p)
        {
            Console.WriteLine("Player state: InAir");
        }

        public override void HandleInputs(GameTime gameTime)
        {
            // Can't move while in air
        }

        public override void NextState()
        {
            player.SetState(new Falling(player));
        }
    }

    internal class Falling : PlayerState
    {
        public Falling(Player p) : base(p)
        {
            Console.WriteLine("Player state: Falling");
        }

        public override void HandleInputs(GameTime gameTime)
        {
            // Can't move while in air
        }

        public override void NextState()
        {
            player.SetState(new Reloading(player));
        }
    }
}
