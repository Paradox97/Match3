using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Match3.GameEntities;
using Match3.ScreenEntities;
using Match3.Controls;

namespace Match3.ScreenEntities
{
    public class Button : ScreenContent
    {
        public Button(Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.Scale = 0.7f;
            this.rotation = 0f;
            this.origin = new Vector2(0,0);
        }

    }
}
