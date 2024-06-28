using System;
using System.Data;
using System.Windows.Forms;
using SilkRoad.Common;

namespace DUTY1000
{
    public partial class duty3015 : SilkRoad.Form.Base.FormX
    {
        CommonLibrary clib = new CommonLibrary();

        ClearNEnableControls cec = new ClearNEnableControls();
        public DataSet ds = new DataSet();
        DataProcFunc df = new DataProcFunc();
        SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();
		string dept = "";

        public duty3015(string _dept, string _dept_nm)
        {
            InitializeComponent();
			dept = _dept;
			lb_dept_nm.Text = _dept_nm;
        }

        #region 0. Initialization
		
        private void SetCancel(int stat)
		{
			if (stat == 0)
			{
				dat_yymm.Enabled = true;
				if (ds.Tables["DUTY_TRSLOFF"] != null)
					ds.Tables["DUTY_TRSLOFF"].Clear();
				grd.DataSource = null;
				SetButtonEnable("100");
			}
			else if (stat == 1)
			{
				df.GetS_MSTNURSDatas(dept, cmb_gubn.SelectedIndex, ds);
				grd2.DataSource = ds.Tables["S_MSTNURS"];
				cec.SetClearControls(srPanel9, new string[] { "" });

				lblDept.Text = "-";
				lblName.Text = "-";
				lblSano.Text = "-";
				lblHpno.Text = "-";
				lblEmil.Text = "-";
			}
			else if (stat == 2)
			{
				df.GetS_BEDDatas(ds);
				grd3.DataSource = ds.Tables["S_BED"];
				
				df.GetS_MSTNURS2Datas(ds);
				sl_nurs.Properties.DataSource = ds.Tables["S_MSTNURS2"];
			}
		}

        #endregion

        #region 1 Form

        private void duty3015_Load(object sender, EventArgs e)
        {
			dat_yymm.DateTime = DateTime.Now;
			sl_nurs.EditValue = null;
        }
		private void duty3015_Shown(object sender, EventArgs e)
		{
			btn_proc.PerformClick();
			df.GetS_MSTNURSDatas(dept, cmb_gubn.SelectedIndex, ds);
			grd2.DataSource = ds.Tables["S_MSTNURS"];
		}

        #endregion

        #region 2 Button
		
		//처리
		private void btn_proc_Click(object sender, EventArgs e)
		{
			if (isNoError_um(1))
			{
				string yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 6);
				df.GetS_DUTY_TRSLOFFDatas(dept, yymm, ds);
				if (ds.Tables["S_DUTY_TRSLOFF"].Rows.Count > 0)
					SetButtonEnable("011");
				else
					SetButtonEnable("010");

				int cnt = clib.TextToInt(clib.DateToText(clib.TextToDateLast(clib.DateToText(dat_yymm.DateTime))).Substring(6, 2));
				for (int i = 1; i < cnt + 1; i++)
				{
					string date = yymm + i.ToString().PadLeft(2, '0');
					if (ds.Tables["S_DUTY_TRSLOFF"].Select("DEPT = '" + dept + "' AND SLDT = '" + date + "'").Length == 0)
					{
						DataRow nrow = ds.Tables["S_DUTY_TRSLOFF"].NewRow();
						nrow["DEPT"] = dept;
						nrow["SLDT"] = date;
						nrow["OFF_CNT"] = 0;
						nrow["SLDT_NM"] = date.Substring(0, 4) + "-" + date.Substring(4, 2) + "-" + date.Substring(6, 2);
						nrow["DAY_NM"] = clib.WeekDay(clib.TextToDate(date));
						ds.Tables["S_DUTY_TRSLOFF"].Rows.Add(nrow);
					}
				}
				
				dat_yymm.Enabled = false;
				ds.Tables["S_DUTY_TRSLOFF"].DefaultView.Sort = "SLDT ASC";
				grd.DataSource = ds.Tables["S_DUTY_TRSLOFF"];
			}
		}
		//시간 가져오기
		private void btn_time_Click(object sender, EventArgs e)
		{
			if (ds.Tables["S_DUTY_TRSLOFF"] != null)
			{
				for (int i = 0; i < ds.Tables["S_DUTY_TRSLOFF"].Rows.Count; i++)
				{
					DataRow drow = ds.Tables["S_DUTY_TRSLOFF"].Rows[i];
					drow["OFF_CNT"] = clib.TextToInt(txt_off.Text.ToString());
				}
			}
		}

