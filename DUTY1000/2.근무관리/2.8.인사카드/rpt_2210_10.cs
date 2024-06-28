using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraPrinting;

namespace DUTY1000
{
    public partial class rpt_2210_10 : DevExpress.XtraReports.UI.XtraReport
    {
        SilkRoad.Common.CustomFormatter cf = new SilkRoad.Common.CustomFormatter();
        SilkRoad.Common.DrawRptSignBox dsb = new SilkRoad.Common.DrawRptSignBox();

        public rpt_2210_10()
        {
            InitializeComponent();
        }

        private void xrTableCell5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell5.Text = GetCurrentColumnValue("TRAIFDAT").ToString() == "" ? "" :
               GetCurrentColumnValue("TRAIFDAT").ToString().Substring(0, 4) + "." +
               GetCurrentColumnValue("TRAIFDAT").ToString().Substring(4, 2) + "." +
               GetCurrentColumnValue("TRAIFDAT").ToString().Substring(6, 2) + " ~ " +
               GetCurrentColumnValue("TRAITDAT").ToString().Substring(0, 4) + "." +
               GetCurrentColumnValue("TRAITDAT").ToString().Substring(4, 2) + "." +
               GetCurrentColumnValue("TRAITDAT").ToString().Substring(6, 2);
        }

        private void xrTableCell1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell1.Text = GetCurrentColumnValue("TRAIILSU").ToString() == "" ? "" :
                GetCurrentColumnValue("TRAIILSU").ToString() + " 일";
        }

		private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
				xrTableCell1.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell2.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell5.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell7.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell8.Borders = BorderSide.Bottom | BorderSide.Right;

		}
	}
}
