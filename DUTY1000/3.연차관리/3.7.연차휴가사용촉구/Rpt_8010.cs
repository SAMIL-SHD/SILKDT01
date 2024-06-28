using System;
using System.Drawing;
using System.Data;
using System.IO;

namespace DUTY1000
{
	public partial class Rpt_8010 : DevExpress.XtraReports.UI.XtraReport
	{
		SilkRoad.Common.CustomFormatter cf = new SilkRoad.Common.CustomFormatter();
		SilkRoad.Common.DrawRptSignBox dsb = new SilkRoad.Common.DrawRptSignBox();
		DataSet ds;

		public Rpt_8010(string sabn, string doc_type, string yc_sq, string comp_nm, DataSet ds)
		{
			InitializeComponent();
			DataRow drow = ds.Tables["SEARCH_8010"].Select("SAWON_NO = '" + sabn + "' AND DOC_TYPE = '" + doc_type + "'  AND YC_SQ = " + yc_sq + "")[0];
			xrTableCell41.Text = drow["SAWON_NM"].ToString(); //이름
			xrTableCell51.Text = drow["YC_IPDT"].ToString().Substring(0, 4)+"."+drow["YC_IPDT"].ToString().Substring(4, 2)+"."+drow["YC_IPDT"].ToString().Substring(6, 2); //입사일
			string calc_frdt = drow["CALC_FRDT"].ToString().Substring(0, 4) + "." + drow["CALC_FRDT"].ToString().Substring(4, 2) + "." + drow["CALC_FRDT"].ToString().Substring(6, 2);
			string calc_todt = drow["CALC_TODT"].ToString().Substring(0, 4) + "." + drow["CALC_TODT"].ToString().Substring(4, 2) + "." + drow["CALC_TODT"].ToString().Substring(6, 2);
			xrTableCell23.Text = calc_frdt + " ~ " + calc_todt; //산정기간
			xrTableCell8.Text = drow["USE_FRDT"].ToString().Substring(0, 4)+"."+drow["USE_FRDT"].ToString().Substring(4, 2)+"."+drow["USE_FRDT"].ToString().Substring(6, 2); //발생시점
			xrTableCell12.Text = string.Format("{0:#,##0.###}", Convert.ToDecimal(drow["YC_TDAY"].ToString())) + " 일"; //발생연차 휴가일수
			string use_frdt = drow["USE_FRDT"].ToString().Substring(0, 4) + "." + drow["USE_FRDT"].ToString().Substring(4, 2) + "." + drow["USE_FRDT"].ToString().Substring(6, 2);
			string use_todt = drow["USE_TODT"].ToString().Substring(0, 4) + "." + drow["USE_TODT"].ToString().Substring(4, 2) + "." + drow["USE_TODT"].ToString().Substring(6, 2);
			xrTableCell14.Text = use_frdt + " ~ " + use_todt; //사용기간
			xrTableCell66.Text = string.Format("{0:#,##0.###}", Convert.ToDecimal(drow["YC_USE_DAY"].ToString())); //사용연차 일수
			xrTableCell72.Text = string.Format("{0:#,##0.###}", Convert.ToDecimal(drow["YC_REMAIN_DAY"].ToString())); //남은연차 일수

            xrTableCell24.Text = "상기인은 현재 일의 연차휴가 중 " + string.Format("{0:#,##0.###}", Convert.ToDecimal(drow["YC_USE_DAY"].ToString())) + "일의 연차휴가를 사용하여 " + use_todt + "까지 " + string.Format("{0:#,##0.###}", Convert.ToDecimal(drow["YC_REMAIN_DAY"].ToString())) + "일의";
            //xrTableCell3.Text = use_todt + "까지 " + string.Format("{0:#,##0.###}", Convert.ToDecimal(drow["YC_REMAIN_DAY"].ToString())) + "일의 연차휴가를 추가로 사용할 수 있습니다.";
			xrTableCell1.Text = "확인자 " + drow["SAWON_NM"].ToString();
			xrTableCell2.Text = comp_nm + " 대표";

            byte[] imgdata = null;
			if (ds.Tables["SEARCH_8010"].Rows.Count > 0)
			{
				if (ds.Tables["SEARCH_8010"].Select("SAWON_NO = '" + sabn + "' AND DOC_TYPE = '" + doc_type + "'  AND YC_SQ = " + yc_sq + "")[0]["SAWON_SIGN"].ToString().Trim() != "")
				{
					imgdata = (byte[])(ds.Tables["SEARCH_8010"].Select("SAWON_NO = '" + sabn + "' AND DOC_TYPE = '" + doc_type + "'  AND YC_SQ = " + yc_sq + "")[0]["SAWON_SIGN"]);
					MemoryStream ms = new MemoryStream(imgdata);
					Bitmap bitmap = new Bitmap(ms);
					Bitmap reSizeBitmap = new Bitmap(bitmap, 300, 200);

					xrPictureBox1.Image = reSizeBitmap;
				}
			}
		}	

	}
}
