using HoboKing.Control;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace HoboKing.Entities
{
    class Player : IGameEntity
    {
        public Sprite Sprite { get; set; }
        public float Speed { get; set; }
        public PlayerState State { get; set; }
        public string ConnectionId { get; set; }
        public bool IsOtherPlayer { get; set; }

        public float VelocityY;
        public float VelocityX;

        // CONSTANTS
        private const float GRAVITY = 300;

        private Vector2 previousPosition;
        private Movement movement;

        public Player(GraphicsDevice graphics, Texture2D texture, Vector2 position, string connectionId, bool isOtherPlayer)
        {
            Sprite = new Sprite(graphics, texture, position, 40);
            ConnectionId = connectionId;
            IsOtherPlayer = isOtherPlayer;

            // Recalculates tiles to absolute coordinates
            float realPosX = Sprite.Position.X * Map.TILE_SIZE;
            float realPosY = Sprite.Position.Y * Map.TILE_SIZE;
            Sprite.Position = new Vector2(realPosX, realPosY);
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
            previousPosition = Sprite.Position;
            
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            VelocityY += GRAVITY * time;
            Sprite.Position = new Vector2(
                Sprite.Position.X + VelocityX * time,
                Sprite.Position.Y + VelocityY * time);

            Sprite.Update(gameTime);

            //Console.WriteLine($"Player coordinates X and Y: {Sprite.Position.X}-{Sprite.Position.Y}");

            // Loop through all Tiles and check for collisions
            foreach (var entity in EntityManager.Entities)
            {
                if (entity is Tile)
                {
                    if (Sprite.Collision(entity.Sprite))
                    {
                        if (previousPosition.Y < Sprite.Rectangle.Top)
                        {
                            Sprite.Position = new Vector2(Sprite.Position.X, previousPosition.Y);
                            VelocityY = 0.01f;
                        }
                    }
                }
            }

            InputControls(gameTime);
        }

        private void InputControls(GameTime gameTime)
        {
            if (InputController.KeyPressed(Keys.Space))
            {
                movement.BeginCharge(gameTime);
            } else if (InputController.KeyReleased(Keys.Space))
            {
                movement.Jump(1000, 0);
            }

            if (InputController.KeyPressedDown(Keys.A))
            {
                movement.Walk("left", gameTime);
            }
            if (InputController.KeyPressedDown(Keys.D))
            {
                movement.Walk("right", gameTime);
            }
            if (InputController.KeyReleased(Keys.A) || InputController.KeyReleased(Keys.D))
            {
                VelocityX = 0;
            }
        }
    }
}
