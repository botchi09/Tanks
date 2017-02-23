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
		GestureType detectedGesture;

		private void resetDrag()
		{
			isDragging = false;
			totalVelocity = 0;
			totalDragUpdates = 0;
			lastUpdateTime = 0;
			System.Diagnostics.Debug.WriteLine("Resetting variables");

		}

		public void getGesture(double time)
		{
			try
			{
				GestureSample gs = TouchPanel.ReadGesture();
				TouchCollection touchCol = TouchPanel.GetState();

				switch(gs.GestureType)
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
							detectedGesture = GestureType.FreeDrag;
						}

						break;

					case GestureType.DragComplete:
						System.Diagnostics.Debug.WriteLine("Drag complete");

						if (totalDragUpdates > 1)
						{
							System.Diagnostics.Debug.WriteLine("Free drag detected.");
						}
						else
						{
							System.Diagnostics.Debug.WriteLine("Flick detected");
						}

						resetDrag();
						break;

					default:
						resetDrag();
						break;
				}

			}
			catch
			{
				//System.Diagnostics.Debug.WriteLine("No input");
			}
		}

	}
}
 