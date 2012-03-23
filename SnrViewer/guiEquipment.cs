//  
//  Author:
//       Fernando Andre <netriver on gmail dot com>
// 
//  Copyright (c) 2012 Fernando Ribeiro
// 
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//  
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using Cairo;

namespace SnrViewer
{
	public class SquareInt
	{
		public int X;
		public int Y;
		
		private NetworkInterface iface;
		
		public SquareInt(){}
		
		public SquareInt(int x, int y, NetworkInterface ni ) 
		{
			X = x;
			Y = y;
			iface = ni;
		}
		
		public NetworkInterface getInterface()
		{
			return iface;
		}
	}
	
	public class guiEquipment : DrawingArea
	{				
		private NetworkEquipment ne;
		
		private DrawingArea area;
		
		private Cairo.Context cr;
				
		private double pX = 0;
		
		private double pY = 0;
		
		private int width = 180;
		
		private int height = 100;
		
		private int boxsize = 8;
		
		private int spacing = (4 + 12);
		
		private List<SquareInt> boxCoordinates = new List<SquareInt>();
		
		private Cairo.Point Offset;
		
		private double Zoom = 1;
		
		public Cairo.Point WorldToScreen( Cairo.Point world )
		{
    		var p = new Cairo.Point();
    		p.X = Convert.ToInt32((world.X - Offset.X) * Zoom);
			
    		p.Y = Convert.ToInt32((world.Y - Offset.Y) * Zoom);
    		return p;
		}
		
		void preLoad()
		{
			Offset.X = width/2;
			Offset.Y = height/2;
			this.AddEvents ((int) EventMask.ButtonPressMask);
			
        	this.ButtonPressEvent += delegate(object o, ButtonPressEventArgs args) {
				//Console.WriteLine(">> " + args.Event.X + " args.Event.Y " + args.Event.Y);
				Cairo.Point world = new Cairo.Point(Convert.ToInt32(args.Event.X), Convert.ToInt32(args.Event.Y));
				hasInterface(world);
        	};
			
			this.ExposeEvent += OnExpose;
						
		}
		
		public guiEquipment ()
		{
			preLoad();
			pX=pY=0;
		}
		
		public guiEquipment (double pX, double pY)
		{
			preLoad();
			this.pX = pX;
			this.pY = pY;
		}
		
		public guiEquipment setEquipment(NetworkEquipment ne)
		{
			this.ne = ne;
			return this;
		}
		
		public Boolean hasInterface(double X, double Y)
		{
			double CordsX = 0;
			double CordsY = 0;
			double sqLimitX		= 0;
			double sqLimitY		= 0;
			
			for (int i=0; i < boxCoordinates.Count; i++)  {
				//Console.WriteLine(" > f {0} n > " + boxCoordinates[i].GetValue(0,0), i );
					sqLimitX = (double)boxCoordinates[i].X +boxsize;
					sqLimitY = (double)boxCoordinates[i].Y+boxsize;
					CordsX = (double)boxCoordinates[i].X;
					CordsY = (double)boxCoordinates[i].Y;
					// if mouse (0 is less) gt or equal to coordsx and mouse Y greater 
            		if ( X >= CordsX && X <= sqLimitX && Y >= CordsY && Y <= sqLimitY
					) {
						Console.WriteLine("FOUND x = {0} ,  x = {1} {2} ", boxCoordinates[i].X, X, sqLimitX);
						Console.WriteLine("FOUND Y = {0} ,  Y = {1} {2} ", boxCoordinates[i].Y, Y, sqLimitY);
						Console.WriteLine(">> " + boxCoordinates[i].getInterface().Descr );
					
					} else {
						//Console.WriteLine("READ x = {0} ,  x = {1} {2} ", boxCoordinates[i].X, X, sqLimitX);
						//Console.WriteLine("READ Y = {0} ,  Y = {1} {2} ", boxCoordinates[i].Y, Y, sqLimitY);
					}
			}
			
			return false;
		}
		
