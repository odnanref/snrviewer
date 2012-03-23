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
using System.Net;

using SnmpSharpNet;

namespace SnrViewer
{		
	public class RemoteQuery
	{
		public String Community;
		
		public int Version = 1;
		
		public System.Net.IPAddress Ip;
		
		public int Port = 161;
				
		private AgentParameters param;
		
		public bool useCache = true;
		
		public bool overrideCache = false;
		
		public RemoteQuery ()
		{
			Community	= "public";
			Port		= 161;
			Ip			= IPAddress.Loopback;
			Version		= 1;
		}
		
		public RemoteQuery (string ipaddr, string community)
		{
			Community	= community;
			Port		= 161;
			Ip			= IPAddress.Parse(ipaddr);
			Version		= 1;
		}
		
		public Pdu basicInfo()
		{
			// Pdu class used for all requests
            Pdu pdu = new Pdu(PduType.Get);
            pdu.VbList.Add("1.3.6.1.2.1.1.1.0"); //sysDescr
            pdu.VbList.Add("1.3.6.1.2.1.1.2.0"); //sysObjectID
            pdu.VbList.Add("1.3.6.1.2.1.1.3.0"); //sysUpTime
            pdu.VbList.Add("1.3.6.1.2.1.1.4.0"); //sysContact
            pdu.VbList.Add("1.3.6.1.2.1.1.5.0"); //sysName
			
			return pdu;
		}
		
		public Dictionary<string, string> Query(String oid, string Type)
		{
			Dictionary<string, string> Res = new Dictionary<string, string>();
			// SNMP community name
            OctetString community = new OctetString(Community);
 
            // Define agent parameters class
            param = new AgentParameters(community);
            // Set SNMP version to 1 (or 2)
			if (Version == 1 ) {
            	param.Version = SnmpVersion.Ver1;
			}
            // Construct the agent address object
            // IpAddress class is easy to use here because
            //  it will try to resolve constructor parameter if it doesn't
            //  parse to an IP address
            IpAddress agent = new IpAddress(Ip.ToString());
 
            // Construct target
            UdpTarget target = new UdpTarget((IPAddress)agent, Port, 2000, 1);		
		
			switch (Type) {
				case "get":
					Res = getResult(target, basicInfo());
				break;
				
				case "walk":
					Res = walk(target, oid);
				break;
			}
						
            target.Close();
			return Res;
		}
		
		public Dictionary<string, string> getResult( UdpTarget target, Pdu pdu )
		{	
			Dictionary<string, string> Res = new Dictionary<string, string> ();
			// Make SNMP request
            SnmpV1Packet result = (SnmpV1Packet)target.Request(pdu, param);
						
			// If result is null then agent didn't reply or we couldn't parse the reply.
            if (result != null)
            {
                // ErrorStatus other then 0 is an error returned by 
                // the Agent - see SnmpConstants for error definitions
                if (result.Pdu.ErrorStatus != 0)
                {
                    // agent reported an error with the request
                    Console.WriteLine("Error in SNMP reply. Error {0} index {1}", 
                        result.Pdu.ErrorStatus,
                        result.Pdu.ErrorIndex);
                }
                else
                {
                    // Reply variables are returned in the same order as they were added
                    //  to the VbList
                    Console.WriteLine("sysDescr({0}) ({1}): {2}",
                        result.Pdu.VbList[0].Oid.ToString(), 
                        SnmpConstants.GetTypeName(result.Pdu.VbList[0].Value.Type),
                        result.Pdu.VbList[0].Value.ToString());
                    Console.WriteLine("sysObjectID({0}) ({1}): {2}",
                        result.Pdu.VbList[1].Oid.ToString(), 
                        SnmpConstants.GetTypeName(result.Pdu.VbList[1].Value.Type),
                        result.Pdu.VbList[1].Value.ToString());
                    Console.WriteLine("sysUpTime({0}) ({1}): {2}",
                        result.Pdu.VbList[2].Oid.ToString(), 
                        SnmpConstants.GetTypeName(result.Pdu.VbList[2].Value.Type),
                        result.Pdu.VbList[2].Value.ToString());
                    Console.WriteLine("sysContact({0}) ({1}): {2}",
                        result.Pdu.VbList[3].Oid.ToString(), 
                        SnmpConstants.GetTypeName(result.Pdu.VbList[3].Value.Type),
                        result.Pdu.VbList[3].Value.ToString());
                    Console.WriteLine("sysName({0}) ({1}): {2}",
                        result.Pdu.VbList[4].Oid.ToString(), 
                        SnmpConstants.GetTypeName(result.Pdu.VbList[4].Value.Type),
                        result.Pdu.VbList[4].Value.ToString());
                }
            }
            else
            {
                Console.WriteLine("No response received from SNMP agent.");
            }
			
			return Res; // FIXME not sure if going to need this method
		}
		
