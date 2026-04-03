using System.Windows;
using AnimalsApp.ViewModels;

namespace AnimalsApp.Views
{
    public partial class MainWindow : Window
    {
        private MainViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainViewModel();
            DataContext = _vm;
        }

        private void MoveDog_Click(object sender, RoutedEventArgs e) => _vm.MoveDog();
        private void StopDog_Click(object sender, RoutedEventArgs e) => _vm.StopDog();
        private void BarkDog_Click(object sender, RoutedEventArgs e) => _vm.BarkDog();

        private void MovePanther_Click(object sender, RoutedEventArgs e) => _vm.MovePanther();
        private void StopPanther_Click(object sender, RoutedEventArgs e) => _vm.StopPanther();
        private void RoarPanther_Click(object sender, RoutedEventArgs e) => _vm.RoarPanther();
        private void ClimbTree_Click(object sender, RoutedEventArgs e) => _vm.ClimbTree();

        private void MoveTurtle_Click(object sender, RoutedEventArgs e) => _vm.MoveTurtle();
        private void StopTurtle_Click(object sender, RoutedEventArgs e) => _vm.StopTurtle();
    }
}