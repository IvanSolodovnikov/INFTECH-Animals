namespace AnimalModels
{
    public interface IAnimal
    {
        double Speed { get; }

        void Move();
        void Stop();
    }
}
