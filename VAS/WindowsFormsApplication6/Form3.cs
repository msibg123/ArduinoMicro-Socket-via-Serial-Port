using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using AForge.Video;
using AForge.Video.FFMPEG;

namespace WindowsFormsApplication6
{
    public partial class Form3 : Form
    {
        string videoFileName;
        String ad_filename_F3;
        string savePath;
        string savePathVideo;
        Image<Bgr, byte> gameEmpty;
        float Ax, Bx, Cx, Dx;
        float Ay, By, Cy, Dy;

        float fAx, fBx, fCx, fDx ;
        float fAy, fBy, fCy, fDy ;
     
        Point[] pointSd;

        double adAlpha;
        double gameAlpha;
        int selectVideo;
 


        public Form3()
        {
            InitializeComponent();
        }

        public Form3(float ax, float ay, float bx, float by, float cx, float cy, float dx, float dy, string video_fileName, Image<Bgr, byte> game,string save, float fax, float fay, float fbx, float fby, float fcx, float fcy, float fdx, float fdy,Point[] point30, String adFilename, double ad_alpha, double game_alpha,int select)
        {
            //variables from Form2
            InitializeComponent();
            videoFileName = video_fileName;
            ad_filename_F3 = adFilename;
            gameEmpty = game;
            savePath = save.Substring(0, save.LastIndexOf("\\")) + @"\tmp\";
            savePathVideo = save;
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            Ax = ax;
            Ay = ay;
            Bx = bx;
            By = by;
            Cx = cx;
            Cy = cy;
            Dx = dx;
            Dy = dy;

            fAx = fax;
            fAy = fay;
            fBx = fbx;
            fBy = fby;
            fCx = fcx;
            fCy = fcy;
            fDx = fdx;
            fDy = fdy;



            pointSd = point30;

            adAlpha = ad_alpha;
            gameAlpha = game_alpha;
            selectVideo = select;

        }

        public void Form3_Load(object sender, EventArgs e)
        {
            Console.WriteLine(ad_filename_F3);
            Console.WriteLine("alpha = " + adAlpha);
        }

        private void Form3_Shown(object sender, EventArgs e)
        {
            MessageBox.Show("開始處理");
            Form3 aForm = this;
            //Create instance
            VideoProcessor processor = new VideoProcessor();
            processor.SetInput(videoFileName);
            processor.SetCompare(gameEmpty);
            processor.SetOutput(savePath, ".jpg");
            processor.SetOutputVideo(savePathVideo);
            processor.SetPoint(Ax, Ay, Bx, By, Cx, Cy, Dx, Dy, fAx, fAy, fBx, fBy, fCx, fCy, fDx, fDy, pointSd, ad_filename_F3, adAlpha, gameAlpha,selectVideo);
            processor.SetProgressBar(aForm);
            //Start the process
            processor.Run();
        } 

