using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DPFP;
using DPFP.Capture;
using DPFP.Processing;
using DPFP.Verification;
using monitor.Data;

namespace monitor
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window, DPFP.Capture.EventHandler
    {
        private Template Template;
        private Verification Verificator;
        private Capture Capturer;
        private UsuarioRepository _usuarioRepository;

        public Login()
        {
            InitializeComponent();
            Loaded += Login_Loaded;
            Unloaded += Login_Unloaded;
        }

        private void Login_Unloaded(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
            Start();
            _usuarioRepository = new UsuarioRepository();
        }

        protected virtual void Init()
        {
            try
            {
                Verificator = new Verification();     // Create a fingerprint template verificator
                Capturer = new Capture();				// Create a capture operation.

                if (null != Capturer)
                    Capturer.EventHandler = this;					// Subscribe for capturing events.
            }
            catch
            {
                //MessageBox.Show("Can't initiate capture operation!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected void Start()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.StartCapture();
                }
                catch
                {
                    //("Can't initiate capture!");
                }
            }
        }

        protected virtual void Process(Sample Sample)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                lblNotValidUser.Visibility = Visibility.Hidden;
            });
            // Process the sample and create a feature set for the enrollment purpose.
            FeatureSet features = ExtractFeatures(Sample, DataPurpose.Verification);

            // Check quality of the sample and start verification if it's good
            if (features!= null)
            {
                foreach (Usuario usuario in _usuarioRepository.GetUsuariosLogin() ?? new List<Usuario>())
                {
                    if (usuario.HuellaDigital != null && usuario.HuellaDigital.Length > 0)
                    {
                        Template = new Template();
                        Template.DeSerialize(usuario.HuellaDigital);
                        // Compare the feature set with our template
                        Verification.Result result = new Verification.Result();
                        Verificator.Verify(features, Template, ref result);
                        //UpdateStatus(result.FARAchieved);
                        if (result.Verified)
                        {
                            Application.Current.Dispatcher.Invoke(delegate
                            {
                                App.usuario = usuario;
                                MainWindow main = new MainWindow();
                                Application.Current.MainWindow = main;
                                Stop();
                                Close();
                                main.Show();
                            });
                            return;
                        }
                    }
                }
                Application.Current.Dispatcher.Invoke(delegate
                {
                    lblNotValidUser.Visibility = Visibility.Visible;
                });
            }
        }

        protected FeatureSet ExtractFeatures(Sample Sample, DataPurpose Purpose)
        {
            FeatureExtraction Extractor = new FeatureExtraction(); 
            CaptureFeedback feedback = CaptureFeedback.None;
            FeatureSet features = new FeatureSet();

            Extractor.CreateFeatureSet(Sample, Purpose, ref feedback, ref features);            
            if (feedback == CaptureFeedback.Good)
            {
                return features;
            }

            return null;
        }

        public void Verify(Template template)
        {
            Template = template;
            ShowDialog();
        }

        protected void Stop()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.StopCapture();
                }
                catch
                {
                    //SetPrompt("Can't terminate capture!");
                }
            }
        }

        public void OnComplete(object Capture, string ReaderSerialNumber, Sample Sample)
        {
            Process(Sample);
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, CaptureFeedback CaptureFeedback)
        {
        }
    }
}
