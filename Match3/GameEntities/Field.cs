using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Linq;

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

        public string[] figurePrefixes;
        string prefix;

        //field boundaries
        public Vector2[] bounds; //topleft, topright, bottomleft, bottomright

        public float 
            figureOffset, figureSize;

        public float
            fieldOffset;
        public int score { get; set; }

        public int[] 
            currentFigure, previousFigure;

        public Figure[,] field;

        public struct Match
        {
            public int i;
            public int j;
            public Match(int hor, int vert)
            {
                i = hor;
                j = vert;
            }
        }

        public string[][]
            figureTexturePaths,
            figureAnimationPaths;

        protected static Paths paths;

        public string[] effectsPaths;

        public Sprite self;

        public Field(float heightOffset, float[] fieldDimensions, ContentManager content, string[] pathPrefixes)
        {
            this.content = content;
            prefix = pathPrefixes[0];
            figurePrefixes = new string[2] { pathPrefixes[1], pathPrefixes[2] };
            figureOffset = 5f;
            fieldOffset = 5f;

            FieldTypes[] fields = ((FieldTypes[])Enum.GetValues(typeof(FieldTypes)));
            path = fields[new Random().Next(0, fields.Length)].ToString();

            bounds = new Vector2[4]
            {
                new Vector2(fieldOffset, heightOffset),                                                        //topleft
                new Vector2(fieldOffset + fieldDimensions[0], heightOffset),                               //topright
                new Vector2(fieldOffset, heightOffset + fieldDimensions[1]),                               //bottomleft
                new Vector2(fieldOffset + fieldDimensions[0], heightOffset + fieldDimensions[1])       //bottomright
            };

            self = new Sprite(prefix + path, bounds[0], content);

            figureSize = 50;

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

            paths = new Paths(figurePrefixes, figureTexturePaths, figureAnimationPaths, effectsPaths);

            this.field = new Figure[FIELD_SIZE_HORIZONTAL, FIELD_SIZE_VERTICAL];
            Create();
        }




        public Field(float figureSize, float offset, Vector2[] bounds, ContentManager content, string[] pathPrefixes)
        {
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

            paths = new Paths(figurePrefixes, figureTexturePaths, figureAnimationPaths, effectsPaths);

            this.field = new Figure[FIELD_SIZE_HORIZONTAL, FIELD_SIZE_VERTICAL];
            Create();
        }

        public int RandomType()
        {
            FigureTypes[] figures = ((FigureTypes[])Enum.GetValues(typeof(FigureTypes)));
            int figureType = (int)figures[new Random().Next(0, figures.Length)];
            return figureType;
        }

        public int NotType(int type)
        {
            int figureType = type;

            while (figureType == type)
            {
                FigureTypes[] figures = ((FigureTypes[])Enum.GetValues(typeof(FigureTypes)));
                figureType = (int)figures[new Random().Next(0, figures.Length)];
            }

            return figureType;
        }

        public void Create()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Figure figure = new Figure(i, j, RandomType(), figureOffset , bounds[0], paths, content); 
                    this.field[j, i] = figure;
                }
            }

           GenerateByDifficulty(1);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Shuffle(i, j);
                }
            }



        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(self.texture, bounds[0], Color.White);

            for (int i = 0; i < FIELD_SIZE_HORIZONTAL; i++)
            {
                for (int j = 0; j < FIELD_SIZE_VERTICAL; j++)
                {
                    field[i, j].Draw(spriteBatch);
                }
            }
            Draw();
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
                            return new int[2] { i, j };
                        }
                    }
                }
                return new int[2] { -1, -1 };
            }
            //out of bounds
            else
            {
                return new int[2] { -1, -1 };
            }
        }

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
                    Swap(currentFigure[0], currentFigure[1], previousFigure[0], previousFigure[1]);
                    previousFigure = null;
                    currentFigure = null;
                }
            }
        }

        public void GenerateByDifficulty(int difficultySeed)
        {
            Random random = new Random();

            int steps, seed;
            int i, j;

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
                        field[i + 1, j] = new Figure(field[i + 1, j], type, paths, content);
                        field[i + 2, j] = new Figure(field[i + 2, j], type, paths, content);
                        return;
                    }

                    if ((i - 1 > 0) && (i + 1 < FIELD_SIZE_HORIZONTAL))
                    {
                        field[i + 1, j] = new Figure(field[i + 1, j], type, paths, content);
                        field[i - 1, j] = new Figure(field[i - 1, j], type, paths, content);
                        return;
                    }

                    break;

                case 1:
                    if (i - 2 > 0)
                    {
                        field[i - 1, j] = new Figure(field[i - 1, j], type, paths, content);
                        field[i - 2, j] = new Figure(field[i - 2, j], type, paths, content);
                        return;
                    }

                    break;

                case 2:
                    if (j + 2 < FIELD_SIZE_HORIZONTAL)
                    {
                        field[i, j + 1] = new Figure(field[i, j + 1], type, paths, content);
                        field[i, j + 2] = new Figure(field[i, j + 2], type, paths, content);
                        return;
                    }

                    if ((j - 1 > 0) && (j + 1 < FIELD_SIZE_HORIZONTAL))
                    {
                        field[i, j + 1] = new Figure(field[i, j + 1], type, paths, content);
                        field[i, j - 1] = new Figure(field[i, j - 1], type, paths, content);
                        return;
                    }

                    break;

                case 3:
                    if (j - 2 > 0)
                    {
                        field[i, j - 1] = new Figure(field[i, j - 1], type, paths, content);
                        field[i, j - 2] = new Figure(field[i, j - 2], type, paths, content);
                        return;
                    }
                    break;
            }
        }
        
        public void Swap(int i1, int j1, int i2, int j2)
        {
            if ((i1 == -1) || (i2 == -1))
                return;

            bool isMatch = false;
            bool isWaiting = false;

            field[i2, j2].Swap(field[i1, j1], i2, j2, i1, j1);
            field[i1, j1].Swap(field[i2, j2], i1, j1, i2, j2);

            field[i2, j2].Update();
            field[i1, j1].Update();

            if (IsMatch(i1, j1) || IsMatch(i2, j2))
            {
                for (int i = 0; i < FIELD_SIZE_HORIZONTAL; i++)
                {
                    for (int j = 0; j < FIELD_SIZE_VERTICAL; j++)
                    {
                        if ((i == i1) && (j == j1))
                        {
                            //Console.WriteLine("hui");
                        }
                        else
                        {
                            if ((i == i2) && (j == j2))
                            {
                                //Console.WriteLine("hui");
                            }
                            else
                                field[i, j].AwaitSwap();
                        }
                    }
                }


                List<Match> matches = WhereMatch(i1, j1);

                foreach (var m in WhereMatch(i2, j2))
                    matches.Add(m);

                foreach(var m in matches)
                {
                    field[m.i, m.j].Blast();
                    field[m.i, m.j] = new Figure(field[m.i, m.j], RandomType(), field[m.i, m.j].animationStates, paths, content);
                }

                Finalize(matches);

                return;
            }

            field[i2, j2].Swap(field[i1, j1], i2, j2, i1, j1);
            field[i1, j1].Swap(field[i2, j2], i1, j1, i2, j2);
        }

        public void Finalize(List<Match> matches)
        {
            List<Match> matches1 = new List<Match>();
            bool AreMatches = false;
            
            foreach (var m in matches)
                 {
                    if (IsMatch(m.i, m.j))
                        {
                            AreMatches = true;
                            foreach(var n in WhereMatch(m.i, m.j))
                                {
                                    matches1.Add(n);
                                }
                        } 
                 }

            for (int i = 0; i < FIELD_SIZE_HORIZONTAL; i++)
            {
                for (int j = 0; j < FIELD_SIZE_VERTICAL; j++)
                {
                    if (matches1.Contains(new Match(i, j)))
                    {
                        field[i, j].Blast();
                        field[i, j] = new Figure(field[i, j], RandomType(), field[i, j].animationStates, paths, content);
                    }
                    else
                        field[i, j].AwaitFalldown();
                }
            }

            if (AreMatches == false)
                return;

            Finalize(matches1);

        }

        public bool IsMatch()
        {
            return false;
        }

        public bool IsMatch(int i, int j)
        {
            int type = field[i, j].figureType;

            if (i + 2 < FIELD_SIZE_HORIZONTAL)
                    {
                        if ((field[i + 1, j].figureType == type) && (field[i + 2, j].figureType == type))
                            return true;
                    }

            if ((i > 0) && (i + 1 < FIELD_SIZE_HORIZONTAL))
                    {
                       if ((field[i + 1, j].figureType == type) && (field[i - 1, j].figureType == type))
                            return true;
                    }

            if (i - 2 >= 0)
                    {
                        if ((field[i - 2, j].figureType == type) && (field[i - 1, j].figureType == type))
                            return true;
                    }

            if (j + 2 < FIELD_SIZE_HORIZONTAL)
                    {
                        if ((field[i, j + 1].figureType == type) && (field[i, j + 2].figureType == type))
                            return true;
                    }

             if ((j > 0) && (j + 1 < FIELD_SIZE_HORIZONTAL))
                    {
                        if ((field[i, j + 1].figureType == type) && (field[i, j - 1].figureType == type))
                            return true;
                    }

              if (j - 2 >= 0)
                    {
                        if ((field[i, j - 1].figureType == type) && (field[i, j - 2].figureType == type))
                            return true;
                    }
            return false;
        }


        public void Shuffle(int i, int j)
        {
            if (!IsMatch(i, j))
                return;

            //List<Match> matches = new List<Match>();

            int type = field[i, j].figureType;
            int notmatch = NotType(type);
            List<Match> matches = WhereMatch(i, j);
            Random rand = new Random();
            int random = 0;
            int k, g;


            while (IsMatch(i, j))
            {
                notmatch = NotType(type);
                random = rand.Next(0, matches.Count);

                k = matches[random].i;
                g = matches[random].j;

                field[k, g] = new Figure(field[k,g], notmatch, paths, content);


                while (IsMatch(k, g))
                        {
                            notmatch = NotType(type);
                            field[k, g] = new Figure(field[k, g], notmatch, paths, content);
                        }
            }
        }

        public List<Match> WhereMatch(int i, int j)
        {
            int type = this.field[i, j].figureType;

            List<Match> match = new List<Match>();
            List<Match> horizontal = new List<Match>();
            List<Match> vertical = new List<Match>();
            List<Match> horDistinct, vertDistinct;

            if (i < FIELD_SIZE_HORIZONTAL)
            {
                for (int k = i; k < FIELD_SIZE_HORIZONTAL; k++)
                {
                    if (field[k, j].figureType == type)
                        horizontal.Add(new Match(k, j));
                    else break;
                }
            }

            if (i > 0)
            {
                for (int k = i; k >= 0; k--)
                {
                    if (field[k, j].figureType == type)
                        horizontal.Add(new Match(k, j));
                    else break;
                }
            }


            if (j < FIELD_SIZE_VERTICAL)
            {
                for (int k = j; k < FIELD_SIZE_HORIZONTAL; k++)
                {
                    if (field[i, k].figureType == type)
                        vertical.Add(new Match(i, k));
                    else break;
                }
            }

            if (j > 0)
            {
                for (int k = j; k >= 0; k--)
                {
                    if (field[i, k].figureType == type)
                        vertical.Add(new Match(i, k));
                    else break;
                }
            }
            
            //counting horizontal Matches
            horDistinct = horizontal.Distinct().ToList();

            //Console.WriteLine("__________________________");

            int count = horDistinct.Count;
            //Console.WriteLine(count);

            if (count >= 3)
            {
                switch (count)
                {
                    case MATCH_CONDITION:
                        break;
                    case LINE_BONUS:
                        break;
                    case BOMB_BONUS:
                        break;
                }
            }
            else
            {
                horDistinct.Clear();
                //Console.WriteLine("correct?");
                //Console.WriteLine(horDistinct.Count);
                //Console.WriteLine("___________");
            }
            
            //counting vertical Matches
            vertDistinct = vertical.Distinct().ToList();
            count = vertDistinct.Count;

            if (count >= 3)
            {
                switch (count)
                {
                    case MATCH_CONDITION:
                        break;
                    case LINE_BONUS:
                        break;
                    case BOMB_BONUS:
                        break;
                }
            }
            else
                vertDistinct.Clear();

            foreach (var m in horDistinct)
            {
                match.Add(m);
            }

            foreach (var m in vertDistinct)
            {
                match.Add(m);
            }

            return match.Distinct().ToList();
        }


        public void UpdateFigures()
        {

            for (int i = 0; i < FIELD_SIZE_HORIZONTAL; i++)
            {
                for (int j = 0; j < FIELD_SIZE_VERTICAL; j++)
                {
                        field[i, j].Update();
                }
            }

            for (int i = 0; i < FIELD_SIZE_HORIZONTAL; i++)
            {
                for (int j = 0; j < FIELD_SIZE_VERTICAL; j++)
                {
                    field[i, j].PostUpdate();
                }
            }
        }


        public void Update(MouseState current, MouseState previous)
        {
            bool isBusy = false;

            for (int i = 0; i < FIELD_SIZE_HORIZONTAL; i++)
            {
                for (int j = 0; j < FIELD_SIZE_VERTICAL; j++)
                {
                    if (field[i, j].IsBusy())
                    {
                        isBusy = true;
                        break;
                    }
                }
                if (isBusy == true)
                    break;
            }

            if (isBusy == false)
                Input(current, previous);

            UpdateFigures();
        }

        public void Draw()      //debug
        {
            Console.SetCursorPosition(0, 0);

            string output = string.Empty;

            for (int i = 0; i < FIELD_SIZE_HORIZONTAL; i++)
            {
                for (int j = 0; j < FIELD_SIZE_VERTICAL; j++)
                {
                    output += this.field[i, j].figureType;
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
