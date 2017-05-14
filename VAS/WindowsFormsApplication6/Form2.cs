using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Drawing.Imaging;

namespace WindowsFormsApplication6
{ 
    public partial class Form2 : Form
    {
        Image<Bgr, byte> game; //空白球場
        Image<Bgra, byte> ad = null;
        Image<Bgra, byte> ad_original = null;
        Image<Bgr, byte> result = null; //結果的預覽圖
        Size size;
        Size ad_size;

        float ax, bx, cx, dx;
        float ay, by, cy, dy;

        float fax, fbx, fcx, fdx;
        float fay, fby, fcy, fdy;

        Point[] point30;
        int select;

        private void button3_Click(object sender, EventArgs e)
        {

        }
        string savePath;
        String adFilename;
        string videoFileName;

        PointF[] ad_image = new PointF[4]; //儲存原始廣告的四點(x,y)
        PointF[] result_image = new PointF[4]; //儲存要貼的四點(x,y)

        double adTransR;
        double adTransG;
        double adTransB;
        double frameR;
        double frameG;
        double frameB;
        double ad_alpha;
        double game_alpha;

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Alpha_trackBar_MouseUp(object sender, MouseEventArgs e)  //Alpha值滑鼠放開重新貼圖
        {
            if (select == 1)
            {
                Image<Bgra, byte> game = new Image<Bgra, byte>(@"C:\Users\The LAB Mac\Videos\finding\need3.JPG"); //空白球場
                ad_original = new Image<Bgra, byte>(adFilename);
                Image<Bgra, byte> ad = ad_original.Resize(1280, 720, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);

                Console.WriteLine(ad.Width + " " + ad.Height);
                Image<Bgra, byte> result = null; //結果的預覽圖
                size = CvInvoke.cvGetSize(game);
                result = new Image<Bgra, byte>(size);
                ad_pictureBox.Image = ad.ToBitmap();

                ad_alpha = Alpha_trackBar.Value * 0.01;
                game_alpha = 1 - ad_alpha;

                HomographyMatrix mywrapmat = CameraCalibration.GetPerspectiveTransform(ad_image, result_image); //原始點與對應點轉換
                Image<Bgra, byte> adTrans = ad.WarpPerspective(mywrapmat, Emgu.CV.CvEnum.INTER.CV_INTER_NN, Emgu.CV.CvEnum.WARP.CV_WARP_FILL_OUTLIERS, new Bgra(0, 0, 0, 0)); //adTrans是廣告變形完的結果
                adTrans_pictureBox.Image = adTrans.ToBitmap();

                for (int i = 0; i < adTrans.Height; i++)
                {
                    for (int j = 0; j < adTrans.Width; j++)
                    {
                        double r = adTrans.Data[i, j, 0]; //adTrans的紅色
                        double g = adTrans.Data[i, j, 1]; //adTrans的綠色
                        double b = adTrans.Data[i, j, 2]; //adTrans的藍色

                        if (r == 0 && g == 0 && b == 0)  //讓育新昏倒的地方 
                        {
                            game.Data[i, j, 0] = game.Data[i, j, 0];
                            game.Data[i, j, 1] = game.Data[i, j, 1];
                            game.Data[i, j, 2] = game.Data[i, j, 2];
                        }
                        else
                        {
                            adTransR = adTrans.Data[i, j, 0] * ad_alpha;
                            adTransG = adTrans.Data[i, j, 1] * ad_alpha;
                            adTransB = adTrans.Data[i, j, 2] * ad_alpha;
                            frameR = game.Data[i, j, 0] * game_alpha;
                            frameG = game.Data[i, j, 1] * game_alpha;
                            frameB = game.Data[i, j, 2] * game_alpha;

                            game.Data[i, j, 0] = (byte)(adTransR + frameR);
                            game.Data[i, j, 1] = (byte)(adTransG + frameG);
                            game.Data[i, j, 2] = (byte)(adTransB + frameB);
                        }
                    }
                }
                CvInvoke.cvCopy(game, result, IntPtr.Zero); //把game的結果複製到result上
                resultPictureBox.Image = result.ToBitmap();
            }else if(select == 2)
            {
                Image<Bgra, byte> game = new Image<Bgra, byte>(@"C:\Users\The LAB Mac\Videos\finding\need2.JPG"); //空白球場
                ad_original = new Image<Bgra, byte>(adFilename);
                Image<Bgra, byte> ad = ad_original.Resize(1280, 720, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);

                Console.WriteLine(ad.Width + " " + ad.Height);
                Image<Bgra, byte> result = null; //結果的預覽圖
                size = CvInvoke.cvGetSize(game);
                result = new Image<Bgra, byte>(size);
                ad_pictureBox.Image = ad.ToBitmap();

                ad_alpha = Alpha_trackBar.Value * 0.01;
                game_alpha = 1 - ad_alpha;

                HomographyMatrix mywrapmat = CameraCalibration.GetPerspectiveTransform(ad_image, result_image); //原始點與對應點轉換
                Image<Bgra, byte> adTrans = ad.WarpPerspective(mywrapmat, Emgu.CV.CvEnum.INTER.CV_INTER_NN, Emgu.CV.CvEnum.WARP.CV_WARP_FILL_OUTLIERS, new Bgra(0, 0, 0, 0)); //adTrans是廣告變形完的結果
                adTrans_pictureBox.Image = adTrans.ToBitmap();

                for (int i = 0; i < adTrans.Height; i++)
                {
                    for (int j = 0; j < adTrans.Width; j++)
                    {
                        double r = adTrans.Data[i, j, 0]; //adTrans的紅色
                        double g = adTrans.Data[i, j, 1]; //adTrans的綠色
                        double b = adTrans.Data[i, j, 2]; //adTrans的藍色

                        if (r == 0 && g == 0 && b == 0)  //讓育新昏倒的地方 
                        {
                            game.Data[i, j, 0] = game.Data[i, j, 0];
                            game.Data[i, j, 1] = game.Data[i, j, 1];
                            game.Data[i, j, 2] = game.Data[i, j, 2];
                        }
                        else
                        {
                            adTransR = adTrans.Data[i, j, 0] * ad_alpha;
                            adTransG = adTrans.Data[i, j, 1] * ad_alpha;
                            adTransB = adTrans.Data[i, j, 2] * ad_alpha;
                            frameR = game.Data[i, j, 0] * game_alpha;
                            frameG = game.Data[i, j, 1] * game_alpha;
                            frameB = game.Data[i, j, 2] * game_alpha;

                            game.Data[i, j, 0] = (byte)(adTransR + frameR);
                            game.Data[i, j, 1] = (byte)(adTransG + frameG);
                            game.Data[i, j, 2] = (byte)(adTransB + frameB);
                        }
                    }
                }
                CvInvoke.cvCopy(game, result, IntPtr.Zero); //把game的結果複製到result上
                resultPictureBox.Image = result.ToBitmap();
            }
        }
            

