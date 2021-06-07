using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;


using System.Timers;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.IO;
using Match3.Controls;
using Match3.GameEntities;
using Match3.ScreenEntities;

namespace Match3.GameEntities
{
    class Field
    {
        public const int
            //Field consts
            FIELD_SIZE_HORIZONTAL = 8,
            FIELD_SIZE_VERTICAL = 8,
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

        ContentManager content;

        public enum FieldTypes
        {
            field_1 = 0
        };

        string 
            path;

        string[] figurePrefixes;
        string prefix;

        //field boundaries
        public Vector2[] bounds; //topleft, topright, bottomleft, bottomright

        public float 
            figureOffset, figureSize;

        public int score { get; set; }

        public int[] 
            currentFigure, previousFigure;

        public Figure[,] field;

        public string[][]
            figureTexturePaths,
            figureAnimationPaths;

        public string[] effectsPaths;

        public Texture2D[][] 
            textureset, //all the textures of game objects
            animationset;
        
        public Texture2D[] effectsset;

        //public Texture2D self;          //field texture

        public Sprite self;

        public Field(Texture2D[][] textureSet, Texture2D[][] animationSet, Texture2D[] effectsSet, float figureSize, float offset, Vector2[] bounds, ContentManager content, string[] pathPrefixes)
        {
            textureset = textureSet;
            animationset = animationSet;
            effectsset = effectsSet;

            this.content = content;
            
            prefix = pathPrefixes[0];
            figurePrefixes = new string[2] { pathPrefixes[1], pathPrefixes[2]};


            FieldTypes[] fields = ((FieldTypes[])Enum.GetValues(typeof(FieldTypes)));
            path = fields[new Random().Next(0, fields.Length)].ToString();
            self = new Sprite(prefix + path, bounds[0], content);

            this.bounds = bounds;

            this.figureOffset = offset;
            this.figureSize = figureSize;

            #region figure textures
            figureTexturePaths = new string[5][];

            figureTexturePaths[0] = new string[4]
            {
                "circle/circle",
                "circle/circle_linehor",
                "circle/circle_linevert",
                "circle/circle_bomb"
            };

            figureTexturePaths[1] = new string[4]
            {
                "crystall/crystall",
                "crystall_linehor",
                "crystall_linevert",
                "crystall/crystall_bomb"
            };

            figureTexturePaths[2] = new string[4]
            {
                "heart/heart",
                "heart/heart_linehor",
                "heart/heart_linevert",
                "heart/heart_bomb"
            };

            figureTexturePaths[3] = new string[4]
            {
                "pyramid/pyramid",
                "pyramid/pyramid_linehor",
                "pyramid/pyramid_linevert",
                "pyramid/pyramid_bomb"
            };

            figureTexturePaths[4] = new string[4]
            {
                "square/square",
                "square/square_linehor",
                "square/square_linevert",
                "square/square_bomb"
            };
            #endregion

            #region figure animation

            this.figureAnimationPaths = new string[5][];

            this.figureAnimationPaths[0] = new string[3]
            {
                "circle/circle_shine1",
                "circle/circle_shine2",
                "circle/circle_shine3"
            };

            this.figureAnimationPaths[1] = new string[3]
            {
                "crystall/crystall_shine1",
                "crystall/crystall_shine2",
                "crystall/crystall_shine3"
            };

            this.figureAnimationPaths[2] = new string[3]
            {
                "heart/heart_shine1",
                "heart/heart_shine2",
                "heart/heart_shine3"
            };

            this.figureAnimationPaths[3] = new string[3]
            {
                "pyramid/pyramid_shine1",
                "pyramid/pyramid_shine2",
                "pyramid/pyramid_shine3"
            };

            this.figureAnimationPaths[4] = new string[3]
            {
                "square/square_shine1",
                "square/square_shine2",
                "square/square_shine3"
            };
            #endregion

            #region effects

            this.effectsPaths = new string[5] {
                "blast",
                "destroyer_down",
                "destroyer_left",
                "destroyer_right",
                "destroyer_up"
                    };
            #endregion

            /*
            Console.WriteLine(this.offset);
            Console.WriteLine(this.figure_size);

            Console.WriteLine(
                "Top Left " + bounds[0].ToString() 
                + "\n" + "Top Right " + bounds[1].ToString() 
                + "\n" + "Bottom Left " + bounds[2].ToString() 
                + "\n" + "Bottom Right " + bounds[3].ToString()
                );
            */

            this.field = new Figure[FIELD_SIZE_HORIZONTAL, FIELD_SIZE_VERTICAL];
            Create();
        }

