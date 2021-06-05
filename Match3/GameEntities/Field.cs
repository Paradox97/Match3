using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


using System.Timers;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.IO;
using Match3.Controls;

namespace Match3.GameEntities
{
    class Field
    {
        public const int
            //Field consts
            FIELD_SIZE_HOR = 8,
            FIELD_SIZE_VERT = 8,
            FIELD_SIZE = 64,

            //Match bonus calculations
            MATCH_CONDITION = 3,
            LINE_BONUS = 4,
            BOMB_BONUS = 5;

        public enum FigureTypes
        {
            Circle = 0,
            Crystall = 1,
            Heart = 2,
            Pyramid = 3,
            Square = 4,
        }

        //field boundaries
        public Vector2[] bounds; //topleft, topright, bottomleft, bottomright
        public int
            width, height;

        public float offset, figure_size;

        public int score { get; set; }

        public MouseState 
            currentMouseState,
            previousMouseState;

        public int[]
            currentFigure,
            previousFigure;

        public Figure[,] field;

        public Texture2D[][] 
            textureset, //all the textures of game objects
            animationset;
        
        public Texture2D[] effectsset;

        public Texture2D self;          //field texture

        public Field(Texture2D[][] textureSet, Texture2D[][] animationSet, Texture2D[] effectsSet, Texture2D[] fieldTextures, Vector2[] bounds, float offset)
        {
            this.textureset = textureSet;
            this.animationset = animationSet;
            this.effectsset = effectsSet;
            this.self = fieldTextures[0];
            this.offset = offset;
            this.figure_size = textureset[0][0].Width;

            this.bounds = bounds;

            Console.WriteLine(this.offset);
            Console.WriteLine(this.figure_size);

            Console.WriteLine(
                "Top Left " + bounds[0].ToString() 
                + "\n" + "Top Right " + bounds[1].ToString() 
                + "\n" + "Bottom Left " + bounds[2].ToString() 
                + "\n" + "Bottom Right " + bounds[3].ToString()
                );

            this.field = new Figure[FIELD_SIZE_HOR, FIELD_SIZE_VERT];
            FieldCreate();
            //locate(new Vector2(65, 180));
        }

        public Field()
        {
            this.field = new Figure[FIELD_SIZE_HOR, FIELD_SIZE_VERT];

        }

        public int FieldCreateFigureRand()
        {
            FigureTypes[] figures = ((FigureTypes[])Enum.GetValues(typeof(FigureTypes)));
            int figureType = (int)figures[new Random().Next(0, figures.Length)];

            return figureType;
        }

