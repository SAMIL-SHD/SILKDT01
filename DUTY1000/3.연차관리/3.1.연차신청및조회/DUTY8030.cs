using System;
using System.Data;
using System.Windows.Forms;
using SilkRoad.Common;
using DevExpress.XtraScheduler;
using System.Drawing;

namespace DUTY1000
{
    public partial class duty8030 : SilkRoad.Form.Base.FormX
    {
        CommonLibrary clib = new CommonLibrary();

        ClearNEnableControls cec = new ClearNEnableControls();
        public DataSet ds = new DataSet();
        DataProcFunc df = new DataProcFunc();
        SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();
		
        private string ends_yn = "";
        private string ends_yn2 = "";
		
		private int admin_lv = 0;
        private string msyn = "";
        private string upyn = "";
        private string p_dpcd = "";
		
        private string use_frdt = "";
        private string use_todt = "";
		
        private string seqno = "";
        public duty8030()
        {
            InitializeComponent();
        }

        #region 0. Initialization

        /// <summary>
        ///컨트롤 초기화 및 활성,비활성 설정
        /// </summary>
        /// <param name="enable"></param>
        private void SetCancel(int stat)
        {
			if (stat == 0)
			{
				//dat_yymm.Enabled = true;
				//dat_yymm2.Enabled = true;
				sl_dept.Enabled = p_dpcd == "%" ? true : false;
				
				if (ds.Tables["DUTY_TRSHREQ"] != null)
					ds.Tables["DUTY_TRSHREQ"].Clear();
				if (ds.Tables["SEARCH_AP_YC_LIST"] != null)
					ds.Tables["SEARCH_AP_YC_LIST"].Clear();
				grd1.DataSource = null;
				if (ds.Tables["SEARCH_YC_LIST"] != null)
					ds.Tables["SEARCH_YC_LIST"].Clear();
				grd1.DataSource = null;
				if (ds.Tables["SEARCH_DEL_YC_LIST"] != null)
					ds.Tables["SEARCH_DEL_YC_LIST"].Clear();
				grd_del.DataSource = null;
				schedulerStorage1.Appointments.DataSource = null;
				
				txt_sabn.Text = "";
				txt_name.Text = "";
				txt_base.Text = "";
				txt_change.Text = "";
				txt_first.Text = "";
				txt_add.Text = "";
				txt_tcnt.Text = "";
				txt_use.Text = "";
				txt_rcnt.Text = "";
				
				if (ds.Tables["SEARCH_TRSHREQ"] != null)
					ds.Tables["SEARCH_TRSHREQ"].Clear();
				grd_yc.DataSource = null;
			}

			if (stat == 1)
			{
				dat_ycdt.Text = "";
				dat_ycdt2.Text = "";
				sl_gnmu.EditValue = null;
				sl_gnmu2.EditValue = null;
				
				sl_embs.Enabled = true;
				dat_ycdt.Enabled = false;
				dat_ycdt2.Enabled = false;
				sl_gnmu.Enabled = false;
				sl_gnmu2.Enabled = false;
				cmb_gubn.Enabled = false;
				chk_line.Enabled = false;

				sl_line1.EditValue = null;
				sl_line2.EditValue = null;
				sl_line3.EditValue = null;
				sl_line1.Enabled = false;
				sl_line2.Enabled = false;
				sl_line3.Enabled = false;

				SetButtonEnable2("1000");
			}
			
            use_frdt = "";
			use_todt = "";
			seqno = "";

			df.GetSEARCH_DEPTDatas(ds);
			sl_dept.Properties.DataSource = ds.Tables["SEARCH_DEPT"];
			df.Get8030_SEARCH_EMBSDatas(p_dpcd, ds);
			sl_embs.Properties.DataSource = ds.Tables["8030_SEARCH_EMBS"];
			df.Get8030_SEARCH_GNMUDatas(ds);
			sl_gnmu.Properties.DataSource = ds.Tables["8030_SEARCH_GNMU"];
			sl_gnmu2.Properties.DataSource = ds.Tables["8030_SEARCH_GNMU"];
        }

		private void END_CHK()
		{
			//string yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 4) + "." + clib.DateToText(dat_yymm.DateTime).Substring(4, 2);
			//df.Get2020_SEARCH_ENDSDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), ds);
			//if (ds.Tables["2020_SEARCH_ENDS"].Rows.Count > 0) //마감월이 저장되어 있으면
			//{
			//	DataRow irow = ds.Tables["2020_SEARCH_ENDS"].Rows[0];
			//	ends_yn = irow["CLOSE_YN"].ToString();
			//	lb_ends.Text = irow["CLOSE_YN"].ToString() == "Y" ? "[" + yymm + " 최종마감 완료]" : irow["CLOSE_YN"].ToString() == "N" ? "[" + yymm + " 최종마감 취소]" : "[ ]";
			//}
			//else
			//{
			//	ends_yn = "";
			//	lb_ends.Text = "[" + yymm + " 최종마감 작업전]";
			//}
		}

