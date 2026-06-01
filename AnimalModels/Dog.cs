using System;

namespace AnimalModels
{
    public class Dog : LivingBeing
    {
        public event Action? OnBark;

        public Dog(double maxSpeed) : base(maxSpeed) { }

        public override void Move()
        {
            Speed += 2;
            if (Speed > MaxSpeed)
                Speed = MaxSpeed;
        }

        public override void Stop()
        {
            Speed -= 2;
            if (Speed < 0)
                Speed = 0;
        }

        public string Bark()
        {
            OnBark?.Invoke();
            return "Собака: гав!";
        }
    }
}
