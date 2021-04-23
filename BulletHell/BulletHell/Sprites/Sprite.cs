﻿namespace BulletHell.Sprites
{
    using System;
    using System.Collections.Generic;
    using BulletHell.Sprites.Movement_Patterns;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    internal abstract class Sprite : ICloneable
    {
        protected bool isRemoved = false;
        private Color color = Color.White;
        private MovementPattern movement;

        public Sprite(Texture2D texture, Color color, MovementPattern movement)
        {
            this.Texture = texture;
            this.Color = color;
            this.Movement = movement;
        }

        public Texture2D Texture { get; set; }

        public MovementPattern Movement { get => this.movement; set => this.movement = value; }

        public Color Color
        {
            get => this.color;
            set => this.color = value;
        }

        public bool IsRemoved
        {
            get => this.isRemoved;
            set => this.isRemoved = value;
        }

        // Serves as hitbox
        public virtual Rectangle Rectangle
        {
            get => new Rectangle(
                    new Point((int)this.Movement.CurrentPosition.X, (int)this.Movement.CurrentPosition.Y),
                    new Point(this.Texture.Width, this.Texture.Height));
        }

        public virtual object Clone()
        {
            Sprite newSprite = (Sprite)this.MemberwiseClone();
            if (this.Movement != null)
            {
                MovementPattern newMovement = (MovementPattern)this.Movement.Clone();
                newSprite.Movement = newMovement;
            }

            return newSprite;
        }

        public virtual void Update(GameTime gametime, List<Sprite> sprites)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Movement.CurrentPosition, null, this.Color, this.Movement.Rotation, this.Movement.Origin, 1, SpriteEffects.None, 0);
        }

        public virtual void OnCollision(Sprite sprite)
        {
        }

        public void CheckForCollision(List<Sprite> sprites)
        {
            for (int i = sprites.Count - 1; i >= 0; i--)
            {
                // Check for hitbox collision
                if (this.Rectangle.Intersects(sprites[i].Rectangle))
                {
                    this.OnCollision(sprites[i]);
                    sprites[i].OnCollision(this);
                }
            }
        }

        public Vector2 GetCenterOfSprite()
        {
            return this.Rectangle.Center.ToVector2();
        }
    }
}