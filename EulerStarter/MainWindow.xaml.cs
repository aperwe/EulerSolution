using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EulerProblems;
using EulerProblems.Problems;
using QBits.Intuition.Mathematics;

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
            //Test BigInteger
            BigInteger bi = 77795551;
            var p = bi.ToString();
            var h = bi.ToHexString();
            var test = ~bi;
            var test2 = -bi;
        }

        private void CreateButtonsForSolvers(IEnumerable<SolverInfo> solvers)
        {
            foreach (var si in solvers)
            {
                Button button = new Button { Content = si.DisplayName };
                button.Style = buttonsPanel.Resources["problemButton"] as Style;
                button.Click += GenericClickHandler;
                button.Tag = si.TypeInfo;
                buttonsPanel.Children.Add(button);
            }
        }

        private void GenericClickHandler(object sender, RoutedEventArgs e)
        {
            var problemType = (sender as Button).Tag as Type;
            var problem = Activator.CreateInstance(problemType) as AbstractEulerProblem;
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.StartSolving();
        }

        private void UpdateAnswerUI(object sender, AnswerAgr e)
        {
            textBoxAnswer.Text = e.Answer;
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
