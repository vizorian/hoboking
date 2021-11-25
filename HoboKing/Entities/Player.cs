using HoboKing.Control;
using HoboKing.Control.Strategy;
using HoboKing.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Entities
{
    internal sealed class Player : GameEntity
    {
        private const int PLAYER_SIZE = 60;

        private Movement movement;

        public Player(Texture2D texture, Vector2 position, string connectionId, bool isOtherPlayer, World world) : base(
            texture, position, PLAYER_SIZE)
        {
            State = PlayerState.Idle;
            ConnectionId = connectionId;
            IsOtherPlayer = isOtherPlayer;

            if (!isOtherPlayer)
            {
                // Set physics
                CreatePhysicsObjects(world, BodyType.Dynamic);
                SetMovementStrategy(new PlayerMovement(this));
            }
        }

        public PlayerState State { get; set; }
        public string ConnectionId { get; set; }
        public bool IsOtherPlayer { get; set; }

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
                    SetMovementStrategy(new IceMovement(this));
                else if (movement is IceMovement)
                    SetMovementStrategy(new DebugMovement(this));
                else if (movement is DebugMovement) SetMovementStrategy(new PlayerMovement(this));
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
    }
}