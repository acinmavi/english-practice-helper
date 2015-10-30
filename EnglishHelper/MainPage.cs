using DBHelper;
using EnglishSTT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TextToSpeechHelper;
using DBHelperGUI;
using WordsMatching;
using System.Diagnostics;

namespace EnglishHelper
{
    public partial class MainPage : Form
    {
        private bool _isRecording;
        private SoundIO _soundIO;
        private IEnglishSentencesRepository target = null;

        public MainPage()
        {
            InitializeComponent();

            FillDevices();
            FillFreqs();

            if (target == null)
            {
                target = new EnglishSentencesRepository();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSentence.Text.Trim()))
            {
                TTSHelper.GetInstance().TTSSpeaker(txtSentence.Text);
            }
            else
            {
                MessageBox.Show("No no,sentences are not found");
            }
        }

        private void buttonRec_Click(object sender, EventArgs e)
        {
            if (_isRecording)
            {
                //buttonRec.Content = "Rec";
                labelRec.Visible = false;
                _isRecording = false;
                btRecord.Enabled = false;
                Stop();
            }
            else
            {
                //buttonRec.Content = "Stop";
                _isRecording = true;

                //micImage.Visibility = Visibility.Visible;
                labelRec.Visible = true;
                Start();
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            txtResponses.Text = "";
        }

        private void Start()
        {
            _soundIO = SoundIO.getSoundIO(cmbDevices.Items[cmbDevices.SelectedIndex].ToString(), "wav", cmbFreq.Items[cmbFreq.SelectedIndex].ToString());

            //new SoundIO(cmbDevices.SelectedItem.ToString(), "wav", cmbFreq.SelectedItem.ToString());
            _soundIO.OutputStream = new MemoryStream();
            _soundIO.StartRecording();
        }

        private void Stop()
        {
            _soundIO.StopRecording();
            txtProcessing.Visible = true;
            Thread requestThread = new Thread(SendRequest);
            requestThread.Start();
        }

        private void UpdateText(string txt)
        {
            txtResponses.BeginInvoke(new Action<string>(t => txtResponses.Text = txt + "\n"), txt);
            labelRec.BeginInvoke(new Action(() => labelRec.Visible = false));
            txtProcessing.BeginInvoke(new Action(() => txtProcessing.Visible = false));
            btRecord.BeginInvoke(new Action(() => btRecord.Enabled = true));
        }

        private void SendRequest()
        {
            string str = "";
            try
            {
                str = SoundRecognition.WavStreamToGoogle(_soundIO.OutputStream);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _soundIO.OutputStream.Close();
                UpdateText(str);

                //thread to compare string
                Thread requestThread = new Thread(CompareString);
                requestThread.Start(str);
            }
        }

        private void CompareString(object str)
        {
            if(string.IsNullOrEmpty(txtSentence.Text) ||str == null ||string.IsNullOrEmpty(str.ToString()))
            {
                MessageBox.Show("Sentence not found,nothing to compare");
            }else
            {
                //string result = ResultCompare.MakeResult(txtSentence.Text, str.ToString(), false);
                float percent = MatchsMaker.getInstance(txtSentence.Text, str.ToString()).Score * 100;
                string stringPercent = percent.ToString("0.##\\%");
                string result = ResultCompare.MakeResultString(txtSentence.Text, str.ToString(), (int) percent);
                txtProcessing.BeginInvoke(new Action(() => txtResult.Text = result));
            }

        }

        private void FillDevices()
        {
            IEnumerable<string> divides = SoundIO.AvailableDevicesDescriptions;
            foreach (var divide in divides)
            {
                cmbDevices.Items.Add(divide);
            }
            cmbDevices.SelectedIndex = 0;
        }

        private void FillFreqs()
        {
            IEnumerable<string> formats = SoundIO.StandartSoundFormatsDescriptions;
            foreach (string format in formats)
            {
                cmbFreq.Items.Add(format);
            }
            cmbFreq.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btNext_Click(object sender, EventArgs e)
        {
            var Sentences = target.ToList();
            if (Sentences != null)
            {
                var rand = new Random();
                var sentence = Sentences.ElementAt(rand.Next(Sentences.Count()));

                if (sentence != null)
                {
                    txtSentence.Text = sentence.Content;
                }
                else
                {
                    MessageBox.Show("Nothing in DB,please using Add to put some data in db");
                }
            }
            else
            {
                MessageBox.Show("Nothing in DB,please using DBHelperGUI to put some data in db");
            }
        }

        private void buttonAddData_Click(object sender, EventArgs e)
        {
            DBGUI formDB = new DBGUI();
            formDB.ShowDialog();
        }

        private void btQuickAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (target.ToList().Any(o => o.Content.ToUpper().Contains(txtSentence.Text.ToUpper())))
                {
                    MessageBox.Show("'" + txtSentence.Text+"'" + " is exist on db");
                }
                else
                {
                    EnglishSentences form = new EnglishSentences();
                    form.Content = txtSentence.Text;
                    target.Create(form);
                    MessageBox.Show("Quick add :'" + txtSentence.Text + "' to db");
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Not that easy :" + ex.Message);
            }
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutForm aboutf = new AboutForm();
            aboutf.ShowDialog();
        }

        private void howToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Directory.GetCurrentDirectory() + @"\Readme\Readme.htm");
        }
    }
}