namespace NUnit.GUI 
{
	using System;
	using System.IO;
	using System.Text;
	using System.Windows.Forms;

	public class TextBoxWriter: TextWriter 
	{
		private TextBoxBase fTextBox;
		
		private TextBoxWriter(){}
    			
		public TextBoxWriter(TextBoxBase textBox) 
		{
			if(textBox!=null)
			{
				fTextBox=textBox;
			}
			else
				throw new ArgumentNullException("textBox");
		}	
		
		public override Encoding Encoding 
		{
			get{ return Encoding.Unicode; }
		}
		
		public override void Write(char c) 
		{
			fTextBox.AppendText(c.ToString());
		}
		
		public override void Write(char[] text)
		{
			this.Write(text,0,text.Length);
		}

		public override void Write(char[] text,int index, int count)
		{
			if(text!=null)
			{
				fTextBox.AppendText(new string(text,index,count));
			}		
		}

		public override void Write(String text) 
		{
			if(text!=null)
				fTextBox.AppendText(text);
		}
	}
}
