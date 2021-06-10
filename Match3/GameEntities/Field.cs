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

        public struct Bonus
        {
            public int i;
            public int j;
            public int subType;

            public Bonus(int hor, int vert, int type)
            {
                i = hor;
                j = vert;
                subType = type;
            }
        }

        public List<Match> matches;

        public string[][]
            figureTexturePaths,
            figureAnimationPaths;

        protected static Paths paths;

        public string[] effectsPaths;

        //public Texture2D self;          //field texture

        public Sprite self;

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
                    Figure figure = new Figure(i,j, RandomType(), figureOffset ,bounds[0], paths, content); 
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

                    
                    /*if ((currentFigure[0] == previousFigure[0])&&(currentFigure[1] == previousFigure[1]))
                    {
                        //Console.WriteLine("No Swap");
                    }*/
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

            field[i2, j2].Swap(field[i1, j1], i2, j2, i1, j1);
            field[i1, j1].Swap(field[i2, j2], i1, j1, i2, j2);

            UpdateFigures();

            if (IsMatch(i1, j1))
            {
                isMatch = true;
            }

            if (IsMatch(i2, j2))
            {
                isMatch = true;
            }

            if (isMatch == true)
                return;

            field[i2, j2].Swap(field[i1, j1], i2, j2, i1, j1);
            field[i1, j1].Swap(field[i2, j2], i1, j1, i2, j2);
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
                        if ((field[i + 1, j].figureType == type)&&(field[i + 2, j].figureType == type))
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
            //Console.WriteLine("false");
            return false;
        }


        public void Shuffle(int i, int j)
        {
            if (!IsMatch(i, j))
                return;

            //List<Match> matches = new List<Match>();

            int type = field[i, j].figureType;
            int notmatch = NotType(type);
            matches = WhereMatch(i, j);
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
                            field[k,g] = new Figure(field[k,g], notmatch, paths, content);
                        }
            }
        }

        public List<Match> WhereMatch(int i, int j) //List<Match> WhereMatch(int i, int j)
        {
            int type = this.field[i, j].figureType;

            List<Match> match = new List<Match>();
            List<Match> horizontal = new List<Match>();
            List<Match> vertical = new List<Match>();
            List<Match> temp = new List<Match>();

            horizontal.Add(new Match(i,j));
            vertical.Add(new Match(i, j));

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
                for (int k = i; k > 0; k--)
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
                        horizontal.Add(new Match(i, k));
                    else break;
                }
            }

            if (j > 0)
            {
                for (int k = j; k > 0; k--)
                {
                    if (field[i, k].figureType == type)
                        horizontal.Add(new Match(i, k));
                    else break;
                }
            }
            
            //counting horizontal Matches
            horizontal.Distinct().ToList();
            int count = horizontal.Count;

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
                horizontal.Clear();
            
            //counting vertical Matches
            vertical.Distinct().ToList();
            count = vertical.Count;

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
                vertical.Clear();

            foreach (var m in horizontal)
            {
                match.Add(m);
            }

            foreach (var m in vertical)
            {
                match.Add(m);
            }

            match.Distinct().ToList();
            return match;
            /*
            int value = this._field[i, j];

            int[] bonus_values = new int[3];

            bonus_values[0] = GetHorLineValue(value);
            bonus_values[1] = GetVertLineValue(value);
            bonus_values[2] = GetBombValue(value);


            List<Match> Match = new List<Match>();

            List<Match> Temp = new List<Match>();

            Match Bonus = new Match();

            List<Match> MatchTypeHor = new List<Match>();
            List<Match> MatchTypeVert = new List<Match>();

            int BombIndex = 666;
            int horizontalIndex = 444;
            int verticalIndex = 555;


            if (i < FIELD_SIZE)
            {
                for (int k = i; k < FIELD_SIZE; k++)
                {
                    if ((this._field[k, j] == value) || (this._field[k, j] == bonus_values[0]) || (this._field[k, j] == bonus_values[1]) || (this._field[k, j] == bonus_values[2]))
                        Temp.Add(new Match(k, j));
                    else break;
                }

                foreach (Match m in Temp)
                {
                    Match.Add(m);
                    MatchTypeHor.Add(m);
                }

                Temp = new List<Match>();
            }

            if (j < FIELD_SIZE)
            {
                for (int k = j; k < FIELD_SIZE; k++)
                {
                    if ((this._field[i, k] == value) || (this._field[i, k] == bonus_values[0]) || (this._field[i, k] == bonus_values[1]) || (this._field[i, k] == bonus_values[2]))
                        Temp.Add(new Match(i, k));
                    else break;
                }

                foreach (Match m in Temp)
                {
                    Match.Add(m);
                    MatchTypeVert.Add(m);
                }

                Temp = new List<Match>();
            }

            if (i > 0)
            {
                for (int k = i; k > 0; k--)
                {
                    if ((this._field[k, j] == value) || (this._field[k, j] == bonus_values[0]) || (this._field[k, j] == bonus_values[1]) || (this._field[k, j] == bonus_values[2]))
                        Temp.Add(new Match(k, j));
                    else break;
                }

                foreach (Match m in Temp)
                {
                    Match.Add(m);
                    MatchTypeHor.Add(m);
                }

                Temp = new List<Match>();
            }

            if (j > 0)
            {
                for (int k = j; k > 0; k--)
                {
                    if ((this._field[i, k] == value) || (this._field[i, k] == bonus_values[0]) || (this._field[i, k] == bonus_values[1]) || (this._field[i, k] == bonus_values[2]))
                        Temp.Add(new Match(i, k));
                    else break;
                }

                foreach (Match m in Temp)
                {
                    Match.Add(m);
                    MatchTypeVert.Add(m);
                }

                Temp = new List<Match>();
            }

            List<Match> NoDuplicatesHor = MatchTypeHor.Distinct().ToList();
            List<Match> NoDuplicatesVert = MatchTypeVert.Distinct().ToList();
            List<Match> NoDuplicates = Match.Distinct().ToList();

            if (NoDuplicatesHor.Count >= BOMB_BONUS)
            {                                               //addding info about bonuses
                Bonus = new Match(BombIndex, BombIndex);
                NoDuplicates.Add(Bonus);
                return NoDuplicates;
            }

            if (NoDuplicatesVert.Count >= BOMB_BONUS)
            {
                Bonus = new Match(BombIndex, BombIndex);
                NoDuplicates.Add(Bonus);
                return NoDuplicates;
            }

            if ((NoDuplicatesVert.Count >= LINE_BONUS) || (NoDuplicatesHor.Count >= LINE_BONUS))
            {
                if (NoDuplicatesVert.Count > NoDuplicatesHor.Count)
                {
                    Bonus = new Match(verticalIndex, verticalIndex);
                }
                else
                    Bonus = new Match(horizontalIndex, horizontalIndex);

                NoDuplicates.Add(Bonus);
                return NoDuplicates;
            }

            return NoDuplicates;
            */
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

        public int[] ConsoleDebug(int i)
        {
            int hor = i / 8;
            int vert = i % 8;
            return new int[2] { hor, vert };
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
