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
        public partial class BigButton : UserControl
        {
            #region OOOOO PROPERTIES OO

            public string LabelText
            {
                get
                {
                    return this.label1.Text;
                }
                set
                {
                    this.label1.Text = value;
                }
            }

            public Image SelectedImage { get; private set; }
            public Image UnselectedImage { get; private set; }

            public bool IsSelected { get; private set; }
            public string KeyId { get; set; }

            #endregion

            #region OOOOO MEMBERS OOOOO

            public event EventHandler<ButtonEventArgs> ButtonClicked;
            public event EventHandler<ButtonEventArgs> LabelClicked;

            #endregion

            #region OOOOO BUILDERS OOOO

            public BigButton()
            {
                InitializeComponent();
                this.setUnselected();
            }

            #endregion

            #region OOOOO PUBLICS OOOOO

            public void ConfigureButton(string text, Image selected, Image unselected)
            {
                this.LabelText = text;

                this.SelectedImage = selected;
                this.UnselectedImage = unselected;

                this.KeyId = string.Empty;

                this.setUnselected();
            }

            public void ConfigureButton(string id, string text, Image selected, Image unselected)
            {
                this.LabelText = text;

                this.SelectedImage = selected;
                this.UnselectedImage = unselected;

                this.KeyId = id;

                this.setUnselected();
            }

            #endregion

            #region OOOOO EVENTS OOOOOO

            private void label1_Click(object sender, EventArgs e)
            {
                this.OnRaiseLabelClicked(new ButtonEventArgs(this.LabelText, -1));
            }

            private void button1_Click(object sender, EventArgs e)
            {
                if (this.IsSelected)
                {
                    this.setUnselected();
                }
                else
                {
                    this.setSelected();
                }
                this.OnRaiseButtonClicked(new ButtonEventArgs(this.LabelText, -1));
            }

            protected virtual void OnRaiseButtonClicked(ButtonEventArgs e)
            {
                EventHandler<ButtonEventArgs> handler = ButtonClicked;
                if (handler != null)
                {
                    handler(this, e);
                }
            }

            protected virtual void OnRaiseLabelClicked(ButtonEventArgs e)
            {
                EventHandler<ButtonEventArgs> handler = LabelClicked;
                if (handler != null)
                {
                    handler(this, e);
                }
            }

            #endregion

            #region OOOOO PRIVATES OOOO

            private void setUnselected()
            {
                this.button1.BackgroundImage = this.UnselectedImage;
                this.IsSelected = false;
            }

            private void setSelected()
            {
                this.button1.BackgroundImage = this.SelectedImage;
                this.IsSelected = true;
            }

            #endregion

        }
}