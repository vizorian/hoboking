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
    class Player : IGameEntity
    {
        public Sprite Sprite { get; set; }
        public PlayerState State { get; set; }
        public string ConnectionId { get; set; }
        public bool IsOtherPlayer { get; set; }

        private Movement movement;
        private bool isOnGround = true;

        private World world;

        public Player(Texture2D texture, Vector2 position, string connectionId, bool isOtherPlayer, World world)
        {
            // Recalculates tiles to absolute coordinates
            float realPosX = position.X * MapComponent.TILE_SIZE;
            float realPosY = position.Y * MapComponent.TILE_SIZE;

            ConnectionId = connectionId;
            IsOtherPlayer = isOtherPlayer;
            if (isOtherPlayer)
            {
                Sprite = new Sprite(texture, new Vector2(realPosX, realPosY), 60);
            }
            else {
                Sprite = new Sprite(texture, new Vector2(realPosX, realPosY), world, BodyType.Dynamic, 60);
            }

            SetMovementStrategy(new PlayerMovement(this));

            this.world = world;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }
        
        public void SetMovementStrategy(Movement movementStrategy)
        {
            movement = movementStrategy;
        }

        public void Update(GameTime gameTime)
        {
            // Switches between movements when F2 is pressed.
            if (InputController.KeyPressed(Keys.F2))
            {
                if (movement is PlayerMovement)
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

        public IGameEntity ShallowCopy()
        {
            return MemberwiseClone() as Player;
        }

        public IGameEntity DeepCopy()
        {
            var clone = MemberwiseClone() as Player;
            if (IsOtherPlayer)
            {
                clone.Sprite = new Sprite(Sprite.Texture, Sprite.Position, 60);
            }
            else
            {
                clone.Sprite = new Sprite(Sprite.Texture, Sprite.Position, world, BodyType.Dynamic, 60);
            }
            clone.ConnectionId = new string(ConnectionId);
            return clone;
        }
    }
}
