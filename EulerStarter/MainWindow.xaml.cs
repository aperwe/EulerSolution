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
            var si = (sender as Button).Tag as SolverInfo;
            var problemType = si.TypeInfo;
            this.textBoxProblemDefinition.Text = "[MOVE TO RESOURCE FILE] Problem definition (if provided) will be displayed here.";
            if (!string.IsNullOrEmpty(si.ProblemDescription)) { this.textBoxProblemDefinition.Text = si.ProblemDescription; }
            var problem = Activator.CreateInstance(problemType) as AbstractEulerProblem;
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            Task result = Task.Run(() => problem.StartSolving());
        }

        private void UpdateAnswerUI(object sender, AnswerAgr e)
        {
            this.InvokeOnUIThread(() => textBoxAnswer.Text = e.Answer);
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
