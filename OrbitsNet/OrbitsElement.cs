using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeptomoby.OrbitTools;

namespace OrbitsNet
{
    class OrbitsElement
    {
        public Tle ElementTle { get; set; }
        
        public DateTime ProcessingStartTime { get; set; }
        public DateTime ProcessingStopTime { get; set; }
        public int ProcessingStepTime { get; set; }
        public Site SiteCoords { get; set; }

        public Orbit ElementOrbit { get; private set; }
        public List<DateTime> TimeData { get; private set; }
        public List<Topo> TopocentricData { get; private set; }
        public double[] AzimuthValues { get; private set; }
        public double[] ElevationValues { get; private set; }
        public double[] RangeValues { get; private set; }
        public double[] RangeRateValues { get; private set; }
        public double[] LatitudeValues { get; private set; }
        public double[] LongitudeValues { get; private set; }
        public DateTime[] TimeValues { get; private set; }


        public OrbitsElement()
        {
            this.TopocentricData = new List<Topo>();
            this.TimeData = new List<DateTime>();
        }

        public void CalcData()
        {
            this.ElementOrbit = new Orbit(this.ElementTle);

            for (DateTime indexTime = this.ProcessingStartTime; indexTime < this.ProcessingStopTime; indexTime = indexTime.AddSeconds(this.ProcessingStepTime))
            {
                Topo topo = this.SiteCoords.GetLookAngle(this.ElementOrbit.GetPosition(indexTime));
                this.TopocentricData.Add(topo);
                this.TimeData.Add(indexTime);
            }

            this.TimeValues = new DateTime[this.TimeData.Count];
            this.AzimuthValues = new double[this.TopocentricData.Count];
            this.ElevationValues = new double[this.TopocentricData.Count];
            this.RangeValues = new double[this.TopocentricData.Count];
            this.RangeRateValues = new double[this.TopocentricData.Count];
            this.LatitudeValues = new double[this.TopocentricData.Count];
            this.LongitudeValues = new double[this.TopocentricData.Count];

            for (int idx = 0; idx < this.TopocentricData.Count; idx++)
            {
                this.TimeValues[idx] = this.TimeData[idx];
                this.AzimuthValues[idx] = this.TopocentricData[idx].AzimuthDeg;
                this.ElevationValues[idx] = this.TopocentricData[idx].ElevationDeg;
                this.RangeValues[idx] = this.TopocentricData[idx].Range;
                this.RangeRateValues[idx] = this.TopocentricData[idx].RangeRate;
                EciTime eciData = this.ElementOrbit.GetPosition(this.TimeData[idx]);
                GeoTime geoData = new GeoTime(eciData);
                this.LatitudeValues[idx] = geoData.LatitudeDeg;
                this.LongitudeValues[idx] = geoData.LongitudeDeg;
            }
        }
    }
}
