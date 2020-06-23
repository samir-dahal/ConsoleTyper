using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ConsoleTyperUI
{
    public class Game
    {
        private string _sentenceToType { get; set; }
        private int _currentIndex { get; set; }
        private int _totalErrors { get; set; }
        private int _totalKeyStrokes { get; set; }
        private const ConsoleColor _currentColor = ConsoleColor.White;
        private const ConsoleColor _remainingColor = ConsoleColor.DarkGreen;
        private const ConsoleColor _completedColor = ConsoleColor.DarkGreen;
        private const ConsoleColor _congratsColor = ConsoleColor.Cyan;
        private const ConsoleColor _replayColor = ConsoleColor.Yellow;
        private const ConsoleColor _goodByeColor = ConsoleColor.Green;
        private Stopwatch sw = new Stopwatch();
        public Game()
        {
            _sentenceToType = new SentenceGenerator().Generate();
        }
        public void EnterGame()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Press \"Enter\" to begin typing");
                while (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    Console.Clear();
                    Console.WriteLine("Press \"Enter\" to begin typing");
                }
                Console.Clear();

                //begin stop watch
                sw.Start();

                BeginTyping();

                //end stop watch
                sw.Stop();

                //calculate result
                CalculateResult();

                //replay input
                Console.ForegroundColor = _replayColor;
                Console.WriteLine();
                Console.WriteLine("Press 'R' to replay");
            } while (char.ToLower(Console.ReadKey().KeyChar) == 'r');

            //revert back to default console foreground color
            Console.ForegroundColor = _goodByeColor;
            Console.WriteLine("Thank you for enjoying ConsoleTyper :)");
        }

        private void BeginTyping()
        {
            while (_currentIndex < _sentenceToType.Length)
            {
                //completed color
                Console.ForegroundColor = _completedColor;
                Console.Write(_sentenceToType.Substring(0, _currentIndex));

                //current color
                Console.ForegroundColor = _currentColor;
                Console.Write((_sentenceToType[_currentIndex] == ' ') ? '_' : _sentenceToType[_currentIndex]);

                //remaining color
                Console.ForegroundColor = _remainingColor;
                Console.Write(_sentenceToType.Substring(_currentIndex + 1, _sentenceToType.Length - (_currentIndex + 1)));
                Console.WriteLine();

                //accept input
                var input = Console.ReadKey().KeyChar;

                //increment keystrokes 
                _totalKeyStrokes++;

                //increment index if input matches with current char of sentence
                if (input == _sentenceToType[_currentIndex])
                {
                    _currentIndex++;
                    Console.Clear();
                }
                else
                {
                    //increment error
                    _totalErrors++;
                    Console.Beep();
                    Console.Clear();
                }
            }
        }
        private void CalculateResult()
        {
            //result calulations
            var totalWords = _sentenceToType.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
            var totalMinutes = sw.Elapsed.TotalMinutes;
            var typingAccuracy = (_sentenceToType.Length - _totalErrors) / (double)_sentenceToType.Length * 100;
            var wpm = (int)Math.Floor(totalWords / totalMinutes);

            //congrats mesg
            Console.ForegroundColor = _congratsColor;
            Console.WriteLine("CONGRATULATIONS!");
            Console.WriteLine(new String('#', 15));
            Console.WriteLine();
            Console.WriteLine("You completed the typing test.");
            Console.WriteLine($"Your max typing speed is {wpm} WPM (Words Per Minute)");
            Console.WriteLine($"Your typing accuracy is {typingAccuracy.ToString("#.##")}%");
            Console.WriteLine($"Your total keystrokes is {_totalKeyStrokes} keys");
            Console.WriteLine($"Your total typos are {_totalErrors}");

            //reset values
            _totalErrors = 0;
            _totalKeyStrokes = 0;
            _currentIndex = 0;
        }
    }
}