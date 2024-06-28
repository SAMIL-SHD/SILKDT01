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

namespace DUTY1000
{
    public partial class duty3010 : SilkRoad.Form.Base.FormX
    {
        CommonLibrary clib = new CommonLibrary();

        ClearNEnableControls cec = new ClearNEnableControls();
        public DataSet ds = new DataSet();
        static DataProcessing dp = new DataProcessing();
        DataProcFunc df = new DataProcFunc();
        SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();
        private string ends_yn = "";
		
		private int admin_lv = 0;
        private string msyn = "";
        private string upyn = "";
        private string p_dpcd = "";
		
        private string grid_dept = "";
        private string s_yymm = "";
        public duty3010()
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
			s_yymm = "";

			if (ds.Tables["SEARCH_PLAN"] != null)
				ds.Tables["SEARCH_PLAN"].Clear();
			if (ds.Tables["3010_KT_DT1"] != null)
				ds.Tables["3010_KT_DT1"].Clear();
			if (ds.Tables["3010_KT_DT2"] != null)
				ds.Tables["3010_KT_DT2"].Clear();
			if (ds.Tables["SUM_PLAN"] != null)
				ds.Tables["SUM_PLAN"].Clear();
			
			if (ds.Tables["SEARCH_DYYC"] != null)
				ds.Tables["SEARCH_DYYC"].Clear();
			if (ds.Tables["SEARCH_TRSHREQ"] != null)
				ds.Tables["SEARCH_TRSHREQ"].Clear();
			
			srTabControl1.Visible = false;
			btn_expand.Image = DUTY1000.Properties.Resources.확대;

            btn_save.Text = "저  장";
            btn_save.Image = DUTY1000.Properties.Resources.저장;
			grd1.Enabled = false;
			grd2.Enabled = false;

			dat_yymm.Enabled = true;
			sl_dept.Enabled = true;
			//sl_dept.Enabled = p_dpcd == "%" ? true : false;
            sl_nurs.Enabled = false;
			btn_lineadd.Enabled = false;
			btn_linedel.Enabled = false;
			btn_yc_adp.Enabled = false;
			btn_bf_plan.Enabled = false;
			chk_g_adt.Checked = false;
			
			dat_fr.Text = "";
			dat_to.Text = "";
			lb_fr.Text = "( )";
			lb_to.Text = "( )";
			srLabel4.Text = "[마감체크]";
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
			SetButtonEnable("0000");
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

        #region 1 Form

        private void duty3010_Load(object sender, EventArgs e)
        {
			dat_yymm.DateTime = DateTime.Now;
			sl_dept.EditValue = null;
			sl_gnmu.EditValue = null;
		}

