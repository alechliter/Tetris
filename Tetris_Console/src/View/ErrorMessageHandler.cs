using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public class ErrorMessageHandler
    {
        public static event Action<ComponentContent[,]> NewErrorMessage;

        public static void DisplayMessage(string message)
        {
            ComponentContent[,] content = new ComponentContent[message.Length, 1];

            for (int i = 0; i < message.Length; i++)
            {
                content[i, 0] = new ComponentContent(message[i], eTextColor.Red);
            }
            NewErrorMessage?.Invoke(content);
        }
    }
}
