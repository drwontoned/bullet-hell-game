﻿namespace BulletHell.Sprites.The_Player
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BulletHell.Sprites.Entities;
    using BulletHell.Sprites.Entities.Enemies;
    using BulletHell.Sprites.Movement_Patterns;
    using BulletHell.Sprites.PowerUps;
    using BulletHell.Sprites.PowerUps.Concrete_PowerUps;
    using BulletHell.Sprites.Projectiles;
    using BulletHell.The_Player;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Player : Entity
    {
        public bool SlowMode;
        public bool Invincible;
        private double initialSpawnTime;
        private bool spawning;
        private bool resetGameTime = true;

        private KeyboardState currentKey;
        private KeyboardState previousKey;

        public Player(Texture2D texture, Color color, MovementPattern movement, int hp, List<Attack> attacks)
            : base(texture, color, movement, hp, attacks)
        {
            this.spawning = true;
            this.Invincible = true;
            this.damageLevel = 0;

            foreach (Attack attack in attacks)
            {
                attack.ExecuteAttackEventHandler += this.LaunchAttack;
                attack.Attacker = this;
                attack.ProjectileToLaunch.Parent = attack;
            }
        }

        public int Lives { get; set; }

        // Serves as hitbox; Player hitbox is smaller than enemies'
        //public override Rectangle Rectangle
        //{
        //    get
        //    {
        //        //int xPos = (int)(this.Movement.CurrentPosition.X + (this.Texture.Width / 4));
        //        //int yPos = (int)(this.Movement.CurrentPosition.Y + (this.Texture.Height / 4));
        //        return new Rectangle(this.Movement.CurrentPosition.ToPoint(), new Point(this.Texture.Width, this.Texture.Height));
        //    }
        //}

        public override void Update(GameTime gameTime, List<Sprite> enemies)
        {
            if (this.resetGameTime)
            {
                this.initialSpawnTime = gameTime.TotalGameTime.TotalSeconds;
                this.resetGameTime = !this.resetGameTime;
            }

            this.previousKey = this.currentKey;
            this.currentKey = Keyboard.GetState();

            this.SetInvincibility(gameTime);

            // check if slow speed
            this.SlowMode = this.IsSlowPressed();

            this.Move();
        }

        public override void LaunchAttack(object source, EventArgs args)
        {
            if (this.currentKey.IsKeyDown(Input.Attack))
            {
                base.LaunchAttack(source, args);
            }
        }

        public override void OnCollision(Sprite sprite)
        {
            this.Movement.ZeroXVelocity();
            this.Movement.ZeroYVelocity();

            if (sprite is PowerUp)
            {
                if (sprite is DamageUp)
                {
                    this.IncreaseDamage();
                }
                else if (sprite is ExtraLife)
                {
                    this.HP += 1;
                }
            }
            else if (this.Invincible == false)
            {
                if (sprite is Projectile projectile && projectile.Parent != this)
                {
                    this.IsRemoved = true;
                }
                else if (sprite is Enemy)
                {
                    this.IsRemoved = true;
                }
            }
        }

        public bool IsSlowPressed()
        {
            if (this.currentKey.IsKeyDown(Input.SlowMode))
            {
                this.Movement.CurrentSpeed = this.Movement.Speed / 2;
                return true;
            }

            return false;
        }

        public void Respawn(GameTime gameTime)
        {
            this.Respawn();
            this.IsRemoved = false;
            this.spawning = true;
            this.Invincible = true;
            this.initialSpawnTime = gameTime.TotalGameTime.TotalSeconds;
            this.Attacks.ForEach(item =>
            {
                item.CooldownToAttack.Stop();
            });
        }

        private void SetInvincibility(GameTime gameTime)
        {
            if (this.spawning == true)
            {
                if ((gameTime.TotalGameTime.TotalSeconds - this.initialSpawnTime) >= 2)
                {
                    this.Invincible = false;
                    this.spawning = false;
                }
            }
            else
            {
                if (this.currentKey.IsKeyDown(Input.CheatingMode) && !this.previousKey.IsKeyDown(Input.CheatingMode))
                {
                    this.Invincible = !this.Invincible;
                }
            }
        }

        private void IncreaseDamage()
        {
            this.damageLevel += 1;
            switch (this.damageLevel)
            {
                case 1:
                    this.DamageModifier += 1;
                    break;
                case 2:
                    this.DamageModifier += 1;
                    break;
                case 3:
                    this.DamageModifier += 1;
                    break;
                default:
                    Debug.WriteLine("At max damage level");
                    break;
            }

            this.Attacks.ForEach(item => item.ProjectileToLaunch.SetTextureBasedOnDamageLevel());
        }
    }
}
