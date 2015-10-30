using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TextToSpeechHelper;
namespace TextToSpeechHelperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test TTS");
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\testTTS.wav";
            string text = "If you see that text,you must be interested in TTS part of English Practice Helper , thank you ";

            try
            {                

                Console.WriteLine("Test TTS Speaker");
                TTSHelper.GetInstance().TTSSpeaker(text);

                Thread.Sleep(10000);

                Console.WriteLine("Test TTS speaker and save to file");
                TTSHelper.GetInstance().TTSToFile(text, filePath);
                Console.WriteLine("Save to " + filePath);
            }catch(Exception ex)
            {
                Console.WriteLine("We got trouble:"+ex.Message);
            }
            Console.ReadLine();
        }
    }
}
