﻿using System;
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
        static string wagedb = "WAGEDB" + SilkRoad.Config.SRConfig.WorkPlaceNo;
        static string comm_db = "COMMDB" + SilkRoad.Config.SRConfig.WorkPlaceNo;

		
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
				string qry = " SELECT A.* "
						   + "   FROM "
						   + "        ( SELECT USERIDEN, USERNAME, "
						   + "                 (CASE WHEN RTRIM(USERDPCD)='' THEN NULL ELSE USERDPCD END) AS USERDPCD, USERUPYN, USERMSYN "
						   + "            FROM DUTY_MSTUSER "
						   + "         UNION ALL "
						   + "          SELECT RTRIM(USERIDEN) USERNAME, RTRIM(USERNAME) USERNAME, NULL AS USERDPCD, '' USERUPYN, '' USERMSYN "
						   + "            FROM SILKDBCM..MSTUSER "
						   + "           WHERE USERIDEN NOT IN (SELECT USERIDEN FROM DUTY_MSTUSER) ) A "
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
		public void GetSEARCH_MSTNURSDatas(string dept, DataSet ds)
		{
			try
			{
				string qry = " SELECT A1.*, (CASE WHEN A1.GUBN='1' THEN '등록' ELSE '미등록' END) GUBN_NM, "
						   + "        (CASE WHEN A1.STAT=1 THEN '정상' ELSE '사용중지' END) STAT_NM, "
						   + "        (CASE A1.SHIFT_WORK WHEN 1 THEN 'Y' WHEN 2 THEN 'N' ELSE '' END) SHIFT_WORK_NM, "  //교대여부
						   + "        RTRIM(ISNULL(X3.DEPRNAM1,'')) DEPT_NM, "
						   //+ "        (CASE A1.EXP_LV WHEN 1 THEN '전문가' WHEN 2 THEN '숙련가' WHEN 3 THEN '초보자' ELSE '' END) EXP_LV_NM, "
						   //+ "        RTRIM(ISNULL(X4.EMBSNAME,'')) PRE_RN_NM, RTRIM(ISNULL(X5.G_FNM,'')) G_FNM, "
						   //+ "        (CASE WHEN A1.TM_FR<>'' AND A1.TM_TO<>'' THEN LEFT(A1.TM_FR,2)+':'+SUBSTRING(A1.TM_FR,3,2)+'~'+LEFT(A1.TM_TO,2)+':'+SUBSTRING(A1.TM_TO,3,2) ELSE A1.TM_FR+'~'+A1.TM_TO END) TM_FRTO, "
						   //+ "        (CASE WHEN A1.RETURN_DT='' THEN '' ELSE LEFT(A1.RETURN_DT,4)+'-'+SUBSTRING(A1.RETURN_DT,5,2)+'-'+SUBSTRING(A1.RETURN_DT,7,2) END) RETURN_DT_NM, "
						   + "        RTRIM(cast(DECRYPTBYPASSPHRASE('samilpas',X2.EMBSPHPN) as varchar(100))) HPNO, RTRIM(X2.EMBSEMAL) AS EMAIL_ID "
						   + "  FROM ( "
						   + "		SELECT '1' GUBN, A.SAWON_NO, A.SAWON_NM, ISNULL(X1.EMBSDPCD,'') DEPTCODE, ISNULL(A.SHIFT_WORK,0) SHIFT_WORK, "// A.EXP_LV, A.PRE_RN, A.RSP_YN, A.RSP_GNMU, "
						   + "             A.MAX_NCNT, A.ALLOWOFF, A.LIMIT_OFF, " //A.TM_YN, A.TM_FR, A.TM_TO, A.FIRST_GNMU, A.MAX_NCNT, A.MAX_CCNT, A.ALLOWOFF, "
						   + "			   A.RETURN_DT, A.CHARGE_YN, A.STAT, A.LDAY, A.INDT, A.UPDT, A.USID, A.PSTY "
						   + "		  FROM DUTY_MSTNURS A "
						   + "        LEFT OUTER JOIN " + wagedb + ".DBO.MSTEMBS X1 ON A.SAWON_NO=X1.EMBSSABN "
						   + "     UNION ALL "
						   + "		SELECT '2' GUBN, RTRIM(A.EMBSSABN) AS SAWON_NO, RTRIM(A.EMBSNAME) AS SAWON_NM, A.EMBSDPCD AS DEPTCODE, 0 SHIFT_WORK, "//0 EXP_LV, '' PRE_RN, '' RSP_YN, '' RSP_GNMU, "
						   + "             0 MAX_NCNT, 0 ALLOWOFF, 0 LIMIT_OFF, " //'' TM_YN, '' TM_FR, '' TM_TO, '' FIRST_GNMU, 0 MAX_NCNT, 0 MAX_CCNT, 0 ALLOWOFF, "
						   + "			   '' RETURN_DT, '' CHARGE_YN, 1 STAT, '' LDAY, '' INDT, '' UPDT, '' USID, '' PSTY "
						   + "		  FROM " + wagedb + ".DBO.MSTEMBS A "
						   + "        LEFT OUTER JOIN DUTY_MSTNURS X1 ON A.EMBSSABN=X1.SAWON_NO "
						   + "		 WHERE A.EMBSSTAT=1 AND A.EMBSJOCD IN (SELECT JONGCODE FROM DUTY_INFOJONG) AND X1.SAWON_NO IS NULL ) A1 "
						   + "  LEFT OUTER JOIN " + wagedb + ".DBO.MSTEMBS X2 ON A1.SAWON_NO=X2.EMBSSABN "
						   //+ "  LEFT OUTER JOIN DUTY_MSTPART X3 ON A1.PARTCODE=X3.PARTCODE "
						   + "  LEFT OUTER JOIN " + wagedb + ".DBO.MSTDEPR X3 ON A1.DEPTCODE=X3.DEPRCODE "
						   //+ "  LEFT OUTER JOIN DUTY_MSTNURS X4 ON A1.PRE_RN=X4.SAWON_NO "
						   //+ "  LEFT OUTER JOIN DUTY_MSTGNMU X5 ON A1.RSP_GNMU=X5.G_CODE "
						   + " WHERE A1.DEPTCODE LIKE '" + dept + "'"
						   + " ORDER BY A1.GUBN DESC, A1.DEPTCODE, A1.SAWON_NO ";

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
						   + "        RTRIM(X1.DEPRNAM1) DEPT_NM, ISNULL(A.EMBSADGB,'') EMBSADGB "
						   + "   FROM " + wagedb + ".dbo.MSTEMBS A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X1 "
						   + "     ON A.EMBSDPCD = X1.DEPRCODE"
						   + "  WHERE A.EMBSSTAT='1' AND A.EMBSDPCD LIKE '" + dept + "' ";

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
				string dt_nm = gubn == "1" ? "SEARCH_OVTM" : "SEARCH_OVTM2";
				string qry = " SELECT A.*, '' CHK, '' C_CHK, "
						   + "        A.CALL_CNT1+A.CALL_CNT2 AS CALL_CNT, "
						   + "        A.CALL_TIME1+A.CALL_TIME2 AS CALL_TIME, "
						   + "        A.OT_TIME1+A.OT_TIME2 AS OT_TIME, "
						   + "        (CASE WHEN A.OT_GUBN='1' THEN '콜' ELSE 'OT' END) GUBN_NM, "
						   + "		  RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        LEFT(A.OT_DATE,4)+'-'+SUBSTRING(A.OT_DATE,5,2)+'-'+SUBSTRING(A.OT_DATE,7,2) AS SLDT_NM, "
						   //+ "        (CASE WHEN A.PSTY='U' THEN A.UPDT+' ' ELSE A.INDT+' ' END)+RTRIM(X6.USERNAME) USID_NM, "
						   + "        (CASE A.AP_TAG WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '4' THEN '진행' ELSE '' END) AP_TAG_NM, "
						   //+ "        (CASE A.AP_TAG WHEN '1' THEN A.AP_DT+' ' +RTRIM(C1.USERNAME) WHEN '2' THEN A.CANC_DT+' ' +RTRIM(C2.USERNAME) ELSE '' END) AP_DT_NM, "
						   + "        RTRIM(ISNULL(X1.SAWON_NM,X3.EMBSNAME)) SAWON_NM, "
						   + "        CONVERT(DATETIME,A.OT_DATE) FR_DATE, DATEADD(DAY,1,A.OT_DATE) TO_DATE, "
						   + "        (CASE WHEN A.OT_GUBN='1' THEN 1 ELSE 3 END) AS LABEL, "
						   + "        (CASE WHEN A.OT_GUBN='1' THEN '콜' ELSE 'OT' END) AS REMARK "
						   + "   FROM DUTY_TRSOVTM A "
						   + "   LEFT OUTER JOIN DUTY_MSTNURS X1 "
						   + "     ON A.SABN=X1.SAWON_NO "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						   + "     ON A.SABN=X3.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						   + "     ON X3.EMBSDPCD=X4.DEPRCODE "
						   //+ "   LEFT OUTER JOIN SILKDBCM..MSTUSER X6 "
						   //+ "     ON A.USID=X6.USERIDEN "
						   //+ "   LEFT OUTER JOIN SILKDBCM..MSTUSER C1 "
						   //+ "     ON A.AP_USID=C1.USERIDEN "
						   //+ "   LEFT OUTER JOIN SILKDBCM..MSTUSER C2 "
						   //+ "     ON A.CANC_USID=C2.USERIDEN "
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
				string qry = " SELECT A.* "
						   + "   FROM DUTY_TRSOVTM A "
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

		//부서코드 불러오기
		public void Get2060_DANGDEPTDatas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.DEPRCODE CODE, RTRIM(DEPRNAM1) DEPT_NM, "
						   + "        (CASE WHEN X1.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) CHK "
						   + "   FROM " + wagedb + ".DBO.MSTDEPR A "
						   + "  INNER JOIN DUTY_INFODANG X1 ON A.DEPRCODE=X1.DEPTCODE "
						   + "  WHERE A.DEPRSTAT=1 " //AND A.DEPRCODE LIKE '" + p_dpcd + "'"
						   + "  ORDER BY A.DEPRCODE ";

				if (yymm != "")
				{
					qry = " SELECT A.DEPRCODE CODE, RTRIM(DEPRNAM1) DEPT_NM, "
						+ "        (CASE WHEN X1.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) CHK, "
						+ "        (CASE WHEN X2.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) DATA_CHK "
						+ "   FROM " + wagedb + ".DBO.MSTDEPR A "
						+ "  INNER JOIN DUTY_INFODANG X1 ON A.DEPRCODE=X1.DEPTCODE "
						+ "   LEFT OUTER JOIN (SELECT DISTINCT DEPTCODE FROM DUTY_TRSDANG WHERE PLANYYMM='" + yymm +"' ) X2 "
						+ "     ON A.DEPRCODE=X2.DEPTCODE "
						+ "  WHERE A.DEPRSTAT=1 " //AND A.DEPRCODE LIKE '" + p_dpcd + "'"
						+ "  ORDER BY A.DEPRCODE ";
				}

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("2060_DANGDEPT", dt, ref ds);
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
				string qry = " SELECT * "
						   + "   FROM DUTY_MSTGNMU "
						   + "  ORDER BY G_TYPE ";
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
				string qry = "  IF EXISTS(SELECT SAWON_NO FROM DUTY_TRSDANG WHERE PLANYYMM='" + yymm + "' AND DEPTCODE = '" + dept + "' ) "
						   + "  BEGIN "
						   + "      SELECT RIGHT(A.USERID,5) as SABN, A.USERNAME, CONVERT(VARCHAR,ACCESSDATE,112) SLDT "
						   + "        FROM TB_ACCESS A "
						   + "       WHERE RIGHT(A.USERID,5) in (SELECT SAWON_NO FROM DUTY_TRSDANG WHERE PLANYYMM='" + yymm + "' AND DEPTCODE = '" + dept + "' )"
						   + "         AND CONVERT(VARCHAR,A.ACCESSDATE,112) LIKE '" + yymm + "%' AND A.AUTHMODE IN ('82') AND A.AUTHMODE1 IN ('0') "
						   + "       ORDER BY RIGHT(A.USERID,5), A.ACCESSDATE "
						   + "  END "
						   + "  ELSE "
						   + "  BEGIN"
						   + "      SELECT RIGHT(A.USERID,5) as SABN, A.USERNAME, CONVERT(VARCHAR,ACCESSDATE,112) SLDT "
						   + "        FROM TB_ACCESS A "
						   + "       WHERE RIGHT(A.USERID,5) in (SELECT EMBSSABN FROM MSTEMBS WHERE EMBSSTAT='1' AND EMBSDPCD = '" + dept + "' )"
						   + "         AND CONVERT(VARCHAR,A.ACCESSDATE,112) LIKE '" + yymm + "%' AND A.AUTHMODE IN ('82') AND A.AUTHMODE1 IN ('0') "
						   + "       ORDER BY RIGHT(A.USERID,5), A.ACCESSDATE "
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
				string bf_mm = clib.DateToText(clib.TextToDate(yymm + "01").AddMonths(-1));
				string qry = " SELECT A.*, RTRIM(X1.EMBSNAME) SAWON_NM, "
						   + "        '' D01_NM, '' D02_NM, '' D03_NM, '' D04_NM, '' D05_NM, '' D06_NM, '' D07_NM, '' D08_NM, '' D09_NM, '' D10_NM, "
						   + "        '' D11_NM, '' D12_NM, '' D13_NM, '' D14_NM, '' D15_NM, '' D16_NM, '' D17_NM, '' D18_NM, '' D19_NM, '' D20_NM, "
						   + "        '' D21_NM, '' D22_NM, '' D23_NM, '' D24_NM, '' D25_NM, '' D26_NM, '' D27_NM, '' D28_NM, '' D29_NM, '' D30_NM, '' D31_NM "
						   + "   FROM DUTY_TRSDANG A "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X1 "
						   + "     ON A.SAWON_NO = X1.EMBSSABN "
						   //+ "   LEFT OUTER JOIN (SELECT SAWON_NO, "
						   //+ "                           SUM(CASE WHEN '" + yymm.Substring(4, 2) + "' = '01' THEN 0 ELSE MM_CNT1 END) Y_CNT1, "
						   //+ "                           SUM(CASE WHEN '" + yymm.Substring(4, 2) + "' = '01' THEN 0 ELSE MM_CNT2 END) Y_CNT2 "
						   //+ "		                FROM DUTY_TRSDANG "
						   //+ "                     WHERE DEPTCODE = '" + dept + "' "
						   //+ "                       AND PLANYYMM BETWEEN '" + yymm.Substring(0, 4) + "01' AND '" + bf_mm.Substring(0, 6) + "' "
						   //+ "				       GROUP BY SAWON_NO) X3 "
						   //+ "     ON A.SAWON_NO=X3.SAWON_NO "
						   + "  WHERE A.PLANYYMM = '" + yymm + "' AND A.DEPTCODE = '" + dept + "' "
						   + "  ORDER BY A.PLAN_SQ ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				if (gubn == 1)
					dp.AddDatatable2Dataset("DUTY_TRSDANG", dt, ref ds);
				else
					dp.AddDatatable2Dataset("SEARCH_DANG_PLAN", dt, ref ds);
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
		
		//부서코드 불러오기
		public void Get3010_DANGDEPTDatas(string yymm, DataSet ds)
		{
			try
			{
				string qry = " SELECT A.DEPRCODE CODE, RTRIM(DEPRNAM1) DEPT_NM, "
						   + "        (CASE WHEN X1.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) CHK "
						   + "   FROM " + wagedb + ".DBO.MSTDEPR A "
						   + "  INNER JOIN DUTY_INFONURS X1 ON A.DEPRCODE=X1.DEPTCODE "
						   + "  WHERE A.STAT=1 " //AND A.DEPRCODE LIKE '" + p_dpcd + "'"
						   + "  ORDER BY A.DEPRCODE ";

				if (yymm != "")
				{
					qry = " SELECT A.DEPRCODE CODE, RTRIM(DEPRNAM1) DEPT_NM, "
						+ "        (CASE WHEN X1.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) CHK, "
						+ "        (CASE WHEN X2.DEPTCODE IS NOT NULL THEN '1' ELSE '0' END) DATA_CHK "
						+ "   FROM " + wagedb + ".DBO.MSTDEPR A "
						+ "  INNER JOIN DUTY_INFONURS X1 ON A.DEPRCODE=X1.DEPTCODE "
						+ "   LEFT OUTER JOIN (SELECT DISTINCT DEPTCODE FROM DUTY_TRSPLAN WHERE PLANYYMM='" + yymm +"' ) X2 "
						+ "     ON A.DEPRCODE=X2.DEPTCODE "
						+ "  WHERE A.DEPRSTAT=1 " //AND A.DEPRCODE LIKE '" + p_dpcd + "'"
						+ "  ORDER BY A.DEPRCODE ";
				}

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3010_DANGDEPT", dt, ref ds);
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
				string qry = " SELECT G_CODE, G_FNM, G_SNM "
						   + "   FROM DUTY_MSTGNMU "
						   + "  ORDER BY G_CODE "; //WHERE REQ_YN='Y' '" + yymm + "', '" + part + "' ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("GNMU_LIST", dt, ref ds);
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
				string qry = "  IF EXISTS(SELECT SAWON_NO FROM DUTY_TRSPLAN WHERE PLANYYMM='" + yymm + "' AND DEPTCODE = '" + dept + "' ) "
						   + "  BEGIN "
						   + "      SELECT RIGHT(A.USERID,5) as SABN, A.USERNAME, CONVERT(VARCHAR,ACCESSDATE,112) SLDT "
						   + "        FROM TB_ACCESS A "
						   + "       WHERE RIGHT(A.USERID,5) in (SELECT SAWON_NO FROM DUTY_TRSPLAN WHERE PLANYYMM='" + yymm + "' AND DEPTCODE = '" + dept + "' )"
						   + "         AND CONVERT(VARCHAR,A.ACCESSDATE,112) LIKE '" + yymm + "%' AND A.AUTHMODE IN ('82') AND A.AUTHMODE1 IN ('0') "
						   + "       ORDER BY RIGHT(A.USERID,5), A.ACCESSDATE "
						   + "  END "
						   + "  ELSE "
						   + "  BEGIN"
						   + "      SELECT RIGHT(A.USERID,5) as SABN, A.USERNAME, CONVERT(VARCHAR,ACCESSDATE,112) SLDT "
						   + "        FROM TB_ACCESS A "
						   + "       WHERE RIGHT(A.USERID,5) in (SELECT EMBSSABN FROM MSTEMBS WHERE EMBSSTAT='1' AND EMBSDPCD = '" + dept + "' )"
						   + "         AND CONVERT(VARCHAR,A.ACCESSDATE,112) LIKE '" + yymm + "%' AND A.AUTHMODE IN ('82') AND A.AUTHMODE1 IN ('0') "
						   + "       ORDER BY RIGHT(A.USERID,5), A.ACCESSDATE "
						   + "  END ";

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
				string qry = " SELECT RIGHT(A.USERID,5) as SABN, A.USERNAME, CONVERT(VARCHAR,ACCESSDATE,120) ACC_DT "
						   + "   FROM TB_ACCESS A "
						   + "  WHERE RIGHT(A.USERID,5) = '" + sabn + "'"
						   + "    AND CONVERT(VARCHAR,A.ACCESSDATE,112) LIKE '" + yymm + "%' AND A.AUTHMODE IN ('82') AND A.AUTHMODE1 IN ('0') "
						   + "  ORDER BY A.ACCESSDATE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("3010_KT_DT1", dt, ref ds);
				
				qry = " SELECT RIGHT(A.USERID,5) as SABN, A.USERNAME, CONVERT(VARCHAR,ACCESSDATE,120) ACC_DT "
						   + "   FROM TB_ACCESS A "
						   + "  WHERE RIGHT(A.USERID,5) = '" + sabn + "'"
						   + "    AND CONVERT(VARCHAR,A.ACCESSDATE,112) LIKE '" + yymm + "%' AND A.AUTHMODE IN ('83') AND A.AUTHMODE1 IN ('0') "
						   + "  ORDER BY A.ACCESSDATE ";

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
				string bf_mm = clib.DateToText(clib.TextToDate(yymm + "01").AddMonths(-1));
				string qry = " SELECT A.*, RTRIM(X1.SAWON_NM) SAWON_NM, ISNULL(X1.ALLOWOFF,0) MASTER_OFF, "
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
						   + "   LEFT OUTER JOIN (SELECT SAWON_NO, "
						   + "                           SUM(CASE WHEN '" + yymm.Substring(4, 2) + "' = '01' THEN 0 ELSE MM_CNT1 END) Y_CNT1, "
						   + "                           SUM(CASE WHEN '" + yymm.Substring(4, 2) + "' = '01' THEN 0 ELSE MM_CNT2 END) Y_CNT2, "
						   + "                           SUM(CASE WHEN '" + yymm.Substring(4, 2) + "' = '01' THEN 0 ELSE MM_CNT3 END) Y_CNT3, "
						   + "                           SUM(CASE WHEN '" + yymm.Substring(4, 2) + "' = '01' THEN 0 ELSE MM_CNT4 END) Y_CNT4, "
						   + "                           SUM(CASE WHEN '" + yymm.Substring(4, 2) + "' = '01' THEN 0 ELSE MM_CNT5 END) Y_CNT5 "
						   + "		                FROM DUTY_TRSPLAN "
						   + "                     WHERE DEPTCODE = '" + dept + "' "
						   + "                       AND PLANYYMM BETWEEN '" + yymm.Substring(0, 4) + "01' AND '" + bf_mm.Substring(0, 6) + "' "
						   + "				       GROUP BY SAWON_NO) X3 "
						   + "     ON A.SAWON_NO=X3.SAWON_NO "
						   + "  WHERE A.PLANYYMM = '" + yymm + "' AND A.DEPTCODE = '" + dept + "' "
						   + "  ORDER BY A.PLAN_SQ ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				if (gubn == 1)
					dp.AddDatatable2Dataset("DUTY_TRSPLAN", dt, ref ds);
				else
					dp.AddDatatable2Dataset("SEARCH_PLAN", dt, ref ds);
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
				string qry = " SELECT * "
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
		public void GetWORK_3080Datas(string yymm, string dept, string sabn, DataSet ds)
		{
			try
			{
				string qry = " EXEC USP_DUTY3080_PRC_211122 '" + yymm + "', '" + dept + "', '" + sabn + "' ";

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
				string qry = " SELECT * "
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
						   + "   LEFT OUTER JOIN MSTEMBS X1 "
						   + "     ON A.SAWON_NO=X1.EMBSSABN "
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
		//인사급여 테이블 불러오기
		public void GetMSTWGPCDatas(string yymm, string sabn, DataSet ds)
		{
			try
			{
				string qry = " SELECT * "
						   + "   FROM MSTWGPC "
						   + "  WHERE WGPCYYMM = '" + yymm + "' AND WGPCSABN LIKE '" + sabn + "' AND WGPCSQNO='1'";

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
						   + "  WHERE GTMMYYMM = '" + yymm + "' AND GTMMSABN LIKE '" + sabn + "' ";

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
				string qry = " SELECT A.USERID, RIGHT(A.USERID,5) as SABN, A.USERNAME, A.DEPARTMENTNAME AS DEPT_NM, " //RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        LEFT(CONVERT(VARCHAR,ACCESSDATE,112),4)+'-'+SUBSTRING(CONVERT(VARCHAR,ACCESSDATE,112),5,2)+'-'+SUBSTRING(CONVERT(VARCHAR,ACCESSDATE,112),7,2) SLDT_NM, "
						   + "        CONVERT(VARCHAR,A.ACCESSDATE,120) AS ACCESSDATE, A.DEVICENAME, A.AUTHMODE, "
						   + "        (CASE A.AUTHMODE WHEN '82' THEN '출근' WHEN '83' THEN '퇴근' ELSE '' END) GUBN_NM, "
						   + "        (CASE WHEN X1.EMBSSABN IS NULL THEN '1' ELSE '' END) EMBS_STAT "
						   + "   FROM TB_ACCESS A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON RIGHT(A.USERID,5)=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE CONVERT(VARCHAR,A.ACCESSDATE,112) LIKE '" + yymm + "%' "
						   + "    AND SUBSTRING(A.USERID,8,1)='9' AND A.AUTHMODE IN ('82','83') AND A.AUTHMODE1 IN ('0')  "
						   + "  ORDER BY RIGHT(A.USERID,5), A.ACCESSDATE ";

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
				string qry = " SELECT A.USERID, RIGHT(A.USERID,5) as SABN, A.USERNAME, A.DEPARTMENTNAME AS DEPT_NM, " //RTRIM(ISNULL(X2.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        LEFT(CONVERT(VARCHAR,ACCESSDATE,112),4)+'-'+SUBSTRING(CONVERT(VARCHAR,ACCESSDATE,112),5,2)+'-'+SUBSTRING(CONVERT(VARCHAR,ACCESSDATE,112),7,2) SLDT_NM, "
						   + "        CONVERT(VARCHAR,A.ACCESSDATE,120) AS ACCESSDATE, "
						   + "        (CASE A.AUTHMODE1 WHEN '2' THEN '조식' WHEN '3' THEN '중식' WHEN '4' THEN '석식' ELSE '' END) GUBN_NM, "
						   + "        (CASE WHEN X1.EMBSSABN IS NULL THEN '1' ELSE '' END) EMBS_STAT "
						   + "   FROM TB_ACCESS A "
						   + "   LEFT OUTER JOIN MSTEMBS X1 ON RIGHT(A.USERID,5)=X1.EMBSSABN "
						   + "   LEFT OUTER JOIN MSTDEPR X2 ON X1.EMBSDPCD=X2.DEPRCODE "
						   + "  WHERE CONVERT(VARCHAR,A.ACCESSDATE,112) BETWEEN '" + frdt + "' AND '" + todt + "'"
						   + "    AND SUBSTRING(A.USERID,8,1)='9' AND A.AUTHMODE1 IN ('2','3','4')   "
						   + "  ORDER BY RIGHT(A.USERID,5), A.ACCESSDATE ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("SEARCH_KT2", dt, ref ds);
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
				string qry = " SELECT '" + yymm.Substring(0, 6) + "' AS YYMM, "
						   + "        '" + yymm.Substring(0, 4) + "-" + yymm.Substring(4, 2) +"' AS YYMM_NM, "
						   + "        RTRIM(A.EMBSSABN) AS SABN, RTRIM(A.EMBSNAME) AS SABN_NM, "
						   + "        RTRIM(ISNULL(X1.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        '' YC_YEAR, '' USE_TODT, "
						   + "        0.0 AS YC_SUM, 0.0 AS YC_CHANGE, 0.0 AS YC_TOTAL, "
						   + "        0.0 AS YC_USE, 0.0 AS YC_REMAIN, 0.0 AS YC_MI_CNT "
						   + "   FROM " + wagedb + ".dbo.MSTEMBS A "
						   + "   LEFT OUTER JOIN MSTDEPR X1 ON A.EMBSDPCD=X1.DEPRCODE "
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
						   + "   LEFT OUTER JOIN DUTY_TRSHREQ X3 ON A.YC_YEAR=X3.REQ_YEAR AND A.SAWON_NO=X3.SABN "
						   + "  WHERE (X1.EMBSSTAT=1 AND LEFT(A.USE_TODT,6) = '" + yymm + "')"
						   + "     OR (X1.EMBSSTAT=2 AND LEFT(X1.EMBSTSDT,6) = '" + yymm + "') "
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
						   + "  ORDER BY A.EMBSJOCD, A.EMBSDPCD, A.EMBSSABN ";

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
		public void GetSEARCH_YCDatas(string sldt, DataSet ds)
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
						   //+ "        (CASE WHEN A1.YC_TYPE=1 AND DATEDIFF(DAY, A1.IN_DATE, '" + sldt + "')<365 THEN DATEADD(M,-3,DATEADD(YEAR,1,A1.USE_FRDT)) "
		   				//   + "              ELSE DATEADD(DAY,1,DATEADD(M,-6,A1.USE_TODT)) END) AS YC_HURRY1, "
						   //+ "        (CASE WHEN A1.YC_TYPE=1 AND DATEDIFF(DAY, A1.IN_DATE, '" + sldt + "')<365 THEN DATEADD(DAY,-1,DATEADD(M,-1,DATEADD(YEAR,1,A1.USE_FRDT))) "
		   				//   + "              ELSE DATEADD(M,-2,A1.USE_TODT) END) AS YC_HURRY2 "
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
						   + "             ON A.YC_YEAR=X1.REQ_YEAR AND A.SAWON_NO=X1.SABN " //AND X1.REQ_DATE<='" + sldt + "' "
						   //+ "           LEFT OUTER JOIN DUTY_MSTGNMU X2 "
						   //+ "             ON X1.REQ_TYPE=X2.G_CODE "
						   + "           LEFT OUTER JOIN MSTEMBS X3 "
						   + "             ON A.SAWON_NO=X3.EMBSSABN "
						   + "			 LEFT OUTER JOIN MSTDEPR X4 "
						   + "			   ON X3.EMBSDPCD=X4.DEPRCODE "
						   + "          WHERE (A.YC_TYPE=0 AND A.YC_YEAR='" + sldt.Substring(0, 4) + "' ) "
						   + "             OR (A.YC_TYPE IN (1,3) AND A.USE_FRDT<='" + sldt + "' AND A.USE_TODT>='" + sldt + "') "
						   + "          GROUP BY A.YC_YEAR, A.SAWON_NO, A.SAWON_NM, X3.EMBSEMAL, X4.DEPRNAM1, A.YC_TYPE, A.IN_DATE, A.CALC_FRDT, A.CALC_TODT, A.USE_FRDT, A.USE_TODT,"
						   + "                   A.YC_BASE, A.YC_CHANGE, A.YC_FIRST, A.YC_ADD, A.YC_TOTAL ) A1 "
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
						   + "              WHEN '4' THEN '진행' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
						   //+ "        (CASE ISNULL(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '4' THEN '진행' ELSE '' END) AP_TAG_NM, "
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
						   + "        RTRIM(X1.DEPRNAM1) DEPT_NM, RTRIM(X2.GRADNAM1) GRAD_NM, ISNULL(A.EMBSADGB,'') EMBSADGB "
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

		//연차조회
		public void GetSEARCH_YC_LISTDatas(string FLAG, string fr_yymm, string to_yymm, string dept, DataSet ds)
		{
			try
			{
				string tb_nm = FLAG == "C" ? "DUTY_TRSHREQ" : "DEL_TRSHREQ";
				string dt_nm = FLAG == "C" ? "SEARCH_YC_LIST" : "SEARCH_DEL_YC_LIST";
				string qry = " SELECT A.*, '' CHK, '' C_CHK, "
						   + "		  RTRIM(X3.EMBSNAME) SAWON_NM, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2) "
						   + "			    ELSE SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2)+'~'+ "
						   + "					 SUBSTRING(A.REQ_DATE2,3,2)+'.'+SUBSTRING(A.REQ_DATE2,5,2)+'.'+SUBSTRING(A.REQ_DATE2,7,2) END) AS DATE_NM, "
						   + "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN X1.G_FNM ELSE X1.G_FNM+'~'+X2.G_FNM END) AS GNMU_NM, "
						   + "        (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
						   + "              WHEN '4' THEN '진행' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
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
						   + "   FROM " + tb_nm +" A "
						   + "   LEFT OUTER JOIN DUTY_MSTGNMU X1 "
						   + "     ON A.REQ_TYPE=X1.G_CODE "
						   + "   LEFT OUTER JOIN DUTY_MSTGNMU X2 "
						   + "     ON A.REQ_TYPE2=X2.G_CODE "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						   + "     ON A.SABN=X3.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						   + "     ON X3.EMBSDPCD=X4.DEPRCODE "
						   + "  WHERE LEFT(A.REQ_DATE,6) BETWEEN '" + fr_yymm + "' AND '" + to_yymm + "'"
						   + "    AND X3.EMBSDPCD LIKE '" + dept + "'"
						   + "  ORDER BY A.REQ_DATE, A.SABN, X3.EMBSDPCD ";

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

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("DUTY_YC_BASE", dt, ref ds);
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
						   + "        X1.G_FNM+'('+(CASE A.PAY_YN WHEN 1 THEN '무급' ELSE '유급' END)+')' AS GNMU_NM, "
						   + "        (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
						   + "              WHEN '4' THEN '진행' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
						   + "        (CASE A.LINE_CNT WHEN 4 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3+' -> '+A.GW_NAME4 "
						   + "              WHEN 3 THEN A.GW_NAME1+' -> '+A.GW_NAME2+' -> '+A.GW_NAME3 "
						   + "              WHEN 2 THEN A.GW_NAME1+' -> '+A.GW_NAME2 "
						   + "              WHEN 1 THEN A.GW_NAME1 ELSE '' END) GW_LINE, "
						   + "        A.GW_NAME1+'('+A.GW_DT1+')'+(CASE WHEN A.GW_DT2<>'' THEN ' -> '+A.GW_NAME2+'('+A.GW_DT2+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT3<>'' THEN ' -> '+A.GW_NAME3+'('+A.GW_DT3+')' ELSE '' END) "
						   + "        + (CASE WHEN A.GW_DT4<>'' THEN ' -> '+A.GW_NAME4+'('+A.GW_DT4+')' ELSE '' END) AS LINE_STAT, "
						   + "        CONVERT(DATETIME,A.REQ_DATE) FR_DATE, DATEADD(DAY,1,A.REQ_DATE2) TO_DATE, "
						   + "        0 AS TYPE, 1 AS ALLDAY, (CASE A.AP_TAG WHEN '1' THEN 2 WHEN '2' THEN 1 ELSE 3 END) AS LABEL, "
						   + "        X1.G_FNM AS REMARK "
						   + "   FROM " + tb_nm + " A "
						   + "   LEFT OUTER JOIN DUTY_MSTGNMU X1 "
						   + "     ON A.REQ_TYPE=X1.G_CODE "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTEMBS X3 "
						   + "     ON A.SABN=X3.EMBSSABN "
						   + "   LEFT OUTER JOIN " + wagedb + ".dbo.MSTDEPR X4 "
						   + "     ON X3.EMBSDPCD=X4.DEPRCODE "
						   + "  WHERE LEFT(A.REQ_DATE,6) BETWEEN '" + fr_yymm + "' AND '" + to_yymm + "'"
						   + "    AND X3.EMBSDPCD LIKE '" + dept + "'"
						   + "  ORDER BY X3.EMBSDPCD, A.REQ_DATE, A.SABN ";

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
		public decimal GetHOLI_DAYS_CALCDatas(string frdt, string todt, DataSet ds)
		{
			decimal holi_days = 0;
			try
			{
				string qry = " EXEC HOLI_DAYS_CALC '" + frdt + "','" + todt + "' ";				
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
						+ "        (CASE ISNULL(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료'"
						+ "              WHEN '4' THEN '진행' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
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
						+ "        X1.G_FNM+'('+(CASE A.PAY_YN WHEN 1 THEN '무급' ELSE '유급' END)+')' AS GNMU_NM, "
						+ "        A.HOLI_DAYS AS USE_DAYS, "
						+ "        (CASE ISNULL(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료'"
						+ "              WHEN '4' THEN '진행' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
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
		
		//연차,휴가결재조회
		public void Get5060_AP_YCHG_LISTDatas(string usid, DataSet ds)
		{
			try
			{
				string qry = " SELECT A1.* FROM ( "
						   + " SELECT '1' AS GUBN, '연차' AS GUBN_NM, A.AP_TAG, A.REQ_DATE, A.SABN, '' CHK, '' C_CHK, "
						   + "        (CASE WHEN (CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "' THEN '1' ELSE '' END) CHK_STAT, "
						   + "        (CASE WHEN (CASE WHEN A.GW_DT4<>'' THEN A.GW_SABN4 WHEN A.GW_DT3<>'' THEN A.GW_SABN3 WHEN A.GW_DT2<>'' THEN A.GW_SABN2 ELSE '' END) = '" + usid + "' THEN '1' ELSE '' END) C_CHK_STAT, "
						   + "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2) "
						   + "			    ELSE SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2)+'~'+ "
						   + "					 SUBSTRING(A.REQ_DATE2,3,2)+'.'+SUBSTRING(A.REQ_DATE2,5,2)+'.'+SUBSTRING(A.REQ_DATE2,7,2) END) AS DATE_NM, "
						   + "		  RTRIM(X3.EMBSNAME) SAWON_NM, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN X1.G_FNM ELSE X1.G_FNM+'~'+X2.G_FNM END) AS GNMU_NM, A.YC_DAYS, "
						   + "        (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
						   + "              WHEN '4' THEN '진행' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
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
				if (usid == "SAMIL")
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
					+ "  SELECT '2' AS GUBN, '휴가' AS GUBN_NM, A.AP_TAG, A.REQ_DATE, A.SABN, '' CHK, '' C_CHK, "
					+ "        (CASE WHEN (CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "' THEN '1' ELSE '' END) CHK_STAT, "
					+ "        (CASE WHEN (CASE WHEN A.GW_DT4<>'' THEN A.GW_SABN4 WHEN A.GW_DT3<>'' THEN A.GW_SABN3 WHEN A.GW_DT2<>'' THEN A.GW_SABN2 ELSE '' END) = '" + usid + "' THEN '1' ELSE '' END) C_CHK_STAT, "
					+ "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2) "
					+ "			    ELSE SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2)+'~'+ "
					+ "					 SUBSTRING(A.REQ_DATE2,3,2)+'.'+SUBSTRING(A.REQ_DATE2,5,2)+'.'+SUBSTRING(A.REQ_DATE2,7,2) END) AS DATE_NM, "
					+ "		  RTRIM(X3.EMBSNAME) SAWON_NM, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
					+ "        X1.G_FNM AS GNMU_NM, A.HOLI_DAYS AS YC_DAYS, "
					+ "        (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
					+ "              WHEN '4' THEN '진행' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
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
					+ "     ON X3.EMBSDPCD=X4.DEPRCODE ";
				if (usid == "SAMIL")
				{
					qry += "  WHERE isnull(A.AP_TAG,'') IN ('','4') ";
				}
				else
				{
					qry += "  WHERE isnull(A.AP_TAG,'') IN ('','4') "
						+ "     AND ((CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "'"
						+ "      OR (CASE WHEN A.GW_DT4<>'' THEN A.GW_SABN4 WHEN A.GW_DT3<>'' THEN A.GW_SABN3 WHEN A.GW_DT2<>'' THEN A.GW_SABN2 ELSE '' END) = '" + usid + "')";
				}

				qry += " ) A1 ORDER BY A1.GUBN, A1.REQ_DATE, A1.SABN ";

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
		public void Get5060_DUTY_TRSHREQDatas(string gubn, string sabn, string sldt, DataSet ds)
		{
			try
			{
				string tb_nm = gubn == "1" ? "DUTY_TRSHREQ" : "DUTY_TRSJREQ";
				string qry = " SELECT A.* "
						   + "   FROM " + tb_nm + " A "
						   + "  WHERE A.SABN = '" + sabn + "'"
						   + "    AND A.REQ_DATE = '" + sldt + "'";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset(tb_nm, dt, ref ds);
			}
			catch (System.Exception ec)
			{
				System.Windows.Forms.MessageBox.Show("자료를 가져오는중 오류가 발생했습니다. : " + ec.Message,
													 "조회오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
		
		#region 5080 - 완결문서관리
		
		//연차,휴가결재조회
		public void Get5080_AP_YCHG_LISTDatas(string type, string usid, DataSet ds)
		{
			try
			{
				string qry = " SELECT A1.* FROM ( "
						   + " SELECT '1' AS GUBN, '연차' AS GUBN_NM, A.AP_TAG, A.REQ_DATE, A.SABN, '' CHK, '' C_CHK, "
						   + "        (CASE WHEN (CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "' THEN '1' ELSE '' END) CHK_STAT, "
						   + "        (CASE WHEN (CASE WHEN A.GW_DT4<>'' THEN A.GW_SABN4 WHEN A.GW_DT3<>'' THEN A.GW_SABN3 WHEN A.GW_DT2<>'' THEN A.GW_SABN2 ELSE '' END) = '" + usid + "' THEN '1' ELSE '' END) C_CHK_STAT, "
						   + "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2) "
						   + "			    ELSE SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2)+'~'+ "
						   + "					 SUBSTRING(A.REQ_DATE2,3,2)+'.'+SUBSTRING(A.REQ_DATE2,5,2)+'.'+SUBSTRING(A.REQ_DATE2,7,2) END) AS DATE_NM, "
						   + "		  RTRIM(X3.EMBSNAME) SAWON_NM, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
						   + "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN X1.G_FNM ELSE X1.G_FNM+'~'+X2.G_FNM END) AS GNMU_NM, A.YC_DAYS, "
						   + "        (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
						   + "              WHEN '4' THEN '진행' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
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
				if (usid == "SAMIL")
				{
					qry += "  WHERE isnull(A.AP_TAG,'') NOT IN ('','4') ";
				}
				else
				{
					qry += "  WHERE isnull(A.AP_TAG,'') IN ('1') "
						+ "     AND ((CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "'"
						+ "      OR (CASE WHEN A.GW_DT4<>'' THEN A.GW_SABN4 WHEN A.GW_DT3<>'' THEN A.GW_SABN3 WHEN A.GW_DT2<>'' THEN A.GW_SABN2 ELSE '' END) = '" + usid + "')";
				}
				qry += " UNION ALL "
					+ "  SELECT '2' AS GUBN, '휴가' AS GUBN_NM, A.AP_TAG, A.REQ_DATE, A.SABN, '' CHK, '' C_CHK, "
					+ "        (CASE WHEN (CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "' THEN '1' ELSE '' END) CHK_STAT, "
					+ "        (CASE WHEN (CASE WHEN A.GW_DT4<>'' THEN A.GW_SABN4 WHEN A.GW_DT3<>'' THEN A.GW_SABN3 WHEN A.GW_DT2<>'' THEN A.GW_SABN2 ELSE '' END) = '" + usid + "' THEN '1' ELSE '' END) C_CHK_STAT, "
					+ "        (CASE WHEN A.REQ_DATE=A.REQ_DATE2 THEN SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2) "
					+ "			    ELSE SUBSTRING(A.REQ_DATE,3,2)+'.'+SUBSTRING(A.REQ_DATE,5,2)+'.'+SUBSTRING(A.REQ_DATE,7,2)+'~'+ "
					+ "					 SUBSTRING(A.REQ_DATE2,3,2)+'.'+SUBSTRING(A.REQ_DATE2,5,2)+'.'+SUBSTRING(A.REQ_DATE2,7,2) END) AS DATE_NM, "
					+ "		  RTRIM(X3.EMBSNAME) SAWON_NM, RTRIM(ISNULL(X4.DEPRNAM1,'')) AS DEPT_NM, "
					+ "        X1.G_FNM AS GNMU_NM, A.HOLI_DAYS AS YC_DAYS, "
					+ "        (CASE isnull(A.AP_TAG,'') WHEN '' THEN '신청' WHEN '1' THEN '승인' WHEN '2' THEN '취소' WHEN '3' THEN '완료' "
					+ "              WHEN '4' THEN '진행' WHEN '9' THEN '정산' ELSE '' END) AP_TAG_NM, "
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
					+ "     ON X3.EMBSDPCD=X4.DEPRCODE ";
				if (usid == "SAMIL")
				{
					qry += "  WHERE isnull(A.AP_TAG,'') NOT IN ('','4') ";
				}
				else
				{
					qry += "  WHERE isnull(A.AP_TAG,'') IN ('1') "
						+ "     AND ((CASE WHEN A.GW_DT2='' THEN A.GW_SABN2 WHEN A.GW_DT3='' THEN A.GW_SABN3 WHEN A.GW_DT4='' THEN A.GW_SABN4 ELSE '' END) = '" + usid + "'"
						+ "      OR (CASE WHEN A.GW_DT4<>'' THEN A.GW_SABN4 WHEN A.GW_DT3<>'' THEN A.GW_SABN3 WHEN A.GW_DT2<>'' THEN A.GW_SABN2 ELSE '' END) = '" + usid + "')";
				}

				qry += " ) A1 "
					+ " WHERE A1.GUBN LIKE '" + type + "'"
					+ " ORDER BY A1.GUBN, A1.REQ_DATE DESC, A1.SABN ";

				DataTable dt = gd.GetDataInQuery(clib.TextToInt(DataAccess.DBtype), dbname, qry);
				dp.AddDatatable2Dataset("5080_AP_YCHG_LIST", dt, ref ds);
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