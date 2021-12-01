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

namespace HoboKing.Entities
{
    internal sealed class Player : GameEntity
    {
        private const int PLAYER_SIZE = 60;
        public string ConnectionId { get; set; }
        public bool IsOtherPlayer { get; set; }

        private Movement movement;

        public Player(Texture2D texture, Vector2 position, string connectionId, bool isOtherPlayer, World world) : base(
            texture, position, PLAYER_SIZE)
        {
            ConnectionId = connectionId;
            IsOtherPlayer = isOtherPlayer;

            if (!isOtherPlayer)
            {
                // Set physics
                CreatePhysicsObjects(world, BodyType.Dynamic);
                SetMovementStrategy(new PlayerMovement(this));
            }
        }

        public void SetMovementStrategy(Movement movementStrategy)
        {
            movement = movementStrategy;
        }

        public void Update(GameTime gameTime)
        {
            base.Update();
            // Switches between movements when F2 is pressed.
            if (InputController.KeyPressed(Keys.F2))
            {
                switch (movement)
                {
                    case PlayerMovement _:
                        SetMovementStrategy(new IceMovement(this));
                        break;
                    case IceMovement _:
                        SetMovementStrategy(new DebugMovement(this));
                        break;
                    case DebugMovement _:
                        SetMovementStrategy(new PlayerMovement(this));
                        break;
                }
            }

            movement.AcceptInputs(gameTime);
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