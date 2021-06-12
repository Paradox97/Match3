using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using Match3.ScreenEntities;

namespace Match3.GameEntities
{
    class SelectArgs : EventArgs
    {
        public int[] position;

        public SelectArgs(int[] position)
        {
            this.position = position;
        }
    }

    class FigureReplacement
    {
        private MouseState currentMouseState,
            previousMouseState;

        public Vector2[] bounds;

        public Sprite self;

        public int[] position;      //field position

        EventHandler select;

        public const int SIZE = 50;

        public FigureReplacement(Vector2 position, Paths paths, ContentManager content)
        {
            bounds = new Vector2[4] {
                position,
                new Vector2(position.X + SIZE, position.Y),
                new Vector2(position.X, position.Y + SIZE),
                new Vector2(position.X + SIZE, position.Y + SIZE)
            };

        }

        public void Update(MouseState current, MouseState previous)
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
                    select.Invoke(this, new SelectArgs(position));
            }

        }



    }
}
