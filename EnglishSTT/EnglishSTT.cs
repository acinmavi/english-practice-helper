using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Istrib.Sound;
using Istrib.Sound.Formats;
using System.IO;
using System.Net;
using CUETools.Codecs.FLAKE;
using CUETools.Codecs;
using System.Text.RegularExpressions;

namespace EnglishSTT
{
    public class Constants
    {
        public static string GoogleRequestString =
            "https://www.google.com/speech-api/v1/recognize?xjerr=1&client=chromium&lang=en-EN";

    }

    public class SoundIO : IDisposable
    {
        private Mp3SoundCapture _mp3SoundCapture;


        public SoundCaptureDevice CaptureDevice
        {
            get { return _mp3SoundCapture.CaptureDevice; }
            set
            {
                _mp3SoundCapture.CaptureDevice = value;
            }
        }

        public Mp3SoundCapture.Outputs OutputType
        {
            get { return _mp3SoundCapture.OutputType; }
            set { _mp3SoundCapture.OutputType = value; }
        }

        public PcmSoundFormat SoundFormat
        {
            get { return _mp3SoundCapture.WaveFormat; }
            set { _mp3SoundCapture.WaveFormat = value; }
        }

        /// <summary>
        /// If true record stream will be closed after all the date is written.
        /// If false record stream will be closed immediately. The data loss is possible.
        /// </summary>
        public bool WaitOnStop
        {
            get { return _mp3SoundCapture.WaitOnStop; }
            set { _mp3SoundCapture.WaitOnStop = value; }
        }

        /// <summary>
        /// If true volume level would be normilized
        /// </summary>
        public bool NormalizeVolume
        {
            get { return _mp3SoundCapture.NormalizeVolume; }
            set { _mp3SoundCapture.NormalizeVolume = value; }
        }

        /// <summary>
        /// Use this to subscribe on events
        /// </summary>
        public Mp3SoundCapture SoundCaptureEngine
        {
            get { return _mp3SoundCapture; }
        }

        public Stream OutputStream { get; set; }

        public String OutputFilePath { get; set; }

        private static IEnumerable<SoundCaptureDevice> AvailableDevices
        { get { return SoundCaptureDevice.AllAvailable; } }

        private static IEnumerable<PcmSoundFormat> StandartSoundFormats
        { get { return PcmSoundFormat.StandardFormats.Where(d => d.BitsPerSample >= 16); } }

        public static IEnumerable<String> AvailableDevicesDescriptions
        { get { return SoundCaptureDevice.AllAvailable.Select(d => d.Description); } }

        public static IEnumerable<String> StandartSoundFormatsDescriptions
        { get { return PcmSoundFormat.StandardFormats.Where(d => d.BitsPerSample >= 16).Select(d => d.Description); } }

        public SoundIO(SoundCaptureDevice captureDevice, Mp3SoundCapture.Outputs outputType, PcmSoundFormat soundFormat)
        {
            _mp3SoundCapture = new Mp3SoundCapture
            {
                CaptureDevice = captureDevice,
                OutputType = outputType,
                WaveFormat = soundFormat,
                WaitOnStop = true,
                NormalizeVolume = true
            };
        }

        public static SoundIO getSoundIO(String captureDeviceDesc, String outputTypeDesc, String soundFormatDesc)
        {
            Mp3SoundCapture.Outputs outputType;
            switch (outputTypeDesc.ToLower())
            {
                case "wav": outputType = Mp3SoundCapture.Outputs.Wav; break;
                case "mp3": outputType = Mp3SoundCapture.Outputs.Mp3; break;
                case "rawpcm": outputType = Mp3SoundCapture.Outputs.RawPcm; break;
                default:
                    outputType = Mp3SoundCapture.Outputs.Wav;
                    break;
            }
            SoundCaptureDevice captureDevice = SoundCaptureDevice.AllAvailable.Where(d => d.Description == captureDeviceDesc).FirstOrDefault();
            PcmSoundFormat soundFormat = StandartSoundFormats.Where(d => d.Description == soundFormatDesc).FirstOrDefault();

            return new SoundIO(captureDevice, outputType, soundFormat);
            //_mp3SoundCapture = new Mp3SoundCapture
            //{
            //    CaptureDevice = captureDevice,
            //    OutputType = outputType,
            //    WaveFormat = soundFormat,
            //    WaitOnStop = true,
            //    NormalizeVolume = true
            //};
        }

        public void StartRecording()
        {
            if (OutputStream != null)
            {
                StartRecording(OutputStream);
                return;
            }

            if (!string.IsNullOrEmpty(OutputFilePath))
            {
                StartRecording(OutputFilePath);
                return;
            }

            throw new ArgumentException("Both OutputStream and OutputFilePath were not set.");
        }

        public void StartRecording(Stream stream)
        {
            _mp3SoundCapture.Start(stream);
        }

        public void StartRecording(string path)
        {
            _mp3SoundCapture.Start(path);
        }


        public void StopRecording()
        {
            _mp3SoundCapture.Stop();
        }

        public void Dispose()
        {
            _mp3SoundCapture.Dispose();
            if (OutputStream != null)
                OutputStream.Close();
        }
    }

    public class SoundRecognition
    {
        /// <summary>
        /// Send request to google speech recognition service and return recognized string
        /// </summary>
        /// <param name="flacName">path to .flac file</param>
        /// <param name="sampleRate">Rate</param>
        /// <returns>recognized string</returns>
        public static string GoogleRequest(string flacName, int sampleRate)
        {
            var bytes = File.ReadAllBytes(flacName);
            return GoogleRequest(bytes, sampleRate);
        }

