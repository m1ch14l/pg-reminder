using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pgReminder
{
    public partial class Form1 : Form
    {
        private Timer timer1;
        private Timer timer2;
        private Boolean alreadySeen = false;

        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(int hWnd);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String newTime = maskedTextBox_time.Text;
            newTime = newTime.Replace(' ', '0');
            while(newTime.Length < 5)
            {
                newTime = newTime + "0";
            }
            this.dataGridView1.Rows.Add(1, newTime);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitTimer();

            
        }

        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000;
            timer1.Start();

            timer2 = new Timer();
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Interval = 60100;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            alreadySeen = false;
            timer2.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.TopMost = false;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if(row.Cells["c_enabled"].Value != null && alreadySeen == false) { 
                    String rowTime = row.Cells["c_time"].Value.ToString();
                    String curTime = DateTime.Now.ToString("HH:mm");
                    if (rowTime.Equals(curTime))
                    {
                        showMessage();
                    }
                }
            }
        }

        private void showMessage()
        {
            if (alreadySeen == false) {
                alreadySeen = true;

                SetForegroundWindow(Handle.ToInt32());
                this.TopMost = true;
                this.Focus();

                System.Media.SystemSounds.Asterisk.Play();
                MessageBox.Show(textBox_input.Text,
                    "PG's reminder",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);

                timer2.Start();
            }
        }
    }
}
