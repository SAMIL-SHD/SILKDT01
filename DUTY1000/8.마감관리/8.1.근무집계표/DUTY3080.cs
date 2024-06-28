using System;
using System.Data;
using System.Windows.Forms;
using SilkRoad.Common;

namespace DUTY1000
{
	public partial class duty3080 : SilkRoad.Form.Base.FormX
	{
		CommonLibrary clib = new CommonLibrary();

		ClearNEnableControls cec = new ClearNEnableControls();
		public DataSet ds = new DataSet();
		DataProcFunc df = new DataProcFunc();
		SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();
		static DataProcessing dp = new DataProcessing();
        private string ends_yn = "";

		public duty3080()
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
			df.GetSEARCH_EMBSDatas(ds);
			sl_embs.Properties.DataSource = ds.Tables["SEARCH_EMBS"];

			df.GetSEARCH_DEPTDatas(ds);
			sl_s_dept.Properties.DataSource = ds.Tables["SEARCH_DEPT"];
		}

		#endregion

		#region 1 Form

		private void duty3080_Load(object sender, EventArgs e)
		{
			SetCancel();
		}
		private void duty3080_Shown(object sender, EventArgs e)
		{
			dat_p_yymm.DateTime = DateTime.Now.AddMonths(-1);
			cmb_gubn.SelectedIndex = 0;

			dat_yymm.DateTime = DateTime.Now;
			dat_s_frmm.DateTime = DateTime.Now;
			dat_s_tomm.DateTime = DateTime.Now;
			
			sl_embs.EditValue = null;
			sl_s_dept.EditValue = null;

			info_Search();
		}

		private void info_Search()
		{
			//수당설정된 컬럼만 보이게 하고
			df.GetSEARCH_INFOSD02Datas(ds);
			if (ds.Tables["SEARCH_INFOSD02"].Rows.Count > 0)
			{
				for (int i = 0; i < grdv_search.Columns.Count; i++)
				{
					if (grdv_search.Columns[i].Caption.Substring(0, 1) == "A")
					{
						grdv_search.Columns[i].Visible = false;
						for (int j = 1; j <= 14; j++)
						{
							if (j != 9)
							{
								if (grdv_search.Columns[i].Caption == ds.Tables["SEARCH_INFOSD02"].Rows[0]["A0" + j].ToString()
                                    || grdv_search.Columns[i].Caption == "A012")
								{
									grdv_search.Columns[i].Visible = true;
									break;
								}
							}
						}
					}
				}

				for (int i = 0; i < grdv_end.Columns.Count; i++)
				{
					if (grdv_end.Columns[i].Caption.Substring(0, 1) == "A")
					{
						grdv_end.Columns[i].Visible = false;
						for (int j = 1; j <= 14; j++)
						{
							if (j != 9)
							{
								if (grdv_end.Columns[i].Caption == ds.Tables["SEARCH_INFOSD02"].Rows[0]["A0" + j].ToString()
                                    || grdv_end.Columns[i].Caption == "A012")
                                {
									grdv_end.Columns[i].Visible = true;
									break;
								}
							}
						}
					}
				}
			}

			//수당명칭 가져와서 설정
			df.GetSEARCH_INFOWAGEDatas(ds);
			foreach (DataRow drow in ds.Tables["SEARCH_INFOWAGE"].Rows) //for (int i = 0; i < ds.Tables["SEARCH_INFOWAGE"].Rows.Count; i++)
			{
				for (int i = 0; i < grdv_search.Columns.Count; i++)
				{
					if (grdv_search.Columns[i].Caption == drow["IFWGCODE"].ToString())
						grdv_search.Columns[i].Caption = drow["IFWGNAME"].ToString();
				}

				for (int i = 0; i < grdv_end.Columns.Count; i++)
				{
					if (grdv_end.Columns[i].Caption == drow["IFWGCODE"].ToString())
						grdv_end.Columns[i].Caption = drow["IFWGNAME"].ToString();
				}
			}

			////수당설정된 컬럼만 보이게 하고
			//for (int i = 0; i < grdv_search.Columns.Count; i++)
			//{
			//	if (grdv_search.Columns[i].Caption.Substring(0, 1) == "A")
			//		grdv_search.Columns[i].Visible = false;
			//}
		}

		#endregion

		#region 2 Button

