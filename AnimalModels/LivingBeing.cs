namespace AnimalModels
{
    public abstract class LivingBeing : IAnimal
    {
        public double Speed { get; protected set; }
        protected double MaxSpeed { get; set; }

        public LivingBeing(double maxSpeed)
        {
            MaxSpeed = maxSpeed;
            Speed = 0;
        }

        public string SetMaxSpeed(double maxSpeed)
        {
            MaxSpeed = maxSpeed;
            if (Speed > MaxSpeed)
                Speed = MaxSpeed;

            return $"Максимальная скорость изменена: {MaxSpeed}";
        }

        public abstract void Move();
        public abstract void Stop();
    }
}
