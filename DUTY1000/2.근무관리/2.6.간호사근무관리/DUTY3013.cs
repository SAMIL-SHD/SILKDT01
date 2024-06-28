using System;
using System.Data;
using System.Windows.Forms;
using SilkRoad.Common;
using System.Drawing;
using System.IO;
using ExcelDataReader;

namespace DUTY1000
{
    public partial class duty3013 : SilkRoad.Form.Base.FormX
    {
        CommonLibrary clib = new CommonLibrary();

        ClearNEnableControls cec = new ClearNEnableControls();
        public DataSet ds = new DataSet();
        DataProcFunc df = new DataProcFunc();
        SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();
		int gubn = 0;
		string yymm = "";

        public duty3013(int _gubn, string _yymm, int point_x, int point_y)
        {
            InitializeComponent();
			gubn = _gubn;
			yymm = _yymm;

            //this.Location = new Point(point_x - 150, point_y + 20);    //Form Start Point
			
            if (point_x + this.Width > SystemInformation.VirtualScreen.Width)            
                point_x = System.Windows.Forms.SystemInformation.VirtualScreen.Width - this.Width;
            if (point_y + this.Height + 200 > System.Windows.Forms.SystemInformation.VirtualScreen.Height)
                point_y = System.Windows.Forms.SystemInformation.VirtualScreen.Height - this.Height - 200;
            this.Location = new Point(point_x, point_y);
			//dept = _dept;
        }

        #region 0. Initialization

        /// <summary>
        ///컨트롤 초기화 및 활성,비활성 설정
        /// </summary>
        /// <param name="enable"></param>
        private void SetCancel()
        {
			grd1.DataSource = null;
			grd_e.DataSource = null;
            //SetButtonEnable("100");
        }

        #endregion

        #region 1 Form

        private void duty3012_Load(object sender, EventArgs e)
        {
			srTitle1.SRTitleTxt = gubn == 1 ? "밤근무 수당내역" : "간호간병비 수당내역";

            if (gubn == 1)
            {
                gridColumn13.Visible = false;
                gridColumn19.Visible = false;
            }
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
				string adgb = ds.Tables["8030_SEARCH_EMBS"].Select("CODE = '" + sl_embs.EditValue.ToString() + "'")[0]["EMBSADGB"].ToString();
				ADGB_STAT(adgb);
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
			gr_detail.Text = yymm.Substring(2, 2) + "년 " + yymm.Substring(4, 2) + "월 간호부 " + srTitle1.SRTitleTxt;
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
			df.Get3013_SEARCH_SDDatas(gubn, yymm, SilkRoad.Config.SRConfig.USID, ds);
			grd1.DataSource = ds.Tables["3013_SEARCH_SD"];
			grd_e.DataSource = ds.Tables["3013_SEARCH_SD"];
        }
		
