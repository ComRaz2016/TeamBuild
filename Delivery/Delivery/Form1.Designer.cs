namespace Delivery
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.driverBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.testDataSet = new Delivery.TestDataSet();
            this.driverTableAdapter = new Delivery.TestDataSetTableAdapters.DriverTableAdapter();
            this.buttonCreateOrder = new System.Windows.Forms.Button();
            this.buttonProviderMaterial = new System.Windows.Forms.Button();
            this.buttonDriverTS = new System.Windows.Forms.Button();
            this.buttonStatistics = new System.Windows.Forms.Button();
            this.buttonObserverOrder = new System.Windows.Forms.Button();
            this.buttonCheckCost = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.driverBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // driverBindingSource
            // 
            this.driverBindingSource.DataMember = "Driver";
            this.driverBindingSource.DataSource = this.testDataSet;
            // 
            // testDataSet
            // 
            this.testDataSet.DataSetName = "TestDataSet";
            this.testDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // driverTableAdapter
            // 
            this.driverTableAdapter.ClearBeforeFill = true;
            // 
            // buttonCreateOrder
            // 
            this.buttonCreateOrder.Location = new System.Drawing.Point(37, 12);
            this.buttonCreateOrder.Name = "buttonCreateOrder";
            this.buttonCreateOrder.Size = new System.Drawing.Size(200, 67);
            this.buttonCreateOrder.TabIndex = 0;
            this.buttonCreateOrder.Text = "Оформление заказа";
            this.buttonCreateOrder.UseVisualStyleBackColor = true;
            this.buttonCreateOrder.Click += new System.EventHandler(this.buttonCreateOrder_Click);
            // 
            // buttonProviderMaterial
            // 
            this.buttonProviderMaterial.Location = new System.Drawing.Point(37, 231);
            this.buttonProviderMaterial.Name = "buttonProviderMaterial";
            this.buttonProviderMaterial.Size = new System.Drawing.Size(200, 67);
            this.buttonProviderMaterial.TabIndex = 3;
            this.buttonProviderMaterial.Text = "Поставщики и материалы";
            this.buttonProviderMaterial.UseVisualStyleBackColor = true;
            this.buttonProviderMaterial.Click += new System.EventHandler(this.buttonProviderMaterial_Click);
            // 
            // buttonDriverTS
            // 
            this.buttonDriverTS.Location = new System.Drawing.Point(37, 304);
            this.buttonDriverTS.Name = "buttonDriverTS";
            this.buttonDriverTS.Size = new System.Drawing.Size(200, 67);
            this.buttonDriverTS.TabIndex = 4;
            this.buttonDriverTS.Text = "Водители и ТС";
            this.buttonDriverTS.UseVisualStyleBackColor = true;
            this.buttonDriverTS.Click += new System.EventHandler(this.buttonDriverTS_Click);
            // 
            // buttonStatistics
            // 
            this.buttonStatistics.Location = new System.Drawing.Point(37, 377);
            this.buttonStatistics.Name = "buttonStatistics";
            this.buttonStatistics.Size = new System.Drawing.Size(200, 67);
            this.buttonStatistics.TabIndex = 5;
            this.buttonStatistics.Text = "Статистика";
            this.buttonStatistics.UseVisualStyleBackColor = true;
            this.buttonStatistics.Click += new System.EventHandler(this.buttonStatistics_Click);
            // 
            // buttonObserverOrder
            // 
            this.buttonObserverOrder.Location = new System.Drawing.Point(37, 85);
            this.buttonObserverOrder.Name = "buttonObserverOrder";
            this.buttonObserverOrder.Size = new System.Drawing.Size(200, 67);
            this.buttonObserverOrder.TabIndex = 1;
            this.buttonObserverOrder.Text = "Отслеживание заказов";
            this.buttonObserverOrder.UseVisualStyleBackColor = true;
            this.buttonObserverOrder.Click += new System.EventHandler(this.buttonObserverOrder_Click);
            // 
            // buttonCheckCost
            // 
            this.buttonCheckCost.Location = new System.Drawing.Point(37, 158);
            this.buttonCheckCost.Name = "buttonCheckCost";
            this.buttonCheckCost.Size = new System.Drawing.Size(200, 67);
            this.buttonCheckCost.TabIndex = 2;
            this.buttonCheckCost.Text = "Предварительный рассчет стоимости";
            this.buttonCheckCost.UseVisualStyleBackColor = true;
            this.buttonCheckCost.Click += new System.EventHandler(this.buttonCheckCost_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 467);
            this.Controls.Add(this.buttonCheckCost);
            this.Controls.Add(this.buttonObserverOrder);
            this.Controls.Add(this.buttonStatistics);
            this.Controls.Add(this.buttonDriverTS);
            this.Controls.Add(this.buttonProviderMaterial);
            this.Controls.Add(this.buttonCreateOrder);
            this.Name = "Form1";
            this.Text = "Главное окно";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.driverBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private TestDataSet testDataSet;
        private System.Windows.Forms.BindingSource driverBindingSource;
        private TestDataSetTableAdapters.DriverTableAdapter driverTableAdapter;
        private System.Windows.Forms.Button buttonCreateOrder;
        private System.Windows.Forms.Button buttonProviderMaterial;
        private System.Windows.Forms.Button buttonDriverTS;
        private System.Windows.Forms.Button buttonStatistics;
        private System.Windows.Forms.Button buttonObserverOrder;
        private System.Windows.Forms.Button buttonCheckCost;
    }
}