        public Field()
        {
            this.field = new Figure[FIELD_SIZE_HORIZONTAL, FIELD_SIZE_VERTICAL];

        }

        public int CreateFigure()
        {
            FigureTypes[] figures = ((FigureTypes[])Enum.GetValues(typeof(FigureTypes)));
            int figureType = (int)figures[new Random().Next(0, figures.Length)];

            return figureType;
        }

        public void Create()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {       
                    //figure position on Screen
                    Vector2 figurePos = new Vector2(
                        this.bounds[0].X + j * figureOffset + j * figureSize + figureOffset,
                        this.bounds[0].Y + i * figureOffset + i * figureSize + figureOffset
                        );

                    //figure bounds
                    Vector2[] figureBounds = new Vector2[4]
                    {
                        figurePos,
                        new Vector2(figurePos.X + figureSize, figurePos.Y),
                        new Vector2(figurePos.X, figurePos.Y + figureSize),
                        new Vector2(figurePos.X + figureSize, figurePos.Y + figureSize)
                    };

                    int type = CreateFigure();

                    Figure figure = new Figure(figurePos, figureBounds, type, figurePrefixes, figureTexturePaths[type], figureAnimationPaths[type], effectsPaths, content);        // type, textureset[type], animationset[type], effectsset delta between all figures
                    this.field[j, i] = figure;
                }
            }

            GenerateByDifficulty(1);




        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(self.texture, bounds[0], Color.White);

