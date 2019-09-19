using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTools
{
    class PowerHelper
    {
        public struct PhiParam
        {
            public double PhiUa;
            public double PhiUb;
            public double PhiUc;
            public double PhiIa;
            public double PhiIb;
            public double PhiIc;
            public override string ToString()
            {
                return $"{PhiUa}, {PhiUb}, {PhiUc}, {PhiIa}, {PhiIb}, {PhiIc}";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TestMode">M1P2W,M3P4W,M3P3W,MQ260,MQ290,MQ390,M1P3W</param>
        /// <param name="PQ">Active,Reactive</param>
        /// <param name="FX">Forward,Reverse</param>
        /// <param name="eYuanjian">H,A,B,C</param>
        /// <param name="Glys">0.0-1.0</param>
        /// <param name="LC">Inductive,Capacitive</param>
        /// <param name="anticlockwise">true:anticlockwise,false:clockwise</param>
        /// <param name="PhiParam">(degree)</param>
        /// <returns></returns>
        public static bool CalculatePhaseDegree(string TestMode, string PQ, string FX, string eYuanjian, double Glys, string LC, bool anticlockwise, out PhiParam PhiParam)
        {
            PhiParam = new PhiParam();
            double XwUa = 0;
            double XwUb = 0;
            double XwUc = 0;
            double XwIa = 0;
            double XwIb = 0;
            double XwIc = 0;
            float BaseXwIa = 0;
            float BaseXwIb = 240;
            float BaseXwIc = 120;
            double LcValue = 0;
            double Phi = 0;

            LcValue = Math.Abs(Glys);
            switch (TestMode)
            {
                case "M1P2W":
                case "M3P4W":
                    #region
                    XwUa = 0;
                    XwUb = 240;
                    XwUc = 120;
                    BaseXwIa = 0;
                    BaseXwIb = 240;
                    BaseXwIc = 120;
                    if ("Reactive" == PQ)
                    {
                        BaseXwIa = 0 - 90;
                        BaseXwIb = 240 - 90;
                        BaseXwIc = 120 - 90;
                    }
                    Phi = Math.Round(Math.Acos(LcValue) * (180 / Math.PI), 5);
                    if (LcValue != 0 && LcValue != 1)
                    {
                        if (LC == "Inductive" && "Active" == PQ)
                        {
                            Phi = -1 * Phi;
                        }
                        else if (LC == "Capacitive" && "Reactive" == PQ)
                        {
                            Phi = -1 * Phi;
                        }
                    }
                    #endregion
                    break;
                case "M3P3W":
                    #region
                    XwUa = 0;
                    XwUb = 240;
                    XwUc = 120;
                    BaseXwIa = 0;
                    BaseXwIc = 120;
                    if ("H" != eYuanjian)
                    {
                        BaseXwIa = BaseXwIa + 30;
                        BaseXwIc = BaseXwIc - 30;
                    }
                    if ("Reactive" == PQ)
                    {
                        BaseXwIa = BaseXwIa - 90;
                        BaseXwIc = BaseXwIc - 90;
                    }
                    Phi = Math.Round(Math.Acos(LcValue) * (180 / Math.PI), 5);
                    if (LcValue != 0 && LcValue != 1)
                    {
                        if (LC == "Inductive" && "Active" == PQ)
                        {
                            Phi = -1 * Phi;
                        }
                        else if (LC == "Capacitive" && "Reactive" == PQ)
                        {
                            Phi = -1 * Phi;
                        }
                    }
                    #endregion
                    break;
                case "MQ260":
                    #region
                    XwUa = 0 + 30;
                    XwUc = 120 - 30;
                    BaseXwIa = 0 - 90;
                    BaseXwIc = 120 - 90;
                    if ("H" != eYuanjian)
                    {
                        BaseXwIa = BaseXwIa + 30;
                        BaseXwIc = BaseXwIc - 30;
                    }
                    Phi = Math.Round(Math.Acos(LcValue) * (180 / Math.PI), 5);
                    if (LcValue != 0 && LcValue != 1)
                    {
                        if (LC == "Capacitive")
                        {
                            Phi = -1 * Phi;
                        }
                    }
                    #endregion
                    break;
                case "MQ290":
                case "MQ390":
                    #region
                    XwUa = 0;
                    XwUb = 240;
                    XwUc = 120;
                    BaseXwIa = 0 - 90;
                    BaseXwIb = 240 - 90;
                    BaseXwIc = 120 - 90;
                    Phi = Math.Round(Math.Acos(LcValue) * (180 / Math.PI), 5);
                    if (LcValue != 0 && LcValue != 1)
                    {
                        if (LC == "Capacitive")
                        {
                            Phi = -1 * Phi;
                        }
                    }
                    #endregion
                    break;
                case "M1P3W":
                    #region
                    XwUa = 0;
                    BaseXwIa = 0;
                    BaseXwIb = 180;
                    if ("Reactive" == PQ)
                    {
                        BaseXwIa = 0 - 90;
                        BaseXwIb = 180 - 90;
                    }
                    Phi = Math.Round(Math.Acos(LcValue) * (180 / Math.PI), 5);
                    if (LcValue != 0 && LcValue != 1)
                    {
                        if (LC == "Inductive" && "Active" == PQ)
                        {
                            Phi = -1 * Phi;
                        }
                        else if (LC == "Capacitive" && "Reactive" == PQ)
                        {
                            Phi = -1 * Phi;
                        }
                    }
                    #endregion
                    break;
            }
            XwIa = BaseXwIa + Phi;
            XwIb = BaseXwIb + Phi;
            XwIc = BaseXwIc + Phi;
            if (FX == "Reverse")
            {
                XwIa = XwIa + 180;
                XwIb = XwIb + 180;
                XwIc = XwIc + 180;
            }
            if (XwIa < 0) XwIa = XwIa + 360;
            if (XwIa >= 360) XwIa = XwIa - 360;

            if (XwIb < 0) XwIb = XwIb + 360;
            if (XwIb >= 360) XwIb = XwIb - 360;

            if (XwIc < 0) XwIc = XwIc + 360;
            if (XwIc >= 360) XwIc = XwIc - 360;

            PhiParam.PhiUa = XwUa;
            PhiParam.PhiIa = XwIa;
            if (true == anticlockwise)
            {
                PhiParam.PhiUb = XwUc;
                PhiParam.PhiUc = XwUb;
                PhiParam.PhiIb = XwIc;
                PhiParam.PhiIc = XwIb;
            }
            else
            {
                PhiParam.PhiUb = XwUb;
                PhiParam.PhiUc = XwUc;
                PhiParam.PhiIb = XwIb;
                PhiParam.PhiIc = XwIc;
            }
            return true;
        }
    }
}
