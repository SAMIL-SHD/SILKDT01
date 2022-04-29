﻿using System;
using System.Data;
using System.Windows.Forms;
using SilkRoad.Common;
using System.Data.OleDb;
using System.Collections;

namespace DUTY1000
{
    public partial class duty3030 : SilkRoad.Form.Base.FormX
    {
        CommonLibrary clib = new CommonLibrary();

        ClearNEnableControls cec = new ClearNEnableControls();
        public DataSet ds = new DataSet();
        DataProcFunc df = new DataProcFunc();
        SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();
        private string ends_yn = "";

        public duty3030()
        {
            InitializeComponent();
        }

        #region 0. Initialization

        private void SetCancel(int stat)
		{
			if (stat == 1)
			{
				if (ds.Tables["DUTY_TRSHREQ"] != null)
					ds.Tables["DUTY_TRSHREQ"].Clear();
				grd_search.DataSource = null;
				SetButtonEnable("100");
				dat_s_yymm.Enabled = false;
			}
			else if (stat == 2)
			{
				if (ds.Tables["SEARCH_LAST_YC"] != null)
					ds.Tables["SEARCH_LAST_YC"].Clear();
				grd1.DataSource = null;
				SetButtonEnable2("100");
				dat_yymm.Enabled = false;
			}
			else if (stat == 3)
			{
				if (ds.Tables["SEARCH_DUTY_TRSHREQ"] != null)
					ds.Tables["SEARCH_DUTY_TRSHREQ"].Clear();
				pv_grd.DataSource = null;
			}
		}

        #endregion

        #region 1 Form

        private void duty3030_Load(object sender, EventArgs e)
        {
			dat_s_yymm.DateTime = DateTime.Now;
			dat_yymm.DateTime = DateTime.Now;

			dat_frmm.DateTime = clib.TextToDate(clib.DateToText(DateTime.Now).Substring(0, 4) + "0101");
			dat_tomm.DateTime = DateTime.Now;
        }
		private void duty3030_Shown(object sender, EventArgs e)
		{
			SetCancel(1);
		}

        #endregion

        #region 2 Button
		
		
		private void btn_s_search_Click(object sender, EventArgs e)
		{
			if (isNoError_um(1))
			{
				df.GetDEL_DUTY_TRSHREQDatas(clib.DateToText(dat_s_yymm.DateTime).Substring(0, 6), ds);
				grd_search.DataSource = ds.Tables["DEL_DUTY_TRSHREQ"];				
				SetButtonEnable("011");
				dat_s_yymm.Enabled = true;
			}
		}

