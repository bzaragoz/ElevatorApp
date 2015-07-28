using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
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
using System.Windows.Threading;

using ElevatorManagementSystem;

namespace ElevatorWPFApp{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window{
        // Fields
        static private ElevatorLibrary elevatorLibrary;
        static private List<BackgroundWorker> bWorkerList;
        static private Dictionary<string, int> activeElevators;
        static private SoundPlayer alarmPlayer;
        static private bool alarmOn;
        private DispatcherTimer dTimer;

        // Main window
        public MainWindow(){
            InitializeComponent();
            elevatorLibrary = new ElevatorLibrary();
            bWorkerList = new List<BackgroundWorker>();
            activeElevators = new Dictionary<string, int>();
            alarmPlayer = new SoundPlayer(@"C:\Users\Byron\Documents\Visual Studio 2013\Projects\Elevator\Alarm.wav");
            alarmOn = false;
            InitializeDTimer();
            createElevators();
        }

        // Initialize background worker
        private void InitializeBWorker() {
            BackgroundWorker bWorker = new BackgroundWorker();
            bWorker.DoWork += backgroundWorker_DoWork;
            bWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            bWorkerList.Add(bWorker);
        }

        // Initialize dispatch timer
        private void InitializeDTimer(){
            dTimer = new DispatcherTimer();
            dTimer.Tick += new EventHandler(dTimer_Tick);
            dTimer.Interval = new TimeSpan(0, 0, 1);
        }

        // Create elevators
        private void createElevators(){
            elevatorLibrary.CreateElevator("Kone");
            elevatorLibrary.CreateElevator("Otis");
            elevatorLibrary.CreateElevator("Schindler");
            elevatorLibrary.CreateElevator("Thyssen Krupp");
        }

        // Click Floor Button
        private void btn_Click(object sender, RoutedEventArgs e){
            string selectedElevator = ((ComboBoxItem)cboxElevator.SelectedItem).Content.ToString();

            if (elevatorLibrary.GetElevator(selectedElevator).Opened)
                MessageBox.Show("Error: \"" + selectedElevator + "\" doors are currently open.");
            else if (!activeElevators.ContainsKey(selectedElevator)){
                InitializeBWorker();
                
                KeyValuePair<string, int> activeElevator = new KeyValuePair<string,int>(selectedElevator, Convert.ToInt32(((Button)sender).Content));
                activeElevators.Add(activeElevator.Key, activeElevator.Value);
                switch (activeElevator.Key) {
                    case "Kone": lblKoneStop.Foreground = new SolidColorBrush(Color.FromArgb(255, 161, 4, 4));
                                 lblKoneStatus.Foreground = new SolidColorBrush(Color.FromArgb(255,182,154,6));
                                 lblKoneStatus.Content = "Moving"; break;
                    case "Otis": lblOtisStop.Foreground = new SolidColorBrush(Color.FromArgb(255, 161, 4, 4));
                                 lblOtisStatus.Foreground = new SolidColorBrush(Color.FromArgb(255,182,154,6));
                                 lblOtisStatus.Content = "Moving"; break;
                    case "Schindler": lblSchindlerStop.Foreground = new SolidColorBrush(Color.FromArgb(255, 161, 4, 4));
                                      lblSchindlerStatus.Foreground = new SolidColorBrush(Color.FromArgb(255,182,154,6));
                                      lblSchindlerStatus.Content = "Moving"; break;
                    case "Thyssen Krupp": lblThyssenKruppStop.Foreground = new SolidColorBrush(Color.FromArgb(255, 161, 4, 4));
                                          lblThyssenKruppStatus.Foreground = new SolidColorBrush(Color.FromArgb(255,182,154,6));
                                          lblThyssenKruppStatus.Content = "Moving"; break;
                }
                
                bWorkerList.Last().RunWorkerAsync(activeElevator);
                if (!dTimer.IsEnabled)
                    dTimer.Start();
            } else {
                MessageBox.Show("Error: \"" + selectedElevator + "\" is currently in motion.");
            }
        }

        // Timer tick function
        private void dTimer_Tick(object sender, EventArgs e){
            updateElevatorLabels();
            if (!activeElevators.Any())
                dTimer.Stop();
        }

