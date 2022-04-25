using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using TUIO;

	public class TUIOTEST : Form , TuioListener
	{
	class actor
	{
		public int x, y;
		public Bitmap im;
		public List<Bitmap> imgs = new List<Bitmap>();
		public int frame;
        internal object o;
    }

	private TuioClient client;

		private Dictionary<long,TuioObject> objectList;
		private Dictionary<long,TuioCursor> cursorList;
		private Dictionary<long,TuioBlob> blobList;

		public static int width, height;
		private int window_width =  640;
		private int window_height = 480;
		private int window_left = 0;
		private int window_top = 0;
		private int screen_width = Screen.PrimaryScreen.Bounds.Width;
		private int screen_height = Screen.PrimaryScreen.Bounds.Height;
		int sorax,soray,sora2x,sora2y,sora3x,sora3y,sora4x,sora4y,sora5x,sora5y = 0;
		int sora1x, sora1y, sora22x, sora22y, sora33x, sora33y, sora44x, sora44y, sora55x, sora55y = 0;
		int g = 0;
		actor ball;
		Bitmap sora = new Bitmap("C:\\Users\\Moeme\\source\\repos\\TUIO11_NET1\\bin\\Debug\\one.bmp");
		Bitmap sora1 = new Bitmap("C:\\Users\\Moeme\\source\\repos\\TUIO11_NET1\\bin\\Debug\\1.bmp");
		Bitmap sora2 = new Bitmap("C:\\Users\\Moeme\\source\\repos\\TUIO11_NET1\\bin\\Debug\\two.bmp");
		Bitmap sora22 = new Bitmap("C:\\Users\\Moeme\\source\\repos\\TUIO11_NET1\\bin\\Debug\\2.bmp");
		Bitmap sora3 = new Bitmap("C:\\Users\\Moeme\\source\\repos\\TUIO11_NET1\\bin\\Debug\\3.bmp");
		Bitmap sora33 = new Bitmap("C:\\Users\\Moeme\\source\\repos\\TUIO11_NET1\\bin\\Debug\\three.bmp");
		Bitmap sora4 = new Bitmap("C:\\Users\\Moeme\\source\\repos\\TUIO11_NET1\\bin\\Debug\\four.bmp");
		Bitmap sora44 = new Bitmap("C:\\Users\\Moeme\\source\\repos\\TUIO11_NET1\\bin\\Debug\\4.bmp");
		Bitmap sora5 = new Bitmap("C:\\Users\\Moeme\\source\\repos\\TUIO11_NET1\\bin\\Debug\\five.bmp");
		Bitmap sora55 = new Bitmap("C:\\Users\\Moeme\\source\\repos\\TUIO11_NET1\\bin\\Debug\\5.bmp");
		Bitmap sora10 = new Bitmap("C:\\Users\\Moeme\\source\\repos\\TUIO11_NET1\\bin\\Debug\\star.bmp");

	List<actor> Lballs = new List<actor>();
		
	private bool fullscreen;
		private bool verbose;

		Font font = new Font("Arial", 100);
		SolidBrush fntBrush = new SolidBrush(Color.Green);
		SolidBrush bgrBrush = new SolidBrush(Color.Black);
		SolidBrush curBrush = new SolidBrush(Color.FromArgb(192, 0, 192));
		SolidBrush objBrush = new SolidBrush(Color.Black);
		SolidBrush blbBrush = new SolidBrush(Color.FromArgb(64, 64, 64));
		Pen curPen = new Pen(new SolidBrush(Color.Black), 1);

		public TUIOTEST(int port) {
		
			verbose = false;
			fullscreen = false;
			width = window_width;
			height = window_height;

			this.ClientSize = new System.Drawing.Size(width, height);
			this.Name = "TuioTest";
			this.Text = "TuioTest";
			
			this.Closing+=new CancelEventHandler(Form_Closing);
			this.KeyDown+=new KeyEventHandler(Form_KeyDown);

			this.SetStyle( ControlStyles.AllPaintingInWmPaint |
							ControlStyles.UserPaint |
							ControlStyles.DoubleBuffer, true);

			objectList = new Dictionary<long,TuioObject>(128);
			cursorList = new Dictionary<long,TuioCursor>(128);
			blobList   = new Dictionary<long,TuioBlob>(128);
			
			client = new TuioClient(port);
			client.addTuioListener(this);

			client.connect();
		}

		private void Form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {

 			if ( e.KeyData == Keys.F1) {
	 			if (fullscreen == false) {

					width = screen_width;
					height = screen_height;

					window_left = this.Left;
					window_top = this.Top;

					this.FormBorderStyle = FormBorderStyle.None;
		 			this.Left = 0;
		 			this.Top = 0;
		 			this.Width = screen_width;
		 			this.Height = screen_height;

		 			fullscreen = true;
	 			} else {

					width = window_width;
					height = window_height;

		 			this.FormBorderStyle = FormBorderStyle.Sizable;
		 			this.Left = window_left;
		 			this.Top = window_top;
		 			this.Width = window_width;
		 			this.Height = window_height;

		 			fullscreen = false;
	 			}
 			} else if ( e.KeyData == Keys.Escape) {
				this.Close();

 			} else if ( e.KeyData == Keys.V ) {
 				verbose=!verbose;
 			}

 		}

		private void Form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			client.removeTuioListener(this);

			client.disconnect();
			System.Environment.Exit(0);
		}

		public void addTuioObject(TuioObject o) {
			lock(objectList) {
				objectList.Add(o.SessionID,o);
			} if (verbose) Console.WriteLine("add obj "+o.SymbolID+" ("+o.SessionID+") "+o.X+" "+o.Y+" "+o.Angle);
		}

		public void updateTuioObject(TuioObject o) {

			if (verbose) Console.WriteLine("set obj "+o.SymbolID+" "+o.SessionID+" "+o.X+" "+o.Y+" "+o.Angle+" "+o.MotionSpeed+" "+o.RotationSpeed+" "+o.MotionAccel+" "+o.RotationAccel);
		}

		public void removeTuioObject(TuioObject o) {
			lock(objectList) {
				objectList.Remove(o.SessionID);
			}
			if (verbose) Console.WriteLine("del obj "+o.SymbolID+" ("+o.SessionID+")");
		}

		public void addTuioCursor(TuioCursor c) {
			lock(cursorList) {
				cursorList.Add(c.SessionID,c);
			}
			if (verbose) Console.WriteLine("add cur "+c.CursorID + " ("+c.SessionID+") "+c.X+" "+c.Y);
		}

		public void updateTuioCursor(TuioCursor c) {
			if (verbose) Console.WriteLine("set cur "+c.CursorID + " ("+c.SessionID+") "+c.X+" "+c.Y+" "+c.MotionSpeed+" "+c.MotionAccel);
		}

		public void removeTuioCursor(TuioCursor c) {
			lock(cursorList) {
				cursorList.Remove(c.SessionID);
			}
			if (verbose) Console.WriteLine("del cur "+c.CursorID + " ("+c.SessionID+")");
 		}

		public void addTuioBlob(TuioBlob b) {
			lock(blobList) {
				blobList.Add(b.SessionID,b);
			}
			if (verbose) Console.WriteLine("add blb "+b.BlobID + " ("+b.SessionID+") "+b.X+" "+b.Y+" "+b.Angle+" "+b.Width+" "+b.Height+" "+b.Area);
		}

		public void updateTuioBlob(TuioBlob b) {
		
			if (verbose) Console.WriteLine("set blb "+b.BlobID + " ("+b.SessionID+") "+b.X+" "+b.Y+" "+b.Angle+" "+b.Width+" "+b.Height+" "+b.Area+" "+b.MotionSpeed+" "+b.RotationSpeed+" "+b.MotionAccel+" "+b.RotationAccel);
		}

		public void removeTuioBlob(TuioBlob b) {
			lock(blobList) {
				blobList.Remove(b.SessionID);
			}
			if (verbose) Console.WriteLine("del blb "+b.BlobID + " ("+b.SessionID+")");
		}

		public void refresh(TuioTime frameTime) {
			Invalidate();
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		// Getting the graphics object
		Graphics g = pevent.Graphics;
			g.FillRectangle(bgrBrush, new Rectangle(0,0,width,height));
		//	g.DrawImage(Lballs[0].im, Lballs[0], Lballs[0].o.Y);

		//int ox = tobj.getScreenX(width);
		//int oy = tobj.getScreenY(height);
		//int x = ox;
		//int y = oy;

		//g.DrawImage(Lballs[i].im, Lballs[i].x, Lballs[i].y);

		// draw the cursor path
		if (cursorList.Count > 0) {
 			 lock(cursorList) {
			 foreach (TuioCursor tcur in cursorList.Values) {
					List<TuioPoint> path = tcur.Path;
					TuioPoint current_point = path[0];

					for (int i = 0; i < path.Count; i++) {
						TuioPoint next_point = path[i];
						g.DrawLine(curPen, current_point.getScreenX(width), current_point.getScreenY(height), next_point.getScreenX(width), next_point.getScreenY(height));
						current_point = next_point;
					}
					g.FillEllipse(curBrush, current_point.getScreenX(width) - height / 100, current_point.getScreenY(height) - height / 100, height / 50, height / 50);
				//	g.DrawString(tcur.CursorID + "", font, fntBrush, new PointF(tcur.getScreenX(width) - 10, tcur.getScreenY(height) - 10));
					
				}
			}
		 }

			// draw the objects
			if (objectList.Count > 0) 
			{
				lock (objectList)
				{
					foreach (TuioObject tobj in objectList.Values)
					{
						int ox = tobj.getScreenX(width);
						int oy = tobj.getScreenY(height);
						//int x = ox;
						//int y = oy;
						int size = height / 40;
						if (tobj.SymbolID == 1)
					{
						g.TranslateTransform(ox, oy);
						g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-ox, -oy);

						g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));
						sora = new Bitmap(sora, new Size(100, 100));
						sora.MakeTransparent(sora.GetPixel(0, 0));
						g.DrawImage(sora, ox - size / 2, oy - size / 2);
						sorax = ox;
						soray = oy;
						g.TranslateTransform(ox, oy);
						g.RotateTransform(-1 * (float)(tobj.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-ox, -oy);

						//g.DrawString(tobj.SymbolID + "", font, fntBrush, new PointF(ox - 10, oy - 10));
					}
						else if (tobj.SymbolID == 2)
					{
						g.TranslateTransform(ox, oy);
						g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-ox, -oy);

						g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));
						sora1 = new Bitmap(sora1, new Size(50, 50));
						sora1.MakeTransparent(sora1.GetPixel(0, 0));
						sora1x = ox;
						sora1y = oy;
						if (sorax > sora1x - sora1.Width * 2 && sorax < sora1x + sora1.Width * 2)
						{
							if (soray > sora1y - sora1.Height * 2 && soray < sora1y + sora1.Height * 2)
							{
								g.DrawString("True" + "", font, fntBrush, new PointF(ox - 10, oy - 10));
								//MessageBox.Show("t");

							}
						}
						g.DrawImage(sora1, ox - size / 2, oy - size / 2);

						g.TranslateTransform(ox, oy);
						g.RotateTransform(-1 * (float)(tobj.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-ox, -oy);

						//g.DrawString(tobj.SymbolID + "", font, fntBrush, new PointF(ox - 10, oy - 10));
					}
						else if(tobj.SymbolID==3)
					    {
						g.TranslateTransform(ox, oy);
						g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-ox, -oy);

						g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));
						sora2 = new Bitmap(sora2, new Size(150, 150));
						sora2.MakeTransparent(sora2.GetPixel(0, 0));
						sora2x = ox;
						sora2y = oy;
						g.DrawImage(sora2, ox - size / 2, oy - size / 2);
						g.TranslateTransform(ox, oy);
						g.RotateTransform(-1 * (float)(tobj.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-ox, -oy);
						}
						else if(tobj.SymbolID==4)
				        {
						g.TranslateTransform(ox, oy);
						g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-ox, -oy);

						g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));
						sora22 = new Bitmap(sora22, new Size(150, 150));
						sora22.MakeTransparent(sora22.GetPixel(0, 0));
						sora22x = ox;
						sora22y = oy;
						if (sora2x > sora22x - sora2.Width && sora2x < sora22x + sora22.Width)
						{
							if (sora2y > sora22y - sora22.Height && sora2y < sora22y + sora22.Height)
							{
								g.DrawString("True" + "", font, fntBrush, new PointF(ox - 10, oy - 10));
								//MessageBox.Show("t");

							}
						}
						g.DrawImage(sora22, ox - size / 2, oy - size / 2);
						g.TranslateTransform(ox, oy);
						g.RotateTransform(-1 * (float)(tobj.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-ox, -oy);
					}
						else if (tobj.SymbolID == 5)
						{
							g.TranslateTransform(ox, oy);
							g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
							g.TranslateTransform(-ox, -oy);

							g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));
							sora3 = new Bitmap(sora3, new Size(150, 150));
							sora3.MakeTransparent(sora3.GetPixel(0, 0));

							sora3x = ox;
							sora3y = oy;
							g.DrawImage(sora3, ox - size / 2, oy - size / 2);
							g.TranslateTransform(ox, oy);
							g.RotateTransform(-1 * (float)(tobj.Angle / Math.PI * 180.0f));
							g.TranslateTransform(-ox, -oy);
						}
						else if (tobj.SymbolID == 6)
						{
							g.TranslateTransform(ox, oy);
							g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
							g.TranslateTransform(-ox, -oy);

							g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));
							sora33 = new Bitmap(sora33, new Size(150, 150));
							sora33.MakeTransparent(sora33.GetPixel(0, 0));

							sora33x = ox;
							sora33y = oy;
							if (sora3x > sora33x - sora3.Width && sora3x < sora33x + sora33.Width)
							{
								if (sora3y > sora33y - sora33.Height && sora3y < sora33y + sora33.Height)
								{
									g.DrawString("True" + "", font, fntBrush, new PointF(ox - 10, oy - 10));
									//MessageBox.Show("t");

								}
							}
							g.DrawImage(sora33, ox - size / 2, oy - size / 2);
							g.TranslateTransform(ox, oy);
							g.RotateTransform(-1 * (float)(tobj.Angle / Math.PI * 180.0f));
							g.TranslateTransform(-ox, -oy);
						}
						else if (tobj.SymbolID == 7)
						{
							g.TranslateTransform(ox, oy);
							g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
							g.TranslateTransform(-ox, -oy);

							g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));
							sora4 = new Bitmap(sora4, new Size(150, 150));
							sora4.MakeTransparent(sora4.GetPixel(0, 0));

							sora4x = ox;
							sora4y = oy;
							g.DrawImage(sora4, ox - size / 2, oy - size / 2);
							g.TranslateTransform(ox, oy);
							g.RotateTransform(-1 * (float)(tobj.Angle / Math.PI * 180.0f));
							g.TranslateTransform(-ox, -oy);
						}
						else if (tobj.SymbolID == 8)
						{
							g.TranslateTransform(ox, oy);
							g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
							g.TranslateTransform(-ox, -oy);

							g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));
							sora44 = new Bitmap(sora44, new Size(150, 150));
							sora44.MakeTransparent(sora44.GetPixel(0, 0));

							sora44x = ox;
							sora44y = oy;
							if (sora4x > sora44x - sora4.Width && sora4x < sora44x + sora44.Width)
							{
								if (sora4y > sora44y - sora44.Height && sora4y < sora44y + sora44.Height)
								{
									g.DrawString("True" + "", font, fntBrush, new PointF(ox - 10, oy - 10));
									//MessageBox.Show("t");

								}
							}
							g.DrawImage(sora44, ox - size / 2, oy - size / 2);
							g.TranslateTransform(ox, oy);
							g.RotateTransform(-1 * (float)(tobj.Angle / Math.PI * 180.0f));
							g.TranslateTransform(-ox, -oy);
						}
						else if (tobj.SymbolID == 9)
						{
							g.TranslateTransform(ox, oy);
							g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
							g.TranslateTransform(-ox, -oy);

							g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));
							sora5 = new Bitmap(sora5, new Size(150, 150));
							sora5.MakeTransparent(sora5.GetPixel(0, 0));

							sora5x = ox;
							sora5y = oy;
							g.DrawImage(sora5, ox - size / 2, oy - size / 2);
							g.TranslateTransform(ox, oy);
							g.RotateTransform(-1 * (float)(tobj.Angle / Math.PI * 180.0f));
							g.TranslateTransform(-ox, -oy);
						}
						else if (tobj.SymbolID == 10)
						{
							g.TranslateTransform(ox, oy);
							g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
							g.TranslateTransform(-ox, -oy);

							g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));
							sora55 = new Bitmap(sora55, new Size(100, 100));
							sora55.MakeTransparent(sora55.GetPixel(0, 0));

							sora55x = ox;
							sora55y = oy;
							if (sora5x > sora55x - sora5.Width && sora5x < sora55x + sora55.Width)
							{
								if (sora5y > sora55y - sora55.Height && sora5y < sora55y + sora55.Height)
								{
									g.DrawString("True" + "", font, fntBrush, new PointF(ox - 10, oy - 10));
									//MessageBox.Show("t");

								}
							}
							g.DrawImage(sora55, ox - size / 2, oy - size / 2);
							g.TranslateTransform(ox, oy);
							g.RotateTransform(-1 * (float)(tobj.Angle / Math.PI * 180.0f));
							g.TranslateTransform(-ox, -oy);
						}
					    else
					    {
							g.TranslateTransform(ox, oy);
							g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
							g.TranslateTransform(-ox, -oy);

							g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));
							sora10 = new Bitmap(sora10, new Size(100, 100));
							sora10.MakeTransparent(sora10.GetPixel(0, 0));
							g.DrawImage(sora10, ox - size / 2, oy - size / 2);
							g.TranslateTransform(ox, oy);
							g.RotateTransform(-1 * (float)(tobj.Angle / Math.PI * 180.0f));
							g.TranslateTransform(-ox, -oy);
						}

					}
				}
			}
			// draw the blobs
			if (blobList.Count > 0) {
				lock(blobList) {
					foreach (TuioBlob tblb in blobList.Values) {
						int bx = tblb.getScreenX(width);
						int by = tblb.getScreenY(height);
						float bw = tblb.Width*width;
						float bh = tblb.Height*height;

						g.TranslateTransform(bx, by);
						g.RotateTransform((float)(tblb.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-bx, -by);

						g.FillEllipse(blbBrush, bx - bw / 2, by - bh / 2, bw, bh);

						g.TranslateTransform(bx, by);
						g.RotateTransform(-1 * (float)(tblb.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-bx, -by);
						
						//g.DrawString(tblb.BlobID + "", font, fntBrush, new PointF(bx, by));
					}
				}
			}
		}
    private void InitializeComponent()
    {
            this.SuspendLayout();
            // 
            // TUIOTEST
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "TUIOTEST";
            this.Load += new System.EventHandler(this.TUIOTEST_Load);
            this.ResumeLayout(false);

    }

    private void TUIOTEST_Load(object sender, EventArgs e)
    {
		
		ball = new actor();
		//ball.x = x;
		//ball.y = this.Height - y;
		ball.im = new Bitmap("ball2.bmp");
		ball.im.MakeTransparent(ball.im.GetPixel(0, 0));
		Lballs.Add(ball);
}

    public static void Main(String[] argv) {
	 		int port = 0;
			switch (argv.Length) {
				case 1:
					port = int.Parse(argv[0],null);
					if(port==0) goto default;
					break;
				case 0:
					port = 3333;
					break;
				default:
					Console.WriteLine("usage: mono TuioDemo [port]");
					System.Environment.Exit(0);
					break;
			}
			
			TUIOTEST app = new TUIOTEST(port);
			Application.Run(app);
		}
	}
