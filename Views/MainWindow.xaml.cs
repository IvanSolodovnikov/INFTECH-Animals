using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using AnimalModels;

namespace AnimalsApp.Views
{
    public partial class MainWindow : Window
    {
        private readonly List<Type> _animalTypes = new();
        private readonly List<TextBox> _constructorTextBoxes = new();
        private readonly List<TextBox> _methodTextBoxes = new();
        private readonly Dictionary<Type, object> _animals = new();

        public MainWindow()
        {
            InitializeComponent();
            LibraryPathTextBox.Text = GetDefaultLibraryPath();
        }

        private string GetDefaultLibraryPath()
        {
            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "AnimalModels.dll");
        }

        private void LoadModels_Click(object sender, RoutedEventArgs e)
        {
            ClassesListBox.Items.Clear();
            MethodsListBox.Items.Clear();
            ConstructorPanel.Children.Clear();
            MethodPanel.Children.Clear();
            ClassInfoTextBox.Text = "";
            ResultTextBox.Text = "";
            _animalTypes.Clear();
            _animals.Clear();

            try
            {
                string path = LibraryPathTextBox.Text.Trim();
                if (!File.Exists(path))
                {
                    ResultTextBox.Text = "Файл не найден";
                    return;
                }

                Assembly assembly = Assembly.LoadFrom(path);
                Type animalInterface = typeof(IAnimal);

                List<Type> types = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .Where(t => animalInterface.IsAssignableFrom(t))
                    .ToList();

                foreach (Type type in types)
                {
                    _animalTypes.Add(type);
                    ClassesListBox.Items.Add(type.Name);
                }

                ResultTextBox.Text = $"Найдено животных в Models: {types.Count}";
            }
            catch (Exception ex)
            {
                ResultTextBox.Text = ex.Message;
            }
        }

        private void ClassesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MethodsListBox.Items.Clear();
            ConstructorPanel.Children.Clear();
            MethodPanel.Children.Clear();
            _constructorTextBoxes.Clear();
            _methodTextBoxes.Clear();

            Type? type = GetSelectedType();
            if (type == null)
                return;

            if (_animals.ContainsKey(type))
            {
                ConstructorPanel.Children.Add(new TextBlock
                {
                    Text = "Объект уже создан. Его данные сохраняются."
                });
            }
            else
            {
                ConstructorInfo? constructor = GetConstructor(type);
                if (constructor != null)
                    AddParameterInputs(ConstructorPanel, _constructorTextBoxes, constructor.GetParameters());
            }

            List<MethodInfo> methods = GetMethods(type);
            foreach (MethodInfo method in methods)
                MethodsListBox.Items.Add(method.Name);

