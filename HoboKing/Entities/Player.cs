using HoboKing.Control;
using HoboKing.Control.Strategy;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Entities
{
    class Player : GameEntity
    {
        const int PLAYER_SIZE = 60;

        public PlayerState State { get; set; }
        public string ConnectionId { get; set; }
        public bool IsOtherPlayer { get; set; }

        private Movement movement;

        private World world;

        public Player(Texture2D texture, Vector2 position, string connectionId, bool isOtherPlayer, World world) : base(texture, position, PLAYER_SIZE)
        {
            ConnectionId = connectionId;
            IsOtherPlayer = isOtherPlayer;

            if (!isOtherPlayer)
            {
                // Set physics
                CreatePhysicsObjects(world, BodyType.Dynamic);
                SetMovementStrategy(new PlayerMovement(this));
            }


            this.world = world;
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
                if (movement is PlayerMovement)
                {
                    SetMovementStrategy(new IceMovement(this));
                }
                else if (movement is IceMovement)
                {
                    SetMovementStrategy(new DebugMovement(this));
                }
                else if (movement is DebugMovement)
                {
                    SetMovementStrategy(new PlayerMovement(this));
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
            var clone = MemberwiseClone() as Player;
            clone.ConnectionId = new string(ConnectionId);
            return clone;
        }
    }
}