        private void Alpha_trackBar_Scroll(object sender, EventArgs e) //拉trackBar改變Alpha值
        {
            Alpha_label.Text = Alpha_trackBar.Value.ToString();
        }



        public Form2()
        {
            InitializeComponent();
        }

        public Form2(float pointax, float pointay,float pointbx, float pointby,float pointcx,float pointcy,float pointdx,float pointdy,Image emptyFrame, String ad_filename,string video_fileName,float fpointax, float fpointay, float fpointbx, float fpointby, float fpointcx, float fpointcy, float fpointdx, float fpointdy,Point[] fpoint,int selectVideo)
        {
            //variables from Form1
            InitializeComponent();
            adFilename = ad_filename;
            game = new Image<Bgr, byte>(new Bitmap(emptyFrame));
            ax = pointax;
            ay = pointay;
            bx = pointbx;
            by = pointby;
            cx = pointcx;
            cy = pointcy;
            dx = pointdx;
            dy = pointdy;

            fax = fpointax;
            fay = fpointay;
            fbx = fpointbx;
            fby = fpointby;
            fcx = fpointcx;
            fcy = fpointcy;
            fdx = fpointdx;
            fdy = fpointdy;

            point30 = fpoint;

            videoFileName = video_fileName;
            select = selectVideo;


        }

        public void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog path = new SaveFileDialog();
            path.InitialDirectory = System.IO.Path.GetDirectoryName(videoFileName);
            path.Filter = "avi files (*.avi)|*.avi";
            path.FileName = System.IO.Path.GetFileNameWithoutExtension(videoFileName) + "_vas";
            if (path.ShowDialog() == DialogResult.OK)
            {
                savePath = path.FileName;
                Console.WriteLine(savePath);
            }
            Form3 f3 = new Form3(ax,ay,bx,by,cx,cy,dx,dy,videoFileName,game,savePath, fax, fay, fbx, fby, fcx, fcy, fdx, fdy,point30, adFilename, ad_alpha, game_alpha,select);//產生Form2的物件，才可以使用它所提供的Method

