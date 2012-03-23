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
using System.Collections;
using System.Collections.Generic;

namespace SnrViewer
{		
	public class NetworkInterface 
	{
		public String Id;
		public String Descr;
		public string Type;
		public string SubType;
		// speed , and other type of specific details 
		// about a interface
		public Dictionary<String, double> Details;
	}
	
	public class NetworkEquipment : NetworkEquipmentType
	{ 
		public String Contact;
						
		public String Name;
		
		public int TotalInterfaces {
			get { 
				return (
					InterfaceTypes["upstream"].Count + 
					InterfaceTypes["downstream"].Count +
					InterfaceTypes["ethernet"].Count
					); 
			}	
		}
		
		public String Ip;
		
		public String Community = "public";
						
		public Dictionary<string, ArrayList> InterfaceTypes = 
			new Dictionary<string, ArrayList>()
							{
							    {"upstream", new ArrayList()},
							    {"downstream", new ArrayList()},
							    {"ethernet", new ArrayList()},
							    {"gethernet", new ArrayList()}
							};
		
		public NetworkEquipment addInterface(NetworkInterface ni)
		{
			if ( ni == null || ni.Type == null ) {
				return this;
			}

			InterfaceTypes[ni.Type].Add(ni);
			
			return this;
		}
		
		public NetworkEquipment addInterface(string oid, string name)
		{
			String[] tmp = oid.Split('.');
			NetworkInterface ni = new NetworkInterface();
			ni.Id				= tmp[(tmp.Length-1)];
			ni.Descr			= name;

			// determine type
			if (name.Contains("Upstream") || name.Contains("US ")) {
				ni.Type = "upstream";
				if (name.Contains("Channel") || name.Contains(" CH ")) {
					ni.SubType = "channel";
				}
				
				if (name.Contains("Interface")) {
					ni.SubType = "interface";
				}
				
			} else if (name.Contains("Ethernet") || name.Contains("ETH ")) {
				// ethernet interface
				ni.Type = "ethernet";
				if (name.Contains("Fast")) {
					ni.SubType = "fastethernet";
				}
			} else if (name.Contains("Downstream") || name.Contains("DS ")) {
				ni.Type = "downstream";
			} else {
				return this;// if we don't know what it is, we don't care	
			}
			
			return addInterface(ni);
		}
		
		public String getInterfaceType(int oid)
		{
			return "a";
		}
		
		public void loadInterfaceDetails()
		{
			
		}
		
		public NetworkEquipment ()
		{
		
		}
		
		public NetworkEquipment(string Type)
		{
			this.Type = Type;
		}
		
		public int getTotalInterfaceByType(String Type)
		{
			if (this.InterfaceTypes.ContainsKey(Type)) {
				return this.InterfaceTypes[Type].Count;
			} else {
				return 0;
			}
		}
		
		public int getTotalInterfaceByType(String Type, String SubType)
		{
			if (this.InterfaceTypes.ContainsKey(Type)) {
				int x=0;
				foreach (SnrViewer.NetworkInterface i in this.InterfaceTypes[Type]) {
					if (i != null && ( i.SubType.Contains(SubType) )) {
						x++;
						//Console.WriteLine("\t " + i.Descr + " -- " + i.Id);
					}
				}
				
				return x;
				
			} else {
				return 0;
			}
		
			
		}
		
		public List<NetworkInterface> getInterfacesByType(String Type, String SubType)
		{
			List<NetworkInterface> Interfaces = new List<NetworkInterface>();
			if (this.InterfaceTypes.ContainsKey(Type)) {
				foreach (SnrViewer.NetworkInterface i in this.InterfaceTypes[Type]) {
					if (i != null && ( i.SubType.Contains(SubType) )) {
						Interfaces.Add(i);						
					}
				}
			}
			
			return Interfaces;
		}
		
		public NetworkEquipment setType(string type)
		{
			Type = type;
			
			return this;
		}
		
	}
}
  