namespace AnimalsApp.Models
{
    public interface IAnimal
    {
        double Speed { get; }

        void Move();
        void Stop();
    }
}
