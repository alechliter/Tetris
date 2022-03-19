using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lechliter.Tetris_Console;

using System.IO;

namespace TetrisConsoleTest
{
    [TestClass]
    public class KeyPressTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            KeyPress3 keypress = new KeyPress3();

            StringReader stringReader = new StringReader("Hello\nWorld");
            System.Console.SetIn(stringReader);

            //string input = System.Console.ReadLine();
            //System.Console.WriteLine(input);

            keypress.Frame(keypress.ReadInput, keypress.ParseInput);
            //Assert.AreEqual("Hello World", input);
        }
    }
}
