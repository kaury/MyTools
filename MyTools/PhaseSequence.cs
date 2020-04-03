using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTools
{
    public class PSData
    {
        /// <summary>
        /// 0:false:fail; 1:true:sucess; -1:ex; n:...
        /// </summary>
        public int Result;
        public string Message = string.Empty;
        public double Amplitude;
        public double Phase;
    }
    public class PhaseSequence
    {
        public PSData CalcPositive(Polar a, Polar b, Polar c)
        {
            PSData data = new PSData();
            try
            {
                Polar sum = a + b.Rotate(120) + c.Rotate(240);
                data.Amplitude = sum.Modul / 3d;
                data.Phase = sum.Angle;
                data.Result = 1;
            }
            catch (Exception ex)
            {
                data.Result = -1;
                data.Message = ex.Message;
            }
            return data;
        }
        public PSData CalcNegative(Polar a, Polar b, Polar c)
        {
            PSData data = new PSData();
            try
            {
                Polar sum = a + b.Rotate(240) + c.Rotate(120);
                data.Amplitude = sum.Modul / 3d;
                data.Phase = sum.Angle;
                data.Result = 1;
            }
            catch (Exception ex)
            {
                data.Result = -1;
                data.Message = ex.Message;
            }
            return data;
        }
        public PSData CalcZero(Polar a, Polar b, Polar c)
        {
            PSData data = new PSData();
            try
            {
                Polar sum = a + b + c;
                data.Amplitude = sum.Modul / 3d;
                data.Phase = sum.Angle;
                data.Result = 1;
            }
            catch (Exception ex)
            {
                data.Result = -1;
                data.Message = ex.Message;
            }
            return data;
        }
    }
}
