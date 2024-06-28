using System;
using System.Data;
using System.Windows.Forms;
using SilkRoad.Common;
using System.Drawing;

namespace DUTY1000
{
    public partial class duty2062 : SilkRoad.Form.Base.FormX
    {
        CommonLibrary clib = new CommonLibrary();
        static DataProcessing dp = new DataProcessing();

        ClearNEnableControls cec = new ClearNEnableControls();
        public DataSet ds = new DataSet();
        DataProcFunc df = new DataProcFunc();
        SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();
		string yymm = "";
		string dept = "";
		string dept_nm = "";

        public duty2062(DataTable _dt, string _yymm, string _dept, string _dept_nm, int point_x, int point_y)
        {
            InitializeComponent();
			
            if (point_x + this.Width > System.Windows.Forms.SystemInformation.VirtualScreen.Width)            
                point_x = System.Windows.Forms.SystemInformation.VirtualScreen.Width - this.Width;
            if (point_y + this.Height + 200 > System.Windows.Forms.SystemInformation.VirtualScreen.Height)
                point_y = System.Windows.Forms.SystemInformation.VirtualScreen.Height - this.Height - 200;
            this.Location = new Point(point_x, point_y);
			yymm = _yymm;
			dept = _dept;
			dept_nm = _dept_nm;
        }

        #region 0. Initialization

        /// <summary>
        ///컨트롤 초기화 및 활성,비활성 설정
        /// </summary>
        /// <param name="enable"></param>
        private void SetCancel()
        {
			grd1.DataSource = null;
            //SetButtonEnable("100");
        }

        #endregion

        #region 1 Form

        private void duty2062_Load(object sender, EventArgs e)
        {			
			df.Get8030_SEARCH_EMBSDatas("%", ds);
			sl_embs.Properties.DataSource = ds.Tables["8030_SEARCH_EMBS"];

			if (ds.Tables["8030_SEARCH_EMBS"].Select("CODE = '" + SilkRoad.Config.SRConfig.USID + "'").Length > 0)
				sl_embs.EditValue = SilkRoad.Config.SRConfig.USID;
			else
				sl_embs.EditValue = null;
			
			sl_line1.EditValue = null;
			sl_line2.EditValue = null;

			if (sl_embs.EditValue != null)
            {
                DataRow drow = ds.Tables["8030_SEARCH_EMBS"].Select("CODE = '" + sl_embs.EditValue.ToString() + "'")[0];
                string adgb = drow["EMBSADGB"].ToString();
                if ("1200,2600".Contains(drow["EMBSDPCD"].ToString()))
                    adgb = "1";
                ADGB_STAT(adgb);

				//종검실일때만 23.03.10
				if (ds.Tables["8030_SEARCH_EMBS"].Select("CODE = '" + sl_embs.EditValue.ToString() + "'")[0]["EMBSDPCD"].ToString() == "2500")
				{
					chk_line.Checked = true;
					if (ds.Tables["GW_LINE1"].Select("CODE= '10001'").Length > 0)
						sl_line1.EditValue = "10001"; //이영철 고정
				}
			}
			else
			{
				string embs = sl_embs.EditValue == null ? "%" : sl_embs.EditValue.ToString();
				df.GetGW_LINE1Datas(embs, "'2'", ds);
				sl_line1.Properties.DataSource = ds.Tables["GW_LINE1"];
				df.GetGW_LINE2Datas(embs, "'5','6'", ds);
				sl_line2.Properties.DataSource = ds.Tables["GW_LINE2"];
			}

			dat_jsmm.DateTime = clib.TextToDate(yymm + "01");
			gr_detail.Text = yymm.Substring(2, 2) + "년 " + yymm.Substring(4, 2) + "월 " + dept_nm + " 근무현황표";
			df.Get2060_DANG_GNMUDatas(ds);
			grd_sl_gnmu.DataSource = ds.Tables["2060_DANG_GNMU"];
			Proc();
        }

        #endregion

        #region 2 Button

        //처리
		private void btn_search_Click(object sender, EventArgs e)
		{
			Proc();
		}

        private void Proc()
        {
			#region 요일에따른 헤더 설정
			df.GetSEARCH_HOLIDatas(yymm, ds);
			int lastday = clib.TextToInt(clib.DateToText(clib.TextToDateLast(yymm+"01")).Substring(6, 2));

			for (int i = 1; i <= lastday; i++)
			{
				DateTime day = clib.TextToDate(yymm + "01").AddDays(i - 1);
				grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].Caption = i.ToString() + "\r\n" + clib.WeekDay(day); //일+요일. 한칸 내려서 보이도록. 엔터마냥. 
				if (clib.WeekDay(day) == "토")				
					grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Blue;				
				else if (clib.WeekDay(day) == "일")				
					grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Red;				
				else				
					grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Black;				

				if (ds.Tables["SEARCH_HOLI"].Select("H_DATE = '" + clib.DateToText(day) + "'").Length > 0)
				{
					grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Red;
					grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.Font = new Font(FontFamily.GenericSansSerif, 9F, FontStyle.Underline);
					grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].ToolTip = ds.Tables["SEARCH_HOLI"].Select("H_DATE = '" + clib.DateToText(day) + "'")[0]["H_NAME"].ToString();
				}
			}

