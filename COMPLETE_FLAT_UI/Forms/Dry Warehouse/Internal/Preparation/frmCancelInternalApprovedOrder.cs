﻿using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ULTRAMAVERICK.Models;

namespace ULTRAMAVERICK.Forms.Dry_Warehouse.Internal.Preparation
{
    public partial class frmCancelInternalApprovedOrder : MaterialForm
    {
        ViewApprovedItemsInternalOrder ths;
        DataSet dSet = new DataSet();
        myclasses myClass = new myclasses();
        myclasses xClass = new myclasses();
        IStoredProcedures g_objStoredProcCollection = null;
        IStoredProcedures objStorProc = null;
        PopupNotifierClass GlobalStatePopup = new PopupNotifierClass();
        int validate = 0;
        public frmCancelInternalApprovedOrder(ViewApprovedItemsInternalOrder frm)
        {
            InitializeComponent();
            ths = frm;
            textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
        }

        private void frmCancelInternalApprovedOrder_Load(object sender, EventArgs e)
        {
            this.ConnectionInit();
            //this.loadRemarksDropdown();
            this.ClearTextboxesState();
        }

        private void ClearTextboxesState()
        {
            this.textBox1.Text = String.Empty;
            this.cboReason.Items.Clear();
        }

        public void loadRemarksDropdown()
        {

            this.myClass.fillComboBoxDepartment(this.cboReason, 
            "internal_order_activation_remarks_activated_minor_typecancel", dSet);


            //this.lblItemClassID.Text = cboItemClass.SelectedValue.ToString();
        }

        private void ConnectionInit()
        {
            g_objStoredProcCollection = myClass.g_objStoredProc.GetCollections(); // Main Stored Procedure Collections
            objStorProc = xClass.g_objStoredProc.GetCollections(); //Call the StoreProcedure With Class
        }





        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ths.textBox2.Text = textBox1.Text;
        }

        private void matBtnSave_Click(object sender, EventArgs e)
        {
            if (this.cboReason.Text == String.Empty)
            {
                this.GlobalStatePopup.FillRequiredFields();
                this.cboReason.Focus();
                return;
            }

            if (MetroFramework.MetroMessageBox.Show(this, "Cancel the consolidated order? ", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.validate = 1;
                this.textBox1.Text = this.cboReason.Text;
                this.Close();
            }
            else
            {
                return;
            }
        }

        private void frmCancelInternalApprovedOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(this.validate == 1)
            {
                this.textBox1.Text = this.cboReason.Text;
            }

   
        }

        private void cboReason_Click(object sender, EventArgs e)
        {
            this.loadRemarksDropdown();
        }
    }
}
