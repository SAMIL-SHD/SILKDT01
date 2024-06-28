using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraPrinting;

namespace DUTY1000
{
    public partial class rpt_2210_14 : DevExpress.XtraReports.UI.XtraReport
    {
        SilkRoad.Common.CustomFormatter cf = new SilkRoad.Common.CustomFormatter();
        SilkRoad.Common.DrawRptSignBox dsb = new SilkRoad.Common.DrawRptSignBox();

        public rpt_2210_14()
        {
            InitializeComponent();
        }

		private void xrTableCell2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			if (GetCurrentColumnValue("ESTMGUBN").ToString() == "1")
			{
				xrTableCell2.Text = GetCurrentColumnValue("ESTMYEAR").ToString()+ "상반기";
			}
			else if(GetCurrentColumnValue("ESTMGUBN").ToString() == "2")
			{
				xrTableCell2.Text = GetCurrentColumnValue("ESTMYEAR").ToString() + "하반기";
			}
		}

		private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
				xrTableCell1.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell2.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell3.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell4.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell5.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell6.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell8.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell9.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell10.Borders = BorderSide.Bottom | BorderSide.Right;
		}
	}
}
