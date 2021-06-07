using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace Match3.GameEntities
{
    class Sprite
    {
        public Color color;
        public Texture2D texture;
        public Vector2 position, origin;

        public Sprite(string texturePath, Vector2 position, ContentManager content)
        {
            this.texture = content.Load<Texture2D>(texturePath);
            this.position = position;
            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.color = Color.CornflowerBlue;
        }

        /*
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);
        }
        */

    }
}