        public void FieldCreate()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {       
                    //figure position on Screen
                    Vector2 figurePos = new Vector2(
                        this.bounds[0].X + j * offset + j * figure_size + offset,
                        this.bounds[0].Y + i * offset + i * figure_size + offset
                        );

                    //figure bounds
                    Vector2[] figureBounds = new Vector2[4]
                    {
                        figurePos,
                        new Vector2(figurePos.X + figure_size, figurePos.Y),
                        new Vector2(figurePos.X, figurePos.Y + figure_size),
                        new Vector2(figurePos.X + figure_size, figurePos.Y + figure_size)
                    };

                    int type = FieldCreateFigureRand();

                    Figure figure = new Figure(figurePos, figureBounds, type, textureset[type], animationset[type], effectsset);        // type, textureset[type], animationset[type], effectsset delta between all figures
                    this.field[j, i] = figure;
                }
            }






        }

        public void FieldDraw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();

            spriteBatch.Draw(self, bounds[0], Color.White);

            for (int i = 0; i < FIELD_SIZE_HOR; i++)
            {
                for (int j = 0; j < FIELD_SIZE_VERT; j++)
                {
                    //field[i, j].Next();
                    spriteBatch.Draw(field[i, j].texture, field[i, j].position, Color.White);
                }
            }

            //spriteBatch.End();
        }

        public int[] FieldLocate(Vector2 position)       //field position by cursor position
        {
            //in bounds
            if (
                (position.X >= bounds[0].X) && (position.X <= bounds[1].X)
                &&
                (position.Y >= bounds[0].Y) && (position.Y <= bounds[3].Y)
                )
            {
                for (int i = 0; i < FIELD_SIZE_HOR; i++)
                {
                    for (int j = 0; j < FIELD_SIZE_VERT; j++)
                    {
                        if (
                           (position.X >= field[i, j].bounds[0].X) && (position.X <= field[i, j].bounds[1].X)
                            &&
                           (position.Y >= field[i, j].bounds[0].Y) && (position.Y <= field[i, j].bounds[3].Y)
                           )
                        {
                            //Console.WriteLine(i.ToString() + j.ToString());
                            return new int[2] { i, j };
                        }
                    }
                }
                //in between
                Console.WriteLine("In Between");
                return new int[2] { -1, -1 };
            }
            //out of bounds
            else
            {
                Console.WriteLine("Out of bounds");
                return new int[2] { -1, -1 };
            }
        }

        /*
        public Vector2 locate(int[] fieldPosition)  //on screen bounaries by field position
        {
            return new Vector2();
            //return new Vector2[4] { };
        }
        */

        public void FieldInput()
        {
            this.previousMouseState = this.currentMouseState;
            Input input = Input.GetInput();
            this.currentMouseState = input.mouseInput;

            if (
                (this.currentMouseState.LeftButton == ButtonState.Pressed)
                &&
                (this.previousMouseState.LeftButton == ButtonState.Released)
                )
            {
                this.previousFigure = currentFigure;

                this.currentFigure = FieldLocate(new Vector2(currentMouseState.X, currentMouseState.Y));
                
                if (previousFigure != null)
                {
                    Console.WriteLine("CURR" + currentFigure[0].ToString() + currentFigure[1].ToString());
                    Console.WriteLine("PREV" + previousFigure[0].ToString() + previousFigure[1].ToString());
                    
                    FieldSwap(currentFigure[0], currentFigure[1], previousFigure[0], previousFigure[1]);

                    //Console.WriteLine("VALUES" + this.field[currentFigure[0], currentFigure[1]].figureType.ToString() +  this.field[previousFigure[0], previousFigure[1]].figureType.ToString());

                    if ((currentFigure[0] == previousFigure[0])&&(currentFigure[1] == previousFigure[1]))
                    {
                        Console.WriteLine("No Swap");
                    }
                    Console.WriteLine("Swap");
             
                    this.previousFigure = null;
                    this.currentFigure = null;
                }
            }

        }
        public void GenerateByDifficulty(int difficultySeed)
        {
            Random random = new Random();

            int steps, seed;
            int i, j;   //indices of field

            //const int min = 5, max = 7;

            switch (difficultySeed)
            {
                case 1:
                    steps = random.Next(5, 7);

                    for (int k = 0; k < steps; k++)
                    {
                        i = random.Next(0, FIELD_SIZE_HOR - 1);
                        j = random.Next(0, FIELD_SIZE_VERT - 1);
                        seed = random.Next(0, 3);
                        FieldGenerate(i, j, seed);
                    }
                    break;
                case 2:
                    steps = random.Next(3, 5);

                    for (int k = 0; k < steps; k++)
                    {
                        i = random.Next(0, FIELD_SIZE_HOR - 1);
                        j = random.Next(0, FIELD_SIZE_VERT - 1);
                        seed = random.Next(0, 3);
                        FieldGenerate(i, j, seed);
                    }

                    break;
                case 3:
                    steps = random.Next(2, 4);

                    for (int k = 0; k < steps; k++)
                    {
                        i = random.Next(0, FIELD_SIZE_HOR - 1);
                        j = random.Next(0, FIELD_SIZE_VERT - 1);
                        seed = random.Next(0, 3);
                        FieldGenerate(i, j, seed);
                    }

                    break;
            }
        }

        public void FieldGenerate(int i, int j, int seed)
        {
            int type = this.field[i, j].figureType;

            switch (seed)
            {
                case 0:
                    if (i + 2 < FIELD_SIZE_HOR)
                    {
                        this.field[i + 1, j].figureType = type;
                        this.field[i + 2, j].figureType = type;
                        return;
                    }

                    if ((i - 1 > 0) && (i + 1 < FIELD_SIZE_HOR))
                    {
                        this.field[i + 1, j].figureType = type;
                        this.field[i - 1, j].figureType = type;
                        return;
                    }

                    break;

                case 1:
                    if (i - 2 > 0)
                    {
                        this.field[i - 1, j].figureType = type;
                        this.field[i - 2, j].figureType = type;
                        return;
                    }

                    break;

                case 2:
                    if (j + 2 < FIELD_SIZE_HOR)
                    {
                        this.field[i, j + 1].figureType = type;
                        this.field[i, j + 2].figureType = type;
                        return;
                    }

                    if ((j - 1 > 0) && (j + 1 < FIELD_SIZE_HOR))
                    {
                        this.field[i, j + 1].figureType = type;
                        this.field[i, j - 1].figureType = type;
                        return;
                    }

                    break;

                case 3:
                    if (j - 2 > 0)
                    {
                        this.field[i, j - 1].figureType = type;
                        this.field[i, j - 2].figureType = type;
                        return;
                    }

                    break;
            }
        }
        


        public void FieldSwap(int i1, int j1, int i2, int j2)
        {
            if ((i1 == -1) || (i2 == -1))
                return;

            Texture2D tempTex = this.field[i1, j1].texture;
            int type = this.field[i1, j1].figureType;

            this.field[i1, j1].texture = this.field[i2, j2].texture;
            this.field[i1, j1].figureType = this.field[i2, j2].figureType;

            this.field[i2, j2].texture = tempTex;
            this.field[i2, j2].figureType = type;
        }


        public void FieldUpdate()
        {



        }

        public void Draw()      //debug
        {
            Console.SetCursorPosition(0, 0);

            string output = string.Empty;

            for (int i = 0; i < FIELD_SIZE_HOR; i++)
            {
                for (int j = 0; j < FIELD_SIZE_VERT; j++)
                {
                    output += this.field[j, i].figureType;
                }
                output += "|\n";
            }

            for (int j = 0; j < 8; j++)
            {
                output += "^";
            }

            Console.Write(output);
        }
    }

}
