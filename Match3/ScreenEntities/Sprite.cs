using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Match3.ScreenEntities
{
    class Sprite
    {
        public Texture2D
            texture;

        public Vector2 position,
            origin;

        public Color color;

        public Sprite(Texture2D texture, Vector2 position) //plain Sprite
        {
            this.texture = texture;
            this.position = position;
            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.color = Color.CornflowerBlue;
        }

        public void SpriteGetTextures(Texture2D texture, Vector2 position) 
        {
            this.texture = texture;
            this.position = position;
            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.color = Color.CornflowerBlue;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);

        }

        public void Draw(SpriteBatch spriteBatch, int flag)
        {
            switch (flag)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
            }
          
        }

    }
}
