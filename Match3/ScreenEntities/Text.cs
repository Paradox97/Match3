using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Match3.GameEntities;
using Match3.ScreenEntities;

namespace Match3.ScreenEntities
{
    public class Text : ScreenContent
    {
        public Text(Texture2D texture, Vector2 position) : base(texture, position)
        {
            Scale = 0.9f;
            rotation = 0f;
            origin = new Vector2(0,0);
        }
    }
}
