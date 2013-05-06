using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbitsNet
{
    public partial class OrbitsFileSelector : UserControl
    {

        #region OOOOO PROPERTIES OOO

        public List<string> SelectedItems { get; private set; }

        #endregion

        #region OOOOO MEMBERS OOOOOO

        public event EventHandler<ButtonEventArgs> SelectorButtonClicked;

        #endregion

        #region OOOOO BUILDERS OOOOO

        public OrbitsFileSelector()
        {
            InitializeComponent();
            this.SelectedItems = new List<string>();
            this.configureGroups();
        }

        #endregion

        #region OOOOO PRIVATES OOOOO

        private void configureGroups()
        {
            //special interest:
            this.expandableGroup1.Configure(Properties.Resources.specialInterest, Properties.Resources.iconBookOpen, Properties.Resources.iconBook2);
            this.expandableGroup1.SetItem("breeze", Properties.Resources.breeze, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup1.SetItem("cosmos", Properties.Resources.cosmos, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup1.SetItem("iridiumSpecial", Properties.Resources.iridiumSpecial, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup1.SetItem("fengyun", Properties.Resources.fengyun, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup1.SetItem("brightest", Properties.Resources.brightest, Properties.Resources.iconStar_swirl_o, Properties.Resources.iconStar_swirl);
            this.expandableGroup1.SetItem("spaceStations", Properties.Resources.spaceStations, Properties.Resources.iconForward_field_o, Properties.Resources.iconForward_field);
            this.expandableGroup1.SetItem("lastLaunches", Properties.Resources.lastLaunches, Properties.Resources.iconRocket_flight_o, Properties.Resources.iconRocket_flight);
            this.expandableGroup1.ButtonLabelClicked += expandableGroup1_ButtonLabelClicked;

            //weather
            this.expandableGroup2.Configure(Properties.Resources.weatherAndEarth, Properties.Resources.iconBookOpen, Properties.Resources.iconBook2);
            this.expandableGroup2.SetItem("trackingAndRelay", Properties.Resources.trackingAndRelay, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup2.SetItem("disaster", Properties.Resources.disaster, Properties.Resources.tornado_o, Properties.Resources.tornado);
            this.expandableGroup2.SetItem("searchAndRescue", Properties.Resources.searchAndRescue, Properties.Resources.hospital_cross_o, Properties.Resources.hospital_cross);
            this.expandableGroup2.SetItem("earthResources", Properties.Resources.earthResources, Properties.Resources.iconWorld_o, Properties.Resources.iconWorld);
            this.expandableGroup2.SetItem("goes", Properties.Resources.goes, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup2.SetItem("noaa", Properties.Resources.noaa, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup2.SetItem("weather", Properties.Resources.weather, Properties.Resources.iconRaining_o, Properties.Resources.iconRaining);
            this.expandableGroup2.ButtonLabelClicked += expandableGroup2_ButtonLabelClicked;

            //communication
            this.expandableGroup3.Configure(Properties.Resources.communication, Properties.Resources.iconBookOpen, Properties.Resources.iconBook2);
            this.expandableGroup3.SetItem("otherComm", Properties.Resources.otherComm, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup3.SetItem("experimental", Properties.Resources.experimental, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup3.SetItem("amateurRadio", Properties.Resources.amateurRadio, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup3.SetItem("globalstar", Properties.Resources.globalstar, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup3.SetItem("orbcomm", Properties.Resources.orbcomm, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup3.SetItem("iridiumComm", Properties.Resources.iridiumComm, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup3.SetItem("molniya", Properties.Resources.molniya, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup3.SetItem("raduga", Properties.Resources.raduga, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup3.SetItem("gorizont", Properties.Resources.gorizont, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup3.SetItem("intelsat", Properties.Resources.intelsat, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup3.SetItem("geostationary", Properties.Resources.geostationary, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup3.ButtonLabelClicked += expandableGroup3_ButtonLabelClicked;
            //navigation
            this.expandableGroup4.Configure(Properties.Resources.navigation, Properties.Resources.iconBookOpen, Properties.Resources.iconBook2);
            this.expandableGroup4.SetItem("russianLeo", Properties.Resources.russianLeo, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup4.SetItem("navy", Properties.Resources.navy, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup4.SetItem("egnos", Properties.Resources.egnos, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup4.SetItem("galileo", Properties.Resources.galileo, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup4.SetItem("glonass", Properties.Resources.glonass, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup4.SetItem("gps", Properties.Resources.gps, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup4.ButtonLabelClicked += expandableGroup4_ButtonLabelClicked;

            //navigation
            this.expandableGroup5.Configure(Properties.Resources.scientific, Properties.Resources.iconBookOpen, Properties.Resources.iconBook2);
            this.expandableGroup5.SetItem("education", Properties.Resources.education, Properties.Resources.iconBook_o, Properties.Resources.iconBook);
            this.expandableGroup5.SetItem("engineering", Properties.Resources.engineering, Properties.Resources.iconCircuitry_o, Properties.Resources.iconCircuitry);
            this.expandableGroup5.SetItem("geodetic", Properties.Resources.geodetic, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup5.SetItem("spaceAndEarth", Properties.Resources.spaceAndEarth, Properties.Resources.night_sky_o, Properties.Resources.night_sky);
            this.expandableGroup5.ButtonLabelClicked += expandableGroup5_ButtonLabelClicked;

            //miscellaneous
            this.expandableGroup6.Configure(Properties.Resources.miscellaneous, Properties.Resources.iconBookOpen, Properties.Resources.iconBook2);
            this.expandableGroup6.SetItem("otherMisc", Properties.Resources.otherMisc, Properties.Resources.iconMesh_ball_o, Properties.Resources.iconMesh_ball);
            this.expandableGroup6.SetItem("cubes", Properties.Resources.cubes, Properties.Resources.iconCubes_o, Properties.Resources.iconCubes);
            this.expandableGroup6.SetItem("radarCalibration", Properties.Resources.radarCalibration, Properties.Resources.iconRadar_sweep_o, Properties.Resources.iconRadar_sweep);
            this.expandableGroup6.SetItem("miscMilitary", Properties.Resources.miscMilitary, Properties.Resources.medal_o, Properties.Resources.medal);
            this.expandableGroup6.ButtonLabelClicked += expandableGroup6_ButtonLabelClicked;
        }

        public void UpdateSelected()
        {
            this.SelectedItems.Clear();

            this.getSelectedFromGroup(this.expandableGroup1);
            this.getSelectedFromGroup(this.expandableGroup2);
            this.getSelectedFromGroup(this.expandableGroup3);
            this.getSelectedFromGroup(this.expandableGroup4);
            this.getSelectedFromGroup(this.expandableGroup5);
            this.getSelectedFromGroup(this.expandableGroup6);
        }

        private void getSelectedFromGroup(ExpandableGroup group)
        {
            foreach (BigButton bigButton in group.Items)
            {
                if (bigButton.IsSelected)
                {
                    this.SelectedItems.Add(bigButton.KeyId);
                }
            }
        }

        #endregion

        #region OOOOO EVENTS OOOOOOO

        protected virtual void OnRaiseSelectorButtonClicked(ButtonEventArgs e)
        {
            EventHandler<ButtonEventArgs> handler = SelectorButtonClicked;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        void expandableGroup6_ButtonLabelClicked(object sender, ButtonEventArgs e)
        {
            this.OnRaiseSelectorButtonClicked(e);
        }

        void expandableGroup5_ButtonLabelClicked(object sender, ButtonEventArgs e)
        {
            this.OnRaiseSelectorButtonClicked(e);
        }

        void expandableGroup4_ButtonLabelClicked(object sender, ButtonEventArgs e)
        {
            this.OnRaiseSelectorButtonClicked(e);
        }

        void expandableGroup3_ButtonLabelClicked(object sender, ButtonEventArgs e)
        {
            this.OnRaiseSelectorButtonClicked(e);
        }

        void expandableGroup2_ButtonLabelClicked(object sender, ButtonEventArgs e)
        {
            this.OnRaiseSelectorButtonClicked(e);
        }

        void expandableGroup1_ButtonLabelClicked(object sender, ButtonEventArgs e)
        {
            this.OnRaiseSelectorButtonClicked(e);
        }

        #endregion
    }

}