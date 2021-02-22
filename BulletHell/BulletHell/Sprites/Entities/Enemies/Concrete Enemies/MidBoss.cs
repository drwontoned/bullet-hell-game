﻿namespace BulletHell.Sprites.Entities.Enemies.Concrete_Enemies
{
    using System.Collections.Generic;
    using global::BulletHell.Sprites.Movement_Patterns;
    using global::BulletHell.Sprites.Projectiles;
    using Microsoft.Xna.Framework;

    internal class MidBoss : Enemy
    {
        private int previousTime = 0;

        public MidBoss(Dictionary<string, object> midBossProperties)
            : base (midBossProperties)
        {
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            base.Update(gameTime, sprites);

            if (this.previousTime != (int)gameTime.TotalGameTime.TotalSeconds)
            {
                this.Attack(sprites);
            }

            this.previousTime = (int)gameTime.TotalGameTime.TotalSeconds;
        }

        public new void Attack(List<Sprite> sprites)
        {
            Projectile newProjectile = this.Projectile.Clone() as Projectile;
            newProjectile.Movement = this.Projectile.Movement.Clone() as MovementPattern;
            newProjectile.Movement.velocity = this.Movement.velocity;
            newProjectile.Movement.Position = this.Movement.Position;
            int projectileSpeed = newProjectile.Movement.Speed;
            newProjectile.Movement.velocity.Normalize();
            newProjectile.Movement.velocity.X *= projectileSpeed;
            newProjectile.Movement.velocity.Y *= projectileSpeed;
            newProjectile.Movement.velocity.Y += 1;
            newProjectile.Movement.velocity.X += 1;
            newProjectile.Parent = this;

            sprites.Add(newProjectile);

            newProjectile = this.Projectile.Clone() as Projectile;
            newProjectile.Movement = this.Projectile.Movement.Clone() as MovementPattern;
            newProjectile.Movement.velocity = this.Movement.velocity;
            newProjectile.Movement.Position = this.Movement.Position;
            newProjectile.Movement.velocity.Normalize();
            newProjectile.Movement.velocity.X *= projectileSpeed;
            newProjectile.Movement.velocity.Y *= projectileSpeed;
            newProjectile.Movement.velocity.Y -= 1;
            newProjectile.Movement.velocity.X -= 1;
            newProjectile.Parent = this;

            sprites.Add(newProjectile);
        }
    }
}
