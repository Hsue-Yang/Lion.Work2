using System.Timers;

namespace LionTech.EDIService.ERP
{
    partial class ERP
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.timer1 = new System.Timers.Timer();
            this.timer2 = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timer2)).BeginInit();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10000D;
            this.timer1.Elapsed += new ElapsedEventHandler(this.timer1_Elapsed);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1D;
            this.timer2.Elapsed += new ElapsedEventHandler(this.timer2_Elapsed);
            // 
            // ERP
            // 
            this.ServiceName = "ERP";
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timer2)).EndInit();
        }

        #endregion

        private System.Timers.Timer timer1;
        private System.Timers.Timer timer2;
    }
}
