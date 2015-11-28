using System;
using Foundation;
using System.ComponentModel;
using UIKit;
using CoreGraphics;

namespace BinaryTreeApp
{
	[Register("CircleView"), DesignTimeVisible(true)]
	public class CircleView  : UIView
	{
		private string _text;
		[Export("Text"), Browsable(true)]
		public string Text
		{
			get { return _text; }
			set
			{
				_text = value;
				SetNeedsDisplay();
			}
		}

		public CircleView(IntPtr p)
			: base(p)
		{
			Initialize ();
		}
		public CircleView ()
		{
			Initialize ();
		}

		void Initialize()
		{
			this.BackgroundColor = UIColor.Clear;
			this.Layer.CornerRadius = this.Bounds.Width/2;
			this.Layer.BackgroundColor = UIColor.White.CGColor;
		}

		public override void Draw(CGRect rect)
		{
			base.Draw(rect);

			using (var g = UIGraphics.GetCurrentContext())
			{
				float fontSize = 16f;
				g.TranslateCTM (0, fontSize);

				UIColor.Black.SetFill();
				g.ScaleCTM(1f,-1f);
				g.SelectFont ("Helvetica", fontSize, CGTextEncoding.MacRoman);
				g.SetTextDrawingMode(CGTextDrawingMode.Fill);
				g.ShowTextAtPoint(rect.Height/2 - 10 , -rect.Width/2 + 10,Text);
			}
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			this.Layer.CornerRadius = this.Bounds.Width/2;
		}
	}
}

