namespace Delivery
{
    partial class FormStatistics
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePickerOn = new System.Windows.Forms.DateTimePicker();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.testDataSet = new Delivery.TestDataSet();
            this.materialBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.materialTableAdapter = new Delivery.TestDataSetTableAdapters.MaterialTableAdapter();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonCreateOrder = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelAllOrder = new System.Windows.Forms.Label();
            this.labelActiveOrder = new System.Windows.Forms.Label();
            this.labelInactive = new System.Windows.Forms.Label();
            this.labelCompleteOrder = new System.Windows.Forms.Label();
            this.labelCancelOrder = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.materialBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(95, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "С: ";
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Location = new System.Drawing.Point(121, 12);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(137, 20);
            this.dateTimePickerFrom.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(390, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "По: ";
            // 
            // dateTimePickerOn
            // 
            this.dateTimePickerOn.Location = new System.Drawing.Point(423, 12);
            this.dateTimePickerOn.Name = "dateTimePickerOn";
            this.dateTimePickerOn.Size = new System.Drawing.Size(138, 20);
            this.dateTimePickerOn.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.Column1});
            this.dataGridView1.DataSource = this.materialBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(98, 47);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(463, 208);
            this.dataGridView1.TabIndex = 4;
            // 
            // testDataSet
            // 
            this.testDataSet.DataSetName = "TestDataSet";
            this.testDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // materialBindingSource
            // 
            this.materialBindingSource.DataMember = "Material";
            this.materialBindingSource.DataSource = this.testDataSet;
            // 
            // materialTableAdapter
            // 
            this.materialTableAdapter.ClearBeforeFill = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Наименование";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.Width = 200;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Масса проданной продукции";
            this.Column1.Name = "Column1";
            this.Column1.Width = 200;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(360, 427);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Закрыть";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonCreateOrder
            // 
            this.buttonCreateOrder.Location = new System.Drawing.Point(200, 427);
            this.buttonCreateOrder.Name = "buttonCreateOrder";
            this.buttonCreateOrder.Size = new System.Drawing.Size(108, 23);
            this.buttonCreateOrder.TabIndex = 6;
            this.buttonCreateOrder.Text = "Составить отчет";
            this.buttonCreateOrder.UseVisualStyleBackColor = true;
            this.buttonCreateOrder.Click += new System.EventHandler(this.buttonCreateOrder_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(95, 273);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Общее количество заказов: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(95, 295);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(168, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Количество активных заказов: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(95, 318);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(180, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Количество неактивных заказов: ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(95, 363);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(182, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Количество отмененных заказов: ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(95, 340);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(189, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Количество выполненных заказов: ";
            // 
            // labelAllOrder
            // 
            this.labelAllOrder.AutoSize = true;
            this.labelAllOrder.Location = new System.Drawing.Point(255, 273);
            this.labelAllOrder.Name = "labelAllOrder";
            this.labelAllOrder.Size = new System.Drawing.Size(35, 13);
            this.labelAllOrder.TabIndex = 12;
            this.labelAllOrder.Text = "label8";
            // 
            // labelActiveOrder
            // 
            this.labelActiveOrder.AutoSize = true;
            this.labelActiveOrder.Location = new System.Drawing.Point(269, 295);
            this.labelActiveOrder.Name = "labelActiveOrder";
            this.labelActiveOrder.Size = new System.Drawing.Size(35, 13);
            this.labelActiveOrder.TabIndex = 13;
            this.labelActiveOrder.Text = "label8";
            // 
            // labelInactive
            // 
            this.labelInactive.AutoSize = true;
            this.labelInactive.Location = new System.Drawing.Point(281, 318);
            this.labelInactive.Name = "labelInactive";
            this.labelInactive.Size = new System.Drawing.Size(35, 13);
            this.labelInactive.TabIndex = 14;
            this.labelInactive.Text = "label8";
            // 
            // labelCompleteOrder
            // 
            this.labelCompleteOrder.AutoSize = true;
            this.labelCompleteOrder.Location = new System.Drawing.Point(290, 340);
            this.labelCompleteOrder.Name = "labelCompleteOrder";
            this.labelCompleteOrder.Size = new System.Drawing.Size(35, 13);
            this.labelCompleteOrder.TabIndex = 15;
            this.labelCompleteOrder.Text = "label8";
            // 
            // labelCancelOrder
            // 
            this.labelCancelOrder.AutoSize = true;
            this.labelCancelOrder.Location = new System.Drawing.Point(283, 363);
            this.labelCancelOrder.Name = "labelCancelOrder";
            this.labelCancelOrder.Size = new System.Drawing.Size(35, 13);
            this.labelCancelOrder.TabIndex = 16;
            this.labelCancelOrder.Text = "label8";
            // 
            // FormStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 473);
            this.Controls.Add(this.labelCancelOrder);
            this.Controls.Add(this.labelCompleteOrder);
            this.Controls.Add(this.labelInactive);
            this.Controls.Add(this.labelActiveOrder);
            this.Controls.Add(this.labelAllOrder);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonCreateOrder);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.dateTimePickerOn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Controls.Add(this.label1);
            this.Name = "FormStatistics";
            this.Text = "Статистика";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormStatistics_FormClosed);
            this.Load += new System.EventHandler(this.FormStatistics_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.materialBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePickerOn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private TestDataSet testDataSet;
        private System.Windows.Forms.BindingSource materialBindingSource;
        private TestDataSetTableAdapters.MaterialTableAdapter materialTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonCreateOrder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelAllOrder;
        private System.Windows.Forms.Label labelActiveOrder;
        private System.Windows.Forms.Label labelInactive;
        private System.Windows.Forms.Label labelCompleteOrder;
        private System.Windows.Forms.Label labelCancelOrder;
    }
}