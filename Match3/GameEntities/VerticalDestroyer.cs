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
    class VerticalDestroyer:ICloneable
    {
        public ContentManager content;

        public string[] texturePaths;

        public string[] effectsPaths;

        public string[] animationPaths;

        public EventHandler click;

        public Texture2D
            texture;

        public Sprite sprite;
        public string[] pathPrefixes;

        public Vector2[] bounds;
        public Vector2 position;

        public int figureType;

        const int FIGURESIZE = 50;

        public int
            state, stateByTime = 15;
        
        int stateOfAnimation;

        const int ANIMATIONTIME = 1;

        public bool
            isSelected,
            isDragged,
            isBlownUp,
            isFalling,
            isDestroyed;

        private struct State
        {
            public string[] texturePaths;

            public string[] effectsPaths;

            public string[] animationPaths;

            public int type;

            public int subType;
        }

        private struct animationState
        {
            public Vector2 position;

            public string[] texturePaths;

            public string[] effectsPaths;

            public string[] animationPaths;
        }

        private MouseState currentMouseState,
            previousMouseState;


        private List<animationState> animationStates;
        private List<State> figureStates;

        public int subType;

        const int MAX_STATES = 3;

        public VerticalDestroyer(Figure figure, int newType, Paths paths, ContentManager content) //creating new figure from another one
        {
            position = figure.position;
            bounds = figure.bounds;
            figureType = newType;

            this.content = content;

            this.pathPrefixes = paths.figurePrefixes;
            this.texturePaths = paths.figureTexturePaths[figureType];
            this.animationPaths = paths.figureAnimationPaths[figureType];
            this.effectsPaths = paths.effectsPaths;

            this.sprite = new Sprite(pathPrefixes[0] + texturePaths[0], position, content);
            this.texture = sprite.texture;

            animationStates = new List<animationState>();
            figureStates = new List<State>();

            Falldown();
        }

        public VerticalDestroyer(int horizontalPos, int verticalPos, int type, float offset, Vector2 fieldBounds, Paths paths, ContentManager content)
        {
            position = new Vector2(
                         fieldBounds.X + horizontalPos * offset + horizontalPos * FIGURESIZE + offset,
                         fieldBounds.Y + verticalPos * offset + verticalPos * FIGURESIZE + offset
                         );

            //figure bounds
            bounds = new Vector2[4]
            {
                        position,
                        new Vector2(position.X + FIGURESIZE, position.Y),
                        new Vector2(position.X, position.Y + FIGURESIZE),
                        new Vector2(position.X + FIGURESIZE, position.Y + FIGURESIZE)
            };

            figureType = type;

            this.content = content;
            this.pathPrefixes = paths.figurePrefixes;
            this.texturePaths = paths.figureTexturePaths[figureType];
            this.animationPaths = paths.figureAnimationPaths[figureType];
            this.effectsPaths = paths.effectsPaths;

            this.sprite = new Sprite(pathPrefixes[0] + texturePaths[0], position, content);
            this.texture = sprite.texture;

            animationStates = new List<animationState>();
            figureStates = new List<State>();

            Falldown();
        }

        public VerticalDestroyer(Vector2 position, Vector2[] bounds, int type, Paths paths, ContentManager content) //creating figure at position
        {
            this.position = position;
            this.bounds = bounds;
            figureType = type;

            this.content = content;

            this.pathPrefixes = paths.figurePrefixes;
            this.texturePaths = paths.figureTexturePaths[figureType];
            this.animationPaths = paths.figureAnimationPaths[figureType];
            this.effectsPaths = paths.effectsPaths;

            this.sprite = new Sprite(pathPrefixes[0] + texturePaths[0], position, content);
            this.texture = sprite.texture;

            animationStates = new List<animationState>();
            figureStates = new List<State>();
            Falldown();
        }

        public void Falldown()
        {
            //Console.WriteLine(position);
            for (int i = 200; i > -10;)
            {
                animationStates.Add(new animationState() { position = this.position - new Vector2(0, i) });
                i = i - 10;
            }
           //foreach (FigureState animation in animationStates)
                //Console.WriteLine(animation.position);
        }

        public bool IsBusy()
        {
            if (animationStates.Count > 0)
                return true;

            return false;
        }

        public void Change(Figure next)
        {
            figureStates.Add(new State() { texturePaths = next.texturePaths, animationPaths = next.animationPaths, effectsPaths = next.effectsPaths, type = next.figureType, subType = next.subType});
            //changeStates.Add(new FigureState(next));
            //animationStates.Add(new FigureState());
        }

        public void Move(Figure next)
        {
            var swapAnimation = new List<animationState>();
            int seed = 0;
            int deltaX = (int)(position.X - next.position.X);

            if (deltaX > 0)
                seed = 1;

            if (deltaX < 0)
                seed = 2;

            int deltaY = (int)(position.Y - next.position.Y);

            if (deltaY > 0)
                seed = 3;

            if (deltaY < 0)
                seed = 4;

            //Console.WriteLine(deltaX.ToString());

            switch (seed)
            {
                case 1:
                    for (int i = 0; i < deltaX + 1; )
                    {
                        Vector2 pos = next.position + new Vector2(i, 0);
                        swapAnimation.Add(new animationState() {position = pos, texturePaths = next.texturePaths, animationPaths = next.animationPaths, effectsPaths = next.effectsPaths});
                        i = i + 1;
                    }
                    break;
                case 2:
                    for (int i = 0; i < Math.Abs(deltaX) + 1;)
                    {
                        Vector2 pos = next.position - new Vector2(i, 0);
                        swapAnimation.Add(new animationState() { position = pos, texturePaths = next.texturePaths, animationPaths = next.animationPaths, effectsPaths = next.effectsPaths });
                        i = i + 1;
                    }
                    break;
                case 3:
                    for (int i = 0; i < deltaY + 1; )
                    {
                        Vector2 pos = next.position + new Vector2(0, i);
                        swapAnimation.Add(new animationState() { position = pos, texturePaths = next.texturePaths, animationPaths = next.animationPaths, effectsPaths = next.effectsPaths });
                        i = i + 1;
                    }
                    break;
                case 4:
                    for (int i = 0; i < Math.Abs(deltaY) + 1;)
                    {
                        Vector2 pos = next.position - new Vector2(0, i);
                        swapAnimation.Add(new animationState() { position = pos, texturePaths = next.texturePaths, animationPaths = next.animationPaths, effectsPaths = next.effectsPaths });
                        i = i + 1;
                    }
                    break;
            }

            SkipFrames(swapAnimation);

            foreach (var animation in swapAnimation)
                animationStates.Add(animation);

            Change(next);
        }



        private void SkipFrames(List <animationState> animation)
        {
            for (int i = 0; i < animation.Count-5;)
            {
                animation.RemoveAt(i);
                i = i + 5;
            }
        }

        public void Swap(Figure swapped, int i1, int j1, int i2, int j2)
        {
            if ((Math.Abs(i1 - i2) >= 1) && (Math.Abs(j1 - j2) >= 1))
                return;

            Move(swapped);
        }

        public void Update()
        {
            if (figureStates.Count != 0)
            {
                figureType = figureStates[0].type;
                subType = figureStates[0].subType;
                texturePaths = figureStates[0].texturePaths;
                animationPaths = figureStates[0].animationPaths;
                effectsPaths = figureStates[0].effectsPaths;
                figureStates.RemoveAt(0);
            }

            if (IsBusy())
                return;
        }

        public void Input(MouseState current, MouseState previous)
        {
            previousMouseState = previous;

            currentMouseState = current;

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

        public void PostUpdate()
        {
            Animate();
        }

        public void Animate()
        {
            if (animationStates.Count > 0)
            {
                if (animationStates[0].texturePaths != null)
                    sprite = new Sprite(pathPrefixes[0] + animationStates[0].texturePaths[0], animationStates[0].position, content);
                else
                    sprite = new Sprite(pathPrefixes[0] + texturePaths[0], animationStates[0].position, content);

                animationStates.RemoveAt(0);
                return;
            }
            sprite = new Sprite(pathPrefixes[0] + texturePaths[0], position, content);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            sprite.Draw(spritebatch);
        }

        public void Next()
        {
            state += 1;
            //this.texture = animationset[(state/stateByTime) % MAX_STATES];
        }

        public void FigureSelect()
        {
            this.isSelected = true;
        }

        public void FigureDeselect()
        {
            this.isSelected = false;
        }

        public void FigureBlast()
        {
            this.isBlownUp = true;
        }

        public void FigureDestroyDown()
        {
            this.isDestroyed = true;
            //this.texture = effectsset[1];
        }

        public void FigureDestroyLeft()
        {
            this.isDestroyed = true;
            //this.texture = effectsset[2];
        }

        public void FigureDestroyRight()
        {
            this.isDestroyed = true;
            //this.texture = effectsset[3];
        }

        public void FigureDestroyUp()
        {
            this.isDestroyed = true;
            //this.texture = effectsset[4];
        }

        public int FigureAnimate()            //some animations may require completion
        {
            if (this.isSelected == true)
            {
                state = state + 1;
                //this.texture = animationset[(state / stateByTime) % MAX_STATES];
                return 1;
            }

            if (this.isBlownUp == true)
            {
                this.isBlownUp = false;
                //this.texture = effectsset[0];
                return 2;       
            }

            if (this.isDestroyed == true)
            {
                this.isDestroyed = false;
                this.isBlownUp = true;
                return 3;
            }

            state = 0;

            return 0;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void FigureCreateOfType(int type)
        {
            this.figureType = type;
        }
        
        
        public void FigureCreateNotOfType(int type)             
        {
            /*
            FigureType[] figures = ((FigureType[])Enum.GetValues(typeof(FigureType)));

            List<FigureType> figuresSubSet = new List<FigureType>();

            for (int i = 0; i< figures.Length; i++)
            {
                if ((int)figures[i] != type)
                    figuresSubSet.Add(figures[i]);
            }

            this.figureType = (int)figuresSubSet[new Random().Next(1, figuresSubSet.Count - 1)];
            */
        }
       

    }
}
