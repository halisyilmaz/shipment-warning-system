using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shipment_Warning_System
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private Line[] lines = new Line[26];    //Creates a object array for the number of lines
        
        public class Line
        {
            public TextBox Name { get; set; }
            public TextBox IP { get; set; }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            var NameArray = new[] { lineName1, lineName2, lineName3, lineName4, lineName5, lineName6,
                lineName7, lineName8, lineName9, lineName10, lineName11, lineName12, lineName13, lineName14,
                lineName15, lineName16, lineName17, lineName18, lineName19, lineName20  , lineName21  , lineName22,
                lineName23, lineName24, lineName25, lineName26};
            var IPArray = new[] {ipAddress1, ipAddress2, ipAddress3, ipAddress4, ipAddress5, ipAddress6,
                ipAddress7, ipAddress8, ipAddress9 , ipAddress10, ipAddress11, ipAddress12, ipAddress13, ipAddress14,
                ipAddress15, ipAddress16, ipAddress17, ipAddress18, ipAddress19, ipAddress20, ipAddress21, ipAddress22,
                ipAddress23, ipAddress24, ipAddress25, ipAddress26};
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = new Line();
                lines[i].Name = NameArray[i];
                lines[i].IP = IPArray[i];
                lines[i].Name.Text = Properties.Settings.Default.lineName[i];
                lines[i].IP.Text = Properties.Settings.Default.lineIP[i];
            }

            layoutPath.Text = Properties.Settings.Default.layoutPath; 
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                Properties.Settings.Default.lineName[i] = lines[i].Name.Text;
                Properties.Settings.Default.lineIP[i] = lines[i].IP.Text;
            }
            Properties.Settings.Default.layoutPath = layoutPath.Text;
            Properties.Settings.Default.Save();
            this.DialogResult = DialogResult.OK;
            saveLabel.Text = "Ayarlar kaydedildi.";
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            // open file dialog   
            OpenFileDialog open = new OpenFileDialog();
            // image filters  
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                layoutPath.Text = open.FileName;
            }
        }

        private void lineName1_TextChanged(object sender, EventArgs e)
        {

        }
        
    }
}
