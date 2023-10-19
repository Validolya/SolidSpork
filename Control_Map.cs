using OrbitTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Control_Map : UserControl
    {
        int interval, step=0, seconds = 1000, minutes = 60000, hours = 3600000;
        bool start;

        TimeSpan m_dt = new TimeSpan(0, 0, 1);

        public List<SatParameters> Satellites
        {
            set
            {
                checkedListBox_satellites.Items.Clear();
                checkedListBox_satellites.Items.AddRange(value.ToArray());
            }
        }

        public Control_Map()
        {
            InitializeComponent();
        }
       
        private void Control_Map_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;

            control_SatOnMap1.Time = dateTimePicker_sat.Value;
            control_SatOnMap1.SelectedSatChanged += OnSelectedSatChanged;
        }

        private void dateTimePicker_sat_ValueChanged(object sender, EventArgs e)
        {
            control_SatOnMap1.Time = dateTimePicker_sat.Value; 
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //step = Convert.ToInt32(textBox1.Text.Replace(".", ","));

            UpdateTimeSpan();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTimeSpan();
        }

        private void UpdateTimeSpan()
        { 
            interval = Convert.ToInt32(textBox1.Text.Replace(".", ","));

            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    m_dt = new TimeSpan(0, 0, interval);
                    break;
                case 1:
                    m_dt = new TimeSpan(0, interval, 0);
                    break;
                case 2:
                    m_dt = new TimeSpan(interval, 0, 0);
                    break;
            }
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != 44) // цифры, клавиша BackSpace и запятая
            {
                e.Handled = true;
            }
        }


        void intervalChange()
        {

            if (comboBox1.SelectedItem == null & string.IsNullOrEmpty(textBox1.Text)) 
            { 
                interval = 1000; 
                timer.Interval = interval;
                dateTimePicker_sat.Value = dateTimePicker_sat.Value.AddMilliseconds(timer.Interval);
            }
            else
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        //interval = step * seconds;
                        timer.Interval = interval;
                      // dateTimePicker_sat.Value = dateTimePicker_sat.Value.AddMilliseconds(timer.Interval);
                      dateTimePicker_sat.Value = dateTimePicker_sat.Value.AddSeconds(timer.Interval);

                        break;

                    case 1:
                       //interval= step * minutes;
                        timer.Interval = interval;
                       //dateTimePicker_sat.Value = dateTimePicker_sat.Value.AddMilliseconds(timer.Interval);
                       dateTimePicker_sat.Value = dateTimePicker_sat.Value.AddMinutes(timer.Interval);
                        break;

                    case 2:
                        //interval = step * hours;
                        timer.Interval = interval;
                        //dateTimePicker_sat.Value = dateTimePicker_sat.Value.AddMilliseconds(timer.Interval);
                        dateTimePicker_sat.Value = dateTimePicker_sat.Value.AddHours(timer.Interval);
                        break;
                }
            }

        }

        private void timer_Tick(object sender, EventArgs e)
         {
            //if (comboBox1.SelectedItem == null & string.IsNullOrEmpty(textBox1.Text)) { interval = 1000; }
            //else interval=inter;

            //  timer.Interval = interval;
            //dateTimePicker_sat.Value =dateTimePicker_sat.Value.AddMilliseconds(timer.Interval); 
            //intervalChange();

            dateTimePicker_sat.Value += m_dt;
        }

        private void button1_Click(object sender, EventArgs e)
        { 
             timer.Enabled = !timer.Enabled;
        }

        private void checkedListBox_satellites_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<SatParameters> checkedSats = new List<SatParameters>();
            foreach (var item in checkedListBox_satellites.CheckedItems)
            {
                checkedSats.Add(item as SatParameters);
            }

            control_SatOnMap1.Satellite = checkedSats;
            if (checkedListBox_satellites.CheckedItems != null) button1.Enabled = true;

        }
        private void OnSelectedSatChanged(SatParameters sat)
        {
            const double RAD_TO_GRAD = 180 / Math.PI;
            string latit;
            string longit;
            OrbitTools.Orbit o = new Orbit(sat.TLE);
            var pos = o.getPosition(dateTimePicker_sat.Value.ToUniversalTime()).toGeo();

            double lon = pos.Longitude * RAD_TO_GRAD;

            if (lon > 180)
                lon = lon - 360;

            double lat = pos.Latitude * RAD_TO_GRAD;

            lon = Math.Round(lon, 3);
            lat = Math.Round(lat, 3);

            if (lon < 0)
            {
                longit = lon * (-1) + "° з.д.";
                textBox_lon.Text = longit;
            }
            else
            {
                longit = lon + "° в.д.";
                textBox_lon.Text = longit;
            }
            if (lat < 0)
            {
                latit = lat * (-1) + "° ю.ш.";
                textBox_lat.Text = latit;
            }
            else
            {
                latit = lat + "° с.ш.";
                textBox_lat.Text = latit;
            }
        }

       










        //Image Zoom(Image img, Size size)
        //{
        //    Bitmap bmp = new Bitmap(img, img.Width + (img.Width * size.Width / 100), img.Height + (img.Height * size.Height / 100));
        //    Graphics g = Graphics.FromImage(bmp);
        //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //    return bmp;
        //}

        //private void trackBar1_Scroll(object sender, EventArgs e)
        //{
        //    if (trackBar1.Value > 0)
        //    {
        //        pictureBox1.Image = Zoom(imgOriginal, new Size(trackBar1.Value, trackBar1.Value));
        //    }
        //}


        //private void control_SatOnMap1_KeyDown(object sender, KeyEventArgs e)
        //{
        //    //Size StartSize;
        //    //int zoomSlider = 10;
        //    //StartSize = pictureBox.Size;
        //    //if (e.KeyCode== Keys.Up) {
        //    //    pictureBox.Width = StartSize.Width * zoomSlider;
        //    //    pictureBox.Height = StartSize.Height * zoomSlider;
        //    //}

        //}
    }
}
