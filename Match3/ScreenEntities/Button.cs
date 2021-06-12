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

        private MouseState currentMouseState,
            previousMouseState;

        public Button(Texture2D texture, Vector2 position) : base(texture, position)
        {
            Scale = 0.7f;
            rotation = 0f;
            origin = new Vector2(0, 0);


            bounds = new Vector2[4] { 
                position, 
                new Vector2(position.X + texture.Width * Scale, position.Y), 
                new Vector2(position.X, position.Y + texture.Height * Scale), 
                new Vector2(position.X + texture.Width * Scale, position.Y + texture.Height * Scale) 
            };
        }

        public override void Update(MouseState current, MouseState previous)
        {
            previousMouseState = previous;
            currentMouseState = current;

            if (
                (currentMouseState.LeftButton == ButtonState.Pressed)
                &&
                (previousMouseState.LeftButton == ButtonState.Released)
                )
            {
                if (
                    (currentMouseState.X >= bounds[0].X) && (currentMouseState.X <= bounds[1].X)
                    &&
                    (currentMouseState.Y >= bounds[0].Y) && (currentMouseState.Y <= bounds[3].Y)
                    )
                    click.Invoke(this, new EventArgs());
            }

        }

    }
}
