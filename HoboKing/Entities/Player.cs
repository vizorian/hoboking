using HoboKing.Control;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HoboKing.Entities
{
    class Player : IGameEntity
    {
        public Sprite Sprite { get; set; }
        public float Speed { get; set; }
        public PlayerState State { get; set; }
        public string ConnectionId { get; set; }
        public bool IsOtherPlayer { get; set; }

        private Movement movement;

        public float PlayerVelocityY;
        public float PlayerVelocityX;

        // CONSTANTS
        private const float GRAVITY = 10;
        private const int MAX_JUMP = 10;
        private const int MIN_JUMP = 1;
        private const int HORIZONTAL_SPEED = 100;

        public bool onGround;

        private Map Map;
        public Player(GraphicsDevice graphics ,Texture2D texture, Vector2 position, string connectionId, bool isOtherPlayer, Map map)
        {
            Sprite = new Sprite(graphics, texture, position);
            ConnectionId = connectionId;
            IsOtherPlayer = isOtherPlayer;
            Map = map;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float realPosX = Sprite.Position.X * Map.TILE_SIZE;
            float realPosY = Sprite.Position.Y * Map.TILE_SIZE;
            Sprite.Draw(spriteBatch, new Vector2(realPosX, realPosY));
        }
        
        public void SetPlayerMovement(Movement newMovement)
        {
            movement = newMovement;
        }

        public void Update(GameTime gameTime)
        {
            Vector2 NewPlayerPosition = new Vector2(
                Sprite.Position.X + PlayerVelocityX * (float)gameTime.ElapsedGameTime.TotalSeconds,
                Sprite.Position.Y + PlayerVelocityY * (float)gameTime.ElapsedGameTime.TotalSeconds);

            // Gravity
            PlayerVelocityY += GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;

            PlayerVelocityX += -3.0f * PlayerVelocityX * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Math.Abs(PlayerVelocityX) < 0.01f)
            {
                PlayerVelocityX = 0.0f;
            }
            if (PlayerVelocityX == 0 && onGround)
            {
                State = PlayerState.Idle;
            }
            if (PlayerVelocityY < 0)
            {
                State = PlayerState.Falling;
            }

            float maxHorizontalVelocity = 5.0f;
            // Velocity clamping
            if (PlayerVelocityX > maxHorizontalVelocity)
                PlayerVelocityX = maxHorizontalVelocity;
            if (PlayerVelocityX < -maxHorizontalVelocity)
                PlayerVelocityX = -maxHorizontalVelocity;
            if (PlayerVelocityY < -MAX_JUMP)
                PlayerVelocityY = -MAX_JUMP;
            if (PlayerVelocityY > MAX_JUMP)
                PlayerVelocityY = MAX_JUMP;

            // Collisions
            // https://github.com/OneLoneCoder/videos/blob/master/OneLoneCoder_PlatformGame1.cpp
            if (PlayerVelocityX <= 0)
            {
                if (Map.GetTile((int)(NewPlayerPosition.X + 0.0f), (int)(Sprite.Position.Y + 0.0f)) != '.' ||
                    Map.GetTile((int)(NewPlayerPosition.X + 0.0f), (int)(Sprite.Position.Y + 0.9f)) != '.')
                {
                    NewPlayerPosition.X = (int)NewPlayerPosition.X + 1;
                    PlayerVelocityX = 0;
                }
            }
            else
            {
                if (Map.GetTile((int)(NewPlayerPosition.X + 1.0f), (int)(Sprite.Position.Y + 0.0f)) != '.' ||
                    Map.GetTile((int)(NewPlayerPosition.X + 1.0f), (int)(Sprite.Position.Y + 0.9f)) != '.')
                {
                    NewPlayerPosition.X = (int)NewPlayerPosition.X;
                    PlayerVelocityX = 0;
                }
            }
            onGround = false;
            if (PlayerVelocityY <= 0)
            {
                if (Map.GetTile((int)(NewPlayerPosition.X + 0.0f), (int)(Sprite.Position.Y + 0.0f)) != '.' ||
                    Map.GetTile((int)(NewPlayerPosition.X + 0.9f), (int)(Sprite.Position.Y + 0.0f)) != '.')
                {
                    NewPlayerPosition.Y = (int)NewPlayerPosition.Y + 1;
                    PlayerVelocityY = 0;
                }
            }
            else
            {
                if (Map.GetTile((int)(NewPlayerPosition.X + 0.0f), (int)(Sprite.Position.Y + 1.0f)) != '.' ||
                    Map.GetTile((int)(NewPlayerPosition.X + 0.9f), (int)(Sprite.Position.Y + 1.0f)) != '.')
                {
                    NewPlayerPosition.Y = (int)NewPlayerPosition.Y;
                    PlayerVelocityY = 0;
                    onGround = true;
                }
            }

            Sprite.Position = NewPlayerPosition;

        }

        public void BeginCharge(GameTime gameTime)
        {
            movement.BeginCharge(gameTime);
        }

        public void Jump(long jumpStrength, int xDirection)
        {
            movement.Jump(jumpStrength, xDirection);
        }

        public void Walk(string direction, GameTime gameTime)
        {
            movement.Walk(direction, gameTime);
        }

        public void Idle()
        {
            movement.Idle();
        }
    }
}
