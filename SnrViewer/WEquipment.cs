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
using Gtk;


namespace SnrViewer
{
	
	public partial class WEquipment : Gtk.Window
	{
				
		public WEquipment () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
			loadVersion();
		}
		
		public void loadVersion()
		{
			cmbVersion.Clear();
	        CellRendererText cell = new CellRendererText();
	        cmbVersion.PackStart(cell, false);
	        cmbVersion.AddAttribute(cell, "text", 0);
	        
			ListStore store = new ListStore(typeof (string));
			
	        cmbVersion.Model = store;
	 
	        store.AppendValues ("1");
	        store.AppendValues ("2");
	        store.AppendValues ("3"); 
		}
		
		public void loadType()
		{
			NetworkEquipmentType net = new NetworkEquipmentType();
			net.getAll();
		}
		
		protected void btSair (object sender, System.EventArgs e)
		{
			this.Destroy();// fixme
		}
	}
}

