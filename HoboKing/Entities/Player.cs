using HoboKing.Control;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            InputControls(gameTime);
        }

        private void InputControls(GameTime gameTime)
        {
            // JUMP
            if (InputController.KeyPressed(Keys.Space))
            {
                movement.Up(gameTime);
            }
            if (InputController.KeyReleased(Keys.Space))
            {
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
            }

            // DOWN
            if (InputController.KeyPressedDown(Keys.S))
            {
                movement.Down(gameTime);
            }
            if (InputController.KeyReleased(Keys.S))
            {
            }

            // UP
            if (InputController.KeyPressedDown(Keys.W))
            {
                movement.Up(gameTime);
            }
            if (InputController.KeyReleased(Keys.W))
            {
            }
        }
    }
}
