using System;
using System.Data;
using System.Windows.Forms;
using SilkRoad.Common;
using System.Text;
using DevExpress.XtraReports.UI;
using System.Drawing;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.XtraScheduler;

namespace DUTY1000
{
    public partial class duty2060 : SilkRoad.Form.Base.FormX
    {
        CommonLibrary clib = new CommonLibrary();

        ClearNEnableControls cec = new ClearNEnableControls();
        public DataSet ds = new DataSet();
        static DataProcessing dp = new DataProcessing();
        DataProcFunc df = new DataProcFunc();
        SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();
        private string ends_yn = "";
		
        private int admin_lv = 0;
        //private string msyn = "";
        //private string upyn = "";
        private string p_dpcd = "";
		
        private string grid_dept = "";
        public duty2060()
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
			if (ds.Tables["SEARCH_DANG_PLAN"] != null)
				ds.Tables["SEARCH_DANG_PLAN"].Clear();
			if (ds.Tables["3010_KT_DT1"] != null)
				ds.Tables["3010_KT_DT1"].Clear();
			if (ds.Tables["3010_KT_DT2"] != null)
				ds.Tables["3010_KT_DT2"].Clear();
			if (ds.Tables["SEARCH_DANG"] != null)
				ds.Tables["SEARCH_DANG"].Clear();
			if (ds.Tables["SUM_DANG_PLAN"] != null)
				ds.Tables["SUM_DANG_PLAN"].Clear();
			
			if (ds.Tables["SEARCH_DYYC"] != null)
				ds.Tables["SEARCH_DYYC"].Clear();
			if (ds.Tables["SEARCH_TRSHREQ"] != null)
				ds.Tables["SEARCH_TRSHREQ"].Clear();
			
			srTabControl2.Visible = false;
			btn_expand.Image = DUTY1000.Properties.Resources.확대;
			//grd2.DataSource = ds.Tables["SUM_DANG_PLAN"];
            btn_save.Text = "저  장";
            btn_save.Image = DUTY1000.Properties.Resources.저장;
			grd1.Enabled = false;
			grd2.Enabled = false;

			bandedGridColumn2.Visible = false;
			//schedulerStorage1.Appointments.DataSource = null;

			dat_yymm.Enabled = true;
			sl_dept.Enabled = true;
			//sl_dept.Enabled = p_dpcd == "%" ? true : false;
            sl_embs.Enabled = false;
			btn_lineadd.Enabled = false;
			btn_linedel.Enabled = false;
			btn_yc_adp.Enabled = false;
			btn_bf_plan.Enabled = false;
			chk_g_adt.Checked = false;
			cmb_day.SelectedIndex = 0;
            gridBand23.Caption = "";
            gridBand27.Caption = "일자별 근무신청 현황";

            Refresh_Click();
						
