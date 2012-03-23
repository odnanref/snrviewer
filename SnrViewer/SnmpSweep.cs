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
using System.Net;
using System.Collections.Generic;

namespace SnrViewer
{
	public class SnmpSweep
	{		
		public NetworkEquipment ne;
		
		protected MainWindow.MyCallBack callback = null;
		
		public SnmpSweep ()
		{
			
		}
		
		public SnmpSweep (NetworkEquipment ne)
		{
			this.ne = ne;
		}
		
		public SnmpSweep (NetworkEquipment ne, MainWindow.MyCallBack Callback)
		{
			this.ne = ne;
			this.callback = Callback;
		}
		
		public void getInterfacesAsync() 
		{
			ne = getInterfaces();
			//NetworkEquipment ne = new NetworkEquipment();// FIXME: Testing
			if (this.callback != null)
					// Invoke the call back delegate
					this.callback(ne);
		}
		
		public NetworkEquipment getInterfaces() 
		{
			if ( ne.TotalInterfaces > 0 ) {
				return ne;
			}
			
			RemoteQuery rq = new RemoteQuery();
			rq.Ip		 = IPAddress.Parse(ne.Ip);
			rq.Community = ne.Community;
			Dictionary<string, string> Interf = rq.Query("1.3.6.1.2.1.2.2.1.2", "walk"); // interfaces list + description
			// Loop over pairs with foreach
			foreach (KeyValuePair<string, string> pair in Interf)
			{
			    /*
			    Console.WriteLine("Adding {0}, {1}",
				pair.Key,
				pair.Value);
				*/
				ne.addInterface(pair.Key, pair.Value);
			}
						
			return ne;
		}
	}
}

