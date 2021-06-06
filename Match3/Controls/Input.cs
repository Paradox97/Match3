using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Match3.Controls
{
    class Input
    {
        private static Input input;

        public MouseState mouseInput;
        public KeyboardState keyboardState;
        private Input()
        {
            this.keyboardState = Keyboard.GetState();
            this.mouseInput = Mouse.GetState();
        }

        public static Input GetInput()
        {
            input = new Input();
            return input;
        }

        public static Input GetOldInput()
        {
            if (input == null)
                input = new Input();

            return input;
        }
    }
}
