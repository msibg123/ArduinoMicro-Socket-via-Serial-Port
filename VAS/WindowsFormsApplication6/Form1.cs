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
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;


namespace WindowsFormsApplication6
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Console.WriteLine(Application.StartupPath);
            Console.WriteLine(Application.StartupPath + @"\File\need3.JPG");
        }
      
        

        private void button3_Click(object sender, EventArgs e)  //下一步
        {
            Form2 f2 = new Form2(pointax,pointay,pointbx,pointby,pointcx,pointcy,pointdx,pointdy, emptyFrame, ad_filename,videoFileName,fpointax, fpointay, fpointbx, fpointby, fpointcx, fpointcy, fpointdx, fpointdy,fpoint,selectVideo);//產生Form2的物件，才可以使用它所提供的Method

           //將Form1隱藏。由於在Form1的程式碼內使用this，所以this為Form1的物件本身
            f2.Visible = true;//顯示第二個視窗
        }

        //Image<Bgra,byte> bbb;
        String ad_filename;
        int selectVideo;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile = new OpenFileDialog();
            if (OpenFile.ShowDialog(this) == DialogResult.OK)
            {
                ad_filename = OpenFile.FileName;
                pictureBox1.ImageLocation = OpenFile.FileName;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
           
        }
        public void add()
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("choice", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);
            dc = new DataColumn("choiceVal", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);

            string[] strChoice = { "1", "2", "3"};
            string[] strChoiceValue = {  "1", "2", "3"};
            for (int i = 0; i < strChoice.Length; i++)
            {
                DataRow dr = dt.NewRow();
                dr["choice"] = strChoice[i];
                dr["choiceVal"] = strChoiceValue[i];
                dt.Rows.Add(dr);
            }
            comboBox2.DisplayMember = "choice";
            comboBox2.ValueMember = "choiceVal";
            comboBox2.DataSource = dt;
         
        }


        public void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {

            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile = new OpenFileDialog();
            if (OpenFile.ShowDialog(this) == DialogResult.OK)
            {
                add();

                emptyFrame = Image.FromFile(OpenFile.FileName);
                //emptyFrameView = (Image)emptyFrame.Clone();
                pictureBox2.Image = new Bitmap(emptyFrame);
                if (selectVideo == 1)
                {
                    drawNumber1((Bitmap)pictureBox2.Image);
                }
                else if (selectVideo == 2)
                {
                    drawNumber2((Bitmap)pictureBox2.Image);
                }
            }
            //pictureBox2.Image = new Bitmap(@"C://Users/The LAB Mac/Videos/finding/need1.JPG");
            //emptyFrame = pictureBox2.Image;

        }
       
        private void button2_Click_1(object sender, EventArgs e)
        {
            //pictureBox2.Image = emptyFrame;
            //drawNumber((Bitmap)pictureBox2.Image);
            emptyFrameView = (Image)emptyFrame.Clone();
            if (selectVideo == 1)
            {
                drawNumber1((Bitmap)emptyFrameView);
            }
            else if (selectVideo == 2)
            {
                drawNumber2((Bitmap)emptyFrameView);
            }
            pictureBox2.Image = emptyFrameView;
            Console.WriteLine(pointax + "/" + pointay + "/" + pointbx + "/" + pointby + "/" + pointcx + "/" + pointcy + "/" + pointdx + "/" + pointdy);
        }


        public void drawNumber1(Bitmap b1)
        {
            List<PointF> listPoint = new List<PointF>();
            PointF[] convertPoint = new PointF[30];
            Point[] outputPoint = new Point[30];
            Bitmap a1 = new Bitmap(930, 460);
            Graphics g1 = Graphics.FromImage(a1);
            g1.Clear(Color.Black);
            g1.DrawImage(b1, new Rectangle(-180, -130, b1.Width, b1.Height));


            Image<Gray, Byte> image1 = new Image<Gray, Byte>(a1);


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
                    image2.Draw(line, new Gray(0), 1);
                }
                else if (line.Direction.Y > 0.8 || line.Direction.Y < -0.8)
                {
                    image2.Draw(line, new Gray(0), 1);
                }
                //image2.Draw(line, new Gray(0), 1);
            }
            Bitmap a3 = new Bitmap(930, 460);
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
                //image3.Draw(line, new Gray(255), 2);
            }
            PointF[][] points3 = image3.GoodFeaturesToTrack(
                    29,  //maximum number of corners to be returned
                    0.1, //quality level
                    50,   //minimum allowed distance between points 
                    3  //size of the averaging block
                    ); //for all corner


            foreach (PointF point in points3[0].OrderBy(p => p.Y))
            {
                listPoint.Add(point);
                CircleF circle = new CircleF(point, 3);
                image3.Draw(circle, new Gray(0), 2);


            }
            image3.Save(@"\File\checkPoint.JPG");
            listPoint.CopyTo(convertPoint);
            listPoint.Clear();
             Graphics graphics = Graphics.FromImage(b1);
             
            for (int i = 0; i < convertPoint.Length; i++)
            {
                //graphics.DrawString(i.ToString(), new Font("Arial", 24), Brushes.Blue, new PointF(convertPoint[i].X + 180, convertPoint[i].Y + 130));
                outputPoint[i].X = Convert.ToInt32(convertPoint[i].X)+180;
                outputPoint[i].Y = Convert.ToInt32(convertPoint[i].Y)+130;
            }
            fpointax = convertPoint[5].X + 180;
            fpointbx = convertPoint[6].X + 180;
            fpointcx = convertPoint[28].X + 180;
            fpointdx = convertPoint[25].X + 180;
            fpointay = convertPoint[5].Y + 130;
            fpointby = convertPoint[6].Y + 130;
            fpointcy = convertPoint[28].Y + 130;
            fpointdy = convertPoint[25].Y + 130;
            fpoint = outputPoint;
            System.Console.WriteLine(fpointax+"/"+fpointay);
            switch (comboBox2.Text)
            {
                case "1":
                    graphics.DrawString("1", new Font("Arial", 28), Brushes.Red, new PointF((convertPoint[20].X + convertPoint[24].X + convertPoint[27].X + convertPoint[26].X + 720) / 4, (convertPoint[20].Y + convertPoint[24].Y + convertPoint[27].Y + convertPoint[26].Y + 520) / 4));
                    graphics.DrawString("2", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[17].X + convertPoint[19].X + convertPoint[20].X + convertPoint[23].X + 720) / 4, (convertPoint[17].Y + convertPoint[19].Y + convertPoint[20].Y + convertPoint[23].Y + 520) / 4));
                    graphics.DrawString("3", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[19].X + convertPoint[18].X + convertPoint[23].X + convertPoint[24].X + 720) / 4, (convertPoint[19].Y + convertPoint[18].Y + convertPoint[23].Y + convertPoint[24].Y + 520) / 4));
                    Console.WriteLine("1");
                    pointax = convertPoint[20].X + 180;
                    pointbx = convertPoint[24].X + 180;
                    pointcx = convertPoint[27].X + 180;
                    pointdx = convertPoint[26].X + 180;
                    pointay = convertPoint[20].Y + 130;
                    pointby = convertPoint[24].Y + 130;
                    pointcy = convertPoint[27].Y + 130;
                    pointdy = convertPoint[26].Y + 130;
                    break;
                case "2":
                    graphics.DrawString("1", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[20].X + convertPoint[24].X + convertPoint[27].X + convertPoint[26].X + 720) / 4, (convertPoint[20].Y + convertPoint[24].Y + convertPoint[27].Y + convertPoint[26].Y + 520) / 4));
                    graphics.DrawString("2", new Font("Arial", 28), Brushes.Red, new PointF((convertPoint[17].X + convertPoint[19].X + convertPoint[20].X + convertPoint[23].X + 720) / 4, (convertPoint[17].Y + convertPoint[19].Y + convertPoint[20].Y + convertPoint[23].Y + 520) / 4));
                    graphics.DrawString("3", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[19].X + convertPoint[18].X + convertPoint[23].X + convertPoint[24].X + 720) / 4, (convertPoint[19].Y + convertPoint[18].Y + convertPoint[23].Y + convertPoint[24].Y + 520) / 4));
                    Console.WriteLine("2");
                    pointax = convertPoint[17].X + 180;
                    pointbx = convertPoint[19].X + 180;
                    pointcx = convertPoint[20].X + 180;
                    pointdx = convertPoint[23].X + 180;
                    pointay = convertPoint[17].Y + 130;
                    pointby = convertPoint[19].Y + 130;
                    pointcy = convertPoint[20].Y + 130;
                    pointdy = convertPoint[23].Y + 130;
                    break;
                case "3":
                    graphics.DrawString("1", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[20].X + convertPoint[24].X + convertPoint[27].X + convertPoint[26].X + 720) / 4, (convertPoint[20].Y + convertPoint[24].Y + convertPoint[27].Y + convertPoint[26].Y + 520) / 4));
                    graphics.DrawString("2", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[17].X + convertPoint[19].X + convertPoint[20].X + convertPoint[23].X + 720) / 4, (convertPoint[17].Y + convertPoint[19].Y + convertPoint[20].Y + convertPoint[23].Y + 520) / 4));
                    graphics.DrawString("3", new Font("Arial", 28), Brushes.Red, new PointF((convertPoint[19].X + convertPoint[18].X + convertPoint[23].X + convertPoint[24].X + 720) / 4, (convertPoint[19].Y + convertPoint[18].Y + convertPoint[23].Y + convertPoint[24].Y + 520) / 4));
                    Console.WriteLine("3");
                    pointax = convertPoint[19].X + 180;
                    pointbx = convertPoint[18].X + 180;
                    pointcx = convertPoint[23].X + 180;
                    pointdx = convertPoint[24].X + 180;
                    pointay = convertPoint[19].Y + 130;
                    pointby = convertPoint[18].Y + 130;
                    pointcy = convertPoint[23].Y + 130;
                    pointdy = convertPoint[24].Y + 130;
                    break;
                //case "4":
                //    graphics.DrawString("1", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[20].X + convertPoint[24].X + convertPoint[27].X + convertPoint[26].X + 720) / 4, (convertPoint[20].Y + convertPoint[24].Y + convertPoint[27].Y + convertPoint[26].Y + 520) / 4));
                //    graphics.DrawString("2", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[17].X + convertPoint[19].X + convertPoint[20].X + convertPoint[23].X + 720) / 4, (convertPoint[17].Y + convertPoint[19].Y + convertPoint[20].Y + convertPoint[23].Y + 520) / 4));
                //    graphics.DrawString("3", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[19].X + convertPoint[18].X + convertPoint[23].X + convertPoint[24].X + 720) / 4, (convertPoint[19].Y + convertPoint[18].Y + convertPoint[23].Y + convertPoint[24].Y + 520) / 4));
                //    graphics.DrawString("4", new Font("Arial", 28), Brushes.Red, new PointF((convertPoint[16].X + convertPoint[17].X + convertPoint[28].X + convertPoint[27].X + 720) / 4, (convertPoint[16].Y + convertPoint[17].Y + convertPoint[28].Y + convertPoint[27].Y + 520) / 4));
                //    graphics.DrawString("5", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[18].X + convertPoint[15].X + convertPoint[26].X + convertPoint[25].X + 720) / 4, (convertPoint[18].Y + convertPoint[15].Y + convertPoint[26].Y + convertPoint[25].Y + 520) / 4));
                //    Console.WriteLine("4");
                //    pointax = convertPoint[16].X + 180;
                //    pointbx = convertPoint[17].X + 180;
                //    pointcx = convertPoint[28].X + 180;
                //    pointdx = convertPoint[27].X + 180;
                //    pointay = convertPoint[16].Y + 130;
                //    pointby = convertPoint[17].Y + 130;
                //    pointcy = convertPoint[28].Y + 130;
                //    pointdy = convertPoint[27].Y + 130;
                //    break;
                //case "5":
                //    graphics.DrawString("1", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[20].X + convertPoint[24].X + convertPoint[27].X + convertPoint[26].X + 720) / 4, (convertPoint[20].Y + convertPoint[24].Y + convertPoint[27].Y + convertPoint[26].Y + 520) / 4));
                //    graphics.DrawString("2", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[17].X + convertPoint[19].X + convertPoint[20].X + convertPoint[23].X + 720) / 4, (convertPoint[17].Y + convertPoint[19].Y + convertPoint[20].Y + convertPoint[23].Y + 520) / 4));
                //    graphics.DrawString("3", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[19].X + convertPoint[18].X + convertPoint[23].X + convertPoint[24].X + 720) / 4, (convertPoint[19].Y + convertPoint[18].Y + convertPoint[23].Y + convertPoint[24].Y + 520) / 4));
                //    graphics.DrawString("4", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[16].X + convertPoint[17].X + convertPoint[28].X + convertPoint[27].X + 720) / 4, (convertPoint[16].Y + convertPoint[17].Y + convertPoint[28].Y + convertPoint[27].Y + 520) / 4));
                //    graphics.DrawString("5", new Font("Arial", 28), Brushes.Red, new PointF((convertPoint[18].X + convertPoint[15].X + convertPoint[26].X + convertPoint[25].X + 720) / 4, (convertPoint[18].Y + convertPoint[15].Y + convertPoint[26].Y + convertPoint[25].Y + 520) / 4));
                //    Console.WriteLine("5");
                //    pointax = convertPoint[18].X + 180;
                //    pointbx = convertPoint[15].X + 180;
                //    pointcx = convertPoint[26].X + 180;
                //    pointdx = convertPoint[25].X + 180;
                //    pointay = convertPoint[18].Y + 130;
                //    pointby = convertPoint[15].Y + 130;
                //    pointcy = convertPoint[26].Y + 130;
                //    pointdy = convertPoint[25].Y + 130;
                //    break;
                    //case "6":
                    //    graphics.DrawString("1", new Font("Arial", 18), Brushes.Blue, new PointF((convertPoint[25].X + convertPoint[29].X + convertPoint[24].X + convertPoint[28].X + 1040) / 4, (convertPoint[25].Y + convertPoint[29].Y + convertPoint[24].Y + convertPoint[28].Y + 600) / 4));
                    //    graphics.DrawString("2", new Font("Arial", 18), Brushes.Blue, new PointF((convertPoint[22].X + convertPoint[11].X + convertPoint[24].X + convertPoint[13].X + 1040) / 4, (convertPoint[11].Y + convertPoint[13].Y + convertPoint[24].Y + convertPoint[22].Y + 600) / 4));
                    //    graphics.DrawString("3", new Font("Arial", 18), Brushes.Blue, new PointF((convertPoint[18].X + convertPoint[22].X + convertPoint[13].X + convertPoint[25].X + 1040) / 4, (convertPoint[18].Y + convertPoint[22].Y + convertPoint[13].Y + convertPoint[25].Y + 600) / 4));
                    //    graphics.DrawString("4", new Font("Arial", 18), Brushes.Blue, new PointF((convertPoint[19].X + convertPoint[11].X + convertPoint[29].X + convertPoint[26].X + 1040) / 4, (convertPoint[19].Y + convertPoint[11].Y + convertPoint[29].Y + convertPoint[26].Y + 600) / 4));
                    //    graphics.DrawString("5", new Font("Arial", 18), Brushes.Blue, new PointF((convertPoint[18].X + convertPoint[17].X + convertPoint[28].X + convertPoint[27].X + 1040) / 4, (convertPoint[18].Y + convertPoint[17].Y + convertPoint[28].Y + convertPoint[27].Y + 600) / 4));
                    //    graphics.DrawString("6", new Font("Arial", 18), Brushes.Red, new PointF((convertPoint[0].X + convertPoint[1].X + convertPoint[8].X + convertPoint[7].X + 1040) / 4, (convertPoint[8].Y + convertPoint[1].Y + convertPoint[0].Y + convertPoint[7].Y + 600) / 4));
                    //    Console.Write("6");
                    //    break;

            }
            Console.WriteLine(pointax + "/" + pointay + "/" + pointbx + "/" + pointby + "/" + pointcx + "/" + pointcy + "/" + pointdx + "/" + pointdy);




        }
        public void drawNumber2(Bitmap b1)
        {
            List<PointF> listPoint = new List<PointF>();
            PointF[] convertPoint = new PointF[30];
            Point[] outputPoint = new Point[30];
            Bitmap a1 = new Bitmap(930, 460);
            Graphics g1 = Graphics.FromImage(a1);
            g1.Clear(Color.Black);
            g1.DrawImage(b1, new Rectangle(-180, -130, b1.Width, b1.Height));


            Image<Gray, Byte> image1 = new Image<Gray, Byte>(a1);


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
                    image2.Draw(line, new Gray(0), 1);
                }
                else if (line.Direction.Y > 0.8 || line.Direction.Y < -0.8)
                {
                    image2.Draw(line, new Gray(0), 1);
                }
                //image2.Draw(line, new Gray(0), 1);
            }
            Bitmap a3 = new Bitmap(930, 460);
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
                //image3.Draw(line, new Gray(255), 2);
            }
            PointF[][] points3 = image3.GoodFeaturesToTrack(
                    29,  //maximum number of corners to be returned
                    0.1, //quality level
                    50,   //minimum allowed distance between points 
                    3  //size of the averaging block
                    ); //for all corner


            foreach (PointF point in points3[0].OrderBy(p => p.Y))
            {
                listPoint.Add(point);
                CircleF circle = new CircleF(point, 3);
                image3.Draw(circle, new Gray(0), 2);


            }
            image3.Save(@"\File\checkPoint.JPG");
            listPoint.CopyTo(convertPoint);
            listPoint.Clear();
            Graphics graphics = Graphics.FromImage(b1);

            for (int i = 0; i < convertPoint.Length; i++)
            {
                //graphics.DrawString(i.ToString(), new Font("Arial", 24), Brushes.Blue, new PointF(convertPoint[i].X + 180, convertPoint[i].Y + 130));
                outputPoint[i].X = Convert.ToInt32(convertPoint[i].X) + 180;
                outputPoint[i].Y = Convert.ToInt32(convertPoint[i].Y) + 130;
            }
            fpointax = convertPoint[5].X + 180;
            fpointbx = convertPoint[2].X + 180;
            fpointcx = convertPoint[25].X + 180;
            fpointdx = convertPoint[28].X + 180;
            fpointay = convertPoint[5].Y + 130;
            fpointby = convertPoint[2].Y + 130;
            fpointcy = convertPoint[25].Y + 130;
            fpointdy = convertPoint[28].Y + 130;
            fpoint = outputPoint;
            System.Console.WriteLine(fpointax + "/" + fpointay);
            switch (comboBox2.Text)
            {
                case "1":
                    graphics.DrawString("1", new Font("Arial", 28), Brushes.Red, new PointF((convertPoint[21].X + convertPoint[20].X + convertPoint[24].X + convertPoint[26].X + 720) / 4, (convertPoint[21].Y + convertPoint[20].Y + convertPoint[24].Y + convertPoint[26].Y + 520) / 4));
                    graphics.DrawString("2", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[15].X + convertPoint[16].X + convertPoint[21].X + convertPoint[18].X + 720) / 4, (convertPoint[15].Y + convertPoint[16].Y + convertPoint[21].Y + convertPoint[18].Y + 520) / 4));
                    graphics.DrawString("3", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[16].X + convertPoint[14].X + convertPoint[18].X + convertPoint[20].X + 720) / 4, (convertPoint[16].Y + convertPoint[14].Y + convertPoint[18].Y + convertPoint[20].Y + 520) / 4));
                    Console.WriteLine("1");
                    pointax = convertPoint[21].X + 180;
                    pointbx = convertPoint[20].X + 180;
                    pointcx = convertPoint[24].X + 180;
                    pointdx = convertPoint[26].X + 180;
                    pointay = convertPoint[21].Y + 130;
                    pointby = convertPoint[20].Y + 130;
                    pointcy = convertPoint[24].Y + 130;
                    pointdy = convertPoint[26].Y + 130;
                    break;
                case "2":
                    graphics.DrawString("1", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[21].X + convertPoint[20].X + convertPoint[24].X + convertPoint[26].X + 720) / 4, (convertPoint[21].Y + convertPoint[20].Y + convertPoint[24].Y + convertPoint[26].Y + 520) / 4));
                    graphics.DrawString("2", new Font("Arial", 28), Brushes.Red, new PointF((convertPoint[15].X + convertPoint[16].X + convertPoint[21].X + convertPoint[18].X + 720) / 4, (convertPoint[15].Y + convertPoint[16].Y + convertPoint[21].Y + convertPoint[18].Y + 520) / 4));
                    graphics.DrawString("3", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[16].X + convertPoint[14].X + convertPoint[18].X + convertPoint[20].X + 720) / 4, (convertPoint[16].Y + convertPoint[14].Y + convertPoint[18].Y + convertPoint[20].Y + 520) / 4));
                    Console.WriteLine("2");
                    pointax = convertPoint[15].X + 180;
                    pointbx = convertPoint[16].X + 180;
                    pointcx = convertPoint[21].X + 180;
                    pointdx = convertPoint[18].X + 180;
                    pointay = convertPoint[15].Y + 130;
                    pointby = convertPoint[16].Y + 130;
                    pointcy = convertPoint[21].Y + 130;
                    pointdy = convertPoint[18].Y + 130;
                    break;
                case "3":
                    graphics.DrawString("1", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[21].X + convertPoint[20].X + convertPoint[24].X + convertPoint[26].X + 720) / 4, (convertPoint[21].Y + convertPoint[20].Y + convertPoint[24].Y + convertPoint[26].Y + 520) / 4));
                    graphics.DrawString("2", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[15].X + convertPoint[16].X + convertPoint[21].X + convertPoint[18].X + 720) / 4, (convertPoint[15].Y + convertPoint[16].Y + convertPoint[21].Y + convertPoint[18].Y + 520) / 4));
                    graphics.DrawString("3", new Font("Arial", 28), Brushes.Red, new PointF((convertPoint[16].X + convertPoint[14].X + convertPoint[18].X + convertPoint[20].X + 720) / 4, (convertPoint[16].Y + convertPoint[14].Y + convertPoint[18].Y + convertPoint[20].Y + 520) / 4));
                    Console.WriteLine("3");
                    pointax = convertPoint[16].X + 180;
                    pointbx = convertPoint[14].X + 180;
                    pointcx = convertPoint[18].X + 180;
                    pointdx = convertPoint[20].X + 180;
                    pointay = convertPoint[16].Y + 130;
                    pointby = convertPoint[14].Y + 130;
                    pointcy = convertPoint[18].Y + 130;
                    pointdy = convertPoint[20].Y + 130;
                    break;
                    //case "4":
                    //    graphics.DrawString("1", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[20].X + convertPoint[24].X + convertPoint[27].X + convertPoint[26].X + 720) / 4, (convertPoint[20].Y + convertPoint[24].Y + convertPoint[27].Y + convertPoint[26].Y + 520) / 4));
                    //    graphics.DrawString("2", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[17].X + convertPoint[19].X + convertPoint[20].X + convertPoint[23].X + 720) / 4, (convertPoint[17].Y + convertPoint[19].Y + convertPoint[20].Y + convertPoint[23].Y + 520) / 4));
                    //    graphics.DrawString("3", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[19].X + convertPoint[18].X + convertPoint[23].X + convertPoint[24].X + 720) / 4, (convertPoint[19].Y + convertPoint[18].Y + convertPoint[23].Y + convertPoint[24].Y + 520) / 4));
                    //    graphics.DrawString("4", new Font("Arial", 28), Brushes.Red, new PointF((convertPoint[16].X + convertPoint[17].X + convertPoint[28].X + convertPoint[27].X + 720) / 4, (convertPoint[16].Y + convertPoint[17].Y + convertPoint[28].Y + convertPoint[27].Y + 520) / 4));
                    //    graphics.DrawString("5", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[18].X + convertPoint[15].X + convertPoint[26].X + convertPoint[25].X + 720) / 4, (convertPoint[18].Y + convertPoint[15].Y + convertPoint[26].Y + convertPoint[25].Y + 520) / 4));
                    //    Console.WriteLine("4");
                    //    pointax = convertPoint[16].X + 180;
                    //    pointbx = convertPoint[17].X + 180;
                    //    pointcx = convertPoint[28].X + 180;
                    //    pointdx = convertPoint[27].X + 180;
                    //    pointay = convertPoint[16].Y + 130;
                    //    pointby = convertPoint[17].Y + 130;
                    //    pointcy = convertPoint[28].Y + 130;
                    //    pointdy = convertPoint[27].Y + 130;
                    //    break;
                    //case "5":
                    //    graphics.DrawString("1", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[20].X + convertPoint[24].X + convertPoint[27].X + convertPoint[26].X + 720) / 4, (convertPoint[20].Y + convertPoint[24].Y + convertPoint[27].Y + convertPoint[26].Y + 520) / 4));
                    //    graphics.DrawString("2", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[17].X + convertPoint[19].X + convertPoint[20].X + convertPoint[23].X + 720) / 4, (convertPoint[17].Y + convertPoint[19].Y + convertPoint[20].Y + convertPoint[23].Y + 520) / 4));
                    //    graphics.DrawString("3", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[19].X + convertPoint[18].X + convertPoint[23].X + convertPoint[24].X + 720) / 4, (convertPoint[19].Y + convertPoint[18].Y + convertPoint[23].Y + convertPoint[24].Y + 520) / 4));
                    //    graphics.DrawString("4", new Font("Arial", 28), Brushes.Blue, new PointF((convertPoint[16].X + convertPoint[17].X + convertPoint[28].X + convertPoint[27].X + 720) / 4, (convertPoint[16].Y + convertPoint[17].Y + convertPoint[28].Y + convertPoint[27].Y + 520) / 4));
                    //    graphics.DrawString("5", new Font("Arial", 28), Brushes.Red, new PointF((convertPoint[18].X + convertPoint[15].X + convertPoint[26].X + convertPoint[25].X + 720) / 4, (convertPoint[18].Y + convertPoint[15].Y + convertPoint[26].Y + convertPoint[25].Y + 520) / 4));
                    //    Console.WriteLine("5");
                    //    pointax = convertPoint[18].X + 180;
                    //    pointbx = convertPoint[15].X + 180;
                    //    pointcx = convertPoint[26].X + 180;
                    //    pointdx = convertPoint[25].X + 180;
                    //    pointay = convertPoint[18].Y + 130;
                    //    pointby = convertPoint[15].Y + 130;
                    //    pointcy = convertPoint[26].Y + 130;
                    //    pointdy = convertPoint[25].Y + 130;
                    //    break;
                    //case "6":
                    //    graphics.DrawString("1", new Font("Arial", 18), Brushes.Blue, new PointF((convertPoint[25].X + convertPoint[29].X + convertPoint[24].X + convertPoint[28].X + 1040) / 4, (convertPoint[25].Y + convertPoint[29].Y + convertPoint[24].Y + convertPoint[28].Y + 600) / 4));
                    //    graphics.DrawString("2", new Font("Arial", 18), Brushes.Blue, new PointF((convertPoint[22].X + convertPoint[11].X + convertPoint[24].X + convertPoint[13].X + 1040) / 4, (convertPoint[11].Y + convertPoint[13].Y + convertPoint[24].Y + convertPoint[22].Y + 600) / 4));
                    //    graphics.DrawString("3", new Font("Arial", 18), Brushes.Blue, new PointF((convertPoint[18].X + convertPoint[22].X + convertPoint[13].X + convertPoint[25].X + 1040) / 4, (convertPoint[18].Y + convertPoint[22].Y + convertPoint[13].Y + convertPoint[25].Y + 600) / 4));
                    //    graphics.DrawString("4", new Font("Arial", 18), Brushes.Blue, new PointF((convertPoint[19].X + convertPoint[11].X + convertPoint[29].X + convertPoint[26].X + 1040) / 4, (convertPoint[19].Y + convertPoint[11].Y + convertPoint[29].Y + convertPoint[26].Y + 600) / 4));
                    //    graphics.DrawString("5", new Font("Arial", 18), Brushes.Blue, new PointF((convertPoint[18].X + convertPoint[17].X + convertPoint[28].X + convertPoint[27].X + 1040) / 4, (convertPoint[18].Y + convertPoint[17].Y + convertPoint[28].Y + convertPoint[27].Y + 600) / 4));
                    //    graphics.DrawString("6", new Font("Arial", 18), Brushes.Red, new PointF((convertPoint[0].X + convertPoint[1].X + convertPoint[8].X + convertPoint[7].X + 1040) / 4, (convertPoint[8].Y + convertPoint[1].Y + convertPoint[0].Y + convertPoint[7].Y + 600) / 4));
                    //    Console.Write("6");
                    //    break;

            }
            Console.WriteLine(pointax + "/" + pointay + "/" + pointbx + "/" + pointby + "/" + pointcx + "/" + pointcy + "/" + pointdx + "/" + pointdy);




        }

        //廣告的四個點
        float pointax;
        float pointay;
        float pointbx;
        float pointby;
        float pointcx;
        float pointcy;
        float pointdx;
        float pointdy;
        //球場邊緣的四個點
        float fpointax;
        float fpointay;
        float fpointbx;
        float fpointby;
        float fpointcx;
        float fpointcy;
        float fpointdx;
        float fpointdy;
        //空白球場的點
        Point[] fpoint;
        //影片的檔名
        string videoFileName;



        Image emptyFrame;
        Image emptyFrameView;
        private void videoSelectBtn_Click(object sender, EventArgs e)   //選擇影片按鈕
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile = new OpenFileDialog();
            if (OpenFile.ShowDialog(this) == DialogResult.OK)
            {
                videoFileName = OpenFile.FileName;
                Form4 videoForm = new Form4(OpenFile); //產生影片form
                String videoPath1 = Application.StartupPath + @"\File\video1_720p.wmv";
                String videoPath2 = Application.StartupPath + @"\File\video2_720p.wmv";
                if (videoFileName == videoPath1)
                {
                    selectVideo = 1;
                }
                else if (videoFileName == videoPath2)
                {
                    selectVideo = 2;
                }
                Console.WriteLine("selectVideo:" +selectVideo);
                Console.WriteLine("Name:" + videoFileName);
                Console.WriteLine(videoPath1);
                if (videoForm.ShowDialog() == DialogResult.OK)  //開啟影片form，若按下確定按鈕並結束則執行以下程式
                {
                    float frameTime = videoForm.time; //得到form傳出的空白球場時間點
                    vlc1.playlist.items.clear();
                    vlc1.playlist.add("file:///" + OpenFile.FileName, OpenFile.SafeFileName, null); //於左邊播放器載入影片


                    add();//新增選單的項目


                    VideoProcessor processor = new VideoProcessor();
                    processor.SetInput(OpenFile.FileName);
                    emptyFrame = processor.findFrameByTime(frameTime);
                    //String savePath =AppDomain.CurrentDomain.BaseDirectory+@"File\\test1.JPG";
                    //emptyFrame.Save(savePath);
                    //emptyFrameView = (Image)emptyFrame.Clone();
                    //pictureBox2.Image = emptyFrameView;//從時間點找到對應的影格後顯示於畫面上
                    //if (selectVideo == 1)
                    //{
                    //    drawNumber1((Bitmap)emptyFrameView);
                    //}
                    //else if (selectVideo == 2)
                    //{
                    //    drawNumber2((Bitmap)emptyFrameView);
                    //}
                    //呼叫分割區塊的function



                }


                
            }
            if (selectVideo==1)
            {
                pictureBox2.Image = new Bitmap(Application.StartupPath + @"\File\need3.JPG");
                emptyFrame = pictureBox2.Image;
                emptyFrameView = (Image)emptyFrame.Clone();
                pictureBox2.Image = emptyFrameView;
                drawNumber1((Bitmap)emptyFrameView);
            }
            else if (selectVideo==2)
            {
                pictureBox2.Image = new Bitmap(Application.StartupPath + @"\File\need2.JPG");
                emptyFrame = pictureBox2.Image;
                emptyFrameView = (Image)emptyFrame.Clone();
                pictureBox2.Image = emptyFrameView;
                drawNumber2((Bitmap)emptyFrameView);
            }
        }

     
        public class VideoProcessor
        {
            //the OpenCV video capture object
            private Capture capture;

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
            
            //extension of output images
            string extension;
            //模板路徑
            string comparefileName;
            //比較結果
            
            bool same = true;
            Point[] pointz = new Point[28];

            public Image<Gray, Byte> _Image1;
            public Image<Gray, Byte> _Image2;




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
            
            public void SetCompare(string filePath)
            {

                comparefileName = filePath;
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
                    if (same)
                    {

                        pointz = getPoint(frame.Bitmap);

                        //foreach (Point a in pointz)
                        //{
                        //    System.Console.WriteLine(a);
                        //}
                        FindH();

                    }


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
            public double MatchHist()
            {

                int[] hist_size = new int[1] { 256 };//建一个数组来存放直方图数据   

                IntPtr HistImg1 = CvInvoke.cvCreateHist(1, hist_size, Emgu.CV.CvEnum.HIST_TYPE.CV_HIST_ARRAY, null, 1); //创建一个空的直方图  
                IntPtr HistImg2 = CvInvoke.cvCreateHist(1, hist_size, Emgu.CV.CvEnum.HIST_TYPE.CV_HIST_ARRAY, null, 1);
                IntPtr[] inPtr1 = new IntPtr[1] { this._Image1 };
                IntPtr[] inPtr2 = new IntPtr[1] { this._Image2 };
                CvInvoke.cvCalcHist(inPtr1, HistImg1, false, IntPtr.Zero); //计算inPtr1指向图像的数据，并传入HistImg1中  
                CvInvoke.cvCalcHist(inPtr2, HistImg2, false, IntPtr.Zero);

                double compareResult;

                StringBuilder result = new StringBuilder();

                CvInvoke.cvNormalizeHist(HistImg1, 1d); //直方图对比方式   
                CvInvoke.cvNormalizeHist(HistImg2, 1d);

                Emgu.CV.CvEnum.HISTOGRAM_COMP_METHOD compareMethod = Emgu.CV.CvEnum.HISTOGRAM_COMP_METHOD.CV_COMP_CORREL;
                compareResult = CvInvoke.cvCompareHist(HistImg1, HistImg2, compareMethod);
                return compareResult;

            }

            /// <summary>
            /// 由時間點找出對應的影格並回傳
            /// </summary>
            public Image findFrameByTime(float frameTime)
            {
                //由時間*fps得到該截取的影格編號
                int frameIndex = Convert.ToInt32(frameTime * Convert.ToSingle(this.capture.GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FPS)));
                int count = 0;
                while(count != frameIndex)  //用迴圈從0開始一路截到該影格
                {
                    this.capture.QueryFrame();
                    count++;
                }
                IImage spaceFrame = this.capture.QueryFrame(); //只儲存最後要的一個影格
                
                return spaceFrame.Bitmap;
            }



            public Image findFirstSame()
            {
                //current frame
                IImage frame;
                //if no catpure device has been set
                if (!this.IsOpened)
                {
                    return null;
                }
                this.stop = false;

                frame = this.capture.QueryFrame();

                while (!IsStopped)
                {
                    //比較
                    Image<Rgb, byte> f1 = new Image<Rgb, Byte>(frame.Bitmap);
                    Image<Rgb, byte> f2 = new Image<Rgb, byte>(comparefileName);

                    this._Image1 = ((Image<Rgb, Byte>)f1).Convert<Gray, Byte>();
                    this._Image2 = ((Image<Rgb, Byte>)f2).Convert<Gray, Byte>();

                    if (MatchHist() >= 0.9)
                    {
                        break;
                    }

                    //read next frame if any
                    frame = this.capture.QueryFrame();
                    if (frame == null)
                    {
                        break;
                    }
                }
                //this.writer.Dispose();
                return frame.Bitmap;
            }


            public Point[] getPoint(Bitmap b1)
            {
                List<PointF> listPoint = new List<PointF>();
                PointF[] convertPoint = new PointF[28];
                Point[] outputPoint = new Point[28];
                Bitmap a1 = new Bitmap(1500, 600);
                Graphics g1 = Graphics.FromImage(a1);
                g1.Clear(Color.Black);
                g1.DrawImage(b1, new Rectangle(-600, -300, b1.Width, b1.Height));


                Image<Gray, Byte> image1 = new Image<Gray, Byte>(a1);


                Image<Gray, Byte> image2 = new Image<Gray, Byte>(a1);
                //Hough transform for line detection
                LineSegment2D[][] lines = image2.HoughLines(
                    new Gray(125),  //Canny algorithm low threshold
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
                        image2.Draw(line, new Gray(0), 1);
                    }
                    else if (line.Direction.Y > 0.8 || line.Direction.Y < -0.8)
                    {
                        image2.Draw(line, new Gray(0), 1);
                    }

                }
                Bitmap a3 = new Bitmap(1500, 600);
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
                        28,  //maximum number of corners to be returned
                        0.01, //quality level
                        35,   //minimum allowed distance between points 
                        3   //size of the averaging block
                        ); //for all corner


                foreach (PointF point in points3[0].OrderBy(p => p.Y))
                {
                    listPoint.Add(point);
                    CircleF circle = new CircleF(point, 3);
                    image3.Draw(circle, new Gray(0), 2);


                }
                listPoint.CopyTo(convertPoint);
                listPoint.Clear();
                for (int i = 0; i < 28; i++)
                {
                    outputPoint[i].X = Convert.ToInt32(convertPoint[i].X);
                    outputPoint[i].Y = Convert.ToInt32(convertPoint[i].Y);

                }
                return outputPoint;

            }

            public void inputPoint()
            {
                for (int i = 0; i <= pointz.Length - 1; i++)
                {
                    for (int j = 0; j <= 0; j++)
                    {
                        array2D[i, j] = pointz[i].X;  //[0,0,0]=1  把X塞進指定項
                        array2Db[i, j] = pointsb[i].X;
                    }

                }
                for (int i = 0; i <= pointz.Length - 1; i++)
                {
                    for (int j = 1; j <= 1; j++)
                    {

                        array2D[i, j] = pointz[i].Y; //[0,0,1]=2     把Y塞進指定項
                        array2Db[i, j] = pointsb[i].Y;

                    }

                }
                for (int i = 0; i <= pointz.Length - 1; i++)
                {
                    for (int j = 2; j <= 2; j++)
                    {
                        array2D[i, j] = 5; //[0,0,2]=3       Z項全部是5
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
            public void mutipleTranserMatrix()
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
                Console.WriteLine("H............");
                for (int i = 0; i < 9; i++)
                {
                    Console.WriteLine(arraymulH[i, 0]);
                }
            }
            Point[] pointsb = new Point[] { new Point { X = 6415, Y = 3660 }, new Point { X = 6415, Y = 14630 }, new Point { X = 30185, Y = 3660 }, new Point { X = 30185, Y = 14630 }, new Point { X = 6415, Y = 14630 }, new Point { X = 30185, Y = 3660 }, new Point { X = 30185, Y = 14630 }, new Point { X = 6415, Y = 14630 }, new Point { X = 30185, Y = 3660 }, new Point { X = 30185, Y = 14630 }, new Point { X = 6415, Y = 14630 }, new Point { X = 30185, Y = 3660 }, new Point { X = 30185, Y = 14630 }, new Point { X = 6415, Y = 14630 }, new Point { X = 30185, Y = 3660 }, new Point { X = 30185, Y = 14630 }, new Point { X = 6415, Y = 14630 }, new Point { X = 30185, Y = 3660 }, new Point { X = 30185, Y = 14630 }, new Point { X = 6415, Y = 14630 }, new Point { X = 30185, Y = 3660 }, new Point { X = 30185, Y = 14630 }, new Point { X = 6415, Y = 14630 }, new Point { X = 30185, Y = 3660 }, new Point { X = 30185, Y = 14630 }, new Point { X = 6415, Y = 14630 }, new Point { X = 30185, Y = 3660 }, new Point { X = 30185, Y = 14630 } };

            int[,] array2D = new int[28, 28];//寫死
            int[,] array2Db = new int[28, 28];//寫死



            int[,] array2D2 = new int[,] { };
            int[,] array2D2b = new int[,] { };
            float[,] arrayequationtrans = new float[8, 8];//row*column//
            float[,] arraymul = new float[8, 8]; //do the mutiple A'A
            float[,] arraymul2 = new float[8, 1]; //do the A'B
            int[,] arrayL = new int[8, 8];
            int[,] arrayU = new int[8, 8];
            float[,] u = new float[8, 8];
            float[,] l = new float[8, 8];
            float[,] y = new float[8, 8];
            float[,] x = new float[8, 8];
            float[,] b = new float[8, 8];  //求(A'A)^-1
            float[,] arraymulH = new float[9, 1]; //H=(A'A)^-1*(A'B)
                                                  //Do the 8*8 matrix
            int[,] arrayequation = new int[8, 8];//
            int[,] arrayequation2 = new int[,] { }; //

            public void FindH()
            {
                inputPoint();
                do8Matrix();
                transferMatrix();
                mutipleTranserMatrix();
                mutipleABMatrix();
                inverseMatrix();
                FindHmatrix();
            }
        }

        private void vlc1_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        
    }
}