            for (int i = 0; i < FIELD_SIZE_HORIZONTAL; i++)
            {
                for (int j = 0; j < FIELD_SIZE_VERTICAL; j++)
                {
                    spriteBatch.Draw(field[i, j].texture, field[i, j].position, Color.White);
                }
            }
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
                for (int i = 0; i < FIELD_SIZE_HORIZONTAL; i++)
                {
                    for (int j = 0; j < FIELD_SIZE_VERTICAL; j++)
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

        public void Input(MouseState current, MouseState previous)
        {

            if (
                (current.LeftButton == ButtonState.Pressed)
                &&
                (previous.LeftButton == ButtonState.Released)
                )
            {
                previousFigure = currentFigure;

                currentFigure = FieldLocate(new Vector2(current.X, current.Y));
                
                if (previousFigure != null)
                {
                    Console.WriteLine("CURR" + currentFigure[0].ToString() + currentFigure[1].ToString());
                    Console.WriteLine("PREV" + previousFigure[0].ToString() + previousFigure[1].ToString());
                    
                    Swap(currentFigure[0], currentFigure[1], previousFigure[0], previousFigure[1]);

                    if ((currentFigure[0] == previousFigure[0])&&(currentFigure[1] == previousFigure[1]))
                    {
                        Console.WriteLine("No Swap");
                    }
                    Console.WriteLine("Swap");
             
                    previousFigure = null;
                    currentFigure = null;
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
                        i = random.Next(0, FIELD_SIZE_HORIZONTAL - 1);
                        j = random.Next(0, FIELD_SIZE_VERTICAL - 1);
                        seed = random.Next(0, 3);
                        Generate(i, j, seed);
                    }
                    break;
                case 2:
                    steps = random.Next(3, 5);

                    for (int k = 0; k < steps; k++)
                    {
                        i = random.Next(0, FIELD_SIZE_HORIZONTAL - 1);
                        j = random.Next(0, FIELD_SIZE_VERTICAL - 1);
                        seed = random.Next(0, 3);
                        Generate(i, j, seed);
                    }

                    break;
                case 3:
                    steps = random.Next(2, 4);

                    for (int k = 0; k < steps; k++)
                    {
                        i = random.Next(0, FIELD_SIZE_HORIZONTAL - 1);
                        j = random.Next(0, FIELD_SIZE_VERTICAL - 1);
                        seed = random.Next(0, 3);
                        Generate(i, j, seed);
                    }

                    break;
            }
        }

        public void Generate(int i, int j, int seed)
        {
            int type = field[i, j].figureType;

            switch (seed)
            {
                case 0:
                    if (i + 2 < FIELD_SIZE_HORIZONTAL)
                    {
                        field[i + 1, j].figureType = type;
                        field[i + 1, j].texture = textureset[type][0];

                        field[i + 2, j].figureType = type;
                        field[i + 2, j].texture = textureset[type][0];
                        return;
                    }

                    if ((i - 1 > 0) && (i + 1 < FIELD_SIZE_HORIZONTAL))
                    {
                        field[i + 1, j].figureType = type;
                        field[i + 1, j].texture = textureset[type][0];


                        field[i - 1, j].figureType = type;
                        field[i - 1, j].texture = textureset[type][0];
                        return;
                    }

                    break;

                case 1:
                    if (i - 2 > 0)
                    {
                        field[i - 1, j].figureType = type;
                        field[i - 1, j].texture = textureset[type][0];

                        field[i - 2, j].figureType = type;
                        field[i - 2, j].texture = textureset[type][0];
                        return;
                    }

                    break;

                case 2:
                    if (j + 2 < FIELD_SIZE_HORIZONTAL)
                    {
                        field[i, j + 1].figureType = type;
                        field[i, j + 1].texture = textureset[type][0];

                        field[i, j + 2].figureType = type;
                        field[i, j + 2].texture = textureset[type][0];
                        return;
                    }

                    if ((j - 1 > 0) && (j + 1 < FIELD_SIZE_HORIZONTAL))
                    {
                        field[i, j + 1].figureType = type;
                        field[i, j + 1].texture = textureset[type][0];

                        field[i, j - 1].figureType = type;
                        field[i, j - 1].texture = textureset[type][0];
                        return;
                    }

                    break;

                case 3:
                    if (j - 2 > 0)
                    {
                        field[i, j - 1].figureType = type;
                        field[i, j - 1].texture = textureset[type][0];

                        field[i, j - 2].figureType = type;
                        field[i, j - 2].texture = textureset[type][0];
                        return;
                    }

                    break;
            }
        }
        


        public void Swap(int i1, int j1, int i2, int j2)
        {
            if ((i1 == -1) || (i2 == -1))
                return;
            /*
            int type1 = field[i1, j1].figureType;
            int type2 = field[i2, j2].figureType;

            this.field[i1, j1].ChangeType(figureTexturePaths[type2], figureAnimationPaths[type2]);
            this.field[i2, i2].ChangeType(figureTexturePaths[type1], figureAnimationPaths[type1]);
            */

            Texture2D tempTex = this.field[i1, j1].texture;
            int type = this.field[i1, j1].figureType;

            this.field[i1, j1].texture = this.field[i2, j2].texture;
            this.field[i1, j1].figureType = this.field[i2, j2].figureType;

            this.field[i2, j2].texture = tempTex;
            this.field[i2, j2].figureType = type;
        }


        public void Update(MouseState current, MouseState previous)
        {
            Input(current, previous);
        }

        public void Draw()      //debug
        {
            Console.SetCursorPosition(0, 0);

            string output = string.Empty;

            for (int i = 0; i < FIELD_SIZE_HORIZONTAL; i++)
            {
                for (int j = 0; j < FIELD_SIZE_VERTICAL; j++)
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