            for (int i = 1; i <= 31; i++)
            {
				grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Black;
				grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.Font = new Font(FontFamily.GenericSansSerif, 9F, FontStyle.Regular);
				grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].ToolTip = "";
				grdv2.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Black;
				grdv2.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.Font = new Font(FontFamily.GenericSansSerif, 9F, FontStyle.Regular);
				grdv2.Columns["D" + i.ToString().PadLeft(2, '0')].ToolTip = "";
            }
			SetButtonEnable("100000");
        }
		
        //당직숙직내역 조회
        private void baseInfoSearch()
        {
            string dept = sl_dept.EditValue == null ? "%" : sl_dept.EditValue.ToString();
			df.GetSEARCH_DANGDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), dept, ds);
			grd1.DataSource = ds.Tables["SEARCH_DANG"];
			
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
			schedulerStorage1.Appointments.DataSource = ds.Tables["SEARCH_DANG"];

			schedulerStorage1.Appointments.Mappings.Type = "TYPE";         //타입
			schedulerStorage1.Appointments.Mappings.Start = "FR_DATE";     //시작날짜
			schedulerStorage1.Appointments.Mappings.End = "TO_DATE";       //끝날짜
			schedulerStorage1.Appointments.Mappings.AllDay = "ALLDAY";         //전일
			schedulerStorage1.Appointments.Mappings.Subject = "G_FNM";     //주제
			schedulerStorage1.Appointments.Mappings.Location = "SAWON_NM";     //장소
			schedulerStorage1.Appointments.Mappings.Description = "REMARK";    //설명
			schedulerStorage1.Appointments.Mappings.Status = "STATUS";         //상태
			schedulerStorage1.Appointments.Mappings.Label = "LABEL";           //라벨
		}

        #endregion

        #region 1 Form

        private void duty2060_Load(object sender, EventArgs e)
        {
			dat_yymm.DateTime = DateTime.Now;
			sl_dept.EditValue = null;
			sl_gnmu.EditValue = null;			
		}

        private void duty2060_Shown(object sender, EventArgs e)
        {
            df.GetMSTUSER_CHKDatas(ds);  //부서관리여부
            if (ds.Tables["MSTUSER_CHK"].Rows.Count > 0)
                admin_lv = clib.TextToInt(ds.Tables["MSTUSER_CHK"].Rows[0]["EMBSADGB"].ToString()); //권한레벨

            if (SilkRoad.Config.ACConfig.G_MSYN == "1" || SilkRoad.Config.SRConfig.USID == "SAMIL") //부서설정은 관리자만 (23.08.22 조수진 요청)
                btn_info.Enabled = true;

            if (SilkRoad.Config.ACConfig.G_MSYN == "1" || SilkRoad.Config.SRConfig.USID == "SAMIL" || admin_lv > 2)
			{
				admin_lv = 3;
                p_dpcd = "%";
                lb_power.Text = "전체관리 권한";
			}
            else if (admin_lv == 1)
            {
                p_dpcd = SilkRoad.Config.SRConfig.US_DPCD == null ? null : SilkRoad.Config.SRConfig.US_DPCD.Trim();
                lb_power.Text = "부서관리 권한";
			}
            else if (admin_lv == 2)
            {
                p_dpcd = "%";
                lb_power.Text = "부서장관리 권한";
				df.GetCHK_DEPTDatas(SilkRoad.Config.SRConfig.USID, "%", ds);
				string lb_nm = "";
				for (int i = 0; i < ds.Tables["CHK_DEPT"].Rows.Count; i++)
				{
					if (i == 0)
						lb_nm = "(" + ds.Tables["CHK_DEPT"].Rows[i]["DEPT_NM"].ToString();
					else if (i == ds.Tables["CHK_DEPT"].Rows.Count - 1)
						lb_nm += "," + ds.Tables["CHK_DEPT"].Rows[i]["DEPT_NM"].ToString() + ")";
					else
						lb_nm += "," + ds.Tables["CHK_DEPT"].Rows[i]["DEPT_NM"].ToString();
				}

				lb_power.Text += lb_nm;
			}
            else
            {
                p_dpcd = SilkRoad.Config.SRConfig.US_DPCD == null ? null : SilkRoad.Config.SRConfig.US_DPCD.Trim();
                lb_power.Text = "관리권한 없음";
            }

			SetCancel();
        }
		
		private void END_CHK()
		{
			string yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 4) + "." + clib.DateToText(dat_yymm.DateTime).Substring(4, 2);
			df.Get2020_SEARCH_ENDSDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), ds);
			if (ds.Tables["2020_SEARCH_ENDS"].Rows.Count > 0) //마감월이 저장되어 있으면
			{
				DataRow irow = ds.Tables["2020_SEARCH_ENDS"].Rows[0];
				ends_yn = irow["CLOSE_YN"].ToString();
				lb_ends.Text = irow["CLOSE_YN"].ToString() == "Y" ? "[" + yymm + " 최종마감 완료]" : irow["CLOSE_YN"].ToString() == "N" ? "[" + yymm + " 최종마감 취소]" : "[ ]";
			}
			else
			{
				ends_yn = "";
				lb_ends.Text = "[" + yymm + " 최종마감 작업전]";
			}
		}
        #endregion

        #region 2 Button
		
		private void btn_bf_Click(object sender, EventArgs e)
		{
			if (clib.DateToText(dat_yymm.DateTime) == "")
				dat_yymm.DateTime = DateTime.Now;
			else
				dat_yymm.DateTime = dat_yymm.DateTime.AddMonths(-1);
		}
		private void btn_af_Click(object sender, EventArgs e)
		{
			if (clib.DateToText(dat_yymm.DateTime) == "")
				dat_yymm.DateTime = DateTime.Now;
			else
				dat_yymm.DateTime = dat_yymm.DateTime.AddMonths(1);
		}
		//근무표결재
		private void btn_off_gw_Click(object sender, EventArgs e)
		{
			if (sl_dept.EditValue == null)
			{
				MessageBox.Show("부서가 선택되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				sl_dept.Focus();
				return;
			}
			else if (sl_dept.EditValue.ToString().Substring(0, 1) == "A")
			{
				MessageBox.Show("해당부서는 상신할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			END_CHK();
			if (ends_yn == "Y")
			{
				MessageBox.Show("최종마감되어 상신할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else 
			{
				df.Get2060_GW_CHKDatas("5", clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
				if (ds.Tables["2060_GW_CHK"].Rows.Count > 0)
				{
					MessageBox.Show("이미 결재상신된 부서입니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
				{
					duty2062 duty2062 = new duty2062(ds.Tables["SEARCH_DANG_PLAN"], clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds.Tables["2060_DANGDEPT"].Select("CODE = '" + sl_dept.EditValue.ToString() + "'")[0]["DEPT_NM"].ToString(), MousePosition.X, MousePosition.Y);
					duty2062.ShowDialog();
				}
				Refresh_Click();
			}
		}
		//연차휴가 가져오기
		private void btn_yc_adp_Click(object sender, EventArgs e)
		{
			if (cmb_btype.SelectedIndex == 0)
			{
				df.Get3010_SEARCH_HYDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
				if (ds.Tables["3010_SEARCH_HY"].Rows.Count > 0)
				{
					if (ds.Tables["SEARCH_DANG_PLAN"] != null)
					{
						Cursor = Cursors.WaitCursor;
						grd1.DataSource = null;
						for (int i = 0; i < ds.Tables["3010_SEARCH_HY"].Rows.Count; i++)
						{
							DataRow drow = ds.Tables["3010_SEARCH_HY"].Rows[i];
							if (ds.Tables["SEARCH_DANG_PLAN"].Select("SAWON_NO = '" + drow["SABN"].ToString() + "'").Length > 0)
							{
								DataRow trow = ds.Tables["SEARCH_DANG_PLAN"].Select("SAWON_NO = '" + drow["SABN"].ToString() + "'")[0];
								trow["D" + drow["DD"].ToString()] = drow["GNMU"].ToString();
								//month_calc(drow["SABN"].ToString());
							}
						}

						string chk_sabn = "";
						for (int i = 0; i < ds.Tables["3010_SEARCH_HY"].Rows.Count; i++)
						{
							DataRow drow = ds.Tables["3010_SEARCH_HY"].Rows[i];
							if (chk_sabn != ds.Tables["3010_SEARCH_HY"].Rows[i]["SABN"].ToString())
							{
								month_calc(drow["SABN"].ToString());
								chk_sabn = drow["SABN"].ToString();
							}
						}
						Cursor = Cursors.Default;
						MessageBox.Show("휴가/연차 가져오기가 완료되었습니다.\r\n\r\n저장하지 않으면 적용되지 않습니다!", "완료", MessageBoxButtons.OK, MessageBoxIcon.Error);
						grd1.DataSource = ds.Tables["SEARCH_DANG_PLAN"];
					}
				}
				else
				{
					MessageBox.Show("해당년월에 신청된 휴가/연차 내역이 없습니다.", "자료없음", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				df.Get3010_S_TSDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
				if (ds.Tables["3010_S_TS"].Rows.Count > 0)
				{
					if (ds.Tables["SEARCH_DANG_PLAN"] != null)
					{
						Cursor = Cursors.WaitCursor;
						for (int i = 0; i < ds.Tables["3010_S_TS"].Rows.Count; i++)
						{
							DataRow drow = ds.Tables["3010_S_TS"].Rows[i];
							int tsdt = clib.TextToInt(drow["TSDT"].ToString());
							if (ds.Tables["SEARCH_DANG_PLAN"].Select("SAWON_NO = '" + drow["EMBSSABN"].ToString() + "'").Length > 0 && tsdt > 0)
							{
								DataRow trow = ds.Tables["SEARCH_DANG_PLAN"].Select("SAWON_NO = '" + drow["EMBSSABN"].ToString() + "'")[0];
								if (tsdt == 99) //이전퇴사자일땐 라인삭제
								{
									trow.Delete();
								}
								else
								{
									for (int j = tsdt + 1; j <= 31; j++)
									{
										trow["D" + j.ToString().PadLeft(2, '0')] = "";
									}
									month_calc(drow["EMBSSABN"].ToString());
								}
							}
						}
						Cursor = Cursors.Default;
						MessageBox.Show("퇴사자 가져오기가 완료되었습니다.\r\n\r\n저장하지 않으면 적용되지 않습니다!", "완료", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				else
				{
					MessageBox.Show("해당년월에 퇴사자가 없습니다.", "자료없음", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
		//출퇴근 표기
		private void btn_expand_Click(object sender, EventArgs e)
		{
			if (srTabControl2.Visible == true)
			{
				srTabControl2.Visible = false;
				btn_expand.Image = DUTY1000.Properties.Resources.확대;
			}
			else
			{
				srTabControl2.Visible = true;
				btn_expand.Image = DUTY1000.Properties.Resources.축소;
			}
		}
        //처리
        private void btn_proc_Click(object sender, EventArgs e)
        {
			if (isNoError_um(1))
			{
                Cursor = Cursors.WaitCursor;

                gridBand23.Caption = sl_dept.Text;
                gridBand27.Caption = clib.DateToText(dat_yymm.DateTime).Substring(4, 2) + "월 일자별 근무신청 현황";

                #region 처리시작
                END_CHK();
				grd1.Enabled = true;
				grd2.Enabled = true;

				dat_yymm.Enabled = false;
				sl_dept.Enabled = false;

				sl_embs.Enabled = true;
				btn_lineadd.Enabled = true;
				btn_linedel.Enabled = true;
				btn_yc_adp.Enabled = true;
				btn_bf_plan.Enabled = true;
				df.GetLOOK_DANG_EMBSDatas(ds); //clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), 
				sl_embs.Properties.DataSource = ds.Tables["LOOK_DANG_EMBS"];
				sl_embs.EditValue = null;

				baseInfoSearch(); //캘린더 조회

				df.GetSEARCH_HOLIDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), ds);
				//1.기준년월에 따른 하단 일자컬럼header 일자, 요일 설정
				int lastday = clib.TextToInt(clib.DateToText(clib.TextToDateLast(clib.DateToText(dat_yymm.DateTime))).Substring(6, 2));
                cmb_day.Properties.Items.Clear();

				for (int i = 1; i <= lastday; i++)
				{
					DateTime day = clib.TextToDate(clib.DateToText(dat_yymm.DateTime).Substring(0, 6) + "01").AddDays(i - 1);
					grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].Caption = i.ToString() + "\r\n" + clib.WeekDay(day); //일+요일. 한칸 내려서 보이도록. 엔터마냥. 
					if (clib.WeekDay(day) == "토")
					{
						grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Blue;
						grdv2.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Blue;
					}
					else if (clib.WeekDay(day) == "일")
					{
						grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Red;
						grdv2.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Red;
					}
					else
					{
						grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Black;
						grdv2.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Black;
					}

					if (ds.Tables["SEARCH_HOLI"].Select("H_DATE = '" + clib.DateToText(day) + "'").Length > 0)
					{
						grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Red;
						grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.Font = new Font(FontFamily.GenericSansSerif, 9F, FontStyle.Underline);
						grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].ToolTip = ds.Tables["SEARCH_HOLI"].Select("H_DATE = '" + clib.DateToText(day) + "'")[0]["H_NAME"].ToString();

						grdv2.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Red;
						grdv2.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.Font = new Font(FontFamily.GenericSansSerif, 9F, FontStyle.Underline);
						grdv2.Columns["D" + i.ToString().PadLeft(2, '0')].ToolTip = ds.Tables["SEARCH_HOLI"].Select("H_DATE = '" + clib.DateToText(day) + "'")[0]["H_NAME"].ToString();
					}
					cmb_day.Properties.Items.Add(i.ToString() + "일");
				}

				//남은 필드 visible = false;
				for (int k = 1; k < 32; k++)
				{
					if (k > lastday)
					{
						grdv1.Columns["D" + k.ToString().PadLeft(2, '0')].Visible = false;
						grdv2.Columns["D" + k.ToString().PadLeft(2, '0')].Visible = false;
					}
					else
					{
						grdv1.Columns["D" + k.ToString().PadLeft(2, '0')].Visible = true;
						grdv2.Columns["D" + k.ToString().PadLeft(2, '0')].Visible = true;
					}
				}
				//근무신청 내역 조회
				//df.Get3010_SEARCH_OREQDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
				df.Get2060_SEARCH_KTDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
				//근무유형 조회
				df.Get2060_DANG_GNMUDatas(ds);
				#endregion

				df.GetDUTY_TRSDANGDatas(2, clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
				if (ds.Tables["SEARCH_DANG_PLAN"].Rows.Count == 0)
				{
					df.GetSEARCH_DANG_PLANDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
					for (int i = 0; i < ds.Tables["SEARCH_DANG_PLAN"].Rows.Count; i++)
					{
						DataRow crow = ds.Tables["SEARCH_DANG_PLAN"].Rows[i];
						for (int k = 1; k <= lastday; k++)
						{
							DateTime day = clib.TextToDate(clib.DateToText(dat_yymm.DateTime).Substring(0, 6) + "01").AddDays(k - 1);
							string chk_dt = clib.DateToText(clib.TextToDate(clib.DateToText(dat_yymm.DateTime).Substring(0, 6) + "01").AddDays(k - 1)).Substring(6, 2);
							string chk_yn = "";
							if (crow["D" + k.ToString().PadLeft(2, '0')].ToString().Trim() == "" && crow["EMBSIPDT"].ToString().Trim() == "" && crow["MOVE_DT"].ToString().Trim() == "") //OFF신청이 없고 입사일자가 해당년월 이전
								chk_yn = "1";
							else if (crow["D" + k.ToString().PadLeft(2, '0')].ToString().Trim() == "" && crow["EMBSIPDT"].ToString().Trim() != "" && k >= clib.TextToInt(crow["EMBSIPDT"].ToString().PadRight(6, ' ').Substring(6, 2))) //OFF신청이 없고 입사일자 이후
								chk_yn = "1";							
							else if (crow["D" + k.ToString().PadLeft(2, '0')].ToString().Trim() == "" && crow["MOVE_DT"].ToString().Trim() != "" && k >= clib.TextToInt(crow["MOVE_DT"].ToString().PadRight(6, ' ').Substring(6, 2))) //OFF신청이 없고 부서이동 이후
								chk_yn = "1";

							if (chk_yn == "1")
							{
								if (clib.WeekDay(day) == "토")
									crow["D" + k.ToString().PadLeft(2, '0')] = "16";  //하프근무
								else if (clib.WeekDay(day) == "일")
									crow["D" + k.ToString().PadLeft(2, '0')] = "11";  //오프
								else
									crow["D" + k.ToString().PadLeft(2, '0')] = "01";  //데이근무
							}
						}
					}

					if (admin_lv == 2)  //부서장일때 관리부서인지 체크
					{
						df.GetCHK_DEPTDatas(SilkRoad.Config.SRConfig.USID, sl_dept.EditValue.ToString(), ds);
						if (ds.Tables["CHK_DEPT"].Rows.Count > 0)
							SetButtonEnable("011011");
						else
							SetButtonEnable("010011");
					}
					else
					{
						if (p_dpcd == "%" || p_dpcd == sl_dept.EditValue.ToString())
							SetButtonEnable("011011");
						else
							SetButtonEnable("010011");
					}
				}
				else
				{
					btn_save.Text = "수  정";
                    btn_save.Image = DUTY1000.Properties.Resources.수정;

					if (admin_lv == 2)  //부서장일때 관리부서인지 체크
					{
						df.GetCHK_DEPTDatas(SilkRoad.Config.SRConfig.USID, sl_dept.EditValue.ToString(), ds);
						if (ds.Tables["CHK_DEPT"].Rows.Count > 0)
							SetButtonEnable("011111");
						else
							SetButtonEnable("010011");
					}
					else
					{
						if (p_dpcd == "%" || p_dpcd == sl_dept.EditValue.ToString())
							SetButtonEnable("011111");
						else
							SetButtonEnable("010011");
					}

					if (grdv_dept.GetFocusedRowCellValue("GW_STAT").ToString() != "")
						SetButtonEnable("010011");
				}

				for (int i = 0; i < ds.Tables["SEARCH_DANG_PLAN"].Rows.Count; i++)
				{
					for (int k = 1; k <= lastday; k++)
					{
						DataRow crow = ds.Tables["SEARCH_DANG_PLAN"].Rows[i];
						if (ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + crow["D" + k.ToString().PadLeft(2, '0')].ToString().Trim() + "'").Length > 0)
						{
							crow["D" + k.ToString().PadLeft(2, '0') + "_NM"] = ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + crow["D" + k.ToString().PadLeft(2, '0')].ToString() + "'")[0]["G_SNM"].ToString();
						}
					}
				}

				df.GetSUM_DANG_PLANDatas(ds);
				grd2.DataSource = ds.Tables["SUM_DANG_PLAN"];
				//for (int i = 0; i < ds.Tables["SEARCH_CALL_PLAN"].Rows.Count; i++)
				//{
				//	month_calc(ds.Tables["SEARCH_CALL_PLAN"].Rows[i]["SAWON_NO"].ToString());
				//}
				sum_month_calc();

				//고정OT사용부서일 경우 보이게
				df.GetSEARCH_DUTY_INFOFXOTDatas(sl_dept.EditValue.ToString(), ds);
				if (ds.Tables["SEARCH_DUTY_INFOFXOT"].Rows.Count > 0)
					bandedGridColumn2.Visible = true;
				else
					bandedGridColumn2.Visible = false;

				grd1.DataSource = ds.Tables["SEARCH_DANG_PLAN"];
				grdv1.Focus();

				Cursor = Cursors.Default;
			}
		}

        //저장버튼
        private void btn_save_Click(object sender, EventArgs e)
        {
            if (isNoError_um(2))
            {
                Cursor = Cursors.WaitCursor;
                int outVal = 0;
                try
                {
                    // 1. 해당년월에 사번의trscall 가져오기
                    df.GetDUTY_TRSDANGDatas(1, clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);

					// 2.그리드한바퀴 글자들 합해서 넣기
					DataRow nrow = null;
					int sq = 1;
					//for (int i = 0; i < ds.Tables["SEARCH_DANG_PLAN"].Rows.Count; i++)
					//{
					//	DataRow drow = ds.Tables["SEARCH_DANG_PLAN"].Rows[i];
					//	if (drow.RowState != DataRowState.Deleted)
					//	{
					//		if (ds.Tables["DUTY_TRSDANG"].Select("SAWON_NO = '" + drow["SAWON_NO"] + "'").Length > 0)
					//		{
					//			nrow = ds.Tables["DUTY_TRSDANG"].Select("SAWON_NO = '" + drow["SAWON_NO"] + "'")[0];
					//			nrow["UPDT"] = gd.GetNow();
					//			nrow["USID"] = SilkRoad.Config.SRConfig.USID;
					//			nrow["PSTY"] = "U";
					//		}
					//		else
					//		{
					//			nrow = ds.Tables["DUTY_TRSDANG"].NewRow();
					//			nrow["SAWON_NO"] = drow["SAWON_NO"].ToString();
					//			nrow["PLANYYMM"] = clib.DateToText(dat_yymm.DateTime).Substring(0, 6);
					//			nrow["DEPTCODE"] = sl_dept.EditValue.ToString();
					//			nrow["INDT"] = gd.GetNow();
					//			nrow["UPDT"] = "";
					//			nrow["USID"] = SilkRoad.Config.SRConfig.USID;
					//			nrow["PSTY"] = "A";
					//			ds.Tables["DUTY_TRSDANG"].Rows.Add(nrow);
					//		}
					//		nrow["PLAN_SQ"] = sq;
					//		nrow["MM_CNT1"] = drow["MM_CNT1"];
					//		nrow["MM_CNT2"] = drow["MM_CNT2"];
					//		nrow["MM_CNT3"] = drow["MM_CNT3"];
					//		nrow["MM_CNT4"] = drow["MM_CNT4"];
					//		nrow["MM_CNT5"] = drow["MM_CNT5"];
					//		for (int j = 1; j <= 31; j++)
					//		{
					//			nrow["D" + j.ToString().PadLeft(2, '0')] = drow["D" + j.ToString().PadLeft(2, '0')].ToString();
					//		}
					//		sq++;
					//	}
					//	else
					//	{
					//		if (ds.Tables["DUTY_TRSDANG"].Select("SAWON_NO = '" + drow["SAWON_NO", DataRowVersion.Original].ToString().Trim() + "' ").Length > 0)
					//			ds.Tables["DUTY_TRSDANG"].Select("SAWON_NO = '" + drow["SAWON_NO", DataRowVersion.Original].ToString().Trim() + "' ")[0].Delete();
					//	}
					//}
					
					foreach (DataRow drow in ds.Tables["SEARCH_DANG_PLAN"].Rows)
					{
						if (drow.RowState != DataRowState.Deleted)
						{
							if (ds.Tables["DUTY_TRSDANG"].Select("SAWON_NO = '" + drow["SAWON_NO"] + "'").Length > 0)
							{
								nrow = ds.Tables["DUTY_TRSDANG"].Select("SAWON_NO = '" + drow["SAWON_NO"] + "'")[0];
								nrow["UPDT"] = gd.GetNow();
								nrow["USID"] = SilkRoad.Config.SRConfig.USID;
								nrow["PSTY"] = "U";
							}
							else
							{
								nrow = ds.Tables["DUTY_TRSDANG"].NewRow();
								nrow["SAWON_NO"] = drow["SAWON_NO"].ToString();
								nrow["PLANYYMM"] = clib.DateToText(dat_yymm.DateTime).Substring(0, 6);
								nrow["DEPTCODE"] = sl_dept.EditValue.ToString();
								nrow["INDT"] = gd.GetNow();
								nrow["UPDT"] = "";
								nrow["USID"] = SilkRoad.Config.SRConfig.USID;
								nrow["PSTY"] = "A";
								ds.Tables["DUTY_TRSDANG"].Rows.Add(nrow);
							}
							nrow["PLAN_SQ"] = sq;
							nrow["REMARK"] = drow["REMARK"].ToString().Trim();
							nrow["MM_CNT1"] = drow["MM_CNT1"];
							nrow["MM_CNT2"] = drow["MM_CNT2"];
							nrow["MM_CNT3"] = drow["MM_CNT3"];
							nrow["MM_CNT4"] = drow["MM_CNT4"];
							nrow["MM_CNT5"] = drow["MM_CNT5"];
							for (int j = 1; j <= 31; j++)
							{
								nrow["D" + j.ToString().PadLeft(2, '0')] = drow["D" + j.ToString().PadLeft(2, '0')].ToString();
							}
							sq++;
						}
						else
						{
							if (ds.Tables["DUTY_TRSDANG"].Select("SAWON_NO = '" + drow["SAWON_NO", DataRowVersion.Original].ToString().Trim() + "' ").Length > 0)
								ds.Tables["DUTY_TRSDANG"].Select("SAWON_NO = '" + drow["SAWON_NO", DataRowVersion.Original].ToString().Trim() + "' ")[0].Delete();
						}
					}

                    string[] tableNames = new string[] { "DUTY_TRSDANG" };
                    SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
                    outVal = cmd.setUpdate(ref ds, tableNames, null);
                }
                catch (Exception ec)
                {
                    MessageBox.Show(ec.Message, "저장오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    btn_refresh.PerformClick();
                    SetCancel();
                    Cursor = Cursors.Default;
                }
            }
        }

        //삭제
        private void btn_del_Click(object sender, EventArgs e)
        {
			if (isNoError_um(3))
			{
				string yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 4) + "-" + clib.DateToText(dat_yymm.DateTime).Substring(4, 2);
				DialogResult dr = MessageBox.Show(yymm + "월의 근무내역을 모두 삭제하시겠습니까?", "삭제여부", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
				if (dr == DialogResult.OK)
				{
					int outVal = 0;
					try
					{
						df.GetDUTY_TRSDANGDatas(1, clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
						for (int i = 0; i < ds.Tables["DUTY_TRSDANG"].Rows.Count; i++)
						{
							ds.Tables["DUTY_TRSDANG"].Rows[i].Delete();
						}

						string[] tableNames = new string[] { "DUTY_TRSDANG" };
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
							MessageBox.Show("조회된 내역의 근무가 삭제되었습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);

						SetCancel();
						Cursor = Cursors.Default;
					}
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
		
        //미리보기
        private void btn_preview_Click(object sender, EventArgs e)
        {
			print(1);
		}		
        // 인쇄
        private void btn_print_Click(object sender, EventArgs e)
        {
			print(2);
        }

        /// <summary>엑셀버튼</summary>
        private void btn_exel_Click(object sender, EventArgs e)
        {
            clib.gridToExcel(grdv1, this.Text + "(" + this.Name + ")_" + clib.DateToText(DateTime.Now), true);
        }
		private void print(int stat)
		{
			string fxot_gubn = "";
			df.GetSEARCH_DUTY_INFOFXOTDatas(sl_dept.EditValue.ToString(), ds);
			if (ds.Tables["SEARCH_DUTY_INFOFXOT"].Rows.Count > 0)
				fxot_gubn = "1";

			rpt_2060 rpt = new rpt_2060(clib.DateToText(dat_yymm.DateTime), sl_dept.Text.ToString(), fxot_gubn, ds.Tables["SEARCH_HOLI"]);
			rpt.DataSource = ds.Tables["SEARCH_DANG_PLAN"];
			if (stat == 1)
				rpt.ShowPreview();
			else if (stat == 2)
				rpt.Print();
		}
		
		//근무적용
		private void btn_set_gnmu_Click(object sender, EventArgs e)
		{
			if (ds.Tables["SEARCH_DANG_PLAN"] != null && sl_gnmu.EditValue != null)
			{
				string d_nm = (cmb_day.SelectedIndex + 1).ToString().PadLeft(2, '0');
				for (int i = 0; i < ds.Tables["SEARCH_DANG_PLAN"].Rows.Count; i++)
				{
					DataRow drow = ds.Tables["SEARCH_DANG_PLAN"].Rows[i];
					if (drow.RowState != DataRowState.Deleted)
						drow["D"+d_nm] = sl_gnmu.EditValue.ToString();
				}
			}
		}
		//라인추가
		private void btn_lineadd_Click(object sender, EventArgs e)
		{
			if (sl_embs.EditValue == null)
			{
				MessageBox.Show("근무자를 선택하세요!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				sl_embs.Focus();
			}
			else if (ds.Tables["SEARCH_DANG_PLAN"].Select("SAWON_NO = '" + sl_embs.EditValue.ToString() + "'").Length > 0)
			{
				MessageBox.Show("이미 신청된 근무자입니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				sl_embs.Focus();
			}
			else
			{
				int max_sq = clib.TextToInt(ds.Tables["SEARCH_DANG_PLAN"].Compute("MAX(PLAN_SQ)", null).ToString());
				DataRow trow = ds.Tables["LOOK_DANG_EMBS"].Select("CODE = '" + sl_embs.EditValue.ToString() + "'")[0];
				DataRow nrow = ds.Tables["SEARCH_DANG_PLAN"].NewRow();
				nrow["SAWON_NO"] = sl_embs.EditValue.ToString();
				nrow["SAWON_NM"] = trow["NAME"].ToString();
				nrow["PLAN_SQ"] = max_sq + 1;
				ds.Tables["SEARCH_DANG_PLAN"].Rows.Add(nrow);
			}
		}
		//라인삭제
		private void btn_linedel_Click(object sender, EventArgs e)
		{
            DataRow drow = grdv1.GetFocusedDataRow();
            if (drow == null)
                return;
			
            DialogResult dr = MessageBox.Show(drow["SAWON_NM"].ToString() + "님의 근무내역을 삭제하시겠습니까?", "삭제여부", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			if (dr == DialogResult.OK)
			{
				int now_slsq = clib.TextToInt(drow["PLAN_SQ"].ToString());
				drow.Delete();

				for (int i = 0; i < ds.Tables["SEARCH_DANG_PLAN"].Rows.Count; i++)
				{
					DataRow crow = ds.Tables["SEARCH_DANG_PLAN"].Rows[i];
					if (crow.RowState != DataRowState.Deleted)
					{
						if (clib.TextToInt(crow["PLAN_SQ"].ToString()) > now_slsq)
						{
							crow["PLAN_SQ"] = clib.TextToInt(crow["PLAN_SQ"].ToString()) - 1;
						}
					}
				}
			}
		}
		
		//부서설정
		private void btn_info_Click(object sender, EventArgs e)
		{			
			duty2061 duty2061 = new duty2061();
			duty2061.ShowDialog();
			
			sl_dept.EditValue = null;
			sl_embs.EditValue = null;
			SetCancel();

			Refresh_Click();
			//df.GetLOOK_DANG_EMBSDatas(ds);
			//sl_embs.Properties.DataSource = ds.Tables["LOOK_DANG_EMBS"];
		}
		//전월근무조회
		private void btn_bf_plan_Click(object sender, EventArgs e)
		{
			df.GetSEARCH_BF_HOLIDatas(clib.DateToText(dat_yymm.DateTime.AddMonths(-1)).Substring(0, 6), ds);
			df.GetDUTY_BF_TRSDANGDatas(clib.DateToText(dat_yymm.DateTime.AddMonths(-1)).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
			//1.기준년월에 따른 하단 일자컬럼header 일자, 요일 설정
			int lastday = clib.TextToInt(clib.DateToText(clib.TextToDateLast(clib.DateToText(dat_yymm.DateTime.AddMonths(-1)))).Substring(6, 2));
			for (int i = 0; i < ds.Tables["SEARCH_BF_DANG"].Rows.Count; i++)
			{
				for (int k = 1; k <= lastday; k++)
				{
					DataRow crow = ds.Tables["SEARCH_BF_DANG"].Rows[i];
					if (ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + crow["D" + k.ToString().PadLeft(2, '0')].ToString() + "'").Length > 0)
					{
						crow["D" + k.ToString().PadLeft(2, '0') + "_NM"] = ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + crow["D" + k.ToString().PadLeft(2, '0')].ToString() + "'")[0]["G_SNM"].ToString();
					}
				}
			}
			
			string fxot_gubn = "";
			df.GetSEARCH_DUTY_INFOFXOTDatas(sl_dept.EditValue.ToString(), ds);
			if (ds.Tables["SEARCH_DUTY_INFOFXOT"].Rows.Count > 0)
				fxot_gubn = "1";

			rpt_2060 rpt = new rpt_2060(clib.DateToText(dat_yymm.DateTime.AddMonths(-1)), sl_dept.Text.ToString(), fxot_gubn, ds.Tables["SEARCH_BF_HOLI"]);
			rpt.DataSource = ds.Tables["SEARCH_BF_DANG"];
			rpt.ShowPreview();
		}

        /// <summary> refresh버튼 </summary>
        private void btn_refresh_Click(object sender, EventArgs e)
        {
			Refresh_Click();
        }
        /// <summary> refresh버튼 </summary>
        private void Refresh_Click()
        {
			df.Get2060_DANGDEPTDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), ds);
			sl_dept.Properties.DataSource = ds.Tables["2060_DANGDEPT"];
			grd_dept.DataSource = ds.Tables["2060_DANGDEPT"];

			if (grid_dept != "")
				grdv_dept.FocusedRowHandle = grdv_dept.LocateByValue("CODE", grid_dept, null);

			df.Get2060_DANG_GNMUDatas(ds);
			grd_sl_gnmu.DataSource = ds.Tables["2060_DANG_GNMU"];
			sl_gnmu.Properties.DataSource = ds.Tables["2060_DANG_GNMU"];

			//if (sl_dept.EditValue != null)
			//{
			//	df.GetLOOK_DANG_EMBSDatas(sl_dept.EditValue.ToString(), ds);
			//	sl_embs.Properties.DataSource = ds.Tables["LOOK_DANG_EMBS"];
			//}			
        }

		#endregion

		#region 3 EVENT
		
		//메뉴 활성화시
		private void duty2060_Activated(object sender, EventArgs e)
		{
			END_CHK();
		}
		private void duty2060_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)   //취소
            {
                btn_clear.PerformClick();
            }
        }

        //내역 더블클릭시 인사카드
        private void grdv1_DoubleClick(object sender, EventArgs e)
        {
            DataRow drow = grdv1.GetFocusedDataRow();
            if (drow != null)
            {
                if (admin_lv == 2)  //부서장일때 관리부서인지 체크
                {
                    df.GetCHK_DEPTDatas(SilkRoad.Config.SRConfig.USID, sl_dept.EditValue.ToString(), ds);
                    if (ds.Tables["CHK_DEPT"].Rows.Count == 0)
                    {
                        MessageBox.Show("해당부서에 관리권한이 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (admin_lv == 1 && (p_dpcd != sl_dept.EditValue.ToString()))  //팀장일때 관리부서인지 체크    
                {
                    MessageBox.Show("해당부서에 관리권한이 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                df.GetINFOBASEDatas(ref ds);
                df.GetSabnSubInfo(drow["SAWON_NO"].ToString(), ds);  //사원기본정보
                rpt_2210 main_rpt = new rpt_2210(ds);

                main_rpt.DataSource = ds.Tables["INSA_PIC"];
                main_rpt.writeReport(ds, 0);
                main_rpt.ShowPreview();
            }
        }

        //조회년월 변경시 부서 다시 불러오기
        private void dat_yymm_EditValueChanged(object sender, EventArgs e)
		{
            SetCancel();
			//Refresh_Click();
		}
		//부서 더블클릭시
		private void grdv_dept_DoubleClick(object sender, EventArgs e)
		{			
			grid_dept = grdv_dept.GetFocusedRowCellValue("CODE").ToString();
            SetCancel();
			sl_dept.EditValue = grid_dept;
			btn_proc.PerformClick();			
		}
		private void grdv_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
		{
            DataRow drow = grdv1.GetFocusedDataRow();
			if (drow != null)
			{
				if (grdv1.FocusedColumn.Name.ToString().Substring(0, 10) == "grdcol_day")
				{
					if (chk_g_adt.Checked == true)
					{
						e.Cancel = true;
						if (sl_gnmu.EditValue == null)
						{
							drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2)] = "";
							drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2) + "_NM"] = "";
						}
						else
						{
							drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2)] = sl_gnmu.EditValue;
							if (ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + sl_gnmu.EditValue.ToString() + "'").Length > 0)
								drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2) + "_NM"] = ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + sl_gnmu.EditValue.ToString() + "'")[0]["G_SNM"].ToString();
						}
						month_calc(drow["SAWON_NO"].ToString().Trim());
					}
				}
			}
		}

        private void grdv_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            DataRow drow = grdv1.GetFocusedDataRow();
            if (drow != null)
            {
				if (e.Column.Name.StartsWith("grdcol_day"))
				{
					if (e.Value == null)
					{
						drow["D" + e.Column.Name.ToString().Substring(10, 2)] = "";
						drow["D" + e.Column.Name.ToString().Substring(10, 2) + "_NM"] = "";
					}
					else
					{
						drow["D" + e.Column.Name.ToString().Substring(10, 2)] = e.Value.ToString();
						if (ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + e.Value.ToString() + "'").Length > 0)
							drow["D" + e.Column.Name.ToString().Substring(10, 2) + "_NM"] = ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + e.Value.ToString() + "'")[0]["G_SNM"].ToString();
					}
				}
				month_calc(drow["SAWON_NO"].ToString().Trim());
            }
        }
		
        //근무값별 색 주기 ★
        private void grdv_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.Name.StartsWith("grdcol_day"))
            {
				string stdt = clib.DateToText(DateTime.Now);
				string date = clib.DateToText(dat_yymm.DateTime).Substring(0, 6) + e.Column.Name.ToString().Substring(10, 2);
				string sabn = grdv1.GetDataRow(e.RowHandle)["SAWON_NO"].ToString().Trim();

				if (ds.Tables["2060_SEARCH_KT"].Select("SABN = '" + sabn + "' AND SLDT = '" + date + "'").Length > 0)  //출근시 체크
				{
					if (ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + e.CellValue + "'") != null && ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + e.CellValue + "'").Length > 0)
					{
						int g_type = clib.TextToInt(ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["G_TYPE"].ToString());
						if (g_type == 2 || g_type == 4 || g_type == 5 || g_type == 9 || g_type == 10) //근무 or 당직근무인데 출근이면 근무색상
						{
							int colVAlue = clib.TextToInt(ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["G_COLOR"].ToString());
							e.Appearance.BackColor = Color.FromArgb(colVAlue);
						}
						else //근무가 아닌데 출근이면
						{
							if ((g_type == 8 || g_type == 12) && clib.TextToDecimal(ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["YC_DAY"].ToString()) < 8) //반차or시간 출근이면
							{
								int colVAlue = clib.TextToInt(ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["G_COLOR"].ToString());
								e.Appearance.BackColor = Color.FromArgb(colVAlue);
							}
							else
								e.Appearance.BackColor = Color.Red;
						}
					}
				}
				else  //출근하지 않았을때 체크
				{
					if (ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + e.CellValue + "'") != null && ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + e.CellValue + "'").Length > 0)
					{
						int g_type = clib.TextToInt(ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["G_TYPE"].ToString());
						if (g_type == 2 || g_type == 4 || g_type == 5 || g_type == 9 || g_type == 10) //근무 or 당직근무인데 출근하지 않으면 붉은색
						{
							if (clib.TextToDate(date) <= clib.TextToDate(stdt))  //현재일 이전일때 오류색상
								e.Appearance.BackColor = Color.Red;
							else  //현재일 이후는 근무색상
							{
								int colVAlue = clib.TextToInt(ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["G_COLOR"].ToString());
								e.Appearance.BackColor = Color.FromArgb(colVAlue);
							}
						}
						else  // 근무가 아니면서 출근하지 않았을땐 근무색상으로
						{
							int colVAlue = clib.TextToInt(ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["G_COLOR"].ToString());
							e.Appearance.BackColor = Color.FromArgb(colVAlue);
						}
					}
				}
			}
        }

		private void grdv1_RowClick(object sender, RowClickEventArgs e)
		{
			if (e.RowHandle < 0)
				return;

			DataRow drow = grdv1.GetFocusedDataRow();
			df.Get3010_KT_DTDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), drow["SAWON_NO"].ToString().Trim(), ds);
			grd_kt1.DataSource = ds.Tables["3010_KT_DT1"];
			grd_kt2.DataSource = ds.Tables["3010_KT_DT2"];

			string sldt = clib.DateToText(clib.TextToDateLast(clib.DateToText(dat_yymm.DateTime)));
			df.GetSEARCH_DYYCDatas(sldt, drow["SAWON_NO"].ToString().Trim(), ds);
			grd_dyyc.DataSource = ds.Tables["SEARCH_DYYC"];

			DataRow drow2 = grdv_dyyc.GetFocusedDataRow();
			if (drow2 != null)
			{
				string yy = drow2["YC_YEAR"].ToString();
				string sabn = drow2["SAWON_NO"].ToString();

				df.GetSEARCH_TRSHREQDatas(sabn, yy, ds);
				grd_yc.DataSource = ds.Tables["SEARCH_TRSHREQ"];
			}
			else
			{
				grd_yc.DataSource = null;
			}
		}
		
		//그리드 클릭시 연차내역 조회
		private void grdv_dyyc_RowClick(object sender, RowClickEventArgs e)
		{
            if (e.RowHandle < 0)
                return;

            DataRow drow = grdv_dyyc.GetFocusedDataRow();
			string yy = drow["YC_YEAR"].ToString();
			string sabn = drow["SAWON_NO"].ToString();

			df.GetSEARCH_TRSHREQDatas(sabn, yy, ds);
			grd_yc.DataSource = ds.Tables["SEARCH_TRSHREQ"];
		}
		private void grdv_dyyc_DoubleClick(object sender, EventArgs e)
		{
			string yy = grdv_dyyc.GetFocusedRowCellValue("YC_YEAR").ToString();
			string sabn = grdv_dyyc.GetFocusedRowCellValue("SAWON_NO").ToString();

			df.GetSEARCH_TRSHREQDatas(sabn, yy, ds);
			grd_yc.DataSource = ds.Tables["SEARCH_TRSHREQ"];
		}
		#endregion

		#region 4. Drag & Drop

        GridHitInfo downHitInfo = null;
        private void grdv_MouseDown(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView; //1st
            downHitInfo = null;

            GridHitInfo hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));
            if (Control.ModifierKeys != Keys.None)
                return;
            if (e.Button == MouseButtons.Left && hitInfo.InRow && hitInfo.RowHandle != GridControl.NewItemRowHandle)
                downHitInfo = hitInfo;

        }
        private void grdv_MouseMove(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;  //2nd
            if (e.Button == MouseButtons.Left && downHitInfo != null)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(downHitInfo.HitPoint.X - dragSize.Width / 2,
                    downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    view.GridControl.DoDragDrop(downHitInfo, DragDropEffects.All);
                    downHitInfo = null;
                }
            }
        }
        private void grd_DragOver(object sender, DragEventArgs e)
        {
			if (e.Data.GetDataPresent(typeof(BandedGridHitInfo)))
			{
				GridHitInfo downHitInfo = e.Data.GetData(typeof(BandedGridHitInfo)) as GridHitInfo;
				if (downHitInfo == null)
					return;

				GridControl grid = sender as GridControl;
				GridView view = grid.MainView as GridView;
				GridHitInfo hitInfo = view.CalcHitInfo(grid.PointToClient(new Point(e.X, e.Y)));
				if (hitInfo.InRow && hitInfo.RowHandle != downHitInfo.RowHandle && hitInfo.RowHandle != GridControl.NewItemRowHandle)
					e.Effect = DragDropEffects.Copy;//.Move;
				else
					e.Effect = DragDropEffects.None;
			}
		}
        private void grd_DragDrop(object sender, DragEventArgs e)
        {
            GridControl grid = sender as GridControl;
            GridView view = grid.MainView as GridView;
            GridHitInfo srcHitInfo = e.Data.GetData(typeof(BandedGridHitInfo)) as GridHitInfo;
            GridHitInfo hitInfo = view.CalcHitInfo(grid.PointToClient(new Point(e.X, e.Y)));
            int sourceRow = srcHitInfo.RowHandle;
            int targetRow = hitInfo.RowHandle;
            MoveRow(sourceRow, targetRow);
        }
        //순번재정렬
        private void MoveRow(int sourceRow, int targetRow)
        {
            if (sourceRow == targetRow || sourceRow == targetRow + 1)
                return;

            GridView view = grdv1;
            DataRow row = grdv1.GetFocusedDataRow();
			string sabn = row["SAWON_NO"].ToString();
            int now_slsq = clib.TextToInt(row["PLAN_SQ"].ToString());
            if (sourceRow < targetRow)
            {
                for (int i = sourceRow + 1; i <= targetRow; i++)
                {
                    int for_slsq = i + 1;
                    DataRow crow = ds.Tables["SEARCH_DANG_PLAN"].Select("PLAN_SQ = " + for_slsq + "")[0];
                    if (crow.RowState != DataRowState.Deleted)
                    {
                        crow["PLAN_SQ"] = clib.TextToInt(crow["PLAN_SQ"].ToString()) - 1;
                    }
                }
				
				ds.Tables["SEARCH_DANG_PLAN"].Select("SAWON_NO = '" + sabn + "'")[0]["PLAN_SQ"] = targetRow + 1;
            }
            else
            {
				int tg_slsq = targetRow < 0 ? 0 : clib.TextToInt(ds.Tables["SEARCH_DANG_PLAN"].Select("PLAN_SQ = " + (targetRow + 1))[0]["PLAN_SQ"].ToString());
				for (int i = 0; i < ds.Tables["SEARCH_DANG_PLAN"].Rows.Count; i++)
				{
					DataRow crow = ds.Tables["SEARCH_DANG_PLAN"].Rows[i];
					if (crow.RowState != DataRowState.Deleted)
					{
						if (clib.TextToInt(crow["PLAN_SQ"].ToString()) > tg_slsq && clib.TextToInt(crow["PLAN_SQ"].ToString()) <= sourceRow)
						{
							crow["PLAN_SQ"] = clib.TextToInt(crow["PLAN_SQ"].ToString()) + 1;
						}
					}
				}
				ds.Tables["SEARCH_DANG_PLAN"].Select("SAWON_NO = '" + sabn + "'")[0]["PLAN_SQ"] = targetRow < 0 ? 1 : targetRow + 2;
            }
			ds.Tables["SEARCH_DANG_PLAN"].DefaultView.Sort = "PLAN_SQ ASC";
			DataTable dt = ds.Tables["SEARCH_DANG_PLAN"].DefaultView.ToTable();
			dp.AddDatatable2Dataset("SEARCH_DANG_PLAN", dt, ref ds);
			grd1.DataSource = ds.Tables["SEARCH_DANG_PLAN"];
			grdv1.FocusedRowHandle = targetRow + 1;
        }
        private void grd_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
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
			END_CHK();

            if (mode == 1)  //처리
            {
                if (admin_lv == 0)
                {
                    MessageBox.Show("관리권한이 없습니다. 인사기본관리의 관리자구분을 확인하세요!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (dat_yymm.Text.ToString().Trim() == "")
                {
                    MessageBox.Show("조회년월을 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dat_yymm.Focus();
                    return false;
                }
                else if (sl_dept.EditValue == null)
                {
                    MessageBox.Show("부서를 선택하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sl_dept.Focus();
                    return false;
                }
                else
                {
                    isError = true;
                }
            }
            else if (mode == 2)  //저장
            {
				if (ends_yn == "Y")
				{
					MessageBox.Show("최종마감되어 저장할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				else
				{
					isError = true;
				}
			}
            else if (mode == 3)  //삭제
            {
				if (ends_yn == "Y")
				{
					MessageBox.Show("최종마감되어 삭제할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
			btn_proc.Enabled = arr.Substring(0, 1) == "1" ? true : false;
			btn_clear.Enabled = arr.Substring(1, 1) == "1" ? true : false;
			btn_save.Enabled = arr.Substring(2, 1) == "1" ? true : false;
			btn_del.Enabled = arr.Substring(3, 1) == "1" ? true : false;
			btn_preview.Enabled = arr.Substring(4, 1) == "1" ? true : false;
			btn_print.Enabled = arr.Substring(5, 1) == "1" ? true : false;
		}

		private void month_calc(string sabn)
		{
			//근무유형 조회
			df.Get2060_DANG_GNMUDatas(ds);

			if (ds.Tables["SEARCH_DANG_PLAN"].Select("SAWON_NO = '" + sabn + "'").Length > 0)
			{
				DataRow srow = ds.Tables["SEARCH_DANG_PLAN"].Select("SAWON_NO = '" + sabn + "'")[0];
				//1.기준년월에 따른 하단 일자컬럼header 일자, 요일 설정
				int lastday = clib.TextToInt(clib.DateToText(clib.TextToDateLast(clib.DateToText(dat_yymm.DateTime))).Substring(6, 2));
				//남은 필드 visible = false;
				int Dang = 0, Bojo = 0;
				for (int k = 1; k <= lastday; k++)
				{
					if (ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + srow["D" + k.ToString().PadLeft(2, '0')].ToString() + "'").Length > 0)
					{
						string g_type = ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + srow["D" + k.ToString().PadLeft(2, '0')].ToString() + "'")[0]["G_TYPE"].ToString();

						if (g_type == "9")
							Dang += 1;
						else if (g_type == "10")
							Bojo += 1;
					}
				}
				srow["MM_CNT1"] = Dang;
				srow["MM_CNT2"] = Bojo;

				sum_month_calc();
			}
		}
		private void sum_month_calc()
		{
			int T_Dang = 0, T_Bojo = 0;
			int lastday = clib.TextToInt(clib.DateToText(clib.TextToDateLast(clib.DateToText(dat_yymm.DateTime))).Substring(6, 2));
			for (int d = 1; d <= lastday; d++)
			{
				string dd = d.ToString().PadLeft(2, '0');
				int Dang = 0, Bojo = 0;
				for (int i = 0; i < ds.Tables["SEARCH_DANG_PLAN"].Rows.Count; i++)
				{
					DataRow drow = ds.Tables["SEARCH_DANG_PLAN"].Rows[i];
					if (drow.RowState != DataRowState.Deleted)
					{
						if (ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + drow["D" + dd].ToString() + "'").Length > 0)
						{
							string g_type = ds.Tables["2060_DANG_GNMU"].Select("G_CODE = '" + drow["D" + dd].ToString() + "'")[0]["G_TYPE"].ToString();
							if (g_type == "9")
							{
								Dang += 1;
								T_Dang += 1;
							}
							else if (g_type == "10")
							{
								Bojo += 1;
								T_Bojo += 1;
							}
						}
					}
				}
				if (ds.Tables["SUM_DANG_PLAN"].Select("G_TYPE = 9").Length > 0)
				{
					ds.Tables["SUM_DANG_PLAN"].Select("G_TYPE = 9")[0]["D" + dd] = Dang;
					ds.Tables["SUM_DANG_PLAN"].Select("G_TYPE = 9")[0]["MM_CNT1"] = T_Dang;
				}
				if (ds.Tables["SUM_DANG_PLAN"].Select("G_TYPE = 10").Length > 0)
				{
					ds.Tables["SUM_DANG_PLAN"].Select("G_TYPE = 10")[0]["D" + dd] = Bojo;
					ds.Tables["SUM_DANG_PLAN"].Select("G_TYPE = 10")[0]["MM_CNT2"] = T_Bojo;
				}
			}
		}

		#endregion

	}
}
