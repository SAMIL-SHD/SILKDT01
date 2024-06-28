using System;
using System.Data;
using System.Windows.Forms;
using SilkRoad.Common;
using System.Drawing;

namespace DUTY1000
{
    public partial class duty2031 : SilkRoad.Form.Base.FormX
    {
        CommonLibrary clib = new CommonLibrary();

        ClearNEnableControls cec = new ClearNEnableControls();
        public DataSet ds = new DataSet();
        DataProcFunc df = new DataProcFunc();
        SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();
        private string ends_yn = "";
        private string p_dpcd = "";
        private string dept = "";

        public duty2031(string _p_dpcd)
        {
            InitializeComponent();
            p_dpcd = _p_dpcd;
        }

        #region 0. Initialization

        private void SetCancel(int stat)
		{
            dept = "";
            df.Get2030_SEARCH_EMBSDatas(p_dpcd, ds);
			sl_embs.Properties.DataSource = ds.Tables["2030_SEARCH_EMBS"];
			sl_embs3.Properties.DataSource = ds.Tables["2030_SEARCH_EMBS"];
            df.Get2030_DEPTDatas(p_dpcd, ds);
            sl_dept.Properties.DataSource = ds.Tables["2030_DEPT"];

            if (stat == 3)
			{
				btn_save.Text = "저장";
				btn_save.Image = DUTY1000.Properties.Resources.저장;
				sl_embs.Enabled = true;
				dat_svdt.Enabled = true;

				txt_save.Enabled = false;
				txt_time.Enabled = false;
				mm_remk.Enabled = false;
				txt_save.Text = "";
				txt_time.Text = "";
				mm_remk.Text = "";

				SetButtonEnable("1000");
			}
			else if (stat == 33)
			{
				btn_save3.Text = "저장";
				btn_save3.Image = DUTY1000.Properties.Resources.저장;
				sl_embs3.Enabled = true;
				dat_js_date.Enabled = true;

				txt_js_time.Enabled = false;
				txt_calc_time.Enabled = false;
				txt_time3.Enabled = false;
				mm_remk3.Enabled = false;
				txt_js_time.Text = "";
				txt_calc_time.Text = "";
				txt_time3.Text = "";
				mm_remk3.Text = "";

				SetButtonEnable3("1000");
			}
			lb_ends.Text = "";
			lb_ends3.Text = "";
			//if (ds.Tables["S_DUTY_TRSWORK"] != null)
			//	ds.Tables["S_DUTY_TRSWORK"].Clear();
			//grd1.DataSource = null;
			//SetButtonEnable("1000");
		}

        #endregion

        #region 1 Form

        private void duty2031_Load(object sender, EventArgs e)
        {
        }
		private void duty2031_Shown(object sender, EventArgs e)
		{
			dat_yymm.DateTime = DateTime.Now;
            sl_dept.EditValue = null;
            dat_year.DateTime = DateTime.Now;
			dat_yymm3.DateTime = DateTime.Now;

			sl_embs.EditValue = null;
			sl_embs3.EditValue = null;

			SetCancel(3);
			SetCancel(33);

            if (p_dpcd != "%")
                sl_dept.EditValue = p_dpcd;
            //df.GetSEARCH_SAVE_ENDSDatas("", clib.DateToText(dat_year.DateTime).Substring(0, 4), ds);
            //grd_end.DataSource = ds.Tables["SEARCH_SAVE_ENDS"];
        }

		#endregion

		#region 2 Button
		
