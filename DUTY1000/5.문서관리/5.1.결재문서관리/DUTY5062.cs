using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using SilkRoad.Common;
using System.Drawing;
using DevExpress.XtraReports.UI;

namespace DUTY1000
{
    public partial class duty5062 : SilkRoad.Form.Base.FormX
    {
        CommonLibrary clib = new CommonLibrary();

        ClearNEnableControls cec = new ClearNEnableControls();
        public DataSet ds = new DataSet();
        DataProcFunc df = new DataProcFunc();
        SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();
		string stat = "";
		string gubn = "";
		string doc_no = "";

        public duty5062(string _stat, string _gubn, string _doc_no)
        {
            InitializeComponent();
			stat = _stat;
			gubn = _gubn;
			doc_no = _doc_no;

			if (_gubn == "1" || _gubn == "21" || _gubn == "2" || _gubn == "21" || _gubn == "3" || _gubn == "4" || _gubn == "7")
			{
				this.Width = 800;
				btn_preview.Visible = true;
			}
			else
			{
				this.Width = 1200;
				btn_preview.Visible = false;
			}
			//this.Width = clib.TextToInt(_gubn) > 4 ? 1200 : 800;
			//btn_preview.Visible = clib.TextToInt(_gubn) > 3 ? false : true;
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

        private void duty5062_Load(object sender, EventArgs e)
        {
			if (stat == "1")
			{
				//해당 문서가 내 결재차례인지 체크 (22.10.19 추가)
				df.Get5062_CHK_DOC_STATDatas(doc_no, ds);
				if (ds.Tables["5062_CHK_DOC_STAT"].Rows.Count > 0)
				{
					DataRow crow = ds.Tables["5062_CHK_DOC_STAT"].Rows[0];
					if (crow["CHK_SABN1"].ToString().Trim() == SilkRoad.Config.SRConfig.USID || crow["CHK_SABN2"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
					{
                        btn_save.Enabled = true;
                        btn_return.Enabled = true;
                        btn_canc.Enabled = true;
                    }
					else
					{
                        btn_save.Enabled = false;
                        btn_return.Enabled = false;
                        btn_canc.Enabled = false;
                    }
				}
				else
				{
                    btn_save.Enabled = false;
                    btn_return.Enabled = false;
                    btn_canc.Enabled = false;
                }
			}

			if (SilkRoad.Config.ACConfig.G_MSYN == "1" || SilkRoad.Config.SRConfig.USID == "SAMIL")
			{
				btn_admin_save.Visible = true;
				btn_admin_canc.Visible = true;
                btn_save.Enabled = false;
                btn_return.Enabled = false;
                btn_canc.Enabled = true;
            }

			if (stat == "2")
				srTitle1.SRTitleTxt = "완료문서";

            if (gubn == "1" || gubn == "21")
                xtraTabPage1.PageVisible = true;
            else if (gubn == "2")
                xtraTabPage2.PageVisible = true;
            else if (gubn == "3")
                xtraTabPage3.PageVisible = true;
            else if (gubn == "4" || gubn == "7")
                xtraTabPage4.PageVisible = true;
            else if (gubn == "5")
                xtraTabPage5.PageVisible = true;
            else if (gubn == "6")
                xtraTabPage6.PageVisible = true;
            else if (gubn == "8" || gubn == "9" || gubn == "10" || gubn == "11" || gubn == "12" || gubn == "13" || gubn == "14")
            {
                xtraTabPage7.PageVisible = true;
                btn_preview.Visible = true;
            }

            if (gubn == "7")
            {
                xtraTabPage4.Text = "간호간병비 수당내역";
                gridColumn33.Visible = true;
                gridColumn34.Visible = true;
            }

            if (gubn == "21")
            {
                xtraTabPage1.Text = "출장검진내역";
                gridColumn4.Caption = "출장횟수";
                col_code.Visible = false;
            }

            if (gubn == "3" || gubn == "4" || gubn == "7")
                btn_excel.Visible = true;

            Proc();
        }

        #endregion

        #region 2 Button

        private void Proc()
        {
			df.Get5062_SEARCHDatas(gubn, doc_no, ds);
			if (gubn == "1" || gubn == "21")
				grd1.DataSource = ds.Tables["5062_SEARCH"];
			else if (gubn == "2")
				grd2.DataSource = ds.Tables["5062_SEARCH"];
			else if (gubn == "3")
				grd3.DataSource = ds.Tables["5062_SEARCH"];
			else if (gubn == "4" || gubn == "7")
				grd4.DataSource = ds.Tables["5062_SEARCH"];
			else if (gubn == "5")
			{
				#region 요일에따른 헤더 설정
				string yymm = ds.Tables["5062_SEARCH"].Rows[0]["PLANYYMM"].ToString();
				df.GetSEARCH_HOLIDatas(yymm, ds);
				int lastday = clib.TextToInt(clib.DateToText(clib.TextToDateLast(yymm + "01")).Substring(6, 2));

				for (int i = 1; i <= lastday; i++)
				{
					DateTime day = clib.TextToDate(yymm + "01").AddDays(i - 1);
					grdv5.Columns["D" + i.ToString().PadLeft(2, '0')].Caption = i.ToString() + "\r\n" + clib.WeekDay(day); //일+요일. 한칸 내려서 보이도록. 엔터마냥. 
					if (clib.WeekDay(day) == "토")
						grdv5.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Blue;
					else if (clib.WeekDay(day) == "일")
						grdv5.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Red;
					else
						grdv5.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Black;

					if (ds.Tables["SEARCH_HOLI"].Select("H_DATE = '" + clib.DateToText(day) + "'").Length > 0)
					{
						grdv5.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Red;
						grdv5.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.Font = new Font(FontFamily.GenericSansSerif, 9F, FontStyle.Underline);
						grdv5.Columns["D" + i.ToString().PadLeft(2, '0')].ToolTip = ds.Tables["SEARCH_HOLI"].Select("H_DATE = '" + clib.DateToText(day) + "'")[0]["H_NAME"].ToString();
					}
				}

				//남은 필드 visible = false;
				for (int k = 1; k < 32; k++)
				{
					if (k > lastday)
						grdv5.Columns["D" + k.ToString().PadLeft(2, '0')].Visible = false;
					else
						grdv5.Columns["D" + k.ToString().PadLeft(2, '0')].Visible = true;
				}
				#endregion

				df.Get2060_DANG_GNMUDatas(ds);
				grd_sl_gnmu.DataSource = ds.Tables["2060_DANG_GNMU"];
				grd5.DataSource = ds.Tables["5062_SEARCH"];

			}
			else if (gubn == "6")
			{
				#region 요일에따른 헤더 설정
				string yymm = ds.Tables["5062_SEARCH"].Rows[0]["PLANYYMM"].ToString();
				df.GetSEARCH_HOLIDatas(yymm, ds);
				int lastday = clib.TextToInt(clib.DateToText(clib.TextToDateLast(yymm + "01")).Substring(6, 2));

				for (int i = 1; i <= lastday; i++)
				{
					DateTime day = clib.TextToDate(yymm + "01").AddDays(i - 1);
					grdv6.Columns["D" + i.ToString().PadLeft(2, '0')].Caption = i.ToString() + "\r\n" + clib.WeekDay(day); //일+요일. 한칸 내려서 보이도록. 엔터마냥. 
					if (clib.WeekDay(day) == "토")
						grdv6.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Blue;
					else if (clib.WeekDay(day) == "일")
						grdv6.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Red;
					else
						grdv6.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Black;

					if (ds.Tables["SEARCH_HOLI"].Select("H_DATE = '" + clib.DateToText(day) + "'").Length > 0)
					{
						grdv6.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Red;
						grdv6.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.Font = new Font(FontFamily.GenericSansSerif, 9F, FontStyle.Underline);
						grdv6.Columns["D" + i.ToString().PadLeft(2, '0')].ToolTip = ds.Tables["SEARCH_HOLI"].Select("H_DATE = '" + clib.DateToText(day) + "'")[0]["H_NAME"].ToString();
					}
				}

				//남은 필드 visible = false;
				for (int k = 1; k < 32; k++)
				{
					if (k > lastday)
						grdv6.Columns["D" + k.ToString().PadLeft(2, '0')].Visible = false;
					else
						grdv6.Columns["D" + k.ToString().PadLeft(2, '0')].Visible = true;
				}
				#endregion

				df.GetGNMU_LISTDatas(ds);
				grd_lk_gnmu.DataSource = ds.Tables["GNMU_LIST"];
				grd6.DataSource = ds.Tables["5062_SEARCH"];
			}
			else if (gubn == "8" || gubn == "9" || gubn == "10" || gubn == "11" || gubn == "12" || gubn == "13" || gubn == "14")
			{
				df.Get5062_SEARCHDatas(gubn, doc_no, ds);

				byte[] file = (byte[])ds.Tables["5062_SEARCH"].Rows[0]["PDF_FILE"];

				MemoryStream ms = new MemoryStream(file);

				//File.WriteAllBytes(Application.StartupPath + "\\downloaded_" + (next_doc_no.ToString()) + ".pdf", file);
				//System.IO.FileStream fsBLOBFile = new System.IO.FileStream(Application.StartupPath + "\\downloaded_" + (next_doc_no.ToString()) + ".pdf", System.IO.FileMode.Open, System.IO.FileAccess.Read);
				pdfViewer1.DetachStreamAfterLoadComplete = true;
				//pdfViewer1.LoadDocument(fsBLOBFile);
				pdfViewer1.LoadDocument(ms);
			}
		}
        //엑셀변환
        private void btn_excel_Click(object sender, EventArgs e)
        {
            string ex_nm = "";
            if (gubn == "3")
            {
                ex_nm = "OFF,N 추가삭감내역";
                clib.gridToExcel(grdv3, ex_nm + "_" + clib.DateToText(DateTime.Now), true);
            }
            else if (gubn == "4")
            {
                ex_nm = "밤근무수당 수당내역";
                clib.gridToExcel(grdv4, ex_nm + "_" + clib.DateToText(DateTime.Now), true);
            }
            else if (gubn == "7")
            {
                ex_nm = "간호간병비 수당내역";
                clib.gridToExcel(grdv4, ex_nm + "_" + clib.DateToText(DateTime.Now), true);
            }
        }
        //미리보기
        private void btn_preview_Click(object sender, EventArgs e)
		{
			string title = "";
			df.GetDUTY_GWDOCDatas(doc_no, ds);
			if (ds.Tables["DUTY_GWDOC"].Rows.Count > 0)
				title = ds.Tables["DUTY_GWDOC"].Rows[0]["GW_TITLE"].ToString();
			if (ds.Tables["5062_SEARCH"].Rows.Count > 0)
			{
				if (gubn == "1")
				{
					rpt_506a rpt = new rpt_506a(title);
					rpt.DataSource = ds.Tables["5062_SEARCH"];
					rpt.ShowPreview();
                }
                else if (gubn == "21")
                {
                    rpt_506a1 rpt = new rpt_506a1(title);
                    rpt.DataSource = ds.Tables["5062_SEARCH"];
                    rpt.ShowPreview();
                }
                else if (gubn == "2")
				{
					rpt_506b rpt = new rpt_506b(title);
					rpt.DataSource = ds.Tables["5062_SEARCH"];
					rpt.ShowPreview();
				}
				else if (gubn == "3")  //간호 OFF/N 추가,삭감내역
				{
					rpt_506c rpt = new rpt_506c(title);
					rpt.DataSource = ds.Tables["5062_SEARCH"];
					rpt.ShowPreview();
				}
				else if (gubn == "4")  //밤근무 수당
				{
					rpt_506d rpt = new rpt_506d(1, title);
					rpt.DataSource = ds.Tables["5062_SEARCH"];
					rpt.ShowPreview();
				}
				else if (gubn == "7")  //간호간병비 수당
				{
					rpt_506e rpt = new rpt_506e(2, title);
					rpt.DataSource = ds.Tables["5062_SEARCH"];
					rpt.ShowPreview();
				}
				else if (gubn == "8" || gubn == "9" || gubn == "10" || gubn == "11" || gubn == "12" || gubn == "13" || gubn == "14")
				{
					pdfViewer1.Print();
				}
			}
		}
		//결재
		private void btn_save_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
            int outVal = 0;
			int stat = 0;
            try
            {
				DataRow drow = ds.Tables["5062_SEARCH"].Rows[0];
				df.GetDUTY_GWDOCDatas(drow["DOC_NO"].ToString(), ds);
				if (ds.Tables["DUTY_GWDOC"].Rows.Count > 0)
				{
					DataRow hrow = ds.Tables["DUTY_GWDOC"].Rows[0];
					if (hrow["AP_TAG"].ToString() != "1")
					{
						hrow["AP_TAG"] = "4";
						if (hrow["GW_DT2"].ToString().Trim() == "" && hrow["GW_SABN2"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
						{
							stat = 1;
							hrow["GW_DT2"] = gd.GetNow();
							hrow["GW_CHKID2"] = SilkRoad.Config.SRConfig.USID;
							if (hrow["GW_SABN3"].ToString().Trim() == "")
								hrow["AP_TAG"] = "1";
						}
						else if (hrow["GW_DT3"].ToString().Trim() == "" && hrow["GW_SABN3"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
						{
							stat = 1;
							hrow["GW_DT3"] = gd.GetNow();
							hrow["GW_CHKID3"] = SilkRoad.Config.SRConfig.USID;
							if (hrow["GW_SABN4"].ToString().Trim() == "")
								hrow["AP_TAG"] = "1";
						}
						else if (hrow["GW_DT4"].ToString().Trim() == "" && hrow["GW_SABN4"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
						{
							stat = 1;
							hrow["GW_DT4"] = gd.GetNow();
							hrow["GW_CHKID4"] = SilkRoad.Config.SRConfig.USID;
							hrow["AP_TAG"] = "1";
						}

						if (stat == 1)
						{
							string[] tableNames = new string[] { "DUTY_GWDOC" };
							SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
							outVal = cmd.setUpdate(ref ds, tableNames, null);
						}
					}
				}
            }
            catch (Exception ec)
            {
                MessageBox.Show(ec.Message, "결재오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (outVal > 0)
                    MessageBox.Show("승인처리 되었습니다.", "결재상신", MessageBoxButtons.OK, MessageBoxIcon.Information);
				else
                    MessageBox.Show("승인할 내역이 없습니다.", "결재오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.Dispose();
                Cursor = Cursors.Default;
            }
		}		
		//결재취소->디테일하게 수정예정
		private void btn_canc_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
            int outVal = 0;
			int stat = 0;
            try
            {
				DataRow drow = ds.Tables["5062_SEARCH"].Rows[0];
				df.GetDUTY_GWDOCDatas(drow["DOC_NO"].ToString(), ds);
				if (ds.Tables["DUTY_GWDOC"].Rows.Count > 0)
				{
					DataRow hrow = ds.Tables["DUTY_GWDOC"].Rows[0];
					if (hrow["AP_TAG"].ToString() != "1")
					{
						if (hrow["GW_DT2"].ToString().Trim() == "" && hrow["GW_SABN1"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
						{
							stat = 1;
							hrow["GW_DT2"] = gd.GetNow();
							hrow["GW_CHKID2"] = SilkRoad.Config.SRConfig.USID;
							hrow["AP_TAG"] = "2";
						}
						else if (hrow["GW_DT3"].ToString().Trim() == "" && hrow["GW_SABN2"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
						{
							stat = 1;
							hrow["GW_DT3"] = gd.GetNow();
							hrow["GW_CHKID3"] = SilkRoad.Config.SRConfig.USID;
							hrow["AP_TAG"] = "2";
						}
						else if (hrow["GW_DT4"].ToString().Trim() == "" && hrow["GW_SABN3"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
						{
							stat = 1;
							hrow["GW_DT4"] = gd.GetNow();
							hrow["GW_CHKID4"] = SilkRoad.Config.SRConfig.USID;
							hrow["AP_TAG"] = "2";
						}

						if (stat == 1)
						{
							string[] tableNames = new string[] { "DUTY_GWDOC" };
							SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
							outVal = cmd.setUpdate(ref ds, tableNames, null);
						}
					}
				}
            }
            catch (Exception ec)
            {
                MessageBox.Show(ec.Message, "취소오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (outVal > 0)
                    MessageBox.Show("취소처리 되었습니다.", "승인취소", MessageBoxButtons.OK, MessageBoxIcon.Information);
				else
                    MessageBox.Show("취소할 내역이 없습니다.", "취소오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.Dispose();
                Cursor = Cursors.Default;
            }
		}
		//반려
		private void btn_return_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
            int outVal = 0;
			int stat = 0;
            try
            {
				DataRow drow = ds.Tables["5062_SEARCH"].Rows[0];
				df.GetDUTY_GWDOCDatas(drow["DOC_NO"].ToString(), ds);
				if (ds.Tables["DUTY_GWDOC"].Rows.Count > 0)
				{
					DataRow hrow = ds.Tables["DUTY_GWDOC"].Rows[0];
					if (hrow["AP_TAG"].ToString() != "1")
					{
						if (hrow["GW_DT2"].ToString().Trim() == "" && hrow["GW_SABN2"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
						{
							stat = 1;
							hrow["GW_DT2"] = gd.GetNow();
							hrow["GW_CHKID2"] = SilkRoad.Config.SRConfig.USID;
							hrow["AP_TAG"] = "5";
						}
						else if (hrow["GW_DT3"].ToString().Trim() == "" && hrow["GW_SABN3"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
						{
							stat = 1;
							hrow["GW_DT3"] = gd.GetNow();
							hrow["GW_CHKID3"] = SilkRoad.Config.SRConfig.USID;
							hrow["AP_TAG"] = "5";
						}
						else if (hrow["GW_DT4"].ToString().Trim() == "" && hrow["GW_SABN4"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
						{
							stat = 1;
							hrow["GW_DT4"] = gd.GetNow();
							hrow["GW_CHKID4"] = SilkRoad.Config.SRConfig.USID;
							hrow["AP_TAG"] = "5";
						}

						if (stat == 1)
						{
							string[] tableNames = new string[] { "DUTY_GWDOC" };
							SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
							outVal = cmd.setUpdate(ref ds, tableNames, null);
						}
					}
				}
            }
            catch (Exception ec)
            {
                MessageBox.Show(ec.Message, "취소오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (outVal > 0)
                    MessageBox.Show("취소처리 되었습니다.", "승인취소", MessageBoxButtons.OK, MessageBoxIcon.Information);
				else
                    MessageBox.Show("취소할 내역이 없습니다.", "취소오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.Dispose();
                Cursor = Cursors.Default;
            }
		}
		
		//관리자결재
		private void btn_admin_save_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
            int outVal = 0;
            try
            {
				DataRow drow = ds.Tables["5062_SEARCH"].Rows[0];
				df.GetDUTY_GWDOCDatas(drow["DOC_NO"].ToString(), ds);
				if (ds.Tables["DUTY_GWDOC"].Rows.Count > 0)
				{
					DataRow hrow = ds.Tables["DUTY_GWDOC"].Rows[0];
					if (hrow["AP_TAG"].ToString() != "1")
					{
						hrow["AP_TAG"] = "3";
						hrow["GW_REMK"] = gd.GetNow() + " " +SilkRoad.Config.SRConfig.USID;
						
						string[] tableNames = new string[] { "DUTY_GWDOC" };
						SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
						outVal = cmd.setUpdate(ref ds, tableNames, null);
					}
				}
            }
            catch (Exception ec)
            {
                MessageBox.Show(ec.Message, "결재오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (outVal > 0)
                    MessageBox.Show("완료처리 되었습니다.", "결재완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
				else
                    MessageBox.Show("완료할 내역이 없습니다.", "처리오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.Dispose();
                Cursor = Cursors.Default;
            }
		}		
		//관리자결재취소
		private void btn_admin_canc_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
            int outVal = 0;
            try
            {
				DataRow drow = ds.Tables["5062_SEARCH"].Rows[0];
				df.GetDUTY_GWDOCDatas(drow["DOC_NO"].ToString(), ds);
				if (ds.Tables["DUTY_GWDOC"].Rows.Count > 0)
				{
					DataRow hrow = ds.Tables["DUTY_GWDOC"].Rows[0];
					//if (hrow["AP_TAG"].ToString() != "1")  //승인후 취소 가능하게 2022.10.13 정원희 계장 요청.
					//{
						hrow["AP_TAG"] = "2";
						hrow["GW_REMK"] = gd.GetNow() + " " +SilkRoad.Config.SRConfig.USID;

						string[] tableNames = new string[] { "DUTY_GWDOC" };
						SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
						outVal = cmd.setUpdate(ref ds, tableNames, null);						
					//}
				}
            }
            catch (Exception ec)
            {
                MessageBox.Show(ec.Message, "취소오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (outVal > 0)
                    MessageBox.Show("취소처리 되었습니다.", "결재취소", MessageBoxButtons.OK, MessageBoxIcon.Information);
				else
                    MessageBox.Show("취소할 내역이 없습니다.", "취소오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.Dispose();
                Cursor = Cursors.Default;
            }
		}

        #endregion

    }
}
