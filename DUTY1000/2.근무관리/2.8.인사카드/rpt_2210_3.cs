using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraPrinting;

namespace DUTY1000
{
    public partial class rpt_2210_3 : DevExpress.XtraReports.UI.XtraReport
    {
        SilkRoad.Common.CustomFormatter cf = new SilkRoad.Common.CustomFormatter();
        SilkRoad.Common.DrawRptSignBox dsb = new SilkRoad.Common.DrawRptSignBox();

        public rpt_2210_3()
        {
            InitializeComponent();
        }

        private void xrTableCell4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell4.Text = GetCurrentColumnValue("HISTFDAY").ToString() == "" ? "" :
                GetCurrentColumnValue("HISTFDAY").ToString().Substring(0, 4)
              + "." + GetCurrentColumnValue("HISTFDAY").ToString().Substring(4, 2)
              + "." + GetCurrentColumnValue("HISTFDAY").ToString().Substring(6, 2)
              + "~" + GetCurrentColumnValue("HISTTDAY").ToString().Substring(0, 4)
              + "." + GetCurrentColumnValue("HISTTDAY").ToString().Substring(4, 2)
              + "." + GetCurrentColumnValue("HISTTDAY").ToString().Substring(6, 2);
        }

        private void xrTableCell7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (((XRTableCell)sender).Text == "0")
            {
                ((XRTableCell)sender).Text = "";
            }
            if (((XRTableCell)sender).Text == "0.0")
            {
                ((XRTableCell)sender).Text = "";
            }
            if (((XRTableCell)sender).Text == "0.00")
            {
                ((XRTableCell)sender).Text = "";
            }
        }

		private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			if (this.GetCurrentRow().Equals(this.GetNextRow()))
			{
				xrTableCell1.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell2.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell3.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell4.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell5.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell7.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell8.Borders = BorderSide.Bottom | BorderSide.Right;
			}
		}
	}
}
