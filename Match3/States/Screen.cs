using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Match3.GameEntities;
using Match3.ScreenEntities;
using Match3.Controls;

namespace Match3.States
{
    public abstract class Screen
    {
        protected GraphicsDeviceManager graphics;

        //protected GraphicsDevice graphicsDevice;

        protected MatchGame game;

        protected ContentManager content;

        protected List<ScreenContent> screenContent;

        public Screen(MatchGame game, GraphicsDeviceManager graphics, ContentManager content)
        {
            this.game = game;

            this.graphics = graphics;

            this.content = content;
        }

        public abstract void Update(GameTime gameTime, MouseState current, MouseState previous, KeyboardState currentKeyboard, KeyboardState previousKeyboard);

        public abstract void PostUpdate(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    }
}
