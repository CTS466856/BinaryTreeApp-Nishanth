using System;
using Foundation;
using System.ComponentModel;
using UIKit;
using CoreGraphics;

namespace BinaryTreeApp
{
	[Register ("BinarySearchTreeView"), DesignTimeVisible (true)]
	public class BinarySearchTreeView : UIView
	{
		const float NodeHeight = 50;
		const float NodeWidth = 50;
		const float HeightDistanceBetweenLevels = 50;

		const float WidthDistanceBetweenAdjacentNodes = 50;

		NodeView root, current;

		public BinarySearchTreeView ()
		{
			initialize ();
		}

		public BinarySearchTreeView (IntPtr p) : base (p)
		{
			initialize ();
		}

		void initialize ()
		{
			this.BackgroundColor = UIColor.LightGray;
		}

		public void InsertNode (int data)
		{
			if (Contains (data))
				return;

			var newNode = new NodeView (data);
			AddSubview (newNode);

			if (root == null) {
				root = newNode;
				return;
			}

			current = root;

			while (true) {
				if (newNode.Data > current.Data) {
					if (current.RightChild == null) {
						current.RightChild = newNode;
						break;
					}
					current = current.RightChild;
				} else {
					if (current.LeftChild == null) {
						current.LeftChild = newNode;
						break;
					}  
					current = current.LeftChild;
				}
			}

			//Set the current frame
			var treeHeight = HeightOfTheNode (root) + 1;

			var frameHeightToSet = treeHeight * NodeHeight + (treeHeight - 1) * HeightDistanceBetweenLevels;


			//	********************	edit start 0	*********************	//

//			var frameWidthToSet = Math.Pow (2, treeHeight) * NodeWidth;
//			this.Frame = new CoreGraphics.CGRect (0, 0, frameWidthToSet, frameHeightToSet);

			var Editedtreewidth = Math.Pow (2, treeHeight);
			var Editedframewidthtoset = Editedtreewidth * NodeWidth + (Editedtreewidth - 1) * WidthDistanceBetweenAdjacentNodes;

			this.Frame = new CoreGraphics.CGRect (0, 0, Editedframewidthtoset, frameHeightToSet);

			//	********************	edit end 0	*********************	//


			var parent = (this.Superview as UIScrollView);

			if (parent != null) {
				parent.ContentSize = new CGSize (Frame.Width, Frame.Height);
			}

			SetNeedsLayout ();
			SetNeedsDisplay ();
		}

		public int HeightOfTheNode (NodeView node)
		{
			if (node == null)
				return -1;
			else
				return Math.Max (HeightOfTheNode (node.LeftChild), HeightOfTheNode (node.RightChild)) + 1;
		}

		public override void Draw (CoreGraphics.CGRect rect)
		{
			base.Draw (rect);
			Console.WriteLine ("Draw");

			if (root != null) {
				//start from root and draw lines to the children
				using (var g = UIGraphics.GetCurrentContext ()) {
					UIColor.Red.SetStroke ();
					//g.SetStrokeColor (UIColor.Red.CGColor);
					g.SetLineWidth (2f);

					drawLinesToChildren (g, root);
				}
			}
		}

