using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TWoCLoadRemover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            bmpTopLeft = new Bitmap(100, 100);
            bmpTopMid = new Bitmap(100, 100);
            bmpMiddle = new Bitmap(100, 100);
            savedbmp = new Bitmap(100, 100);
            savedbmp2 = new Bitmap(100, 100);
            savedbmp3 = new Bitmap(100, 100);
            savedbmp4 = new Bitmap(100, 100);
            btnStop.IsEnabled = false;
            bst = 0;
            tlst = 0;
            tbxBSV.IsEnabled = false;
            tbxTLSV.IsEnabled = false;
            tbxMLSV.IsEnabled = false;
            tbxWV.IsEnabled = false;

            //create files if they're missing
            if (!File.Exists("saved.bmp"))
            {
                savedbmp.Save("saved.bmp");
            }

            if (!File.Exists("saved2.bmp"))
            {
                savedbmp.Save("saved2.bmp");
            }

            if (!File.Exists("saved3.bmp"))
            {
                savedbmp.Save("saved3.bmp");
            }

            if (!File.Exists("saved4.bmp"))
            {
                savedbmp.Save("saved4.bmp");
            }

            if (!File.Exists("settings.ini"))
            {
                string[] tmpSettings = new string[] { "0", "0", "100", "100", "2", "2", "2", "2", "0", "0", "0", "0" };
                File.WriteAllLines("settings.ini", tmpSettings);
            }
            else //otherwise load settings
            {
                string[] settings = File.ReadAllLines("settings.ini");

                textBox.Text = settings[0];
                textBox1.Text = settings[1];
                textBox2.Text = settings[2];
                textBox3.Text = settings[3];
                textBox4.Text = settings[4];
                textBox5.Text = settings[5];
                textBox6.Text = settings[6];
                textBox7.Text = settings[7];
                tbxBST.Text = settings[8];
                tbxTLST.Text = settings[9];
                tbxMLST.Text = settings[10];
                tbxWT.Text = settings[11];
            }

        }

        Timer timer;
        Bitmap bmpTopLeft;
        Bitmap bmpTopMid;
        Bitmap bmpMiddle;
        Bitmap savedbmp, savedbmp2, savedbmp3, savedbmp4;
        int bst, tlst, mlst, wt;

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            bst = Convert.ToInt32(tbxBST.Text);
            tlst = Convert.ToInt32(tbxTLST.Text);
            mlst = Convert.ToInt32(tbxMLST.Text);
            wt = Convert.ToInt32(tbxWT.Text);
            tbxBST.IsEnabled = false;
            tbxTLST.IsEnabled = false;
            tbxMLST.IsEnabled = false;
            tbxWT.IsEnabled = false;
            savedbmp = new Bitmap("saved.bmp");
            savedbmp2 = new Bitmap("saved2.bmp");
            savedbmp3 = new Bitmap("saved3.bmp");
            savedbmp4 = new Bitmap("saved4.bmp");
            timer = new Timer(1000 / 10);
            timer.Elapsed += OnTimedEvent;

            timer.Start();

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
            tbxBST.IsEnabled = true;
            tbxTLST.IsEnabled = true;
            tbxMLST.IsEnabled = true;
            tbxWT.IsEnabled = true;
            savedbmp.Dispose();
            savedbmp2.Dispose();
            savedbmp3.Dispose();
            savedbmp4.Dispose();
        }


        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                //set default values
                int x = 0;
                int y = 0;
                int w = 100;
                int h = 100;
                int h1 = 2;
                int h2 = 2;
                int w1 = 2;
                int w2 = 2;

                //attempt to read values from UI
                Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        x = Convert.ToInt32(textBox.Text);
                    }
                    catch (Exception ex)
                    {
                        x = 0;
                    }

                    try
                    {
                        y = Convert.ToInt32(textBox1.Text);
                    }
                    catch (Exception ex)
                    {
                        y = 0;
                    }
                    try
                    {
                        w = Convert.ToInt32(textBox2.Text);
                    }
                    catch (Exception ex)
                    {
                        w = 100;
                    }
                    try
                    {
                        h = Convert.ToInt32(textBox3.Text);
                    }
                    catch (Exception ex)
                    {
                        h = 100;
                    }
                    try
                    {
                        h1 = Convert.ToInt32(textBox4.Text);
                    }
                    catch (Exception ex)
                    {
                        h1 = 2;
                    }
                    try
                    {
                        h2 = Convert.ToInt32(textBox5.Text);
                    }
                    catch (Exception ex)
                    {
                        h2 = 2;
                    }
                    try
                    {
                        w1 = Convert.ToInt32(textBox6.Text);
                    }
                    catch (Exception ex)
                    {
                        w1 = 2;
                    }
                    try
                    {
                        w2 = Convert.ToInt32(textBox7.Text);
                    }
                    catch (Exception ex)
                    {
                        w2 = 2;
                    }
                }
                ));
                Bitmap bitmap = new Bitmap(w, h);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(x, y, 0, 0, bitmap.Size);
                }

                bmpTopLeft = CropBitMap((Bitmap)bitmap.Clone(),0,0,w1,h1);
                bmpTopMid = CropBitMap((Bitmap)bitmap.Clone(), bitmap.Width-w2, 0, w2, h1);
                bmpMiddle = CropBitMap((Bitmap)bitmap.Clone(), bitmap.Width - w2, bitmap.Height-h2, w2, h2);

                try
                {
                    Bitmap bm1 = new Bitmap(bmpTopMid, new System.Drawing.Size(bmpTopMid.Width / 2, bmpTopMid.Height / 2));
                    Bitmap bm2 = new Bitmap(savedbmp, new System.Drawing.Size(savedbmp.Width / 2, savedbmp.Height / 2));
                    Bitmap bm3 = new Bitmap(savedbmp2, new System.Drawing.Size(savedbmp2.Width / 2, savedbmp2.Height / 2));

                    Bitmap bm4 = new Bitmap(bmpMiddle, new System.Drawing.Size(bmpMiddle.Width / 2, bmpMiddle.Height / 2));
                    Bitmap bm5 = new Bitmap(savedbmp3, new System.Drawing.Size(savedbmp3.Width / 2, savedbmp3.Height / 2));

                    Bitmap bm6 = new Bitmap(bmpTopLeft, new System.Drawing.Size(bmpTopLeft.Width / 2, bmpTopLeft.Height / 2));
                    Bitmap bm7 = new Bitmap(savedbmp4, new System.Drawing.Size(savedbmp4.Width / 2, savedbmp4.Height / 2));

                    bool flag1 = false;
                    bool flag2 = false;
                    bool flag3 = false;
                    bool flag4 = false;

                    float val1;
                    float val2;
                    float val3;
                    float val4;

                    string response1 = "";
                    string response2 = "";
                    string response3 = "";
                    string response4 = "";

                    try
                    {
                        val1 = (compare(bm4, bm2) + 1) / 10000000;

                        response1 = val1.ToString();

                        if (val1 < bst)
                        {
                            flag1 = true;
                        }
                    }
                    catch(Exception )
                    {
                        response1 = "Image Size Error";
                    }

                    try
                    {
                        val2 = (compare(bm1, bm3) + 1) / 10000000;

                        response2 = val2.ToString();

                        if (val2 < tlst)
                        {
                            flag2 = true;
                        }
                    }
                    catch(Exception )
                    {
                        response2 = "Image Size Error";
                    }

                    try
                    {
                        val3 = (compare(bm4, bm5) + 1) / 10000000;

                        response3 = val3.ToString();

                        if (val3 < mlst)
                        {
                            flag3 = true;
                        }
                    }
                    catch(Exception )
                    {
                        response3 = "Image Size Error";
                    }
                        
                    try
                    {
                        val4 = (compare(bm6, bm7) + 1) / 10000000;

                        response4 = val4.ToString();

                        if (val4 < wt)
                        {
                            flag4 = true;
                        }
                    }
                    catch
                    {
                        //if compare functions fail
                        response4 = "Image Size Error";
                    }


                    Dispatcher.Invoke(new Action(() =>
                    {
                        checkBox.IsChecked = flag1;
                        checkBox1.IsChecked = flag2;
                        checkBox2.IsChecked = flag3;
                        checkBox3.IsChecked = flag4;

                        if (cbxPreview.IsChecked == true)
                        {
                            //resize image to fit preview
                            Bitmap tmp = new Bitmap(bitmap, new System.Drawing.Size(150, 150));
                            imgPreview.Source = BitmapToImageSource(tmp);
                            tbxBSV.Text = response1;
                            tbxTLSV.Text = response2;
                            tbxMLSV.Text = response3;
                            tbxWV.Text = response4;
                        }
                        else if (cbxPreviewH1.IsChecked == true)
                        {
                            //resize image to fit preview
                            Bitmap tmp = new Bitmap(bmpTopMid, new System.Drawing.Size(150, 150));
                            imgPreview.Source = BitmapToImageSource(tmp);
                            tbxBSV.Text = response1;
                            tbxTLSV.Text = response2;
                            tbxMLSV.Text = response3;
                            tbxWV.Text = response4;
                        }
                        else if (cbxPreviewH2.IsChecked == true)
                        {
                            //resize image to fit preview
                            Bitmap tmp = new Bitmap(bmpMiddle, new System.Drawing.Size(150, 150));
                            imgPreview.Source = BitmapToImageSource(tmp);
                            tbxBSV.Text = response1;
                            tbxTLSV.Text = response2;
                            tbxMLSV.Text = response3;
                            tbxWV.Text = response4;
                        }
                        else if (cbxWumpa.IsChecked == true)
                        {
                            //resize image to fit preview
                            Bitmap tmp = new Bitmap(bmpTopLeft, new System.Drawing.Size(150, 150));
                            imgPreview.Source = BitmapToImageSource(tmp);
                            tbxBSV.Text = response1;
                            tbxTLSV.Text = response2;
                            tbxMLSV.Text = response3;
                            tbxWV.Text = response4;
                        }
                    }));
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception exp)
            {
            }
        }

        private float compare(Bitmap bmp1, Bitmap bmp2)
        {
            List<Color> im1 = new List<Color>(HistoGram(bmp1));
            List<Color> im2 = new List<Color>(HistoGram(bmp2));

            float val = 0;
            for (int i = 0; i < im1.Count; i++)
            {
                val += Math.Abs((float)(im1.ElementAt(i).ToArgb() - im2.ElementAt(i).ToArgb()));
            }

            return val;
        }

        private List<Color> HistoGram(Bitmap bitmap)
        {
            Bitmap newbitmap = (Bitmap)bitmap.Clone();
            // Store the histogram in a dictionary          
            List<Color> histo = new List<Color>();
            for (int x = 0; x < newbitmap.Width; x++)
            {
                for (int y = 0; y < newbitmap.Height; y++)
                {
                    // Get pixel color 
                    Color c = newbitmap.GetPixel(x, y);
                    // If it exists in our 'histogram' increment the corresponding value, or add new
                    histo.Add(c);
                }
            }
            return histo;
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            string[] settings = new string[12];

            settings[0] = textBox.Text;
            settings[1] = textBox1.Text;
            settings[2] = textBox2.Text;
            settings[3] = textBox3.Text;
            settings[4] = textBox4.Text;
            settings[5] = textBox5.Text;
            settings[6] = textBox6.Text;
            settings[7] = textBox7.Text;
            settings[8] = tbxBST.Text;
            settings[9] = tbxTLST.Text;
            settings[10] = tbxMLST.Text;
            settings[11] = tbxWT.Text;

            File.WriteAllLines("settings.ini", settings);
            MessageBox.Show("Saved!");
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] settings = File.ReadAllLines("settings.ini");

                textBox.Text = settings[0];
                textBox1.Text = settings[1];
                textBox2.Text = settings[2];
                textBox3.Text = settings[3];
                textBox4.Text = settings[4];
                textBox5.Text = settings[5];
                textBox6.Text = settings[6];
                textBox7.Text = settings[7];
                tbxBST.Text = settings[8];
                tbxTLST.Text = settings[9];
                tbxMLST.Text = settings[10];
                tbxWT.Text = settings[11];
            }
            catch (Exception ex)
            {
                MessageBox.Show("No settings found, be sure to save settings first");
            }

        }

        private void btnImage4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bitmap newbmp = new Bitmap(bmpTopLeft);
                savedbmp4.Dispose();
                bmpTopLeft.Dispose();
                newbmp.Save("saved4.bmp");
                savedbmp4 = newbmp;
                bmpTopLeft = newbmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save failed, is the image or livesplit open?\n" + ex.Message);
            }
        }

        private void btnImage1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bitmap newbmp = new Bitmap(bmpTopMid);
                savedbmp.Dispose();
                bmpTopMid.Dispose();
                newbmp.Save("saved.bmp");
                savedbmp = newbmp;
                bmpTopMid = newbmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save failed, is the image or livesplit open?\n" + ex.Message);
            }

        }

        private void btnImage3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bitmap newbmp = new Bitmap(bmpMiddle);
                savedbmp3.Dispose();
                bmpMiddle.Dispose();
                newbmp.Save("saved3.bmp");
                savedbmp3 = newbmp;
                bmpMiddle = newbmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save failed, is the image or livesplit open?\n" + ex.Message);
            }
        }

        private void btnImage2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bitmap newbmp = new Bitmap(bmpTopMid);
                savedbmp2.Dispose();
                bmpTopMid.Dispose();
                newbmp.Save("saved2.bmp");
                savedbmp2 = newbmp;
                bmpTopMid = newbmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save failed, is the image or livesplit open?\n" + ex.Message);
            }
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        Bitmap CropBitMap(Bitmap tmp, int x, int y, int w, int h)
        {
            Rectangle cropRect = new Rectangle(x, y, w, h);
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(tmp, new Rectangle(0, 0, target.Width, target.Height),
                                 cropRect,
                                 GraphicsUnit.Pixel);
            }

            return target;
        }
    }
}
