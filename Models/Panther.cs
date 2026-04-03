using System;

namespace AnimalsApp.Models
{
    public class Panther : LivingBeing
    {
        public event Action OnRoar;

        public Panther(double maxSpeed) : base(maxSpeed) { }

        public override void Move()
        {
            Speed += 4;
            if (Speed > MaxSpeed)
                Speed = MaxSpeed;
        }

        public override void Stop()
        {
            Speed -= 4;
            if (Speed < 0)
                Speed = 0;
        }

        public void Roar()
        {
            OnRoar?.Invoke();
        }

        public string ClimbTree()
        {
            return "Пантера залезла на дерево";
        }
    }
}