            //將Form1隱藏。由於在Form1的程式碼內使用this，所以this為Form1的物件本身
            f3.Visible = true;//顯示第二個視窗
         
        }
        
        public void button1_Click(object sender, EventArgs e)
        {
            //產生Form2的物件，才可以使用它所提供的Method
            Form1 f1 = new Form1();
            this.Visible = false;//將Form1隱藏。由於在Form1的程式碼內使用this，所以this為Form1的物件本身
                                 // MessageBox.Show(f1.name); // 東西宣告成 public 就可以用
         
        }


        public void Form2_Load(object sender, EventArgs e)
        {
            ad_alpha = 0.7;
            game_alpha = 0.3;
            Alpha_trackBar.Value = 70;
            Alpha_label.Text = Alpha_trackBar.Value.ToString();
            ad_original = new Image<Bgra, byte>(adFilename);
            Image<Bgra, byte> ad = ad_original.Resize(1280, 720, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            Console.WriteLine(ad.Width + " " + ad.Height);
            resultPictureBox.Image = game.ToBitmap();
            size = CvInvoke.cvGetSize(game);
            ad_size = CvInvoke.cvGetSize(ad);
            ad_pictureBox.Image = ad.ToBitmap();
            result = new Image<Bgr, byte>(size);

            CvInvoke.cvCopy(game, result, IntPtr.Zero);

            ad_image[0] = new PointF(0, 0); //原始廣告的點一
            ad_image[1] = new PointF(ad.Width, 0); //原始廣告的點二
            ad_image[2] = new PointF(0, ad.Height); //原始廣告的點三
            ad_image[3] = new PointF(ad.Width, ad.Height); //原始廣告的點四

            result_image[0] = new PointF(ax, ay); //點一的對應點
            result_image[1] = new PointF(bx, by); //點二的對應點
            result_image[2] = new PointF(cx, cy); //點三的對應點
            result_image[3] = new PointF(dx, dy); //點四的對應點


            HomographyMatrix mywrapmat = CameraCalibration.GetPerspectiveTransform(ad_image, result_image); //原始點與對應點轉換
            Image<Bgra, byte> adTrans = ad.WarpPerspective(mywrapmat, Emgu.CV.CvEnum.INTER.CV_INTER_NN, Emgu.CV.CvEnum.WARP.CV_WARP_FILL_OUTLIERS, new Bgra(0, 0, 0, 0)); //adTrans是廣告變形完的結果


            adTrans_pictureBox.Image = adTrans.ToBitmap();

            for (int i = 0; i < adTrans.Height; i++)
            {
                for (int j = 0; j < adTrans.Width; j++)
                {
                    double r = adTrans.Data[i, j, 0]; //adTrans的紅色
                    double g = adTrans.Data[i, j, 1]; //adTrans的綠色
                    double b = adTrans.Data[i, j, 2]; //adTrans的藍色

                    if (r == 0 && g == 0 && b == 0)  //讓育新昏倒的地方 
                    {
                        game.Data[i, j, 0] = game.Data[i, j, 0];
                        game.Data[i, j, 1] = game.Data[i, j, 1];
                        game.Data[i, j, 2] = game.Data[i, j, 2];
                    }
                    else
                    {
                        adTransB = adTrans.Data[i, j, 0] * 0.7;
                        adTransG = adTrans.Data[i, j, 1] * 0.7;
                        adTransR = adTrans.Data[i, j, 2] * 0.7;
                        frameB = game.Data[i, j, 0] * 0.3;
                        frameG = game.Data[i, j, 1] * 0.3;
                        frameR = game.Data[i, j, 2] * 0.3;

                        game.Data[i, j, 0] = (byte)(adTransB + frameB);
                        game.Data[i, j, 1] = (byte)(adTransG + frameG);
                        game.Data[i, j, 2] = (byte)(adTransR + frameR);
                    }
                }
            }
            CvInvoke.cvCopy(game, result, IntPtr.Zero); //把game的結果複製到result上
            resultPictureBox.Image = result.ToBitmap();
            result.Save(@"C://Users/The LAB Mac/Pictures/adframe.JPG");
        }
    }
}
