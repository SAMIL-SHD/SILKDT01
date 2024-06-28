using System;
using System.Data;
using System.Windows.Forms;
using SilkRoad.Common;

namespace DUTY1000
{
    public partial class duty9020 : SilkRoad.Form.Base.FormX
    {
        CommonLibrary clib = new CommonLibrary();

        ClearNEnableControls cec = new ClearNEnableControls();
        public DataSet ds = new DataSet();
        DataProcFunc df = new DataProcFunc();
        SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();

        public duty9020()
        {
            InitializeComponent();
        }

        #region 0. Initialization

        #endregion

        #region 1 Form

        private void duty9020_Load(object sender, EventArgs e)
        {
			sl_sdcd1.EditValue = null;
			sl_sdcd2.EditValue = null;
			sl_sdcd3.EditValue = null;
			sl_sdcd4.EditValue = null;
			sl_sdcd5.EditValue = null;
			sl_sdcd6.EditValue = null;
			sl_sdcd7.EditValue = null;
			sl_sdcd8.EditValue = null;
			sl_sdcd010.EditValue = null;
			sl_sdcd011.EditValue = null;
			sl_sdcd012.EditValue = null;
			
			sl_gjcd1.EditValue = null;

			sl_gtcd11.EditValue = null;
			sl_gtcd12.EditValue = null;
			sl_gtcd13.EditValue = null;
			sl_gtcd14.EditValue = null;
			sl_gtcd21.EditValue = null;
			sl_gtcd22.EditValue = null;
			sl_gtcd31.EditValue = null;
			sl_gtcd41.EditValue = null;
			sl_gtcd51.EditValue = null;
			sl_gtcd61.EditValue = null;
			sl_gtcd71.EditValue = null;
			sl_gtcd72.EditValue = null;
			sl_gtcd81.EditValue = null;
        }
		private void duty9020_Shown(object sender, EventArgs e)
		{
			df.GetSL_SDDatas(ds);
			sl_sdcd1.Properties.DataSource = ds.Tables["SL_SD"];
			sl_sdcd2.Properties.DataSource = ds.Tables["SL_SD"];
			sl_sdcd3.Properties.DataSource = ds.Tables["SL_SD"];
			sl_sdcd4.Properties.DataSource = ds.Tables["SL_SD"];
			sl_sdcd5.Properties.DataSource = ds.Tables["SL_SD"];
			sl_sdcd6.Properties.DataSource = ds.Tables["SL_SD"];
			sl_sdcd7.Properties.DataSource = ds.Tables["SL_SD"];
			sl_sdcd8.Properties.DataSource = ds.Tables["SL_SD"];
			sl_sdcd010.Properties.DataSource = ds.Tables["SL_SD"];
			sl_sdcd011.Properties.DataSource = ds.Tables["SL_SD"];
			sl_sdcd012.Properties.DataSource = ds.Tables["SL_SD"];
			
			df.GetSL_GJDatas(ds);
			sl_gjcd1.Properties.DataSource = ds.Tables["SL_GJ"];

			df.GetSL_GTDatas(ds);
			sl_gtcd11.Properties.DataSource = ds.Tables["SL_GT"];
			sl_gtcd12.Properties.DataSource = ds.Tables["SL_GT"];
			sl_gtcd13.Properties.DataSource = ds.Tables["SL_GT"];
			sl_gtcd14.Properties.DataSource = ds.Tables["SL_GT"];
			sl_gtcd21.Properties.DataSource = ds.Tables["SL_GT"];
			sl_gtcd22.Properties.DataSource = ds.Tables["SL_GT"];
			sl_gtcd31.Properties.DataSource = ds.Tables["SL_GT"];
			sl_gtcd41.Properties.DataSource = ds.Tables["SL_GT"];
			sl_gtcd51.Properties.DataSource = ds.Tables["SL_GT"];
			sl_gtcd61.Properties.DataSource = ds.Tables["SL_GT"];
			sl_gtcd71.Properties.DataSource = ds.Tables["SL_GT"];
			sl_gtcd72.Properties.DataSource = ds.Tables["SL_GT"];
			sl_gtcd81.Properties.DataSource = ds.Tables["SL_GT"];
			sl_gtcd91.Properties.DataSource = ds.Tables["SL_GT"];
			sl_gtcd010.Properties.DataSource = ds.Tables["SL_GT"];			
			sl_gtcd011.Properties.DataSource = ds.Tables["SL_GT"];

			df.GetDUTY_INFOSD02Datas(ds);
			if (ds.Tables["DUTY_INFOSD02"].Rows.Count > 0)
			{
				DataRow srow = ds.Tables["DUTY_INFOSD02"].Rows[0];
				
				sl_sdcd1.EditValue = srow["A01"].ToString().Trim() == "" ? null : srow["A01"].ToString().Trim();
				sl_sdcd2.EditValue = srow["A02"].ToString().Trim() == "" ? null : srow["A02"].ToString().Trim();
				sl_sdcd3.EditValue = srow["A03"].ToString().Trim() == "" ? null : srow["A03"].ToString().Trim();
				sl_sdcd4.EditValue = srow["A04"].ToString().Trim() == "" ? null : srow["A04"].ToString().Trim();
				cmb_off_type.SelectedIndex = clib.TextToInt(srow["A04_TYPE"].ToString().Trim());
				sl_sdcd5.EditValue = srow["A05"].ToString().Trim() == "" ? null : srow["A05"].ToString().Trim();
				cmb_night_type.SelectedIndex = clib.TextToInt(srow["A05_TYPE"].ToString().Trim());
				sl_sdcd6.EditValue = srow["A06"].ToString().Trim() == "" ? null : srow["A06"].ToString().Trim();
				sl_sdcd7.EditValue = srow["A07"].ToString().Trim() == "" ? null : srow["A07"].ToString().Trim();
				sl_sdcd8.EditValue = srow["A08"].ToString().Trim() == "" ? null : srow["A08"].ToString().Trim();
				
				sl_sdcd010.EditValue = srow["A010"].ToString().Trim() == "" ? null : srow["A010"].ToString().Trim();
				sl_sdcd011.EditValue = srow["A011"].ToString().Trim() == "" ? null : srow["A011"].ToString().Trim();
				sl_sdcd012.EditValue = srow["A012"].ToString().Trim() == "" ? null : srow["A012"].ToString().Trim();
				
				sl_gjcd1.EditValue = srow["B01"].ToString().Trim() == "" ? null : srow["B01"].ToString().Trim();
				txt_b01_fee01.Text = srow["B01_FEE01"].ToString();

				sl_gtcd11.EditValue = srow["A11"].ToString().Trim() == "" ? null : srow["A11"].ToString().Trim();
				sl_gtcd12.EditValue = srow["A12"].ToString().Trim() == "" ? null : srow["A12"].ToString().Trim();
				sl_gtcd13.EditValue = srow["A13"].ToString().Trim() == "" ? null : srow["A13"].ToString().Trim();
				sl_gtcd14.EditValue = srow["A14"].ToString().Trim() == "" ? null : srow["A14"].ToString().Trim();
				sl_gtcd21.EditValue = srow["A21"].ToString().Trim() == "" ? null : srow["A21"].ToString().Trim();
				sl_gtcd22.EditValue = srow["A22"].ToString().Trim() == "" ? null : srow["A22"].ToString().Trim();
				sl_gtcd31.EditValue = srow["A31"].ToString().Trim() == "" ? null : srow["A31"].ToString().Trim();
				sl_gtcd41.EditValue = srow["A41"].ToString().Trim() == "" ? null : srow["A41"].ToString().Trim();
				sl_gtcd51.EditValue = srow["A51"].ToString().Trim() == "" ? null : srow["A51"].ToString().Trim();
				sl_gtcd61.EditValue = srow["A61"].ToString().Trim() == "" ? null : srow["A61"].ToString().Trim();
				sl_gtcd71.EditValue = srow["A71"].ToString().Trim() == "" ? null : srow["A71"].ToString().Trim();
				sl_gtcd72.EditValue = srow["A72"].ToString().Trim() == "" ? null : srow["A72"].ToString().Trim();
				sl_gtcd81.EditValue = srow["A81"].ToString().Trim() == "" ? null : srow["A81"].ToString().Trim();
				sl_gtcd91.EditValue = srow["C91"].ToString().Trim() == "" ? null : srow["C91"].ToString().Trim();
				sl_gtcd010.EditValue = srow["C010"].ToString().Trim() == "" ? null : srow["C010"].ToString().Trim();
				sl_gtcd011.EditValue = srow["C011"].ToString().Trim() == "" ? null : srow["C011"].ToString().Trim();

				txt_dfee.Text = srow["A01_DFEE"].ToString();
				txt_d01.Text = srow["A01_D01"].ToString();
				txt_d02.Text = srow["A01_D02"].ToString();
				txt_nfee.Text = srow["A01_NFEE"].ToString();
				txt_n01.Text = srow["A01_N01"].ToString();
				txt_n02.Text = srow["A01_N02"].ToString();

				txt_a02_insu01.Text = srow["A02_INSU01"].ToString();
				txt_a02_insu02.Text = srow["A02_INSU02"].ToString();
				
				txt_a03_insu01.Text = srow["A03_INSU01"].ToString();
				txt_a03_insu02.Text = srow["A03_INSU02"].ToString();
				
				txt_a010_insu01.Text = srow["A010_INSU01"].ToString();
				txt_a010_insu02.Text = srow["A010_INSU02"].ToString();

				txt_a04_insu01.Text = srow["A04_INSU01"].ToString();
				txt_a04_insu02.Text = srow["A04_INSU02"].ToString();
				txt_a04_insu11.Text = srow["A04_INSU11"].ToString();
				txt_a04_insu12.Text = srow["A04_INSU12"].ToString();

				txt_a06_insu01.Text = srow["A06_INSU01"].ToString();
				
				txt_a07_insu01.Text = srow["A07_INSU01"].ToString();
				txt_a07_insu02.Text = srow["A07_INSU02"].ToString();
				txt_a07_insu11.Text = srow["A07_INSU11"].ToString();
				txt_a07_insu12.Text = srow["A07_INSU12"].ToString();

				txt_a08_insu01.Text = srow["A08_INSU01"].ToString();				
			}
		}