		public Boolean hasInterface(Cairo.Point world)
		{
			Cairo.Point screen = WorldToScreen(world);
			return hasInterface(screen.X, screen.Y);	
		}
		/**
		 * for testing only
		 * 
		 */
		private void recordPosition(int x, int y)
		{
			SquareInt sq = new SquareInt();
			sq.X = x;
			sq.Y = y;
			boxCoordinates.Add( sq ); // store coords
		}
		
		private void recordPosition(int x, int y, NetworkInterface ni)
		{
			boxCoordinates.Add(new SquareInt(x, y, ni)); // store coords
		}
		
		void drawInterfaceEth()
		{
			int total	= ne.getTotalInterfaceByType("ethernet");
			int tmp=0;
			
			cr.LineWidth= 1;
			
	        cr.SetSourceRGB(0.7, 0.2, 0.0);
			for (int i=0; i < total; i++) {
				if ( i > 0 && i < 2 ) {
					tmp = spacing;
				}
				
				cr.Rectangle((pX + (i * tmp)) , pY, boxsize, boxsize);				
				recordPosition(Convert.ToInt32((pX + (i * tmp))), Convert.ToInt32(pY));
			}
			
			pY = 10;
			cr.StrokePreserve();
		}
		
		void drawInterfaceUps()
		{
			int totalDown	= ne.getTotalInterfaceByType("downstream");
			//int totalUp		= ne.getTotalInterfaceByType("upstream", "interface");
			double tmp	= 0;
			
			if (pY < 10 ) 
				pY = 10;
			
			double x = 0;

			cr.LineWidth= 1;
	        cr.SetSourceRGB(0, 0.8, 0.0);
			
			for (int z=0; z < totalDown; z++ ) {
				if ( z > 0 && z < 2 ) {
					tmp = spacing;
				}
				
				if ( (z%4) == 0 ) {
					pY	= (pY + spacing);
					x	= 0;
				}
				
				cr.Rectangle(pX + (tmp*x) , pY, boxsize, boxsize );
				Console.WriteLine(" :D {0} {1} {2} " , pX + (tmp*x), (pX + (tmp*x))*2, (pY*2));
				recordPosition(Convert.ToInt32(pX + (tmp*x)) , Convert.ToInt32(pY)); // store coords
				
				x++;
			}
			
			x=0; //Reset X
			tmp = 0;//Reset tmp

			List<NetworkInterface> Interfaces = ne.getInterfacesByType("upstream", "interface");
			
			for (int i=0; i < Interfaces.Count; i++) {

				if ( i > 0 && i < 2 ) {
					tmp = spacing;
				}
				
				if ( (i%8) == 0 ) {
					pY	= (pY + spacing);
					x	= 0;
				}
								
				cr.Rectangle(pX + (tmp*x) , pY, boxsize, boxsize );
				// store coords
				recordPosition(Convert.ToInt32((pX + (tmp*x))) , Convert.ToInt32(pY), Interfaces[i]);
				x++;
			}
			cr.SetSourceRGB(100,100,100);
			cr.Fill();
			cr.StrokePreserve();
		}
		
		void OnExpose(object sender, ExposeEventArgs args)
    	{
	       	area = (DrawingArea) sender;
			cr =  Gdk.CairoHelper.Create(area.GdkWindow);
			
	        cr.LineWidth = 2;
	        cr.SetSourceRGB(0.7, 0.2, 0.0);
			
	        cr.Translate(width/2, height/2);
			cr.Rectangle(pX , pY, width, height);
	        //cr.Arc(pX, pY, (width < height ? width : height) / 2 - 10, 0, 2*Math.PI);
	        cr.StrokePreserve();
			cr.SetSourceRGB(0.3, 0.4, 0.6);
	        cr.Fill();
			
			pX=pY=4;
			
			//drawInterfaceEth();
	        
	        drawInterfaceUps();
	         
	        ((IDisposable) cr.Target).Dispose();                                      
	        ((IDisposable) cr).Dispose();
    	}

	}
}

