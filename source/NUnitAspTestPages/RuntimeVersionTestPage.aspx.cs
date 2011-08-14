using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUnitAspTestPagesv2
{
    public partial class RuntimeVersionTestPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RuntimeVersion.Text = Environment.Version.ToString();
        }
    }
}