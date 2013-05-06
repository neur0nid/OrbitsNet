using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zeptomoby.OrbitTools;
using System.Windows.Forms.DataVisualization.Charting;

namespace OrbitsNet
{
    public partial class OrbitsNet : Form
    {
        #region OOOOO CONSTANTS OO

        private const string SITE_TEXT = "Site";

        #endregion

        #region OOOOO MEMBERS OOOO

        private System.Collections.Hashtable correspondenceFilesTable;
        private List<string> selectedFilesForDownload;
        private List<string> loadedFiles;
        private List<FileAndTle> loadedTles;
        private List<Tle> selectedTles;
        private List<OrbitsElement> processedTles;

        private double latitude; //º
        private double longitude; //º
        private double altitude; //km

        private int calcTime;
        private int stepTime;

        private System.Resources.ResourceManager recursos;
        private System.Globalization.CultureInfo cultura;

        private BackgroundWorker loadWorker;
        private BackgroundWorker plotWorker;

        #endregion

        #region OOOOO BUILDERS OOO

        public OrbitsNet()
        {
            InitializeComponent();
            this.recursos = new System.Resources.ResourceManager("OrbitsNet.Properties.Resources", this.GetType().Assembly);
            this.cultura = new System.Globalization.CultureInfo(System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToUpper());
            this.configureLanguage();
            this.fillHash();
            this.selectedFilesForDownload = new List<string>();
            this.loadedFiles = new List<string>();

            this.loadedTles = new List<FileAndTle>();
            this.selectedTles = new List<Tle>();
            this.processedTles = new List<OrbitsElement>();

            this.resetCartChart();
            this.resetPolarchart();
            this.resetMapChart();

            this.loadWorker = new BackgroundWorker();
            this.loadWorker.WorkerSupportsCancellation = true;
            this.loadWorker.WorkerReportsProgress = true;
            this.loadWorker.DoWork += loadWorker_DoWork;
            this.loadWorker.ProgressChanged += loadWorker_ProgressChanged;
            this.loadWorker.RunWorkerCompleted += loadWorker_RunWorkerCompleted;
            
            this.plotWorker = new BackgroundWorker();
            this.plotWorker.WorkerSupportsCancellation = true;
            this.plotWorker.WorkerReportsProgress = true;
            this.plotWorker.DoWork += plotWorker_DoWork;
            this.plotWorker.ProgressChanged += plotWorker_ProgressChanged;
            this.plotWorker.RunWorkerCompleted += plotWorker_RunWorkerCompleted;

            this.configureDefault();
        }

        private void configureLanguage()
        {
            this.tpFilesSelection.Text = Properties.Resources.filesSelection;
            this.gpFilesToLoad.Text = Properties.Resources.celestrack;
            this.gpLoadLocal.Text = Properties.Resources.localResources;
            this.gpSelectTles.Text = Properties.Resources.selectTles;

            this.btClose.Text = this.recursos.GetString("btClose", this.cultura);
            this.btLoadFiles.Text = this.recursos.GetString("btLoad", this.cultura);
            this.btSelectLocal.Text = this.recursos.GetString("btSelect", this.cultura);
            this.btApplyConfig.Text = this.recursos.GetString("btApply", this.cultura);
            this.btPlot.Text = this.recursos.GetString("btPlot", this.cultura);

            this.tpFilesSelection.Text = this.recursos.GetString("tpFilesSelection", this.cultura);
            this.tpTleSelection.Text = this.recursos.GetString("tpTleSelection", this.cultura);
            this.tpConfig.Text = this.recursos.GetString("tpConfig", this.cultura);
            this.tpCartesian.Text = this.recursos.GetString("tpCartesian", this.cultura);
            this.tpPolar.Text = this.recursos.GetString("tpPolar", this.cultura);
            this.tpMap.Text = this.recursos.GetString("tpMap", this.cultura);

            this.gpFilesToLoad.Text = this.recursos.GetString("gpFilesToLoad", this.cultura);
            this.gpSummarySelected.Text = this.recursos.GetString("gpSummarySelected", this.cultura);
            this.gpLoadLocal.Text = this.recursos.GetString("gpLoadLocal", this.cultura);
            this.gpSelectTles.Text = this.recursos.GetString("gpSelectTles", this.cultura);
            this.gpCoordinates.Text = this.recursos.GetString("gpCoordinates", this.cultura);
            this.gpSummaryTles.Text = this.recursos.GetString("gpSummaryTles", this.cultura);
            this.gpTime.Text = this.recursos.GetString("gpTime", this.cultura);
            this.gpTleDetails.Text = this.recursos.GetString("gpTleDetails", this.cultura);

            this.tbLocalFile.Text = this.recursos.GetString("tbLocalFile", this.cultura);

            this.lblAltitude.Text = this.recursos.GetString("lblAltitude", this.cultura);
            this.lblLatitude.Text = this.recursos.GetString("lblLatitude", this.cultura);
            this.lblLongitude.Text = this.recursos.GetString("lblLongitude", this.cultura);
            this.lblStep.Text = this.recursos.GetString("lblStep", this.cultura);
            this.lblDays.Text = this.recursos.GetString("lblDays", this.cultura);

        }

