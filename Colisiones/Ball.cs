using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colisiones
{
    public class Ball
    {
        public int radius, z;
        public Brush b;
        public Point center;
        public Point speed;
        public Point wind;

        public Ball(int sel,Random rand, Rectangle bounds, Point mouse)
        {
            wind=new Point(0,0);
            if (sel == 0)
                DeclareCollision(rand, bounds);
            if (sel == 1)
                DeclareRain(rand, bounds);
            if (sel == 2)
                DeclareFire(rand, bounds, mouse);
            if (sel == 3)
                DeclareSnow(rand, bounds, mouse);
        }

        public void DeclareFire(Random rand, Rectangle bounds, Point mouse)
        {
            this.radius = rand.Next(15, 40);
            this.center.X = rand.Next(mouse.X-5, mouse.X+6);
            this.center.Y = rand.Next(mouse.Y-5 , mouse.Y+6);
            this.speed.X = rand.Next(-3, 3);
            this.speed.Y = rand.Next(-5, -3);
            this.z = rand.Next(0, 4);
            
            if (z < 1)
                b = new SolidBrush(Color.FromArgb(255, 255, rand.Next(0, 225), 0));
            else if (z < 2)
                b = new SolidBrush(Color.FromArgb(180, 255, rand.Next(0, 225), 0));
            else if (z < 3)
                b = new SolidBrush(Color.FromArgb(100, 255, rand.Next(0, 225), 0));
            else if (z < 4)
                b = new SolidBrush(Color.FromArgb(20, 255, rand.Next(0, 225), 0));
        }

        public void DeclareCollision(Random rand, Rectangle bounds)
        {
            this.radius = rand.Next(15, 40);
            this.center.X = rand.Next(30, bounds.Width - 30);
            this.center.Y = rand.Next(30, bounds.Height - 30);
            this.speed.X = rand.Next(-20, 21);
            this.speed.Y = rand.Next(-20, 21);
            b = new SolidBrush(Color.FromArgb(255, rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)));
        }

        public void DeclareRain(Random rand, Rectangle bounds)
        {
            this.radius = rand.Next(15, 26);
            this.center.X = rand.Next(0, bounds.Width);
            this.center.Y = rand.Next(0, 3);
            this.speed.X = rand.Next(-2, 3);
            this.speed.Y = rand.Next(10, 30);
            this.z = rand.Next(0, 4);
        }

        public void DeclareSnow(Random rand, Rectangle bounds, Point mouse)
        {
            this.radius = rand.Next(5, 15);
            this.center.X = rand.Next(mouse.X-5, mouse.X+5);
            this.center.Y = rand.Next(mouse.Y - 5, mouse.Y + 5);
            this.speed.X = rand.Next(-2, 3);
            this.speed.Y = rand.Next(5, 10);
            this.z = rand.Next(0, 4);
            b = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        }

        public void UpdateRain(Rectangle bounds, Random rand, int c)
        {
            this.center.Offset(this.speed);
            if(c<250)
                this.wind.X = rand.Next(-15, -7);
            else
                this.wind.X = rand.Next(7, 15);
            this.center.Offset(this.wind);

            if (this.center.Y > bounds.Bottom || this.center.X < bounds.Left || this.center.X > bounds.Right)
            {
                DeclareRain(rand, bounds);
            }
        }


        public void UpdateParticle(Rectangle bounds, List<Ball> balls, Random rand, int sel, Point mouse)
        {
            this.center.Offset(this.speed);

            // Check for collisions with walls
            if (this.center.X + this.radius < bounds.Left || this.center.X - this.radius > bounds.Right)
            {
                if (sel == 2)
                    DeclareFire(rand, bounds, mouse);
                if (sel == 3)
                    DeclareSnow(rand, bounds, mouse);
            }
            if (this.center.Y + this.radius < bounds.Top || this.center.Y - this.radius >= bounds.Bottom)
            {
                if (sel == 2)
                    DeclareFire(rand, bounds, mouse);
                if (sel == 3)
                    DeclareSnow(rand, bounds, mouse);
            }

            // Check for collisions with other balls
            foreach (var other in balls)
            {
                if (other.z == this.z)
                {
                    if (other != this && CheckCollision(this, other))
                    {
                        ResolveParticle(this, other);
                    }
                }
            }
        }

        private static void ResolveParticle(Ball a, Ball b)
        {
            int dx = b.center.X - a.center.X;
            int dy = b.center.Y - a.center.Y;
            int dist = a.radius / 2 + b.radius / 2;
            int overlap = dist - (int)Math.Sqrt(dx * dx + dy * dy);

            // Move balls apart to resolve overlap
            a.center.Offset(-overlap * dx / dist, -overlap * dy / dist);
            b.center.Offset(overlap * dx / dist, overlap * dy / dist);
        }

        public void UpdatePosition(Rectangle bounds, List<Ball> balls)
        {
            this.center.Offset(this.speed);

            // Check for collisions with walls
            if (this.center.X  <= bounds.Left || this.center.X + this.radius >= bounds.Right)
            {
                this.speed.X = -this.speed.X;
            }
            if (this.center.Y  <= bounds.Top || this.center.Y + this.radius >= bounds.Bottom)
            {
                this.speed.Y = -this.speed.Y;
            }

            // Check for collisions with other balls
            foreach (var other in balls)
            {
                if (other != this && CheckCollision(this, other))
                {
                    ResolveCollision(this, other);
                }
            }
        }

        private static bool CheckCollision(Ball a, Ball b)
        {
            int dx = a.center.X - b.center.X;
            int dy = a.center.Y - b.center.Y;
            int dist = a.radius/2 + b.radius/2;
            return (dx * dx + dy * dy) < (dist * dist);
        }

        private static void ResolveCollision(Ball a, Ball b)
        {
            int dx = b.center.X - a.center.X;
            int dy = b.center.Y - a.center.Y;
            int dist = a.radius/2 + b.radius/2;
            int overlap = dist - (int)Math.Sqrt(dx * dx + dy * dy);

            // Move balls apart to resolve overlap
            a.center.Offset(-overlap * dx / dist, -overlap * dy / dist);
            b.center.Offset(overlap * dx / dist, overlap * dy / dist);

            // Calculate new velocities using 1D collision formulas
            int totalMass = a.radius/2 + b.radius/2;
            int aSpeed = (a.speed.X * dx + a.speed.Y * dy) / totalMass;
            int bSpeed = (b.speed.X * dx + b.speed.Y * dy) / totalMass;
            a.speed.Offset((bSpeed - aSpeed) * dx / dist, (bSpeed - aSpeed) * dy / dist);
            b.speed.Offset((aSpeed - bSpeed) * dx / dist, (aSpeed - bSpeed) * dy / dist);
        }
    }
}
