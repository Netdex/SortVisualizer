using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SortVisualizer.Algorithm;


namespace SortVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private SignalGenerator signalGenerator;
        private WaveOut waveOut;
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            signalGenerator = new SignalGenerator(44100, 1);
            signalGenerator.Gain = 0.5f;
            signalGenerator.Frequency = 2000;
            signalGenerator.Type = SignalGeneratorType.Triangle;

            waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback());
            waveOut.DesiredLatency = 50;
            waveOut.Init(signalGenerator);

        }

        private SortWrapper ActiveSortWrapper;
        private Rectangle[] Bars;

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            int N = 256;

            Bars = new Rectangle[N];
            double W = canvas.ActualWidth;
            double H = canvas.ActualHeight;
            for (int i = 0; i < N; i++)
            {
                Bars[i] = new Rectangle
                {
                    Width = W / N,
                    Fill = Brushes.White
                };
                Canvas.SetLeft(Bars[i], i * (W / N));
                Canvas.SetBottom(Bars[i], 0);
                canvas.Children.Add(Bars[i]);
            }
            ActiveSortWrapper = new SortWrapper(new MergeSort(), N);
            ActiveSortWrapper.OnStateChanged += (idx, itm) =>
            {
                Bars[idx].Dispatcher.Invoke(() =>
                {
                    Bars[idx].Height = (1.0 * itm.Value / N) * H;

                    switch (itm.State)
                    {
                        case CompareItem.SortState.None:
                            Bars[idx].Fill = Brushes.White;
                            break;
                        case CompareItem.SortState.Comparing:
                            Bars[idx].Fill = Brushes.Red;
                            break;
                        case CompareItem.SortState.Swapping:
                            Bars[idx].Fill = Brushes.LimeGreen;
                            break;
                    }
                });
            };
            ActiveSortWrapper.OnSortFinished += () =>
            {
                btnStart.Dispatcher.Invoke(() =>
                {
                    btnStart.IsEnabled = true;
                    btnReset.IsEnabled = true;
                });
                waveOut.Stop();
            };
            ActiveSortWrapper.OnSwap += (a, b) =>
            {
                signalGenerator.Frequency = Map(ActiveSortWrapper.Items[b].Value, 1, N, 100, 2000);
            };
            ActiveSortWrapper.Randomize();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveSortWrapper != null)
            {
                btnStart.IsEnabled = false;
                btnReset.IsEnabled = false;
                ActiveSortWrapper.SortAsync();
                waveOut.Play();
            }
        }

        private static double Map(double value, double fromSource, double toSource, double fromTarget, double toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

    }
}