        #endregion

        #region OOOOO BUTTONS OOOO

        private void btLoadFiles_Click(object sender, EventArgs e)
        {
            this.resetPrevious();
            this.remoteLoadFiles();
        }

        private void btApplyConfig_Click(object sender, EventArgs e)
        {
            this.configureParams();
            this.tabControl1.SelectedIndex = 2;
        }

        private void btPlot_Click(object sender, EventArgs e)
        {
            this.prepareAndPlot();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region OO BCKGNDWORKRS OO

        void loadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool downloadresult = false;
            int totalFiles = this.selectedFilesForDownload.Count;
            for (int idx = 0; idx < this.selectedFilesForDownload.Count; idx++)
            {
                string fileId = this.selectedFilesForDownload[idx];
                downloadresult = this.downloadFile((string)this.correspondenceFilesTable[fileId], fileId);
                if (downloadresult) this.loadedFiles.Add(fileId);
                this.loadWorker.ReportProgress(idx + 1);
            }
        }
        
        void loadWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.mainProgressBar.PerformStep();
        }

        void loadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.configureLoadedElements();
            this.tabControl1.SelectedIndex = 1;
            this.mainProgressBar.Value = 0;
        }

        void plotWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int progress = 1;
            foreach (Tle tle in this.selectedTles)
            {
                OrbitsElement processingElement = new OrbitsElement();
                processingElement.ElementTle = tle;
                processingElement.SiteCoords = new Site(this.latitude, this.longitude, this.altitude);
                processingElement.ProcessingStartTime = DateTime.Now;
                processingElement.ProcessingStopTime = DateTime.Now.AddSeconds(this.calcTime);
                processingElement.ProcessingStepTime = this.stepTime;

                processingElement.CalcData();

                this.processedTles.Add(processingElement);
                this.plotWorker.ReportProgress(progress++);
            }

        }

        void plotWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.mainProgressBar.PerformStep();
        }

        void plotWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.plotCartesian();
            this.plotPolar();
            this.plotMap();
            this.tabControl1.SelectedIndex = 3;
            this.mainProgressBar.Value = 0;
        }

        #endregion

        #region OOOOO EVENTS OOOOO

        private void orbitsFileSelector1_SelectorButtonClicked(object sender, ButtonEventArgs e)
        {
            this.lvSummarySelected.BeginUpdate();
            this.lvSummarySelected.Items.Clear();
            this.orbitsFileSelector1.UpdateSelected();
            foreach (string tleFile in this.orbitsFileSelector1.SelectedItems)
            {
                ListViewItem lvItem = new ListViewItem(tleFile);
                lvItem.StateImageIndex = 3;

                this.lvSummarySelected.Items.Add(lvItem);
            }
            this.lvSummarySelected.EndUpdate();
        }

        private void multiExTleSelection_ItemClicked(object sender, ButtonEventArgs e)
        {
            if (e.Level == 1)
            {
                Tle clickedTle = this.findTleById(this.loadedTles, e.LabelText);
                if (clickedTle != null)
                {
                    this.tbElementId.Text = clickedTle.Name;
                    this.tbElementLine1.Text = clickedTle.Line1;
                    this.tbElementLine2.Text = clickedTle.Line2;
                }
            }

            this.lvTlesSelected.BeginUpdate();
            this.lvTlesSelected.Items.Clear();
            this.multiExTleSelection.UpdateSelected();
            foreach (string id in this.multiExTleSelection.SelectedItems)
            {
                ListViewItem lvItem = new ListViewItem(id);
                lvItem.StateImageIndex = 7;
                this.lvTlesSelected.Items.Add(lvItem);
            }
            this.lvTlesSelected.EndUpdate();
        }



        #endregion

        #region OOOOO CHARTS OOOOO

        private void resetCartChart()
        {
            this.chartAzimuth.Series.Clear();
            this.chartAzimuth.Titles.Add(this.recursos.GetString("azimuth", this.cultura));
            this.chartAzimuth.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
            this.chartAzimuth.Titles[0].Font = new Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.chartElevation.Series.Clear();
            this.chartElevation.Titles.Add(this.recursos.GetString("elevation", this.cultura));
            this.chartElevation.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
            this.chartElevation.Titles[0].Font = new Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.chartRange.Series.Clear();
            this.chartRange.Titles.Add(this.recursos.GetString("range", this.cultura));
            this.chartRange.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
            this.chartRange.Titles[0].Font = new Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void plotCartesian()
        {
            this.clearCartesian();
            foreach (OrbitsElement element in this.processedTles)
            {
                string serieName = element.ElementTle.Name;
                this.chartAzimuth.Series.Add(serieName);
                this.chartAzimuth.Series[serieName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                this.chartAzimuth.Series[serieName].BorderWidth = 2;

                this.chartElevation.Series.Add(serieName);
                this.chartElevation.Series[serieName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                this.chartElevation.Series[serieName].BorderWidth = 2;

                this.chartRange.Series.Add(serieName);
                this.chartRange.Series[serieName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                this.chartRange.Series[serieName].BorderWidth = 2;

                this.chartAzimuth.Series[serieName].Points.DataBindXY(element.TimeValues, element.AzimuthValues);
                this.chartElevation.Series[serieName].Points.DataBindXY(element.TimeValues, element.ElevationValues);
                this.chartRange.Series[serieName].Points.DataBindXY(element.TimeValues, element.RangeValues);
            }
        }

        private void resetPolarchart()
        {
            this.chartPolar.Series.Clear();
            this.chartPolar.Titles.Add(this.recursos.GetString("polar", this.cultura));
            this.chartPolar.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
            this.chartPolar.Titles[0].Font = new Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartPolar.ChartAreas[0].AxisY.CustomLabels.Add(new CustomLabel(-90, -86, "90", 0, LabelMarkStyle.Box));
            this.chartPolar.ChartAreas[0].AxisY.CustomLabels.Add(new CustomLabel(-72, -68, "72", 0, LabelMarkStyle.Box));
            this.chartPolar.ChartAreas[0].AxisY.CustomLabels.Add(new CustomLabel(-54, -50, "54", 0, LabelMarkStyle.Box));
            this.chartPolar.ChartAreas[0].AxisY.CustomLabels.Add(new CustomLabel(-36, -32, "36", 0, LabelMarkStyle.Box));
            this.chartPolar.ChartAreas[0].AxisY.CustomLabels.Add(new CustomLabel(-18, -14, "18", 0, LabelMarkStyle.Box));
        }

        private void plotPolar()
        {
            this.clearPolar();
            foreach (OrbitsElement element in this.processedTles)
            {
                string serieName = element.ElementTle.Name;
                this.chartPolar.Series.Add(serieName);
                this.chartPolar.Series[serieName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
                this.chartPolar.Series[serieName].BorderWidth = 3;

                this.chartPolar.ChartAreas[0].AxisY.Minimum = -90;
                this.chartPolar.ChartAreas[0].AxisY.Maximum = 0;

                double[] polarElevationValues = new double[element.ElevationValues.Length];
                for (int idx = 0; idx < element.ElevationValues.Length; idx++)
                {
                    polarElevationValues[idx] = (-1.0) * element.ElevationValues[idx];
                }
                this.chartPolar.Series[serieName].Points.DataBindXY(element.AzimuthValues, polarElevationValues);
            }
        }

        private void resetMapChart()
        {
            this.chartMap.Series.Clear();

            NamedImage backImage = new NamedImage("World_Standard_HiRes_grey", Properties.Resources.World_Standard_HiRes_grey);
            this.chartMap.Images.Add(backImage);

            this.chartMap.ChartAreas[0].BackImage = "World_Standard_HiRes_grey";
            this.chartMap.ChartAreas[0].BackImageWrapMode = ChartImageWrapMode.Scaled;

            this.chartMap.Titles.Add(this.recursos.GetString("map", this.cultura));
            this.chartMap.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
            this.chartMap.Titles[0].Font = new Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void plotMap()
        {
            this.clearMap();
            this.plotMapInitPoint();
            foreach (OrbitsElement element in this.processedTles)
            {
                string serieName = element.ElementTle.Name;
                this.chartMap.Series.Add(serieName);
                this.chartMap.Series[serieName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                this.chartMap.Series[serieName].BorderWidth = 2;

                this.chartMap.ChartAreas[0].AxisX.Minimum = -180;
                this.chartMap.ChartAreas[0].AxisX.Maximum = 180;
                this.chartMap.ChartAreas[0].AxisY.Minimum = -90;
                this.chartMap.ChartAreas[0].AxisY.Maximum = 90;

                double[] mapLongitudeValues = new double[element.LongitudeValues.Length];
                for (int idx = 0; idx < element.LongitudeValues.Length; idx++)
                {
                    if (element.LongitudeValues[idx] > 180)
                    {
                        mapLongitudeValues[idx] = element.LongitudeValues[idx] - 360.0;
                    }
                    else
                    {
                        mapLongitudeValues[idx] = element.LongitudeValues[idx];
                    }
                }

                this.chartMap.Series[serieName].Points.DataBindXY(mapLongitudeValues, element.LatitudeValues);
            }
        }

        private void plotMapInitPoint()
        {
            string serieName = SITE_TEXT;
            this.chartMap.Series.Add(serieName);
            this.chartMap.Series[serieName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            this.chartMap.Series[serieName].Color = Color.Black;

            this.chartMap.ChartAreas[0].AxisX.Minimum = -180;
            this.chartMap.ChartAreas[0].AxisX.Maximum = 180;
            this.chartMap.ChartAreas[0].AxisY.Minimum = -90;
            this.chartMap.ChartAreas[0].AxisY.Maximum = 90;
            this.chartMap.Series[serieName].Points.AddXY(this.longitude, this.latitude);
        }

        private void plotMapPoint()
        {
            string serieName = SITE_TEXT;
            this.chartMap.Series[serieName].Points.Clear();
            this.chartMap.Series[serieName].Points.AddXY(this.longitude, this.latitude);
        }

        private void clearCartesian()
        {
            this.chartAzimuth.Series.Clear();
            this.chartElevation.Series.Clear();
            this.chartRange.Series.Clear();
        }

        private void clearPolar()
        {
            this.chartPolar.Series.Clear();
        }

        private void clearMap()
        {
            this.chartMap.Series.Clear();
        }

        #endregion

        #region OOOOO PRIVATES OOO

        private void resetPrevious()
        {
            this.loadedFiles.Clear();
            this.loadedTles.Clear();
            this.selectedTles.Clear();
            this.processedTles.Clear();
        }

        private void configureDefault()
        {
            ////svalbard
            //this.latitude = 78.13;
            //this.longitude = 15.24;
            //this.altitude = 0.5;

            //madrid
            this.latitude = 40.4;
            this.longitude = -3.683;
            this.altitude = 0.6;

            this.calcTime = 259200; // 3 días
            this.stepTime = 60; //s

            this.numLatitude.Value = (decimal)this.latitude;
            this.numLongitude.Value = (decimal)this.longitude;
            this.numAltitude.Value = (decimal)(this.altitude * 1000);
            this.plotMapInitPoint();

            this.numDays.Value = (decimal)(this.calcTime / 86400);
            this.numStep.Value = this.stepTime;

        }

        private void configureParams()
        {
            this.latitude = (double)this.numLatitude.Value;
            this.longitude = (double)this.numLongitude.Value;
            this.altitude = (double)(this.numAltitude.Value / 1000);
            this.plotMapPoint();

            this.calcTime = (int)(this.numDays.Value * 86400);
            this.stepTime = (int)this.numStep.Value;
        }

        private void fillHash()
        {
            this.correspondenceFilesTable = new System.Collections.Hashtable();

            this.correspondenceFilesTable.Add("lastLaunches", "http://www.celestrak.com/NORAD/elements/tle-new.txt");
            this.correspondenceFilesTable.Add("spaceStations", "http://www.celestrak.com/NORAD/elements/stations.txt");
            this.correspondenceFilesTable.Add("brightest", "http://www.celestrak.com/NORAD/elements/visual.txt");
            this.correspondenceFilesTable.Add("fengyun", "http://www.celestrak.com/NORAD/elements/1999-025.txt");
            this.correspondenceFilesTable.Add("iridiumSpecial", "http://www.celestrak.com/NORAD/elements/iridium-33-debris.txt");
            this.correspondenceFilesTable.Add("cosmos", "http://www.celestrak.com/NORAD/elements/cosmos-2251-debris.txt");
            this.correspondenceFilesTable.Add("breeze", "http://www.celestrak.com/NORAD/elements/2012-044.txt");

            this.correspondenceFilesTable.Add("weather", "http://www.celestrak.com/NORAD/elements/weather.txt");
            this.correspondenceFilesTable.Add("noaa", "http://www.celestrak.com/NORAD/elements/noaa.txt");
            this.correspondenceFilesTable.Add("goes", "http://www.celestrak.com/NORAD/elements/goes.txt");
            this.correspondenceFilesTable.Add("earthResources", "http://www.celestrak.com/NORAD/elements/resource.txt");
            this.correspondenceFilesTable.Add("searchAndRescue", "http://www.celestrak.com/NORAD/elements/sarsat.txt");
            this.correspondenceFilesTable.Add("disaster", "http://www.celestrak.com/NORAD/elements/dmc.txt");
            this.correspondenceFilesTable.Add("trackingAndRelay", "http://www.celestrak.com/NORAD/elements/tdrss.txt");

            this.correspondenceFilesTable.Add("geostationary", "http://www.celestrak.com/NORAD/elements/geo.txt");
            this.correspondenceFilesTable.Add("intelsat", "http://www.celestrak.com/NORAD/elements/intelsat.txt");
            this.correspondenceFilesTable.Add("gorizont", "http://www.celestrak.com/NORAD/elements/gorizont.txt");
            this.correspondenceFilesTable.Add("raduga", "http://www.celestrak.com/NORAD/elements/raduga.txt");
            this.correspondenceFilesTable.Add("molniya", "http://www.celestrak.com/NORAD/elements/molniya.txt");
            this.correspondenceFilesTable.Add("iridiumComm", "http://www.celestrak.com/NORAD/elements/iridium.txt");
            this.correspondenceFilesTable.Add("orbcomm", "http://www.celestrak.com/NORAD/elements/orbcomm.txt");
            this.correspondenceFilesTable.Add("globalstar", "http://www.celestrak.com/NORAD/elements/globalstar.txt");
            this.correspondenceFilesTable.Add("amateurRadio", "http://www.celestrak.com/NORAD/elements/amateur.txt");
            this.correspondenceFilesTable.Add("experimental", "http://www.celestrak.com/NORAD/elements/x-comm.txt");
            this.correspondenceFilesTable.Add("otherComm", "http://www.celestrak.com/NORAD/elements/other-comm.txt");

            this.correspondenceFilesTable.Add("gps", "http://www.celestrak.com/NORAD/elements/gps-ops.txt");
            this.correspondenceFilesTable.Add("glonass", "http://www.celestrak.com/NORAD/elements/glo-ops.txt");
            this.correspondenceFilesTable.Add("galileo", "http://www.celestrak.com/NORAD/elements/galileo.txt");
            this.correspondenceFilesTable.Add("egnos", "http://www.celestrak.com/NORAD/elements/sbas.txt");
            this.correspondenceFilesTable.Add("navy", "http://www.celestrak.com/NORAD/elements/nnss.txt");
            this.correspondenceFilesTable.Add("russianLeo", "http://www.celestrak.com/NORAD/elements/musson.txt");

            this.correspondenceFilesTable.Add("spaceAndEarth", "http://www.celestrak.com/NORAD/elements/science.txt");
            this.correspondenceFilesTable.Add("geodetic", "http://www.celestrak.com/NORAD/elements/geodetic.txt");
            this.correspondenceFilesTable.Add("engineering", "http://www.celestrak.com/NORAD/elements/engineering.txt");
            this.correspondenceFilesTable.Add("education", "http://www.celestrak.com/NORAD/elements/education.txt");

            this.correspondenceFilesTable.Add("miscMilitary", "http://www.celestrak.com/NORAD/elements/military.txt");
            this.correspondenceFilesTable.Add("radarCalibration", "http://www.celestrak.com/NORAD/elements/radar.txt");
            this.correspondenceFilesTable.Add("cubes", "http://www.celestrak.com/NORAD/elements/cubesat.txt");
            this.correspondenceFilesTable.Add("otherMisc", "http://www.celestrak.com/NORAD/elements/other.txt");

        }

        private bool downloadFile(string url, string localFileName)
        {
            try
            {
                using (System.Net.WebClient webClient = new System.Net.WebClient())
                {
                    webClient.DownloadFile(url, localFileName);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void remoteLoadFiles()
        {
            this.selectedFilesForDownload.Clear();
            this.orbitsFileSelector1.UpdateSelected();
            foreach (string tleFile in this.orbitsFileSelector1.SelectedItems)
            {
                this.selectedFilesForDownload.Add(tleFile);
            }

            this.mainProgressBar.Maximum = this.selectedFilesForDownload.Count;
            this.loadWorker.RunWorkerAsync();
        }

        private void configureLoadedElements()
        {
            this.multiExTleSelection.ClearGroups();
            this.loadedTles.Clear();
            foreach (string file in this.loadedFiles)
            {
                this.readTles(file);
            }

            foreach (string file in this.loadedFiles)
            {
                this.multiExTleSelection.AddGroup(file, file, Properties.Resources.iconBookOpen, Properties.Resources.iconBook2);

                foreach (FileAndTle fileAndTle in this.loadedTles)
                {
                    if (fileAndTle.FileName == file)
                    {
                        this.multiExTleSelection.AddItem(file, fileAndTle.TwoLineElements.Name, fileAndTle.TwoLineElements.Name,
                            Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
                    }

                }
            }
        }

        #endregion

        #region OOOOO TLES OOOOOOO

        private void readTles(string file)
        {
            using (System.IO.StreamReader strReader = new System.IO.StreamReader(file))
            {
                string line = string.Empty;
                string id = string.Empty;
                string line1 = string.Empty;
                string line2 = string.Empty;
                while ((line = strReader.ReadLine()) != null)
                {
                    if (line.StartsWith("1"))
                    {
                        line1 = line;
                    }
                    else if (line.StartsWith("2"))
                    {
                        line2 = line;
                    }
                    else if (!(line.StartsWith("1") || line.StartsWith("2")))
                    {
                        id = line;
                    }

                    if (!(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(line1) || string.IsNullOrEmpty(line2)))
                    {
                        this.loadedTles.Add(new FileAndTle(file, id, line1, line2));
                        id = string.Empty;
                        line1 = string.Empty;
                        line2 = string.Empty;
                    }
                }
            }
        }

        private Tle findTleById(List<Tle> list, string id)
        {
            foreach (Tle tle in list)
            {
                if (tle.Name == id)
                {
                    return tle;
                }
            }
            return null;
        }

        private Tle findTleById(List<FileAndTle> list, string id)
        {
            foreach (FileAndTle tle in list)
            {
                if (tle.TwoLineElements.Name == id)
                {
                    return tle.TwoLineElements;
                }
            }
            return null;
        }

        #endregion

        #region OOOOO PLOT OOOOOOO

        private void prepareAndPlot()
        {
            this.selectedTles.Clear();

            foreach (ListViewItem item in this.lvTlesSelected.Items)
            {
                foreach (FileAndTle fileAndTle in this.loadedTles)
                {
                    if (fileAndTle.TwoLineElements.Name == item.Text)
                    {
                        this.selectedTles.Add(fileAndTle.TwoLineElements);
                    }
                }
            }
            this.plot();
            
        }

        private void plot()
        {
            this.processedTles.Clear();
            this.calcOrbit();
        }

        private void calcOrbit()
        {
            this.mainProgressBar.Maximum = this.selectedTles.Count;
            this.plotWorker.RunWorkerAsync();
        }

        #endregion
    }
}
