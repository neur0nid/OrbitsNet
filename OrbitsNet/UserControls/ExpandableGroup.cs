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

    public partial class ExpandableGroup : UserControl
    {

        #region OOOOO CONSTANTS OOOO

        private const int NOMINAL_HEIGHT = 34;
        private const int BASIC_HEIGHT = 34;
        private const int MARGIN_HEIGHT = 12;
        private const int MARGIN_ITEMS = 2;
        private const int SANGRIA_CHILDREN = 12;

        #endregion

        #region OOOOO PROPERTIES OOO

        public List<BigButton> Items { get; set; }

        #endregion

        #region OOOOO MEMBERS OOOOOO

        private int currentPanelHeight;
        private int currentTotalHeight;

        public event EventHandler<ButtonEventArgs> ButtonLabelClicked;

        #endregion

        #region OOOOO BUILDERS OOOOO

        public ExpandableGroup()
        {
            InitializeComponent();
            this.Items = new List<BigButton>();
            this.currentPanelHeight = 0;
            this.currentTotalHeight = BASIC_HEIGHT;

        }

        #endregion

        #region OOOOO EVENTS OOOOOOO

        private void bigButton1_ButtonClicked(object sender, ButtonEventArgs e)
        {
            this.SetVisibility();
            this.OnRaiseButtonLabelClicked(new ButtonEventArgs(e.LabelText, 0));
        }

        private void bigButton1_LabelClicked(object sender, ButtonEventArgs e)
        {
            this.OnRaiseButtonLabelClicked(new ButtonEventArgs(e.LabelText, 0));
        }

        protected virtual void OnRaiseButtonLabelClicked(ButtonEventArgs e)
        {
            EventHandler<ButtonEventArgs> handler = ButtonLabelClicked;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        void button_LabelClicked(object sender, ButtonEventArgs e)
        {
            this.OnRaiseButtonLabelClicked(new ButtonEventArgs(e.LabelText, 1));
        }

        void button_ButtonClicked(object sender, ButtonEventArgs e)
        {
            this.OnRaiseButtonLabelClicked(new ButtonEventArgs(e.LabelText, 1));
        }

        #endregion

        #region OOOOO PUBLICS OOOOOO

        public void SetItem(string id, string itemText, Image selected, Image unselected)
        {
            if (this.Items.Count == 0)
            {
                this.createPanel();
                this.currentTotalHeight = BASIC_HEIGHT + MARGIN_HEIGHT + NOMINAL_HEIGHT;
            }
            this.createItem(id, itemText, selected, unselected);
            this.SetVisibility();

        }

        public void Configure(string groupText, Image selected, Image unselected)
        {
            this.bigButton1.ConfigureButton(groupText, selected, unselected);
        }

        public void SetVisibility()
        {
            if (this.Items.Count != 0)
            {
                if (this.bigButton1.IsSelected)
                {
                    this.Controls[1].Visible = true;
                    this.Size = new Size(this.Size.Width, this.currentTotalHeight);
                }
                else
                {
                    this.Controls[1].Visible = false;
                    this.Size = new Size(this.Size.Width, BASIC_HEIGHT);
                }
            }
        }
        #endregion

        #region OOOOO PRIVATES OOOOO

        private void createItem(string id, string itemText, Image selected, Image unselected)
        {
            BigButton button = new BigButton();
            button.ConfigureButton(id, itemText, selected, unselected);
            button.Dock = DockStyle.Top;
            button.ButtonClicked += button_ButtonClicked;
            button.LabelClicked += button_LabelClicked;

            this.Controls[1].Controls.Add(button);
            if (this.Items.Count != 0)
            {
                this.currentPanelHeight = (this.Items.Count + 1) * NOMINAL_HEIGHT + MARGIN_ITEMS;
                this.currentTotalHeight += NOMINAL_HEIGHT;
            }
            this.Items.Add(button);

            this.Controls[1].Size = new Size(this.Controls[1].Size.Width, this.currentPanelHeight);
        }



        private void createPanel()
        {
            this.Size = new Size(this.Size.Width, BASIC_HEIGHT + MARGIN_HEIGHT + NOMINAL_HEIGHT);
            Panel itemsPanel = new Panel();
            //itemsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            itemsPanel.Size = new Size(this.Size.Width - 10, NOMINAL_HEIGHT);
            itemsPanel.Location = new Point(SANGRIA_CHILDREN, NOMINAL_HEIGHT + 2);
            //itemsPanel.Anchor = AnchorStyles.Bottom;
            //itemsPanel.Anchor = AnchorStyles.Left;
            //itemsPanel.Anchor = AnchorStyles.Right;
            //itemsPanel.Anchor = AnchorStyles.Top;
            this.Controls.Add(itemsPanel);
            this.currentPanelHeight = NOMINAL_HEIGHT + MARGIN_ITEMS;
        }

        #endregion

    }

}