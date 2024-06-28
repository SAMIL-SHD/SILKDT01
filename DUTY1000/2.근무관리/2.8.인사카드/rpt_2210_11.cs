using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraPrinting;

namespace DUTY1000
{
    public partial class rpt_2210_11 : DevExpress.XtraReports.UI.XtraReport
    {
        SilkRoad.Common.CustomFormatter cf = new SilkRoad.Common.CustomFormatter();
        SilkRoad.Common.DrawRptSignBox dsb = new SilkRoad.Common.DrawRptSignBox();

        public rpt_2210_11()
        {
            InitializeComponent();
        }

        private void xrTableCell2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell2.Text = GetCurrentColumnValue("AWDIFDAT").ToString() == "" ? "" :
                                GetCurrentColumnValue("AWDIFDAT").ToString().Substring(0, 4) + "."
                              + GetCurrentColumnValue("AWDIFDAT").ToString().Substring(4, 2) + "."
                              + GetCurrentColumnValue("AWDIFDAT").ToString().Substring(6, 2) + " ~ "
                              + GetCurrentColumnValue("AWDITDAT").ToString().Substring(0, 4) + "."
                              + GetCurrentColumnValue("AWDITDAT").ToString().Substring(4, 2) + "."
                              + GetCurrentColumnValue("AWDITDAT").ToString().Substring(6, 2) ;
        }

		private void rpt_2210_11_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
				xrTableCell1.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell2.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell5.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell7.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell8.Borders = BorderSide.Bottom | BorderSide.Right;
		}
	}
}
