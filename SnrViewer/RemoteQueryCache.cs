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
using System.IO;

namespace SnrViewer
{
	public class RemoteQueryCache
	{
		private String cachePath = "./cache/";
		
		public RemoteQueryCache setPath(string path) 
		{
			cachePath = path;
			return this;
		}
		
		public string getPath()
		{
			return cachePath;
		}
		
		public RemoteQueryCache ()
		{
		}
		
		public Dictionary<String, String> getCache(String target, string oid)
		{
			String path = cachePath + target + "_" + oid + ".cache" ;
			if (!File.Exists(path)) {
				return new Dictionary<String, String>();
			}
			
			Dictionary<string,string> cached = new Dictionary<string,string>();
			
			StreamReader file = File.OpenText(path);
			string f = file.ReadToEnd();
			file.Close();
			
			if (!f.Contains("\n")) {
				throw new Exception("File " + path + " couldn't detect line separator ");	
			}
			
			string[] lines = f.Split("\n".ToCharArray());
			foreach (string line in lines ) {
				if (line.Contains(";")) {
					string[] cols = line.Split(";".ToCharArray());
					cached.Add(cols[0], cols[1]);
				}
			}
			
			return cached;
		}
		
		public void setCache(string target, string oid, Dictionary<string,string> data)
		{
			String path = cachePath + target + "_" + oid + ".cache" ;
			
			if (target == null || oid == null || data.Count <= 0 ) {
				throw new Exception("No valid data, oid, target passed");
			}
			
			StreamWriter wr = new StreamWriter(path);
			
			foreach (KeyValuePair<string, string> pair in data ) {
				wr.Write(pair.Key + ";" + pair.Value + "\n");
			}
			
			wr.Close();
		}
	}
}