        private void duty3010_Shown(object sender, EventArgs e)
        {			
            df.GetMSTUSER_CHKDatas(ds);  //부서관리여부
            if (ds.Tables["MSTUSER_CHK"].Rows.Count > 0)
                admin_lv = clib.TextToInt(ds.Tables["MSTUSER_CHK"].Rows[0]["EMBSADGB"].ToString()); //권한레벨

            if (SilkRoad.Config.ACConfig.G_MSYN == "1" || SilkRoad.Config.SRConfig.USID == "SAMIL") //부서설정은 관리자만 (23.08.22 조수진 요청)
                btn_info.Enabled = true;

            if (admin_lv < 5)
			{
				if (SilkRoad.Config.SRConfig.USID != "SAMIL")
				{
					gridBand2.Visible = false;
					gridBand3.Visible = false;
				}
			}

			if (SilkRoad.Config.ACConfig.G_MSYN == "1" || SilkRoad.Config.SRConfig.USID == "SAMIL" || admin_lv > 2)
			{
				admin_lv = 3;
                p_dpcd = "%";
                lb_power.Text = "전체관리 권한";
				//btn_off_gw.Visible = true;
			}
            else if (admin_lv == 1)
            {
                p_dpcd = SilkRoad.Config.SRConfig.US_DPCD == null ? null : SilkRoad.Config.SRConfig.US_DPCD.Trim();
                lb_power.Text = "부서관리 권한";
				cmb_type.SelectedIndex = 3;
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
				cmb_type.SelectedIndex = 0;
			}
            else
            {
                p_dpcd = SilkRoad.Config.SRConfig.US_DPCD == null ? null : SilkRoad.Config.SRConfig.US_DPCD.Trim();
                lb_power.Text = "관리권한 없음";
            }
			SetCancel();
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
		//환경설정
		private void btn_setting_Click(object sender, EventArgs e)
		{
			if (sl_dept.EditValue == null)
			{
				MessageBox.Show("부서를 선택하세요!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				sl_dept.Focus();
			}
			else if (p_dpcd == "%" || p_dpcd == sl_dept.EditValue.ToString())
			{
				string dept = sl_dept.EditValue.ToString();
				string dept_nm = ds.Tables["3010_DANGDEPT"].Select("CODE = '" + dept + "'")[0]["DEPT_NM"].ToString();
				duty3015 duty3015 = new duty3015(dept, dept_nm);
				duty3015.ShowDialog();
			}
			else
			{
				MessageBox.Show("해당 부서의 권한이 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//수당결재
		private void btn_off_gw_Click(object sender, EventArgs e)
		{
			END_CHK();
			if (ends_yn == "Y")
			{
				MessageBox.Show("최종마감되어 저장할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else if (cmb_type.SelectedIndex == 0) //OFF,N 수당결재
			{
				if (admin_lv > 1)
				{
					duty3012 duty3012 = new duty3012(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), MousePosition.X, MousePosition.Y);
					duty3012.ShowDialog();
				}
				else
					MessageBox.Show("권한이 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else if (cmb_type.SelectedIndex == 1) //밤근무 수당결재
			{
				if (admin_lv > 1)
				{
					duty3013 duty3013 = new duty3013(1, clib.DateToText(dat_yymm.DateTime).Substring(0, 6), MousePosition.X, MousePosition.Y);
					duty3013.ShowDialog();
				}
				else
					MessageBox.Show("권한이 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else if (cmb_type.SelectedIndex == 2) //간호간병 수당결재
			{
				if (admin_lv > 1)
				{
					duty3013 duty3013 = new duty3013(2, clib.DateToText(dat_yymm.DateTime).Substring(0, 6), MousePosition.X, MousePosition.Y);
					duty3013.ShowDialog();
				}
				else
					MessageBox.Show("권한이 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else if (cmb_type.SelectedIndex == 3) //근무표 결재
			{
				if (sl_dept.EditValue == null)
				{
					MessageBox.Show("부서가 선택되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_dept.Focus();
				}
				else if (sl_dept.EditValue.ToString().Substring(0, 1) == "A")
				{
					MessageBox.Show("해당부서는 상신할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				else
				{
					df.Get2060_GW_CHKDatas("6", clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
					if (ds.Tables["2060_GW_CHK"].Rows.Count > 0)
					{
						MessageBox.Show("이미 결재상신된 부서입니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						duty3014 duty3014 = new duty3014(ds.Tables["SEARCH_PLAN"], clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds.Tables["3010_DANGDEPT"].Select("CODE = '" + sl_dept.EditValue.ToString() + "'")[0]["DEPT_NM"].ToString(), MousePosition.X, MousePosition.Y);
						duty3014.ShowDialog();
					}
				}
			}
			Refresh_Click();
		}
		//연차휴가 가져오기
		private void btn_yc_adp_Click(object sender, EventArgs e)
		{
			if (cmb_btype.SelectedIndex == 0)
			{
				df.Get3010_SEARCH_HYDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
				if (ds.Tables["3010_SEARCH_HY"].Rows.Count > 0)
				{
					if (ds.Tables["SEARCH_PLAN"] != null)
					{
						Cursor = Cursors.WaitCursor;
						grd1.DataSource = null;
						for (int i = 0; i < ds.Tables["3010_SEARCH_HY"].Rows.Count; i++)
						{
							DataRow drow = ds.Tables["3010_SEARCH_HY"].Rows[i];
							if (ds.Tables["SEARCH_PLAN"].Select("SAWON_NO = '" + drow["SABN"].ToString() + "'").Length > 0)
							{
								DataRow trow = ds.Tables["SEARCH_PLAN"].Select("SAWON_NO = '" + drow["SABN"].ToString() + "'")[0];
								trow["D" + drow["DD"].ToString()] = drow["GNMU"].ToString();
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
						grd1.DataSource = ds.Tables["SEARCH_PLAN"];
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
					if (ds.Tables["SEARCH_PLAN"] != null)
					{
						Cursor = Cursors.WaitCursor;
						for (int i = 0; i < ds.Tables["3010_S_TS"].Rows.Count; i++)
						{
							DataRow drow = ds.Tables["3010_S_TS"].Rows[i];
							int tsdt = clib.TextToInt(drow["TSDT"].ToString());
							if (ds.Tables["SEARCH_PLAN"].Select("SAWON_NO = '" + drow["EMBSSABN"].ToString() + "'").Length > 0 && tsdt > 0)
							{
								DataRow trow = ds.Tables["SEARCH_PLAN"].Select("SAWON_NO = '" + drow["EMBSSABN"].ToString() + "'")[0];
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
			if (srTabControl1.Visible == true)
			{
				srTabControl1.Visible = false;
				btn_expand.Image = DUTY1000.Properties.Resources.확대;
			}
			else
			{
				srTabControl1.Visible = true;
				btn_expand.Image = DUTY1000.Properties.Resources.축소;
			}
		}
		//근무부서설정
		private void btn_info_Click(object sender, EventArgs e)
		{
			duty3011 duty3011 = new duty3011();
			duty3011.ShowDialog();
			
			sl_dept.EditValue = null;
			sl_nurs.EditValue = null;
			SetCancel();

			Refresh_Click();
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

				sl_nurs.Enabled = true;
				btn_lineadd.Enabled = true;
				btn_linedel.Enabled = true;
				btn_bf_plan.Enabled = true;
				btn_yc_adp.Enabled = true;
				df.Get3010_SEARCH_NURSDatas(ds);//clib.DateToText(dat_yymm.DateTime.AddMonths(-1)).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
				sl_nurs.Properties.DataSource = ds.Tables["3010_SEARCH_NURS"];
				sl_nurs.EditValue = null;
				//
				if (sl_dept.EditValue.ToString() == "0100" || sl_dept.EditValue.ToString() == "0200")
					bandedGridColumn7.Caption = "간호간병";
				else
					bandedGridColumn7.Caption = "밤근무";

				s_yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 6);
				df.GetSEARCH_HOLIDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), ds);
				//1.기준년월에 따른 하단 일자컬럼header 일자, 요일 설정
				int lastday = clib.TextToInt(clib.DateToText(clib.TextToDateLast(clib.DateToText(dat_yymm.DateTime))).Substring(6, 2));

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
				df.Get3010_SEARCH_OREQDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
				df.Get3010_SEARCH_KTDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
				//근무유형 조회
				df.Get3010_SEARCH_GNMUDatas(ds);

				df.Get3010_SEARCH_CLOSDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
				if (ds.Tables["3010_SEARCH_CLOS"].Rows.Count > 0) //마감일이 저장되어 있으면
				{
					DataRow irow = ds.Tables["3010_SEARCH_CLOS"].Rows[0];
					dat_fr.Text = irow["POS_FRDT"].ToString();
					dat_to.Text = irow["POS_TODT"].ToString();
					lb_fr.Text = "(" + clib.WeekDay(clib.TextToDate(irow["POS_FRDT"].ToString())) + ")";
					lb_to.Text = "(" + clib.WeekDay(clib.TextToDate(irow["POS_TODT"].ToString())) + ")";
					if (clib.WeekDay(clib.TextToDate(irow["POS_FRDT"].ToString())) == "토")
						lb_fr.ForeColor = Color.Blue;
					else if (clib.WeekDay(clib.TextToDate(irow["POS_FRDT"].ToString())) == "일")
						lb_fr.ForeColor = Color.Red;
					else
						lb_fr.ForeColor = Color.Black;

					if (clib.WeekDay(clib.TextToDate(irow["POS_TODT"].ToString())) == "토")
						lb_to.ForeColor = Color.Blue;
					else if (clib.WeekDay(clib.TextToDate(irow["POS_TODT"].ToString())) == "일")
						lb_to.ForeColor = Color.Red;
					else
						lb_to.ForeColor = Color.Black;

					srLabel4.Text = irow["CLOSE_YN"].ToString() == "Y" ? "[신청마감]" : "[신청중]";
				}
				else
				{
					dat_fr.Text = "";
					dat_to.Text = "";
					lb_fr.Text = "( )";
					lb_to.Text = "( )";
					srLabel4.Text = "[마감체크]";
				}
				#endregion

				df.GetDUTY_TRSPLANDatas(2, clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
				if (ds.Tables["SEARCH_PLAN"].Rows.Count == 0)
				{
					df.GetSEARCH_PLANDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);

					if (admin_lv == 2)  //부서장일때 관리부서인지 체크
					{
						df.GetCHK_DEPTDatas(SilkRoad.Config.SRConfig.USID, sl_dept.EditValue.ToString(), ds);
						if (ds.Tables["CHK_DEPT"].Rows.Count > 0)
							SetButtonEnable("1011");
						else
							SetButtonEnable("0011");
					}
					else
					{
						if (p_dpcd == "%" || p_dpcd == sl_dept.EditValue.ToString())
							SetButtonEnable("1011");
						else
							SetButtonEnable("0011");
                    }

                    //신규일때 추가 (24.02.02)
                    for (int i = 0; i < ds.Tables["SEARCH_PLAN"].Rows.Count; i++)
                    {
                        month_calc(ds.Tables["SEARCH_PLAN"].Rows[i]["SAWON_NO"].ToString());
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
							SetButtonEnable("1011");
						else
							SetButtonEnable("0011");
					}
					else
					{
						if (p_dpcd == "%" || p_dpcd == sl_dept.EditValue.ToString())
							SetButtonEnable("1111");
						else
							SetButtonEnable("0011");
					}					

					if (grdv_dept.GetFocusedRowCellValue("GW_STAT").ToString() != "" || sl_dept.EditValue.ToString().Substring(0, 1) == "A")
						SetButtonEnable("0011");
				}

				for (int i = 0; i < ds.Tables["SEARCH_PLAN"].Rows.Count; i++)
				{
					for (int k = 1; k <= lastday; k++)
					{
						DataRow crow = ds.Tables["SEARCH_PLAN"].Rows[i];
						if (ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + crow["D" + k.ToString().PadLeft(2, '0')].ToString() + "'").Length > 0)
						{
							crow["D" + k.ToString().PadLeft(2, '0') + "_NM"] = ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + crow["D" + k.ToString().PadLeft(2, '0')].ToString() + "'")[0]["G_SNM"].ToString();
						}
					}
				}

				df.GetSUM_PLANDatas(ds);
				grd2.DataSource = ds.Tables["SUM_PLAN"];
				//for (int i = 0; i < ds.Tables["SEARCH_PLAN"].Rows.Count; i++)
				//{
				//	month_calc(ds.Tables["SEARCH_PLAN"].Rows[i]["SAWON_NO"].ToString());
				//}
				sum_month_calc();
				grd1.DataSource = ds.Tables["SEARCH_PLAN"];

				grdv1.Focus();
                Cursor = Cursors.Default;
            }
		}

        //저장버튼
        private void btn_save_Click(object sender, EventArgs e)
        {
            //TRSPLAN 에 확정내역 저장
            if (isNoError_um(2))
            {
                Cursor = Cursors.WaitCursor;
                int outVal = 0;
                try
                {
                    // 1. 해당년월에 사번의trsplan 가져오기
                    df.GetDUTY_TRSPLANDatas(1, s_yymm, sl_dept.EditValue.ToString(), ds);

					// 2.그리드한바퀴 글자들 합해서 넣기
					DataRow nrow = null;
					int sq = 1;

					foreach (DataRow drow in ds.Tables["SEARCH_PLAN"].Rows)
					{
						if (drow.RowState != DataRowState.Deleted)
						{
							if (ds.Tables["DUTY_TRSPLAN"].Select("SAWON_NO = '" + drow["SAWON_NO"] + "'").Length > 0)
							{
								nrow = ds.Tables["DUTY_TRSPLAN"].Select("SAWON_NO = '" + drow["SAWON_NO"] + "'")[0];
								nrow["UPDT"] = gd.GetNow();
								nrow["USID"] = SilkRoad.Config.SRConfig.USID;
								nrow["PSTY"] = "U";
							}
							else
							{
								nrow = ds.Tables["DUTY_TRSPLAN"].NewRow();
								nrow["SAWON_NO"] = drow["SAWON_NO"].ToString();
								nrow["PLANYYMM"] = s_yymm;
								nrow["DEPTCODE"] = sl_dept.EditValue.ToString();
								nrow["INDT"] = gd.GetNow();
								nrow["UPDT"] = "";
								nrow["USID"] = SilkRoad.Config.SRConfig.USID;
								nrow["PSTY"] = "A";
								ds.Tables["DUTY_TRSPLAN"].Rows.Add(nrow);
							}
							nrow["PLAN_SQ"] = sq;
							nrow["REMARK"] = drow["REMARK"].ToString().Trim();
							nrow["BF_NIGHT"] = clib.TextToInt(drow["BF_NIGHT"].ToString());
							nrow["BF_OFF"] = clib.TextToDecimal(drow["BF_OFF"].ToString());
							nrow["SHIFT_WORK"] = drow["SHIFT_WORK"].ToString();
							nrow["MAX_NCNT"] = clib.TextToInt(drow["MAX_NCNT"].ToString());
							nrow["ALLOW_OFF"] = clib.TextToDecimal(drow["ALLOW_OFF"].ToString());
							nrow["REMAIN_NIGHT"] = clib.TextToInt(drow["REMAIN_NIGHT"].ToString());
							nrow["REMAIN_OFF"] = clib.TextToDecimal(drow["REMAIN_OFF"].ToString());
							nrow["MM_CNT1"] = clib.TextToDecimal(drow["MM_CNT1"].ToString());
							nrow["MM_CNT2"] = clib.TextToDecimal(drow["MM_CNT2"].ToString());
							nrow["MM_CNT3"] = clib.TextToDecimal(drow["MM_CNT3"].ToString());
							nrow["MM_CNT4"] = clib.TextToDecimal(drow["MM_CNT4"].ToString());
							nrow["MM_CNT5"] = clib.TextToDecimal(drow["MM_CNT5"].ToString());
							nrow["MM_CNT6"] = 0;
							for (int j = 1; j <= 31; j++)
							{
								nrow["D" + j.ToString().PadLeft(2, '0')] = drow["D" + j.ToString().PadLeft(2, '0')].ToString();
							}
							nrow["EDU_CNT1"] = clib.TextToDecimal(drow["EDU_CNT1"].ToString());
							nrow["EDU_CNT2"] = clib.TextToDecimal(drow["EDU_CNT2"].ToString());
							nrow["HG_CNT1"] = clib.TextToDecimal(drow["HG_CNT1"].ToString());
							nrow["HG_CNT2"] = clib.TextToDecimal(drow["HG_CNT2"].ToString());
							sq++;
						}
						else
						{
							if (ds.Tables["DUTY_TRSPLAN"].Select("SAWON_NO = '" + drow["SAWON_NO", DataRowVersion.Original].ToString().Trim() + "' ").Length > 0)
								ds.Tables["DUTY_TRSPLAN"].Select("SAWON_NO = '" + drow["SAWON_NO", DataRowVersion.Original].ToString().Trim() + "' ")[0].Delete();
						}
					}

                    string[] tableNames = new string[] { "DUTY_TRSPLAN" };
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
						df.GetDUTY_TRSPLANDatas(1, s_yymm, sl_dept.EditValue.ToString(), ds);
						for (int i = 0; i < ds.Tables["DUTY_TRSPLAN"].Rows.Count; i++)
						{
							ds.Tables["DUTY_TRSPLAN"].Rows[i].Delete();
						}

						string[] tableNames = new string[] { "DUTY_TRSPLAN" };
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
			rpt_3010 rpt = new rpt_3010(clib.DateToText(dat_yymm.DateTime), sl_dept.Text.ToString(), ds.Tables["SEARCH_HOLI"]); // txt_yymm.Text, sl_part2.Text, ds.Tables["SEARCH_PLAN"]);
			rpt.DataSource = ds.Tables["SEARCH_PLAN"];
			rpt.writeReport(ds);
			if (stat == 1)
				rpt.ShowPreview();
			else if (stat == 2)
				rpt.Print();
		}
		//라인추가
		private void btn_lineadd_Click(object sender, EventArgs e)
		{
			if (sl_nurs.EditValue == null)
			{
				MessageBox.Show("간호사를 선택하세요!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				sl_nurs.Focus();
			}
			else if (ds.Tables["SEARCH_PLAN"].Select("SAWON_NO = '" + sl_nurs.EditValue.ToString() + "'").Length > 0)
			{
				MessageBox.Show("이미 근무신청된 간호사입니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				sl_nurs.Focus();
			}
			else
			{
				int max_sq = clib.TextToInt(ds.Tables["SEARCH_PLAN"].Compute("MAX(PLAN_SQ)", null).ToString());
				DataRow trow = ds.Tables["3010_SEARCH_NURS"].Select("CODE = '" + sl_nurs.EditValue.ToString() + "'")[0];
				DataRow nrow = ds.Tables["SEARCH_PLAN"].NewRow();
				nrow["SAWON_NO"] = sl_nurs.EditValue.ToString();
				nrow["SAWON_NM"] = trow["NAME"].ToString();
				nrow["PLAN_SQ"] = max_sq + 1;
				//nrow["EXP_LV_NM"] = trow["EXP_LV_NM"].ToString();
				//nrow["BF_OFF"] = trow["BF_OFF"].ToString();
				nrow["ALLOW_OFF"] = clib.TextToDecimal(trow["ALLOW_OFF"].ToString());
				nrow["MAX_NCNT"] = clib.TextToInt(trow["MAX_NCNT"].ToString());
				ds.Tables["SEARCH_PLAN"].Rows.Add(nrow);
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

				for (int i = 0; i < ds.Tables["SEARCH_PLAN"].Rows.Count; i++)
				{
					DataRow crow = ds.Tables["SEARCH_PLAN"].Rows[i];
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
		//전월근무조회
		private void btn_bf_plan_Click(object sender, EventArgs e)
		{
			df.GetSEARCH_BF_HOLIDatas(clib.DateToText(dat_yymm.DateTime.AddMonths(-1)).Substring(0, 6), ds);
			df.GetDUTY_BF_TRSPLANDatas(clib.DateToText(dat_yymm.DateTime.AddMonths(-1)).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
			//1.기준년월에 따른 하단 일자컬럼header 일자, 요일 설정
			int lastday = clib.TextToInt(clib.DateToText(clib.TextToDateLast(clib.DateToText(dat_yymm.DateTime.AddMonths(-1)))).Substring(6, 2));
			for (int i = 0; i < ds.Tables["SEARCH_BF_PLAN"].Rows.Count; i++)
			{
				for (int k = 1; k <= lastday; k++)
				{
					DataRow crow = ds.Tables["SEARCH_BF_PLAN"].Rows[i];
					if (ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + crow["D" + k.ToString().PadLeft(2, '0')].ToString() + "'").Length > 0)
					{
						crow["D" + k.ToString().PadLeft(2, '0') + "_NM"] = ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + crow["D" + k.ToString().PadLeft(2, '0')].ToString() + "'")[0]["G_SNM"].ToString();
					}
				}
			}

			rpt_3010 rpt = new rpt_3010(clib.DateToText(dat_yymm.DateTime.AddMonths(-1)), sl_dept.Text.ToString(), ds.Tables["SEARCH_BF_HOLI"]);
			rpt.DataSource = ds.Tables["SEARCH_BF_PLAN"];
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
			df.Get3010_DANGDEPTDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), ds);
			sl_dept.Properties.DataSource = ds.Tables["3010_DANGDEPT"];
			grd_dept.DataSource = ds.Tables["3010_DANGDEPT"];
			if (grid_dept != "")
				grdv_dept.FocusedRowHandle = grdv_dept.LocateByValue("CODE", grid_dept, null);

			df.GetGNMU_LISTDatas(ds);
			grd_lk_gnmu.DataSource = ds.Tables["GNMU_LIST"];
			grd_sl_gnmu.DataSource = ds.Tables["GNMU_LIST"];
			//sl_gnmu.Properties.DataSource = ds.Tables["GNMU_LIST"];
			
			df.Get3010_GNMUDatas(ds);
			sl_gnmu.Properties.DataSource = ds.Tables["3010_GNMU"];

			if (sl_dept.EditValue != null)
			{
				df.Get3010_SEARCH_NURSDatas(ds);//clib.DateToText(dat_yymm.DateTime.AddMonths(-1)).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
				sl_nurs.Properties.DataSource = ds.Tables["3010_SEARCH_NURS"];
			}
        }

		#endregion

		#region 3 EVENT
		
		//메뉴 활성화시
		private void duty3010_Activated(object sender, EventArgs e)
		{
			END_CHK();

			if (btn_proc.Enabled == false)
			{
				df.Get3010_SEARCH_CLOSDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
				if (ds.Tables["3010_SEARCH_CLOS"].Rows.Count > 0) //마감일이 저장되어 있으면
				{
					DataRow irow = ds.Tables["3010_SEARCH_CLOS"].Rows[0];
					dat_fr.Text = irow["POS_FRDT"].ToString();
					dat_to.Text = irow["POS_TODT"].ToString();
					lb_fr.Text = "(" + clib.WeekDay(clib.TextToDate(irow["POS_FRDT"].ToString())) + ")";
					lb_to.Text = "(" + clib.WeekDay(clib.TextToDate(irow["POS_TODT"].ToString())) + ")";
					if (clib.WeekDay(clib.TextToDate(irow["POS_FRDT"].ToString())) == "토")
						lb_fr.ForeColor = Color.Blue;
					else if (clib.WeekDay(clib.TextToDate(irow["POS_FRDT"].ToString())) == "일")
						lb_fr.ForeColor = Color.Red;
					else
						lb_fr.ForeColor = Color.Black;

					if (clib.WeekDay(clib.TextToDate(irow["POS_TODT"].ToString())) == "토")
						lb_to.ForeColor = Color.Blue;
					else if (clib.WeekDay(clib.TextToDate(irow["POS_TODT"].ToString())) == "일")
						lb_to.ForeColor = Color.Red;
					else
						lb_to.ForeColor = Color.Black;

					srLabel4.Text = irow["CLOSE_YN"].ToString() == "Y" ? "[신청마감]" : "[신청중]";
				}
				else
				{
					dat_fr.Text = "";
					dat_to.Text = "";
					lb_fr.Text = "( )";
					lb_to.Text = "( )";
					srLabel4.Text = "[마감체크]";
				}
			}
		}
		private void duty3010_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)   //취소
            {
                btn_clear.PerformClick();
            }
        }
		
		//조회년월 변경시 부서 다시 불러오기
		private void dat_yymm_EditValueChanged(object sender, EventArgs e)
		{
			//Refresh_Click();
            SetCancel();
		}
		//부서 더블클릭시
		private void grdv_dept_DoubleClick(object sender, EventArgs e)
		{			
			grid_dept = grdv_dept.GetFocusedRowCellValue("CODE").ToString();
            SetCancel();
			sl_dept.EditValue = grid_dept;
			btn_proc.PerformClick();			
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

        private void grd_lk_gnmu_KeyDown(object sender, KeyEventArgs e)
		{
			DataRow drow = grdv1.GetFocusedDataRow();
			if (drow != null)
			{
				if (grdv1.FocusedColumn.Name.ToString().Substring(0, 10) == "grdcol_day")
				{
					if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.D || e.KeyCode == Keys.E || e.KeyCode == Keys.N || e.KeyCode == Keys.O || e.KeyCode == Keys.Y)
					{
						if (e.KeyCode == Keys.D)   //DAY
							drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2)] = "01";
						else if (e.KeyCode == Keys.E)   //EVENING
							drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2)] = "02";
						else if (e.KeyCode == Keys.N)   //NIGHT
							drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2)] = "03";
						else if (e.KeyCode == Keys.O)   //OFF
							drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2)] = "11";
						else if (e.KeyCode == Keys.Y)   //연차
							drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2)] = "12";
						else if (e.KeyCode == Keys.Delete)   //삭제
							drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2)] = "";
						
						e.SuppressKeyPress = true;
						month_calc(drow["SAWON_NO"].ToString());

						SendKeys.Send("{ENTER}");
					}
				}
			}
		}
		private void grd_sl_gnmu_KeyDown(object sender, KeyEventArgs e)
		{
			//DataRow drow = grdv1.GetFocusedDataRow();
			//if (drow != null)
			//{
			//	if (grdv1.FocusedColumn.Name.ToString().Substring(0, 10) == "grdcol_day")
			//	{
			//		if (e.KeyCode == Keys.D || e.KeyCode == Keys.E || e.KeyCode == Keys.N || e.KeyCode == Keys.O || e.KeyCode == Keys.Y)
			//		{
			//			if (e.KeyCode == Keys.D)   //DAY
			//				drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2)] = "01";
			//			if (e.KeyCode == Keys.E)   //EVENING
			//				drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2)] = "02";
			//			if (e.KeyCode == Keys.N)   //NIGHT
			//				drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2)] = "03";
			//			if (e.KeyCode == Keys.O)   //OFF
			//				drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2)] = "11";
			//			if (e.KeyCode == Keys.Y)   //연차
			//				drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2)] = "12";
			//			//SendKeys.Send("{ENTER}"); grd_sl_gnmu.
			//			//SilkRoad.UserControls.SRLookup2
			//			//if (SilkRoad.UserControls.SRLookup2 == !this.IsPopupOpen)
			//			//{
			//			//}
			//		}
			//	}
			//}
		}

		private void grdv_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
		{
            DataRow drow = grdv1.GetFocusedDataRow();
			if (drow != null)
			{
				if (grdv1.FocusedColumn.Name.ToString().PadRight(10, ' ').Substring(0, 10) == "grdcol_day")
				{
					#region 신청불가 근무신청내역 -> 삭제 21.10.22 (박상균 수정가능하도록 요청
					//string sabn = drow["SAWON_NO"].ToString().Trim();
					//string date = clib.DateToText(dat_yymm.DateTime).Substring(0, 6) + grdv1.FocusedColumn.Name.ToString().Substring(10, 2);
					//if (ds.Tables["3010_SEARCH_OREQ"].Select("SABN = '" + sabn + "' AND REQ_DATE = '" + date + "'").Length > 0)
					//{
					//	if (ds.Tables["3010_SEARCH_OREQ"].Select("SABN = '" + sabn + "' AND REQ_DATE = '" + date + "'")[0]["EDIT_YN"].ToString() == "1")
					//		e.Cancel = true;
					//}
					//else
					//{
					//	if (chk_g_adt.Checked == true)
					//	{
					//		drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2)] = sl_gnmu.EditValue;	
					//		if (ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + sl_gnmu.EditValue.ToString() + "'").Length > 0)							
					//			drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2) + "_NM"] = ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + sl_gnmu.EditValue.ToString() + "'")[0]["G_SNM"].ToString();

					//		e.Cancel = true;
					//		month_calc(drow["SAWON_NO"].ToString());
					//	}
					//}
					#endregion

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
							if (ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + sl_gnmu.EditValue.ToString() + "'").Length > 0)
								drow["D" + grdv1.FocusedColumn.Name.ToString().Substring(10, 2) + "_NM"] = ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + sl_gnmu.EditValue.ToString() + "'")[0]["G_SNM"].ToString();
						}
											
						month_calc(drow["SAWON_NO"].ToString());
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
						if (ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + e.Value.ToString() + "'").Length > 0)
							drow["D" + e.Column.Name.ToString().Substring(10, 2) + "_NM"] = ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + e.Value.ToString() + "'")[0]["G_SNM"].ToString();
					}
					month_calc(drow["SAWON_NO"].ToString());
				}
            }
        }
		
        //근무값별 색 주기 ★
        private void grdv_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.Name.StartsWith("grdcol_day"))
            {
				string stdt = clib.DateToText(DateTime.Now);
				string date = s_yymm + e.Column.Name.ToString().Substring(10, 2);
				string sabn = grdv1.GetDataRow(e.RowHandle)["SAWON_NO"].ToString().Trim();

				if (ds.Tables["3010_SEARCH_KT"].Select("SABN = '" + sabn + "' AND SLDT = '" + date + "'").Length > 0)  //출근시 체크
				{
					if (ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + e.CellValue + "'") != null && ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + e.CellValue + "'").Length > 0)
					{
						int g_type = clib.TextToInt(ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["G_TYPE"].ToString());
						if (g_type == 2 || g_type == 4 || g_type == 5 || g_type == 6) //근무인데 출근이면 근무색상
						{
							int colVAlue = clib.TextToInt(ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["G_COLOR"].ToString());
							e.Appearance.BackColor = Color.FromArgb(colVAlue);
							e.Appearance.ForeColor = Color.Black;
							//if (ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["G_RGB"].ToString() == "rgb(255,255,255)")
							//	e.Appearance.ForeColor = Color.Black;
						}
						else //근무가 아닌데 출근이면
						{
							if (g_type == 8 && clib.TextToDecimal(ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["YC_DAY"].ToString()) < 8) //반차or시간 출근이면
							{
								int colVAlue = clib.TextToInt(ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["G_COLOR"].ToString());
								e.Appearance.BackColor = Color.FromArgb(colVAlue);
							}
							else
								e.Appearance.BackColor = Color.Red;
						}
					}
				}
				else  //출근하지 않았을때 체크
				{
					if (ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + e.CellValue + "'") != null && ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + e.CellValue + "'").Length > 0)
					{
						int g_type = clib.TextToInt(ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["G_TYPE"].ToString());
						if (g_type == 2 || g_type == 4 || g_type == 5 || g_type == 6) //근무인데 출근하지 않으면 붉은색
						{
							if (clib.TextToDate(date) <= clib.TextToDate(stdt))  //현재일 이전일때 오류색상
								e.Appearance.BackColor = Color.Red;
							else  //현재일 이후는 근무색상
							{
								int colVAlue = clib.TextToInt(ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["G_COLOR"].ToString());
								e.Appearance.BackColor = Color.FromArgb(colVAlue);
								e.Appearance.ForeColor = Color.Black;
								//if (ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["G_RGB"].ToString() == "rgb(255,255,255)")
								//	e.Appearance.ForeColor = Color.Black;
							}
						}
						else  // 근무가 아니면서 출근하지 않았을땐 근무색상으로
						{
							int colVAlue = clib.TextToInt(ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["G_COLOR"].ToString());
							e.Appearance.BackColor = Color.FromArgb(colVAlue);
							e.Appearance.ForeColor = Color.Black;
							//if (ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + e.CellValue + "'")[0]["G_RGB"].ToString() == "rgb(255,255,255)")
							//	e.Appearance.ForeColor = Color.Black;
						}
					}
				}

