using System;
using System.Linq;
using UIKit;
using CoreGraphics;
using Foundation;

namespace BinaryTreeApp
{
	public partial class ViewController : UIViewController
	{
		BinarySearchTreeView binarySearchTreeView;
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.

			binarySearchTreeView = new BinarySearchTreeView ();
			binarySearchTreeView.Frame = new CGRect(0,0, DropableCanvas.Frame.Width, DropableCanvas.Frame.Height);
			ScrollView.Add (binarySearchTreeView);
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		partial void GenerateNodesBtn_TouchUpInside (UIButton sender)
		{
			//Generate a random 5 numbers which are not in the tree

			var fiveRandomValues = new RandomValueGenerator().GetRandomNumbers(5);

			//Clear the existing subviews of the container
			foreach(var subview in NodesContainerView.Subviews)
			{
				subview.RemoveFromSuperview();
			}
			// Add new subviews to the container

			var frameToSet = new CGRect(10,15,50,50);

			foreach(var randomValue in fiveRandomValues)
			{
				CircleView circleView = new CircleView();
				circleView.Frame = frameToSet;
				circleView.Text = Convert.ToString(randomValue);
				NodesContainerView.Add(circleView);
				frameToSet.X = frameToSet.X + 70;
			}
		}

		#region Touch Events

		CircleView currentMovingNode;
		CGRect currentMovingNodeOriginalPosition;
		bool nodeIsMoving;
		public override void TouchesBegan (Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
			var touch = touches.AnyObject as UITouch;

			if (touch == null) 
				return;

			var locationinContainerView = touch.LocationInView (NodesContainerView);

			//check which subview falls under this
			currentMovingNode = NodesContainerView.Subviews.FirstOrDefault(node=>node.Frame.Contains(locationinContainerView)) as CircleView;
			if (currentMovingNode != null) {
				nodeIsMoving = true;
				currentMovingNodeOriginalPosition = currentMovingNode.Frame;
			}
		}

		public override void TouchesMoved (Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);
			var touch = touches.AnyObject as UITouch;

			if (touch == null || nodeIsMoving == false || currentMovingNode == null) 
				return;

			nfloat deltaX = touch.PreviousLocationInView(NodesContainerView).X - touch.LocationInView(NodesContainerView).X;
			nfloat deltaY = touch.PreviousLocationInView(NodesContainerView).Y - touch.LocationInView(NodesContainerView).Y;

			var newPoint = new CGPoint (currentMovingNode.Frame.X - deltaX, currentMovingNode.Frame.Y - deltaY);

			currentMovingNode.Frame = new CGRect(newPoint, currentMovingNode.Frame.Size);
		}

		public override void TouchesEnded (Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);
			var touch = touches.AnyObject as UITouch;

			if (touch == null || nodeIsMoving == false || currentMovingNode == null) 
				return;

			if (DropableCanvas.Frame.Contains (currentMovingNode.Center) == true) 
			{
				//Remove this view from superview
				currentMovingNode.RemoveFromSuperview();

				binarySearchTreeView.InsertNode (int.Parse (currentMovingNode.Text));
			} 
			else 
			{
				currentMovingNode.Frame = currentMovingNodeOriginalPosition;
			}

			currentMovingNode = null;
			currentMovingNodeOriginalPosition = CGRect.Empty;
			nodeIsMoving = false;

		}

		public override void TouchesCancelled (NSSet touches, UIEvent evt)
		{
			base.TouchesCancelled (touches, evt);

			nodeIsMoving = false;
		}


		partial void ClearAllBtn_TouchUpInside (UIButton sender)
		{
			binarySearchTreeView.ClearAll();
		}
		#endregion
	}
}

