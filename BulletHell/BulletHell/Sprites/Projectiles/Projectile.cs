﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHell.Sprites.Projectiles
{
    class Projectile : Sprite
    {
        public Projectile(Texture2D texture) : base(texture)
        {

        }

        public override void Update(GameTime gametime, List<Sprite> sprits)
        {
            _timer += (float)gametime.ElapsedGameTime.TotalSeconds;

            if(_timer > lifeSpan)
            {
                IsRemoved = true;
            }

            Position += DirectionalLight * LinearVelocity;
        }
    }
}