				if (ds.Tables["3010_SEARCH_OREQ"].Select("SABN = '" + sabn + "' AND REQ_DATE = '" + date + "'").Length > 0)
				{
					DataRow crow = ds.Tables["3010_SEARCH_OREQ"].Select("SABN = '" + sabn + "' AND REQ_DATE = '" + date + "'")[0];
					if (crow["EDIT_YN"].ToString() == "1" && crow["REQ_TYPE"].ToString() == e.CellValue.ToString())
					{
						e.Appearance.ForeColor = Color.Black;
						e.Appearance.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
					}
				}				
			}
        }
		
		//로우클릭시 근태 표기
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

		#region 4 Drag & Drop

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
                    DataRow crow = ds.Tables["SEARCH_PLAN"].Select("PLAN_SQ = " + for_slsq + "")[0];
                    if (crow.RowState != DataRowState.Deleted)
                    {
                        crow["PLAN_SQ"] = clib.TextToInt(crow["PLAN_SQ"].ToString()) - 1;
                    }
                }
				
				ds.Tables["SEARCH_PLAN"].Select("SAWON_NO = '" + sabn + "'")[0]["PLAN_SQ"] = targetRow + 1;
				//ds.Tables["SEARCH_PLAN"].Select("SAWON_NO = " + sabn + "")[0]["PLAN_SQ"] = targetRow + 1;
            }
            else
            {
				int tg_slsq = targetRow < 0 ? 0 : clib.TextToInt(ds.Tables["SEARCH_PLAN"].Select("PLAN_SQ = " + (targetRow + 1))[0]["PLAN_SQ"].ToString());
				for (int i = 0; i < ds.Tables["SEARCH_PLAN"].Rows.Count; i++)
				{
					DataRow crow = ds.Tables["SEARCH_PLAN"].Rows[i];
					if (crow.RowState != DataRowState.Deleted)
					{
						if (clib.TextToInt(crow["PLAN_SQ"].ToString()) > tg_slsq && clib.TextToInt(crow["PLAN_SQ"].ToString()) <= sourceRow)
						{
							crow["PLAN_SQ"] = clib.TextToInt(crow["PLAN_SQ"].ToString()) + 1;
						}
					}
				}
				ds.Tables["SEARCH_PLAN"].Select("SAWON_NO = '" + sabn + "'")[0]["PLAN_SQ"] = targetRow < 0 ? 1 : targetRow + 2;
                //row["PLAN_SQ"] = targetRow + 2;
            }
			ds.Tables["SEARCH_PLAN"].DefaultView.Sort = "PLAN_SQ ASC";
			DataTable dt = ds.Tables["SEARCH_PLAN"].DefaultView.ToTable();
			dp.AddDatatable2Dataset("SEARCH_PLAN", dt, ref ds);
			grd1.DataSource = ds.Tables["SEARCH_PLAN"];
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

				df.Get3010_SEARCH_CLOSDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), sl_dept.EditValue.ToString(), ds);
				string chk_clos = "Y";
				if (ds.Tables["3010_SEARCH_CLOS"].Rows.Count > 0)
				{
					chk_clos = ds.Tables["3010_SEARCH_CLOS"].Rows[0]["CLOSE_YN"].ToString();
					srLabel4.Text = ds.Tables["3010_SEARCH_CLOS"].Rows[0]["CLOSE_YN"].ToString() == "Y" ? "[신청마감]" : "[신청중]";
				}
				else
				{
					chk_clos = "N";
					srLabel4.Text = "[마감체크]";
				}

				if (chk_clos == "Y")
                {
                    isError = true;
                }
                else
                {
                    MessageBox.Show("근무신청 마감 후 저장이 가능합니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
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
			btn_save.Enabled = arr.Substring(0, 1) == "1" ? true : false;
			btn_del.Enabled = arr.Substring(1, 1) == "1" ? true : false;
			btn_preview.Enabled = arr.Substring(2, 1) == "1" ? true : false;
			btn_print.Enabled = arr.Substring(3, 1) == "1" ? true : false;
		}

		private void month_calc(string sabn)
		{
			//근무유형 조회
			df.Get3010_SEARCH_GNMUDatas(ds);

			DataRow srow = ds.Tables["SEARCH_PLAN"].Select("SAWON_NO = '" + sabn + "'")[0];
			//1.기준년월에 따른 하단 일자컬럼header 일자, 요일 설정
			int lastday = clib.TextToInt(clib.DateToText(clib.TextToDateLast(clib.DateToText(dat_yymm.DateTime))).Substring(6, 2));
            //남은 필드 visible = false;
            double Day = 0, Eve = 0, Night = 0, Off = 0; //, yc = 0;
			decimal yc = 0, edu1 = 0, edu2 = 0, hg1 = 0, hg2 = 0;
			for (int k = 1; k <= lastday; k++)
			{
				if (ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + srow["D" + k.ToString().PadLeft(2, '0')].ToString() + "'").Length > 0)
				{
					DataRow trow = ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + srow["D" + k.ToString().PadLeft(2, '0')].ToString() + "'")[0];
					switch (trow["G_TYPE"].ToString())
					{
						case "2":
						case "4":
                            if (trow["G_CODE"].ToString() == "67")
                            {
                                Day += 0.5;
                                Off += 0.5;
                            }
                            else if (trow["G_CODE"].ToString() == "68")
                            {
                                Eve += 0.5;
                                Off += 0.5;
                            }
                            else
                                Day += 1;
							break;
						case "5":
                                Eve += 1;
							break;
						case "6":
							Night += 1;
							break;
						case "7":
							Off += 1;
							break;
						case "8":
							yc += clib.TextToDecimal(trow["YC_DAY"].ToString());
							//if (trow["G_CODE"].ToString() == "05")
							//	edu1 += 1;  //유급
							if (trow["G_CODE"].ToString() == "07" || trow["G_CODE"].ToString() == "79")
								edu2 += clib.TextToDecimal(trow["YC_DAY"].ToString());  //무급
							break;
						case "12":
                            yc += clib.TextToDecimal(trow["YC_DAY"].ToString());
                            if (trow["G_CODE"].ToString() == "05" || trow["G_CODE"].ToString() == "78")
                                edu1 += clib.TextToDecimal(trow["YC_DAY"].ToString());  //유급
                            else
                            {
                                df.Get3010_SEARCH_HUGADatas(sabn, clib.DateToText(dat_yymm.DateTime).Substring(0, 6) + k.ToString().PadLeft(2, '0'), srow["D" + k.ToString().PadLeft(2, '0')].ToString(), ds);
                                if (ds.Tables["3010_SEARCH_HUGA"].Rows.Count > 0)
                                {
                                    DataRow hrow = ds.Tables["3010_SEARCH_HUGA"].Rows[0];
                                    if (hrow["PAY_YN"].ToString() == "1")
                                        hg1 += 1;
                                    else if (hrow["PAY_YN"].ToString() == "2")
                                        hg2 += 1;
                                }
                            }
							break;
					}
				}
			}
			srow["MM_CNT1"] = Day;
			srow["MM_CNT2"] = Eve;
			srow["MM_CNT3"] = Night;
			srow["MM_CNT4"] = Off;
			srow["MM_CNT5"] = yc;

            srow["HG_CNT1"] = hg1;
            srow["HG_CNT2"] = hg2;
            srow["EDU_CNT1"] = edu1;
			srow["EDU_CNT2"] = edu2;
			//srow["Y_CNT1"] = clib.TextToInt(srow["YEAR_CNT1"].ToString()) + Day;
			//srow["Y_CNT2"] = clib.TextToInt(srow["YEAR_CNT2"].ToString()) + Eve;
			//srow["Y_CNT3"] = clib.TextToInt(srow["YEAR_CNT3"].ToString()) + Night;
			//srow["Y_CNT4"] = clib.TextToInt(srow["YEAR_CNT4"].ToString()) + Off;
			//srow["Y_CNT5"] = clib.TextToDecimal(srow["YEAR_CNT5"].ToString()) + yc;
			srow["REMAIN_OFF"] = 0;
			////환경설정 조회
			//df.Get3010_INFO01Datas(ds);
			//if (ds.Tables["INFO01"] != null) //.Rows.Count > 0)
			//{
			//	if (ds.Tables["INFO01"].Rows.Count > 0)
			//		if (ds.Tables["INFO01"].Rows[0]["OFF_NEXT_YN"].ToString() == "2")
			//			srow["REMAIN_OFF"] = clib.TextToInt(srow["BF_OFF"].ToString()) + clib.TextToInt(srow["MASTER_OFF"].ToString()) - Off;
			//}
			grdv1.UpdateTotalSummary();
			sum_month_calc();
		}
		private void sum_month_calc()
		{
            double T_Day = 0, T_Eve = 0, T_Night = 0, T_yc = 0;
			int lastday = clib.TextToInt(clib.DateToText(clib.TextToDateLast(clib.DateToText(dat_yymm.DateTime))).Substring(6, 2));
			for (int d = 1; d <= lastday; d++)
			{
				string dd = d.ToString().PadLeft(2, '0');
				double Day = 0, Eve = 0, Night = 0;
				for (int i = 0; i < ds.Tables["SEARCH_PLAN"].Rows.Count; i++)
				{
					DataRow drow = ds.Tables["SEARCH_PLAN"].Rows[i];
					if (drow.RowState != DataRowState.Deleted)
					{
						if (ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + drow["D" + dd].ToString() + "'").Length > 0)
						{
							DataRow trow = ds.Tables["3010_SEARCH_GNMU"].Select("G_CODE = '" + drow["D" + dd].ToString() + "'")[0];
							switch (trow["G_TYPE"].ToString())
							{
								case "2":
								case "4":
                                    if (trow["G_CODE"].ToString() == "67")
                                    {
                                        Day += 0.5;
                                        T_Day += 0.5;
                                    }
                                    else if (trow["G_CODE"].ToString() == "68")
                                    {
                                        Eve += 0.5;
                                        T_Eve += 0.5;
                                    }
                                    else
                                    {
                                        Day += 1;
                                        T_Day += 1;
                                    }
									break;
								case "5":                                    
                                    Eve += 1;
                                    T_Eve += 1;                                    
									break;
								case "6":
									Night += 1;
									T_Night += 1;
									break;
							}
						}
					}
				}

                if (ds.Tables["SUM_PLAN"] != null)
                {
                    if (ds.Tables["SUM_PLAN"].Select("G_TYPE = 4").Length > 0)
                    {
                        ds.Tables["SUM_PLAN"].Select("G_TYPE = 4")[0]["D" + dd] = Day;
                        ds.Tables["SUM_PLAN"].Select("G_TYPE = 4")[0]["MM_CNT1"] = T_Day;
                    }
                    if (ds.Tables["SUM_PLAN"].Select("G_TYPE = 5").Length > 0)
                    {
                        ds.Tables["SUM_PLAN"].Select("G_TYPE = 5")[0]["D" + dd] = Eve;
                        ds.Tables["SUM_PLAN"].Select("G_TYPE = 5")[0]["MM_CNT2"] = T_Eve;
                    }
                    if (ds.Tables["SUM_PLAN"].Select("G_TYPE = 6").Length > 0)
                    {
                        ds.Tables["SUM_PLAN"].Select("G_TYPE = 6")[0]["D" + dd] = Night;
                        ds.Tables["SUM_PLAN"].Select("G_TYPE = 6")[0]["MM_CNT3"] = T_Night;
                    }
                }
				//if (ds.Tables["SUM_PLAN"].Select("G_TYPE = 8").Length > 0)
				//{
				//	ds.Tables["SUM_PLAN"].Select("G_TYPE = 8")[0]["D" + dd] = yc;
				//	ds.Tables["SUM_PLAN"].Select("G_TYPE = 8")[0]["MM_CNT5"] = T_yc;
				//}
			}
		}

        #endregion

    }
}
