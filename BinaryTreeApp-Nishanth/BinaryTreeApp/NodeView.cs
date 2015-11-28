using System;
using Foundation;

namespace BinaryTreeApp
{
	[Register("NodeView")]
	public class NodeView : CircleView
	{
		public NodeView (int data)
		{
			Data = data;
		}

		public NodeView (IntPtr p,int data) :base(p)
		{
			Data = data;
		}

		public NodeView(IntPtr p): this(p,0){}

		public int Data 
		{
			get {
				if (string.IsNullOrEmpty (Text))
					return -1;
				else
					return int.Parse (Text);
			}
			set
			{
				Text = Convert.ToString(value);
			}
		}

		public NodeView LeftChild { get; set;}

		public NodeView RightChild { get; set;}
	}
}