        /// <summary>
        /// Send request to google speech recognition service and return recognized string
        /// </summary>
        /// <param name="bytes">byte array wich is a sound in .flac</param>
        /// <param name="sampleRate">Rate</param>
        /// <returns>recognized string</returns>
        public static string GoogleRequest(byte[] bytes, int sampleRate)
        {
            Stream stream = null;
            StreamReader sr = null;
            WebResponse response = null;
            JSon.RecognizedItem result;
            try
            {
                WebRequest request = WebRequest.Create(Constants.GoogleRequestString);
                request.Method = "POST";
                request.ContentType = "audio/x-flac; rate=" + sampleRate;
                request.ContentLength = bytes.Length;

                stream = request.GetRequestStream();

                stream.Write(bytes, 0, bytes.Length);
                stream.Close();

                response = request.GetResponse();

                stream = response.GetResponseStream();
                if (stream == null)
                {
                    throw new Exception("Can't get a response from server. Response stream is null.");
                }
                sr = new StreamReader(stream);

                //Get response in JSON format
                string respFromServer = sr.ReadToEnd();

                var parsedResult = JSon.Parse(respFromServer);
                result =
                    parsedResult.hypotheses.Where(d => d.confidence == parsedResult.hypotheses.Max(p => p.confidence)).FirstOrDefault();
            }
            finally
            {
                if (stream != null)
                    stream.Close();

                if (sr != null)
                    sr.Close();

                if (response != null)
                    response.Close();
            }

            return result == null ? "" : result.utterance;
        }

        /// <summary>
        /// Send request to google speech recognition service and return recognized string
        /// </summary>
        /// <param name="flacStream">stream of sound in flac format</param>
        /// <param name="sampleRate">Rate</param>
        /// <returns>recognized string</returns>
        public static string GoogleRequest(MemoryStream flacStream, int sampleRate)
        {
            flacStream.Position = 0;
            var bytes = new byte[flacStream.Length];
            flacStream.Read(bytes, 0, (int)flacStream.Length);
            return GoogleRequest(bytes, sampleRate);
        }

        /// <summary>
        /// Convert .wav file to .flac file with the same name
        /// </summary>
        /// <param name="WavName">path to .wav file</param>
        /// <returns>Sample Rate of converted .flac</returns>
        public static int Wav2Flac(string WavName)
        {
            int sampleRate;
            var flacName = Path.ChangeExtension(WavName, "flac");

            FlakeWriter audioDest = null;
            IAudioSource audioSource = null;
            try
            {
                audioSource = new WAVReader(WavName, null);

                AudioBuffer buff = new AudioBuffer(audioSource, 0x10000);

                audioDest = new FlakeWriter(flacName, audioSource.PCM);

                sampleRate = audioSource.PCM.SampleRate;

                while (audioSource.Read(buff, -1) != 0)
                {
                    audioDest.Write(buff);
                }
            }
            finally
            {
                if (audioDest != null) audioDest.Close();
                if (audioSource != null) audioSource.Close();
            }
            return sampleRate;
        }

        /// <summary>
        /// Convert stream of wav to flac format and send it to google speech recognition service.
        /// </summary>
        /// <param name="stream">wav stream</param>
        /// <returns>recognized result</returns>
        public static string WavStreamToGoogle(Stream stream)
        {
            FlakeWriter audioDest = null;
            IAudioSource audioSource = null;
            string answer;
            try
            {
                var outStream = new MemoryStream();

                stream.Position = 0;

                audioSource = new WAVReader("", stream);

                var buff = new AudioBuffer(audioSource, 0x10000);

                audioDest = new FlakeWriter("", outStream, audioSource.PCM);

                var sampleRate = audioSource.PCM.SampleRate;

                while (audioSource.Read(buff, -1) != 0)
                {
                    audioDest.Write(buff);
                }

                answer = GoogleRequest(outStream, sampleRate);

            }
            finally
            {
                if (audioDest != null) audioDest.Close();
                if (audioSource != null) audioSource.Close();
            }
            return answer;
        }
    }

    public class JSon
    {
        public class RecognizedItem
        {
            public string utterance;
            public float confidence;
        }

        public class RecognitionResult
        {
            public string status;
            public string id;
            public List<RecognizedItem> hypotheses;
        }

        public static RecognitionResult Parse(String toParse)
        {
            //Шапка
            Regex regexCommonInfo = new Regex(@"""status"":(?<status>\d),""id"":""(?<id>[\w-]+)""");
            RecognitionResult result = new RecognitionResult();
            var match = regexCommonInfo.Match(toParse);
            result.id = match.Groups["id"].Value;
            result.status = match.Groups["status"].Value;

            //Гипотезы
            Regex regexUtter = new Regex(@"""utterance"":""(?<utter>[а-яА-Я\s\w.,]+)"",""confidence"":(?<conf>[\d.]+)");

            float confidence;
            var matches = regexUtter.Matches(toParse);
            List<RecognizedItem> hypos = new List<RecognizedItem>();
            foreach (Match m in matches)
            {
                var g = m.Groups;
                confidence = float.Parse(g["conf"].Value.Replace(".", ","));
                hypos.Add(new RecognizedItem { confidence = confidence, utterance = g["utter"].Value });
            }
            result.hypotheses = hypos;


            return result;
        }
    }
}
