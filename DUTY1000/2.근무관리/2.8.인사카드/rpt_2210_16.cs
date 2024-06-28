using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraPrinting;

namespace DUTY1000
{
    public partial class rpt_2210_16 : DevExpress.XtraReports.UI.XtraReport
    {
        SilkRoad.Common.CustomFormatter cf = new SilkRoad.Common.CustomFormatter();
        SilkRoad.Common.DrawRptSignBox dsb = new SilkRoad.Common.DrawRptSignBox();

        public rpt_2210_16()
        {
            InitializeComponent();
        }

        private void xrTableCell2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell2.Text = GetCurrentColumnValue("HJSHFDAY").ToString() == "" ? "" :
                GetCurrentColumnValue("HJSHFDAY").ToString().Substring(0, 4) + "."
                + GetCurrentColumnValue("HJSHFDAY").ToString().Substring(4, 2) + "."
                + GetCurrentColumnValue("HJSHFDAY").ToString().Substring(6, 2) + " ~ "
                + GetCurrentColumnValue("HJSHTDAY").ToString().Substring(0, 4) + "."
                + GetCurrentColumnValue("HJSHTDAY").ToString().Substring(4, 2) + "."
                + GetCurrentColumnValue("HJSHTDAY").ToString().Substring(6, 2);
        }

        private void xrTableCell8_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GetCurrentColumnValue("HJSHGUBN").ToString() == "0")
            {
                xrTableCell8.Text = "무급";
            }
            else if (GetCurrentColumnValue("HJSHGUBN").ToString() == "1")
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
