using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SilkRoad.Common;
using SilkRoad.DAL;
using SilkRoad.DataProc;

namespace WAGE1000
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
				
		#region 1006 - 인사기본관리
		
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
		
    }
}
