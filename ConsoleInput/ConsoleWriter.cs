using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ConsoleInput
{
    public static class ConsoleWriter
    {
        public static void OverwriteSomeLine<T>(T currentNum, int height)
        {
            var text = currentNum is null ? "" : currentNum.ToString();
            SetCursorPosition(0, height);
            Write(new string(' ', BufferWidth));
            SetCursorPosition(0, height);
            Write(text);
        }

        public static void OverwriteCurrentLine<T>(T currentNum)
        {
            OverwriteSomeLine<T>(currentNum, CursorTop);
        }

        public static void OverwritePartOfSomeLine<T>(T currentNum, int leftPos, int rightPos, int height)
        {
            var text = currentNum is null ? "" : currentNum.ToString();
            SetCursorPosition(leftPos, height);
            Write(new string(' ', rightPos - leftPos));
            SetCursorPosition(leftPos, height);
            Write(text);
        }

        public static void OverwritePartOfCurrentLine<T>(T currentNum, int leftPos, int rightPos)
        {
            OverwritePartOfSomeLine<T>(currentNum,leftPos,rightPos, CursorTop);
        }
    }
}
