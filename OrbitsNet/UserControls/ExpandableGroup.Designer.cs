namespace OrbitsNet
{
    partial class ExpandableGroup
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.bigButton1 = new BigButton();
            this.SuspendLayout();
            // 
            // bigButton1
            // 
            this.bigButton1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bigButton1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bigButton1.KeyId = null;
            this.bigButton1.LabelText = "label1";
            this.bigButton1.Location = new System.Drawing.Point(0, 0);
            this.bigButton1.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.bigButton1.Name = "bigButton1";
            this.bigButton1.Size = new System.Drawing.Size(853, 34);
            this.bigButton1.TabIndex = 0;
            this.bigButton1.ButtonClicked += new System.EventHandler<ButtonEventArgs>(this.bigButton1_ButtonClicked);
            this.bigButton1.LabelClicked += new System.EventHandler<ButtonEventArgs>(this.bigButton1_LabelClicked);
            // 
            // ExpandableGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bigButton1);
            this.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "ExpandableGroup";
            this.Size = new System.Drawing.Size(853, 34);
            this.ResumeLayout(false);

        }

        #endregion

        private BigButton bigButton1;
    }
}
