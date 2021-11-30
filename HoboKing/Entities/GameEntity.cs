using System;
using System.Xml.Serialization;
using HoboKing.Components;
using HoboKing.Mediator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Joints;

namespace HoboKing.Entities
{
    public abstract class GameEntity : BaseComponent
    {
        protected const float FRICTION = 0.3f;
        protected const float RESTITUTION = 0.1f;
        protected const float UNIT_TO_PIXEL = 100.0f;
        protected const float PIXEL_TO_UNIT = 1 / UNIT_TO_PIXEL;

        //Intrinsic state
        protected int tileSize;
        public Texture2D Texture { get; protected set; }

        //Extrinsic state
        protected World world;
        public Body Body;
        public Fixture Fixture;
        protected Rectangle sizeRectangle;
        public Vector2 Position { get; set; }

        protected GameEntity()
        {
        }

        protected GameEntity(Texture2D texture, Vector2 position, int size = 0)
        {
            Position = position * MapComponent.TILE_SIZE;
            Texture = texture;
            tileSize = size;

            SetSizeRectangle(size);
        }

        // Sets the size rectangle for the GameEntity.
        // If the size is specified then Rectangle is resized.
        // If size is 0, Rectangle is the size of Texture.
        private void SetSizeRectangle(int size)
        {
            sizeRectangle = size != 0
                ? new Rectangle((int) Position.X, (int) Position.Y, size, size)
                : new Rectangle((int) Position.X, (int) Position.Y, Texture.Width, Texture.Height);
        }

        /// <summary>
        ///     Creates physics objects for a GameEntity, based of BodyType.
        /// </summary>
        /// <param name="w">Shared w object</param>
        /// <param name="bodyType">BodyType.Static or BodyType.Dynamic</param>
        protected virtual void CreatePhysicsObjects(World w, BodyType bodyType)
        {
            world = w;
            Body = w.CreateBody(Position * PIXEL_TO_UNIT, 0, bodyType);
            Body.FixedRotation = true;
            if (bodyType is BodyType.Dynamic)
                CreatePlayerFixture();
            else
                CreateStaticFixture();
            Fixture.Restitution = RESTITUTION;
            Fixture.Friction = FRICTION;
        }

        /// <summary>
        ///     Creates a Fixture for player physics
        /// </summary>
        private void CreatePlayerFixture()
        {
            Fixture = Body.CreateCircle(sizeRectangle.Width / 2f * PIXEL_TO_UNIT, 1f,
                new Vector2(sizeRectangle.Width / 2f * PIXEL_TO_UNIT, sizeRectangle.Height / 2f * PIXEL_TO_UNIT));
        }

        /// <summary>
        ///     Creates a Fixture for static object physics
        /// </summary>
        private void CreateStaticFixture()
        {
            Fixture = Body.CreateRectangle(sizeRectangle.Width * PIXEL_TO_UNIT, sizeRectangle.Height * PIXEL_TO_UNIT,
                1f, Vector2.Zero);
        }

        /// <summary>
        ///     GameEntity update method, u[dates the position of sizeRectangle every frame
        ///     also updates GameEntity.Position
        /// </summary>
        protected void Update()
        {
            if (Body != null)
            {
                sizeRectangle.X = Convert.ToInt32(Body.Position.X * UNIT_TO_PIXEL);
                sizeRectangle.Y = Convert.ToInt32(Body.Position.Y * UNIT_TO_PIXEL);
                Position = new Vector2(sizeRectangle.X, sizeRectangle.Y);
            }
            else
            {
                sizeRectangle.X = Convert.ToInt32(Position.X);
                sizeRectangle.Y = Convert.ToInt32(Position.Y);
            }
        }

        /// <summary>
        ///     Draws GameEntity
        /// </summary>
        /// <param name="spriteBatch">Shared game spriteBatch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, sizeRectangle, Color.White);
        }

        /// <summary>
        ///     Assigns a new texture.
        /// </summary>
        /// <param name="texture">New Texture</param>
        public void ChangeTexture(Texture2D texture)
        {
            Texture = texture;
        }

        /// <summary>
        ///     Changes object position correctly
        /// </summary>
        /// <param name="newPosition">New Position</param>
        public virtual void ChangePosition(Vector2 newPosition)
        {
            Position = newPosition;
            SetSizeRectangle(tileSize);
            if (Body != null)
                Body.Position = newPosition * PIXEL_TO_UNIT;
        }

        /// <summary>
        ///     Changes tile size
        /// </summary>
        /// <param name="newSize">New Size</param>
        public void ChangeTileSize(int newSize)
        {
            tileSize = newSize;
            SetSizeRectangle(tileSize);
        }

        /// <summary>
        ///     Let's you turn the gravity on or off.
        /// </summary>
        /// <param name="useGravity">If gravity should be used or not</param>
        public void UseGravity(bool useGravity)
        {
            if (Body != null)
                Body.IgnoreGravity = !useGravity;
        }

        /// <summary>
        ///     Derived classes need to implement ShallowCopy for prototype design pattern
        /// </summary>
        /// <returns></returns>
        public virtual GameEntity ShallowCopy()
        {
            return MemberwiseClone() as GameEntity;
        }

        /// <summary>
        ///     Derived classes need to implement DeepCopy for prototype design pattern.
        /// </summary>
        /// <returns></returns>
        public abstract GameEntity DeepCopy();
    }
}