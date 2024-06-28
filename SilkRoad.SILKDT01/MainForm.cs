using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraTreeList;
using SilkRoad.ComDialog;
using DevExpress.XtraTabbedMdi;
using System.ComponentModel;
using DevExpress.Utils.Drawing;
using SilkRoad.DAL;
using System.Data;
using SilkRoad.Config;
using SilkRoad.Common;
using System.IO;
using System.Diagnostics;

namespace SilkRoad.SILKDT01
{
    public partial class frmMain : Form.Base.FormX
    {
        private static string _mdiName { get; set; }
        private static bool _threadIsDone { get; set; }
        
        ShowMessage sm = new ShowMessage();
        double orginalWidth, originalHeight;
        static DataProcessing dp = new DataProcessing();
        string user_name = "SilkRoader";

        public frmMain(string title)
        {
            InitializeComponent();
            this.Text = title;
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            orginalWidth = this.Width;
            originalHeight = this.Height;

            getMainUserInfo();  //로그인한사람 부서랑, 사원코드 얻기
            //getDATAMODI();      //DataModi 실행
						
			if (ACConfig.G_MSYN == "1")
				mm_end.Visible = true;

            bar_path.Caption = Application.StartupPath; //시작경로
            bar_Comp.Caption = "Comp No : " + Config.SRConfig.WorkPlaceNo;
            bar_db.Caption = DataAccess.DBname;
			
            ViewNodes(1);
        }

