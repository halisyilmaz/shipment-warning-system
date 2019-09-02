using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;

namespace Shipment_Warning_System
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("Lütfen ağa bağlanın!");
            }
            refreshForm();
        }

        public Line[] lines = new Line[26]; //Creates a object array for the number of lines

        public class Line
        {
            public GroupBox Name { get; set; }
            public string IP { get; set; }
            public Label EmptyBoxLabel { get; set; }
            public Label FullBoxLabel { get; set; }
            public Label ConnLabel { get; set; }
            public bool prevStateEmpty { get; set; }
            public bool prevStateFull { get; set; }
        }

        private void refreshForm()
        {
            var BoxArray = new[] { lineBox1, lineBox2, lineBox3, lineBox4, lineBox5, lineBox6,
                lineBox7, lineBox8, lineBox9, lineBox10, lineBox11, lineBox12, lineBox13, lineBox14,
                lineBox15, lineBox16, lineBox17, lineBox18, lineBox19, lineBox20  , lineBox21  , lineBox22,
                lineBox23, lineBox24, lineBox25, lineBox26};
            var EmptyBoxArray = new[] { emptyLabel1, emptyLabel2, emptyLabel3, emptyLabel4, emptyLabel5, emptyLabel6,
                emptyLabel7, emptyLabel8, emptyLabel9, emptyLabel10, emptyLabel11, emptyLabel12, emptyLabel13, emptyLabel14,
                emptyLabel15, emptyLabel16, emptyLabel17, emptyLabel18, emptyLabel19, emptyLabel20  , emptyLabel21  , emptyLabel22,
                emptyLabel23, emptyLabel24, emptyLabel25, emptyLabel2};
            var FullBoxArray = new[] { fullLabel1, fullLabel2, fullLabel3, fullLabel4, fullLabel5, fullLabel6,
                fullLabel7, fullLabel8, fullLabel9, fullLabel10, fullLabel11, fullLabel2, fullLabel13, fullLabel14,
                fullLabel15, fullLabel16, fullLabel17, fullLabel18, fullLabel19, fullLabel20  , fullLabel21  , fullLabel22,
                fullLabel23, fullLabel24, fullLabel25, fullLabel26};
            var ConnLabelArray = new[] { connLabel1, connLabel2, connLabel3, connLabel4, connLabel5, connLabel6,
                connLabel7, connLabel8, connLabel9, connLabel10, connLabel11, connLabel12, connLabel13, connLabel14,
                connLabel15, connLabel16, connLabel17, connLabel18, connLabel19, connLabel20  , connLabel21  , connLabel22,
                connLabel23, connLabel24, connLabel25, connLabel26};
            try
            {
                pictureBox1.BackgroundImage = Image.FromFile(Properties.Settings.Default.layoutPath);
            }
            catch
            {
                MessageBox.Show("Layout Yüklenemedi!");
            }
            

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = new Line();
                lines[i].Name = BoxArray[i];
                lines[i].EmptyBoxLabel = EmptyBoxArray[i];
                lines[i].FullBoxLabel = FullBoxArray[i];
                lines[i].ConnLabel = ConnLabelArray[i];
                lines[i].IP = Properties.Settings.Default.lineIP[i];
                lines[i].prevStateEmpty = false;
                lines[i].prevStateFull = false;

                if (!String.IsNullOrEmpty(lines[i].IP))
                {
                    lines[i].Name.Visible = true;
                    lines[i].ConnLabel.Visible = true;
                    lines[i].Name.Text = Properties.Settings.Default.lineName[i];
                }
                else
                {
                    lines[i].Name.Visible = false;
                    lines[i].ConnLabel.Visible = false;
                }
            }

        }
        
        private void blinkLabel(Label label,bool startStop)
        {
            if (startStop == true)
            {
                if (label.BackColor == System.Drawing.Color.Transparent)
                {
                    label.BackColor = System.Drawing.Color.Red;
                    label.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    label.BackColor = System.Drawing.Color.Transparent;
                    label.ForeColor = System.Drawing.Color.Black;
                }
            }
            else
            {
                label.BackColor = System.Drawing.Color.Transparent;
                label.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void logAdd(string lineName, string state)
        {
            switch (state)
            {
                case "emptyStart":
                    this.logData.Rows.Insert(0, DateTime.Now.ToString("h:mm:ss tt"), lineName + " boş kutu bekliyor");
                    logData.CurrentCell = logData.Rows[0].Cells[1];
                    break;
                case "emptyStop":
                    this.logData.Rows.Insert(0, DateTime.Now.ToString("h:mm:ss tt"), lineName + " boş kutu geldi");
                    logData.CurrentCell = logData.Rows[0].Cells[1];
                    break;
                case "fullStart":
                    this.logData.Rows.Insert(0, DateTime.Now.ToString("h:mm:ss tt"), lineName + " transpalet bekliyor");
                    logData.CurrentCell = logData.Rows[0].Cells[1];
                    break;
                case "fullStop":
                    this.logData.Rows.Insert(0, DateTime.Now.ToString("h:mm:ss tt"), lineName + " transpalet geldi");
                    logData.CurrentCell = logData.Rows[0].Cells[1];
                    break;
                case "conn":
                    this.logData.Rows.Add(DateTime.Now.ToString("h:mm:ss tt"), lineName+ " bağlandı.");
                    logData.CurrentCell = logData.Rows[0].Cells[1];
                    break;
            }
        }

        private bool Ping(string url)
        {
            Ping x = new Ping();
            PingReply reply = x.Send(IPAddress.Parse(url));

            if (reply.Status == IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string DownloadFromIP(string IP)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string content = client.DownloadString(IP);
                    return content;
                }
                catch
                {
                    return null; 
                }
            }
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings(); //Creates a settingsForm
            if (settings.ShowDialog() == DialogResult.OK)
            {
                refreshForm();
            }
        }

        public bool ValidateIP(string ipString)
        {
            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            timerRefresh.Enabled = false;

            for (int i = 0; i < lines.Length; i++)
            {
                if (!String.IsNullOrWhiteSpace(lines[i].IP))
                {
                    if (ValidateIP(lines[i].IP))
                    {
                        if (Ping(lines[i].IP))
                        {
                            lines[i].ConnLabel.Text = "ONLINE";
                            logAdd(lines[i].IP, "conn");
                            timerRefresh.Enabled = true;
                        }
                        else
                        {
                            lines[i].ConnLabel.Text = "OFFLINE";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Geçersiz IP: " + lines[i].IP);
                    }
                }
            }

        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (!String.IsNullOrWhiteSpace(lines[i].IP) && lines[i].ConnLabel.Text == "ONLINE")
                {
                    if (Ping(lines[i].IP))
                    {
                        string content = DownloadFromIP("http://" + lines[i].IP);
                        if (!String.IsNullOrEmpty(content))
                        {
                            if (content.IndexOf("kutu bekliyor!") != -1)
                            {
                                if (lines[i].prevStateEmpty == false)
                                {
                                    blinkLabel(lines[i].EmptyBoxLabel, true);
                                    logAdd(lines[i].IP, "emptyStart");
                                    lines[i].prevStateEmpty = true;
                                }
                            }
                            else
                            {
                                blinkLabel(lines[i].EmptyBoxLabel, false);
                                if (lines[i].prevStateEmpty == true)
                                {
                                    logAdd(lines[i].IP, "emptyStop");
                                }
                                lines[i].prevStateEmpty = false;

                            }

                            if (content.IndexOf("transpalet bekliyor!") != -1)
                            {
                                if (lines[i].prevStateFull == false)
                                {
                                    blinkLabel(lines[i].FullBoxLabel, true);
                                    logAdd(lines[i].IP, "fullStart");
                                    lines[i].prevStateFull = true;
                                }
                            }
                            else
                            {
                                blinkLabel(lines[i].FullBoxLabel, false);
                                if (lines[i].prevStateFull == true)
                                {
                                    logAdd(lines[i].IP, "fullStop");
                                }
                                lines[i].prevStateFull = false;
                            }
                        }
                    }
                    else
                    {
                        lines[i].ConnLabel.Text = "OFFLINE";
                        blinkLabel(lines[i].FullBoxLabel, false);
                        lines[i].prevStateFull = false;
                        lines[i].prevStateEmpty = false;
                    }
                }
            }


        }

    }
}