        //연차신청내역 조회
        private void baseInfoSearch()
        {
			string dept = sl_dept.EditValue == null ? "%" : sl_dept.EditValue.ToString();
			df.GetSEARCH_YC_LISTDatas("A", clib.DateToText(dat_yymm.DateTime).Substring(0, 6), clib.DateToText(dat_yymm2.DateTime).Substring(0, 6), dept, ds);
			grd1.DataSource = ds.Tables["SEARCH_AP_YC_LIST"];
			df.GetSEARCH_YC_LISTDatas("C", clib.DateToText(dat_yymm.DateTime).Substring(0, 6), clib.DateToText(dat_yymm2.DateTime).Substring(0, 6), dept, ds);
			grd2.DataSource = ds.Tables["SEARCH_YC_LIST"];
			df.GetSEARCH_YC_LISTDatas("D", clib.DateToText(dat_yymm.DateTime).Substring(0, 6), clib.DateToText(dat_yymm2.DateTime).Substring(0, 6), dept, ds);
			grd_del.DataSource = ds.Tables["SEARCH_DEL_YC_LIST"];
			
			int start_index = 0;
			switch (clib.WeekDay(clib.TextToDateFirst(clib.DateToText(dat_yymm.DateTime))))
			{
				case "일":
				start_index = 0;
				break;
				case "월":
				start_index = 1;
				break;
				case "화":
				start_index = 2;
				break;
				case "수":
				start_index = 3;
				break;
				case "목":
				start_index = 4;
				break;
				case "금":
				start_index = 5;
				break;
				case "토":
				start_index = 6;
				break;
			}
			int row_count = start_index + clib.TextToInt(clib.DateToText(clib.TextToDateLast(clib.DateToText(dat_yymm.DateTime))).Substring(6, 2).ToString());
			row_count = row_count / 7 + 1;

			schedulerControl1.Views.MonthView.WeekCount = row_count;
			schedulerStorage1.Appointments.ResourceSharing = true;
			schedulerControl1.GroupType = SchedulerGroupType.Resource;
			schedulerControl1.Start = clib.TextToDateFirst(clib.DateToText(dat_yymm.DateTime));
			schedulerStorage1.Appointments.DataSource = ds.Tables["SEARCH_AP_YC_LIST"];

			schedulerStorage1.Appointments.Mappings.Type = "TYPE";         //타입
			schedulerStorage1.Appointments.Mappings.Start = "FR_DATE";     //시작날짜
			schedulerStorage1.Appointments.Mappings.End = "TO_DATE";       //끝날짜
			schedulerStorage1.Appointments.Mappings.AllDay = "ALLDAY";         //전일
			schedulerStorage1.Appointments.Mappings.Subject = "SAWON_NM";     //주제
			schedulerStorage1.Appointments.Mappings.Location = "REMARK";     //장소
			schedulerStorage1.Appointments.Mappings.Description = "REMARK";    //설명
			schedulerStorage1.Appointments.Mappings.Status = "STATUS";         //상태
			schedulerStorage1.Appointments.Mappings.Label = "LABEL";           //라벨
		}

        #endregion

        #region 1 Form

        private void duty8030_Load(object sender, EventArgs e)
        {
			dat_yymm.DateTime = DateTime.Today;
			dat_yymm2.DateTime = DateTime.Today;
			sl_dept.EditValue = null;
			schedulerControl1.Start = clib.TextToDateFirst(clib.DateToText(DateTime.Today));
			
			sl_embs.EditValue = null;
			sl_gnmu.EditValue = null;
			dat_ycdt.DateTime = DateTime.Today;
			lb_yc_remark.Text = "[연차사용기간 / 잔여연차]";

			#region 요일체크       
			int start_index = 0;
			switch (clib.WeekDay(clib.TextToDateFirst(clib.DateToText(dat_yymm.DateTime))))
			{
				case "일":
				start_index = 0;
				break;
				case "월":
				start_index = 1;
				break;
				case "화":
				start_index = 2;
				break;
				case "수":
				start_index = 3;
				break;
				case "목":
				start_index = 4;
				break;
				case "금":
				start_index = 5;
				break;
				case "토":
				start_index = 6;
				break;
			}
			int row_count = start_index + clib.TextToInt(clib.DateToText(clib.TextToDateLast(clib.DateToText(dat_yymm.DateTime))).Substring(6, 2).ToString());
			row_count = row_count / 7 + 1;

			schedulerControl1.Views.MonthView.WeekCount = row_count;
			#endregion
        }
		
		private void duty8030_Shown(object sender, EventArgs e)
		{
            df.GetMSTUSER_CHKDatas(ds);  //부서관리여부
            if (ds.Tables["MSTUSER_CHK"].Rows.Count > 0)
                admin_lv = clib.TextToInt(ds.Tables["MSTUSER_CHK"].Rows[0]["EMBSADGB"].ToString()); //권한레벨

			if (SilkRoad.Config.ACConfig.G_MSYN == "1" || SilkRoad.Config.SRConfig.USID == "SAMIL" || admin_lv > 1)
			{
				admin_lv = 3;
                p_dpcd = "%";
                lb_power.Text = "전체관리 권한";
				sl_dept.Enabled = true;
			}
            else if (admin_lv == 1)
            {
                p_dpcd = SilkRoad.Config.SRConfig.US_DPCD == null ? null : SilkRoad.Config.SRConfig.US_DPCD.Trim();
                lb_power.Text = "부서조회 권한";
				sl_dept.EditValue = p_dpcd;
				sl_dept.Enabled = false;
			}
            else
            {
                p_dpcd = SilkRoad.Config.SRConfig.US_DPCD == null ? null : SilkRoad.Config.SRConfig.US_DPCD.Trim();
                lb_power.Text = "조회권한 없음";				
				sl_dept.EditValue = p_dpcd;
				sl_dept.Enabled = false;
            }
			
            SetCancel(0);
            SetCancel(1);
		}

        #endregion

        #region 2 Button
		
