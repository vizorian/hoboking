using System;
using System.Xml.Linq;
using HoboKing.Control;
using HoboKing.Control.Strategy;
using HoboKing.State;
using HoboKing.Utils;
using HoboKing.Memento;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using PlayerState = HoboKing.State.PlayerState;

namespace HoboKing.Entities
{
    internal sealed class Player : GameEntity
    {
        private const int PLAYER_SIZE = 60;
        public string ConnectionId { get; set; }
        public bool IsOtherPlayer { get; set; }

        private Movement movement;

        private PlayerState state;
        private bool debug;

        public Player(Texture2D texture, Vector2 position, string connectionId, bool isOtherPlayer, World world) : base(
            texture, position, PLAYER_SIZE)
        {
            ConnectionId = connectionId;
            IsOtherPlayer = isOtherPlayer;
            debug = false;
            
            if (!isOtherPlayer)
            {
                // Set physics
                CreatePhysicsObjects(world, BodyType.Dynamic);
                SetState(new NormalState(this));
            }
        }

        public void SetState(PlayerState newState)
        {
            state = newState;
        }

        public PlayerState GetState()
        {
            return state;
        }

        public void Update(GameTime gameTime)
        {
            base.Update();
            // Switches between movements when F2 is pressed.
            if (InputController.KeyPressed(Keys.M))
            {
                switch (debug)
                {
                    case true:
                        debug = false;
                        break;
                    case false:
                        debug = true;
                        break;
                }

            }

            if (debug)
            {
                if(!(state is DebugState))
                    state = new DebugState(this);
            }
            else
            {
                // If on Ice Level
                if (Position.Y > 1000 && Position.Y < 2000)
                {
                    if (!(state is OnIce) && !(state is InAir) && !(state is Falling) && !(state is Reloading))
                        state = new OnIce(this);
                }
                // If on normal level
                else if (!(state is NormalState) && !(state is InAir) && !(state is Falling) && !(state is Reloading))
                {
                    state = new NormalState(this);
                }

                // Check if jumping
                if (Body.LinearVelocity.Y < -1f && !(state is InAir) && !(state is Falling))
                {
                    state = new InAir(this);
                }

                // Check if falling
                if (Body.LinearVelocity.Y > 1f && state is InAir)
                {
                    state.NextState();
                }

                if (state is Falling && Math.Abs(Body.LinearVelocity.Y) < 0.5f)
                {
                    state.NextState();
                }
            }

            state.HandleInputs(gameTime);
        }

        public override GameEntity ShallowCopy()
        {
            return MemberwiseClone() as Player;
        }

        public override GameEntity DeepCopy()
        {
            var clone = ShallowCopy() as Player;
            clone.ConnectionId = new string(ConnectionId);
            return clone;
        }

        public Snapshot Save()
        {
            return new Snapshot(Position);
        }

        public void Restore(Snapshot snapshot)
        {
            Position = snapshot.GetPosition();
            CreatePhysicsObjects(world, BodyType.Dynamic);
        }
    }
}