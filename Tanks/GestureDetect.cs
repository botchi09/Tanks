using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework.Input.Touch;

namespace Tanks
{
	class GestureDetect
	{
		/*
		 * Ref: http://stackoverflow.com/a/15155929
		 */

		bool isDragging = false;
		double lastUpdateTime = 0;
		int totalDragUpdates = 0;
		float totalVelocity = 0;

		private void resetDrag()
		{
			isDragging = false;
			totalVelocity = 0;
			totalDragUpdates = 0;
			lastUpdateTime = 0;
			System.Diagnostics.Debug.WriteLine("Resetting variables");

		}

		public DetectedGesture getGesture(double time)
		{


			GestureSample gs = TouchPanel.ReadGesture();
			DetectedGesture detectedGs = new DetectedGesture();

			detectedGs.Delta = gs.Delta;
			detectedGs.Delta2 = gs.Delta2;
			detectedGs.Position = gs.Position;
			detectedGs.Position2 = gs.Position2;
			detectedGs.Timestamp = gs.Timestamp;


			switch (gs.GestureType)
			{
				case GestureType.Tap:
					System.Diagnostics.Debug.WriteLine("Tap!");
					break;

				case GestureType.FreeDrag:
					isDragging = true;
					totalVelocity += gs.Delta.LengthSquared(); //LengthSquared is more efficient than Length

					//Test every 150ms to see if velocity warrants a flick, or a drag.
					if (lastUpdateTime + 150 <= time) //150ms update
					{
						totalDragUpdates += 1;
						lastUpdateTime = time;
						System.Diagnostics.Debug.WriteLine("Incrementing counter to " + totalDragUpdates);

					}

					if (totalDragUpdates > 1) //User has spent more than 150ms dragging. Assume user means to Drag and draw accordingly.
					{

						detectedGs.GestureType = GestureType.FreeDrag;
						return detectedGs;
					}

					break;

				case GestureType.DragComplete:
					System.Diagnostics.Debug.WriteLine("Drag complete");

					if (totalDragUpdates > 1)
					{
						System.Diagnostics.Debug.WriteLine("Free drag detected.");

						detectedGs.GestureType = GestureType.DragComplete;
						return detectedGs;
					}
					else
					{
						System.Diagnostics.Debug.WriteLine("Flick detected");

						detectedGs.GestureType = GestureType.Flick;
						return detectedGs;
					}

				default:
					resetDrag();
					break;
			}

			return GestureType.None;
		}
	}
}
