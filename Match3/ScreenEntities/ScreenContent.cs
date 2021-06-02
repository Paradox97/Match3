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
    public abstract class ScreenContent
    {

        private MouseState currentMouseState,
            previousMouseState;

        public Vector2 position;

        public Vector2[] bounds;

        public Texture2D texture;

        public Color color;
        public ScreenContent(Texture2D texture)
        {
            this.texture = texture;
            this.color = Color.White;
        }

        public void Position(Vector2 position)
        {
            this.position = position;


            this.bounds = new Vector2[4];
            this.bounds[0] = position;
            this.bounds[1] = new Vector2(position.X + texture.Width, position.Y);
            this.bounds[2] = new Vector2(position.X, position.Y + texture.Height);
            this.bounds[3] = new Vector2(position.X + texture.Width, position.Y + texture.Height);
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);
        }






    }
}
