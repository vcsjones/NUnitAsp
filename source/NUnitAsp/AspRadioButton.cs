using System;

namespace NUnit.Extensions.Asp
{
	public class AspRadioButton : AspControl
	{
		public AspRadioButton(string aspId, Control container) : base(aspId, container)
		{
		}

		public bool Checked
		{
			get
			{
				return GetOptionalAttributeValue("checked") != "";
			}
			set
			{
				if (value == true) 
				{
					EnterInputValue(GetAttributeValue("name"), GetAttributeValue("value"));
				}
				else
				{
					throw new ApplicationException("not implemented");
				}
			}
		}
	}
}
