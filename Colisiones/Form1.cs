using System.Runtime.CompilerServices;
using static System.Formats.Asn1.AsnWriter;

namespace Colisiones
{
    public partial class Form1 : Form
    {
        int counter, sel;
        Canvas canvas;
        Ball b;
        Graphics g;
        Bitmap bmp;
        Random random;
        Brush brush;
        List<Ball> balls;
        Point mousePos;
        Rectangle bounds;
        public bool rain, fire, colision, snow=false;

        public Form1()
        {
            InitializeComponent();
            random= new Random();
            mousePos = new Point(0, 0);
            canvas = new Canvas(pictureBox1.Width, pictureBox1.Height);
            balls = new List<Ball>();
            bmp =new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bounds = new Rectangle(new Point(0, 0), new Size(pictureBox1.Width, pictureBox1.Height));
            pictureBox1.Image= bmp;
            g = Graphics.FromImage(bmp);
            counter = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            sel = 1;
            balls.Clear();
            fire = false;
            colision = false;
            snow = false;
            rain = true;
            for (int i = 0; i < 500; i++)
            {
                b = new Ball(sel, random, bounds, mousePos);
                balls.Add(b);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sel = 2;
            balls.Clear();
            fire = true;
            colision = false;
            snow = false;
            rain = false;
            for (int i = 0; i < 250; i++)
            {
                b = new Ball(sel, random, bounds, mousePos);
                balls.Add(b);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sel = 3;
            balls.Clear();
            fire = false;
            colision = false;
            snow = true;
            rain = false;
            for (int i = 0; i < 300; i++)
            {
                b = new Ball(sel, random, bounds, mousePos);
                balls.Add(b);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            sel = 0;
            balls.Clear();
            fire = false;
            colision = true;
            snow = false;
            rain = false;
            for (int i = 0; i < 50; i++)
            {
                b = new Ball(sel, random, bounds, mousePos);
                balls.Add(b);
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
                mousePos = e.Location;
        }
        public void Render()
        {
            g.Clear(Color.Black);
            if (colision)
            {
                for (int i = 0; i < balls.Count; i++)
                {
                    g.FillEllipse(balls[i].b, balls[i].center.X, balls[i].center.Y, balls[i].radius, balls[i].radius);
                }
            }

            if (rain)
            {
                for (int i = 0; i < balls.Count; i++)
                {
                    if (balls[i].z==0)
                        brush = new SolidBrush(Color.FromArgb(30, 0, 0, 225));
                    else if (balls[i].z == 1)
                        brush = new SolidBrush(Color.FromArgb(180, 0, 0, 225));
                    else if (balls[i].z == 2)
                        brush = new SolidBrush(Color.FromArgb(130, 0, 0, 225));
                    else if (balls[i].z == 3)
                        brush = new SolidBrush(Color.FromArgb(100, 0, 0, 225));
                    else
                        brush = new SolidBrush(Color.FromArgb(225, 0, 0, 225));

                    g.FillEllipse(Brushes.Blue, balls[i].center.X, balls[i].center.Y, 3, balls[i].radius);
                }
            }

            if (fire)
            {
                for (int i = 0; i < balls.Count; i++)
                {
                    g.FillEllipse(balls[i].b, balls[i].center.X, balls[i].center.Y, balls[i].radius, balls[i].radius);
                }
            }

            if (snow)
            {
                g.DrawImage(Resource1.bg, 0, 0, pictureBox1.Width, pictureBox1.Height);
                for (int i = 0; i < balls.Count; i++)
                {
                    g.DrawImage(Resource1.copo_de_nieve2, balls[i].center.X, balls[i].center.Y, balls[i].radius, balls[i].radius);
                }
            }
            
            pictureBox1.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            canvas.FastClear();
            if (rain)
            {
                counter++;
                if (counter > 500) { counter = 0; }
                for (int i = 0; i < balls.Count; i++)
                    balls[i].UpdateRain(bounds, random, counter);
            }
            if(colision)
            {
                for (int i = 0; i < balls.Count; i++)
                    balls[i].UpdatePosition(bounds, balls);
            }
            if (fire)
            {
                for (int i = 0; i < balls.Count; i++)
                    balls[i].UpdateParticle(bounds, balls, random, sel, mousePos);
            }
            if (snow)
            {
                for (int i = 0; i < balls.Count; i++)
                    balls[i].UpdateParticle(bounds, balls, random, sel, mousePos);
            }

            Render();
        }
    }
}