		//엑셀변환
		private void btn_excel_Click(object sender, EventArgs e)
		{
            clib.gridToExcel(grdv1, (gubn == 1 ? "밤근무 수당내역_" : "간호간병비 수당내역_") + clib.DateToText(DateTime.Now), true);
        }
        //엑셀양식다운
        private void btn_e_dn_Click(object sender, EventArgs e)
        {
            clib.gridToExcel(grdv_e, (gubn == 1 ? "밤근무 수당양식_" : "간호간병비 수당양식_") + clib.DateToText(DateTime.Now), true);
        }
        //엑셀업로드
        private void btn_e_up_Click(object sender, EventArgs e)
		{
			#region 엑셀 읽어오기
			//System.Data.DataTable dt = null;
			//System.Windows.Forms.OpenFileDialog fd = new System.Windows.Forms.OpenFileDialog();
			//fd.DefaultExt = "xls | xlsx";
			//fd.Filter = "Excel files (*.xls)|*.xls|Excel Files (*.xlsx)|*.xlsx";
			//if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			//{
			//	try
			//	{
			//		OleDbConnection oledbCn = null;
			//		OleDbDataAdapter da = null;

			//		try
			//		{
			//			string type = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties='Excel 12.0;HDR=YES'";
			//			oledbCn = new OleDbConnection(string.Format(type, fd.FileName));
			//			oledbCn.Open();

			//			//첫번째 시트 무조건 가지고 오기
			//			System.Data.DataTable worksheets = oledbCn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
			//			da = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}]", worksheets.Rows[0]["TABLE_NAME"]), oledbCn);

			//			dt = new System.Data.DataTable();
			//			da.Fill(dt);
			//		}
			//		catch (Exception ex)
			//		{
			//			System.Windows.Forms.MessageBox.Show("ReadExcel Err:" + ex.Message);
			//		}
			//		finally
			//		{
			//			if (da != null)
			//				da.Dispose();
			//			if (oledbCn != null)
			//			{
			//				if (oledbCn.State != ConnectionState.Closed)
			//					oledbCn.Close();
			//				oledbCn.Dispose();
			//			}
			//		}
			//	}
			//	catch (Exception ex)
			//	{
			//		System.Windows.Forms.MessageBox.Show("파일을 읽을 수 없습니다. " + ex.Message);
			//	}
			//	finally
			//	{
			//		fd.Dispose();
			//	}
			//}

			//if (dt == null)
			//	return;
			#endregion
			
			#region 엑셀 읽어오기
			System.Data.DataTable dt = null;
			System.Windows.Forms.OpenFileDialog fd = new System.Windows.Forms.OpenFileDialog();
			fd.DefaultExt = "xls | xlsx";
			fd.Filter = "Excel files (*.xls)|*.xls|Excel Files (*.xlsx)|*.xlsx";

			if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
                string fileName = fd.FileName;
				FileInfo fi = new FileInfo(fileName);
				if ((fi.Extension == ".xls" | fi.Extension == ".xlsx") == false)
				{
					return;
				}
				FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				IExcelDataReader reader = ExcelReaderFactory.CreateReader(fs);
				DataSet result = reader.AsDataSet();
				reader.Close();
				if (result == null)
				{
					return;
				}
				dt = result.Tables[0];
			}

			#endregion

