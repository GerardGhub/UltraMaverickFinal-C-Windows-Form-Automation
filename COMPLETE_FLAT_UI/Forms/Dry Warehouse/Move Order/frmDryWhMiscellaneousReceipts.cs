﻿using COMPLETE_FLAT_UI.Models;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ULTRAMAVERICK.API.Data;
using ULTRAMAVERICK.API.Entities;
using ULTRAMAVERICK.Models;

namespace ULTRAMAVERICK.Forms.Dry_Warehouse.Move_Order
{
    public partial class frmDryWhMiscellaneousReceipts : MaterialForm
    {
        myclasses xClass = new myclasses();
        frmDryWhMiscellaneousReceipts ths;

        TblCustomersRepository TblCustomerRepo = new TblCustomersRepository();
        TblCustomers TblCustomersEntity = new TblCustomers();
        PopupNotifierClass GlobalStatePopup = new PopupNotifierClass();
        IStoredProcedures g_objStoredProcCollection = null;
        myclasses myClass = new myclasses();
        DataSet dSet = new DataSet();


        DateTime dNow = DateTime.Now;

        int Useridentity = 0;
        int p_id = 0;
        DataSet dSet_temp = new DataSet();

        public frmDryWhMiscellaneousReceipts()
        {
            InitializeComponent();
        }

        private void frmDryWhMiscellaneousReceipts_Load(object sender, EventArgs e)
        {
            this.ConnetionString();
            this.Useridentity = userinfo.user_id;
            this.SearchMethodJarVarCallingSP();
            this.doSearchInTextBox();
            this.LoadItemCodeDropdown();
        }


        public void LoadItemCodeDropdown()
        {

            myClass.fillComboBoxRMDryItemCode(MatCmbSupplierCode, "Suppliers_dropdown", dSet);
            this.MatCmbSupplierCode_SelectionChangeCommitted(new object(), new System.EventArgs());
        }

        private void ConnetionString()
        {
            g_objStoredProcCollection = myClass.g_objStoredProc.GetCollections(); 

        }

        DataSet dset_emp_SearchEngines = new DataSet();
        private void SearchMethodJarVarCallingSP()
        {
            myglobal.global_module = "Active"; 
            dset_emp_SearchEngines.Clear();


            dset_emp_SearchEngines = g_objStoredProcCollection.sp_getMajorTables("DryWHReceipt_Major");

        }

        private void doSearchInTextBox()
        {
            try
            {


                if (dset_emp_SearchEngines.Tables.Count > 0)
                {
                    DataView dv = new DataView(dset_emp_SearchEngines.Tables[0]);

                    if (myglobal.global_module == "Active")
                    {
                        dv.RowFilter = "AddedBy = '" + this.Useridentity + "' ";
                    }

                    this.guna2DgvMaterialPreparation.DataSource = dv;
                    lbltotalrecords.Text = this.guna2DgvMaterialPreparation.RowCount.ToString();
                }
            }
            catch (SyntaxErrorException)
            {
                MessageBox.Show("Invalid character found xxx!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }
            catch (EvaluateException)
            {
                MessageBox.Show("Invalid character found 2.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            this.DataGridVisibilityHidden();


        }

        private void DataGridVisibilityHidden()
        {
            this.guna2DgvMaterialPreparation.Columns["ParentDescription"].Visible = false;
            this.guna2DgvMaterialPreparation.Columns["TransactionNo"].Visible = false;
            this.guna2DgvMaterialPreparation.Columns["LotNumber"].Visible = false;
            this.guna2DgvMaterialPreparation.Columns["AddedBy"].Visible = false;
            this.guna2DgvMaterialPreparation.Columns["DateAdded"].Visible = false;
            this.guna2DgvMaterialPreparation.Columns["IsActive"].Visible = false;
            this.guna2DgvMaterialPreparation.Columns["Supplier"].Visible = false;
            this.guna2DgvMaterialPreparation.Columns["Id"].Visible = false;
            this.guna2DgvMaterialPreparation.Columns["Remarks"].Visible = false;
            this.guna2DgvMaterialPreparation.Columns["LotDescription"].Visible = false;
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            this.MatBtnNew.Enabled = false;
            FrmAddNewMiscellaneousReceipt AddReceipt = new FrmAddNewMiscellaneousReceipt(this);
            AddReceipt.ShowDialog();
        
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(this.textBox1.Text != String.Empty)
            {
                this.MatBtnNew.Enabled = true;
                this.textBox1.Text = String.Empty;
                this.frmDryWhMiscellaneousReceipts_Load(sender, e);
            }
        }

        private void guna2DgvMaterialPreparation_CurrentCellChanged(object sender, EventArgs e)
        {
            if (this.guna2DgvMaterialPreparation.Rows.Count > 0)
            {
                if (this.guna2DgvMaterialPreparation.CurrentRow != null)
                {
                    if (this.guna2DgvMaterialPreparation.CurrentRow.Cells["ItemCode"].Value != null)
                    {
                        this.p_id = Convert.ToInt32(this.guna2DgvMaterialPreparation.CurrentRow.Cells["Id"].Value);
                        this.MatTxtLotNo.Text = this.guna2DgvMaterialPreparation.CurrentRow.Cells["LotNumber"].Value.ToString();
                        this.MatTxtLotDescription.Text = this.guna2DgvMaterialPreparation.CurrentRow.Cells["LotDescription"].Value.ToString();
                        this.MatTxtQty.Text = this.guna2DgvMaterialPreparation.CurrentRow.Cells["Quantity"].Value.ToString();
                        this.MatTxtCategory.Text = this.guna2DgvMaterialPreparation.CurrentRow.Cells["Category"].Value.ToString();
                        this.MatTxtExpiryDays.Text = this.guna2DgvMaterialPreparation.CurrentRow.Cells["ExpiryDays"].Value.ToString();

                    }
                }
            }


        }

        private void MatCmbSupplierCode_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int IndexOf = 0;
            this.MatTxtSupploer.Text = MatCmbSupplierCode.SelectedValue.ToString();


            IndexOf = MatCmbSupplierCode.SelectedIndex;
            xClass.DataSetRMMoverOrderReceipt = g_objStoredProcCollection.sp_getMinorTables("Suppliers_dropdown", null, null, null, null, null);
            this.MatTxtCategory.Text = xClass.DataSetRMMoverOrderReceipt.Tables[0].Rows[IndexOf]["SupplierName"].ToString();


        }

        private void lbltotalrecords_TextChanged(object sender, EventArgs e)
        {
            if (this.lbltotalrecords.Text == "0")
            {
                this.MatBtnSave.Visible = false;
            }
            else
            {
                this.MatBtnSave.Visible = true;
            }
        }

        private void MatBtnSave_Click(object sender, EventArgs e)
        {
            if(this.MatTxtSupploer.Text == String.Empty)
            {
                this.GlobalStatePopup.FillRequiredFields();
                this.MatTxtSupploer.Focus();
                return;
            }

            if(this.MatTxtParentDescription.Text == String.Empty)
            {
                this.GlobalStatePopup.FillRequiredFields();
                this.MatTxtParentDescription.Focus();
                return;
            }

            if (this.matCmbRemarks.Text == String.Empty)
            {
                this.GlobalStatePopup.FillRequiredFields();
                this.matCmbRemarks.Focus();
                return;
            }


        }
    }
}
