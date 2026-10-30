using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EulerProblems;
using QBits.Intuition.Mathematics;
using QBits.Intuition.UI;
using System.Threading;
using System.ComponentModel;
using EulerProblems.Storage;

namespace EulerStarter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource? searchStop;
        private bool busy;
        private bool closeAfterStop;
        private AbstractEulerProblem? activeProblem;
        private AbstractEulerProblem? selectedProblem;
        private string selectedName = "";

        public MainWindow()
        {
            InitializeComponent();
            var classLoader = new ProblemSolverClassLoader();
            var solvers = classLoader.LoadProblemSolvers();
            CreateButtonsForSolvers(solvers);
            Closing += WindowClosing;
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
            if (busy) return;
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
            selectedProblem = problem;
            selectedName = si.DisplayName;
            textBoxAnswer.Text = $"Selected {selectedName}. Choose Run to start.";
            SetBusy(false);
        }

        private async void Run_Click(object sender, RoutedEventArgs e)
        {
            if (busy || selectedProblem == null) return;
            var problem = selectedProblem;
            bool bounded = boundedCheck.IsChecked == true;
            activeProblem = problem;
            SetBusy(true);
            problem.AnswerAvailableEventHandler += UpdateAnswerUI;
            try
            {
                if (problem is IManagedProblem managed)
                {
                    searchStop = new CancellationTokenSource();
                    stopButton.IsEnabled = true;
                    await managed.RunAsync(bounded, searchStop.Token);
                }
                else await Task.Run(problem.StartSolving);
            }
            catch (OperationCanceledException) when (searchStop?.IsCancellationRequested == true)
            {
                // The search has already published its final saved-coverage report.
            }
            catch (Exception error)
            {
                closeAfterStop = false; // Keep errors visible, especially a failed final save.
                textBoxAnswer.Text += $"\nRun failed: {error.Message}";
                if (error is CheckpointRecoveryRequiredException recovery &&
                    MessageBox.Show("Restore the shared checkpoint backup? Progress for ALL solvers may roll back. " +
                        "Stop other application instances first. The current file will be archived.",
                        "Shared checkpoint recovery", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        await Task.Run(() => CheckpointRecordStore.RestoreBackup(recovery.DirectoryPath, true));
                        textBoxAnswer.Text += "\nBackup restored. Select Run to resume.";
                    }
                    catch (Exception restoreError) { textBoxAnswer.Text += $"\nRecovery failed: {restoreError.Message}"; }
                }
            }
            finally
            {
                problem.AnswerAvailableEventHandler -= UpdateAnswerUI;
                activeProblem = null;
                searchStop?.Dispose();
                searchStop = null;
                SetBusy(false);
                if (closeAfterStop) Close();
            }
        }

        private void UpdateAnswerUI(object? sender, AnswerAgr e)
        {
            this.InvokeOnUIThread(() => {
                if (ReferenceEquals(sender, activeProblem)) textBoxAnswer.Text = e.Answer;
            });
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SetBusy(bool value)
        {
            busy = value;
            buttonsPanel.IsEnabled = !value;
            runButton.IsEnabled = !value && selectedProblem != null;
            boundedCheck.IsEnabled = !value && selectedProblem is IManagedProblem;
            freshStartButton.IsEnabled = !value && selectedProblem is IManagedProblem;
            if (!value) stopButton.IsEnabled = false;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            stopButton.IsEnabled = false;
            searchStop?.Cancel();
        }

        private void WindowClosing(object? sender, CancelEventArgs e)
        {
            if (searchStop == null) return;
            e.Cancel = true;
            closeAfterStop = true;
            stopButton.IsEnabled = false;
            searchStop.Cancel(); // The async handler closes after workers and save finish.
        }

        private async void FreshStart_Click(object sender, RoutedEventArgs e)
        {
            if (busy || selectedProblem is not IManagedProblem managed) return;
            bool bounded = boundedCheck.IsChecked == true;
            if (MessageBox.Show($"Archive saved progress for {selectedName} ({(bounded ? "bounded" : "normal")} run)?\n" +
                "The next run will start fresh.", "Fresh start",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            SetBusy(true);
            try
            {
                textBoxAnswer.Text = await managed.ArchiveProgressAsync(bounded);
            }
            catch (Exception error) { textBoxAnswer.Text = $"Fresh start failed: {error.Message}"; }
            finally { SetBusy(false); }
        }
    }
}
