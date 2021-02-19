﻿namespace BulletHell.Sprites.Movement_Patterns.Concrete_Movement_Patterns
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class Static : MovementPattern
    {
        public Static(Dictionary<string, object> staticProperties)
            : base(staticProperties)
        {
            this.Position.X = Convert.ToSingle((int)staticProperties["xPosition"]);
            this.Position.Y = Convert.ToSingle((int)staticProperties["yPosition"]);
        }
    }
}