        #endregion

        #region 2 Button
		
        //저장
		private void btn_save_Click(object sender, EventArgs e)
		{			
            Cursor = Cursors.WaitCursor;
            int outVal = 0;
            try
            {
				df.GetDUTY_INFOSD02Datas(ds);
				DataRow nrow;
				if (ds.Tables["DUTY_INFOSD02"].Rows.Count > 0)
				{
					nrow = ds.Tables["DUTY_INFOSD02"].Rows[0];
				}
				else
				{
					nrow = ds.Tables["DUTY_INFOSD02"].NewRow();
					ds.Tables["DUTY_INFOSD02"].Rows.Add(nrow);
				}

				nrow["A01"] = sl_sdcd1.EditValue == null ? "" : sl_sdcd1.EditValue.ToString();
				nrow["A02"] = sl_sdcd2.EditValue == null ? "" : sl_sdcd2.EditValue.ToString();
				nrow["A03"] = sl_sdcd3.EditValue == null ? "" : sl_sdcd3.EditValue.ToString();
				nrow["A04"] = sl_sdcd4.EditValue == null ? "" : sl_sdcd4.EditValue.ToString();
				nrow["A04_TYPE"] = cmb_off_type.SelectedIndex.ToString();

				nrow["A05"] = sl_sdcd5.EditValue == null ? "" : sl_sdcd5.EditValue.ToString();
				nrow["A05_TYPE"] = cmb_night_type.SelectedIndex.ToString();
				nrow["A06"] = sl_sdcd6.EditValue == null ? "" : sl_sdcd6.EditValue.ToString();
				nrow["A07"] = sl_sdcd7.EditValue == null ? "" : sl_sdcd7.EditValue.ToString();
				nrow["A08"] = sl_sdcd8.EditValue == null ? "" : sl_sdcd8.EditValue.ToString();
				
				nrow["A010"] = sl_sdcd010.EditValue == null ? "" : sl_sdcd010.EditValue.ToString();
				nrow["A011"] = sl_sdcd011.EditValue == null ? "" : sl_sdcd011.EditValue.ToString();
				nrow["A012"] = sl_sdcd012.EditValue == null ? "" : sl_sdcd012.EditValue.ToString();
				
				nrow["B01"] = sl_gjcd1.EditValue == null ? "" : sl_gjcd1.EditValue.ToString();
				nrow["B01_FEE01"] = clib.TextToDecimal(txt_b01_fee01.Text.ToString());
				
				nrow["A11"] = sl_gtcd11.EditValue == null ? "" : sl_gtcd11.EditValue.ToString();
				nrow["A12"] = sl_gtcd12.EditValue == null ? "" : sl_gtcd12.EditValue.ToString();
				nrow["A13"] = sl_gtcd13.EditValue == null ? "" : sl_gtcd13.EditValue.ToString();
				nrow["A14"] = sl_gtcd14.EditValue == null ? "" : sl_gtcd14.EditValue.ToString();
				nrow["A21"] = sl_gtcd21.EditValue == null ? "" : sl_gtcd21.EditValue.ToString();
				nrow["A22"] = sl_gtcd22.EditValue == null ? "" : sl_gtcd22.EditValue.ToString();
				nrow["A31"] = sl_gtcd31.EditValue == null ? "" : sl_gtcd31.EditValue.ToString();
				nrow["A41"] = sl_gtcd41.EditValue == null ? "" : sl_gtcd41.EditValue.ToString();
				nrow["A51"] = sl_gtcd51.EditValue == null ? "" : sl_gtcd51.EditValue.ToString();
				nrow["A61"] = sl_gtcd61.EditValue == null ? "" : sl_gtcd61.EditValue.ToString();
				nrow["A71"] = sl_gtcd71.EditValue == null ? "" : sl_gtcd71.EditValue.ToString();
				nrow["A72"] = sl_gtcd72.EditValue == null ? "" : sl_gtcd72.EditValue.ToString();
				nrow["A81"] = sl_gtcd81.EditValue == null ? "" : sl_gtcd81.EditValue.ToString();
				nrow["C91"] = sl_gtcd91.EditValue == null ? "" : sl_gtcd91.EditValue.ToString();
				nrow["C010"] = sl_gtcd010.EditValue == null ? "" : sl_gtcd010.EditValue.ToString();
				nrow["C011"] = sl_gtcd011.EditValue == null ? "" : sl_gtcd011.EditValue.ToString();

				nrow["A01_DFEE"] = clib.TextToDecimal(txt_dfee.Text.ToString());
				nrow["A01_D01"] = clib.TextToDecimal(txt_d01.Text.ToString());
				nrow["A01_D02"] = clib.TextToDecimal(txt_d02.Text.ToString());
				nrow["A01_NFEE"] = clib.TextToDecimal(txt_nfee.Text.ToString());
				nrow["A01_N01"] = clib.TextToDecimal(txt_n01.Text.ToString());
				nrow["A01_N02"] = clib.TextToDecimal(txt_n02.Text.ToString());
					
				nrow["A02_INSU01"] = clib.TextToDecimal(txt_a02_insu01.Text.ToString());
				nrow["A02_INSU02"] = clib.TextToDecimal(txt_a02_insu02.Text.ToString());
					
				nrow["A03_INSU01"] = clib.TextToDecimal(txt_a03_insu01.Text.ToString());
				nrow["A03_INSU02"] = clib.TextToDecimal(txt_a03_insu02.Text.ToString());
					
				nrow["A010_INSU01"] = clib.TextToDecimal(txt_a03_insu01.Text.ToString());
				nrow["A010_INSU02"] = clib.TextToDecimal(txt_a03_insu02.Text.ToString());
					
				nrow["A04_INSU01"] = clib.TextToDecimal(txt_a04_insu01.Text.ToString());
				nrow["A04_INSU02"] = clib.TextToDecimal(txt_a04_insu02.Text.ToString());
				nrow["A04_INSU11"] = clib.TextToDecimal(txt_a04_insu11.Text.ToString());
				nrow["A04_INSU12"] = clib.TextToDecimal(txt_a04_insu12.Text.ToString());
					
				nrow["A06_INSU01"] = clib.TextToDecimal(txt_a06_insu01.Text.ToString());
					
				nrow["A07_INSU01"] = clib.TextToDecimal(txt_a07_insu01.Text.ToString());
				nrow["A07_INSU02"] = clib.TextToDecimal(txt_a07_insu02.Text.ToString());
				nrow["A07_INSU11"] = clib.TextToDecimal(txt_a07_insu11.Text.ToString());
				nrow["A07_INSU12"] = clib.TextToDecimal(txt_a07_insu12.Text.ToString());
				
				nrow["A08_INSU01"] = clib.TextToDecimal(txt_a08_insu01.Text.ToString());				

				nrow["REG_DT"] = gd.GetNow();
				nrow["REG_ID"] = SilkRoad.Config.SRConfig.USID;	

				string[] tableNames = new string[] { "DUTY_INFOSD02" };
				SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
				outVal = cmd.setUpdate(ref ds, tableNames, null);				
            }
            catch (Exception ec)
            {
                MessageBox.Show(ec.Message, "저장오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
				if (outVal > 0)							
					MessageBox.Show("해당 내용이 저장되었습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.Dispose();
            }
		}
    
        #endregion

        #region 3 EVENT

        private void duty9020_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)   //종료
            {
                btn_exit.PerformClick();
            }
        }

		#endregion

		#region 9. ETC

		#endregion

	}
}