		//마감처리
		private void btn_proc_Click(object sender, EventArgs e)
		{
			if (isNoError_um(1))
			{
				string yymm = clib.DateToText(dat_p_yymm.DateTime).Substring(0, 6);
				string gubn = cmb_gubn.SelectedIndex.ToString();

				df.GetWORK_3080Datas(yymm, gubn, ds);
				grd1.DataSource = ds.Tables["WORK_3080"];

				if (ds.Tables["WORK_3080"].Rows.Count == 0)
					MessageBox.Show("처리할 내역이 없습니다.", "없음", MessageBoxButtons.OK, MessageBoxIcon.Error);

				dat_end.DateTime = dat_p_yymm.DateTime.AddMonths(1);  //수당은 근무월 다음달 25일에 급여처리
			}
		}
		//취소
		private void btn_canc_Click(object sender, EventArgs e)
		{
			if (ds.Tables["WORK_3080"] != null)
				ds.Tables["WORK_3080"].Clear();
			grd1.DataSource = null;
		}
		//마감등록
		private void btn_end_Click(object sender, EventArgs e)
		{
			if (isNoError_um(2))
			{
				DialogResult dr = MessageBox.Show("조회된 내역을 마감등록하시겠습니까?", "등록여부", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
				if (dr == DialogResult.OK)
				{
					Cursor = Cursors.WaitCursor;
					int outVal = 0;
					try
					{
						if (ds.Tables["WORK_3080"].Select("END_YYMM_NM = ''").Length == 0)//.Rows.Count < 1)
						{
							MessageBox.Show("마감처리할 데이터가 없습니다.", "마감오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
						else
						{
							df.GetSEARCH_INFOSD02Datas(ds);
							df.GetS_DUTY_MSTWGPCDatas(ds);
							DataRow nrow;
							foreach (DataRow drow in ds.Tables["WORK_3080"].Rows)
							{
								if (drow["END_YYMM_NM"].ToString() == "" && clib.TextToDecimal(drow["SD_AMT"].ToString()) != 0)  //금액이 있는것만
								{
									#region 환경설정 수당코드 가져오기
									string sd_code = "", sd_code2 = "", sd_code3 = "";
                                    int sd_gubn = clib.TextToInt(drow["GUBN"].ToString());

                                    DataRow irow = ds.Tables["SEARCH_INFOSD02"].Rows[0];
									string time_code = irow["A012"].ToString().Substring(2, 2);
									switch (sd_gubn)
									{
										case 1:
											sd_code = irow["A01"].ToString().Substring(2, 2);
											break;
										case 2:
											sd_code = irow["A07"].ToString().Substring(2, 2);
											break;
										case 3:
											sd_code = irow["A02"].ToString().Substring(2, 2);
											break;
										case 4:
											sd_code = irow["A03"].ToString().Substring(2, 2);
											break;
										case 5:
											sd_code = irow["A04"].ToString().Substring(2, 2);
											break;
										case 6:
											sd_code = irow["A05"].ToString().Substring(2, 2);
											break;
										case 7:
											sd_code = "15";
											break;
										case 8:  //간호간병인센티브
											sd_code = "16";
                                            sd_code2 = "15";
                                            sd_code3 = "12";
                                            break;
										case 9:
											sd_code = irow["A06"].ToString().Substring(2, 2);
											break;
										case 10:
											sd_code = irow["A08"].ToString().Substring(2, 2);
											break;
									}
									#endregion

									if (ds.Tables["S_DUTY_MSTWGPC"].Select("SAWON_NO = '" + drow["SAWON_NO"].ToString().Trim() + "' AND YYMM = '" + drow["SLDT"].ToString().Substring(0, 6) + "'").Length > 0)
									{
										nrow = ds.Tables["S_DUTY_MSTWGPC"].Select("SAWON_NO = '" + drow["SAWON_NO"].ToString().Trim() + "' AND YYMM = '" + drow["SLDT"].ToString().Substring(0, 6) + "' ")[0];

										if (clib.TextToDecimal(drow["TIME_AMT"].ToString()) != 0)
											nrow["TIME_AMT"] = clib.TextToDecimal(drow["TIME_AMT"].ToString());
										if (drow["GUBN"].ToString() == "6" && clib.TextToDecimal(drow["T_AMT"].ToString()) != 0)
											nrow["NIGHT_AMT"] = clib.TextToDecimal(drow["T_AMT"].ToString());

										nrow["WGPCSD" + sd_code] = clib.TextToDecimal(nrow["WGPCSD" + sd_code].ToString()) + clib.TextToDecimal(drow["SD_AMT"].ToString());

										if (clib.TextToDecimal(nrow["TIME_AMT"].ToString()) != 0)
											nrow["WGPCSD" + time_code] = clib.TextToDecimal(drow["TIME_AMT"].ToString());

										nrow["WGPCGT01"] = clib.TextToDecimal(nrow["WGPCGT01"].ToString()) + clib.TextToDecimal(drow["CALL_CNT"].ToString());
										nrow["WGPCGT02"] = clib.TextToDecimal(nrow["WGPCGT02"].ToString()) + clib.TextToDecimal(drow["CALL_DAY_TIME"].ToString());
										nrow["WGPCGT03"] = clib.TextToDecimal(nrow["WGPCGT03"].ToString()) + clib.TextToDecimal(drow["CALL_N_CNT"].ToString());
										nrow["WGPCGT04"] = clib.TextToDecimal(nrow["WGPCGT04"].ToString()) + clib.TextToDecimal(drow["CALL_N_TIME"].ToString());
										nrow["WGPCGT05"] = clib.TextToDecimal(nrow["WGPCGT05"].ToString()) + clib.TextToDecimal(drow["DANG_DTIME"].ToString());
										nrow["WGPCGT06"] = clib.TextToDecimal(nrow["WGPCGT06"].ToString()) + clib.TextToDecimal(drow["DANG_NTIME"].ToString());
										nrow["WGPCGT07"] = clib.TextToDecimal(nrow["WGPCGT07"].ToString()) + clib.TextToDecimal(drow["INS_HOLI_CNT"].ToString());
										nrow["WGPCGT08"] = clib.TextToDecimal(nrow["WGPCGT08"].ToString()) + clib.TextToDecimal(drow["OFF_CNT"].ToString());
										nrow["WGPCGT09"] = clib.TextToDecimal(nrow["WGPCGT09"].ToString()) + clib.TextToDecimal(drow["NIGHT_CNT"].ToString());
										nrow["WGPCGT10"] = clib.TextToDecimal(nrow["WGPCGT10"].ToString()) + clib.TextToDecimal(drow["FXOT_TIME"].ToString());
										nrow["WGPCGT11"] = clib.TextToDecimal(nrow["WGPCGT11"].ToString()) + clib.TextToDecimal(drow["OT_DAY"].ToString());
										nrow["WGPCGT12"] = clib.TextToDecimal(nrow["WGPCGT12"].ToString()) + clib.TextToDecimal(drow["OT_NIGHT"].ToString());
										nrow["WGPCGT13"] = clib.TextToDecimal(nrow["WGPCGT13"].ToString()) + clib.TextToDecimal(drow["MIYC_CNT"].ToString());
										nrow["WGPCGT14"] = clib.TextToDecimal(nrow["WGPCGT14"].ToString()) + clib.TextToDecimal(drow["HOLI_CNT"].ToString());
									}
									else
									{
										nrow = ds.Tables["S_DUTY_MSTWGPC"].NewRow();
										nrow["SAWON_NO"] = drow["SAWON_NO"].ToString().Trim();
										nrow["YYMM"] = drow["SLDT"].ToString().Substring(0, 6);

										nrow["TIME_AMT"] = clib.TextToDecimal(drow["TIME_AMT"].ToString());
										if (drow["GUBN"].ToString() == "6")
											nrow["NIGHT_AMT"] = clib.TextToDecimal(drow["T_AMT"].ToString());

										for (int i = 1; i <= 50; i++)
										{
											nrow["WGPCSD" + i.ToString().PadLeft(2, '0')] = 0;
											if (i <= 30)
												nrow["WGPCGT" + i.ToString().PadLeft(2, '0')] = 0;
										}
										//string sd_code = ds.Tables["SEARCH_INFOSD02"].Rows[0]["A0" + drow["GUBN"].ToString()].ToString().Substring(2, 2);

                                        if (sd_gubn == 8)
                                        {
                                            nrow["WGPCSD" + sd_code] = clib.TextToDecimal(drow["SD_AMT1"].ToString());
                                            nrow["WGPCSD" + sd_code2] = clib.TextToDecimal(drow["SD_AMT2"].ToString());
                                            nrow["WGPCSD" + sd_code3] = clib.TextToDecimal(drow["SD_AMT3"].ToString());
                                        }
                                        else
										    nrow["WGPCSD" + sd_code] = clib.TextToDecimal(drow["SD_AMT"].ToString());

										nrow["WGPCSD" + time_code] = clib.TextToDecimal(drow["TIME_AMT"].ToString());

										nrow["WGPCGT01"] = clib.TextToDecimal(drow["CALL_CNT"].ToString());
										nrow["WGPCGT02"] = clib.TextToDecimal(drow["CALL_DAY_TIME"].ToString());
										nrow["WGPCGT03"] = clib.TextToDecimal(drow["CALL_N_CNT"].ToString());
										nrow["WGPCGT04"] = clib.TextToDecimal(drow["CALL_N_TIME"].ToString());
										nrow["WGPCGT05"] = clib.TextToDecimal(drow["DANG_DTIME"].ToString());
										nrow["WGPCGT06"] = clib.TextToDecimal(drow["DANG_NTIME"].ToString());
										nrow["WGPCGT07"] = clib.TextToDecimal(drow["INS_HOLI_CNT"].ToString());
										nrow["WGPCGT08"] = clib.TextToDecimal(drow["OFF_CNT"].ToString());
										nrow["WGPCGT09"] = clib.TextToDecimal(drow["NIGHT_CNT"].ToString());
										nrow["WGPCGT10"] = clib.TextToDecimal(drow["FXOT_TIME"].ToString());
										nrow["WGPCGT11"] = clib.TextToDecimal(drow["OT_DAY"].ToString());
										nrow["WGPCGT12"] = clib.TextToDecimal(drow["OT_NIGHT"].ToString());
										nrow["WGPCGT13"] = clib.TextToDecimal(drow["MIYC_CNT"].ToString());
										nrow["WGPCGT14"] = clib.TextToDecimal(drow["HOLI_CNT"].ToString());

										ds.Tables["S_DUTY_MSTWGPC"].Rows.Add(nrow);
									}

									df.GetDUTY_MSTWGPC_ENDDatas(clib.DateToText(dat_end.DateTime).Substring(0, 6), drow["SLDT"].ToString().Substring(0, 6), drow["SAWON_NO"].ToString().Trim(), drow["GUBN"].ToString(), ds);
									if (ds.Tables["DUTY_MSTWGPC_END"].Rows.Count == 0)
									{
										DataRow erow = ds.Tables["DUTY_MSTWGPC_END"].NewRow();
										erow["END_YYMM"] = clib.DateToText(dat_end.DateTime).Substring(0, 6);
										erow["SAWON_NO"] = drow["SAWON_NO"].ToString().Trim();
										erow["YYMM"] = drow["SLDT"].ToString().Substring(0, 6);
										erow["GUBN"] = drow["GUBN"].ToString().Trim();
										erow["REG_DT"] = gd.GetNow();
										erow["REG_ID"] = SilkRoad.Config.SRConfig.USID;

										ds.Tables["DUTY_MSTWGPC_END"].Rows.Add(erow);
										string[] tb_nm = new string[] { "DUTY_MSTWGPC_END" };
										SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd0 = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
										cmd0.setUpdate(ref ds, tb_nm, null);
									}								
								}									
							}

							DataRow nrow2;
							foreach (DataRow drow in ds.Tables["S_DUTY_MSTWGPC"].Rows)
							{
								df.GetDUTY_MSTWGPCDatas(clib.DateToText(dat_end.DateTime).Substring(0, 6), drow["YYMM"].ToString().Trim(), drow["SAWON_NO"].ToString().Trim(), ds);
								if (ds.Tables["DUTY_MSTWGPC"].Rows.Count > 0)
								{
									nrow2 = ds.Tables["DUTY_MSTWGPC"].Rows[0];
									for (int i = 1; i <= 50; i++)
									{
										nrow2["WGPCSD" + i.ToString().PadLeft(2, '0')] = clib.TextToDecimal(nrow2["WGPCSD" + i.ToString().PadLeft(2, '0')].ToString()) + clib.TextToDecimal(drow["WGPCSD" + i.ToString().PadLeft(2, '0')].ToString());
										if (i <= 30)
											nrow2["WGPCGT" + i.ToString().PadLeft(2, '0')] = clib.TextToDecimal(nrow2["WGPCGT" + i.ToString().PadLeft(2, '0')].ToString()) + clib.TextToDecimal(drow["WGPCGT" + i.ToString().PadLeft(2, '0')].ToString());
									}
									nrow2["UPDT"] = gd.GetNow();
									nrow2["PSTY"] = "U";
								}
								else
								{
									nrow2 = ds.Tables["DUTY_MSTWGPC"].NewRow();
									nrow2["END_YYMM"] = clib.DateToText(dat_end.DateTime).Substring(0, 6);
									nrow2["SAWON_NO"] = drow["SAWON_NO"].ToString().Trim();
									nrow2["YYMM"] = drow["YYMM"].ToString().Trim();
									for (int i = 1; i <= 50; i++)
									{
										nrow2["WGPCSD" + i.ToString().PadLeft(2, '0')] = clib.TextToDecimal(drow["WGPCSD" + i.ToString().PadLeft(2, '0')].ToString());
										if (i <= 30)
											nrow2["WGPCGT" + i.ToString().PadLeft(2, '0')] = clib.TextToDecimal(drow["WGPCGT" + i.ToString().PadLeft(2, '0')].ToString());
									}
									nrow2["INDT"] = gd.GetNow();
									nrow2["UPDT"] = "";
									nrow2["PSTY"] = "A";
									ds.Tables["DUTY_MSTWGPC"].Rows.Add(nrow2);
								}
								nrow2["TIME_AMT"] = clib.TextToDecimal(drow["TIME_AMT"].ToString());
								nrow2["NIGHT_AMT"] = clib.TextToDecimal(drow["NIGHT_AMT"].ToString());
								nrow2["USID"] = SilkRoad.Config.SRConfig.USID;

								string[] tableNames = new string[] { "DUTY_MSTWGPC" };
								SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
								outVal += cmd.setUpdate(ref ds, tableNames, null);
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
							MessageBox.Show("마감이 " + String.Format("{0:#,###}", outVal) + "건 저장되었습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);

						SetCancel();
						//Search();

						if (ds.Tables["WORK_3080"] != null)
						{
							string yymm = clib.DateToText(dat_p_yymm.DateTime).Substring(0, 6);
							//string dept = sl_dept.EditValue == null ? "%" : sl_dept.EditValue.ToString();
							//string sabn = sl_p_embs.EditValue == null ? "%" : sl_p_embs.EditValue.ToString();
							df.GetWORK_3080Datas(yymm, cmb_gubn.SelectedIndex.ToString(), ds);
							grd1.DataSource = ds.Tables["WORK_3080"];
						}
						Cursor = Cursors.Default;
					}
				}
			}
		}

		//조회
		private void btn_search_Click(object sender, EventArgs e)
		{
			if (isNoError_um(3))
			{
				string yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 6);
				string sabn = sl_embs.EditValue == null ? "%" : sl_embs.EditValue.ToString();
				df.GetSEARCH_3080Datas(yymm, sabn, ds);
				grd_end.DataSource = ds.Tables["SEARCH_3080"];

				if (ds.Tables["SEARCH_3080"].Rows.Count == 0)
					MessageBox.Show("조회된 내역이 없습니다.", "없음", MessageBoxButtons.OK, MessageBoxIcon.Error);

				info_Search();
			}
		}
		//삭제
		private void btn_s_del_Click(object sender, EventArgs e)
		{
			if (isNoError_um(3))
			{
				DialogResult dr = MessageBox.Show("조회된 내역을 삭제하시겠습니까?", "삭제여부", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
				if (dr == DialogResult.OK)
				{
					Cursor = Cursors.WaitCursor;
					int outVal = 0;
					try
					{
						if (grdv_end.RowCount == 0)
						{
							MessageBox.Show("삭제할 데이터가 없습니다.", "삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
						else
						{
							for (int i = 0; i < grdv_end.RowCount; i++)
							{
								if (grdv_end.GetVisibleRowHandle(i) > -1)
								{
									DataRow drow = grdv_end.GetDataRow(grdv_end.GetVisibleRowHandle(i));
									df.GetDUTY_MSTWGPCDatas(drow["END_YYMM"].ToString(), drow["YYMM"].ToString(), drow["SAWON_NO"].ToString(), ds);
									if (ds.Tables["DUTY_MSTWGPC"].Rows.Count != 0)
										ds.Tables["DUTY_MSTWGPC"].Rows[0].Delete();

									string[] tableNames = new string[] { "DUTY_MSTWGPC" };
									SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
									outVal += cmd.setUpdate(ref ds, tableNames, null);

									#region 환경설정 수당코드 가져오기
									string gubn = "";
									DataRow irow = ds.Tables["SEARCH_INFOSD02"].Rows[0];
									for (int j = 1; j <= 50; j++)
									{
										if (clib.TextToDecimal(drow["WGPCSD" + j.ToString().PadLeft(2, '0')].ToString()) != 0)
										{
											switch (j.ToString().PadLeft(2, '0'))
											{
												case "08":
													gubn = "1";
													break;
												case "20":
													gubn = "2";
													break;
												case "09":
													gubn = "3";
													break;
												case "11":
													gubn = "4";
													break;
												case "13":
													gubn = "5";
													break;
												case "18":
													gubn = "6";
													break;
                                                case "12":
                                                case "16":
                                                    gubn = "8";
                                                    break;
                                                case "15":
													gubn = "7";
													break;
												case "19":
													gubn = "9";
													break;
												case "17":
													gubn = "10";
													break;
											}
											df.GetDUTY_MSTWGPC_ENDDatas(drow["END_YYMM"].ToString(), drow["YYMM"].ToString(), drow["SAWON_NO"].ToString(), gubn, ds);
											if (ds.Tables["DUTY_MSTWGPC_END"].Rows.Count > 0)
											{
												ds.Tables["DUTY_MSTWGPC_END"].Rows[0].Delete();
												string[] tb_nm = new string[] { "DUTY_MSTWGPC_END" };
												cmd.setUpdate(ref ds, tb_nm, null);
											}
										}
									}
									#endregion
								}
							}
						}
					}
					catch (Exception ec)
					{
						MessageBox.Show(ec.Message, "삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					finally
					{
						if (outVal > 0)
							MessageBox.Show(String.Format("{0:#,###}", outVal) + "건의 마감내용이 삭제되었습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);

						SetCancel();
						if (ds.Tables["SEARCH_3080"] != null)
							ds.Tables["SEARCH_3080"].Clear();
						grd_end.DataSource = null;
						//Search();
						Cursor = Cursors.Default;
					}
				}
			}
		}
		//급여전송(급여변동항목,월근태등록)
		private void btn_wage_Click(object sender, EventArgs e)
		{
			if (isNoError_um(4))
			{
				string yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 4) + "." + clib.DateToText(dat_yymm.DateTime).Substring(4, 2);
				DialogResult dr = MessageBox.Show("조회된 " + yymm + " 내역을 인사급여로 전송하시겠습니까?", "전송여부", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
				if (dr == DialogResult.OK)
				{
					Cursor = Cursors.WaitCursor;
					int outVal = 0;
					try
					{
						if (ds.Tables["SEARCH_3080"].Rows.Count < 1)
						{
							MessageBox.Show("전송할 데이터가 없습니다.", "전송오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
						else
						{
							DataRow srow;
							DataTable dt = ds.Tables["SEARCH_3080"].Clone();
							dp.AddDatatable2Dataset("C_3080", dt, ref ds);

							foreach (DataRow drow in ds.Tables["SEARCH_3080"].Rows)
							{
								if (ds.Tables["C_3080"].Select("END_YYMM = '" + drow["END_YYMM"].ToString() + "' AND SAWON_NO = '" + drow["SAWON_NO"].ToString().Trim() + "'").Length > 0)
								{
									srow = ds.Tables["C_3080"].Select("END_YYMM = '" + drow["END_YYMM"].ToString() + "' AND SAWON_NO = '" + drow["SAWON_NO"].ToString().Trim() + "'")[0];
									for (int i = 1; i <= 30; i++)
									{
										if (i <= 13)
											srow["WGPCGT" + i.ToString().PadLeft(2, '0')] = clib.TextToDecimal(srow["WGPCGT" + i.ToString().PadLeft(2, '0')].ToString()) + clib.TextToDecimal(drow["WGPCGT" + i.ToString().PadLeft(2, '0')].ToString());
										srow["WGPCSD" + i.ToString().PadLeft(2, '0')] = clib.TextToDecimal(srow["WGPCSD" + i.ToString().PadLeft(2, '0')].ToString()) + clib.TextToDecimal(drow["WGPCSD" + i.ToString().PadLeft(2, '0')].ToString());
									}
								}
								else
								{
									srow = ds.Tables["C_3080"].NewRow();
									srow["END_YYMM"] = drow["END_YYMM"].ToString();
									srow["SAWON_NO"] = drow["SAWON_NO"].ToString().Trim();
									for (int i = 1; i <= 30; i++)
									{
										if (i <= 13)
											srow["WGPCGT" + i.ToString().PadLeft(2, '0')] = clib.TextToDecimal(drow["WGPCGT" + i.ToString().PadLeft(2, '0')].ToString());
										srow["WGPCSD" + i.ToString().PadLeft(2, '0')] = clib.TextToDecimal(drow["WGPCSD" + i.ToString().PadLeft(2, '0')].ToString());
									}
									ds.Tables["C_3080"].Rows.Add(srow);
								}
							}
							//string yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 6);WGPCGT01~13
							//string dept = sl_s_dept.EditValue == null ? "%" : sl_s_dept.EditValue.ToString();
							//df.GetDUTY_MSTWGPCDatas(yymm, dept, "%", ds);
							DataRow nrow;
							for (int z = 0; z < ds.Tables["C_3080"].Rows.Count; z++)
							{
								DataRow drow = ds.Tables["C_3080"].Rows[z];
								df.GetMSTWGPCDatas(drow["END_YYMM"].ToString(), drow["SAWON_NO"].ToString().Trim(), ds);
								if (ds.Tables["MSTWGPC"].Select("WGPCYYMM = '" + drow["END_YYMM"].ToString() + "' AND WGPCSABN = '" + drow["SAWON_NO"].ToString().Trim() + "'").Length > 0)
								{
									nrow = ds.Tables["MSTWGPC"].Select("WGPCYYMM = '" + drow["END_YYMM"].ToString() + "' AND WGPCSABN = '" + drow["SAWON_NO"].ToString().Trim() + "'")[0];

									nrow["WGPCUPDT"] = gd.GetNow().Substring(0, 8);
									nrow["WGPCPSTY"] = "U";
								}
								else
								{
									nrow = ds.Tables["MSTWGPC"].NewRow();
									nrow["WGPCYYMM"] = drow["END_YYMM"].ToString();
									nrow["WGPCSQNO"] = "1";
									nrow["WGPCSABN"] = drow["SAWON_NO"].ToString().Trim();
									for (int i = 1; i <= 50; i++)
									{
										nrow["WGPCSD" + i.ToString().PadLeft(2, '0')] = 0;
									}
									for (int i = 1; i <= 30; i++)
									{
										nrow["WGPCGJ" + i.ToString().PadLeft(2, '0')] = 0;
									}
									//nrow["WGPCUSID"] = SilkRoad.Config.SRConfig.USID;
									//for (int i = 0; i < grdv_search.Columns.Count; i++)
									//{
									//	if (grdv_search.Columns[i].FieldName.Substring(4, 2) == "SD")
									//		nrow[grdv_search.Columns[i].FieldName.ToString()] = clib.TextToDecimal(drow[grdv_search.Columns[i].FieldName.ToString()].ToString());
									//}
									nrow["WGPCINDT"] = gd.GetNow().Substring(0, 8);
									nrow["WGPCUPDT"] = "";
									nrow["WGPCPSTY"] = "A";
									ds.Tables["MSTWGPC"].Rows.Add(nrow);
								}
								nrow["WGPCUSID"] = SilkRoad.Config.SRConfig.USID;
								for (int i = 0; i < grdv_search.Columns.Count; i++)
								{
									if (grdv_search.Columns[i].FieldName.Substring(4, 2) == "SD")
										nrow[grdv_search.Columns[i].FieldName.ToString()] = clib.TextToDecimal(drow[grdv_search.Columns[i].FieldName.ToString()].ToString());
								}

								//근태 넘기기
								DataRow nrow2;
								df.GetMSTGTMMDatas(drow["END_YYMM"].ToString(), drow["SAWON_NO"].ToString().Trim(), ds);
								if (ds.Tables["MSTGTMM"].Select("GTMMYYMM = '" + drow["END_YYMM"].ToString() + "' AND GTMMSABN = '" + drow["SAWON_NO"].ToString().Trim() + "'").Length > 0)
								{
									nrow2 = ds.Tables["MSTGTMM"].Select("GTMMYYMM = '" + drow["END_YYMM"].ToString() + "' AND GTMMSABN = '" + drow["SAWON_NO"].ToString().Trim() + "'")[0];

									nrow2["GTMMUPDT"] = gd.GetNow().Substring(0, 8);
									nrow2["GTMMPSTY"] = "U";
								}
								else
								{
									nrow2 = ds.Tables["MSTGTMM"].NewRow();
									nrow2["GTMMYYMM"] = drow["END_YYMM"].ToString();
									nrow2["GTMMSABN"] = drow["SAWON_NO"].ToString().Trim();

									for (int i = 1; i <= 30; i++)
									{
										nrow2["GTMMGT" + i.ToString().PadLeft(2, '0')] = 0;
									}

									nrow2["GTMMINDT"] = gd.GetNow().Substring(0, 8);
									nrow2["GTMMUPDT"] = "";
									nrow2["GTMMPSTY"] = "A";
									ds.Tables["MSTGTMM"].Rows.Add(nrow2);
								}

								nrow2["GTMMUSID"] = SilkRoad.Config.SRConfig.USID;
								DataRow grow = ds.Tables["SEARCH_INFOSD02"].Rows[0];
								if (grow["A11"].ToString().Trim() != "")
									nrow2["GTMMGT" + grow["A11"].ToString().Trim().Substring(2, 2)] = drow["WGPCGT01"].ToString();
								if (grow["A12"].ToString().Trim() != "")
									nrow2["GTMMGT" + grow["A12"].ToString().Trim().Substring(2, 2)] = clib.TextToDecimal(drow["WGPCGT02"].ToString()) + clib.TextToDecimal(drow["WGPCGT04"].ToString());
								if (grow["A13"].ToString().Trim() != "")
									nrow2["GTMMGT" + grow["A13"].ToString().Trim().Substring(2, 2)] = drow["WGPCGT03"].ToString();
								//if (grow["A14"].ToString().Trim() != "")
								//	nrow2["GTMMGT" + grow["A14"].ToString().Trim().Substring(2, 2)] = drow["WGPCGT04"].ToString();
								if (grow["A21"].ToString().Trim() != "")
									nrow2["GTMMGT" + grow["A21"].ToString().Trim().Substring(2, 2)] = clib.TextToDecimal(drow["WGPCGT05"].ToString()) + clib.TextToDecimal(drow["WGPCGT06"].ToString());
								//if (grow["A22"].ToString().Trim() != "")
								//	nrow2["GTMMGT" + grow["A22"].ToString().Trim().Substring(2, 2)] = drow["WGPCGT06"].ToString();
								if (grow["A31"].ToString().Trim() != "")
									nrow2["GTMMGT" + grow["A31"].ToString().Trim().Substring(2, 2)] = drow["WGPCGT07"].ToString();
								if (grow["A41"].ToString().Trim() != "")
									nrow2["GTMMGT" + grow["A41"].ToString().Trim().Substring(2, 2)] = drow["WGPCGT08"].ToString();
								if (grow["A51"].ToString().Trim() != "")
									nrow2["GTMMGT" + grow["A51"].ToString().Trim().Substring(2, 2)] = drow["WGPCGT09"].ToString();
								if (grow["A61"].ToString().Trim() != "")
									nrow2["GTMMGT" + grow["A61"].ToString().Trim().Substring(2, 2)] = drow["WGPCGT10"].ToString();
								if (grow["A71"].ToString().Trim() != "")
									nrow2["GTMMGT" + grow["A71"].ToString().Trim().Substring(2, 2)] = clib.TextToDecimal(drow["WGPCGT11"].ToString()) + clib.TextToDecimal(drow["WGPCGT12"].ToString());
								//if (grow["A72"].ToString().Trim() != "")
								//	nrow2["GTMMGT" + grow["A72"].ToString().Trim().Substring(2, 2)] = drow["WGPCGT12"].ToString();
								if (grow["A81"].ToString().Trim() != "")
									nrow2["GTMMGT" + grow["A81"].ToString().Trim().Substring(2, 2)] = drow["WGPCGT13"].ToString();

								string[] tableNames = new string[] { "MSTWGPC", "MSTGTMM" };
								SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
								outVal += cmd.setUpdate(ref ds, tableNames, null);
							}
						}
					}
					catch (Exception ec)
					{
						MessageBox.Show(ec.Message, "전송오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					finally
					{
						if (outVal > 0)
							MessageBox.Show("마감내용이 전송되었습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);

						SetCancel();
						//Search();
						Cursor = Cursors.Default;
					}
				}
			}
		}
		//급여전송취소(급여변동항목,월근태 삭제)
		private void btn_wage_canc_Click(object sender, EventArgs e)
		{
			//for (int i = 0; i < grdv_end.RowCount; i++)
			//{
			//	if (grdv_end.GetVisibleRowHandle(i) > -1)
			//	{
			//		if (grdv_end.GetDataRow(grdv_end.GetVisibleRowHandle(i))["IMCHTEL1"].ToString().Substring(0, 2) == "01")
			//			grdv_end.GetDataRow(grdv_end.GetVisibleRowHandle(i))["CHK"] = "1";
			//	}
			//}
		}
		//내역조회
		private void btn_s_search_Click(object sender, EventArgs e)
		{
			if (isNoError_um(5))
			{
				string frmm = clib.DateToText(dat_s_frmm.DateTime).Substring(0, 6);
				string tomm = clib.DateToText(dat_s_tomm.DateTime).Substring(0, 6);
				string dept = sl_s_dept.EditValue == null ? "%" : sl_s_dept.EditValue.ToString();
				df.GetSEARCH_3081Datas(frmm, tomm, dept, ds);
				grd_search.DataSource = ds.Tables["SEARCH_3081"];

				if (ds.Tables["SEARCH_3081"].Rows.Count == 0)
					MessageBox.Show("조회된 내역이 없습니다.", "없음", MessageBoxButtons.OK, MessageBoxIcon.Error);

				info_Search();
			}
		}

		//엑셀변환
		private void btn_excel_Click(object sender, EventArgs e)
		{
			clib.gridToExcel(grdv_search, this.Text + "(" + this.Name + ")_" + clib.DateToText(DateTime.Now), true);
		}

		//탭페이지 변경시
		private void srTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
		{
			if (srTabControl1.SelectedTabPageIndex == 0 && ds.Tables["WORK_3080"] != null)
			{
				if (ds.Tables["WORK_3080"].Rows.Count > 0)
				{
					string yymm = clib.DateToText(dat_p_yymm.DateTime).Substring(0, 6);
					df.GetWORK_3080Datas(yymm, cmb_gubn.SelectedIndex.ToString(), ds);
					grd1.DataSource = ds.Tables["WORK_3080"];
				}
			}
		}


		#endregion

		#region 3 EVENT
				
		private void WAGEEND_CHK()
		{
			string yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 6);
			df.Get3080_WAGE_ENDSDatas(yymm, ds);
			if (ds.Tables["3080_WAGE_ENDS"].Rows.Count > 0) //마감월이 저장되어 있으면
			{
				DataRow irow = ds.Tables["3080_WAGE_ENDS"].Rows[0];
				if (irow["YY_CHK"].ToString() == "1") //년마감 일때		
					ends_yn = "1";				
				else //년마감 아닐때 월마감 체크
					ends_yn = irow["MM_CHK"].ToString();				
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
			bool isError = false;
			if (mode == 1)  //집계처리
			{
				if (clib.DateToText(dat_p_yymm.DateTime) == "")
				{
					MessageBox.Show("근무년월 입력하세요!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_p_yymm.Focus();
					return false;
				}
				else
				{
					isError = true;
				}
			}
			else if (mode == 2) //마감
			{
				if (clib.DateToText(dat_end.DateTime) == "")
				{
					MessageBox.Show("등록할 마감년월을 입력하세요!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_end.Focus();
					return false;
				}
				else
				{
					isError = true;
				}
			}
			else if (mode == 3)  //급여처리조회
			{
				if (clib.DateToText(dat_yymm.DateTime) == "")
				{
					MessageBox.Show("마감년월을 입력하세요!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_yymm.Focus();
					return false;
				}
				else
				{
					isError = true;
				}
			}
			else if (mode == 4)  //급여처리
			{
				WAGEEND_CHK();
				if (ends_yn == "1")
				{
					string yymm = clib.DateToText(dat_yymm.DateTime).Substring(0, 4) + "." + clib.DateToText(dat_yymm.DateTime).Substring(4, 2);
					MessageBox.Show(yymm+"은 인사급여에서 마감되어 전송할 수 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				else
				{
					isError = true;
				}
			}
			else if (mode == 5)  //조회
			{
				if (clib.DateToText(dat_s_frmm.DateTime) == "")
				{
					MessageBox.Show("마감년월(fr)을 입력하세요!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_s_frmm.Focus();
					return false;
				}
				else if (clib.DateToText(dat_s_tomm.DateTime) == "")
				{
					MessageBox.Show("마감년월(to)를 입력하세요!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_s_tomm.Focus();
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
