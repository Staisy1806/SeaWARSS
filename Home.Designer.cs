using System.Windows.Forms;

namespace SeaWars
{
    partial class Home
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnPlay;
        private Button btnExit;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
        }
    }
}
