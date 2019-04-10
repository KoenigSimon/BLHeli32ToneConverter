using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace NotenKonverter
{
    public partial class Form1 : Form
    {   
        public Form1()
        {
            InitializeComponent();
            //bild umskalieren
            Image bildAnfang = pictureBox1.Image;
            pictureBox1.Image = ResizeImage(bildAnfang, pictureBox1.Width, pictureBox1.Height);
        }

        string[] inputLines;

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            inputLines = richTextBox1.Lines;

            for (int i = 0; i < inputLines.Length; i++)
            {
                //leere ignorieren
                if (inputLines[i] == "")
                {
                    richTextBox2.AppendText("\n");
                    continue;
                }

                //LH und RH entfernen
                if (inputLines[i][0] == 'L' || inputLines[i][0] == 'R')
                {
                    inputLines[i] = inputLines[i].Remove(0, 3);
                }

                //oktave bestimmen + korrigieren
                int octave = 0;
                try
                {
                    octave = (int.Parse(inputLines[i][0].ToString())) + Convert.ToInt32(textBox1.Text);
                } catch (Exception ex)
                {

                }

                //zuschneiden pipes entfernen                
                inputLines[i] = inputLines[i].Remove(0, 2);
                inputLines[i] = inputLines[i].Remove(inputLines[i].Length - 1, 1);

                //konvertieren
                string final = "";
                for (int j = 0; j < inputLines[i].Length; j++)
                {
                    switch(inputLines[i][j])
                    {
                        case '-':
                            if(checkBox1.Checked)
                            {
                                if (j < inputLines[i].Length - 1 && inputLines[i][j + 1] == '-')
                                {
                                    if (j < inputLines[i].Length - 2 && inputLines[i][j + 2] == '-')
                                    {
                                        if (j < inputLines[i].Length - 3 && inputLines[i][j + 3] == '-')
                                        {
                                            j += 3;
                                            final += "P1";
                                            break;
                                        }
                                        j += 2;
                                        final += "P1/2 P1/4";
                                        break;
                                    }
                                    j++;
                                    final += "P1/2";
                                    break;
                                }
                            }                            
                            final += "P1/4 ";
                            break;
                        case 'c':
                            final += "C" + octave + "1/4 ";
                            break;
                        case 'd':
                            final += "D" + octave + "1/4 ";
                            break;
                        case 'e':
                            final += "E" + octave + "1/4 ";
                            break;
                        case 'f':
                            final += "F" + octave + "1/4 ";
                            break;
                        case 'g':
                            final += "G" + octave + "1/4 ";
                            break;
                        case 'a':
                            final += "A" + octave + "1/4 ";
                            break;
                        case 'b':
                            final += "B" + octave + "1/4 ";
                            break;
                        case 'C':
                            final += "C#" + octave + "1/4 ";
                            break;
                        case 'D':
                            final += "D#" + octave + "1/4 ";
                            break;
                        case 'F':
                            final += "F#" + octave + "1/4 ";
                            break;
                        case 'G':
                            final += "G#" + octave + "1/4 ";
                            break;
                        case 'A':
                            final += "A#" + octave + "1/4 ";
                            break;
                    }
                }
                richTextBox2.AppendText(final);
                richTextBox2.AppendText("\n");              
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }

        private void VisitLink()
        {
            // Change the color of the link text by setting LinkVisited   
            // to true.  
            linkLabel1.LinkVisited = true;
            //Call the Process.Start method to open the default browser   
            //with a URL:  
            System.Diagnostics.Process.Start("https://pianoletternotes.blogspot.com/p/meme-songs-list.html");
        }

        public static Image ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return (Image)destImage;
        }
    }

}
