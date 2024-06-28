using System;
using System.Data;
using System.Windows.Forms;
using SilkRoad.Common;


namespace DUTY1000
{
    public partial class duty8021 : SilkRoad.Form.Base.FormX
    {
        CommonLibrary clib = new CommonLibrary();

        ClearNEnableControls cec = new ClearNEnableControls();
        public DataSet ds = new DataSet();
        DataProcFunc df = new DataProcFunc();
        SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();
        string _seqno;

        public duty8021()
        {
            InitializeComponent();
        }
        public duty8021(string seqno)
        {
            InitializeComponent();
            _seqno = seqno;
        }

        #region 0. Initialization

        /// <summary>
        ///컨트롤 초기화 및 활성,비활성 설정
        /// </summary>
        /// <param name="enable"></param>
        private void SetCancel()
        {
        }

        #endregion

        #region 1 Form

        private void duty8021_Load(object sender, EventArgs e)
        {
            df.Get8030_SEARCH_EMBSDatas("%", ds);
            sl_embs.Properties.DataSource = ds.Tables["8030_SEARCH_EMBS"];
            df.Get8030_SEARCH_GNMUDatas(ds);
            sl_gnmu.Properties.DataSource = ds.Tables["8030_SEARCH_GNMU"];
            sl_gnmu2.Properties.DataSource = ds.Tables["8030_SEARCH_GNMU"];
        }
        private void duty8021_Shown(object sender, EventArgs e)
        {
            Proc();
            dat_year.Focus();
        }

        #endregion

        #region 2 Button

        private void Proc()
        {
            df.GetDUTY_TRSHREQDatas(_seqno, ds);
            if (ds.Tables["DUTY_TRSHREQ"].Rows.Count > 0)
            {
                DataRow hrow = ds.Tables["DUTY_TRSHREQ"].Rows[0];
                sl_embs.EditValue = hrow["SABN"].ToString().Trim();
                dat_year.DateTime = clib.TextToDate(hrow["REQ_YEAR"].ToString() + "0101");
                dat_ycdt.DateTime = clib.TextToDate(hrow["REQ_DATE"].ToString());
                dat_ycdt2.DateTime = clib.TextToDate(hrow["REQ_DATE2"].ToString());
                sl_gnmu.EditValue = hrow["REQ_TYPE"].ToString();
                sl_gnmu2.EditValue = hrow["REQ_TYPE2"].ToString();
                cmb_gubn.SelectedIndex = hrow["GUBN"].ToString() == "D" ? 1 : 0;
            }
        }

        //수정
        private void btn_save_Click(object sender, EventArgs e)
        {
            if (isNoError_um(1))
            {
                Cursor = Cursors.WaitCursor;
                int outVal = 0;
                try
                {
                    df.GetDUTY_TRSHREQDatas(_seqno, ds);
                    if (ds.Tables["DUTY_TRSHREQ"].Rows.Count > 0)
                    {
                        DataRow hrow = ds.Tables["DUTY_TRSHREQ"].Rows[0];
                        hrow["REQ_YEAR"] = clib.DateToText(dat_year.DateTime).Substring(0, 4);
                        hrow["UPDT"] = gd.GetNow();
                        hrow["USID"] = SilkRoad.Config.SRConfig.USID;
                        hrow["PSTY"] = "U";

                        string[] tableNames = new string[] { "DUTY_TRSHREQ" };
                        SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
                        outVal = cmd.setUpdate(ref ds, tableNames, null);
                    }
                }
                catch (Exception ec)
                {
                    MessageBox.Show(ec.Message, "저장오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor = Cursors.Default;
                    this.Dispose();
                }
            }
        }

        #endregion

        #region 3 EVENT

        private void duty8021_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)   //취소
            {
                this.Dispose();
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

            if (mode == 1)  //조회
            {
                if (clib.DateToText(dat_year.DateTime) == "")
                {
                    MessageBox.Show("기준년도를 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dat_year.Focus();
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

        #region 9 ETC

        #endregion

    }
}
