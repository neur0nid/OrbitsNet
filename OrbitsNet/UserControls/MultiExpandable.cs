using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace OrbitsNet
{

    public partial class MultiExpandable : UserControl
    {
        #region OOOOO PROPERTIES OOO

        public List<string> SelectedItems { get; private set; }

        #endregion

        #region OOOOO MEMBERS OOOOOO

        private System.Collections.Hashtable items;

        public event EventHandler<ButtonEventArgs> ItemClicked;

        #endregion

        #region OOOOO BUILDERS OOOOO

        public MultiExpandable()
        {
            InitializeComponent();
            this.SelectedItems = new List<string>();
            this.items = new System.Collections.Hashtable();
        }

        #endregion

        #region OOOOO EVENTS OOOOOOO

        void group_ButtonLabelClicked(object sender, ButtonEventArgs e)
        {
            this.OnRaiseItemClicked(e);
        }

        protected virtual void OnRaiseItemClicked(ButtonEventArgs e)
        {
            EventHandler<ButtonEventArgs> handler = ItemClicked;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region OOOOO PUBLICS OOOOOO

        public void AddGroup(string groupId, string groupText, Image selected, Image unselected)
        {
            ExpandableGroup group = new ExpandableGroup();
            group.Dock = DockStyle.Top;
            group.Configure(groupText, selected, unselected);
            group.ButtonLabelClicked += group_ButtonLabelClicked;
            this.items.Add(groupId, group);
            this.Controls.Add(group);
        }

        public void AddItem(string groupId, string id, string itemText, Image selected, Image unselected)
        {
            var configItem = this.items[groupId] as ExpandableGroup;
            configItem.SetItem(id, itemText, selected, unselected);
        }

        public void UpdateSelected()
        {
            this.SelectedItems.Clear();

            foreach (DictionaryEntry entry in this.items)
            {
                this.getSelectedFromGroup(entry.Value as ExpandableGroup);
            }
        }

        public void ClearGroups()
        {
            this.items.Clear();
            this.Controls.Clear();
        }

        #endregion

        #region OOOOO PRIVATES OOOOO

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
    }

}