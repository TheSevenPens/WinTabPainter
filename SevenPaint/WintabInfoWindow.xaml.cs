using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows;

namespace SevenPaint
{
    public partial class WintabInfoWindow : Window
    {
        public WintabInfoWindow()
        {
            InitializeComponent();
            LoadWintabInfo();
        }

        private void LoadWintabInfo()
        {
            try
            {
                string systemPath = Environment.SystemDirectory;
                string dllPath = System.IO.Path.Combine(systemPath, "wintab32.dll");

                if (!File.Exists(dllPath))
                {
                    TxtPath.Text = "wintab32.dll not found in System32.";
                    return;
                }

                TxtPath.Text = dllPath;

                // File Info
                FileInfo fi = new FileInfo(dllPath);
                TxtSize.Text = $"{fi.Length / 1024.0:F2} KB ({fi.Length} bytes)";

                // Version Info
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(dllPath);
                TxtFileVersion.Text = fvi.FileVersion ?? "N/A";
                TxtProductName.Text = fvi.ProductName ?? "N/A";
                TxtProductVersion.Text = fvi.ProductVersion ?? "N/A";
                TxtCopyright.Text = fvi.LegalCopyright ?? "N/A";

                // Signer
                try
                {
                    X509Certificate cert = X509Certificate.CreateFromSignedFile(dllPath);
                    TxtSigner.Text = cert.Subject;
                }
                catch
                {
                    TxtSigner.Text = "Not Signed or Unable to Read Signature";
                }
            }
            catch (Exception ex)
            {
                TxtPath.Text = $"Error reading info: {ex.Message}";
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
