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
        }

        #region Problem solver callers

        private void Problem1Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem1();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();

        }

        private void Problem2Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem2();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem3Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem3();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();

        }

        private void Problem4Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem4();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem6Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem6();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem7Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem7();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem8Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem8();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem10Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem10();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem11Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem11();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem12Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem12();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem13Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem13();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem14Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem14();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem15Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem15();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem16Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem16();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem17Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem17();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem18Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem18();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem19Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem19();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem20Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem20();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem21Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem21();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem22Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem22();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem23Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem23();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem24Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem24();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        private void Problem67Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractEulerProblem problem = new EulerProblem67();
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            problem.Solve();
        }

        #endregion

        private void UpdateAnswerUI(object sender, AnswerAgr e)
        {
            textBlock.Text = e.Answer;
        }
    }
}