		//삭제
		private void btn_del_Click(object sender, EventArgs e)
		{
			string yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 4) + "." + clib.DateToText(dat_yymm.DateTime).Substring(4, 2);
			END_CHK(clib.DateToText(dat_yymm.DateTime).Substring(0, 6));
			if (ends_yn == "Y")
			{
				MessageBox.Show(yymm + "월은 최종 마감되어 삭제할 수 없습니다.", "마감완료", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (ds.Tables["DEL_DUTY_TRSHREQ"].Rows.Count > 0)
			{
				DialogResult dr = MessageBox.Show(yymm + "월의 연차정산내역을 모두 삭제하시겠습니까?", "삭제여부", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
				if (dr == DialogResult.OK)
				{
					int outVal = 0;
					try
					{
						for (int i = 0; i < ds.Tables["DEL_DUTY_TRSHREQ"].Rows.Count; i++)
						{
							ds.Tables["DEL_DUTY_TRSHREQ"].Rows[i].Delete();
						}

						string[] tableNames = new string[] { "DEL_DUTY_TRSHREQ" };
						SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
						outVal = cmd.setUpdate(ref ds, tableNames, null);
					}
					catch (Exception ec)
					{
						MessageBox.Show(ec.Message, "삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					finally
					{
						if (outVal > 0)
							MessageBox.Show(yymm + "월의 연차정산내역이 삭제되었습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);

						SetCancel(1);
						Cursor = Cursors.Default;
					}
				}
			}
			else
			{
                MessageBox.Show(yymm+"월은 삭제할 내역이 없습니다.", "삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void btn_s_canc_Click(object sender, EventArgs e)
		{
			SetCancel(1);
		}
		//엑셀변환
		private void btn_excel_Click(object sender, EventArgs e)
		{
            clib.gridToExcel(grdv_search, "연차정산내역_" + clib.DateToText(DateTime.Now), true);
		}

		//처리
		private void btn_proc_Click(object sender, EventArgs e)
		{
			if (isNoError_um(2))
			{
				df.GetSEARCH_LAST_YCDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), ds);

				for (int i = 0; i < ds.Tables["BASE_LAST_YC"].Rows.Count; i++)
				{
					DataRow drow = ds.Tables["BASE_LAST_YC"].Rows[i];
					if (ds.Tables["SEARCH_LAST_YC"].Select("SABN = '" + drow["SABN"].ToString() + "'").Length > 0)
					{
						DataRow nrow = ds.Tables["SEARCH_LAST_YC"].Select("SABN = '" + drow["SABN"].ToString() + "'")[0];
						nrow["YYMM"] = clib.DateToText(dat_yymm.DateTime).Substring(0, 6);
						nrow["SABN"] = drow["SABN"].ToString();
						nrow["YC_YEAR"] = drow["YC_YEAR"].ToString();
						nrow["USE_TODT"] = drow["USE_TODT"].ToString();
						nrow["SABN_NM"] = drow["SABN_NM"].ToString();
						nrow["DEPT_NM"] = drow["DEPT_NM"].ToString();
						nrow["YC_SUM"] = clib.TextToDecimal(drow["YC_SUM"].ToString());
						nrow["YC_CHANGE"] = clib.TextToDecimal(drow["YC_CHANGE"].ToString());
						nrow["YC_TOTAL"] = clib.TextToDecimal(drow["YC_TOTAL"].ToString());
						nrow["YC_USE"] = clib.TextToDecimal(drow["YC_USE"].ToString());
						nrow["YC_REMAIN"] = clib.TextToDecimal(drow["YC_REMAIN"].ToString());
						nrow["YC_MI_CNT"] = clib.TextToDecimal(drow["YC_MI_CNT"].ToString());
					}
					else
					{
						DataRow nrow = ds.Tables["SEARCH_LAST_YC"].NewRow();
						nrow["YYMM"] = clib.DateToText(dat_yymm.DateTime).Substring(0, 6);
						nrow["SABN"] = drow["SABN"].ToString();
						nrow["YC_YEAR"] = drow["YC_YEAR"].ToString();
						nrow["USE_TODT"] = drow["USE_TODT"].ToString();
						nrow["SABN_NM"] = drow["SABN_NM"].ToString();
						nrow["DEPT_NM"] = drow["DEPT_NM"].ToString();
						nrow["YC_SUM"] = clib.TextToDecimal(drow["YC_SUM"].ToString());
						nrow["YC_CHANGE"] = clib.TextToDecimal(drow["YC_CHANGE"].ToString());
						nrow["YC_TOTAL"] = clib.TextToDecimal(drow["YC_TOTAL"].ToString());
						nrow["YC_USE"] = clib.TextToDecimal(drow["YC_USE"].ToString());
						nrow["YC_REMAIN"] = clib.TextToDecimal(drow["YC_REMAIN"].ToString());
						nrow["YC_MI_CNT"] = clib.TextToDecimal(drow["YC_MI_CNT"].ToString());
						ds.Tables["SEARCH_LAST_YC"].Rows.Add(nrow);
					}
				}

				grd1.DataSource = ds.Tables["SEARCH_LAST_YC"];				
				SetButtonEnable2("011");
				dat_yymm.Enabled = false;
			}
		}
        //저장
		private void btn_save_Click(object sender, EventArgs e)
		{
			string yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 4) + "." + clib.DateToText(dat_yymm.DateTime).Substring(4, 2);
			END_CHK(clib.DateToText(dat_yymm.DateTime).Substring(0, 6));
			if (ends_yn == "Y")
			{
				MessageBox.Show(yymm + "월은 최종 마감되어 저장할 수 없습니다.", "마감완료", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (MessageBox.Show("해당 연차내역을 저장하시겠습니까?", "저장", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
							== DialogResult.OK)
			{
				Cursor = Cursors.WaitCursor;
				int outVal = 0;
				try
				{
					for (int i = 0; i < ds.Tables["SEARCH_LAST_YC"].Rows.Count; i++)
					{
						DataRow drow = ds.Tables["SEARCH_LAST_YC"].Rows[i];
						if (clib.TextToDecimal(drow["YC_MI_CNT"].ToString()) != 0)
						{
							df.GetDUTY_TRSHREQ_JSDatas(drow["SABN"].ToString(), drow["USE_TODT"].ToString(), ds);
							if (ds.Tables["DUTY_TRSHREQ"].Select("SABN = '" + drow["SABN"].ToString() + "' AND REQ_YEAR = '" + drow["YC_YEAR"].ToString() + "' AND REQ_DATE = '" + drow["USE_TODT"].ToString() + "'").Length > 0)
							{
								DataRow nrow = ds.Tables["DUTY_TRSHREQ"].Select("SABN = '" + drow["SABN"].ToString() + "' AND REQ_YEAR = '" + drow["YC_YEAR"].ToString() + "' AND REQ_DATE = '" + drow["USE_TODT"].ToString() + "'")[0];
								nrow["REQ_TYPE"] = "";
								nrow["REQ_TYPE2"] = "";
								nrow["YC_DAYS"] = clib.TextToDecimal(drow["YC_MI_CNT"].ToString());
								nrow["AP_TAG"] = "9";

								nrow["UPDT"] = gd.GetNow();
								nrow["USID"] = SilkRoad.Config.SRConfig.USID;
							}
							else
							{
								DataRow nrow = ds.Tables["DUTY_TRSHREQ"].NewRow();
								nrow["SABN"] = drow["SABN"].ToString();
								nrow["REQ_YEAR"] = drow["YC_YEAR"].ToString();
								nrow["REQ_DATE"] = drow["USE_TODT"].ToString();
								nrow["REQ_DATE2"] = drow["USE_TODT"].ToString();
								nrow["REQ_TYPE"] = "";
								nrow["REQ_TYPE2"] = "";
								nrow["YC_DAYS"] = clib.TextToDecimal(drow["YC_MI_CNT"].ToString());
								nrow["AP_TAG"] = "9";

								nrow["INDT"] = gd.GetNow();
								nrow["UPDT"] = "";
								nrow["USID"] = SilkRoad.Config.SRConfig.USID;
								nrow["PSTY"] = "A";
								ds.Tables["DUTY_TRSHREQ"].Rows.Add(nrow);
							}

							string[] tableNames = new string[] { "DUTY_TRSHREQ" };
							SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
							outVal = cmd.setUpdate(ref ds, tableNames, null);
						}
					}
				}
				catch (Exception ec)
				{
					MessageBox.Show(ec.Message, "저장오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					if (outVal > 0)
						MessageBox.Show("해당 내용이 저장되었습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
					SetCancel(2);
					Cursor = Cursors.Default;
				}
			}
		}		
		//취소
		private void btn_canc_Click(object sender, EventArgs e)
		{
			SetCancel(2);
		}
		
		//연차내역조회
		private void btn_search2_Click(object sender, EventArgs e)
		{
			if (isNoError_um(3))
			{
				df.GetSEARCH_DUTY_TRSHREQDatas(clib.DateToText(dat_frmm.DateTime).Substring(0, 6), clib.DateToText(dat_tomm.DateTime).Substring(0, 6), ds);
				pv_grd.DataSource = ds.Tables["SEARCH_DUTY_TRSHREQ"];
			}
		}		
		private void btn_canc2_Click(object sender, EventArgs e)
		{
			SetCancel(3);
		}
		private void btn_s_excel_Click(object sender, EventArgs e)
		{
			clib.gridToExcel(pv_grd, "연차내역조회_"  + clib.DateToText(DateTime.Now), true);
		}

        #endregion

        #region 3 EVENT

        private void duty3030_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)   //종료
            {
                btn_exit.PerformClick();
            }
        }
		
		private void END_CHK(string yymm)
		{
			df.Get2020_SEARCH_ENDSDatas(yymm, ds);
			if (ds.Tables["2020_SEARCH_ENDS"].Rows.Count > 0) //마감월이 저장되어 있으면
			{
				DataRow irow = ds.Tables["2020_SEARCH_ENDS"].Rows[0];
				ends_yn = irow["CLOSE_YN"].ToString();
			}
			else
			{
				ends_yn = "";
			}
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
            bool isError = true;
            if (mode == 1)  //처리내역 조회
            {
                if (clib.DateToText(dat_s_yymm.DateTime) == "")
                {
                    MessageBox.Show("조회년월을 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dat_s_yymm.Focus();
                    return false;
				}
                else
                {
                    isError = true;
                }
            }
            else if (mode == 2)  //처리
            {
                if (clib.DateToText(dat_yymm.DateTime) == "")
                {
                    MessageBox.Show("조회년월을 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dat_yymm.Focus();
                    return false;
				}
                else
                {
                    isError = true;
                }
            }
            else if (mode == 3) //조회
            {
                if (clib.DateToText(dat_frmm.DateTime) == "")
                {
                    MessageBox.Show("조회년월(fr)을 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dat_frmm.Focus();
                    return false;
				}
                else if (clib.DateToText(dat_tomm.DateTime) == "")
                {
                    MessageBox.Show("조회년월(to)을 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dat_tomm.Focus();
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
		
		private void SetButtonEnable(string arr)
		{
			btn_s_search.Enabled = arr.Substring(0, 1) == "1" ? true : false;
			btn_del.Enabled = arr.Substring(1, 1) == "1" ? true : false;
			btn_s_canc.Enabled = arr.Substring(2, 1) == "1" ? true : false;
		}

		private void SetButtonEnable2(string arr)
		{
			btn_proc.Enabled = arr.Substring(0, 1) == "1" ? true : false;
			btn_save.Enabled = arr.Substring(1, 1) == "1" ? true : false;
			btn_canc.Enabled = arr.Substring(2, 1) == "1" ? true : false;
		}


		#endregion

	}
}