        private void button2_Click(object sender, EventArgs e)
        {
            //產生Form2的物件，才可以使用它所提供的Method

            this.Visible = false;//將Form1隱藏。由於在Form1的程式碼內使用this，所以this為Form1的物件本身
            //顯示第二個視窗
            Form2 f2 = new Form2();
            Form1 f1 = new Form1();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        public class VideoProcessor
        {
            StreamWriter sw = new StreamWriter(@"C:/point txt/Data.TXT");

            Form3 _aForm;
            int countP = 0;
            int allX = 0;
            int allfX = 0;
            //the OpenCV video capture object
            private Capture capture;
            //Point[] tempA, tempB, tempC, tempD = new Point[1];
            Point[] tempA = new Point[1];
            Point[] tempB = new Point[1];
            Point[] tempC = new Point[1];
            Point[] tempD = new Point[1];

            //a bool to determine if the process
            //callback will be called
            private bool callIt;
            //the callback function to be called
            //for the processing of each frame
            private Func<IImage, IImage> process;
            //delay between each frame processing
            int delay;
            //number of processed frames
            long fnumber;
            //stop at this frame number
            long frameToStop;
            //to stop the processing
            bool stop;

            //The OpenCV video witer object
            VideoWriter writer;
            //output filename
            string outputFile;
            //current index for output images
            int currentIndex;
            //number of digits in output image filename
            int digits;

            string videoSavePath;

            //extension of output images
            string extension;
            //模板路徑
            string comparefileName;
            //比較結果
            float Ax, Bx, Cx, Dx;
            float Ay, By, Cy, Dy;

            float fAx, fBx, fCx, fDx;
            float fAy, fBy, fCy, fDy;
            //廣告以及球場的點
            String ad_filename_F3;

            Point[] pointSd = new Point[4];
            double adAlpha;
            double gameAlpha;
            int selectVideo;
            //進度條
            int progressPercent;




            //30組空白球場點

            public int getXmove()
            {

                int resultXmove = 0;
                resultXmove = (pointz[0].X + pointz[1].X - Convert.ToInt32(fAx) - Convert.ToInt32(fBx)) / 2;
                if (resultXmove > 0)
                {
                    resultXmove = resultXmove - 10;
                    if (resultXmove < 0)
                    {
                        resultXmove = 0;
                    }
                }
                else if (resultXmove < 0)
                {
                    resultXmove = resultXmove + 10;
                    if (resultXmove > 0)
                    {
                        resultXmove = 0;
                    }
                }

                return resultXmove;
            }
            public int getYmove()
            {

                int resultYmove = 0;
                resultYmove = (pointz[0].Y + pointz[1].Y - Convert.ToInt32(fAy) - Convert.ToInt32(fBy)) / 2;
                if (resultYmove > 0)
                {
                    resultYmove = resultYmove - 60;
                    if (resultYmove < 0)
                    {
                        resultYmove = 0;
                    }
                }
                else if (resultYmove < 0)
                {
                    resultYmove = resultYmove + 60;
                    if (resultYmove > 0)
                    {
                        resultYmove = 0;
                    }
                }

                return resultYmove;
            }


            bool same = true;
            Point[] pointz = new Point[28];

            public Image<Gray, Byte> _Image1;
            public Image<Gray, Byte> _Image2;

            //frame是否輸出完畢
            bool isProcessOver = false;


            //球場變形 & 前景移除的variables
            Image<Bgr, byte> player = null;
            Size size;
            byte binary_value;
            int threshold = 23;
            int range;
            PointF[] game_image = new PointF[4]; //儲存原始廣告的四點(x,y)
            PointF[] gameTrans_image = new PointF[4]; //儲存要貼的四點(x,y)


            //貼廣告的variables
            Image<Bgr, byte> game;
            Image<Bgr, byte> game1;
            Image<Bgr, byte> ad = null;
            Image<Bgr, byte> ad_original = null;
            Image<Bgr, byte> ad_frame = null;
            Size ad_size;
            PointF[] ad_image = new PointF[4]; //儲存原始廣告的四點(x,y)
            PointF[] result_image = new PointF[4]; //儲存要貼的四點(x,y)
            double adTransR;
            double adTransG;
            double adTransB;
            double adframeR;
            double adframeG;
            double adframeB;



            //Construct
            public VideoProcessor()
            {
                callIt = true;
                delay = 0;
                fnumber = 0;
                stop = false;
                frameToStop = -1;
            }

            /// <summary>
            /// set the callback function that will be called
            /// for each frame
            /// </summary>
            /// <param name="process">the callback function</param>
            public void SetFrameProcessor(Func<IImage, IImage> process)
            {
                this.process = process;
            }
            /// <summary>
            /// set the name of the video file
            /// </summary>
            /// <param name="filename">the name of the video file</param>
            /// <returns>success or failure</returns>
            public bool SetInput(string filename)
            {
                this.fnumber = 0;
                //In case a resource was already
                //associated with the Capture instance
                if (this.capture != null)
                {
                    this.capture.Dispose();
                }
                try
                {
                    this.capture = new Capture(filename);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            /// <summary>
            /// Stop the porcessing
            /// </summary>
            public void StopIt()
            {
                this.stop = true;
            }
            /// <summary>
            /// Is the process stopped?
            /// </summary>
            public bool IsStopped
            {
                get { return this.stop; }
            }
            /// <summary>
            /// Is a capture device opened?
            /// </summary>
            public bool IsOpened
            {
                get { return this.capture != null; }
            }

            /// <summary>
            /// process callback to be called
            /// </summary>
            public void CallProcess()
            {
                this.callIt = true;
            }
            /// <summary>
            /// do not call process callback
            /// </summary>
            public void DontCallProcess()
            {
                this.callIt = false;
            }
            /// <summary>
            /// return the frame rate of the video file
            /// </summary>
            /// <returns>the frame rate of the video file</returns>
            public double GetFrameRate()
            {
                double frameRate = this.capture.GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FPS);
                return frameRate;
            }

            public void SetCompare(Image<Bgr, byte> gameEmpty)
            {
                game = gameEmpty;
            }

            public bool SetOutput(string filename, string ext)
            {
                //filenames and their common extension
                this.outputFile = filename;
                this.extension = ext;
                //number of digits in the file numbering scheme
                this.digits = 3;
                //start numbering at this index
                this.currentIndex = 0;
                return true;
            }

            public void SetOutputVideo(string path)
            {
                videoSavePath = path;
            }

            public void SetProgressBar(Form3 aForm)
            {
                _aForm = aForm;
            }

            public void SetPoint(float Ax, float Ay, float Bx, float By, float Cx, float Cy, float Dx, float Dy, float fAx, float fAy, float fBx, float fBy, float fCx, float fCy, float fDx, float fDy, Point[] pointSd, String ad_filename_F3, double adAlpha, double gameAlpha,int selectVideo)
            {
                //廣告以及球場點的傳值
                this.Ax = Ax;
                this.Ay = Ay;
                this.Bx = Bx;
                this.By = By;
                this.Cx = Cx;
                this.Cy = Cy;
                this.Dx = Dx;
                this.Dy = Dy;

                this.fAx = fAx;
                this.fAy = fAy;
                this.fBx = fBx;
                this.fBy = fBy;
                this.fCx = fCx;
                this.fCy = fCy;
                this.fDx = fDx;
                this.fDy = fDy;

                this.pointSd = pointSd;



                this.ad_filename_F3 = ad_filename_F3;
                this.adAlpha = adAlpha;
                this.gameAlpha = gameAlpha;
                this.selectVideo = selectVideo;

            }

            /// <summary>
            /// to write the output frame
            /// could be video or image
            /// </summary>
            /// <param name="frame">output frame</param>
            private void WriteNextFrame(IImage frame)
            {
                if (!string.IsNullOrWhiteSpace(this.extension))
                {
                    string imageFile = this.outputFile + this.currentIndex + this.extension;
                    this.currentIndex++;

                    frame.Save(imageFile);
                }
                else
                {
                    if (frame.NumberOfChannels == 1)
                    {
                        this.writer.WriteFrame((Image<Gray, Byte>)frame);
                    }
                    else
                    {
                        this.writer.WriteFrame((Image<Bgr, Byte>)frame);
                    }
                }

            }
            /// <summary>
            /// 比對相似度
            /// </summary>
            public bool isMatch(Image<Gray, Byte> f1, Image<Gray, Byte> f2)
            {
                int[] hist_size = new int[1] { 256 };//建一个数组来存放直方图数据   

                IntPtr HistImg1 = CvInvoke.cvCreateHist(1, hist_size, Emgu.CV.CvEnum.HIST_TYPE.CV_HIST_ARRAY, null, 1); //创建一个空的直方图  
                IntPtr HistImg2 = CvInvoke.cvCreateHist(1, hist_size, Emgu.CV.CvEnum.HIST_TYPE.CV_HIST_ARRAY, null, 1);
                IntPtr[] inPtr1 = new IntPtr[1] { f1 };
                IntPtr[] inPtr2 = new IntPtr[1] { f2 };
                CvInvoke.cvCalcHist(inPtr1, HistImg1, false, IntPtr.Zero); //计算inPtr1指向图像的数据，并传入HistImg1中  
                CvInvoke.cvCalcHist(inPtr2, HistImg2, false, IntPtr.Zero);

                double compareResult;

                StringBuilder result = new StringBuilder();

                CvInvoke.cvNormalizeHist(HistImg1, 1d); //直方图对比方式   
                CvInvoke.cvNormalizeHist(HistImg2, 1d);

                Emgu.CV.CvEnum.HISTOGRAM_COMP_METHOD compareMethod = Emgu.CV.CvEnum.HISTOGRAM_COMP_METHOD.CV_COMP_CORREL;
                compareResult = CvInvoke.cvCompareHist(HistImg1, HistImg2, compareMethod);

                f1.Dispose();
                f2.Dispose();
                if (compareResult >= 0.8)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            public void changeProgressBar()
            {
                int tmp = progressPercent;
                progressPercent = (currentIndex*100) / (int)this.capture.GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_COUNT);
                Console.WriteLine(progressPercent.ToString() + "%");
                if (progressPercent != tmp)
                {
                    //_aForm.label2.Text = progressPercent.ToString() + "%";
                    _aForm.progressBar1.Value = progressPercent;
                }
                
            }

            public static bool DeleteDirectory(string target_dir)
            {
                bool result = false;
                string[] files = Directory.GetFiles(target_dir);
                string[] dirs = Directory.GetDirectories(target_dir);
                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir);
                }
                Directory.Delete(target_dir, false);
                return result;
            }


            //主程式
            public void Run()
            {
                //點的文字檔
                sw.WriteLine("Ax" + " " + "Ay" + " " + "Bx" + " " + "By" + " " + "Cx" + " " + "Cy" + " " + "Dx" + " " + "Dy"+" "+"DeltaX"+" "+"DeltaY");

                //current frame
                IImage frame;
                //if no catpure device has been set
                if (!this.IsOpened)
                {
                    return;
                }
                this.stop = false;

                while (!IsStopped)
                {
                    //read next frame if any
                    frame = this.capture.QueryFrame();
                    if (frame == null)
                    {
                        StopIt();
                        break;
                    }

                    //比較
                    Image<Rgb, byte> f1 = new Image<Rgb, byte>(frame.Bitmap);
                    Image<Rgb, byte> f2 = game.Convert<Rgb, byte>();

                    if (isMatch(f1.Convert<Gray, Byte>(), f2.Convert<Gray, Byte>()))
                    {
                        //若是球場畫面則進行後續處理
                        Console.WriteLine(this.currentIndex + ":same");
                        pointz = getPoint(frame.Bitmap);
                        FindPoint();
                        //呼叫賈函數 貼圖

                        sw.WriteLine(Ax_prime + " " + Ay_prime + " " + Bx_prime + " " + By_prime + " " + Cx_prime + " " + Cy_prime + " " + Dx_prime + " " + Dy_prime + " "+getXmove()+" "+getYmove());

                        Image<Bgr, byte> frameConvert_ad = new Image<Bgr, byte>(frame.Bitmap);
                        Image<Bgr, byte> frameConvert_sub = new Image<Bgr, byte>(frame.Bitmap);
                        if(selectVideo == 1)
                        {
                           Image<Bgr, byte> game1 = new Image<Bgr, byte>(@"C:\Users\The LAB Mac\Videos\finding\need3.JPG");  //空白球場
                            ad_paste(frameConvert_ad);
                            image_subtraction(frameConvert_sub, game1);
                            frame = frameConvert_sub;
                            WriteNextFrame(frame);
                            changeProgressBar();
                            frameConvert_ad.Dispose();
                            frameConvert_sub.Dispose();
                            game1.Dispose();
                        }
                        else if (selectVideo == 2)
                        {
                           Image<Bgr, byte> game1 = new Image<Bgr, byte>(@"C:\Users\The LAB Mac\Videos\finding\need2.JPG");  //空白球場
                            ad_paste(frameConvert_ad);
                            image_subtraction(frameConvert_sub, game1);
                            frame = frameConvert_sub;
                            WriteNextFrame(frame);
                            changeProgressBar();
                            frameConvert_ad.Dispose();
                            frameConvert_sub.Dispose();
                            game1.Dispose();
                        }
                        //ad_paste(frameConvert_ad);
                        //image_subtraction(frameConvert_sub, game1);
                        //frame = frameConvert_sub;

                        //WriteNextFrame(frame);
                        //changeProgressBar();

                        //frameConvert_ad.Dispose();
                        //frameConvert_sub.Dispose();
                        //game1.Dispose();
                    }
                    else
                    {
                        //不是球場則不做處理
                        Console.WriteLine(this.currentIndex + ":not same");

                        WriteNextFrame(frame);
                        changeProgressBar();
                    }
                    f1.Dispose();
                    f2.Dispose();
                    
                    //進行圖片輸出
                    //if (!string.IsNullOrWhiteSpace(this.outputFile))
                    //{
                    //    WriteNextFrame(frame);
                    //    changeProgressBar();
                    //}

                    
                }

                //全部輸出完畢，以下進行影片合成
                //要取出fps直接呼叫GetFrameRate()就有了
                Console.WriteLine("all done.");
                
                sw.Close();

                string basePath = outputFile;  //frame的路徑
                double fps = GetFrameRate();  //取得影片fps值        
                var imagesCount = this.currentIndex; //frame的數目
                using (var videoWriter = new VideoFileWriter())
                {
                    //videoWriter.Open(basePath + "test.avi", 1280, 720, (int)fps, VideoCodec.Default, 63200000);
                    videoWriter.Open(videoSavePath, 1280, 720, (int)fps, VideoCodec.Default, 63200000); //設定輸出影片的資料
                    for (int imageFrame = 0; imageFrame < imagesCount; imageFrame++)  //將每一張frame讀進videoWriter中
                    {
                        var imgPath = string.Format("{0}{1}.jpg", basePath, imageFrame);
                        using (Bitmap image = Bitmap.FromFile(imgPath) as Bitmap)  //把frame轉成bitmap
                        {
                            videoWriter.WriteVideoFrame(image);
                        }
                        Console.WriteLine("write:" + imageFrame);
                    }
                    videoWriter.Close();
                }
                DeleteDirectory(basePath);
                MessageBox.Show("輸出成功");
                //結束

            }


            public Point[] getPoint(Bitmap b1)
            {
                List<PointF> listPoint = new List<PointF>();
                PointF[] convertPoint = new PointF[35];
                Point[] outputPoint = new Point[35];
                Point[] find4Point = new Point[4];
                Bitmap a1 = new Bitmap(1280, 460);
                Graphics g1 = Graphics.FromImage(a1);
                g1.Clear(Color.Black);
                g1.DrawImage(b1, new Rectangle(0, -130, b1.Width, b1.Height));



                Image<Gray, Byte> image2 = new Image<Gray, Byte>(a1);
                //Hough transform for line detection
                LineSegment2D[][] lines = image2.HoughLines(
                    new Gray(165),  //Canny algorithm low threshold
                    new Gray(260),  //Canny algorithm high threshold
                    1,              //rho parameter
                    Math.PI / 180.0,  //theta parameter 
                    100,            //threshold
                    100,             //min length for a line
                    300);            //max allowed gap along the line
                                     //draw lines on image
                foreach (var line in lines[0])
                {
                    if (line.Direction.Y == 0)
                    {
                        image2.Draw(line, new Gray(0), 2);
                    }
                    else if (line.Direction.Y > 0.8 || line.Direction.Y < -0.8)
                    {
                        image2.Draw(line, new Gray(0), 2);
                    }

                }
                Bitmap a3 = new Bitmap(1280,460);
                Graphics g3 = Graphics.FromImage(a3);
                g3.Clear(Color.Gray);
                Image<Gray, Byte> image3 = new Image<Gray, Byte>(a3);
                foreach (var line in lines[0])
                {
                    if (line.Direction.Y == 0 && line.Length > 400)
                    {
                        image3.Draw(line, new Gray(255), 2);
                    }
                    else if (line.Direction.Y > 0.8 || line.Direction.Y < -0.8)
                    {
                        image3.Draw(line, new Gray(255), 2);
                    }
                }
                 PointF[][] points3 = image3.GoodFeaturesToTrack(
                        35,  //maximum number of corners to be returned
                        0.01, //quality level
                        50,   //minimum allowed distance between points 
                        3   //size of the averaging block
                        ); //for all corner


                foreach (PointF point in points3[0].OrderBy(p => p.Y))
                {
                    listPoint.Add(point);
                    //CircleF circle = new CircleF(point, 3);
                    //image3.Draw(circle, new Gray(0), 2);


                }
                listPoint.CopyTo(convertPoint);
                listPoint.Clear();
                allX = 0;
                for (int i = 0; i < convertPoint.Length; i++)
                {
                    outputPoint[i].X = Convert.ToInt32(convertPoint[i].X);
                    outputPoint[i].Y = Convert.ToInt32(convertPoint[i].Y)+20;
                }
                if (countP == 0)
                {
                    if (selectVideo == 1)
                    {
                        tempA[0] = pointSd[5];
                        tempB[0] = pointSd[6];
                        tempC[0] = pointSd[28];
                        tempD[0] = pointSd[25];
                    }
                    else if (selectVideo == 2)
                    {
                        tempA[0] = pointSd[5];
                        tempB[0] = pointSd[2];
                        tempC[0] = pointSd[25];
                        tempD[0] = pointSd[28];
                    }
                }
                for (int i = 0; i < outputPoint.Length; i++)//左上
                {
                    if ((outputPoint[i].X - tempA[0].X) * (outputPoint[i].X - tempA[0].X) + (outputPoint[i].Y - tempA[0].Y) * (outputPoint[i].Y - tempA[0].Y) <169)
                    {
                        find4Point[0] = outputPoint[i];
                        tempA[0] = outputPoint[i];
                    }
                }
                if (find4Point[0].IsEmpty)
                {
                    find4Point[0] = tempA[0];
                }
                for (int i = 0; i < outputPoint.Length; i++)//右上
                {
                    if ((outputPoint[i].X - tempB[0].X) * (outputPoint[i].X - tempB[0].X) + (outputPoint[i].Y - tempB[0].Y) * (outputPoint[i].Y - tempB[0].Y) <169)
                    {
                        find4Point[1] = outputPoint[i];
                        tempB[0] = outputPoint[i];
                    }
                }
                if (find4Point[1].IsEmpty)
                {
                    find4Point[1] = tempB[0];
                }
                for (int i = 0; i < outputPoint.Length; i++)//左下
                {
                    if ((outputPoint[i].X - tempC[0].X) * (outputPoint[i].X - tempC[0].X) + (outputPoint[i].Y - tempC[0].Y) * (outputPoint[i].Y - tempC[0].Y) < 169)
                    {
                        find4Point[2] = outputPoint[i];
                        tempC[0] = outputPoint[i];
                    }
                }
                if (find4Point[2].IsEmpty)
                {
                    find4Point[2] = tempC[0];
                }
                for (int i = 0; i < outputPoint.Length; i++)//右下
                {
                    if ((outputPoint[i].X - tempD[0].X) * (outputPoint[i].X - tempD[0].X) + (outputPoint[i].Y - tempD[0].Y) * (outputPoint[i].Y - tempD[0].Y) < 169)
                    {
                        find4Point[3] = outputPoint[i];
                        tempD[0] = outputPoint[i];
                    }
                }
                if (find4Point[3].IsEmpty)
                {
                    find4Point[3] = tempD[0];
                }
                countP += 1;
                CircleF circleA = new CircleF(new Point(tempA[0].X, tempA[0].Y - 130), 3);
                image3.Draw(circleA, new Gray(0), 2);
                CircleF circleB = new CircleF(new Point(tempB[0].X, tempB[0].Y - 130), 3);
                image3.Draw(circleB, new Gray(0), 2);
                CircleF circleC = new CircleF(new Point(tempC[0].X, tempC[0].Y - 130), 3);
                image3.Draw(circleC, new Gray(0), 2);
                CircleF circleD = new CircleF(new Point(tempD[0].X, tempD[0].Y - 130), 3);
                image3.Draw(circleD, new Gray(0), 2);
                image3.Save(@"C://Users/The LAB Mac/Pictures/checkPoint" + countP + ".JPG");
                Console.WriteLine(find4Point[0] + "/" + find4Point[1] + "/" + find4Point[2] + "/" + find4Point[3]);
                image2.Dispose();
                a3.Dispose();
                a1.Dispose();
                image3.Dispose();
                g1.Dispose();
                g3.Dispose();
                return find4Point;

            }

            //public void inputPoint()
            //{
            //    for (int i = 0; i <= pointz.Length - 1; i++)
            //    {
            //        for (int j = 0; j <= 0; j++)
            //        {
            //            array2D[i, j] = pointz[i].X;  //[0,0]  把X塞進指定項
            //            array2Db[i, j] = pointSd[i].X;
            //        }

            //    }
            //    for (int i = 0; i <= pointz.Length - 1; i++)
            //    {
            //        for (int j = 1; j <= 1; j++)
            //        {

            //            array2D[i, j] = pointz[i].Y; //[0,1]    把Y塞進指定項
            //            array2Db[i, j] = pointSd[i].Y;

            //        }

            //    }
            //    for (int i = 0; i <= pointz.Length - 1; i++)
            //    {
            //        for (int j = 2; j <= 2; j++)
            //        {
            //            array2D[i, j] = 1; //[0,2]       Z項全部是1
            //            array2Db[i, j] = 1;
            //        }

            //    }


            //    array2D2  = array2D;
            //    array2D2b = array2Db;


            //}
            ////60*8
            //public void do8Matrix()
            //{

            //    for (int i = 0; i <= 58; i += 2)
            //    {
            //        for (int j = 0; j <= 5; j++)
            //        {
            //            if (j <= 2)
            //            {
            //                arrayequation[i, j] = array2D2b[i / 2, j];
            //            }else
            //            {
            //                arrayequation[i, j] = 0;
            //            }
            //        }

            //    }
            //    for (int i = 1; i <= 59; i += 2)
            //    {
            //        for (int j = 0; j <= 5; j++)
            //        {
            //            if (j <= 2)
            //            {
            //                arrayequation[i, j] = 0;
            //            }
            //            else
            //            {
            //                arrayequation[i, j] = array2D2b[(i - 1) / 2, j - 3];
            //            }

            //        }


            //    }
            //    for (int i = 0; i <= 58; i += 2)
            //    {
            //        for (int j = 6; j <= 7; j++)
            //        {
            //            if (j == 6)
            //            {
            //                arrayequation[i, j] = -array2D2b[i / 2, 0] * array2D2[i / 2, 0];
            //            }
            //            else
            //            {
            //                arrayequation[i, j] = -array2D2b[i / 2, 1] * array2D2[i / 2, 0];
            //            }


            //        }

            //    }
            //    for (int i = 1; i <= 59; i += 2)
            //    {
            //        for (int j = 6; j <= 7; j++)
            //        {
            //            if (j == 6)
            //            {
            //                arrayequation[i, j] = -array2D2b[(i - 1) / 2, 0] * array2D2[(i - 1) / 2, 1];
            //            }
            //            else
            //            {
            //                arrayequation[i, j] = -array2D2b[(i - 1) / 2, 1] * array2D2[(i - 1) / 2, 1];
            //            }


            //        }

            //    }
            //    arrayequation2 = arrayequation;


            //}
            ////A' 8*60
            //public void transferMatrix()
            //{
            //    for (int i = 0; i <= 59; i++)
            //    {
            //        for (int j = 0; j <= 7; j++)
            //        {
            //            arrayequationtrans[j, i] = arrayequation2[i, j];

            //        }

            //    }


            //}
            ////A'A 8*8 
            //public void multipleTranserMatrix()
            //{
            //    for (int i = 0; i <= 7; i++)
            //    {
            //        for (int j = 0; j <= 7; j++)
            //        {

            //            arraymul[i, j] = arrayequationtrans[i, 0] * arrayequation2[0, j]
            //                           + arrayequationtrans[i, 1] * arrayequation2[1, j]
            //                           + arrayequationtrans[i, 2] * arrayequation2[2, j]
            //                           + arrayequationtrans[i, 3] * arrayequation2[3, j]
            //                           + arrayequationtrans[i, 4] * arrayequation2[4, j]
            //                           + arrayequationtrans[i, 5] * arrayequation2[5, j]
            //                           + arrayequationtrans[i, 6] * arrayequation2[6, j]
            //                           + arrayequationtrans[i, 7] * arrayequation2[7, j]
            //                           + arrayequationtrans[i, 8] * arrayequation2[8, j]
            //                           + arrayequationtrans[i, 9] * arrayequation2[9, j]
            //                           + arrayequationtrans[i, 10] * arrayequation2[10, j]
            //                           + arrayequationtrans[i, 11] * arrayequation2[11, j]
            //                           + arrayequationtrans[i, 12] * arrayequation2[12, j]
            //                           + arrayequationtrans[i, 13] * arrayequation2[13, j]
            //                           + arrayequationtrans[i, 14] * arrayequation2[14, j]
            //                           + arrayequationtrans[i, 15] * arrayequation2[15, j]
            //                           + arrayequationtrans[i, 16] * arrayequation2[16, j]
            //                           + arrayequationtrans[i, 17] * arrayequation2[17, j]
            //                           + arrayequationtrans[i, 18] * arrayequation2[18, j]
            //                           + arrayequationtrans[i, 19] * arrayequation2[19, j]
            //                           + arrayequationtrans[i, 20] * arrayequation2[20, j]
            //                           + arrayequationtrans[i, 21] * arrayequation2[21, j]
            //                           + arrayequationtrans[i, 22] * arrayequation2[22, j]
            //                           + arrayequationtrans[i, 23] * arrayequation2[23, j]
            //                           + arrayequationtrans[i, 24] * arrayequation2[24, j]
            //                           + arrayequationtrans[i, 25] * arrayequation2[25, j]
            //                           + arrayequationtrans[i, 26] * arrayequation2[26, j]
            //                           + arrayequationtrans[i, 27] * arrayequation2[27, j]
            //                           + arrayequationtrans[i, 28] * arrayequation2[28, j]
            //                           + arrayequationtrans[i, 29] * arrayequation2[29, j]
            //                           + arrayequationtrans[i, 30] * arrayequation2[30, j]
            //                           + arrayequationtrans[i, 31] * arrayequation2[31, j]
            //                           + arrayequationtrans[i, 32] * arrayequation2[32, j]
            //                           + arrayequationtrans[i, 33] * arrayequation2[33, j]
            //                           + arrayequationtrans[i, 34] * arrayequation2[34, j]
            //                           + arrayequationtrans[i, 35] * arrayequation2[35, j]
            //                           + arrayequationtrans[i, 36] * arrayequation2[36, j]
            //                           + arrayequationtrans[i, 37] * arrayequation2[37, j]
            //                           + arrayequationtrans[i, 38] * arrayequation2[38, j]
            //                           + arrayequationtrans[i, 39] * arrayequation2[39, j]
            //                           + arrayequationtrans[i, 40] * arrayequation2[40, j]
            //                           + arrayequationtrans[i, 41] * arrayequation2[41, j]
            //                           + arrayequationtrans[i, 42] * arrayequation2[42, j]
            //                           + arrayequationtrans[i, 43] * arrayequation2[43, j]
            //                           + arrayequationtrans[i, 44] * arrayequation2[44, j]
            //                           + arrayequationtrans[i, 45] * arrayequation2[45, j]
            //                           + arrayequationtrans[i, 46] * arrayequation2[46, j]
            //                           + arrayequationtrans[i, 47] * arrayequation2[47, j]
            //                           + arrayequationtrans[i, 48] * arrayequation2[48, j]
            //                           + arrayequationtrans[i, 49] * arrayequation2[49, j]
            //                           + arrayequationtrans[i, 50] * arrayequation2[50, j]
            //                           + arrayequationtrans[i, 51] * arrayequation2[51, j]
            //                           + arrayequationtrans[i, 52] * arrayequation2[52, j]
            //                           + arrayequationtrans[i, 53] * arrayequation2[53, j]
            //                           + arrayequationtrans[i, 54] * arrayequation2[54, j]
            //                           + arrayequationtrans[i, 55] * arrayequation2[55, j]
            //                           + arrayequationtrans[i, 56] * arrayequation2[56, j]
            //                           + arrayequationtrans[i, 57] * arrayequation2[57, j]
            //                           + arrayequationtrans[i, 58] * arrayequation2[58, j]
            //                           + arrayequationtrans[i, 59] * arrayequation2[59, j];

            //        }
            //    }

            //}

            ////A'B 8*1
            //public void mutipleABMatrix()
            //{

            //    for (int j = 0; j <= 7; j++)
            //    {
            //       arraymul2[j, 0] = arrayequationtrans[j, 0] * array2D2[0, 0]
            //                       + arrayequationtrans[j, 1] * array2D2[0, 1]
            //                       + arrayequationtrans[j, 2] * array2D2[1, 0]
            //                       + arrayequationtrans[j, 3] * array2D2[1, 1]
            //                       + arrayequationtrans[j, 4] * array2D2[2, 0]
            //                       + arrayequationtrans[j, 5] * array2D2[2, 1]
            //                       + arrayequationtrans[j, 6] * array2D2[3, 0]
            //                       + arrayequationtrans[j, 7] * array2D2[3, 1]
            //                       + arrayequationtrans[j, 8] * array2D2[4, 0]
            //                       + arrayequationtrans[j, 9] * array2D2[4, 1]
            //                       + arrayequationtrans[j, 10] * array2D2[5, 0]
            //                       + arrayequationtrans[j, 11] * array2D2[5, 1]
            //                       + arrayequationtrans[j, 12] * array2D2[6, 0]
            //                       + arrayequationtrans[j, 13] * array2D2[6, 1]
            //                       + arrayequationtrans[j, 14] * array2D2[7, 0]
            //                       + arrayequationtrans[j, 15] * array2D2[7, 1]
            //                       + arrayequationtrans[j, 16] * array2D2[8, 0]
            //                       + arrayequationtrans[j, 17] * array2D2[8, 1]
            //                       + arrayequationtrans[j, 18] * array2D2[9, 0]
            //                       + arrayequationtrans[j, 19] * array2D2[9, 1]
            //                       + arrayequationtrans[j, 20] * array2D2[10, 0]
            //                       + arrayequationtrans[j, 21] * array2D2[10, 1]
            //                       + arrayequationtrans[j, 22] * array2D2[11, 0]
            //                       + arrayequationtrans[j, 23] * array2D2[11, 1]
            //                       + arrayequationtrans[j, 24] * array2D2[12, 0]
            //                       + arrayequationtrans[j, 25] * array2D2[12, 1]
            //                       + arrayequationtrans[j, 26] * array2D2[13, 0]
            //                       + arrayequationtrans[j, 27] * array2D2[13, 1]
            //                       + arrayequationtrans[j, 28] * array2D2[14, 0]
            //                       + arrayequationtrans[j, 29] * array2D2[14, 1]
            //                       + arrayequationtrans[j, 30] * array2D2[15, 0]
            //                       + arrayequationtrans[j, 31] * array2D2[15, 1]
            //                       + arrayequationtrans[j, 32] * array2D2[16, 0]
            //                       + arrayequationtrans[j, 33] * array2D2[16, 1]
            //                       + arrayequationtrans[j, 34] * array2D2[17, 0]
            //                       + arrayequationtrans[j, 35] * array2D2[17, 1]
            //                       + arrayequationtrans[j, 36] * array2D2[18, 0]
            //                       + arrayequationtrans[j, 37] * array2D2[18, 1]
            //                       + arrayequationtrans[j, 38] * array2D2[19, 0]
            //                       + arrayequationtrans[j, 39] * array2D2[19, 1]
            //                       + arrayequationtrans[j, 40] * array2D2[20, 0]
            //                       + arrayequationtrans[j, 41] * array2D2[20, 1]
            //                       + arrayequationtrans[j, 42] * array2D2[21, 0]
            //                       + arrayequationtrans[j, 43] * array2D2[21, 1]
            //                       + arrayequationtrans[j, 44] * array2D2[22, 0]
            //                       + arrayequationtrans[j, 45] * array2D2[22, 1]
            //                       + arrayequationtrans[j, 46] * array2D2[23, 0]
            //                       + arrayequationtrans[j, 47] * array2D2[23, 1]
            //                       + arrayequationtrans[j, 48] * array2D2[24, 0]
            //                       + arrayequationtrans[j, 49] * array2D2[24, 1]
            //                       + arrayequationtrans[j, 50] * array2D2[25, 0]
            //                       + arrayequationtrans[j, 51] * array2D2[25, 1]
            //                       + arrayequationtrans[j, 52] * array2D2[26, 0]
            //                       + arrayequationtrans[j, 53] * array2D2[26, 1]
            //                       + arrayequationtrans[j, 54] * array2D2[27, 0]
            //                       + arrayequationtrans[j, 55] * array2D2[27, 1]
            //                       + arrayequationtrans[j, 56] * array2D2[28, 0]
            //                       + arrayequationtrans[j, 57] * array2D2[28, 1]
            //                       + arrayequationtrans[j, 58] * array2D2[29, 0]
            //                       + arrayequationtrans[j, 59] * array2D2[29, 1]
            //                       ;


            //    }



            //}
            ////(A'A)^-1
            //public void inverseMatrix()
            //{



            //    int i, j, k;
            //    for (i = 0; i < 8; i++)
            //    {
            //        for (j = 0; j < 8; j++)
            //        {
            //            if (i == j)
            //            {
            //                b[i, j] = 1;
            //            }
            //            else
            //            {
            //                b[i, j] = 0;
            //            }
            //        }
            //    }
            //    for (i = 0; i < 8; i++)
            //    {
            //        for (j = 0; j < 8; j++)
            //        {
            //            if (j < i)
            //            {
            //                l[j, i] = 0;
            //            }

            //            else
            //            {
            //                l[j, i] = arraymul[j, i];
            //                for (k = 0; k < i; k++)
            //                {
            //                    l[j, i] = l[j, i] - l[j, k] * u[k, i];
            //                }
            //            }
            //        }
            //        for (j = 0; j < 8; j++)
            //        {
            //            if (j < i)
            //                u[i, j] = 0;
            //            else if (j == i)
            //                u[i, j] = 1;
            //            else
            //            {
            //                u[i, j] = arraymul[i, j] / l[i, i];
            //                for (k = 0; k < i; k++)
            //                {
            //                    u[i, j] = u[i, j] - ((l[i, k] * u[k, j]) / l[i, i]);
            //                }
            //            }
            //        }
            //    }
            //    for (i = 0; i < 8; i++)
            //    {
            //        for (j = 0; j < 8; j++)
            //        {
            //            for (k = 0; k < j; k++)
            //            {
            //                if (j == 0)
            //                {
            //                    b[j, i] = b[j, i];
            //                }
            //                else
            //                {
            //                    b[j, i] = b[j, i] - (l[j, k] * y[k, i]);
            //                }

            //            }
            //            y[j, i] = b[j, i] / l[j, j];

            //        }
            //    }
            //    for (i = 0; i < 8; i++)
            //    {
            //        for (j = 7; j >= 0; j--)
            //        {
            //            for (k = j + 1; k < 8; k++)
            //            {
            //                if (j == 7)
            //                {
            //                    y[j, i] = y[j, i];
            //                }
            //                else
            //                {
            //                    y[j, i] = y[j, i] - (x[k, i] * u[j, k]);
            //                }

            //            }
            //            x[j, i] = y[j, i] / u[j, j];

            //        }
            //    }

            //}

            ////(A'A)^-1*(A'B)
            //public void FindHmatrix()
            //{

            //    for (int i = 0; i < 8; i++)
            //    {
            //        arraymulH[i, 0] = x[i, 0] * arraymul2[0, 0]
            //                        + x[i, 1] * arraymul2[1, 0]
            //                        + x[i, 2] * arraymul2[2, 0]
            //                        + x[i, 3] * arraymul2[3, 0]
            //                        + x[i, 4] * arraymul2[4, 0]
            //                        + x[i, 5] * arraymul2[5, 0]
            //                        + x[i, 6] * arraymul2[6, 0]
            //                        + x[i, 7] * arraymul2[7, 0];

            //    }
            //    arraymulH[8, 0] = 1;
            //    //Console.WriteLine("H............");
            //    //for (int i = 0; i < 9; i++)
            //    //{
            //    //    Console.WriteLine(arraymulH[i, 0]);
            //    //}
            //}
            
            public void inputPoint()
            {
                
                pointSd[0] = new Point(Convert.ToInt32(fAx), Convert.ToInt32(fAy));
                pointSd[1] = new Point(Convert.ToInt32(fBx), Convert.ToInt32(fBy));
                pointSd[2] = new Point(Convert.ToInt32(fCx), Convert.ToInt32(fCy));
                pointSd[3] = new Point(Convert.ToInt32(fDx), Convert.ToInt32(fDy));

                for (int i = 0; i <= pointz.Length - 1; i++)
                {
                    for (int j = 0; j <= 0; j++)
                    {
                        array2D[i, j] = pointz[i].X;  //[0,0,0]=1  把X塞進指定項
                        array2Db[i, j] = pointSd[i].X;
                    }

                }
                for (int i = 0; i <= pointz.Length - 1; i++)
                {
                    for (int j = 1; j <= 1; j++)
                    {

                        array2D[i, j] = pointz[i].Y; //[0,0,1]=2     把Y塞進指定項
                        array2Db[i, j] = pointSd[i].Y;

                    }

                }
                for (int i = 0; i <= pointz.Length - 1; i++)
                {
                    for (int j = 2; j <= 2; j++)
                    {
                        array2D[i, j] = 1; //[0,0,2]=3       Z項全部是5
                        array2Db[i, j] = 1;
                    }

                }


                array2D2 = array2D;
                array2D2b = array2Db;


            }
            public void do8Matrix()
            {

                for (int i = 0; i <= 6; i += 2)
                {
                    for (int j = 0; j <= 5; j++)
                    {
                        if (j <= 2)
                        {
                            arrayequation[i, j] = array2D2b[i / 2, j];
                        }
                        else
                        {
                            arrayequation[i, j] = 0;
                        }

                    }

                }
                for (int i = 1; i <= 7; i += 2)
                {
                    for (int j = 0; j <= 5; j++)
                    {
                        if (j <= 2)
                        {
                            arrayequation[i, j] = 0;
                        }
                        else
                        {
                            arrayequation[i, j] = array2D2b[(i - 1) / 2, j - 3];
                        }

                    }


                }
                for (int i = 0; i <= 6; i += 2)
                {
                    for (int j = 6; j <= 7; j++)
                    {
                        if (j == 6)
                        {
                            arrayequation[i, j] = -array2D2b[i / 2, 0] * array2D2[i / 2, 0];
                        }
                        else
                        {
                            arrayequation[i, j] = -array2D2b[i / 2, 1] * array2D2[i / 2, 0];
                        }


                    }

                }
                for (int i = 1; i <= 7; i += 2)
                {
                    for (int j = 6; j <= 7; j++)
                    {
                        if (j == 6)
                        {
                            arrayequation[i, j] = -array2D2b[(i - 1) / 2, 0] * array2D2[(i - 1) / 2, 1];
                        }
                        else
                        {
                            arrayequation[i, j] = -array2D2b[(i - 1) / 2, 1] * array2D2[(i - 1) / 2, 1];
                        }


                    }

                }
                arrayequation2 = arrayequation;


            }
            public void transferMatrix()
            {
                for (int i = 0; i <= 7; i++)
                {
                    for (int j = 0; j <= 7; j++)
                    {
                        arrayequationtrans[j, i] = arrayequation2[i, j];

                    }

                }


            }
            public void multipleTranserMatrix()
            {

                for (int i = 0; i <= 7; i++)
                {
                    for (int j = 0; j <= 7; j++)
                    {

                        arraymul[i, j] = arrayequationtrans[i, 0] * arrayequation2[0, j]
                                       + arrayequationtrans[i, 1] * arrayequation2[1, j]
                                       + arrayequationtrans[i, 2] * arrayequation2[2, j]
                                       + arrayequationtrans[i, 3] * arrayequation2[3, j]
                                       + arrayequationtrans[i, 4] * arrayequation2[4, j]
                                       + arrayequationtrans[i, 5] * arrayequation2[5, j]
                                       + arrayequationtrans[i, 6] * arrayequation2[6, j]
                                       + arrayequationtrans[i, 7] * arrayequation2[7, j];
                    }
                }

            }
            public void mutipleABMatrix()
            {

                for (int j = 0; j <= 7; j++)
                {

                    arraymul2[j, 0] = arrayequationtrans[j, 0] * array2D2[0, 0]
                                    + arrayequationtrans[j, 1] * array2D2[0, 1]
                                    + arrayequationtrans[j, 2] * array2D2[1, 0]
                                    + arrayequationtrans[j, 3] * array2D2[1, 1]
                                    + arrayequationtrans[j, 4] * array2D2[2, 0]
                                    + arrayequationtrans[j, 5] * array2D2[2, 1]
                                    + arrayequationtrans[j, 6] * array2D2[3, 0]
                                    + arrayequationtrans[j, 7] * array2D2[3, 1];


                }



            }
            public void inverseMatrix()
            {



                int i, j, k;
                for (i = 0; i < 8; i++)
                {
                    for (j = 0; j < 8; j++)
                    {
                        if (i == j)
                        {
                            b[i, j] = 1;
                        }
                        else
                        {
                            b[i, j] = 0;
                        }
                    }
                }
                for (i = 0; i < 8; i++)
                {
                    for (j = 0; j < 8; j++)
                    {
                        if (j < i)
                        {
                            l[j, i] = 0;
                        }

                        else
                        {
                            l[j, i] = arraymul[j, i];
                            for (k = 0; k < i; k++)
                            {
                                l[j, i] = l[j, i] - l[j, k] * u[k, i];
                            }
                        }
                    }
                    for (j = 0; j < 8; j++)
                    {
                        if (j < i)
                            u[i, j] = 0;
                        else if (j == i)
                            u[i, j] = 1;
                        else
                        {
                            u[i, j] = arraymul[i, j] / l[i, i];
                            for (k = 0; k < i; k++)
                            {
                                u[i, j] = u[i, j] - ((l[i, k] * u[k, j]) / l[i, i]);
                            }
                        }
                    }
                }
                for (i = 0; i < 8; i++)
                {
                    for (j = 0; j < 8; j++)
                    {
                        for (k = 0; k < j; k++)
                        {
                            if (j == 0)
                            {
                                b[j, i] = b[j, i];
                            }
                            else
                            {
                                b[j, i] = b[j, i] - (l[j, k] * y[k, i]);
                            }

                        }
                        y[j, i] = b[j, i] / l[j, j];

                    }
                }
                for (i = 0; i < 8; i++)
                {
                    for (j = 7; j >= 0; j--)
                    {
                        for (k = j + 1; k < 8; k++)
                        {
                            if (j == 7)
                            {
                                y[j, i] = y[j, i];
                            }
                            else
                            {
                                y[j, i] = y[j, i] - (x[k, i] * u[j, k]);
                            }

                        }
                        x[j, i] = y[j, i] / u[j, j];

                    }
                }

            }
            public void FindHmatrix()
            {

                for (int i = 0; i < 8; i++)
                {
                    arraymulH[i, 0] = x[i, 0] * arraymul2[0, 0]
                                    + x[i, 1] * arraymul2[1, 0]
                                    + x[i, 2] * arraymul2[2, 0]
                                    + x[i, 3] * arraymul2[3, 0]
                                    + x[i, 4] * arraymul2[4, 0]
                                    + x[i, 5] * arraymul2[5, 0]
                                    + x[i, 6] * arraymul2[6, 0]
                                    + x[i, 7] * arraymul2[7, 0];

                }
                arraymulH[8, 0] = 1;
                //Console.WriteLine("H............");
                //for (int i = 0; i < 9; i++)
                //{
                //    Console.WriteLine(arraymulH[i, 0]);
                //}
            }
            int[,] array2D = new int[100, 100];//寫死
            int[,] array2Db = new int[100, 100];//寫死



            int[,] array2D2 = new int[,] { };
            int[,] array2D2b = new int[,] { };
            float[,] arrayequationtrans = new float[8, 8];//row*column//寫死
            float[,] arraymul = new float[8, 8]; //do the mutiple A'A
            float[,] arraymul2 = new float[8, 1]; //do the A'B
            
            float[,] u = new float[8, 8];
            float[,] l = new float[8, 8];
            float[,] y = new float[8, 8];
            float[,] x = new float[8, 8];
            float[,] b = new float[8, 8];  //求(A'A)^-1
            float[,] arraymulH = new float[9, 1]; //H=(A'A)^-1*(A'B)
                                                  //Do the 8*8 matrix
            int[,] arrayequation = new int[8, 8];//寫死
            int[,] arrayequation2 = new int[,] { }; //




            float Ax_prime, Bx_prime, Cx_prime, Dx_prime, Ay_prime, By_prime, Cy_prime, Dy_prime;
            float fAx_prime, fBx_prime, fCx_prime, fDx_prime, fAy_prime, fBy_prime, fCy_prime, fDy_prime;


           
            public void outputPoint(float Ax, float Ay, float Bx, float By, float Cx, float Cy, float Dx, float Dy, float fAx, float fAy, float fBx, float fBy, float fCx, float fCy, float fDx, float fDy)
            {
               
                //要除掉的常數
                float constant = arraymulH[6, 0]* Ax + arraymulH[7, 0]* Ay + 1;
                //把廣告的四個點拿過來做變形   //x y
                int resultXmove = 0;
                resultXmove = (pointz[0].X + pointz[1].X - Convert.ToInt32(fAx) - Convert.ToInt32(fBx)) / 2;
                if (resultXmove > 20)
                {
                    Ax_prime = (Ax * arraymulH[0, 0] + Ay * arraymulH[1, 0] + 1 * arraymulH[2, 0]) / constant;
                    Ay_prime = (Ax * arraymulH[3, 0] + Ay * arraymulH[4, 0] + 1 * arraymulH[5, 0]) / constant;
                    Bx_prime = (Bx * arraymulH[0, 0] + Bx * arraymulH[1, 0] + 1 * arraymulH[2, 0]) / constant;
                    By_prime = (By * arraymulH[3, 0] + By * arraymulH[4, 0] + 1 * arraymulH[5, 0]) / constant;
                    Cx_prime = (Cx * arraymulH[0, 0] + Cx * arraymulH[1, 0] + 1 * arraymulH[2, 0]) / constant;
                    Cy_prime = (Cy * arraymulH[3, 0] + Cy * arraymulH[4, 0] + 1 * arraymulH[5, 0]) / constant;
                    Dx_prime = (Dx * arraymulH[0, 0] + Dx * arraymulH[1, 0] + 1 * arraymulH[2, 0]) / constant;
                    Dy_prime = (Dy * arraymulH[3, 0] + Dy * arraymulH[4, 0] + 1 * arraymulH[5, 0]) / constant;

                    Ax_prime = (Ax_prime - Ax) / 5 + Ax;
                    Ay_prime = (Ay_prime - Ay) / 5 + Ay;
                    Bx_prime = (Bx_prime - Bx) / 5 + Bx;
                    By_prime = (By_prime - By) / 5 + By;
                    Cx_prime = (Cx_prime - Cx) / 5 + Cx;
                    Cy_prime = (Cy_prime - Cy) / 5 + Cy;
                    Dx_prime = (Dx_prime - Dx) / 5 + Dx;
                    Dy_prime = (Dy_prime - Dy) / 5 + Dy;

                    fAx_prime = (fAx * arraymulH[0, 0] + fAy * arraymulH[1, 0] + 1 * arraymulH[2, 0]) / constant;
                    fAy_prime = (fAx * arraymulH[3, 0] + fAy * arraymulH[4, 0] + 1 * arraymulH[5, 0]) / constant;
                    fBx_prime = (fBx * arraymulH[0, 0] + fBy * arraymulH[1, 0] + 1 * arraymulH[2, 0]) / constant;
                    fBy_prime = (fBx * arraymulH[3, 0] + fBy * arraymulH[4, 0] + 1 * arraymulH[5, 0]) / constant;
                    fCx_prime = (fCx * arraymulH[0, 0] + fCy * arraymulH[1, 0] + 1 * arraymulH[2, 0]) / constant;
                    fCy_prime = (fCx * arraymulH[3, 0] + fCy * arraymulH[4, 0] + 1 * arraymulH[5, 0]) / constant;
                    fDx_prime = (fDx * arraymulH[0, 0] + fDy * arraymulH[1, 0] + 1 * arraymulH[2, 0]) / constant;
                    fDy_prime = (fDx * arraymulH[3, 0] + fDy * arraymulH[4, 0] + 1 * arraymulH[5, 0]) / constant;

                    fAx_prime = (fAx_prime - fAx) / 5 + fAx;
                    fAy_prime = (fAy_prime - fAy) / 5 + fAy;
                    fBx_prime = (fBx_prime - fBx) / 5 + fBx;
                    fBy_prime = (fBy_prime - fBy) / 5 + fBy;
                    fCx_prime = (fCx_prime - fCx) / 5 + fCx;
                    fCy_prime = (fCy_prime - fCy) / 5 + fCy;
                    fDx_prime = (fDx_prime - fDx) / 5 + fDx;
                    fDy_prime = (fDy_prime - fDy) / 5 + fDy;
                }
                else if (resultXmove <= 20)
                {
                    //把空白球場的四個點拿過來做變形   //y y
                    Ax_prime = (Ax * arraymulH[0, 0] + Ay * arraymulH[1, 0] + 1 * arraymulH[2, 0]) / constant;
                    Ay_prime = (Ax * arraymulH[3, 0] + Ay * arraymulH[4, 0] + 1 * arraymulH[5, 0]) / constant;
                    Bx_prime = (Bx * arraymulH[0, 0] + Bx * arraymulH[1, 0] + 1 * arraymulH[2, 0]) / constant;
                    By_prime = (By * arraymulH[3, 0] + By * arraymulH[4, 0] + 1 * arraymulH[5, 0]) / constant;
                    Cx_prime = (Cx * arraymulH[0, 0] + Cx * arraymulH[1, 0] + 1 * arraymulH[2, 0]) / constant;
                    Cy_prime = (Cy * arraymulH[3, 0] + Cy * arraymulH[4, 0] + 1 * arraymulH[5, 0]) / constant;
                    Dx_prime = (Dx * arraymulH[0, 0] + Dx * arraymulH[1, 0] + 1 * arraymulH[2, 0]) / constant;
                    Dy_prime = (Dy * arraymulH[3, 0] + Dy * arraymulH[4, 0] + 1 * arraymulH[5, 0]) / constant;

                    Ax_prime = (Ax_prime - Ax) / 15 + Ax;
                    Ay_prime = (Ay_prime - Ay) / 15 + Ay;
                    Bx_prime = (Bx_prime - Bx) / 15 + Bx;
                    By_prime = (By_prime - By) / 15 + By;
                    Cx_prime = (Cx_prime - Cx) / 15 + Cx;
                    Cy_prime = (Cy_prime - Cy) / 15 + Cy;
                    Dx_prime = (Dx_prime - Dx) / 15 + Dx;
                    Dy_prime = (Dy_prime - Dy) / 15 + Dy;

                    fAx_prime = (fAx * arraymulH[0, 0] + fAy * arraymulH[1, 0] + 1 * arraymulH[2, 0]) / constant;
                    fAy_prime = (fAx * arraymulH[3, 0] + fAy * arraymulH[4, 0] + 1 * arraymulH[5, 0]) / constant;
                    fBx_prime = (fBx * arraymulH[0, 0] + fBy * arraymulH[1, 0] + 1 * arraymulH[2, 0]) / constant;
                    fBy_prime = (fBx * arraymulH[3, 0] + fBy * arraymulH[4, 0] + 1 * arraymulH[5, 0]) / constant;
                    fCx_prime = (fCx * arraymulH[0, 0] + fCy * arraymulH[1, 0] + 1 * arraymulH[2, 0]) / constant;
                    fCy_prime = (fCx * arraymulH[3, 0] + fCy * arraymulH[4, 0] + 1 * arraymulH[5, 0]) / constant;
                    fDx_prime = (fDx * arraymulH[0, 0] + fDy * arraymulH[1, 0] + 1 * arraymulH[2, 0]) / constant;
                    fDy_prime = (fDx * arraymulH[3, 0] + fDy * arraymulH[4, 0] + 1 * arraymulH[5, 0]) / constant;

                    fAx_prime = (fAx_prime - fAx) / 15 + fAx;
                    fAy_prime = (fAy_prime - fAy) / 15 + fAy;
                    fBx_prime = (fBx_prime - fBx) / 15 + fBx;
                    fBy_prime = (fBy_prime - fBy) / 15 + fBy;
                    fCx_prime = (fCx_prime - fCx) / 15 + fCx;
                    fCy_prime = (fCy_prime - fCy) / 15 + fCy;
                    fDx_prime = (fDx_prime - fDx) / 15 + fDx;
                    fDy_prime = (fDy_prime - fDy) / 15 + fDy;
                }



                Console.WriteLine(Ax +"/"+Ay+"/"+Ax_prime+"/"+Ay_prime);
                Console.WriteLine(Bx + "/" + By + "/" + Bx_prime + "/" + By_prime);
                Console.WriteLine(Cx + "/" + Cy + "/" + Cx_prime + "/" + Cy_prime);
                Console.WriteLine(Dx + "/" + Dy + "/" + Dx_prime + "/" + Dy_prime);

                Console.WriteLine(fAx + "/" + fAy + "/" + fAx_prime + "/" + fAy_prime);
                Console.WriteLine(fBx + "/" + fBy + "/" + fBx_prime + "/" + fBy_prime);
                Console.WriteLine(fCx + "/" + fCy + "/" + fCx_prime + "/" + fCy_prime);
                Console.WriteLine(fDx + "/" + fDy + "/" + fDx_prime + "/" + fDy_prime);
                //Console.WriteLine("H............");
                //for (int i = 0; i < 8; i++)
                //{
                //    for (int j = 0; j < 8; j++)
                //    {
                //        Console.WriteLine(arraymul[i, j]);
                //    }

                //}

                //if(Math.Abs(Ax-Ax_prime)<50 && Math.Abs(Ay - Ay_prime) < 50)
                //{
                //    //Console.WriteLine("H............");
                //    //for (int i = 0; i < 9; i++)
                //    //{
                //    //    Console.WriteLine(arraymulH[i, 0]);
                //    //}
                //    Console.WriteLine(Ax+"/"+Ay+"/"+Ax_prime+"/"+Ay_prime);
                //}


            }

            
            public void FindPoint()
            {
                inputPoint();
                do8Matrix();
                transferMatrix();
                multipleTranserMatrix();
                mutipleABMatrix();
                inverseMatrix();
                FindHmatrix();
                outputPoint(Ax, Ay, Bx, By, Cx, Cy, Dx, Dy, fAx, fAy, fBx, fBy, fCx, fCy, fDx, fDy);
            }


            /*private void VideoWrite()  //frame合成影片的function
            {
                const string basePath = @"C:\hist\";  //frame的路徑

                double fps = 20;  //取得影片fps值        

                var imagesCount = 1015; //frame的數目

                using (var videoWriter = new VideoFileWriter())
                {
                    videoWriter.Open(basePath + "test.avi", 1015, 573, (int)fps, VideoCodec.Default, 1000000);  //設定輸出影片的資料

                    for (int imageFrame = 0; imageFrame < imagesCount; imageFrame++)  //將每一張frame讀進videoWriter中
                    {
                        var imgPath = string.Format("{0}{1}.jpg", basePath, imageFrame);

                        using (Bitmap image = Bitmap.FromFile(imgPath) as Bitmap)  //把frame轉成bitmap
                        {
                            videoWriter.WriteVideoFrame(image);
                        }
                        Console.WriteLine(imageFrame);
                    }
                    videoWriter.Close();
                }
            }*/


            private void image_subtraction(Image<Bgr, byte> frame, Image<Bgr, byte> game1) //球場變形 & 前景移除 & 完成frame 的function
            {
                ////if (selectVideo == 1)
                ////{
                //    Image<Bgr, byte> game1 = new Image<Bgr, byte>(@"C:\Users\The LAB Mac\Videos\finding\need3.JPG");  //空白球場
                ////}
                ////else if (selectVideo == 2)
                ////{
                ////    Image<Bgr, byte> game1 = new Image<Bgr, byte>(@"C:\Users\The LAB Mac\Videos\finding\need2.JPG");  //空白球場
                ////}
                //球場變形
                game_image[0] = new PointF(fAx, fAy);
                game_image[1] = new PointF(fBx, fBy);
                game_image[2] = new PointF(fCx, fCy);
                game_image[3] = new PointF(fDx, fDy);

                //gameTrans_image[0] = new PointF(fAx_prime, fAy_prime);
                //gameTrans_image[1] = new PointF(fBx_prime, fBy_prime);
                //gameTrans_image[2] = new PointF(fCx_prime, fCy_prime);
                //gameTrans_image[3] = new PointF(fDx_prime, fDy_prime);

                gameTrans_image[0] = new PointF(fAx + (float)getXmove(), fAy + (float)getYmove());
                gameTrans_image[1] = new PointF(fBx + (float)getXmove(), fBy + (float)getYmove());
                gameTrans_image[2] = new PointF(fCx + (float)getXmove(), fCy + (float)getYmove());
                gameTrans_image[3] = new PointF(fDx + (float)getXmove(), fDy + (float)getYmove());

                HomographyMatrix mywrapmat = CameraCalibration.GetPerspectiveTransform(game_image, gameTrans_image);
                Image<Bgr, byte> gameTrans = game1.WarpPerspective(mywrapmat, Emgu.CV.CvEnum.INTER.CV_INTER_NN, Emgu.CV.CvEnum.WARP.CV_WARP_FILL_OUTLIERS, new Bgr(0, 0, 0));

                //前景移除
                size = CvInvoke.cvGetSize(frame);
                player = new Image<Bgr, byte>(size);
                player = frame;  //player讀入球場frame的圖
                player = player.AbsDiff(gameTrans);   //player與gameTrans做相減
                //player = player.ThresholdBinary(new Bgr(threshold, threshold, threshold), new Bgr(255, 255, 255));  //player是不同部份

                for (int i = 0; i < size.Height; i++)
                {
                    for (int j = 0; j < size.Width; j++)
                    {
                        binary_value = 255;
                        range = (int)(((int)player.Data[i, j, 0] + (int)player.Data[i, j, 1] + (int)player.Data[i, j, 2]) / 3);  //差值
                        if (range > threshold) //不相同
                        {
                            player.Data[i, j, 0] = frame.Data[i, j, 0];
                            player.Data[i, j, 1] = frame.Data[i, j, 1];
                            player.Data[i, j, 2] = frame.Data[i, j, 2];

                            ad_frame.Data[i, j, 0] = player.Data[i, j, 0];
                            ad_frame.Data[i, j, 1] = player.Data[i, j, 1];
                            ad_frame.Data[i, j, 2] = player.Data[i, j, 2];
                        }
                    }
                }
                CvInvoke.cvCopy(ad_frame, frame, IntPtr.Zero); //把ad_frame複製到frame上
                player.Dispose();
                gameTrans.Dispose();
                //game1.Dispose();
                ad_frame.Dispose();
            }






            private void ad_paste(Image<Bgr, byte> frame)  //廣告變形並貼上的function
            {
                ad_original = new Image<Bgr, byte> (ad_filename_F3);
                Image<Bgr, byte> ad = ad_original.Resize(1280, 720, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
                size = CvInvoke.cvGetSize(frame);
                ad_size = CvInvoke.cvGetSize(ad);

                ad_frame = frame;

                ad_image[0] = new PointF(0, 0); //原始廣告的點一
                ad_image[1] = new PointF(ad.Width, 0); //原始廣告的點二
                ad_image[2] = new PointF(0, ad.Height); //原始廣告的點三
                ad_image[3] = new PointF(ad.Width, ad.Height); //原始廣告的點四

                //result_image[0] = new PointF(Ax_prime, Ay_prime); //點一的對應點
                //result_image[1] = new PointF(Bx_prime, By_prime); //點二的對應點
                //result_image[2] = new PointF(Cx_prime, Cy_prime); //點三的對應點
                //result_image[3] = new PointF(Dx_prime, Dy_prime); //點四的對應點
                if (selectVideo == 1)
                {
                    result_image[0] = new PointF(Ax - 10 + (float)getXmove(), Ay + 10 + (float)getYmove()); //點一的對應點
                    result_image[1] = new PointF(Bx - 10 + (float)getXmove(), By + 5 + (float)getYmove()); //點二的對應點
                    result_image[2] = new PointF(Cx - 10 + (float)getXmove(), Cy + 10 + (float)getYmove()); //點三的對應點
                    result_image[3] = new PointF(Dx - 10 + (float)getXmove(), Dy + 10 + (float)getYmove()); //點四的對應點
                }
                else if (selectVideo == 2)
                {
                    result_image[0] = new PointF(Ax  + (float)getXmove(), Ay  + (float)getYmove()); //點一的對應點
                    result_image[1] = new PointF(Bx  + (float)getXmove(), By  + (float)getYmove()); //點二的對應點
                    result_image[2] = new PointF(Cx  + (float)getXmove(), Cy  + (float)getYmove()); //點三的對應點
                    result_image[3] = new PointF(Dx  + (float)getXmove(), Dy  + (float)getYmove()); //點四的對應點
                }


                HomographyMatrix mywrapmat = CameraCalibration.GetPerspectiveTransform(ad_image, result_image); //原始點與對應點轉換
                Image<Bgr, byte> adTrans = ad.WarpPerspective(mywrapmat, Emgu.CV.CvEnum.INTER.CV_INTER_NN, Emgu.CV.CvEnum.WARP.CV_WARP_FILL_OUTLIERS, new Bgr(0, 0, 0)); //adTrans是廣告變形完的結果

                for (int i = 0; i < adTrans.Height; i++)
                {
                    for (int j = 0; j < adTrans.Width; j++)
                    {
                        double r = adTrans.Data[i, j, 0]; //adTrans的紅色
                        double g = adTrans.Data[i, j, 1]; //adTrans的綠色
                        double b = adTrans.Data[i, j, 2]; //adTrans的藍色

                        if (r == 0 && g == 0 && b == 0)  //讓育新昏倒的地方 
                        {
                            ad_frame.Data[i, j, 0] = ad_frame.Data[i, j, 0];
                            ad_frame.Data[i, j, 1] = ad_frame.Data[i, j, 1];
                            ad_frame.Data[i, j, 2] = ad_frame.Data[i, j, 2];
                        }
                        else
                        {
                            adTransB = adTrans.Data[i, j, 0] * adAlpha;
                            adTransG = adTrans.Data[i, j, 1] * adAlpha;
                            adTransR = adTrans.Data[i, j, 2] * adAlpha;
                            adframeB = ad_frame.Data[i, j, 0] * gameAlpha;
                            adframeG = ad_frame.Data[i, j, 1] * gameAlpha;
                            adframeR = ad_frame.Data[i, j, 2] * gameAlpha;

                            //adTransB = adTrans.Data[i, j, 0] * 0.70;
                            //adTransG = adTrans.Data[i, j, 1] * 0.70;
                            //adTransR = adTrans.Data[i, j, 2] * 0.70;
                            //adframeB = ad_frame.Data[i, j, 0] * 0.30;
                            //adframeG = ad_frame.Data[i, j, 1] * 0.30;
                            //adframeR = ad_frame.Data[i, j, 2] * 0.30;

                            ad_frame.Data[i, j, 0] = (byte)(adTransB + adframeB);
                            ad_frame.Data[i, j, 1] = (byte)(adTransG + adframeG);
                            ad_frame.Data[i, j, 2] = (byte)(adTransR + adframeR);
                        }
                    }
                }
                ad_original.Dispose();
                ad.Dispose();
                adTrans.Dispose();
            }


        }
        


        
    }
}
