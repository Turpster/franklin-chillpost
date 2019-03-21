using System;
using System.Collections.Generic;
using System.Data;

namespace Discord_Bot.LoggerNS
{
    public class LoggerFormat
    {
        private const char FormatChar = '§';
        
        private static readonly Dictionary<char, LoggerFormat> TextFormats = new Dictionary<char, LoggerFormat>();
        
        public static LoggerFormat Black = new LoggerFormat('0', () => { Console.Write("\u001b[30m");});
        public static LoggerFormat Blue = new LoggerFormat('1', () => { Console.Write("\u001b[34m");});
        public static LoggerFormat Green = new LoggerFormat('2', () => { Console.Write("\u001b[32m");});
        public static LoggerFormat Cyan = new LoggerFormat('3', () => { Console.Write("\u001b[36m");});
        public static LoggerFormat Red = new LoggerFormat('4', () => { Console.Write("\u001b[31m");});
        public static LoggerFormat Magenta = new LoggerFormat('5', () => { Console.Write("\u001b[35m");});
        public static LoggerFormat Yellow = new LoggerFormat('6', () => { Console.Write("\u001b[33m");});
        public static LoggerFormat LightGrey = new LoggerFormat('7', () => { Console.Write("\u001b[37m");});
        public static LoggerFormat Grey = new LoggerFormat('8', () => { Console.Write("\u001b[90m");});
        public static LoggerFormat LightBlue = new LoggerFormat('9', () => { Console.Write("\u001b[94m");});
        public static LoggerFormat LightGreen = new LoggerFormat('a', () => { Console.Write("\u001b[92m");});
        public static LoggerFormat LightCyan = new LoggerFormat('b', () => { Console.Write("\u001b[96m");});
        public static LoggerFormat LightRed = new LoggerFormat('c', () => { Console.Write("\u001b[91m");});
        public static LoggerFormat LightMagenta = new LoggerFormat('d', () => { Console.Write("\u001b[95m");});
        public static LoggerFormat LightYellow = new LoggerFormat('e', () => { Console.Write("\u001b[93m");});
        public static LoggerFormat White = new LoggerFormat('f', () => { Console.Write("\u001b[97m");});    
        public static LoggerFormat Bold = new LoggerFormat('l', () => { Console.Write("\u001b[1m");});
        public static LoggerFormat Italics = new LoggerFormat('o', () => { Console.Write("\u001b[3m");});
        public static LoggerFormat Underline = new LoggerFormat('m', () => { Console.Write("\u001b[4m");});
        public static LoggerFormat Strikethrough = new LoggerFormat('n', () => { Console.Write("\u001b[9m");});
        public static LoggerFormat Faint = new LoggerFormat('v', () => { Console.Write("\u001b[2m");});
        public static LoggerFormat SlowBlink = new LoggerFormat('t', () => { Console.Write("\u001b[5m");});
        public static LoggerFormat RapidBlink = new LoggerFormat('p', () => { Console.Write("\u001b[6m");});
        public static LoggerFormat InvertColor = new LoggerFormat('i', () => { Console.Write("\u001b[7m");});
//            public static LoggerFormat Conceal = new LoggerFormat('x', () => { Console.Write("\u001b[8m");});        
        public static LoggerFormat Reset = new LoggerFormat('r', () => { Console.Write("\u001b[0m");});


        private readonly char _char;
        private readonly Action _display;

        private LoggerFormat(char targetChar, Action display)
        {
            targetChar = char.ToLower(targetChar);
            if (TextFormats.ContainsKey(targetChar))
            {
                throw new DataException("Character has already been assigned.");
            }
            TextFormats.Add(targetChar, this);

            _char = targetChar;
            _display = display;
        }

        public override string ToString()
        {
            return FormatChar + _char.ToString();
        }

        public static void Write(string message)
        {
            bool previousColor = false;
            for (int i = 0; i < message.Length - 1; i++)
            {
                if (char.ToLower(message[i]) == FormatChar)
                {
                    if (message.Length - 2 == i)
                    {
                        previousColor = true;
                    }
                    char targetChar = message[i + 1];
                    if (TextFormats.ContainsKey(targetChar))
                    {
                        TextFormats[targetChar].Display();
                        i++;
                    }

                }
                else Console.Write(message[i]);

            }
            if (!previousColor)
            {
                Console.Write(message[message.Length - 1]);
            }
        }
        
        public void Display() => _display();
        
    }
}