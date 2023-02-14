using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colisiones
{
    public class Ball
    {
        public int radius;
        public Point center;
        public Point speed;

        public Ball(int radius, Point position, Point velocity)
        {
            this.radius = radius;
            this.center = position;
            this.speed = velocity;
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
