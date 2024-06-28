using System.Windows.Forms;
using SilkRoad.Config;
using DevExpress.XtraTreeList.Nodes;
using SilkRoad.Common;
using System.Data;
using SilkRoad.DAL;
using SilkRoad.DataProc;

namespace SilkRoad.SILKDT01
{
    public class AppendTreeNode
    {
        static DataProcessing dp = new DataProcessing();
        public DataSet ds = new DataSet();
        static GetData gd = new GetData();
        static SetData sd = new SetData();
        static CommonLibrary clib = new CommonLibrary();

        #region Node Append or Delete
        public string ViewNode(int toolbarType, Control cont)
        {
            string menuCap = string.Empty;
            try
            {
                if (cont is DevExpress.XtraTreeList.TreeList)
                {
                    DeleteNode2TreeList(cont);
                    menuCap = AddNode2TreeList(toolbarType, cont);
                    DevExpress.XtraTreeList.TreeList tl = cont as DevExpress.XtraTreeList.TreeList;
                    tl.ExpandAll();
                }
            }
            catch
            {
                menuCap = string.Empty;
            }
            return menuCap;
        }

        private void DeleteNode2TreeList(Control cont)
        {
            DevExpress.XtraTreeList.TreeList tl = cont as DevExpress.XtraTreeList.TreeList;
            int nodeCnt = tl.Nodes.Count;
            for (int i = nodeCnt - 1; i >= 0; i--)
            {
                tl.DeleteNode(tl.Nodes[i]);
            }
        }

        /// <summary>
        /// 트리메뉴생성
        /// </summary>
        /// <param name="toolbarType"></param>
        /// <param name="cont"></param>
        /// <returns></returns>
        private string AddNode2TreeList(int toolbarType, Control cont)
        {
            int admin_lv = 0;
            string qry = " SELECT * "
                       + "   FROM MSTEMBS "
                       + "  WHERE EMBSSABN = '" + SilkRoad.Config.SRConfig.USID + "' ";

            DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), DataAccess.DBname, qry);
            dp.AddDatatable2Dataset("MSTUSER_CHK", dt, ref ds);
            if (ds.Tables["MSTUSER_CHK"].Rows.Count > 0)
                admin_lv = clib.TextToInt(ds.Tables["MSTUSER_CHK"].Rows[0]["EMBSADGB"].ToString()); //권한레벨

