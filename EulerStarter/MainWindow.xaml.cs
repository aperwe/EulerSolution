using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EulerProblems;
using QBits.Intuition.Mathematics;
using QBits.Intuition.UI;

namespace EulerStarter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var classLoader = new ProblemSolverClassLoader();
            var solvers = classLoader.LoadProblemSolvers();
            CreateButtonsForSolvers(solvers);
        }

        private void CreateButtonsForSolvers(IEnumerable<SolverInfo> solvers)
        {
            foreach (var si in solvers)
            {
                Button button = new Button { Content = si.DisplayName };
                button.Style = buttonsPanel.Resources["problemButton"] as Style;
                button.Click += GenericClickHandler;
                //Make Button.Tag property as SolverInfo object.
                button.Tag = si;
                buttonsPanel.Children.Add(button);
            }
        }

        private void GenericClickHandler(object sender, RoutedEventArgs e)
        {
            var si = ((Button)sender).Tag as SolverInfo;
            if (si == null || si.TypeInfo == null)
            {
                MessageBox.Show("Solver information is missing or incomplete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new InvalidOperationException("Solver information is missing or incomplete.");
            }
            var problemType = si.TypeInfo;
            this.textBoxProblemDefinition.Text = "[MOVE TO RESOURCE FILE] Problem definition (if provided) will be displayed here.";
            if (!string.IsNullOrEmpty(si.ProblemDescription)) { this.textBoxProblemDefinition.Text = si.ProblemDescription; }
            var problem = Activator.CreateInstance(problemType) as AbstractEulerProblem;
            if (problem == null)
            {
                MessageBox.Show("Failed to create problem instance.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new InvalidOperationException("Failed to create problem instance.");
            }
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            Task result = Task.Run(() => problem.StartSolving());
        }

        private void UpdateAnswerUI(object? sender, AnswerAgr e)
        {
            this.InvokeOnUIThread(() => textBoxAnswer.Text = e.Answer);
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