		//조회
		private void btn_search_Click(object sender, EventArgs e)
		{
			if (isNoError_um(1))
			{
				string yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 6);
				df.GetS_DUTY_TRSSVTMDatas(p_dpcd, yymm, ds);
				grd1.DataSource = ds.Tables["S_DUTY_TRSSVTM"];

				if (ds.Tables["S_DUTY_TRSSVTM"].Rows.Count == 0)
					MessageBox.Show("조회된 내역이 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
		//엑셀변환
		private void btn_excel_Click(object sender, EventArgs e)
		{			
            clib.gridToExcel(grdv1, "SAVE시간관리_" + clib.DateToText(DateTime.Now), true);
		}
		
		//처리
		private void btn_proc_Click(object sender, EventArgs e)
		{
			if (isNoError_um(2))
			{
				txt_save.Enabled = true;
				txt_time.Enabled = true;
				mm_remk.Enabled = true;

				string sldt = clib.DateToText(dat_svdt.DateTime);
				df.GetDUTY_TRSSVTMDatas(sl_embs.EditValue.ToString(), sldt, ds);
				if (ds.Tables["DUTY_TRSSVTM"].Rows.Count > 0)
				{
					DataRow drow = ds.Tables["DUTY_TRSSVTM"].Rows[0];
					txt_save.Text = drow["SAVE_TIME"].ToString();
					txt_time.Text = drow["TIME_REMK"].ToString();
					mm_remk.Text = drow["REMARK"].ToString();

					btn_save.Text = "수정";
					btn_save.Image = DUTY1000.Properties.Resources.수정;
					SetButtonEnable("0111");
				}
				else
				{
					btn_save.Text = "저장";
					btn_save.Image = DUTY1000.Properties.Resources.저장;
					SetButtonEnable("0101");
				}
			}
		}
        //저장
		private void btn_save_Click(object sender, EventArgs e)
		{
			if (isNoError_um(3))
			{
				if (MessageBox.Show("SAVE시간을 저장하시겠습니까?", "저장", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
								== DialogResult.OK)
				{
					Cursor = Cursors.WaitCursor;
					int outVal = 0;
					try
					{
						DataRow hrow;
						string sldt = clib.DateToText(dat_svdt.DateTime);
						df.GetDUTY_TRSSVTMDatas(sl_embs.EditValue.ToString(), sldt, ds);
						if (ds.Tables["DUTY_TRSSVTM"].Rows.Count > 0)
						{
							hrow = ds.Tables["DUTY_TRSSVTM"].Rows[0];
							hrow["UPDT"] = gd.GetNow(); //수정
							hrow["PSTY"] = "U";
						}
						else
						{
							hrow = ds.Tables["DUTY_TRSSVTM"].NewRow();
							hrow["SABN"] = sl_embs.EditValue.ToString();
							hrow["SAVE_DATE"] = clib.DateToText(dat_svdt.DateTime);
							hrow["INDT"] = gd.GetNow();
							hrow["UPDT"] = "";
							hrow["PSTY"] = "A";
							ds.Tables["DUTY_TRSSVTM"].Rows.Add(hrow);
						}
						hrow["SAVE_TIME"] = clib.TextToDecimal(txt_save.Text.ToString());
						hrow["TIME_REMK"] = txt_time.Text.ToString();
						hrow["REMARK"] = mm_remk.Text.ToString();
						hrow["USID"] = SilkRoad.Config.SRConfig.USID;

						string[] tableNames = new string[] { "DUTY_TRSSVTM", };
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
						SetCancel(3);
						string yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 6);
						df.GetS_DUTY_TRSSVTMDatas(p_dpcd, yymm, ds);
						grd1.DataSource = ds.Tables["S_DUTY_TRSSVTM"];
						Cursor = Cursors.Default;
					}
				}
			}
		}
		//삭제
		private void btn_del_Click(object sender, EventArgs e)
		{
			if (isNoError_um(4))
			{
				string sldt = clib.DateToText(dat_svdt.DateTime);
				df.GetDUTY_TRSSVTMDatas(sl_embs.EditValue.ToString(), sldt, ds);
				if (ds.Tables["DUTY_TRSSVTM"].Rows.Count > 0)
				{
					DialogResult dr = MessageBox.Show("해당 SAVE시간을 삭제하시겠습니까?", "삭제여부", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
					if (dr == DialogResult.OK)
					{
						int outVal = 0;
						try
						{
							DataRow drow = ds.Tables["DUTY_TRSSVTM"].Rows[0];

							df.GetDEL_TRSSVTMDatas(ds);
							DataRow hrow = ds.Tables["DEL_TRSSVTM"].NewRow();
							hrow["SABN"] = drow["SABN"].ToString();
							hrow["SAVE_DATE"] = drow["SAVE_DATE"].ToString();
							hrow["SAVE_TIME"] = clib.TextToDecimal(drow["SAVE_TIME"].ToString());
							hrow["TIME_REMK"] = drow["TIME_REMK"].ToString();
							hrow["REMARK"] = drow["REMARK"].ToString();
							hrow["INDT"] = drow["INDT"].ToString();
							hrow["UPDT"] = drow["UPDT"].ToString();
							hrow["USID"] = drow["USID"].ToString();
							hrow["PSTY"] = drow["PSTY"].ToString();

							hrow["DEL_DT"] = gd.GetNow();
							hrow["DEL_ID"] = SilkRoad.Config.SRConfig.USID;
							ds.Tables["DEL_TRSSVTM"].Rows.Add(hrow);

							ds.Tables["DUTY_TRSSVTM"].Rows[0].Delete();

							string[] tableNames = new string[] { "DEL_TRSSVTM", "DUTY_TRSSVTM" };
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
								MessageBox.Show("해당 SAVE시간이 삭제되었습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);

							SetCancel(3);
							string yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 6);
							df.GetS_DUTY_TRSSVTMDatas(p_dpcd, yymm, ds);
							grd1.DataSource = ds.Tables["S_DUTY_TRSSVTM"];
							Cursor = Cursors.Default;
						}
					}
				}
				else
				{
					MessageBox.Show("삭제할 내역이 없습니다.", "삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
		//취소
		private void btn_canc_Click(object sender, EventArgs e)
		{
			SetCancel(3);
		}
		
		//마감조회
		private void btn_search2_Click(object sender, EventArgs e)
		{
			if (isNoError_um(11))
			{
                sl_dept.Enabled = false;
                dat_year.Enabled = false;

                df.GetSEARCH_SAVE_ENDSDatas(sl_dept.EditValue.ToString(), sl_dept.Text.ToString(), clib.DateToText(dat_year.DateTime).Substring(0, 4), ds);
				grd_end.DataSource = ds.Tables["SEARCH_SAVE_ENDS"];
			}
		}
		//마감조회 취소
		private void btn_clear_Click(object sender, EventArgs e)
        {
            sl_dept.Enabled = true;
            dat_year.Enabled = true;
            grd_end.DataSource = null;
		}

		
		//조회
		private void btn_search3_Click(object sender, EventArgs e)
		{
			if (isNoError_um(31))
			{
				string yymm = clib.DateToText(dat_yymm3.DateTime).Substring(0, 6);
				df.GetS_DUTY_TRSOVTM_JSDatas(p_dpcd, yymm, ds);
				grd3.DataSource = ds.Tables["S_DUTY_TRSOVTM_JS"];

				if (ds.Tables["S_DUTY_TRSOVTM_JS"].Rows.Count == 0)
					MessageBox.Show("조회된 내역이 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
		//엑셀변환
		private void btn_excel3_Click(object sender, EventArgs e)
		{			
            clib.gridToExcel(grdv3, "정산시간_" + clib.DateToText(DateTime.Now), true);
		}
		
		//처리
		private void btn_proc3_Click(object sender, EventArgs e)
		{
			if (isNoError_um(32))
			{
				txt_js_time.Enabled = true;
				txt_calc_time.Enabled = true;
				txt_time3.Enabled = true;
				mm_remk3.Enabled = true;

				string sldt = clib.DateToText(dat_js_date.DateTime);
				df.GetDUTY_TRSOVTM_JSDatas(sl_embs3.EditValue.ToString(), sldt, ds);
				if (ds.Tables["DUTY_TRSOVTM_JS"].Rows.Count > 0)
				{
					DataRow drow = ds.Tables["DUTY_TRSOVTM_JS"].Rows[0];
					txt_js_time.Text = drow["JS_TIME"].ToString();
					txt_calc_time.Text = drow["CALC_TIME"].ToString();
					txt_time3.Text = drow["TIME_REMK"].ToString();
					mm_remk3.Text = drow["REMARK"].ToString();

					btn_save3.Text = "수정";
					btn_save3.Image = DUTY1000.Properties.Resources.수정;
					SetButtonEnable3("0111");
				}
				else
				{
					btn_save3.Text = "저장";
					btn_save3.Image = DUTY1000.Properties.Resources.저장;
					SetButtonEnable3("0101");
				}
			}
		}
        //저장
		private void btn_save3_Click(object sender, EventArgs e)
		{
			if (isNoError_um(33))
			{
				if (MessageBox.Show("정산시간을 저장하시겠습니까?", "저장", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
								== DialogResult.OK)
				{
					Cursor = Cursors.WaitCursor;
					int outVal = 0;
					try
					{
						DataRow hrow;
						string sldt = clib.DateToText(dat_js_date.DateTime);
						df.GetDUTY_TRSOVTM_JSDatas(sl_embs3.EditValue.ToString(), sldt, ds);
						if (ds.Tables["DUTY_TRSOVTM_JS"].Rows.Count > 0)
						{
							hrow = ds.Tables["DUTY_TRSOVTM_JS"].Rows[0];
							hrow["UPDT"] = gd.GetNow(); //수정
							hrow["PSTY"] = "U";
						}
						else
						{
							hrow = ds.Tables["DUTY_TRSOVTM_JS"].NewRow();
							hrow["SABN"] = sl_embs3.EditValue.ToString();
							hrow["JS_DATE"] = clib.DateToText(dat_js_date.DateTime);
							hrow["INDT"] = gd.GetNow();
							hrow["UPDT"] = "";
							hrow["PSTY"] = "A";
							ds.Tables["DUTY_TRSOVTM_JS"].Rows.Add(hrow);
						}
						hrow["JS_TIME"] = clib.TextToDecimal(txt_js_time.Text.ToString());
						hrow["CALC_TIME"] = clib.TextToDecimal(txt_calc_time.Text.ToString());
						hrow["TIME_REMK"] = txt_time3.Text.ToString();
						hrow["REMARK"] = mm_remk3.Text.ToString();
						hrow["USID"] = SilkRoad.Config.SRConfig.USID;

						string[] tableNames = new string[] { "DUTY_TRSOVTM_JS", };
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
						SetCancel(3);
						string yymm = clib.DateToText(dat_yymm3.DateTime).Substring(0, 6);
						df.GetS_DUTY_TRSOVTM_JSDatas(p_dpcd, yymm, ds);
						grd3.DataSource = ds.Tables["S_DUTY_TRSOVTM_JS"];
						Cursor = Cursors.Default;
					}
				}
			}
		}
		//삭제
		private void btn_del3_Click(object sender, EventArgs e)
		{
			if (isNoError_um(34))
			{
				string sldt = clib.DateToText(dat_js_date.DateTime);
				df.GetDUTY_TRSOVTM_JSDatas(sl_embs3.EditValue.ToString(), sldt, ds);
				if (ds.Tables["DUTY_TRSOVTM_JS"].Rows.Count > 0)
				{
					DialogResult dr = MessageBox.Show("해당 정산시간을 삭제하시겠습니까?", "삭제여부", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
					if (dr == DialogResult.OK)
					{
						int outVal = 0;
						try
						{
							DataRow drow = ds.Tables["DUTY_TRSOVTM_JS"].Rows[0];

							//df.GetDEL_TRSSVTMDatas(ds);
							//DataRow hrow = ds.Tables["DEL_TRSSVTM"].NewRow();
							//hrow["SABN"] = drow["SABN"].ToString();
							//hrow["SAVE_DATE"] = drow["SAVE_DATE"].ToString();
							//hrow["SAVE_TIME"] = clib.TextToDecimal(drow["SAVE_TIME"].ToString());
							//hrow["TIME_REMK"] = drow["TIME_REMK"].ToString();
							//hrow["REMARK"] = drow["REMARK"].ToString();
							//hrow["INDT"] = drow["INDT"].ToString();
							//hrow["UPDT"] = drow["UPDT"].ToString();
							//hrow["USID"] = drow["USID"].ToString();
							//hrow["PSTY"] = drow["PSTY"].ToString();

							//hrow["DEL_DT"] = gd.GetNow();
							//hrow["DEL_ID"] = SilkRoad.Config.SRConfig.USID;
							//ds.Tables["DEL_TRSSVTM"].Rows.Add(hrow);

							ds.Tables["DUTY_TRSOVTM_JS"].Rows[0].Delete();

							string[] tableNames = new string[] { "DUTY_TRSOVTM_JS" };
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
								MessageBox.Show("해당 정산시간이 삭제되었습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);

							SetCancel(3);
							string yymm = clib.DateToText(dat_yymm3.DateTime).Substring(0, 6);
							df.GetS_DUTY_TRSOVTM_JSDatas(p_dpcd, yymm, ds);
							grd3.DataSource = ds.Tables["S_DUTY_TRSOVTM_JS"];
							Cursor = Cursors.Default;
						}
					}
				}
				else
				{
					MessageBox.Show("삭제할 내역이 없습니다.", "삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
		//취소
		private void btn_canc3_Click(object sender, EventArgs e)
		{
			SetCancel(33);
		}

        #endregion

        #region 3 EVENT

        private void duty2031_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)   //종료
            {
                btn_exit.PerformClick();
            }
        }
		
		//더블클릭시 수정
		private void grdv1_DoubleClick(object sender, EventArgs e)
		{
			DataRow drow = grdv1.GetFocusedDataRow();
            if (drow == null)
                return;

			SetCancel(3);
            sl_embs.EditValue = drow["SABN"].ToString().Trim();
			dat_svdt.DateTime = clib.TextToDate(drow["SAVE_DATE"].ToString());
			btn_proc.PerformClick();
		}
		private void grdv3_DoubleClick(object sender, EventArgs e)
		{
			DataRow drow = grdv3.GetFocusedDataRow();
            if (drow == null)
                return;

			SetCancel(33);
            sl_embs3.EditValue = drow["SABN"].ToString().Trim();
			dat_js_date.DateTime = clib.TextToDate(drow["JS_DATE"].ToString());
			btn_proc3.PerformClick();
		}
		
		private void END_CHK(int page, string dept, string sldt)
		{
			string yymm_nm = sldt.Substring(0, 4) + "." + sldt.Substring(4, 2);
			string txt_end = "";
			df.GetS_SAVE_ENDSDatas(dept, sldt.Substring(0, 6), ds);
			if (ds.Tables["S_SAVE_ENDS"].Rows.Count > 0) //마감월이 저장되어 있으면
			{
				DataRow irow = ds.Tables["S_SAVE_ENDS"].Rows[0];
				ends_yn = irow["CLOSE_YN"].ToString();
				txt_end = irow["CLOSE_YN"].ToString() == "1" ? "[" + yymm_nm + " SAVE 마감완료]" : irow["CLOSE_YN"].ToString() == "2" ? "[" + yymm_nm + " SAVE 마감전]" : "[ ]";
				
			}
			else
			{
				ends_yn = "";
				txt_end = "[" + yymm_nm + " SAVE마감 작업전]";
			}

			if (page == 1)
				lb_ends.Text = txt_end;
			if (page == 3)
				lb_ends3.Text = txt_end;
		}
		
		private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
		{
			string yymm = grdv_end.GetFocusedDataRow()["END_YYMM"].ToString();
            string dept = grdv_end.GetFocusedDataRow()["DEPT"].ToString();
            if (((DevExpress.XtraEditors.CheckEdit)sender).EditValue.ToString() == "1")
			{
				Set_End(dept, yymm, 1);
				MessageBox.Show("마감 되었습니다.", "월마감", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				Set_End(dept, yymm, 2);
				MessageBox.Show("마감취소 되었습니다.", "월마감", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
		private void Set_End(string dept, string yymm, int stat)
		{
			df.GetDUTY_MSTSAVE_ENDSDatas(dept, yymm, ds);
			DataRow nrow;
			if (ds.Tables["DUTY_MSTSAVE_ENDS"].Rows.Count > 0)
			{
				nrow = ds.Tables["DUTY_MSTSAVE_ENDS"].Rows[0];
				if (stat == 1)
				{
					nrow["END_DT"] = gd.GetNow();
					nrow["END_ID"] = SilkRoad.Config.SRConfig.USID;
				}
				else
				{
					nrow["CANC_DT"] = gd.GetNow();
					nrow["CANC_ID"] = SilkRoad.Config.SRConfig.USID;
				}
			}
			else
			{
				nrow = ds.Tables["DUTY_MSTSAVE_ENDS"].NewRow();
                nrow["DEPT"] = dept;
                nrow["END_YYMM"] = yymm;
				nrow["END_DT"] = stat == 1 ? gd.GetNow() : "";
				nrow["END_ID"] = stat == 1 ? SilkRoad.Config.SRConfig.USID : "";
				nrow["CANC_DT"] = stat == 1 ? "" : gd.GetNow();
				nrow["CANC_ID"] = stat == 1 ? "" : SilkRoad.Config.SRConfig.USID;
				ds.Tables["DUTY_MSTSAVE_ENDS"].Rows.Add(nrow);
			}
			nrow["CLOSE_YN"] = stat == 1 ? "1" : "2";

			string[] tableNames = new string[] { "DUTY_MSTSAVE_ENDS" };  //USP_DUTY2030_END_220809
            string[] qry = stat == 1 ? new string[] { "EXEC USP_DUTY2030_END_230530 '" + dept + "','" + yymm + "','" + SilkRoad.Config.SRConfig.USID + "'" } : new string[] { " DELETE FROM DUTY_SUM_SAVE WHERE DEPT='" + dept + "' AND END_YYMM='" + yymm + "'" };
			SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
			cmd.setUpdate(ref ds, tableNames, qry);
			
			df.GetSEARCH_SAVE_ENDSDatas(dept, sl_dept.Text.ToString(), clib.DateToText(dat_year.DateTime).Substring(0, 4), ds);
			grd_end.DataSource = ds.Tables["SEARCH_SAVE_ENDS"];
			grdv_end.FocusedRowHandle = yymm == "" ? 0 : grdv_end.LocateByValue("END_YYMM", yymm, null);
		}
		
		private void txt_js_time_Leave(object sender, EventArgs e)
		{
			txt_calc_time.Text = (Math.Round((double)clib.TextToDecimal(txt_js_time.Text.ToString()) / 1.5, 1)).ToString();
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
            if (mode == 1)  //조회
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
            else if (mode == 2)  //처리
            {
				if (sl_embs.EditValue == null)
				{
					MessageBox.Show("직원을 선택하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_embs.Focus();
					return false;
				}
				else if (clib.DateToText(dat_svdt.DateTime) == "")
				{
					MessageBox.Show("SAVE 근무일자를 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_svdt.Focus();
					return false;
				}
				else
				{
					isError = true;
				}

                if (sl_embs.EditValue != null)
                    dept = ds.Tables["2030_SEARCH_EMBS"].Select("CODE = '" + sl_embs.EditValue.ToString() + "'")[0]["EMBSDPCD"].ToString();
                END_CHK(1, dept, clib.DateToText(dat_svdt.DateTime));
            }
            else if (mode == 3)  //저장
            {
				if (clib.TextToDecimal(txt_save.Text.ToString()) == 0)
				{
					MessageBox.Show("SAVE 시간을 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					txt_save.Focus();
					return false;
				}

                if (sl_embs.EditValue != null)
                    dept = ds.Tables["2030_SEARCH_EMBS"].Select("CODE = '" + sl_embs.EditValue.ToString() + "'")[0]["EMBSDPCD"].ToString();
                END_CHK(1, dept, clib.DateToText(dat_svdt.DateTime));
				if (ends_yn == "1")
				{
					MessageBox.Show("SAVE 마감되어 저장할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				else
				{
					isError = true;
				}
            }
            else if (mode == 4)  //삭제
            {
				if (clib.DateToText(dat_svdt.DateTime) == "")
				{
					MessageBox.Show("근무일자가 입력되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_svdt.Focus();
					return false;
				}

                if (sl_embs.EditValue != null)
                    dept = ds.Tables["2030_SEARCH_EMBS"].Select("CODE = '" + sl_embs.EditValue.ToString() + "'")[0]["EMBSDPCD"].ToString();
                END_CHK(1, dept, clib.DateToText(dat_svdt.DateTime));
				if (ends_yn == "1")
				{
					MessageBox.Show("SAVE 마감되어 삭제할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				else
				{
					isError = true;
				}
            }
            if (mode == 11)  //마감조회
            {
                if (sl_dept.EditValue == null)
                {
                    MessageBox.Show("부서를 선택하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sl_dept.Focus();
                    return false;
				}
                else if (clib.DateToText(dat_year.DateTime) == "")
                {
                    MessageBox.Show("조회년도를 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dat_year.Focus();
                    return false;
                }
                else
                {
                    isError = true;
                }
            }
            if (mode == 31)  //조회
            {
                if (clib.DateToText(dat_yymm3.DateTime) == "")
                {
                    MessageBox.Show("조회년월을 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dat_yymm3.Focus();
                    return false;
				}
                else 
                {
                    isError = true;
                }
            }
            else if (mode == 32)  //처리
            {
				if (sl_embs3.EditValue == null)
				{
					MessageBox.Show("직원을 선택하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_embs3.Focus();
					return false;
				}
				else if (clib.DateToText(dat_js_date.DateTime) == "")
				{
					MessageBox.Show("정산일자를 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_js_date.Focus();
					return false;
				}
				else
				{
					isError = true;
				}

                if (sl_embs.EditValue != null)
                    dept = ds.Tables["2030_SEARCH_EMBS"].Select("CODE = '" + sl_embs3.EditValue.ToString() + "'")[0]["EMBSDPCD"].ToString();
                END_CHK(3, dept, clib.DateToText(dat_js_date.DateTime));
            }
            else if (mode == 33)  //저장
            {
				if (clib.TextToDecimal(txt_js_time.Text.ToString()) == 0)
				{
					MessageBox.Show("정산시간을 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					txt_js_time.Focus();
					return false;
				}

                if (sl_embs.EditValue != null)
                    dept = ds.Tables["2030_SEARCH_EMBS"].Select("CODE = '" + sl_embs3.EditValue.ToString() + "'")[0]["EMBSDPCD"].ToString();
                END_CHK(3, dept, clib.DateToText(dat_js_date.DateTime));
				if (ends_yn == "1")
				{
					MessageBox.Show("SAVE 마감되어 저장할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				else
				{
					isError = true;
				}
            }
            else if (mode == 34)  //삭제
            {
				if (clib.DateToText(dat_js_date.DateTime) == "")
				{
					MessageBox.Show("정산일자가 입력되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_js_date.Focus();
					return false;
				}

                if (sl_embs.EditValue != null)
                    dept = ds.Tables["2030_SEARCH_EMBS"].Select("CODE = '" + sl_embs3.EditValue.ToString() + "'")[0]["EMBSDPCD"].ToString();
                END_CHK(3, dept, clib.DateToText(dat_js_date.DateTime));
				if (ends_yn == "1")
				{
					MessageBox.Show("SAVE 마감되어 삭제할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

		#region 9.ETC
		
		private void SetButtonEnable(string arr)
		{
			btn_proc.Enabled = arr.Substring(0, 1) == "1" ? true : false;
			btn_save.Enabled = arr.Substring(1, 1) == "1" ? true : false;
			btn_del.Enabled = arr.Substring(2, 1) == "1" ? true : false;
			btn_canc.Enabled = arr.Substring(3, 1) == "1" ? true : false;
		}
		private void SetButtonEnable3(string arr)
		{
			btn_proc3.Enabled = arr.Substring(0, 1) == "1" ? true : false;
			btn_save3.Enabled = arr.Substring(1, 1) == "1" ? true : false;
			btn_del3.Enabled = arr.Substring(2, 1) == "1" ? true : false;
			btn_canc3.Enabled = arr.Substring(3, 1) == "1" ? true : false;
		}

		#endregion

	}
}
