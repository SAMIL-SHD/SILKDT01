using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraPrinting;

namespace DUTY1000
{
    public partial class rpt_2210_4 : DevExpress.XtraReports.UI.XtraReport
    {
        SilkRoad.Common.CustomFormatter cf = new SilkRoad.Common.CustomFormatter();
        SilkRoad.Common.DrawRptSignBox dsb = new SilkRoad.Common.DrawRptSignBox();

        public rpt_2210_4()
        {
            InitializeComponent();
        }

        private void xrTableCell6_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell6.Text = GetCurrentColumnValue("PLICCDAY").ToString().Trim() == "" ? ""
                : GetCurrentColumnValue("PLICCDAY").ToString().Substring(0, 4) + "."
                + GetCurrentColumnValue("PLICCDAY").ToString().Substring(4, 2) + "."
                + GetCurrentColumnValue("PLICCDAY").ToString().Substring(6, 2);

        }
        private void xrTableCell1_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell1.Text = GetCurrentColumnValue("PLICDDAY").ToString().Trim() == "" ? ""
                : GetCurrentColumnValue("PLICDDAY").ToString().Substring(0, 4) + "."
                + GetCurrentColumnValue("PLICDDAY").ToString().Substring(4, 2) + "."
                + GetCurrentColumnValue("PLICDDAY").ToString().Substring(6, 2);
        }

        private void xrTableCell7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell7.Text = GetCurrentColumnValue("PLICEDAY").ToString().Trim() == "" ? ""
                : GetCurrentColumnValue("PLICEDAY").ToString().Substring(0, 4) + "."
                + GetCurrentColumnValue("PLICEDAY").ToString().Substring(4, 2) + "."
                + GetCurrentColumnValue("PLICEDAY").ToString().Substring(6, 2);
        }

		private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			if (this.GetCurrentRow().Equals(this.GetNextRow()))
			{
				xrTableCell1.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell2.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell4.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell5.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell6.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell7.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell8.Borders = BorderSide.Bottom | BorderSide.Right;
			}
		}
	}
}