		public Dictionary<string, string> walk(UdpTarget target, string oid)
		{
			Dictionary<string, string> Output = new Dictionary<string, string>();
			// Define Oid that is the root of the MIB
            //  tree you wish to retrieve
            Oid rootOid = new Oid(oid); // ifDescr
 
            // This Oid represents last Oid returned by
            //  the SNMP agent
            Oid lastOid = (Oid)rootOid.Clone();
 
            // Pdu class used for all requests
            Pdu pdu = new Pdu(PduType.GetNext);
			
			if (useCache == true ) {
				Dictionary<string, string> data = getCache(target, oid);
				if (data.Count > 0 ) {
					return data;	
				}
			}
 
            // Loop through results
            while (lastOid != null)
            {
                // When Pdu class is first constructed, RequestId is set to a random value
                // that needs to be incremented on subsequent requests made using the
                // same instance of the Pdu class.
                if (pdu.RequestId != 0)
                {
                    pdu.RequestId += 1;
                }
                // Clear Oids from the Pdu class.
                pdu.VbList.Clear();
                // Initialize request PDU with the last retrieved Oid
                pdu.VbList.Add(lastOid);
                // Make SNMP request
                SnmpV1Packet result = (SnmpV1Packet)target.Request(pdu, param);
                // You should catch exceptions in the Request if using in real application.
 
                // If result is null then agent didn't reply or we couldn't parse the reply.
                if (result != null)
                {
                    // ErrorStatus other then 0 is an error returned by 
                    // the Agent - see SnmpConstants for error definitions
                    if (result.Pdu.ErrorStatus != 0)
                    {
                        // agent reported an error with the request
                        Console.WriteLine("Error in SNMP reply. Error {0} index {1}", 
                            result.Pdu.ErrorStatus,
                            result.Pdu.ErrorIndex);
                        lastOid = null;
                        break;
                    }
                    else
                    {
                        // Walk through returned variable bindings
                        foreach (Vb v in result.Pdu.VbList)
                        {
                            // Check that retrieved Oid is "child" of the root OID
                            if (rootOid.IsRootOf(v.Oid))
                            {
								/*
                                  Console.WriteLine("{0} ({1}): {2}",
                                    v.Oid.ToString(), 
                                    SnmpConstants.GetTypeName(v.Value.Type), 
                                    v.Value.ToString());
								 */
								// keep the result
								Output.Add(v.Oid.ToString(), v.Value.ToString());
								
                                lastOid = v.Oid;
                            }
                            else
                            {
                                // we have reached the end of the requested
                                // MIB tree. Set lastOid to null and exit loop
                                lastOid = null;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No response received from SNMP agent.");
                }
            }
			
			if (useCache == true || overrideCache == true ) {
				setCache(target, oid, Output);	
			}
			return Output;
		}
		
		public Dictionary<String, String> getCache(UdpTarget target, string oid)
		{
			RemoteQueryCache rqc = new RemoteQueryCache();
			Console.WriteLine("trying from cache");
			Dictionary<string,string> data = rqc.getCache(target.Address.ToString(), oid);
			Console.WriteLine("reading from cache " + data.Count );
			return data;
		}
		
		public void setCache(UdpTarget target, string oid, Dictionary<string, string> data)
		{
			RemoteQueryCache rqc = new RemoteQueryCache();
			Console.WriteLine("writting to cache");
			rqc.setCache(target.Address.ToString(), oid, data);
		}
	}
}