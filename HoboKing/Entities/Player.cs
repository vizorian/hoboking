using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace HoboKing.Entities
{
    class Player : IGameEntity
    {
        public Sprite Sprite { get; set; }
        public float Speed { get; set; }
        public Vector2 Position { get; set; }
        public PlayerState State { get; private set; }
        public string ConnectionId { get; set; }
        public bool IsOtherPlayer { get; set; }

        private SoundEffect JumpSound;
        private float PlayerVelocityY;
        private float PlayerVelocityX;

        // CONSTANTS
        private const float GRAVITY = 10;
        private const int MAX_JUMP = 10;
        private const int MIN_JUMP = 1;
        private const int HORIZONTAL_SPEED = 100;

        public bool onGround;

        private Map Map;
        public Player(Texture2D spriteSheet, Vector2 position, SoundEffect jumpSound, string connectionId, bool isOtherPlayer, Map map)
        {
            Sprite = new Sprite(spriteSheet, position);
            Position = position;
            JumpSound = jumpSound;
            ConnectionId = connectionId;
            IsOtherPlayer = isOtherPlayer;
            Map = map;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            float realPosX = Position.X * Map.TileWidth;
            float realPosY = Position.Y * Map.TileWidth;
            Sprite.Draw(spriteBatch, new Vector2(realPosX, realPosY));
        }

        public void Update(GameTime gameTime)
        {
            Vector2 NewPlayerPosition = new Vector2(
                Position.X + PlayerVelocityX * (float)gameTime.ElapsedGameTime.TotalSeconds, 
                Position.Y + PlayerVelocityY * (float)gameTime.ElapsedGameTime.TotalSeconds);

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
                if (Map.GetTile((int)(NewPlayerPosition.X + 0.0f), (int)(Position.Y + 0.0f)) != '.' ||
                    Map.GetTile((int)(NewPlayerPosition.X + 0.0f), (int)(Position.Y + 0.9f)) != '.')
                {
                    NewPlayerPosition.X = (int)NewPlayerPosition.X + 1;
                    PlayerVelocityX = 0;
                }
            }
            else
            {
                if (Map.GetTile((int)(NewPlayerPosition.X + 1.0f), (int)(Position.Y + 0.0f)) != '.' ||
                    Map.GetTile((int)(NewPlayerPosition.X + 1.0f), (int)(Position.Y + 0.9f)) != '.')
                {
                    NewPlayerPosition.X = (int)NewPlayerPosition.X;
                    PlayerVelocityX = 0;
                }
            }
            onGround = false;
            if (PlayerVelocityY <= 0)
            {
                if (Map.GetTile((int)(NewPlayerPosition.X + 0.0f), (int)(Position.Y + 0.0f)) != '.' ||
                    Map.GetTile((int)(NewPlayerPosition.X + 0.9f), (int)(Position.Y + 0.0f)) != '.')
                {
                    NewPlayerPosition.Y = (int)NewPlayerPosition.Y + 1;
                    PlayerVelocityY = 0;
                }
            }
            else
            {
                if (Map.GetTile((int)(NewPlayerPosition.X + 0.0f), (int)(Position.Y + 1.0f)) != '.' ||
                    Map.GetTile((int)(NewPlayerPosition.X + 0.9f), (int)(Position.Y + 1.0f)) != '.')
                {
                    NewPlayerPosition.Y = (int)NewPlayerPosition.Y;
                    PlayerVelocityY = 0;
                    onGround = true;
                }
            }

            Position = NewPlayerPosition;

        }

        public bool BeginCharge(GameTime gameTime)
        {
            if (State == PlayerState.Jumping || State == PlayerState.Falling)
            {
                return false;
            }
            State = PlayerState.Charging;
            return true;
        }

        public void Jump(long jumpStrength, int xDirection)
        {
            if (jumpStrength > MAX_JUMP)
                jumpStrength = MAX_JUMP;
            if (jumpStrength < MIN_JUMP)
                jumpStrength = MIN_JUMP;
            if (PlayerVelocityY == 0)
            {
                JumpSound.Play();
                PlayerVelocityY = -jumpStrength;
                State = PlayerState.Jumping;
            }
            switch (xDirection)
            {
                case -1:
                    Console.WriteLine($"Jump strenght is {jumpStrength}, LEFT");
                    PlayerVelocityX = xDirection * HORIZONTAL_SPEED;
                    return;
                case 1:
                    Console.WriteLine($"Jump strenght is {jumpStrength}, RIGHT");
                    PlayerVelocityX = xDirection * HORIZONTAL_SPEED;
                    return;
                default:
                    Console.WriteLine($"Jump strenght is {jumpStrength}, STRAIGHT UP");
                    return;
            }
        }

        public void Walk(string direction, GameTime gameTime)
        {
            State = PlayerState.Walking;
            if (direction == "left")
            {
                PlayerVelocityX += -HORIZONTAL_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (direction == "right")
            {
                PlayerVelocityX += HORIZONTAL_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void Idle()
        {
            State = PlayerState.Idle;
        }
    }
}
