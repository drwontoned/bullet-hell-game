﻿namespace BulletHell
{
    using System;

    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var game = new GUI())
            {
                game.Run();
            }
        }
    }
}
