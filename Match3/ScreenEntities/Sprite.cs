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
        public Texture2D[]
            textureset,
            animationset,
            effectsset;

        public Texture2D
            texture;

        public Vector2 position;

        public Color color;

        public int
            state, stateByTime = 15;

        const int MAX_STATES = 3;
        public Sprite(Texture2D[] textureSet, Texture2D[] animationSet, Texture2D[] effectsSet, Vector2 position)
        {
            
        }

        public Sprite(Texture2D texture, Vector2 position) //plain Sprite
        {
            this.texture = texture;
            this.position = position;
            this.color = Color.CornflowerBlue;
        }

        public void SpriteGetTextures(Texture2D[] textureSet, Texture2D[] animationSet, Texture2D[] effectsSet, Vector2 position) //game object sprite
        {
            this.textureset = textureSet;

            this.texture = textureset[0];
        }

        public void SpriteGetTextures(Texture2D texture, Vector2 position)          //plain Sprite
        {
            this.texture = texture;
            this.position = position;
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
