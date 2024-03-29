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
using Microsoft.Xna.Framework;

namespace Tanks
{
	class GestureDetect
	{
		/*
		 * Ref: http://stackoverflow.com/a/15155929
		 * Class that turns iffy MonoGame touch library into more reliable "custom" touch detection
		 */

		private int dragIntervalThreshold = 6; //How many ticks to detect a drag
		private int updateIntervalMs = (1 / (60)) * 1000; //Update 60 fps

		private double lastUpdateTime = 0;
		private int totalDragUpdates = 0;

		private Vector2? lastDelta;
		private Vector2? lastPosition;

		private List<Vector2> lastGestures = new List<Vector2>();

		private bool isDragging = false;
		private bool firstTimeDrag = false;

		private void resetDrag(double time)
		{
			totalDragUpdates = 0;
			lastUpdateTime = time; //So drawings in quick succession aren't interpreted as flicks

			lastDelta = null;
			lastPosition = null;

			isDragging = false;
			firstTimeDrag = false;

			lastGestures = new List<Vector2>();

			//System.Diagnostics.Debug.WriteLine("Resetting variables");

		}

		public DetectedGesture getGesture(double time)
		{
			DetectedGesture detectedGs = new DetectedGesture();
			if (TouchPanel.IsGestureAvailable)
			{
				//If uncaught, Queue Empty exception when no input occurs
				GestureSample gs = TouchPanel.ReadGesture();

				detectedGs.Delta = gs.Delta;
				detectedGs.Delta2 = gs.Delta2;
				detectedGs.Position = gs.Position;
				detectedGs.Position2 = gs.Position2;
				detectedGs.Timestamp = gs.Timestamp;


				switch (gs.GestureType)
				{
					case GestureType.Tap:
						System.Diagnostics.Debug.WriteLine("Tap!");

						detectedGs.GestureType = GestureType.Tap;
						return detectedGs;

					case GestureType.Pinch:
						System.Diagnostics.Debug.WriteLine("Pinching..");

						detectedGs.GestureType = GestureType.Pinch;
						return detectedGs;

					case GestureType.PinchComplete:
						System.Diagnostics.Debug.WriteLine("Pinch complete!");

						detectedGs.GestureType = GestureType.PinchComplete;
						return detectedGs;

					case GestureType.FreeDrag:

						lastDelta = detectedGs.Delta;
						lastPosition = detectedGs.Position;

						//Test every updateIntervalMs to see if velocity warrants a flick, or a drag.
						if (lastUpdateTime + updateIntervalMs <= time)
						{
							if (isDragging == false)
							{
								firstTimeDrag = true;
							}
							isDragging = true;
							totalDragUpdates += 1;
							lastUpdateTime = time;
							System.Diagnostics.Debug.WriteLine("Incrementing counter to " + totalDragUpdates);

						}

						if (totalDragUpdates > dragIntervalThreshold) //User has spent more than 6 ticks dragging. Assume user means to Drag and draw accordingly.
						{
							detectedGs.GestureType = GestureType.FreeDrag;
							if (firstTimeDrag)
							{
								detectedGs.firstDetection = true;
								detectedGs.detectedGestureList = lastGestures;
								firstTimeDrag = false;
								System.Diagnostics.Debug.WriteLine("First time drag detected...");
							}
							return detectedGs;
						}

						break;

					case GestureType.DragComplete:
						System.Diagnostics.Debug.WriteLine("Drag complete");

						Vector2 lastDeltaNonNull = (Vector2)lastDelta;

						System.Diagnostics.Debug.WriteLine(lastDeltaNonNull.LengthSquared());

						if (totalDragUpdates > dragIntervalThreshold)
						{
							System.Diagnostics.Debug.WriteLine("Free drag detected.");

							detectedGs.GestureType = GestureType.DragComplete;
							resetDrag(time);
							return detectedGs; //No other gesture data available otherwise
						}
						else
						{
							System.Diagnostics.Debug.WriteLine("Flick detected");

							detectedGs.GestureType = GestureType.Flick; //No other gesture data available otherwise
							if (lastDelta != null && lastPosition != null)
							{
								detectedGs.Delta = Vector2.Subtract(lastGestures.Last(), lastGestures.First());
								detectedGs.Position = lastGestures.First(); 
							}

							resetDrag(time);
							return detectedGs;
						}

					default:
						break;
				}

				lastGestures.Add(detectedGs.Position);

				detectedGs.GestureType = GestureType.None;
				return detectedGs;
			}
			else
			{
				detectedGs.GestureType = GestureType.None;
				return detectedGs;
			}
		}
	}
}