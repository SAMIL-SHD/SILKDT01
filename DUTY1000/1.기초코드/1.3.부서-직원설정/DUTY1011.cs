using System;
using System.Data;
using System.Windows.Forms;
using SilkRoad.Common;
using System.Text;
using System.Drawing;

namespace DUTY1000
{
    public partial class duty1011 : SilkRoad.Form.Base.FormX
    {
        CommonLibrary clib = new CommonLibrary();
        static string wagedb = "WG06DB" + SilkRoad.Config.SRConfig.WorkPlaceNo;

        ClearNEnableControls cec = new ClearNEnableControls();
        public DataSet ds = new DataSet();
        DataProcFunc df = new DataProcFunc();
        SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();

        public duty1011()
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
			btn_refresh_CK();

			if (ds.Tables["SEARCH_NURS"] != null)
				ds.Tables["SEARCH_NURS"].Clear();
            grd2.DataSource = null;
			
			if (ds.Tables["S_TRSDEPT"] != null)
				ds.Tables["S_TRSDEPT"].Clear();
            grd_log.DataSource = null;

            sl_dept.EditValue = null;
			txt_num.Text = "";
		}

        #endregion

        #region 1 Form
		
        private void duty1011_Load(object sender, EventArgs e)
        {
            SetCancel();
			dat_frdt.DateTime = clib.TextToDateFirst(clib.DateToText(DateTime.Today));
			dat_todt.DateTime = DateTime.Today;
        }

        #endregion

        #region 2 Button
		
		private void btn_refresh_Click(object sender, EventArgs e)
		{
			btn_refresh_CK();
		}
		private void btn_refresh_CK()
		{
			df.GetSEARCH_MSTDEPTDatas(ds);
            grd1.DataSource = ds.Tables["SEARCH_MSTDEPT"];

			df.GetSEARCH_DEPTDatas(ds);
			sl_dept.Properties.DataSource = ds.Tables["SEARCH_DEPT"];
		}
		
		//전체선택
		private void btn_select_all_Click(object sender, EventArgs e)
		{
			if (ds.Tables["SEARCH_NURS"] != null)
			{
				for (int i = 0; i < ds.Tables["SEARCH_NURS"].Rows.Count; i++)
				{
					ds.Tables["SEARCH_NURS"].Rows[i]["CHK"] = "1";
				}
				txt_num.Text = ds.Tables["SEARCH_NURS"].Select("CHK='1'").Length.ToString();
			}
		}
		//선택해지
		private void btn_select_del_Click(object sender, EventArgs e)
		{
			if (ds.Tables["SEARCH_NURS"] != null)
			{
				for (int i = 0; i < ds.Tables["SEARCH_NURS"].Rows.Count; i++)
				{
					ds.Tables["SEARCH_NURS"].Rows[i]["CHK"] = "0";
				}
				txt_num.Text = ds.Tables["SEARCH_NURS"].Select("CHK='1'").Length.ToString();
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
					df.GetDUTY_TRSDEPTDatas(ds);
                    string sawon = "";
					for (int i = 0; i < ds.Tables["SEARCH_NURS"].Rows.Count; i++)
					{
						DataRow drow = ds.Tables["SEARCH_NURS"].Rows[i];
						if (ds.Tables["SEARCH_NURS"].Rows[i]["CHK"].ToString() == "1")
						{
							DataRow nrow = ds.Tables["DUTY_TRSDEPT"].NewRow();
							nrow["SAWON_NO"] = drow["SAWON_NO"].ToString();
							nrow["MOVE_DATE"] = clib.DateToText(dat_move.DateTime);
							nrow["FR_DEPT"] = drow["DEPTCODE"].ToString();
							nrow["TO_DEPT"] = sl_dept.EditValue;
							nrow["REG_DT"] = gd.GetNow();
							nrow["REG_ID"] = SilkRoad.Config.SRConfig.USID;
							ds.Tables["DUTY_TRSDEPT"].Rows.Add(nrow);

							sawon += ",'" + drow["SAWON_NO"].ToString().Trim() + "'";
						}
					}

                    string[] UpQry = { "UPDATE " + wagedb + ".dbo.MSTEMBS SET EMBSDPCD = '" + sl_dept.EditValue + "' WHERE EMBSSABN IN (" + sawon.Substring(1) + ")" };

					string[] tableNames = new string[] { "DUTY_TRSDEPT" };
					//string[] tableNames = new string[] { null };
					SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
					outVal = cmd.setUpdate(ref ds, tableNames, UpQry);
                    
                }
                catch (Exception ec)
                {
                    MessageBox.Show(ec.Message, "저장오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
					if (outVal <= 0)                    
                        MessageBox.Show("저장된 내용이 없습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information); 
					else
                        MessageBox.Show("선택된 직원들의 부서이동이 처리되었습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetCancel();
                    Cursor = Cursors.Default;
                }
            }
        }
        /// <summary>취소버튼</summary>
        private void btn_clear_Click(object sender, EventArgs e)
        {
            SetCancel();
        }
        
		private void btn_search_Click(object sender, EventArgs e)
		{
			if (isNoError_um(1))
			{
				Cursor = Cursors.WaitCursor;
				df.GetSEARCH_TRSDEPTDatas(clib.DateToText(dat_frdt.DateTime), clib.DateToText(dat_todt.DateTime), ds);
				grd_search.DataSource = ds.Tables["SEARCH_TRSDEPT"];
				Cursor = Cursors.Default;
			}
		}
		private void btn_exel_Click(object sender, EventArgs e)
		{
            clib.gridToExcel(grdv_search, this.Text + "(" + this.Name + ")_" + clib.DateToText(DateTime.Now), true);
		}

        #endregion

        #region 3 EVENT
		
        private void grdv1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle < 0)
                return;

            DataRow drow = grdv1.GetFocusedDataRow();

            df.GetSEARCH_DEPT_NURSDatas(drow["CODE"].ToString().Trim(), ds);
            grd2.DataSource = ds.Tables["SEARCH_NURS"];
			txt_num.Text = "";
			
			if (ds.Tables["S_TRSDEPT"] != null)
				ds.Tables["S_TRSDEPT"].Clear();
            grd_log.DataSource = null;
        }

