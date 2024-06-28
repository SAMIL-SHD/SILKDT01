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
    public partial class duty2030 : SilkRoad.Form.Base.FormX
    {
        CommonLibrary clib = new CommonLibrary();

        ClearNEnableControls cec = new ClearNEnableControls();
        public DataSet ds = new DataSet();
        static DataProcessing dp = new DataProcessing();
        DataProcFunc df = new DataProcFunc();
        SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();

        private int admin_lv = 0;
        private string p_dpcd = "";

        public duty2030()
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
			if (ds.Tables["SEARCH_SAVE"] != null)
				ds.Tables["SEARCH_SAVE"].Clear();
			grd1.Enabled = false;

            sl_dept.Enabled = true;
            dat_yymm.Enabled = true;
						
            for (int i = 1; i <= 31; i++)
            {
				grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.ForeColor = Color.Black;
				grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].AppearanceHeader.Font = new Font(FontFamily.GenericSansSerif, 9F, FontStyle.Regular);
				grdv1.Columns["D" + i.ToString().PadLeft(2, '0')].ToolTip = "";
            }
            Refresh_Click();
            SetButtonEnable("1000");
        }
		
        //내역 조회
        private void baseInfoSearch()
        {
			df.GetSEARCH_SAVEDatas(sl_dept.EditValue.ToString(), clib.DateToText(dat_yymm.DateTime).Substring(0, 6), ds);
			grd1.DataSource = ds.Tables["SEARCH_SAVE"];
		}

        #endregion

        #region 1 Form

        private void duty2030_Load(object sender, EventArgs e)
        {
		}

        private void duty2030_Shown(object sender, EventArgs e)
        {
            df.GetMSTUSER_CHKDatas(ds);  //부서관리여부
            if (ds.Tables["MSTUSER_CHK"].Rows.Count > 0)
                admin_lv = clib.TextToInt(ds.Tables["MSTUSER_CHK"].Rows[0]["EMBSADGB"].ToString()); //권한레벨

            if (SilkRoad.Config.ACConfig.G_MSYN == "1" || SilkRoad.Config.SRConfig.USID == "SAMIL" || admin_lv > 2)
            {
                admin_lv = 3;
                p_dpcd = "%";
            }
            else if (admin_lv == 1)
            {
                p_dpcd = SilkRoad.Config.SRConfig.US_DPCD == null ? null : SilkRoad.Config.SRConfig.US_DPCD.Trim();
            }
            else if (admin_lv == 2)
            {
                p_dpcd = "%";
            }
            else
            {
                p_dpcd = SilkRoad.Config.SRConfig.US_DPCD == null ? null : SilkRoad.Config.SRConfig.US_DPCD.Trim();
            }

            SetCancel();
            dat_yymm.DateTime = DateTime.Now;
            sl_dept.EditValue = null;

            if (p_dpcd != "%")
                sl_dept.EditValue = p_dpcd;
        }
        #endregion

        #region 2 Button
		
		private void btn_bf_Click(object sender, EventArgs e)
		{
			if (clib.DateToText(dat_yymm.DateTime) == "")
				dat_yymm.DateTime = DateTime.Now;
			else
				dat_yymm.DateTime = dat_yymm.DateTime.AddMonths(-1);

			btn_proc.PerformClick();
		}
		private void btn_af_Click(object sender, EventArgs e)
		{
			if (clib.DateToText(dat_yymm.DateTime) == "")
				dat_yymm.DateTime = DateTime.Now;
			else
				dat_yymm.DateTime = dat_yymm.DateTime.AddMonths(1);

			btn_proc.PerformClick();
		}
        //처리
        private void btn_proc_Click(object sender, EventArgs e)
        {
			if (isNoError_um(1))
			{
                Cursor = Cursors.WaitCursor;
				
				grd1.Enabled = true;
                sl_dept.Enabled = false;
                dat_yymm.Enabled = false;

				baseInfoSearch(); //내역조회

				df.GetSEARCH_HOLIDatas(clib.DateToText(dat_yymm.DateTime).Substring(0, 6), ds);
				//1.기준년월에 따른 하단 일자컬럼header 일자, 요일 설정
				int lastday = clib.TextToInt(clib.DateToText(clib.TextToDateLast(clib.DateToText(dat_yymm.DateTime))).Substring(6, 2));

				for (int i = 1; i <= lastday; i++)
				{
					DateTime day = clib.TextToDate(clib.DateToText(dat_yymm.DateTime).Substring(0, 6) + "01").AddDays(i - 1);
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

				SetButtonEnable("0111");
				grd1.DataSource = ds.Tables["SEARCH_SAVE"];
				Cursor = Cursors.Default;
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
			//rpt_2030 rpt = new rpt_2030(clib.DateToText(dat_yymm.DateTime), ds.Tables["SEARCH_HOLI"]);
			//rpt.DataSource = ds.Tables["SEARCH_SAVE"];
			//if (stat == 1)
			//	rpt.ShowPreview();
			//else if (stat == 2)
			//	rpt.Print();
		}
		
		//환경설정
		private void btn_info_Click(object sender, EventArgs e)
		{			
			duty2031 duty2031 = new duty2031(p_dpcd);
			duty2031.ShowDialog();

			SetCancel();
		}

        #endregion

        #region 3 EVENT

        /// <summary> refresh버튼 </summary>
        private void Refresh_Click()
        {
            df.Get2030_DEPTDatas(p_dpcd, ds);
            sl_dept.Properties.DataSource = ds.Tables["2030_DEPT"];
        }

        //메뉴 활성화시
        private void duty2030_Activated(object sender, EventArgs e)
		{
			//END_CHK();
		}
		private void duty2030_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)   //취소
            {
                btn_clear.PerformClick();
            }
        }
		
		//조회년월 변경시 부서 다시 불러오기
		private void dat_yymm_EditValueChanged(object sender, EventArgs e)
		{
            SetCancel();
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

            if (mode == 1)  //처리
            {
                if (sl_dept.EditValue == null)
                {
                    MessageBox.Show("부서를 선택하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sl_dept.Focus();
                    return false;
                }
                else if (dat_yymm.Text.ToString().Trim() == "")
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
			btn_preview.Enabled = arr.Substring(2, 1) == "1" ? true : false;
			btn_print.Enabled = arr.Substring(3, 1) == "1" ? true : false;
		}

		#endregion

	}
}