            DevExpress.XtraTreeList.TreeList tl = cont as DevExpress.XtraTreeList.TreeList;
            string menuName = string.Empty;
            tl.BeginUnboundLoad();
            switch (toolbarType)
            {
                case 0:
                    menuName = "코드관리";
                    #region 코드관리
                    TreeListNode parentForRootNodes = null;
					
					TreeListNode rootNode = tl.AppendNode(new object[] { "기초코드", "", "", "" }, parentForRootNodes);
					tl.AppendNode(new object[] { "인사기본관리", "WAGE1000.duty1006", "", "1" }, rootNode);
					tl.AppendNode(new object[] { "사용자부서연결", "DUTY1000.duty1005", "", "1" }, rootNode);
					tl.AppendNode(new object[] { "간호사정보관리", "DUTY1000.duty1050", "", "" }, rootNode);
					tl.AppendNode(new object[] { "부서-직원설정", "DUTY1000.duty1011", "", "" }, rootNode);
					tl.AppendNode(new object[] { "근무유형정의", "DUTY1000.duty1030", "", "" }, rootNode);
					tl.AppendNode(new object[] { "휴일설정", "DUTY1000.duty1040", "", "" }, rootNode);

					TreeListNode rootNode2 = tl.AppendNode(new object[] { "급여관리", "", "", "" }, parentForRootNodes);
					tl.AppendNode(new object[] { "고정항목관리", "WAGE1000.wage3220", "", "" }, rootNode2);
					tl.AppendNode(new object[] { "변동항목관리", "WAGE1000.wage3230", "", "" }, rootNode2);
					#endregion
					break;
                case 1:
                    menuName = "Duty관리";
                    #region Duty관리
                    parentForRootNodes = null;
                    rootNode = tl.AppendNode(new object[] { "공지사항관리", "", "" }, parentForRootNodes);
					tl.AppendNode(new object[] { "공지사항관리", "DUTY1000.duty4010", "" }, rootNode);

					rootNode2 = tl.AppendNode(new object[] { "근무관리", "", "", "" }, parentForRootNodes);
					tl.AppendNode(new object[] { "CALL/OT/출장관리", "DUTY1000.duty2020", "", "" }, rootNode2); //OT조회및승인
					if (ACConfig.G_MSYN == "1" || admin_lv > 1 || SRConfig.US_DPCD.Contains("1000") || SRConfig.US_DPCD.Contains("1700") || SRConfig.US_DPCD.Contains("2500") || SRConfig.US_DPCD.Contains("2900"))  // 1000 내시경실, 1700 외래, 2500 종합검진실, 2900 진료협력실
                        tl.AppendNode(new object[] { "SAVE/OT내역", "DUTY1000.duty2030", "", "" }, rootNode2);
					tl.AppendNode(new object[] { "근무및당직관리", "DUTY1000.duty2060", "", "" }, rootNode2);
					tl.AppendNode(new object[] { "OFF신청조회", "DUTY1000.duty2010", "", "" }, rootNode2);
                    tl.AppendNode(new object[] { "간호사근무관리", "DUTY1000.duty3010", "", "" }, rootNode2);
					tl.AppendNode(new object[] { "간호사근무마감설정", "DUTY1000.duty3020", "", "" }, rootNode2);

					TreeListNode rootNode3 = tl.AppendNode(new object[] { "연차및휴가관리", "", "", "" }, parentForRootNodes);
					tl.AppendNode(new object[] { "연차신청및조회", "DUTY1000.duty8030", "", "" }, rootNode3);
					tl.AppendNode(new object[] { "휴가신청및조회", "DUTY1000.duty8050", "", "" }, rootNode3);
					tl.AppendNode(new object[] { "연차및휴가현황", "DUTY1000.duty8090", "", "" }, rootNode3);
					tl.AppendNode(new object[] { "사원별연차관리", "DUTY1000.duty8020", "", "" }, rootNode3);
					tl.AppendNode(new object[] { "연차휴가사용촉구현황", "DUTY1000.duty8010", "", "" }, rootNode3);
					
					TreeListNode rootNode4 = tl.AppendNode(new object[] { "문서관리", "", "", "" }, parentForRootNodes);
					tl.AppendNode(new object[] { "결재문서관리", "DUTY1000.duty5060", "", "" }, rootNode4);
					//tl.AppendNode(new object[] { "진행문서관리", "DUTY1000.duty5070", "", "" }, rootNode4);
					tl.AppendNode(new object[] { "완결문서관리", "DUTY1000.duty5080", "", "" }, rootNode4);

					#endregion
					break;
                case 2:
                    menuName = "마감관리";
                    #region 마감관리
                    parentForRootNodes = null;
                    rootNode = tl.AppendNode(new object[] { "마감관리", "", "" }, parentForRootNodes);
					tl.AppendNode(new object[] { "근무집계표", "DUTY1000.duty3080", "", "" }, rootNode);
					tl.AppendNode(new object[] { "최종마감관리", "DUTY1000.duty3090", "", "" }, rootNode);
					tl.AppendNode(new object[] { "KT근태연동조회", "DUTY1000.duty5010", "", "" }, rootNode);
					tl.AppendNode(new object[] { "연차정산관리", "DUTY1000.duty3030", "", "" }, rootNode);
					
					rootNode2 = tl.AppendNode(new object[] { "환경설정", "", "", "" }, parentForRootNodes);
					tl.AppendNode(new object[] { "시급관리", "DUTY1000.duty9010", "", "" }, rootNode2);
                    tl.AppendNode(new object[] { "밤근무수당관리", "DUTY1000.duty9015", "", "" }, rootNode2);
                    tl.AppendNode(new object[] { "당직시간관리", "DUTY1000.duty9030", "", "" }, rootNode2);
					tl.AppendNode(new object[] { "N수당관리", "DUTY1000.duty9040", "", "" }, rootNode2);
					tl.AppendNode(new object[] { "고정OT부서설정", "DUTY1000.duty9050", "", "" }, rootNode2);
					tl.AppendNode(new object[] { "수당산식설정", "DUTY1000.duty9020", "", "" }, rootNode2);

					#endregion
					break;
            }
            tl.EndUnboundLoad();
            string menuCaption = menuName + "항목";
            return menuCaption;
        }
        #endregion
    }
}