        //저장
		private void btn_save_Click(object sender, EventArgs e)
		{
			if (srTabControl1.SelectedTabPageIndex == 0)
			{
				#region off제한 저장
				string yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 6);
				if (MessageBox.Show(yymm.Substring(0, 4) + "." + yymm.Substring(4, 2) + "월의 OFF제한 개수를 저장하시겠습니까?", "저장", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
								== DialogResult.OK)
				{
					Cursor = Cursors.WaitCursor;
					int outVal = 0;
					try
					{
						df.GetDUTY_TRSLOFFDatas(dept, yymm, ds);
						for (int i = 0; i < ds.Tables["S_DUTY_TRSLOFF"].Rows.Count; i++)
						{
							DataRow drow = ds.Tables["S_DUTY_TRSLOFF"].Rows[i];
							if (ds.Tables["DUTY_TRSLOFF"].Select("DEPT = '" + drow["DEPT"].ToString() + "' AND SLDT = '" + drow["SLDT"].ToString() + "'").Length > 0)
							{
								DataRow nrow = ds.Tables["DUTY_TRSLOFF"].Select("DEPT = '" + drow["DEPT"].ToString() + "' AND SLDT = '" + drow["SLDT"].ToString() + "'")[0];
								nrow["OFF_CNT"] = clib.TextToDecimal(drow["OFF_CNT"].ToString());
								nrow["REG_DT"] = gd.GetNow();
								nrow["REG_ID"] = SilkRoad.Config.SRConfig.USID;
							}
							else
							{
								DataRow nrow = ds.Tables["DUTY_TRSLOFF"].NewRow();
								nrow["DEPT"] = dept;
								nrow["SLDT"] = drow["SLDT"].ToString();
								nrow["OFF_CNT"] = clib.TextToDecimal(drow["OFF_CNT"].ToString());
								nrow["REG_DT"] = gd.GetNow();
								nrow["REG_ID"] = SilkRoad.Config.SRConfig.USID;
								ds.Tables["DUTY_TRSLOFF"].Rows.Add(nrow);
							}
						}

						string[] tableNames = new string[] { "DUTY_TRSLOFF", };
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

						SetCancel(srTabControl1.SelectedTabPageIndex);
						Cursor = Cursors.Default;
					}
				}
				#endregion
			}
			else if (srTabControl1.SelectedTabPageIndex == 1)
			{
				#region 간호사정보저장
				if (isNoError_um(2))
				{
					Cursor = Cursors.WaitCursor;
					int outVal = 0;
					try
					{
						DataRow hrow;
						string _Flag = "";
						df.GetDUTY_MSTNURSDatas(lblSano.Text.ToString().Trim(), ds);
						if (ds.Tables["DUTY_MSTNURS"].Rows.Count > 0)
						{
							_Flag = "U";
							hrow = ds.Tables["DUTY_MSTNURS"].Rows[0];
						}
						else                    
						{
							_Flag = "C";
							hrow = ds.Tables["DUTY_MSTNURS"].NewRow();
							hrow["PARTCODE"] = "";
						}
                    
						hrow["SAWON_NO"] = lblSano.Text.ToString().Trim();
						hrow["SAWON_NM"] = lblName.Text.ToString().Trim();
						hrow["EXP_LV"] = 0; //cmb_exp.SelectedIndex.ToString();
						hrow["PRE_RN"] = ""; //sl_nurs.EditValue == null ? "" : sl_nurs.EditValue.ToString();
						hrow["RSP_YN"] = ""; //cmb_rsp_yn.EditValue.ToString();
						hrow["RSP_GNMU"] = ""; //sl_gnmu.EditValue == null ? "" : sl_gnmu.EditValue.ToString();
						hrow["SHIFT_WORK"] = cmb_shift_work.SelectedIndex;
						hrow["TM_YN"] = ""; //cmb_tm_yn.EditValue.ToString();
						hrow["TM_FR"] = ""; //txt_tmfr.Text.ToString().Replace(":", "");
						hrow["TM_TO"] = ""; //txt_tmto.Text.ToString().Replace(":", "");
						hrow["FIRST_GNMU"] = ""; //cmb_same1st.EditValue.ToString();
						hrow["MAX_NCNT"]  = clib.TextToInt(cmb_max_n.SelectedIndex.ToString()); 
						hrow["MAX_CCNT"]  = 0; //cmb_max_c.SelectedIndex.ToString(); 
						hrow["ALLOWOFF"] = clib.TextToInt(cmb_allowoff.SelectedIndex.ToString());
						hrow["LIMIT_OFF"] = clib.TextToInt(cmb_limitoff.SelectedIndex.ToString());
						hrow["RETURN_DT"] = ""; //clib.DateToText(dat_rsn_dt.DateTime);
						hrow["CHARGE_YN"] = ""; //cmb_charge.EditValue.ToString();  

						hrow["EXP_YEAR"] = clib.TextToDecimal(txt_exp.Text.ToString());
						hrow["EXP_Y2"] = clib.TextToDecimal(txt_exp2.Text.ToString());
						hrow["EXP_Y3"] = clib.TextToDecimal(txt_exp3.Text.ToString());
						hrow["EXP_Y4"] = clib.TextToDecimal(txt_exp4.Text.ToString());

						hrow["STAT"] = cmb_stat.SelectedIndex + 1;
						hrow["LDAY"] = clib.DateToText(dat_lday.DateTime);                          
						hrow["USID"] = SilkRoad.Config.SRConfig.USID;

						if (_Flag == "C")  //신규
						{
							hrow["EXP_YEAR"] = 0;
							hrow["INDT"] = gd.GetNow();
							hrow["UPDT"] = " ";
							hrow["PSTY"] = "A";

							ds.Tables["DUTY_MSTNURS"].Rows.Add(hrow);
						}
						else //수정
						{
							hrow["UPDT"] = gd.GetNow();
							hrow["PSTY"] = "U";
						}
						string[] tableNames = new string[] { "DUTY_MSTNURS" };
						SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
						outVal = cmd.setUpdate(ref ds, tableNames, null);

						if (outVal <= 0)                    
							MessageBox.Show("저장된 내용이 없습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);                    
						else                    
							MessageBox.Show("해당 간호사 정보가 저장되었습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);                    
					}
					catch (Exception ec)
					{
						MessageBox.Show(ec.Message, "저장오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
					finally
					{
						SetCancel(srTabControl1.SelectedTabPageIndex);
						Cursor = Cursors.Default;
					}
				}
				#endregion
			}
		}

		//삭제
		private void btn_del_Click(object sender, EventArgs e)
		{
			string yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 6);
			df.GetDUTY_TRSLOFFDatas(dept, yymm, ds);
			if (ds.Tables["DUTY_TRSLOFF"].Rows.Count > 0)
			{
				DialogResult dr = MessageBox.Show(yymm.Substring(0, 4) + "." + yymm.Substring(4, 2) + "월의 OFF제한 개수를 모두 삭제하시겠습니까?", "삭제여부", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
				if (dr == DialogResult.OK)
				{
					int outVal = 0;
					try
					{
						for (int i = 0; i < ds.Tables["DUTY_TRSLOFF"].Rows.Count; i++)
						{
							ds.Tables["DUTY_TRSLOFF"].Rows[i].Delete();
						}

						string[] tableNames = new string[] { "DUTY_TRSLOFF" };
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
							MessageBox.Show(yymm.Substring(0, 4) + "." + yymm.Substring(4, 2) + "월의 OFF제한 개수를 삭제되었습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);

						SetCancel(srTabControl1.SelectedTabPageIndex);
						Cursor = Cursors.Default;
					}
				}
			}
			else
			{
                MessageBox.Show(yymm.Substring(0, 4) + "." + yymm.Substring(4, 2) + "월은 삭제할 내역이 없습니다.", "삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btn_canc_Click(object sender, EventArgs e)
		{
			SetCancel(srTabControl1.SelectedTabPageIndex);
		}

		//간호사정보 조회
		private void btn_search_Click(object sender, EventArgs e)
		{			
			df.GetS_MSTNURSDatas(dept, cmb_gubn.SelectedIndex, ds);
			grd2.DataSource = ds.Tables["S_MSTNURS"];
		}

		private void btn_add_Click(object sender, EventArgs e)
		{
			if (isNoError_um(3))
			{
				Cursor = Cursors.WaitCursor;
				int outVal = 0;
				try
				{
					DataRow hrow;
					df.GetDUTY_TRSPLAN_ETCDatas(sl_nurs.EditValue.ToString().Trim(), ds);
					if (ds.Tables["DUTY_TRSPLAN_ETC"].Rows.Count > 0)
					{
						hrow = ds.Tables["DUTY_TRSPLAN_ETC"].Rows[0];
						hrow["UPDT"] = gd.GetNow();
						hrow["USID"] = SilkRoad.Config.SRConfig.USID;
						hrow["PSTY"] = "U";
					}
					else                    
					{
						hrow = ds.Tables["DUTY_TRSPLAN_ETC"].NewRow();
						hrow["DEPTCODE"] = "A001";
						hrow["SAWON_NO"] = sl_nurs.EditValue.ToString().Trim();
						hrow["SAWON_NM"] = ds.Tables["S_MSTNURS2"].Select("CODE = '" + sl_nurs.EditValue.ToString().Trim() + "'")[0]["NAME"].ToString();
						hrow["INDT"] = gd.GetNow();
						hrow["UPDT"] = "";
						hrow["USID"] = SilkRoad.Config.SRConfig.USID;
						hrow["PSTY"] = "A";
						ds.Tables["DUTY_TRSPLAN_ETC"].Rows.Add(hrow);
					}
					string[] tableNames = new string[] { "DUTY_TRSPLAN_ETC" };
					SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
					outVal = cmd.setUpdate(ref ds, tableNames, null);

					if (outVal <= 0)                    
						MessageBox.Show("저장된 내용이 없습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);                    
					else                    
						MessageBox.Show("해당직원이 추가되었습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);                    
				}
				catch (Exception ec)
				{
					MessageBox.Show(ec.Message, "저장오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				finally
				{
					SetCancel(srTabControl1.SelectedTabPageIndex);
					Cursor = Cursors.Default;
				}
			}
		}
		//직원제회
		private void btn_remove_Click(object sender, EventArgs e)
		{
			if (isNoError_um(3))
			{
				Cursor = Cursors.WaitCursor;
				int outVal = 0;
				try
				{
					DataRow hrow;
					df.GetDUTY_TRSPLAN_ETCDatas(sl_nurs.EditValue.ToString().Trim(), ds);
					if (ds.Tables["DUTY_TRSPLAN_ETC"].Rows.Count > 0)
					{
						hrow = ds.Tables["DUTY_TRSPLAN_ETC"].Rows[0];
						hrow["UPDT"] = gd.GetNow();
						hrow["USID"] = SilkRoad.Config.SRConfig.USID;
						hrow["PSTY"] = "D";
						string[] tableNames = new string[] { "DUTY_TRSPLAN_ETC" };
						SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
						outVal = cmd.setUpdate(ref ds, tableNames, null);
					}

					if (outVal <= 0)                    
						MessageBox.Show("제외할 직원이 없습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);                    
					else                    
						MessageBox.Show("해당직원이 제외되었습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);                    
				}
				catch (Exception ec)
				{
					MessageBox.Show(ec.Message, "제외오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				finally
				{
					SetCancel(srTabControl1.SelectedTabPageIndex);
					Cursor = Cursors.Default;
				}
			}
		}
        #endregion

        #region 3 EVENT

        private void duty3015_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)   //종료
            {
                btn_exit.PerformClick();
            }
        }
		
		private void srTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
		{
			if (srTabControl1.SelectedTabPageIndex == 0)
			{
				btn_save.Visible = true;
				btn_del.Visible = true;
			}
			else if (srTabControl1.SelectedTabPageIndex == 1)
			{
				btn_save.Visible = true;
				btn_del.Visible = true;
				btn_del.Enabled = false;
			}
			else if (srTabControl1.SelectedTabPageIndex == 2)
			{
				SetCancel(srTabControl1.SelectedTabPageIndex);
				btn_save.Visible = false;
				btn_del.Visible = false;
			}
		}
		
		//더블클릭시 등록,수정
		private void grdv2_DoubleClick(object sender, EventArgs e)
		{
			DataRow drow2 = grdv2.GetFocusedDataRow();
            if (drow2 == null)
                return;
			
			lblDept.Text = drow2["DEPT_NM"].ToString();
            lblName.Text = drow2["SAWON_NM"].ToString();
			lblSano.Text = drow2["SAWON_NO"].ToString();
			lblHpno.Text = drow2["HPNO"].ToString();
			lblEmil.Text = drow2["EMAIL_ID"].ToString();
			
			df.GetDUTY_MSTNURSDatas(lblSano.Text.ToString().Trim(), ds);
			if (ds.Tables["DUTY_MSTNURS"].Rows.Count > 0)
			{
				DataRow drow = ds.Tables["DUTY_MSTNURS"].Rows[0];
				cmb_shift_work.SelectedIndex = clib.TextToInt(drow["SHIFT_WORK"].ToString());
				cmb_max_n.SelectedIndex = clib.TextToInt(drow["MAX_NCNT"].ToString());
				cmb_allowoff.SelectedIndex = clib.TextToInt(drow["ALLOWOFF"].ToString());
				cmb_limitoff.SelectedIndex = clib.TextToInt(drow["LIMIT_OFF"].ToString());
				txt_exp.Text = drow["EXP_YEAR"].ToString();
				txt_exp2.Text = drow["EXP_Y2"].ToString();
				txt_exp3.Text = drow["EXP_Y3"].ToString();
				txt_exp4.Text = drow["EXP_Y4"].ToString();

				cmb_stat.SelectedIndex = clib.TextToInt(drow["STAT"].ToString()) < 1 ? 0 : clib.TextToInt(drow["STAT"].ToString()) - 1;
				if (drow["LDAY"].ToString() != "")
					dat_lday.DateTime = clib.TextToDate(drow["LDAY"].ToString());
			}
			else
			{
				cmb_shift_work.SelectedIndex = 1;
				cmb_max_n.SelectedIndex = 6;
				cmb_allowoff.SelectedIndex = 9;
				cmb_limitoff.SelectedIndex = 3;
				cmb_stat.SelectedIndex = 0;
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
            if (mode == 1)  //처리
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
            else if (mode == 2)  //저장
            {
                if (lblSano.Text == null)
                {
                    MessageBox.Show("직원을 선택하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
				{ 
					isError = true;
                }
            }
            else if (mode == 3)  //추가
            {
                if (sl_nurs.EditValue == null)
                {
                    MessageBox.Show("직원을 선택하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sl_nurs.Focus();
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
		}

		#endregion

	}
}