		//조회
		private void btn_search_Click(object sender, EventArgs e)
		{
			if (isNoError_um(1))
			{
				txt_sabn.Text = "";
				txt_name.Text = "";
				txt_base.Text = "";
				txt_change.Text = "";
				txt_first.Text = "";
				txt_add.Text = "";
				txt_tcnt.Text = "";
				txt_use.Text = "";
				txt_rcnt.Text = "";
				
				if (ds.Tables["SEARCH_TRSHREQ"] != null)
					ds.Tables["SEARCH_TRSHREQ"].Clear();
				grd_yc.DataSource = null;

				baseInfoSearch();
			}
		}
		
		
		//연차신청 처리
		private void btn_proc_Click(object sender, EventArgs e)
		{			
			if (sl_embs.EditValue == null)
			{
				MessageBox.Show("사원이 선택되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				sl_embs.Focus();
			}
			else
			{
				sl_embs.Enabled = false;
				dat_ycdt.Enabled = true;
				dat_ycdt2.Enabled = true;
				cmb_gubn.Enabled = true;
				sl_gnmu.Enabled = true;
				sl_gnmu2.Enabled = true;
				chk_line.Enabled = true;

				sl_line1.EditValue = null;
				sl_line2.EditValue = null;
				sl_line3.EditValue = null;
				sl_line1.Enabled = true;
				sl_line2.Enabled = true;
				sl_line3.Enabled = true;
				if (ds.Tables["8030_SEARCH_EMBS"].Select("CODE = '" + sl_embs.EditValue.ToString() + "'").Length > 0)
				{
					string adgb = ds.Tables["8030_SEARCH_EMBS"].Select("CODE = '" + sl_embs.EditValue.ToString() + "'")[0]["EMBSADGB"].ToString();
					ADGB_STAT(adgb);
				}

				SetButtonEnable2("0101");
				dat_ycdt.Focus();
			}
		}
		private void ADGB_STAT(string adgb)
		{
			string line_txt = "";
			if (adgb == "1")
			{
				df.GetGW_LINE1Datas(sl_embs.EditValue.ToString(), "'2'", ds);
				sl_line1.Properties.DataSource = ds.Tables["GW_LINE1"];
				df.GetGW_LINE2Datas(sl_embs.EditValue.ToString(), "'5','6'", ds);
				sl_line2.Properties.DataSource = ds.Tables["GW_LINE2"];

				sl_line2.Visible = true;
				line_txt = "[팀장 -> 부서장 -> 대표/담당원장]";
			}
			else if (adgb == "2")
			{
				df.GetGW_LINE1Datas(sl_embs.EditValue.ToString(), "'5','6'", ds);
				sl_line1.Properties.DataSource = ds.Tables["GW_LINE1"];

				sl_line2.Visible = false;
				line_txt = "[부서장 -> 대표/담당원장]";
			}
			else if (adgb == "3")
			{
				df.GetGW_LINE1Datas(sl_embs.EditValue.ToString(), "'5'", ds);
				sl_line1.Properties.DataSource = ds.Tables["GW_LINE1"];

				sl_line2.Visible = false;
				line_txt = "[원장단 -> 담당원장]";
			}
			else if (adgb == "4")
			{
				df.GetGW_LINE1Datas(sl_embs.EditValue.ToString(), "'6'", ds);
				sl_line1.Properties.DataSource = ds.Tables["GW_LINE1"];

				sl_line2.Visible = false;
				line_txt = "[원장단 -> 대표원장]";
			}
			else if (adgb == "5")
			{
				df.GetGW_LINE1Datas(sl_embs.EditValue.ToString(), "'5'", ds);
				sl_line1.Properties.DataSource = ds.Tables["GW_LINE1"];
				sl_line1.EditValue = sl_embs.EditValue;

				sl_line2.Visible = false;
				line_txt = "[담당원장]";
			}
			else if (adgb == "6")
			{
				df.GetGW_LINE1Datas(sl_embs.EditValue.ToString(), "'6'", ds);
				sl_line1.Properties.DataSource = ds.Tables["GW_LINE1"];
				sl_line1.EditValue = sl_embs.EditValue;

				sl_line2.Visible = false;
				line_txt = "[대표원장]";
			}
			else
			{
				df.GetGW_LINE1Datas(sl_embs.EditValue.ToString(), "'1'", ds);
				sl_line1.Properties.DataSource = ds.Tables["GW_LINE1"];
				df.GetGW_LINE2Datas(sl_embs.EditValue.ToString(), "'2'", ds);
				sl_line2.Properties.DataSource = ds.Tables["GW_LINE2"];

				sl_line2.Visible = true;
				line_txt = "[팀원 -> 팀장]";
			}
			lb_line.Text = line_txt;
			chk_line.Visible = adgb == "1" ? true : false;
			chk_line.Checked = false;
		}
		
		//연차신청 취소
		private void btn_canc_Click(object sender, EventArgs e)
		{
			SetCancel(1);
		}

		//저장
		private void btn_save_Click(object sender, EventArgs e)
		{
			if (isNoError_um(5))
            {
                Cursor = Cursors.WaitCursor;
                int outVal = 0;
                try
                {
					string sldt = clib.DateToText(dat_ycdt.DateTime);
					string sldt2 = clib.DateToText(dat_ycdt2.DateTime);
					string yc_year = df.GetYC_YEAR_CHKDatas(sl_embs.EditValue.ToString(), sldt, ds);
					string to_gnmu = sl_gnmu2.EditValue == null ? sl_gnmu.EditValue.ToString() : sl_gnmu2.EditValue.ToString();
					//신청일자fr-to 에 대한 오류체크
					df.GetYC_DAYS_ECHKDatas(sl_embs.EditValue.ToString(), yc_year, sldt, sldt2, sl_gnmu.EditValue.ToString(), to_gnmu, ds);
					if (ds.Tables["YC_DAYS_ECHK"].Rows.Count > 0)
					{
						DataRow crow = ds.Tables["YC_DAYS_ECHK"].Rows[0];
						if (clib.TextToInt(crow["ECHK"].ToString()) < 0)
						{
							MessageBox.Show(crow["PCEROR"].ToString(), "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
							outVal = -1;
							return;
						}
					}

					if (seqno != "")  //수정
						df.GetDUTY_TRSHREQDatas(seqno, ds);
					else  //신규
						df.GetDUTY_TRSHREQDatas(ds);

					if (ds.Tables["DUTY_TRSHREQ"].Rows.Count > 0)
					{
						DataRow hrow = ds.Tables["DUTY_TRSHREQ"].Rows[0];
						hrow["GUBN"] = cmb_gubn.SelectedIndex == 1 ? "D" : "C";
						hrow["YC_DAYS"] = df.GetYC_DAYS_CALC_SABNDatas(sl_embs.EditValue.ToString(), sldt, sldt2, sl_gnmu.EditValue.ToString(), to_gnmu, ds);
						if (cmb_gubn.SelectedIndex == 1)
							hrow["YC_DAYS"] = -clib.TextToDecimal(hrow["YC_DAYS"].ToString());
						hrow["LINE_CNT"] = 1;
						if (sl_line3.EditValue != null)
							hrow["LINE_CNT"] = 4;
						else if (sl_line2.EditValue != null)
							hrow["LINE_CNT"] = 3;
						else if (sl_line1.EditValue != null)
							hrow["LINE_CNT"] = 2;
						
						hrow["AP_TAG"] = "4";
						hrow["GW_SABN1"] = sl_embs.EditValue.ToString();
						hrow["GW_DT1"] = gd.GetNow();
						hrow["GW_NAME1"] = ds.Tables["8030_SEARCH_EMBS"].Select("CODE ='" + sl_embs.EditValue.ToString() + "'")[0]["NAME"].ToString();
						hrow["GW_JICK1"] = ds.Tables["8030_SEARCH_EMBS"].Select("CODE ='" + sl_embs.EditValue.ToString() + "'")[0]["GRAD_NM"].ToString();
						for (int i = 2; i < 5; i++)
						{
							hrow["GW_SABN" + i.ToString()] = "";
							hrow["GW_DT" + i.ToString()] = "";
							hrow["GW_CHKID" + i.ToString()] = "";
							hrow["GW_NAME" + i.ToString()] = "";
							hrow["GW_JICK" + i.ToString()] = "";
						}
						if (sl_line1.EditValue != null)
						{
							hrow["GW_SABN2"] = sl_line1.EditValue.ToString();
							hrow["GW_NAME2"] = ds.Tables["GW_LINE1"].Select("CODE ='" + sl_line1.EditValue.ToString() + "'")[0]["NAME"].ToString();
							hrow["GW_JICK2"] = ds.Tables["GW_LINE1"].Select("CODE ='" + sl_line1.EditValue.ToString() + "'")[0]["GRAD_NM"].ToString();
						}
						if (sl_line2.EditValue != null)
						{
							hrow["GW_SABN3"] = sl_line2.EditValue.ToString();
							hrow["GW_NAME3"] = ds.Tables["GW_LINE2"].Select("CODE ='" + sl_line2.EditValue.ToString() + "'")[0]["NAME"].ToString();
							hrow["GW_JICK3"] = ds.Tables["GW_LINE2"].Select("CODE ='" + sl_line2.EditValue.ToString() + "'")[0]["GRAD_NM"].ToString();
						}		
						hrow["UPDT"] = gd.GetNow();
						hrow["USID"] = SilkRoad.Config.SRConfig.USID;
						hrow["PSTY"] = "U";
					}
					else
					{
						DataRow hrow = ds.Tables["DUTY_TRSHREQ"].NewRow();
						hrow["SEQNO"] = df.GetHREQ_SEQNODatas();
						hrow["GUBN"] = cmb_gubn.SelectedIndex == 1 ? "D" : "C";
						hrow["SABN"] = sl_embs.EditValue.ToString();
						hrow["REQ_YEAR"] = yc_year;
						hrow["REQ_DATE"] = sldt;
						hrow["REQ_DATE2"] = sldt2;
						hrow["REQ_TYPE"] = sl_gnmu.EditValue.ToString();
						hrow["REQ_TYPE2"] = to_gnmu;

						hrow["YC_DAYS"] = df.GetYC_DAYS_CALC_SABNDatas(sl_embs.EditValue.ToString(), sldt, sldt2, sl_gnmu.EditValue.ToString(), to_gnmu, ds);
						if (cmb_gubn.SelectedIndex == 1)
							hrow["YC_DAYS"] = -clib.TextToDecimal(hrow["YC_DAYS"].ToString());
						hrow["AP_TAG"] = "4";
						hrow["LINE_CNT"] = 1;
						if (sl_line3.EditValue != null)
							hrow["LINE_CNT"] = 4;
						else if (sl_line2.EditValue != null)
							hrow["LINE_CNT"] = 3;
						else if (sl_line1.EditValue != null)
							hrow["LINE_CNT"] = 2;

						hrow["GW_SABN1"] = sl_embs.EditValue.ToString();
						hrow["GW_DT1"] = gd.GetNow();
						hrow["GW_NAME1"] = ds.Tables["8030_SEARCH_EMBS"].Select("CODE ='" + sl_embs.EditValue.ToString() + "'")[0]["NAME"].ToString();
						hrow["GW_JICK1"] = ds.Tables["8030_SEARCH_EMBS"].Select("CODE ='" + sl_embs.EditValue.ToString() + "'")[0]["GRAD_NM"].ToString();
						for (int i = 2; i < 5; i++)
						{
							hrow["GW_SABN" + i.ToString()] = "";
							hrow["GW_DT" + i.ToString()] = "";
							hrow["GW_CHKID" + i.ToString()] = "";
							hrow["GW_NAME" + i.ToString()] = "";
							hrow["GW_JICK" + i.ToString()] = "";
						}
						if (sl_line1.EditValue != null)
						{
							hrow["GW_SABN2"] = sl_line1.EditValue.ToString();
							hrow["GW_NAME2"] = ds.Tables["GW_LINE1"].Select("CODE ='" + sl_line1.EditValue.ToString() + "'")[0]["NAME"].ToString();
							hrow["GW_JICK2"] = ds.Tables["GW_LINE1"].Select("CODE ='" + sl_line1.EditValue.ToString() + "'")[0]["GRAD_NM"].ToString();
						}
						if (sl_line2.EditValue != null)
						{
							hrow["GW_SABN3"] = sl_line2.EditValue.ToString();
							hrow["GW_NAME3"] = ds.Tables["GW_LINE2"].Select("CODE ='" + sl_line2.EditValue.ToString() + "'")[0]["NAME"].ToString();
							hrow["GW_JICK3"] = ds.Tables["GW_LINE2"].Select("CODE ='" + sl_line2.EditValue.ToString() + "'")[0]["GRAD_NM"].ToString();
						}

						hrow["INDT"] = gd.GetNow();
						hrow["UPDT"] = "";
						hrow["USID"] = SilkRoad.Config.SRConfig.USID;
						hrow["PSTY"] = "A";
						ds.Tables["DUTY_TRSHREQ"].Rows.Add(hrow);
					}
                    
                    string[] tableNames = new string[] { "DUTY_TRSHREQ" };
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
                        MessageBox.Show("저장되었습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (outVal > -1)
						SetCancel(1);
					baseInfoSearch();
                    Cursor = Cursors.Default;
                }
            }
		}
		//삭제
		private void btn_del_Click(object sender, EventArgs e)
		{
			if (isNoError_um(6))
			{
                Cursor = Cursors.WaitCursor;
				int outVal = 0;
				try
				{
					string sldt = clib.DateToText(dat_ycdt.DateTime);
					if (seqno != "")
						df.GetDUTY_TRSHREQDatas(seqno, ds);
					else
						df.GetDUTY_TRSHREQDatas(sl_embs.EditValue.ToString(), sldt, ds);

					if (ds.Tables["DUTY_TRSHREQ"].Rows.Count > 0)
					{
						DataRow drow = ds.Tables["DUTY_TRSHREQ"].Rows[0];
						if (drow["AP_TAG"].ToString() == "1")
						{
							MessageBox.Show("승인된 내역은 삭제할 수 없습니다.", "삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						else
						{
							df.GetDEL_TRSHREQDatas(ds);								
							DataRow hrow = ds.Tables["DEL_TRSHREQ"].NewRow();
							hrow["SEQNO"] = drow["SEQNO"].ToString();
							hrow["GUBN"] = drow["GUBN"].ToString();
							hrow["SABN"] = drow["SABN"].ToString();
							hrow["REQ_YEAR"] = drow["REQ_YEAR"].ToString();
							hrow["REQ_DATE"] = drow["REQ_DATE"].ToString();
							hrow["REQ_DATE2"] = drow["REQ_DATE2"].ToString();
							hrow["REQ_TYPE"] = drow["REQ_TYPE"].ToString();
							hrow["REQ_TYPE2"] = drow["REQ_TYPE2"].ToString();

							hrow["YC_DAYS"] = clib.TextToDecimal(drow["YC_DAYS"].ToString());
							hrow["AP_TAG"] = drow["AP_TAG"].ToString();
							hrow["LINE_CNT"] = clib.TextToInt(drow["LINE_CNT"].ToString());
							for (int i = 1; i < 5; i++)
							{
								hrow["GW_SABN" + i.ToString()] = drow["GW_SABN" + i.ToString()].ToString();
								hrow["GW_DT" + i.ToString()] = drow["GW_DT" + i.ToString()].ToString();
								if (i != 1)
									hrow["GW_CHKID" + i.ToString()] = drow["GW_CHKID" + i.ToString()].ToString();
								hrow["GW_NAME" + i.ToString()] = drow["GW_NAME" + i.ToString()].ToString();
								hrow["GW_JICK" + i.ToString()] = drow["GW_JICK" + i.ToString()].ToString();
							}

							hrow["INDT"] = drow["INDT"].ToString();
							hrow["UPDT"] = drow["UPDT"].ToString();
							hrow["USID"] = drow["USID"].ToString();
							hrow["PSTY"] = drow["PSTY"].ToString();
							hrow["DEL_DT"] = gd.GetNow();
							hrow["DEL_ID"] = SilkRoad.Config.SRConfig.USID;
							ds.Tables["DEL_TRSHREQ"].Rows.Add(hrow);

							ds.Tables["DUTY_TRSHREQ"].Rows[0].Delete();
							string[] tableNames = new string[] { "DUTY_TRSHREQ", "DEL_TRSHREQ" };
							SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
							outVal = cmd.setUpdate(ref ds, tableNames, null);
						}
					}
					else
					{
						MessageBox.Show("등록된 연차내역이 없어 삭제할 수 없습니다.", "삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				catch (Exception ec)
				{
					MessageBox.Show(ec.Message, "삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					if (outVal > 0)
						MessageBox.Show("삭제되었습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
					SetCancel(1);
					baseInfoSearch();
					Cursor = Cursors.Default;
				}
			}
		}

        /// <summary>취소버튼</summary>
        private void btn_clear_Click(object sender, EventArgs e)
        {
            SetCancel(0);
        }		
		
        #endregion

        #region 3 EVENT
		
		//메뉴 활성화시
		private void duty8030_Activated(object sender, EventArgs e)
		{

		}

        private void duty8030_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)   //취소
            {
                btn_clear.PerformClick();
            }
        }
		//그리드 더블클릭시
		private void grdv1_DoubleClick(object sender, EventArgs e)
		{
			DataRow drow = grdv1.GetFocusedDataRow();
            if (drow == null)
                return;
			
			sl_embs.EditValue = grdv1.GetFocusedRowCellValue("SABN").ToString().Trim();
			btn_proc.PerformClick();
			
			seqno = grdv1.GetFocusedRowCellValue("SEQNO").ToString();
			DbClick(drow["SABN"].ToString(), drow["REQ_DATE"].ToString());
		}
		private void grdv2_DoubleClick(object sender, EventArgs e)
		{
			DataRow drow = grdv2.GetFocusedDataRow();
            if (drow == null)
                return;
			
			sl_embs.EditValue = grdv2.GetFocusedRowCellValue("SABN").ToString().Trim();
			btn_proc.PerformClick();
			
			seqno = grdv2.GetFocusedRowCellValue("SEQNO").ToString();
			DbClick(drow["SABN"].ToString(), drow["REQ_DATE"].ToString());
		}

		private void DbClick(string sabn, string req_dt)
		{
			if (seqno != "")			
				df.GetDUTY_TRSHREQDatas(seqno, ds);			
			else
				df.GetDUTY_TRSHREQDatas(sabn, req_dt, ds);
			
			if (ds.Tables["DUTY_TRSHREQ"].Rows.Count > 0)
			{
				DataRow hrow = ds.Tables["DUTY_TRSHREQ"].Rows[0];
				dat_ycdt.DateTime = clib.TextToDate(hrow["REQ_DATE"].ToString());
				dat_ycdt2.DateTime = clib.TextToDate(hrow["REQ_DATE2"].ToString());
				sl_gnmu.EditValue = hrow["REQ_TYPE"].ToString();
				sl_gnmu2.EditValue = hrow["REQ_TYPE2"].ToString();
				cmb_gubn.SelectedIndex = hrow["GUBN"].ToString() == "D" ? 1 : 0;

				if (SilkRoad.Config.ACConfig.G_MSYN == "1")
				{
					SetButtonEnable2("0011");
				}
				else
				{
					if (hrow["AP_TAG"].ToString() == "1" || hrow["AP_TAG"].ToString() == "3" || hrow["AP_TAG"].ToString() == "9")  //1.승인,3.완료,9.정산
						SetButtonEnable2("0001");
					else if (hrow["AP_TAG"].ToString() == "4" && hrow["GW_DT2"].ToString().Trim() == "") //4.진행이고 다음결재가 없을때
						SetButtonEnable2("0011");
					else if (hrow["AP_TAG"].ToString() == "5") //5.반려
						SetButtonEnable2("0111");
					else
						SetButtonEnable2("0011");
				}
			}
			dat_ycdt.Enabled = false;
			dat_ycdt2.Enabled = false;
			cmb_gubn.Enabled = false;
			sl_gnmu.Enabled = false;
			sl_gnmu2.Enabled = false;
			sl_line1.EditValue = null;
			sl_line2.EditValue = null;

			if (ds.Tables["8030_SEARCH_EMBS"].Select("CODE = '" + sl_embs.EditValue.ToString() + "'").Length > 0)
			{
				string adgb = ds.Tables["8030_SEARCH_EMBS"].Select("CODE = '" + sl_embs.EditValue.ToString() + "'")[0]["EMBSADGB"].ToString();
				ADGB_STAT(adgb);
			}
		}

		//그리드 클릭시 연차내역 조회
		private void grdv1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
		{
			DataRow drow2 = grdv1.GetFocusedDataRow();
            if (drow2 == null)
                return;
			
			dat_year.DateTime = clib.TextToDate(grdv1.GetFocusedRowCellValue("REQ_YEAR").ToString() + "0101");
			txt_sabn.Text = grdv1.GetFocusedRowCellValue("SABN").ToString();
			df.GetDUTY_TRSDYYCDatas(grdv1.GetFocusedRowCellValue("REQ_YEAR").ToString(), grdv1.GetFocusedRowCellValue("SABN").ToString(), ds);

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
				txt_use.Text = "";
				txt_rcnt.Text = drow["YC_TOTAL"].ToString();

				df.GetSEARCH_TRSHREQDatas(drow["SAWON_NO"].ToString(), drow["YC_YEAR"].ToString(), ds);
				grd_yc.DataSource = ds.Tables["SEARCH_TRSHREQ"];

				if (ds.Tables["SEARCH_TRSHREQ"].Rows.Count > 0)
					txt_use.Text = ds.Tables["SEARCH_TRSHREQ"].Compute("SUM(YC_DAYS)", "AP_TAG<>'5'").ToString();
				
				txt_rcnt.Text = (clib.TextToDecimal(txt_tcnt.Text.ToString()) + clib.TextToDecimal(txt_change.Text.ToString()) - clib.TextToDecimal(txt_use.Text.ToString())).ToString();
				//df.GetSEARCH_DUTY_MSTYCCJDatas(drow["SAWON_NO"].ToString(), drow["YC_YEAR"].ToString(), "", "", ds);
				//grd.DataSource = ds.Tables["SEARCH_DUTY_MSTYCCJ"];

				//SetButtonEnable("111");
			}
		}
		private void grdv2_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
		{
			DataRow drow2 = grdv2.GetFocusedDataRow();
			if (drow2 == null)
				return;

			dat_year.DateTime = clib.TextToDate(grdv2.GetFocusedRowCellValue("REQ_YEAR").ToString() + "0101");
			txt_sabn.Text = grdv2.GetFocusedRowCellValue("SABN").ToString();
			df.GetDUTY_TRSDYYCDatas(grdv2.GetFocusedRowCellValue("REQ_YEAR").ToString(), grdv2.GetFocusedRowCellValue("SABN").ToString(), ds);

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
				txt_use.Text = "";
				txt_rcnt.Text = drow["YC_TOTAL"].ToString();

				df.GetSEARCH_TRSHREQDatas(drow["SAWON_NO"].ToString(), drow["YC_YEAR"].ToString(), ds);
				grd_yc.DataSource = ds.Tables["SEARCH_TRSHREQ"];

				if (ds.Tables["SEARCH_TRSHREQ"].Rows.Count > 0)
					txt_use.Text = ds.Tables["SEARCH_TRSHREQ"].Compute("SUM(YC_DAYS)", "AP_TAG<>'5'").ToString();

				txt_rcnt.Text = (clib.TextToDecimal(txt_tcnt.Text.ToString()) + clib.TextToDecimal(txt_change.Text.ToString()) - clib.TextToDecimal(txt_use.Text.ToString())).ToString();
			}
		}
		private void grdv_del_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
		{
			DataRow drow2 = grdv_del.GetFocusedDataRow();
            if (drow2 == null)
                return;

			dat_year.DateTime = clib.TextToDate(grdv_del.GetFocusedRowCellValue("REQ_YEAR").ToString()+"0101");
			txt_sabn.Text = grdv_del.GetFocusedRowCellValue("SABN").ToString();

			df.GetDUTY_TRSDYYCDatas(grdv_del.GetFocusedRowCellValue("REQ_YEAR").ToString(), grdv_del.GetFocusedRowCellValue("SABN").ToString(), ds);
			if (ds.Tables["DUTY_TRSDYYC"].Rows.Count > 0)
			{
				DataRow drow = ds.Tables["DUTY_TRSDYYC"].Rows[0];
				txt_name.Text = drow["SAWON_NM"].ToString();
				cmb_type.SelectedIndex = clib.TextToInt(drow["YC_TYPE"].ToString());
				dat_indt.DateTime = clib.TextToDate(drow["IN_DATE"].ToString());
				txt_base.Text = drow["YC_BASE"].ToString();
				txt_change.Text = drow["YC_CHANGE"].ToString();
				txt_first.Text = drow["YC_FIRST"].ToString();
				txt_add.Text = drow["YC_ADD"].ToString();
				txt_tcnt.Text = drow["YC_TOTAL"].ToString();
				txt_use.Text = "";
				txt_rcnt.Text = drow["YC_TOTAL"].ToString();

				df.GetSEARCH_TRSHREQDatas(drow["SAWON_NO"].ToString(), drow["YC_YEAR"].ToString(), ds);
				grd_yc.DataSource = ds.Tables["SEARCH_TRSHREQ"];

				if (ds.Tables["SEARCH_TRSHREQ"].Rows.Count > 0)
					txt_use.Text = ds.Tables["SEARCH_TRSHREQ"].Compute("SUM(YC_DAYS)", "AP_TAG<>'5'").ToString();
				
				txt_rcnt.Text = (clib.TextToDecimal(txt_tcnt.Text.ToString()) + clib.TextToDecimal(txt_change.Text.ToString()) - clib.TextToDecimal(txt_use.Text.ToString())).ToString();
			}
		}
				
		//연차사용일자 선택시
		private void dat_ycdt_EditValueChanged(object sender, EventArgs e)
		{
			if (clib.isDate(clib.DateToText(dat_ycdt.DateTime)) && sl_embs.EditValue != null)
				YC_REMAIN();
			else
				lb_yc_remark.Text = "[연차사용기간 / 잔여연차]";
			YC_GNMU_CHK();

			if (clib.DateToText(dat_ycdt2.DateTime) == "")
				dat_ycdt2.DateTime = dat_ycdt.DateTime;
		}
		private void dat_ycdt2_EditValueChanged(object sender, EventArgs e)
		{
			YC_GNMU_CHK();
		}

		private void YC_GNMU_CHK()
		{
			if (clib.DateToText(dat_ycdt.DateTime) != "" && clib.DateToText(dat_ycdt2.DateTime) != "")
			{
				if (clib.DateToText(dat_ycdt.DateTime) == clib.DateToText(dat_ycdt2.DateTime))
				{
					sl_gnmu2.Enabled = false;
					sl_gnmu2.EditValue = null;
				}
				else
				{
					sl_gnmu2.Enabled = true;
				}
			}
			else
			{
				sl_gnmu2.Enabled = true;
			}
		}
		private void YC_REMAIN()
		{
			string yc_year = df.GetYC_YEAR_CHKDatas(sl_embs.EditValue.ToString(), clib.DateToText(dat_ycdt.DateTime), ds);
			df.GetSEARCH_YC_YEARDatas(sl_embs.EditValue.ToString(), yc_year, ds);
			if (ds.Tables["SEARCH_YC_YEAR"].Rows.Count == 0)
			{
				//[연차사용기간 : 2021.01.01 ~ 2021.12.31 / 잔여연차 : 15 일]
				//USP_DUTY8010_BASE '%', '199802015', '20211202', 'SA'
				df.GetDUTY_YC_BASEDatas(sl_embs.EditValue.ToString(), clib.DateToText(dat_ycdt.DateTime), ds);
				df.GetSEARCH_YC_YEARDatas(sl_embs.EditValue.ToString(), yc_year, ds);				
			}

			if (ds.Tables["SEARCH_YC_YEAR"].Rows.Count > 0)
			{
				DataRow drow = ds.Tables["SEARCH_YC_YEAR"].Rows[0];
				use_frdt = drow["USE_FRDT"].ToString();
				use_todt = drow["USE_TODT"].ToString();
				string frto_dt = drow["USE_FRDT"].ToString().Substring(0, 4) + "." + drow["USE_FRDT"].ToString().Substring(4, 2) + "." + drow["USE_FRDT"].ToString().Substring(6, 2) + " ~ " + drow["USE_TODT"].ToString().Substring(0, 4) + "." + drow["USE_TODT"].ToString().Substring(4, 2) + "." + drow["USE_TODT"].ToString().Substring(6, 2);
				df.GetSUM_YC_USEDatas(sl_embs.EditValue.ToString(), yc_year, ds);
				decimal yc_remain = clib.TextToDecimal(drow["YC_TOTAL"].ToString()) - clib.TextToDecimal(ds.Tables["SUM_YC_USE"].Rows[0]["YC_DAY"].ToString());
				lb_yc_remark.Text = "[연차사용기간 : " + frto_dt + " / 잔여연차 : " + yc_remain.ToString() + " 일]";

				string yymm = clib.DateToText(dat_ycdt.DateTime).Substring(0, 4) + "." + clib.DateToText(dat_ycdt.DateTime).Substring(4, 2);
				df.Get2020_SEARCH_ENDSDatas(clib.DateToText(dat_ycdt.DateTime).Substring(0, 6), ds);
				if (ds.Tables["2020_SEARCH_ENDS"].Rows.Count > 0) //마감월이 저장되어 있으면
				{
					DataRow irow = ds.Tables["2020_SEARCH_ENDS"].Rows[0];
					ends_yn2 = irow["CLOSE_YN"].ToString();
					lb_ends.Text = irow["CLOSE_YN"].ToString() == "Y" ? "[" + yymm + " 최종마감 완료]" : irow["CLOSE_YN"].ToString() == "N" ? "[" + yymm + " 최종마감 취소]" : "[ ]";
				}
				else
				{
					ends_yn2 = "";
					lb_ends.Text = "[" + yymm + " 최종마감 작업전]";
				}
			}
		}
		
		//중간관리자 제외
		private void chk_line_CheckedChanged(object sender, EventArgs e)
		{
			if (chk_line.Checked == false)
			{
				sl_line1.EditValue = null;
				sl_line2.EditValue = null;
				df.GetGW_LINE1Datas(sl_embs.EditValue.ToString(), "'2'", ds);
				sl_line1.Properties.DataSource = ds.Tables["GW_LINE1"];
				df.GetGW_LINE2Datas(sl_embs.EditValue.ToString(), "'5','6'", ds);
				sl_line2.Properties.DataSource = ds.Tables["GW_LINE2"];

				sl_line2.Visible = true;
				lb_line.Text = "[팀장 -> 부서장 -> 대표/담당원장]";
			}
			else
			{
				sl_line1.EditValue = null;
				df.GetGW_LINE1Datas(sl_embs.EditValue.ToString(), "'5','6'", ds);
				sl_line1.Properties.DataSource = ds.Tables["GW_LINE1"];

				sl_line2.Visible = false;
				lb_line.Text = "[팀장 -> 대표/담당원장]";
			}
		}
				
		private void grdv1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
		{			
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)            
                e.Info.DisplayText = (e.RowHandle + 1).ToString();     
		}	
		private void grdv2_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
		{			
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)            
                e.Info.DisplayText = (e.RowHandle + 1).ToString();     
		}
		private void grdv_del_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
		{			
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)            
                e.Info.DisplayText = (e.RowHandle + 1).ToString();     
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
                if (admin_lv == 0)
                {
                    MessageBox.Show("조회권한이 없습니다. 인사기본관리의 관리자구분을 확인하세요!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (clib.DateToText(dat_yymm.DateTime) == "")
                {
                    MessageBox.Show("조회년월(시작)을 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dat_yymm.Focus();
                    return false;
                }
                else if (clib.DateToText(dat_yymm2.DateTime) == "")
                {
                    MessageBox.Show("조회년월(종료)를 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dat_yymm2.Focus();
                    return false;
                }
                else
                {
                    isError = true;
                }
            }
			else if (mode == 2)  //승인
			{
				//if (ends_yn == "Y")
				//{
				//	MessageBox.Show("최종마감되어 승인 할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				//	return false;
				//}

				if (ds.Tables["SEARCH_AP_YC_LIST"] != null)
				{
					ds.Tables["SEARCH_AP_YC_LIST"].AcceptChanges();
					if (ds.Tables["SEARCH_AP_YC_LIST"].Select("CHK='1'").Length == 0)
					{
						MessageBox.Show("선택된 내역이 없습니다!", "승인에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
					else
					{
						isError = true;
					}
				}
			}
			//        else if (mode == 3)  //승인취소
			//        {
			//if (ends_yn == "Y")
			//{
			//	MessageBox.Show("최종마감되어 승인취소 할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//	return false;
			//}
			//if (ds.Tables["SEARCH_AP_YC_LIST"] != null)
			//{
			//	if (ds.Tables["SEARCH_AP_YC_LIST"].Select("C_CHK='1'").Length == 0)
			//	{
			//		MessageBox.Show("선택된 내역이 없습니다!", "취소에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//		return false;
			//	}
			//	else
			//	{
			//		isError = true;
			//	}
			//}
			//        }
			else if (mode == 4)  //처리
            {
				if (sl_embs.EditValue == null)
				{
					MessageBox.Show("사원이 선택되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_embs.Focus();
					return false;
				}
				else
				{
					isError = true;
				}
            }
            else if (mode == 5)  //등록
            {
				//if (ends_yn2 == "Y")
				//{
				//	MessageBox.Show("최종마감되어 등록할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				//	return false;
				//}
				string adgb = ds.Tables["8030_SEARCH_EMBS"].Select("CODE = '" + sl_embs.EditValue.ToString() + "'")[0]["EMBSADGB"].ToString();
				if (clib.DateToText(dat_ycdt.DateTime) == "")
				{
					MessageBox.Show("신청일자(시작)가 입력되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_ycdt.Focus();
					return false;
				}
				else if (clib.DateToText(dat_ycdt2.DateTime) == "")
				{
					MessageBox.Show("신청일자(종료)가 입력되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_ycdt2.Focus();
					return false;
				}
				else if (dat_ycdt2.DateTime > clib.TextToDate(use_todt))
				{
					MessageBox.Show("신청일자(종료)가 현재 연차 사용기간 이후의 일자입니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_ycdt2.Focus();
					return false;
				}
				else if (sl_gnmu.EditValue == null)
				{
					MessageBox.Show("연차유형(시작)이 선택되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_gnmu.Focus();
					return false;
				}
				else if (clib.DateToText(dat_ycdt.DateTime) != clib.DateToText(dat_ycdt2.DateTime) && sl_gnmu2.EditValue == null)
				{
					MessageBox.Show("연차유형(종료)이 선택되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_gnmu2.Focus();
					return false;
				}
				else if (clib.DateToText(dat_ycdt.DateTime) != clib.DateToText(dat_ycdt2.DateTime) && sl_gnmu.EditValue.ToString() == "13")
				{
					MessageBox.Show("연차유형(시작)이 오전반차로 선택할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_gnmu.Focus();
					return false;
				}
				else if (clib.DateToText(dat_ycdt.DateTime) != clib.DateToText(dat_ycdt2.DateTime) && sl_gnmu2.EditValue.ToString() == "15")
				{
					MessageBox.Show("연차유형(종료)이 오후반차로 선택할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_gnmu2.Focus();
					return false;
				}
				else if (sl_line1.Visible == true && sl_line1.EditValue == null)
				{
					MessageBox.Show("결재자1이 선택되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_line1.Focus();
					return false;
				}
				else if (adgb == "1" && sl_line2.Visible == true && sl_line2.EditValue == null)
				{
					MessageBox.Show("결재자2가 선택되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_line2.Focus();
					return false;
				}
				else if (sl_line3.Visible == true && sl_line3.EditValue == null)
				{
					MessageBox.Show("결재자3이 선택되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_line3.Focus();
					return false;
				}
				else
				{
					isError = true;
				}
            }
            else if (mode == 6)  //삭제
            {
				//if (ends_yn2 == "Y")
				//{
				//	MessageBox.Show("최종마감되어 삭제할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				//	return false;
				//}
				if (clib.DateToText(dat_ycdt.DateTime) == "")
				{
					MessageBox.Show("신청일자가 입력되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_ycdt.Focus();
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
			btn_search.Enabled = arr.Substring(0, 1) == "1" ? true : false;
			btn_clear.Enabled = arr.Substring(1, 1) == "1" ? true : false;
		}
		
        private void SetButtonEnable2(string arr)
        {
			btn_proc.Enabled = arr.Substring(0, 1) == "1" ? true : false;
			btn_save.Enabled = arr.Substring(1, 1) == "1" ? true : false;
			btn_del.Enabled = arr.Substring(2, 1) == "1" ? true : false;
			btn_canc.Enabled = arr.Substring(3, 1) == "1" ? true : false;
		}

		#endregion

	}
}
