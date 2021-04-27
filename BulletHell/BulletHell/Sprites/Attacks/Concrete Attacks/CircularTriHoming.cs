﻿namespace BulletHell.Sprites.Attacks.Concrete_Attacks
{
    using System;
    using System.Collections.Generic;
    using System.Timers;
    using BulletHell.Sprites.Movement_Patterns;
    using BulletHell.Sprites.Movement_Patterns.Concrete_Movement_Patterns;
    using BulletHell.Sprites.Projectiles;
    using BulletHell.States;
    using Microsoft.Xna.Framework;

    internal class CircularTriHoming : Attack
    {
        private Circular movement;

        public override MovementPattern Movement { get => this.movement; set => this.movement = (Circular)value; }

        public CircularTriHoming(Projectile projectile, Circular circularMovement, Timer cooldownToAttack, Timer cooldownToCreateProjectile)
            : base(projectile, circularMovement, cooldownToAttack, cooldownToCreateProjectile)
        {
            this.Movement = circularMovement;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            this.Move();

            if (this.movement.IsMovementDone())
            {
                this.IsRemoved = true;
                this.CooldownToAttack.Stop();
                this.CooldownToCreateProjectile.Stop();
            }
        }

        public override void CreateProjectile(object source, ElapsedEventArgs args)
        {
            Projectile newProjectile = this.ProjectileToLaunch.Clone() as Projectile;
            newProjectile.Movement = this.ProjectileToLaunch.Movement.Clone() as MovementPattern;
            newProjectile.Movement.Parent = newProjectile;

            Vector2 targetPosition = GameState.GetPlayerPosition();
            Vector2 velocity = this.Movement.CalculateVelocity(this.Movement.CurrentPosition, targetPosition, newProjectile.Movement.Speed);

            newProjectile.Movement.Velocity = velocity;
            newProjectile.Movement.CurrentPosition = this.movement.GetActualPosition();
            newProjectile.Parent = this.Attacker;
            GameState.Projectiles.Add(newProjectile);

            Projectile secondProjectile = this.ProjectileToLaunch.Clone() as Projectile;
            secondProjectile.Movement = this.ProjectileToLaunch.Movement.Clone() as MovementPattern;
            secondProjectile.Movement.Parent = secondProjectile;

            Vector2 secondVelocity = default(Vector2);
            secondVelocity.X = (float)((velocity.X * Math.Cos(120 * (Math.PI / 180))) - (velocity.Y * Math.Sin(120 * (Math.PI / 180))));
            secondVelocity.Y = (float)((velocity.X * Math.Sin(120 * (Math.PI / 180))) + (velocity.Y * Math.Cos(120 * (Math.PI / 180))));

            secondProjectile.Movement.Velocity = secondVelocity;
            secondProjectile.Movement.CurrentPosition = this.movement.GetActualPosition();
            secondProjectile.Parent = this.Attacker;
            GameState.Projectiles.Add(secondProjectile);

            Projectile thirdProjectile = this.ProjectileToLaunch.Clone() as Projectile;
            thirdProjectile.Movement = this.ProjectileToLaunch.Movement.Clone() as MovementPattern;
            thirdProjectile.Movement.Parent = thirdProjectile;

            Vector2 thirdVelocity = default(Vector2);
            thirdVelocity.X = (float)((velocity.X * Math.Cos(240 * (Math.PI / 180))) - (velocity.Y * Math.Sin(240 * (Math.PI / 180))));
            thirdVelocity.Y = (float)((velocity.X * Math.Sin(240 * (Math.PI / 180))) + (velocity.Y * Math.Cos(240 * (Math.PI / 180))));

            thirdProjectile.Movement.Velocity = thirdVelocity;
            thirdProjectile.Movement.CurrentPosition = this.movement.GetActualPosition();
            thirdProjectile.Parent = this.Attacker;
            GameState.Projectiles.Add(thirdProjectile);
        }

        public override object Clone()
        {
            Attack newAttack = (Attack)this.MemberwiseClone();
            if (this.Movement != null)
            {
                Circular newMovement = (Circular)this.Movement.Clone();
                newAttack.Movement = newMovement;
            }

            Projectile newProjectile = (Projectile)this.ProjectileToLaunch.Clone();
            newAttack.ProjectileToLaunch = newProjectile;

            newAttack.Attacker = this.Attacker;

            newAttack.CooldownToAttack.Elapsed -= this.ExecuteAttack;
            newAttack.CooldownToAttack.Elapsed += newAttack.ExecuteAttack;
            newAttack.CooldownToCreateProjectile.Elapsed -= this.CreateProjectile;
            newAttack.CooldownToCreateProjectile.Elapsed += newAttack.CreateProjectile;

            return newAttack;
        }

        private void Move()
        {
            this.Movement.Move();
        }
    }
}
