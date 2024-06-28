using System;
using System.IO;
using System.Windows.Forms;

namespace SilkRoad.SILKDT01
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]

        static void Main(string[] args)
        {
			SilkRoad.DAL.DataAccess.DBtype = "1";
			SilkRoad.Config.SRConfig.SilkDnno = "20";              // DB_NO
			SilkRoad.DAL.DataAccess.OracleServiceName = "";
            if (args.Length > 0)
            {
                SilkRoad.Config.SRConfig.WorkPlaceNo = args[0].Trim(); // 사업장번호
                SilkRoad.DAL.DataAccess.DBhost = args[1].Trim();       // IP
                SilkRoad.DAL.DataAccess.DBusid = args[2].Trim();       // ID
                SilkRoad.DAL.DataAccess.DBuspw = args[3].Trim();       // PASSWORD
                SilkRoad.Config.SRConfig.USID = args[4].Trim();        // USID

                SilkRoad.DAL.DataAccess.DBname = "DT01DB" + Config.SRConfig.WorkPlaceNo;
            }
            else
            {
				//silkroad config 위치
				string parentPath, fullPath;
#if DEBUG
                parentPath = @"C:\silkroad\";
#else
                string drv = Application.StartupPath.Substring(0, 3);
                if (Application.StartupPath.ToUpper().Contains("SILKROAD"))                
                    parentPath = drv + Application.StartupPath.Substring(3, Application.StartupPath.ToUpper().IndexOf("SILKROAD") + 6).Replace("\\", "");                
                else                
                    parentPath = @"C:\silkroad\";                
#endif

                fullPath = parentPath + "\\config";

                #region 사업장번호 가져오기
                string connectionString = "Provider=VFPOLEDB.1;Data Source=" + fullPath + ";Mode=ReadWrite|Share Deny None;Collating Sequence=MACHINE";
				string queryStatements = "SELECT * FROM INFOCRNT.DBF";

				Microsoft.Practices.EnterpriseLibrary.Data.Database db = new Microsoft.Practices.EnterpriseLibrary.Data.GenericDatabase(connectionString, System.Data.OleDb.OleDbFactory.Instance);
				System.Data.Common.DbCommand dCom = db.GetSqlStringCommand(queryStatements);
				System.Data.DataSet ds = db.ExecuteDataSet(dCom);

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					Config.SRConfig.WorkPlaceNo = ds.Tables[0].Rows[0]["CRNTSQNO"].ToString().Trim();
					SilkRoad.DAL.DataAccess.DBname = "DT01DB" + Config.SRConfig.WorkPlaceNo;
				}

				#endregion
				
				#region 접속정보 가져오기
				queryStatements = "SELECT * FROM INFOCFCM.DBF";
				dCom = db.GetSqlStringCommand(queryStatements);
				ds = db.ExecuteDataSet(dCom);

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					DAL.DataAccess.DBhost = ds.Tables[0].Select("CFCMSQNO = '" + Config.SRConfig.WorkPlaceNo + "'")[0]["CFCMSVNM"].ToString().Trim();
					DAL.DataAccess.DBusid = ds.Tables[0].Select("CFCMSQNO = '" + Config.SRConfig.WorkPlaceNo + "'")[0]["CFCMSVUS"].ToString().Trim();
					DAL.DataAccess.DBuspw = ds.Tables[0].Select("CFCMSQNO = '" + Config.SRConfig.WorkPlaceNo + "'")[0]["CFCMSVPW"].ToString().Trim();
				}
				#endregion

				#region 로그인 사용자 가져오기
				queryStatements = "SELECT * FROM INFOCRNT.DBF";
				dCom = db.GetSqlStringCommand(queryStatements);
				ds = db.ExecuteDataSet(dCom);

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
					Config.SRConfig.USID = ds.Tables[0].Rows[0]["CRNTUSID"].ToString().Trim();

				#endregion
				
#if DEBUG
				//디버깅할때
				SilkRoad.Config.SRConfig.WorkPlaceNo = "01";
				SilkRoad.DAL.DataAccess.DBname = "DT01DB01";
                SilkRoad.DAL.DataAccess.DBhost = "125.136.91.159,9245";                                                                                                                                                                                                  
                SilkRoad.DAL.DataAccess.DBusid = "sa";       // ID
				SilkRoad.DAL.DataAccess.DBuspw = "Samil1234";
				SilkRoad.Config.SRConfig.USID = "SAMIL";
                //Config.SRConfig.USID = "21273"; //한지영
                //Config.SRConfig.USID = "21157"; //김정은
                //Config.SRConfig.USID = "24018"; //김정숙
#endif
            }

            string qry = " SELECT * FROM SYSLINES ";
            string commdb = "COMMDB" + Config.SRConfig.WorkPlaceNo;
            SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();
            System.Data.DataRow drow = gd.GetOneRowInQuery(Convert.ToInt32(SilkRoad.DAL.DataAccess.DBtype), commdb, qry);

            if (drow != null)
            {
                SilkRoad.Config.SRConfig.WorkPlaceName = drow["S_COMP"].ToString().Trim();
                SilkRoad.Config.SRConfig.WorkPlaceSano = drow["S_SANO"].ToString().Trim();
            }

            string titlename = "Duty관리_" + Config.SRConfig.WorkPlaceNo + "." + SilkRoad.Config.SRConfig.WorkPlaceName;
			
            string strFullPath = Application.StartupPath;         
            string tempTargetDir = Path.Combine(strFullPath, "TEMP");
			if (Directory.Exists(tempTargetDir))
				Directory.Delete(tempTargetDir, true);  
            tempTargetDir = Path.Combine(strFullPath, "DevTemp");
			if (Directory.Exists(tempTargetDir))
				Directory.Delete(tempTargetDir, true);
            /******     메인 잡을시 필수 필요사항!!!!!이 부분은 메인 새로 잡을때 꼭 복사해야함.*************************************************************/
            //메인에서 title을 "구매영업 01_(주)대한시멘트" 요런식으로 잡기 때문에 slkroad>config>infocrnt.dbf 파일에서 사업장번호 가져와서 구매영업 뒤에 붙여서 창 찾기..
            //메인프로젝트의  [속성]->[어셈블리정보]에서 제품을 실행파일이름하고 같도록 설정해주고..예를들면 SILKSLET로..
            //메인창 이름을 "구매영업" 등으로 꼭 설정해주기. [MainForm.Text]값임..
            //폼 체크할때 핸들값하고. 제목값을 비교해서 지금 띄워져있는 프로세스면 그거 앞으로 가져오게 하는거..
            /***********************************************************************************************************************************************/
            if (SISMonitor.ProcessChecker.IsOnlyProcess(titlename))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                frmMain main = new frmMain(titlename);
                Application.Run(main);
            }
        }
    }
}