        // Update elevator labels
        private void updateElevatorLabels(){
            foreach (KeyValuePair<string, int> item in activeElevators){
                switch (item.Key){
                    case "Kone": lblKoneFloor.Content = elevatorLibrary.GetElevator(item.Key).CurrentFloor.ToString(); break;
                    case "Otis": lblOtisFloor.Content = elevatorLibrary.GetElevator(item.Key).CurrentFloor.ToString(); break;
                    case "Schindler": lblSchindlerFloor.Content = elevatorLibrary.GetElevator(item.Key).CurrentFloor.ToString(); break;
                    case "Thyssen Krupp": lblThyssenKruppFloor.Content = elevatorLibrary.GetElevator(item.Key).CurrentFloor.ToString(); break;
                }
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e){
            KeyValuePair<string, int> activeElevator = (KeyValuePair<string, int>)e.Argument;
            e.Result = activeElevator.Key;
            elevatorLibrary.GetElevator(activeElevator.Key).Goto(activeElevator.Value);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e){
            updateElevatorLabels();
            string activeElevator = e.Result.ToString();
            activeElevators.Remove(activeElevator);
            switch (activeElevator){
                case "Kone": lblKoneStop.Foreground = Brushes.Gray;
                             lblKoneStatus.Foreground = new SolidColorBrush(Color.FromArgb(255, 42, 182, 6));
                             lblKoneStatus.Content = "Ready"; break;
                case "Otis": lblOtisStop.Foreground = Brushes.Gray;
                             lblOtisStatus.Foreground = new SolidColorBrush(Color.FromArgb(255, 42, 182, 6));
                             lblOtisStatus.Content = "Ready"; break;
                case "Schindler": lblSchindlerStop.Foreground = Brushes.Gray;
                                  lblSchindlerStatus.Foreground = new SolidColorBrush(Color.FromArgb(255, 42, 182, 6));
                                  lblSchindlerStatus.Content = "Ready"; break;
                case "Thyssen Krupp": lblThyssenKruppStop.Foreground = Brushes.Gray;
                                      lblThyssenKruppStatus.Foreground = new SolidColorBrush(Color.FromArgb(255, 42, 182, 6));
                                      lblThyssenKruppStatus.Content = "Ready"; break;
            }
        }

        private void lblStop_MouseDoubleClick(object sender, RoutedEventArgs e){
            string btnName = ((Label)sender).Name;

            if (((Label)sender).Foreground != Brushes.Gray){
                switch (btnName){
                    case "lblKoneStop":         elevatorLibrary.GetElevator("Kone").Stop(); break;
                    case "lblOtisStop":         elevatorLibrary.GetElevator("Otis").Stop(); break;
                    case "lblSchindlerStop":    elevatorLibrary.GetElevator("Schindler").Stop(); break;
                    case "lblThyssenKruppStop": elevatorLibrary.GetElevator("Thyssen Krupp").Stop(); break;
                }
                ((Label)sender).Foreground = Brushes.Gray;
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e){
            string selectedElevator = ((ComboBoxItem)cboxElevator.SelectedItem).Content.ToString();

            if (!activeElevators.ContainsKey(selectedElevator)) {
                elevatorLibrary.GetElevator(selectedElevator).Open();
                switch (selectedElevator) {
                    case "Kone": lblKoneStatus.Foreground = new SolidColorBrush(Color.FromArgb(255,182,154,6));
                                 lblKoneStatus.Content = "Open"; break;
                    case "Otis": lblOtisStatus.Foreground = new SolidColorBrush(Color.FromArgb(255,182,154,6));
                                 lblOtisStatus.Content = "Open"; break;
                    case "Schindler": lblSchindlerStatus.Foreground = new SolidColorBrush(Color.FromArgb(255,182,154,6));
                                      lblSchindlerStatus.Content = "Open"; break;
                    case "Thyssen Krupp": lblThyssenKruppStatus.Foreground = new SolidColorBrush(Color.FromArgb(255,182,154,6));
                                          lblThyssenKruppStatus.Content = "Open"; break;
                }
            } else
                MessageBox.Show("Error: \"" + selectedElevator + "\" is currently in motion.");
        }

        private void btnClose_Click(object sender, RoutedEventArgs e){
            string selectedElevator = ((ComboBoxItem)cboxElevator.SelectedItem).Content.ToString();

            if (!activeElevators.ContainsKey(selectedElevator)){
                elevatorLibrary.GetElevator(selectedElevator).Close();
                switch (selectedElevator){
                    case "Kone": lblKoneStatus.Foreground = new SolidColorBrush(Color.FromArgb(255, 42, 182, 6));
                                 lblKoneStatus.Content = "Ready"; break;
                    case "Otis": lblOtisStatus.Foreground = new SolidColorBrush(Color.FromArgb(255, 42, 182, 6));
                                 lblOtisStatus.Content = "Ready"; break;
                    case "Schindler": lblSchindlerStatus.Foreground = new SolidColorBrush(Color.FromArgb(255, 42, 182, 6));
                                      lblSchindlerStatus.Content = "Ready"; break;
                    case "Thyssen Krupp": lblThyssenKruppStatus.Foreground = new SolidColorBrush(Color.FromArgb(255, 42, 182, 6));
                                          lblThyssenKruppStatus.Content = "Ready"; break;
                }
            }
            else
                MessageBox.Show("Error: \"" + selectedElevator + "\" is currently in motion.");
        }

        private void btnAlarm_Click(object sender, RoutedEventArgs e){
            if (alarmOn){
                alarmPlayer.Stop(); alarmOn = false;
            } else{
                alarmPlayer.Play(); alarmOn = true;
            }
        }
    }
}