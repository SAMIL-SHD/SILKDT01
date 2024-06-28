using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace DUTY1000
{
    public partial class rpt_506a1 : DevExpress.XtraReports.UI.XtraReport
    {
        public rpt_506a1(string title)
        {
            InitializeComponent();

            xrLabel1.Text = title;
			s_qt1.DataBindings.Add("Text", DataSource, "CALL_CNT", "");
		}
    }
}
