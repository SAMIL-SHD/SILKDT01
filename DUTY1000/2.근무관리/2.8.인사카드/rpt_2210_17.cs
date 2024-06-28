using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraPrinting;

namespace DUTY1000
{
    public partial class rpt_2210_17 : DevExpress.XtraReports.UI.XtraReport
    {
        SilkRoad.Common.CustomFormatter cf = new SilkRoad.Common.CustomFormatter();
        SilkRoad.Common.DrawRptSignBox dsb = new SilkRoad.Common.DrawRptSignBox();

        public rpt_2210_17()
        {
            InitializeComponent();
        }

        private void xrTableCell2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell2.Text = GetCurrentColumnValue("BGSHFDAY").ToString() == "" ? "" :
                GetCurrentColumnValue("BGSHFDAY").ToString().Substring(0, 4) + "."
                + GetCurrentColumnValue("BGSHFDAY").ToString().Substring(4, 2) + "."
                + GetCurrentColumnValue("BGSHFDAY").ToString().Substring(6, 2) + " ~ "
                + GetCurrentColumnValue("BGSHTDAY").ToString().Substring(0, 4) + "."
                + GetCurrentColumnValue("BGSHTDAY").ToString().Substring(4, 2) + "."
                + GetCurrentColumnValue("BGSHTDAY").ToString().Substring(6, 2);
        }

        private void xrTableCell8_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GetCurrentColumnValue("BGSHGUBN").ToString() == "0")
            {
                xrTableCell8.Text = "무급";
            }
            else if (GetCurrentColumnValue("BGSHGUBN").ToString() == "1")
            {
                xrTableCell8.Text = "유급";
            }
            else
            {
                xrTableCell8.Text = "";
            }

        }

		private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			if (this.GetCurrentRow().Equals(this.GetNextRow()))
			{
				xrTableCell1.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell2.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell8.Borders = BorderSide.Bottom | BorderSide.Right;

			}
		}
	}
}