            ClassInfoTextBox.Text = GetClassInfo(type);
        }

        private void MethodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MethodPanel.Children.Clear();
            _methodTextBoxes.Clear();

            MethodInfo? method = GetSelectedMethod();
            if (method != null)
                AddParameterInputs(MethodPanel, _methodTextBoxes, method.GetParameters());
        }

        private void CreateObject_Click(object sender, RoutedEventArgs e)
        {
            Type? type = GetSelectedType();
            if (type == null)
            {
                ResultTextBox.Text = "Выберите класс";
                return;
            }

            try
            {
                object animal = CreateObject(type);
                ResultTextBox.Text = $"Объект создан. Скорость: {GetSpeed(animal)}";
            }
            catch (Exception ex)
            {
                ResultTextBox.Text = ex.InnerException?.Message ?? ex.Message;
            }
        }

        private void RunMethod_Click(object sender, RoutedEventArgs e)
        {
            Type? type = GetSelectedType();
            MethodInfo? method = GetSelectedMethod();

            if (type == null || method == null)
            {
                ResultTextBox.Text = "Выберите класс и метод";
                return;
            }

            try
            {
                if (!_animals.ContainsKey(type))
                    CreateObject(type);

                object?[] methodParameters = GetValues(
                    method.GetParameters(),
                    _methodTextBoxes);

                object? result = method.Invoke(_animals[type], methodParameters);

                ResultTextBox.Text = GetResultText(result, _animals[type]);
            }
            catch (Exception ex)
            {
                ResultTextBox.Text = ex.InnerException?.Message ?? ex.Message;
            }
        }

        private Type? GetSelectedType()
        {
            int index = ClassesListBox.SelectedIndex;
            if (index < 0 || index >= _animalTypes.Count)
                return null;

            return _animalTypes[index];
        }

        private ConstructorInfo? GetConstructor(Type type)
        {
            return type.GetConstructors().FirstOrDefault();
        }

        private object CreateObject(Type type)
        {
            if (_animals.ContainsKey(type))
                return _animals[type];

            ConstructorInfo? constructor = GetConstructor(type);
            if (constructor == null)
                throw new Exception("У класса нет публичного конструктора");

            object?[] constructorParameters = GetValues(
                constructor.GetParameters(),
                _constructorTextBoxes);

            object animal = constructor.Invoke(constructorParameters);
            _animals.Add(type, animal);

            ConstructorPanel.Children.Clear();
            ConstructorPanel.Children.Add(new TextBlock
            {
                Text = "Объект уже создан. Его данные сохраняются."
            });

            return animal;
        }

        private MethodInfo? GetSelectedMethod()
        {
            Type? type = GetSelectedType();
            if (type == null || MethodsListBox.SelectedItem == null)
                return null;

            string methodName = MethodsListBox.SelectedItem.ToString() ?? "";

            return GetMethods(type).FirstOrDefault(m => m.Name == methodName);
        }

        private List<MethodInfo> GetMethods(Type type)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => !m.IsSpecialName && m.DeclaringType != typeof(object))
                .ToList();
        }

        private string GetClassInfo(Type type)
        {
            string properties = string.Join(", ", type.GetProperties().Select(p => $"{p.PropertyType.Name} {p.Name}"));
            string methods = string.Join(", ", GetMethods(type).Select(m => m.Name));
            string constructors = string.Join(", ", type.GetConstructors().Select(c => $"{type.Name}({GetParametersText(c.GetParameters())})"));

            return $"Название: {type.FullName}{Environment.NewLine}" +
                   $"Конструкторы: {constructors}{Environment.NewLine}" +
                   $"Свойства: {properties}{Environment.NewLine}" +
                   $"Методы: {methods}";
        }

        private string GetParametersText(ParameterInfo[] parameters)
        {
            return string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
        }

        private void AddParameterInputs(StackPanel panel, List<TextBox> textBoxes, ParameterInfo[] parameters)
        {
            if (parameters.Length == 0)
            {
                panel.Children.Add(new TextBlock { Text = "Параметров нет" });
                return;
            }

            foreach (ParameterInfo parameter in parameters)
            {
                TextBlock label = new TextBlock
                {
                    Text = $"{parameter.Name} ({parameter.ParameterType.Name})",
                    Margin = new Thickness(0, 5, 0, 0)
                };

                TextBox textBox = new TextBox
                {
                    Margin = new Thickness(0, 2, 0, 5)
                };

                panel.Children.Add(label);
                panel.Children.Add(textBox);
                textBoxes.Add(textBox);
            }
        }

        private object?[] GetValues(ParameterInfo[] parameters, List<TextBox> textBoxes)
        {
            object?[] values = new object?[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                string text = textBoxes[i].Text;
                Type parameterType = parameters[i].ParameterType;

                if (parameterType == typeof(string))
                    values[i] = text;
                else if (parameterType == typeof(int))
                    values[i] = int.Parse(text);
                else if (parameterType == typeof(double))
                    values[i] = double.Parse(text, CultureInfo.InvariantCulture);
                else if (parameterType == typeof(bool))
                    values[i] = bool.Parse(text);
                else
                    values[i] = Convert.ChangeType(text, parameterType, CultureInfo.InvariantCulture);
            }

            return values;
        }

        private string GetResultText(object? result, object animal)
        {
            if (result != null)
                return result.ToString() ?? "";

            return $"Метод выполнен. Скорость: {GetSpeed(animal)}";
        }

        private object? GetSpeed(object animal)
        {
            PropertyInfo? speedProperty = animal.GetType().GetProperty("Speed");
            if (speedProperty != null)
                return speedProperty.GetValue(animal);

            return "";
        }
    }
}
