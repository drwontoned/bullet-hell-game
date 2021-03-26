﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BulletHell.States.Emitters
{
    public class SnowEmitter : Emitter
    {

        public SnowEmitter(SpriteLike particle)
          : base(particle)
        {

        }

        protected override void ApplyGlobalVelocity()
        {
            var xSway = (float)BulletHell.Random.Next(-2, 2);
            foreach (var particle in _particles)
                particle.Velocity.X = (xSway * particle.Scale) / 50;
        }

        protected override SpriteLike GenerateParticle()
        {
            var sprite = _particlePrefab.Clone() as SpriteLike;

            var xPosition = BulletHell.Random.Next(0, BulletHell.ScreenWidth);
            var ySpeed = BulletHell.Random.Next(10, 100) / 100f;

            sprite.Position = new Microsoft.Xna.Framework.Vector2(xPosition, -sprite.Rectangle.Height);
            sprite.Opacity = (float)BulletHell.Random.NextDouble();
            sprite.Rotation = MathHelper.ToRadians(BulletHell.Random.Next(0, 360));
            sprite.Scale = (float)BulletHell.Random.NextDouble() + BulletHell.Random.Next(0, 3);
            sprite.Velocity = new Microsoft.Xna.Framework.Vector2(0, ySpeed);

            return sprite;
        }
    }
}