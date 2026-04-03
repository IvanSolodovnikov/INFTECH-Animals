namespace AnimalsApp.Models
{
    public class Turtle : LivingBeing
    {
        public Turtle(double maxSpeed) : base(maxSpeed) { }

        public override void Move()
        {
            Speed += 0.5;
            if (Speed > MaxSpeed)
                Speed = MaxSpeed;
        }

        public override void Stop()
        {
            Speed -= 0.5;
            if (Speed < 0)
                Speed = 0;
        }
    }
}