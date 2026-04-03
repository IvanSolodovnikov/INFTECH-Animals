namespace AnimalsApp.Models
{
    public abstract class LivingBeing
    {
        public double Speed { get; protected set; }
        protected double MaxSpeed { get; set; }

        public LivingBeing(double maxSpeed)
        {
            MaxSpeed = maxSpeed;
            Speed = 0;
        }

        public abstract void Move();
        public abstract void Stop();
    }
}