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
            if ("C".Equals(cmb_LC.Text))
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
        }

        private void btn_PicZip_Click(object sender, EventArgs e)
        {
            PicHelper.GetPicThumbnail(txt_PicFullName.Text, @"D:\S\Pictures\ziptmp.png", 100, 400, 100);
            //PicHelper.CutImageWhitePart(@"D:\S\Pictures\ziptmp.png", 0);
        }
    }
}
