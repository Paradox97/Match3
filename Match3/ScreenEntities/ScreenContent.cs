﻿using Microsoft.Xna.Framework;
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

        private MouseState currentMouseState,
            previousMouseState;

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

            this.bounds = new Vector2[4];
            
            this.bounds[0] = position;
            this.bounds[1] = new Vector2(position.X + texture.Width, position.Y);
            this.bounds[2] = new Vector2(position.X, position.Y + texture.Height);
            this.bounds[3] = new Vector2(position.X + texture.Width, position.Y + texture.Height);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color, 
                0f, origin, Scale, SpriteEffects.None, 0f);
        }

        public void Reposition(Vector2 position)
        {

        }
        public void Update()
        {
            Input input = Input.GetInput();
            MouseState mouseState = input.mouseInput;

            //Console.WriteLine(new Vector2(mouseState.X, mouseState.Y));

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (
                    (mouseState.X >= bounds[0].X) && (mouseState.X <= bounds[1].X)
                    &&
                    (mouseState.Y >= bounds[0].Y) && (mouseState.Y <= bounds[3].Y)
                    )
                    click.Invoke(this, new EventArgs());
            }
        }

    }
}
