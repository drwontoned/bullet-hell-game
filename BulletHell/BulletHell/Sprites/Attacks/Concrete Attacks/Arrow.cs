﻿namespace BulletHell.Sprites.Attacks.Concrete_Attacks
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BulletHell.Sprites.Movement_Patterns;
    using BulletHell.Sprites.Projectiles;
    using BulletHell.States;
    using Microsoft.Xna.Framework;

    internal class Arrow : Attack
    {
        private int widthOfArrow;

        public Arrow(Projectile projectile, MovementPattern movement, float cooldownToCreateProjectile, int widthOfArrow)
            : base(projectile, movement, cooldownToCreateProjectile)
        {
            this.widthOfArrow = widthOfArrow;
        }

        public override void Update(GameTime gametime, List<Sprite> sprites)
        {
            this.CreateProjectile(sprites);
            this.IsRemoved = true;
        }

        protected override void CreateProjectile(List<Sprite> sprites)
        {
            float spacing = 2;
            int verticalOffset = 20;

            for (int row = 0; row < this.widthOfArrow; row++)
            {
                for (int col = 0; col <= row; col++)
                {
                    Vector2 targetPosition = GameState.GetPlayerPosition();
                    Projectile newProjectile = this.ProjectileToLaunch.Clone() as Projectile;
                    newProjectile.Movement = this.ProjectileToLaunch.Movement.Clone() as MovementPattern;
                    newProjectile.Movement.Parent = newProjectile;

                    if (row < col / 2.0)
                    {
                        Vector2 velocity = this.Movement.CalculateVelocity(this.Movement.CurrentPosition, targetPosition, newProjectile.Movement.Speed);

                        velocity.X = (float)((velocity.X * Math.Cos((col - (row / 2.0)) * spacing * (Math.PI / 180))) - (velocity.Y * Math.Sin((col - (row / 2.0)) * spacing * (Math.PI / 180))));
                        velocity.Y = (float)((velocity.X * Math.Sin((col - (row / 2.0)) * spacing * (Math.PI / 180))) + (velocity.Y * Math.Cos((col - (row / 2.0)) * spacing * (Math.PI / 180))));

                        newProjectile.Movement.Velocity = velocity;
                    }
                    else if (row == col / 2.0)
                    {
                        Vector2 velocity = this.Movement.CalculateVelocity(this.Movement.CurrentPosition, targetPosition, newProjectile.Movement.Speed);

                        newProjectile.Movement.Velocity = velocity;
                    }
                    else if (row > col / 2.0)
                    {
                        Vector2 velocity = this.Movement.CalculateVelocity(this.Movement.CurrentPosition, targetPosition, newProjectile.Movement.Speed);

                        velocity.X = (float)((velocity.X * Math.Cos((col - (row / 2.0)) * spacing * (Math.PI / 180))) - (velocity.Y * Math.Sin((col - (row / 2.0)) * spacing * (Math.PI / 180))));
                        velocity.Y = (float)((velocity.X * Math.Sin((col - (row / 2.0)) * spacing * (Math.PI / 180))) + (velocity.Y * Math.Cos((col - (row / 2.0)) * spacing * (Math.PI / 180))));

                        newProjectile.Movement.Velocity = velocity;
                    }

                    Vector2 position = this.Movement.CurrentPosition;

                    position.Y -= verticalOffset * row;

                    newProjectile.Movement.CurrentPosition = position;

                    newProjectile.Parent = this.Attacker;
                    sprites.Add(newProjectile);
                }
            }
        }
    }
}