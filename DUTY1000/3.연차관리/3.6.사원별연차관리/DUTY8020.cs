﻿using System;
using System.Data;
using System.Windows.Forms;
using SilkRoad.Common;
using System.Text;
using DevExpress.XtraGrid.Views;
using System.Drawing;
using DevExpress.XtraReports.UI;

namespace DUTY1000
{
    public partial class duty8020 : SilkRoad.Form.Base.FormX
    {
        CommonLibrary clib = new CommonLibrary();
        static DataProcessing dp = new DataProcessing();

        ClearNEnableControls cec = new ClearNEnableControls();
        public DataSet ds = new DataSet();
        DataProcFunc df = new DataProcFunc();
        SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();
        string _Flag = "";

        public duty8020()
        {
            InitializeComponent();
        }

        #region 0. Initialization

        /// <summary>
        ///컨트롤 초기화 및 활성,비활성 설정
        /// </summary>
        /// <param name="enable"></param>
        private void SetCancel()
        {
            cec.SetClearControls(srGroupBox5, new string[] { "cmb_type" });
			//cec.SetClearControls(srPanel7, new string[] { "" });
			grd_yc.DataSource = null;
			grd.DataSource = null;

            txt_change.Enabled = false;
            SetButtonEnable("001");

            dat_sldt.Focus();
        }

        #endregion

        #region 1 Form

        private void duty8020_Load(object sender, EventArgs e)
        {
            SetCancel();
			dat_sldt.DateTime = DateTime.Now;
			lb_yccj.Text = "";

			btn_search.PerformClick();
        }

        #endregion

        #region 2 Button
		
