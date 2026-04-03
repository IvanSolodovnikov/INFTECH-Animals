using System.ComponentModel;
using AnimalsApp.Models;

namespace AnimalsApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public Dog Dog { get; }
        public Panther Panther { get; }
        public Turtle Turtle { get; }

        private string _dogOutput = "Собака";
        public string DogOutput
        {
            get => _dogOutput;
            set { _dogOutput = value; OnPropertyChanged(nameof(DogOutput)); }
        }

        private string _pantherOutput = "Пантера";
        public string PantherOutput
        {
            get => _pantherOutput;
            set { _pantherOutput = value; OnPropertyChanged(nameof(PantherOutput)); }
        }

        private string _turtleOutput = "Черепаха";
        public string TurtleOutput
        {
            get => _turtleOutput;
            set { _turtleOutput = value; OnPropertyChanged(nameof(TurtleOutput)); }
        }

        public MainViewModel()
        {
            Dog = new Dog(10);
            Panther = new Panther(20);
            Turtle = new Turtle(3);

            Dog.OnBark += () => DogOutput = "Собака: гав!";
            Panther.OnRoar += () => PantherOutput = "Пантера: ррр!";
        }

        public void MoveDog()
        {
            Dog.Move();
            DogOutput = $"Собака: {Dog.Speed}";
        }

        public void StopDog()
        {
            Dog.Stop();
            DogOutput = $"Собака: {Dog.Speed}";
        }

        public void BarkDog() => Dog.Bark();

        public void MovePanther()
        {
            Panther.Move();
            PantherOutput = $"Пантера: {Panther.Speed}";
        }

        public void StopPanther()
        {
            Panther.Stop();
            PantherOutput = $"Пантера: {Panther.Speed}";
        }

        public void RoarPanther() => Panther.Roar();

        public void ClimbTree()
        {
            PantherOutput = Panther.ClimbTree();
        }

        public void MoveTurtle()
        {
            Turtle.Move();
            TurtleOutput = $"Черепаха: {Turtle.Speed}";
        }

        public void StopTurtle()
        {
            Turtle.Stop();
            TurtleOutput = $"Черепаха: {Turtle.Speed}";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}