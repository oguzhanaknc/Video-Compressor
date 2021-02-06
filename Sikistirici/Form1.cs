using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xabe.FFmpeg;
namespace Sikistirici
{

    public partial class Form1 : Form
    {
        string filePath = "";
        string fileName = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog1.FileName;
                fileName = openFileDialog1.SafeFileName;
                label1.Text = "Seçilen Video : " + filePath;


            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (filePath == "")
            {
                MessageBox.Show("Lütfen Küçültülecek Dosyayı Seçin", "Hata");
            }

            string outputPath = "Sikistirilmis_"+fileName;
            IMediaInfo inputFile = await FFmpeg.GetMediaInfo(filePath);
            IVideoStream output  =   inputFile.VideoStreams.First().SetSize(VideoSize.Hd480);
            IConversion conversion =  FFmpeg.Conversions.New().AddStream(output).SetOutput(outputPath);
            conversion.OnProgress += async (sender, args) =>
            {
                progressBar1.Value = args.Percent;
            };
            IConversionResult conversionResult = await conversion.Start();
            if (progressBar1.Value == 100)
            {
                MessageBox.Show("Video Başarıyla Küçültüldü.","Başarılı");
                progressBar1.Value = 0;
                filePath = "";
            }

        }

    }
}