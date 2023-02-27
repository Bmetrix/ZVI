using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace Cv02
{

	/// <summary>
	/// Solution of the exercise 02 in ZVI
	/// </summary>
	class Program
	{

		/// <summary>
		/// Representation of the point in the image
		/// </summary>
		public struct Point
		{
			public int x, y;

			public Point(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}

		/// <summary>
		/// Find start point in the image
		/// </summary>
		/// <param name="img">Image where we will find start point</param>
		/// <returns>start point</returns>
		public static Point GetStart(Image<Bgr, Byte> img)
		{

			for (int y = 0; y < img.Height; y++)
				for (int x = 0; x < img.Width; x++)
				{

					if (img.ToBitmap().GetPixel(x, y).R == 0)
						return new Point(x, y);
				}

			return new Point(0, 0);
		}


		/// <summary>
		/// Get next point depending on the direction
		/// </summary>
		/// <param name="actual">actual point</param>
		/// <param name="direction">actual direction</param>
		/// <returns>next point</returns>
		public static Point GetNext(Point actual, int direction)
		{

			Point next = new Point(actual.x, actual.y);

			if (direction == 0)
			{
				next.x += 1;
			}
			else if (direction == 1)
			{
				next.y -= 1;
			}
			else if (direction == 2)
			{
				next.x -= 1;
			}
			else if (direction == 3)
			{
				next.y += 1;
			}

			return next;
		}

		/// <summary>
		/// Makes outside border of the image
		/// </summary>
		/// <param name="imageName">name of the image</param>
		public static void OutsideImageBorder(string imageName)
		{

			// read image
			Mat mat = CvInvoke.Imread(imageName, Emgu.CV.CvEnum.ImreadModes.Grayscale);
			Image<Bgr, Byte> img = mat.ToImage<Bgr, Byte>();

			// find starting point
			Point start = GetStart(img);

			// colection of the border points
			List<Point> borderPoints = new List<Point>();
			// defined starting direction o
			int smer = 3;
			borderPoints.Add(start);

			//img[start.y, start.x] = new Bgr(System.Drawing.Color.Red);

			int dt = (smer + 3) % 4;
			while (true)
			{

				Point next = GetNext(start, dt);

				// is end pixel
				if (next.x == borderPoints[0].x && next.y == borderPoints[0].y)
				{
					break;
				}

				// is border pixel
				if (img[next.y, next.x].R == 0)
				{
					borderPoints.Add(next);
				//	img[next.y, next.x] = new Bgr(System.Drawing.Color.Red);
					start = next;
					dt = (dt + 3) % 4;

				}
				else // next point is in the clockwise direction
					dt = (++dt) % 4;
			}

			ImageViewer viewer = new ImageViewer(); //create an image viewer

			viewer.Image = img; //draw the image obtained from camera
			viewer.ShowDialog(); //show the image viewer
		}

		/*public static void Clustering(string imageName)
		{

			// Load the image
			Image<Gray, byte> img = new Image<Gray, byte>(imageName);

			// Calculate the Hu Moments of the image
			MCvMoments moments = img.GetMoments(false);
			Matrix<float> huMoments = new Matrix<float>(new float[7, 1]);
			CvInvoke.HuMoments(moments, huMoments);

			// Reshape the Hu Moments matrix to a 1D array
			Matrix<float> huMomentsFlat = new Matrix<float>(huMoments.Rows * huMoments.Cols, 1);
			int index = 0;
			for (int i = 0; i < huMoments.Rows; i++)
			{
				for (int j = 0; j < huMoments.Cols; j++)
				{
					huMomentsFlat[index, 0] = huMoments[i, j];
					index++;
				}
			}

			// Convert the data type of the Hu Moments to float32
			Matrix<float> huMomentsFloat = new Matrix<float>(huMomentsFlat.Rows, huMomentsFlat.Cols);
			huMomentsFlat.CopyTo(huMomentsFloat);

			// Define the criteria for the k-means algorithm
			MCvTermCriteria criteria = new MCvTermCriteria(10, 0.1);

			// Specify the number of clusters (k)
			int k = 3;
			int attempts = 10;


			Emgu.CV.Util.VectorOfInt labels = null;


			// Apply the k-means algorithm using the KMeans class
			CvInvoke.Kmeans(huMomentsFloat, k, labels, criteria, attempts, Emgu.CV.CvEnum.KMeansInitType.RandomCenters);


			// Print the labels assigned to each cluster
			for (int i = 0; i < labels.Size; i++)
			{
				Console.WriteLine("Label {0}: {1}", i, labels[i]);
			}
		}*/

		static void Main(string[] args)
		{

			OutsideImageBorder("dvojka.png");
			//Clustering("dvojka.png");
		}
	}
}
