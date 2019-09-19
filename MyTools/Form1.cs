using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cmb_clockwise.SelectedIndex = 0;
            cmb_FR.SelectedIndex = 0;
            cmb_HABC.SelectedIndex = 0;
            cmb_LC.SelectedIndex = 0;
            cmb_PQ.SelectedIndex = 0;
            cmb_TestMode.SelectedIndex = 0;
        }

        private void btn_CvtHex2Dec_Click(object sender, EventArgs e)
        {
            byte[] org = new byte[4];
            string strHex = txtHexOrg.Text.Replace(" ", "").PadLeft(8, '0');
            for (int i = 0; i < strHex.Length / 2; i++)
            {
                org[i] = byte.Parse(strHex.Substring(i * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            txtDecRst.Text = BitConverter.ToSingle(org, 0).ToString();
        }

        private void btn_CvtDec2Hex_Click(object sender, EventArgs e)
        {
            float.TryParse(txtDecOrg.Text.Trim(), out float org);
            txtHexRst.Text = BitConverter.ToString(BitConverter.GetBytes(org)).Replace("-", "");
        }

        private void btn_CvtHex2Dec2_Click(object sender, EventArgs e)
        {
            byte[] org = new byte[8];
            string strHex = txtHexOrg2.Text.Replace(" ", "").PadLeft(16, '0');
            for (int i = 0; i < strHex.Length / 2; i++)
            {
                org[i] = byte.Parse(strHex.Substring(i * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            txtDecRst2.Text = BitConverter.ToDouble(org, 0).ToString();
        }

        private void btn_CvtDec2Hex2_Click(object sender, EventArgs e)
        {
            double.TryParse(txtDecOrg2.Text.Trim(), out double org);
            txtHexRst2.Text = BitConverter.ToString(BitConverter.GetBytes(org)).Replace("-", "");
        }

        private void btn_MeterCalc_Click(object sender, EventArgs e)
        {
            double.TryParse(txt_U.Text, out double Ub);
            double.TryParse(txt_I.Text, out double Ib);
            double.TryParse(txt_C.Text, out double C);
            double m = 1;
            switch (cmb_TestMode.Text)
            {
                case "1P2W": m = 1; break;
                case "3P3W": m = Math.Sqrt(3); break;
                case "3P4W": m = 3; break;
            }
            double factor = 1;
            bool IsMinus = false;
            if (!string.IsNullOrWhiteSpace(txt_PF.Text))
            {
                double.TryParse(txt_PF.Text, out factor);
                if (txt_PF.Text.IndexOf("-") >= 0) IsMinus = true;
                if (cmb_FR.Text == "Reverse") IsMinus = true;
            }
            double s = Ub * Ib * m;
            double power = s * factor;
            double CFreq = Math.Abs(power * C / 3600 / 1000);
            double CTime = 0;
            if (CFreq != 0)
            {
                CTime = 1 / CFreq;
            }
            double seconds = CTime * C;

            double angle = Math.Acos(Math.Abs(factor)) / Math.PI * 180;
            if (IsMinus) angle = angle + 180;
            if ("Capacitive".Equals(cmb_LC.Text))
            {
                angle = angle * -1;
                if (angle < 0) angle = angle + 360;
            }
            txt_P.Text = power.ToString();
            txt_Q.Text = Math.Sqrt(s * s - power * power).ToString();
            txt_S.Text = s.ToString();
            txt_A.Text = angle.ToString();
            txt_CHz.Text = CFreq.ToString();
            txt_OnePulseTime.Text = CTime.ToString();
            txt_TimePerkWh.Text = seconds.ToString();
            txt_MinPerkWh.Text = (seconds / 60F).ToString();

            PowerHelper.CalculatePhaseDegree($"M{cmb_TestMode.Text}", cmb_PQ.Text, cmb_FR.Text, cmb_HABC.Text, factor, cmb_LC.Text, cmb_clockwise.Text == "anticlockwise", out PowerHelper.PhiParam phis);
            txt_PhiU1.Text = phis.PhiUa.ToString();
            txt_PhiU2.Text = phis.PhiUb.ToString();
            txt_PhiU3.Text = phis.PhiUc.ToString();
            txt_PhiI1.Text = phis.PhiIa.ToString();
            txt_PhiI2.Text = phis.PhiIb.ToString();
            txt_PhiI3.Text = phis.PhiIc.ToString();
            PMS.Platform.Utility.VectorHelper vector = new PMS.Platform.Utility.VectorHelper()
            {
                UL1Angle = (float)phis.PhiUa,
                UL2Angle = (float)phis.PhiUb,
                UL3Angle = (float)phis.PhiUc,
                IL1Angle = (float)phis.PhiIa,
                IL2Angle = (float)phis.PhiIb,
                IL3Angle = (float)phis.PhiIc,
            };
            switch (cmb_TestMode.Text)
            {
                case "1P2W":
                    vector.UL2Value = 0;
                    vector.UL3Value = 0;
                    vector.IL2Value = 0;
                    vector.IL3Value = 0;
                    break;
                case "3P3W":
                    vector.IL2Value = 0;
                    break;
                case "3P4W":
                    break;
            }
            if (vector.DrawVector()) pic_phase.Image = PicHelper.GetBitmap(vector.VectorFullPath);
            else pic_phase.Image = null;
        }

        private void btn_PicZip_Click(object sender, EventArgs e)
        {
            PicHelper.GetPicThumbnail(txt_PicFullName.Text, @"D:\S\Pictures\ziptmp.png", 100, 400, 100);
            //PicHelper.CutImageWhitePart(@"D:\S\Pictures\ziptmp.png", 0);
        }

        private void btn_CvtHex2Dec3_Click(object sender, EventArgs e)
        {
            byte[] org = new byte[8];
            string strHex = txtHexOrg3.Text.Replace(" ", "").PadLeft(16, '0');
            for (int i = 0; i < strHex.Length / 2; i++)
            {
                org[i] = byte.Parse(strHex.Substring(i * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            txtDecRst3.Text = BitConverter.ToInt64(org.Reverse().ToArray(), 0).ToString();
        }

        private void btn_CvtDec2Hex3_Click(object sender, EventArgs e)
        {
            long.TryParse(txtDecOrg3.Text.Trim(), out long org);
            txtHexRst3.Text = BitConverter.ToString(BitConverter.GetBytes(org).Reverse().ToArray()).Replace("-", "");
        }

        private void Btn_DFT_Click(object sender, EventArgs e)
        {
            string[] smps = Txt_Data.Text.Split('\r');
            int n = int.Parse(Txt_N.Text);
            double[] signal = new double[n];
            for (int i = 0; i < n; i++)
            {
                if (smps[i].IndexOf(';') < 0) continue;
                double.TryParse(smps[i].Split(';')[1], out double temp);
                signal[i] = temp;
            }
            DFT.complex[] complexs = DFT.dft(signal, n);
            double[] amps = DFT.amplitude(complexs, n);
            double thds = 0;
            double[] pers = new double[n / 2];
            for (int i = 0; i < n / 2; i++)
            {
                pers[i] = amps[i] / amps[1] * 100;
                if (i >= 2)
                    thds += Math.Pow(pers[i], 2);
            }
            Txt_THD.Text = Math.Sqrt(thds).ToString("F4");
            Rtxt_View.Text = string.Join(Environment.NewLine, pers);
        }

        private void Btn_Count_Click(object sender, EventArgs e)
        {
            Txt_Data.Text = Txt_Data.Text.Replace("\n", "").Replace("\r", "\r\n");
            string[] smps = Txt_Data.Text.Split('\r');
            Txt_N.Text = smps.Length.ToString();
        }
    }
}
