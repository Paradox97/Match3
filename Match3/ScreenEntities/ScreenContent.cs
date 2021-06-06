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
    public abstract class ScreenContent
    {
        public Vector2 position, origin;

        public Vector2[] bounds;

        public Texture2D texture;

        public Color color;

        public float Scale;

        public float rotation;

        public EventHandler click;

        public ScreenContent(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.color = Color.White;
            
            this.position = position;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color, 
                0f, origin, Scale, SpriteEffects.None, 0f);
        }

        public void Reposition(Vector2 position)
        {

        }
        public virtual void Update(MouseState current, MouseState previous)
        {
         
        }

    }
}
