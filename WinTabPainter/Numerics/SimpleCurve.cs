﻿using System;

namespace WinTabPainter.Numerics
{
    public class SimpleCurve
    {

        double amt;

        private static RangeD range = new RangeD(-1.0,1.0);
        public double BendAmount { 
            get => this.amt; 
            set => this.amt = range.Clamp(value); }

        public SimpleCurve()
        {
            this.BendAmount = 0.0;
        }

        public double ApplyCurve(double value)
        {

            double new_value;

            if (this.BendAmount > 0)
            {
                new_value = Math.Pow(value, 1.0 - this.BendAmount);
            }
            else if (this.BendAmount < 0)
            {
                new_value = Math.Pow(value, 1.0 / (1.0 + this.BendAmount));
            }
            else
            {
                new_value = value;
            }

            return new_value;
        }
    }
}
