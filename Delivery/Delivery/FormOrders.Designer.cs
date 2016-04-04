namespace Delivery
{
    partial class FormOrders
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabPageComplite = new System.Windows.Forms.TabPage();
            this.tabPageNotActive = new System.Windows.Forms.TabPage();
            this.tabPageActive = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonCompliteOrder = new System.Windows.Forms.Button();
            this.buttonCancelOrder = new System.Windows.Forms.Button();
            this.colNumOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatusOrder = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colAdr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBulkOrBad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageActive.SuspendLayout();
            this.tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabPageComplite
            // 
            this.tabPageComplite.Location = new System.Drawing.Point(4, 22);
            this.tabPageComplite.Name = "tabPageComplite";
            this.tabPageComplite.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageComplite.Size = new System.Drawing.Size(1059, 392);
            this.tabPageComplite.TabIndex = 2;
            this.tabPageComplite.Text = "Обработанные";
            this.tabPageComplite.UseVisualStyleBackColor = true;
            // 
            // tabPageNotActive
            // 
            this.tabPageNotActive.Location = new System.Drawing.Point(4, 22);
            this.tabPageNotActive.Name = "tabPageNotActive";
            this.tabPageNotActive.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNotActive.Size = new System.Drawing.Size(1059, 462);
            this.tabPageNotActive.TabIndex = 1;
            this.tabPageNotActive.Text = "Не активные";
            this.tabPageNotActive.UseVisualStyleBackColor = true;
            // 
            // tabPageActive
            // 
            this.tabPageActive.Controls.Add(this.buttonCancelOrder);
            this.tabPageActive.Controls.Add(this.buttonCompliteOrder);
            this.tabPageActive.Controls.Add(this.dataGridView1);
            this.tabPageActive.Location = new System.Drawing.Point(4, 22);
            this.tabPageActive.Name = "tabPageActive";
            this.tabPageActive.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageActive.Size = new System.Drawing.Size(1059, 462);
            this.tabPageActive.TabIndex = 0;
            this.tabPageActive.Text = "Активные";
            this.tabPageActive.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageActive);
            this.tabControl1.Controls.Add(this.tabPageNotActive);
            this.tabControl1.Controls.Add(this.tabPageComplite);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1067, 488);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumOrder,
            this.colStatusOrder,
            this.colAdr,
            this.colTime,
            this.colCost,
            this.colVolume,
            this.colBulkOrBad});
            this.dataGridView1.Location = new System.Drawing.Point(57, 17);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(939, 318);
            this.dataGridView1.TabIndex = 0;
            // 
            // buttonCompliteOrder
            // 
            this.buttonCompliteOrder.Location = new System.Drawing.Point(231, 374);
            this.buttonCompliteOrder.Name = "buttonCompliteOrder";
            this.buttonCompliteOrder.Size = new System.Drawing.Size(150, 45);
            this.buttonCompliteOrder.TabIndex = 1;
            this.buttonCompliteOrder.Text = "Заказ обработан";
            this.buttonCompliteOrder.UseVisualStyleBackColor = true;
            // 
            // buttonCancelOrder
            // 
            this.buttonCancelOrder.Location = new System.Drawing.Point(661, 374);
            this.buttonCancelOrder.Name = "buttonCancelOrder";
            this.buttonCancelOrder.Size = new System.Drawing.Size(150, 45);
            this.buttonCancelOrder.TabIndex = 2;
            this.buttonCancelOrder.Text = "Отменить заказ";
            this.buttonCancelOrder.UseVisualStyleBackColor = true;
            // 
            // colNumOrder
            // 
            this.colNumOrder.HeaderText = "Номер заказа";
            this.colNumOrder.MinimumWidth = 100;
            this.colNumOrder.Name = "colNumOrder";
            // 
            // colStatusOrder
            // 
            this.colStatusOrder.FillWeight = 200F;
            this.colStatusOrder.HeaderText = "Статус заказа";
            this.colStatusOrder.Items.AddRange(new object[] {
            "Ожидает доставки",
            "Едет на загрузку",
            "Загружается",
            "Осуществляте доставку",
            "Доставка выполнена",
            "Заказ обработан"});
            this.colStatusOrder.MinimumWidth = 200;
            this.colStatusOrder.Name = "colStatusOrder";
            this.colStatusOrder.Width = 200;
            // 
            // colAdr
            // 
            this.colAdr.FillWeight = 200F;
            this.colAdr.HeaderText = "Адрес заказа";
            this.colAdr.MinimumWidth = 200;
            this.colAdr.Name = "colAdr";
            this.colAdr.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAdr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAdr.Width = 200;
            // 
            // colTime
            // 
            this.colTime.HeaderText = "Время доставки";
            this.colTime.MinimumWidth = 100;
            this.colTime.Name = "colTime";
            this.colTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colCost
            // 
            this.colCost.HeaderText = "Стоимость заказа";
            this.colCost.MinimumWidth = 100;
            this.colCost.Name = "colCost";
            this.colCost.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCost.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colVolume
            // 
            this.colVolume.FillWeight = 75F;
            this.colVolume.HeaderText = "Объем заказа";
            this.colVolume.MinimumWidth = 75;
            this.colVolume.Name = "colVolume";
            this.colVolume.Width = 75;
            // 
            // colBulkOrBad
            // 
            this.colBulkOrBad.HeaderText = "Форма доставки";
            this.colBulkOrBad.Name = "colBulkOrBad";
            // 
            // FormOrders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1091, 512);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormOrders";
            this.Text = "Заказы";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormOrders_FormClosed);
            this.Load += new System.EventHandler(this.FormOrders_Load);
            this.tabPageActive.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabPageComplite;
        private System.Windows.Forms.TabPage tabPageNotActive;
        private System.Windows.Forms.TabPage tabPageActive;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button buttonCancelOrder;
        private System.Windows.Forms.Button buttonCompliteOrder;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumOrder;
        private System.Windows.Forms.DataGridViewComboBoxColumn colStatusOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdr;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVolume;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBulkOrBad;
    }
}