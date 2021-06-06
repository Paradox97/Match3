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
            this.Scale = 0.7f;
            this.rotation = 0f;
            this.origin = new Vector2(0, 0);
        }

        public override void Update()
        {
            Input old = Input.GetOldInput();
            
            previousMouseState = old.mouseInput;

            Input input = Input.GetInput();

            currentMouseState = input.mouseInput;

            //Console.WriteLine(new Vector2(mouseState.X, mouseState.Y));

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