		void drawLinesToChildren (CGContext context, NodeView node)
		{
			if (node.LeftChild != null) {
				context.MoveTo (node.Frame.X + NodeWidth / 2, node.Frame.Y + NodeHeight);
				context.AddLineToPoint (node.LeftChild.Frame.X + NodeWidth / 2, node.LeftChild.Frame.Y);
				context.StrokePath ();
				drawLinesToChildren (context, node.LeftChild);
			}

			if (node.RightChild != null) {
				context.MoveTo (node.Frame.X + NodeWidth / 2, node.Frame.Y + NodeHeight);
				context.AddLineToPoint (node.RightChild.Frame.X + NodeWidth / 2, node.RightChild.Frame.Y);
				context.StrokePath ();
				drawLinesToChildren (context, node.RightChild);
			}
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			Console.WriteLine ("Layout");

			//start with root node and set the frame for its children
			var treeHeight = HeightOfTheNode (root);

			//	********************	edit start 1	*********************	//

//			if (treeHeight != -1) {
//				SetFrame (root, new CGRect (((Math.Pow (2, treeHeight) / 2)) * 2 * NodeWidth, 0, NodeWidth, NodeHeight));
//
//				var parent = (this.Superview as UIScrollView);
//				if (parent != null) {
//					parent.ScrollRectToVisible (new CGRect(root.Frame.X + 150 , root.Frame.Y,root.Frame.Width,root.Frame.Height), true);
//				}
//			}

			if (treeHeight != -1) {

				// root is not null
				// set frame for root

				EditedSetFrame (root,
					new CGRect (
						Math.Pow(2,HeightOfTheNode(root)) * ( WidthDistanceBetweenAdjacentNodes + NodeWidth ),
						0,
						NodeWidth,
						NodeHeight
					)
				);
				var parent = (this.Superview as UIScrollView);
				if (parent != null) {
					parent.ScrollRectToVisible (new CGRect (root.Frame.X + 150, root.Frame.Y, root.Frame.Width, root.Frame.Height), true);
				}


			}

			//	********************	edit end 1	*********************	//

			SetNeedsDisplay ();
		}

		//	********************	edit start 2	*********************	//

		void EditedSetFrame (NodeView node, CGRect frame)
		{

			node.Frame = frame;

			if (node.LeftChild != null) {

				EditedSetFrame (node.LeftChild, new CGRect (frame.X - (WidthDistanceBetweenAdjacentNodes * (Math.Pow(2,HeightOfTheNode(node.LeftChild)))), frame.Y + (NodeHeight + HeightDistanceBetweenLevels), NodeWidth, NodeHeight));
			}

			if (node.RightChild != null) {

				EditedSetFrame (node.RightChild, new CGRect (frame.X + (WidthDistanceBetweenAdjacentNodes * (Math.Pow(2,HeightOfTheNode(node.RightChild)))), frame.Y + (NodeHeight + HeightDistanceBetweenLevels), NodeWidth, NodeHeight));
			}

		}

		//	********************	edit end 2	*********************	//


		void SetFrame (NodeView node, CGRect frame)
		{
			node.Frame = frame;

			if (node.LeftChild != null) {
				var nodeHeight = HeightOfTheNode (node.LeftChild) + 1;

				var nodeHeightForParentRight = HeightOfTheNode (node.RightChild) + 1;

				if (nodeHeightForParentRight > nodeHeight)
					nodeHeight = nodeHeightForParentRight;
					
				SetFrame (node.LeftChild, new CGRect (frame.X - (nodeHeight * NodeWidth), frame.Y + NodeHeight + HeightDistanceBetweenLevels, NodeWidth, NodeHeight));
			}
					
			if (node.RightChild != null) {
				var nodeHeight = HeightOfTheNode (node.RightChild) + 1;

				var nodeHeightForParentLeft = HeightOfTheNode (node.LeftChild) + 1;

				if (nodeHeightForParentLeft > nodeHeight)
					nodeHeight = nodeHeightForParentLeft;
				
				SetFrame (node.RightChild, new CGRect (frame.X + (NodeWidth * nodeHeight), frame.Y + NodeHeight + HeightDistanceBetweenLevels, NodeWidth, NodeHeight));
			}
		}

		bool Contains (int data)
		{
			current = root; 
			while (true) {
				if (current == null)
					return false;
				
				if (current.Data == data)
					return true;

				if (data > current.Data)
					current = current.RightChild;
				else
					current = current.LeftChild;
			}
		}

		public void ClearAll ()
		{
			foreach (var subview in Subviews)
				subview.RemoveFromSuperview ();

			root = null;

			SetNeedsDisplay ();
		}
	}
}

