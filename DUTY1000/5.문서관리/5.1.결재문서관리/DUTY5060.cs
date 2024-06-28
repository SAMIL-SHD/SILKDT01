using System;
using System.Data;
using System.Windows.Forms;
using SilkRoad.Common;
using DevExpress.XtraScheduler;
using System.Drawing;

namespace DUTY1000
{
	public partial class duty5060 : SilkRoad.Form.Base.FormX
	{
		CommonLibrary clib = new CommonLibrary();

		ClearNEnableControls cec = new ClearNEnableControls();
		public DataSet ds = new DataSet();
		DataProcFunc df = new DataProcFunc();
		SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();

		public duty5060()
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
				if (ds.Tables["5060_AP_YCHG_LIST"] != null)
					ds.Tables["5060_AP_YCHG_LIST"].Clear();
				grd_ap.DataSource = null;
			}
			else if (stat == 1)
			{
				if (ds.Tables["5060_AP_YCHG_LIST2"] != null)
					ds.Tables["5060_AP_YCHG_LIST2"].Clear();
				grd_ap2.DataSource = null;
			}
			else if (stat == 2)
			{
				if (ds.Tables["5060_AP_YCHG_LIST3"] != null)
					ds.Tables["5060_AP_YCHG_LIST3"].Clear();
				grd_ap3.DataSource = null;
			}
			else if (stat == 3)
			{
				if (ds.Tables["5060_AP_YCHG_LIST4"] != null)
					ds.Tables["5060_AP_YCHG_LIST4"].Clear();
				grd_ap4.DataSource = null;
			}
			else if (stat == 4)
			{
				if (ds.Tables["5060_AP_YCHG_LIST5"] != null)
					ds.Tables["5060_AP_YCHG_LIST5"].Clear();
				grd_ap5.DataSource = null;
			}
		}

		#endregion

		#region 1 Form

		private void duty5060_Load(object sender, EventArgs e)
		{

		}

		private void duty5060_Shown(object sender, EventArgs e)
		{
			SetCancel(0);
			//SetCancel(1);

			df.Get5060_AP_YCHG_LISTDatas(SilkRoad.Config.SRConfig.USID, ds);
			grd_ap.DataSource = ds.Tables["5060_AP_YCHG_LIST"];
		}

		#endregion

		#region 2 Button	
		
		//원장단 근무표
		private void btn_doctor_calendar_Click(object sender, EventArgs e)
		{
			duty5064 duty5064 = new duty5064();
			duty5064.Show();
		}

		//승인내역 조회
		private void btn_ap_search_Click(object sender, EventArgs e)
		{
			if (srTabControl1.SelectedTabPageIndex == 0)
			{
				df.Get5060_AP_YCHG_LISTDatas(SilkRoad.Config.SRConfig.USID, ds);
				grd_ap.DataSource = ds.Tables["5060_AP_YCHG_LIST"];
				if (ds.Tables["5060_AP_YCHG_LIST"].Rows.Count == 0)
					MessageBox.Show("결재할 연차/휴가 내역이 없습니다!", "결재", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else if (srTabControl1.SelectedTabPageIndex == 1)
			{
				df.Get5060_AP_YCHG_LIST2Datas(SilkRoad.Config.SRConfig.USID, ds);
				grd_ap2.DataSource = ds.Tables["5060_AP_YCHG_LIST2"];
				if (ds.Tables["5060_AP_YCHG_LIST2"].Rows.Count == 0)
					MessageBox.Show("결재할 CALL/OT 내역이 없습니다!", "결재", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else if (srTabControl1.SelectedTabPageIndex == 2)
			{
				df.Get5060_AP_YCHG_LIST3Datas(SilkRoad.Config.SRConfig.USID, ds);
				grd_ap3.DataSource = ds.Tables["5060_AP_YCHG_LIST3"];
				if (ds.Tables["5060_AP_YCHG_LIST3"].Rows.Count == 0)
					MessageBox.Show("결재할 OFF,N/밤근무 내역이 없습니다!", "결재", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else if (srTabControl1.SelectedTabPageIndex == 3)
			{
				df.Get5060_AP_YCHG_LIST4Datas(SilkRoad.Config.SRConfig.USID, ds);
				grd_ap4.DataSource = ds.Tables["5060_AP_YCHG_LIST4"];
				if (ds.Tables["5060_AP_YCHG_LIST4"].Rows.Count == 0)
					MessageBox.Show("결재할 근무표 내역이 없습니다!", "결재", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else if (srTabControl1.SelectedTabPageIndex == 4)
			{
				string gubn = get_doc_gubn(4);
				df.Get5060_AP_YCHG_LIST5Datas(gubn, SilkRoad.Config.SRConfig.USID, ds);
				grd_ap5.DataSource = ds.Tables["5060_AP_YCHG_LIST5"];
				if (ds.Tables["5060_AP_YCHG_LIST5"].Rows.Count == 0)
					MessageBox.Show("결재할 회계문서 내역이 없습니다!", "결재", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//승인내역 조회clear
		private void btn_ap_clear_Click(object sender, EventArgs e)
		{
			SetCancel(srTabControl1.SelectedTabPageIndex);
		}
		//연차,휴가승인
		private void btn_ap_save_Click(object sender, EventArgs e)
		{
			if (isNoError_um(11))
			{
				Cursor = Cursors.WaitCursor;
				int outVal = 0;
				try
				{
					for (int i = 0; i < ds.Tables["5060_AP_YCHG_LIST"].Rows.Count; i++)
					{
						DataRow drow = ds.Tables["5060_AP_YCHG_LIST"].Rows[i];
						if (drow["CHK"].ToString() == "1")
						{
							string tb_nm = drow["GUBN"].ToString() == "1" ? "DUTY_TRSHREQ" : "DUTY_TRSJREQ";
							df.Get5060_DUTY_TRSHREQDatas(drow["GUBN"].ToString(), drow["SEQNO"].ToString(), ds);//drow["GUBN"].ToString(), drow["SABN"].ToString(), drow["REQ_DATE"].ToString(), ds);
							if (ds.Tables[tb_nm].Rows.Count > 0)
							{
								DataRow hrow = ds.Tables[tb_nm].Rows[0];
								if (hrow["AP_TAG"].ToString() != "1")
								{
									hrow["AP_TAG"] = "4";
									if (hrow["GW_DT2"].ToString().Trim() == "" && hrow["GW_SABN2"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
									{
										hrow["GW_DT2"] = gd.GetNow();
										hrow["GW_CHKID2"] = SilkRoad.Config.SRConfig.USID;
										if (hrow["GW_SABN3"].ToString().Trim() == "")
											hrow["AP_TAG"] = "1";
									}
									else if (hrow["GW_DT3"].ToString().Trim() == "" && hrow["GW_SABN3"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
									{
										hrow["GW_DT3"] = gd.GetNow();
										hrow["GW_CHKID3"] = SilkRoad.Config.SRConfig.USID;
										if (hrow["GW_SABN4"].ToString().Trim() == "")
											hrow["AP_TAG"] = "1";
									}
									else if (hrow["GW_DT4"].ToString().Trim() == "" && hrow["GW_SABN4"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
									{
										hrow["GW_DT4"] = gd.GetNow();
										hrow["GW_CHKID4"] = SilkRoad.Config.SRConfig.USID;
										hrow["AP_TAG"] = "1";
									}
																		
									if (SilkRoad.Config.SRConfig.USID == "SAMIL")
									{
										hrow["GW_DT4"] = gd.GetNow();
										hrow["GW_CHKID4"] = SilkRoad.Config.SRConfig.USID;
										hrow["AP_TAG"] = "1";
									}
									string[] tableNames = new string[] { tb_nm };
									SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
									outVal += cmd.setUpdate(ref ds, tableNames, null);
								}
							}
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
						MessageBox.Show(outVal + "건의 선택된 내역이 승인처리 되었습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
					df.Get5060_AP_YCHG_LISTDatas(SilkRoad.Config.SRConfig.USID, ds);
					grd_ap.DataSource = ds.Tables["5060_AP_YCHG_LIST"];
					Cursor = Cursors.Default;
				}
			}
		}
		//연차,휴가 승인취소
		private void btn_ap_canc_Click(object sender, EventArgs e)
		{
			if (isNoError_um(12))
			{
				Cursor = Cursors.WaitCursor;
				int outVal = 0;
				try
				{
					for (int i = 0; i < ds.Tables["5060_AP_YCHG_LIST"].Rows.Count; i++)
					{
						DataRow drow = ds.Tables["5060_AP_YCHG_LIST"].Rows[i];
						if (drow["C_CHK"].ToString() == "1")
						{
							string tb_nm = drow["GUBN"].ToString() == "1" ? "DUTY_TRSHREQ" : "DUTY_TRSJREQ";
							df.Get5060_DUTY_TRSHREQDatas(drow["GUBN"].ToString(), drow["SEQNO"].ToString(), ds);//drow["GUBN"].ToString(), drow["SABN"].ToString(), drow["REQ_DATE"].ToString(), ds);
							if (ds.Tables[tb_nm].Rows.Count > 0)
							{
								DataRow hrow = ds.Tables[tb_nm].Rows[0];
								if (hrow["AP_TAG"].ToString() == "4")
								{
									if (hrow["GW_DT4"].ToString().Trim() != "" && hrow["GW_SABN4"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
									{
										hrow["GW_DT4"] = "";
										hrow["GW_CHKID4"] = SilkRoad.Config.SRConfig.USID;
									}
									else if (hrow["GW_DT3"].ToString().Trim() != "" && hrow["GW_SABN3"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
									{
										hrow["GW_DT3"] = "";
										hrow["GW_CHKID3"] = SilkRoad.Config.SRConfig.USID;
									}
									else if (hrow["GW_DT2"].ToString().Trim() != "" && hrow["GW_SABN2"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
									{
										hrow["GW_DT2"] = "";
										hrow["GW_CHKID2"] = SilkRoad.Config.SRConfig.USID;
									}
									string[] tableNames = new string[] { tb_nm };
									SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
									outVal += cmd.setUpdate(ref ds, tableNames, null);
								}
							}
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
						MessageBox.Show(outVal + "건의 선택된 내역이 취소처리 되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
					df.Get5060_AP_YCHG_LISTDatas(SilkRoad.Config.SRConfig.USID, ds);
					grd_ap.DataSource = ds.Tables["5060_AP_YCHG_LIST"];
					Cursor = Cursors.Default;
				}
			}
		}
		//연차,휴가 반려
		private void btn_ap_return_Click(object sender, EventArgs e)
		{
			if (isNoError_um(11))
			{
				Cursor = Cursors.WaitCursor;
				int outVal = 0;
				try
				{
					for (int i = 0; i < ds.Tables["5060_AP_YCHG_LIST"].Rows.Count; i++)
					{
						DataRow drow = ds.Tables["5060_AP_YCHG_LIST"].Rows[i];
						if (drow["CHK"].ToString() == "1")
						{
							string tb_nm = drow["GUBN"].ToString() == "1" ? "DUTY_TRSHREQ" : "DUTY_TRSJREQ";
							df.Get5060_DUTY_TRSHREQDatas(drow["GUBN"].ToString(), drow["SEQNO"].ToString(), ds);//drow["GUBN"].ToString(), drow["SABN"].ToString(), drow["REQ_DATE"].ToString(), ds);
							if (ds.Tables[tb_nm].Rows.Count > 0)
							{
								DataRow hrow = ds.Tables[tb_nm].Rows[0];
								if (hrow["AP_TAG"].ToString() == "4")
								{
									if (hrow["GW_DT2"].ToString().Trim() == "" && hrow["GW_SABN2"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
									{
										hrow["GW_DT2"] = gd.GetNow();
										hrow["GW_CHKID2"] = SilkRoad.Config.SRConfig.USID;
										hrow["AP_TAG"] = "5";
									}
									else if (hrow["GW_DT3"].ToString().Trim() == "" && hrow["GW_SABN3"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
									{
										hrow["GW_DT3"] = gd.GetNow();
										hrow["GW_CHKID3"] = SilkRoad.Config.SRConfig.USID;
										hrow["AP_TAG"] = "5";
									}
									else if (hrow["GW_DT4"].ToString().Trim() == "" && hrow["GW_SABN4"].ToString().Trim() == SilkRoad.Config.SRConfig.USID)
									{
										hrow["GW_DT4"] = gd.GetNow();
										hrow["GW_CHKID4"] = SilkRoad.Config.SRConfig.USID;
										hrow["AP_TAG"] = "5";
									}
									string[] tableNames = new string[] { tb_nm };
									SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
									outVal += cmd.setUpdate(ref ds, tableNames, null);
								}
							}
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
						MessageBox.Show(outVal + "건의 선택된 내역이 반려처리 되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
					df.Get5060_AP_YCHG_LISTDatas(SilkRoad.Config.SRConfig.USID, ds);
					grd_ap.DataSource = ds.Tables["5060_AP_YCHG_LIST"];
					Cursor = Cursors.Default;
				}
			}
		}

		#endregion

		#region 3 EVENT

		//탭 변경시 새로고침
		private void srTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
		{
			Page_Refresh();
		}
		//메뉴 활성화시
		private void duty5060_Activated(object sender, EventArgs e)
		{
			Page_Refresh();
		}

		private void Page_Refresh()
		{
			if (srTabControl1.SelectedTabPageIndex == 0)
			{
				df.Get5060_AP_YCHG_LISTDatas(SilkRoad.Config.SRConfig.USID, ds);
				grd_ap.DataSource = ds.Tables["5060_AP_YCHG_LIST"];
			}
			else if (srTabControl1.SelectedTabPageIndex == 1)
			{
				df.Get5060_AP_YCHG_LIST2Datas(SilkRoad.Config.SRConfig.USID, ds);
				grd_ap2.DataSource = ds.Tables["5060_AP_YCHG_LIST2"];
			}
			else if (srTabControl1.SelectedTabPageIndex == 2)
			{
				df.Get5060_AP_YCHG_LIST3Datas(SilkRoad.Config.SRConfig.USID, ds);
				grd_ap3.DataSource = ds.Tables["5060_AP_YCHG_LIST3"];
			}
			else if (srTabControl1.SelectedTabPageIndex == 3)
			{
				df.Get5060_AP_YCHG_LIST4Datas(SilkRoad.Config.SRConfig.USID, ds);
				grd_ap4.DataSource = ds.Tables["5060_AP_YCHG_LIST4"];
			}
			else if (srTabControl1.SelectedTabPageIndex == 4)
			{
				string gubn = get_doc_gubn(4);
				df.Get5060_AP_YCHG_LIST5Datas(gubn, SilkRoad.Config.SRConfig.USID, ds);
				grd_ap5.DataSource = ds.Tables["5060_AP_YCHG_LIST5"];
			}
		}
		
		private void grdv_ap_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
		{
            DataRow drow = grdv_ap.GetFocusedDataRow();
			if (drow != null)
			{
				if (grdv_ap.FocusedColumn.Name.ToString() == "col_chk")
				{
					if (drow["CHK_STAT"].ToString() != "1" && SilkRoad.Config.SRConfig.USID != "SAMIL")
						e.Cancel = true;
				}
				else if (grdv_ap.FocusedColumn.Name.ToString() == "col_c_chk")
				{
					if (drow["C_CHK_STAT"].ToString() != "1")
						e.Cancel = true;
				}
			}
		}
		private void grdv_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
		{
			if (e.Info.IsRowIndicator && e.RowHandle >= 0)
				e.Info.DisplayText = (e.RowHandle + 1).ToString();
		}

		// CALL/OT/출장 타이틀 클릭시 등록화면
		private void grd_LinkEdit1_Click(object sender, EventArgs e)
		{			
			DataRow frow = grdv_ap2.GetFocusedDataRow();
			if (frow == null)
				return;
			
			string _gubn = grdv_ap2.GetFocusedRowCellValue("DOC_GUBN").ToString();
			string _doc_no = grdv_ap2.GetFocusedRowCellValue("DOC_NO").ToString();

			int T_index = grdv_ap2.TopRowIndex;
			int R_index = grdv_ap2.FocusedRowHandle;
			
			duty5062 duty5062 = new duty5062("1", _gubn, _doc_no);
			duty5062.Show();
			//duty5062.ShowDialog();
			
			df.Get5060_AP_YCHG_LIST2Datas(SilkRoad.Config.SRConfig.USID, ds);
			grd_ap2.DataSource = ds.Tables["5060_AP_YCHG_LIST2"];
		}

		// OFF,N/밤근무 타이틀 클릭시 등록화면
		private void grd_LinkEdit2_Click(object sender, EventArgs e)
		{			
			DataRow frow = grdv_ap3.GetFocusedDataRow();
			if (frow == null)
				return;
			
			string _gubn = grdv_ap3.GetFocusedRowCellValue("DOC_GUBN").ToString();
			string _doc_no = grdv_ap3.GetFocusedRowCellValue("DOC_NO").ToString();

			int T_index = grdv_ap3.TopRowIndex;
			int R_index = grdv_ap3.FocusedRowHandle;
			
			duty5062 duty5062 = new duty5062("1", _gubn, _doc_no);
			duty5062.Show();
			//duty5062.ShowDialog();
			
			df.Get5060_AP_YCHG_LIST3Datas(SilkRoad.Config.SRConfig.USID, ds);
			grd_ap3.DataSource = ds.Tables["5060_AP_YCHG_LIST3"];
		}

		// 근무표 타이틀 클릭시 등록화면
		private void grd_LinkEdit3_Click(object sender, EventArgs e)
		{			
			DataRow frow = grdv_ap4.GetFocusedDataRow();
			if (frow == null)
				return;
			
			string _gubn = grdv_ap4.GetFocusedRowCellValue("DOC_GUBN").ToString();
			string _doc_no = grdv_ap4.GetFocusedRowCellValue("DOC_NO").ToString();

			int T_index = grdv_ap4.TopRowIndex;
			int R_index = grdv_ap4.FocusedRowHandle;
			
			duty5062 duty5062 = new duty5062("1", _gubn, _doc_no);
			duty5062.Show();
			//duty5062.ShowDialog();
			
			df.Get5060_AP_YCHG_LIST4Datas(SilkRoad.Config.SRConfig.USID, ds);
			grd_ap4.DataSource = ds.Tables["5060_AP_YCHG_LIST4"];
		}

		// 회계 타이틀 클릭시 등록화면
		private void grd_LinkEdit4_Click(object sender, EventArgs e)
		{
			DataRow frow = grdv_ap5.GetFocusedDataRow();
			if (frow == null)
				return;

			string _gubn = grdv_ap5.GetFocusedRowCellValue("DOC_GUBN").ToString();
			string _doc_no = grdv_ap5.GetFocusedRowCellValue("DOC_NO").ToString();

			int T_index = grdv_ap5.TopRowIndex;
			int R_index = grdv_ap5.FocusedRowHandle;

			duty5062 duty5062 = new duty5062("1", _gubn, _doc_no);
			duty5062.Show();
			//duty5062.ShowDialog();

			string gubn = get_doc_gubn(4);
			df.Get5060_AP_YCHG_LIST5Datas(gubn, SilkRoad.Config.SRConfig.USID, ds);
			grd_ap5.DataSource = ds.Tables["5060_AP_YCHG_LIST5"];
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

            if (mode == 11)  //연차,휴가승인 + 반려
            {
				if (ds.Tables["5060_AP_YCHG_LIST"] != null)
				{
					ds.Tables["5060_AP_YCHG_LIST"].AcceptChanges();
					if (ds.Tables["5060_AP_YCHG_LIST"].Select("CHK='1'").Length == 0)
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
			else if (mode == 12)  //승인취소
			{
				if (ds.Tables["5060_AP_YCHG_LIST"] != null)
				{
					ds.Tables["5060_AP_YCHG_LIST"].AcceptChanges();
					if (ds.Tables["5060_AP_YCHG_LIST"].Select("C_CHK='1'").Length == 0)
					{
						MessageBox.Show("선택된 내역이 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
					else
					{
						isError = true;
					}
				}
			}
            return isError;
        }

		#endregion

		#region 9. ETC

		private string get_doc_gubn(int seleceted_page_index)
		{
			string doc_gubn = string.Empty;
			switch (seleceted_page_index)
			{
				case 4:
					{
						doc_gubn = cmb_gubn5.SelectedIndex == 0 ? "8,9,10,11,12,13,14"  //전체
								: cmb_gubn5.SelectedIndex == 1 ? "8"    //일지출예정
								: cmb_gubn5.SelectedIndex == 2 ? "9"    //일지출내역
								: cmb_gubn5.SelectedIndex == 3 ? "10"   //일계표
								: cmb_gubn5.SelectedIndex == 4 ? "11"   //월별지출내역
								: cmb_gubn5.SelectedIndex == 5 ? "12"   //월별수입내역
								: cmb_gubn5.SelectedIndex == 6 ? "13"   //연지출내역
								: cmb_gubn5.SelectedIndex == 7 ? "14"   //연수입내역
								: "-1";
						break;
					}
			}
			return doc_gubn;
		}

		#endregion
	}
}