        private void grdv2_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            //clib.gridview_CustomDrawColumnHeader_forCheck(sender, e);
        }
        private void grdv2_MouseDown(object sender, MouseEventArgs e)
        {
            //clib.gridview_MouseDown_forCheck(sender, e);
        }

        //체크한 인원수만 count되도록
		private void grdv2_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
		{
			DataRow drow = grdv2.GetFocusedDataRow();
			if (drow != null)
			{
				if (e.Column == col_CHK)  //선택       
				{
					drow["CHK"] = e.Value.ToString();
					txt_num.Text = ds.Tables["SEARCH_NURS"].Select("CHK='1'").Length.ToString();
				}
			}
		}
        private void grdv2_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
			//ds.Tables["SEARCH_NURS"].AcceptChanges();

			//if (e.IsTotalSummary)
			//{
			//	if (ds.Tables["SEARCH_NURS"].Select("CHECK = 'true'") == null || ds.Tables["SEARCH_NURS"].Select("CHECK = 'true'").Length == 0)
			//	{
			//		e.TotalValue = 0;
			//	}
			//	else
			//	{
			//		e.TotalValue = ds.Tables["SEARCH_NURS"].Select("CHECK = 'true'").Length;
			//	}
			//	txt_num.Text = e.TotalValue.ToString();
			//}
		}
        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            ////값변화 바로 반영
            //grdv2.PostEditor();
            //ds.Tables["SEARCH_NURS"].AcceptChanges();
        }
		
		private void grdv2_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
		{
            if (e.RowHandle < 0)
                return;

            DataRow drow = grdv2.GetFocusedDataRow();
			df.GetS_TRSDEPTDatas(drow["SAWON_NO"].ToString(), ds);
            grd_log.DataSource = ds.Tables["S_TRSDEPT"];
		}
        private void grdv2_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
			if (e.Column == col_CHK)			
				grdv2.PostEditor(); //값변화 바로 반영			
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
				if (clib.DateToText(dat_frdt.DateTime) == "")
				{
					MessageBox.Show("이동일자(fr)을 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_frdt.Focus();
					return false;
				}
				else if (clib.DateToText(dat_todt.DateTime) == "")
				{
					MessageBox.Show("이동일자(to)을 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_todt.Focus();
					return false;
				}
				else
				{
					isError = true;
				}
            }
            else if (mode == 2)  //저장
            {
				if (ds.Tables["SEARCH_NURS"].Select("CHK = '1'").Length == 0)
				{
					MessageBox.Show("선택된 직원이 없습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				else if (clib.DateToText(dat_move.DateTime) == "")
				{
					MessageBox.Show("이동일자를 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				else if (clib.isLookupNull(sl_dept.EditValue) || sl_dept.EditValue.ToString().Equals("부서선택"))
				{
					MessageBox.Show("이동할 부서를 선택하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				else if (grdv1.GetFocusedDataRow()["CODE"].ToString() == sl_dept.EditValue.ToString())
				{
					MessageBox.Show("이동할 부서가 기존과 동일합니다.\r\n부서를 다시 확인하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
