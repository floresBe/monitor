using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPFP;
using DPFP.Capture;
using DPFP.Processing;

namespace monitor.Data
{
    public class FingerprintManager : DPFP.Capture.EventHandler
    {
        public int stateEnrroller = 0;     // controla el estado del proceso de incripción.
        private Capture Capturer;
        public Enrollment Enroller;
        public string typeProcces = "checkin"; // tipo de proceso que ejecutara el lector (register/validation/checkin) 

        public FingerprintManager()
        {
            Init();
            Start();

        }

        protected virtual void Init()
        {
            try
            {
                Enroller = new Enrollment();            // Create an enrollment.
                Capturer = new Capture(Priority.Low);              // Create a capture operation.

                UpdateStatus();
                if (null != Capturer)
                    Capturer.EventHandler = this;                   // Subscribe for capturing events.
            }
            catch
            {

            }
        }

        private void UpdateStatus()
        {
            this.stateEnrroller = (int)Enroller.FeaturesNeeded;
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

                }
            }
        }

        protected void SetPrompt(string prompt)
        {
            //this.Invoke(new Function(delegate ()
            //{
            //    //Prompt.Text = prompt;
            //}));
        }

        public void OnComplete(object Capture, string ReaderSerialNumber, Sample Sample)
        {
            //MakeReport("Huella capturada correctamente.");
            //SetPrompt("Capture la misma huella nuevamente.");
            Process(Sample);
        }

        protected virtual void Process(DPFP.Sample Sample)
        {
            // Draw fingerprint sample image.
            DrawPicture(ConvertSampleToBitmap(Sample));
        }

        private void DrawPicture(Bitmap bitmap)
        {
            //this.Invoke(new Function(delegate ()
            //{
            //    Picture.Image = new Bitmap(bitmap, Picture.Size);   // fit the image into the picture box
            //}));
        }

        protected Bitmap ConvertSampleToBitmap(DPFP.Sample Sample)
        {
            DPFP.Capture.SampleConversion Convertor = new DPFP.Capture.SampleConversion();  // Create a sample convertor.
            Bitmap bitmap = null;
            Convertor.ConvertToPicture(Sample, ref bitmap);
            return bitmap;
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            throw new NotImplementedException();
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            //throw new NotImplementedException();
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            //throw new NotImplementedException();
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            throw new NotImplementedException();
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, CaptureFeedback CaptureFeedback)
        {
            throw new NotImplementedException();
        }
    }
}
