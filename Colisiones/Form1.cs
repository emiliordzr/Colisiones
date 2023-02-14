using System.Runtime.CompilerServices;

namespace Colisiones
{
    public partial class Form1 : Form
    {
        Ball b;
        Graphics g;
        Bitmap bmp;
        List<Ball> balls;
        Rectangle bounds;
        int radius = 20;
        int speedX = 5;
        int speedY=5;

        Point center, speed;

        public Form1()
        {
            InitializeComponent();
            var random= new Random();

            balls = new List<Ball>();
            bmp =new Bitmap(pictureBox1.Width, pictureBox1.Height);
            center=new Point(pictureBox1.Width/2, pictureBox1.Height/2);
            bounds = new Rectangle(new Point(0, 0), new Size(pictureBox1.Width, pictureBox1.Height));
            pictureBox1.Image= bmp;
            g=Graphics.FromImage(bmp);

            speedX = random.Next(-10,11);
            speedY = random.Next(-10, 11);
            speed = new Point(speedX, speedY);
            b = new Ball(radius, center, speed);
            balls.Add(b);

            speedX = random.Next(-10, 11);
            speedY = random.Next(-10, 11);
            speed = new Point(speedX, speedY);
            center = new Point(pictureBox1.Width / 2+50, pictureBox1.Height / 2-15);
            b = new Ball(radius+5, center, speed);
            balls.Add(b);

            speedX = random.Next(-10, 11);
            speedY = random.Next(-10, 11);
            speed = new Point(speedX, speedY);
            center = new Point(pictureBox1.Width / 2 -50, pictureBox1.Height / 2 + 20);
            b = new Ball(radius + 10, center, speed);
            balls.Add(b);

            speedX = random.Next(-10, 11);
            speedY = random.Next(-10, 11);
            speed = new Point(speedX, speedY);
            center = new Point(pictureBox1.Width / 2 + 20, pictureBox1.Height / 2 - 40);
            b = new Ball(radius + 15, center, speed);
            balls.Add(b);

            speedX = random.Next(-10, 11);
            speedY = random.Next(-10, 11);
            speed = new Point(speedX, speedY);
            center = new Point(pictureBox1.Width / 2 -70, pictureBox1.Height / 2 -30);
            b = new Ball(radius - 5, center, speed);
            balls.Add(b);
        }

        public void Render()
        {
            g.Clear(Color.Black);
            for(int i=0; i<balls.Count; i++)
            {
                g.FillEllipse(Brushes.Yellow, balls[i].center.X, balls[i].center.Y, balls[i].radius, balls[i].radius);
            }
            
            pictureBox1.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < balls.Count; i++)
                balls[i].UpdatePosition(bounds, balls);
            Render();
        }
    }
}