			//남은 필드 visible = false;
			for (int k = 1; k < 32; k++)
			{
				if (k > lastday)
					grdv1.Columns["D" + k.ToString().PadLeft(2, '0')].Visible = false;
				else
					grdv1.Columns["D" + k.ToString().PadLeft(2, '0')].Visible = true;
			}
			#endregion

			df.GetDUTY_TRSDANGDatas(3, yymm, dept, ds);
			grd1.DataSource = ds.Tables["GW_SEARCH_DANG_PLAN"];
        }

        //저장버튼
        private void btn_save_Click(object sender, EventArgs e)
        {
			if (clib.DateToText(dat_jsmm.DateTime) == "")
			{
				MessageBox.Show("정산년월이 입력되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else if (ds.Tables["GW_SEARCH_DANG_PLAN"].Select("CHK='1'").Length == 0)
			{
				MessageBox.Show("선택된 직원이 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				int stat = 0;
				decimal doc_no = 0;
				if (isNoError_um(1))
				{
					Cursor = Cursors.WaitCursor;
					int outVal = 0;
					try
					{
						#region 문서 헤더
						df.GetDUTY_GWDOCDatas(ds);
						DataRow hrow = ds.Tables["DUTY_GWDOC"].NewRow();
						doc_no = df.GetGWDOC_NODatas(ds);

						hrow["DOC_NO"] = doc_no;
						hrow["DOC_GUBN"] = 5;  //1.콜 2.OT 3.OFF,N 수당 4.밤근무 5.당직근무표 6.간호사근무표
						hrow["DOC_DATE"] = yymm;
						hrow["DOC_JSMM"] = clib.DateToText(dat_jsmm.DateTime).Substring(0, 6);
						hrow["GW_TITLE"] = yymm.Substring(2, 2) + "년 " + yymm.Substring(4, 2) + "월 " + dept_nm + " 근무현황표";
						hrow["GW_REMK"] = "";
						hrow["AP_TAG"] = "4";
						hrow["LINE_CNT"] = 1;
						if (sl_line2.EditValue != null)
							hrow["LINE_CNT"] = 3;
						else if (sl_line1.EditValue != null)
							hrow["LINE_CNT"] = 2;

						hrow["GW_SABN1"] = sl_embs.EditValue.ToString();
						hrow["GW_DT1"] = gd.GetNow();
						hrow["GW_NAME1"] = ds.Tables["8030_SEARCH_EMBS"].Select("CODE ='" + sl_embs.EditValue.ToString() + "'")[0]["NAME"].ToString();
						hrow["GW_JICK1"] = ds.Tables["8030_SEARCH_EMBS"].Select("CODE ='" + sl_embs.EditValue.ToString() + "'")[0]["GRAD_NM"].ToString();
						for (int i = 2; i <= 4; i++)
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
						if (ds.Tables["GW_SEARCH_DANG_PLAN"].Rows.Count != ds.Tables["GW_SEARCH_DANG_PLAN"].Select("CHK = '1'").Length)
							hrow["ETC_GUBN"] = "1";  //일부상신
						else
							hrow["ETC_GUBN"] = ""; //전체상신
						hrow["REG_DT"] = gd.GetNow();
						hrow["REG_ID"] = SilkRoad.Config.SRConfig.USID;
						ds.Tables["DUTY_GWDOC"].Rows.Add(hrow);

						string[] tableNames = new string[] { "DUTY_GWDOC" };
						SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
						outVal = cmd.setUpdate(ref ds, tableNames, null);
						#endregion

						if (outVal > 0)
						{
							df.GetGW_TRSDANGDatas(ds);  //등록
							foreach (DataRow dr in ds.Tables["GW_SEARCH_DANG_PLAN"].Rows)
							{
								if (dr["CHK"].ToString() == "1")
								{
									DataRow nrow = ds.Tables["GW_TRSDANG"].NewRow();
									nrow["DOC_NO"] = doc_no;
									nrow["SAWON_NO"] = dr["SAWON_NO"].ToString();
									nrow["JS_YYMM"] = clib.DateToText(dat_jsmm.DateTime).Substring(0, 6);
									nrow["PLANYYMM"] = yymm;
									nrow["PLAN_SQ"] = dr["PLAN_SQ"].ToString();
									nrow["DEPTCODE"] = dr["DEPTCODE"].ToString();

									for (int i = 1; i <= 31; i++)
									{
										if (i < 6)
											nrow["MM_CNT" + i.ToString()] = clib.TextToDecimal(dr["MM_CNT" + i.ToString()].ToString());
										nrow["D" + i.ToString().PadLeft(2, '0')] = dr["D" + i.ToString().PadLeft(2, '0')].ToString();
									}

									nrow["INDT"] = dr["INDT"].ToString();
									nrow["UPDT"] = dr["UPDT"].ToString();
									nrow["USID"] = dr["USID"].ToString();
									nrow["PSTY"] = dr["PSTY"].ToString();

									ds.Tables["GW_TRSDANG"].Rows.Add(nrow);
								}
							}

							tableNames = new string[] { "GW_TRSDANG" };
							cmd.setUpdate(ref ds, tableNames, null);

							stat = 2;
						}
					}
					catch (Exception ec)
					{
						stat = 1;
						MessageBox.Show(ec.Message, "저장오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					finally
					{
						//SetCancel();
						Cursor = Cursors.Default;
						if (stat == 2)
							this.Dispose();
					}
				}
			}
        }

        /// <summary>취소버튼</summary>
        private void btn_clear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("해당 내역을 취소하시겠습니까?", "취소", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                SetCancel();
            }
        }
		
		private void btn_select_all_Click(object sender, EventArgs e)
		{			
			if (ds.Tables["GW_SEARCH_DANG_PLAN"] != null)
			{
				for (int i = 0; i < ds.Tables["GW_SEARCH_DANG_PLAN"].Rows.Count; i++)
				{
					ds.Tables["GW_SEARCH_DANG_PLAN"].Rows[i]["CHK"] = "1";
				}
			}
		}
		private void btn_select_canc_Click(object sender, EventArgs e)
		{
			if (ds.Tables["GW_SEARCH_DANG_PLAN"] != null)
			{
				for (int i = 0; i < ds.Tables["GW_SEARCH_DANG_PLAN"].Rows.Count; i++)
				{
					ds.Tables["GW_SEARCH_DANG_PLAN"].Rows[i]["CHK"] = "";
				}
			}
		}

        #endregion

        #region 3 EVENT
		
		private void sl_embs_EditValueChanged(object sender, EventArgs e)
		{
			if (sl_embs.EditValue != null)
			{
                DataRow drow = ds.Tables["8030_SEARCH_EMBS"].Select("CODE = '" + sl_embs.EditValue.ToString() + "'")[0];

                string adgb = drow["EMBSADGB"].ToString();
                if ("1200,2600".Contains(drow["EMBSDPCD"].ToString()))
                    adgb = "1";
				ADGB_STAT(adgb);
			}
		}

		private void ADGB_STAT(string adgb)
		{
			string line_txt = "";
			if (adgb == "1")
			{
				df.GetGW_LINE1Datas(sl_embs.EditValue.ToString(), "'2'", ds);
				sl_line1.Properties.DataSource = ds.Tables["GW_LINE1"];
				df.GetGW_LINE2_RQSTDatas(sl_embs.EditValue.ToString(), "'5','6'", ds);
				sl_line2.Properties.DataSource = ds.Tables["GW_LINE2"];

				sl_line2.Visible = true;
				//line_txt = "[팀장 -> 부서장 -> 대표/담당원장]";
				//sl_line2.Visible = false;
				line_txt = "[팀장 -> 부서장]";
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

        private void duty2062_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)   //취소
            {
                //btn_clear.PerformClick();
            }
        }
		
		//중간관리자 제외
		private void chk_line_CheckedChanged(object sender, EventArgs e)
		{
			if (chk_line.Checked == false)
			{
				sl_line1.EditValue = null;
				sl_line2.EditValue = null;
				string embs = sl_embs.EditValue == null ? "%" : sl_embs.EditValue.ToString();
				df.GetGW_LINE1Datas(embs, "'2'", ds);
				sl_line1.Properties.DataSource = ds.Tables["GW_LINE1"];
				//df.GetGW_LINE2Datas(embs, "'5','6'", ds);
				df.GetGW_LINE2_RQSTDatas(embs, "'5','6'", ds);
				sl_line2.Properties.DataSource = ds.Tables["GW_LINE2"];

				sl_line2.Visible = true;
				//lb_line.Text = "[팀장 -> 부서장 -> 대표/담당원장]";
				lb_line.Text = "[팀장 -> 부서장]";
			}
			else
			{
				sl_line1.EditValue = null;
				string embs = sl_embs.EditValue == null ? "%" : sl_embs.EditValue.ToString();
				//df.GetGW_LINE1Datas(embs, "'5','6'", ds);
				df.GetGW_LINE1_RQSTDatas(embs, "'5','6'", ds);
				sl_line1.Properties.DataSource = ds.Tables["GW_LINE1"];

				sl_line2.Visible = false;
				lb_line.Text = "[팀장 -> 대표/담당원장]";
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
            bool isError = false;
            if (mode == 1)  //상신
            {
				if (ds.Tables["GW_SEARCH_DANG_PLAN"].Rows.Count == 0)
				{
					MessageBox.Show("결재상신할 내용이 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				else if (sl_embs.EditValue == null)
				{
					MessageBox.Show("기안자를 선택하세요!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_embs.Focus();
					return false;
				}
				else if (sl_line1.Visible == true && sl_line1.EditValue == null)
				{
					MessageBox.Show("결재자1이 선택되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_line1.Focus();
					return false;
				}
				//else if (sl_line2.Visible == true && sl_line2.EditValue == null)
				//{
				//	MessageBox.Show("결재자2가 선택되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				//	sl_line2.Focus();
				//	return false;
				//}
				else
				{
					isError = true;
				}
            }
            return isError;
        }

		#endregion

		#region 9. ETC

		#endregion
		
	}
}
