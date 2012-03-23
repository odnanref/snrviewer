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
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Gtk;
using Cairo;

public partial class MainWindow: Gtk.Window
{	
	Thread sweep;
	
	public delegate void MyCallBack(SnrViewer.NetworkEquipment Response);
	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		
		loadListEquipments();
		
		DrawRectangle();
	}
	
	~MainWindow()
	{
		if (sweep != null ) {
			sweep.Abort();
		}
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	
	private void DrawRectangle()
    {
		SnrViewer.NetworkEquipment ne = new SnrViewer.NetworkEquipment();
		ne.Ip = "192.168.0.33";
		ne.Community = "stvtelco";
		SnrViewer.SnmpSweep snmpsw = new SnrViewer.SnmpSweep(ne, new MyCallBack(this.OnResponse));
		sweep = new Thread(
			new ThreadStart(
					snmpsw.getInterfacesAsync
				)
			);
		sweep.Start();
	}
	
	void OnResponse(SnrViewer.NetworkEquipment Response)
	{
		SnrViewer.guiEquipment gE = new SnrViewer.guiEquipment();
		gE.setEquipment(Response);
		vbox2.Add(gE);
		vbox2.ShowAll();
	}
	
	protected void loadListEquipments()
	{
		Gtk.TreeViewColumn equipName = new Gtk.TreeViewColumn ();
		equipName.Title = "Name";
		Gtk.TreeViewColumn equipIP = new Gtk.TreeViewColumn ();
		equipIP.Title = "IP";
		
		Gtk.CellRendererText equipNameText = new Gtk.CellRendererText ();
		Gtk.CellRendererText equipIPText = new Gtk.CellRendererText ();
		
		equipName.PackStart(equipNameText, true);
		equipIP.PackStart(equipIPText, true);
		
		equipmentList.AppendColumn(equipName);
		equipmentList.AppendColumn(equipIP);
		
		equipName.AddAttribute (equipNameText, "text", 0);
		equipIP.AddAttribute (equipIPText, "text", 1);
		
		Gtk.ListStore lista = new Gtk.ListStore (typeof (string), typeof (string));
		
		lista.AppendValues ("CMTS 1", "exemplo");
		lista.AppendValues ("CMTS 2", "exemplos");
  
		equipmentList.Model = lista;
	}
	
	protected void btSair (object sender, System.EventArgs e)
	{
		Application.Quit();
	}
	
	protected void runQuery()
	{
		try {
		SnrViewer.NetworkEquipment ne = new SnrViewer.NetworkEquipment();
		ne.Ip = "192.168.0.33";
		ne.Community = "stvtelco";
		SnrViewer.SnmpSweep ss = new SnrViewer.SnmpSweep(ne);
		ne = ss.getInterfaces();
		if (ne.TotalInterfaces > 0 ) {
			foreach (KeyValuePair<string, ArrayList> pair in ne.InterfaceTypes) {				
				Console.WriteLine(">>" + pair.Key);
				if (pair.Value != null ) {
					foreach (SnrViewer.NetworkInterface i in pair.Value) {
						if (i != null ) 
							Console.WriteLine("\t " + i.Descr + " -- " + i.Id);	
					}
				}
			}
		} else 
			Console.WriteLine("No interfaces");
		}catch (Exception e ) {
			Console.WriteLine(e.ToString());	
		}
	}
		
	protected void btRunQuery (object sender, System.EventArgs e)
	{
		runQuery();
	}

	protected void newEquipmentAction (object sender, System.EventArgs e)
	{
		try {
			SnrViewer.WEquipment formEq = new SnrViewer.WEquipment();
			formEq.ShowAll();
		}catch(Exception ex ) {
			Console.WriteLine(ex.ToString());	
		}
	}

}
