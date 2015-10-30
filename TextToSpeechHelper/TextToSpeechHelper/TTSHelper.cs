using SpeechLib;
using System.IO;

namespace TextToSpeechHelper
{
    public class TTSHelper
    {
        //param
        private SpFileStream spFileStream;    //declaring and Initializing fileStream obj

        private SpeechStreamFileMode spFileMode;  //declaring fileStreamMode as to Create or Write

        private int sp_Rate, sp_Volume;
        private SpeechVoiceSpeakFlags my_Spflag; // declaring and initializing Speech Voice Flags

        // SpeechVoiceSpeakFlags my_Spflag = SpeechVoiceSpeakFlags.SVSFlagsAsync; // declaring and initializing Speech Voice Flags
        private SpVoice my_Voice;                   //declaring and initializing SpVoice Class

        private static TTSHelper Instance = null;

        public TTSHelper()
        {
            //param
            spFileStream = new SpFileStream();     //declaring and Initializing fileStream obj
            spFileMode = SpeechStreamFileMode.SSFMCreateForWrite;  //declaring fileStreamMode as to Create or Write

            // SpeechVoiceSpeakFlags my_Spflag = SpeechVoiceSpeakFlags.SVSFlagsAsync; // declaring and initializing Speech Voice Flags
            my_Voice = new SpVoice();                   //declaring and initializing SpVoice Class
            sp_Rate = 0; sp_Volume = 100;
            my_Spflag = SpeechVoiceSpeakFlags.SVSFlagsAsync;
        }

        public TTSHelper(int volume, int rate)
        {
            //param
            spFileStream = new SpFileStream();     //declaring and Initializing fileStream obj
            spFileMode = SpeechStreamFileMode.SSFMCreateForWrite;  //declaring fileStreamMode as to Create or Write

            // SpeechVoiceSpeakFlags my_Spflag = SpeechVoiceSpeakFlags.SVSFlagsAsync; // declaring and initializing Speech Voice Flags
            my_Voice = new SpVoice();                   //declaring and initializing SpVoice Class
            sp_Rate = rate; sp_Volume = volume;
            my_Spflag = SpeechVoiceSpeakFlags.SVSFlagsAsync;
        }

        public static TTSHelper GetInstance()
        {
            if (Instance == null)
            {
                Instance = new TTSHelper();
            }
            return Instance;
        }

        public void TTSToFile(string text, string filepath)
        {
            if (!File.Exists(filepath))
            {
                File.Create(filepath);
            }
            spFileStream.Open(filepath, spFileMode);
            my_Voice.AudioOutputStream = spFileStream;
            my_Voice.Speak(text, my_Spflag);
            my_Voice.WaitUntilDone(-1);
            spFileStream.Close();
        }

        public void TTSSpeaker(string text)
        {
            my_Voice.Speak(text, my_Spflag);
        }
    }
}