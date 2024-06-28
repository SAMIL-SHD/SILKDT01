using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraPrinting;

namespace DUTY1000
{
    public partial class rpt_2210_7 : DevExpress.XtraReports.UI.XtraReport
    {
        SilkRoad.Common.CustomFormatter cf = new SilkRoad.Common.CustomFormatter();
        SilkRoad.Common.DrawRptSignBox dsb = new SilkRoad.Common.DrawRptSignBox();

        public rpt_2210_7()
        {
            InitializeComponent();
        }

        private void xrTableCell2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell2.Text = GetCurrentColumnValue("MOVE_DATE").ToString() == "" ? "" :
                                GetCurrentColumnValue("MOVE_DATE").ToString().Substring(0, 4) + "."
                              + GetCurrentColumnValue("MOVE_DATE").ToString().Substring(4, 2) + "."
                              + GetCurrentColumnValue("MOVE_DATE").ToString().Substring(6, 2);
        }

		private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			if (this.GetCurrentRow().Equals(this.GetNextRow()))
			{
				xrTableCell1.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell2.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell3.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell5.Borders = BorderSide.Bottom | BorderSide.Right;
			}
		}
	}
}