        //로그인 유저 정보 가져오기
        private void getMainUserInfo()
        {
            try
            {
                string qry = " SELECT * FROM MSTUSER "
                           + "  WHERE USERIDEN = '" + Config.SRConfig.USID + "' ";
                DataProc.GetData gd = new DataProc.GetData();
                System.Data.DataRow drow = gd.GetOneRowInQuery(Convert.ToInt32(DataAccess.DBtype.ToString()), "SILKDBCM", qry);
				
				if (drow != null)
					ACConfig.G_MSYN = drow["USERMSYN"].ToString().Trim();
				if (SRConfig.USID == "SAMIL")
					ACConfig.G_MSYN = "1";

				Mbtn_1.Visibility = ACConfig.G_MSYN == "1" ? BarItemVisibility.Always : BarItemVisibility.Never;
				Mbtn_3.Visibility = ACConfig.G_MSYN == "1" ? BarItemVisibility.Always : BarItemVisibility.Never;	

                user_name = drow != null ? drow["USERNAME"].ToString().Trim()  : user_name ;
                bar_user.Caption = Config.SRConfig.USID + "(" + user_name + ")";
								
                //string dbname = "COMMDB" + SRConfig.WorkPlaceNo;
                string wgdb = "WG06DB" + SRConfig.WorkPlaceNo;
                qry = " SELECT A.EMBSDPCD, A.EMBSNAME, A.EMBSADGB, RTRIM(ISNULL(X1.DEPRNAM1,'')) DEPT_NM "
                    + "   FROM MSTEMBS A "
                    + "   LEFT OUTER JOIN MSTDEPR X1 "
                    + "     ON A.EMBSDPCD = X1.DEPRCODE "
                    + "  WHERE A.EMBSSABN = '" + SRConfig.USID + "' ";
                DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype.ToString()), wgdb, qry);
                //bar_user.Caption = SRConfig.USID + " (SilkRoader)";
                if (dt.Rows.Count > 0)
                {
					user_name = dt.Rows[0]["EMBSNAME"].ToString().Trim();
                    SRConfig.US_DPCD = dt.Rows[0]["EMBSDPCD"].ToString().Trim();
                    //SRConfig.US_PRCD = dt.Rows[0]["USERPRCD"].ToString().Trim();
                    if (dt.Rows[0]["DEPT_NM"].ToString().Trim() == "")                    
                        bar_user.Caption = SRConfig.USID + " (" + user_name + ")";                    
                    else                    
                        bar_user.Caption = SRConfig.USID + " (" + user_name + ")" + " [" + dt.Rows[0]["DEPT_NM"].ToString().Trim() + "]";
				}
            }
            catch (Exception ec)
            {
                MessageBox.Show(ec.Message, "getMainUserInfo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //DATA MODIFICATION
        private void getDATAMODI()
        {
			string dbnm = DataAccess.DBname;
			string qry = " SELECT * FROM sysobjects WHERE NAME = 'SMS_SEND'  ";
			DataProc.GetData gd = new DataProc.GetData();
			DataTable dt = gd.GetDataInQuery(1, dbnm, qry);
			if (dt.Rows.Count < 1)
			{
				DataModi DataModi = new DataModi();
				DataModi.frmMain = this;
				DataModi.ShowDialog();
			}
		}

        #region MDI Form
        /// <summary>
        /// 모듈을 실행시킨다.
        /// </summary>
        /// <param name="str">모듈 한글명</param>
        /// <param name="strFormName">모듈 파일명</param>
        /// <param name="args">해당 모듈로 보낼 Parameters</param>
        private void CreateMdiForm(string str, string strFormName, params object[] args)
        {
            _threadIsDone = false;
            string[] formName = strFormName.Split('.');

            foreach (Form.Base.FormX childForm in xtraTabbedMdiManager1.FloatForms)  //Float폼 체크
            {
                if ((childForm.ProductName + "." + childForm.Name) == strFormName)
                {
                    childForm.Activate();
                    return;
                }
            }
            foreach (Form.Base.FormX childForm in MdiChildren)  //메인에 열린폼 체크
            {
                if ((childForm.ProductName + "." + childForm.Name) == strFormName)  
                {
                    childForm.Activate();
                    return;
                }
            }

            try
            {
                Object obj = null;
                if (args == null)
                {
                    _mdiName = str;
                    splashManager1.ShowWaitForm();
                }
                else
                {
                    if (args.Length == 0)
                    {
                        _mdiName = str;
                        splashManager1.ShowWaitForm();
                    }
                }

                //프로젝트 안에 들어가도록..
                string strDllName = formName[0] + ".dll";
                Assembly asm = Assembly.LoadFrom(System.IO.Path.Combine(Application.StartupPath, strDllName));
                string strReflectionName = asm.GetName().Name.ToUpper() + "." + formName[1];
                Type type = asm.GetType(strReflectionName, false, true);

                if (args == null)
                {
                    obj = Activator.CreateInstance(type);
                }
                else
                {
                    if (args.Length == 0)
                    {
                        if (type == null)
                        {
                            if (splashManager1.IsSplashFormVisible)                            
                                splashManager1.CloseWaitForm();
                            
                            MessageBox.Show("해당 폼은 현재 작업중입니다.", "^_^");
                            return;
                        }
                        else
                        {
                            obj = Activator.CreateInstance(type);
                        }
                    }
                    else
                    {
                        obj = Activator.CreateInstance(type, args);
                    }
                }

                Form.Base.FormX frm = (Form.Base.FormX)obj;
                frm.Text = str;
                frm.MdiParent = this;
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("해당 폼이 존재하지 않습니다." + ex.Message, "폼 호출 에러");
            }
            finally
            {
                if (splashManager1.IsSplashFormVisible)                
                    splashManager1.CloseWaitForm();                
            }
        }

        #endregion

        #region 기타

        //프로그램 종료
        private void Mbtn_exit_ItemClick(object sender, ItemClickEventArgs e)
        {
            //this.Dispose();
            this.Close();
        }

        private void ViewNodes(int toolbarType)
        {
            ShowDockPanel();
            AppendTreeNode atn = new AppendTreeNode();

            string menuCaption = atn.ViewNode(toolbarType, treeList1);
            if (!string.IsNullOrEmpty(menuCaption))            
                dockPanel1.Text = menuCaption;            
            else            
                MessageBox.Show("메뉴 리스트 생성에 실패하였습니다.", "Fn: ViewNode", MessageBoxButtons.OK, MessageBoxIcon.Error);            
        }

        private void ShowDockPanel()
        {
            if (dockPanel1.Visibility == DevExpress.XtraBars.Docking.DockVisibility.AutoHide)            
                dockPanel1.Show();            
        }        

        #endregion

        #region TreeListView Item View

        private void treeList1_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs ea = (MouseEventArgs)e;
            TreeListHitInfo hi = ((TreeList)sender).CalcHitInfo(new Point(ea.X, ea.Y));
            if (hi.Column != null)
            {
                string str1 = string.Empty;
                string str2 = string.Empty;
                string str3 = string.Empty;

                str1 = hi.Node.GetDisplayText("Module");
                str2 = hi.Node.GetDisplayText("MdKey");
                str3 = hi.Node.GetDisplayText("MdPara");

                if (str2 == "") return;
                //권한체크
                if (CheckUscp(str2.Substring(9, 8)))
                {
                    if (str2.Trim().Length > 0)
                    {
                        if (Config.SRConfig.USID != "SAMIL") //SAMIL ID면 pass...
                        {
                            //화면별 로그인정보 입력
                            string sday = DateTime.Now.ToString().Replace("-", "").Substring(0, 8);
                            string stime = DateTime.Now.ToString("HH:mm:ss").Replace(":", "");//'" + SilkRoad.Config.SRConfig.WorkPlaceNo + "',
                            string commdb = "COMMDB" + Config.SRConfig.WorkPlaceNo;
                            string qry = " INSERT INTO LOGUSER VALUES ('1','" + sday + "','" + stime + "','" + str2.Substring(9, 8) + "','','" + Config.SRConfig.USID + "','" + user_name + "','12') ";

                            DataProc.GetData gd = new DataProc.GetData();
                            object obj = gd.GetOneData(Convert.ToInt32(DataAccess.DBtype.ToString()), commdb, qry);
                        }

						if (string.IsNullOrEmpty(str3))
							CreateMdiForm(str1, str2);
						else
							CreateMdiForm(str1, str2, str3);
					}
                }
                else
                {
                    MessageBox.Show("해당메뉴에 접근권한이 없습니다.", "권한", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // 메뉴 권한체크
        private bool CheckUscp(string dllName)
        {
            bool res = false;
            SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();

			//res = true;
			//SAMIL ID면 pass...
			if (SRConfig.USID == "SAMIL" || dllName.ToUpper().Substring(0, 7) == "DUTY100" || dllName.ToUpper().Substring(0, 7) == "DUTY506" 
				|| dllName.ToUpper().Substring(0, 7) == "DUTY508" || dllName.ToUpper().Substring(0, 8) == "duty2030")
				return true;

			//메뉴 권한 검사     
			string qry = "  SELECT USCPIDEN "
					   + "    FROM COMMDB" + SilkRoad.Config.SRConfig.WorkPlaceNo + ".DBO.MSTUSCP "
					   + "   WHERE USCPIDEN = '" + SilkRoad.Config.SRConfig.USID + "' "
					   + "     AND USCPFMID = '" + dllName + "'";

			object iden = gd.GetOneData(1, "COMMDB" + SilkRoad.Config.SRConfig.WorkPlaceNo, qry);

			if (iden != null && iden.ToString().Length > 0)
				res = true;

			return res;
        }

        private void tb_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
				getMainUserInfo();

                string btnName = ((BarLargeButtonItem)e.Item).Caption.ToString().Trim();
                int btnIter = GetModuleIteration(btnName);
                if (btnIter > -1)
                {
                    ViewNodes(btnIter);

                    dockPanel1.Text = btnName + " 항목 ";
                    this.Refresh();
                }
            }
            catch (Exception ie)
            {
                MessageBox.Show(ie.Message, "tb_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //클릭한 버튼명에따라서 메뉴xml 불러오도록
        private int GetModuleIteration(string btnName)
        {
            int resVal = -1;

            switch (btnName.Trim())
            {
                case "코드관리":
                    resVal = 0;
                    break;
                case "Duty관리":
                    resVal = 1;
                    break;
                case "마감관리":
                    resVal = 2;
                    break;
            }

            return resVal;
        }


        //하단바에 선택된 폼명이랑 폼번호보여주기랑, 해당폼 title에 메뉴에서 설정한 폼 제목 보여주기
        //((SilkRoad.UserControls.SRTitle)xtraTabbedMdiManager1.SelectedPage.MdiChild.Controls.Find("srTitle1", true)[0]).SRTitleTxt = xtraTabbedMdiManager1.SelectedPage.MdiChild.Text;
        private void xtraTabbedMdiManager1_SelectedPageChanged(object sender, EventArgs e)
        {
			if (xtraTabbedMdiManager1.SelectedPage != null)
			{
				bar_title.Caption = xtraTabbedMdiManager1.SelectedPage.MdiChild.Text + "(" + xtraTabbedMdiManager1.SelectedPage.MdiChild.GetType().Name + ")";
				if (ACConfig.G_MSYN == "1")
					mm_end.Visible = false;
			}
			else
			{
				bar_title.Caption = "-"; //폼 없는경우는 빈칸으로
				if (ACConfig.G_MSYN == "1")
					mm_end.Visible = true;
			}
        }


		//QR코드열기
		private void barbtn_qr_ItemClick(object sender, ItemClickEventArgs e)
		{
            if (Application.OpenForms["QRcode"] != null)
                Application.OpenForms["QRcode"].Close();
            QRcode QRcode = new QRcode();
            QRcode.Show();
		}
		//프린터설정 열기
		private void barbtn_print_ItemClick(object sender, ItemClickEventArgs e)
		{
			printDialog1.ShowDialog();
		}
		//그룹웨어사이트연결 OR 홈페이지연결
		private void Mbtn_logo_ItemClick(object sender, ItemClickEventArgs e)
        {
			string url = "https://988.co.kr/samil/";
			try
			{
				Process.Start("chrome.exe", url);
			}
			catch
			{
				Process.Start(url);
			}
        }

		#endregion
    }

	#region 메인배경처리 TabbedMdiManagerX 클래스
    //MDI parent 배경때문에; 요렇게 처리*********************
    public class TabbedMdiManagerX : XtraTabbedMdiManager
    {
        private Image backImage = null;
        //private Image backImage = SILKDUTY SILKDUTY...Resources.여수QR; //resource 이미지

        public TabbedMdiManagerX()
            : base()
        {
        }

        public TabbedMdiManagerX(IContainer container)
            : base(container)
        {
        }

        protected override void DrawNC(DXPaintEventArgs e)
        {
            base.DrawNC(e);
            if (Pages.Count == 0 && backImage != null) e.Graphics.DrawImage(backImage, Bounds);
        }
    }

	#endregion

}