			if (dt.Rows[0][1].ToString().ToString() != "사번")  // dt.Columns[0]
			{
				MessageBox.Show("엑셀형식이 바르지 않습니다.\r\n첫번째 열의 타이틀은 [사번]으로 작성해야합니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			else if (dt.Rows[0][3].ToString() != "년차")
			{
				MessageBox.Show("엑셀형식이 바르지 않습니다.\r\n세번째 열의 타이틀은 [년차]로 작성해야합니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			//else if (dt.Rows[0][3].ToString() != "갯수")
			//{
			//	MessageBox.Show("엑셀형식이 바르지 않습니다.\r\n네번째 열의 타이틀은 [갯수]으로 작성해야합니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//	return;
			//}
			else if (dt.Rows[0][6].ToString() != "비용")
			{
				MessageBox.Show("엑셀형식이 바르지 않습니다.\r\n여섯번째 열의 타이틀은 [비용]으로 작성해야합니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Cursor = Cursors.WaitCursor;
			grd1.DataSource = null;
			grd_e.DataSource = null;
			int outVal = 0;

			int t_cnt = dt.Columns.Count;
			foreach (DataRow drow in dt.Rows)
			{
				if (drow[1].ToString().Trim() != "") // && clib.TextToDecimal(drow[2].ToString()) != 0)
				{
                    string sabn = drow[1].ToString().Trim();
                    if (ds.Tables["3013_SEARCH_SD"].Select("SAWON_NO = '" + sabn + "'").Length > 0)
					{
						DataRow nrow = ds.Tables["3013_SEARCH_SD"].Select("SAWON_NO = '" + sabn + "'")[0];
						nrow["PAST_YEAR"] = clib.TextToDecimal(drow[3].ToString());
						nrow["MM_CNT3"] = clib.TextToDecimal(drow[4].ToString());
                        decimal amt = clib.TextToDecimal(drow[6].ToString());
                        if (gubn == 1)
                        {
                            nrow["SD_AMT"] = amt;
                            nrow["SD_AMT2"] = 0;
                            nrow["SD_AMT3"] = 0;
                        }
                        else if (gubn == 2) //엑셀작성한 금액 그대로 업로드 수정 23.10.11 조수진(간호과장요청)
                        {
                            nrow["SD_AMT"] = amt;
                            nrow["SD_AMT2"] = clib.TextToDecimal(drow[7].ToString());
                            nrow["SD_AMT3"] = clib.TextToDecimal(drow[8].ToString());
                        }
                        //else if (gubn == 2 && amt != 0)
                        //{
                        //    //3교대 제외, 간호조무사는 특별수당 1만원 + 나이트전담 (14개)
                        //    //그외 야간간호료 2만,특별수당 1만원으로 분배
                        //    df.GetCHK_INCENTDatas(sabn, ds);
                        //    if (ds.Tables["CHK_INCENT"].Rows.Count > 0)
                        //    {
                        //        nrow["SD_AMT"] = amt - 10000;
                        //        nrow["SD_AMT2"] = 0;
                        //        nrow["SD_AMT3"] = 10000;
                        //    }
                        //    else
                        //    {
                        //        nrow["SD_AMT"] = amt - 30000;
                        //        nrow["SD_AMT2"] = 20000;
                        //        nrow["SD_AMT3"] = 10000;
                        //    }
                        //}
                        outVal++;
					}
				}
			}
			if (outVal > 0)			
				MessageBox.Show("엑셀업로드가 완료되었습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
			
			grd1.DataSource = ds.Tables["3013_SEARCH_SD"];
			grd_e.DataSource = ds.Tables["3013_SEARCH_SD"];
			Cursor = Cursors.Default;
		}
        //저장버튼
        private void btn_save_Click(object sender, EventArgs e)
        {
			int stat = 0;
			decimal doc_no = 0;
			if (isNoError_um(1))
			{
				Cursor = Cursors.WaitCursor;
				int outVal = 0;
				try
				{
					df.GetDUTY_GWDOCDatas(ds);
					DataRow hrow = ds.Tables["DUTY_GWDOC"].NewRow();
					doc_no = df.GetGWDOC_NODatas(ds);
					string title_nm = gubn == 1 ? "밤근무" : "간호간병비";
					hrow["DOC_NO"] = doc_no;
					hrow["DOC_GUBN"] = gubn == 1 ? 4 : 7;
					hrow["DOC_JSMM"] = clib.DateToText(dat_jsmm.DateTime).Substring(0, 6);
					hrow["DOC_DATE"] = yymm;
					hrow["GW_TITLE"] = yymm.Substring(2, 2) + "년 " + yymm.Substring(4, 2) + "월 간호부 " + title_nm + " 수당";
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
					hrow["ETC_GUBN"] = "";
					hrow["REG_DT"] = gd.GetNow();
					hrow["REG_ID"] = SilkRoad.Config.SRConfig.USID;
					ds.Tables["DUTY_GWDOC"].Rows.Add(hrow);

					string[] tableNames = new string[] { "DUTY_GWDOC" };
					SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
					outVal = cmd.setUpdate(ref ds, tableNames, null);

					if (outVal > 0)
					{
						df.GetGW_TRSYAGANDatas(ds);  //등록
						foreach (DataRow dr in ds.Tables["3013_SEARCH_SD"].Rows)
						{
							if (dr["CHK"].ToString() == "1")
							{
								DataRow nrow = ds.Tables["GW_TRSYAGAN"].NewRow();
								nrow["DOC_NO"] = doc_no;
								nrow["JS_YYMM"] = clib.DateToText(dat_jsmm.DateTime).Substring(0, 6);
								nrow["PLANYYMM"] = yymm;
								nrow["DEPTCODE"] = dr["DEPTCODE"].ToString();
								nrow["DEPT_NM"] = dr["DEPT_NM"].ToString();
								nrow["SAWON_NO"] = dr["SAWON_NO"].ToString();
								nrow["EMBSNAME"] = dr["EMBSNAME"].ToString();
								
								nrow["PAST_YEAR"] = clib.TextToDecimal(dr["PAST_YEAR"].ToString());
								nrow["N_CNT"] = clib.TextToDecimal(dr["MM_CNT3"].ToString());
								nrow["IPDT"] = dr["IPDT"].ToString();
								nrow["SD_AMT"] = clib.TextToDecimal(dr["SD_AMT"].ToString());
                                nrow["SD_AMT2"] = clib.TextToDecimal(dr["SD_AMT2"].ToString());
                                nrow["SD_AMT3"] = clib.TextToDecimal(dr["SD_AMT3"].ToString());

                                ds.Tables["GW_TRSYAGAN"].Rows.Add(nrow);
							}
						}

						tableNames = new string[] { "GW_TRSYAGAN" };
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

        /// <summary>취소버튼</summary>
        private void btn_clear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("해당 내역을 취소하시겠습니까?", "취소", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                SetCancel();
            }
        }

        #endregion

        #region 3 EVENT
        private void grdv1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //DataRow drow = grdv1.GetFocusedDataRow();
            //if (drow != null)
            //{
            //    decimal amt = clib.TextToDecimal(drow["SD_AMT"].ToString());
            //    string sabn = drow["SAWON_NO"].ToString().Trim();
            //    if (grdv1.FocusedColumn == gridColumn3)
            //    {
            //        if (gubn == 2 && amt != 0)
            //        {
            //            //3교대 제외, 간호조무사는 특별수당 1만원
            //            //그외 야간간호료 2만,특별수당 1만원으로 분배
            //            df.GetCHK_INCENTDatas(sabn, ds);
            //            if (ds.Tables["CHK_INCENT"].Rows.Count > 0)
            //            {
            //                drow["SD_AMT"] = amt - 10000;
            //                drow["SD_AMT2"] = 0;
            //                drow["SD_AMT3"] = 10000;
            //            }
            //            else
            //            {
            //                drow["SD_AMT"] = amt - 30000;
            //                drow["SD_AMT2"] = 20000;
            //                drow["SD_AMT3"] = 10000;
            //            }
            //        }
            //    }
            //}
        }

        private void grd1_EditorKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (!this.grdv1.IsLastVisibleRow)
					this.grdv1.MoveNext();
				else
					this.grdv1.MoveFirst();
			}
		}

		private void sl_embs_EditValueChanged(object sender, EventArgs e)
		{
			if (sl_embs.EditValue != null)
			{
				string adgb = ds.Tables["8030_SEARCH_EMBS"].Select("CODE = '" + sl_embs.EditValue.ToString() + "'")[0]["EMBSADGB"].ToString();
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

        private void duty3012_KeyDown(object sender, KeyEventArgs e)
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
				df.GetGW_LINE2Datas(embs, "'5','6'", ds);
				sl_line2.Properties.DataSource = ds.Tables["GW_LINE2"];

				sl_line2.Visible = true;
				lb_line.Text = "[팀장 -> 부서장 -> 대표/담당원장]";
			}
			else
			{
				sl_line1.EditValue = null;
				string embs = sl_embs.EditValue == null ? "%" : sl_embs.EditValue.ToString();
				df.GetGW_LINE1Datas(embs, "'5','6'", ds);
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
				if (ds.Tables["3013_SEARCH_SD"].Select("CHK='1'").Length < 1)
				{
					MessageBox.Show("선택된 내역이 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				else if (sl_line2.Visible == true && sl_line2.EditValue == null)
				{
					MessageBox.Show("결재자2가 선택되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_line2.Focus();
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

        #endregion

    }
}
