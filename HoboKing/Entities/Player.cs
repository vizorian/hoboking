using HoboKing.Control;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoboKing.Entities
{
    class Player : IGameEntity
    {
        public Sprite Sprite { get; set; }
        public PlayerState State { get; set; }
        public string ConnectionId { get; set; }
        public bool IsOtherPlayer { get; set; }

        public float VelocityY;
        public float VelocityX;

        private Vector2 previousPosition;
        private Movement movement;

        public Player(GraphicsDevice graphics, Texture2D texture, Vector2 position, string connectionId, bool isOtherPlayer)
        {
            Sprite = new Sprite(graphics, texture, position, 60);
            ConnectionId = connectionId;
            IsOtherPlayer = isOtherPlayer;

            // Recalculates tiles to absolute coordinates
            float realPosX = Sprite.Position.X * Map.TILE_SIZE;
            float realPosY = Sprite.Position.Y * Map.TILE_SIZE;
            Sprite.Position = new Vector2(realPosX, realPosY);
        }

        public Player(GraphicsDevice graphics, Texture2D texture, Vector2 position, bool isOtherPlayer)
        {
            Sprite = new Sprite(graphics, texture, position, 60);
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
            VelocityY += movement.Gravity * time;
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
                        if (Sprite.Rectangle.Bottom < entity.Sprite.Rectangle.Top)
                        {
                            Sprite.Position = new Vector2(Sprite.Position.X, previousPosition.Y);
                            VelocityY = 0.01f;
                        }

                        if (Sprite.Rectangle.Top < entity.Sprite.Rectangle.Bottom)
                        {
                            Sprite.Position = new Vector2(Sprite.Position.X, previousPosition.Y);
                            VelocityY = 0.01f;
                        }

                        if (Sprite.Rectangle.Right > entity.Sprite.Rectangle.Left)
                        {
                            Sprite.Position = new Vector2(previousPosition.X, Sprite.Position.Y);
                            VelocityX = 0.01f;
                        }

                        if (Sprite.Rectangle.Left < entity.Sprite.Rectangle.Right)
                        {
                            Sprite.Position = new Vector2(previousPosition.X, Sprite.Position.Y);
                            VelocityX = 0.01f;
                        }
                    }
                }
            }
            InputControls(gameTime);
        }

        private void InputControls(GameTime gameTime)
        {
            // JUMP
            if (InputController.KeyPressed(Keys.Space))
            {
                movement.BeginCharge(gameTime);
            }
            if (InputController.KeyReleased(Keys.Space))
            {
                movement.Jump(1000, 0);
            }

            // LEFT, RIGHT
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

            // DOWN
            if (InputController.KeyPressedDown(Keys.S))
            {
                movement.Down(gameTime);
            }
            if (InputController.KeyReleased(Keys.S))
            {
                VelocityY = 0;
            }

            // UP
            if (InputController.KeyPressedDown(Keys.W))
            {
                movement.Up(gameTime);
            }
            if (InputController.KeyReleased(Keys.W))
            {
                VelocityY = 0;
            }
        }
    }
}
