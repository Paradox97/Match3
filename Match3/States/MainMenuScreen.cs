using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Match3.GameEntities;
using Match3.ScreenEntities;
using Match3.Controls;

namespace Match3.States
{
    public class MainMenuScreen : Screen
    {
        public MainMenuScreen(MatchGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Texture2D play = content.Load<Texture2D>("buttons/Play");
            Button playButton = new Button(play);
            playButton.Position(new Vector2(0, 0));


            Texture2D highScores = content.Load<Texture2D>("buttons/High_scores");
            Button highScoresButton = new Button(highScores);
            highScoresButton.Position(new Vector2(0, 100));


            Texture2D quit = content.Load<Texture2D>("buttons/Quit");
            Button quitButton = new Button(quit);
            quitButton.Position(new Vector2(0, 200));

            this.screenContent = new List<ScreenContent>() { playButton, highScoresButton, quitButton };//, highScoresButton, quitButton };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach(ScreenContent component in screenContent)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
