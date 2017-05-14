using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxAXVLC;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using Emgu.Util;
using AForge.Video;
using AForge.Video.FFMPEG;

namespace WindowsFormsApplication6
{
    public partial class Form4 : Form
    {
        float frameTime;

        


        public Form4()
        {
            InitializeComponent();
        }

        public Form4(OpenFileDialog strUrl)
        {
            InitializeComponent();
            vlc1.playlist.items.clear();
            vlc1.playlist.add("file:///" + strUrl.FileName, strUrl.SafeFileName, null);
            vlc1.playlist.play();
        }

        public float time
        {
            set
            {
                frameTime = value;
            }
            get
            {
                return frameTime;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }



        private void vlc1_MediaPlayerTimeChanged(object sender, DVLCEvents_MediaPlayerTimeChangedEvent e)
        {
            float t = 1000;
            float time = e.time / t;
            int min = Convert.ToInt32(time) / 60;
            int sec = Convert.ToInt32(time) - min*60;
            textMin.Text = min.ToString();
            textSec.Text = sec.ToString();
            frameTime = time;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }


        


        


    }
}
