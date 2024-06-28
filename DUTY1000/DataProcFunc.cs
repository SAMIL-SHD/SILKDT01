using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SilkRoad.Common;
using SilkRoad.DAL;
using SilkRoad.DataProc;

namespace DUTY1000
{
    class DataProcFunc
    {
        static DataProcessing dp = new DataProcessing();
        static GetData gd = new GetData();
        static SetData sd = new SetData();
        static CommonLibrary clib = new CommonLibrary();
        static string dbname = DataAccess.DBname;
        static string wagedb = "WG06DB" + SilkRoad.Config.SRConfig.WorkPlaceNo;
        static string comm_db = "COMMDB" + SilkRoad.Config.SRConfig.WorkPlaceNo;
        //static SilkRoad.BaseCode.BaseCode bc = new BaseCode();

        string substring = (DataAccess.DBtype == "2") ? "SUBSTR" : "SUBSTRING"; //ORACLE(2) 과 MSSQL(1) 구문 차이
        string plus = (DataAccess.DBtype == "2") ? "||" : "+"; //ORACLE(2) 과 MSSQL(1) 구문 차이
        string isnull = (DataAccess.DBtype == "2") ? "NVL" : "ISNULL"; //ORACLE(2) 과 MSSQL(1) 구문 차이

        #region 1005 - 사용자부서설정

        //팀장/부서장/전체관리자 체크
        public void GetMSTUSER_CHKDatas(DataSet ds)
        {
            try
            {
                string qry = " SELECT * "
                           + "   FROM MSTEMBS "
                           + "  WHERE EMBSSABN = '" + SilkRoad.Config.SRConfig.USID + "' ";

                DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
                dp.AddDatatable2Dataset("MSTUSER_CHK", dt, ref ds);
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                        "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //전체관리/부서관리 여부 체크
        public void GetMSTUSER_CHK2Datas(DataSet ds)
        {
            try
            {
                string qry = " SELECT * "
                            + "   FROM DUTY_MSTUSER "
                            + "  WHERE USERIDEN = '" + SilkRoad.Config.SRConfig.USID + "' ";

                DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
                dp.AddDatatable2Dataset("MSTUSER_CHK", dt, ref ds);
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                        "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

		public void GetSEARCH_USERDEPTDatas(DataSet ds)
		{
			try
			{
				//string qry = " SELECT A.* "
				//		   + "   FROM "
				//		   + "        ( SELECT USERIDEN, USERNAME, "
				//		   + "                 (CASE WHEN RTRIM(USERDPCD)='' THEN NULL ELSE USERDPCD END) AS USERDPCD, USERUPYN, USERMSYN "
				//		   + "            FROM DUTY_MSTUSER "
				//		   + "         UNION ALL "
				//		   + "          SELECT RTRIM(USERIDEN) USERIDEN, RTRIM(USERNAME) USERNAME, NULL AS USERDPCD, '' USERUPYN, '' USERMSYN "
				//		   + "            FROM SILKDBCM..MSTUSER "
				//		   + "           WHERE USERIDEN NOT IN (SELECT USERIDEN FROM DUTY_MSTUSER) ) A "
				//		   + "  ORDER BY A.USERIDEN ";
				string qry = " SELECT RTRIM(A.USERIDEN) USERIDEN, RTRIM(A.USERNAME) USERNAME, X1.EMBSDPCD AS USERDPCD, '' USERUPYN, '' USERMSYN "
						   + "   FROM SILKDBCM..MSTUSER A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON A.USERIDEN=X1.EMBSSABN "
						   + "  WHERE X1.EMBSADGB IN ('2') " //X1.EMBSDPCD IS NOT NULL "
						   + "  ORDER BY A.USERIDEN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_USERDEPT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
        public void GetUserDeptDatas(string useid, DataSet ds)
        {
            try
            {
                string qry = " SELECT A.DEPRCODE AS CODE, A.DEPRCODE+' '+RTRIM(A.DEPRNAM1) AS NAME, "
                           + "        (CASE WHEN B.SABN IS NULL THEN '0' ELSE '1' END) AS CHK "
                           + "   FROM MSTDEPR A "
                           + "   LEFT OUTER JOIN DUTY_PWERDEPT B ON A.DEPRCODE = B.DEPT AND B.SABN = '" + useid + "'"
                           + "  WHERE A.DEPRSTAT = 1 ORDER BY A.DEPRCODE ";

                DataTable dt = gd.GetDataInQuery(1, dbname, qry);
                dp.AddDatatable2Dataset("USERDEPT", dt, ref ds);

            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void GetDUTY_PWERDEPTDatas(DataSet ds)
        {
            try
            {
                string qry = " SELECT * "
                           + "   FROM DUTY_PWERDEPT"
                           + "  WHERE 1=2 ";

                DataTable dt = gd.GetDataInQuery(1, dbname, qry);
                dp.AddDatatable2Dataset("DUTY_PWERDEPT", dt, ref ds);

            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

		//저장시
		public void GetDUTY_MSTUSERDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTUSER ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_MSTUSER", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//부서 LOOKUP
		public void GetSL_DEPTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT DEPRCODE CODE, RTRIM(DEPRNAM1) NAME "
						   + "   FROM " + wagedb +".DBO.MSTDEPR "
						   + "  WHERE DEPRSTAT=1 "
						   + "  ORDER BY DEPRCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SL_DEPT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
		
		#region 1006 - 인사기본관리
		
		//사업부코드 lookup
		public void GetWAGE_GLOVDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT GLOVCODE CODE, RTRIM(GLOVNAM1) NAME, (CASE GLOVSTAT WHEN 1 THEN '정상' ELSE '사용중지' END) STAT_NM "
						   + "   FROM " + wagedb + ".dbo.MSTGLOV "
						   + "  ORDER BY GLOVCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("WAGE_GLOV", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//부서코드 lookup
		public void GetWAGE_DEPRDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT DEPRCODE CODE, RTRIM(DEPRNAM1) NAME, (CASE DEPRSTAT WHEN 1 THEN '정상' ELSE '사용중지' END) STAT_NM "
						   + "   FROM " + wagedb + ".dbo.MSTDEPR "
						   + "  ORDER BY DEPRCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("WAGE_DEPR", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//직종코드 lookup
		public void GetWAGE_JONGDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT JONGCODE CODE, RTRIM(JONGNAM1) NAME, (CASE JONGSTAT WHEN 1 THEN '정상' ELSE '사용중지' END) STAT_NM "
						   + "   FROM " + wagedb + ".dbo.MSTJONG "
						   + "  ORDER BY JONGCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("WAGE_JONG", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//직위코드 lookup
		public void GetWAGE_GRADDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT GRADCODE CODE, RTRIM(GRADNAM1) NAME, (CASE GRADSTAT WHEN 1 THEN '정상' ELSE '사용중지' END) STAT_NM "
						   + "   FROM " + wagedb + ".dbo.MSTGRAD "
						   + "  ORDER BY GRADCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("WAGE_GRAD", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//인사기본정보조회
		public void GetSEARCH_MSTEMBSDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, "
						   + "        RTRIM(X1.GLOVNAM1) AS GLOV_NM, RTRIM(X2.DEPRNAM1) AS DEPR_NM, "
						   + "        RTRIM(X3.JONGNAM1) AS JONG_NM, RTRIM(X4.GRADNAM1) AS GRAD_NM, "
						   + "        LEFT(RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPTSA) as varchar(13))),6)+'-'+"
						   + "        SUBSTRING(RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPTSA) as varchar(13))),7,7) AS JMNO_NM, "
						   + "        RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPTSA) as varchar(13))) AS D_JMNO, "
						   + "        RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPHPN) as varchar(20))) AS D_HPNO, "
						   + "        RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPTLN) as varchar(20))) AS D_TLNO, "
						   + "        RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPAD1) as varchar(100))) AS D_ADR1, "
						   + "        RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPAD2) as varchar(100))) AS D_ADR2, "
						   + "        RTRIM(RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPAD1) as varchar(100)))+' '+ "
						   + "        cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPAD2) as varchar(100))) AS D_ADDR, "
						   + "        (CASE A.EMBSIPDT WHEN '' THEN '' ELSE LEFT(A.EMBSIPDT,4)+'-'+SUBSTRING(A.EMBSIPDT,5,2)+'-'+SUBSTRING(A.EMBSIPDT,7,2) END) IPDT_NM, "
						   + "        (CASE A.EMBSTSDT WHEN '' THEN '' ELSE LEFT(A.EMBSTSDT,4)+'-'+SUBSTRING(A.EMBSTSDT,5,2)+'-'+SUBSTRING(A.EMBSTSDT,7,2) END) TSDT_NM, "
						   + "        (CASE A.EMBSSTAT WHEN 1 THEN '재직' WHEN 2 THEN '퇴직' ELSE '' END) STAT_NM, "
						   + "        (CASE A.EMBSADGB WHEN '1' THEN '팀장' WHEN '2' THEN '부서장' "
						   + "              WHEN '3' THEN '원장단->담당원장' WHEN '4' THEN '원장단->대표원장' "
						   + "              WHEN '5' THEN '담당원장' WHEN '6' THEN '대표원장' ELSE '' END) ADGB_NM "
						   //+ "        (CASE A.EMBSADGB WHEN '1' THEN 'Y' ELSE '' END) ADGB_NM "
						   + "   FROM MSTEMBS A "
						   + "   LEFT OUTER JOIN MSTGLOV X1 "
						   + "     ON A.EMBSGLCD=X1.GLOVCODE "
						   + "   LEFT OUTER JOIN MSTDEPR X2 "
						   + "     ON A.EMBSDPCD=X2.DEPRCODE "
						   + "   LEFT OUTER JOIN MSTJONG X3 "
						   + "     ON A.EMBSJOCD=X3.JONGCODE "
						   + "   LEFT OUTER JOIN MSTGRAD X4 "
						   + "     ON A.EMBSGRCD=X4.GRADCODE "
						   + "  ORDER BY A.EMBSSABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), wagedb, qry);
				dp.AddDatatable2Dataset("SEARCH_MSTEMBS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//인사기본정보_상세
		public void GetMSTEMBSDatas(string sabn, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, "
						   + "        RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPTSA) as varchar(13))) AS D_JMNO, "
						   + "        RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPHPN) as varchar(20))) AS D_HPNO, "
						   + "        RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPTLN) as varchar(20))) AS D_TLNO, "
						   + "        RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPAD1) as varchar(100))) AS D_ADR1, "
						   + "        RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPAD2) as varchar(100))) AS D_ADR2 "
						   + "   FROM MSTEMBS A "
						   + "  WHERE A.EMBSSABN = '" + sabn + "'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), wagedb, qry);
				dp.AddDatatable2Dataset("MSTEMBS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//로그인 계정 여부
		public void GetCHK_MSTUSERDatas(string usid, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM SILKDBCM.DBO.MSTUSER A "
						   + "  WHERE A.USERIDEN = '" + usid + "'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), wagedb, qry);
				dp.AddDatatable2Dataset("CHK_MSTUSER", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//로그인 테이블 구조
		public void GetMSTUSERDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM SILKDBCM.DBO.MSTUSER "
						   + "  WHERE 1=2";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), wagedb, qry);
				dp.AddDatatable2Dataset("MSTUSER", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region 1010 - 파트관리

		//코드분류
		public void GetSEARCH_MENUDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT '1' GUBN, 'MSTGLOV' CODE, '사업부코드' GUBN_NM UNION ALL "
						   + " SELECT '2' GUBN, 'MSTDEPR' CODE, '부서코드' GUBN_NM UNION ALL "
						   + " SELECT '3' GUBN, 'DUTY_MSTPART' CODE, '파트코드' GUBN_NM ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_MENU", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//해당 코드조회
		public void GetSELECT_TABLEDatas(string gubn, string code, DataSet ds)
		{
			try
			{
				string qry = "";
				if (gubn == "1")				
					qry = " SELECT GLOVCODE CODE, NAME1 NAME, (CASE STAT WHEN 1 THEN '정상' WHEN 2 THEN '사용중지' ELSE '' END) STAT_NM, "
						+ "        (CASE WHEN LDAY='' THEN '' ELSE LEFT(LDAY,4)+'-'+SUBSTRING(LDAY,5,2)+'-'+SUBSTRING(LDAY,7,2) END) LDAY_NM "
						+ "   FROM " + wagedb + ".DBO." + code + " ORDER BY GLOVCODE";
				else if (gubn == "2")				
					qry = " SELECT DEPRCODE CODE, NAME1 NAME, (CASE STAT WHEN 1 THEN '정상' WHEN 2 THEN '사용중지' ELSE '' END) STAT_NM, "
						+ "        (CASE WHEN LDAY='' THEN '' ELSE LEFT(LDAY,4)+'-'+SUBSTRING(LDAY,5,2)+'-'+SUBSTRING(LDAY,7,2) END) LDAY_NM "
						+ "   FROM " + wagedb + ".DBO." + code + " ORDER BY DEPRCODE";
				else if (gubn == "3")				
					qry = " SELECT A.PARTCODE CODE, A.PARTNAME NAME, (CASE A.STAT WHEN 1 THEN '정상' WHEN 2 THEN '사용중지' ELSE '' END) STAT_NM, "
						+ "        (CASE WHEN A.LDAY='' THEN '' ELSE LEFT(A.LDAY,4)+'-'+SUBSTRING(A.LDAY,5,2)+'-'+SUBSTRING(A.LDAY,7,2) END) LDAY_NM, "
						+ "        RTRIM(X1.NAME1) AS DEPR_NM "
						+ "   FROM " + code + " A"
						+ "   LEFT OUTER JOIN " + wagedb + ".DBO.MSTDEPR X1 ON A.DEPRCODE=X1.DEPRCODE "
						+ "  ORDER BY A.PARTCODE";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SELECT_TABLE", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//파트코드 불러오기
		public void GetDUTY_MSTPARTDatas(string code, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTPART "
						   + "  WHERE PARTCODE = '" + code + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_MSTPART", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//소속부서 lookup
		public void GetSEARCH_DEPRDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT DEPRCODE CODE, RTRIM(DEPRNAM1) NAME "
						   + "   FROM " + wagedb + ".DBO.MSTDEPR "
						   + "  WHERE DEPRSTAT=1 "
						   + "  ORDER BY DEPRCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_DEPR", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
		
		#region 1050 - 간호사정보관리
		//부서코드 lookup
		public void GetSEARCH_DEPTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT DEPRCODE CODE, RTRIM(DEPRNAM1) NAME "
						   + "   FROM " + wagedb + ".dbo.MSTDEPR "
						   + "  WHERE DEPRSTAT=1 "
						   + "  ORDER BY DEPRCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_DEPT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//파트코드 lookup
		public void GetSEARCH_PARTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT PARTCODE CODE, RTRIM(PARTNAME) NAME "
						   + "   FROM DUTY_MSTPART "
						   + "  ORDER BY PARTCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_PART", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//직원코드 lookup
		public void GetSEARCH_NURSDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT A.SAWON_NO CODE, RTRIM(A.SAWON_NM) NAME "
						   + "   FROM DUTY_MSTNURS A "
						   + "  ORDER BY A.SAWON_NO ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_NURS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//근무코드 lookup
		public void GetSEARCH_GNMUDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT G_CODE CODE, RTRIM(G_FNM) NAME "
						   + "   FROM DUTY_MSTGNMU "
						   + "  ORDER BY G_CODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_GNMU", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		public void GetSEARCH_MSTNURSDatas(string dept, string stat, DataSet ds)
		{
			try
			{
				string qry = " SELECT A1.*, "
						   + "        A1.EXP_YEAR+A1.EXP_Y2+A1.EXP_Y3+A1.EXP_Y4 AS EXP_T, "
						   + "		  (CASE WHEN A1.GUBN='1' THEN '등록' ELSE '미등록' END) GUBN_NM, "
						   + "        (CASE WHEN A1.STAT=1 THEN '정상' ELSE '사용중지' END) STAT_NM, "
						   + "        (CASE A1.SHIFT_WORK WHEN 1 THEN 'Y' WHEN 2 THEN 'N' ELSE '' END) SHIFT_WORK_NM, "  //교대여부
						   + "        RTRIM(ISNULL(X3.DEPRNAM1,'')) DEPT_NM, "
						   + "        RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',X2.EMBSPHPN) as varchar(100))) HPNO, RTRIM(X2.EMBSEMAL) AS EMAIL_ID "
						   + "  FROM ( "
						   + "		SELECT '1' GUBN, A.SAWON_NO, A.SAWON_NM, ISNULL(X1.EMBSDPCD,'') DEPTCODE, ISNULL(A.SHIFT_WORK,0) SHIFT_WORK, "
						   + "             A.MAX_NCNT, A.ALLOWOFF, A.LIMIT_OFF, " 
						   + "			   A.RETURN_DT, A.CHARGE_YN, A.EXP_YEAR, A.EXP_Y2, A.EXP_Y3, A.EXP_Y4, A.STAT, A.LDAY, A.INDT, A.UPDT, A.USID, A.PSTY "
						   + "		  FROM DUTY_MSTNURS A "
						   + "        LEFT OUTER JOIN " + wagedb + ".DBO.MSTEMBS X1 ON A.SAWON_NO=X1.EMBSSABN "
						   + "     UNION ALL "
						   + "		SELECT '2' GUBN, RTRIM(A.EMBSSABN) AS SAWON_NO, RTRIM(A.EMBSNAME) AS SAWON_NM, A.EMBSDPCD AS DEPTCODE, 0 SHIFT_WORK, "
						   + "             0 MAX_NCNT, 0 ALLOWOFF, 0 LIMIT_OFF, "
						   + "			   '' RETURN_DT, '' CHARGE_YN, 0 EXP_YEAR, 0 EXP_Y2, 0 EXP_Y3, 0 EXP_Y4, 1 STAT, '' LDAY, '' INDT, '' UPDT, '' USID, '' PSTY "
						   + "		  FROM " + wagedb + ".DBO.MSTEMBS A "
						   + "        LEFT OUTER JOIN DUTY_MSTNURS X1 ON A.EMBSSABN=X1.SAWON_NO "
						   + "		 WHERE A.EMBSSTAT=1 AND A.EMBSDPCD IN (SELECT DEPTCODE FROM DUTY_INFONURS) AND X1.SAWON_NO IS NULL ) A1 "
						   + "  LEFT OUTER JOIN " + wagedb + ".DBO.MSTEMBS X2 ON A1.SAWON_NO=X2.EMBSSABN "
						   + "  LEFT OUTER JOIN " + wagedb + ".DBO.MSTDEPR X3 ON A1.DEPTCODE=X3.DEPRCODE "
						   + " WHERE A1.DEPTCODE LIKE '" + dept + "'";
				if (stat != "%")
					qry += " AND A1.STAT = " + stat + " ";
				qry += " ORDER BY A1.GUBN DESC, A1.DEPTCODE, A1.SAWON_NO ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_MSTNURS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//간호사코드 불러오기
		public void GetDUTY_MSTNURSDatas(string code, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTNURS "
						   + "  WHERE SAWON_NO = '" + code + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_MSTNURS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region 1051 - 간호사직종설정

		//직종코드 불러오기
		public void GetSEARCH_MSTJONGDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT (CASE WHEN X1.JONGCODE IS NOT NULL THEN '1' ELSE '0' END) CHK, "
						   + "        RTRIM(JONGNAM1) JONGNAME, A.*  "
						   + "   FROM " + wagedb + ".DBO.MSTJONG A "
						   + "   LEFT OUTER JOIN DUTY_INFOJONG X1 ON A.JONGCODE=X1.JONGCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_MSTJONG", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//삭제할 직종설정 테이블 불러오기
		public void GetD_DUTY_INFOJONGDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOJONG A ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("D_DUTY_INFOJONG", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//등록할 직종설정 테이블 불러오기
		public void GetDUTY_INFOJONGDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOJONG A ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_INFOJONG", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
		
		#region 1011 - 부서-직원설정

		//부서코드 불러오기
		public void GetSEARCH_MSTDEPTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT '' CODE, '없음' NAME "
						   + "  UNION ALL "
						   + " SELECT DEPRCODE CODE, RTRIM(DEPRNAM1) NAME "
						   + "   FROM " + wagedb + ".dbo.MSTDEPR "
						   + "  WHERE DEPRSTAT=1"
						   + "  ORDER BY CODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_MSTDEPT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//해당부서의 간호사 불러오기
		public void GetSEARCH_DEPT_NURSDatas(string dept, DataSet ds)
		{
			try
			{
				//string qry = " SELECT A.*, X1.EMBSDPCD DEPTCODE, RTRIM(X2.DEPRNAM1) DEPT_NM, '0' CHK, "
				//		   + "        (CASE A.EXP_LV WHEN 1 THEN '전문가' WHEN 2 THEN '숙련가' WHEN 3 THEN '초보자' ELSE '' END) EXP_LV_NM "
				//		   + "   FROM DUTY_MSTNURS A "
				//		   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X1 "
				//		   + "     ON A.SAWON_NO = X1.EMBSSABN "
				//		   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X2 "
				//		   + "     ON X1.EMBSDPCD = X2.DEPRCODE "
				//		   + "  WHERE X1.EMBSDPCD LIKE '" + dept + "'";
				string qry = " SELECT A.*, RTRIM(A.EMBSSABN) SAWON_NO, RTRIM(A.EMBSNAME) SAWON_NM, "
						   + "        A.EMBSDPCD DEPTCODE, RTRIM(X1.DEPRNAM1) DEPT_NM, '0' CHK "
						   + "   FROM MSTEMBS A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
						   + "     ON A.EMBSDPCD = X1.DEPRCODE "
						   + "  WHERE A.EMBSDPCD LIKE '" + dept + "'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_NURS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//부서이력 테이블 구조 불러오기
		public void GetDUTY_TRSDEPTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_TRSDEPT WHERE 1=2 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSDEPT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//해당직원 부서이력 불러오기
		public void GetS_TRSDEPTDatas(string sabn, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*,  "
						   + "        (CASE WHEN A.MOVE_DATE<>'' THEN LEFT(A.MOVE_DATE,4)+'.'+SUBSTRING(A.MOVE_DATE,5,2)+'.'+SUBSTRING(A.MOVE_DATE,7,2) "
						   + "              ELSE '' END) MOVE_DATE_NM,"
						   + "        A.FR_DEPT+"
						   + "        (CASE WHEN A.FR_DEPT='' THEN '' ELSE '('+RTRIM(ISNULL(X1.DEPRNAM1,''))+')' END) + ' -> '+A.TO_DEPT+ "
						   + "        (CASE WHEN A.TO_DEPT='' THEN '' ELSE '('+RTRIM(ISNULL(X2.DEPRNAM1,''))+')' END) AS DEPT_LOG, "
						   + "        (CASE WHEN X3.USERNAME IS NULL THEN A.REG_ID "
						   + "              ELSE A.REG_ID+'('+RTRIM(ISNULL(X3.USERNAME,''))+')' END) AS REG_ID_NM "
						   + "   FROM DUTY_TRSDEPT A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
						   + "     ON A.FR_DEPT = X1.DEPRCODE "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X2 "
						   + "     ON A.TO_DEPT = X2.DEPRCODE "
						   + "   LEFT OUTER JOIN SILKDBCM.DBO.MSTUSER X3 "
						   + "     ON A.REG_ID = X3.USERIDEN "
						   + "  WHERE A.SAWON_NO = '" + sabn + "' "
						   + "  ORDER BY A.DEPT_SEQ ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("S_TRSDEPT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//직원 부서이력 불러오기
		public void GetSEARCH_TRSDEPTDatas(string frdt, string todt, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*,  "
						   + "        (CASE WHEN A.MOVE_DATE<>'' THEN LEFT(A.MOVE_DATE,4)+'.'+SUBSTRING(A.MOVE_DATE,5,2)+'.'+SUBSTRING(A.MOVE_DATE,7,2) "
						   + "              ELSE '' END) MOVE_DATE_NM,"
						   + "        RTRIM(X1.EMBSNAME) EMBSNAME, "
						   + "        A.FR_DEPT+"
						   + "        (CASE WHEN A.FR_DEPT='' THEN '' ELSE '('+RTRIM(ISNULL(X2.DEPRNAM1,''))+')' END) + ' -> '+A.TO_DEPT+ "
						   + "        (CASE WHEN A.TO_DEPT='' THEN '' ELSE '('+RTRIM(ISNULL(X3.DEPRNAM1,''))+')' END) AS DEPT_LOG, "
						   + "        (CASE WHEN X4.USERNAME IS NULL THEN A.REG_ID "
						   + "              ELSE A.REG_ID+'('+RTRIM(ISNULL(X4.USERNAME,''))+')' END) AS REG_ID_NM "
						   + "   FROM DUTY_TRSDEPT A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 "
						   + "     ON A.SAWON_NO = X1.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X2 "
						   + "     ON A.FR_DEPT = X2.DEPRCODE "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X3 "
						   + "     ON A.TO_DEPT = X3.DEPRCODE "
						   + "   LEFT OUTER JOIN SILKDBCM.DBO.MSTUSER X4 "
						   + "     ON A.REG_ID = X4.USERIDEN "
						   + "  WHERE A.MOVE_DATE BETWEEN '" + frdt + "' AND '" + todt + "' "
						   + "  ORDER BY A.MOVE_DATE, A.SAWON_NO, A.DEPT_SEQ ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_TRSDEPT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
				
		#region 1012 - 파트-직원설정

		//파트코드 불러오기
		public void GetSEARCH_MSTPARTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT '' CODE, '없음' NAME "
						   + "  UNION ALL "
						   + " SELECT PARTCODE CODE, RTRIM(PARTNAME) NAME "
						   + "   FROM DUTY_MSTPART ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_MSTPART", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//해당파트의 간호사 불러오기
		public void GetSEARCH_PART_NURSDatas(string part, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, '0' CHK, RTRIM(X1.PARTNAME) PARTNAME, "
						   + "        (CASE A.EXP_LV WHEN 1 THEN '전문가' WHEN 2 THEN '숙련가' WHEN 3 THEN '초보자' ELSE '' END) EXP_LV_NM "
						   + "   FROM DUTY_MSTNURS A "
						   + "   LEFT OUTER JOIN DUTY_MSTPART X1 "
						   + "     ON A.PARTCODE = X1.PARTCODE "
						   + "  WHERE A.PARTCODE LIKE '" + part + "'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_NURS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//파트이력 테이블 구조 불러오기
		public void GetDUTY_TRSPARTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_TRSPART WHERE 1=2 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSPART", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//해당직원 파트이력 불러오기
		public void GetS_TRSPARTDatas(string sabn, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*,  "
						   + "        A.FR_PART+"
						   + "        (CASE WHEN A.FR_PART='' THEN '' ELSE '('+RTRIM(ISNULL(X1.PARTNAME,''))+')' END) + ' -> '+A.TO_PART+ "
						   + "        (CASE WHEN A.TO_PART='' THEN '' ELSE '('+RTRIM(ISNULL(X2.PARTNAME,''))+')' END) AS PART_LOG, "
						   + "        (CASE WHEN X3.USERNAME IS NULL THEN A.REG_ID "
						   + "              ELSE A.REG_ID+'('+RTRIM(ISNULL(X3.USERNAME,''))+')' END) AS REG_ID_NM "
						   + "   FROM DUTY_TRSPART A "
						   + "   LEFT OUTER JOIN DUTY_MSTPART X1 "
						   + "     ON A.FR_PART = X1.PARTCODE "
						   + "   LEFT OUTER JOIN DUTY_MSTPART X2 "
						   + "     ON A.TO_PART = X2.PARTCODE "
						   + "   LEFT OUTER JOIN SILKDBCM.DBO.MSTUSER X3 "
						   + "     ON A.REG_ID = X3.USERIDEN "
						   + "  WHERE A.SAWON_NO = '" + sabn + "' "
						   + "  ORDER BY A.PART_SEQ ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("S_TRSPART", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
				
		#region 1030 - 근무유형관리

		//근무유형 전체조회
		public void GetSEARCH_MSTGNMUDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, "
						   + "        (CASE A.G_FRTM WHEN '' THEN '' ELSE LEFT(A.G_FRTM,2)+':'+SUBSTRING(A.G_FRTM,3,2) END) G_FRTM_NM, "
						   + "        (CASE A.G_TOTM WHEN '' THEN '' ELSE LEFT(A.G_TOTM,2)+':'+SUBSTRING(A.G_TOTM,3,2) END) G_TOTM_NM, "
						   + "        (CASE A.G_TYPE WHEN 2 THEN 'Day like' WHEN 3 THEN 'Off like' WHEN 4 THEN 'Day' "
						   + "			             WHEN 5 THEN 'Evening' WHEN 6 THEN 'Night' WHEN 7 THEN 'Off' "
						   + "			             WHEN 8 THEN '연차' WHEN 9 THEN '당직' WHEN 10 THEN '낮당직' "
						   + "			             WHEN 12 THEN '휴가' "
						   + "                       ELSE '' END) G_TYPE_NM "
						   + "   FROM DUTY_MSTGNMU A "
						   + "  ORDER BY A.G_CODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_MSTGNMU", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//근무유형 처리
		public void GetDUTY_MSTGNMUDatas(string code, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTGNMU "
						   + "  WHERE G_CODE = '" + code + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_MSTGNMU", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//근무유형 다른테이블에서 사용중인지 체크
		public void GetCHK_GNMUDatas(string code, DataSet ds)
		{
			try
			{
				string qry = " SELECT '휴가/근무신청' CHK_NM "
						   + "   FROM DUTY_TRSOREQ "
						   + "  WHERE REQ_TYPE = '" + code + "' "
						   + "    AND PSTY<>'D' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("CHK_GNMU", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region 1040 - 휴일설정
		
		//휴일조회
		public void GetSEARCH_HolidayDatas(string bf_year, string year, DataSet ds)
		{
			try
			{
				string qry = " SELECT H_DATE, H_NAME, REPEAT_CHK, LEFT(DATENAME(DW,H_DATE),1) AS DD_NM, "
						   + "        LEFT(H_DATE,4)+'-'+SUBSTRING(H_DATE,5,2)+'-'+SUBSTRING(H_DATE,7,2) AS DD, "
						   + "        GUBN, (CASE GUBN WHEN '1' THEN '휴일' ELSE '대체휴일' END) GUBN_NM, "
						   + "        '등록' STAT_NM "
						   + "   FROM DUTY_MSTHOLI A "
						   + "  WHERE H_DATE LIKE '" + year + "%'"
						   //+ "    AND H_DATE NOT IN ( SELECT CONVERT(CHAR(8),DATEADD(YEAR,1,H_DATE),112) FROM DUTY_MSTHOLI "
						   //+ "                         WHERE H_DATE LIKE '" + bf_year + "%' AND REPEAT_CHK = '1') "
						   + "  UNION ALL"
						   + " SELECT CONVERT(CHAR(8),DATEADD(YEAR,1,H_DATE),112) H_DATE, H_NAME, REPEAT_CHK, "
						   + "        LEFT(DATENAME(DW,CONVERT(CHAR(8),DATEADD(YEAR,1,H_DATE),112)),1) AS DD_NM, "
						   + "        LEFT(CONVERT(CHAR(8),DATEADD(YEAR,1,H_DATE),112),4)+'-'+SUBSTRING(CONVERT(CHAR(8),DATEADD(YEAR,1,H_DATE),112),5,2)+'-'+SUBSTRING(CONVERT(CHAR(8),DATEADD(YEAR,1,H_DATE),112),7,2) AS DD, "
						   + "        GUBN, (CASE GUBN WHEN '1' THEN '휴일' ELSE '대체휴일' END) GUBN_NM, "
						   + "        '미등록' STAT_NM "
						   + "   FROM DUTY_MSTHOLI A "
						   + "  WHERE H_DATE LIKE '" + bf_year + "%'"
						   + "    AND REPEAT_CHK = '1' "
						   + "    AND H_DATE NOT IN ( SELECT CONVERT(CHAR(8),DATEADD(YEAR,-1,H_DATE),112) FROM DUTY_MSTHOLI "
						   + "                         WHERE H_DATE LIKE '" + year + "%' AND REPEAT_CHK = '1') "
						   + "  ORDER BY H_DATE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_HOLIDAY", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//휴일설정 테이블 구조 불러오기
		public void GetDUTY_MSTHOLIDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTHOLI WHERE 1=2 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_MSTHOLI", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//휴일설정 기존데이터 불러오기
		public void GetD_DUTY_MSTHOLIDatas(string year, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTHOLI "
						   + "  WHERE H_DATE LIKE '" + year +"%' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("D_DUTY_MSTHOLI", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
		
		#region wage3220 - 고정항목관리
		
		public void GetFIX_SDDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM " + wagedb + ".dbo.INFOWAGE A "
						   + "   LEFT OUTER JOIN (SELECT DISTINCT IFJWCODE, IFJWJYGB FROM INFOJOWG WHERE IFJWJYGB='11') X1 "
						   + "     ON A.IFWGCODE=X1.IFJWCODE "
						   + "  WHERE (A.IFWGCRGB='1' AND A.IFWGJYGB='11') OR (A.IFWGCRGB='2' AND X1.IFJWJYGB='11') "
						   + "  ORDER BY A.IFWGCODE ";
				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("FIX_SD", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//고정항목처리
		public void GetWGFX_LISTDatas(string dept, string tsdt, DataSet ds)
		{
			try
			{
				string qry = " SELECT RTRIM(A.WGFXSABN) AS SABN, "
					       + "        RTRIM(X1.EMBSNAME) AS EMBSNAME, "
					       + "        RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM, A.* "
						   + "   FROM " + wagedb + ".dbo.MSTWGFX A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON A.WGFXSABN=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE X1.EMBSDPCD LIKE '" + dept + "'"
						   + "    AND (X1.EMBSSTAT='1' OR X1.EMBSTSDT > '" + tsdt + "')"
						   + "  ORDER BY A.WGFXSABN ";
				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("WGFX_LIST", dt, ref ds);

				qry = " SELECT RTRIM(A.EMBSSABN) AS SABN, "
					+ "        RTRIM(A.EMBSNAME) AS SABN_NM, "
					+ "        RTRIM(ISNULL(X1.DEPRNAM1,'')) AS DEPT_NM "
					+ "   FROM " + wagedb + ".dbo.MSTEMBS A "
					+ "   LEFT OUTER JOIN MSTDEPR X1 ON A.EMBSDPCD=X1.DEPRCODE "
					+ "  WHERE A.EMBSDPCD LIKE '" + dept + "'"
					+ "    AND (A.EMBSSTAT='1' OR A.EMBSTSDT > '" + tsdt + "')"
					+ "  ORDER BY A.EMBSJOCD, A.EMBSDPCD, A.EMBSSABN ";

				dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("EMBS_LIST", dt, ref ds);

				for (int i = 0; i < ds.Tables["EMBS_LIST"].Rows.Count; i++)
				{
					DataRow drow = ds.Tables["EMBS_LIST"].Rows[i];
					if (ds.Tables["WGFX_LIST"].Select("SABN = '" + drow["SABN"].ToString() + "'").Length == 0)
					{
						DataRow nrow = ds.Tables["WGFX_LIST"].NewRow();
						nrow["SABN"] = drow["SABN"].ToString();
						nrow["EMBSNAME"] = drow["SABN_NM"].ToString();
						nrow["DEPT_NM"] = drow["DEPT_NM"].ToString();
						ds.Tables["WGFX_LIST"].Rows.Add(nrow);
					}
				}
				ds.Tables["WGFX_LIST"].DefaultView.Sort = "SABN ASC";
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void GetMSTWGFXDatas(string sabn, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM " + wagedb + ".dbo.MSTWGFX A "
						   + "  WHERE A.WGFXSABN LIKE '" + sabn + "'"
						   + "  ORDER BY A.WGFXSABN ";
				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("MSTWGFX", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region wage3230 - 변동항목관리
		
		public void GetWGPC_SDDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM " + wagedb + ".dbo.INFOWAGE A "
						   + "   LEFT OUTER JOIN (SELECT DISTINCT IFJWCODE, IFJWJYGB FROM INFOJOWG WHERE IFJWJYGB='12') X1 "
						   + "     ON A.IFWGCODE=X1.IFJWCODE "
						   + "  WHERE ((A.IFWGCRGB='1' AND A.IFWGJYGB IN ('12','39','40','41','43')) "
						   + "        OR (A.IFWGCRGB='2' AND X1.IFJWJYGB='12')) "
						   + "    AND A.IFWGPUSE='1' "
						   + "  ORDER BY A.IFWGCODE ";
				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("WGPC_SD", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//고정항목처리
		public void GetWGPC_LISTDatas(string yymm, string sqno, string dept, string tsdt, DataSet ds)
		{
			try
			{
				string qry = " SELECT RTRIM(A.WGPCSABN) AS SABN, "
					       + "        RTRIM(X1.EMBSNAME) AS EMBSNAME, "
					       + "        RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM, A.* "
						   + "   FROM " + wagedb + ".dbo.MSTWGPC A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON A.WGPCSABN=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE A.WGPCYYMM = '" + yymm + "'"
						   + "    AND A.WGPCSQNO = '" + sqno + "'"
						   + "    AND X1.EMBSDPCD LIKE '" + dept + "'"
						   + "    AND (X1.EMBSSTAT='1' OR X1.EMBSTSDT > '" + tsdt + "')"
						   + "  ORDER BY A.WGPCSABN ";
				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("WGPC_LIST", dt, ref ds);

				qry = " SELECT RTRIM(A.EMBSSABN) AS SABN, "
					+ "        RTRIM(A.EMBSNAME) AS SABN_NM, "
					+ "        RTRIM(ISNULL(X1.DEPRNAM1,'')) AS DEPT_NM "
					+ "   FROM " + wagedb + ".dbo.MSTEMBS A "
					+ "   LEFT OUTER JOIN MSTDEPR X1 ON A.EMBSDPCD=X1.DEPRCODE "
					+ "  WHERE A.EMBSDPCD LIKE '" + dept + "'"
					+ "    AND (A.EMBSSTAT='1' OR A.EMBSTSDT > '" + tsdt + "')"
					+ "  ORDER BY A.EMBSJOCD, A.EMBSDPCD, A.EMBSSABN ";

				dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("EMBS_LIST", dt, ref ds);

				for (int i = 0; i < ds.Tables["EMBS_LIST"].Rows.Count; i++)
				{
					DataRow drow = ds.Tables["EMBS_LIST"].Rows[i];
					if (ds.Tables["WGPC_LIST"].Select("SABN = '" + drow["SABN"].ToString() + "'").Length == 0)
					{
						DataRow nrow = ds.Tables["WGPC_LIST"].NewRow();
						nrow["WGPCYYMM"] = yymm;
						nrow["WGPCSQNO"] = sqno;
						nrow["SABN"] = drow["SABN"].ToString();
						nrow["EMBSNAME"] = drow["SABN_NM"].ToString();
						nrow["DEPT_NM"] = drow["DEPT_NM"].ToString();
						ds.Tables["WGPC_LIST"].Rows.Add(nrow);
					}
				}
				ds.Tables["WGPC_LIST"].DefaultView.Sort = "SABN ASC";
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void GetMSTWGPCDatas(string yymm, string sqno, string sabn, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM " + wagedb + ".dbo.MSTWGPC A "
						   + "  WHERE A.WGPCYYMM = '" + yymm + "'"
						   + "    AND A.WGPCSQNO = '" + sqno + "'"
						   + "    AND A.WGPCSABN = '" + sabn + "'"
						   + "  ORDER BY A.WGPCSABN ";
				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("MSTWGPC", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion


		#region 2020 - OT조회및승인

		//최종마감 데이터
		public void Get2020_SEARCH_ENDSDatas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTENDS "
						   + "  WHERE END_YYMM = '" + yymm + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2020_SEARCH_ENDS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//부서별 직원LOOKUP
		public void Get2020_SEARCH_EMBSDatas(string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT RTRIM(A.EMBSSABN) CODE, RTRIM(A.EMBSNAME) NAME, "
						   + "        RTRIM(X1.DEPRNAM1) DEPT_NM, "
                           + "        (CASE A.EMBSSTAT WHEN 1 THEN '재직' WHEN 2 THEN '퇴직' ELSE '' END) STAT_NM, "
                           + "        ISNULL(A.EMBSADGB,'') EMBSADGB "
						   + "   FROM " + wagedb + ".dbo.MSTEMBS A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
						   + "     ON A.EMBSDPCD = X1.DEPRCODE"
						   + "  WHERE A.EMBSDPCD LIKE '" + dept + "' " //A.EMBSSTAT='1' AND 
						   + "  ORDER BY A.EMBSSTAT, A.EMBSNAME";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2020_SEARCH_EMBS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//OT근무 LOOKUP
		public void Get2020_SEARCH_GNMUDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT G_CODE, G_FNM, G_SNM "
						   + "   FROM DUTY_MSTGNMU "
						   + "  WHERE G_TYPE<7 ORDER BY G_CODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2020_SEARCH_GNMU", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//OT조회
		public void GetSEARCH_OVTMDatas(string gubn, string frmm, string tomm, string dept, DataSet ds)
		{
			try
			{
                string dt_nm = "SEARCH_OVTM";
                if (gubn == "2")
                    dt_nm = "SEARCH_OVTM2";
                else if (gubn == "3")
                    dt_nm = "SEARCH_OVTM3";

                string qry = " SELECT A.*, '' CHK, '' C_CHK, "
						   + "        A.CALL_CNT1+A.CALL_CNT2 AS CALL_CNT, "
						   + "        A.CALL_TIME1+A.CALL_TIME2 AS CALL_TIME, "
						   + "        A.OT_TIME1+A.OT_TIME2 AS OT_TIME, "
						   + "        (CASE A.OT_GUBN WHEN '1' THEN '콜' WHEN '2' THEN 'OT' ELSE '출장' END) GUBN_NM, "
                           + "		  RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        LEFT(A.OT_DATE,4)+'-'+SUBSTRING(A.OT_DATE,5,2) AS OT_YYMM, "
						   + "        LEFT(A.OT_DATE,4)+'-'+SUBSTRING(A.OT_DATE,5,2)+'-'+SUBSTRING(A.OT_DATE,7,2) AS SLDT_NM, "
						   + "        (CASE C2.AP_TAG WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' WHEN '4' THEN '진행' WHEN '5' THEN '반려' ELSE '' END) AP_TAG_NM, "
						   + "        RTRIM(ISNULL(X1.SAWON_NM,X3.EMBSNAME)) SAWON_NM, "
						   + "        (CASE A.OT_GUBN WHEN '1' THEN '콜' WHEN '2' THEN 'OT' ELSE '출장' END) AS REMARK "
                           + "   FROM DUTY_TRSOVTM A "
						   + "   LEFT OUTER JOIN DUTY_MSTNURS X1 "
						   + "     ON A.SABN=X1.SAWON_NO "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						   + "     ON A.SABN=X3.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						   + "     ON X3.EMBSDPCD=X4.DEPRCODE "
						   + "   LEFT OUTER JOIN DUTY_GWDOC C2 "
						   + "     ON A.DOC_NO=C2.DOC_NO AND C2.AP_TAG NOT IN ('2','5')"
						   + "  WHERE LEFT(A.OT_DATE,6) BETWEEN '" + frmm + "' AND '" + tomm + "' "
						   + "    AND X3.EMBSDPCD LIKE '" + dept + "' AND A.OT_GUBN LIKE '" + gubn + "' "
						   + "  ORDER BY A.OT_GUBN, X3.EMBSDPCD, A.SABN, A.OT_DATE  ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset(dt_nm, dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//콜,OT 조회
		public void GetDUTY_TRSOVTMDatas(string sabn, string sldt, string gubn, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, C2.AP_TAG AS GW_TAG "
						   + "   FROM DUTY_TRSOVTM A "
						   //+ "   LEFT OUTER JOIN GW_TRSOVTM C1 "
						   //+ "     ON A.SABN=C1.SABN "
						   //+ "    AND A.OT_DATE=C1.OT_DATE "
						   //+ "    AND A.OT_GUBN=C1.OT_GUBN "
						   + "   LEFT OUTER JOIN DUTY_GWDOC C2 "
						   + "     ON A.DOC_NO=C2.DOC_NO AND C2.AP_TAG NOT IN ('2','5')"
						   + "  WHERE A.SABN = '" + sabn + "'"
						   + "    AND A.OT_DATE = '" + sldt + "' AND A.OT_GUBN = '" + gubn + "'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSOVTM", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//콜,OT 삭제 테이블 구조
		public void GetDEL_TRSOVTMDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DEL_TRSOVTM "
						   + "  WHERE 1=2";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DEL_TRSOVTM", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
		
		#region 2021 - CALL/OT 결재상신
		
		//CALL,출장 조회
		public void Get2021_SEARCH_CALLDatas(string yymm, string dept, string gubn, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, '1' CHK, "
						   + "        A.CALL_CNT1+A.CALL_CNT2 AS CALL_CNT, "
						   + "        A.CALL_TIME1+A.CALL_TIME2 AS CALL_TIME, "
						   + "        LEFT(A.OT_DATE,4)+'.'+SUBSTRING(A.OT_DATE,5,2) AS OT_YYMM, "
						   + "        LEFT(A.OT_DATE,4)+'.'+SUBSTRING(A.OT_DATE,5,2)+'.'+SUBSTRING(A.OT_DATE,7,2) AS OT_DATE_NM, "
						   + "        RTRIM(X1.EMBSNAME) EMBSNAME, RTRIM(X2.DEPRNAM1) DEPT_NM "
						   + "   FROM DUTY_TRSOVTM A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 "
						   + "     ON A.SABN = X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 "
						   + "     ON X1.EMBSDPCD = X2.DEPRCODE "
						   + "   LEFT OUTER JOIN DUTY_GWDOC C2 "
						   + "     ON A.DOC_NO=C2.DOC_NO AND C2.AP_TAG NOT IN ('2','5')"
						   + "  WHERE A.OT_GUBN='" + gubn + "' AND LEFT(A.OT_DATE,6) = '" + yymm + "' "
						   + "    AND X1.EMBSDPCD LIKE '" + dept + "' AND ISNULL(C2.AP_TAG,'') IN ('','5')"
						   + "  ORDER BY A.SABN, A.OT_DATE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2021_SEARCH_CALL", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//OT,정산 조회
		public void Get2022_SEARCH_CALLDatas(string yymm, string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, (CASE WHEN A.TIME_REMK='' THEN '' ELSE '1' END) CHK, "
						   + "        LEFT(A.OT_DATE,4)+'.'+SUBSTRING(A.OT_DATE,5,2) AS OT_YYMM, "
						   + "        LEFT(A.OT_DATE,4)+'.'+SUBSTRING(A.OT_DATE,5,2)+'.'+SUBSTRING(A.OT_DATE,7,2) AS OT_DATE_NM, "
						   + "        RTRIM(X1.EMBSNAME) EMBSNAME, RTRIM(X2.DEPRNAM1) DEPT_NM "
						   + "   FROM ( "
						   + "        SELECT '1' GUBN, '시간외' GUBN_NM, "
						   + "               A.SABN, A.OT_DATE, A.OT_TIME1, A.OT_TIME2, "						   
						   + "               A.OT_TIME1+A.OT_TIME2 AS OT_TIME, "
						   + "               0 AS JS_TIME, 0 AS CALC_TIME, "
						   + "               A.TIME_REMK, A.REMARK, A.DOC_NO "
						   + "          FROM DUTY_TRSOVTM A "
						   + "         WHERE A.OT_GUBN='2' AND LEFT(A.OT_DATE,6) = '" + yymm + "' "
						   + "       UNION ALL "
						   + "        SELECT '2' GUBN, '정산' GUBN_NM, "
						   + "               A.SABN, A.JS_DATE AS OT_DATE, 0 AS OT_TIME1, 0 AS OT_TIME2, "
						   + "               A.CALC_TIME AS OT_TIME, "
						   + "               A.JS_TIME, A.CALC_TIME, "
						   + "               A.TIME_REMK, A.REMARK, A.DOC_NO "
						   + "          FROM DUTY_TRSOVTM_JS A "
						   + "         WHERE LEFT(A.JS_DATE,6) = '" + yymm + "' "
						   + "        ) A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 "
						   + "     ON A.SABN = X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 "
						   + "     ON X1.EMBSDPCD = X2.DEPRCODE "
						   + "   LEFT OUTER JOIN DUTY_GWDOC C2 "
						   + "     ON A.DOC_NO=C2.DOC_NO AND C2.AP_TAG NOT IN ('2','5') "
						   //+ "  WHERE A.OT_GUBN='2' AND LEFT(A.OT_DATE,6) = '" + yymm + "' "
						   + "  WHERE X1.EMBSDPCD = '" + dept + "' AND ISNULL(C2.AP_TAG,'') IN ('','5')"
						   + "  ORDER BY A.SABN, A.OT_DATE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2022_SEARCH_CALL", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//GW_TRSOVTM 테이블 불러오기
		public void GetGW_TRSOVTMDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM GW_TRSOVTM WHERE 1=2 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("GW_TRSOVTM", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//GW_TRSOVTM 테이블 불러오기
		public void GetGW_TRSOVTM_JSDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM GW_TRSOVTM_JS WHERE 1=2 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("GW_TRSOVTM_JS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		//DUTY_GWDOC 테이블 불러오기
		public void GetDUTY_GWDOCDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_GWDOC WHERE 1=2 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_GWDOC", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}		
		//문서번호 가져오기
		public decimal GetGWDOC_NODatas(DataSet ds)
		{
			decimal max_sq = 0;
			try
			{
				string qry = " SELECT ISNULL(MAX(DOC_NO) + 1, 1) AS MAX_SQ "
						   + "   FROM DUTY_GWDOC ";
				
                object obj = gd.GetOneData(1, dbname, qry);
                max_sq = clib.TextToDecimal(obj.ToString());

			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return max_sq;
		}

		#endregion

		
		#region 2023 - 콜 일괄등록
		
		//부서별 직원리스트 조회
		public void GetSEARCH_DEPT_CALLDatas(string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT '' DATE_NM, "
						   + "        RTRIM(A.EMBSSABN) SABN, RTRIM(A.EMBSNAME) EMBSNAME, "
						   + "        0 AS CALL_CNT1, 0 AS CALL_CNT2, "
						   + "        0 AS CALL_TIME1, 0 AS CALL_TIME2, "
						   + "        '' TIME_REMK, '' REMARK, '0' CHK "
						   + "   FROM " + wagedb + ".dbo.MSTEMBS A "
						   + "  WHERE A.EMBSSTAT='1' AND A.EMBSDPCD = '" + dept + "'"
						   + "  ORDER BY A.EMBSSABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_DEPT_CALL", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region 2024 - OT 일괄등록
		
		//부서별 직원리스트 조회
		public void GetSEARCH_DEPT_OTDatas(string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT '' DATE_NM, "
						   + "        RTRIM(A.EMBSSABN) SABN, RTRIM(A.EMBSNAME) EMBSNAME, "
						   + "        0 AS OT_TIME1, 0 AS OT_TIME2, "
						   + "        '' TIME_REMK, '' REMARK, '0' CHK "
						   + "   FROM " + wagedb + ".dbo.MSTEMBS A "
						   + "  WHERE A.EMBSSTAT='1' AND A.EMBSDPCD = '" + dept + "'"
						   + "  ORDER BY A.EMBSSABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_DEPT_OT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//직원별 리스트 조회
		public void GetSEARCH_DEPT_OTDatas(string sabn, string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, '' CHK, "
						   + "        LEFT(A.OT_DATE,4)+'-'+SUBSTRING(A.OT_DATE,5,2)+'-'+SUBSTRING(A.OT_DATE,7,2) AS DATE_NM, "
						   + "        (CASE DATEPART(DW,A.OT_DATE) WHEN 1 THEN '일' WHEN 2 THEN '월' WHEN 3 THEN '화' "
						   + "         WHEN 4 THEN '수' WHEN 5 THEN '목' WHEN 6 THEN '금' WHEN 7 THEN '토' ELSE '' END) DAY_NM, "
						   + "        C2.AP_TAG AS GW_TAG "
						   + "   FROM DUTY_TRSOVTM A "
						   + "   LEFT OUTER JOIN DUTY_GWDOC C2 "
						   + "     ON A.DOC_NO=C2.DOC_NO AND C2.AP_TAG NOT IN ('2','5')"
						   + "  WHERE A.SABN = '" + sabn + "'"
						   + "    AND LEFT(A.OT_DATE,6) = '" + yymm + "' AND A.OT_GUBN = '2'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_EMBS_OT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        #endregion

        #region 2030 - OT&SAVE 보고서

        //부서코드 불러오기
        public void Get2030_DEPTDatas(string dpcd, DataSet ds)
        {
            try
            {
                string qry = " SELECT A.DEPRCODE CODE, RTRIM(A.DEPRNAM1) NAME "
                           + "   FROM MSTDEPR A ";
                if (dpcd == "%")
                    qry += "  WHERE DEPRCODE IN ('1000','1700','2500','2900') ";
                else
                    qry += "  WHERE DEPRCODE = '" + dpcd + "' ";
                qry += "  ORDER BY DEPRCODE ";

                DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
                dp.AddDatatable2Dataset("2030_DEPT", dt, ref ds);
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //SAVE-OT 조회
        public void GetSEARCH_SAVEDatas(string dept, string yymm, DataSet ds)
		{
			try
            {
                string qry = "";
                if (dept == "")
                    qry = " EXEC USP_DUTY2030_RPT_220809 '" + yymm + "' ";
                else
                    qry = " EXEC USP_DUTY2030_RPT_230301 '" + yymm + "', '" + dept + "' ";

                DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_SAVE", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        //부서별 직원LOOKUP
        public void Get2030_SEARCH_EMBSDatas(string dpcd, DataSet ds)
        {
            try
            {
                string qry = " SELECT RTRIM(A.EMBSSABN) CODE, RTRIM(A.EMBSNAME) NAME, "
                           + "        RTRIM(X1.DEPRNAM1) DEPT_NM, A.EMBSDPCD, "
                           + "        (CASE A.EMBSSTAT WHEN 1 THEN '재직' WHEN 2 THEN '퇴직' ELSE '' END) STAT_NM, "
                           + "        ISNULL(A.EMBSADGB,'') EMBSADGB "
                           + "   FROM " + wagedb + ".dbo.MSTEMBS A "
                           + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
                           + "     ON A.EMBSDPCD = X1.DEPRCODE";
                if (dpcd == "%")
                    qry += "  WHERE A.EMBSDPCD IN ('1000','1700','2500','2900') ";
                else
                    qry += "  WHERE A.EMBSDPCD = '" + dpcd + "'";
                qry += "  ORDER BY A.EMBSSTAT, A.EMBSDPCD, A.EMBSNAME";

                DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
                dp.AddDatatable2Dataset("2030_SEARCH_EMBS", dt, ref ds);
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 2031 - OT&SAVE관리

        //저장내역조회
        public void GetS_DUTY_TRSSVTMDatas(string dpcd, string yymm, DataSet ds)
		{
			try
			{
                string qry = " SELECT A.*, "
                           + "        LEFT(A.SAVE_DATE,4)+'-'+SUBSTRING(A.SAVE_DATE,5,2)+'-'+SUBSTRING(A.SAVE_DATE,7,2) AS SLDT_NM, "
                           + "        RTRIM(X1.EMBSNAME) SAWON_NM, "
                           + "        RTRIM(X2.DEPRNAM1) DEPT_NM "
                           + "   FROM DUTY_TRSSVTM A "
                           + "   LEFT OUTER JOIN MSTEMBS X1 "
                           + "     ON A.SABN = X1.EMBSSABN "
                           + "   LEFT OUTER JOIN MSTDEPR X2 "
                           + "     ON X1.EMBSDPCD = X2.DEPRCODE "
                           + "  WHERE LEFT(A.SAVE_DATE,6) = '" + yymm + "'";
                if (dpcd != "%")
                    qry += " AND X1.EMBSDPCD = '" + dpcd + "'";

                qry += "  ORDER BY A.SABN, A.SAVE_DATE" ;

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("S_DUTY_TRSSVTM", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//SAVE시간 테이블
		public void GetDUTY_TRSSVTMDatas(string embs, string sldt, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_TRSSVTM "
						   + "  WHERE SABN = '" + embs +"' AND SAVE_DATE = '" + sldt +"' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSSVTM", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//SAVE마감 조회
		public void GetS_SAVE_ENDSDatas(string dept, string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTSAVE_ENDS "
						   + "  WHERE DEPT = '" + dept + "' "
                           + "    AND END_YYMM = '" + yymm + "'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("S_SAVE_ENDS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//SAVE 삭제 테이블 구조
		public void GetDEL_TRSSVTMDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DEL_TRSSVTM "
						   + "  WHERE 1=2";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DEL_TRSSVTM", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//년월
		public void GetSEARCH_SAVE_ENDSDatas(string dept, string dept_nm, string year, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, RTRIM(X1.DEPRNAM1) DEPT_NM, "
						   + "        LEFT(A.END_YYMM,4)+'-'+SUBSTRING(A.END_YYMM,5,2) END_YYMM_NM, "
						   + "        (CASE WHEN A.CLOSE_YN='1' THEN END_DT+'('+END_ID+')' ELSE CANC_DT+'('+CANC_ID+')' END) MOD_DT, "
						   + "        (CASE A.CLOSE_YN WHEN '1' THEN '마감' WHEN '2' THEN '마감취소' ELSE '' END) CLOSE_NM "
						   + "   FROM DUTY_MSTSAVE_ENDS A "
                           + "   LEFT OUTER JOIN MSTDEPR X1 "
                           + "     ON A.DEPT = X1.DEPRCODE "
                           + "  WHERE A.DEPT = '" + dept + "' AND LEFT(A.END_YYMM,4) = '" + year +"' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_SAVE_ENDS", dt, ref ds);
				
                for (int i = 1; i <= 12; i++)
                {
                    if (ds.Tables["SEARCH_SAVE_ENDS"].Select("END_YYMM = '" + year.Substring(0, 4) + i.ToString().PadLeft(2, '0') + "'").Length == 0)
                    {
                        DataRow drow = ds.Tables["SEARCH_SAVE_ENDS"].NewRow();
                        drow["DEPT"] = dept;
                        drow["DEPT_NM"] = dept_nm;
                        drow["END_YYMM"] = year + i.ToString().PadLeft(2, '0');
						drow["END_YYMM_NM"] = year + "-" + i.ToString().PadLeft(2, '0');
                        drow["CLOSE_YN"] = "";
                        ds.Tables["SEARCH_SAVE_ENDS"].Rows.Add(drow);
                    }
                }
                ds.Tables["SEARCH_SAVE_ENDS"].DefaultView.Sort = "END_YYMM";
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void GetDUTY_MSTSAVE_ENDSDatas(string dept, string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM DUTY_MSTSAVE_ENDS A "
						   + "  WHERE A.DEPT = '" + dept + "' AND A.END_YYMM = '" + yymm + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_MSTSAVE_ENDS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//직원별 리스트 조회
		//public void GetSEARCH_DEPT_OTDatas(string sabn, string yymm, DataSet ds)
		//{
		//	try
		//	{
		//		string qry = " SELECT A.*, '' CHK, "
		//				   + "        LEFT(A.OT_DATE,4)+'-'+SUBSTRING(A.OT_DATE,5,2)+'-'+SUBSTRING(A.OT_DATE,7,2) AS DATE_NM, "
		//				   + "        (CASE DATEPART(DW,A.OT_DATE) WHEN 1 THEN '일' WHEN 2 THEN '월' WHEN 3 THEN '화' "
		//				   + "         WHEN 4 THEN '수' WHEN 5 THEN '목' WHEN 6 THEN '금' WHEN 7 THEN '토' ELSE '' END) DAY_NM, "
		//				   + "        C2.AP_TAG AS GW_TAG "
		//				   + "   FROM DUTY_TRSOVTM A "
		//				   + "   LEFT OUTER JOIN DUTY_GWDOC C2 "
		//				   + "     ON A.DOC_NO=C2.DOC_NO AND C2.AP_TAG NOT IN ('2','5')"
		//				   + "  WHERE A.SABN = '" + sabn + "'"
		//				   + "    AND LEFT(A.OT_DATE,6) = '" + yymm + "' AND A.OT_GUBN = '2'";

		
		//저장내역조회
		public void GetS_DUTY_TRSOVTM_JSDatas(string dpcd, string yymm, DataSet ds)
		{
			try
			{
                string qry = " SELECT A.*, "
                           + "        LEFT(A.JS_DATE,4)+'-'+SUBSTRING(A.JS_DATE,5,2)+'-'+SUBSTRING(A.JS_DATE,7,2) AS SLDT_NM, "
                           + "        RTRIM(X1.EMBSNAME) SAWON_NM, "
                           + "        RTRIM(X2.DEPRNAM1) DEPT_NM "
                           + "   FROM DUTY_TRSOVTM_JS A "
                           + "   LEFT OUTER JOIN MSTEMBS X1 "
                           + "     ON A.SABN = X1.EMBSSABN "
                           + "   LEFT OUTER JOIN MSTDEPR X2 "
                           + "     ON X1.EMBSDPCD = X2.DEPRCODE "
                           + "  WHERE LEFT(A.JS_DATE,6) = '" + yymm + "'";
                if (dpcd != "%")
                    qry += " AND X1.EMBSDPCD = '" + dpcd + "'";
                qry += "  ORDER BY A.SABN, A.JS_DATE" ;

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("S_DUTY_TRSOVTM_JS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//SAVE시간 테이블
		public void GetDUTY_TRSOVTM_JSDatas(string embs, string sldt, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_TRSOVTM_JS "
						   + "  WHERE SABN = '" + embs +"' AND JS_DATE = '" + sldt +"' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSOVTM_JS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion	
		
		#region 2030 - OT&SAVE 보고서xxxxxx
		//당직 캘린더 조회
		//public void GetSEARCH_SAVEDatas(string yymm, DataSet ds)
		//{
		//	try
		//	{
		//		string qry = " EXEC USP_DUTY2030_RPT_220809 '" + yymm + "' ";

		//		DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
		//		dp.AddDatatable2Dataset("SEARCH_SAVE", dt, ref ds);
		//	}
		//	catch (System.Exception ec)
		//	{
		//		System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
		//											 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}

		#endregion	
		
		#region 2031 - OT&SAVE관리xxxxx
		
		//저장내역조회
		//public void GetS_DUTY_TRSSVTMDatas(string yymm, DataSet ds)
		//{
		//	try
		//	{
		//		string qry = " SELECT A.*, "
		//				   + "        LEFT(A.SAVE_DATE,4)+'-'+SUBSTRING(A.SAVE_DATE,5,2)+'-'+SUBSTRING(A.SAVE_DATE,7,2) AS SLDT_NM, "
		//				   + "        RTRIM(X1.EMBSNAME) SAWON_NM, "
		//				   + "        RTRIM(X2.DEPRNAM1) DEPT_NM "
		//				   + "   FROM DUTY_TRSSVTM A "
		//				   + "   LEFT OUTER JOIN MSTEMBS X1 "
		//				   + "     ON A.SABN = X1.EMBSSABN "
		//				   + "   LEFT OUTER JOIN MSTDEPR X2 "
		//				   + "     ON X1.EMBSDPCD = X2.DEPRCODE "
		//				   + "  WHERE LEFT(A.SAVE_DATE,6) = '" + yymm + "'"
		//				   + "  ORDER BY A.SABN, A.SAVE_DATE" ;

		//		DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
		//		dp.AddDatatable2Dataset("S_DUTY_TRSSVTM", dt, ref ds);
		//	}
		//	catch (System.Exception ec)
		//	{
		//		System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
		//											 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}
		////SAVE시간 테이블
		//public void GetDUTY_TRSSVTMDatas(string embs, string sldt, DataSet ds)
		//{
		//	try
		//	{
		//		string qry = " SELECT * "
		//				   + "   FROM DUTY_TRSSVTM "
		//				   + "  WHERE SABN = '" + embs +"' AND SAVE_DATE = '" + sldt +"' ";

		//		DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
		//		dp.AddDatatable2Dataset("DUTY_TRSSVTM", dt, ref ds);
		//	}
		//	catch (System.Exception ec)
		//	{
		//		System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
		//											 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}
		////SAVE마감 조회
		//public void GetS_SAVE_ENDSDatas(string yymm, DataSet ds)
		//{
		//	try
		//	{
		//		string qry = " SELECT * "
		//				   + "   FROM DUTY_MSTSAVE_ENDS "
		//				   + "  WHERE END_YYMM = '" + yymm +"' ";

		//		DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
		//		dp.AddDatatable2Dataset("S_SAVE_ENDS", dt, ref ds);
		//	}
		//	catch (System.Exception ec)
		//	{
		//		System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
		//											 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}
		
		////SAVE 삭제 테이블 구조
		//public void GetDEL_TRSSVTMDatas(DataSet ds)
		//{
		//	try
		//	{
		//		string qry = " SELECT * "
		//				   + "   FROM DEL_TRSSVTM "
		//				   + "  WHERE 1=2";

		//		DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
		//		dp.AddDatatable2Dataset("DEL_TRSSVTM", dt, ref ds);
		//	}
		//	catch (System.Exception ec)
		//	{
		//		System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
		//											 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}
		
		////년월
		//public void GetSEARCH_SAVE_ENDSDatas(string year, DataSet ds)
		//{
		//	try
		//	{
		//		string qry = " SELECT A.*, "
		//				   + "        LEFT(A.END_YYMM,4)+'-'+SUBSTRING(A.END_YYMM,5,2) END_YYMM_NM, "
		//				   + "        (CASE WHEN A.CLOSE_YN='1' THEN END_DT+'('+END_ID+')' ELSE CANC_DT+'('+CANC_ID+')' END) MOD_DT, "
		//				   + "        (CASE A.CLOSE_YN WHEN '1' THEN '마감' WHEN '2' THEN '마감취소' ELSE '' END) CLOSE_NM "
		//				   + "   FROM DUTY_MSTSAVE_ENDS A "
		//				   + "  WHERE LEFT(A.END_YYMM,4) = '" + year +"' ";

		//		DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
		//		dp.AddDatatable2Dataset("SEARCH_SAVE_ENDS", dt, ref ds);
				
  //              for (int i = 1; i <= 12; i++)
  //              {
  //                  if (ds.Tables["SEARCH_SAVE_ENDS"].Select("END_YYMM = '" + year.Substring(0, 4) + i.ToString().PadLeft(2, '0') + "'").Length == 0)
  //                  {
  //                      DataRow drow = ds.Tables["SEARCH_SAVE_ENDS"].NewRow();
		//				drow["END_YYMM"] = year + i.ToString().PadLeft(2, '0');
		//				drow["END_YYMM_NM"] = year + "-" + i.ToString().PadLeft(2, '0');
  //                      drow["CLOSE_YN"] = "";
  //                      ds.Tables["SEARCH_SAVE_ENDS"].Rows.Add(drow);
  //                  }
  //              }
  //              ds.Tables["SEARCH_SAVE_ENDS"].DefaultView.Sort = "END_YYMM";
		//	}
		//	catch (System.Exception ec)
		//	{
		//		System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
		//											 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}

		//public void GetDUTY_MSTSAVE_ENDSDatas(string yymm, DataSet ds)
		//{
		//	try
		//	{
		//		string qry = " SELECT A.* "
		//				   + "   FROM DUTY_MSTSAVE_ENDS A "
		//				   + "  WHERE A.END_YYMM = '" + yymm +"' ";

		//		DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
		//		dp.AddDatatable2Dataset("DUTY_MSTSAVE_ENDS", dt, ref ds);
		//	}
		//	catch (System.Exception ec)
		//	{
		//		System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
		//											 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}

		
		////저장내역조회
		//public void GetS_DUTY_TRSOVTM_JSDatas(string yymm, DataSet ds)
		//{
		//	try
		//	{
		//		string qry = " SELECT A.*, "
		//				   + "        LEFT(A.JS_DATE,4)+'-'+SUBSTRING(A.JS_DATE,5,2)+'-'+SUBSTRING(A.JS_DATE,7,2) AS SLDT_NM, "
		//				   + "        RTRIM(X1.EMBSNAME) SAWON_NM, "
		//				   + "        RTRIM(X2.DEPRNAM1) DEPT_NM "
		//				   + "   FROM DUTY_TRSOVTM_JS A "
		//				   + "   LEFT OUTER JOIN MSTEMBS X1 "
		//				   + "     ON A.SABN = X1.EMBSSABN "
		//				   + "   LEFT OUTER JOIN MSTDEPR X2 "
		//				   + "     ON X1.EMBSDPCD = X2.DEPRCODE "
		//				   + "  WHERE LEFT(A.JS_DATE,6) = '" + yymm + "'"
		//				   + "  ORDER BY A.SABN, A.JS_DATE" ;

		//		DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
		//		dp.AddDatatable2Dataset("S_DUTY_TRSOVTM_JS", dt, ref ds);
		//	}
		//	catch (System.Exception ec)
		//	{
		//		System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
		//											 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}
		////SAVE시간 테이블
		//public void GetDUTY_TRSOVTM_JSDatas(string embs, string sldt, DataSet ds)
		//{
		//	try
		//	{
		//		string qry = " SELECT * "
		//				   + "   FROM DUTY_TRSOVTM_JS "
		//				   + "  WHERE SABN = '" + embs +"' AND JS_DATE = '" + sldt +"' ";

		//		DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
		//		dp.AddDatatable2Dataset("DUTY_TRSOVTM_JS", dt, ref ds);
		//	}
		//	catch (System.Exception ec)
		//	{
		//		System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
		//											 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}

		#endregion	


		#region 204X - CALL관리-사용안함

		public void Get2040_SEARCHDatas(string frdt, string todt, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        RTRIM(X1.PARTNAME) PARTNAME, RTRIM(X2.EMBSNAME) SAWON_NM "
						   + "   FROM DUTY_TRSCALL A "
						   + "   LEFT OUTER JOIN DUTY_MSTPART X1 "
						   + "     ON A.PARTCODE=X1.PARTCODE "
						   + "   LEFT OUTER JOIN DUTY_MSTNURS X2 "
						   + "     ON A.SAWON_NO=X2.SAWON_NO "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						   + "     ON A.SAWON_NO=X3.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						   + "     ON X3.EMBSDPCD=X4.DEPRCODE "
						   + "  WHERE A.CALL_DT BETWEEN '" + frdt + "' AND '" + todt + "' "
						   + "  ORDER BY A.REG_DT, A.SAWON_NO ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2040_SEARCH", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		public void Get2040_S_NURSDatas(string part, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, RTRIM(X1.PARTNAME) PARTNAME, '' CHK "
						   + "   FROM DUTY_MSTNURS A "
						   + "   LEFT OUTER JOIN DUTY_MSTPART X1 "
						   + "     ON A.PARTCODE=X1.PARTCODE "
						   + "  WHERE A.PARTCODE LIKE '" + part + "' "
						   + "  ORDER BY A.PARTCODE, A.SAWON_NO ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2040_S_NURS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//CALL신청 테이블 구조
		public void GetDUTY_TRSCALLDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_TRSCALL "
						   + "  WHERE 1=2 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSCALL", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//CALL신청_SQ
		public int GetSEARCH_CALL_SQDatas(string sabn, string sldt, DataSet ds)
		{
			int max_sq = 0;
			try
			{
				string qry = " SELECT ISNULL(MAX(CALL_SQ) + 1, 1) AS MAX_SQ "
						   + "   FROM DUTY_TRSCALL "
						   + "  WHERE SAWON_NO = '" + sabn +"' "
						   + "    AND CALL_DT = '" + sldt +"' ";
				
                object obj = gd.GetOneData(1, dbname, qry);
                max_sq = clib.TextToInt(obj.ToString());

			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return max_sq;
		}
		
		#endregion

		#region 2040 - CALL관리
		
		//부서코드 불러오기
		public void Get2040_CALLDEPTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT A.DEPRCODE CODE, RTRIM(DEPRNAM1) DEPT_NM, "
						   + "        (CASE WHEN X1.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) CHK "
						   + "   FROM " + wagedb + ".DBO.MSTDEPR A "
						   + "  INNER JOIN DUTY_INFOCALL X1 ON A.DEPRCODE=X1.DEPTCODE "
						   + "  ORDER BY A.DEPRCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2040_CALLDEPT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//CALL 근무유형 조회
		public void Get2040_CALL_GNMUDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTGNMU "
						   + "  WHERE G_TYPE=11";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2040_CALL_GNMU", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}		
		//CALL부서 직원lookup
		public void GetLOOK_CALL_EMBSDatas(string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.EMBSSABN CODE, RTRIM(A.EMBSNAME) NAME, "
						   + "        A.EMBSDPCD DEPTCODE, RTRIM(X1.DEPRNAM1) AS DEPT_NM "
						   + "   FROM " + wagedb + ".dbo.MSTEMBS A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
						   + "     ON A.EMBSDPCD = X1.DEPRCODE "
						   //+ "   INNER JOIN DUTY_INFOCALL X2 "
						   //+ "     ON A.DEPTCODE = X2.DEPTCODE "
						   + "  WHERE A.EMBSSTAT='1' AND A.EMBSDPCD='" + dept + "'"
						   + "  ORDER BY A.SAWON_NO ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("LOOK_CALL_EMBS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//CALL대기 등록내역조회
		public void GetDUTY_TRSCALLDatas(int gubn, string yymm, string dept, DataSet ds)
		{
			try
			{
				string bf_mm = clib.DateToText(clib.TextToDate(yymm + "01").AddMonths(-1));
				string qry = " SELECT A.*, RTRIM(X1.EMBSNAME) SAWON_NM, "
						   + "        '' D01_NM, '' D02_NM, '' D03_NM, '' D04_NM, '' D05_NM, '' D06_NM, '' D07_NM, '' D08_NM, '' D09_NM, '' D10_NM, "
						   + "        '' D11_NM, '' D12_NM, '' D13_NM, '' D14_NM, '' D15_NM, '' D16_NM, '' D17_NM, '' D18_NM, '' D19_NM, '' D20_NM, "
						   + "        '' D21_NM, '' D22_NM, '' D23_NM, '' D24_NM, '' D25_NM, '' D26_NM, '' D27_NM, '' D28_NM, '' D29_NM, '' D30_NM, '' D31_NM, "
						   + "        A.MM_CNT1 + ISNULL(X3.Y_CNT1,0) AS Y_CNT1, A.MM_CNT2 + ISNULL(X3.Y_CNT2,0) AS Y_CNT2, "
						   + "        ISNULL(X3.Y_CNT1,0) AS YEAR_CNT1, ISNULL(X3.Y_CNT2,0) AS YEAR_CNT2 "
						   + "   FROM DUTY_TRSCALL A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X1 "
						   + "     ON A.SAWON_NO = X1.EMBSSABN "
						   + "   LEFT OUTER JOIN (SELECT SAWON_NO, "
						   + "                           SUM(CASE WHEN '" + yymm.Substring(4, 2) + "' = '01' THEN 0 ELSE MM_CNT1 END) Y_CNT1, "
						   + "                           SUM(CASE WHEN '" + yymm.Substring(4, 2) + "' = '01' THEN 0 ELSE MM_CNT2 END) Y_CNT2 "
						   + "		                FROM DUTY_TRSCALL "
						   + "                     WHERE DEPTCODE = '" + dept + "' "
						   + "                       AND PLANYYMM BETWEEN '" + yymm.Substring(0, 4) + "01' AND '" + bf_mm.Substring(0, 6) + "' "
						   + "				       GROUP BY SAWON_NO) X3 "
						   + "     ON A.SAWON_NO=X3.SAWON_NO "
						   + "  WHERE A.PLANYYMM = '" + yymm + "' AND A.DEPTCODE = '" + dept + "' "
						   + "  ORDER BY A.PLAN_SQ ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				if (gubn == 1)
					dp.AddDatatable2Dataset("DUTY_TRSCALL", dt, ref ds);
				else
					dp.AddDatatable2Dataset("SEARCH_CALL_PLAN", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//CALL대기조회
		public void GetSEARCH_CALL_PLANDatas(string yymm, string dept, DataSet ds)
		{
			try
			{
				string qry = " EXEC USP_DUTY2040_PRC_211203 '" + yymm + "', '" + dept + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_CALL_PLAN", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//Sum_CALL대기
		public void GetSUM_CALL_PLANDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT 0 AS G_TYPE, '' G_NM, * FROM DUTY_TRSCALL WHERE 1 = 2 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SUM_CALL_PLAN", dt, ref ds);
				
				DataRow nrow = ds.Tables["SUM_CALL_PLAN"].NewRow();
				nrow["G_TYPE"] = "11";
				nrow["G_NM"] = "C";
				ds.Tables["SUM_CALL_PLAN"].Rows.Add(nrow);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//전월CALL조회
		public void GetDUTY_BF_TRSCALLDatas(string bf_yymm, string dept, DataSet ds)
		{
			try
			{
				string bf_mm = clib.DateToText(clib.TextToDate(bf_yymm + "01").AddMonths(-1));
				string qry = " SELECT A.*, RTRIM(X1.EMBSNAME) SAWON_NM, "
						   + "        '' D01_NM, '' D02_NM, '' D03_NM, '' D04_NM, '' D05_NM, '' D06_NM, '' D07_NM, '' D08_NM, '' D09_NM, '' D10_NM, "
						   + "        '' D11_NM, '' D12_NM, '' D13_NM, '' D14_NM, '' D15_NM, '' D16_NM, '' D17_NM, '' D18_NM, '' D19_NM, '' D20_NM, "
						   + "        '' D21_NM, '' D22_NM, '' D23_NM, '' D24_NM, '' D25_NM, '' D26_NM, '' D27_NM, '' D28_NM, '' D29_NM, '' D30_NM, '' D31_NM "
						   + "   FROM DUTY_TRSCALL A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X1 "
						   + "     ON A.SAWON_NO = X1.EMBSSABN "
						   + "  WHERE A.PLANYYMM = '" + bf_yymm + "' AND A.DEPTCODE = '" + dept + "' "
						   + "  ORDER BY A.PLAN_SQ ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_BF_CALL", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//휴일체크
		public void GetCHK_HOLIDatas(string date, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTHOLI "
						   + "  WHERE H_DATE='"+ date  +"'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("CHK_HOLI", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}	


		//CALL 조회->사용안함
		public void GetSEARCH_CALLDatas(string yymm, string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, "
						   + "        LEFT(A.CALL_DATE,4)+'-'+SUBSTRING(A.CALL_DATE,5,2)+'-'+SUBSTRING(A.CALL_DATE,7,2) AS SLDT_NM, "
						   + "        RTRIM(X1.EMBSNAME) SAWON_NM, X2.G_FNM, RTRIM(X3.DEPRNAM1) AS DEPT_NM, "
						   + "        CONVERT(DATETIME,A.CALL_DATE) FR_DATE, DATEADD(DAY,1,A.CALL_DATE) TO_DATE, "
						   + "        0 AS TYPE, 1 AS ALLDAY, 3 AS LABEL, '' REMARK "
						   + "   FROM DUTY_TRSCALL A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X1 "
						   + "     ON A.SABN=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN DUTY_MSTGNMU X2 "
						   + "     ON A.CALL_TYPE=X2.G_CODE "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X3 "
						   + "     ON X1.EMBSDPCD=X3.DEPRCODE "
						   + "  WHERE LEFT(A.CALL_DATE,6) = '" + yymm + "' AND X1.EMBSDPCD LIKE '" + dept + "'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_CALL", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//CALL 조회->사용안함
		public void GetDUTY_TRSCALLDatas(string sabn, string sldt, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM DUTY_TRSCALL A "
						   + "  WHERE A.SABN = '" + sabn + "'"
						   + "    AND A.CALL_DATE = '" + sldt + "'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSCALL", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
				
		#region 2041 - CALL사용부서설정
		
		//부서코드 불러오기
		public void GetSEARCH_CALLDEPTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT A.DEPRCODE CODE, RTRIM(DEPRNAM1) DEPT_NM, "
						   + "        (CASE WHEN X1.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) CHK "
						   + "   FROM " + wagedb + ".DBO.MSTDEPR A "
						   + "   LEFT OUTER JOIN DUTY_INFOCALL X1 ON A.DEPRCODE=X1.DEPTCODE "
						   + "  WHERE A.DEPRSTAT=1 "
						   + "  ORDER BY A.DEPRCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_CALLDEPT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//삭제할 부서설정 테이블 불러오기
		public void GetD_DUTY_INFOCALLDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOCALL A ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("D_DUTY_INFOCALL", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//등록할 부서설정 테이블 불러오기
		public void GetDUTY_INFOCALLDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOCALL A ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_INFOCALL", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region 2060 - 당직관리

		//결재상신 체크
		public void Get2060_GW_CHKDatas(string gubn, string yymm, string dept, DataSet ds)
		{
			try
			{
				string tb_nm = gubn == "5" ? "GW_TRSDANG" : "GW_TRSPLAN";
				string qry = " SELECT A.* "
						   + "   FROM (SELECT DISTINCT DOC_NO FROM " + tb_nm +" WHERE PLANYYMM = '" + yymm + "' AND DEPTCODE = '" + dept + "') A "
						   + "   INNER JOIN DUTY_GWDOC X1 ON A.DOC_NO=X1.DOC_NO AND AP_TAG NOT IN ('2','5') AND ISNULL(X1.ETC_GUBN,'')<>'1' ";
				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2060_GW_CHK", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//부서코드 불러오기
		public void Get2060_DANGDEPTDatas(string yymm, DataSet ds)
		{
			try
			{
				string qry =  " SELECT A.DEPRCODE CODE, A.DEPT_NM,  '' AS GW_STAT, "
						   + "        '1' CHK, "
						   + "        (CASE WHEN X2.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) DATA_CHK "
						   + "   FROM (SELECT 'A002' DEPRCODE, '팀장' DEPT_NM ) A "
						   + "   LEFT OUTER JOIN (SELECT DISTINCT 'A002' AS DEPTCODE FROM DUTY_TRSDANG "
						   + "                     WHERE SAWON_NO IN (SELECT EMBSSABN FROM MSTEMBS WHERE EMBSADGB='1') AND PLANYYMM='" + yymm + "' ) X2 "
						   + "     ON A.DEPRCODE=X2.DEPTCODE "
						   + "UNION ALL ";

				qry += " SELECT A.DEPRCODE CODE, RTRIM(DEPRNAM1) DEPT_NM,  '' AS GW_STAT, "
					+ "        (CASE WHEN X1.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) CHK, "
					+ "        (CASE WHEN X2.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) DATA_CHK "
					+ "   FROM " + wagedb + ".DBO.MSTDEPR A "
					+ "  INNER JOIN DUTY_INFODANG X1 ON A.DEPRCODE=X1.DEPTCODE "
					+ "   LEFT OUTER JOIN (SELECT DISTINCT DEPTCODE FROM DUTY_TRSDANG WHERE PLANYYMM='" + yymm +"' ) X2 "
					+ "     ON A.DEPRCODE=X2.DEPTCODE "
					+ "  WHERE A.DEPRSTAT=1 "
					+ "  ORDER BY A.DEPRCODE ";

				//qry = " SELECT A.DEPRCODE CODE, RTRIM(DEPRNAM1) DEPT_NM, '' AS GW_STAT, "
				//	+ "        (CASE WHEN X1.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) CHK "
				//	+ "   FROM " + wagedb + ".DBO.MSTDEPR A "
				//	+ "  INNER JOIN DUTY_INFODANG X1 ON A.DEPRCODE=X1.DEPTCODE "
				//	+ "  WHERE A.DEPRSTAT=1 "
				//	+ "  ORDER BY A.DEPRCODE ";

				//if (yymm != "")
				//{
				//	qry += " SELECT A.DEPRCODE CODE, RTRIM(DEPRNAM1) DEPT_NM, '' AS GW_STAT,  "
				//		+ "        (CASE WHEN X1.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) CHK, "
				//		+ "        (CASE WHEN X2.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) DATA_CHK "
				//		+ "   FROM " + wagedb + ".DBO.MSTDEPR A "
				//		+ "  INNER JOIN DUTY_INFODANG X1 ON A.DEPRCODE=X1.DEPTCODE "
				//		+ "   LEFT OUTER JOIN (SELECT DISTINCT DEPTCODE FROM DUTY_TRSDANG WHERE PLANYYMM='" + yymm +"' ) X2 "
				//		+ "     ON A.DEPRCODE=X2.DEPTCODE "
				//		+ "  WHERE A.DEPRSTAT=1 "
				//		+ "  ORDER BY A.DEPRCODE ";
				//}

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2060_DANGDEPT", dt, ref ds);

				qry = " SELECT A.DEPTCODE, X1.* "
					+ "   FROM ("
					+ "         SELECT DISTINCT DOC_NO, PLANYYMM, DEPTCODE FROM GW_TRSDANG WHERE PLANYYMM='" + yymm + "' ) A"
					+ "  INNER JOIN DUTY_GWDOC X1 ON A.DOC_NO=X1.DOC_NO AND X1.AP_TAG NOT IN ('2','5') AND X1.DOC_GUBN='5' AND ISNULL(X1.ETC_GUBN,'')<>'1' ";
				dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2060_GWDANG", dt, ref ds);

				for (int i = 0; i < ds.Tables["2060_GWDANG"].Rows.Count; i++)
				{
					DataRow grow = ds.Tables["2060_GWDANG"].Rows[i];
					if (ds.Tables["2060_DANGDEPT"].Select("CODE ='" + grow["DEPTCODE"].ToString() + "'").Length > 0)
					{
						DataRow drow = ds.Tables["2060_DANGDEPT"].Select("CODE ='" + grow["DEPTCODE"].ToString() + "'")[0];
						string stat_nm = "";
						switch (grow["AP_TAG"].ToString())
						{
							case "1":
								stat_nm = "승인";
								break;
							case "3":
								stat_nm = "완료";
								break;
							case "4":
								stat_nm = "진행";
								break;
						}
						drow["GW_STAT"] = stat_nm;
					}
				}
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//당직 근무유형 조회
		public void Get2060_DANG_GNMUDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, "
						   + "        (CASE WHEN A.G_TYPE IN (2,3,4,5,6,7) THEN '2.근무' "
						   + "			    WHEN A.G_TYPE=8 THEN '3.연차' WHEN A.G_TYPE IN (9,10) THEN '1.당직' "
						   + "			    WHEN A.G_TYPE=12 THEN '4.휴가' "
						   + "              ELSE '' END) G_TYPE_NM, "
						   + "        (CASE WHEN A.G_FRTM<>'' AND A.G_TOTM<>'' "
						   + "              THEN A.G_SNM+' ('+LEFT(A.G_FRTM,2)+':'+SUBSTRING(A.G_FRTM,3,2)+'-'+LEFT(A.G_TOTM,2)+':'+SUBSTRING(A.G_TOTM,3,2)+')' ELSE A.G_SNM END) G_SNM2 "
						   + "   FROM DUTY_MSTGNMU A "
						   + "  ORDER BY G_TYPE_NM, A.G_CODE "; // A.G_TYPE ";
						   //+ "  WHERE G_TYPE IN (9,10)";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2060_DANG_GNMU", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}		
		//출근내역 조회
		public void Get2060_SEARCH_KTDatas(string yymm, string dept, DataSet ds)
		{
			try
			{
				string where1 = dept == "A001" ? "SAWON_NO IN (SELECT SAWON_NO FROM DUTY_TRSPLAN_ETC WHERE DEPTCODE = '" + dept + "')" : "DEPTCODE = '" + dept + "'";
				string where2 = dept == "A001" ? "EMBSSABN IN (SELECT SAWON_NO FROM DUTY_TRSPLAN_ETC WHERE DEPTCODE = '" + dept + "')" : "EMBSDPCD = '" + dept + "'";

				if (dept == "A002")
				{
					where1 = "SAWON_NO IN (SELECT EMBSSABN FROM MSTEMBS WHERE EMBSADGB = '1')";
					where2 = "EMBSADGB = '1'";
				}

				string qry = "  IF EXISTS(SELECT SAWON_NO FROM DUTY_TRSDANG WHERE PLANYYMM='" + yymm + "' AND DEPTCODE = '" + dept + "' ) "
						   + "  BEGIN "
						   + "      SELECT RIGHT(A.USERID,5) as SABN, X1.USERNAME, CONVERT(VARCHAR,WORKSTART,112) SLDT "
						   + "        FROM TB_WORKRESULT A LEFT OUTER JOIN TB_USER X1 ON A.USERID=X1.USERID "
						   + "       WHERE RIGHT(A.USERID,5) in (SELECT SAWON_NO FROM DUTY_TRSDANG WHERE PLANYYMM='" + yymm + "' AND " + where1 +") " //DEPTCODE = '" + dept + "' )"
						   + "         AND CONVERT(VARCHAR,A.WORKSTART,112) LIKE '" + yymm + "%' AND A.WORKSTART IS NOT NULL "
						   + "       ORDER BY RIGHT(A.USERID,5), A.WORKSTART "
						   + "  END "
						   + "  ELSE "
						   + "  BEGIN"
						   + "      SELECT RIGHT(A.USERID,5) as SABN, X1.USERNAME, CONVERT(VARCHAR,WORKSTART,112) SLDT "
						   + "        FROM TB_WORKRESULT A LEFT OUTER JOIN TB_USER X1 ON A.USERID=X1.USERID "
						   + "       WHERE RIGHT(A.USERID,5) in (SELECT EMBSSABN FROM MSTEMBS WHERE EMBSSTAT='1' AND " + where2 +") " // EMBSDPCD = '" + dept + "' )"
						   + "         AND CONVERT(VARCHAR,A.WORKSTART,112) LIKE '" + yymm + "%' AND A.WORKSTART IS NOT NULL "
						   + "       ORDER BY RIGHT(A.USERID,5), A.WORKSTART "
						   + "  END ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2060_SEARCH_KT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		public void GetSEARCH_DYYCDatas(string sldt, string sabn, DataSet ds)
		{
			try
			{
				string qry = " SELECT A1.*, '" + sldt + "' AS YC_STDT, "
						   + "        (SELECT ISNULL(MAX(YC_SQ),0) YC_SQ FROM DUTY_MSTYCCJ "
						   + "          WHERE YC_YEAR=A1.YC_YEAR AND SAWON_NO=A1.SAWON_NO) AS YCCJ_SQ"
						   + "   FROM ( "
						   + "         SELECT A.YC_YEAR, A.SAWON_NO SAWON_NO, RTRIM(A.SAWON_NM) SAWON_NM, RTRIM(X3.EMBSEMAL) GW_EMAIL, RTRIM(ISNULL(X4.DEPRNAM1,'')) DEPT_NM, "
						   + "                A.YC_TYPE, A.IN_DATE, A.CALC_FRDT, A.CALC_TODT, A.USE_FRDT, A.USE_TODT,"
						   + "                A.YC_BASE, A.YC_CHANGE, A.YC_FIRST, A.YC_ADD, A.YC_TOTAL, "
						   + "				  (CASE WHEN A.YC_TYPE=0 THEN '회계년도기준' ELSE '입사일기준' END) YC_TYPE_NM, "
						   + "      		  LEFT(A.IN_DATE,4)+'-'+SUBSTRING(A.IN_DATE,5,2)+'-'+SUBSTRING(A.IN_DATE,7,2) AS IN_DATE_NM, "
						   + "                LEFT(A.CALC_FRDT,4)+'-'+SUBSTRING(A.CALC_FRDT,5,2)+'-'+SUBSTRING(A.CALC_FRDT,7,2)+' ~ '+ "
						   + "                LEFT(A.CALC_TODT,4)+'-'+SUBSTRING(A.CALC_TODT,5,2)+'-'+SUBSTRING(A.CALC_TODT,7,2) AS CALC_DT_NM, "
						   + "                LEFT(A.USE_FRDT,4)+'-'+SUBSTRING(A.USE_FRDT,5,2)+'-'+SUBSTRING(A.USE_FRDT,7,2) AS USE_FRDT_NM, "
						   + "                LEFT(A.USE_TODT,4)+'-'+SUBSTRING(A.USE_TODT,5,2)+'-'+SUBSTRING(A.USE_TODT,7,2) AS USE_TODT_NM, "
						   + "                A.YC_BASE+A.YC_FIRST+A.YC_ADD as YC_SUM,"
						   + "                SUM(ISNULL(X1.YC_DAYS,0)) AS YC_USE, "
						   + "                A.YC_TOTAL - SUM(ISNULL(X1.YC_DAYS,0)) AS YC_REMAIN "
						   + "           FROM DUTY_TRSDYYC A "
						   + "           LEFT OUTER JOIN DUTY_TRSHREQ X1 "
						   + "             ON A.YC_YEAR=X1.REQ_YEAR AND A.SAWON_NO=X1.SABN AND X1.AP_TAG NOT IN ('5') "  //반려는 카운트 제외 23.01.13
						   + "           LEFT OUTER JOIN MSTEMBS X3 "
						   + "             ON A.SAWON_NO=X3.EMBSSABN "
						   + "			 LEFT OUTER JOIN MSTDEPR X4 "
						   + "			   ON X3.EMBSDPCD=X4.DEPRCODE "
						   + "          WHERE ((A.YC_TYPE=0 AND A.YC_YEAR='" + sldt.Substring(0, 4) + "' ) "
						   + "                OR (A.YC_TYPE IN (1,3) AND A.USE_FRDT<='" + sldt + "' AND A.USE_TODT>='" + sldt + "')) "
						   + "            AND A.SAWON_NO='" + sabn + "'"
						   + "          GROUP BY A.YC_YEAR, A.SAWON_NO, A.SAWON_NM, X3.EMBSEMAL, X4.DEPRNAM1, A.YC_TYPE, A.IN_DATE, A.CALC_FRDT, A.CALC_TODT, A.USE_FRDT, A.USE_TODT,"
						   + "                   A.YC_BASE, A.YC_CHANGE, A.YC_FIRST, A.YC_ADD, A.YC_TOTAL ) A1 "
						   + "  ORDER BY A1.YC_YEAR ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_DYYC", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//당직부서 직원lookup //string yymm, string dept, 
		public void GetLOOK_DANG_EMBSDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT A.EMBSSABN CODE, RTRIM(A.EMBSNAME) NAME, "
						   + "        A.EMBSDPCD, RTRIM(X1.DEPRNAM1) AS DEPT_NM "
						   + "   FROM " + wagedb + ".dbo.MSTEMBS A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
						   + "     ON A.EMBSDPCD = X1.DEPRCODE "
						   + "   INNER JOIN DUTY_INFODANG X2 "
						   + "     ON A.EMBSDPCD = X2.DEPTCODE "
						   //+ "  WHERE (A.WORKSTAT=1 OR (A.WORKSTAT=2 AND LEFT(A.OUT_DATE,6)>='" + yymm + "')) "
						   //+ "    AND A.DEPTCODE = '" + dept +"'"
						   + "  ORDER BY A.EMBSSABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("LOOK_DANG_EMBS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//당직 등록내역조회
		public void GetDUTY_TRSDANGDatas(int gubn, string yymm, string dept, DataSet ds)
		{
			try
			{
				string where = "";
				if (dept == "A002")  //팀장
					where = "SAWON_NO IN (SELECT EMBSSABN FROM MSTEMBS WHERE EMBSADGB='1') ";
				else
					where = "DEPTCODE = '" + dept + "'";

				string bf_mm = clib.DateToText(clib.TextToDate(yymm + "01").AddMonths(-1));
				string qry = " SELECT '1' CHK, A.*, RTRIM(X1.EMBSNAME) SAWON_NM, "
						   + "        '' D01_NM, '' D02_NM, '' D03_NM, '' D04_NM, '' D05_NM, '' D06_NM, '' D07_NM, '' D08_NM, '' D09_NM, '' D10_NM, "
						   + "        '' D11_NM, '' D12_NM, '' D13_NM, '' D14_NM, '' D15_NM, '' D16_NM, '' D17_NM, '' D18_NM, '' D19_NM, '' D20_NM, "
						   + "        '' D21_NM, '' D22_NM, '' D23_NM, '' D24_NM, '' D25_NM, '' D26_NM, '' D27_NM, '' D28_NM, '' D29_NM, '' D30_NM, '' D31_NM "
						   + "   FROM DUTY_TRSDANG A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X1 "
						   + "     ON A.SAWON_NO = X1.EMBSSABN "
						   + "  WHERE A.PLANYYMM = '" + yymm + "' AND A." + where;
				if (gubn == 3)
					qry += "   AND A.SAWON_NO NOT IN (SELECT G1.SAWON_NO FROM GW_TRSDANG G1 "
						 + "                           INNER JOIN DUTY_GWDOC G2 ON G1.DOC_NO=G2.DOC_NO AND ISNULL(G2.AP_TAG,'') IN ('1','3','4') "
						 + "                           WHERE G1.PLANYYMM = '" + yymm + "' AND G1." + where + ")";
				qry += "  ORDER BY A.PLAN_SQ ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				if (gubn == 1)
					dp.AddDatatable2Dataset("DUTY_TRSDANG", dt, ref ds);
				else if (gubn == 2)
					dp.AddDatatable2Dataset("SEARCH_DANG_PLAN", dt, ref ds);
				else if (gubn == 3)
					dp.AddDatatable2Dataset("GW_SEARCH_DANG_PLAN", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//DANG조회
		public void GetSEARCH_DANG_PLANDatas(string yymm, string dept, DataSet ds)
		{
			try
			{
				string qry = " EXEC USP_DUTY2060_PRC_211203 '" + yymm + "', '" + dept + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_DANG_PLAN", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//Sum_당직
		public void GetSUM_DANG_PLANDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT 0 AS G_TYPE, '' G_NM, * FROM DUTY_TRSDANG WHERE 1 = 2 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SUM_DANG_PLAN", dt, ref ds);
				
				DataRow nrow = ds.Tables["SUM_DANG_PLAN"].NewRow();
				nrow["G_TYPE"] = "9";
				nrow["G_NM"] = "당직";
				ds.Tables["SUM_DANG_PLAN"].Rows.Add(nrow);
				nrow = ds.Tables["SUM_DANG_PLAN"].NewRow();
				nrow["G_TYPE"] = "10";
				nrow["G_NM"] = "낮당직";
				ds.Tables["SUM_DANG_PLAN"].Rows.Add(nrow);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//전월당직조회
		public void GetDUTY_BF_TRSDANGDatas(string bf_yymm, string dept, DataSet ds)
		{
			try
			{
				string bf_mm = clib.DateToText(clib.TextToDate(bf_yymm + "01").AddMonths(-1));
				string qry = " SELECT A.*, RTRIM(X1.EMBSNAME) SAWON_NM, "
						   + "        '' D01_NM, '' D02_NM, '' D03_NM, '' D04_NM, '' D05_NM, '' D06_NM, '' D07_NM, '' D08_NM, '' D09_NM, '' D10_NM, "
						   + "        '' D11_NM, '' D12_NM, '' D13_NM, '' D14_NM, '' D15_NM, '' D16_NM, '' D17_NM, '' D18_NM, '' D19_NM, '' D20_NM, "
						   + "        '' D21_NM, '' D22_NM, '' D23_NM, '' D24_NM, '' D25_NM, '' D26_NM, '' D27_NM, '' D28_NM, '' D29_NM, '' D30_NM, '' D31_NM "
						   + "   FROM DUTY_TRSDANG A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X1 "
						   + "     ON A.SAWON_NO = X1.EMBSSABN "
						   + "  WHERE A.PLANYYMM = '" + bf_yymm + "' AND A.DEPTCODE = '" + dept + "' "
						   + "  ORDER BY A.PLAN_SQ ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_BF_DANG", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//당직 캘린더 조회
		public void GetSEARCH_DANGDatas(string yymm, string dept, DataSet ds)
		{
			try
			{
				string qry = " EXEC USP_DUTY2060_CALENDER '" + yymm + "', '" + dept + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_DANG", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//고정ot 사용부서 체크
		public void GetSEARCH_DUTY_INFOFXOTDatas(string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOFXOT "
						   + "  WHERE DEPTCODE LIKE '" + dept + "'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_DUTY_INFOFXOT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//당직숙직 조회->사용안함
		public void GetSEARCH_DREQDatas(string yymm, string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, "
						   + "        LEFT(A.REQ_DATE,4)+'-'+SUBSTRING(A.REQ_DATE,5,2)+'-'+SUBSTRING(A.REQ_DATE,7,2) AS SLDT_NM, "
						   + "        RTRIM(X1.EMBSNAME) SAWON_NM, X2.G_FNM, RTRIM(X3.DEPRNAM1) AS DEPT_NM, "
						   + "        CONVERT(DATETIME,REQ_DATE) FR_DATE, DATEADD(DAY,1,REQ_DATE) TO_DATE, "
						   + "        0 AS TYPE, 1 AS ALLDAY, 3 AS LABEL, '' REMARK "
						   + "   FROM DUTY_TRSDREQ A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X1 "
						   + "     ON A.SABN=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN DUTY_MSTGNMU X2 "
						   + "     ON A.REQ_TYPE=X2.G_CODE "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X3 "
						   + "     ON X1.EMBSDPCD=X3.DEPRCODE "
						   + "  WHERE LEFT(A.REQ_DATE,6) = '" + yymm + "' AND X1.DEPTCODE LIKE '" + dept + "'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_DREQ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//당직숙직 조회->사용안함
		public void GetDUTY_TRSDREQDatas(string sabn, string sldt, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM DUTY_TRSDREQ A "
						   + "  WHERE A.SABN = '" + sabn + "'"
						   + "    AND A.REQ_DATE = '" + sldt + "'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSDREQ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
				
		#region 2061 - 당직사용부서설정

		//부서코드 불러오기
		public void GetSEARCH_DANGDEPTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT (CASE WHEN X1.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) CHK, "
						   + "        RTRIM(DEPRNAM1) DEPT_NM, A.*  "
						   + "   FROM " + wagedb + ".DBO.MSTDEPR A "
						   + "   LEFT OUTER JOIN DUTY_INFODANG X1 ON A.DEPRCODE=X1.DEPTCODE "
						   + "  WHERE A.DEPRSTAT=1 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_DANGDEPT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//삭제할 부서설정 테이블 불러오기
		public void GetD_DUTY_INFODANGDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFODANG A ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("D_DUTY_INFODANG", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//등록할 부서설정 테이블 불러오기
		public void GetDUTY_INFODANGDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFODANG A ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_INFODANG", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
						
		#region 2062 - 당직근무표 결재
		
		//GW_TRSPLAN 테이블 불러오기
		public void GetGW_TRSDANGDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM GW_TRSDANG WHERE 1=2 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("GW_TRSDANG", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region 2010 - OFF신청조회
				
		//부서코드 lookup
		public void Get2010_SEARCH_DEPTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT A.DEPRCODE CODE, RTRIM(A.DEPRNAM1) NAME "
						   + "   FROM MSTDEPR A "
						   + "  INNER JOIN DUTY_INFONURS X1 "
						   + "     ON A.DEPRCODE = X1.DEPTCODE"
						   + "  WHERE A.DEPRSTAT=1 "
						   + "  ORDER BY A.DEPRCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2010_SEARCH_DEPT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//OFF신청조회
		public void GetSEARCH_OREQDatas(string yymm, string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, X2.EMBSDPCD, "
						   + "        LEFT(A.REQ_DATE,4)+'-'+SUBSTRING(A.REQ_DATE,5,2)+'-'+SUBSTRING(A.REQ_DATE,7,2) AS REQ_DATE_NM, "
						   + "        RTRIM(X2.EMBSNAME) SAWON_NM, X1.G_FNM, X1.G_SNM, RTRIM(X3.DEPRNAM1) DEPT_NM, "
						   + "        (CASE WHEN A.EDIT_YN='1' THEN 'Y' ELSE '' END) AS EDIT_YN_NM, "
						   + "        CONVERT(DATETIME,A.REQ_DATE) FR_DATE, DATEADD(DAY,1,A.REQ_DATE) TO_DATE, "
						   + "        0 AS TYPE, 1 AS ALLDAY, (CASE WHEN A.EDIT_YN=1 THEN 1 ELSE 3 END) AS LABEL, '' REMARK "
						   + "   FROM DUTY_TRSOREQ A "
						   + "   LEFT OUTER JOIN DUTY_MSTGNMU X1 "
						   + "     ON A.REQ_TYPE=X1.G_CODE "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X2 "
						   + "     ON A.SABN=X2.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X3 "
						   + "     ON X2.EMBSDPCD=X3.DEPRCODE "
						   + "  WHERE LEFT(A.REQ_DATE,6) = '" + yymm + "'"
						   + "    AND X2.EMBSDPCD LIKE '" + dept + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSOREQ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		#endregion

		#region 3010 - 근무신청조회

		//부서장의 관리부서 여부 체크
		public void GetCHK_DEPTDatas(string sabn, string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, RTRIM(X1.DEPRNAM1) DEPT_NM "
						   + "   FROM DUTY_PWERDEPT A "
						   + "   LEFT OUTER JOIN MSTDEPR X1 ON A.DEPT=X1.DEPRCODE "
						   + "  WHERE A.SABN = '" + sabn + "' AND A.DEPT LIKE '" + dept + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("CHK_DEPT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//부서코드 불러오기
		public void Get3010_DANGDEPTDatas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.DEPRCODE CODE, A.DEPT_NM,  '' AS GW_STAT, "
						   + "        '1' CHK, "
						   + "        (CASE WHEN X2.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) DATA_CHK "
						   + "   FROM (SELECT 'A001' DEPRCODE, '베드이송' DEPT_NM ) A "
						   + "   LEFT OUTER JOIN (SELECT DISTINCT 'A001' AS DEPTCODE FROM DUTY_TRSPLAN "
						   + "                     WHERE SAWON_NO IN ('21257','21407') AND PLANYYMM='" + yymm + "' ) X2 "
						   + "     ON A.DEPRCODE=X2.DEPTCODE "
						   + "UNION ALL "
						   + " SELECT A.DEPRCODE CODE, A.DEPT_NM,  '' AS GW_STAT, "
						   + "        '1' CHK, "
						   + "        (CASE WHEN X2.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) DATA_CHK "
						   + "   FROM (SELECT 'A002' DEPRCODE, '팀장' DEPT_NM ) A "
						   + "   LEFT OUTER JOIN (SELECT DISTINCT 'A002' AS DEPTCODE FROM DUTY_TRSPLAN "
						   + "                     WHERE SAWON_NO IN (SELECT EMBSSABN FROM MSTEMBS WHERE EMBSADGB='1') AND PLANYYMM='" + yymm + "' ) X2 "
						   + "     ON A.DEPRCODE=X2.DEPTCODE "
						   + "UNION ALL ";

				qry += " SELECT A.DEPRCODE CODE, RTRIM(DEPRNAM1) DEPT_NM,  '' AS GW_STAT, "
					+ "        (CASE WHEN X1.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) CHK, "
					+ "        (CASE WHEN X2.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) DATA_CHK "
					+ "   FROM " + wagedb + ".DBO.MSTDEPR A "
					+ "  INNER JOIN DUTY_INFONURS X1 ON A.DEPRCODE=X1.DEPTCODE "
					+ "   LEFT OUTER JOIN (SELECT DISTINCT DEPTCODE FROM DUTY_TRSPLAN WHERE PLANYYMM='" + yymm +"' ) X2 "
					+ "     ON A.DEPRCODE=X2.DEPTCODE "
					+ "  WHERE A.DEPRSTAT=1 "
					+ "  ORDER BY A.DEPRCODE ";				

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3010_DANGDEPT", dt, ref ds);				

				qry = " SELECT A.DEPTCODE, X1.* "
					+ "   FROM ("
					+ "         SELECT DISTINCT DOC_NO, PLANYYMM, DEPTCODE FROM GW_TRSPLAN WHERE PLANYYMM='" + yymm + "' ) A"
					+ "  INNER JOIN DUTY_GWDOC X1 ON A.DOC_NO=X1.DOC_NO AND X1.AP_TAG NOT IN ('2','5') AND X1.DOC_GUBN='6' AND ISNULL(X1.ETC_GUBN,'')<>'1'";
				dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2060_GWDANG", dt, ref ds);

				for (int i = 0; i < ds.Tables["2060_GWDANG"].Rows.Count; i++)
				{
					DataRow grow = ds.Tables["2060_GWDANG"].Rows[i];
					if (ds.Tables["3010_DANGDEPT"].Select("CODE ='" + grow["DEPTCODE"].ToString() + "'").Length > 0)
					{
						DataRow drow = ds.Tables["3010_DANGDEPT"].Select("CODE ='" + grow["DEPTCODE"].ToString() + "'")[0];
						string stat_nm = "";
						switch (grow["AP_TAG"].ToString())
						{
							case "1":
								stat_nm = "승인";
								break;
							case "3":
								stat_nm = "완료";
								break;
							case "4":
								stat_nm = "진행";
								break;
						}
						drow["GW_STAT"] = stat_nm;
					}
				}
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//조회년월 휴가연차 가져오기
		public void Get3010_SEARCH_HYDatas(string yymm, string dept, DataSet ds)
		{
			try
			{
				string qry = " EXEC USP_DUTY3010_HY_220419 '" + yymm + "', '" + dept + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3010_SEARCH_HY", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//조회년월 퇴사자 가져오기
		public void Get3010_S_TSDatas(string yymm, string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT EMBSSABN, "
						   + "        (CASE WHEN LEFT(EMBSTSDT,6)='" + yymm + "' THEN SUBSTRING(EMBSTSDT,7,2) "
						   + "              WHEN EMBSSTAT='2' THEN '99' ELSE '' END) AS TSDT "
						   + "   FROM MSTEMBS "
						   + "  WHERE EMBSDPCD='" + dept + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3010_S_TS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//휴일조회
		public void GetSEARCH_HOLIDatas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTHOLI "
						   + "  WHERE LEFT(H_DATE,6) = '" + yymm + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_HOLI", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//전월휴일조회
		public void GetSEARCH_BF_HOLIDatas(string bf_yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTHOLI "
						   + "  WHERE LEFT(H_DATE,6) = '" + bf_yymm + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_BF_HOLI", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//근무LIST
		public void GetGNMU_LISTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT (CASE WHEN A.G_TYPE IN (2,3,4,5,6,7) THEN '1.근무' "
						   + "			    WHEN A.G_TYPE=8 THEN '2.연차' WHEN A.G_TYPE IN (9,10) THEN '4.당직' "
						   + "			    WHEN A.G_TYPE=12 THEN '3.휴가' "
						   + "              ELSE '' END) G_TYPE_NM, "
						   + "        A.G_CODE, A.G_FNM, A.G_SNM "
						   + "   FROM DUTY_MSTGNMU A "
						   + "  ORDER BY G_TYPE_NM, A.G_CODE "; // A.G_TYPE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("GNMU_LIST", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//3교대 근무유형 조회
		public void Get3010_GNMUDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, "
						   + "        (CASE WHEN A.G_TYPE IN (2,3,4,5,6,7) THEN '1.근무' "
						   + "			    WHEN A.G_TYPE=8 THEN '2.연차' WHEN A.G_TYPE IN (9,10) THEN '4.당직' "
						   + "			    WHEN A.G_TYPE=12 THEN '3.휴가' "
						   + "              ELSE '' END) G_TYPE_NM, "
						   + "        (CASE WHEN A.G_FRTM<>'' AND A.G_TOTM<>'' "
						   + "              THEN A.G_SNM+' ('+LEFT(A.G_FRTM,2)+':'+SUBSTRING(A.G_FRTM,3,2)+'-'+LEFT(A.G_TOTM,2)+':'+SUBSTRING(A.G_TOTM,3,2)+')' ELSE A.G_SNM END) G_SNM2 "
						   + "   FROM DUTY_MSTGNMU A "
						   + "  ORDER BY G_TYPE_NM, A.G_CODE "; // A.G_TYPE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3010_GNMU", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}	
		//근무신청마감일자
		public void Get3010_SEARCH_CLOSDatas(string yymm, string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTCLOS "
						   + "  WHERE PLANYYMM = '" + yymm + "' AND DEPTCODE = '" + dept + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3010_SEARCH_CLOS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//근무신청내역 조회
		public void Get3010_SEARCH_OREQDatas(string yymm, string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM DUTY_TRSOREQ A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X1 "
						   + "     ON A.SABN = X1.EMBSSABN "
						   + "  WHERE LEFT(A.REQ_DATE,6) = '" + yymm + "' AND X1.EMBSDPCD = '" + dept + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3010_SEARCH_OREQ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//출근내역 조회
		public void Get3010_SEARCH_KTDatas(string yymm, string dept, DataSet ds)
		{
			try
			{
				string where1 = dept == "A001" ? "SAWON_NO IN (SELECT SAWON_NO FROM DUTY_TRSPLAN_ETC WHERE DEPTCODE = '" + dept + "')" : "DEPTCODE = '" + dept + "'";
				string where2 = dept == "A001" ? "EMBSSABN IN (SELECT SAWON_NO FROM DUTY_TRSPLAN_ETC WHERE DEPTCODE = '" + dept + "')" : "EMBSDPCD = '" + dept + "'";

				if (dept == "A002")
				{
					where1 = "SAWON_NO IN (SELECT EMBSSABN FROM MSTEMBS WHERE EMBSADGB = '1')";
					where2 = "EMBSADGB = '1'";
				}

				string qry = "  IF EXISTS(SELECT SAWON_NO FROM DUTY_TRSPLAN WHERE PLANYYMM='" + yymm + "' AND DEPTCODE = '" + dept + "' ) "
						   + "  BEGIN "
						   + "      SELECT RIGHT(A.USERID,5) as SABN, X1.USERNAME, CONVERT(VARCHAR,WORKSTART,112) SLDT "
						   + "        FROM TB_WORKRESULT A LEFT OUTER JOIN TB_USER X1 ON A.USERID=X1.USERID "
						   + "       WHERE RIGHT(A.USERID,5) in (SELECT SAWON_NO FROM DUTY_TRSPLAN WHERE PLANYYMM='" + yymm + "' AND " + where1 + " )"
						   + "         AND CONVERT(VARCHAR,A.WORKSTART,112) LIKE '" + yymm + "%' AND A.WORKSTART IS NOT NULL "
						   + "       ORDER BY RIGHT(A.USERID,5), A.WORKSTART "
						   + "  END "
						   + "  ELSE "
						   + "  BEGIN"
						   + "      SELECT RIGHT(A.USERID,5) as SABN, X1.USERNAME, CONVERT(VARCHAR,WORKSTART,112) SLDT "
						   + "        FROM TB_WORKRESULT A LEFT OUTER JOIN TB_USER X1 ON A.USERID=X1.USERID "
						   + "       WHERE RIGHT(A.USERID,5) in (SELECT EMBSSABN FROM MSTEMBS WHERE EMBSSTAT='1' AND " + where2 + " )"
						   + "         AND CONVERT(VARCHAR,A.WORKSTART,112) LIKE '" + yymm + "%' AND A.WORKSTART IS NOT NULL "
						   + "       ORDER BY RIGHT(A.USERID,5), A.WORKSTART "
						   + "  END ";
				//string qry = "  IF EXISTS(SELECT SAWON_NO FROM DUTY_TRSPLAN WHERE PLANYYMM='" + yymm + "' AND DEPTCODE = '" + dept + "' ) "
				//		   + "  BEGIN "
				//		   + "      SELECT RIGHT(A.USERID,5) as SABN, A.USERNAME, CONVERT(VARCHAR,ACCESSDATE,112) SLDT "
				//		   + "        FROM TB_ACCESS A "
				//		   + "       WHERE RIGHT(A.USERID,5) in (SELECT SAWON_NO FROM DUTY_TRSPLAN WHERE PLANYYMM='" + yymm + "' AND " + where1 + " )"
				//		   + "         AND CONVERT(VARCHAR,A.ACCESSDATE,112) LIKE '" + yymm + "%' AND A.AUTHMODE IN ('82') AND A.AUTHMODE1 IN ('0') "
				//		   + "       ORDER BY RIGHT(A.USERID,5), A.ACCESSDATE "
				//		   + "  END "
				//		   + "  ELSE "
				//		   + "  BEGIN"
				//		   + "      SELECT RIGHT(A.USERID,5) as SABN, A.USERNAME, CONVERT(VARCHAR,ACCESSDATE,112) SLDT "
				//		   + "        FROM TB_ACCESS A "
				//		   + "       WHERE RIGHT(A.USERID,5) in (SELECT EMBSSABN FROM MSTEMBS WHERE EMBSSTAT='1' AND " + where2 + " )"
				//		   + "         AND CONVERT(VARCHAR,A.ACCESSDATE,112) LIKE '" + yymm + "%' AND A.AUTHMODE IN ('82') AND A.AUTHMODE1 IN ('0') "
				//		   + "       ORDER BY RIGHT(A.USERID,5), A.ACCESSDATE "
				//		   + "  END ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3010_SEARCH_KT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//출근내역 조회
		public void Get3010_KT_DTDatas(string yymm, string sabn, DataSet ds)
		{
			try
			{
				string qry = " SELECT RIGHT(A.USERID,5) as SABN, X1.USERNAME, CONVERT(VARCHAR,WORKSTART,120) ACC_DT "
						   + "   FROM TB_WORKRESULT A LEFT OUTER JOIN TB_USER X1 ON A.USERID=X1.USERID "
						   + "  WHERE RIGHT(A.USERID,5) = '" + sabn + "'"
						   + "    AND CONVERT(VARCHAR,A.WORKSTART,112) LIKE '" + yymm + "%' AND A.WORKSTART IS NOT NULL "
						   + "  ORDER BY A.WORKSTART ";
				//string qry = " SELECT RIGHT(A.USERID,5) as SABN, A.USERNAME, CONVERT(VARCHAR,ACCESSDATE,120) ACC_DT "
				//		   + "   FROM TB_ACCESS A "
				//		   + "  WHERE RIGHT(A.USERID,5) = '" + sabn + "'"
				//		   + "    AND CONVERT(VARCHAR,A.ACCESSDATE,112) LIKE '" + yymm + "%' AND A.AUTHMODE IN ('82') AND A.AUTHMODE1 IN ('0') "
				//		   + "  ORDER BY A.ACCESSDATE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3010_KT_DT1", dt, ref ds);
				
				qry = " SELECT RIGHT(A.USERID,5) as SABN, X1.USERNAME, CONVERT(VARCHAR,WORKEND,120) ACC_DT "
						   + "   FROM TB_WORKRESULT A LEFT OUTER JOIN TB_USER X1 ON A.USERID=X1.USERID "
						   + "  WHERE RIGHT(A.USERID,5) = '" + sabn + "'"
						   + "    AND CONVERT(VARCHAR,A.WORKEND,112) LIKE '" + yymm + "%' AND A.WORKEND IS NOT NULL "
						   + "  ORDER BY A.WORKEND ";
				//qry = " SELECT RIGHT(A.USERID,5) as SABN, A.USERNAME, CONVERT(VARCHAR,ACCESSDATE,120) ACC_DT "
				//		   + "   FROM TB_ACCESS A "
				//		   + "  WHERE RIGHT(A.USERID,5) = '" + sabn + "'"
				//		   + "    AND CONVERT(VARCHAR,A.ACCESSDATE,112) LIKE '" + yymm + "%' AND A.AUTHMODE IN ('83') AND A.AUTHMODE1 IN ('0') "
				//		   + "  ORDER BY A.ACCESSDATE ";

				dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3010_KT_DT2", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//근무유형 조회
		public void Get3010_SEARCH_GNMUDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTGNMU ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3010_SEARCH_GNMU", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//휴가내역 조회
		public void Get3010_SEARCH_HUGADatas(string sabn, string date, string g_code, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_TRSJREQ WHERE SABN='" + sabn + "' "
						   + "    AND '" + date + "' BETWEEN REQ_DATE AND REQ_DATE2 "
						   + "    AND REQ_TYPE= '" + g_code +"'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3010_SEARCH_HUGA", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//환경설정 조회
		public void Get3010_INFO01Datas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOSD01 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3010_INFO01", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//근무신청조회
		public void GetSEARCH_PLANDatas(string yymm, string dept, DataSet ds)
		{
			try
			{
				string qry = " EXEC USP_DUTY3010_PRC_211014 '" + yymm + "', '" + dept + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_PLAN", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//근무신청내역조회
		public void GetDUTY_TRSPLANDatas(int gubn, string yymm, string dept, DataSet ds)
		{
			try
			{
				string where = "";
				if (dept == "A001")  //베드이송
					where = "SAWON_NO IN (SELECT SAWON_NO FROM DUTY_TRSPLAN_ETC WHERE PSTY<>'D') ";
				else if (dept == "A002")  //팀장
					where = "SAWON_NO IN (SELECT EMBSSABN FROM MSTEMBS WHERE EMBSADGB='1') ";
				else
					where = "DEPTCODE = '" + dept + "'";				

				string bf_mm = clib.DateToText(clib.TextToDate(yymm + "01").AddMonths(-1));
				string qry = " SELECT A.*, '1' CHK, RTRIM(X1.SAWON_NM) SAWON_NM, ISNULL(X1.ALLOWOFF,0) MASTER_OFF, "
						   + "        '' D01_NM, '' D02_NM, '' D03_NM, '' D04_NM, '' D05_NM, '' D06_NM, '' D07_NM, '' D08_NM, '' D09_NM, '' D10_NM, "
						   + "        '' D11_NM, '' D12_NM, '' D13_NM, '' D14_NM, '' D15_NM, '' D16_NM, '' D17_NM, '' D18_NM, '' D19_NM, '' D20_NM, "
						   + "        '' D21_NM, '' D22_NM, '' D23_NM, '' D24_NM, '' D25_NM, '' D26_NM, '' D27_NM, '' D28_NM, '' D29_NM, '' D30_NM, '' D31_NM, "
						   + "        X1.EXP_LV, (CASE X1.EXP_LV WHEN 1 THEN '전문가' WHEN 2 THEN '숙련가' WHEN 3 THEN '초보자' ELSE '' END) EXP_LV_NM, "
						   + "        A.MM_CNT1 + ISNULL(X3.Y_CNT1,0) AS Y_CNT1, A.MM_CNT2 + ISNULL(X3.Y_CNT2,0) AS Y_CNT2, A.MM_CNT3 + ISNULL(X3.Y_CNT3,0) AS Y_CNT3, "
						   + "        A.MM_CNT4 + ISNULL(X3.Y_CNT4,0) AS Y_CNT4, A.MM_CNT5 + ISNULL(X3.Y_CNT5,0) AS Y_CNT5, "
						   + "        ISNULL(X3.Y_CNT1,0) AS YEAR_CNT1, ISNULL(X3.Y_CNT2,0) AS YEAR_CNT2, ISNULL(X3.Y_CNT3,0) AS YEAR_CNT3, ISNULL(X3.Y_CNT4,0) AS YEAR_CNT4, ISNULL(X3.Y_CNT5,0) AS YEAR_CNT5 "
						   + "   FROM DUTY_TRSPLAN A "
						   + "   LEFT OUTER JOIN DUTY_MSTNURS X1 "
						   + "     ON A.SAWON_NO = X1.SAWON_NO "
                           //+ "   LEFT OUTER JOIN MSTEMBS X2 "
                           //+ "     ON A.SAWON_NO = X2.EMBSSABN "
                           + "   LEFT OUTER JOIN (SELECT SAWON_NO, "
						   + "                           SUM(CASE WHEN '" + yymm.Substring(4, 2) + "' = '01' THEN 0 ELSE MM_CNT1 END) Y_CNT1, "
						   + "                           SUM(CASE WHEN '" + yymm.Substring(4, 2) + "' = '01' THEN 0 ELSE MM_CNT2 END) Y_CNT2, "
						   + "                           SUM(CASE WHEN '" + yymm.Substring(4, 2) + "' = '01' THEN 0 ELSE MM_CNT3 END) Y_CNT3, "
						   + "                           SUM(CASE WHEN '" + yymm.Substring(4, 2) + "' = '01' THEN 0 ELSE MM_CNT4 END) Y_CNT4, "
						   + "                           SUM(CASE WHEN '" + yymm.Substring(4, 2) + "' = '01' THEN 0 ELSE MM_CNT5 END) Y_CNT5 "
						   + "		                FROM DUTY_TRSPLAN "
						   + "                     WHERE " + where + "" //DEPTCODE = '" + dept + "' "
						   + "                       AND PLANYYMM BETWEEN '" + yymm.Substring(0, 4) + "01' AND '" + bf_mm.Substring(0, 6) + "' "
						   + "				       GROUP BY SAWON_NO) X3 "
						   + "     ON A.SAWON_NO=X3.SAWON_NO "
						   + "  WHERE A.PLANYYMM = '" + yymm + "' AND A." + where;
				if (gubn == 3)
					qry += "   AND A.SAWON_NO NOT IN (SELECT G1.SAWON_NO FROM GW_TRSPLAN G1 "
						 + "                           INNER JOIN DUTY_GWDOC G2 ON G1.DOC_NO=G2.DOC_NO AND ISNULL(G2.AP_TAG,'') IN ('1','3','4') "
						 + "                           WHERE G1.PLANYYMM = '" + yymm + "' AND G1." + where + ")";

				qry += "  ORDER BY A.PLAN_SQ ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				if (gubn == 1)
					dp.AddDatatable2Dataset("DUTY_TRSPLAN", dt, ref ds);
				else if (gubn == 2)
					dp.AddDatatable2Dataset("SEARCH_PLAN", dt, ref ds);
				else if (gubn == 3)
					dp.AddDatatable2Dataset("GW_SEARCH_PLAN", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//전월근무신청내역조회
		public void GetDUTY_BF_TRSPLANDatas(string bf_yymm, string dept, DataSet ds)
		{
			try
			{
				string bf_mm = clib.DateToText(clib.TextToDate(bf_yymm + "01").AddMonths(-1));
				string qry = " SELECT A.*, RTRIM(X1.SAWON_NM) SAWON_NM, "
						   + "        '' D01_NM, '' D02_NM, '' D03_NM, '' D04_NM, '' D05_NM, '' D06_NM, '' D07_NM, '' D08_NM, '' D09_NM, '' D10_NM, "
						   + "        '' D11_NM, '' D12_NM, '' D13_NM, '' D14_NM, '' D15_NM, '' D16_NM, '' D17_NM, '' D18_NM, '' D19_NM, '' D20_NM, "
						   + "        '' D21_NM, '' D22_NM, '' D23_NM, '' D24_NM, '' D25_NM, '' D26_NM, '' D27_NM, '' D28_NM, '' D29_NM, '' D30_NM, '' D31_NM, "
						   + "        X1.EXP_LV, (CASE X1.EXP_LV WHEN 1 THEN '전문가' WHEN 2 THEN '숙련가' WHEN 3 THEN '초보자' ELSE '' END) EXP_LV_NM "
						   + "   FROM DUTY_TRSPLAN A "
						   + "   LEFT OUTER JOIN DUTY_MSTNURS X1 "
						   + "     ON A.SAWON_NO = X1.SAWON_NO "
						   + "  WHERE A.PLANYYMM = '" + bf_yymm + "' AND A.DEPTCODE = '" + dept + "' "
						   + "  ORDER BY A.PLAN_SQ ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_BF_PLAN", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//Sum_근무신청내역
		public void GetSUM_PLANDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT 0 AS G_TYPE, '' G_NM, * FROM DUTY_TRSPLAN WHERE 1 = 2 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SUM_PLAN", dt, ref ds);
				
				DataRow nrow = ds.Tables["SUM_PLAN"].NewRow();
				nrow["G_TYPE"] = "4";
				nrow["G_NM"] = "D";
				ds.Tables["SUM_PLAN"].Rows.Add(nrow);
				nrow = ds.Tables["SUM_PLAN"].NewRow();
				nrow["G_TYPE"] = "5";
				nrow["G_NM"] = "E";
				ds.Tables["SUM_PLAN"].Rows.Add(nrow);
				nrow = ds.Tables["SUM_PLAN"].NewRow();
				nrow["G_TYPE"] = "6";
				nrow["G_NM"] = "N";
				ds.Tables["SUM_PLAN"].Rows.Add(nrow);
				//nrow = ds.Tables["SUM_PLAN"].NewRow();
				//nrow["G_TYPE"] = "3";
				//nrow["G_NM"] = "연차";
				//ds.Tables["SUM_PLAN"].Rows.Add(nrow);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//조회부서 간호사 조회
		public void Get3010_SEARCH_NURSDatas(DataSet ds) //string bf_yymm, string dept, 
		{
			try
			{
				string qry = " SELECT A.SAWON_NO CODE, RTRIM(X1.EMBSNAME) NAME, "
						   + "        RTRIM(X2.DEPRNAM1) AS DEPT_NM, "
						   + "        A.RSP_GNMU, A.ALLOWOFF AS ALLOW_OFF, A.MAX_NCNT "
						   + "   FROM DUTY_MSTNURS A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X1 "
						   + "     ON A.SAWON_NO = X1.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X2 "
						   + "     ON X1.EMBSDPCD = X2.DEPRCODE "
						   + "  ORDER BY X1.EMBSDPCD ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3010_SEARCH_NURS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
				
		#region 3011 - 간호사사용부서설정

		//부서코드 불러오기
		public void GetSEARCH_NURSDEPTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT (CASE WHEN X1.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) CHK, "
						   + "        RTRIM(DEPRNAM1) DEPT_NM, A.*  "
						   + "   FROM " + wagedb + ".DBO.MSTDEPR A "
						   + "   LEFT OUTER JOIN DUTY_INFONURS X1 ON A.DEPRCODE=X1.DEPTCODE "
						   + "  WHERE A.DEPRSTAT=1 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_NURSDEPT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//삭제할 부서설정 테이블 불러오기
		public void GetD_DUTY_INFONURSDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFONURS A ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("D_DUTY_INFONURS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//등록할 부서설정 테이블 불러오기
		public void GetDUTY_INFONURSDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFONURS A ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_INFONURS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		#endregion
				
		#region 3012 - OFF,N 수당 결재
		
		//등록할 부서설정 테이블 불러오기
		public void Get3012_SEARCH_SDDatas(string yymm, string sabn, DataSet ds)
		{
			try
			{
				string qry = " IF OBJECT_ID('tempdb..#DUTY_TRSPLAN') IS NOT NULL "
						   + "  	DROP TABLE #DUTY_TRSPLAN ";
				qry += " SELECT A1.* INTO #DUTY_TRSPLAN "
					+ "    FROM ( SELECT A.* FROM DUTY_TRSPLAN A WHERE A.PLANYYMM='" + yymm + "' AND A.DEPTCODE IN ('0100','0200') " // 5A,5B 간호조무사 포함 23.03.08 조수진
					+ "         UNION ALL "
					+ "           SELECT A.* FROM DUTY_TRSPLAN A "
					+ "             LEFT OUTER JOIN MSTEMBS X1 ON A.SAWON_NO=X1.EMBSSABN "
					+ "            WHERE A.PLANYYMM='" + yymm + "' AND A.DEPTCODE NOT IN ('0100','0200') AND X1.EMBSGRCD NOT IN ('0200') "
					+ "         ) A1 ";

				qry += " SELECT '1' CHK, A.DEPTCODE, RTRIM(X2.DEPRNAM1) AS DEPT_NM, "
					+ "        A.SAWON_NO, RTRIM(X1.EMBSNAME) AS EMBSNAME, "
					+ "        A.BF_OFF+A.ALLOW_OFF-A.REMAIN_OFF-A.MM_CNT4 AS OFF_CNT, "
					+ "        A.MM_CNT3-A.MAX_NCNT-A.BF_NIGHT+A.REMAIN_NIGHT AS N_CNT, "
                    + "        ISNULL(X9.T_AMT,0) AS OFF_SD, "
                    + "        (A.BF_OFF+A.ALLOW_OFF-A.REMAIN_OFF-A.MM_CNT4) * ISNULL(X9.T_AMT,0) * (SELECT A04_INSU11*A04_INSU12 FROM DUTY_INFOSD02) AS OFF_AMT, "
                    + "        (CASE WHEN (A.MM_CNT3-A.MAX_NCNT-A.BF_NIGHT+A.REMAIN_NIGHT) < 0 "
                    + "              THEN ISNULL(X8.MINUS_NAMT,0) ELSE ISNULL(X8.PLUS_NAMT,0) END) AS N_SD, "
                    + "        (CASE WHEN (A.MM_CNT3-A.MAX_NCNT-A.BF_NIGHT+A.REMAIN_NIGHT) < 0 "
					+ "              THEN (A.MM_CNT3-A.MAX_NCNT-A.BF_NIGHT+A.REMAIN_NIGHT) * ISNULL(X8.MINUS_NAMT,0) "
					+ "              ELSE (A.MM_CNT3-A.MAX_NCNT-A.BF_NIGHT+A.REMAIN_NIGHT) * ISNULL(X8.PLUS_NAMT,0) END) AS N_AMT, "
					+ "        (A.BF_OFF+A.ALLOW_OFF-A.REMAIN_OFF-A.MM_CNT4) * ISNULL(X9.T_AMT,0) * (SELECT A04_INSU11*A04_INSU12 FROM DUTY_INFOSD02) + "
					+ "        (CASE WHEN (A.MM_CNT3-A.MAX_NCNT-A.BF_NIGHT+A.REMAIN_NIGHT) < 0 "
					+ "              THEN (A.MM_CNT3-A.MAX_NCNT-A.BF_NIGHT+A.REMAIN_NIGHT) * ISNULL(X8.MINUS_NAMT,0) "
					+ "              ELSE (A.MM_CNT3-A.MAX_NCNT-A.BF_NIGHT+A.REMAIN_NIGHT) * ISNULL(X8.PLUS_NAMT,0) END) AS SD_AMT, "
					+ "        '' REMARK, "
					+ "        A.BF_OFF, A.MM_CNT4, A.ALLOW_OFF, A.REMAIN_OFF, "
					+ "        A.BF_NIGHT, A.MM_CNT3, A.MAX_NCNT, A.REMAIN_NIGHT "
					+ "   FROM #DUTY_TRSPLAN A "
					+ "   LEFT OUTER JOIN MSTEMBS X1 ON A.SAWON_NO=X1.EMBSSABN "
					+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X2 ON A.DEPTCODE=X2.DEPRCODE "
					+ "   LEFT OUTER JOIN DUTY_INFOSD01 X9 ON A.SAWON_NO=X9.SABN AND A.PLANYYMM=X9.YYMM "  //시급
					+ "   LEFT OUTER JOIN DUTY_INFOSD05 X8 ON A.SAWON_NO=X8.SABN AND A.PLANYYMM=X8.YYMM ";  //N수당
				if (SilkRoad.Config.SRConfig.USID != "SAMIL")
					qry += "    INNER JOIN DUTY_PWERDEPT X3 ON A.DEPTCODE=X3.DEPT AND X3.SABN='" + sabn + "' ";
				//qry += " WHERE A.PLANYYMM='" + yymm +"' " //AND X1.EMBSGRCD NOT IN ('0200')"  //간호조무사 제외 23.01.12  
				qry += "  WHERE (X1.EMBSSTAT=1 OR (X1.EMBSSTAT=2 AND LEFT(X1.EMBSTSDT,6)>= '" + yymm + "') ) "  // 퇴직제외 23.01.12
					+ "    AND (A.BF_OFF+A.ALLOW_OFF-A.REMAIN_OFF-A.MM_CNT4 <> 0 OR A.MM_CNT3-A.MAX_NCNT-A.BF_NIGHT+A.REMAIN_NIGHT <> 0 ) "
					+ "  ORDER BY A.DEPTCODE, A.SAWON_NO  ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3012_SEARCH_SD", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//GW_TRSOFFN 테이블 불러오기
		public void GetGW_TRSOFFNDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM GW_TRSOFFN WHERE 1=2 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("GW_TRSOFFN", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		#endregion
				
		#region 3013 - 밤근무/간호간병비 결재
				
		//등록할 부서설정 테이블 불러오기
		public void Get3013_SEARCH_SDDatas(int gubn, string yymm, string sabn, DataSet ds)
		{
			try
			{
                //1:밤근무수당 2:간호간병비
                //밤근무수당 김정은-6A/6B/7A/7B, 한지영 응급실/중환자실
                //간호간병비 김정은-5A/5B
                string qry = " SELECT '1' CHK, A.DEPTCODE, RTRIM(X2.DEPRNAM1) AS DEPT_NM, "
						   + "        A.SAWON_NO, RTRIM(X1.EMBSNAME) AS EMBSNAME, "
						   + "        0.0 AS PAST_YEAR, A.MM_CNT3, X1.EMBSIPDT, "
						   + "        (CASE WHEN X1.EMBSIPDT='' THEN '' ELSE LEFT(X1.EMBSIPDT,4)+'-'+SUBSTRING(X1.EMBSIPDT,5,2)+'-'+SUBSTRING(X1.EMBSIPDT,7,2) END) AS IPDT, "
						   + "        0 AS SD_AMT, 0 AS SD_AMT2, 0 AS SD_AMT3 "
                           + "   FROM DUTY_TRSPLAN A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON A.SAWON_NO=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X2 ON A.DEPTCODE=X2.DEPRCODE "
						   //+ "   INNER JOIN DUTY_PWERDEPT X3 ON A.DEPTCODE=X3.DEPT AND X3.SABN='" + sabn + "' "
						   + "  WHERE A.PLANYYMM='" + yymm + "' AND ISNULL(A.SHIFT_WORK,0) <> 0 ";
				if (gubn == 1 && SilkRoad.Config.SRConfig.USID == "SAMIL")
					qry += "    AND A.DEPTCODE NOT IN ('0100','0200') ";
				else if (gubn == 1 && SilkRoad.Config.SRConfig.USID != "SAMIL")
					qry += "    AND A.DEPTCODE NOT IN ('0100','0200') AND A.DEPTCODE IN (SELECT DEPT FROM DUTY_PWERDEPT WHERE SABN = '" + SilkRoad.Config.SRConfig.USID + "') ";
				else if (gubn == 2)
					qry += "    AND A.DEPTCODE IN ('0100','0200') ";
				qry += "  ORDER BY A.DEPTCODE, A.SAWON_NO  ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3013_SEARCH_SD", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        //3교대제외인지(고정n수=0 으로 체크) 간호조무사 인지 체크
        public void GetCHK_INCENTDatas(string sabn, DataSet ds)
        {
            try
            {
                string qry = " SELECT SAWON_NO FROM DUTY_MSTNURS WHERE MAX_NCNT IN (0,14) AND SAWON_NO = '" + sabn + "'"
                           + " UNION ALL"
                           + " SELECT EMBSSABN FROM MSTEMBS WHERE EMBSGRCD = '0200' AND EMBSSABN = '" + sabn + "'";

                DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
                dp.AddDatatable2Dataset("CHK_INCENT", dt, ref ds);
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //GW_TRSYAGAN 테이블 불러오기
        public void GetGW_TRSYAGANDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM GW_TRSYAGAN WHERE 1=2 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("GW_TRSYAGAN", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		#endregion
				
		#region 3013 - 밤근무/간호간병비 결재xxxxxxx
				
		////등록할 부서설정 테이블 불러오기
		//public void Get3013_SEARCH_SDDatas(int gubn, string yymm, string sabn, DataSet ds)
		//{
		//	try
		//	{
		//		//1:밤근무수당 2:간호간병비
		//		//김정은 5A/5B - 간호간병비, 6A/6B/7A/7B - 밤근무수당
		//		//한지영 응급실/중환자실 - 밤근무수당
		//		string qry = " SELECT '1' CHK, A.DEPTCODE, RTRIM(X2.DEPRNAM1) AS DEPT_NM, "
		//				   + "        A.SAWON_NO, RTRIM(X1.EMBSNAME) AS EMBSNAME, "
		//				   + "        0.0 AS PAST_YEAR, A.MM_CNT3, X1.EMBSIPDT, "
		//				   + "        (CASE WHEN X1.EMBSIPDT='' THEN '' ELSE LEFT(X1.EMBSIPDT,4)+'-'+SUBSTRING(X1.EMBSIPDT,5,2)+'-'+SUBSTRING(X1.EMBSIPDT,7,2) END) AS IPDT, "
		//				   + "        0 AS SD_AMT "
		//				   + "   FROM DUTY_TRSPLAN A "
		//				   + "   LEFT OUTER JOIN MSTEMBS X1 ON A.SAWON_NO=X1.EMBSSABN "
		//				   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X2 ON A.DEPTCODE=X2.DEPRCODE "
		//				   //+ "   INNER JOIN DUTY_PWERDEPT X3 ON A.DEPTCODE=X3.DEPT AND X3.SABN='" + sabn + "' "
		//				   + "  WHERE A.PLANYYMM='" + yymm + "' AND ISNULL(A.SHIFT_WORK,0) <> 0 ";
		//		if (gubn == 1 && SilkRoad.Config.SRConfig.USID == "SAMIL")
		//			qry += "    AND A.DEPTCODE NOT IN ('0100','0200') ";
		//		else if (gubn == 1 && SilkRoad.Config.SRConfig.USID != "SAMIL")
		//			qry += "    AND A.DEPTCODE NOT IN ('0100','0200') AND A.DEPTCODE IN (SELECT DEPT FROM DUTY_PWERDEPT WHERE SABN = '" + SilkRoad.Config.SRConfig.USID + "') ";
		//		else if (gubn == 2)
		//			qry += "    AND A.DEPTCODE IN ('0100','0200') ";
		//		qry += "  ORDER BY A.DEPTCODE, A.SAWON_NO  ";

		//		DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
		//		dp.AddDatatable2Dataset("3013_SEARCH_SD", dt, ref ds);
		//	}
		//	catch (System.Exception ec)
		//	{
		//		System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
		//											 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}
		
		////GW_TRSYAGAN 테이블 불러오기
		//public void GetGW_TRSYAGANDatas(DataSet ds)
		//{
		//	try
		//	{
		//		string qry = " SELECT * "
		//				   + "   FROM GW_TRSYAGAN WHERE 1=2 ";

		//		DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
		//		dp.AddDatatable2Dataset("GW_TRSYAGAN", dt, ref ds);
		//	}
		//	catch (System.Exception ec)
		//	{
		//		System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
		//											 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}
		#endregion
				
		#region 3014 - 근무표 결재
		
		//GW_TRSPLAN 테이블 불러오기
		public void GetGW_TRSPLANDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM GW_TRSPLAN WHERE 1=2 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("GW_TRSPLAN", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
				
		#region 3015 - 환경설정
		
		//당직시간 조회
		public void GetS_DUTY_TRSLOFFDatas(string dept, string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT *, "
						   + "        LEFT(SLDT,4)+'-'+SUBSTRING(SLDT,5,2)+'-'+SUBSTRING(SLDT,7,2) AS SLDT_NM, "
						   + "        (CASE DATEPART(DW,SLDT) WHEN 1 THEN '일' WHEN 2 THEN '월' WHEN 3 THEN '화' "
						   + "         WHEN 4 THEN '수' WHEN 5 THEN '목' WHEN 6 THEN '금' WHEN 7 THEN '토' ELSE '' END) DAY_NM "
						   + "   FROM DUTY_TRSLOFF "
						   + "  WHERE DEPT = '" + dept + "' AND LEFT(SLDT,6)= '" + yymm + "' "
						   + "  ORDER BY SLDT ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("S_DUTY_TRSLOFF", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//일자별 당직시간 저장
		public void GetDUTY_TRSLOFFDatas(string dept, string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_TRSLOFF "
						   + "  WHERE DEPT = '" + dept + "' AND LEFT(SLDT,6)= '" + yymm + "' "
						   + "  ORDER BY SLDT ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSLOFF", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		public void GetS_MSTNURSDatas(string dept, int gubn, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*,"
						   + "        A.EXP_YEAR+A.EXP_Y2+A.EXP_Y3+A.EXP_Y4 AS EXP_T, "
						   + "        (CASE WHEN A.STAT=1 THEN '정상' ELSE '사용중지' END) STAT_NM, "
						   + "        (CASE A.SHIFT_WORK WHEN 1 THEN 'Y' WHEN 2 THEN 'N' ELSE '' END) SHIFT_WORK_NM, "  //교대여부
						   + "        ISNULL(X1.EMBSDPCD,'') DEPTCODE, RTRIM(ISNULL(X2.DEPRNAM1,'')) DEPT_NM, "
						   + "        RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',X1.EMBSPHPN) as varchar(100))) HPNO, RTRIM(X1.EMBSEMAL) AS EMAIL_ID "
						   + "   FROM DUTY_MSTNURS A "
						   + "   LEFT OUTER JOIN " + wagedb + ".DBO.MSTEMBS X1 ON A.SAWON_NO=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".DBO.MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE X1.EMBSDPCD LIKE '" + dept + "'";
				if (gubn > 0)
					qry += " AND X1.EMBSSTAT = " + gubn;
				qry += "  ORDER BY A.SAWON_NO ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("S_MSTNURS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//베드이송 직원 조회
		public void GetS_BEDDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, "
						   + "        RTRIM(ISNULL(X2.DEPRNAM1,'')) DEPT_NM "
						   + "   FROM DUTY_TRSPLAN_ETC A "
						   + "   LEFT OUTER JOIN " + wagedb + ".DBO.MSTEMBS X1 ON A.SAWON_NO=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".DBO.MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE A.PSTY<>'D' "
						   + "  ORDER BY A.SAWON_NO ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("S_BED", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//베드이송 직원 조회
		public void GetS_MSTNURS2Datas(DataSet ds)
		{
			try
			{
				string qry = " SELECT A.SAWON_NO AS CODE, RTRIM(X1.EMBSNAME) NAME, "
						   + "        A.*, "
						   + "        (CASE WHEN A.STAT=1 THEN '정상' ELSE '사용중지' END) STAT_NM, "
						   + "        (CASE A.SHIFT_WORK WHEN 1 THEN 'Y' WHEN 2 THEN 'N' ELSE '' END) SHIFT_WORK_NM, "  //교대여부
						   + "        ISNULL(X1.EMBSDPCD,'') DEPTCODE, RTRIM(ISNULL(X2.DEPRNAM1,'')) DEPT_NM, "
						   + "        RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',X1.EMBSPHPN) as varchar(100))) HPNO, RTRIM(X1.EMBSEMAL) AS EMAIL_ID "
						   + "   FROM DUTY_MSTNURS A "
						   + "   LEFT OUTER JOIN " + wagedb + ".DBO.MSTEMBS X1 ON A.SAWON_NO=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".DBO.MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  ORDER BY X1.EMBSDPCD, X1.EMBSNAME ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("S_MSTNURS2", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//베드이송 직원 조회
		public void GetDUTY_TRSPLAN_ETCDatas(string sabn, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM DUTY_TRSPLAN_ETC A "
						   + "  WHERE A.SAWON_NO = '" + sabn + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSPLAN_ETC", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        #endregion
        
        #region xxxxxx


        ////베드이송 직원 조회
        //public void GetS_BEDDatas(DataSet ds)
        //{
        //	try
        //	{
        //		string qry = " SELECT A.*, "
        //				   + "        RTRIM(ISNULL(X2.DEPRNAM1,'')) DEPT_NM "
        //				   + "   FROM DUTY_TRSPLAN_ETC A "
        //				   + "   LEFT OUTER JOIN " + wagedb + ".DBO.MSTEMBS X1 ON A.SAWON_NO=X1.EMBSSABN "
        //				   + "   LEFT OUTER JOIN " + wagedb + ".DBO.MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
        //				   + "  WHERE A.PSTY<>'D' "
        //				   + "  ORDER BY A.SAWON_NO ";

        //		DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
        //		dp.AddDatatable2Dataset("S_BED", dt, ref ds);
        //	}
        //	catch (System.Exception ec)
        //	{
        //		System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
        //											 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //	}
        //}
        ////베드이송 직원 조회
        //public void GetS_MSTNURS2Datas(DataSet ds)
        //{
        //	try
        //	{
        //		string qry = " SELECT A.SAWON_NO AS CODE, RTRIM(X1.EMBSNAME) NAME, "
        //				   + "        A.*, "
        //				   + "        (CASE WHEN A.STAT=1 THEN '정상' ELSE '사용중지' END) STAT_NM, "
        //				   + "        (CASE A.SHIFT_WORK WHEN 1 THEN 'Y' WHEN 2 THEN 'N' ELSE '' END) SHIFT_WORK_NM, "  //교대여부
        //				   + "        ISNULL(X1.EMBSDPCD,'') DEPTCODE, RTRIM(ISNULL(X2.DEPRNAM1,'')) DEPT_NM, "
        //				   + "        RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',X1.EMBSPHPN) as varchar(100))) HPNO, RTRIM(X1.EMBSEMAL) AS EMAIL_ID "
        //				   + "   FROM DUTY_MSTNURS A "
        //				   + "   LEFT OUTER JOIN " + wagedb + ".DBO.MSTEMBS X1 ON A.SAWON_NO=X1.EMBSSABN "
        //				   + "   LEFT OUTER JOIN " + wagedb + ".DBO.MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
        //				   + "  ORDER BY X1.EMBSDPCD, X1.EMBSNAME ";

        //		DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
        //		dp.AddDatatable2Dataset("S_MSTNURS2", dt, ref ds);
        //	}
        //	catch (System.Exception ec)
        //	{
        //		System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
        //											 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //	}
        //}
        //베드이송 직원 조회
        public void Get123DUTY_TRSPLAN_ETCDatas(string sabn, DataSet ds)
        {
            try
            {
                string qry = " SELECT A.* "
                           + "   FROM DUTY_TRSPLAN_ETC A "
                           + "  WHERE A.SAWON_NO = '" + sabn + "' ";

                //DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
                //dp.AddDatatable2Dataset("DUTY_TRSPLAN_ETC", dt, ref ds);
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        #region 인사카드

        //INFOBASE
        public void GetINFOBASEDatas(ref DataSet ds)
        {
            try
            {
                string qry = " SELECT * "
                           + "   FROM INFOBASE "
                           + "  WHERE IFBSSBCN <> 0 OR IFBSSBCN = 0";

                DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);
                dp.AddDatatable2Dataset("INFOBASE", dt, ref ds);
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //레포트 부분 - 기본정보 및 사진
        public void GetSabnSubInfo(string sabn, DataSet ds)
        {
            try
            {
                string qry = "";
                qry = "     SELECT A.EMBSNAME AS 성명, A.EMBSSABN AS 사번, A.PHOTO AS 사진, A.EMBSPOST AS 우편, "
                    + "            RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPTSA) as varchar(13))) AS 주민번호, "
                    + "            RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPTLN) as varchar(20))) AS 전화번호, "
                    + "            RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPHPN) as varchar(20))) AS 핸드폰, "
                    + "            RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPAD1) as varchar(200))) AS 주소1, "
                    + "            RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',A.EMBSPAD2) as varchar(100))) AS 주소2, "
                    + "            (CASE WHEN EMBSIPDT = '' THEN '' ELSE " + substring + "(EMBSIPDT,1,4) " + plus + " '-' " + plus + " " + substring + "(EMBSIPDT,5,2) " + plus + " '-' " + plus + " " + substring + "(EMBSIPDT,7,2) END) AS 입사일 , "
                    + "            (CASE WHEN EMBSTSDT = '' THEN '' ELSE " + substring + "(EMBSTSDT,1,4) " + plus + " '-' " + plus + " " + substring + "(EMBSTSDT,5,2) " + plus + " '-' " + plus + " " + substring + "(EMBSTSDT,7,2) END) AS 퇴사일 , "
                    + "            (CASE WHEN EMBSGRDT = '' THEN '' ELSE " + substring + "(EMBSGRDT,1,4) " + plus + " '-' " + plus + " " + substring + "(EMBSGRDT,5,2) " + plus + " '-' " + plus + " " + substring + "(EMBSGRDT,7,2) END) AS 그룹입사일 , "
                    + "            A_1.GLOVNAM2 AS 사업부, "
                    + "            A_2.DEPRNAM2 AS 부서, "
                    + "            A_3.SITENAM2 AS 현장,"
                    + "            A_4.JONGNAM2 AS 직종,"
                    + "            A_5.POSINAM2 AS 직위,"
                    + "            A_6.JDEFNAM2 AS 직무,"
                    + "            A_7.GRADNAM2 AS 직급,"
                    + "            A.EMBSHOBO AS 호봉"
                    + "   FROM MSTEMBS A"
                    + "   LEFT OUTER JOIN MSTGLOV A_1 ON A.EMBSGLCD = A_1.GLOVCODE "
                    + "   LEFT OUTER JOIN MSTDEPR A_2 ON A.EMBSDPCD = A_2.DEPRCODE"
                    + "   LEFT OUTER JOIN MSTSITE A_3 ON A.EMBSSTCD = A_3.SITECODE"
                    + "   LEFT OUTER JOIN MSTJONG A_4 ON A.EMBSJOCD = A_4.JONGCODE"
                    + "   LEFT OUTER JOIN MSTPOSI A_5 ON A.EMBSPSCD = A_5.POSICODE"
                    + "   LEFT OUTER JOIN MSTJDEF A_6 ON A.EMBSJDCD = A_6.JDEFCODE"
                    + "   LEFT OUTER JOIN MSTGRAD A_7 ON A.EMBSGRCD = A_7.GRADCODE"
                    + "  WHERE EMBSSABN = '" + sabn + "' ";

                DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), wagedb, qry);
                dp.AddDatatable2Dataset("INSA_PIC", dt, ref ds);
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //레포트 부분 - 개인사항
        public void GetMstEmplDatas(string sabn, ref DataSet ds)
        {
            try
            {
                string qry = " SELECT A.*, "
                            + "       B.ENTRNAM2 AS 입사유형, C.JOKYNAM2 AS 종교, D.KUBYNAM2 AS 군별, E.BYYONAM2 AS 병역, F.KEGENAM2 AS 계급, G.JUNYNAM2 AS 전역구분, "
                            + "       H.JUGONAM2 AS 주거구분, I.CHDONAM2 AS 출신도, J_1.LANGNAM2 AS 외국어1, J_2.LANGNAM2 AS 외국어2, J_3.LANGNAM2 AS 외국어3 "
                            + "  FROM MSTEMPL A"
                            + "  LEFT OUTER JOIN (SELECT ENTRCODE, ENTRNAM2 FROM MSTENTR ) B"
                            + "    ON A.EMPLETGU = B.ENTRCODE"
                            + "  LEFT OUTER JOIN (SELECT JOKYCODE, JOKYNAM2 FROM MSTJOKY ) C"
                            + "    ON A.EMPLJONG = C.JOKYCODE"
                            + "  LEFT OUTER JOIN (SELECT KUBYCODE, KUBYNAM2 FROM MSTKUBY ) D"
                            + "    ON A.EMPLKBGU = D.KUBYCODE"
                            + "  LEFT OUTER JOIN (SELECT BYYOCODE, BYYONAM2 FROM MSTBYYO ) E"
                            + "    ON A.EMPLYJGU = E.BYYOCODE"
                            + "  LEFT OUTER JOIN (SELECT KEGECODE, KEGENAM2 FROM MSTKEGE ) F"
                            + "    ON A.EMPLKGUP = F.KEGECODE"
                            + "  LEFT OUTER JOIN (SELECT JUNYCODE, JUNYNAM2 FROM MSTJUNY ) G"
                            + "    ON A.EMPLJYGU = G.JUNYCODE"
                            + "  LEFT OUTER JOIN (SELECT JUGOCODE, JUGONAM2 FROM MSTJUGO ) H"
                            + "    ON A.EMPLHUGU = H.JUGOCODE"
                            + "  LEFT OUTER JOIN (SELECT CHDOCODE, CHDONAM2 FROM MSTCHDO ) I"
                            + "    ON A.EMPLCDCD = I.CHDOCODE"
                            + "  LEFT OUTER JOIN (SELECT LANGCODE, LANGNAM2 FROM MSTLANG) J_1"
                            + "    ON A.EMPLLAN1 = J_1.LANGCODE"
                            + "  LEFT OUTER JOIN (SELECT LANGCODE, LANGNAM2 FROM MSTLANG ) J_2"
                            + "    ON A.EMPLLAN2 = J_2.LANGCODE"
                            + "  LEFT OUTER JOIN (SELECT LANGCODE, LANGNAM2 FROM MSTLANG ) J_3"
                            + "    ON A.EMPLLAN3 = J_3.LANGCODE"
                            + " WHERE EMPLSABN LIKE '%" + sabn + "%'";

                DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), wagedb, qry);
                dp.AddDatatable2Dataset("MSTEMPL", dt, ref ds);

                //기본 1ROW 추가
                for (int i = 0; ds.Tables["MSTEMPL"].Rows.Count < 1; i++)
                {
                    DataRow drow = ds.Tables["MSTEMPL"].NewRow();
                    ds.Tables["MSTEMPL"].Rows.Add(drow);
                }

            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        //레포트 부분 - 학력사항
        public void GetTrsHakrDatas(string sabn, ref DataSet ds)
        {
            try
            {
                string top_qry = string.Empty;
                string top_qry2 = string.Empty;

                if (DataAccess.DBtype == "1")
                {
                    top_qry = " TOP(5) ";
                }
                else
                {
                    top_qry = " ";
                    top_qry2 = " AND ROWNUM <= 5 ";
                }


                string qry = " SELECT "
                            + top_qry
                            + " A.HAKRSABN, A.HAKRIHMM, A.HAKRJYMM, A.HAKRADDR, A.HAKRINDT, A.HAKRUPDT, A.HAKRUSID, A.HAKRPSTY, "
                            + "       A.HAKRGRCD AS 학력구분, A.HAKRSCCD AS 출신학교,  A.HAKRMJCD AS 전공, A.HAKRGACD AS 학위  "
                            + " FROM TRSHAKR A "
                            + "WHERE HAKRSABN LIKE '%" + sabn + "%'"
                            + top_qry2
                            + "ORDER BY A.HAKRJYMM DESC";

                DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);
                dp.AddDatatable2Dataset("TRSHAKR", dt, ref ds);

                //기본 7ROW 추가                
                for (int i = 0; ds.Tables["TRSHAKR"].Rows.Count < 5; i++)
                {
                    DataRow drow = ds.Tables["TRSHAKR"].NewRow();
                    ds.Tables["TRSHAKR"].Rows.Add(drow);
                }
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //레포트 부분 - 경력사항
        public void GetTrsHistDatas(string sabn, ref DataSet ds)
        {
            try
            {
                string qry = string.Empty;
                //남흥계열사이면
                if (SilkRoad.Config.WGConfig.NHYN)
                {
                    qry = " SELECT TOP(10) * "
                        + "  FROM TRSHIST"
                        + " WHERE HISTSABN LIKE '%" + sabn + "%'"
                        + " ORDER BY HISTFDAY DESC";

                    DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);
                    dp.AddDatatable2Dataset("TRSHIST", dt, ref ds);

                    //기본 10ROW 추가
                    for (int i = 0; ds.Tables["TRSHIST"].Rows.Count < 10; i++)
                    {
                        DataRow drow = ds.Tables["TRSHIST"].NewRow();
                        ds.Tables["TRSHIST"].Rows.Add(drow);
                    }
                }
                else
                {
                    qry = " SELECT "
                        + (DataAccess.DBtype == "1" ? "TOP(5) * " : " * ")
                        + "  FROM TRSHIST"
                        + " WHERE HISTSABN LIKE '%" + sabn + "%'"
                        + (DataAccess.DBtype == "1" ? " " : " AND ROWNUM <= 5 ")
                        + " ORDER BY HISTFDAY DESC";

                    DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);
                    dp.AddDatatable2Dataset("TRSHIST", dt, ref ds);

                    //기본 7ROW 추가
                    for (int i = 0; ds.Tables["TRSHIST"].Rows.Count < 5; i++)
                    {
                        DataRow drow = ds.Tables["TRSHIST"].NewRow();
                        ds.Tables["TRSHIST"].Rows.Add(drow);
                    }
                }
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //레포트 부분 - 자격사항
        public void GetTrsPlicDatas(string sabn, ref DataSet ds)
        {
            try
            {
                string qry = " SELECT "
                            + (DataAccess.DBtype == "1" ? "TOP(6) " : " ")
                            + " A.PLICSABN, A.PLICLCNO, A.PLICSVNO, A.PLICCDAY, A.PLICDDAY, A.PLICEDAY, A.PLICDESC, A.PLICJCYN, "
                            + "              A.PLICINDT, A.PLICUPDT, A.PLICUSID, A.PLICPSTY, "
                            + "              A.PLICLCCD AS 자격명  "
                            + " FROM TRSPLIC A "
                            + " WHERE PLICSABN LIKE '%" + sabn + "%'"
                            + (DataAccess.DBtype == "1" ? " " : " AND ROWNUM <= 6 ")
                            + " ORDER BY PLICCDAY DESC";

                DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);
                dp.AddDatatable2Dataset("TRSPLIC", dt, ref ds);

                for (int i = 0; ds.Tables["TRSPLIC"].Rows.Count < 6; i++)
                {
                    DataRow drow = ds.Tables["TRSPLIC"].NewRow();
                    ds.Tables["TRSPLIC"].Rows.Add(drow);
                }
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        #region PAGE2_1
        //레포트 부분 - 상벌사항
        public void GetTrsAwdiDatas(string sabn, string gubn, ref DataSet ds)
        {
            try
            {
                string top = "";
                string and_top = "";

                if (gubn == "1" || gubn == "2")
                {
                    top = DataAccess.DBtype == "1" ? "TOP(4) " : " ";
                    and_top = DataAccess.DBtype == "1" ? " " : " AND ROWNUM <= 4 ";
                }
                else
                {
                    top = DataAccess.DBtype == "1" ? "TOP(6) " : " ";
                    and_top = DataAccess.DBtype == "1" ? " " : " AND ROWNUM <= 5 ";
                }


                string qry = " SELECT " + top + " A.AWDISABN, A.AWDIFDAT, A.AWDITDAT, A.AWDIDESC, A.AWDIDOWN,A.AWDIDEPT, A.AWDIINDT, A.AWDIUPDT, A.AWDIUSID, A.AWDIPSTY, "
                            + "       A.AWDIAWCD AS 구분 "
                            + "  FROM TRSAWDI A "
                            + " WHERE AWDISABN LIKE '%" + sabn + "%'"
                            + and_top
                            + " ORDER BY AWDIFDAT DESC";

                DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);

                if (gubn == "1")
                {
                    dp.AddDatatable2Dataset("TRSAWDI_1", dt, ref ds);
                    for (int i = 0; ds.Tables["TRSAWDI_1"].Rows.Count < 4; i++)
                    {
                        DataRow drow = ds.Tables["TRSAWDI_1"].NewRow();
                        ds.Tables["TRSAWDI_1"].Rows.Add(drow);
                    }
                }
                else if (gubn == "2")
                {
                    dp.AddDatatable2Dataset("TRSAWDI_2", dt, ref ds);
                    for (int i = 0; ds.Tables["TRSAWDI_2"].Rows.Count < 4; i++)
                    {
                        DataRow drow = ds.Tables["TRSAWDI_2"].NewRow();
                        ds.Tables["TRSAWDI_2"].Rows.Add(drow);
                    }
                }
                else
                {
                    dp.AddDatatable2Dataset("TRSAWDI", dt, ref ds);
                    for (int i = 0; ds.Tables["TRSAWDI"].Rows.Count < 5; i++)
                    {
                        DataRow drow = ds.Tables["TRSAWDI"].NewRow();
                        ds.Tables["TRSAWDI"].Rows.Add(drow);
                    }
                }
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //레포트 부분 - 교육사항
        public void GetTrsAwaiDatas(string sabn, ref DataSet ds)
        {
            try
            {
                string qry = " SELECT "
                            + (DataAccess.DBtype == "1" ? "TOP(6) " : " ")
                            + "A.AWAISABN, A.AWAIFDAT, A.AWAITDAT, A.AWAIDESC, A.AWAIAREA, A.AWAIDEPT, A.AWAIIAMT, A.AWAIOAMT, "
                            + "       A.AWAIINDT, A.AWAIUPDT, A.AWAIUSID, A.AWAIPSTY, A.AWAIREMK, "
                            + "       B.EDUCCODE AS AWAIEUCD "
                            + "  FROM TRSAWAI A "
                            + "  LEFT OUTER JOIN (SELECT EDUCCODE, EDUCNAM2 FROM MSTEDUC ) B "
                            + "    ON A.AWAIEUCD = B.EDUCCODE "
                            + " WHERE AWAISABN LIKE '%" + sabn + "%'"
                            + (DataAccess.DBtype == "1" ? " " : " AND ROWNUM <= 6 ")
                            + " ORDER BY AWAIFDAT DESC";

                DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);
                dp.AddDatatable2Dataset("TRSAWAI", dt, ref ds);

                //기본 7ROW 추가
                for (int i = 0; ds.Tables["TRSAWAI"].Rows.Count < 6; i++)
                {
                    DataRow drow = ds.Tables["TRSAWAI"].NewRow();
                    ds.Tables["TRSAWAI"].Rows.Add(drow);
                }
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //레포트 부분 - 부서이동내역
        public void GetTrsDeptDatas(string sabn, ref DataSet ds)
        {
            try
            {
                string qry = " SELECT "
                           + (DataAccess.DBtype == "1" ? "TOP(6) " : " ")
                           + " A.SAWON_NO, A.MOVE_DATE, A.FR_DEPT, A.TO_DEPT, A.REMARK, B.DEPRNAM2 AS FR_DEPTNAM, C.DEPRNAM2 AS TO_DEPTNAM "
                           + "   FROM TRSDEPT A "
                           + "   LEFT OUTER JOIN (SELECT DEPRCODE, DEPRNAM2 FROM MSTDEPR ) B "
                           + "     ON A.FR_DEPT = B.DEPRCODE "
                           + "   LEFT OUTER JOIN (SELECT DEPRCODE, DEPRNAM2 FROM MSTDEPR ) C "
                           + "     ON A.TO_DEPT = C.DEPRCODE "
                           + " WHERE SAWON_NO LIKE '%" + sabn + "%'"
                           + (DataAccess.DBtype == "1" ? " " : " AND ROWNUM <= 12 ")
                           + " ORDER BY MOVE_DATE DESC";

                DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);
                dp.AddDatatable2Dataset("TRSDEPT", dt, ref ds);

                for (int i = 0; ds.Tables["TRSDEPT"].Rows.Count < 12; i++)
                {
                    DataRow drow = ds.Tables["TRSDEPT"].NewRow();
                    ds.Tables["TRSDEPT"].Rows.Add(drow);
                }
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //레포트 부분 - 인사고과사항
        public void GetTrsEstmDatas(string sabn, ref DataSet ds)
        {
            string qry = "";
            try
            {
                if (SilkRoad.Config.SRConfig.WorkPlaceSano != "4098205251") //(주)천주의성요한수도회 이외 업체
                {
                    qry = " SELECT "
                       + (DataAccess.DBtype == "1" ? "TOP(13) * " : " * ")
                       + "  FROM TRSESTM "
                       + " WHERE ESTMSABN LIKE '%" + sabn + "%'"
                       + (DataAccess.DBtype == "1" ? " " : " AND ROWNUM <= 13 ")
                       + " ORDER BY ESTMYEAR DESC";

                    DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);
                    dp.AddDatatable2Dataset("TRSESTM", dt, ref ds);

                    for (int i = 0; ds.Tables["TRSESTM"].Rows.Count < 13; i++)
                    {
                        DataRow drow = ds.Tables["TRSESTM"].NewRow();
                        ds.Tables["TRSESTM"].Rows.Add(drow);
                    }
                }
                else
                {
                    qry = " SELECT "
                        + DataAccess.DBtype == "1" ? "TOP(5) * " : " * "
                        + "  FROM TRSESTM "
                        + " WHERE ESTMSABN LIKE '%" + sabn + "%'"
                        + DataAccess.DBtype == "1" ? " " : " AND ROWNUM <= 5 "
                        + " ORDER BY ESTMYEAR DESC";

                    DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);
                    dp.AddDatatable2Dataset("TRSESTM", dt, ref ds);

                    for (int i = 0; ds.Tables["TRSESTM"].Rows.Count < 5; i++)
                    {
                        DataRow drow = ds.Tables["TRSESTM"].NewRow();
                        ds.Tables["TRSESTM"].Rows.Add(drow);
                    }
                }
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //레포트 부분 - 발령사항
        public void GetTrsWorkDatas(string sabn, ref DataSet ds)
        {
            string qry = "";
            try
            {
                if (SilkRoad.Config.SRConfig.WorkPlaceSano != "4098205251") //(주)천주의성요한수도회 이외 업체
                {
                    string comp = string.Empty;

                    if (DataAccess.DBtype == "1")   //MSSQL
                    {
                        comp = ds.Tables["INFOBASE"].Rows[0]["IFBSNHYN"].ToString() == "1" ? "TOP(20)" : "TOP(10)";
                    }
                    else
                    {
                        comp = " ";
                    }


                    qry = " SELECT " + comp + " A.WORKSABN, A.WORKBDAY, RTRIM(A.WORKHBCD) AS HOBONG, "
                       + "       B.WORDNAM2 AS 발령구분, C.GLOVNAM2 AS 사업부, D.DEPRNAM2 AS 부서, E.SITENAM2 AS 현장, "
                       + "       F.JONGNAM2 AS 직종, G.POSINAM2 AS 직위, H.JDEFNAM2 AS 직무, I.GRADNAM2 AS 직급, J.AREANAM2 AS 근무지 "
                       + "  FROM TRSWORK A "
                       + "  LEFT OUTER JOIN (SELECT WORDCODE, WORDNAM2 FROM MSTWORD ) B "
                       + "    ON A.WORKWRCD = B.WORDCODE "
                       + "  LEFT OUTER JOIN (SELECT GLOVCODE, GLOVNAM2 FROM MSTGLOV) C "
                       + "    ON A.WORKGOCD = C.GLOVCODE "
                       + "  LEFT OUTER JOIN (SELECT DEPRCODE, DEPRNAM2 FROM MSTDEPR) D "
                       + "    ON A.WORKDPCD = D.DEPRCODE "
                       + "  LEFT OUTER JOIN (SELECT SITECODE, SITENAM2 FROM MSTSITE) E "
                       + "    ON A.WORKSTCD = E.SITECODE "
                       + "  LEFT OUTER JOIN (SELECT JONGCODE, JONGNAM2 FROM MSTJONG) F "
                       + "    ON A.WORKJNCD = F.JONGCODE "
                       + "  LEFT OUTER JOIN (SELECT POSICODE, POSINAM2 FROM MSTPOSI) G "
                       + "    ON A.WORKPSCD = G.POSICODE "
                       + "  LEFT OUTER JOIN (SELECT JDEFCODE, JDEFNAM2 FROM MSTJDEF) H "
                       + "    ON A.WORKJECD = H.JDEFCODE "
                       + "  LEFT OUTER JOIN (SELECT GRADCODE, GRADNAM2 FROM MSTGRAD) I "
                       + "    ON A.WORKGACD = I.GRADCODE "
                       + "  LEFT OUTER JOIN (SELECT AREACODE, AREANAM2 FROM MSTAREA ) J "
                       + "    ON A.WORKAECD = J.AREACODE "
                       + " WHERE WORKSABN LIKE '%" + sabn + "%'"
                       + (DataAccess.DBtype == "1" ? " " : " AND ROWNUM <= 10 ")
                       + " ORDER BY A.WORKBDAY DESC";

                    DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);
                    dp.AddDatatable2Dataset("TRSWORK", dt, ref ds);

                    //남흥계열사이면
                    if (ds.Tables["INFOBASE"].Rows[0]["IFBSNHYN"].ToString() == "1")
                    {
                        for (int i = 0; ds.Tables["TRSWORK"].Rows.Count < 20; i++)
                        {
                            DataRow drow = ds.Tables["TRSWORK"].NewRow();
                            ds.Tables["TRSWORK"].Rows.Add(drow);
                        }
                    }
                    else
                    {
                        for (int i = 0; ds.Tables["TRSWORK"].Rows.Count < 10; i++)
                        {
                            DataRow drow = ds.Tables["TRSWORK"].NewRow();
                            ds.Tables["TRSWORK"].Rows.Add(drow);
                        }
                    }
                }
                else
                {
                    qry = " SELECT TOP(5) A.WORKSABN, A.WORKBDAY, RTRIM(A.WORKHOBO) AS HOBONG, "
                       + "       B.WORDNAM2 AS 발령구분, C.GLOVNAM2 AS 사업부, D.DEPRNAM2 AS 부서, E.SITENAM2 AS 현장, "
                       + "       F.JONGNAM2 AS 직종, G.POSINAM2 AS 직위, H.JDEFNAM2 AS 직무, I.GRADNAM2 AS 직급, J.AREANAM2 AS 근무지 "
                       + "  FROM TRSWORK A "
                       + "  LEFT OUTER JOIN (SELECT WORDCODE, WORDNAM2 FROM MSTWORD ) B "
                       + "    ON A.WORKWRCD = B.WORDCODE "
                       + "  LEFT OUTER JOIN (SELECT GLOVCODE, GLOVNAM2 FROM MSTGLOV) C "
                       + "    ON A.WORKGOCD = C.GLOVCODE "
                       + "  LEFT OUTER JOIN (SELECT DEPRCODE, DEPRNAM2 FROM MSTDEPR) D "
                       + "    ON A.WORKDPCD = D.DEPRCODE "
                       + "  LEFT OUTER JOIN (SELECT SITECODE, SITENAM2 FROM MSTSITE) E "
                       + "    ON A.WORKSTCD = E.SITECODE "
                       + "  LEFT OUTER JOIN (SELECT JONGCODE, JONGNAM2 FROM MSTJONG) F "
                       + "    ON A.WORKJNCD = F.JONGCODE "
                       + "  LEFT OUTER JOIN (SELECT POSICODE, POSINAM2 FROM MSTPOSI) G "
                       + "    ON A.WORKPOSI = G.POSICODE "
                       + "  LEFT OUTER JOIN (SELECT JDEFCODE, JDEFNAM2 FROM MSTJDEF) H "
                       + "    ON A.WORKJECD = H.JDEFCODE "
                       + "  LEFT OUTER JOIN (SELECT GRADCODE, GRADNAM2 FROM MSTGRAD) I "
                       + "    ON A.WORKGACD = I.GRADCODE "
                       + "  LEFT OUTER JOIN (SELECT AREACODE, AREANAM2 FROM MSTAREA ) J "
                       + "    ON A.WORKAECD = J.AREACODE "
                       + " WHERE WORKSABN LIKE '%" + sabn + "%'"
                       + " ORDER BY A.WORKBDAY DESC";

                    DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);
                    dp.AddDatatable2Dataset("TRSWORK", dt, ref ds);

                    for (int i = 0; ds.Tables["TRSWORK"].Rows.Count < 5; i++)
                    {
                        DataRow drow = ds.Tables["TRSWORK"].NewRow();
                        ds.Tables["TRSWORK"].Rows.Add(drow);
                    }
                }
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region PAGE2_2

        //레포트 부분 - 해외연수
        public void GetTrsTraiDatas(string sabn, ref DataSet ds)
        {
            try
            {
                string qry = " SELECT "
                           + (DataAccess.DBtype == "1" ? "TOP(4) * " : " * ")
                           + "   FROM TRSTRAI"
                           + "  WHERE TRAISABN LIKE '%" + sabn + "%'"
                           + (DataAccess.DBtype == "1" ? " " : " AND ROWNUM <= 4 ")
                           + "  ORDER BY TRAIYEAR DESC";

                DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);
                dp.AddDatatable2Dataset("TRSTRAI", dt, ref ds);

                //기본 4ROW 추가
                for (int i = 0; ds.Tables["TRSTRAI"].Rows.Count < 4; i++)
                {
                    DataRow drow = ds.Tables["TRSTRAI"].NewRow();
                    ds.Tables["TRSTRAI"].Rows.Add(drow);
                }
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //레포트 부분 - 승진급관리
        public void GetTrsUpgrDatas(string sabn, ref DataSet ds)
        {
            try
            {
                string qry = " SELECT "
                           + (DataAccess.DBtype == "1" ? "TOP(4) * " : " * ")
                           + "   FROM TRSUPGR"
                           + "  WHERE UPGRSABN LIKE '%" + sabn + "%'"
                           + (DataAccess.DBtype == "1" ? " " : " AND ROWNUM <= 4 ")
                           + "  ORDER BY UPGRBDAY DESC";

                DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);
                dp.AddDatatable2Dataset("TRSUPGR", dt, ref ds);

                for (int i = 0; ds.Tables["TRSUPGR"].Rows.Count < 4; i++)
                {
                    DataRow drow = ds.Tables["TRSUPGR"].NewRow();
                    ds.Tables["TRSUPGR"].Rows.Add(drow);
                }
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //레포트 부분 - 휴직관리
        public void GetTrsHjshDatas(string sabn, ref DataSet ds)
        {
            try
            {
                string qry = " SELECT "
                            + (DataAccess.DBtype == "1" ? "TOP(5) * " : " * ")
                            + "  FROM TRSHJSH"
                            + " WHERE HJSHSABN LIKE '%" + sabn + "%'"
                            + (DataAccess.DBtype == "1" ? " " : " AND ROWNUM <= 5 ")
                            + " ORDER BY HJSHFDAY DESC";

                DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);
                dp.AddDatatable2Dataset("TRSHJSH", dt, ref ds);

                //기본 20ROW 추가
                for (int i = 0; ds.Tables["TRSHJSH"].Rows.Count < 5; i++)
                {
                    DataRow drow = ds.Tables["TRSHJSH"].NewRow();
                    ds.Tables["TRSHJSH"].Rows.Add(drow);
                }
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //레포트 부분 - 병가관리
        public void GetTrsBgshDatas(string sabn, ref DataSet ds)
        {
            try
            {
                string qry = " SELECT "
                            + (DataAccess.DBtype == "1" ? "TOP(5) * " : " * ")
                            + "  FROM TRSBGSH"
                            + " WHERE BGSHSABN LIKE '%" + sabn + "%'"
                            + (DataAccess.DBtype == "1" ? " " : " AND ROWNUM <= 5 ")
                            + " ORDER BY BGSHFDAY DESC";

                DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);
                dp.AddDatatable2Dataset("TRSBGSH", dt, ref ds);

                for (int i = 0; ds.Tables["TRSBGSH"].Rows.Count < 5; i++)
                {
                    DataRow drow = ds.Tables["TRSBGSH"].NewRow();
                    ds.Tables["TRSBGSH"].Rows.Add(drow);
                }
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //레포트 부분 - 지인사항 (남흥계열사 전용)
        public void GetTrsFrndDatas(string sabn, ref DataSet ds)
        {
            try
            {
                string qry = " SELECT * "
                           + "  FROM TRSFRND"
                           + " WHERE FRNDSABN LIKE '%" + sabn + "%'"
                           + " ORDER BY FRNDSABN, FRNDSQNO ";

                DataTable dt = gd.GetDataInQuery(Convert.ToInt32(DataAccess.DBtype), wagedb, qry);
                dp.AddDatatable2Dataset("TRSFRND", dt, ref ds);

                //기본 30ROW 추가
                for (int i = 0; ds.Tables["TRSFRND"].Rows.Count < 30; i++)
                {
                    DataRow drow = ds.Tables["TRSFRND"].NewRow();
                    ds.Tables["TRSFRND"].Rows.Add(drow);
                }
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion



        #region 3020 - 근무마감설정

        //부서리스트
        public void GetSET_DEPTDatas(string code, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.DEPRCODE CODE, RTRIM(A.DEPRNAM1) NAME "
						   + "   FROM " + wagedb + ".dbo.MSTDEPR A "
						   + "  INNER JOIN DUTY_INFONURS X1 ON A.DEPRCODE=X1.DEPTCODE "
						   + "  WHERE A.DEPRCODE LIKE '" + code + "' AND A.DEPRSTAT=1 "
						   + "  ORDER BY A.DEPRCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SET_DEPT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//년월
		public void GetSEARCH_CLOSDatas(string year, string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, RTRIM(X1.DEPRNAM1) DEPT_NM, "
						   + "        LEFT(A.PLANYYMM,4)+'-'+SUBSTRING(A.PLANYYMM,5,2) YYMM_NM, "
						   + "        LEFT(A.POS_FRDT,4)+'/'+SUBSTRING(A.POS_FRDT,5,2)+'/'+SUBSTRING(A.POS_FRDT,7,2)+' ~ '+ "
						   + "        LEFT(A.POS_TODT,4)+'/'+SUBSTRING(A.POS_TODT,5,2)+'/'+SUBSTRING(A.POS_TODT,7,2) AS FRTO_NM, "
						   + "        (CASE WHEN A.CLOSE_YN='Y' THEN '신청마감' ELSE '신청중' END) CLOSE_NM "
						   + "   FROM DUTY_MSTCLOS A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 ON A.DEPTCODE=X1.DEPRCODE "
						   + "  WHERE LEFT(A.PLANYYMM,4) = '" + year +"' "
						   + "    AND A.DEPTCODE = '" + dept +"' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_CLOS", dt, ref ds);
				
                for (int i = 1; i <= 12; i++)
                {
                    if (ds.Tables["SEARCH_CLOS"].Select("PLANYYMM = '" + year.Substring(0, 4) + i.ToString().PadLeft(2, '0') + "'").Length == 0)
                    {
                        DataRow drow = ds.Tables["SEARCH_CLOS"].NewRow();
						drow["PLANYYMM"] = year + i.ToString().PadLeft(2, '0');
						drow["YYMM_NM"] = year + "-" + i.ToString().PadLeft(2, '0');
                        drow["FRTO_NM"] = "";
                        drow["CLOSE_NM"] = "";
                        ds.Tables["SEARCH_CLOS"].Rows.Add(drow);
                    }
                }
                ds.Tables["SEARCH_CLOS"].DefaultView.Sort = "PLANYYMM";
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//부서리스트
		public void GetDUTY_MSTCLOSDatas(string dept, string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTCLOS "
						   + "  WHERE DEPTCODE = '" + dept + "' AND PLANYYMM = '" + yymm + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_MSTCLOS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		#endregion				
		
		#region 303X - 연차미사용등록
		
		//연차내역처리
		public void GetS_DUTY_TRSYCMIDatas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT '" + yymm.Substring(0, 6) + "' AS YYMM, "
						   + "        '" + yymm.Substring(0, 4) + "-" + yymm.Substring(4, 2) +"' AS YYMM_NM, "
						   + "        RTRIM(A.EMBSSABN) AS SABN, "
						   + "        RTRIM(A.EMBSNAME) AS SABN_NM, "
						   + "        RTRIM(ISNULL(X1.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        ISNULL(X2.YC_T_CNT,0) AS YC_T_CNT, "
						   + "        ISNULL(X2.YC_BF_SUM_CNT,0) AS YC_BF_SUM_CNT, "
						   + "        ISNULL(X2.YC_THIS_YCNT,0) AS YC_THIS_YCNT, "
						   + "        ISNULL(X2.YC_THIS_BCNT,0) AS YC_THIS_BCNT, "
						   + "        ISNULL(X2.YC_NOW_SUM_CNT,0) AS YC_NOW_SUM_CNT, "
						   + "        ISNULL(X2.YC_REMAIN_CNT,0) AS YC_REMAIN_CNT, "
						   + "        ISNULL(X2.YC_MI_CNT,0) AS YC_MI_CNT "
						   + "   FROM " + wagedb + ".dbo.MSTEMBS A "
						   + "   LEFT OUTER JOIN MSTDEPR X1 ON A.EMBSDPCD=X1.DEPRCODE "
						   + "   LEFT OUTER JOIN DUTY_TRSYCMI X2 ON A.EMBSSABN=X2.SABN "
						   + "    AND X2.YYMM = '" + yymm.Substring(0, 6) + "' "
						   + "  ORDER BY A.EMBSJOCD, A.EMBSDPCD, A.EMBSSABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("S_DUTY_TRSYCMI", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//전월N수당조회
		public void GetBF_DUTY_TRSYCMIDatas(string bfmm, DataSet ds)
		{
			try
			{
				string qry = " SELECT '" + bfmm.Substring(0, 6) + "' AS YYMM, "
						   + "        '" + bfmm.Substring(0, 4) + "-" + bfmm.Substring(4, 2) +"' AS YYMM_NM, "
						   + "        RTRIM(X1.EMBSSABN) AS SABN, "
						   + "        RTRIM(X1.EMBSNAME) AS SABN_NM, "
						   + "        RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        A.YC_T_CNT, A.YC_BF_SUM_CNT, A.YC_THIS_YCNT, A.YC_THIS_BCNT, "
						   + "        A.YC_NOW_SUM_CNT, A.YC_REMAIN_CNT, A.YC_MI_CNT "
						   + "   FROM DUTY_TRSYCMI A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON A.SABN=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE A.YYMM = '" + bfmm.Substring(0, 6) + "' "
						   + "  ORDER BY X1.EMBSJOCD, X1.EMBSDPCD, X1.EMBSSABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("BF_DUTY_TRSYCMI", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//연차내역 불러오기
		public void GetDUTY_TRSYCMIDatas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_TRSYCMI WHERE YYMM = '" + yymm + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSYCMI", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//내역조회
		public void GetSEARCH_DUTY_TRSYCMIDatas(string frmm, string tomm, DataSet ds)
		{
			try
			{
				string qry = " SELECT RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        LEFT(A.YYMM,4)+'-'+SUBSTRING(A.YYMM,5,2) AS YYMM_NM, "
						   + "        A.* "
						   + "   FROM DUTY_TRSYCMI A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON A.SABN=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE A.YYMM BETWEEN '" + frmm + "' AND '" + tomm + "' "
						   + "  ORDER BY X1.EMBSJOCD, X1.EMBSDPCD ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_DUTY_TRSYCMI", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		#endregion
					

						
		#region 3080 - 근무집계표
		
		//부서코드 lookup
		public void GetSEARCH_EMBSDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT RTRIM(EMBSSABN) CODE, RTRIM(EMBSNAME) NAME "
						   + "   FROM MSTEMBS "
						   + "  ORDER BY EMBSSABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_EMBS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//급여처리
		public void GetSEARCH_3080Datas(string yymm, string sabn, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, "
						   + "        A.WGPCGT01+A.WGPCGT03 AS CALL_TCNT, "
						   + "        A.WGPCGT02+A.WGPCGT04 AS CALL_TTIME, "
						   + "        A.WGPCGT05+A.WGPCGT06 AS DANG_TTIME, "
						   + "        A.WGPCGT11+A.WGPCGT12 AS OT_TTIME, "
						   + "        RTRIM(X1.EMBSNAME) SAWON_NM, RTRIM(X2.DEPRNAM1) DEPT_NM, "
						   + "        LEFT(A.END_YYMM,4)+'-'+SUBSTRING(A.END_YYMM,5,2) END_YYMM_NM, "
						   + "        LEFT(A.YYMM,4)+'-'+SUBSTRING(A.YYMM,5,2) YYMM_NM, "
						   + "        '' SEND_YN "
						   + "   FROM DUTY_MSTWGPC A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON A.SAWON_NO=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "   LEFT OUTER JOIN MSTWGPC W1 ON A.END_YYMM=W1.WGPCYYMM AND W1.WGPCSQNO='1' AND A.SAWON_NO=W1.WGPCSABN "
						   + "   LEFT OUTER JOIN MSTGTMM W2 ON A.END_YYMM=W2.GTMMYYMM AND A.SAWON_NO=W2.GTMMSABN "
						   + "  WHERE A.END_YYMM = '" + yymm + "' AND A.SAWON_NO LIKE '" + sabn + "'"
						   + "  ORDER BY A.END_YYMM, A.YYMM, X1.EMBSDPCD, A.SAWON_NO ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_3080", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//조회
		public void GetSEARCH_3081Datas(string frmm, string tomm, string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, "
						   + "        A.WGPCGT01+A.WGPCGT03 AS CALL_TCNT, "
						   + "        A.WGPCGT02+A.WGPCGT04 AS CALL_TTIME, "
						   + "        A.WGPCGT05+A.WGPCGT06 AS DANG_TTIME, "
						   + "        A.WGPCGT11+A.WGPCGT12 AS OT_TTIME, "
						   + "        RTRIM(X1.EMBSNAME) SAWON_NM, RTRIM(X2.DEPRNAM1) DEPT_NM, "
						   + "        LEFT(A.END_YYMM,4)+'-'+SUBSTRING(A.END_YYMM,5,2) END_YYMM_NM, "
						   + "        LEFT(A.YYMM,4)+'-'+SUBSTRING(A.YYMM,5,2) YYMM_NM "
						   + "   FROM DUTY_MSTWGPC A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X1 ON A.SAWON_NO=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE A.END_YYMM BETWEEN '" + frmm + "' AND '" + tomm + "' AND X1.EMBSDPCD LIKE '" + dept + "'"
						   + "  ORDER BY A.END_YYMM, A.YYMM, X1.EMBSDPCD, A.SAWON_NO ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_3081", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//근태조회 -> 수당조회
		public void GetSEARCH_INFOWAGEDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM " + wagedb + ".dbo.INFOWAGE A "
						   + "  ORDER BY A.IFWGCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_INFOWAGE", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//수당산식관리 조회
		public void GetSEARCH_INFOSD02Datas(DataSet ds)
		{
			try
			{
				string qry = " SELECT *, 'A015' AS A013, 'A016' AS A014 "
						   + "   FROM DUTY_INFOSD02 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_INFOSD02", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//처리
		public void GetWORK_3080Datas(string yymm, string gubn, DataSet ds)
		{
			try
			{
				string qry = "";
				//qry = " EXEC USP_DUTY3080_PRC_220523 '" + yymm + "', " + gubn + " ";
				//qry = " EXEC USP_DUTY3080_PRC_221013 '" + yymm + "', " + gubn + " ";
				qry = " EXEC USP_DUTY3080_PRC_230109 '" + yymm + "', " + gubn + " ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("WORK_3080", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//마감 테이블 구조
		public void GetS_DUTY_MSTWGPCDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT *, 0 GUBN "
						   + "   FROM DUTY_MSTWGPC "
						   + "  WHERE 1=2 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("S_DUTY_MSTWGPC", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//마감 테이블 불러오기
		public void GetDUTY_MSTWGPCDatas(string end_yymm, string yymm, string sabn, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM DUTY_MSTWGPC A "
						   + "  WHERE A.END_YYMM = '" + end_yymm + "' AND A.YYMM LIKE '" + yymm + "' "
						   + "    AND A.SAWON_NO LIKE '" + sabn + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_MSTWGPC", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//마감 테이블 불러오기
		public void GetDUTY_MSTWGPC_ENDDatas(string end_yymm, string yymm, string sabn, string gubn, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM DUTY_MSTWGPC_END A "
						   + "  WHERE A.END_YYMM = '" + end_yymm + "' AND A.YYMM = '" + yymm + "' "
						   + "    AND A.SAWON_NO = '" + sabn + "' AND A.GUBN = " + gubn + "";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_MSTWGPC_END", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		
		//인사급여 마감 데이터
		public void Get3080_WAGE_ENDSDatas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT CLSEYN" + yymm.Substring(4, 2) + " AS MM_CHK, "
						   + "        CLSEYN00 AS YY_CHK"
						   + "   FROM MSTCLSE "
						   + "  WHERE CLSEYEAR = '" + yymm.Substring(0, 4) + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), wagedb, qry);
				dp.AddDatatable2Dataset("3080_WAGE_ENDS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//인사급여 테이블 불러오기
		public void GetMSTWGPCDatas(string yymm, string sabn, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM MSTWGPC "
						   + "  WHERE WGPCYYMM = '" + yymm.Substring(0, 6) + "' AND WGPCSABN LIKE '" + sabn + "' AND WGPCSQNO='1'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("MSTWGPC", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//인사급여 테이블 불러오기
		public void GetMSTGTMMDatas(string yymm, string sabn, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM MSTGTMM "
						   + "  WHERE GTMMYYMM = '" + yymm.Substring(0, 6) + "' AND GTMMSABN LIKE '" + sabn + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("MSTGTMM", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region 3090 - 최종마감설정
				
		//최종마감현황 조회
		public void GetSEARCH_ENDSDatas(string year, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, "
						   + "        LEFT(A.END_YYMM,4)+'-'+SUBSTRING(A.END_YYMM,5,2) YYMM_NM, "
						   + "        (CASE A.CLOSE_YN WHEN 'Y' THEN '마감완료' WHEN 'N' THEN '마감취소' ELSE '' END) CLOSE_NM, "
						   + "        (CASE WHEN A.CLOSE_YN='Y' THEN A.END_DT+' (' +A.END_ID+')' "
						   + "              WHEN A.CLOSE_YN='N' THEN A.CANC_DT+' (' +A.CANC_ID+')' ELSE '' END) REMARK "
						   + "   FROM DUTY_MSTENDS A "
						   + "  WHERE LEFT(A.END_YYMM,4) = '" + year + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_ENDS", dt, ref ds);
				
                for (int i = 1; i <= 12; i++)
                {
                    if (ds.Tables["SEARCH_ENDS"].Select("END_YYMM = '" + year.Substring(0, 4) + i.ToString().PadLeft(2, '0') + "'").Length == 0)
                    {
                        DataRow drow = ds.Tables["SEARCH_ENDS"].NewRow();
						drow["END_YYMM"] = year + i.ToString().PadLeft(2, '0');
						drow["YYMM_NM"] = year + "-" + i.ToString().PadLeft(2, '0');
                        drow["CLOSE_NM"] = "";
                        drow["REMARK"] = "";
                        ds.Tables["SEARCH_ENDS"].Rows.Add(drow);
                    }
                }
                ds.Tables["SEARCH_ENDS"].DefaultView.Sort = "END_YYMM";
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//저장시 해당년월 마감조회
		public void GetDUTY_MSTENDSDatas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTENDS "
						   + "  WHERE END_YYMM = '" + yymm + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_MSTENDS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//마감로그조회
		public void GetSEARCH_ENDS_LOGDatas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, "
						   + "        LEFT(A.END_YYMM,4)+'-'+SUBSTRING(A.END_YYMM,5,2) YYMM_NM, "
						   + "        (CASE A.CLOSE_YN WHEN 'Y' THEN '마감완료' WHEN 'N' THEN '마감취소' ELSE '' END) CLOSE_NM, "
						   + "        A.REG_DT+' (' +A.REG_ID+')' AS REMARK "
						   + "   FROM DUTY_MSTENDS_LOG A "
						   + "  WHERE A.END_YYMM = '" + yymm +"'"
						   + "  ORDER BY A.REG_DT ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_ENDS_LOG", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//마감로그 테이블구조
		public void GetDUTY_MSTENDS_LOGDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTENDS_LOG "
						   + "  WHERE 1=2 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_MSTENDS_LOG", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region 5010 - KT근태연동조회
		
		//출퇴근조회
		public void GetSEARCH_KT1Datas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT A1.* FROM ("
						   + " SELECT A.USERID, RIGHT(A.USERID,5) as SABN, X3.USERNAME, RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        SUBSTRING(CONVERT(VARCHAR,WORKSTART,112),5,2)+'/'+SUBSTRING(CONVERT(VARCHAR,WORKSTART,112),7,2) AS SLDT, 1 as ACC_TIME, "
						   + "        RIGHT(A.USERID,5)+SUBSTRING(CONVERT(VARCHAR,WORKSTART,112),5,2)+'/'+SUBSTRING(CONVERT(VARCHAR,WORKSTART,112),7,2) AS CHK_ROW, "
						   + "        LEFT(CONVERT(VARCHAR,WORKSTART,112),4)+'-'+SUBSTRING(CONVERT(VARCHAR,WORKSTART,112),5,2)+'-'+SUBSTRING(CONVERT(VARCHAR,WORKSTART,112),7,2) SLDT_NM, "
						   + "        CONVERT(VARCHAR,A.WORKSTART,120) AS ACCESSDATE, "
						   + "        '출근' AS GUBN_NM, "
						   + "        (CASE WHEN X1.EMBSSABN IS NULL THEN '1' ELSE '' END) EMBS_STAT "
						   + "   FROM TB_WORKRESULT A LEFT OUTER JOIN TB_USER X3 ON A.USERID=X3.USERID "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON RIGHT(A.USERID,5)=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE CONVERT(VARCHAR,A.WORKSTART,112) LIKE '" + yymm + "%' "
						   + "    AND SUBSTRING(A.USERID,8,1)='9' AND A.WORKSTART IS NOT NULL "
						   + " UNION ALL "
				           + " SELECT A.USERID, RIGHT(A.USERID,5) as SABN, X3.USERNAME, RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        SUBSTRING(CONVERT(VARCHAR,WORKEND,112),5,2)+'/'+SUBSTRING(CONVERT(VARCHAR,WORKEND,112),7,2) AS SLDT, 1 as ACC_TIME, "
						   + "        RIGHT(A.USERID,5)+SUBSTRING(CONVERT(VARCHAR,WORKEND,112),5,2)+'/'+SUBSTRING(CONVERT(VARCHAR,WORKEND,112),7,2) AS CHK_ROW, "
						   + "        LEFT(CONVERT(VARCHAR,WORKEND,112),4)+'-'+SUBSTRING(CONVERT(VARCHAR,WORKEND,112),5,2)+'-'+SUBSTRING(CONVERT(VARCHAR,WORKEND,112),7,2) SLDT_NM, "
						   + "        CONVERT(VARCHAR,A.WORKEND,120) AS ACCESSDATE, "
						   + "        '퇴근' AS GUBN_NM, "
						   + "        (CASE WHEN X1.EMBSSABN IS NULL THEN '1' ELSE '' END) EMBS_STAT "
						   + "   FROM TB_WORKRESULT A LEFT OUTER JOIN TB_USER X3 ON A.USERID=X3.USERID "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON RIGHT(A.USERID,5)=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE CONVERT(VARCHAR,A.WORKEND,112) LIKE '" + yymm + "%' "
						   + "    AND SUBSTRING(A.USERID,8,1)='9' AND A.WORKEND IS NOT NULL "
						   + " ) A1 "
						   + "  ORDER BY A1.SABN, A1.ACCESSDATE ";
				//string qry = " SELECT A.USERID, RIGHT(A.USERID,5) as SABN, A.USERNAME, A.DEPARTMENTNAME AS DEPT_NM, " //RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM, "
				//		   + "        SUBSTRING(CONVERT(VARCHAR,ACCESSDATE,112),5,2)+'/'+SUBSTRING(CONVERT(VARCHAR,ACCESSDATE,112),7,2) AS SLDT, 1 as ACC_TIME, "
				//		   + "        RIGHT(A.USERID,5)+SUBSTRING(CONVERT(VARCHAR,ACCESSDATE,112),5,2)+'/'+SUBSTRING(CONVERT(VARCHAR,ACCESSDATE,112),7,2) AS CHK_ROW, "
				//		   + "        LEFT(CONVERT(VARCHAR,ACCESSDATE,112),4)+'-'+SUBSTRING(CONVERT(VARCHAR,ACCESSDATE,112),5,2)+'-'+SUBSTRING(CONVERT(VARCHAR,ACCESSDATE,112),7,2) SLDT_NM, "
				//		   + "        CONVERT(VARCHAR,A.ACCESSDATE,120) AS ACCESSDATE, A.DEVICENAME, A.AUTHMODE, "
				//		   + "        (CASE A.AUTHMODE WHEN '82' THEN '출근' WHEN '83' THEN '퇴근' ELSE '' END) GUBN_NM, "
				//		   + "        (CASE WHEN X1.EMBSSABN IS NULL THEN '1' ELSE '' END) EMBS_STAT "
				//		   + "   FROM TB_ACCESS A "
				//		   + "   LEFT OUTER JOIN MSTEMBS X1 ON RIGHT(A.USERID,5)=X1.EMBSSABN "
				//		   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
				//		   + "  WHERE CONVERT(VARCHAR,A.ACCESSDATE,112) LIKE '" + yymm + "%' "
				//		   + "    AND SUBSTRING(A.USERID,8,1)='9' AND A.AUTHMODE IN ('82','83') AND A.AUTHMODE1 IN ('0')  "
				//		   + "  ORDER BY RIGHT(A.USERID,5), A.ACCESSDATE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_KT1", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//식수조회
		public void GetSEARCH_KT2Datas(string frdt, string todt, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.USERID, RIGHT(A.USERID,5) as SABN, A.USERNAME, A.DEPARTMENTNAME AS DEPT_NM, " 
						   + "        LEFT(CONVERT(VARCHAR,ACCE_DATE,112),4)+'-'+SUBSTRING(CONVERT(VARCHAR,ACCE_DATE,112),5,2)+'-'+SUBSTRING(CONVERT(VARCHAR,ACCE_DATE,112),7,2) SLDT_NM, "
						   + "        CONVERT(VARCHAR,A.ACCE_DATE,120) AS ACCESSDATE, "
						   + "        (CASE WHEN X1.EMBSSABN IS NULL THEN '1' ELSE '' END) EMBS_STAT "
						   + "   FROM TB_FOODRESULT A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON RIGHT(A.USERID,5)=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE CONVERT(VARCHAR,A.ACCE_DATE,112) BETWEEN '" + frdt + "' AND '" + todt + "'"
						   + "    AND SUBSTRING(A.USERID,8,1)='9' "
						   + "  ORDER BY RIGHT(A.USERID,5), A.ACCE_DATE ";
				//string qry = " SELECT A.USERID, RIGHT(A.USERID,5) as SABN, A.USERNAME, A.DEPARTMENTNAME AS DEPT_NM, " //RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM, "
				//		   + "        LEFT(CONVERT(VARCHAR,ACCESSDATE,112),4)+'-'+SUBSTRING(CONVERT(VARCHAR,ACCESSDATE,112),5,2)+'-'+SUBSTRING(CONVERT(VARCHAR,ACCESSDATE,112),7,2) SLDT_NM, "
				//		   + "        CONVERT(VARCHAR,A.ACCESSDATE,120) AS ACCESSDATE, "
				//		   + "        (CASE A.AUTHMODE1 WHEN '2' THEN '조식' WHEN '3' THEN '중식' WHEN '4' THEN '석식' ELSE '' END) GUBN_NM, "
				//		   + "        (CASE WHEN X1.EMBSSABN IS NULL THEN '1' ELSE '' END) EMBS_STAT "
				//		   + "   FROM TB_ACCESS A "
				//		   + "   LEFT OUTER JOIN MSTEMBS X1 ON RIGHT(A.USERID,5)=X1.EMBSSABN "
				//		   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
				//		   + "  WHERE CONVERT(VARCHAR,A.ACCESSDATE,112) BETWEEN '" + frdt + "' AND '" + todt + "'"
				//		   + "    AND SUBSTRING(A.USERID,8,1)='9' AND A.AUTHMODE1 IN ('2','3','4')   "
				//		   + "  ORDER BY RIGHT(A.USERID,5), A.ACCESSDATE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_KT2", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//근무신청내역조회
		public void Get5010_SEARCH3Datas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT A1.* FROM ( "
						   + " SELECT A.SAWON_NO, A.DEPTCODE, A.PLAN_SQ, "
						   + "        RTRIM(X1.EMBSNAME) SAWON_NM, RTRIM(X2.DEPRNAM1) DEPT_NM, "
						   + "        A.D01, A.D02, A.D03, A.D04, A.D05, A.D06, A.D07, A.D08, A.D09, A.D10, "
						   + "        A.D11, A.D12, A.D13, A.D14, A.D15, A.D16, A.D17, A.D18, A.D19, A.D20, "
						   + "        A.D21, A.D22, A.D23, A.D24, A.D25, A.D26, A.D27, A.D28, A.D29, A.D30, A.D31, "
						   + "        '' D01_NM, '' D02_NM, '' D03_NM, '' D04_NM, '' D05_NM, '' D06_NM, '' D07_NM, '' D08_NM, '' D09_NM, '' D10_NM, "
						   + "        '' D11_NM, '' D12_NM, '' D13_NM, '' D14_NM, '' D15_NM, '' D16_NM, '' D17_NM, '' D18_NM, '' D19_NM, '' D20_NM, "
						   + "        '' D21_NM, '' D22_NM, '' D23_NM, '' D24_NM, '' D25_NM, '' D26_NM, '' D27_NM, '' D28_NM, '' D29_NM, '' D30_NM, '' D31_NM "
						   + "   FROM DUTY_TRSDANG A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 "
						   + "     ON A.SAWON_NO = X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 "
						   + "     ON A.DEPTCODE = X2.DEPRCODE "
						   + "  WHERE A.PLANYYMM = '" + yymm + "' "
						   + "  UNION ALL "
						   + " SELECT A.SAWON_NO, A.DEPTCODE, A.PLAN_SQ, "
						   + "        RTRIM(X1.SAWON_NM) SAWON_NM, RTRIM(X2.DEPRNAM1) DEPT_NM, "
						   + "        A.D01, A.D02, A.D03, A.D04, A.D05, A.D06, A.D07, A.D08, A.D09, A.D10, "
						   + "        A.D11, A.D12, A.D13, A.D14, A.D15, A.D16, A.D17, A.D18, A.D19, A.D20, "
						   + "        A.D21, A.D22, A.D23, A.D24, A.D25, A.D26, A.D27, A.D28, A.D29, A.D30, A.D31, "
						   + "        '' D01_NM, '' D02_NM, '' D03_NM, '' D04_NM, '' D05_NM, '' D06_NM, '' D07_NM, '' D08_NM, '' D09_NM, '' D10_NM, "
						   + "        '' D11_NM, '' D12_NM, '' D13_NM, '' D14_NM, '' D15_NM, '' D16_NM, '' D17_NM, '' D18_NM, '' D19_NM, '' D20_NM, "
						   + "        '' D21_NM, '' D22_NM, '' D23_NM, '' D24_NM, '' D25_NM, '' D26_NM, '' D27_NM, '' D28_NM, '' D29_NM, '' D30_NM, '' D31_NM "
						   + "   FROM DUTY_TRSPLAN A "
						   + "   LEFT OUTER JOIN DUTY_MSTNURS X1 "
						   + "     ON A.SAWON_NO = X1.SAWON_NO "
						   + "   LEFT OUTER JOIN MSTDEPR X2 "
						   + "     ON A.DEPTCODE = X2.DEPRCODE "
						   + "  WHERE A.PLANYYMM = '" + yymm + "' "
						   + " ) A1 "
						   + "  ORDER BY A1.DEPTCODE, A1.PLAN_SQ ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("5010_SEARCH3", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//출근내역 조회
		public void Get5010_SEARCH3_KTDatas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.USERID, RIGHT(A.USERID,5) as SABN, X1.USERNAME, "
						   + "        LEFT(CONVERT(VARCHAR,WORKSTART,112),8) AS SLDT "
						   + "   FROM TB_WORKRESULT A LEFT OUTER JOIN TB_USER X1 ON A.USERID=X1.USERID "
						   + "  WHERE CONVERT(VARCHAR,A.WORKSTART,112) LIKE '" + yymm + "%' "
						   + "    AND SUBSTRING(A.USERID,8,1)='9' AND A.WORKSTART IS NOT NULL  "
						   + "  ORDER BY RIGHT(A.USERID,5), A.WORKSTART ";
				//string qry = " SELECT A.USERID, RIGHT(A.USERID,5) as SABN, A.USERNAME, "
				//		   + "        LEFT(CONVERT(VARCHAR,ACCESSDATE,112),8) AS SLDT "
				//		   + "   FROM TB_ACCESS A "
				//		   + "   LEFT OUTER JOIN MSTEMBS X1 ON RIGHT(A.USERID,5)=X1.EMBSSABN "
				//		   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
				//		   + "  WHERE CONVERT(VARCHAR,A.ACCESSDATE,112) LIKE '" + yymm + "%' "
				//		   + "    AND SUBSTRING(A.USERID,8,1)='9' AND A.AUTHMODE IN ('82') AND A.AUTHMODE1 IN ('0')  "
				//		   + "  ORDER BY RIGHT(A.USERID,5), A.ACCESSDATE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("5010_SEARCH3_KT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		#endregion		

		#region 3030 - 연차정산관리
		
		//연차정산대상자 조회
		public void GetSEARCH_LAST_YCDatas(string yymm, DataSet ds)
		{
			try
			{
				string last_date = clib.DateToText(clib.TextToDate(yymm + "01").AddMonths(1).AddDays(-1));
				string qry = " SELECT '" + yymm.Substring(0, 6) + "' AS YYMM, "
						   + "        '" + yymm.Substring(0, 4) + "-" + yymm.Substring(4, 2) +"' AS YYMM_NM, "
						   + "        RTRIM(A.EMBSSABN) AS SABN, RTRIM(A.EMBSNAME) AS SABN_NM, "
						   + "        RTRIM(ISNULL(X1.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        X2.YC_YEAR, '" + last_date +"' AS USE_TODT, "
						   + "        0.0 AS YC_SUM, 0.0 AS YC_CHANGE, 0.0 AS YC_TOTAL, "
						   + "        0.0 AS YC_USE, 0.0 AS YC_REMAIN, 0.0 AS YC_MI_CNT "
						   + "   FROM " + wagedb + ".dbo.MSTEMBS A "
						   + "   LEFT OUTER JOIN MSTDEPR X1 ON A.EMBSDPCD=X1.DEPRCODE "
						   + "   LEFT OUTER JOIN DUTY_TRSDYYC X2 ON A.EMBSSABN=X2.SAWON_NO "
						   + "    AND '" + last_date +"' BETWEEN X2.USE_FRDT AND X2.USE_TODT"
						   + "  WHERE A.EMBSSTAT=1 "
						   + "  ORDER BY A.EMBSJOCD, A.EMBSDPCD, A.EMBSSABN ";
				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_LAST_YC", dt, ref ds);

				string qry1 = " SELECT '" + yymm.Substring(0, 6) + "' AS YYMM, "
						   + "        '" + yymm.Substring(0, 4) + "-" + yymm.Substring(4, 2) +"' AS YYMM_NM, "
						   + "        A.SAWON_NO AS SABN, RTRIM(X1.EMBSNAME) AS SABN_NM, "
						   + "        RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        A.YC_YEAR, " //A.USE_TODT, X1.EMBSTSDT, "
						   + "        (CASE WHEN X1.EMBSSTAT=2 AND LEFT(X1.EMBSTSDT,6) = '" + yymm + "' THEN X1.EMBSTSDT "
						   + "              WHEN X1.EMBSSTAT=1 AND LEFT(A.USE_TODT,6) = '" + yymm + "' THEN A.USE_TODT ELSE '' END) AS USE_TODT, "
						   + "        A.YC_BASE+A.YC_FIRST+A.YC_ADD AS YC_SUM, A.YC_CHANGE, A.YC_TOTAL, "
						   + "        SUM(ISNULL(X3.YC_DAYS,0)) AS YC_USE, "
						   + "        A.YC_TOTAL - SUM(ISNULL(X3.YC_DAYS,0)) AS YC_REMAIN, "
						   + "        A.YC_TOTAL - SUM(ISNULL(X3.YC_DAYS,0)) AS YC_MI_CNT "
						   + "   FROM DUTY_TRSDYYC A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON A.SAWON_NO=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "   LEFT OUTER JOIN DUTY_TRSHREQ X3 ON A.YC_YEAR=X3.REQ_YEAR AND A.SAWON_NO=X3.SABN AND X3.AP_TAG NOT IN ('5') "  //23.01.13 반려는 카운트 제외
						   + "  WHERE (X1.EMBSSTAT=1 AND LEFT(A.USE_TODT,6) = '" + yymm + "')"
						   + "     OR (X1.EMBSSTAT=2 AND LEFT(X1.EMBSTSDT,6) = '" + yymm + "' AND '" + last_date +"' BETWEEN A.USE_FRDT AND A.USE_TODT) "
						   + "  GROUP BY A.SAWON_NO, X1.EMBSNAME, X2.DEPRNAM1, A.YC_YEAR, X1.EMBSSTAT, A.USE_TODT, X1.EMBSTSDT, "
						   + "           A.YC_BASE+A.YC_FIRST+A.YC_ADD, A.YC_CHANGE, A.YC_TOTAL "
						   //+ "  HAVING A.YC_TOTAL - SUM(ISNULL(X3.YC_DAYS,0)) <> 0 "
						   + "  ORDER BY X2.DEPRNAM1, A.SAWON_NO ";

				dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry1);
				dp.AddDatatable2Dataset("BASE_LAST_YC", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//저장시 기존 연차정산내역 조회
		public void GetDUTY_TRSHREQ_JSDatas(string sabn, string sldt, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM DUTY_TRSHREQ A "
						   + "  WHERE A.SABN = '" + sabn + "'"
						   + "    AND A.REQ_DATE = '" + sldt + "' AND A.AP_TAG='9' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSHREQ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//저장내역 조회
		public void GetDEL_DUTY_TRSHREQDatas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, "
						   + "        LEFT(A.REQ_DATE,4)+SUBSTRING(A.REQ_DATE,5,2) AS YYMM_NM, "
						   + "        RTRIM(X1.EMBSNAME) AS EMBSNAME, RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM "
						   + "   FROM DUTY_TRSHREQ A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON A.SABN=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE LEFT(A.REQ_DATE,6) = '" + yymm + "' AND AP_TAG='9' "
						   + "  ORDER BY X2.DEPRNAM1, A.SABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DEL_DUTY_TRSHREQ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//내역조회
		public void GetSEARCH_DUTY_TRSHREQDatas(string frmm, string tomm, DataSet ds)
		{
			try
			{
				string qry = " SELECT RTRIM(ISNULL(X1.EMBSNAME,'')) AS EMBSNAME, RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        LEFT(A.REQ_DATE,4)+'-'+SUBSTRING(A.REQ_DATE,5,2) AS YYMM_NM, "
						   + "        A.* "
						   + "   FROM DUTY_TRSHREQ A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON A.SABN=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE LEFT(A.REQ_DATE,6) BETWEEN '" + frmm + "' AND '" + tomm + "' "
						   + "    AND A.AP_TAG='9' "
						   + "  ORDER BY X1.EMBSJOCD, X1.EMBSDPCD ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_DUTY_TRSHREQ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		

		#region 9010 - 시급관리
		
		//시급처리
		public void GetS_INFOSD01Datas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT '" + yymm.Substring(0, 6) + "' AS YYMM, "
						   + "        '" + yymm.Substring(0, 4) + "-" + yymm.Substring(4, 2) +"' AS YYMM_NM, "
						   + "        RTRIM(A.EMBSSABN) AS SABN, "
						   + "        RTRIM(A.EMBSNAME) AS SABN_NM, "
						   + "        RTRIM(ISNULL(X1.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        ISNULL(X2.YY_AMT,0) AS YY_AMT, "
						   + "        ISNULL(X2.T_AMT,0) AS T_AMT "
						   + "   FROM " + wagedb + ".dbo.MSTEMBS A "
						   + "   LEFT OUTER JOIN MSTDEPR X1 ON A.EMBSDPCD=X1.DEPRCODE "
						   + "   LEFT OUTER JOIN DUTY_INFOSD01 X2 ON A.EMBSSABN=X2.SABN "
						   + "    AND X2.YYMM = '" + yymm.Substring(0, 6) + "' "
						   + "  WHERE A.EMBSSTAT=1 OR (A.EMBSSTAT=2 AND LEFT(A.EMBSTSDT,6)>='" + yymm.Substring(0, 6) + "')"
						   + "  ORDER BY A.EMBSJOCD, A.EMBSDPCD, A.EMBSSABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("S_INFOSD01", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//전월시급조회
		public void GetBF_INFOSD01Datas(string bfmm, DataSet ds)
		{
			try
			{
				string qry = " SELECT '" + bfmm.Substring(0, 6) + "' AS YYMM, "
						   + "        '" + bfmm.Substring(0, 4) + "-" + bfmm.Substring(4, 2) +"' AS YYMM_NM, "
						   + "        RTRIM(X1.EMBSSABN) AS SABN, "
						   + "        RTRIM(X1.EMBSNAME) AS SABN_NM, "
						   + "        RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        ISNULL(A.YY_AMT,0) AS YY_AMT, "
						   + "        ISNULL(A.T_AMT,0) AS T_AMT "
						   + "   FROM DUTY_INFOSD01 A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON A.SABN=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE A.YYMM = '" + bfmm.Substring(0, 6) + "' "
						   + "  ORDER BY X1.EMBSJOCD, X1.EMBSDPCD, X1.EMBSSABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("BF_INFOSD01", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//시급 불러오기
		public void GetDUTY_INFOSD01Datas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOSD01 WHERE YYMM = '" + yymm + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_INFOSD01", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//부서명칭 가져오기
		public string GetDept_nmData(string sabn)
		{
			string dept_nm = "";
			try
			{
				string qry = " SELECT RTRIM(ISNULL(X1.DEPRNAM1,'')) AS NAME "
						   + "   FROM MSTEMBS A "
						   + "  WHERE A.EMBSSABN = '" + sabn +"'"
						   + "   LEFT OUTER JOIN MSTDEPR X1 ON A.EMBSDPCD=X1.DEPRCODE ";
				
                object obj = gd.GetOneData(1, dbname, qry);
                dept_nm = obj == null ? "" : obj.ToString();
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return dept_nm;
		}
		//사원명칭 가져오기
		public string GetSawon_nmData(string sabn)
		{
			string Sawon_nm = "";
			try
			{
				string qry = " SELECT RTRIM(A.EMBSNAME) AS NAME "
						   + "   FROM MSTEMBS A "
						   + "  WHERE A.EMBSSABN = '" + sabn +"'";
				
                object obj = gd.GetOneData(1, dbname, qry);
                Sawon_nm = obj == null ? "" : obj.ToString();
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return Sawon_nm;
		}
		//시급 조회
		public void GetSEARCH_INFOSD01Datas(string frmm, string tomm, DataSet ds)
		{
			try
			{
				string qry = " SELECT RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        RTRIM(X1.EMBSNAME) AS EMBSNAME, "
						   + "        LEFT(A.YYMM,4)+'-'+SUBSTRING(A.YYMM,5,2) AS YYMM_NM, "
						   + "        A.* "
						   + "   FROM DUTY_INFOSD01 A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON A.SABN=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE A.YYMM BETWEEN '" + frmm + "' AND '" + tomm + "' "
						   + "    AND (A.YY_AMT <> 0 OR A.T_AMT <> 0)"
						   + "  ORDER BY X1.EMBSJOCD, X1.EMBSDPCD ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_INFOSD01", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        #endregion


        #region 9015 - 밤근무수당관리

        //밤근무수당 처리
        public void GetS_INFO_NSDDatas(string yymm, DataSet ds)
        {
            try
            {
                string qry = " SELECT '" + yymm.Substring(0, 6) + "' AS YYMM, "
                           + "        '" + yymm.Substring(0, 4) + "-" + yymm.Substring(4, 2) + "' AS YYMM_NM, "
                           + "        A.SQ, "
                           + "        ISNULL(A.SD_AMT,0) AS SD_AMT, "
                           + "        ISNULL(A.T_AMT,0) AS T_AMT "
                           + "   FROM DUTY_INFO_NSD A "
                           + "  INNER JOIN (SELECT MAX(YYMM) YYMM FROM DUTY_INFO_NSD WHERE YYMM <= '" + yymm.Substring(0, 6) + "') X1 "
                           + "     ON A.YYMM=X1.YYMM "
                           + "  ORDER BY A.SQ ";

                DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
                dp.AddDatatable2Dataset("S_INFO_NSD", dt, ref ds);

                for (int i = 1; i <= 10; i++)
                {
                    if (ds.Tables["S_INFO_NSD"].Select("SQ = " + i + "").Length == 0)
                    {
                        DataRow drow = ds.Tables["S_INFO_NSD"].NewRow();
                        drow["YYMM"] = yymm.Substring(0, 6);
                        drow["YYMM_NM"] = yymm.Substring(0, 4) + "-" + yymm.Substring(4, 2);
                        drow["SQ"] = i;
                        drow["SD_AMT"] = 0;
                        drow["T_AMT"] = 0;
                        ds.Tables["S_INFO_NSD"].Rows.Add(drow);
                    }
                }
                ds.Tables["S_INFO_NSD"].DefaultView.Sort = "SQ";
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //밤근무수당 불러오기
        public void GetDUTY_INFO_NSDDatas(string yymm, DataSet ds)
        {
            try
            {
                string qry = " SELECT * "
                           + "   FROM DUTY_INFO_NSD WHERE YYMM = '" + yymm + "' ";

                DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
                dp.AddDatatable2Dataset("DUTY_INFO_NSD", dt, ref ds);
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //밤근무수당 조회
        public void GetSEARCH_INFO_NSDDatas(string yymm, DataSet ds)
        {
            try
            {
                string qry = " SELECT A.* "
                           + "   FROM DUTY_INFO_NSD A "
                           + "  INNER JOIN (SELECT MAX(YYMM) YYMM FROM DUTY_INFO_NSD WHERE YYMM <= '" + yymm + "') X1 "
                           + "     ON A.YYMM=X1.YYMM "
                           + "  ORDER BY A.SQ ";

                DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
                dp.AddDatatable2Dataset("SEARCH_INFO_NSD", dt, ref ds);
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 9020 - 수당산식설정

        public void GetSEARCH_INFO1Datas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOSD01 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_INFOSD01", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		public void GetSEARCH_INFO2Datas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOSD02 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_INFO2", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//삭제
		public void GetD_DUTY_INFOSD02Datas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOSD02 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("D_DUTY_INFOSD02", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//저장시 필요한 테이블구조
		public void GetDUTY_INFOSD02Datas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOSD02";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_INFOSD02", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//근태종류 LOOKUP
		public void GetSL_GTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT IFWGCODE GT_CODE, RTRIM(IFWGNAME) GT_NAME "
						   + "   FROM " + wagedb +".DBO.INFOWAGE "
						   + "  WHERE LEFT(IFWGCODE,1)='C' "
						   + "  ORDER BY IFWGCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SL_GT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//근무유형 LOOKUP
		public void GetSL_GNMUDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT G_CODE, G_FNM "
						   + "   FROM DUTY_MSTGNMU "
						   + "  ORDER BY G_CODE "; // WHERE DANG_YN='Y' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SL_GNMU", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//수당종류 LOOKUP
		public void GetSL_SDDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT IFWGCODE SD_CODE, RTRIM(IFWGNAME) SD_NAME "
						   + "   FROM " + wagedb +".DBO.INFOWAGE "
						   + "  WHERE LEFT(IFWGCODE,1)='A' "
						   + "  ORDER BY IFWGCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SL_SD", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//공제종류 LOOKUP
		public void GetSL_GJDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT IFWGCODE GJ_CODE, RTRIM(IFWGNAME) GJ_NAME "
						   + "   FROM " + wagedb +".DBO.INFOWAGE "
						   + "  WHERE LEFT(IFWGCODE,1)='B' "
						   + "  ORDER BY IFWGCODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SL_GJ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region 9030 - 당직시간관리
				
		//당직시간 조회
		public void GetS_INFOSD04Datas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT *, "
						   + "        LEFT(DANG_DT,4)+'-'+SUBSTRING(DANG_DT,5,2)+'-'+SUBSTRING(DANG_DT,7,2) AS SLDT_NM, "
						   + "        (CASE DATEPART(DW,DANG_DT) WHEN 1 THEN '일' WHEN 2 THEN '월' WHEN 3 THEN '화' "
						   + "         WHEN 4 THEN '수' WHEN 5 THEN '목' WHEN 6 THEN '금' WHEN 7 THEN '토' ELSE '' END) DAY_NM "
						   + "   FROM DUTY_INFOSD04 "
						   + "  WHERE LEFT(DANG_DT,6)= '" + yymm + "' "
						   + "  ORDER BY DANG_DT ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("S_INFOSD04", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//일자별 당직시간 저장
		public void GetDUTY_INFOSD04Datas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOSD04 "
						   + "  WHERE LEFT(DANG_DT,6)= '" + yymm + "' "
						   + "  ORDER BY DANG_DT ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_INFOSD04", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//요일별설정
		public void GetDUTY_INFOSD03Datas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOSD03";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_INFOSD03", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		#endregion		
		
		#region 9040 - N수당관리
		
		//시급처리
		public void GetS_INFOSD05Datas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT '" + yymm.Substring(0, 6) + "' AS YYMM, "
						   + "        '" + yymm.Substring(0, 4) + "-" + yymm.Substring(4, 2) +"' AS YYMM_NM, "
						   + "        RTRIM(A.EMBSSABN) AS SABN, "
						   + "        RTRIM(A.EMBSNAME) AS SABN_NM, "
						   + "        RTRIM(ISNULL(X1.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        ISNULL(X2.MINUS_NAMT,0) AS MINUS_NAMT, "
						   + "        ISNULL(X2.PLUS_NAMT,0) AS PLUS_NAMT "
						   + "   FROM " + wagedb + ".dbo.MSTEMBS A "
						   + "   LEFT OUTER JOIN MSTDEPR X1 ON A.EMBSDPCD=X1.DEPRCODE "
						   + "   LEFT OUTER JOIN DUTY_INFOSD05 X2 ON A.EMBSSABN=X2.SABN "
						   + "    AND X2.YYMM = '" + yymm.Substring(0, 6) + "' "
						   + "   INNER JOIN DUTY_INFONURS X3 ON A.EMBSDPCD=X3.DEPTCODE "
						   + "  WHERE A.EMBSSTAT='1' OR (A.EMBSSTAT='2' AND LEFT(A.EMBSTSDT,6)>='" + yymm.Substring(0, 6) + "')"
						   + "  ORDER BY A.EMBSDPCD, A.EMBSJOCD, A.EMBSSABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("S_INFOSD05", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//전월N수당조회
		public void GetBF_INFOSD05Datas(string bfmm, DataSet ds)
		{
			try
			{
				string qry = " SELECT '" + bfmm.Substring(0, 6) + "' AS YYMM, "
						   + "        '" + bfmm.Substring(0, 4) + "-" + bfmm.Substring(4, 2) +"' AS YYMM_NM, "
						   + "        RTRIM(X1.EMBSSABN) AS SABN, "
						   + "        RTRIM(X1.EMBSNAME) AS SABN_NM, "
						   + "        RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        ISNULL(A.MINUS_NAMT,0) AS MINUS_NAMT, "
						   + "        ISNULL(A.PLUS_NAMT,0) AS PLUS_NAMT "
						   + "   FROM DUTY_INFOSD05 A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON A.SABN=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE A.YYMM = '" + bfmm.Substring(0, 6) + "' "
						   + "  ORDER BY X1.EMBSJOCD, X1.EMBSDPCD, X1.EMBSSABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("BF_INFOSD05", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//시급 불러오기
		public void GetDUTY_INFOSD05Datas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOSD05 WHERE YYMM = '" + yymm + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_INFOSD05", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//N수당 조회
		public void GetSEARCH_INFOSD05Datas(string frmm, string tomm, DataSet ds)
		{
			try
			{
				string qry = " SELECT RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        LEFT(A.YYMM,4)+'-'+SUBSTRING(A.YYMM,5,2) AS YYMM_NM, "
						   + "        A.* "
						   + "   FROM DUTY_INFOSD05 A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON A.SABN=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE A.YYMM BETWEEN '" + frmm + "' AND '" + tomm + "' "
						   + "  ORDER BY X1.EMBSJOCD, X1.EMBSDPCD ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_INFOSD05", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
				
		#region 9050 - 고정OT사용부서설정

		//부서코드 불러오기
		public void GetSEARCH_FXOTDEPTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT (CASE WHEN X1.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) CHK, "
						   + "        RTRIM(A.DEPRNAM1) DEPT_NM, A.*  "
						   + "   FROM " + wagedb + ".DBO.MSTDEPR A "
						   + "   LEFT OUTER JOIN DUTY_INFOFXOT X1 ON A.DEPRCODE=X1.DEPTCODE "
						   + "  WHERE A.DEPRSTAT=1 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_FXOTDEPT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//삭제할 부서설정 테이블 불러오기
		public void GetD_DUTY_INFOFXOTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOFXOT A ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("D_DUTY_INFOFXOT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//등록할 부서설정 테이블 불러오기
		public void GetDUTY_INFOFXOTDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOFXOT A ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_INFOFXOT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
						

		#region 4010 - 공지사항관리
		
		//공지사항 전체조회
		public void GetSEARCH_TRSNOTIDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, "
						   + "        RTRIM(ISNULL(X1.DEPRNAM1,'전체부서')) DEPT_NM, "
						   + "        LEFT(A.NOTIDATE,4)+'-'+SUBSTRING(A.NOTIDATE,5,2)+'-'+SUBSTRING(A.NOTIDATE,7,2) AS NOTIDATE_NM "
						   + "   FROM DUTY_TRSNOTI A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
						   + "     ON A.DEPTCODE=X1.DEPRCODE "
						   + "  WHERE A.PSTY <> 'D' ORDER BY A.NOTIDATE DESC, A.IDX DESC ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_TRSNOTI", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//공지사항 불러오기
		public void GetDUTY_TRSNOTIDatas(int idx, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_TRSNOTI WHERE IDX = " + idx + " ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSNOTI", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//IDX 키값 불러오기
		public int GetIDX_DUTY_TRSNOTIDatas(DataSet ds)
		{
			int idx = 0;
			try
			{
				string qry = " SELECT ISNULL(MAX(IDX),0) + 1 "
						   + "   FROM DUTY_TRSNOTI ";
				
                object obj = gd.GetOneData(1, dbname, qry);
                idx = clib.TextToInt(obj.ToString());
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return idx;
		}
				
		#endregion


		#region 8020 - 사원별연차관리
		
		//연차조회
		public void GetSEARCH_YCDatas(int lv, string dpcd, string sldt, DataSet ds)
		{
			try
			{
				string qry = " SELECT A1.*, '" + sldt + "' AS YC_STDT, "
						   + "        LEFT(A1.FR_H1,4)+'-'+SUBSTRING(A1.FR_H1,5,2)+'-'+SUBSTRING(A1.FR_H1,7,2)+' ~ '+ "
						   + "        LEFT(A1.TO_H1,4)+'-'+SUBSTRING(A1.TO_H1,5,2)+'-'+SUBSTRING(A1.TO_H1,7,2) AS FRTO_H1, "
						   + "        LEFT(A1.FR_H2,4)+'-'+SUBSTRING(A1.FR_H2,5,2)+'-'+SUBSTRING(A1.FR_H2,7,2)+' ~ '+ "
						   + "        LEFT(A1.TO_H2,4)+'-'+SUBSTRING(A1.TO_H2,5,2)+'-'+SUBSTRING(A1.TO_H2,7,2) AS FRTO_H2, "
						   + "        (CASE WHEN '" + sldt + "' BETWEEN A1.FR_H1 AND A1.TO_H1 THEN 1 ELSE 0 END) CHK1, "
						   + "        (CASE WHEN '" + sldt + "' BETWEEN A1.FR_H2 AND A1.TO_H2 THEN 1 ELSE 0 END) CHK2, "
						   + "        (CASE WHEN '" + sldt + "' BETWEEN A1.FR_H1 AND A1.TO_H1 THEN 'Y' ELSE '' END) CHK1_NM, "
						   + "        (CASE WHEN '" + sldt + "' BETWEEN A1.FR_H2 AND A1.TO_H2 THEN 'Y' ELSE '' END) CHK2_NM, "
						   + "        (SELECT ISNULL(MAX(YC_SQ),0) YC_SQ FROM DUTY_MSTYCCJ "
						   + "          WHERE YC_YEAR=A1.YC_YEAR AND SAWON_NO=A1.SAWON_NO) AS YCCJ_SQ"
						   + "   FROM ( "
						   + "         SELECT A.YC_YEAR, A.SAWON_NO SAWON_NO, RTRIM(A.SAWON_NM) SAWON_NM, RTRIM(X3.EMBSEMAL) GW_EMAIL, RTRIM(ISNULL(X4.DEPRNAM1,'')) DEPT_NM, "
						   + "                A.YC_TYPE, A.IN_DATE, A.CALC_FRDT, A.CALC_TODT, A.USE_FRDT, A.USE_TODT,"
						   + "                A.YC_BASE, A.YC_CHANGE, A.YC_FIRST, A.YC_ADD, A.YC_TOTAL, "
						   + "				  (CASE WHEN A.YC_TYPE=0 THEN '회계년도기준' ELSE '입사일기준' END) YC_TYPE_NM, "
						   + "      		  LEFT(A.IN_DATE,4)+'-'+SUBSTRING(A.IN_DATE,5,2)+'-'+SUBSTRING(A.IN_DATE,7,2) AS IN_DATE_NM, "
						   + "                LEFT(A.CALC_FRDT,4)+'-'+SUBSTRING(A.CALC_FRDT,5,2)+'-'+SUBSTRING(A.CALC_FRDT,7,2)+' ~ '+ "
						   + "                LEFT(A.CALC_TODT,4)+'-'+SUBSTRING(A.CALC_TODT,5,2)+'-'+SUBSTRING(A.CALC_TODT,7,2) AS CALC_DT_NM, "
						   + "                LEFT(A.USE_FRDT,4)+'-'+SUBSTRING(A.USE_FRDT,5,2)+'-'+SUBSTRING(A.USE_FRDT,7,2) AS USE_FRDT_NM, "
						   + "                LEFT(A.USE_TODT,4)+'-'+SUBSTRING(A.USE_TODT,5,2)+'-'+SUBSTRING(A.USE_TODT,7,2) AS USE_TODT_NM, "
						   + "                A.YC_BASE+A.YC_FIRST+A.YC_ADD as YC_SUM,"
						   + "                SUM(ISNULL(X1.YC_DAYS,0)) AS YC_USE, A.YC_TOTAL - SUM(ISNULL(X1.YC_DAYS,0)) AS YC_REMAIN, "
						   + "                (CASE WHEN A.YC_TYPE IN (1,3) AND DATEDIFF(DAY, A.IN_DATE, '" + sldt + "')<365 THEN CONVERT(CHAR,DATEADD(M,-3,DATEADD(YEAR,1,A.USE_FRDT)),112) "
						   + "                      ELSE CONVERT(CHAR,DATEADD(DAY,1,DATEADD(M,-6,A.USE_TODT)),112) END) AS FR_H1, "
						   + "                (CASE WHEN A.YC_TYPE IN (1,3) AND DATEDIFF(DAY, A.IN_DATE, '" + sldt + "')<365 THEN CONVERT(CHAR,DATEADD(DAY,9,DATEADD(M,-3,DATEADD(YEAR,1,A.USE_FRDT))),112) "
						   + "                      ELSE CONVERT(CHAR,DATEADD(DAY,10,DATEADD(M,-6,A.USE_TODT)),112) END) AS TO_H1, "
						   + "                (CASE WHEN A.YC_TYPE IN (1,3) AND DATEDIFF(DAY, A.IN_DATE, '" + sldt + "')<365 THEN CONVERT(CHAR,DATEADD(DAY,-1,DATEADD(M,-1,DATEADD(YEAR,1,A.USE_FRDT))),112) "
						   + "                      ELSE CONVERT(CHAR,DATEADD(M,-2,A.USE_TODT),112) END) AS FR_H2, "
						   + "                (CASE WHEN A.YC_TYPE IN (1,3) AND DATEDIFF(DAY, A.IN_DATE, '" + sldt + "')<365 THEN CONVERT(CHAR,DATEADD(DAY,8,DATEADD(M,-1,DATEADD(YEAR,1,A.USE_FRDT))),112)  "
						   + "                      ELSE CONVERT(CHAR,DATEADD(DAY,9,DATEADD(M,-2,A.USE_TODT)),112) END) AS TO_H2 "
						   + "           FROM DUTY_TRSDYYC A "
						   + "           LEFT OUTER JOIN DUTY_TRSHREQ X1 "
						   + "             ON A.YC_YEAR=X1.REQ_YEAR AND A.SAWON_NO=X1.SABN AND X1.AP_TAG NOT IN ('5') "  //23.01.13 반려는 카운트 제외
						   + "           INNER JOIN MSTEMBS X3 "
						   + "             ON A.SAWON_NO=X3.EMBSSABN AND (X3.EMBSSTAT='1' OR X3.EMBSSTAT='2' AND X3.EMBSTSDT>='" + sldt + "' ) AND X3.EMBSIPDT <> ''"
                           + "			 LEFT OUTER JOIN MSTDEPR X4 "
						   + "			   ON X3.EMBSDPCD=X4.DEPRCODE ";
				if (lv == 2)  //부서장일경우
					qry += " WHERE X3.EMBSDPCD IN (SELECT DEPT FROM DUTY_PWERDEPT WHERE SABN = '" + SilkRoad.Config.SRConfig.USID + "') ";
				else
					qry += " WHERE X3.EMBSDPCD LIKE '" + dpcd + "'";
				qry += "           AND ( (A.YC_TYPE=0 AND A.YC_YEAR='" + sldt.Substring(0, 4) + "' ) "
					+ "                  OR (A.YC_TYPE IN (1,3) AND A.USE_FRDT<='" + sldt + "' AND A.USE_TODT>='" + sldt + "') ) "
					+ "          GROUP BY A.YC_YEAR, A.SAWON_NO, A.SAWON_NM, X3.EMBSEMAL, X4.DEPRNAM1, A.YC_TYPE, A.IN_DATE, A.CALC_FRDT, A.CALC_TODT, A.USE_FRDT, A.USE_TODT,"
					+ "                   A.YC_BASE, A.YC_CHANGE, A.YC_FIRST, A.YC_ADD, A.YC_TOTAL  ) A1 "
					+ "  ORDER BY A1.SAWON_NO ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_YC", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//연차촉진 조회 TRSHREQ
		public void GetSEARCH_TRSHREQDatas(string sabn, string yc_year, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, "
						   + "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2) "
						   + "			    ELSE SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2)+'~'+ "
						   + "					 SUBSTRING(A.REQ_DATE2,3,2)+'.'+SUBSTRING(A.REQ_DATE2,5,2)+'.'+SUBSTRING(A.REQ_DATE2,7,2) END) AS DATE_NM, "
						   + "        (CASE ISNULL(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
						   + "              WHEN '4' THEN '진행' WHEN '5' THEN '반려' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
						   + "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN X1.G_FNM ELSE X1.G_FNM+'~'+X2.G_FNM END) AS GNMU_NM " //, ISNULL(X1.YC_DAY,0) YC_USE "
						   + "   FROM DUTY_TRSHREQ A "
						   + "   LEFT OUTER JOIN DUTY_MSTGNMU X1 "
						   + "     ON A.REQ_TYPE=X1.G_CODE "
						   + "   LEFT OUTER JOIN DUTY_MSTGNMU X2 "
						   + "     ON A.REQ_TYPE2=X2.G_CODE "
						   + "  WHERE A.SABN= '" + sabn + "' AND A.REQ_YEAR = '" + yc_year + "' "
						   + "  ORDER BY A.REQ_DATE ";
				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_TRSHREQ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//연차촉진 조회
		public void GetSEARCH_DUTY_MSTYCCJDatas(string sabn, string yc_year, string doc_type, string yc_sq, DataSet ds)
		{
			try
			{
				string qry = "";

				if (yc_sq == "")
				{
					qry = " SELECT *, (CASE WHEN DOC_TYPE='202101' THEN '1차' ELSE '2차' END) TYPE_NM "
						+ "   FROM DUTY_MSTYCCJ "
						+ "  WHERE SAWON_NO= '" + sabn + "' AND YC_YEAR = '" + yc_year + "' "
						+ "  ORDER BY DOC_TYPE, YC_SQ ";
					DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
					dp.AddDatatable2Dataset("SEARCH_DUTY_MSTYCCJ", dt, ref ds);
				}
				else
				{
					qry = " SELECT * FROM DUTY_MSTYCCJ "
						+ "  WHERE SAWON_NO= '" + sabn + "' AND YC_YEAR = '" + yc_year + "'"
						+ "    AND DOC_TYPE= '" + doc_type + "' AND YC_SQ= " + yc_sq + " ";
					DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
					dp.AddDatatable2Dataset("DUTY_MSTYCCJ", dt, ref ds);
				}

			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//연차산정작업
		public void GetUSP_MAKE_YCDatas(string sldt, DataSet ds)
		{
			try
			{
				string qry = " EXEC USP_DUTY8010_BASE '%', '%', '" + sldt + "', '" + SilkRoad.Config.SRConfig.USID + "' ";
				object obj = gd.GetOneData(1, dbname, qry);

				//DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				//dp.AddDatatable2Dataset("USP_MAKE_YC", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//사원별연차조회
		public void GetDUTY_TRSDYYCDatas(string year, string sabn, DataSet ds)
		{
			try
			{
				string qry = " SELECT YC_BASE+YC_FIRST+YC_ADD as YC_SUM, * "
						   + "   FROM DUTY_TRSDYYC "
						   + "  WHERE YC_YEAR='" + year + "' AND SAWON_NO='" + sabn + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSDYYC", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//연차촉진 전송체크
		public void GetCHK_MSTYCCJDatas(string year, string sabn, string type, DataSet ds)
		{
			try
			{
				string qry = " SELECT SAWON_NO "
						   + "   FROM DUTY_MSTYCCJ "
						   + "  WHERE YC_YEAR='" + year + "' AND SAWON_NO='" + sabn + "'  AND DOC_TYPE='" + type + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("CHK_MSTYCCJ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
				
		#region 8030 - 연차조회및승인
		
		//연차내역 조회
		public void GetYC_DAYS_ECHKDatas(string sabn, string year, string frdt, string todt, string frgn, string togn, DataSet ds)
		{
			try
			{
				string qry = " EXEC YC_DAYS_ECHK '" + sabn + "', '" + year + "', '" + frdt + "', '" + todt + "', '" + frgn + "', '" + togn + "'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("YC_DAYS_ECHK", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//연차근무 LOOKUP
		public void Get8030_SEARCH_GNMUDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT G_CODE, G_FNM, G_SNM "
						   + "   FROM DUTY_MSTGNMU "
						   + "  WHERE G_TYPE=8 ORDER BY G_CODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("8030_SEARCH_GNMU", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//부서별 직원LOOKUP
		public void Get8030_SEARCH_EMBSDatas(string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT RTRIM(A.EMBSSABN) CODE, RTRIM(A.EMBSNAME) NAME, "
						   + "        A.EMBSDPCD, RTRIM(X1.DEPRNAM1) DEPT_NM, "
						   + "        RTRIM(X2.GRADNAM1) GRAD_NM, ISNULL(A.EMBSADGB,'') EMBSADGB "
						   + "   FROM " + wagedb + ".dbo.MSTEMBS A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
						   + "     ON A.EMBSDPCD = X1.DEPRCODE"
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTGRAD X2 "
						   + "     ON A.EMBSGRCD = X2.GRADCODE"
						   + "  WHERE A.EMBSSTAT='1' AND A.EMBSDPCD LIKE '" + dept + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("8030_SEARCH_EMBS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//결재라인1
		public void GetGW_LINE1Datas(string sabn, string adgb, DataSet ds)
		{
			try
			{
				string qry = "";
				if (adgb == "'1'")
				{
					qry = " SELECT RTRIM(A.EMBSSABN) CODE, RTRIM(A.EMBSNAME) NAME, "
						+ "        RTRIM(X1.DEPRNAM1) DEPT_NM, RTRIM(X2.GRADNAM1) GRAD_NM, ISNULL(A.EMBSADGB,'') EMBSADGB "
						+ "   FROM " + wagedb + ".dbo.MSTEMBS A "
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
						+ "     ON A.EMBSDPCD = X1.DEPRCODE"
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTGRAD X2 "
						+ "     ON A.EMBSGRCD = X2.GRADCODE"
						+ "  INNER JOIN (SELECT EMBSDPCD FROM MSTEMBS WHERE EMBSSABN = '" + sabn + "' ) L1 "
						+ "     ON A.EMBSDPCD = L1.EMBSDPCD"
						+ "  WHERE A.EMBSADGB IN (" + adgb + ") ";
				}
				else
				{
					qry = " SELECT RTRIM(A.EMBSSABN) CODE, RTRIM(A.EMBSNAME) NAME, "
						+ "        RTRIM(X1.DEPRNAM1) DEPT_NM, RTRIM(X2.GRADNAM1) GRAD_NM, ISNULL(A.EMBSADGB,'') EMBSADGB "
						+ "   FROM " + wagedb + ".dbo.MSTEMBS A "
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
						+ "     ON A.EMBSDPCD = X1.DEPRCODE"
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTGRAD X2 "
						+ "     ON A.EMBSGRCD = X2.GRADCODE"
						+ "  WHERE A.EMBSADGB IN (" + adgb + ") ";
				}

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("GW_LINE1", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//결재라인2
		public void GetGW_LINE2Datas(string sabn, string adgb, DataSet ds)
		{
			try
			{
				string qry = "";
				if (adgb == "'1'")
				{
					qry = " SELECT RTRIM(A.EMBSSABN) CODE, RTRIM(A.EMBSNAME) NAME, "
						+ "        RTRIM(X1.DEPRNAM1) DEPT_NM, RTRIM(X2.GRADNAM1) GRAD_NM, ISNULL(A.EMBSADGB,'') EMBSADGB "
						+ "   FROM " + wagedb + ".dbo.MSTEMBS A "
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
						+ "     ON A.EMBSDPCD = X1.DEPRCODE"
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTGRAD X2 "
						+ "     ON A.EMBSGRCD = X2.GRADCODE"
						+ "  INNER JOIN (SELECT EMBSDPCD FROM MSTEMBS WHERE EMBSSABN = '" + sabn + "' ) L1 "
						+ "     ON A.EMBSDPCD = L1.EMBSDPCD"
						+ "  WHERE A.EMBSADGB IN (" + adgb + ") ";
				}
				else
				{
					qry = " SELECT RTRIM(A.EMBSSABN) CODE, RTRIM(A.EMBSNAME) NAME, "
						+ "        RTRIM(X1.DEPRNAM1) DEPT_NM, RTRIM(X2.GRADNAM1) GRAD_NM, ISNULL(A.EMBSADGB,'') EMBSADGB "
						+ "   FROM " + wagedb + ".dbo.MSTEMBS A "
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
						+ "     ON A.EMBSDPCD = X1.DEPRCODE"
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTGRAD X2 "
						+ "     ON A.EMBSGRCD = X2.GRADCODE"
						+ "  WHERE A.EMBSADGB IN (" + adgb + ") ";
				}

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("GW_LINE2", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//결재라인1
		public void GetGW_LINE1_RQSTDatas(string sabn, string adgb, DataSet ds)
		{
			try
			{
				string qry = "";
				if (adgb == "'1'")
				{
					qry = " SELECT RTRIM(A.EMBSSABN) CODE, RTRIM(A.EMBSNAME) NAME, "
						+ "        RTRIM(X1.DEPRNAM1) DEPT_NM, RTRIM(X2.GRADNAM1) GRAD_NM, ISNULL(A.EMBSADGB,'') EMBSADGB "
						+ "   FROM " + wagedb + ".dbo.MSTEMBS A "
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
						+ "     ON A.EMBSDPCD = X1.DEPRCODE"
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTGRAD X2 "
						+ "     ON A.EMBSGRCD = X2.GRADCODE"
						+ "  INNER JOIN (SELECT EMBSDPCD FROM MSTEMBS WHERE EMBSSABN = '" + sabn + "' ) L1 "
						+ "     ON A.EMBSDPCD = L1.EMBSDPCD"
						+ "  WHERE A.EMBSADGB IN (" + adgb + ") ";
				}
				else
				{
					qry = " SELECT RTRIM(A.EMBSSABN) CODE, RTRIM(A.EMBSNAME) NAME, "
						+ "        RTRIM(X1.DEPRNAM1) DEPT_NM, RTRIM(X2.GRADNAM1) GRAD_NM, ISNULL(A.EMBSADGB,'') EMBSADGB "
						+ "   FROM " + wagedb + ".dbo.MSTEMBS A "
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
						+ "     ON A.EMBSDPCD = X1.DEPRCODE"
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTGRAD X2 "
						+ "     ON A.EMBSGRCD = X2.GRADCODE"
						+ "  WHERE A.EMBSADGB IN (" + adgb + ") ";
				}

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("GW_LINE1", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//결재라인2
		public void GetGW_LINE2_RQSTDatas(string sabn, string adgb, DataSet ds)
		{
			try
			{
				string qry = "";
				if (adgb == "'1'")
				{
					qry = " SELECT RTRIM(A.EMBSSABN) CODE, RTRIM(A.EMBSNAME) NAME, "
						+ "        RTRIM(X1.DEPRNAM1) DEPT_NM, RTRIM(X2.GRADNAM1) GRAD_NM, ISNULL(A.EMBSADGB,'') EMBSADGB "
						+ "   FROM " + wagedb + ".dbo.MSTEMBS A "
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
						+ "     ON A.EMBSDPCD = X1.DEPRCODE"
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTGRAD X2 "
						+ "     ON A.EMBSGRCD = X2.GRADCODE"
						+ "  INNER JOIN (SELECT EMBSDPCD FROM MSTEMBS WHERE EMBSSABN = '" + sabn + "' ) L1 "
						+ "     ON A.EMBSDPCD = L1.EMBSDPCD"
						+ "  WHERE A.EMBSADGB IN (" + adgb + ") ";
				}
				else
				{
					qry = " SELECT RTRIM(A.EMBSSABN) CODE, RTRIM(A.EMBSNAME) NAME, "
						+ "        RTRIM(X1.DEPRNAM1) DEPT_NM, RTRIM(X2.GRADNAM1) GRAD_NM, ISNULL(A.EMBSADGB,'') EMBSADGB "
						+ "   FROM " + wagedb + ".dbo.MSTEMBS A "
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
						+ "     ON A.EMBSDPCD = X1.DEPRCODE"
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTGRAD X2 "
						+ "     ON A.EMBSGRCD = X2.GRADCODE"
						+ "  WHERE A.EMBSADGB IN (" + adgb + ") "; //A.EMBSSABN IN ('10001','10004') "; //
				}

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("GW_LINE2", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//연차조회
		public void GetSEARCH_YC_LISTDatas(string FLAG, string fr_yymm, string to_yymm, string dept, DataSet ds)
		{
			try
			{
				string tb_nm = FLAG == "D" ? "DEL_TRSHREQ" : "DUTY_TRSHREQ";
				string dt_nm = FLAG == "D" ? "SEARCH_DEL_YC_LIST" : FLAG == "A" ? "SEARCH_AP_YC_LIST" : "SEARCH_YC_LIST";
				//string tb_nm = FLAG == "C" ? "DUTY_TRSHREQ" : "DEL_TRSHREQ";
				//string dt_nm = FLAG == "C" ? "SEARCH_YC_LIST" : "SEARCH_DEL_YC_LIST";
				string qry = " SELECT A.*, '' CHK, '' C_CHK, "
						   + "		  RTRIM(X3.EMBSNAME) SAWON_NM, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2) "
						   + "			    ELSE SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2)+'~'+ "
						   + "					 SUBSTRING(A.REQ_DATE2,3,2)+'.'+SUBSTRING(A.REQ_DATE2,5,2)+'.'+SUBSTRING(A.REQ_DATE2,7,2) END) AS DATE_NM, "
						   + "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN X1.G_FNM ELSE X1.G_FNM+'~'+X2.G_FNM END) AS GNMU_NM, "
						   + "        (CASE ISNULL(A.GUBN,'C') WHEN 'C' THEN '결재신청' WHEN 'D' THEN '취소신청' ELSE '' END) GUBN_NM, "
						   + "        (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
						   + "              WHEN '4' THEN '진행' WHEN '5' THEN '반려' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
						   + "        (CASE A.LINE_CNT WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4 "
						   + "              WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3 "
						   + "              WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2 "
						   + "              WHEN 1 THEN A.GW_NAME1 ELSE '' END) GW_LINE, "
						   + "        A.GW_NAME1+'('+A.GW_DT1+')'+(CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT, "
						   + "        CONVERT(DATETIME,A.REQ_DATE) FR_DATE, DATEADD(DAY,1,A.REQ_DATE2) TO_DATE, "
						   + "        0 AS TYPE, 1 AS ALLDAY, (CASE A.AP_TAG WHEN '1' THEN 2 WHEN '2' THEN 1 ELSE 3 END) AS LABEL, "
						   + "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN X1.G_FNM ELSE X1.G_FNM+'~'+X2.G_FNM END) AS REMARK "
						   + "   FROM " + tb_nm + " A "
						   + "   LEFT OUTER JOIN DUTY_MSTGNMU X1 "
						   + "     ON A.REQ_TYPE=X1.G_CODE "
						   + "   LEFT OUTER JOIN DUTY_MSTGNMU X2 "
						   + "     ON A.REQ_TYPE2=X2.G_CODE "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						   + "     ON A.SABN=X3.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						   + "     ON X3.EMBSDPCD=X4.DEPRCODE "
						   + "  WHERE ( (LEFT(REQ_DATE,6) BETWEEN '" + fr_yymm + "' AND '" + to_yymm + "') OR "
						   + "         (LEFT(REQ_DATE2,6) BETWEEN '" + fr_yymm + "' AND '" + to_yymm + "') OR ('" + fr_yymm + "' <= LEFT(REQ_DATE,6) AND LEFT(REQ_DATE2,6) <= '" + to_yymm + "') )"
						   //+ "  WHERE ('" + fr_yymm + "' BETWEEN LEFT(A.REQ_DATE,6) AND LEFT(A.REQ_DATE2,6) "
						   //+ "        OR '" + to_yymm + "' BETWEEN LEFT(A.REQ_DATE,6) AND LEFT(A.REQ_DATE2,6) ) "
						   + "    AND X3.EMBSDPCD LIKE '" + dept + "'";				
				if (FLAG == "A")
					qry +="  AND ISNULL(A.AP_TAG,'') IN ('','2','4','5')";
				else if (FLAG == "C")
					qry +="  AND ISNULL(A.AP_TAG,'') NOT IN ('','2','4','5')";

				qry += "  ORDER BY A.REQ_DATE DESC, X3.EMBSDPCD, A.SABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset(dt_nm, dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//연차내역 조회_상세
		public void GetDUTY_TRSHREQDatas(string sabn, string sldt, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM DUTY_TRSHREQ A "
						   + "  WHERE A.SABN = '" + sabn + "'"
						   + "    AND A.REQ_DATE = '" + sldt + "'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSHREQ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//연차내역 신규등록
		public void GetDUTY_TRSHREQDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM DUTY_TRSHREQ A "
						   + "  WHERE 1=2";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSHREQ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//연차내역 조회_상세NEW
		public void GetDUTY_TRSHREQDatas(string seqno, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM DUTY_TRSHREQ A "
						   + "  WHERE A.SEQNO = " + clib.TextToDecimal(seqno) + "";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSHREQ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//연차내역 seq번호 가져오기
		public decimal GetHREQ_SEQNODatas()
		{
			decimal max_sq = 0;
			try
			{
				string qry = " SELECT ISNULL(MAX(SEQNO) + 1, 1) AS MAX_SQ "
						   + "   FROM DUTY_TRSHREQ ";
				
                object obj = gd.GetOneData(1, dbname, qry);
                max_sq = clib.TextToDecimal(obj.ToString());

			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return max_sq;
		}
		//연차내역 신규등록
		//public void GetDUTY_TRSHREQDatas(DataSet ds)
		//{
		//	try
		//	{
		//		string qry = " SELECT A.* "
		//				   + "   FROM DUTY_TRSHREQ A "
		//				   + "  WHERE 1=2";

		//		DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
		//		dp.AddDatatable2Dataset("DUTY_TRSHREQ", dt, ref ds);
		//	}
		//	catch (System.Exception ec)
		//	{
		//		System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
		//											 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}
		////연차내역 조회_상세NEW
		//public void GetDUTY_TRSHREQDatas(string seqno, DataSet ds)
		//{
		//	try
		//	{
		//		string qry = " SELECT A.* "
		//				   + "   FROM DUTY_TRSHREQ A "
		//				   + "  WHERE A.SEQNO = " + clib.TextToDecimal(seqno) + "";

		//		DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
		//		dp.AddDatatable2Dataset("DUTY_TRSHREQ", dt, ref ds);
		//	}
		//	catch (System.Exception ec)
		//	{
		//		System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
		//											 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}
		////연차내역 seq번호 가져오기
		//public decimal GetHREQ_SEQNODatas()
		//{
		//	decimal max_sq = 0;
		//	try
		//	{
		//		string qry = " SELECT ISNULL(MAX(SEQNO) + 1, 1) AS MAX_SQ "
		//				   + "   FROM DUTY_TRSHREQ ";
				
  //              object obj = gd.GetOneData(1, dbname, qry);
  //              max_sq = clib.TextToDecimal(obj.ToString());

		//	}
		//	catch (System.Exception ec)
		//	{
		//		System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
		//											 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//	return max_sq;
		//}
		//연차 삭제이력
		public void GetDEL_TRSHREQDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DEL_TRSHREQ "
						   + "  WHERE 1=2";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DEL_TRSHREQ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//연차사용년도 조회
		public string GetYC_YEAR_CHKDatas(string sabn, string sldt, DataSet ds)
		{
			string yc_year = "";
			try
			{
				string qry = " EXEC USP_YC_YEAR_CHK '" + sabn + "','" + sldt + "' ";				
                object obj = gd.GetOneData(1, dbname, qry);
                yc_year = obj.ToString().Trim();
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return yc_year;
        }
        //연차사용일수 조회 -23.10.30 조수진 요청.
        public decimal GetYC_DAYS_CALC_SABNDatas(string sabn, string frdt, string todt, string fr_gnmu, string to_gnmu, DataSet ds)
        {
            decimal yc_days = 0;
            try
            {
                string qry = " EXEC YC_DAYS_CALC_SABN '" + sabn + "','" + frdt + "','" + todt + "','" + fr_gnmu + "','" + to_gnmu + "' ";
                object obj = gd.GetOneData(1, dbname, qry);
                yc_days = Convert.ToDecimal(obj.ToString().Trim());
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return yc_days;
        }
        //연차사용일수 조회
        public decimal GetYC_DAYS_CALCDatas(string frdt, string todt, string fr_gnmu, string to_gnmu, DataSet ds)
		{
			decimal yc_days = 0;
			try
			{
				string qry = " EXEC YC_DAYS_CALC '" + frdt + "','" + todt + "','" + fr_gnmu + "','" + to_gnmu + "' ";				
                object obj = gd.GetOneData(1, dbname, qry);
                yc_days = Convert.ToDecimal(obj.ToString().Trim());
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return yc_days;
		}
		//연차발생 조회
		public void GetSEARCH_YC_YEARDatas(string sabn, string year, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_TRSDYYC "
						   + "  WHERE SAWON_NO='" + sabn + "' AND YC_YEAR='" + year + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_YC_YEAR", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//연차산정작업
		public void GetDUTY_YC_BASEDatas(string sabn, string sldt, DataSet ds)
		{
			try
			{
				string qry = " EXEC USP_DUTY8010_BASE '%', '" + sabn + "','" + sldt + "', '" + SilkRoad.Config.SRConfig.USID + "' ";
				object obj = gd.GetOneData(1, dbname, qry);

				//DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				//dp.AddDatatable2Dataset("DUTY_YC_BASE", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//연차사용내역
		public void GetSUM_YC_USEDatas(string sabn, string year, DataSet ds)
		{
			try
			{
				string qry = " SELECT SUM(ISNULL(A.YC_DAYS,0)) AS YC_DAY "
						   + "   FROM DUTY_TRSHREQ A "
						   //+ "   LEFT OUTER JOIN DUTY_MSTGNMU X1 "
						   //+ "     ON A.REQ_TYPE=X1.G_CODE "
						   + "  WHERE A.SABN='" + sabn + "' AND A.REQ_YEAR='" + year + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SUM_YC_USE", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
		
				
		#region 8050 - 휴가조회및승인
		
		//휴가근무 LOOKUP
		public void Get8050_SEARCH_GNMUDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT G_CODE, G_FNM, G_SNM "
						   + "   FROM DUTY_MSTGNMU "
						   + "  WHERE G_TYPE=12 ORDER BY G_CODE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("8050_SEARCH_GNMU", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//휴가조회
		public void GetSEARCH_JREQ_LISTDatas(string FLAG, string fr_yymm, string to_yymm, string dept, DataSet ds)
		{
			try
			{
				string tb_nm = FLAG == "C" ? "DUTY_TRSJREQ" : "DEL_TRSJREQ";
				string dt_nm = FLAG == "C" ? "SEARCH_JREQ_LIST" : "SEARCH_DEL_JREQ_LIST";
				string qry = " SELECT A.*, '' CHK, '' C_CHK, "
						   + "		  RTRIM(X3.EMBSNAME) SAWON_NM, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2) "
						   + "			    ELSE SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2)+'~'+ "
						   + "					 SUBSTRING(A.REQ_DATE2,3,2)+'.'+SUBSTRING(A.REQ_DATE2,5,2)+'.'+SUBSTRING(A.REQ_DATE2,7,2) END) AS DATE_NM, "
						   + "        X1.G_FNM+'('+(CASE A.PAY_YN WHEN 1 THEN '유급' ELSE '무급' END)+')' AS GNMU_NM, "
						   + "        (CASE ISNULL(A.GUBN,'C') WHEN 'C' THEN '결재신청' ELSE '취소신청' END) AS GUBN_NM, "
						   + "        (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
						   + "              WHEN '4' THEN '진행' WHEN '5' THEN '반려' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
						   + "        (CASE A.LINE_CNT WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4 "
						   + "              WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3 "
						   + "              WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2 "
						   + "              WHEN 1 THEN A.GW_NAME1 ELSE '' END) GW_LINE, "
						   + "        A.GW_NAME1+'('+A.GW_DT1+')'+(CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT, "
						   + "        CONVERT(DATETIME,A.REQ_DATE) FR_DATE, DATEADD(DAY,1,A.REQ_DATE2) TO_DATE, "
						   + "        0 AS TYPE, 1 AS ALLDAY, "
						   + "        (CASE WHEN A.AP_TAG IN ('1','3','8') THEN 2 WHEN A.AP_TAG = '5' THEN 1 ELSE 3 END) AS LABEL, "
						   + "        X1.G_FNM AS REMARK "
						   + "   FROM " + tb_nm + " A "
						   + "   LEFT OUTER JOIN DUTY_MSTGNMU X1 "
						   + "     ON A.REQ_TYPE=X1.G_CODE "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						   + "     ON A.SABN=X3.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						   + "     ON X3.EMBSDPCD=X4.DEPRCODE "
						   + "  WHERE ( (LEFT(REQ_DATE,6) BETWEEN '" + fr_yymm + "' AND '" + to_yymm + "') OR "
						   + "         (LEFT(REQ_DATE2,6) BETWEEN '" + fr_yymm + "' AND '" + to_yymm + "') OR ('" + fr_yymm + "' <= LEFT(REQ_DATE,6) AND LEFT(REQ_DATE2,6) <= '" + to_yymm + "') )"
						   //+ "  WHERE ('" + fr_yymm + "' BETWEEN LEFT(A.REQ_DATE,6) AND LEFT(A.REQ_DATE2,6) "
						   //+ "        OR '" + to_yymm + "' BETWEEN LEFT(A.REQ_DATE,6) AND LEFT(A.REQ_DATE2,6) ) "
						   + "    AND X3.EMBSDPCD LIKE '" + dept + "'"
						   + "  ORDER BY A.REQ_DATE DESC, X3.EMBSDPCD, A.SABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset(dt_nm, dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//휴가 삭제이력
		public void GetDEL_TRSJREQDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DEL_TRSJREQ "
						   + "  WHERE 1=2";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DEL_TRSJREQ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//휴가내역 조회
		public void GetDUTY_TRSJREQDatas(string sabn, string frdt, string todt, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.* "
						   + "   FROM DUTY_TRSJREQ A "
						   + "  WHERE A.SABN = '" + sabn + "'"
						   + "    AND A.REQ_DATE = '" + frdt + "' AND A.REQ_DATE2 = '" + todt + "'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_TRSJREQ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//휴가사용일수 조회
		public decimal GetHOLI_DAYS_CALCDatas(string frdt, string todt, string gnmu, DataSet ds)
		{
			decimal holi_days = 0;
			try
			{
				string qry = " EXEC HOLI_DAYS_CALC_220607 '" + frdt + "','" + todt + "','" + gnmu + "' ";				
                object obj = gd.GetOneData(1, dbname, qry);
                holi_days = Convert.ToDecimal(obj.ToString().Trim());
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return holi_days;
		}
		//휴가내역 seq번호 가져오기
		public decimal GetJREQ_SEQNODatas()
		{
			decimal max_sq = 0;
			try
			{
				string qry = " SELECT ISNULL(MAX(SEQNO) + 1, 1) AS MAX_SQ "
						   + "   FROM DUTY_TRSJREQ ";
				
                object obj = gd.GetOneData(1, dbname, qry);
                max_sq = clib.TextToDecimal(obj.ToString());

			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return max_sq;
		}

		#endregion
		
		#region 8010 - 연차휴가사용촉구
				
		//연차휴가사용촉구조회 
		public void GetSEARCH_8010Datas(string year, string dept, DataSet ds)
		{
			try
			{
				string qry = "  SELECT X1.*, X2.EMBSDPCD AS DEPTCODE, RTRIM(ISNULL(X3.DEPRNAM1,'')) DEPT_NM, RTRIM(X2.EMBSEMAL) GW_EMAIL,  "
						   + "         (CASE WHEN X1.DOC_TYPE='202101' THEN '1차' ELSE '2차' END) TYPE_NM "
						   + "    FROM (SELECT YC_YEAR, SAWON_NO, DOC_TYPE, MAX(YC_SQ) YC_SQ FROM DUTY_MSTYCCJ "
						   + "           WHERE YC_YEAR='" + year + "' GROUP BY YC_YEAR, SAWON_NO, DOC_TYPE ) A "
						   + "    LEFT OUTER JOIN DUTY_MSTYCCJ X1 "
						   + "      ON A.YC_YEAR=X1.YC_YEAR AND A.SAWON_NO=X1.SAWON_NO AND A.DOC_TYPE=X1.DOC_TYPE "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X2 "
						   + "     ON X1.SAWON_NO=X2.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X3 "
						   + "     ON X2.EMBSDPCD=X3.DEPRCODE "
						   + "  WHERE (X2.EMBSSTAT='1' OR (X2.EMBSSTAT='2' AND X2.EMBSTSDT <= '" + year + "1231') ) "
						   + "    AND X2.EMBSDPCD LIKE '" + dept + "'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_8010", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//환경설정 조회
		public void GetSEARCH_INFODatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_INFOSD01 ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_INFO", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//재직자 전체조회 
		public void GetSEARCH_EMBSDatas(string year, string yc_code, string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT '" + year + "' YC_YEAR, RTRIM(A.EMBSSABN) SAWON_NO, RTRIM(A.EMBSNAME) AS SAWON_NM, RTRIM(A.EMBSEMAL) GW_EMAIL, A.EMBSDPCD AS DEPTCODE, X1.PARTCODE,"
						   + "        A.EMBSIPDT AS IN_DATE,  "
						   + "        RTRIM(ISNULL(X3.DEPRNAM1,'')) DEPT_NM, RTRIM(ISNULL(X2.PARTNAME,'')) PARTNAME, "
						   + "        ISNULL(X4.YC_SQ,0) YC_SQ, X4.SAWON_SIGN, ISNULL(X4.SEND_DT,'') SEND_DT, ISNULL(X4.SEND_ID,'') SEND_ID, "
						   + "        ISNULL(X5.YC_USE_DAY,0) YC_USE_DAY, ISNULL(X6.DYYCTQTY,0) DYYCTQTY, "
						   + "        ISNULL(X6.DYYCTQTY,0) - ISNULL(X5.YC_USE_DAY,0) YC_REMAIN_DAY "
						   + "   FROM " + wagedb + ".dbo.MSTEMBS A "
						   + "   LEFT OUTER JOIN DUTY_MSTNURS X1 "
						   + "     ON A.EMBSSABN=X1.SAWON_NO "
						   + "   LEFT OUTER JOIN DUTY_MSTPART X2 "
						   + "     ON X1.PARTCODE=X2.PARTCODE "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X3 "
						   + "     ON A.EMBSDPCD=X3.DEPRCODE "
						   + "   LEFT OUTER JOIN (SELECT B.SAWON_NO, B.YC_SQ, B.SEND_DT, B.SEND_ID, B.SAWON_SIGN "
						   + "                      FROM DUTY_MSTYCCJ B"
						   + "                     INNER JOIN (SELECT YC_YEAR, SAWON_NO, MAX(YC_SQ) YC_SQ FROM DUTY_MSTYCCJ "
						   + "                                  WHERE YC_YEAR='" + year + "' GROUP BY YC_YEAR, SAWON_NO ) B1 "
						   + "                        ON B.YC_YEAR=B1.YC_YEAR AND B.SAWON_NO=B1.SAWON_NO AND B.YC_SQ=B1.YC_SQ "
						   + "                     WHERE B.YC_YEAR='" + year + "') X4 "
						   + "     ON A.EMBSSABN=X4.SAWON_NO "
						   + "   LEFT OUTER JOIN (SELECT B.SAWON_NO, SUM(B.GTMMGT"+yc_code.Substring(2,2)+") YC_USE_DAY "
						   + "                      FROM " + wagedb + ".dbo.MSTGTMM B"
						   + "                     WHERE B.GT_YYMM BETWEEN '" + year + "01' AND '" + year + "06'"
						   + "                     GROUP BY B.SAWON_NO) X5 "
						   + "     ON A.EMBSSABN=X5.SAWON_NO "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.TRSDYYC X6 "
						   + "     ON A.EMBSSABN=X6.SAWON_NO "
						   + "    AND X6.YEAR = '" + year + "'"
						   + "  WHERE (A.EMBSSTAT='1' OR (A.EMBSSTAT='2' AND A.EMBSTSDT <= '" + year + "1231') ) "
						   + "    AND A.EMBSDPCD LIKE '" + dept + "'"
						   + "  ORDER BY A.EMBSDPCD, X1.PARTCODE, A.EMBSSABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_EMBS", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//연차휴가사용촉구 테이블구조
		public void GetDUTY_MSTYCCJDatas(DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTYCCJ "
						   + "  WHERE 1=2";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_MSTYCCJ", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//연차촉진_SQ
		public int GetSEARCH_YCCJ_SQDatas(string type, string year, string sabn, DataSet ds)
		{
			int max_sq = 0;
			try
			{
				string qry = " SELECT ISNULL(MAX(YC_SQ) + 1, 1) AS YC_SQ "
						   + "   FROM DUTY_MSTYCCJ "
						   + "  WHERE DOC_TYPE = '" + type +"' "
						   + "    AND SAWON_NO = '" + sabn +"' "
						   + "    AND YC_YEAR = '" + year +"' ";
				
                object obj = gd.GetOneData(1, dbname, qry);
                max_sq = clib.TextToInt(obj.ToString());
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return max_sq;
		}


		#endregion
		
		#region 8090 - 연차및휴가조회

		//연차휴가조회
		public void GetSEARCH_8090_LISTDatas(string fr_yymm, string to_yymm, string dept, int type, DataSet ds)
		{
			try
			{
				string qry = "";

				if (type == 0 || type == 1)
				{
					qry = " SELECT '1' TYPE, '연차' TYPE_NM, A.REQ_DATE, A.AP_TAG, "
						+ "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2) "
						+ "			    ELSE SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2)+'~'+ "
						+ "					 SUBSTRING(A.REQ_DATE2,3,2)+'.'+SUBSTRING(A.REQ_DATE2,5,2)+'.'+SUBSTRING(A.REQ_DATE2,7,2) END) AS DATE_NM, "
						+ "		   RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, RTRIM(X3.EMBSNAME) SAWON_NM , "
						+ "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN X1.G_FNM ELSE X1.G_FNM+'~'+X2.G_FNM END) AS GNMU_NM, "
						+ "        A.YC_DAYS AS USE_DAYS, "
						+ "        ISNULL(A.GUBN,'C') AS GUBN, (CASE ISNULL(A.GUBN,'C') WHEN 'C' THEN '결재신청' ELSE '취소신청' END) AS GUBN_NM, "
						+ "        (CASE ISNULL(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료'"
						+ "              WHEN '4' THEN '진행' WHEN '5' THEN '반려' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
						+ "        (CASE A.LINE_CNT WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4 "
						+ "              WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3 "
						+ "              WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2 "
						+ "              WHEN 1 THEN A.GW_NAME1 ELSE '' END) GW_LINE, "
						+ "        A.GW_NAME1+'('+A.GW_DT1+')'+(CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END) "
						+ "        + (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) "
						+ "        + (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT "
						+ "   FROM DUTY_TRSHREQ A "
						+ "   LEFT OUTER JOIN DUTY_MSTGNMU X1 "
						+ "     ON A.REQ_TYPE=X1.G_CODE "
						+ "   LEFT OUTER JOIN DUTY_MSTGNMU X2 "
						+ "     ON A.REQ_TYPE2=X2.G_CODE "
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						+ "     ON A.SABN=X3.EMBSSABN "
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						+ "     ON X3.EMBSDPCD=X4.DEPRCODE "
						+ "  WHERE LEFT(A.REQ_DATE,6) BETWEEN '" + fr_yymm + "' AND '" + to_yymm + "'"
						+ "    AND X3.EMBSDPCD LIKE '" + dept + "'";
				}
				if (type == 0)
				{
					qry += " UNION ALL ";
				}
				if (type == 0 || type == 2)
				{
					qry += " SELECT '2' TYPE, '휴가' TYPE_NM, A.REQ_DATE, A.AP_TAG, "
						+ "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2) "
						+ "			    ELSE SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2)+'~'+ "
						+ "					 SUBSTRING(A.REQ_DATE2,3,2)+'.'+SUBSTRING(A.REQ_DATE2,5,2)+'.'+SUBSTRING(A.REQ_DATE2,7,2) END) AS DATE_NM, "
						+ "		   RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, RTRIM(X3.EMBSNAME) SAWON_NM, "
						+ "        X1.G_FNM+'('+(CASE A.PAY_YN WHEN 1 THEN '유급' ELSE '무급' END)+')' AS GNMU_NM, "
						+ "        A.HOLI_DAYS AS USE_DAYS, "
						+ "        ISNULL(A.GUBN,'C') AS GUBN, (CASE ISNULL(A.GUBN,'C') WHEN 'C' THEN '결재신청' ELSE '취소신청' END) AS GUBN_NM, "
						+ "        (CASE ISNULL(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료'"
						+ "              WHEN '4' THEN '진행' WHEN '5' THEN '반려' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
						+ "        (CASE A.LINE_CNT WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4 "
						+ "              WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3 "
						+ "              WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2 "
						+ "              WHEN 1 THEN A.GW_NAME1 ELSE '' END) GW_LINE, "
						+ "        A.GW_NAME1+'('+A.GW_DT1+')'+(CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END) "
						+ "        + (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) "
						+ "        + (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT "
						+ "   FROM DUTY_TRSJREQ A "
						+ "   LEFT OUTER JOIN DUTY_MSTGNMU X1 "
						+ "     ON A.REQ_TYPE=X1.G_CODE "
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						+ "     ON A.SABN=X3.EMBSSABN "
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						+ "     ON X3.EMBSDPCD=X4.DEPRCODE "
						+ "  WHERE LEFT(A.REQ_DATE,6) BETWEEN '" + fr_yymm + "' AND '" + to_yymm + "'"
						+ "    AND X3.EMBSDPCD LIKE '" + dept + "'";
				}

				qry += " ORDER BY TYPE, REQ_DATE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_8090_LIST", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


        #endregion


        #region 5060 - 결재문서관리
        // 1.call 2.ot 3.off,n 4.밤근무 5.당직근무표 6.간호근무표 7.간호간병수당 
        // 8.일지출예정 9.일지출내역 10.일계표 11.월별지출내역 12.월별수입내역 13.연지출내역 14.연수입내역
        // 21.출장근무


        //연차,휴가결재조회
        public void Get5060_AP_YCHG_LISTDatas(string usid, DataSet ds)
		{
			try
			{
				string qry = " SELECT A1.* FROM ( "
						   + " SELECT A.SEQNO, '1' AS GUBN, '연차' AS GUBN_NM, A.AP_TAG, A.REQ_DATE, A.SABN, '' CHK, '' C_CHK, "
						   + "        (CASE WHEN (CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "' THEN '1' ELSE '' END) CHK_STAT, "
						   + "        (CASE WHEN (CASE WHEN A.GW_DT4<>'' THEN A.GW_SABN4 WHEN A.GW_DT3<>'' THEN A.GW_SABN3 WHEN A.GW_DT2<>'' THEN A.GW_SABN2 ELSE '' END) = '" + usid + "' THEN '1' ELSE '' END) C_CHK_STAT, "
						   + "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2) "
						   + "			    ELSE SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2)+'~'+ "
						   + "					 SUBSTRING(A.REQ_DATE2,3,2)+'.'+SUBSTRING(A.REQ_DATE2,5,2)+'.'+SUBSTRING(A.REQ_DATE2,7,2) END) AS DATE_NM, "
						   + "		  RTRIM(X3.EMBSNAME) SAWON_NM, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN X1.G_FNM ELSE X1.G_FNM+'~'+X2.G_FNM END) AS GNMU_NM, A.YC_DAYS, "
						   + "        ISNULL(A.GUBN,'C') AS GUBNTYPE, (CASE ISNULL(A.GUBN,'C') WHEN 'C' THEN '결재신청' ELSE '취소신청' END) AS TYPE_NM, "
						   + "        (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
						   + "              WHEN '4' THEN '진행' WHEN '5' THEN '반려' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
						   + "        (CASE A.LINE_CNT WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4 "
						   + "              WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3 "
						   + "              WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2 "
						   + "              WHEN 1 THEN A.GW_NAME1 ELSE '' END) GW_LINE, "
						   + "        A.GW_NAME1+'('+A.GW_DT1+')'+(CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT "
						   + "   FROM DUTY_TRSHREQ A "
						   + "   LEFT OUTER JOIN DUTY_MSTGNMU X1 "
						   + "     ON A.REQ_TYPE=X1.G_CODE "
						   + "   LEFT OUTER JOIN DUTY_MSTGNMU X2 "
						   + "     ON A.REQ_TYPE2=X2.G_CODE "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						   + "     ON A.SABN=X3.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						   + "     ON X3.EMBSDPCD=X4.DEPRCODE ";
				if (usid == "SAMIL" || SilkRoad.Config.ACConfig.G_MSYN == "1")
				{
					qry += "  WHERE isnull(A.AP_TAG,'') IN ('','4') ";
				}
				else
				{
					qry += "  WHERE isnull(A.AP_TAG,'') IN ('','4') "
						+ "     AND ((CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "'"
						+ "      OR (CASE WHEN A.GW_DT4<>'' THEN A.GW_SABN4 WHEN A.GW_DT3<>'' THEN A.GW_SABN3 WHEN A.GW_DT2<>'' THEN A.GW_SABN2 ELSE '' END) = '" + usid + "')";
				}
				qry += " UNION ALL "
					+ "  SELECT A.SEQNO, '2' AS GUBN, '휴가' AS GUBN_NM, A.AP_TAG, A.REQ_DATE, A.SABN, '' CHK, '' C_CHK, "
					+ "         (CASE WHEN (CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "' THEN '1' ELSE '' END) CHK_STAT, "
					+ "         (CASE WHEN (CASE WHEN A.GW_DT4<>'' THEN A.GW_SABN4 WHEN A.GW_DT3<>'' THEN A.GW_SABN3 WHEN A.GW_DT2<>'' THEN A.GW_SABN2 ELSE '' END) = '" + usid + "' THEN '1' ELSE '' END) C_CHK_STAT, "
					+ "         (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2) "
					+ "			     ELSE SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2)+'~'+ "
					+ "					  SUBSTRING(A.REQ_DATE2,3,2)+'.'+SUBSTRING(A.REQ_DATE2,5,2)+'.'+SUBSTRING(A.REQ_DATE2,7,2) END) AS DATE_NM, "
					+ "		    RTRIM(X3.EMBSNAME) SAWON_NM, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
					+ "         X1.G_FNM+'('+(CASE A.PAY_YN WHEN 1 THEN '유급' ELSE '무급' END)+')' AS GNMU_NM, A.HOLI_DAYS AS YC_DAYS, "
					+ "         ISNULL(A.GUBN,'C') AS GUBNTYPE, (CASE ISNULL(A.GUBN,'C') WHEN 'C' THEN '결재신청' ELSE '취소신청' END) AS TYPE_NM, "
					+ "         (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
					+ "               WHEN '4' THEN '진행' WHEN '5' THEN '반려' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
					+ "         (CASE A.LINE_CNT WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4 "
					+ "               WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3 "
					+ "               WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2 "
					+ "               WHEN 1 THEN A.GW_NAME1 ELSE '' END) GW_LINE, "
					+ "         A.GW_NAME1+'('+A.GW_DT1+')'+(CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END) "
					+ "         + (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) "
					+ "         + (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT "
					+ "    FROM DUTY_TRSJREQ A "
					+ "    LEFT OUTER JOIN DUTY_MSTGNMU X1 "
					+ "      ON A.REQ_TYPE=X1.G_CODE "
					+ "    LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
					+ "      ON A.SABN=X3.EMBSSABN "
					+ "    LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
					+ "      ON X3.EMBSDPCD=X4.DEPRCODE ";
				if (usid == "SAMIL" || SilkRoad.Config.ACConfig.G_MSYN == "1")
				{
					qry += "  WHERE isnull(A.AP_TAG,'') IN ('','4') ";
				}
				else
				{
					qry += "  WHERE isnull(A.AP_TAG,'') IN ('','4') "
						+ "     AND ((CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "'"
						+ "      OR (CASE WHEN A.GW_DT4<>'' THEN A.GW_SABN4 WHEN A.GW_DT3<>'' THEN A.GW_SABN3 WHEN A.GW_DT2<>'' THEN A.GW_SABN2 ELSE '' END) = '" + usid + "')";
				}

				qry += " ) A1 ORDER BY A1.REQ_DATE DESC, A1.GUBN, A1.SABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("5060_AP_YCHG_LIST", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//연차내역 조회_상세
		public void Get5060_DUTY_TRSHREQDatas(string gubn, string seqno, DataSet ds)
		{
			try
			{
				string tb_nm = gubn == "1" ? "DUTY_TRSHREQ" : "DUTY_TRSJREQ";
				string qry = " SELECT A.* "
						   + "   FROM " + tb_nm + " A "
						   + "  WHERE A.SEQNO = " + clib.TextToDecimal(seqno) + "";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset(tb_nm, dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//연차,휴가 삭제테이블
		public void Get5060_DEL_TRSHREQDatas(string gubn, DataSet ds)
		{
			try
			{
				string tb_nm = gubn == "1" ? "DEL_TRSHREQ" : "DEL_TRSJREQ";
				string qry = " SELECT A.* "
						   + "   FROM " + tb_nm + " A ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset(tb_nm, dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//CALL,OT 결재조회
		public void Get5060_AP_YCHG_LIST2Datas(string usid, DataSet ds)
		{
			try
			{
				string qry = " SELECT A1.* FROM ( "
						   + " SELECT (CASE A.DOC_GUBN WHEN 1 THEN 'CALL' WHEN 2 THEN 'OT' WHEN 21 THEN '출장' ELSE '' END) AS GUBN_NM, "
                           + "        (CASE LEN(ISNULL(A.DOC_DATE,'')) WHEN 6 THEN LEFT(A.DOC_DATE,4)+'-'+SUBSTRING(A.DOC_DATE,5,2) "
						   + "				WHEN 8 THEN LEFT(A.DOC_DATE,4)+'-'+SUBSTRING(A.DOC_DATE,5,2)+'-'+SUBSTRING(A.DOC_DATE,7,2) ELSE '' END) DOC_DATE_NM, "
						   + "        (CASE WHEN ISNULL(A.DOC_JSMM,'')='' THEN '' ELSE LEFT(A.DOC_JSMM,4)+'-'+SUBSTRING(A.DOC_JSMM,5,2) END) DOC_JSMM_NM, "
						   + "        A.DOC_GUBN, A.DOC_NO, A.GW_TITLE, A.AP_TAG, A.REG_DT, '' CHK, '' C_CHK, "
						   + "        (CASE WHEN (CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "' THEN '1' ELSE '' END) CHK_STAT, "
						   + "		  RTRIM(X3.EMBSNAME)+'('+RTRIM(ISNULL(X4.DEPRNAM1,''))+')' SAWON_NM, "
						   + "        X3.EMBSDPCD, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
						   + "              WHEN '4' THEN '진행' WHEN '5' THEN '반려' ELSE '' END) AP_TAG_NM, "
						   + "        (CASE A.LINE_CNT WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4 "
						   + "              WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3 "
						   + "              WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2 "
						   + "              WHEN 1 THEN A.GW_NAME1 ELSE '' END) GW_LINE, "
						   + "        A.GW_NAME1+'('+A.GW_DT1+')'+(CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT "
						   + "   FROM DUTY_GWDOC A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						   + "     ON A.GW_SABN1=X3.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						   + "     ON X3.EMBSDPCD=X4.DEPRCODE "
						   + "  WHERE A.DOC_GUBN IN (1,2,21) AND isnull(A.AP_TAG,'') IN ('','4') ";
				if (usid == "SAMIL" || SilkRoad.Config.ACConfig.G_MSYN == "1")
				{
					// SAMIL이나 관리자는 모든 결재문서 조회가능
				}
				else
				{
					// 그 외의 이용자는 자신과 관련있는 결재문서만 조회가능
					qry += "   AND (A.GW_SABN4 = '" + usid + "' OR A.GW_SABN3 = '" + usid + "' OR A.GW_SABN2 = '" + usid + "' OR A.GW_SABN1 = '" + usid + "') ";
					//qry += "    AND (CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "'";
				}
				qry += " ) A1 ORDER BY A1.DOC_DATE_NM DESC, A1.EMBSDPCD, A1.DOC_JSMM_NM DESC, A1.DOC_GUBN ";//A1.REG_DT DESC, A1.DOC_GUBN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("5060_AP_YCHG_LIST2", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}		
		
		//OFF,N 결재조회
		public void Get5060_AP_YCHG_LIST3Datas(string usid, DataSet ds)
		{
			try
			{
				string qry = " SELECT A1.* FROM ( "
						   + " SELECT (CASE A.DOC_GUBN WHEN 3 THEN 'OFF/N' WHEN 4 THEN '밤근무' WHEN 7 THEN '간호간병' ELSE '' END) AS GUBN_NM, "
						   + "        (CASE LEN(ISNULL(A.DOC_DATE,'')) WHEN 6 THEN LEFT(A.DOC_DATE,4)+'-'+SUBSTRING(A.DOC_DATE,5,2) "
						   + "				WHEN 8 THEN LEFT(A.DOC_DATE,4)+'-'+SUBSTRING(A.DOC_DATE,5,2)+'-'+SUBSTRING(A.DOC_DATE,7,2) ELSE '' END) DOC_DATE_NM, "
						   + "        (CASE WHEN ISNULL(A.DOC_JSMM,'')='' THEN '' ELSE LEFT(A.DOC_JSMM,4)+'-'+SUBSTRING(A.DOC_JSMM,5,2) END) DOC_JSMM_NM, "
						   + "        A.DOC_GUBN, A.DOC_NO, A.GW_TITLE, A.AP_TAG, A.REG_DT, '' CHK, '' C_CHK, "
						   + "        (CASE WHEN (CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "' THEN '1' ELSE '' END) CHK_STAT, "
						   + "		  RTRIM(X3.EMBSNAME)+'('+RTRIM(ISNULL(X4.DEPRNAM1,''))+')' SAWON_NM, "
						   + "        X3.EMBSDPCD, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
						   + "              WHEN '4' THEN '진행' WHEN '5' THEN '반려' ELSE '' END) AP_TAG_NM, "
						   + "        (CASE A.LINE_CNT WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4 "
						   + "              WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3 "
						   + "              WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2 "
						   + "              WHEN 1 THEN A.GW_NAME1 ELSE '' END) GW_LINE, "
						   + "        A.GW_NAME1+'('+A.GW_DT1+')'+(CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT "
						   + "   FROM DUTY_GWDOC A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						   + "     ON A.GW_SABN1=X3.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						   + "     ON X3.EMBSDPCD=X4.DEPRCODE "
						   + "  WHERE A.DOC_GUBN IN (3,4,7) AND isnull(A.AP_TAG,'') IN ('','4') ";
				if (usid == "SAMIL" || SilkRoad.Config.ACConfig.G_MSYN == "1")
				{
					// SAMIL이나 관리자는 모든 결재문서 조회가능
				}
				else
				{
					// 그 외의 이용자는 자신과 관련있는 결재문서만 조회가능
					qry += "   AND (A.GW_SABN4 = '" + usid + "' OR A.GW_SABN3 = '" + usid + "' OR A.GW_SABN2 = '" + usid + "' OR A.GW_SABN1 = '" + usid + "') ";
					//qry += "    AND (CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "'";
				}
				qry += " ) A1 ORDER BY A1.DOC_DATE_NM DESC, A1.EMBSDPCD, A1.DOC_JSMM_NM DESC, A1.DOC_GUBN ";//A1.REG_DT DESC, A1.DOC_GUBN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("5060_AP_YCHG_LIST3", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}		
		
		//근무표 결재조회
		public void Get5060_AP_YCHG_LIST4Datas(string usid, DataSet ds)
		{
			try
			{
				string qry = " SELECT A1.* FROM ( "
						   + " SELECT (CASE A.DOC_GUBN WHEN 5 THEN '근무현황표' WHEN 6 THEN '간호사근무표' ELSE '' END) AS GUBN_NM, "
						   + "        (CASE LEN(ISNULL(A.DOC_DATE,'')) WHEN 6 THEN LEFT(A.DOC_DATE,4)+'-'+SUBSTRING(A.DOC_DATE,5,2) "
						   + "				WHEN 8 THEN LEFT(A.DOC_DATE,4)+'-'+SUBSTRING(A.DOC_DATE,5,2)+'-'+SUBSTRING(A.DOC_DATE,7,2) ELSE '' END) DOC_DATE_NM, "
						   + "        (CASE WHEN ISNULL(A.DOC_JSMM,'')='' THEN '' ELSE LEFT(A.DOC_JSMM,4)+'-'+SUBSTRING(A.DOC_JSMM,5,2) END) DOC_JSMM_NM, "
						   + "        A.DOC_GUBN, A.DOC_NO, A.GW_TITLE, A.AP_TAG, A.REG_DT, '' CHK, '' C_CHK, "
						   + "        (CASE WHEN (CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "' THEN '1' ELSE '' END) CHK_STAT, "
						   + "		  RTRIM(X3.EMBSNAME)+'('+RTRIM(ISNULL(X4.DEPRNAM1,''))+')' SAWON_NM, "
						   + "        X3.EMBSDPCD, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
						   + "              WHEN '4' THEN '진행' WHEN '5' THEN '반려' ELSE '' END) AP_TAG_NM, "
						   + "        (CASE A.LINE_CNT WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4 "
						   + "              WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3 "
						   + "              WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2 "
						   + "              WHEN 1 THEN A.GW_NAME1 ELSE '' END) GW_LINE, "
						   + "        A.GW_NAME1+'('+A.GW_DT1+')'+(CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT "
						   + "   FROM DUTY_GWDOC A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						   + "     ON A.GW_SABN1=X3.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						   + "     ON X3.EMBSDPCD=X4.DEPRCODE "
						   + "  WHERE A.DOC_GUBN IN (5,6) AND isnull(A.AP_TAG,'') IN ('','4') ";
				if (usid == "SAMIL" || SilkRoad.Config.ACConfig.G_MSYN == "1")
				{
					// SAMIL이나 관리자는 모든 결재문서 조회가능
				}
				else
				{
					// 그 외의 이용자는 자신과 관련있는 결재문서만 조회가능
					qry += "   AND (A.GW_SABN4 = '" + usid + "' OR A.GW_SABN3 = '" + usid + "' OR A.GW_SABN2 = '" + usid + "' OR A.GW_SABN1 = '" + usid + "') ";
					//qry += "    AND (CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "'";
				}
				qry += " ) A1 ORDER BY A1.DOC_DATE_NM DESC, A1.EMBSDPCD, A1.DOC_JSMM_NM DESC, A1.DOC_GUBN ";//A1.REG_DT DESC, A1.DOC_GUBN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("5060_AP_YCHG_LIST4", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void Get5060_AP_YCHG_LIST5Datas(string gubn, string usid, DataSet ds)
		{
			try
			{
				string power = (usid == "SAMIL")
								// SAMIL은 모든 결재문서 조회가능
								? string.Empty
								// 그 외의 이용자는 자신과 관련있는 결재문서만 조회가능
								: $@"AND 
							(
								A.GW_SABN4 = '{usid}' OR A.GW_SABN3 = '{usid}' OR A.GW_SABN2 = '{usid}' OR A.GW_SABN1 = '{usid}'
								OR '{usid}' IN (SELECT EMBSSABN FROM MSTEMBS WHERE EMBSADGB > '4')	-- 대표원장과 담당원장은 모든문서 조회가능
								OR '{usid}' IN (SELECT EMBSSABN FROM MSTEMBS WHERE EMBSDPCD = '3000' AND EMBSADGB = '2')	-- 총무팀 부서장은 모든문서 조회가능
							)";

				string qry = $@"
					SELECT	A1.*
					FROM (
						SELECT	ISNULL(B.GUBNNAME,'') AS GUBN_NM,
								CASE LEN(ISNULL(A.DOC_DATE,''))
									WHEN 6 THEN LEFT(A.DOC_DATE,4)+'-'+SUBSTRING(A.DOC_DATE,5,2)
									WHEN 8 THEN LEFT(A.DOC_DATE,4)+'-'+SUBSTRING(A.DOC_DATE,5,2)+'-'+SUBSTRING(A.DOC_DATE,7,2)
									ELSE ''
								END AS DOC_DATE_NM,
								CASE WHEN ISNULL(A.DOC_JSMM,'') = '' THEN ''
									ELSE LEFT(A.DOC_JSMM,4)+'-'+SUBSTRING(A.DOC_JSMM,5,2) 
								END AS DOC_JSMM_NM,
								A.DOC_GUBN, A.DOC_NO, A.GW_TITLE, A.AP_TAG, A.REG_DT, '' CHK, '' C_CHK,
								CASE WHEN CASE 
											WHEN A.GW_DT2='' THEN A.GW_SABN2
											WHEN A.GW_DT3='' THEN A.GW_SABN3
											WHEN A.GW_DT4='' THEN A.GW_SABN4
											ELSE '' END = '{usid}' THEN '1'
									ELSE ''
								END AS CHK_STAT,
								RTRIM(X3.EMBSNAME)+'('+RTRIM(ISNULL(X4.DEPRNAM1,''))+')' AS SAWON_NM,
								X3.EMBSDPCD,
								RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM,
								CASE ISNULL(A.AP_TAG,'')
									WHEN ''  THEN '신청'
									WHEN '1' THEN '승인'
									WHEN '2' THEN '취소'
									WHEN '3' THEN '완료'
									WHEN '4' THEN '진행'
									WHEN '5' THEN '반려'
									ELSE '' 
								END AS AP_TAG_NM,
								CASE A.LINE_CNT
									WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4
									WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3
									WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2
									WHEN 1 THEN A.GW_NAME1 ELSE '' 
								END AS GW_LINE,
								A.GW_NAME1+'('+A.GW_DT1+')'
									+ (CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END)
									+ (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) 
									+ (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT 
						FROM DUTY_GWDOC A
						LEFT OUTER JOIN DUTY_GWDOC_GUBN B
							ON A.DOC_GUBN = B.DOC_GUBN
						LEFT OUTER JOIN {wagedb}.DBO.MSTEMBS X3
							ON A.GW_SABN1=X3.EMBSSABN
						LEFT OUTER JOIN {wagedb}.DBO.MSTDEPR X4
							ON X3.EMBSDPCD=X4.DEPRCODE
						WHERE A.DOC_GUBN IN ({gubn}) AND ISNULL(A.AP_TAG,'') IN ('','4')
							{power}
					) A1
					ORDER BY A1.DOC_DATE_NM DESC, A1.EMBSDPCD, A1.DOC_JSMM_NM DESC, A1.DOC_GUBN
					";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("5060_AP_YCHG_LIST5", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region 5062 - 진행문서 상세조회  

		public void Get5062_SEARCHDatas(string gubn, string doc_no, DataSet ds)
		{
			try
			{
				string qry = "";
				if (gubn == "1" || gubn == "2" || gubn == "21")
				{
					qry = " SELECT A.*, "
						+ "        A.CALL_CNT1+A.CALL_CNT2 AS CALL_CNT, "
						+ "        A.CALL_TIME1+A.CALL_TIME2 AS CALL_TIME, "
						+ "        A.OT_TIME1+A.OT_TIME2 AS OT_TIME, "
						+ "        LEFT(A.OT_DATE,4)+'.'+SUBSTRING(A.OT_DATE,5,2)+'.'+SUBSTRING(A.OT_DATE,7,2) AS OT_DATE_NM, "
						+ "        RTRIM(X1.EMBSNAME) EMBSNAME, RTRIM(X2.DEPRNAM1) DEPT_NM "
						+ "   FROM ( "
						+ "        SELECT A.DOC_NO, (CASE A.OT_GUBN WHEN '1' THEN 'CALL' WHEN '2' THEN '시간외' WHEN '3' THEN '출장' END) GUBN_NM, "
                        + "               A.SABN, A.OT_DATE, A.CALL_CNT1, A.CALL_CNT2, A.CALL_TIME1, A.CALL_TIME2, "
						+ "               A.OT_TIME1, A.OT_TIME2, A.TIME_REMK, A.REMARK "
						+ "          FROM GW_TRSOVTM A WHERE A.DOC_NO = " + doc_no + ""
						+ "     UNION ALL "
						+ "        SELECT A.DOC_NO, '정산' GUBN_NM, "
						+ "               A.SABN, A.JS_DATE AS OT_DATE, 0 CALL_CNT1, 0 CALL_CNT2, 0 CALL_TIME1, 0 CALL_TIME2, "
						+ "               A.CALC_TIME OT_TIME1, 0 OT_TIME2, A.TIME_REMK, A.REMARK "
						+ "          FROM GW_TRSOVTM_JS A WHERE A.DOC_NO = " + doc_no + ""
						+ "        ) A "
						+ "   LEFT OUTER JOIN MSTEMBS X1 "
						+ "     ON A.SABN = X1.EMBSSABN "
						+ "   LEFT OUTER JOIN MSTDEPR X2 "
						+ "     ON X1.EMBSDPCD = X2.DEPRCODE "
						//+ "  WHERE A.DOC_NO = " + doc_no + ""
						+ "  ORDER BY A.SABN, A.OT_DATE ";
				}
				else if (gubn == "3")
				{
					qry = " SELECT A.* "
						+ "   FROM GW_TRSOFFN A "
						+ "  WHERE A.DOC_NO = " + doc_no + ""
						+ "  ORDER BY A.DEPTCODE, A.SAWON_NO ";
				}
				else if (gubn == "4" || gubn == "7")
				{
					qry = " SELECT A.* "
						+ "   FROM GW_TRSYAGAN A "
						+ "  WHERE A.DOC_NO = " + doc_no + ""
						+ "  ORDER BY A.DEPTCODE, A.SAWON_NO ";
				}
				else if (gubn == "5")
				{
					qry = " SELECT A.*, RTRIM(X1.EMBSNAME) SAWON_NM "
						+ "   FROM GW_TRSDANG A "
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X1 "
						+ "     ON A.SAWON_NO = X1.EMBSSABN "
						+ "  WHERE A.DOC_NO = " + doc_no + ""
						+ "  ORDER BY A.PLAN_SQ ";
				}
				else if (gubn == "6")
				{
					qry = " SELECT A.*, RTRIM(X1.EMBSNAME) SAWON_NM "
						+ "   FROM GW_TRSPLAN A "
						+ "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X1 "
						+ "     ON A.SAWON_NO = X1.EMBSSABN "
						+ "  WHERE A.DOC_NO = " + doc_no + ""
						+ "  ORDER BY A.PLAN_SQ ";
				}
				else if (gubn == "8" || gubn == "9" || gubn == "10" || gubn == "11" || gubn == "12" || gubn == "13" || gubn == "14")
				{
					qry = $"SELECT * FROM DUTY_GWDOC_AC04REPORT WHERE DOC_NO = {doc_no}";
				}

					DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("5062_SEARCH", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		public void GetDUTY_GWDOCDatas(string doc_no, DataSet ds)
		{
			try
			{
				string qry = " SELECT X1.EMBSDPCD, A.* "
                           + "   FROM DUTY_GWDOC A "
                           + "   LEFT OUTER JOIN MSTEMBS X1 ON A.GW_SABN1=X1.EMBSSABN"
                           + "  WHERE A.DOC_NO = " + doc_no + "";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_GWDOC", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		
		public void Get5062_CHK_DOC_STATDatas(string doc_no, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.*, "
						   + "        (CASE WHEN A.AP_TAG='4' AND A.GW_SABN2<>'' AND A.GW_DT2='' THEN A.GW_SABN1 ELSE '' END) CHK_SABN1, "
						   + "        (CASE WHEN A.AP_TAG='4' AND A.GW_SABN2<>'' AND A.GW_DT2='' THEN A.GW_SABN2 "
						   + "              WHEN A.AP_TAG='4' AND A.GW_SABN3<>'' AND A.GW_DT3='' THEN A.GW_SABN3 "
						   + "              WHEN A.AP_TAG='4' AND A.GW_SABN4<>'' AND A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) CHK_SABN2 "
						   + "   FROM DUTY_GWDOC A "
						   + "  WHERE A.DOC_NO = " + doc_no + "";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("5062_CHK_DOC_STAT", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		#endregion
		
		#region 5064 - 원장단 연차/휴가 조회

		//휴가조회
		public void GetS_5064_JREQ_LISTDatas(string fr_yymm, DataSet ds)
		{
			try
			{
				string qry = " EXEC USP_DUTY5064_CALENDER '" + fr_yymm + "', '1900' ";
				//string qry = " SELECT A1.* "
				//		   + "   FROM ( "
				//		   + "		   SELECT A.SABN, A.REQ_DATE, A.AP_TAG, RTRIM(X3.EMBSNAME) SAWON_NM, "
				//		   + "		          CONVERT(DATETIME,A.REQ_DATE) FR_DATE, DATEADD(DAY,1,A.REQ_DATE2) TO_DATE, "
				//		   + "		          0 AS TYPE, 1 AS ALLDAY, "
				//		   + "		          (CASE WHEN A.AP_TAG IN ('1','3') THEN 2 WHEN A.AP_TAG = '5' THEN 1 ELSE 3 END) AS LABEL, "
				//		   + "		          X1.G_FNM AS REMARK "
				//		   + "		     FROM DUTY_TRSHREQ A "
				//		   + "		     LEFT OUTER JOIN DUTY_MSTGNMU X1 "
				//		   + "		       ON A.REQ_TYPE=X1.G_CODE "
				//		   + "		     LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
				//		   + "		       ON A.SABN=X3.EMBSSABN "
				//		   + "		    WHERE '" + fr_yymm + "' BETWEEN LEFT(A.REQ_DATE,6) AND LEFT(A.REQ_DATE2,6) "
				//		   + "		      AND X3.EMBSDPCD = '1900' UNION ALL "
				//		   + "		   SELECT A.SABN, A.REQ_DATE, A.AP_TAG, RTRIM(X3.EMBSNAME) SAWON_NM, "
				//		   + "		          CONVERT(DATETIME,A.REQ_DATE) FR_DATE, DATEADD(DAY,1,A.REQ_DATE2) TO_DATE, "
				//		   + "		          0 AS TYPE, 1 AS ALLDAY, "
				//		   + "		          (CASE WHEN A.AP_TAG IN ('1','3') THEN 2 WHEN A.AP_TAG = '5' THEN 1 ELSE 3 END) AS LABEL, "
				//		   + "		          X1.G_FNM AS REMARK "
				//		   + "		     FROM DUTY_TRSJREQ A "
				//		   + "		     LEFT OUTER JOIN DUTY_MSTGNMU X1 "
				//		   + "		       ON A.REQ_TYPE=X1.G_CODE "
				//		   + "		     LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
				//		   + "		       ON A.SABN=X3.EMBSSABN "
				//		   + "		    WHERE '" + fr_yymm + "' BETWEEN LEFT(A.REQ_DATE,6) AND LEFT(A.REQ_DATE2,6) "
				//		   + "		      AND X3.EMBSDPCD = '1900' ) A1 "
				//		   + "  ORDER BY A1.AP_TAG, A1.REQ_DATE, A1.SABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("S_5064_JREQ_LIST", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
		
		#region 5080 - 완결문서관리
		
		//연차,휴가 결재완료조회
		public void Get5080_AP_YCHG_LISTDatas(string _gubn, string usid, DataSet ds)
		{
			try
			{
				string gubn = _gubn.Substring(0, 1);
				string fr_yy = _gubn.Substring(1, 4);
				string to_yy = _gubn.Substring(5, 4);
				string qry = " SELECT A1.* FROM ( "
						   + " SELECT A.SEQNO, '1' AS GUBN, '연차' AS GUBN_NM, A.AP_TAG, A.REQ_DATE, A.SABN, '' CHK, '' C_CHK, "
						   + "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2) "
						   + "			    ELSE SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2)+'~'+ "
						   + "					 SUBSTRING(A.REQ_DATE2,3,2)+'.'+SUBSTRING(A.REQ_DATE2,5,2)+'.'+SUBSTRING(A.REQ_DATE2,7,2) END) AS DATE_NM, "
						   + "		  RTRIM(X3.EMBSNAME) SAWON_NM, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN X1.G_FNM ELSE X1.G_FNM+'~'+X2.G_FNM END) AS GNMU_NM, A.YC_DAYS, "
						   + "        ISNULL(A.GUBN,'C') AS GUBNTYPE, (CASE ISNULL(A.GUBN,'C') WHEN 'C' THEN '결재신청' ELSE '취소신청' END) AS TYPE_NM, "
						   + "        (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
						   + "              WHEN '4' THEN '진행' WHEN '5' THEN '반려' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
						   + "        (CASE A.LINE_CNT WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4 "
						   + "              WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3 "
						   + "              WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2 "
						   + "              WHEN 1 THEN A.GW_NAME1 ELSE '' END) GW_LINE, "
						   + "        A.GW_NAME1+'('+A.GW_DT1+')'+(CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT "
						   + "   FROM DUTY_TRSHREQ A "
						   + "   LEFT OUTER JOIN DUTY_MSTGNMU X1 "
						   + "     ON A.REQ_TYPE=X1.G_CODE "
						   + "   LEFT OUTER JOIN DUTY_MSTGNMU X2 "
						   + "     ON A.REQ_TYPE2=X2.G_CODE "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						   + "     ON A.SABN=X3.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						   + "     ON X3.EMBSDPCD=X4.DEPRCODE "
						   + "  WHERE isnull(A.AP_TAG,'') IN ('1','3','8') "
						   + "    AND ( (LEFT(REQ_DATE,4) BETWEEN '" + fr_yy + "' AND '" + to_yy + "') OR "
						   + "         (LEFT(REQ_DATE2,4) BETWEEN '" + fr_yy + "' AND '" + to_yy + "') OR ('" + fr_yy + "' <= LEFT(REQ_DATE,4) AND LEFT(REQ_DATE2,4) <= '" + to_yy + "') )";
				if (usid == "SAMIL" || SilkRoad.Config.ACConfig.G_MSYN == "1")
				{
				}
				else // if (usid != "SAMIL")				
					qry += "   AND (A.GW_SABN4 = '" + usid + "' OR A.GW_SABN3 = '" + usid + "' OR A.GW_SABN2 = '" + usid + "' OR A.GW_SABN1 = '" + usid + "') ";
				
				qry += " UNION ALL "
					+ "  SELECT A.SEQNO, '2' AS GUBN, '휴가' AS GUBN_NM, A.AP_TAG, A.REQ_DATE, A.SABN, '' CHK, '' C_CHK, "
					+ "         (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2) "
					+ "			      ELSE SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2)+'~'+ "
					+ "					   SUBSTRING(A.REQ_DATE2,3,2)+'.'+SUBSTRING(A.REQ_DATE2,5,2)+'.'+SUBSTRING(A.REQ_DATE2,7,2) END) AS DATE_NM, "
					+ "		    RTRIM(X3.EMBSNAME) SAWON_NM, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
					+ "         X1.G_FNM+'('+(CASE A.PAY_YN WHEN 1 THEN '유급' ELSE '무급' END)+')' AS GNMU_NM, A.HOLI_DAYS AS YC_DAYS, "
					+ "         ISNULL(A.GUBN,'C') AS GUBNTYPE, (CASE ISNULL(A.GUBN,'C') WHEN 'C' THEN '결재신청' ELSE '취소신청' END) AS TYPE_NM, "
					+ "         (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
					+ "               WHEN '4' THEN '진행' WHEN '5' THEN '반려' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
					+ "         (CASE A.LINE_CNT WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4 "
					+ "               WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3 "
					+ "               WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2 "
					+ "               WHEN 1 THEN A.GW_NAME1 ELSE '' END) GW_LINE, "
					+ "         A.GW_NAME1+'('+A.GW_DT1+')'+(CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END) "
					+ "         + (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) "
					+ "         + (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT "
					+ "    FROM DUTY_TRSJREQ A "
					+ "    LEFT OUTER JOIN DUTY_MSTGNMU X1 "
					+ "      ON A.REQ_TYPE=X1.G_CODE "
					+ "    LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
					+ "      ON A.SABN=X3.EMBSSABN "
					+ "    LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
					+ "      ON X3.EMBSDPCD=X4.DEPRCODE "
					+ "   WHERE isnull(A.AP_TAG,'') IN ('1','3') "
					+ "     AND ( (LEFT(REQ_DATE,4) BETWEEN '" + fr_yy + "' AND '" + to_yy + "') OR "
					+ "         (LEFT(REQ_DATE2,4) BETWEEN '" + fr_yy + "' AND '" + to_yy + "') OR ('" + fr_yy + "' <= LEFT(REQ_DATE,4) AND LEFT(REQ_DATE2,4) <= '" + to_yy + "') )";
				if (usid == "SAMIL" || SilkRoad.Config.ACConfig.G_MSYN == "1")
				{
				}
				else // if (usid != "SAMIL")
					qry += "  AND (A.GW_SABN4 = '" + usid + "' OR A.GW_SABN3 = '" + usid + "' OR A.GW_SABN2 = '" + usid + "' OR A.GW_SABN1 = '" + usid + "') ";

				qry += " ) A1 "
					+ " WHERE A1.GUBN LIKE '" + gubn + "'"
					+ " ORDER BY A1.REQ_DATE DESC, A1.GUBN, A1.SABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("5080_AP_YCHG_LIST", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		//CALL,OT,출장 결재완료조회
		public void Get5080_AP_YCHG_LIST2Datas(string gubn, string usid, DataSet ds)
		{
			try
			{
				string qry = " SELECT A1.* FROM ( "
						   + " SELECT (CASE A.DOC_GUBN WHEN 1 THEN 'CALL' WHEN 2 THEN 'OT' WHEN 21 THEN '출장' ELSE '' END) AS GUBN_NM, "
                           + "        (CASE LEN(ISNULL(A.DOC_DATE,'')) WHEN 6 THEN LEFT(A.DOC_DATE,4)+'-'+SUBSTRING(A.DOC_DATE,5,2) "
						   + "				WHEN 8 THEN LEFT(A.DOC_DATE,4)+'-'+SUBSTRING(A.DOC_DATE,5,2)+'-'+SUBSTRING(A.DOC_DATE,7,2) ELSE '' END) DOC_DATE_NM, "
						   + "        (CASE WHEN ISNULL(A.DOC_JSMM,'')='' THEN '' ELSE LEFT(A.DOC_JSMM,4)+'-'+SUBSTRING(A.DOC_JSMM,5,2) END) DOC_JSMM_NM, "
						   + "        A.DOC_GUBN, A.DOC_NO, A.GW_TITLE, A.AP_TAG, A.REG_DT, "
						   + "        RTRIM(X3.EMBSNAME)+'('+RTRIM(ISNULL(X4.DEPRNAM1,''))+')' SAWON_NM, "
						   + "        X3.EMBSDPCD, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
						   + "              WHEN '4' THEN '진행' WHEN '5' THEN '반려' ELSE '' END) AP_TAG_NM, "
						   + "        (CASE A.LINE_CNT WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4 "
						   + "              WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3 "
						   + "              WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2 "
						   + "              WHEN 1 THEN A.GW_NAME1 ELSE '' END) GW_LINE, "
						   + "        A.GW_NAME1+'('+A.GW_DT1+')'+(CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT "
						   + "   FROM DUTY_GWDOC A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						   + "     ON A.GW_SABN1=X3.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						   + "     ON X3.EMBSDPCD=X4.DEPRCODE "
						   + "  WHERE A.DOC_GUBN IN (1,2,21) AND isnull(A.AP_TAG,'') IN ('1','3') ";
				if (usid == "SAMIL" || SilkRoad.Config.ACConfig.G_MSYN == "1")
				{
				}
				else
				{
					qry += "   AND (A.GW_SABN4 = '" + usid + "' OR A.GW_SABN3 = '" + usid + "' OR A.GW_SABN2 = '" + usid + "' OR A.GW_SABN1 = '" + usid + "') ";
				}

				qry += " ) A1 "
					+ " WHERE A1.DOC_GUBN LIKE '" + gubn + "'"
					+ " ORDER BY A1.DOC_DATE_NM DESC, A1.EMBSDPCD, A1.DOC_JSMM_NM DESC, A1.DOC_GUBN ";//A1.REG_DT DESC, A1.DOC_GUBN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("5080_AP_YCHG_LIST2", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//OFF,N 결재완료조회
		public void Get5080_AP_YCHG_LIST3Datas(string gubn, string usid, DataSet ds)
		{
			try
			{
				string qry = " SELECT A1.* FROM ( "
						   + " SELECT (CASE A.DOC_GUBN WHEN 3 THEN 'OFF,N' WHEN 4 THEN '밤근무' WHEN 7 THEN '간호간병' ELSE '' END) AS GUBN_NM, "
						   + "        (CASE LEN(ISNULL(A.DOC_DATE,'')) WHEN 6 THEN LEFT(A.DOC_DATE,4)+'-'+SUBSTRING(A.DOC_DATE,5,2) "
						   + "				WHEN 8 THEN LEFT(A.DOC_DATE,4)+'-'+SUBSTRING(A.DOC_DATE,5,2)+'-'+SUBSTRING(A.DOC_DATE,7,2) ELSE '' END) DOC_DATE_NM, "
						   + "        (CASE WHEN ISNULL(A.DOC_JSMM,'')='' THEN '' ELSE LEFT(A.DOC_JSMM,4)+'-'+SUBSTRING(A.DOC_JSMM,5,2) END) DOC_JSMM_NM, "
						   + "        A.DOC_GUBN, A.DOC_NO, A.GW_TITLE, A.AP_TAG, A.REG_DT, "
						   + "        RTRIM(X3.EMBSNAME)+'('+RTRIM(ISNULL(X4.DEPRNAM1,''))+')' SAWON_NM, "
						   + "        X3.EMBSDPCD, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
						   + "              WHEN '4' THEN '진행' WHEN '5' THEN '반려' ELSE '' END) AP_TAG_NM, "
						   + "        (CASE A.LINE_CNT WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4 "
						   + "              WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3 "
						   + "              WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2 "
						   + "              WHEN 1 THEN A.GW_NAME1 ELSE '' END) GW_LINE, "
						   + "        A.GW_NAME1+'('+A.GW_DT1+')'+(CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT "
						   + "   FROM DUTY_GWDOC A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						   + "     ON A.GW_SABN1=X3.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						   + "     ON X3.EMBSDPCD=X4.DEPRCODE "
						   + "  WHERE A.DOC_GUBN IN (3,4,7) AND isnull(A.AP_TAG,'') IN ('1','3') ";
				if (usid == "SAMIL" || SilkRoad.Config.ACConfig.G_MSYN == "1")
				{
				}
				else
				{
					qry += "   AND (A.GW_SABN4 = '" + usid + "' OR A.GW_SABN3 = '" + usid + "' OR A.GW_SABN2 = '" + usid + "' OR A.GW_SABN1 = '" + usid + "') ";
				}
				
				qry += " ) A1 "
					+ " WHERE A1.DOC_GUBN LIKE '" + gubn + "'"
					+ " ORDER BY A1.DOC_DATE_NM DESC, A1.EMBSDPCD, A1.DOC_JSMM_NM DESC, A1.DOC_GUBN ";//A1.REG_DT DESC, A1.DOC_GUBN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("5080_AP_YCHG_LIST3", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//근무표 결재완료조회
		public void Get5080_AP_YCHG_LIST4Datas(string gubn, string usid, DataSet ds)
		{
			try
			{
				string qry = " SELECT A1.* FROM ( "
						   + " SELECT (CASE A.DOC_GUBN WHEN 5 THEN '근무현황표' WHEN 6 THEN '간호사근무표' ELSE '' END) AS GUBN_NM, "
						   + "        (CASE LEN(ISNULL(A.DOC_DATE,'')) WHEN 6 THEN LEFT(A.DOC_DATE,4)+'-'+SUBSTRING(A.DOC_DATE,5,2) "
						   + "				WHEN 8 THEN LEFT(A.DOC_DATE,4)+'-'+SUBSTRING(A.DOC_DATE,5,2)+'-'+SUBSTRING(A.DOC_DATE,7,2) ELSE '' END) DOC_DATE_NM, "
						   + "        (CASE WHEN ISNULL(A.DOC_JSMM,'')='' THEN '' ELSE LEFT(A.DOC_JSMM,4)+'-'+SUBSTRING(A.DOC_JSMM,5,2) END) DOC_JSMM_NM, "
						   + "        A.DOC_GUBN, A.DOC_NO, A.GW_TITLE, A.AP_TAG, A.REG_DT, "
						   + "        RTRIM(X3.EMBSNAME)+'('+RTRIM(ISNULL(X4.DEPRNAM1,''))+')' SAWON_NM, "
						   + "        X3.EMBSDPCD, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
						   + "              WHEN '4' THEN '진행' WHEN '5' THEN '반려' ELSE '' END) AP_TAG_NM, "
						   + "        (CASE A.LINE_CNT WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4 "
						   + "              WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3 "
						   + "              WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2 "
						   + "              WHEN 1 THEN A.GW_NAME1 ELSE '' END) GW_LINE, "
						   + "        A.GW_NAME1+'('+A.GW_DT1+')'+(CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT "
						   + "   FROM DUTY_GWDOC A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						   + "     ON A.GW_SABN1=X3.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						   + "     ON X3.EMBSDPCD=X4.DEPRCODE "
						   + "  WHERE A.DOC_GUBN IN (5,6) AND isnull(A.AP_TAG,'') IN ('1','3') ";
				if (usid == "SAMIL" || SilkRoad.Config.ACConfig.G_MSYN == "1")
				{
				}
				else
				{
					qry += "   AND (A.GW_SABN4 = '" + usid + "' OR A.GW_SABN3 = '" + usid + "' OR A.GW_SABN2 = '" + usid + "' OR A.GW_SABN1 = '" + usid + "') ";
				}
				
				qry += " ) A1 "
					+ " WHERE A1.DOC_GUBN LIKE '" + gubn + "'"
					+ " ORDER BY A1.DOC_DATE_NM DESC, A1.EMBSDPCD, A1.DOC_JSMM_NM DESC, A1.DOC_GUBN ";//A1.REG_DT DESC, A1.DOC_GUBN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("5080_AP_YCHG_LIST4", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//근무표 결재완료조회
		public void Get5080_AP_YCHG_LIST5Datas(string gubn, string usid, DataSet ds)
		{
			try
			{
				string power = (usid == "SAMIL")
								// SAMIL은 결재문서 조회가능
								? string.Empty
								// 그 외의 이용자는 자신과 관련있는 결재문서만 조회가능
								: $@"AND 
							(
								A.GW_SABN4 = '{usid}' OR A.GW_SABN3 = '{usid}' OR A.GW_SABN2 = '{usid}' OR A.GW_SABN1 = '{usid}'
								OR '{usid}' IN (SELECT EMBSSABN FROM MSTEMBS WHERE EMBSADGB > '4')	-- 대표원장과 담당원장은 모든문서 조회가능
								OR '{usid}' IN (SELECT EMBSSABN FROM MSTEMBS WHERE EMBSDPCD = '3000' AND EMBSADGB = '2')	-- 총무팀 부서장은 모든문서 조회가능
							)";

				string qry = $@"
					SELECT	A1.*
					FROM (
						SELECT	ISNULL(B.GUBNNAME,'') AS GUBN_NM,
								CASE LEN(ISNULL(A.DOC_DATE,''))
									WHEN 6 THEN LEFT(A.DOC_DATE,4)+'-'+SUBSTRING(A.DOC_DATE,5,2)
									WHEN 8 THEN LEFT(A.DOC_DATE,4)+'-'+SUBSTRING(A.DOC_DATE,5,2)+'-'+SUBSTRING(A.DOC_DATE,7,2)
									ELSE ''
								END AS DOC_DATE_NM,
								CASE WHEN ISNULL(A.DOC_JSMM,'') = '' THEN ''
									ELSE LEFT(A.DOC_JSMM,4)+'-'+SUBSTRING(A.DOC_JSMM,5,2) 
								END AS DOC_JSMM_NM,
								A.DOC_GUBN, A.DOC_NO, A.GW_TITLE, A.AP_TAG, A.REG_DT,
								RTRIM(X3.EMBSNAME)+'('+RTRIM(ISNULL(X4.DEPRNAM1,''))+')' AS SAWON_NM,
								X3.EMBSDPCD,
								RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM,
								CASE ISNULL(A.AP_TAG,'')
									WHEN ''  THEN '신청'
									WHEN '1' THEN '승인'
									WHEN '2' THEN '취소'
									WHEN '3' THEN '완료'
									WHEN '4' THEN '진행'
									WHEN '5' THEN '반려'
									ELSE '' 
								END AS AP_TAG_NM,
								CASE A.LINE_CNT
									WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4
									WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3
									WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2
									WHEN 1 THEN A.GW_NAME1 ELSE '' 
								END AS GW_LINE,
								A.GW_NAME1+'('+A.GW_DT1+')'
									+ (CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END)
									+ (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) 
									+ (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT 
						FROM DUTY_GWDOC A
						LEFT OUTER JOIN DUTY_GWDOC_GUBN B
							ON A.DOC_GUBN = B.DOC_GUBN
						LEFT OUTER JOIN {wagedb}.DBO.MSTEMBS X3
							ON A.GW_SABN1=X3.EMBSSABN
						LEFT OUTER JOIN {wagedb}.DBO.MSTDEPR X4
							ON X3.EMBSDPCD=X4.DEPRCODE
						WHERE A.DOC_GUBN IN ({gubn}) AND ISNULL(A.AP_TAG,'') IN ('1','3')
							{power}
					) A1
					ORDER BY A1.DOC_DATE_NM DESC, A1.EMBSDPCD, A1.DOC_JSMM_NM DESC, A1.DOC_GUBN
					";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("5080_AP_YCHG_LIST5", dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region 환경설정-결재라인관리
		/// <summary>
		/// 결재라인관리
		/// </summary>
		public void GetSIGNPath(DataSet ds)
        {
            try
            {
                string qry = " SELECT * "
                           + "  FROM INFO_SIGN "
                           + " ORDER BY SQ";

                DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
                dp.AddDatatable2Dataset("INFO_SIGN", dt, ref ds);

                ds.Tables["INFO_SIGN"].Columns["SIGN_SIZE"].DefaultValue = 1;
            }
            catch (System.Exception ec)
            {
                System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
                                                     "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
		
    }
}
