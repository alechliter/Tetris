using System;
using System.Threading;

namespace Lechliter.Tetris_Console
{
    class KeyPress3 
    {
        public event Action moveLeft, moveRight, moveDown;

        public event Action rotateLeft, rotateRight;

        public event Action drop;

        public event Action save;

        private Timer timer;

        private string input;

        public KeyPress3(){
            timer = new Timer(new TimerCallback(readInput), null, 0, 5);
            Console.WriteLine(input);
        }

        public void frame(){
            
        }

        private void readInput(object state){
            input = Console.ReadLine();
        }
    }
}