		private void btn_expand_Click(object sender, EventArgs e)
		{			
            clib.SetExpandFold(btn_expand, srSplitContainer1, DevExpress.XtraEditors.SplitPanelVisibility.Panel1);
		}
		//연차촉진
		private void btn_yccj_Click(object sender, EventArgs e)
		{
			if (isNoError_um(1))
			{
				try
				{
					string c_nm = cmb_yccj.SelectedIndex == 0 ? "CHK1" : "CHK2";

					DataTable table = ds.Tables["SEARCH_YC"].Clone();
					for (int i = 0; i < grdv1.RowCount; i++)
					{
						if (grdv1.GetDataRow(i)[c_nm].ToString() == "1")
							table.ImportRow(grdv1.GetDataRow(i));
					}
					dp.AddDatatable2Dataset("COPY_SEARCH_YC", table, ref ds);

					sendemail sm = new sendemail(ds, cmb_yccj.SelectedIndex + 1);
					sm.StartPosition = FormStartPosition.CenterScreen;
					sm.ShowDialog();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				finally
				{
					string sldt = clib.DateToText(dat_sldt.DateTime);
					df.GetSEARCH_YCDatas(sldt, ds);
					grd1.DataSource = ds.Tables["SEARCH_YC"];
					string yc1 = ds.Tables["SEARCH_YC"].Compute("COUNT(SAWON_NO)", "CHK1=1").ToString();
					string yc2 = ds.Tables["SEARCH_YC"].Compute("COUNT(SAWON_NO)", "CHK2=1").ToString();
					lb_yccj.Text = "[" + sldt.Substring(0, 4) + "." + sldt.Substring(4, 2) + "." + sldt.Substring(6, 2) + " 기준 1차촉진대상 " + yc1 + "명, 2차촉진대상 " + yc2 + "명]";
				}
			}
		}
		
        //조회
        private void btn_search_Click(object sender, EventArgs e)
        {
			if (isNoError_um(1))
			{
				string sldt = clib.DateToText(dat_sldt.DateTime);
				df.GetSEARCH_YCDatas(sldt, ds);
				grd1.DataSource = ds.Tables["SEARCH_YC"];
				//string.Format("{0:#,##0.##}", Convert.ToDecimal(drow["YC_TOTAL"].ToString()))
				string yc1 = string.Format("{0:#,##0.##}", Convert.ToDecimal(ds.Tables["SEARCH_YC"].Compute("COUNT(SAWON_NO)", "CHK1=1").ToString()));
				string yc2 = string.Format("{0:#,##0.##}", Convert.ToDecimal(ds.Tables["SEARCH_YC"].Compute("COUNT(SAWON_NO)", "CHK2=1").ToString()));
				lb_yccj.Text = "[" + sldt.Substring(0, 4) + "." + sldt.Substring(4, 2) + "." + sldt.Substring(6, 2) + " 기준 1차촉진대상 " + yc1 + "명, 2차촉진대상 " + yc2 + "명]";
			}
        }
		//연차산정
		private void btn_yc_Click(object sender, EventArgs e)
		{
			if (isNoError_um(1))
			{
				Cursor = Cursors.WaitCursor;
				string sldt = clib.DateToText(dat_sldt.DateTime);
				df.GetUSP_MAKE_YCDatas(sldt, ds);

				if (ds.Tables["SEARCH_YC"] != null)
				{
					df.GetSEARCH_YCDatas(sldt, ds);
					grd1.DataSource = ds.Tables["SEARCH_YC"];
				}
				Cursor = Cursors.Default;
				
				MessageBox.Show("연차산정 작업이 완료되었습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

        //저장버튼
        private void btn_save_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            int outVal = 0;
            try
            {
                DataRow hrow = ds.Tables["DUTY_TRSDYYC"].Select("YC_YEAR = '" + clib.DateToText(dat_year.DateTime).Substring(0, 4) + "' AND SAWON_NO = '" + txt_sabn.Text.ToString().Trim() + "'")[0];

				hrow["YC_CHANGE"] = clib.TextToDecimal(txt_change.Text.ToString());
				hrow["YC_TOTAL"] = clib.TextToDecimal(txt_base.Text.ToString()) + clib.TextToDecimal(txt_change.Text.ToString()) + clib.TextToDecimal(txt_first.Text.ToString()) + clib.TextToDecimal(txt_add.Text.ToString());
				hrow["MOD_DT"] = gd.GetNow();
                hrow["MOD_ID"] = SilkRoad.Config.SRConfig.USID;

				string[] tableNames = new string[] { "DUTY_TRSDYYC" };
				SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
				outVal = cmd.setUpdate(ref ds, tableNames, null);

				if (outVal <= 0)
					MessageBox.Show("수정된 내용이 없습니다.", "수정", MessageBoxButtons.OK, MessageBoxIcon.Information);
				else
					MessageBox.Show("수정되었습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ec)
            {
                MessageBox.Show(ec.Message, "수정오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
				df.GetSEARCH_YCDatas(clib.DateToText(dat_sldt.DateTime), ds);
				grd1.DataSource = ds.Tables["SEARCH_YC"];
                SetCancel();
                Cursor = Cursors.Default;
            }
        }

        //삭제
        private void btn_del_Click(object sender, EventArgs e)
        {
			DialogResult dr = MessageBox.Show("해당사원의 연차정보를 삭제하시겠습니까?", "삭제여부", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			if (dr == DialogResult.OK)
			{
				int outVal = 0;
				try
				{
					ds.Tables["DUTY_TRSDYYC"].Select("YC_YEAR = '" + clib.DateToText(dat_year.DateTime).Substring(0, 4) + "' AND SAWON_NO = '" + txt_sabn.Text.ToString().Trim() + "'")[0].Delete();

					string[] tableNames = new string[] { "DUTY_TRSDYYC" };
					SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
					outVal = cmd.setUpdate(ref ds, tableNames, null);

					if (outVal > 0)
						MessageBox.Show("해당사원의 연차정보가 삭제되었습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch (Exception ec)
				{
					MessageBox.Show(ec.Message, "삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					df.GetSEARCH_YCDatas(clib.DateToText(dat_sldt.DateTime), ds);
					grd1.DataSource = ds.Tables["SEARCH_YC"];
					SetCancel();
					Cursor = Cursors.Default;
				}
			}			
        }

        /// <summary>취소버튼</summary>
        private void btn_clear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("해당 내역을 취소하시겠습니까?", "취소", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                               == DialogResult.OK)
            {
                SetCancel();
            }
        }

        /// <summary> refresh버튼 </summary>
        private void btn_refresh_Click(object sender, EventArgs e)
        {
			df.GetSEARCH_MSTGNMUDatas(ds);
			grd1.DataSource = ds.Tables["SEARCH_MSTGNMU"];
		}

        /// <summary>엑셀버튼</summary>
        private void btn_exel_Click(object sender, EventArgs e)
        {
			clib.gridToExcel(grdv1, this.Name + "_" + clib.DateToText(DateTime.Now));
		}

        #endregion

        #region 3 EVENT

        private void duty8020_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)   //취소
                btn_clear.PerformClick();            
        }
		
		//그리드 더블클릭시 코드 조회
		private void grdv1_DoubleClick(object sender, EventArgs e)
		{
			dat_year.DateTime = clib.TextToDate(grdv1.GetFocusedRowCellValue("YC_YEAR").ToString()+"0101");
			txt_sabn.Text = grdv1.GetFocusedRowCellValue("SAWON_NO").ToString();

			df.GetDUTY_TRSDYYCDatas(grdv1.GetFocusedRowCellValue("YC_YEAR").ToString(), grdv1.GetFocusedRowCellValue("SAWON_NO").ToString(), ds);
			if (ds.Tables["DUTY_TRSDYYC"].Rows.Count > 0)
			{
				DataRow drow = ds.Tables["DUTY_TRSDYYC"].Rows[0];
				txt_name.Text = drow["SAWON_NM"].ToString();
				cmb_type.SelectedIndex = clib.TextToInt(drow["YC_TYPE"].ToString());
				dat_indt.DateTime = clib.TextToDate(drow["IN_DATE"].ToString());
				txt_base.Text = drow["YC_BASE"].ToString();
				txt_first.Text = drow["YC_FIRST"].ToString();
				txt_add.Text = drow["YC_ADD"].ToString();
				txt_tcnt.Text = drow["YC_SUM"].ToString();

				txt_change.Text = drow["YC_CHANGE"].ToString();
				txt_change.Enabled = true;
				txt_use.Text = "";
				txt_rcnt.Text = drow["YC_TOTAL"].ToString();

				df.GetSEARCH_TRSHREQDatas(drow["SAWON_NO"].ToString(), drow["YC_YEAR"].ToString(), ds);
				grd_yc.DataSource = ds.Tables["SEARCH_TRSHREQ"];
				
				if (ds.Tables["SEARCH_TRSHREQ"].Rows.Count > 0)
					txt_use.Text = ds.Tables["SEARCH_TRSHREQ"].Compute("SUM(YC_DAYS)", null).ToString();
				
				txt_rcnt.Text = (clib.TextToDecimal(txt_tcnt.Text.ToString()) + clib.TextToDecimal(txt_change.Text.ToString()) - clib.TextToDecimal(txt_use.Text.ToString())).ToString();

				df.GetSEARCH_DUTY_MSTYCCJDatas(drow["SAWON_NO"].ToString(), drow["YC_YEAR"].ToString(), "", "", ds);
				grd.DataSource = ds.Tables["SEARCH_DUTY_MSTYCCJ"];

				SetButtonEnable("111");
			}
		}		
		//연차촉진 미리보기
		private void grdv_DoubleClick(object sender, EventArgs e)
		{
			DataRow drow = grdv.GetFocusedDataRow();
			if (drow == null)
				return;

			df.GetSEARCH_DUTY_MSTYCCJDatas(txt_sabn.Text.ToString(), clib.DateToText(dat_year.DateTime).Substring(0, 4), drow["DOC_TYPE"].ToString(), drow["YC_SQ"].ToString(), ds);
			Rpt_8020 r = new Rpt_8020(drow["SAWON_NO"].ToString(), SilkRoad.Config.SRConfig.WorkPlaceName, ds);
			r.DataSource = ds.Tables["SEARCH_YC"].Select("SAWON_NO = '" + drow["SAWON_NO"] + "'").CopyToDataTable();
			//r.ShowPreview();
			
            r.CreateDocument();
            if (drow["DOC_TYPE"].ToString() == "202101") //1차촉진양식
            {
                Rpt_8021 r2 = new Rpt_8021(drow["SAWON_NO"].ToString(), SilkRoad.Config.SRConfig.WorkPlaceName, ds);
                r2.DataSource = ds.Tables["SEARCH_YC"].Select("SAWON_NO = '" + drow["SAWON_NO"] + "'").CopyToDataTable();
                r2.CreateDocument();
                r.Pages.AddRange(r2.Pages);
            }
            else if (drow["DOC_TYPE"].ToString() == "202102") //2차촉진양식
            {
                Rpt_8022 r2 = new Rpt_8022(drow["SAWON_NO"].ToString(), SilkRoad.Config.SRConfig.WorkPlaceName, ds);
                r2.DataSource = ds.Tables["SEARCH_YC"].Select("SAWON_NO = '" + drow["SAWON_NO"] + "'").CopyToDataTable();
                r2.CreateDocument();
                r.Pages.AddRange(r2.Pages);
            }
            r.ShowPreview();
		}
		
        #endregion

        #region 7. Error Check

        /// <summary>
        /// 모드에 따른 컨트롤 유효성체크
        /// </summary>
        /// <param name="mode">1:처리모드(키값검사), 2:입력,수정모드 </param>
        /// <returns></returns>
        private bool isNoError_um(int mode)
        {
            bool isError = false;

			if (mode == 1)  //조회
			{
				if (clib.DateToText(dat_sldt.DateTime) == "")
				{
					MessageBox.Show("기준일자를 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_sldt.Focus();
					return false;
				}
				else
				{
					isError = true;
				}
			}

            return isError;
        }

        #endregion

        #region 9. ETC

        /// <summary>
        /// 배열에따른 버튼상태설정
        /// </summary>
        /// <param name="mode"></param>
        private void SetButtonEnable(string arr)
        {
            btn_save.Enabled = arr.Substring(0, 1) == "1" ? true : false;
            btn_del.Enabled  = arr.Substring(1, 1) == "1" ? true : false;
            btn_clear.Enabled = arr.Substring(2, 1) == "1" ? true : false;
        }


		#endregion

	}
}
